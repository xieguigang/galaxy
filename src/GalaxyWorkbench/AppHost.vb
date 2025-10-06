Public Interface AppHost

    Property WindowState As FormWindowState
    ''' <summary>
    ''' get/set current actived mdi document
    ''' </summary>
    ''' <returns></returns>
    ReadOnly Property ActiveDocument As Form

    Event ResizeForm(location As Point, size As Size)

    Function GetDesktopLocation() As Point
    Function GetClientSize() As Size
    Function GetDocuments() As IEnumerable(Of Form)
    Function GetDockPanel() As Control

    Sub StatusMessage(msg As String, Optional icon As Image = Nothing)

End Interface
