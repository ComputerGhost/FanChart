Imports System.Globalization
Imports System.Text.RegularExpressions

Public Class MilestoneNotice

    Property Item As SyncedItem
    Property Template As String

    Function FormatForTweet() As String
        Dim regex As New Regex("\{(.*?)\}")
        Dim evaluator As New MatchEvaluator(AddressOf ParameterMatchEvaluator)
        Return regex.Replace(Template, evaluator)
    End Function

    Private Function ParameterMatchEvaluator(match As Match) As String
        Select Case match.Groups(1).Value
            Case "title" : Return Item.Title
            Case "count" : Return FormatNumber(Item.NewCount)
            Case "daily" : Return FormatNumber(Item.NewDaily)
            Case "link" : Return Item.Url
            Case "tags" : Return My.Settings.TwitterTags
        End Select
        Return ""
    End Function

    Private Function FormatNumber(number As Integer) As String
        Dim nfi As NumberFormatInfo = CultureInfo.InvariantCulture.NumberFormat.Clone()
        nfi.NumberGroupSeparator = " "
        Return number.ToString("N0", nfi)
    End Function

End Class
