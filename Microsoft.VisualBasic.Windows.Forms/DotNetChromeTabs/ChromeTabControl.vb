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


    <System.ComponentModel.ToolboxItem(True)>
    Public Class ChromeTabControl
        Inherits Control

        ''' <summary>
        ''' Creates a new tab control.
        ''' </summary>
        Public Sub New()
            ' Set up some Windows variables
            Me.DoubleBuffered = True
            Me.AllowDrop = True
            SetStyle(System.Windows.Forms.ControlStyles.SupportsTransparentBackColor, True)
            SetStyle(ControlStyles.ContainerControl, True)
            SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
            SetStyle(ControlStyles.UserPaint, True)
            SetStyle(ControlStyles.AllPaintingInWmPaint, True)

            ' Set up some local variables
            _TabPages = New TabPage.TabPageCollection(Me)
            _TrayItems = New TrayItem.TrayItemCollection(Me)
            CLOSE_BUTTON_IMAGES = CreateCloseButtonImages()

            ' Create the context menu strip
            ContextMenuStrip = New ContextMenuStrip()
            pinItem = New ToolStripMenuItem("Pin tab")
            reopenItem = New ToolStripMenuItem("Reopen last closed tab")
            AddHandler ContextMenuStrip.Opening, AddressOf contextMenuStrip_Opening
            AddHandler pinItem.Click, AddressOf pinItem_Click
            AddHandler reopenItem.Click, AddressOf reopenItem_Click
            ContextMenuStrip.Items.Add(pinItem)
            ContextMenuStrip.Items.Add(New ToolStripSeparator())

            ContextMenuStrip.Items.Add(reopenItem)
        End Sub

#Region "Windows Functions"

        <DllImport("user32.dll", SetLastError:=True)>
        Public Shared Function ReleaseCapture() As Long
        End Function

        <DllImport("user32.dll", CharSet:=CharSet.Auto)>
        Public Shared Function SendMessage(hWnd As IntPtr, msg As Integer, wParam As Integer, lParam As Integer) As IntPtr
        End Function

#End Region

#Region "Cosntants"

        Public Const TABSTRIP_HEIGHT As Single = 27
        Public Const CONTROL_SHADOW_SIZE As Integer = 3
        Private Const TAB_SPACING As Single = 0
        Private Const TAB_BEZIER_WIDTH As Single = 13
        Private Const TRAY_ITEM_SPACING As Single = 5
        Private Const TRAY_ITEM_SIZE As Integer = 16
        Private Const TRAY_AREA_Y_OFFSET As Integer = 0

#End Region

#Region "Pens and Brushes"

        Private OUTLINE_PEN As New Pen(Brushes.DimGray)
        Private CONTENT_BRUSH As Brush = New SolidBrush(Color.White)
        Private UNSELECTED_BRUSH As Brush = New SolidBrush(Color.FromArgb(210, 207, 217, 228))
        Private HOVER_BRUSH As Brush = New SolidBrush(Color.FromArgb(210, 224, 231, 238))
        Private HOVER_DOWN_BRUSH As Brush = New SolidBrush(Color.FromArgb(210, 224 - 30, 231 - 30, 238 - 30))
        Private UNSELECTED_OUTLINE_PEN As New Pen(New SolidBrush(Color.FromArgb(200, Color.DimGray)))
        Private UNSELECTED_WHITE_ACCENT As New Pen(Color.FromArgb(70, 255, 255, 255))
        Private TABSTRIP_FONT As New Font("Segoe UI", 12, GraphicsUnit.Pixel)
        Private TABSTRIP_FONT_BRUSH As Brush = New SolidBrush(Color.FromArgb(200, 0, 0, 0))

#End Region

#Region "Instance Variables"

        Private CLOSE_BUTTON_IMAGES As Bitmap()
        Private _canvas As TabPage
        Private _dontUpdateTabWidth As Boolean = False

#End Region

#Region "Events"

        ''' <summary>
        ''' Gets called when the new tab button is clicked.
        ''' </summary>
        Public Event NewTabClicked As NewTabClickedEventHandler
        Public Delegate Sub NewTabClickedEventHandler(sender As Object, e As EventArgs)

#End Region

#Region "Public Properties"

        ''' <summary>
        ''' The currently opened pages.
        ''' </summary>
        Public ReadOnly Property TabPages() As TabPage.TabPageCollection

        ''' <summary>
        ''' The current tray items being shown.
        ''' </summary>
        Public ReadOnly Property TrayItems() As TrayItem.TrayItemCollection

        ''' <summary>
        ''' The current width of normal tabs.
        ''' </summary>
        Public ReadOnly Property TabWidth() As Single

        ''' <summary>
        ''' Whether or not to show the new tab button.
        ''' </summary>
        Public Property NewTabButton() As Boolean
            Get
                Return _newTabButton
            End Get
            Set
                Dim flag As Boolean = _newTabButton <> Value
                _newTabButton = Value
                If flag Then
                    UpdateAreas()
                    Invalidate()
                End If
            End Set
        End Property
        Private _newTabButton As Boolean = True

#End Region

#Region "Computed Areas and Paths"

        Private _trayArea As RectangleF
        Private _TabstripArea As RectangleF
        Private _ContentArea As RectangleF
        Private _ContentAreaPath As GraphicsPath

#End Region

