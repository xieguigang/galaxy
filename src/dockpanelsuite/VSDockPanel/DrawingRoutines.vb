Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Runtime.CompilerServices

Namespace WeifenLuo.WinFormsUI.Docking
    Public Module DrawingRoutines
        <Extension()>
        Public Sub SafelyDrawLinearGradient(rectangle As Rectangle, startColor As Color, endColor As Color, mode As LinearGradientMode, graphics As Graphics, Optional blend As Blend = Nothing)
            If rectangle.Width > 0 AndAlso rectangle.Height > 0 Then
                Using brush As LinearGradientBrush = New LinearGradientBrush(rectangle, startColor, endColor, mode)
                    If blend IsNot Nothing Then
                        brush.Blend = blend
                    End If

                    graphics.FillRectangle(brush, rectangle)
                End Using
            End If
        End Sub

        <Extension()>
        Public Sub SafelyDrawLinearGradientF(rectangle As RectangleF, startColor As Color, endColor As Color, mode As LinearGradientMode, graphics As Graphics)
            If rectangle.Width > 0 AndAlso rectangle.Height > 0 Then
                Using brush As LinearGradientBrush = New LinearGradientBrush(rectangle, startColor, endColor, mode)
                    graphics.FillRectangle(brush, rectangle)
                End Using
            End If
        End Sub
    End Module
End Namespace
