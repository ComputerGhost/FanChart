Imports MySql.Data.MySqlClient

Public Class EngineProcess

    Event RecoverableExceptionThrown(moduleName As String, ex As Exception)

    Async Function ProcessAllAsync() As Task
        Await SyncToDatabaseAsync()
        ProcessNumbers()
        Await SendTweetsAsync()
    End Function

    Async Function SyncToDatabaseAsync() As Task

        ' First we need to clean up any old data
        Using connection As New MySqlConnection(My.Settings.ConnectionString)
            connection.Open()
            Dim cmd As New MySqlCommand("DELETE FROM sync_history WHERE processed=0", connection)
            cmd.ExecuteNonQuery()
        End Using

        ' Now we can sync in the new data
        Dim spotifyTask = New SpotifySync().FetchAlbumStatsAsync()
        Dim twitterTask = New TwitterSync().FetchUserStatsAsync()
        Dim youtubeTask = New YouTubeSync().FetchAllAsync()
        Try
            Await Task.WhenAll(spotifyTask, twitterTask, youtubeTask)
        Catch ex As Exception
            HandleAggregateExceptions("SpotifySync", spotifyTask.Exception)
            HandleAggregateExceptions("TwitterSync", twitterTask.Exception)
            HandleAggregateExceptions("YouTubeSync", youtubeTask.Exception)
        End Try

    End Function

    Sub ProcessNumbers()
        Using connection As New MySqlConnection(My.Settings.ConnectionString)
            connection.Open()

            Dim cmd As New MySqlCommand()
            cmd.Connection = connection
            cmd.Parameters.AddRange({
                New MySqlParameter("currentCount", MySqlDbType.Int32) With {.Value = 0},
                New MySqlParameter("newCount", MySqlDbType.Int32) With {.Value = 0},
                New MySqlParameter("newDaily", MySqlDbType.Int32) With {.Value = 0},
                New MySqlParameter("property", MySqlDbType.VarChar) With {.Value = ""},
                New MySqlParameter("propertyId", MySqlDbType.Int32) With {.Value = 0},
                New MySqlParameter("site", MySqlDbType.VarChar) With {.Value = ""},
                New MySqlParameter("text", MySqlDbType.VarChar) With {.Value = ""}
            })

            ' Pull latest sync from database
            Dim items As New List(Of SyncedItem)
            cmd.CommandText = "SELECT * FROM latest_syncs"
            Using dReader = cmd.ExecuteReader()
                While dReader.Read()
                    Dim item = New SyncedItem With {
                        .PropertyId = dReader("property_id"),
                        .TheirId = dReader("their_id"),
                        .Site = dReader("site"),
                        .Type = dReader("type"),
                        .Title = dReader("title"),
                        .Url = dReader("url"),
                        .PropertyName = dReader("property"),
                        .NewCount = dReader("new_count")}
                    If Not IsDBNull(dReader("current_count")) Then item.CurrentCount = dReader("current_count")
                    If Not IsDBNull(dReader("current_daily")) Then item.CurrentDaily = dReader("current_daily")
                    If Not IsDBNull(dReader("new_daily")) Then item.NewDaily = CInt(dReader("new_daily"))
                    items.Add(item)
                End While
            End Using

            ' Process for milestones
            Dim milestones As New List(Of MilestoneNotice)
            cmd.CommandText =
                "SELECT template FROM milestones
                WHERE site=@site AND property=@property
                  AND @newCount>=count AND @currentCount<count
                  AND @newDaily BETWEEN min_delta+1 AND max_delta
                ORDER BY count,min_delta LIMIT 1"
            For Each item In items
                If item.PropertyId = 173 Then
                    Dim x = 1
                End If
                If item.IsNew Then Continue For
                cmd.Parameters("site").Value = item.Site
                cmd.Parameters("property").Value = item.PropertyName
                cmd.Parameters("newCount").Value = item.NewCount
                cmd.Parameters("newDaily").Value = item.NewDaily
                cmd.Parameters("currentCount").Value = item.CurrentCount
                Dim template = cmd.ExecuteScalar()
                If template IsNot Nothing Then
                    milestones.Add(New MilestoneNotice With {
                        .Item = item,
                        .Template = template})
                End If
            Next

            ' Doing all writing in a transaction so we can recover on error
            Using transaction = connection.BeginTransaction()
                cmd.Transaction = transaction

                ' Save the updated info
                cmd.CommandText = "UPDATE monitored_properties SET count=@newCount,daily=@newDaily WHERE id=@propertyId"
                For Each item In items
                    cmd.Parameters("propertyId").Value = item.PropertyId
                    cmd.Parameters("newCount").Value = item.NewCount
                    cmd.Parameters("newDaily").Value = If(item.NewDaily.HasValue, item.NewDaily, DBNull.Value)
                    cmd.ExecuteNonQuery()
                Next

                ' Mark all syncs as complete
                cmd.CommandText = "UPDATE sync_history SET processed=1"
                cmd.ExecuteNonQuery()

                ' Enqueue the milestone tweets
                cmd.CommandText = "INSERT INTO queued_tweets (text) VALUES (@text)"
                For Each milestone In milestones
                    cmd.Parameters("text").Value = milestone.FormatForTweet()
                    cmd.ExecuteNonQuery()
                Next

                transaction.Commit()
            End Using

        End Using
    End Sub

    Async Function SendTweetsAsync() As Task
        Try
            Await New TwitterSync().SendQueuedTweetsAsync()
        Catch ex As Exception
            HandleException("TwitterSync", ex)
        End Try
    End Function


    Private Sub HandleAggregateExceptions(moduleName As String, ex As AggregateException)
        If ex?.InnerExceptions Is Nothing Then Exit Sub
        For Each innerException In ex.InnerExceptions
            RaiseEvent RecoverableExceptionThrown(moduleName, innerException)
        Next
    End Sub

    Private Sub HandleException(moduleName As String, ex As Exception)
        RaiseEvent RecoverableExceptionThrown(moduleName, ex)
    End Sub

End Class
