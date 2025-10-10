Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms

Namespace Docking
    Public Module DrawHelper
        Public Function RtlTransform(control As Control, point As Point) As Point
            If control.RightToLeft <> RightToLeft.Yes Then
                Return point
            Else
                Return New Point(control.Right - point.X, point.Y)
            End If
        End Function

        Public Function RtlTransform(control As Control, rectangle As Rectangle) As Rectangle
            If control.RightToLeft <> RightToLeft.Yes Then
                Return rectangle
            Else
                Return New Rectangle(control.ClientRectangle.Right - rectangle.Right, rectangle.Y, rectangle.Width, rectangle.Height)
            End If
        End Function

        Public Function GetRoundedCornerTab(graphicsPath As GraphicsPath, rect As Rectangle, upCorner As Boolean) As GraphicsPath
            If graphicsPath Is Nothing Then
                graphicsPath = New GraphicsPath()
            Else
                graphicsPath.Reset()
            End If

            Dim curveSize = 6
            If upCorner Then
                graphicsPath.AddLine(rect.Left, rect.Bottom, rect.Left, CSng(rect.Top + curveSize / 2))
                graphicsPath.AddArc(New Rectangle(rect.Left, rect.Top, curveSize, curveSize), 180, 90)
                graphicsPath.AddLine(CSng(rect.Left + curveSize / 2), rect.Top, CSng(rect.Right - curveSize / 2), rect.Top)
                graphicsPath.AddArc(New Rectangle(rect.Right - curveSize, rect.Top, curveSize, curveSize), -90, 90)
                graphicsPath.AddLine(rect.Right, CSng(rect.Top + curveSize / 2), rect.Right, rect.Bottom)
            Else
                graphicsPath.AddLine(rect.Right, rect.Top, rect.Right, CSng(rect.Bottom - curveSize / 2))
                graphicsPath.AddArc(New Rectangle(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize), 0, 90)
                graphicsPath.AddLine(CSng(rect.Right - curveSize / 2), rect.Bottom, CSng(rect.Left + curveSize / 2), rect.Bottom)
                graphicsPath.AddArc(New Rectangle(rect.Left, rect.Bottom - curveSize, curveSize, curveSize), 90, 90)
                graphicsPath.AddLine(rect.Left, CSng(rect.Bottom - curveSize / 2), rect.Left, rect.Top)
            End If

            Return graphicsPath
        End Function

        Public Function CalculateGraphicsPathFromBitmap(bitmap As Bitmap) As GraphicsPath
            Return CalculateGraphicsPathFromBitmap(bitmap, Color.Empty)
        End Function

        ' From http://edu.cnzz.cn/show_3281.html
        Public Function CalculateGraphicsPathFromBitmap(bitmap As Bitmap, colorTransparent As Color) As GraphicsPath
            Dim graphicsPath As GraphicsPath = New GraphicsPath()
            If colorTransparent = Color.Empty Then colorTransparent = bitmap.GetPixel(0, 0)

            For row = 0 To bitmap.Height - 1
                Dim colOpaquePixel = 0
                For col = 0 To bitmap.Width - 1
                    If bitmap.GetPixel(col, row) <> colorTransparent Then
                        colOpaquePixel = col
                        Dim colNext = col
                        For colNext = colOpaquePixel To bitmap.Width - 1
                            If bitmap.GetPixel(colNext, row) = colorTransparent Then Exit For
                        Next

                        graphicsPath.AddRectangle(New Rectangle(colOpaquePixel, row, colNext - colOpaquePixel, 1))
                        col = colNext
                    End If
                Next
            Next
            Return graphicsPath
        End Function

        Public Function Balance(length As Integer, margin As Integer, input As Integer, lower As Integer, upper As Integer) As Integer
            Return Max(Min(input, upper - length - margin), lower + margin)
        End Function

        Private Function Min(one As Integer, other As Integer) As Integer
            Return If(one > other, other, one)
        End Function

        Private Function Max(one As Integer, other As Integer) As Integer
            Return If(one < other, other, one)
        End Function
    End Module
End Namespace
