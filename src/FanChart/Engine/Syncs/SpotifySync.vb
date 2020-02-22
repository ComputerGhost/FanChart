Imports System.Net
Imports Newtonsoft.Json

Public Class SpotifySync
    Inherits SyncBase

    Private Service As New SpotifyAPI()

    Async Function FetchAlbumStatsAsync() As Task

        ' Contains {TheirId, Count}
        Dim songCache As New Dictionary(Of String, Integer)

        For Each id In GetMonitoredIdsFor("Spotify", "Song")

            ' The entire album must be pulled, but we can store our results for later.
            ' If we've hit the song's album already, we can just skip this part.
            If Not songCache.ContainsKey(id.TheirId) Then
                Dim albumId = id.TheirId.Split(":"c).First()
                For Each track In Await Service.GetAlbumPlayCountAsync(albumId)
                    Dim theirId = String.Format("{0}:{1}:{2}", albumId, track.disc, track.number)
                    songCache.Add(theirId, track.playcount)
                Next
            End If

            ' At this point, the song should be in the cache
            Dim playcount = songCache(id.TheirId)
            SaveUpdate(id.OurId, "Plays", playcount)

            Await Task.Delay(1000)
        Next
    End Function

End Class
