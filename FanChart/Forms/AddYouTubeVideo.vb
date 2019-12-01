Imports System.Web
Imports Google.Apis.Services
Imports Google.Apis.YouTube.v3

Public Class AddYouTubeVideo

    Private monitor As Engine

    Overloads Sub ShowDialog(monitor As Engine)
        Me.monitor = monitor
        MyBase.ShowDialog()
    End Sub

    Private Sub AddYouTubeVideo_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtTitle.Text = ""
        txtURL.Text = ""
    End Sub

    Private Async Sub btnLoad_Click(sender As Object, e As EventArgs) Handles btnLoad.Click
        Dim videoId = GetVideoIdFromUrl(txtURL.Text)
        If videoId IsNot Nothing Then
            txtTitle.Text = Await LoadTitleAsync(videoId)
        End If
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim videoId = GetVideoIdFromUrl(txtURL.Text)
        If videoId Is Nothing Then Exit Sub

        monitor.Add(New MonitoredItem With {
            .SourceSite = "YouTube",
            .Identifier = videoId,
            .WatchedProperty = "Views",
            .Title = txtTitle.Text})
        monitor.Add(New MonitoredItem With {
            .SourceSite = "YouTube",
            .Identifier = videoId,
            .WatchedProperty = "Likes",
            .Title = txtTitle.Text})

        Close()
    End Sub

    Private Async Function LoadTitleAsync(videoId As String) As Task(Of String)
        Dim service As New YouTubeService(New BaseClientService.Initializer With {
            .ApiKey = My.Settings.YoutubeApiKey})
        Dim request = New VideosResource(service).List("snippet")
        request.Id = videoId
        Dim response = Await request.ExecuteAsync()

        If response.Items.Count = 0 Then Return Nothing
        Return response.Items.First().Snippet.Title
    End Function

    Private Function GetVideoIdFromUrl(url As String) As String
        Dim uri As Uri = Nothing
        If Not Uri.TryCreate(url, UriKind.Absolute, uri) Then Return Nothing
        If uri.Segments.Count = 0 Then Return Nothing

        If uri.Segments.Last() = "watch" Then
            Dim parameters = HttpUtility.ParseQueryString(uri.Query)
            If Not parameters.AllKeys.Contains("v") Then Return Nothing
            Return parameters("v")
        Else
            Return uri.Segments.Last()
        End If
    End Function
End Class