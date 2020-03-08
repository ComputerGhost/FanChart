Imports MySql.Data.MySqlClient

Public Class EditResource

    Property monitoredId As Integer

    Overloads Function ShowDialog(propertyId As Integer) As DialogResult

        Using connection As New MySqlConnection(My.Settings.ConnectionString)
            connection.Open()
            Dim cmd As New MySqlCommand(
                "SELECT i.id, i.site, i.their_id, i.type, i.title, i.url
                FROM monitored_properties p LEFT JOIN monitored_items i ON i.id=p.monitored_id
                WHERE p.id=@propertyId LIMIT 1", connection)
            cmd.Parameters.AddWithValue("propertyId", propertyId)

            Dim dReader = cmd.ExecuteReader()
            dReader.Read()

            monitoredId = dReader("id")
            txtType.Text = String.Format("{0} {1}", dReader("site"), dReader("type"))
            txtIdentifier.Text = dReader("their_id")
            txtTitle.Text = dReader("title")
            txtUrl.Text = dReader("url")

        End Using

        Return MyBase.ShowDialog()
    End Function

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        Using connection As New MySqlConnection(My.Settings.ConnectionString)
            connection.Open()

            Dim cmd = connection.CreateCommand()
            cmd.CommandText = "UPDATE monitored_items SET title=@title, url=@url WHERE id=@monitoredId"
            cmd.Parameters.AddRange({
                New MySqlParameter("monitoredId", MySqlDbType.Int32) With {.Value = monitoredId},
                New MySqlParameter("title", MySqlDbType.VarChar) With {.Value = txtTitle.Text},
                New MySqlParameter("url", MySqlDbType.VarChar) With {.Value = txtUrl.Text}})
            cmd.ExecuteNonQuery()

        End Using

        DialogResult = DialogResult.OK
        Close()

    End Sub

End Class