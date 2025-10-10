Imports System.Drawing

Namespace Docking
    Public Interface IPaintingService
        Function GetPen(color As Color, Optional thickness As Integer = 1) As Pen
        Function GetBrush(color As Color) As SolidBrush
        Sub CleanUp()
    End Interface
End Namespace
