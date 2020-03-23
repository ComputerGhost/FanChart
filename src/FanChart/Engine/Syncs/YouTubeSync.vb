Imports System.Data.SqlClient
Imports Google.Apis.Services
Imports Google.Apis.YouTube.v3

Public Class YouTubeSync
    Inherits SyncBase

    Property Service As YouTubeService

    Sub New()
        Service = New YouTubeService(New BaseClientService.Initializer With {
            .ApiKey = My.Settings.YoutubeApiKey})
    End Sub

    Function FetchAllAsync() As Task
        Return Task.WhenAll(
            FetchChannelStatsAsync(),
            FetchVideoStatsAsync())
    End Function

    Async Function FetchChannelStatsAsync() As Task
        For Each id In GetMonitoredIdsFor("YouTube", "Channel")
            Dim request = New ChannelsResource(Service).List("statistics")
            request.Id = id.TheirId
            Dim response = Await request.ExecuteAsync()

            If response.Items.Count = 0 Then Continue For

            Dim stats = response.Items.First().Statistics
            SaveUpdate(id.OurId, "Subscribers", stats.SubscriberCount)

            Await Task.Delay(1000)
        Next
    End Function

    Async Function FetchVideoStatsAsync() As Task
        For Each id In GetMonitoredIdsFor("YouTube", "Video")
            Dim request = New VideosResource(Service).List("statistics")
            request.Id = id.TheirId
            Dim response = Await request.ExecuteAsync

            If response.Items.Count = 0 Then Continue For

            Dim stats = response.Items.First().Statistics
            SaveUpdate(id.OurId, "Likes", If(stats.LikeCount, 0))
            SaveUpdate(id.OurId, "Views", If(stats.ViewCount, 0))

            Await Task.Delay(1000)
        Next
    End Function

End Class
