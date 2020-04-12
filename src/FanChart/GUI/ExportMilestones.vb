Imports System.IO
Imports Csv
Imports MySql.Data.MySqlClient

Public Class ExportMilestones

    Private Sub ExportMilestones_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ddlSiteProperty.SelectedIndex = -1
        txtFilename.Text = ""
    End Sub

    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
        dlgSave.ShowDialog()
        txtFilename.Text = dlgSave.FileName
    End Sub

    Private Sub txtFilename_TextChanged(sender As Object, e As EventArgs) Handles txtFilename.TextChanged
        btnExport.Enabled = (txtFilename.Text.Length > 0) And (ddlSiteProperty.SelectedIndex >= 0)
    End Sub

    Private Sub ddlSiteProperty_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlSiteProperty.SelectedIndexChanged
        btnExport.Enabled = (txtFilename.Text.Length > 0) And (ddlSiteProperty.SelectedIndex >= 0)
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        Dim site = ddlSiteProperty.Text.Split(" ")(0)
        Dim prop = ddlSiteProperty.Text.Split(" ")(1)
        ExportData(txtFilename.Text, site, prop)
        Close()
    End Sub

    Private Sub ExportData(filename As String, site As String, prop As String)
        Dim lines As New List(Of String())
        Using connection As New MySqlConnection(My.Settings.ConnectionString)
            connection.Open()
            Dim cmd As New MySqlCommand(
                "SELECT count,min_delta,max_delta,template 
                FROM milestones 
                WHERE site=@site AND property=@property", connection)
            cmd.Parameters.AddWithValue("site", site)
            cmd.Parameters.AddWithValue("property", prop)

            Dim reader = cmd.ExecuteReader()
            While reader.Read()
                lines.Add({reader("count"), reader("min_delta"), reader("max_delta"), reader("template")})
            End While
        End Using

        Dim columns = {"count", "min_delta", "max_delta", "template"}
        Dim data = CsvWriter.WriteToText(columns, lines)
        File.WriteAllText(filename, data)
    End Sub

End Class