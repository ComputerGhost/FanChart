Imports System.Web
Imports Google.Apis.Services
Imports Google.Apis.YouTube.v3
Imports MySql.Data.MySqlClient

Public Class AddResource

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
                txtTitle.Text = Await GetChannelTitleAsync(info.identifier)
            Case AssetUrlType.YouTubeVideo
                txtTitle.Text = Await GetVideoTitleAsync(info.identifier)
        End Select
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim info = GetAssetUrlInfo(txtURL.Text)
        Select Case info?.type

            Case AssetUrlType.SpotifyAlbum
                For Each track In New SpotifyAPI().GetAlbumPlayCountAsync(info.identifier).Result
                    AddItem("Spotify", "Song", info.identifier, "Plays", txtTitle.Text, txtURL.Text)
                Next

            Case AssetUrlType.YouTubeChannel
                AddItem("YouTube", "Channel", info.identifier, "Subscribers", txtTitle.Text, txtURL.Text)

            Case AssetUrlType.YouTubeVideo
                AddItem("YouTube", "Video", info.identifier, "Views", txtTitle.Text, txtURL.Text)
                AddItem("YouTube", "Video", info.identifier, "Likes", txtTitle.Text, txtURL.Text)

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


    Private Sub AddItem(site As String, type As String, theirId As String, propName As String, title As String, url As String)
        Using connection As New MySqlConnection(My.Settings.ConnectionString)
            connection.Open()

            Dim cmd = connection.CreateCommand()
            cmd.Parameters.AddRange({
                New MySqlParameter("site", MySqlDbType.VarChar) With {.Value = site},
                New MySqlParameter("type", MySqlDbType.VarChar) With {.Value = type},
                New MySqlParameter("theirId", MySqlDbType.VarChar) With {.Value = theirId},
                New MySqlParameter("propName", MySqlDbType.VarChar) With {.Value = propName},
                New MySqlParameter("title", MySqlDbType.VarChar) With {.Value = title},
                New MySqlParameter("url", MySqlDbType.VarChar) With {.Value = url},
                New MySqlParameter("monitoredId", MySqlDbType.Int32) With {.Value = 0}})

            ' Get monitored item; insert if it doesn't exist
            cmd.CommandText = "SELECT id FROM monitored_items WHERE site=@site AND type=@type AND their_id=@theirId"
            Dim monitoredId = cmd.ExecuteScalar()
            If monitoredId Is Nothing Then
                cmd.CommandText = "INSERT INTO monitored_items (site,their_id,type,title,url) VALUES (@site,@theirId,@type,@title,@url);"
                cmd.ExecuteNonQuery()
                monitoredId = cmd.LastInsertedId()
            End If
            cmd.Parameters("monitoredId").Value = monitoredId

            ' Check if property already exists.  If not, add it.
            cmd.CommandText = "SELECT COUNT(*) FROM monitored_properties WHERE monitored_id=@monitoredId AND property=@propName"
            If cmd.ExecuteScalar() > 0 Then Exit Sub
            cmd.CommandText = "INSERT INTO monitored_properties (monitored_id,property) VALUES (@monitoredid,@propName)"
            cmd.ExecuteNonQuery()

        End Using
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
                Return New AssetUrlInfo(AssetUrlType.YouTubeVideo, uri.Segments(1))

            Case "youtube.com", "www.youtube.com"
                If uri.Segments.Count >= 3 AndAlso uri.Segments(1) = "channel/" Then
                    Return New AssetUrlInfo(AssetUrlType.YouTubeChannel, uri.Segments(2))

                ElseIf uri.Segments.Count = 2 AndAlso uri.Segments(1) = "watch" Then
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

    Private Async Function GetChannelTitleAsync(channelId As String) As Task(Of String)
        Dim service As New YouTubeService(New BaseClientService.Initializer With {
            .ApiKey = My.Settings.YoutubeApiKey})
        Dim request = New ChannelsResource(service).List("snippet")
        request.Id = channelId
        Dim response = Await request.ExecuteAsync()

        If response.Items.Count = 0 Then Return Nothing
        Return response.Items.First().Snippet.Title
    End Function

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