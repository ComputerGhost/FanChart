Public Class EditSettings

    Private Sub EditSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        With My.Settings
            txtSpotifyEndpoint.Text = .SpotifyEndpoint
            txtSpotifyStreamsNotice.Text = .SpotifyStreamsNotice
            txtTwitterAppToken.Text = .TwitterAppToken
            txtTwitterAppSecret.Text = .TwitterAppSecret
            txtTwitterUserToken.Text = .TwitterUserToken
            txtTwitterUserSecret.Text = .TwitterUserSecret
            txtYoutubeApiKey.Text = .YoutubeApiKey
            txtYoutubeLikesNotice.Text = .YoutubeLikesNotice
            txtYoutubeViewsNotice.Text = .YoutubeViewsNotice
            txtYouTubeSubsNotice.Text = .YoutubeSubsNotice
        End With
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        With My.Settings
            .SpotifyEndpoint = txtSpotifyEndpoint.Text
            .SpotifyStreamsNotice = txtSpotifyStreamsNotice.Text
            .TwitterAppToken = txtTwitterAppToken.Text
            .TwitterAppSecret = txtTwitterAppSecret.Text
            .TwitterUserToken = txtTwitterUserToken.Text
            .TwitterUserSecret = txtTwitterUserSecret.Text
            .YoutubeApiKey = txtYoutubeApiKey.Text
            .YoutubeLikesNotice = txtYoutubeLikesNotice.Text
            .YoutubeViewsNotice = txtYoutubeViewsNotice.Text
            .YoutubeSubsNotice = txtYouTubeSubsNotice.Text
        End With
        My.Settings.Save()
        Close()
    End Sub
End Class