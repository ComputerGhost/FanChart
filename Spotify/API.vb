Imports System.Net
Imports Newtonsoft.Json

Public Class API

    Private endpoint As String

    Sub New(endpoint As String)
        Me.endpoint = endpoint
    End Sub

    Function GetAlbumPlayCount(albumId As String) As Track()
        Using client As New WebClient()
            Dim json = client.DownloadString(endpoint & "?albumid=" & albumId)
            Dim deserialized = JsonConvert.DeserializeObject(Of AlbumPlayCountResponse)(json)
            Return deserialized.data
        End Using
    End Function

End Class
