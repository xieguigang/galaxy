Imports System
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Windows.Forms

Namespace Docking
    Public Class VisualStudioToolStripRenderer
        Inherits ToolStripProfessionalRenderer
        Private Shared baseSizeGripRectangles As Rectangle() = {New Rectangle(6, 0, 1, 1), New Rectangle(6, 2, 1, 1), New Rectangle(6, 4, 1, 1), New Rectangle(6, 6, 1, 1), New Rectangle(4, 2, 1, 1), New Rectangle(4, 4, 1, 1), New Rectangle(4, 6, 1, 1), New Rectangle(2, 4, 1, 1), New Rectangle(2, 6, 1, 1), New Rectangle(0, 6, 1, 1)}

        Private Const GRIP_PADDING As Integer = 4
        Private _statusBarBrush As SolidBrush
        Private _statusGripBrush As SolidBrush
        Private _statusGripAccentBrush As SolidBrush
        Private _toolBarBrush As SolidBrush
        Private _gripBrush As SolidBrush
        Private _toolBarBorderPen As Pen
        Private _table As VisualStudioColorTable
        Private _palette As DockPanelColorPalette

        Public Property UseGlassOnMenuStrip As Boolean

        Public Sub New(palette As DockPanelColorPalette)
            MyBase.New(New VisualStudioColorTable(palette))
            _table = CType(ColorTable, VisualStudioColorTable)
            _palette = palette
            RoundedEdges = False
            _statusBarBrush = New SolidBrush(palette.MainWindowStatusBarDefault.Background)
            _statusGripBrush = New SolidBrush(palette.MainWindowStatusBarDefault.ResizeGrip)
            _statusGripAccentBrush = New SolidBrush(palette.MainWindowStatusBarDefault.ResizeGripAccent)
            _toolBarBrush = New SolidBrush(palette.CommandBarToolbarDefault.Background)
            _gripBrush = New SolidBrush(palette.CommandBarToolbarDefault.Grip)
            _toolBarBorderPen = New Pen(palette.CommandBarToolbarDefault.Border)

            UseGlassOnMenuStrip = True
        End Sub

