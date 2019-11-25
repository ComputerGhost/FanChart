Imports System.IO
Imports Google.Apis.Services
Imports Google.Apis.YouTube.v3
Imports Newtonsoft.Json

Public Class Monitor

    Property Items As New Dictionary(Of String, MonitoredItem)

    Event ItemAdded(monitoredItem As MonitoredItem)
    Event ItemsLoaded(monitoredItems As MonitoredItem())
    Event ItemRemoved(monitoredItem As MonitoredItem)
    Event ItemUpdated(monitoredItem As MonitoredItem)

    Sub Add(item As MonitoredItem)
        If Items.ContainsKey(item.UniqueKey) Then Exit Sub
        Items.Add(item.UniqueKey, item)
        RaiseEvent ItemAdded(item)
    End Sub

    Sub Remove(item As MonitoredItem)
        If Items.ContainsKey(item.UniqueKey) Then
            Dim removedItem = Items(item.UniqueKey)
            Items.Remove(item.UniqueKey)
            RaiseEvent ItemRemoved(removedItem)
        End If
    End Sub

    Sub Update(key As String, latestCount As Integer, tweetTemplate As String)
        If Not Items.ContainsKey(key) Then Exit Sub
        Dim item = Items(key)

        Const DIGITS = 2

        ' Skip if updated today or 2 significant digits are the same
        Dim truncatedNewCount As Integer
        If item.LastUpdated.HasValue And item.LatestCount.HasValue Then
            If DateDiff(DateInterval.Hour, item.LastUpdated.Value, Now) < 24 Then Exit Sub
            Dim scale = Math.Pow(10, Math.Floor(Math.Log10(latestCount)) + 1 - DIGITS)
            truncatedNewCount = scale * Math.Round(latestCount / scale, DIGITS)
            Dim truncatedOldCount = CInt(scale * Math.Round(item.LatestCount.Value / scale, DIGITS))
            If Math.Abs(truncatedOldCount - truncatedNewCount) < 0.5 Then Exit Sub ' (a-b)<e for double errors
        End If

        ' Calculate the daily average
        Dim dailyAverage As Double? = Nothing
        If item.LatestCount.HasValue And item.LastUpdated.HasValue Then
            Dim updatedDiff = DateDiff(DateInterval.Hour, Now, item.LastUpdated.Value) / 24
            Dim countDiff = latestCount - item.LatestCount.Value
            dailyAverage = countDiff / updatedDiff
        End If

        ' Get plain English version of latestCount
        Dim latestCountEnglish As String = latestCount
        Dim digitCount = Math.Floor(Math.Log10(latestCount) + 1)
        If digitCount <= 3 Then
            latestCountEnglish = latestCount
        ElseIf digitCount <= 6 Then
            latestCountEnglish = String.Format("{0} thousand", latestCount / 1000)
        ElseIf digitCount <= 9 Then
            latestCountEnglish = String.Format("{0} million", latestCount / 1000000)
        ElseIf digitCount <= 12 Then
            latestCountEnglish = String.Format("{0} billion", latestCount / 1000000000)
        End If

        ' Send tweet
        Dim twitter As New Twitter.API(My.Settings.TwitterAppToken, My.Settings.TwitterAppSecret, My.Settings.TwitterUserToken, My.Settings.TwitterUserSecret)
        twitter.Tweet(tweetTemplate _
            .Replace("{title}", item.Title) _
            .Replace("{count}", latestCountEnglish) _
            .Replace("{daily}", If(dailyAverage, "")) _
            .Replace("{link}", ""))

        ' Update in our list
        item.DailyAverage = dailyAverage
        item.LastUpdated = Now
        item.LatestCount = latestCount
        Items(key) = item
        RaiseEvent ItemUpdated(item)

    End Sub

    Sub Load(filename As String)
        If File.Exists(filename) Then
            Dim contents = File.ReadAllText(filename)
            Items = JsonConvert.DeserializeObject(Of Dictionary(Of String, MonitoredItem))(contents)
            RaiseEvent ItemsLoaded(Items.Values.ToArray())
        End If
    End Sub

    Sub Save(filename As String)
        Using out = File.CreateText(filename)
            Call New JsonSerializer().Serialize(out, Items)
        End Using
    End Sub

    Sub Run()
        Dim spotifyAlbums As New HashSet(Of String)
        Dim youtubeVideos As New HashSet(Of String)
        For Each item In Items
            Dim keyParts = item.Key.Split(":"c) ' source:itemId:[number:]property
            Select Case keyParts(0)
                Case "Spotify"
                    spotifyAlbums.Add(keyParts(1))
                Case "YouTube"
                    youtubeVideos.Add(keyParts(1))
            End Select
        Next

        Dim spotify As New Spotify.API(My.Settings.SpotifyEndpoint)
        For Each albumId In spotifyAlbums
            Dim tracks = TryGetSpotifyAlbumPlayCount(spotify, albumId)
            If tracks Is Nothing Then Continue For

            For Each track In tracks
                Dim key = String.Format("Spotify:{0}:{1}:{2}:playcount", albumId, track.disc, track.number)
                Update(key, track.playcount, My.Settings.SpotifyStreamsNotice)
            Next
        Next

        Dim youtube As New YouTubeService(New BaseClientService.Initializer With {
            .ApiKey = My.Settings.YoutubeApiKey})
        For Each videoId In youtubeVideos
            Dim stats = TryGetYoutubeVideoStats(youtube, videoId)
            If stats Is Nothing Then Continue For

            Update(String.Format("YouTube:{0}:Likes", videoId), stats.LikeCount, My.Settings.YoutubeLikesNotice)
            Update(String.Format("YouTube:{0}:Views", videoId), stats.ViewCount, My.Settings.YoutubeViewsNotice)
        Next
    End Sub

    Private Function TryGetSpotifyAlbumPlayCount(spotify As Spotify.API, albumId As Integer) As Spotify.Track()
        Try
            Return spotify.GetAlbumPlayCount(albumId)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Private Function TryGetYoutubeVideoStats(youtube As YouTubeService, videoId As String) As Data.VideoStatistics
        Try
            Dim request = New VideosResource(youtube).List("statistics")
            request.Id = videoId
            Dim response = request.Execute()

            If response.Items.Count = 0 Then Return Nothing
            Return response.Items.First().Statistics
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

End Class
