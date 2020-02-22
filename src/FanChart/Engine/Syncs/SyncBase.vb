Imports MySql.Data.MySqlClient

Public Class SyncBase

    Structure MonitoredId
        Property OurId As Integer
        Property TheirId As String
    End Structure

    Function GetMonitoredIdsFor(site As String, type As String) As List(Of MonitoredId)
        Dim ids As New List(Of MonitoredId)
        Using connection As New MySqlConnection(My.Settings.ConnectionString)
            connection.Open()

            Dim cmd As New MySqlCommand(
                "SELECT id,their_id FROM monitored_items
                WHERE site=@site AND type=@type", connection)
            cmd.Parameters.AddWithValue("site", site)
            cmd.Parameters.AddWithValue("type", type)

            Dim dReader = cmd.ExecuteReader()
            While dReader.Read()
                ids.Add(New MonitoredId With {
                    .OurId = dReader(0),
                    .TheirId = dReader(1)})
            End While

        End Using
        Return ids
    End Function

    Sub SaveUpdate(monitoredId As Integer, propName As String, count As Integer)
        Using connection As New MySqlConnection(My.Settings.ConnectionString)
            connection.Open()
            Dim cmd As New MySqlCommand(
                "INSERT INTO sync_history (property_id, count)
                SELECT id,@count FROM monitored_properties
                WHERE monitored_id=@monitoredId AND property=@propName",
                connection)
            cmd.Parameters.AddWithValue("monitoredId", monitoredId)
            cmd.Parameters.AddWithValue("propName", propName)
            cmd.Parameters.AddWithValue("count", count)
            cmd.ExecuteNonQuery()
        End Using
    End Sub

End Class
