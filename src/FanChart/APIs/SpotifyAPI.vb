Imports System.Net
Imports System.Text
Imports Newtonsoft.Json

Public Class SpotifyAPI

    ' Deserialized structures:

    Structure Track
        Property name As String
        Property playcount As String
        Property disc As Integer
        Property number As Integer
        Property uri As String
    End Structure

    Structure Disc
        Property number As Integer
        Property tracks As Track()
    End Structure

    Structure Album
        Property discs As Disc()
    End Structure

    Structure AlbumPlayCountResponse
        Property success As Boolean
        Property data As Album
    End Structure


    Private EndPoint As String

    Sub New()
        EndPoint = My.Settings.SpotifyEndpoint.TrimEnd("/"c)
    End Sub

    Async Function GetAlbumPlayCountAsync(albumId As String) As Task(Of Track())

        Dim response As AlbumPlayCountResponse
        Using client As New WebClient()
            client.Encoding = Encoding.UTF8
            Dim json = Await client.DownloadStringTaskAsync(EndPoint & "/albumPlayCount?albumid=" & albumId)
            response = JsonConvert.DeserializeObject(Of AlbumPlayCountResponse)(json)
        End Using

        ' Post-process, because the info we need is nested in there and in a different format
        Dim tracks As New List(Of Track)
        For Each disc In response.data.discs
            For Each track In disc.tracks
                track.disc += 1
                tracks.Add(track)
            Next
        Next
        Return tracks.ToArray()

    End Function

End Class
