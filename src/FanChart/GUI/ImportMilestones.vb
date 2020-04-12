Imports System.IO
Imports Csv
Imports MySql.Data.MySqlClient
Imports [Shared].ComponentModel.SortableBindingList

Public Class ImportMilestones

    Public Structure RowItem
        Property count As Integer
        Property min_delta As Integer
        Property max_delta As Integer
        Property template As String
    End Structure
    Property RowItems As List(Of RowItem)

    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
        dlgOpen.ShowDialog()
        txtFilename.Text = dlgOpen.FileName
    End Sub

    Private Sub txtFilename_TextChanged(sender As Object, e As EventArgs) Handles txtFilename.TextChanged
        btnImport.Enabled = ShouldEnableImport()
        LoadData(txtFilename.Text)
        RefreshList()
    End Sub

    Private Sub ddlSiteProperty_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlSiteProperty.SelectedIndexChanged
        btnImport.Enabled = ShouldEnableImport()
    End Sub

    Private Sub btnImport_Click(sender As Object, e As EventArgs) Handles btnImport.Click
        Dim site = ddlSiteProperty.Text.Split(" ")(0)
        Dim prop = ddlSiteProperty.Text.Split(" ")(1)
        ImportData(site, prop)
        Close()
    End Sub


    Private Function ShouldEnableImport() As Boolean
        If txtFilename.Text.Length = 0 Then Return False
        If ddlSiteProperty.SelectedIndex < 0 Then Return False
        If RowItems Is Nothing Then Return False
        Return True
    End Function

    ' Loads from file into RAM
    Private Sub LoadData(filename As String)
        RowItems = New List(Of RowItem)
        Try
            Dim data = File.ReadAllText(filename)
            For Each line In CsvReader.ReadFromText(data)
                RowItems.Add(New RowItem With {
                    .count = line(0),
                    .min_delta = line(1),
                    .max_delta = line(2),
                    .template = line(3)
                })
            Next
        Catch ex As Exception
            RowItems = Nothing
        End Try
    End Sub

    ' Refresh the UI preview of the data
    Private Sub RefreshList()
        lstPreview.Items.Clear()
        If RowItems IsNot Nothing Then
            For Each row In RowItems
                lstPreview.Items.Add(New ListViewItem({
                    row.count, row.min_delta, row.max_delta, row.template
                }))
            Next
        End If
    End Sub

    ' Moves the loaded/previewed data into the database
    Private Sub ImportData(site As String, prop As String)
        Using connection As New MySqlConnection(My.Settings.ConnectionString)
            connection.Open()
            Using transaction = connection.BeginTransaction()
                Dim cmd As New MySqlCommand(connection, transaction)
                cmd.Parameters.AddWithValue("site", site)
                cmd.Parameters.AddWithValue("property", prop)

                cmd.CommandText = "DELETE FROM milestones WHERE site=@site AND property=@property"
                cmd.ExecuteNonQuery()

                cmd.CommandText =
                    "INSERT INTO milestones (site, property, count, min_delta, max_delta, template)
                    VALUES (@site, @property, @count, @min_delta, @max_delta, @template)"
                cmd.Parameters.Add("count", MySqlDbType.Int32)
                cmd.Parameters.Add("min_delta", MySqlDbType.Int32)
                cmd.Parameters.Add("max_delta", MySqlDbType.Int32)
                cmd.Parameters.Add("template", MySqlDbType.VarChar)
                For Each row In RowItems
                    cmd.Parameters("count").Value = row.count
                    cmd.Parameters("min_delta").Value = row.min_delta
                    cmd.Parameters("max_delta").Value = row.max_delta
                    cmd.Parameters("template").Value = row.template
                    cmd.ExecuteNonQuery()
                Next

                transaction.Commit()
            End Using
        End Using
    End Sub

End Class