Imports MySql.Data.MySqlClient

Public Class Main

    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If String.IsNullOrEmpty(My.Settings.ConnectionString) Then
            EditAccounts.ShowDialog()
        End If
        RefreshItems()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Close()
    End Sub

    Private Async Sub RunNowToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RunNowToolStripMenuItem.Click
        Try
            RunNowToolStripMenuItem.Enabled = False
            LogText("Running...")
            lblStatus.Text = "Running..."

            Dim process As New EngineProcess
            AddHandler process.RecoverableExceptionThrown, AddressOf LogException
            Await process.ProcessAllAsync()

            RunNowToolStripMenuItem.Enabled = True
            LogText("Completed.")
            lblStatus.Text = "Completed"

        Catch ex As Exception
            RunNowToolStripMenuItem.Enabled = True
            LogException("EngineProcess", ex)
            lblStatus.Text = "Error: " & ex.Message
        End Try
    End Sub

    Private Sub AccountsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AccountsToolStripMenuItem.Click
        EditAccounts.ShowDialog()
    End Sub

    Private Sub AddResourceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddResourceToolStripMenuItem.Click
        AddResource.ShowDialog()
        RefreshItems()
    End Sub

    Private Sub RemoveCurrentToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RemoveCurrentToolStripMenuItem.Click
        Dim removingItems As New List(Of ListViewItem)
        removingItems.AddRange(lstMonitoredItems.SelectedItems)
        For Each item In removingItems
            RemoveItem(item)
        Next
    End Sub


    Private Sub LogException(moduleName As String, ex As Exception)
        LogText(String.Format("Error in `{0}`: {1}{2}Stacktrace: {3}", moduleName, ex.Message, vbCrLf, ex.StackTrace))
    End Sub

    Private Sub LogText(text As String)
        Dim timestamp = Now.ToString("yyyy-MM-dd HH:mm:ss")
        txtLog.AppendText(String.Format("{0}: {1}{2}{2}", timestamp, text, vbCrLf))
    End Sub


    Private Sub RefreshItems()

        lstMonitoredItems.Items.Clear()

        Using connection As New MySqlConnection(My.Settings.ConnectionString)
            connection.Open()

            Dim cmd As New MySqlCommand(
                "SELECT p.id,i.site,i.type,i.their_id,p.property,i.title
                FROM monitored_properties p LEFT JOIN monitored_items i ON i.id=p.monitored_id
                ORDER BY i.site,i.type,i.their_id,p.property", connection)

            Dim dReader = cmd.ExecuteReader()
            While dReader.Read()
                Dim item = New ListViewItem({
                    String.Format("{0}:{1}", dReader("site"), dReader("type")),
                    dReader("their_id"),
                    dReader("property"),
                    dReader("title")})
                item.Tag = dReader("id")
                lstMonitoredItems.Items.Add(item)
            End While

        End Using
    End Sub

    Private Sub RemoveItem(listItem As ListViewItem)
        Using connection As New MySqlConnection(My.Settings.ConnectionString)
            connection.Open()
            Dim cmd As New MySqlCommand()
            cmd.Connection = connection
            cmd.Parameters.Add("id", MySqlDbType.Int32).Value = listItem.Tag

            ' We'll need this info after we delete the property
            cmd.CommandText = "SELECT monitored_id FROM monitored_properties WHERE id=@id"
            Dim monitoredId = cmd.ExecuteNonQuery()

            ' Delete the property
            cmd.CommandText =
                "DELETE FROM monitored_properties WHERE id=@id;
                DELETE FROM sync_history WHERE property_id=@id"
            cmd.ExecuteNonQuery()

            ' If the monitored item has no more monitored properties, delete it too
            cmd.Parameters("id").Value = monitoredId
            cmd.CommandText = "SELECT COUNT(*) FROM monitored_properties WHERE monitored_id=@id"
            If cmd.ExecuteScalar() = 0 Then
                cmd.CommandText = "DELETE FROM monitored_items WHERE id=@id"
                cmd.ExecuteNonQuery()
            End If
        End Using

        ' Finally, remove the list item
        lstMonitoredItems.Items.Remove(listItem)
    End Sub

End Class