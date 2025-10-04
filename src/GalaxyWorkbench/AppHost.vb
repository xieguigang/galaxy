Public Interface AppHost

    Event ResizeForm(location As Point, size As Size)

    Function GetDesktopLocation() As Point
    Function GetClientSize() As Size

    Sub StatusMessage(msg As String, Optional icon As Image = Nothing)

End Interface
