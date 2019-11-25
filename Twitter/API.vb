Imports System.Net.Http
Imports System.Security.Cryptography
Imports System.Text

Public Class API
    ' I've translated this from C# to VB.  The original source is at:
    ' https://blog.dantup.com/2016/07/simplest-csharp-code-to-post-a-tweet-using-oauth/

    Private consumerKey As String
    Private consumerKeySecret As String
    Private accessToken As String
    Private accessTokenSecret As String
    Private sigHasher As HMACSHA1

    Sub New(consumerKey As String, consumerKeySecret As String, accessToken As String, accessTokenSecret As String)
        Me.consumerKey = consumerKey
        Me.consumerKeySecret = consumerKeySecret
        Me.accessToken = accessToken
        Me.accessTokenSecret = accessTokenSecret
        sigHasher = New HMACSHA1(New ASCIIEncoding().GetBytes(String.Format("{0}&{1}", consumerKeySecret, accessTokenSecret)))
    End Sub

    Public Function Tweet(text As String) As Task(Of String)
        Dim data As New Dictionary(Of String, String) From {
            {"status", text},
            {"trim_user", 1}}
        Return SendRequest("statuses/update.json", data)
    End Function


    Private Function SendRequest(url As String, data As Dictionary(Of String, String)) As Task(Of String)
        Dim fullUrl = "https://api.twitter.com/1.1/" & url
        Dim timestamp = CInt((DateTime.UtcNow - New Date(1970, 1, 1)).TotalSeconds)

        data.Add("oauth_consumer_key", consumerKey)
        data.Add("oauth_signature_method", "HMAC-SHA1")
        data.Add("oauth_timestamp", timestamp.ToString())
        data.Add("oauth_nonce", "ignored")
        data.Add("oauth_token", accessToken)
        data.Add("oauth_version", "1.0")
        data.Add("oauth_signature", GenerateSignature(fullUrl, data))

        Dim oAuthHeader = GenerateOAuthHeader(data)

        Dim formData = New FormUrlEncodedContent(data.Where(Function(kvp) Not kvp.Key.StartsWith("oauth_")))

        Return SendRequest(fullUrl, oAuthHeader, formData)
    End Function

    Private Function GenerateSignature(url As String, data As Dictionary(Of String, String)) As String
        Dim sigString = String.Join("&", data _
            .Union(data) _
            .Select(Function(kvp) String.Format("{0}={1}", Uri.EscapeDataString(kvp.Key), Uri.EscapeDataString(kvp.Value))) _
            .OrderBy(Function(s) s))
        Dim fullSigData = String.Format("{0}&{1}&{2}", "POST", Uri.EscapeDataString(url), Uri.EscapeDataString(sigString.ToString()))
        Return Convert.ToBase64String(sigHasher.ComputeHash(New ASCIIEncoding().GetBytes(fullSigData.ToString())))
    End Function

    Private Function GenerateOAuthHeader(data As Dictionary(Of String, String))
        Return "Oauth " & String.Join(
            ", ",
            data _
                .Where(Function(kvp) kvp.Key.StartsWith("oauth_")) _
                .Select(Function(kvp) String.Format("{0}=""{1}""", Uri.EscapeDataString(kvp.Key), Uri.EscapeDataString(kvp.Value))) _
                .OrderBy(Function(s) s))
    End Function

    Private Async Function SendRequest(fullUrl As String, oAuthHeader As String, formData As FormUrlEncodedContent) As Task(Of String)
        Using http As New HttpClient()
            http.DefaultRequestHeaders.Add("Authorization", oAuthHeader)
            Dim httpResp = Await http.PostAsync(fullUrl, formData)
            Dim respBody = Await httpResp.Content.ReadAsStringAsync()
            Return respBody
        End Using
    End Function


End Class
