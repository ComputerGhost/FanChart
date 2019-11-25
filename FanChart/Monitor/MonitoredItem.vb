Imports Newtonsoft.Json

Public Structure MonitoredItem

    Property SourceSite As String
    Property Identifier As String
    Property WatchedProperty As String
    Property Title As String
    Property LatestCount As Integer?
    Property DailyAverage As Double?
    Property LastUpdated As Date?

    <JsonIgnore>
    ReadOnly Property UniqueKey As String
        Get
            Return String.Format("{0}:{1}:{2}", SourceSite, Identifier, WatchedProperty)
        End Get
    End Property

End Structure
