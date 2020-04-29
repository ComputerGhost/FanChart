Imports MySql.Data.MySqlClient
Imports [Shared].ComponentModel.SortableBindingList

Public Class Main

    Public Structure RowItem
        Property MonitoredId As Integer
        Property PropertyId As Integer
        Property Site As String
        Property Type As String
        Property TheirId As String
        Property PropertyName As String
        Property Title As String
        Property Url As String
    End Structure
    Property RowItemBindingSource As SortableBindingList(Of RowItem)

    Sub New()
        InitializeComponent()

        ' Additional form setup
        RowItemBindingSource = New SortableBindingList(Of RowItem)
        gvItems.AutoGenerateColumns = False
        gvItems.DataSource = RowItemBindingSource
        gvItems.Columns.AddRange(New DataGridViewColumn() {
            New DataGridViewTextBoxColumn With {.DataPropertyName = "PropertyId", .HeaderText = "Our Id", .Width = 70},
            New DataGridViewTextBoxColumn With {.DataPropertyName = "Site", .HeaderText = "Site", .Width = 70},
            New DataGridViewTextBoxColumn With {.DataPropertyName = "Type", .HeaderText = "Type", .Width = 60},
            New DataGridViewTextBoxColumn With {.DataPropertyName = "TheirId", .HeaderText = "Their Id", .Width = 175},
            New DataGridViewTextBoxColumn With {.DataPropertyName = "PropertyName", .HeaderText = "Property", .Width = 60},
            New DataGridViewTextBoxColumn With {.DataPropertyName = "Title", .HeaderText = "Title", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill},
            New DataGridViewTextBoxColumn With {.DataPropertyName = "Url", .HeaderText = "Url", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill}
        })

    End Sub

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

    Private Sub ImportToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ImportToolStripMenuItem.Click
        ImportMilestones.ShowDialog()
    End Sub

    Private Sub ExportToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExportToolStripMenuItem.Click
        ExportMilestones.ShowDialog()
    End Sub

    Private Sub AddResourceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddResourceToolStripMenuItem.Click
        If AddResource.ShowDialog() = DialogResult.OK Then
            For Each propertyId In AddResource.propertyIds
                RowItemBindingSource.Add(GetItemFromDB(propertyId))
            Next
        End If
    End Sub

    Private Sub RemoveSelectedToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RemoveSelectedToolStripMenuItem.Click
        For Each row As DataGridViewRow In gvItems.SelectedRows
            RemoveItemFromDB(row.DataBoundItem.PropertyId)
            RowItemBindingSource.Remove(row.DataBoundItem)
        Next
    End Sub

    Private Sub EditSelectedToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EditSelectedToolStripMenuItem.Click
        If gvItems.SelectedRows.Count = 0 Then Exit Sub
        EditItem(gvItems.SelectedRows(0).DataBoundItem.PropertyId)
    End Sub

    Private Sub gvItems_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles gvItems.CellDoubleClick
        If e.RowIndex < 0 Then Exit Sub
        EditItem(gvItems.Rows(e.RowIndex).DataBoundItem.PropertyId)
    End Sub


    Private Sub LogException(moduleName As String, ex As Exception)
        Dim message = String.Format("Error in `{0}`: {1}{2}Stacktrace: {3}", moduleName, ex.Message, vbCrLf, ex.StackTrace)
        MessageBox.Show(message, "Error")
        LogText(message)
    End Sub

    Private Sub LogText(text As String)
        'Dim timestamp = Now.ToString("yyyy-MM-dd HH:mm:ss")
        'txtLog.AppendText(String.Format("{0}: {1}{2}{2}", timestamp, text, vbCrLf))
    End Sub


    Private Sub EditItem(propertyId As Integer)
        If EditResource.ShowDialog(propertyId) = DialogResult.OK Then
            For i = 0 To RowItemBindingSource.Count - 1
                Dim item As RowItem = RowItemBindingSource(i)
                If item.MonitoredId = EditResource.monitoredId Then
                    RowItemBindingSource(i) = GetItemFromDB(item.PropertyId)
                End If
            Next
        End If
    End Sub

    Private Sub RefreshItems()

        RowItemBindingSource.Clear()

        Using connection As New MySqlConnection(My.Settings.ConnectionString)
            connection.Open()

            Dim cmd As New MySqlCommand(
                "SELECT i.id`monitoredId`,p.id`propertyId`,i.site,i.type,i.their_id,p.property,i.title,i.url
                FROM monitored_properties p LEFT JOIN monitored_items i ON i.id=p.monitored_id
                ORDER BY i.id DESC", connection)

            Dim dReader = cmd.ExecuteReader()
            While dReader.Read()
                RowItemBindingSource.Add(New RowItem With {
                    .MonitoredId = dReader("monitoredId"),
                    .PropertyId = dReader("propertyId"),
                    .Site = dReader("site"),
                    .Type = dReader("type"),
                    .TheirId = dReader("their_id"),
                    .PropertyName = dReader("property"),
                    .Title = dReader("title"),
                    .Url = dReader("url")})
            End While

        End Using

    End Sub


    Private Function GetItemFromDB(propertyId As Integer) As RowItem
        Using connection As New MySqlConnection(My.Settings.ConnectionString)
            connection.Open()
            Dim cmd As New MySqlCommand(
                "SELECT i.id`monitoredId`,p.id`propertyId`,i.site,i.type,i.their_id,p.property,i.title,i.url
                FROM monitored_properties p LEFT JOIN monitored_items i ON i.id=p.monitored_id
                WHERE p.id=@propertyId", connection)
            cmd.Parameters.AddWithValue("propertyId", propertyId)

            Dim dReader = cmd.ExecuteReader()
            dReader.Read()

            Return New RowItem With {
                .MonitoredId = dReader("monitoredId"),
                .PropertyId = dReader("propertyId"),
                .Site = dReader("site"),
                .Type = dReader("type"),
                .TheirId = dReader("their_id"),
                .PropertyName = dReader("property"),
                .Title = dReader("title"),
                .Url = dReader("url")}
        End Using
    End Function

    Private Sub RemoveItemFromDB(propertyId As Integer)
        Using connection As New MySqlConnection(My.Settings.ConnectionString)
            connection.Open()
            Dim cmd As New MySqlCommand()
            cmd.Connection = connection
            cmd.Parameters.Add("id", MySqlDbType.Int32).Value = propertyId

            ' We'll need this info after we delete the property
            cmd.CommandText = "SELECT monitored_id FROM monitored_properties WHERE id=@id"
            Dim monitoredId As Integer = cmd.ExecuteScalar()

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
    End Sub

End Class