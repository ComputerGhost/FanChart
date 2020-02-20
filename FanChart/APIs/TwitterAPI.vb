Imports System.Net.Http
Imports OAuth

Public Class TwitterAPI

    Private Const BaseUrl = "https://api.twitter.com/1.1/"
    Private ConsumerKey As String
    Private ConsumerKeySecret As String
    Private AccessToken As String
    Private AccessTokenSecret As String

    Sub New()
        ConsumerKey = My.Settings.TwitterAppToken
        ConsumerKeySecret = My.Settings.TwitterAppSecret
        AccessToken = My.Settings.TwitterUserToken
        AccessTokenSecret = My.Settings.TwitterUserSecret
    End Sub

    Async Function Tweet(text) As Task
        Dim parameters As New Dictionary(Of String, String) From {
            {"status", text},
            {"trim_user", 1}}
        Await SendRequest(HttpMethod.Post, "statuses/update.json", parameters)
    End Function


    Private Async Function SendRequest(method As HttpMethod, endpoint As String, parameters As Dictionary(Of String, String)) As Task(Of String)

        Dim oauth = OAuthRequest.ForAccessToken(ConsumerKey, ConsumerKeySecret, AccessToken, AccessTokenSecret)
        oauth.RequestUrl = BaseUrl & endpoint
        oauth.Method = method.Method

        Dim message As New HttpRequestMessage(method, BaseUrl & endpoint)
        message.Headers.Add("Authorization", oauth.GetAuthorizationHeader(parameters))
        message.Content = New FormUrlEncodedContent(parameters)

        Using http As New HttpClient
            Dim httpResp = Await http.SendAsync(message)
            Dim respBody = Await httpResp.Content.ReadAsStringAsync()
            Return respBody
        End Using

    End Function

End Class
