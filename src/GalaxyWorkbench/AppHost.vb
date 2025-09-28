Public Interface AppHost

    Event ResizeForm(location As Point, size As Size)

    Function GetDesktopLocation() As Point
    Function GetClientSize() As Size

End Interface
