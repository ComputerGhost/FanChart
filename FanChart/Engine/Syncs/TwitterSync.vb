﻿Imports MySql.Data.MySqlClient

Public Class TwitterSync
    Inherits SyncBase

    Property Service As New TwitterAPI

    Async Function SendQueuedTweetsAsync() As Task
        Using connection As New MySqlConnection(My.Settings.ConnectionString)
            connection.Open()
            Dim cmd As New MySqlCommand()
            cmd.Connection = connection

            Dim tweets As New Dictionary(Of Integer, String)
            cmd.CommandText = "SELECT id,text FROM queued_tweets"
            Using dReader = cmd.ExecuteReader()
                While dReader.Read()
                    tweets.Add(dReader(0), dReader(1))
                End While
            End Using

            cmd.CommandText = "DELETE FROM queued_tweets WHERE id=@id"
            cmd.Parameters.Add("id", MySqlDbType.Int32)
            For Each tweet In tweets
                Await Service.Tweet(tweet.Value)
                cmd.Parameters("id").Value = tweet.Key
                cmd.ExecuteNonQuery()
            Next

        End Using
    End Function

End Class
