Imports System.Net.Http
Imports System.Security.Cryptography
Imports System.Text
Imports Newtonsoft.Json

Public Class TwitterAPI

    ' https://developer.twitter.com/en/docs/tweets/data-dictionary/overview/user-object
    Structure UserObject
        Property id_str As String
        Property name As String
        Property screen_name As String
        Property followers_count As Integer
    End Structure

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

    ' https://developer.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-users-show
    Async Function GetUserAsync(username As String) As Task(Of UserObject)
        Dim parameters As New Dictionary(Of String, String) From {
            {"screen_name", username}}
        Dim response = Await SendGetRequest("users/show.json", parameters)
        Return JsonConvert.DeserializeObject(Of UserObject)(response)
    End Function

    Async Function Tweet(text) As Task
        Dim parameters As New Dictionary(Of String, String) From {
            {"status", text},
            {"trim_user", 1}}
        Await SendPostRequest("statuses/update.json", parameters)
    End Function

    Private Async Function SendGetRequest(endpoint As String, parameters As Dictionary(Of String, String)) As Task(Of String)
        Dim url = BaseUrl & endpoint

        Dim paramString = Await New FormUrlEncodedContent(parameters).ReadAsStringAsync()
        Dim fullUrl = url & "?" & paramString

        Dim request As New HttpRequestMessage(HttpMethod.Get, fullUrl)
        request.Headers.Add("Authorization", GenerateOauthHeader(HttpMethod.Get, url, parameters))

        Using http As New HttpClient
            Dim httpResp = Await http.SendAsync(request)
            Dim respBody = Await httpResp.Content.ReadAsStringAsync()
            If httpResp.StatusCode <> 200 Then
                Throw New Exception(respBody)
            End If
            Return respBody
        End Using

    End Function

    Private Async Function SendPostRequest(endpoint As String, parameters As Dictionary(Of String, String)) As Task(Of String)
        Dim url = BaseUrl & endpoint

        Dim request As New HttpRequestMessage(HttpMethod.Post, url)
        request.Content = New FormUrlEncodedContent(parameters)
        request.Headers.Add("Authorization", GenerateOauthHeader(HttpMethod.Post, url, parameters))

        Using http As New HttpClient
            Dim httpResp = Await http.SendAsync(request)
            Dim respBody = Await httpResp.Content.ReadAsStringAsync()
            If httpResp.StatusCode <> 200 Then
                Throw New Exception(respBody)
            End If
            Return respBody
        End Using

    End Function

    ' Libraries for OAuth and libraries for form encoding differ, so we hard-code it and can make everything consistent.
    ' The alternative is using libraries that actually work together...  :/
    Private Function GenerateOauthHeader(method As HttpMethod, url As String, parameters As Dictionary(Of String, String)) As String

        Dim oauthParams As New Dictionary(Of String, String) From {
            {"oauth_consumer_key", ConsumerKey},
            {"oauth_signature_method", "HMAC-SHA1"},
            {"oauth_timestamp", (DateTime.UtcNow - #1/1/1970#).TotalSeconds},
            {"oauth_nonce", (DateTime.UtcNow - #1/1/1970#).TotalSeconds},
            {"oauth_token", AccessToken},
            {"oauth_version", "1.0"}}

        Dim sigString = String.Join("&", oauthParams.Union(parameters) _
            .Select(Function(kvp) String.Format("{0}={1}", Uri.EscapeDataString(kvp.Key), Uri.EscapeDataString(kvp.Value))) _
            .OrderBy(Function(s) s))
        Dim fullSigData = String.Format("{0}&{1}&{2}", method.ToString(), Uri.EscapeDataString(url), Uri.EscapeDataString(sigString.ToString()))
        Dim sigHasher = New HMACSHA1(New ASCIIEncoding().GetBytes(String.Format("{0}&{1}", ConsumerKeySecret, AccessTokenSecret)))
        Dim signature = Convert.ToBase64String(sigHasher.ComputeHash(New ASCIIEncoding().GetBytes(fullSigData.ToString())))

        oauthParams.Add("oauth_signature", signature)

        Return "Oauth " & String.Join(", ", oauthParams _
            .Select(Function(kvp) String.Format("{0}=""{1}""", Uri.EscapeDataString(kvp.Key), Uri.EscapeDataString(kvp.Value))) _
            .OrderBy(Function(s) s))

    End Function

End Class