#Region "Menu Strip Properties"

        Private hoverAtTimeOfMenuOpening As Integer = -1
        Private pinItem As ToolStripMenuItem
        Private reopenItem As ToolStripMenuItem
        Private Sub pinItem_Click(sender As Object, e As EventArgs)
            _TabPages(hoverAtTimeOfMenuOpening).Pinned = Not _TabPages(hoverAtTimeOfMenuOpening).Pinned
        End Sub
        Private Sub reopenItem_Click(sender As Object, e As EventArgs)
            _TabPages.ReopenTab()
        End Sub
        Private Sub contextMenuStrip_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs)
            hoverAtTimeOfMenuOpening = _hoverTabIndex
            pinItem.Text = "Pin Tab"
            ' The >= accounts for the new tab button
            If _hoverTabIndex < 0 OrElse _hoverTabIndex >= _TabPages.Count Then
                pinItem.Enabled = False
            ElseIf Not _TabPages(hoverAtTimeOfMenuOpening).CanSelect Then
                pinItem.Enabled = False
            Else
                If _TabPages(hoverAtTimeOfMenuOpening).Pinned Then
                    pinItem.Text = "Unpin Tab"
                End If
                pinItem.Enabled = _TabPages(hoverAtTimeOfMenuOpening).CanPin
            End If
            reopenItem.Text = "Reopen last closed tab"
            reopenItem.Enabled = _TabPages.HasTabToReopen()
            If reopenItem.Enabled Then
                reopenItem.Text = "Reopen """ + _TabPages.GetTabToReopen().Title & """"
            End If
        End Sub
#End Region

#Region "Updating Methods"

        ''' <summary>
        ''' Updates the areas to draw into. Usually called when resized.
        ''' </summary>
        Public Sub UpdateAreas()
            ' Update the areas
            _trayArea = New RectangleF(Me.Width - ((TRAY_ITEM_SIZE + TRAY_ITEM_SPACING) * TrayItems.Count), (TABSTRIP_HEIGHT / 2) - (TRAY_ITEM_SIZE \ 2) + TRAY_AREA_Y_OFFSET, ((TRAY_ITEM_SIZE + TRAY_ITEM_SPACING) * TrayItems.Count) - CONTROL_SHADOW_SIZE, TRAY_ITEM_SIZE)
            ' _ContentArea = New RectangleF(CONTROL_SHADOW_SIZE, TABSTRIP_HEIGHT, Me.Width - (CONTROL_SHADOW_SIZE * 2) - 1, Me.Height - TABSTRIP_HEIGHT - CONTROL_SHADOW_SIZE - 1)
            _ContentArea = New RectangleF(0, TABSTRIP_HEIGHT, Me.Width - 1, Me.Height - TABSTRIP_HEIGHT - 1)
            _ContentAreaPath = Utils.GetRoundedRectanglePath(_ContentArea, 3)
            ' _TabstripArea = New RectangleF(CONTROL_SHADOW_SIZE + 3 + TAB_BEZIER_WIDTH, CONTROL_SHADOW_SIZE, Me.Width - (CONTROL_SHADOW_SIZE * 2) - TAB_BEZIER_WIDTH - 6 - (Me.Width - _trayArea.X), TABSTRIP_HEIGHT - CONTROL_SHADOW_SIZE)
            _TabstripArea = New RectangleF(3 + TAB_BEZIER_WIDTH, CONTROL_SHADOW_SIZE, Me.Width - TAB_BEZIER_WIDTH - 6 - (Me.Width - _trayArea.X), TABSTRIP_HEIGHT - CONTROL_SHADOW_SIZE)
            If _newTabButton Then
                _TabstripArea.Width -= 40
            End If

            ' Update tab width and fix for non-full tabs
            If _dontUpdateTabWidth = False Then
                _TabWidth = Convert.ToSingle((_TabstripArea.Width - (TAB_SPACING * _TabPages.Count)) / _TabPages.Count)
                If (_TabPages IsNot Nothing) Then
                    If _TabPages.Count > 0 Then
                        Dim _totalSubtractor As Single = 0
                        Dim _totalModified As Integer = 0
                        For i As Integer = 0 To _TabPages.Count - 1
                            If _TabPages(i).TabWidth > -1 Then
                                _totalSubtractor += (_TabWidth - _TabPages(i).TabWidth)
                                _totalModified += 1
                            End If
                        Next
                        If _TabPages.Count > _totalModified Then
                            _TabWidth += _totalSubtractor / (_TabPages.Count - _totalModified)
                        End If
                    End If
                End If
                _TabWidth = sys.Min(_TabWidth, 175)
                _TabWidth = sys.Max(_TabWidth, 50)
            End If

            ' Redraw the control
            Invalidate()

            ' Call a mouse reclip
            ReclipMouse()

        End Sub

        ''' <summary>
        ''' Sets the current page to act as the canvas.
        ''' </summary>
        ''' <param name="page">The page to set.</param>
        Friend Sub SetCanvas(page As TabPage)
            ' If we already have a canvas in place, let's let it know
            ' it's no longer the canvas
            If _canvas IsNot Nothing Then
                _canvas.OnDeselected()
            End If

            If page Is Nothing Then
                Me.Controls.Clear()
                _canvas = Nothing
                Return
            End If
            _canvas = page
            _canvas.Location = New Point(10, CInt(Math.Truncate(TABSTRIP_HEIGHT)) + 10 + 1)
            _canvas.Size = New Size(Me.Width - 20, Me.Height - 20 - CInt(Math.Truncate(TABSTRIP_HEIGHT)) - 1)
            _canvas.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Top Or AnchorStyles.Right
            _canvas.BackColor = Color.White
            Me.Controls.Clear()
            Me.Controls.Add(_canvas)
            '_canvas.BringToFront();
            _canvas.Focus()

            ' Let the page know it's now the canvas
            _canvas.OnSelected()

        End Sub

        ''' <summary>
        ''' Gets called when this control gets resized.
        ''' </summary>
        ''' <param name="e">The event paramaters.</param>
        Protected Overrides Sub OnResize(e As System.EventArgs)
            MyBase.OnResize(e)
            UpdateAreas()
            Invalidate()
        End Sub

        ''' <summary>
        ''' Creates the array of images used to close tabs.
        ''' </summary>
        ''' <returns>An array of bitmaps with the close tab images.</returns>
        Private Function CreateCloseButtonImages() As Bitmap()

            Dim penWidth As Single = 1.6F

            Dim normal As New Bitmap(16, 16)
            Dim g1 As Graphics = Graphics.FromImage(normal)
            g1.SmoothingMode = SmoothingMode.AntiAlias
            g1.DrawLine(New Pen(Color.FromArgb(204, 132, 144, 159), penWidth), 5, 5, 11, 11)
            g1.DrawLine(New Pen(Color.FromArgb(204, 132, 144, 159), penWidth), 5, 11, 11, 5)
            g1.Dispose()

            Dim hover As New Bitmap(16, 16)
            Dim g2 As Graphics = Graphics.FromImage(hover)
            g2.SmoothingMode = SmoothingMode.AntiAlias
            g2.FillEllipse(New SolidBrush(Color.FromArgb(204, 149, 15, 15)), 2, 2, 12, 12)
            g2.DrawLine(New Pen(Color.FromArgb(204, Color.White), penWidth), 5, 5, 11, 11)
            g2.DrawLine(New Pen(Color.FromArgb(204, Color.White), penWidth), 5, 11, 11, 5)
            g2.Dispose()

            Dim hoverDown As New Bitmap(16, 16)
            Dim g3 As Graphics = Graphics.FromImage(hoverDown)
            g3.SmoothingMode = SmoothingMode.AntiAlias
            g3.FillEllipse(New SolidBrush(Color.FromArgb(204, Color.Black)), 2, 2, 12, 12)
            g3.DrawLine(New Pen(Color.FromArgb(204, Color.White), penWidth), 5, 5, 11, 11)
            g3.DrawLine(New Pen(Color.FromArgb(204, Color.White), penWidth), 5, 11, 11, 5)
            g3.Dispose()

            Return New Bitmap() {normal, hover, hoverDown, Utils.SetBitmapOpacity(hover, 80), Utils.SetBitmapOpacity(hoverDown, 80)}

        End Function

#End Region

#Region "Painting Methods"

        ''' <summary>
        ''' Gets the rectangle associated with the new tab button.
        ''' </summary>
        ''' <param name="rects">The rectangles </param>
        ''' <returns></returns>
        Private Function GetNewTabButtonRect(rects As List(Of RectangleF)) As RectangleF
            Dim height As Single = TABSTRIP_HEIGHT * 0.6F
            If rects.Count > 0 Then
                Return New RectangleF(rects(rects.Count - 1).Right - (TAB_BEZIER_WIDTH / 2), rects(rects.Count - 1).Y + ((TABSTRIP_HEIGHT / 2) - (height / 2)) - 2, 30, height)
            Else
                Return New RectangleF(_TabstripArea.X, _TabstripArea.Y + ((TABSTRIP_HEIGHT / 2) - (height / 2)) - 2, 30, height)
            End If
        End Function

        ''' <summary>
        ''' Gets called when the control gets painted.
        ''' </summary>
        ''' <param name="e">The PaintEventArgs for this event.</param>
        Protected Overrides Sub OnPaint(e As System.Windows.Forms.PaintEventArgs)
            MyBase.OnPaint(e)
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias

            ' Draw each unselected tab
            Dim rects As New List(Of RectangleF)()
            Dim totalSubtractor As Single = 0
            For i As Integer = 0 To _TabPages.Count - 1
                Dim thisSubtractor As Single = 0
                If _TabPages(i).TabWidth > -1 Then
                    thisSubtractor = _TabWidth - (_TabPages(i).TabWidth)
                End If
                Dim rect As New RectangleF(_TabstripArea.X + (_TabWidth * i) + (TAB_SPACING * i) - totalSubtractor, _TabstripArea.Y, _TabWidth - thisSubtractor, _TabstripArea.Height)
                rects.Add(rect)
                totalSubtractor += thisSubtractor
            Next
            Dim ii As Integer = rects.Count - 1
            While ii >= 0
                If _TabPages.SelectedIndex <> ii Then
                    If _TabPages(ii).Width > 100 Then
                        DrawTab(e.Graphics, ii, rects(ii))
                    End If
                End If
                ii += -1
            End While

            ' Draw the content box and outline
            Utils.DrawPathShadow(e.Graphics, _ContentAreaPath, CONTROL_SHADOW_SIZE)
            e.Graphics.FillPath(CONTENT_BRUSH, _ContentAreaPath)
            e.Graphics.DrawPath(OUTLINE_PEN, _ContentAreaPath)

            ' Draw the selected tab
            If _TabPages.SelectedIndex > -1 Then
                If _TabPages(_TabPages.SelectedIndex).Width > 3 Then
                    DrawTab(e.Graphics, _TabPages.SelectedIndex, rects(_TabPages.SelectedIndex))
                End If
            End If

            ' Draw the new tab button
            If _newTabButton Then
                Dim newTabButtonRect As RectangleF = GetNewTabButtonRect(rects)
                Dim path As New GraphicsPath()
                path.AddPath(Utils.GetRoundedRectanglePath(newTabButtonRect, 2.5F), False)
                Dim mat As New Matrix()
                mat.Shear(0.4F, 0F)
                path.Transform(mat)
                Dim path2 As GraphicsPath = DirectCast(path.Clone(), GraphicsPath)
                Utils.ScalePath(path2, -1)
                Utils.DrawPathShadow(e.Graphics, path, 1)
                If _hoverTabIndex = _TabPages.Count Then
                    If _hoverTabCloseDownIndex = _TabPages.Count Then
                        e.Graphics.FillPath(HOVER_DOWN_BRUSH, path)
                    Else
                        e.Graphics.FillPath(HOVER_BRUSH, path)
                    End If
                    e.Graphics.DrawPath(OUTLINE_PEN, path)
                Else
                    e.Graphics.FillPath(UNSELECTED_BRUSH, path)
                    e.Graphics.DrawPath(UNSELECTED_OUTLINE_PEN, path)
                    e.Graphics.DrawPath(UNSELECTED_WHITE_ACCENT, path2)
                End If
            End If

            ' Draw the tray items
            For i As Integer = 0 To TrayItems.Count - 1
                Dim rect As New RectangleF(_trayArea.Right - (i * (TRAY_ITEM_SIZE + TRAY_ITEM_SPACING)) - TRAY_ITEM_SIZE, _trayArea.Y, TRAY_ITEM_SIZE, TRAY_ITEM_SIZE)
                If HoverTrayIndex = i Then
                    If HoverTrayDownIndex = i Then
                        e.Graphics.DrawImage(TrayItems(i).Image16Dark, rect)
                    Else
                        e.Graphics.DrawImage(TrayItems(i).Image16, rect)
                    End If
                Else
                    e.Graphics.DrawImage(TrayItems(i).Image16Transparent, rect)
                End If
            Next
        End Sub

        ''' <summary>
        ''' Gets called when the control paints its background.
        ''' </summary>
        ''' <param name="pevent"></param>
        Protected Overrides Sub OnPaintBackground(pevent As System.Windows.Forms.PaintEventArgs)
            If Me.BackColor = Color.Transparent Then
                Return
            Else
                MyBase.OnPaintBackground(pevent)
            End If
        End Sub

        ''' <summary>
        ''' The different ways to draw the tabs.
        ''' </summary>
        Private Enum TabDrawType As Byte
            Normal = 0
            Hover = 1
            Selected = 2
        End Enum

        ''' <summary>
        ''' Draws a specific tab to the control.
        ''' </summary>
        ''' <param name="g">The graphics to use when drawing.</param>
        ''' <param name="index">The index of the tab.</param>
        ''' <param name="rect">The rectangle associated with the tab.</param>
        Private Sub DrawTab(g As Graphics, index As Integer, rect As RectangleF)

            ' Change the rect around a little
            rect.X -= TAB_BEZIER_WIDTH
            rect.Width += TAB_BEZIER_WIDTH

            ' Prevent clipping issues between the two sides
            If rect.Width < TAB_BEZIER_WIDTH * 2 Then
                rect.Width = TAB_BEZIER_WIDTH * 2
            End If

            ' See if we should draw this being dragged around
            If index = dragIndex Then
                ' Set the new x corresponding to the mouse
                rect.X = CSng(PointToClient(New Point(MousePosition.X - CInt(Math.Truncate(dragStartOffset)), CInt(Math.Truncate(rect.Y)))).X)

                ' Make sure we don't clip outside the box
                If rect.Left < Me._TabstripArea.Left - TAB_BEZIER_WIDTH Then
                    rect.X = _TabstripArea.X - TAB_BEZIER_WIDTH
                End If
                If rect.Right + TAB_BEZIER_WIDTH > Me._TabstripArea.Right Then
                    rect.X = _TabstripArea.Right - rect.Width - TAB_BEZIER_WIDTH
                End If
            End If

            ' Create the tab path
            Dim path As New GraphicsPath()
            path.StartFigure()
            path.AddBezier(rect.X, rect.Bottom, Convert.ToSingle(rect.X + (TAB_BEZIER_WIDTH / 2)), Convert.ToSingle(rect.Bottom - (TAB_BEZIER_WIDTH / 4)), Convert.ToSingle(rect.X + (TAB_BEZIER_WIDTH / 2)), Convert.ToSingle(rect.Top + (TAB_BEZIER_WIDTH / 4)),
            rect.X + TAB_BEZIER_WIDTH, rect.Top)
            path.AddBezier(rect.Right - TAB_BEZIER_WIDTH, rect.Top, Convert.ToSingle(rect.Right - (TAB_BEZIER_WIDTH / 2)), Convert.ToSingle(rect.Top + (TAB_BEZIER_WIDTH / 4)), Convert.ToSingle(rect.Right - (TAB_BEZIER_WIDTH / 2)), Convert.ToSingle(rect.Bottom - (TAB_BEZIER_WIDTH / 4)),
            rect.Right, rect.Bottom)
            path.CloseFigure()

            ' Draw the tab background
            If index = TabPages.SelectedIndex Then
                Utils.DrawPathShadow(g, path, CONTROL_SHADOW_SIZE)
                g.FillPath(CONTENT_BRUSH, path)
                g.DrawPath(OUTLINE_PEN, path)
                g.DrawLine(New Pen(CONTENT_BRUSH), rect.Left + 1.0F, rect.Bottom, rect.Right - 1.0F, rect.Bottom)
                g.FillRectangle(CONTENT_BRUSH, rect.Left + 1.0F, rect.Bottom, rect.Width - 2.0F, CONTROL_SHADOW_SIZE + 1)
            ElseIf index = HoverTabIndex Then
                Dim _mode As CompositingMode = g.CompositingMode
                g.CompositingMode = CompositingMode.SourceCopy
                g.FillPath(HOVER_BRUSH, path)
                g.CompositingMode = _mode
                g.DrawPath(UNSELECTED_OUTLINE_PEN, path)
            Else
                Dim _mode As CompositingMode = g.CompositingMode
                g.CompositingMode = CompositingMode.SourceCopy
                g.FillPath(UNSELECTED_BRUSH, path)
                g.CompositingMode = _mode
                g.DrawPath(UNSELECTED_OUTLINE_PEN, path)
                Dim path2 As GraphicsPath = DirectCast(path.Clone(), GraphicsPath)
                Utils.MovePath(path2, 0, 1)
                g.DrawPath(UNSELECTED_WHITE_ACCENT, path2)
            End If

            ' Setup the offset modifiers
            Dim inside_padding As Integer = 5
            Dim inside_box As New RectangleF(rect.X + TAB_BEZIER_WIDTH + 3, rect.Y + (rect.Height / 2) - 8, rect.Width - (TAB_BEZIER_WIDTH * 2) - 6, 16)
            Dim left_modifier As Single = 0
            Dim right_modifier As Single = 0

            g.InterpolationMode = InterpolationMode.HighQualityBicubic

            ' If pinned and its currently not shrinking
            If TabPages(index).Pinned AndAlso TabPages(index).Animator.Working = False Then
                If TabPages(index).Image Is Nothing Then
                    Return
                End If
                If index = TabPages.SelectedIndex Then
                    g.DrawImage(TabPages(index).Image16, inside_box.X + (inside_box.Width / 2) - 8, inside_box.Y, 16, 16)
                ElseIf index = HoverTabIndex Then
                    g.DrawImage(TabPages(index).Image16, inside_box.X + (inside_box.Width / 2) - 8, inside_box.Y, 16, 16)
                Else
                    g.DrawImage(TabPages(index).Image16Transparent, inside_box.X + (inside_box.Width / 2) - 8, inside_box.Y, 16, 16)
                End If
                Return
            End If

            ' Draw the image if it has one
            If inside_box.Width > 10 Then
                If (TabPages(index).Image IsNot Nothing) Then
                    If index = TabPages.SelectedIndex Then
                        g.DrawImage(TabPages(index).Image16, inside_box.X + 1, inside_box.Y, 16, 16)
                    ElseIf index = HoverTabIndex Then
                        g.DrawImage(TabPages(index).Image16, inside_box.X + 1, inside_box.Y, 16, 16)
                    Else
                        g.DrawImage(TabPages(index).Image16Transparent, inside_box.X + 1, inside_box.Y, 16, 16)
                    End If
                    left_modifier += 16 + inside_padding
                End If
            End If

            ' Draw the close button
            If TabPages(index).CanClose = True AndAlso TabPages(index).Animator.Working = False Then
                If HoverTabCloseIndex = index Then
                    If index = TabPages.SelectedIndex Then
                        If HoverTabCloseDownIndex = index Then
                            g.DrawImage(CLOSE_BUTTON_IMAGES(2), inside_box.Right - 14, inside_box.Y, 16, 16)
                        Else
                            g.DrawImage(CLOSE_BUTTON_IMAGES(1), inside_box.Right - 14, inside_box.Y, 16, 16)
                        End If
                    Else
                        If HoverTabCloseDownIndex = index Then
                            g.DrawImage(CLOSE_BUTTON_IMAGES(4), inside_box.Right - 14, inside_box.Y, 16, 16)
                        Else
                            g.DrawImage(CLOSE_BUTTON_IMAGES(3), inside_box.Right - 14, inside_box.Y, 16, 16)
                        End If
                    End If
                Else
                    g.DrawImage(CLOSE_BUTTON_IMAGES(0), inside_box.Right - 14, inside_box.Y, 16, 16)
                End If
                right_modifier += 8 + inside_padding
            End If

            'Draw the text
            inside_box.X += left_modifier
            inside_box.Width -= left_modifier
            inside_box.Width -= right_modifier
            If inside_box.Width > 5 Then
                If Not String.IsNullOrEmpty(TabPages(index).Title) Then
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias
                    If HoverTabIndex = index OrElse TabPages.SelectedIndex = index Then
                        g.DrawString(TabPages(index).Title, TABSTRIP_FONT, Brushes.Black, inside_box)
                    Else
                        g.DrawString(TabPages(index).Title, TABSTRIP_FONT, TABSTRIP_FONT_BRUSH, inside_box)
                    End If
                End If
            End If

        End Sub

#End Region

#Region "Mouse Variables"

        Private _mouseIsDown As Boolean = False
        Private _mouseWasDown As Boolean = False
        Private _hoverTabIndex As Integer = -1
        Private Property HoverTabIndex() As Integer
            Get
                Return _hoverTabIndex
            End Get
            Set
                If Value < -1 Then
                    Value = -1
                End If
                Dim flag As Boolean = Value <> _hoverTabIndex
                _hoverTabIndex = Value
                If flag Then
                    Invalidate()
                End If
            End Set
        End Property
        Private _hoverTabCloseIndex As Integer = -1
        Private Property HoverTabCloseIndex() As Integer
            Get
                Return _hoverTabCloseIndex
            End Get
            Set
                If Value < -1 Then
                    Value = -1
                End If
                If Value <> HoverTabIndex Then
                    Value = -1
                End If
                Dim flag As Boolean = Value <> _hoverTabCloseIndex
                _hoverTabCloseIndex = Value
                If flag Then
                    Invalidate()
                End If
            End Set
        End Property
        Private _hoverTabCloseDownIndex As Integer = -1
        Private Property HoverTabCloseDownIndex() As Integer
            Get
                Return _hoverTabCloseDownIndex
            End Get
            Set
                If Value < -1 Then
                    Value = -1
                End If
                Dim flag As Boolean = Value <> _hoverTabCloseDownIndex
                _hoverTabCloseDownIndex = Value
                If flag Then
                    Invalidate()
                End If
            End Set
        End Property
        Private _hoverTrayIndex As Integer = -1
        Private Property HoverTrayIndex() As Integer
            Get
                Return _hoverTrayIndex
            End Get
            Set
                If Value < -1 Then
                    Value = -1
                End If
                Dim flag As Boolean = Value <> _hoverTrayIndex
                _hoverTrayIndex = Value
                If flag Then
                    Invalidate()
                End If
            End Set
        End Property
        Private _hoverTrayDownIndex As Integer = -1
        Private Property HoverTrayDownIndex() As Integer
            Get
                Return _hoverTrayDownIndex
            End Get
            Set
                If Value < -1 Then
                    Value = -1
                End If
                Dim flag As Boolean = Value <> _hoverTrayDownIndex
                _hoverTrayDownIndex = Value
                If flag Then
                    Invalidate()
                End If
            End Set
        End Property

#End Region

#Region "Mouse Tracking Methods"

        ''' <summary>
        ''' Returns the tab page index given a point on the screen.
        ''' </summary>
        ''' <param name="pnt">The point on the screen.</param>
        ''' <returns>The index of the tab that the point is over.</returns>
        Private Function PointToTabIndex(pnt As PointF) As Integer
            If _TabstripArea.Contains(pnt) = False Then
                Return -1
            End If
            Dim _subtractor As Single = 0
            For i As Integer = 0 To _TabPages.Count - 1
                Dim _thisSubtractor As Single = 0
                If (_TabPages(i).TabWidth > -1) Then
                    _thisSubtractor = _TabWidth - _TabPages(i).TabWidth
                End If
                If New RectangleF(_TabstripArea.X + (_TabWidth * i) + (TAB_SPACING * i) - _subtractor - (TAB_BEZIER_WIDTH / 2), _TabstripArea.Y, _TabWidth - _thisSubtractor, _TabstripArea.Height).Contains(pnt) Then
                    Return i
                End If
                If (_TabPages(i).TabWidth > -1) Then
                    _subtractor += _thisSubtractor
                End If
            Next
            Return -1
        End Function

        ''' <summary>
        ''' Causes the mouse move event to be triggered without the mouse having to be moved.
        ''' </summary>
        Friend Sub ReclipMouse()
            ' Call a "Mouse Move" event without having the user move the mouse
            Dim pnt As Point = PointToClient(MousePosition)
            OnMouseMove(New MouseEventArgs(System.Windows.Forms.MouseButtons.None, 0, pnt.X, pnt.Y, 0))
        End Sub

        ' Used for animating the currently dragged item
        Private dragStartOffset As Single = 0
        Private dragIndex As Integer = -1
        Protected Overrides Sub OnMouseMove(e As System.Windows.Forms.MouseEventArgs)
            MyBase.OnMouseMove(e)

            'Check the tabs
            Dim newTabStripArea As New RectangleF(_TabstripArea.Location, _TabstripArea.Size)
            If NewTabButton Then
                newTabStripArea.Width += 40
            End If
            If newTabStripArea.Contains(e.Location) Then
                Dim newPoint As PointF = e.Location
                Dim totalSubtractor As Single = 0
                Dim changed As Boolean = False
                Dim rects As New List(Of RectangleF)()
                For i As Integer = 0 To _TabPages.Count - 1
                    Dim thisSubtractor As Single = 0
                    If _TabPages(i).TabWidth > -1 Then
                        thisSubtractor = _TabWidth - _TabPages(i).TabWidth
                    End If
                    Dim viewRect As New RectangleF(_TabstripArea.X + (_TabWidth * i) + (TAB_SPACING * i) - totalSubtractor, _TabstripArea.Y, _TabWidth - thisSubtractor, _TabstripArea.Height)
                    Dim clipRect As New RectangleF(_TabstripArea.X + (_TabWidth * i) + (TAB_SPACING * i) - totalSubtractor - (TAB_BEZIER_WIDTH / 2), _TabstripArea.Y, _TabWidth - thisSubtractor, _TabstripArea.Height)
                    rects.Add(viewRect)
                    If clipRect.Contains(newPoint) Then
                        HoverTabIndex = i

                        'Get the box for the close button and check it
                        Dim x As Single = (_TabstripArea.X + (_TabWidth * i) + (TAB_SPACING * i) - totalSubtractor) + TAB_BEZIER_WIDTH + 3 + (TabWidth - thisSubtractor - (TAB_BEZIER_WIDTH * 2) - 6) - 14
                        Dim close_box As New RectangleF(x, _TabstripArea.Y + (_TabstripArea.Height / 2) - 8, 16, 16)
                        If close_box.Contains(e.Location) AndAlso TabPages(i).Pinned = False AndAlso TabPages(i).CanClose Then
                            HoverTabCloseIndex = i
                        Else
                            HoverTabCloseIndex = -1
                        End If

                        'Drag, make sure were not hovering over the close button and that a close button anywhere isnt pressed
                        'And _mouseDownTrayIndex = -1 Then
                        If _hoverTabCloseIndex <> i AndAlso HoverTabCloseDownIndex = -1 AndAlso TabPages.SelectedIndex = i AndAlso HoverTrayDownIndex = -1 Then
                            If e.Button = System.Windows.Forms.MouseButtons.Left AndAlso _mouseIsDown AndAlso _mouseWasDown Then
                                If TabPages(i).TabDraggable AndAlso TabPages(i).TabSelectable AndAlso TabPages.Count > 1 Then
                                    ' Set the drag index and offset from the mouse
                                    dragIndex = i
                                    dragStartOffset = newPoint.X - clipRect.X

                                    ' Let's not show the new tab button if we're showing it
                                    Dim before As Boolean = _newTabButton
                                    NewTabButton = False

                                    ' Do the drag
                                    DoDragDrop(New TabPage.DragShell(TabPages(i)), DragDropEffects.All)

                                    ' Reset the index and the tab button
                                    NewTabButton = before
                                    dragIndex = -1
                                End If
                            End If
                        End If

                        changed = True
                        Exit For
                    End If
                    totalSubtractor += thisSubtractor
                Next

                If Not changed AndAlso _newTabButton Then
                    Dim newTabButtonRect As RectangleF = GetNewTabButtonRect(rects)
                    newTabButtonRect.X += 6
                    ' make up for the skew
                    If newTabButtonRect.Contains(newPoint) Then
                        _hoverTabIndex = _TabPages.Count
                        changed = True
                    End If
                End If

                'Change them in bulk
                If Not changed Then
                    _hoverTabIndex = -1
                    _hoverTabCloseIndex = -1
                End If
                _hoverTrayIndex = -1

                Invalidate()
            ElseIf _trayArea.Contains(e.Location) Then
                If _dontUpdateTabWidth Then
                    _dontUpdateTabWidth = False
                    UpdateAreas()
                End If

                Dim changed As Boolean = False
                Dim rect As New RectangleF(_trayArea.Right - TRAY_ITEM_SIZE, _trayArea.Y, TRAY_ITEM_SIZE, TRAY_ITEM_SIZE)
                For i As Integer = 0 To TrayItems.Count - 1
                    If rect.Contains(e.Location) Then
                        _hoverTrayIndex = i
                        changed = True
                        Exit For
                    End If
                    rect.X -= (TRAY_ITEM_SIZE + TRAY_ITEM_SPACING)
                Next
                If Not changed Then
                    _hoverTrayIndex = -1
                End If
                _hoverTabIndex = -1
                _hoverTabCloseIndex = -1
                Invalidate()
            Else
                If _dontUpdateTabWidth Then
                    _dontUpdateTabWidth = False
                    UpdateAreas()
                End If

                ' Not over the tabs or tray
                _hoverTabIndex = -1
                _hoverTabCloseIndex = -1
                _hoverTrayIndex = -1
                Invalidate()
            End If

            ' Update the mouse variables
            If _mouseIsDown = True AndAlso _mouseWasDown = False Then
                _mouseWasDown = True
            End If

        End Sub

        Protected Overrides Sub OnMouseDown(e As System.Windows.Forms.MouseEventArgs)
            MyBase.OnMouseDown(e)

            ' Only deal with the left button
            If e.Button <> System.Windows.Forms.MouseButtons.Left Then
                Return
            End If

            ' Check if something is being hovered
            If HoverTabIndex > -1 AndAlso HoverTabIndex < _TabPages.Count Then
                If HoverTabCloseIndex = HoverTabIndex Then
                    HoverTabCloseDownIndex = HoverTabCloseIndex
                Else
                    TabPages.SelectedIndex = HoverTabIndex
                    HoverTabCloseDownIndex = -1
                End If
            ElseIf HoverTabIndex = _TabPages.Count Then
                HoverTabCloseDownIndex = _TabPages.Count
            ElseIf HoverTrayIndex > -1 Then
                HoverTrayDownIndex = HoverTrayIndex
                ' Do nothing
            ElseIf _ContentArea.Contains(e.Location) Then
            Else
                Try
                    If Me.Parent IsNot Nothing Then
                        If TypeOf Me.Parent Is Form Then
                            ChromeTabControl.ReleaseCapture()
                            ChromeTabControl.SendMessage(Me.Parent.Handle, &HA1, 2, 0)
                        End If
                    End If
                Catch
                End Try
            End If

            'Update the mouse down boolean
            _mouseIsDown = True

        End Sub

        Protected Overrides Sub OnMouseUp(e As System.Windows.Forms.MouseEventArgs)
            MyBase.OnMouseUp(e)

            ' Check for a right click
            If e.Button = System.Windows.Forms.MouseButtons.Right Then
                If _TabstripArea.Contains(e.Location) Then
                    ContextMenuStrip.Show(MousePosition)
                End If
                Return
            End If

            'Deal only with the left button
            If e.Button <> System.Windows.Forms.MouseButtons.Left Then
                Return
            End If

            ' Check to see if we clicked the new tab button
            If _hoverTabIndex = _TabPages.Count AndAlso NewTabButton Then
                RaiseEvent NewTabClicked(Me, New EventArgs())
            End If

            If HoverTabCloseDownIndex > -1 AndAlso HoverTabCloseIndex = HoverTabCloseDownIndex AndAlso HoverTabCloseIndex < _TabPages.Count AndAlso HoverTabCloseDownIndex < _TabPages.Count Then
                If _TabPages(HoverTabCloseDownIndex).CanClose Then
                    If HoverTabCloseDownIndex < _TabPages.Count - 1 Then
                        _dontUpdateTabWidth = True
                    End If
                    TabPages.RemoveAt(HoverTabCloseDownIndex)
                End If
            End If
            If HoverTrayDownIndex > -1 AndAlso HoverTrayIndex = HoverTrayDownIndex AndAlso HoverTrayIndex < _TrayItems.Count Then
                Dim rect As New RectangleF(_trayArea.Right - (HoverTrayDownIndex * (TRAY_ITEM_SIZE + TRAY_ITEM_SPACING)) - TRAY_ITEM_SIZE, _trayArea.Y, TRAY_ITEM_SIZE, TRAY_ITEM_SIZE)
                TrayItems(HoverTrayDownIndex).RaiseClicked(rect)
            End If

            HoverTrayDownIndex = -1
            HoverTabCloseDownIndex = -1

            'Update the mouse variables
            _mouseIsDown = False
            _mouseWasDown = False

        End Sub

        Protected Overrides Sub OnMouseLeave(e As System.EventArgs)
            MyBase.OnMouseLeave(e)
            If _dontUpdateTabWidth Then
                _dontUpdateTabWidth = False
                UpdateAreas()
            End If
            _hoverTabCloseIndex = -1
            _hoverTabIndex = -1
            _hoverTrayIndex = -1
            Invalidate()
        End Sub

        Protected Overrides Sub OnDragOver(drgevent As System.Windows.Forms.DragEventArgs)
            MyBase.OnDragOver(drgevent)

            'Moving tabs around
            If (drgevent.Data.GetDataPresent(GetType(TabPage.DragShell))) Then
                'Make sure we're dragging over the tab area
                Dim drag_point As Point = PointToClient(New Point(drgevent.X, drgevent.Y))
                If _TabstripArea.Contains(drag_point) = False Then
                    ' Let's see if we're close, to give the 
                    If _TabstripArea.Contains(New Point(drag_point.X, drag_point.Y + 10)) Then
                        drag_point.Y += 10
                    ElseIf _TabstripArea.Contains(New Point(drag_point.X, drag_point.Y - 10)) Then
                        drag_point.Y -= 10
                    Else
                        drgevent.Effect = DragDropEffects.None
                        Return
                    End If
                End If

                'Get the dragged tab
                Dim drag_tab As TabPage = DirectCast(drgevent.Data.GetData(GetType(TabPage.DragShell)), TabPage.DragShell).Tab

                'Check if the managers equal
                If Not Me.Equals(drag_tab._tabControl) Then
                    drgevent.Effect = DragDropEffects.None
                Else
                    drgevent.Effect = DragDropEffects.Move
                    Dim drag_index As Integer = TabPages.IndexOf(drag_tab)
                    If drag_index < 0 Then
                        Return
                    End If
                    Dim drop_index As Integer = PointToTabIndex(drag_point)
                    If drop_index < 0 Then
                        drgevent.Effect = DragDropEffects.None
                        Return
                    End If
                    If TabPages(drop_index).TabDraggable = False Then
                        drgevent.Effect = DragDropEffects.None
                        Return
                    End If
                    'allow to switch pins only with other pins
                    If drag_tab.Pinned <> TabPages(drop_index).Pinned Then
                        drgevent.Effect = DragDropEffects.None
                        Return
                    End If
                    If drag_index <> drop_index Then
                        TabPages.MoveItem(drag_index, drop_index)

                        ' We need to change the index of the drag to show the correct item drag
                        dragIndex = drop_index
                    End If
                End If
            End If

            ' Invalidate to show any changes to the drag animation
            Invalidate()

        End Sub
#End Region
    End Class
End Namespace