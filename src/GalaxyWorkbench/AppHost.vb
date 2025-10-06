Public Interface AppHost

    Property WindowState As FormWindowState
    ''' <summary>
    ''' get/set current actived mdi document
    ''' </summary>
    ''' <returns></returns>
    Property ActiveDocument As Form

    Event ResizeForm(location As Point, size As Size)

    Function GetDesktopLocation() As Point
    Function GetClientSize() As Size

    Sub StatusMessage(msg As String, Optional icon As Image = Nothing)

End Interface
