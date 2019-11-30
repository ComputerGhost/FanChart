﻿Imports System.IO
Imports Google.Apis.Services
Imports Google.Apis.YouTube.v3
Imports Newtonsoft.Json

Public Class Engine

    Property Tweeter As New Tweeter

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

        ' Send tweet, if it's not our first time
        If item.LastUpdated.HasValue Then
            Dim api As New Twitter.API(My.Settings.TwitterAppToken, My.Settings.TwitterAppSecret, My.Settings.TwitterUserToken, My.Settings.TwitterUserSecret)
            api.Tweet(item.GetTweetText())
        End If

        ' Now raise the update event, after we've had chance to error out
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

    Async Function RunAsync() As Task

        ' Since our list will be changing, go ahead and copy the keys
        Dim spotifyAlbumIds As New HashSet(Of String)
        Dim youtubeVideoIds As New HashSet(Of String)
        For Each item In Items.Values

            ' Skip if already updated within 24h
            If item.LastUpdated.HasValue Then
                If DateDiff(DateInterval.Hour, item.LastUpdated.Value, Now) < 24 Then
                    Continue For
                End If
            End If

            ' Get the info
            Select Case item.SourceSite
                Case "Spotify"
                    spotifyAlbumIds.Add(item.Identifier.Split(":"c)(0))
                Case "YouTube"
                    youtubeVideoIds.Add(item.Identifier)
            End Select
        Next

        ' Now we actually run it
        Dim spotifyTask = RunSpotifyAsync(spotifyAlbumIds)
        Dim youtubeTask = RunYouTubeAsync(youtubeVideoIds)
        Await Task.WhenAll(spotifyTask, youtubeTask)
    End Function

    Private Async Function RunSpotifyAsync(albumIds As HashSet(Of String)) As Task
        Dim service As New Spotify.API(My.Settings.SpotifyEndpoint)
        For Each albumId In albumIds
            For Each track In service.GetAlbumPlayCount(albumId)
                Dim key = String.Format("Spotify:{0}:{1}:{2}:playcount", albumId, track.disc, track.number)
                Update(key, track.playcount)
            Next
            Await Task.Delay(1000)
        Next
    End Function

    Private Async Function RunYouTubeAsync(videoIds As HashSet(Of String)) As Task
        Dim service As New YouTubeService(New BaseClientService.Initializer With {
            .ApiKey = My.Settings.YoutubeApiKey})
        For Each videoId In videoIds
            Dim request = New VideosResource(service).List("statistics")
            request.Id = videoId
            Dim response = Await request.ExecuteAsync()

            If response.Items.Count = 0 Then Continue For

            Dim stats = response.Items.First().Statistics
            Update(String.Format("YouTube:{0}:Likes", videoId), stats.LikeCount)
            Update(String.Format("YouTube:{0}:Views", videoId), stats.ViewCount)

            Await Task.Delay(1000)
        Next
    End Function

End Class
