Imports System.ComponentModel
Imports System.Drawing.Drawing2D
Imports Microsoft.VisualStudio.WinForms.Docking

Namespace ThemeVS2012
    <ToolboxItem(False)>
    Friend Class VS2012AutoHideStrip
        Inherits AutoHideStripBase
        Private Class TabVS2012
            Inherits Tab
            Friend Sub New(content As IDockContent)
                MyBase.New(content)
            End Sub

            ''' <summary>
            ''' X for this <see href="TabVS2012"/> inside the logical strip rectangle.
            ''' </summary>
            Public Property TabX As Integer

            ''' <summary>
            ''' Width of this <see href="TabVS2012"/>.
            ''' </summary>
            Public Property TabWidth As Integer

            Public Property IsMouseOver As Boolean
        End Class

        Private Const TextGapLeft As Integer = 0
        Private Const TextGapRight As Integer = 0
        Private Const TextGapBottom As Integer = 3
        Private Const TabGapTop As Integer = 3
        Private Const TabGapBottom As Integer = 8
        Private Const TabGapLeft As Integer = 0
        Private Const TabGapBetween As Integer = 12

#Region "Customizable Properties"
        Public ReadOnly Property TextFont As Font
            Get
                Return DockPanel.Theme.Skin.AutoHideStripSkin.TextFont
            End Get
        End Property

        Private Shared _stringFormatTabHorizontal As StringFormat
        Private ReadOnly Property StringFormatTabHorizontal As StringFormat
            Get
                If _stringFormatTabHorizontal Is Nothing Then
                    _stringFormatTabHorizontal = New StringFormat()
                    _stringFormatTabHorizontal.Alignment = StringAlignment.Near
                    _stringFormatTabHorizontal.LineAlignment = StringAlignment.Center
                    _stringFormatTabHorizontal.FormatFlags = StringFormatFlags.NoWrap
                    _stringFormatTabHorizontal.Trimming = StringTrimming.None
                End If

                If MyBase.RightToLeft = RightToLeft.Yes Then
                    _stringFormatTabHorizontal.FormatFlags = _stringFormatTabHorizontal.FormatFlags Or StringFormatFlags.DirectionRightToLeft
                Else
                    _stringFormatTabHorizontal.FormatFlags = _stringFormatTabHorizontal.FormatFlags And Not StringFormatFlags.DirectionRightToLeft
                End If

                Return _stringFormatTabHorizontal
            End Get
        End Property

        Private Shared _stringFormatTabVertical As StringFormat
        Private ReadOnly Property StringFormatTabVertical As StringFormat
            Get
                If _stringFormatTabVertical Is Nothing Then
                    _stringFormatTabVertical = New StringFormat()
                    _stringFormatTabVertical.Alignment = StringAlignment.Near
                    _stringFormatTabVertical.LineAlignment = StringAlignment.Center
                    _stringFormatTabVertical.FormatFlags = StringFormatFlags.NoWrap Or StringFormatFlags.DirectionVertical
                    _stringFormatTabVertical.Trimming = StringTrimming.None
                End If
                If MyBase.RightToLeft = RightToLeft.Yes Then
                    _stringFormatTabVertical.FormatFlags = _stringFormatTabVertical.FormatFlags Or StringFormatFlags.DirectionRightToLeft
                Else
                    _stringFormatTabVertical.FormatFlags = _stringFormatTabVertical.FormatFlags And Not StringFormatFlags.DirectionRightToLeft
                End If

                Return _stringFormatTabVertical
            End Get
        End Property

