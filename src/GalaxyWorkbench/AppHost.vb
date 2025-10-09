
''' <summary>
''' the main framework of the workbench desktop application and the plugin system base
''' </summary>
Public Interface AppHost

    ReadOnly Property ClientRectangle As Rectangle

    ''' <summary>
    ''' get/set current actived mdi document
    ''' </summary>
    ''' <returns></returns>
    ReadOnly Property ActiveDocument As Form

    Event ResizeForm(location As Point, size As Size)
    Event CloseWorkbench(args As FormClosingEventArgs)

    Sub SetWorkbenchVisible(visible As Boolean)
    Sub SetWindowState(stat As FormWindowState)

    Function GetDesktopLocation() As Point
    Function GetClientSize() As Size
    Function GetDocuments() As IEnumerable(Of Form)
    Function GetDockPanel() As Control
    Function GetWindowState() As FormWindowState

    Sub SetTitle(title As String)
    Sub StatusMessage(msg As String, Optional icon As Image = Nothing)
    Sub Warning(msg As String)
    Sub LogText(text As String)
    Sub ShowProperties(obj As Object)

End Interface
