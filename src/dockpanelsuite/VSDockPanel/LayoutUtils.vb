Imports System.Drawing

Namespace Docking
    Public Module LayoutUtils
        Public Function IsZeroWidthOrHeight(rectangle As Rectangle) As Boolean
            Return rectangle.Width = 0 OrElse rectangle.Height = 0
        End Function
    End Module
End Namespace