#End Region

        Private Shared _matrixIdentity As Matrix = New Matrix()
        Private Shared ReadOnly Property MatrixIdentity As Matrix
            Get
                Return _matrixIdentity
            End Get
        End Property

        Private Shared _dockStates As DockState()
        Private Shared ReadOnly Property DockStates As DockState()
            Get
                If _dockStates Is Nothing Then
                    _dockStates = New DockState(3) {}
                    _dockStates(0) = DockState.DockLeftAutoHide
                    _dockStates(1) = DockState.DockRightAutoHide
                    _dockStates(2) = DockState.DockTopAutoHide
                    _dockStates(3) = DockState.DockBottomAutoHide
                End If
                Return _dockStates
            End Get
        End Property

        Private Shared _graphicsPath As GraphicsPath
        Friend Shared ReadOnly Property GraphicsPath As GraphicsPath
            Get
                If _graphicsPath Is Nothing Then _graphicsPath = New GraphicsPath()

                Return _graphicsPath
            End Get
        End Property

        Public Sub New(panel As DockPanel)
            MyBase.New(panel)
            SetStyle(ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer, True)
            MyBase.BackColor = DockPanel.Theme.ColorPalette.MainWindowActive.Background
        End Sub

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            MyBase.OnPaint(e)
            Dim g = e.Graphics
            DrawTabStrip(g)
        End Sub

        Protected Overrides Sub OnLayout(levent As LayoutEventArgs)
            CalculateTabs()
            MyBase.OnLayout(levent)
        End Sub

        Private Sub DrawTabStrip(g As Graphics)
            DrawTabStrip(g, DockState.DockTopAutoHide)
            DrawTabStrip(g, DockState.DockBottomAutoHide)
            DrawTabStrip(g, DockState.DockLeftAutoHide)
            DrawTabStrip(g, DockState.DockRightAutoHide)
        End Sub

        Private Sub DrawTabStrip(g As Graphics, dockState As DockState)
            Dim rectTabStrip = GetLogicalTabStripRectangle(dockState)

            If rectTabStrip.IsEmpty Then Return

            Dim matrixIdentity = g.Transform
            If dockState = DockState.DockLeftAutoHide OrElse dockState = DockState.DockRightAutoHide Then
                Dim matrixRotated As Matrix = New Matrix()
                matrixRotated.RotateAt(90, New PointF(rectTabStrip.X + CSng(rectTabStrip.Height) / 2, rectTabStrip.Y + CSng(rectTabStrip.Height) / 2))
                g.Transform = matrixRotated
            End If

            For Each pane In GetPanes(dockState)
                For Each tab As TabVS2012 In pane.AutoHideTabs
                    DrawTab(g, tab)
                Next
            Next

            g.Transform = matrixIdentity
        End Sub

        Private Sub CalculateTabs()
            CalculateTabs(DockState.DockTopAutoHide)
            CalculateTabs(DockState.DockBottomAutoHide)
            CalculateTabs(DockState.DockLeftAutoHide)
            CalculateTabs(DockState.DockRightAutoHide)
        End Sub

        Private Sub CalculateTabs(dockState As DockState)
            Dim rectTabStrip = GetLogicalTabStripRectangle(dockState)

            Dim x = TabGapLeft + rectTabStrip.X
            For Each pane In GetPanes(dockState)
                For Each tab As TabVS2012 In pane.AutoHideTabs
                    Dim width = TextRenderer.MeasureText(tab.Content.DockHandler.TabText, TextFont).Width + TextGapLeft + TextGapRight
                    tab.TabX = x
                    tab.TabWidth = width
                    x += width + TabGapBetween
                Next
            Next
        End Sub

        Private Function RtlTransform(rect As Rectangle, dockState As DockState) As Rectangle
            Dim rectTransformed As Rectangle
            If dockState = DockState.DockLeftAutoHide OrElse dockState = DockState.DockRightAutoHide Then
                rectTransformed = rect
            Else
                rectTransformed = DrawHelper.RtlTransform(Me, rect)
            End If

            Return rectTransformed
        End Function

        Private Function GetTabOutline(tab As TabVS2012, rtlTransform As Boolean) As GraphicsPath
            Dim dockState = tab.Content.DockHandler.DockState
            Dim rectTab = GetTabRectangle(tab)
            If rtlTransform Then rectTab = Me.RtlTransform(rectTab, dockState)

            If GraphicsPath IsNot Nothing Then
                Call GraphicsPath.Reset()
                GraphicsPath.AddRectangle(rectTab)
            End If

            Return GraphicsPath
        End Function

        Private Sub DrawTab(g As Graphics, tab As TabVS2012)
            Dim rectTabOrigin = GetTabRectangle(tab)
            If rectTabOrigin.IsEmpty Then Return

            Dim dockState = tab.Content.DockHandler.DockState
            Dim content = tab.Content

            'Set no rotate for drawing icon and text
            Dim matrixRotate = g.Transform
            g.Transform = MatrixIdentity

            Dim borderColor As Color
            Dim backgroundColor As Color
            Dim textColor As Color
            If tab.IsMouseOver Then
                borderColor = DockPanel.Theme.ColorPalette.AutoHideStripHovered.Border
                backgroundColor = DockPanel.Theme.ColorPalette.AutoHideStripHovered.Background
                textColor = DockPanel.Theme.ColorPalette.AutoHideStripHovered.Text
            Else
                borderColor = DockPanel.Theme.ColorPalette.AutoHideStripDefault.Border
                backgroundColor = DockPanel.Theme.ColorPalette.AutoHideStripDefault.Background
                textColor = DockPanel.Theme.ColorPalette.AutoHideStripDefault.Text
            End If

            g.FillRectangle(DockPanel.Theme.PaintingService.GetBrush(backgroundColor), rectTabOrigin)

            Dim rectBorder = GetBorderRectangle(rectTabOrigin, dockState, TextRenderer.MeasureText(tab.Content.DockHandler.TabText, TextFont).Width)
            g.FillRectangle(DockPanel.Theme.PaintingService.GetBrush(borderColor), rectBorder)

            ' Draw the text
            Dim rectText = GetTextRectangle(rectTabOrigin, dockState)

            If dockState = DockState.DockLeftAutoHide OrElse dockState = DockState.DockRightAutoHide Then
                g.DrawString(content.DockHandler.TabText, TextFont, DockPanel.Theme.PaintingService.GetBrush(textColor), rectText, StringFormatTabVertical)
            Else
                g.DrawString(content.DockHandler.TabText, TextFont, DockPanel.Theme.PaintingService.GetBrush(textColor), rectText, StringFormatTabHorizontal)
            End If

            ' Set rotate back
            g.Transform = matrixRotate
        End Sub

        Private Function GetBorderRectangle(tab As Rectangle, state As DockState, width As Integer) As Rectangle
            Dim result = New Rectangle(tab.Location, tab.Size)
            If state = DockState.DockLeftAutoHide Then
                result.Height = width
                result.Width = DockPanel.Theme.Measures.AutoHideTabLineWidth
                result.Y += TextGapLeft
                Return result
            End If

            If state = DockState.DockRightAutoHide Then
                result.Height = width
                result.Width = DockPanel.Theme.Measures.AutoHideTabLineWidth
                result.X += tab.Width - result.Width
                result.Y += TextGapLeft
                Return result
            End If

            If state = DockState.DockBottomAutoHide Then
                result.Width = width
                result.Height = DockPanel.Theme.Measures.AutoHideTabLineWidth
                result.X += TextGapLeft
                result.Y += tab.Height - result.Height
                Return result
            End If

            If state = DockState.DockTopAutoHide Then
                result.Width = width
                result.Height = DockPanel.Theme.Measures.AutoHideTabLineWidth
                result.X += TextGapLeft
                Return result
            End If

            Return Rectangle.Empty
        End Function

        Public Function GetLogicalTabStripRectangle(state As DockState) As Rectangle
            Dim rectStrip = GetTabStripRectangle(state)
            Dim location = rectStrip.Location
            If state = DockState.DockLeftAutoHide OrElse state = DockState.DockRightAutoHide Then
                Return New Rectangle(0, 0, rectStrip.Height, rectStrip.Width)
            End If

            Return New Rectangle(0, 0, rectStrip.Width, rectStrip.Height)
        End Function

        Private Function GetTabRectangle(tab As TabVS2012) As Rectangle
            Dim state = tab.Content.DockHandler.DockState
            Dim rectStrip = GetTabStripRectangle(state)
            Dim location = rectStrip.Location
            If state = DockState.DockLeftAutoHide OrElse state = DockState.DockRightAutoHide Then
                location.Y += tab.TabX
                Return New Rectangle(location.X, location.Y, rectStrip.Width, tab.TabWidth)
            End If

            location.X += tab.TabX
            Return New Rectangle(location.X, location.Y, tab.TabWidth, rectStrip.Height)
        End Function

        Private Function GetTextRectangle(tab As Rectangle, state As DockState) As Rectangle
            Dim result = New Rectangle(tab.Location, tab.Size)
            If state = DockState.DockLeftAutoHide Then
                result.X += TextGapBottom
                result.Y += TextGapLeft
                result.Height -= TextGapLeft + TextGapRight
                result.Width -= TextGapBottom
                Return result
            End If

            If state = DockState.DockRightAutoHide Then
                result.Y += TextGapLeft
                result.Height -= TextGapLeft + TextGapRight
                result.Width -= TextGapBottom
                Return result
            End If

            If state = DockState.DockBottomAutoHide Then
                result.X += TextGapLeft
                result.Width -= TextGapLeft + TextGapRight
                result.Height -= TextGapBottom
                Return result
            End If

            If state = DockState.DockTopAutoHide Then
                result.X += TextGapLeft
                result.Y += TextGapBottom
                result.Width -= TextGapLeft + TextGapRight
                result.Height -= TextGapBottom
                Return result
            End If

            Return Rectangle.Empty
        End Function

        Protected Overrides Function HitTest(point As Point) As IDockContent
            Dim tab = TabHitTest(point)

            If tab IsNot Nothing Then
                Return tab.Content
            Else
                Return Nothing
            End If
        End Function

        Protected Overrides Function GetTabBounds(tab As Tab) As Rectangle
            Dim path = GetTabOutline(CType(tab, TabVS2012), True)
            Dim bounds As RectangleF = path.GetBounds()
            Return New Rectangle(bounds.Left, bounds.Top, bounds.Width, bounds.Height)
        End Function

        Protected Function TabHitTest(ptMouse As Point) As Tab
            For Each state In DockStates
                Dim rectTabStrip = GetTabStripRectangle(state)
                If Not rectTabStrip.Contains(ptMouse) Then Continue For

                For Each pane In GetPanes(state)
                    For Each tab As TabVS2012 In pane.AutoHideTabs
                        Dim path = GetTabOutline(tab, True)
                        If path.IsVisible(ptMouse) Then Return tab
                    Next
                Next
            Next

            Return Nothing
        End Function

        Private lastSelectedTab As TabVS2012 = Nothing

        Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
            MyBase.OnMouseMove(e)
            Dim tab = CType(TabHitTest(PointToClient(MousePosition)), TabVS2012)
            If tab IsNot Nothing Then
                tab.IsMouseOver = True
                Invalidate()
            End If

            If lastSelectedTab IsNot tab Then
                If lastSelectedTab IsNot Nothing Then
                    lastSelectedTab.IsMouseOver = False
                    Invalidate()
                End If

                lastSelectedTab = tab
            End If
        End Sub

        Protected Overrides Sub OnMouseLeave(e As EventArgs)
            MyBase.OnMouseLeave(e)

            If lastSelectedTab IsNot Nothing Then lastSelectedTab.IsMouseOver = False
            Invalidate()
        End Sub

        Protected Overrides Function MeasureHeight() As Integer
            Return 31
        End Function

        Protected Overrides Sub OnRefreshChanges()
            CalculateTabs()
            Invalidate()
        End Sub

        Protected Overrides Function CreateTab(content As IDockContent) As Tab
            Return New TabVS2012(content)
        End Function
    End Class
End Namespace
