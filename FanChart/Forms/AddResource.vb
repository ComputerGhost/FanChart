Imports System.Web
Imports Google.Apis.Services
Imports Google.Apis.YouTube.v3

Public Class AddResource

    Private engine As Engine

    Overloads Sub ShowDialog(engine As Engine)
        Me.engine = engine
        MyBase.ShowDialog()
    End Sub

    Private Sub AddResource_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtURL.Text = ""
        txtURL.Select()
        txtTitle.Text = ""
        txtTitle.Enabled = False
        btnLoad.Enabled = False
    End Sub

    Private Async Sub btnLoad_Click(sender As Object, e As EventArgs) Handles btnLoad.Click
        Dim info = GetAssetUrlInfo(txtURL.Text)
        Select Case info?.type
            Case AssetUrlType.YouTubeChannel
            Case AssetUrlType.YouTubeVideo
                txtTitle.Text = Await GetVideoTitleAsync(info.identifier)
        End Select
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim info = GetAssetUrlInfo(txtURL.Text)
        Select Case info?.type

            Case AssetUrlType.SpotifyAlbum
                Dim albumId = info.identifier
                Dim api As New Spotify.API(My.Settings.SpotifyEndpoint)
                Dim tracks = api.GetAlbumPlayCount(albumId)
                For Each track In tracks
                    engine.Add(New MonitoredItem With {
                        .SourceSite = "Spotify",
                        .Identifier = String.Format("{0}:{1}:{2}", albumId, track.disc, track.number),
                        .WatchedProperty = "playcount",
                        .Title = track.name,
                        .LatestCount = track.playcount,
                        .LastUpdated = Now,
                        .LastSynced = Now})
                Next

            Case AssetUrlType.YouTubeChannel

            Case AssetUrlType.YouTubeVideo
                Dim videoId = info.identifier
                engine.Add(New MonitoredItem With {
                    .SourceSite = "YouTube",
                    .Identifier = videoId,
                    .WatchedProperty = "Views",
                    .Title = txtTitle.Text})
                engine.Add(New MonitoredItem With {
                    .SourceSite = "YouTube",
                    .Identifier = videoId,
                    .WatchedProperty = "Likes",
                    .Title = txtTitle.Text})

        End Select

        Close()
    End Sub

    Private Sub txtURL_TextChanged(sender As Object, e As EventArgs) Handles txtURL.TextChanged
        txtTitle.Enabled = False
        btnLoad.Enabled = False

        Dim uri As Uri = Nothing
        If Uri.TryCreate(txtURL.Text.ToLower, UriKind.Absolute, uri) Then
            If {"youtu.be", "youtube.com", "www.youtube.com"}.Contains(uri.Host) Then
                txtTitle.Enabled = True
                btnLoad.Enabled = True
            End If
        End If
    End Sub


    ' The following code pulls asset url type and identifier from a URL

    Private Enum AssetUrlType
        SpotifyAlbum
        YouTubeChannel
        YouTubeVideo
    End Enum

    Private Class AssetUrlInfo
        Property type As AssetUrlType
        Property identifier As String

        Sub New(type As AssetUrlType, identifier As String)
            Me.type = type
            Me.identifier = identifier
        End Sub
    End Class

    Private Function GetAssetUrlInfo(url As String) As AssetUrlInfo
        Dim uri As Uri = Nothing
        If Not Uri.TryCreate(url, UriKind.Absolute, uri) Then Return Nothing

        Select Case uri.Host
            Case "youtu.be"
                If uri.Segments.Count <> 2 Then Return Nothing
                Return New AssetUrlInfo(AssetUrlType.YouTubeVideo, uri.Segments(0))

            Case "youtube.com", "www.youtube.com"
                If uri.Segments.Count >= 3 AndAlso uri.Segments.First() = "channel" Then
                    Return New AssetUrlInfo(AssetUrlType.YouTubeChannel, uri.Segments(2))

                ElseIf uri.Segments.Count = 2 AndAlso uri.Segments.Last() = "watch" Then
                    Dim parameters = HttpUtility.ParseQueryString(uri.Query)
                    If Not parameters.AllKeys.Contains("v") Then Return Nothing
                    Return New AssetUrlInfo(AssetUrlType.YouTubeVideo, parameters("v"))
                End If

            Case "open.spotify.com"
                If uri.Segments.Count <> 3 Then Return Nothing
                Return New AssetUrlInfo(AssetUrlType.SpotifyAlbum, uri.Segments(2))

        End Select

        Return Nothing
    End Function


    ' YouTube helpers

    Private Async Function GetVideoTitleAsync(videoId As String) As Task(Of String)
        Dim service As New YouTubeService(New BaseClientService.Initializer With {
            .ApiKey = My.Settings.YoutubeApiKey})
        Dim request = New VideosResource(service).List("snippet")
        request.Id = videoId
        Dim response = Await request.ExecuteAsync()

        If response.Items.Count = 0 Then Return Nothing
        Return response.Items.First().Snippet.Title
    End Function

End Class