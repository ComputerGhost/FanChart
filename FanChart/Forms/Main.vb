Imports System.Environment

Public Class Main

    Private WithEvents monitor As New Monitor

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
                listItem.SubItems(colCount.Index).Text = monitoredItem.LatestCount
                listItem.SubItems(colDaily.Index).Text = monitoredItem.DailyAverage
                listItem.SubItems(colUpdated.Index).Text = monitoredItem.LastUpdated
                listItem.Tag = monitoredItem
                Exit For
            End If
        Next
    End Sub

    Private Function MonitoredItemToListViewItem(monitoredItem As MonitoredItem) As ListViewItem
        Dim listItem As New ListViewItem({
            monitoredItem.SourceSite & ":" & monitoredItem.Identifier,
            monitoredItem.Title,
            monitoredItem.WatchedProperty,
            If(monitoredItem.LatestCount, ""),
            If(monitoredItem.DailyAverage, ""),
            If(monitoredItem.LastUpdated, "")})
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

    Private Sub RunNowToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RunNowToolStripMenuItem.Click
        monitor.Run()
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