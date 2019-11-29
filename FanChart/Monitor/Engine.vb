Imports System.IO
Imports Google.Apis.Services
Imports Google.Apis.YouTube.v3
Imports Newtonsoft.Json

Public Class Engine

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

    Sub Update(key As String, latestCount As Integer)
        If Not Items.ContainsKey(key) Then Exit Sub
        Dim item = Items(key)

        ' Skip if already updated within 24h
        If item.LastUpdated.HasValue Then
            If DateDiff(DateInterval.Hour, item.LastUpdated.Value, Now) < 24 Then
                Exit Sub
            End If
        End If

        ' Skip if no significant change in latest count
        Dim oldLatestCount = item.EnglishLatestCount
        item.LatestCount = latestCount
        Dim newLatestCount = item.EnglishLatestCount
        If oldLatestCount = newLatestCount Then Exit Sub

        ' Calculate the daily average
        Dim dailyAverage As Double? = Nothing
        If item.LatestCount.HasValue And item.LastUpdated.HasValue Then
            Dim updatedDiff = DateDiff(DateInterval.Hour, Now, item.LastUpdated.Value) / 24
            Dim countDiff = latestCount - item.LatestCount.Value
            dailyAverage = countDiff / updatedDiff
        End If

        ' Update in our list
        item.DailyAverage = dailyAverage
        item.LastUpdated = Now
        item.LatestCount = latestCount
        Items(key) = item
        RaiseEvent ItemUpdated(item)

        ' Send tweet, if it's not our first time
        If item.LastUpdated.HasValue Then
            Dim twitter As New Twitter.API(My.Settings.TwitterAppToken, My.Settings.TwitterAppSecret, My.Settings.TwitterUserToken, My.Settings.TwitterUserSecret)
            twitter.Tweet(item.GetTweetText())
        End If

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
                Update(key, track.playcount)
            Next
        Next

        Dim youtube As New YouTubeService(New BaseClientService.Initializer With {
            .ApiKey = My.Settings.YoutubeApiKey})
        For Each videoId In youtubeVideos
            Dim stats = TryGetYoutubeVideoStats(youtube, videoId)
            If stats Is Nothing Then Continue For

            Update(String.Format("YouTube:{0}:Likes", videoId), stats.LikeCount)
            Update(String.Format("YouTube:{0}:Views", videoId), stats.ViewCount)
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
