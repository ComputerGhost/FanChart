Imports Spotify

Public Class AddSpotifyAlbum

    Private monitor As Engine

    Overloads Sub ShowDialog(monitor As Engine)
        Me.monitor = monitor
        MyBase.ShowDialog()
    End Sub

    Private Sub AddSpotifyAlbum_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtURL.Text = ""
        txtURL.Focus()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim albumId = GetAlbumIdFromUrl(txtURL.Text)
        If albumId Is Nothing Then Exit Sub

        Dim api As New API(My.Settings.SpotifyEndpoint)
        Dim tracks = api.GetAlbumPlayCount(albumId)

        For Each track In tracks
            monitor.Add(New MonitoredItem With {
                .SourceSite = "Spotify",
                .Identifier = String.Format("{0}:{1}:{2}", albumId, track.disc, track.number),
                .WatchedProperty = "playcount",
                .Title = track.name,
                .LatestCount = track.playcount,
                .LastUpdated = Now})
        Next

        Close()
    End Sub

    Private Function GetAlbumIdFromUrl(url As String)
        Dim uri As Uri = Nothing
        If Not Uri.TryCreate(url, UriKind.Absolute, uri) Then Return Nothing
        If uri.Segments.Count = 0 Then Return Nothing

        Return uri.Segments.Last()
    End Function
End Class