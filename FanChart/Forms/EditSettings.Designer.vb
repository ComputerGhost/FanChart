<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class EditSettings
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(EditSettings))
        Me.txtSpotifyStreamsNotice = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtYoutubeLikesNotice = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtYoutubeViewsNotice = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.PictureBox3 = New System.Windows.Forms.PictureBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtTwitterAppSecret = New System.Windows.Forms.TextBox()
        Me.txtYoutubeApiKey = New System.Windows.Forms.TextBox()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.txtTwitterAppToken = New System.Windows.Forms.TextBox()
        Me.txtSpotifyEndpoint = New System.Windows.Forms.TextBox()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.txtTwitterUserToken = New System.Windows.Forms.TextBox()
        Me.txtTwitterUserSecret = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtSpotifyStreamsNotice
        '
        Me.txtSpotifyStreamsNotice.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSpotifyStreamsNotice.Location = New System.Drawing.Point(146, 45)
        Me.txtSpotifyStreamsNotice.MaxLength = 140
        Me.txtSpotifyStreamsNotice.Name = "txtSpotifyStreamsNotice"
        Me.txtSpotifyStreamsNotice.Size = New System.Drawing.Size(291, 20)
        Me.txtSpotifyStreamsNotice.TabIndex = 2
        Me.txtSpotifyStreamsNotice.Text = "{title} has reached {count} streams on Spotify!  (The current daily average is {d" &
    "aily}.)  {link}"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(58, 48)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(82, 13)
        Me.Label10.TabIndex = 11
        Me.Label10.Text = "Streams Notice:"
        '
        'txtYoutubeLikesNotice
        '
        Me.txtYoutubeLikesNotice.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtYoutubeLikesNotice.Location = New System.Drawing.Point(84, 97)
        Me.txtYoutubeLikesNotice.MaxLength = 140
        Me.txtYoutubeLikesNotice.Name = "txtYoutubeLikesNotice"
        Me.txtYoutubeLikesNotice.Size = New System.Drawing.Size(353, 20)
        Me.txtYoutubeLikesNotice.TabIndex = 8
        Me.txtYoutubeLikesNotice.Text = "{title} has reached {count} likes on YouTube!  (The current daily average is {dai" &
    "ly}.)  {link}"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(6, 100)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(69, 13)
        Me.Label9.TabIndex = 8
        Me.Label9.Text = "Likes Notice:"
        '
        'txtYoutubeViewsNotice
        '
        Me.txtYoutubeViewsNotice.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtYoutubeViewsNotice.Location = New System.Drawing.Point(84, 71)
        Me.txtYoutubeViewsNotice.MaxLength = 140
        Me.txtYoutubeViewsNotice.Name = "txtYoutubeViewsNotice"
        Me.txtYoutubeViewsNotice.Size = New System.Drawing.Size(353, 20)
        Me.txtYoutubeViewsNotice.TabIndex = 7
        Me.txtYoutubeViewsNotice.Text = "{title} has reached {count} views on YouTube!  (The current daily average is {dai" &
    "ly}.)  {link}"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(6, 74)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(72, 13)
        Me.Label8.TabIndex = 6
        Me.Label8.Text = "Views Notice:"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(58, 22)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(48, 13)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "API Key:"
        '
        'PictureBox3
        '
        Me.PictureBox3.BackgroundImage = Global.FanChart.My.Resources.Resources.youtube
        Me.PictureBox3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.PictureBox3.Location = New System.Drawing.Point(6, 19)
        Me.PictureBox3.Name = "PictureBox3"
        Me.PictureBox3.Size = New System.Drawing.Size(46, 46)
        Me.PictureBox3.TabIndex = 4
        Me.PictureBox3.TabStop = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(58, 48)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(63, 13)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "App Secret:"
        '
        'txtTwitterAppSecret
        '
        Me.txtTwitterAppSecret.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTwitterAppSecret.Location = New System.Drawing.Point(130, 45)
        Me.txtTwitterAppSecret.Name = "txtTwitterAppSecret"
        Me.txtTwitterAppSecret.Size = New System.Drawing.Size(307, 20)
        Me.txtTwitterAppSecret.TabIndex = 4
        '
        'txtYoutubeApiKey
        '
        Me.txtYoutubeApiKey.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtYoutubeApiKey.Location = New System.Drawing.Point(156, 19)
        Me.txtYoutubeApiKey.Name = "txtYoutubeApiKey"
        Me.txtYoutubeApiKey.Size = New System.Drawing.Size(281, 20)
        Me.txtYoutubeApiKey.TabIndex = 5
        '
        'PictureBox2
        '
        Me.PictureBox2.BackgroundImage = Global.FanChart.My.Resources.Resources.twitter
        Me.PictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.PictureBox2.Location = New System.Drawing.Point(6, 19)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(46, 46)
        Me.PictureBox2.TabIndex = 2
        Me.PictureBox2.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(58, 22)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(52, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Endpoint:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(58, 22)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(63, 13)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "App Token:"
        '
        'PictureBox1
        '
        Me.PictureBox1.BackgroundImage = Global.FanChart.My.Resources.Resources.spotify
        Me.PictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.PictureBox1.Location = New System.Drawing.Point(6, 19)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(46, 46)
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'txtTwitterAppToken
        '
        Me.txtTwitterAppToken.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTwitterAppToken.Location = New System.Drawing.Point(130, 19)
        Me.txtTwitterAppToken.Name = "txtTwitterAppToken"
        Me.txtTwitterAppToken.Size = New System.Drawing.Size(307, 20)
        Me.txtTwitterAppToken.TabIndex = 3
        '
        'txtSpotifyEndpoint
        '
        Me.txtSpotifyEndpoint.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSpotifyEndpoint.Location = New System.Drawing.Point(151, 19)
        Me.txtSpotifyEndpoint.Name = "txtSpotifyEndpoint"
        Me.txtSpotifyEndpoint.Size = New System.Drawing.Size(286, 20)
        Me.txtSpotifyEndpoint.TabIndex = 1
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSave.Location = New System.Drawing.Point(12, 353)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(75, 23)
        Me.btnSave.TabIndex = 10
        Me.btnSave.Text = "Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(380, 353)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 11
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.txtSpotifyStreamsNotice)
        Me.GroupBox1.Controls.Add(Me.Label10)
        Me.GroupBox1.Controls.Add(Me.PictureBox1)
        Me.GroupBox1.Controls.Add(Me.txtSpotifyEndpoint)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(443, 71)
        Me.GroupBox1.TabIndex = 12
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Spotify"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.Label5)
        Me.GroupBox3.Controls.Add(Me.Label2)
        Me.GroupBox3.Controls.Add(Me.txtTwitterUserSecret)
        Me.GroupBox3.Controls.Add(Me.txtTwitterUserToken)
        Me.GroupBox3.Controls.Add(Me.PictureBox2)
        Me.GroupBox3.Controls.Add(Me.txtTwitterAppToken)
        Me.GroupBox3.Controls.Add(Me.Label4)
        Me.GroupBox3.Controls.Add(Me.txtTwitterAppSecret)
        Me.GroupBox3.Controls.Add(Me.Label3)
        Me.GroupBox3.Location = New System.Drawing.Point(12, 89)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(443, 123)
        Me.GroupBox3.TabIndex = 13
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Twitter"
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.txtYoutubeLikesNotice)
        Me.GroupBox4.Controls.Add(Me.txtYoutubeApiKey)
        Me.GroupBox4.Controls.Add(Me.Label9)
        Me.GroupBox4.Controls.Add(Me.PictureBox3)
        Me.GroupBox4.Controls.Add(Me.txtYoutubeViewsNotice)
        Me.GroupBox4.Controls.Add(Me.Label6)
        Me.GroupBox4.Controls.Add(Me.Label8)
        Me.GroupBox4.Location = New System.Drawing.Point(12, 218)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(443, 123)
        Me.GroupBox4.TabIndex = 14
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "YouTube"
        '
        'txtTwitterUserToken
        '
        Me.txtTwitterUserToken.Location = New System.Drawing.Point(130, 71)
        Me.txtTwitterUserToken.Name = "txtTwitterUserToken"
        Me.txtTwitterUserToken.Size = New System.Drawing.Size(307, 20)
        Me.txtTwitterUserToken.TabIndex = 5
        '
        'txtTwitterUserSecret
        '
        Me.txtTwitterUserSecret.Location = New System.Drawing.Point(130, 97)
        Me.txtTwitterUserSecret.Name = "txtTwitterUserSecret"
        Me.txtTwitterUserSecret.Size = New System.Drawing.Size(307, 20)
        Me.txtTwitterUserSecret.TabIndex = 6
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(58, 74)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(66, 13)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "User Token:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(58, 100)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(66, 13)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "User Secret:"
        '
        'EditSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(467, 388)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnSave)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "EditSettings"
        Me.Text = "FanChart Settings"
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnSave As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents txtSpotifyEndpoint As TextBox
    Friend WithEvents txtTwitterAppSecret As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents txtTwitterAppToken As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents txtYoutubeApiKey As TextBox
    Friend WithEvents PictureBox2 As PictureBox
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents PictureBox3 As PictureBox
    Friend WithEvents txtYoutubeLikesNotice As TextBox
    Friend WithEvents Label9 As Label
    Friend WithEvents txtYoutubeViewsNotice As TextBox
    Friend WithEvents Label8 As Label
    Friend WithEvents txtSpotifyStreamsNotice As TextBox
    Friend WithEvents Label10 As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents Label5 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents txtTwitterUserSecret As TextBox
    Friend WithEvents txtTwitterUserToken As TextBox
End Class