#Region "Rendering Improvements (includes fixes for bugs occured when Windows Classic theme is on)."
        '*
        Protected Overrides Sub OnRenderMenuItemBackground(e As ToolStripItemRenderEventArgs)
            ' Do not draw disabled item background.
            If e.Item.Enabled Then
                Dim isMenuDropDown = TypeOf e.Item.Owner Is MenuStrip
                If isMenuDropDown AndAlso e.Item.Pressed Then
                    MyBase.OnRenderMenuItemBackground(e)
                ElseIf e.Item.Selected Then
                    ' Rect of item's content area.
                    Dim contentRect = e.Item.ContentRectangle

                    ' Fix item rect.
                    Dim itemRect As Rectangle = If(isMenuDropDown, New Rectangle(contentRect.X + 2, contentRect.Y - 2, contentRect.Width - 5, contentRect.Height + 3), New Rectangle(contentRect.X, contentRect.Y - 1, contentRect.Width, contentRect.Height + 1))

                    ' Border pen and fill brush.
                    Dim pen = ColorTable.MenuItemBorder
                    Dim brushBegin As Color
                    Dim brushEnd As Color

                    If isMenuDropDown Then
                        brushBegin = ColorTable.MenuItemSelectedGradientBegin
                        brushEnd = ColorTable.MenuItemSelectedGradientEnd
                    Else
                        brushBegin = ColorTable.MenuItemSelected
                        brushEnd = Color.Empty
                    End If

                    DrawRectangle(e.Graphics, itemRect, brushBegin, brushEnd, pen, UseGlassOnMenuStrip)
                End If
            End If
        End Sub

        Protected Overrides Sub OnRenderToolStripBorder(e As ToolStripRenderEventArgs)
            Dim status = TryCast(e.ToolStrip, StatusStrip)
            If status IsNot Nothing Then
                ' IMPORTANT: left empty to remove white border.
                Return
            End If

            Dim context = TryCast(e.ToolStrip, MenuStrip)
            If context IsNot Nothing Then
                MyBase.OnRenderToolStripBorder(e)
                Return
            End If

            Dim drop = TryCast(e.ToolStrip, ToolStripDropDown)
            If drop IsNot Nothing Then
                MyBase.OnRenderToolStripBorder(e)
                Return
            End If

            Dim rect = e.ToolStrip.ClientRectangle
            e.Graphics.DrawRectangle(_toolBarBorderPen, New Rectangle(rect.Location, New Size(rect.Width - 1, rect.Height - 1)))
        End Sub

        Protected Overrides Sub OnRenderToolStripBackground(e As ToolStripRenderEventArgs)
            Dim status = TryCast(e.ToolStrip, StatusStrip)
            If status IsNot Nothing Then
                MyBase.OnRenderToolStripBackground(e)
                Return
            End If

            Dim context = TryCast(e.ToolStrip, MenuStrip)
            If context IsNot Nothing Then
                MyBase.OnRenderToolStripBackground(e)
                Return
            End If

            Dim drop = TryCast(e.ToolStrip, ToolStripDropDown)
            If drop IsNot Nothing Then
                MyBase.OnRenderToolStripBackground(e)
                Return
            End If

            e.Graphics.FillRectangle(_toolBarBrush, e.ToolStrip.ClientRectangle)
        End Sub

        Protected Overrides Sub OnRenderStatusStripSizingGrip(e As ToolStripRenderEventArgs)
            ' IMPORTANT: below code was taken from Microsoft's reference code (MIT license).
            Dim g = e.Graphics
            Dim statusStrip As StatusStrip = TryCast(e.ToolStrip, StatusStrip)

            ' we have a set of stock rectangles.  Translate them over to where the grip is to be drawn
            ' for the white set, then translate them up and right one pixel for the grey.


            If statusStrip IsNot Nothing Then
                Dim sizeGripBounds = statusStrip.SizeGripBounds
                If Not IsZeroWidthOrHeight(sizeGripBounds) Then
                    Dim whiteRectangles = New Rectangle(baseSizeGripRectangles.Length - 1) {}
                    Dim greyRectangles = New Rectangle(baseSizeGripRectangles.Length - 1) {}

                    For i = 0 To baseSizeGripRectangles.Length - 1
                        Dim baseRect = baseSizeGripRectangles(i)
                        If statusStrip.RightToLeft = RightToLeft.Yes Then
                            baseRect.X = sizeGripBounds.Width - baseRect.X - baseRect.Width
                        End If
                        baseRect.Offset(sizeGripBounds.X, sizeGripBounds.Bottom - 12)
                        greyRectangles(i) = baseRect
                        If statusStrip.RightToLeft = RightToLeft.Yes Then
                            baseRect.Offset(1, -1)
                        Else
                            baseRect.Offset(-1, -1)
                        End If
                        whiteRectangles(i) = baseRect
                    Next

                    g.FillRectangles(_statusGripAccentBrush, whiteRectangles)
                    g.FillRectangles(_statusGripBrush, greyRectangles)
                End If
            End If
        End Sub

        Protected Overrides Sub OnRenderGrip(e As ToolStripGripRenderEventArgs)
            Dim g = e.Graphics
            Dim bounds = e.GripBounds
            Dim toolStrip = e.ToolStrip

            Dim rightToLeft = e.ToolStrip.RightToLeft = System.Windows.Forms.RightToLeft.Yes

            Dim height = If(toolStrip.Orientation = Orientation.Horizontal, bounds.Height, bounds.Width)
            Dim width = If(toolStrip.Orientation = Orientation.Horizontal, bounds.Width, bounds.Height)

            Dim numRectangles As Integer = CInt((height - GRIP_PADDING * 2) / 4)

            If numRectangles > 0 Then
                numRectangles += 1
                ' a MenuStrip starts its grip lower and has fewer grip rectangles.
                Dim yOffset = If((TypeOf toolStrip Is MenuStrip), 2, 0)

                Dim shadowRects = New Rectangle(numRectangles - 1) {}
                Dim startY = GRIP_PADDING + 1 + yOffset
                Dim startX As Integer = CInt(width / 2)

                For i = 0 To numRectangles - 1
                    shadowRects(i) = If(toolStrip.Orientation = Orientation.Horizontal, New Rectangle(startX, startY, 1, 1), New Rectangle(startY, startX, 1, 1))

                    startY += 4
                Next

                ' in RTL the GripLight rects should paint to the left of the GripDark rects.
                Dim xOffset = If(rightToLeft, 2, -2)

                If rightToLeft Then
                    ' scoot over the rects in RTL so they fit within the bounds.
                    For i = 0 To numRectangles - 1
                        shadowRects(i).Offset(-xOffset, 0)
                    Next
                End If

                Dim b As Brush = _gripBrush
                For i = 0 To numRectangles - 1 - 1
                    g.FillRectangle(b, shadowRects(i))
                Next

                For i = 0 To numRectangles - 1
                    shadowRects(i).Offset(xOffset, -2)
                Next

                g.FillRectangles(b, shadowRects)

                For i = 0 To numRectangles - 1
                    shadowRects(i).Offset(-2 * xOffset, 0)
                Next

                g.FillRectangles(b, shadowRects)
            End If
        End Sub

        Protected Overrides Sub OnRenderButtonBackground(e As ToolStripItemRenderEventArgs)
            Dim button As ToolStripButton = TryCast(e.Item, ToolStripButton)
            If button IsNot Nothing AndAlso button.Enabled Then
                If button.Selected OrElse button.Checked Then
                    ' Rect of item's content area.
                    Dim contentRect As Rectangle = New Rectangle(0, 0, button.Width - 1, button.Height - 1)

                    Dim pen As Color
                    Dim brushBegin As Color
                    Dim brushMiddle As Color
                    Dim brushEnd As Color

                    If button.Checked Then
                        If button.Selected Then
                            pen = _table.ButtonCheckedHoveredBorder
                            brushBegin = _table.ButtonCheckedHoveredBackground
                            brushMiddle = _table.ButtonCheckedHoveredBackground
                            brushEnd = _table.ButtonCheckedHoveredBackground
                        Else
                            pen = _table.ButtonCheckedBorder
                            brushBegin = ColorTable.ButtonCheckedGradientBegin
                            brushMiddle = ColorTable.ButtonCheckedGradientMiddle
                            brushEnd = ColorTable.ButtonCheckedGradientEnd
                        End If
                    ElseIf button.Pressed Then
                        pen = ColorTable.ButtonPressedBorder
                        brushBegin = ColorTable.ButtonPressedGradientBegin
                        brushMiddle = ColorTable.ButtonPressedGradientMiddle
                        brushEnd = ColorTable.ButtonPressedGradientEnd
                    Else
                        pen = ColorTable.ButtonSelectedBorder
                        brushBegin = ColorTable.ButtonSelectedGradientBegin
                        brushMiddle = ColorTable.ButtonSelectedGradientMiddle
                        brushEnd = ColorTable.ButtonSelectedGradientEnd
                    End If

                    DrawRectangle(e.Graphics, contentRect, brushBegin, brushMiddle, brushEnd, pen, False)
                End If
            Else
                MyBase.OnRenderButtonBackground(e)
            End If
        End Sub

        Protected Overrides Sub Initialize(toolStrip As ToolStrip)
            MyBase.Initialize(toolStrip)
            ' IMPORTANT: enlarge grip area so grip can be rendered fully.
            toolStrip.GripMargin = New Padding(toolStrip.GripMargin.All + 1)
        End Sub

        Protected Overrides Sub OnRenderOverflowButtonBackground(e As ToolStripItemRenderEventArgs)
            Dim cache = _palette.CommandBarMenuPopupDefault.BackgroundTop

            ' IMPORTANT: not 100% accurate as the color change should only happen when the overflow menu is hovered.
            ' here color change happens when the overflow menu is displayed.
            If e.Item.Pressed Then _palette.CommandBarMenuPopupDefault.BackgroundTop = _palette.CommandBarToolbarOverflowPressed.Background
            MyBase.OnRenderOverflowButtonBackground(e)
            If e.Item.Pressed Then _palette.CommandBarMenuPopupDefault.BackgroundTop = cache
        End Sub

        Protected Overrides Sub OnRenderArrow(e As ToolStripArrowRenderEventArgs)
            e.ArrowColor = If(e.Item.Selected, _palette.CommandBarMenuPopupHovered.Arrow, _palette.CommandBarMenuPopupDefault.Arrow)
            MyBase.OnRenderArrow(e)
        End Sub

        Protected Overrides Sub OnRenderItemCheck(e As ToolStripItemImageRenderEventArgs)
            ' base.OnRenderItemCheck(e);
            Using imageAttr = New ImageAttributes()
                Dim foreColor = If(e.Item.Selected, _palette.CommandBarMenuPopupHovered.Checkmark, _palette.CommandBarMenuPopupDefault.Checkmark)
                Dim backColor = If(e.Item.Selected, _palette.CommandBarMenuPopupHovered.CheckmarkBackground, _palette.CommandBarMenuPopupDefault.CheckmarkBackground)
                Dim borderColor = _palette.CommandBarMenuPopupDefault.Border

                ' Create a color map.
                Dim colorMap = New ColorMap(0) {}
                colorMap(0) = New ColorMap()

                ' old color determined from testing
                colorMap(0).OldColor = Color.FromArgb(4, 2, 4)
                colorMap(0).NewColor = foreColor
                imageAttr.SetRemapTable(colorMap)

                Using b = New SolidBrush(backColor)
                    e.Graphics.FillRectangle(b, e.ImageRectangle)
                End Using
                e.Graphics.DrawImage(e.Image, e.ImageRectangle, 0, 0, e.Image.Width, e.Image.Height, GraphicsUnit.Pixel, imageAttr)
                Using p = New Pen(borderColor)
                    e.Graphics.DrawRectangle(p, e.ImageRectangle)
                End Using
            End Using
        End Sub

        Protected Overrides Sub OnRenderSeparator(e As ToolStripSeparatorRenderEventArgs)
            Dim r = e.Item.ContentRectangle
            If e.Vertical Then
                Using p = New Pen(_palette.CommandBarToolbarDefault.Separator)
                    e.Graphics.DrawLine(p, r.X, r.Y, r.X, r.Y + r.Height)
                End Using
                Using p = New Pen(_palette.CommandBarToolbarDefault.SeparatorAccent)
                    e.Graphics.DrawLine(p, r.X + 1, r.Y, r.X + 1, r.Y + r.Height)
                End Using
            Else
                ' if this is a menu, then account for the image column
                Dim x1 = r.X
                Dim x2 = r.X + r.Width
                Dim menu = TryCast(e.ToolStrip, ToolStripDropDownMenu)
                If menu IsNot Nothing Then
                    x1 += menu.Padding.Left
                    x2 -= menu.Padding.Right
                End If

                Using p = New Pen(_palette.CommandBarToolbarDefault.Separator)
                    e.Graphics.DrawLine(p, x1, r.Y, x2, r.Y)
                End Using
                Using p = New Pen(_palette.CommandBarToolbarDefault.SeparatorAccent)
                    e.Graphics.DrawLine(p, x1, r.Y + 1, x2, r.Y + 1)
                End Using
            End If
        End Sub

        Protected Overrides Sub OnRenderItemText(e As ToolStripItemTextRenderEventArgs)
            Dim color As Color = Color.Black
            Dim toolStrip = e.ToolStrip
            If TypeOf toolStrip Is StatusStrip Then
                If e.Item.Selected Then
                    color = _palette.MainWindowStatusBarDefault.HighlightText
                Else
                    color = _palette.MainWindowStatusBarDefault.Text
                End If
            ElseIf TypeOf toolStrip Is MenuStrip Then
                Dim button = TryCast(e.Item, ToolStripButton)
                Dim checkedButton = button IsNot Nothing AndAlso button.Checked
                If Not e.Item.Enabled Then
                    color = _palette.CommandBarMenuPopupDisabled.Text
                ElseIf button IsNot Nothing AndAlso button.Pressed Then
                    color = _palette.CommandBarToolbarButtonPressed.Text
                ElseIf e.Item.Selected AndAlso checkedButton Then
                    color = _palette.CommandBarToolbarButtonCheckedHovered.Text
                ElseIf e.Item.Selected Then
                    color = _palette.CommandBarMenuTopLevelHeaderHovered.Text
                ElseIf checkedButton Then
                    color = _palette.CommandBarToolbarButtonChecked.Text
                Else
                    color = _palette.CommandBarMenuDefault.Text
                End If
            ElseIf TypeOf toolStrip Is ToolStripDropDown Then
                ' This might differ from above branch, but left the same here.
                Dim button = TryCast(e.Item, ToolStripButton)
                Dim checkedButton = button IsNot Nothing AndAlso button.Checked
                If Not e.Item.Enabled Then
                    color = _palette.CommandBarMenuPopupDisabled.Text
                ElseIf button IsNot Nothing AndAlso button.Pressed Then
                    color = _palette.CommandBarToolbarButtonPressed.Text
                ElseIf e.Item.Selected AndAlso checkedButton Then
                    color = _palette.CommandBarToolbarButtonCheckedHovered.Text
                ElseIf e.Item.Selected Then
                    color = _palette.CommandBarMenuTopLevelHeaderHovered.Text
                ElseIf checkedButton Then
                    color = _palette.CommandBarToolbarButtonChecked.Text
                Else
                    color = _palette.CommandBarMenuDefault.Text
                End If
            Else
                ' Default color, if not it will be black no matter what 
                If Not e.Item.Enabled Then
                    color = _palette.CommandBarMenuPopupDisabled.Text
                Else
                    color = _palette.CommandBarMenuDefault.Text
                End If
            End If

            TextRenderer.DrawText(e.Graphics, e.Text, e.TextFont, e.TextRectangle, color, e.TextFormat)
        End Sub

