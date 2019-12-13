<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Main
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Main))
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SyncToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RunNowToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.SettingsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddSpotifyAlbumToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddYoutubeVideoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.RemoveCurrentToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.lstSources = New System.Windows.Forms.ListView()
        Me.colSource = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colTitle = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colProperty = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colCount = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colDaily = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colUpdated = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colSynced = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.lblStatus = New System.Windows.Forms.ToolStripStatusLabel()
        Me.MenuStrip1.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.SyncToolStripMenuItem, Me.AddToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(915, 24)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "&File"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(92, 22)
        Me.ExitToolStripMenuItem.Text = "E&xit"
        '
        'SyncToolStripMenuItem
        '
        Me.SyncToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RunNowToolStripMenuItem, Me.ToolStripSeparator1, Me.SettingsToolStripMenuItem})
        Me.SyncToolStripMenuItem.Name = "SyncToolStripMenuItem"
        Me.SyncToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.SyncToolStripMenuItem.Text = "&Sync"
        '
        'RunNowToolStripMenuItem
        '
        Me.RunNowToolStripMenuItem.Name = "RunNowToolStripMenuItem"
        Me.RunNowToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.RunNowToolStripMenuItem.Text = "&Run Now"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(177, 6)
        '
        'SettingsToolStripMenuItem
        '
        Me.SettingsToolStripMenuItem.Name = "SettingsToolStripMenuItem"
        Me.SettingsToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.SettingsToolStripMenuItem.Text = "&Settings..."
        '
        'AddToolStripMenuItem
        '
        Me.AddToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddSpotifyAlbumToolStripMenuItem, Me.AddYoutubeVideoToolStripMenuItem, Me.ToolStripSeparator2, Me.RemoveCurrentToolStripMenuItem})
        Me.AddToolStripMenuItem.Name = "AddToolStripMenuItem"
        Me.AddToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.AddToolStripMenuItem.Text = "&List"
        '
        'AddSpotifyAlbumToolStripMenuItem
        '
        Me.AddSpotifyAlbumToolStripMenuItem.Name = "AddSpotifyAlbumToolStripMenuItem"
        Me.AddSpotifyAlbumToolStripMenuItem.Size = New System.Drawing.Size(185, 22)
        Me.AddSpotifyAlbumToolStripMenuItem.Text = "Add &Spotify Album..."
        '
        'AddYoutubeVideoToolStripMenuItem
        '
        Me.AddYoutubeVideoToolStripMenuItem.Name = "AddYoutubeVideoToolStripMenuItem"
        Me.AddYoutubeVideoToolStripMenuItem.Size = New System.Drawing.Size(185, 22)
        Me.AddYoutubeVideoToolStripMenuItem.Text = "Add &Youtube Video..."
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(182, 6)
        '
        'RemoveCurrentToolStripMenuItem
        '
        Me.RemoveCurrentToolStripMenuItem.Name = "RemoveCurrentToolStripMenuItem"
        Me.RemoveCurrentToolStripMenuItem.Size = New System.Drawing.Size(185, 22)
        Me.RemoveCurrentToolStripMenuItem.Text = "&Remove Selected"
        '
        'lstSources
        '
        Me.lstSources.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lstSources.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colSource, Me.colTitle, Me.colProperty, Me.colCount, Me.colDaily, Me.colUpdated, Me.colSynced})
        Me.lstSources.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstSources.Location = New System.Drawing.Point(0, 24)
        Me.lstSources.Name = "lstSources"
        Me.lstSources.Size = New System.Drawing.Size(915, 478)
        Me.lstSources.TabIndex = 1
        Me.lstSources.UseCompatibleStateImageBehavior = False
        Me.lstSources.View = System.Windows.Forms.View.Details
        '
        'colSource
        '
        Me.colSource.Text = "Source"
        Me.colSource.Width = 200
        '
        'colTitle
        '
        Me.colTitle.Text = "Title"
        Me.colTitle.Width = 175
        '
        'colProperty
        '
        Me.colProperty.Text = "Property"
        '
        'colCount
        '
        Me.colCount.Text = "Count"
        Me.colCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.colCount.Width = 100
        '
        'colDaily
        '
        Me.colDaily.Text = "Daily Average"
        Me.colDaily.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.colDaily.Width = 100
        '
        'colUpdated
        '
        Me.colUpdated.Text = "Updated"
        Me.colUpdated.Width = 140
        '
        'colSynced
        '
        Me.colSynced.Text = "Synced"
        Me.colSynced.Width = 140
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblStatus})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 502)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(915, 22)
        Me.StatusStrip1.TabIndex = 2
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'lblStatus
        '
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(39, 17)
        Me.lblStatus.Text = "Ready"
        '
        'Main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(915, 524)
        Me.Controls.Add(Me.lstSources)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "Main"
        Me.Text = "FanChart"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents FileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AddToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents lstSources As ListView
    Friend WithEvents colSource As ColumnHeader
    Friend WithEvents colTitle As ColumnHeader
    Friend WithEvents colCount As ColumnHeader
    Friend WithEvents colDaily As ColumnHeader
    Friend WithEvents colUpdated As ColumnHeader
    Friend WithEvents SyncToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RunNowToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents SettingsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AddSpotifyAlbumToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AddYoutubeVideoToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents RemoveCurrentToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents colProperty As ColumnHeader
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents lblStatus As ToolStripStatusLabel
    Friend WithEvents colSynced As ColumnHeader
End Class
