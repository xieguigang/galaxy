'
'    ChromeTabControl is a .Net control that mimics Google Chrome's tab bar.
'    Copyright (C) 2013  Brandon Francis
'
'    This program is free software: you can redistribute it and/or modify
'    it under the terms of the GNU General Public License as published by
'    the Free Software Foundation, either version 3 of the License, or
'    (at your option) any later version.
'
'    This program is distributed in the hope that it will be useful,
'    but WITHOUT ANY WARRANTY; without even the implied warranty of
'    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'    GNU General Public License for more details.
'
'    You should have received a copy of the GNU General Public License
'    along with this program.  If not, see <http://www.gnu.org/licenses/>.
'


Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Runtime.InteropServices
Imports sys = System.Math

Namespace ChromeTabControl

    ''' <summary>
    ''' Static utility methods.
    ''' </summary>
    Public Module Utils

        ''' <summary>
        ''' Tints an image with a specified color and opacity.
        ''' </summary>
        ''' <param name="source">The bitmap to tint.</param>
        ''' <param name="tintcolor">The color to tint the bitmap.</param>
        ''' <param name="opacityPercent">The opacity of the tint.</param>
        ''' <returns>The new tinted bitmap.</returns>
        Public Function TintBitmap(source As Bitmap, tintcolor As Color, opacityPercent As Single) As Bitmap
            If opacityPercent = 0 Then
                Return source
            End If
            Dim rtn As New Bitmap(source)

            Dim source2 As New Bitmap(rtn)
            Dim manipulator As New BitmapPixelManipulator(source2)
            manipulator.LockBits()
            For y As Integer = 0 To manipulator.Height - 1
                For x As Integer = 0 To manipulator.Width - 1
                    Dim pix As Color = manipulator.GetPixel(x, y)
                    If pix.A > 0 Then
                        manipulator.SetPixel(x, y, Color.FromArgb(pix.A, tintcolor))
                    End If
                Next
            Next
            manipulator.UnlockBits()

            Dim g As Graphics = Graphics.FromImage(rtn)
            g.InterpolationMode = InterpolationMode.HighQualityBicubic
            g.SmoothingMode = SmoothingMode.AntiAlias
            g.DrawImage(Utils.SetBitmapOpacity(source, opacityPercent), New Rectangle(0, 0, rtn.Width, rtn.Height))
            g.Dispose()
            Return rtn
        End Function

        ''' <summary>
        ''' Resizes a bitmap to a new size. Unconstrained.
        ''' </summary>
        ''' <param name="source">The image to resize.</param>
        ''' <param name="size">The size to resize the image to.</param>
        ''' <returns>A resized image as a bitmap.</returns>
        Public Function ResizeBitmap(source As Bitmap, size As Size) As Bitmap
            If source.Size.Equals(size) Then
                Return source
            End If
            Dim b As New Bitmap(size.Width, size.Height)
            Dim g As Graphics = Graphics.FromImage(b)
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic
            g.DrawImage(source, 0, 0, size.Width, size.Height)
            g.Dispose()
            Return b
        End Function

        ''' <summary>
        ''' Draws an image with a specific opacity.
        ''' </summary>
        ''' <param name="g">Graphics to use.</param>
        ''' <param name="source">The source image to draw.</param>
        ''' <param name="rect">The bounds for the image to draw.</param>
        ''' <param name="opacityPercent">The opacity as a percent.</param>
        Public Sub DrawImageTransparent(g As Graphics, source As Bitmap, rect As RectangleF, opacityPercent As Single)
            If opacityPercent > 100 Then
                opacityPercent = 100
            End If
            If opacityPercent < 0 Then
                opacityPercent = 0
            End If
            If opacityPercent = 0 Then
                Return
            End If
            If opacityPercent = 100 Then
                g.DrawImage(source, rect)
                Return
            End If
            Dim ia As New System.Drawing.Imaging.ImageAttributes()
            Dim cm As New System.Drawing.Imaging.ColorMatrix()
            cm.Matrix33 = (opacityPercent / 100)
            ia.SetColorMatrix(cm)
            Dim destPoints As PointF() = New PointF() {New PointF(rect.X, rect.Y), New PointF(rect.X + source.Width, rect.Y), New PointF(rect.X, rect.Y + source.Height)}
            g.DrawImage(source, destPoints, New Rectangle(0, 0, source.Width, source.Height), GraphicsUnit.Pixel, ia)
        End Sub

        ''' <summary>
        ''' Draws an image with a specific opacity.
        ''' </summary>
        ''' <param name="g">The graphics to use.</param>
        ''' <param name="source">The image to draw.</param>
        ''' <param name="x">The x-coordinate to draw to.</param>
        ''' <param name="y">The y-coordinate to draw to.</param>
        ''' <param name="opacityPercent">The opacity as a percent.</param>
        Public Sub DrawImageTransparent(g As Graphics, source As Bitmap, x As Single, y As Single, opacityPercent As Single)
            DrawImageTransparent(g, source, New RectangleF(x, y, source.Width, source.Height), opacityPercent)
        End Sub


        ''' <summary>
        ''' Sets the opacity of the bitmap.
        ''' </summary>
        ''' <param name="source">The bitmap to set the opacity of.</param>
        ''' <param name="opacityPercent">The percent of the opacity between 0 and 100.</param>
        ''' <returns>The new bitmap.</returns>
        Public Function SetBitmapOpacity(source As Bitmap, opacityPercent As Single) As Bitmap
            Dim rtn As New Bitmap(source.Width, source.Height)
            Dim g As Graphics = Graphics.FromImage(rtn)
            g.InterpolationMode = InterpolationMode.HighQualityBicubic
            g.SmoothingMode = SmoothingMode.AntiAlias
            DrawImageTransparent(g, source, New RectangleF(0, 0, source.Width, source.Height), opacityPercent)
            g.Dispose()
            Return rtn
        End Function

        ''' <summary>
        ''' Returns a rounded rectangle graphics path according to a base rectangle and radius.
        ''' </summary>
        ''' <param name="baseRect">The rectangle to copy from.</param>
        ''' <param name="radius">The radius of the rounded corners.</param>
        ''' <returns>A rounded rectangle as a graphics path.</returns>
        Public Function GetRoundedRectanglePath(baseRect As RectangleF, radius As Single) As GraphicsPath
            ' Set up the graphics path
            Dim path As New GraphicsPath()

            ' Return the original rectangle if radius is 0
            If radius <= 0F Then
                path.AddRectangle(baseRect)
                path.CloseFigure()
                Return path
            End If

            ' Return a capsule if needed
            If radius >= (sys.Min(baseRect.Width, baseRect.Height) / 2.0F) Then
                Return GetCapsulePath(baseRect)
            End If

            ' Create the arc
            Dim diameter As Single = radius * 2.0F
            Dim sizef As New SizeF(diameter, diameter)
            Dim arc As New RectangleF(baseRect.Location, sizef)

            ' Top left arc
            path.AddArc(arc, 180, 90)

            ' Top right arc
            arc.X = baseRect.Right - diameter
            path.AddArc(arc, 270, 90)

            ' Bottom right arc
            arc.Y = baseRect.Bottom - diameter
            path.AddArc(arc, 0, 90)

            ' Bottom left arc
            arc.X = baseRect.Left
            path.AddArc(arc, 90, 90)

            ' Close and return
            path.CloseFigure()
            Return path

        End Function

        ''' <summary>
        ''' Returns a capsule according to a base rectangle.
        ''' </summary>
        ''' <param name="baseRect">The base rectangle to copy from.</param>
        ''' <returns>A capsule as a graphics path.</returns>
        Public Function GetCapsulePath(baseRect As RectangleF) As GraphicsPath
            Dim diameter As Single
            Dim arc As RectangleF
            Dim path As New GraphicsPath()
            Try
                If baseRect.Width > baseRect.Height Then
                    diameter = baseRect.Height
                    Dim sizef As New SizeF(diameter, diameter)
                    arc = New RectangleF(baseRect.Location, sizef)
                    path.AddArc(arc, 90, 180)
                    arc.X = baseRect.Right - diameter
                    path.AddArc(arc, 270, 180)
                ElseIf baseRect.Width < baseRect.Height Then
                    diameter = baseRect.Height
                    Dim sizef As New SizeF(diameter, diameter)
                    arc = New RectangleF(baseRect.Location, sizef)
                    path.AddArc(arc, 180, 180)
                    arc.Y = baseRect.Bottom - diameter
                    path.AddArc(arc, 0, 180)
                Else
                    path.AddEllipse(baseRect)
                End If
            Catch
                path.AddEllipse(baseRect)
            Finally
                path.CloseFigure()
            End Try
            Return path
        End Function

        ''' <summary>
        ''' Draws a path's shadow.
        ''' </summary>
        ''' <param name="g">The graphics to use.</param>
        ''' <param name="path">The path to get the shadow for.</param>
        ''' <param name="size">The size in pixels of the shadow.</param>
        Public Sub DrawPathShadow(g As Graphics, path As GraphicsPath, size As Integer)
            If size <= 0 Then
                Return
            End If

            'Create the new path and scale it
            Dim path2 As GraphicsPath = DirectCast(path.Clone(), GraphicsPath)
            ScalePath(path2, size)

            'Start stepping through
            Dim maxOpacity As Integer = 35
            Dim stepsize As Integer = maxOpacity \ size
            Dim i As Integer = size - 1
            While i >= 0
                'g.FillPath(New SolidBrush(Color.FromArgb(maxOpacity - (stepsize * i), Color.DimGray)), path2)
                g.DrawPath(New Pen(Color.FromArgb(maxOpacity - (stepsize * i), Color.DimGray)), path2)
                ScalePath(path2, -1)
                i += -1
            End While

        End Sub

        ''' <summary>
        ''' Scales a graphics path a specific number of pixels.
        ''' </summary>
        ''' <param name="path">The path to scale.</param>
        ''' <param name="pixels">The amount to scale in pixels.</param>
        Public Sub ScalePath(path As GraphicsPath, pixels As Single)
            Dim rect As RectangleF = path.GetBounds()
            path.Warp(New PointF() {New PointF(rect.X - pixels, rect.Y - pixels), New PointF(rect.Right + pixels, rect.Y - pixels), New PointF(rect.X - pixels, rect.Bottom + pixels)}, rect)
        End Sub

        ''' <summary>
        ''' Moves a graphics path a certain amount of pixels.
        ''' </summary>
        ''' <param name="path">The path to move.</param>
        ''' <param name="xmodifier">The pixels to move in the x position.</param>
        ''' <param name="ymodifier">The pixels to move in the y position.</param>
        Public Sub MovePath(path As GraphicsPath, xmodifier As Single, ymodifier As Single)
            Dim translator As New Matrix()
            translator.Translate(xmodifier, ymodifier)
            path.Transform(translator)
        End Sub

        ''' <summary>
        ''' Draws a rectangle's shadow.
        ''' </summary>
        ''' <param name="g">The graphics to use.</param>
        ''' <param name="rect">The retangle to get the shadow of.</param>
        ''' <param name="size">The size of the shadow in pixels.</param>
        Public Sub DrawPathShadow(g As Graphics, rect As RectangleF, size As Integer)
            If size <= 0 Then
                Return
            End If
            Dim path As New GraphicsPath()
            path.AddRectangle(rect)
            DrawPathShadow(g, path, size)
        End Sub

        ''' <summary>
        ''' Locks and unlocks bitmap bits for faster pixel manipulation.
        ''' </summary>
        Public Class BitmapPixelManipulator

            Private source As Bitmap = Nothing
            Private Iptr As IntPtr = IntPtr.Zero

            Private bitmapData As System.Drawing.Imaging.BitmapData = Nothing

            Public Property Pixels() As Byte()
            Public Property Depth() As Integer
            Public Property Width() As Integer
            Public Property Height() As Integer

            Public Sub New(source As Bitmap)
                Me.source = source
            End Sub

            Public Sub LockBits()
                ' Get width and height of bitmap
                Width = source.Width
                Height = source.Height

                ' get total locked pixels count
                Dim PixelCount As Integer = Width * Height

                ' Create rectangle to lock
                Dim rect As New Rectangle(0, 0, Width, Height)

                ' get source bitmap pixel format size
                Depth = System.Drawing.Bitmap.GetPixelFormatSize(source.PixelFormat)

                ' Check if bpp (Bits Per Pixel) is 8, 24, or 32
                If Depth <> 8 AndAlso Depth <> 24 AndAlso Depth <> 32 Then
                    Throw New ArgumentException("Only 8, 24 and 32 bpp images are supported.")
                End If

                ' Lock bitmap and return bitmap data
                bitmapData = source.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, source.PixelFormat)

                ' create byte array to copy pixel values
                Dim [step] As Integer = Depth \ 8
                Pixels = New Byte(PixelCount * [step] - 1) {}
                Iptr = bitmapData.Scan0

                ' Copy data from pointer to array
                System.Runtime.InteropServices.Marshal.Copy(Iptr, Pixels, 0, Pixels.Length)
            End Sub

            Public Sub UnlockBits()
                ' Copy data from byte array to pointer
                System.Runtime.InteropServices.Marshal.Copy(Pixels, 0, Iptr, Pixels.Length)

                ' Unlock bitmap data
                source.UnlockBits(bitmapData)
            End Sub

            Public Function GetPixel(x As Integer, y As Integer) As Color
                Dim clr As Color = Color.Empty

                ' Get color components count
                Dim cCount As Integer = Depth \ 8

                ' Get start index of the specified pixel
                Dim i As Integer = ((y * Width) + x) * cCount

                If i > Pixels.Length - cCount Then
                    Throw New IndexOutOfRangeException()
                End If

                If Depth = 32 Then
                    ' For 32 bpp get Red, Green, Blue and Alpha
                    Dim b As Byte = Pixels(i)
                    Dim g As Byte = Pixels(i + 1)
                    Dim r As Byte = Pixels(i + 2)
                    Dim a As Byte = Pixels(i + 3)
                    ' a
                    clr = Color.FromArgb(a, r, g, b)
                End If
                If Depth = 24 Then
                    ' For 24 bpp get Red, Green and Blue
                    Dim b As Byte = Pixels(i)
                    Dim g As Byte = Pixels(i + 1)
                    Dim r As Byte = Pixels(i + 2)
                    clr = Color.FromArgb(r, g, b)
                End If
                If Depth = 8 Then
                    ' For 8 bpp get color value (Red, Green and Blue values are the same)
                    Dim c As Byte = Pixels(i)
                    clr = Color.FromArgb(c, c, c)
                End If
                Return clr
            End Function

            Public Sub SetPixel(x As Integer, y As Integer, color As Color)
                ' Get color components count
                Dim cCount As Integer = Depth \ 8

                ' Get start index of the specified pixel
                Dim i As Integer = ((y * Width) + x) * cCount

                If Depth = 32 Then
                    ' For 32 bpp set Red, Green, Blue and Alpha
                    Pixels(i) = color.B
                    Pixels(i + 1) = color.G
                    Pixels(i + 2) = color.R
                    Pixels(i + 3) = color.A
                End If
                If Depth = 24 Then
                    ' For 24 bpp set Red, Green and Blue
                    Pixels(i) = color.B
                    Pixels(i + 1) = color.G
                    Pixels(i + 2) = color.R
                End If
                If Depth = 8 Then
                    ' For 8 bpp set color value (Red, Green and Blue values are the same)
                    Pixels(i) = color.B
                End If
            End Sub
        End Class
    End Module
End Namespace