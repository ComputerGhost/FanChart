﻿Imports MySql.Data.MySqlClient

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
                New MySqlParameter("syncId", MySqlDbType.Int32) With {.Value = 0},
                New MySqlParameter("text", MySqlDbType.VarChar) With {.Value = ""}
            })

            ' Pull latest sync from database
            Dim items As New List(Of SyncedItem)
            cmd.CommandText = "SELECT * FROM latest_syncs"
            Using dReader = cmd.ExecuteReader()
                While dReader.Read()
                    items.Add(New SyncedItem With {
                        .SyncId = dReader("sync_id"),
                        .PropertyId = dReader("property_id"),
                        .TheirId = dReader("their_id"),
                        .Site = dReader("site"),
                        .Type = dReader("type"),
                        .Title = dReader("title"),
                        .Url = dReader("url"),
                        .PropertyName = dReader("property"),
                        .CurrentCount = If(IsDBNull(dReader("current_count")), Nothing, CInt(dReader("current_count"))),
                        .CurrentDaily = If(IsDBNull(dReader("current_daily")), Nothing, CInt(dReader("current_daily"))),
                        .NewCount = dReader("new_count"),
                        .NewDaily = If(IsDBNull(dReader("new_daily")), Nothing, CInt(dReader("new_daily")))
                    })
                End While
            End Using

            ' Save the updated info
            cmd.CommandText = "UPDATE monitored_properties SET count=@newCount,daily=@newDaily WHERE id=@propertyId"
            For Each item In items
                cmd.Parameters("propertyId").Value = item.PropertyId
                cmd.Parameters("newCount").Value = item.NewCount
                cmd.Parameters("newDaily").Value = item.NewDaily
                cmd.ExecuteNonQuery()
            Next

            ' Process for milestones
            Dim milestones As New List(Of MilestoneNotice)
            cmd.CommandText =
                "SELECT template FROM milestones
                WHERE site=@site AND property=@property
                  AND @newCount>count AND @currentCount<count
                  AND @newDaily BETWEEN min_delta+1 AND max_delta
                ORDER BY count,min_delta LIMIT 1"
            For Each item In items
                If item.IsNew Then
                    Continue For
                End If
                cmd.Parameters("site").Value = item.Site
                cmd.Parameters("property").Value = item.PropertyName
                cmd.Parameters("newCount").Value = item.NewCount
                cmd.Parameters("newDaily").Value = item.NewDaily
                cmd.Parameters("currentCount").Value = item.CurrentCount
                Dim template = cmd.ExecuteScalar()
                If template IsNot Nothing Then
                    milestones.Add(New MilestoneNotice With {
                        .Item = item,
                        .Template = template
                    })
                End If
            Next

            ' Enqueue the milestone tweets
            cmd.CommandText = "INSERT INTO queued_tweets (text) VALUES (@text)"
            For Each milestone In milestones
                cmd.Parameters("text").Value = milestone.FormatForTweet()
                cmd.ExecuteNonQuery()
            Next

            ' Mark the sync as complete
            cmd.CommandText = "UPDATE sync_history SET processed=1 WHERE id=@syncId"
            For Each item In items
                cmd.Parameters("syncId").Value = item.SyncId
                cmd.ExecuteNonQuery()
            Next

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