#Region "helpers"
        Private Shared Sub DrawRectangle(graphics As Graphics, rect As Rectangle, brushBegin As Color, brushMiddle As Color, brushEnd As Color, penColor As Color, glass As Boolean)
            Dim firstHalf As RectangleF = New RectangleF(rect.X, rect.Y, rect.Width, CSng(rect.Height) / 2)

            Dim secondHalf As RectangleF = New RectangleF(rect.X, rect.Y + CSng(rect.Height) / 2, rect.Width, CSng(rect.Height) / 2)

            If brushMiddle.IsEmpty AndAlso brushEnd.IsEmpty Then
                graphics.FillRectangle(New SolidBrush(brushBegin), rect)
            End If
            If brushMiddle.IsEmpty Then
                rect.SafelyDrawLinearGradient(brushBegin, brushEnd, LinearGradientMode.Vertical, graphics)
            Else
                firstHalf.SafelyDrawLinearGradientF(brushBegin, brushMiddle, LinearGradientMode.Vertical, graphics)
                secondHalf.SafelyDrawLinearGradientF(brushMiddle, brushEnd, LinearGradientMode.Vertical, graphics)
            End If

            If glass Then
                Dim glassBrush As Brush = New SolidBrush(Color.FromArgb(120, Color.White))
                graphics.FillRectangle(glassBrush, firstHalf)
            End If

            If penColor.A > 0 Then
                graphics.DrawRectangle(New Pen(penColor), rect)
            End If
        End Sub

        Private Shared Sub DrawRectangle(graphics As Graphics, rect As Rectangle, brushBegin As Color, brushEnd As Color, penColor As Color, glass As Boolean)
            DrawRectangle(graphics, rect, brushBegin, Color.Empty, brushEnd, penColor, glass)
        End Sub

        Private Shared Sub DrawRectangle(graphics As Graphics, rect As Rectangle, brush As Color, penColor As Color, glass As Boolean)
            DrawRectangle(graphics, rect, brush, Color.Empty, Color.Empty, penColor, glass)
        End Sub

        Private Shared Sub FillRoundRectangle(graphics As Graphics, brush As Brush, rect As Rectangle, radius As Integer)
            Dim fx = Convert.ToSingle(rect.X)
            Dim fy = Convert.ToSingle(rect.Y)
            Dim fwidth = Convert.ToSingle(rect.Width)
            Dim fheight = Convert.ToSingle(rect.Height)
            Dim fradius = Convert.ToSingle(radius)
            FillRoundRectangle(graphics, brush, fx, fy, fwidth, fheight, fradius)
        End Sub

        Private Shared Sub FillRoundRectangle(graphics As Graphics, brush As Brush, x As Single, y As Single, width As Single, height As Single, radius As Single)
            Dim rectangle As RectangleF = New RectangleF(x, y, width, height)
            Dim path = GetRoundedRect(rectangle, radius)
            graphics.FillPath(brush, path)
        End Sub

        Private Shared Sub DrawRoundRectangle(graphics As Graphics, pen As Pen, rect As Rectangle, radius As Integer)
            Dim fx = Convert.ToSingle(rect.X)
            Dim fy = Convert.ToSingle(rect.Y)
            Dim fwidth = Convert.ToSingle(rect.Width)
            Dim fheight = Convert.ToSingle(rect.Height)
            Dim fradius = Convert.ToSingle(radius)
            DrawRoundRectangle(graphics, pen, fx, fy, fwidth, fheight, fradius)
        End Sub

        Private Shared Sub DrawRoundRectangle(graphics As Graphics, pen As Pen, x As Single, y As Single, width As Single, height As Single, radius As Single)
            Dim rectangle As RectangleF = New RectangleF(x, y, width, height)
            Dim path = GetRoundedRect(rectangle, radius)
            graphics.DrawPath(pen, path)
        End Sub

        Private Shared Function GetRoundedRect(baseRect As RectangleF, radius As Single) As GraphicsPath
            ' if corner radius is less than or equal to zero, 
            ' return the original rectangle 

            If radius <= 0 Then
                Dim mPath As GraphicsPath = New GraphicsPath()
                mPath.AddRectangle(baseRect)
                mPath.CloseFigure()
                Return mPath
            End If

            ' if the corner radius is greater than or equal to 
            ' half the width, or height (whichever is shorter) 
            ' then return a capsule instead of a lozenge 

            If radius >= Math.Min(baseRect.Width, baseRect.Height) / 2.0 Then Return GetCapsule(baseRect)

            ' create the arc for the rectangle sides and declare 
            ' a graphics path object for the drawing 

            Dim diameter = radius * 2.0F
            Dim sizeF As SizeF = New SizeF(diameter, diameter)
            Dim arc As RectangleF = New RectangleF(baseRect.Location, sizeF)
            Dim path As GraphicsPath = New GraphicsPath()

            ' top left arc 
            path.AddArc(arc, 180, 90)

            ' top right arc 
            arc.X = baseRect.Right - diameter
            path.AddArc(arc, 270, 90)

            ' bottom right arc 
            arc.Y = baseRect.Bottom - diameter
            path.AddArc(arc, 0, 90)

            ' bottom left arc
            arc.X = baseRect.Left
            path.AddArc(arc, 90, 90)

            path.CloseFigure()
            Return path
        End Function

        Private Shared Function GetCapsule(baseRect As RectangleF) As GraphicsPath
            Dim arc As RectangleF
            Dim path As GraphicsPath = New GraphicsPath()

            Try
                Dim diameter As Single
                If baseRect.Width > baseRect.Height Then
                    ' return horizontal capsule 
                    diameter = baseRect.Height
                    Dim sizeF As SizeF = New SizeF(diameter, diameter)
                    arc = New RectangleF(baseRect.Location, sizeF)
                    path.AddArc(arc, 90, 180)
                    arc.X = baseRect.Right - diameter
                    path.AddArc(arc, 270, 180)
                ElseIf baseRect.Width < baseRect.Height Then
                    ' return vertical capsule 
                    diameter = baseRect.Width
                    Dim sizeF As SizeF = New SizeF(diameter, diameter)
                    arc = New RectangleF(baseRect.Location, sizeF)
                    path.AddArc(arc, 180, 180)
                    arc.Y = baseRect.Bottom - diameter
                    path.AddArc(arc, 0, 180)
                Else
                    ' return circle 
                    path.AddEllipse(baseRect)
                End If

            Catch
                path.AddEllipse(baseRect)
            Finally
                path.CloseFigure()
            End Try

            Return path
        End Function
#End Region
        ' 
#End Region
    End Class
End Namespace
