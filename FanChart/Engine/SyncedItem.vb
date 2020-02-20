Public Structure SyncedItem
    Property SyncId As Integer
    Property PropertyId As Integer
    Property TheirId As String
    Property Site As String
    Property Type As String
    Property Title As String
    Property Url As String
    Property PropertyName As String
    Property CurrentCount As Integer
    Property CurrentDaily As Integer?
    Property NewCount As Integer
    Property NewDaily As Integer?
End Structure
