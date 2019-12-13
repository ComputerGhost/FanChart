Imports System.Environment

Public Class Main

    Private WithEvents monitor As New Engine

    Private Sub monitor_ItemAdded(monitoredItem As MonitoredItem) Handles monitor.ItemAdded
        lstSources.Items.Add(MonitoredItemToListViewItem(monitoredItem))
        monitor.Save(GetFolderPath(SpecialFolder.ApplicationData) & "/sources.js")
    End Sub

    Private Sub monitor_ItemsLoaded(monitoredItems As MonitoredItem()) Handles monitor.ItemsLoaded
        lstSources.Items.AddRange((
            From item In monitoredItems
            Select MonitoredItemToListViewItem(item)
            ).ToArray())
    End Sub

    Private Sub monitor_ItemRemoved(monitoredItem As MonitoredItem) Handles monitor.ItemRemoved
        For Each listItem As ListViewItem In lstSources.Items
            If CType(listItem.Tag, MonitoredItem).UniqueKey = monitoredItem.UniqueKey Then
                lstSources.Items.Remove(listItem)
                Exit For
            End If
        Next
    End Sub

    Private Sub monitor_ItemUpdated(monitoredItem As MonitoredItem) Handles monitor.ItemUpdated
        For Each listItem As ListViewItem In lstSources.Items
            If CType(listItem.Tag, MonitoredItem).UniqueKey = monitoredItem.UniqueKey Then
                listItem.SubItems(colCount.Index).Text = If(monitoredItem.LatestCount?.ToString("N0"), "")
                listItem.SubItems(colDaily.Index).Text = If(monitoredItem.DailyAverage?.ToString("N0"), "")
                listItem.SubItems(colUpdated.Index).Text = If(monitoredItem.LastUpdated, "")
                listItem.SubItems(colSynced.Index).Text = If(monitoredItem.LastSynced, "")
                listItem.Tag = monitoredItem
                Exit For
            End If
        Next
        monitor.Save(GetFolderPath(SpecialFolder.ApplicationData) & "/sources.js")
    End Sub

    Private Function MonitoredItemToListViewItem(monitoredItem As MonitoredItem) As ListViewItem
        Dim listItem As New ListViewItem({
            monitoredItem.SourceSite & ":" & monitoredItem.Identifier,
            monitoredItem.Title,
            monitoredItem.WatchedProperty,
            If(monitoredItem.LatestCount?.ToString("N0"), ""),
            If(monitoredItem.DailyAverage?.ToString("N0"), ""),
            If(monitoredItem.LastUpdated, ""),
            If(monitoredItem.LastSynced, "")})
        listItem.Tag = monitoredItem
        Return listItem
    End Function


    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If String.IsNullOrEmpty(My.Settings.TwitterUserToken) Then
            EditSettings.ShowDialog()
        End If
        monitor.Load(GetFolderPath(SpecialFolder.ApplicationData) & "/sources.js")
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Close()
    End Sub

    Private Async Sub RunNowToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RunNowToolStripMenuItem.Click
        Try
            RunNowToolStripMenuItem.Enabled = False
            lblStatus.Text = "Running..."

            Await monitor.RunAsync()

            RunNowToolStripMenuItem.Enabled = True
            lblStatus.Text = "Completed"

        Catch ex As Exception
            RunNowToolStripMenuItem.Enabled = True
            lblStatus.Text = "Error: " & ex.Message
            MessageBox.Show(ex.ToString())
        End Try
    End Sub

    Private Sub SettingsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SettingsToolStripMenuItem.Click
        EditSettings.ShowDialog()
    End Sub

    Private Sub AddSpotifyAlbumToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddSpotifyAlbumToolStripMenuItem.Click
        AddSpotifyAlbum.ShowDialog(monitor)
    End Sub

    Private Sub AddYoutubeVideoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddYoutubeVideoToolStripMenuItem.Click
        AddYouTubeVideo.ShowDialog(monitor)
    End Sub

    Private Sub RemoveCurrentToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RemoveCurrentToolStripMenuItem.Click
        Dim removingItems As New List(Of MonitoredItem)
        For Each listItem As ListViewItem In lstSources.SelectedItems
            removingItems.Add(listItem.Tag)
        Next
        For Each item In removingItems
            monitor.Remove(item)
        Next
        monitor.Save(GetFolderPath(SpecialFolder.ApplicationData) & "/sources.js")
    End Sub
End Class