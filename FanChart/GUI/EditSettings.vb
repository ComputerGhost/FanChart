Public Class EditSettings

    Private Sub EditSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        With My.Settings
            txtConnectionString.Text = .ConnectionString
            txtSpotifyEndpoint.Text = .SpotifyEndpoint
            txtTwitterAppToken.Text = .TwitterAppToken
            txtTwitterAppSecret.Text = .TwitterAppSecret
            txtTwitterUserToken.Text = .TwitterUserToken
            txtTwitterUserSecret.Text = .TwitterUserSecret
            txtYoutubeApiKey.Text = .YoutubeApiKey
        End With
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        With My.Settings
            .ConnectionString = txtConnectionString.Text
            .SpotifyEndpoint = txtSpotifyEndpoint.Text
            .TwitterAppToken = txtTwitterAppToken.Text
            .TwitterAppSecret = txtTwitterAppSecret.Text
            .TwitterUserToken = txtTwitterUserToken.Text
            .TwitterUserSecret = txtTwitterUserSecret.Text
            .YoutubeApiKey = txtYoutubeApiKey.Text
        End With
        My.Settings.Save()
        Close()
    End Sub
End Class