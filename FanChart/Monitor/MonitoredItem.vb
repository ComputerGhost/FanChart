Imports Newtonsoft.Json

Public Structure MonitoredItem

    Property SourceSite As String
    Property Identifier As String
    Property WatchedProperty As String
    Property Title As String
    Property LatestCount As Integer?
    Property DailyAverage As Double?
    Property LastUpdated As Date?

    <JsonIgnore>
    ReadOnly Property UniqueKey As String
        Get
            Return String.Format("{0}:{1}:{2}", SourceSite, Identifier, WatchedProperty)
        End Get
    End Property

    <JsonIgnore>
    ReadOnly Property EnglishLatestCount As String
        Get
            If Not LatestCount.HasValue Then Return ""
            Return GetEnglishNumber(LatestCount.Value, 3)
        End Get
    End Property

    <JsonIgnore>
    ReadOnly Property URL As String
        Get
            Select Case SourceSite
                Case "Spotify"
                    Return "https://open.spotify.com/album/" & Identifier.Split(":")(0)
                Case "YouTube"
                    Return "https://youtu.be/" & Identifier
            End Select
            Return ""
        End Get
    End Property


    Public Function GetTweetTemplate() As String
        Select Case SourceSite & ":" & WatchedProperty
            Case "Spotify:playcount"
                Return My.Settings.SpotifyStreamsNotice
            Case "YouTube:Likes"
                Return My.Settings.YoutubeLikesNotice
            Case "YouTube:Views"
                Return My.Settings.YoutubeViewsNotice
        End Select
        Return Nothing
    End Function

    Public Function GetTweetText() As String
        Return GetTweetTemplate() _
            .Replace("{title}", Title) _
            .Replace("{count}", EnglishLatestCount) _
            .Replace("{daily}", If(DailyAverage.HasValue, String.Format("{0:n0}", DailyAverage.Value), "")) _
            .Replace("{link}", URL)
    End Function

    Public Function IsSignificantChange(newCount As Integer) As Boolean
        If Not LatestCount.HasValue Then Return False
        Return GetEnglishNumber(LatestCount.Value, 2) <> GetEnglishNumber(newCount, 2)
    End Function


    Private Function GetEnglishNumber(number As Integer, significantDigits As Integer) As String
        Dim digitCount As Integer = Math.Floor(Math.Log10(number) + 1)
        Dim scale = Math.Pow(10, digitCount - significantDigits)
        Dim truncated As Integer = scale * Math.Floor(number / scale)

        If digitCount > 9 Then
            Return String.Format("{0} billion", truncated / 1000000000)
        ElseIf digitCount > 6 Then
            Return String.Format("{0} million", truncated / 1000000)
        ElseIf digitCount > 3 Then
            Return String.Format("{0} thousand", truncated / 1000)
        Else
            Return truncated
        End If
    End Function

End Structure
