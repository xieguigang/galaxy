Imports System.Data

Public Interface IDataTableViewer

    Sub LoadTable(apply As Action(Of DataTable))

End Interface