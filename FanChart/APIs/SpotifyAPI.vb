Imports System.Net
Imports Newtonsoft.Json

Public Class SpotifyAPI

    Structure Track
        Property name As String
        Property playcount As String
        Property disc As Integer
        Property number As Integer
        Property uri As String
    End Structure

    Structure AlbumPlayCountResponse
        Property success As Boolean
        Property data As Track()
    End Structure

    Private EndPoint As String

    Sub New()
        EndPoint = My.Settings.SpotifyEndpoint
    End Sub

    Async Function GetAlbumPlayCountAsync(albumId As String) As Task(Of Track())
        Using client As New WebClient()
            Dim json = Await client.DownloadStringTaskAsync(EndPoint & "?albumid=" & albumId)
            Dim deserialized = JsonConvert.DeserializeObject(Of AlbumPlayCountResponse)(json)
            Return deserialized.data
        End Using
    End Function

End Class
