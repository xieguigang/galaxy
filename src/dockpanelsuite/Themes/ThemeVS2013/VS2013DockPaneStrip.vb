Imports System.ComponentModel
Imports System.Drawing.Drawing2D
Imports ThemeVS2015.WeifenLuo.WinFormsUI.Docking
Imports WeifenLuo.WinFormsUI.Docking

Namespace WeifenLuo.WinFormsUI.ThemeVS2013
    <ToolboxItem(False)>
    Friend Class VS2013DockPaneStrip
        Inherits DockPaneStripBase
        Private Class TabVS2013
            Inherits Tab
            Public Sub New(content As IDockContent)
                MyBase.New(content)
            End Sub

            Private m_tabX As Integer
            Public Property TabX As Integer
                Get
                    Return m_tabX
                End Get
                Set(value As Integer)
                    m_tabX = value
                End Set
            End Property

            Private m_tabWidth As Integer
            Public Property TabWidth As Integer
                Get
                    Return m_tabWidth
                End Get
                Set(value As Integer)
                    m_tabWidth = value
                End Set
            End Property

            Private m_maxWidth As Integer
            Public Property MaxWidth As Integer
                Get
                    Return m_maxWidth
                End Get
                Set(value As Integer)
                    m_maxWidth = value
                End Set
            End Property

            Private m_flag As Boolean
            Protected Friend Property Flag As Boolean
                Get
                    Return m_flag
                End Get
                Set(value As Boolean)
                    m_flag = value
                End Set
            End Property
        End Class

        Protected Overrides Function CreateTab(content As IDockContent) As Tab
            Return New TabVS2013(content)
        End Function

        <ToolboxItem(False)>
        Private NotInheritable Class InertButton
            Inherits InertButtonBase
            Private _hovered, _normal, _pressed As Bitmap

            Public Sub New(hovered As Bitmap, normal As Bitmap, pressed As Bitmap)
                MyBase.New()
                _hovered = hovered
                _normal = normal
                _pressed = pressed
            End Sub

            Public Overrides ReadOnly Property Image As Bitmap
                Get
                    Return _normal
                End Get
            End Property

            Public Overrides ReadOnly Property HoverImage As Bitmap
                Get
                    Return _hovered
                End Get
            End Property

            Public Overrides ReadOnly Property PressImage As Bitmap
                Get
                    Return _pressed
                End Get
            End Property
        End Class

#Region "Constants"

        Private Const _ToolWindowStripGapTop As Integer = 0
        Private Const _ToolWindowStripGapBottom As Integer = 0
        Private Const _ToolWindowStripGapLeft As Integer = 0
        Private Const _ToolWindowStripGapRight As Integer = 0
        Private Const _ToolWindowImageHeight As Integer = 16
        Private Const _ToolWindowImageWidth As Integer = 0 '16;
        Private Const _ToolWindowImageGapTop As Integer = 3
        Private Const _ToolWindowImageGapBottom As Integer = 1
        Private Const _ToolWindowImageGapLeft As Integer = 2
        Private Const _ToolWindowImageGapRight As Integer = 0
        Private Const _ToolWindowTextGapRight As Integer = 3
        Private Const _ToolWindowTabSeperatorGapTop As Integer = 3
        Private Const _ToolWindowTabSeperatorGapBottom As Integer = 3

        Private Const _DocumentStripGapTop As Integer = 0
        Private Const _DocumentStripGapBottom As Integer = 1
        Private Const _DocumentTabMaxWidth As Integer = 200
        Private Const _DocumentButtonGapTop As Integer = 3
        Private Const _DocumentButtonGapBottom As Integer = 3
        Private Const _DocumentButtonGapBetween As Integer = 0
        Private Const _DocumentButtonGapRight As Integer = 3
        Private Const _DocumentTabGapTop As Integer = 0 '3;
        Private Const _DocumentTabGapLeft As Integer = 0 '3;
        Private Const _DocumentTabGapRight As Integer = 0 '3;
        Private Const _DocumentIconGapBottom As Integer = 2 '2;
        Private Const _DocumentIconGapLeft As Integer = 8
        Private Const _DocumentIconGapRight As Integer = 0
        Private Const _DocumentIconHeight As Integer = 16
        Private Const _DocumentIconWidth As Integer = 16
        Private Const _DocumentTextGapRight As Integer = 6

#End Region

#Region "Members"

        Private m_selectMenu As ContextMenuStrip
        Private m_buttonOverflow As InertButton
        Private m_buttonWindowList As InertButton
        Private m_components As IContainer
        Private m_toolTip As ToolTip
        Private m_font As Font
        Private m_boldFont As Font
        Private m_startDisplayingTab As Integer = 0
        Private m_endDisplayingTab As Integer = 0
        Private m_firstDisplayingTab As Integer = 0
        Private m_documentTabsOverflow As Boolean = False
        Private Shared m_toolTipSelect As String
        Private _activeClose As Rectangle
        Private _selectMenuMargin As Integer = 5
        Private m_suspendDrag As Boolean = False
#End Region

#Region "Properties"

        Private ReadOnly Property TabStripRectangle As Rectangle
            Get
                If Appearance = DockPane.AppearanceStyle.Document Then
                    Return TabStripRectangle_Document
                Else
                    Return TabStripRectangle_ToolWindow
                End If
            End Get
        End Property

        Private ReadOnly Property TabStripRectangle_ToolWindow As Rectangle
            Get
                Dim rect = ClientRectangle
                Return New Rectangle(rect.X, rect.Top + ToolWindowStripGapTop, rect.Width, rect.Height - ToolWindowStripGapTop - ToolWindowStripGapBottom)
            End Get
        End Property

        Private ReadOnly Property TabStripRectangle_Document As Rectangle
            Get
                Dim rect = ClientRectangle
                Return New Rectangle(rect.X, rect.Top + DocumentStripGapTop, rect.Width, rect.Height + DocumentStripGapTop - DocumentStripGapBottom)
            End Get
        End Property

        Private ReadOnly Property TabsRectangle As Rectangle
            Get
                If Appearance = DockPane.AppearanceStyle.ToolWindow Then Return TabStripRectangle

                Dim rectWindow = TabStripRectangle
                Dim x = rectWindow.X
                Dim y = rectWindow.Y
                Dim width = rectWindow.Width
                Dim height = rectWindow.Height

                x += DocumentTabGapLeft
                width -= DocumentTabGapLeft + DocumentTabGapRight + DocumentButtonGapRight + ButtonOverflow.Width + ButtonWindowList.Width + 2 * DocumentButtonGapBetween

                Return New Rectangle(x, y, width, height)
            End Get
        End Property

        Private ReadOnly Property SelectMenu As ContextMenuStrip
            Get
                Return m_selectMenu
            End Get
        End Property

        Public Property SelectMenuMargin As Integer
            Get
                Return _selectMenuMargin
            End Get
            Set(value As Integer)
                _selectMenuMargin = value
            End Set
        End Property

        Private ReadOnly Property ButtonOverflow As InertButton
            Get
                If m_buttonOverflow Is Nothing Then
                    m_buttonOverflow = New InertButton(DockPane.DockPanel.Theme.ImageService.DockPaneHover_OptionOverflow, DockPane.DockPanel.Theme.ImageService.DockPane_OptionOverflow, DockPane.DockPanel.Theme.ImageService.DockPanePress_OptionOverflow)
                    AddHandler m_buttonOverflow.Click, New EventHandler(AddressOf WindowList_Click)
                    Controls.Add(m_buttonOverflow)
                End If

                Return m_buttonOverflow
            End Get
        End Property

        Private ReadOnly Property ButtonWindowList As InertButton
            Get
                If m_buttonWindowList Is Nothing Then
                    m_buttonWindowList = New InertButton(DockPane.DockPanel.Theme.ImageService.DockPaneHover_List, DockPane.DockPanel.Theme.ImageService.DockPane_List, DockPane.DockPanel.Theme.ImageService.DockPanePress_List)
                    AddHandler m_buttonWindowList.Click, New EventHandler(AddressOf WindowList_Click)
                    Controls.Add(m_buttonWindowList)
                End If

                Return m_buttonWindowList
            End Get
        End Property

        Private Shared ReadOnly Property GraphicsPath As GraphicsPath
            Get
                Return VS2012AutoHideStrip.GraphicsPath
            End Get
        End Property

        Private ReadOnly Property Components As IContainer
            Get
                Return m_components
            End Get
        End Property

        Public ReadOnly Property TextFont As Font
            Get
                Return DockPane.DockPanel.Theme.Skin.DockPaneStripSkin.TextFont
            End Get
        End Property

        Private ReadOnly Property BoldFont As Font
            Get
                If IsDisposed Then Return Nothing

                If m_boldFont Is Nothing Then
                    m_font = TextFont
                    m_boldFont = New Font(TextFont, FontStyle.Bold)
                ElseIf m_font IsNot TextFont Then
                    m_boldFont.Dispose()
                    m_font = TextFont
                    m_boldFont = New Font(TextFont, FontStyle.Bold)
                End If

                Return m_boldFont
            End Get
        End Property

        Private Property StartDisplayingTab As Integer
            Get
                Return m_startDisplayingTab
            End Get
            Set(value As Integer)
                m_startDisplayingTab = value
                Invalidate()
            End Set
        End Property

        Private Property EndDisplayingTab As Integer
            Get
                Return m_endDisplayingTab
            End Get
            Set(value As Integer)
                m_endDisplayingTab = value
            End Set
        End Property

        Private Property FirstDisplayingTab As Integer
            Get
                Return m_firstDisplayingTab
            End Get
            Set(value As Integer)
                m_firstDisplayingTab = value
            End Set
        End Property

        Private WriteOnly Property DocumentTabsOverflow As Boolean
            Set(value As Boolean)
                If m_documentTabsOverflow = value Then Return

                m_documentTabsOverflow = value
                SetInertButtons()
            End Set
        End Property

#Region "Customizable Properties"

        Private Shared ReadOnly Property ToolWindowStripGapTop As Integer
            Get
                Return _ToolWindowStripGapTop
            End Get
        End Property

        Private Shared ReadOnly Property ToolWindowStripGapBottom As Integer
            Get
                Return _ToolWindowStripGapBottom
            End Get
        End Property

        Private Shared ReadOnly Property ToolWindowStripGapLeft As Integer
            Get
                Return _ToolWindowStripGapLeft
            End Get
        End Property

        Private Shared ReadOnly Property ToolWindowStripGapRight As Integer
            Get
                Return _ToolWindowStripGapRight
            End Get
        End Property

        Private Shared ReadOnly Property ToolWindowImageHeight As Integer
            Get
                Return _ToolWindowImageHeight
            End Get
        End Property

        Private Shared ReadOnly Property ToolWindowImageWidth As Integer
            Get
                Return _ToolWindowImageWidth
            End Get
        End Property

        Private Shared ReadOnly Property ToolWindowImageGapTop As Integer
            Get
                Return _ToolWindowImageGapTop
            End Get
        End Property

        Private Shared ReadOnly Property ToolWindowImageGapBottom As Integer
            Get
                Return _ToolWindowImageGapBottom
            End Get
        End Property

        Private Shared ReadOnly Property ToolWindowImageGapLeft As Integer
            Get
                Return _ToolWindowImageGapLeft
            End Get
        End Property

        Private Shared ReadOnly Property ToolWindowImageGapRight As Integer
            Get
                Return _ToolWindowImageGapRight
            End Get
        End Property

        Private Shared ReadOnly Property ToolWindowTextGapRight As Integer
            Get
                Return _ToolWindowTextGapRight
            End Get
        End Property

        Private Shared ReadOnly Property ToolWindowTabSeperatorGapTop As Integer
            Get
                Return _ToolWindowTabSeperatorGapTop
            End Get
        End Property

        Private Shared ReadOnly Property ToolWindowTabSeperatorGapBottom As Integer
            Get
                Return _ToolWindowTabSeperatorGapBottom
            End Get
        End Property

        Private Shared ReadOnly Property ToolTipSelect As String
            Get
                If Equals(m_toolTipSelect, Nothing) Then m_toolTipSelect =  My.Resources.DockPaneStrip_ToolTipWindowList
                Return m_toolTipSelect
            End Get
        End Property

        Private ReadOnly Property ToolWindowTextFormat As TextFormatFlags
            Get
                Dim textFormat = TextFormatFlags.EndEllipsis Or TextFormatFlags.HorizontalCenter Or TextFormatFlags.SingleLine Or TextFormatFlags.VerticalCenter
                If MyBase.RightToLeft = RightToLeft.Yes Then
                    Return textFormat Or TextFormatFlags.RightToLeft Or TextFormatFlags.Right
                Else
                    Return textFormat
                End If
            End Get
        End Property

        Private Shared ReadOnly Property DocumentStripGapTop As Integer
            Get
                Return _DocumentStripGapTop
            End Get
        End Property

        Private Shared ReadOnly Property DocumentStripGapBottom As Integer
            Get
                Return _DocumentStripGapBottom
            End Get
        End Property

        Private ReadOnly Property DocumentTextFormat As TextFormatFlags
            Get
                Dim textFormat = TextFormatFlags.EndEllipsis Or TextFormatFlags.SingleLine Or TextFormatFlags.VerticalCenter Or TextFormatFlags.HorizontalCenter
                If MyBase.RightToLeft = RightToLeft.Yes Then
                    Return textFormat Or TextFormatFlags.RightToLeft
                Else
                    Return textFormat
                End If
            End Get
        End Property

        Private Shared ReadOnly Property DocumentTabMaxWidth As Integer
            Get
                Return _DocumentTabMaxWidth
            End Get
        End Property

        Private Shared ReadOnly Property DocumentButtonGapTop As Integer
            Get
                Return _DocumentButtonGapTop
            End Get
        End Property

        Private Shared ReadOnly Property DocumentButtonGapBottom As Integer
            Get
                Return _DocumentButtonGapBottom
            End Get
        End Property

        Private Shared ReadOnly Property DocumentButtonGapBetween As Integer
            Get
                Return _DocumentButtonGapBetween
            End Get
        End Property

        Private Shared ReadOnly Property DocumentButtonGapRight As Integer
            Get
                Return _DocumentButtonGapRight
            End Get
        End Property

        Private Shared ReadOnly Property DocumentTabGapTop As Integer
            Get
                Return _DocumentTabGapTop
            End Get
        End Property

        Private Shared ReadOnly Property DocumentTabGapLeft As Integer
            Get
                Return _DocumentTabGapLeft
            End Get
        End Property

        Private Shared ReadOnly Property DocumentTabGapRight As Integer
            Get
                Return _DocumentTabGapRight
            End Get
        End Property

        Private Shared ReadOnly Property DocumentIconGapBottom As Integer
            Get
                Return _DocumentIconGapBottom
            End Get
        End Property

        Private Shared ReadOnly Property DocumentIconGapLeft As Integer
            Get
                Return _DocumentIconGapLeft
            End Get
        End Property

        Private Shared ReadOnly Property DocumentIconGapRight As Integer
            Get
                Return _DocumentIconGapRight
            End Get
        End Property

        Private Shared ReadOnly Property DocumentIconWidth As Integer
            Get
                Return _DocumentIconWidth
            End Get
        End Property

        Private Shared ReadOnly Property DocumentIconHeight As Integer
            Get
                Return _DocumentIconHeight
            End Get
        End Property

        Private Shared ReadOnly Property DocumentTextGapRight As Integer
            Get
                Return _DocumentTextGapRight
            End Get
        End Property

#End Region

#End Region

        Public Sub New(pane As DockPane)
            MyBase.New(pane)
            SetStyle(ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer, True)

            SuspendLayout()

            m_components = New Container()
            m_toolTip = New ToolTip(Components)
            m_selectMenu = New ContextMenuStrip(Components)
            pane.DockPanel.Theme.ApplyTo(m_selectMenu)

            ResumeLayout()
        End Sub

        Protected Overrides Sub Dispose(disposing As Boolean)
            If disposing Then
                Components.Dispose()
                If m_boldFont IsNot Nothing Then
                    m_boldFont.Dispose()
                    m_boldFont = Nothing
                End If
            End If
            MyBase.Dispose(disposing)
        End Sub

        Protected Overrides Function MeasureHeight() As Integer
            If Appearance = DockPane.AppearanceStyle.ToolWindow Then
                Return MeasureHeight_ToolWindow()
            Else
                Return MeasureHeight_Document()
            End If
        End Function

        Private Function MeasureHeight_ToolWindow() As Integer
            If DockPane.IsAutoHide OrElse Tabs.Count <= 1 Then Return 0

            Dim height = Math.Max(TextFont.Height + If(EnableHighDpi = True, DocumentIconGapBottom, 0), ToolWindowImageHeight + ToolWindowImageGapTop + ToolWindowImageGapBottom) + ToolWindowStripGapTop + ToolWindowStripGapBottom

            Return height
        End Function

        Private Function MeasureHeight_Document() As Integer
            Dim height = Math.Max(TextFont.Height + DocumentTabGapTop + If(EnableHighDpi = True, DocumentIconGapBottom, 0), ButtonOverflow.Height + DocumentButtonGapTop + DocumentButtonGapBottom) + DocumentStripGapBottom + DocumentStripGapTop

            Return height
        End Function

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            MyBase.OnPaint(e)
            CalculateTabs()
            If Appearance = DockPane.AppearanceStyle.Document AndAlso DockPane.ActiveContent IsNot Nothing Then
                If EnsureDocumentTabVisible(DockPane.ActiveContent, False) Then CalculateTabs()
            End If

            DrawTabStrip(e.Graphics)
        End Sub

        Protected Overrides Sub OnRefreshChanges()
            SetInertButtons()
            Invalidate()
        End Sub

        Public Overrides Function GetOutline(index As Integer) As GraphicsPath
            If Appearance = DockPane.AppearanceStyle.Document Then
                Return GetOutline_Document(index)
            Else
                Return GetOutline_ToolWindow(index)
            End If
        End Function

        Private Function GetOutline_Document(index As Integer) As GraphicsPath
            Dim rectTab = Tabs(index).Rectangle.Value
            rectTab.X -= rectTab.Height / 2
            rectTab.Intersect(TabsRectangle)
            rectTab = RectangleToScreen(RtlTransform(Me, rectTab))
            Dim rectPaneClient = DockPane.RectangleToScreen(DockPane.ClientRectangle)

            Dim path As GraphicsPath = New GraphicsPath()
            Dim pathTab = GetTabOutline_Document(Tabs(index), True, True, True)
            path.AddPath(pathTab, True)

            If DockPane.DockPanel.DocumentTabStripLocation = DocumentTabStripLocation.Bottom Then
                path.AddLine(rectTab.Right, rectTab.Top, rectPaneClient.Right, rectTab.Top)
                path.AddLine(rectPaneClient.Right, rectTab.Top, rectPaneClient.Right, rectPaneClient.Top)
                path.AddLine(rectPaneClient.Right, rectPaneClient.Top, rectPaneClient.Left, rectPaneClient.Top)
                path.AddLine(rectPaneClient.Left, rectPaneClient.Top, rectPaneClient.Left, rectTab.Top)
                path.AddLine(rectPaneClient.Left, rectTab.Top, rectTab.Right, rectTab.Top)
            Else
                path.AddLine(rectTab.Right, rectTab.Bottom, rectPaneClient.Right, rectTab.Bottom)
                path.AddLine(rectPaneClient.Right, rectTab.Bottom, rectPaneClient.Right, rectPaneClient.Bottom)
                path.AddLine(rectPaneClient.Right, rectPaneClient.Bottom, rectPaneClient.Left, rectPaneClient.Bottom)
                path.AddLine(rectPaneClient.Left, rectPaneClient.Bottom, rectPaneClient.Left, rectTab.Bottom)
                path.AddLine(rectPaneClient.Left, rectTab.Bottom, rectTab.Right, rectTab.Bottom)
            End If
            Return path
        End Function

        Private Function GetOutline_ToolWindow(index As Integer) As GraphicsPath
            Dim rectTab = Tabs(index).Rectangle.Value
            rectTab.Intersect(TabsRectangle)
            rectTab = RectangleToScreen(RtlTransform(Me, rectTab))
            Dim rectPaneClient = DockPane.RectangleToScreen(DockPane.ClientRectangle)

            Dim path As GraphicsPath = New GraphicsPath()
            Dim pathTab = GetTabOutline(Tabs(index), True, True)
            path.AddPath(pathTab, True)
            path.AddLine(rectTab.Left, rectTab.Top, rectPaneClient.Left, rectTab.Top)
            path.AddLine(rectPaneClient.Left, rectTab.Top, rectPaneClient.Left, rectPaneClient.Top)
            path.AddLine(rectPaneClient.Left, rectPaneClient.Top, rectPaneClient.Right, rectPaneClient.Top)
            path.AddLine(rectPaneClient.Right, rectPaneClient.Top, rectPaneClient.Right, rectTab.Top)
            path.AddLine(rectPaneClient.Right, rectTab.Top, rectTab.Right, rectTab.Top)
            Return path
        End Function

        Private Sub CalculateTabs()
            If Appearance = DockPane.AppearanceStyle.ToolWindow Then
                CalculateTabs_ToolWindow()
            Else
                CalculateTabs_Document()
            End If
        End Sub

        Private Sub CalculateTabs_ToolWindow()
            If Tabs.Count <= 1 OrElse DockPane.IsAutoHide Then Return

            Dim rectTabStrip = TabStripRectangle

            ' Calculate tab widths
            Dim countTabs = Tabs.Count
            For Each tab As TabVS2013 In Tabs
                tab.MaxWidth = GetMaxTabWidth(Tabs.IndexOf(tab))
                tab.Flag = False
            Next

            ' Set tab whose max width less than average width
            Dim anyWidthWithinAverage = True
            Dim totalWidth = rectTabStrip.Width - ToolWindowStripGapLeft - ToolWindowStripGapRight
            Dim totalAllocatedWidth = 0
            Dim averageWidth As Integer = totalWidth / countTabs
            Dim remainedTabs = countTabs
            anyWidthWithinAverage = True

            While anyWidthWithinAverage AndAlso remainedTabs > 0
                anyWidthWithinAverage = False
                For Each tab As TabVS2013 In Tabs
                    If tab.Flag Then Continue For

                    If tab.MaxWidth <= averageWidth Then
                        tab.Flag = True
                        tab.TabWidth = tab.MaxWidth
                        totalAllocatedWidth += tab.TabWidth
                        anyWidthWithinAverage = True
                        remainedTabs -= 1
                    End If
                Next
                If remainedTabs <> 0 Then averageWidth = (totalWidth - totalAllocatedWidth) / remainedTabs
            End While

            ' If any tab width not set yet, set it to the average width
            If remainedTabs > 0 Then
                Dim roundUpWidth = totalWidth - totalAllocatedWidth - averageWidth * remainedTabs
                For Each tab As TabVS2013 In Tabs
                    If tab.Flag Then Continue For

                    tab.Flag = True
                    If roundUpWidth > 0 Then
                        tab.TabWidth = averageWidth + 1
                        roundUpWidth -= 1
                    Else
                        tab.TabWidth = averageWidth
                    End If
                Next
            End If

            ' Set the X position of the tabs
            Dim x = rectTabStrip.X + ToolWindowStripGapLeft
            For Each tab As TabVS2013 In Tabs
                tab.TabX = x
                x += tab.TabWidth
            Next
        End Sub

        Private Function CalculateDocumentTab(rectTabStrip As Rectangle, ByRef x As Integer, index As Integer) As Boolean
            Dim overflow = False

            Dim tab = TryCast(Tabs(index), TabVS2013)
            tab.MaxWidth = GetMaxTabWidth(index)
            Dim width = Math.Min(tab.MaxWidth, DocumentTabMaxWidth)
            If x + width < rectTabStrip.Right OrElse index = StartDisplayingTab Then
                tab.TabX = x
                tab.TabWidth = width
                EndDisplayingTab = index
            Else
                tab.TabX = 0
                tab.TabWidth = 0
                overflow = True
            End If
            x += width

            Return overflow
        End Function

        ''' <summary>
        ''' Calculate which tabs are displayed and in what order.
        ''' </summary>
        Private Sub CalculateTabs_Document()
            If m_startDisplayingTab >= Tabs.Count Then m_startDisplayingTab = 0

            Dim rectTabStrip = TabsRectangle

            Dim x = rectTabStrip.X '+ rectTabStrip.Height / 2;
            Dim overflow = False

            ' Originally all new documents that were considered overflow
            ' (not enough pane strip space to show all tabs) were added to
            ' the far left (assuming not right to left) and the tabs on the
            ' right were dropped from view. If StartDisplayingTab is not 0
            ' then we are dealing with making sure a specific tab is kept in focus.
            If m_startDisplayingTab > 0 Then
                Dim tempX = x
                Dim tab = TryCast(Tabs(m_startDisplayingTab), TabVS2013)
                tab.MaxWidth = GetMaxTabWidth(m_startDisplayingTab)

                ' Add the active tab and tabs to the left
                For i = StartDisplayingTab To 0 Step -1
                    CalculateDocumentTab(rectTabStrip, tempX, i)
                Next

                ' Store which tab is the first one displayed so that it
                ' will be drawn correctly (without part of the tab cut off)
                FirstDisplayingTab = EndDisplayingTab

                tempX = x ' Reset X location because we are starting over

                ' Start with the first tab displayed - name is a little misleading.
                ' Loop through each tab and set its location. If there is not enough
                ' room for all of them overflow will be returned.
                For i = EndDisplayingTab To Tabs.Count - 1
                    overflow = CalculateDocumentTab(rectTabStrip, tempX, i)
                Next

                ' If not all tabs are shown then we have an overflow.
                If FirstDisplayingTab <> 0 Then overflow = True
            Else
                For i = StartDisplayingTab To Tabs.Count - 1
                    overflow = CalculateDocumentTab(rectTabStrip, x, i)
                Next
                For i = 0 To StartDisplayingTab - 1
                    overflow = CalculateDocumentTab(rectTabStrip, x, i)
                Next

                FirstDisplayingTab = StartDisplayingTab
            End If

            If Not overflow Then
                m_startDisplayingTab = 0
                FirstDisplayingTab = 0
                x = rectTabStrip.X
                For Each tab As TabVS2013 In Tabs
                    tab.TabX = x
                    x += tab.TabWidth
                Next
            End If

            DocumentTabsOverflow = overflow
        End Sub

        Protected Overrides Sub EnsureTabVisible(content As IDockContent)
            If Appearance <> DockPane.AppearanceStyle.Document OrElse Not Tabs.Contains(content) Then Return

            CalculateTabs()
            EnsureDocumentTabVisible(content, True)
        End Sub

        Private Function EnsureDocumentTabVisible(content As IDockContent, repaint As Boolean) As Boolean
            Dim index = Tabs.IndexOf(content)
            If index = -1 Then Return False ' TODO: should prevent it from being -1;

            Dim tab = TryCast(Tabs(index), TabVS2013)
            If tab.TabWidth <> 0 Then Return False

            StartDisplayingTab = index
            If repaint Then Invalidate()

            Return True
        End Function

        Private Function GetMaxTabWidth(index As Integer) As Integer
            If Appearance = DockPane.AppearanceStyle.ToolWindow Then
                Return GetMaxTabWidth_ToolWindow(index)
            Else
                Return GetMaxTabWidth_Document(index)
            End If
        End Function

        Private Function GetMaxTabWidth_ToolWindow(index As Integer) As Integer
            Dim content = Tabs(index).Content
            Dim sizeString = TextRenderer.MeasureText(content.DockHandler.TabText, TextFont)
            Return ToolWindowImageWidth + sizeString.Width + ToolWindowImageGapLeft + ToolWindowImageGapRight + ToolWindowTextGapRight
        End Function

        Private Const TAB_CLOSE_BUTTON_WIDTH As Integer = 30

        Private Function GetMaxTabWidth_Document(index As Integer) As Integer
            Dim content = Tabs(index).Content
            Dim height = GetTabRectangle_Document(index).Height
            Dim sizeText As Size = TextRenderer.MeasureText(content.DockHandler.TabText, BoldFont, New Size(DocumentTabMaxWidth, height), DocumentTextFormat)

            Dim width As Integer
            If DockPane.DockPanel.ShowDocumentIcon Then
                width = sizeText.Width + DocumentIconWidth + DocumentIconGapLeft + DocumentIconGapRight + DocumentTextGapRight
            Else
                width = sizeText.Width + DocumentIconGapLeft + DocumentTextGapRight
            End If

            width += TAB_CLOSE_BUTTON_WIDTH
            Return width
        End Function

        Private Sub DrawTabStrip(g As Graphics)
            ' IMPORTANT: fill background.
            Dim rectTabStrip = TabStripRectangle
            g.FillRectangle(DockPane.DockPanel.Theme.PaintingService.GetBrush(DockPane.DockPanel.Theme.ColorPalette.MainWindowActive.Background), rectTabStrip)

            If Appearance = DockPane.AppearanceStyle.Document Then
                DrawTabStrip_Document(g)
            Else
                DrawTabStrip_ToolWindow(g)
            End If
        End Sub

        Private Sub DrawTabStrip_Document(g As Graphics)
            Dim count = Tabs.Count
            If count = 0 Then Return

            Dim rectTabStrip As Rectangle = New Rectangle(TabStripRectangle.Location, TabStripRectangle.Size)
            rectTabStrip.Height += 1

            ' Draw the tabs
            Dim rectTabOnly = TabsRectangle
            Dim rectTab = Rectangle.Empty
            Dim tabActive As TabVS2013 = Nothing
            g.SetClip(RtlTransform(Me, rectTabOnly))
            For i = 0 To count - 1
                rectTab = GetTabRectangle(i)
                If Tabs(i).Content Is DockPane.ActiveContent Then
                    tabActive = TryCast(Tabs(i), TabVS2013)
                    tabActive.Rectangle = rectTab
                    Continue For
                End If

                If rectTab.IntersectsWith(rectTabOnly) Then
                    Dim tab = TryCast(Tabs(i), TabVS2013)
                    tab.Rectangle = rectTab
                    DrawTab(g, tab)
                End If
            Next

            g.SetClip(rectTabStrip)

            If DockPane.DockPanel.DocumentTabStripLocation = DocumentTabStripLocation.Bottom Then
            Else
                Dim tabUnderLineColor As Color
                If tabActive IsNot Nothing AndAlso DockPane.IsActiveDocumentPane Then
                    tabUnderLineColor = DockPane.DockPanel.Theme.ColorPalette.TabSelectedActive.Background
                Else
                    tabUnderLineColor = DockPane.DockPanel.Theme.ColorPalette.TabSelectedInactive.Background
                End If

                g.DrawLine(DockPane.DockPanel.Theme.PaintingService.GetPen(tabUnderLineColor, 4), rectTabStrip.Left, rectTabStrip.Bottom, rectTabStrip.Right, rectTabStrip.Bottom)
            End If

            g.SetClip(RtlTransform(Me, rectTabOnly))
            If tabActive IsNot Nothing Then
                rectTab = tabActive.Rectangle.Value
                If rectTab.IntersectsWith(rectTabOnly) Then
                    rectTab.Intersect(rectTabOnly)
                    tabActive.Rectangle = rectTab
                    DrawTab(g, tabActive)
                End If
            End If
        End Sub

        Private Sub DrawTabStrip_ToolWindow(g As Graphics)
            Dim rect = TabStripRectangle_ToolWindow
            Dim borderColor = DockPane.DockPanel.Theme.ColorPalette.ToolWindowBorder

            g.DrawLine(DockPane.DockPanel.Theme.PaintingService.GetPen(borderColor), rect.Left, rect.Top, rect.Right, rect.Top)

            For i = 0 To Tabs.Count - 1
                Dim tab = TryCast(Tabs(i), TabVS2013)
                tab.Rectangle = GetTabRectangle(i)
                DrawTab(g, tab)
            Next
        End Sub

        Private Function GetTabRectangle(index As Integer) As Rectangle
            If Appearance = DockPane.AppearanceStyle.ToolWindow Then
                Return GetTabRectangle_ToolWindow(index)
            Else
                Return GetTabRectangle_Document(index)
            End If
        End Function

        Private Function GetTabRectangle_ToolWindow(index As Integer) As Rectangle
            Dim rectTabStrip = TabStripRectangle

            Dim tab = CType(Tabs(index), TabVS2013)
            Return New Rectangle(tab.TabX, rectTabStrip.Y, tab.TabWidth, rectTabStrip.Height)
        End Function

        Private Function GetTabRectangle_Document(index As Integer) As Rectangle
            Dim rectTabStrip = TabStripRectangle
            Dim tab = CType(Tabs(index), TabVS2013)

            Dim rect As Rectangle = New Rectangle()
            rect.X = tab.TabX
            rect.Width = tab.TabWidth
            rect.Height = rectTabStrip.Height - DocumentTabGapTop

            If DockPane.DockPanel.DocumentTabStripLocation = DocumentTabStripLocation.Bottom Then
                rect.Y = rectTabStrip.Y + DocumentStripGapBottom
            Else
                rect.Y = rectTabStrip.Y + DocumentTabGapTop
            End If

            Return rect
        End Function

        Private Sub DrawTab(g As Graphics, tab As TabVS2013)
            If Appearance = DockPane.AppearanceStyle.ToolWindow Then
                DrawTab_ToolWindow(g, tab)
            Else
                DrawTab_Document(g, tab)
            End If
        End Sub

        Private Function GetTabOutline(tab As Tab, rtlTransform As Boolean, toScreen As Boolean) As GraphicsPath
            If Appearance = DockPane.AppearanceStyle.ToolWindow Then
                Return GetTabOutline_ToolWindow(tab, rtlTransform, toScreen)
            Else
                Return GetTabOutline_Document(tab, rtlTransform, toScreen, False)
            End If
        End Function

        Private Function GetTabOutline_ToolWindow(tab As Tab, rtlTransform As Boolean, toScreen As Boolean) As GraphicsPath
            Dim rect = GetTabRectangle(Tabs.IndexOf(tab))
            If rtlTransform Then rect = DrawHelper.RtlTransform(Me, rect)
            If toScreen Then rect = RectangleToScreen(rect)

            GetRoundedCornerTab(GraphicsPath, rect, False)
            Return GraphicsPath
        End Function

        Private Function GetTabOutline_Document(tab As Tab, rtlTransform As Boolean, toScreen As Boolean, full As Boolean) As GraphicsPath
            Call GraphicsPath.Reset()
            Dim rect = GetTabRectangle(Tabs.IndexOf(tab))

            ' Shorten TabOutline so it doesn't get overdrawn by icons next to it
            rect.Intersect(TabsRectangle)
            rect.Width -= 1

            If rtlTransform Then rect = DrawHelper.RtlTransform(Me, rect)
            If toScreen Then rect = RectangleToScreen(rect)

            GraphicsPath.AddRectangle(rect)
            Return GraphicsPath
        End Function

        Private Sub DrawTab_ToolWindow(g As Graphics, tab As TabVS2013)
            Dim rect = tab.Rectangle.Value
            Dim rectIcon As Rectangle = New Rectangle(rect.X + ToolWindowImageGapLeft, rect.Y + rect.Height - ToolWindowImageGapBottom - ToolWindowImageHeight, ToolWindowImageWidth, ToolWindowImageHeight)
            Dim rectText As Rectangle = If(EnableHighDpi = True, New Rectangle(rect.X + ToolWindowImageGapLeft, rect.Y + rect.Height - ToolWindowImageGapBottom - TextFont.Height, ToolWindowImageWidth, TextFont.Height), rectIcon)
            rectText.X += rectIcon.Width + ToolWindowImageGapRight
            rectText.Width = rect.Width - rectIcon.Width - ToolWindowImageGapLeft - ToolWindowImageGapRight - ToolWindowTextGapRight

            Dim rectTab = RtlTransform(Me, rect)
            rectText = RtlTransform(Me, rectText)
            rectIcon = RtlTransform(Me, rectIcon)
            Dim borderColor = DockPane.DockPanel.Theme.ColorPalette.ToolWindowBorder

            Dim separatorColor = DockPane.DockPanel.Theme.ColorPalette.ToolWindowSeparator
            If DockPane.ActiveContent Is tab.Content Then
                Dim textColor As Color
                Dim backgroundColor As Color
                If DockPane.IsActiveDocumentPane Then
                    textColor = DockPane.DockPanel.Theme.ColorPalette.ToolWindowTabSelectedActive.Text
                    backgroundColor = DockPane.DockPanel.Theme.ColorPalette.ToolWindowTabSelectedActive.Background
                Else
                    textColor = DockPane.DockPanel.Theme.ColorPalette.ToolWindowTabSelectedInactive.Text
                    backgroundColor = DockPane.DockPanel.Theme.ColorPalette.ToolWindowTabSelectedInactive.Background
                End If

                g.FillRectangle(DockPane.DockPanel.Theme.PaintingService.GetBrush(backgroundColor), rect)
                g.DrawLine(DockPane.DockPanel.Theme.PaintingService.GetPen(borderColor), rect.Left, rect.Top, rect.Left, rect.Bottom)
                g.DrawLine(DockPane.DockPanel.Theme.PaintingService.GetPen(borderColor), rect.Left, rect.Bottom - 1, rect.Right, rect.Bottom - 1)
                g.DrawLine(DockPane.DockPanel.Theme.PaintingService.GetPen(borderColor), rect.Right - 1, rect.Top, rect.Right - 1, rect.Bottom)
                TextRenderer.DrawText(g, tab.Content.DockHandler.TabText, TextFont, rectText, textColor, ToolWindowTextFormat)
            Else
                Dim textColor As Color
                Dim backgroundColor As Color
                If tab.Content Is DockPane.MouseOverTab Then
                    textColor = DockPane.DockPanel.Theme.ColorPalette.ToolWindowTabUnselectedHovered.Text
                    backgroundColor = DockPane.DockPanel.Theme.ColorPalette.ToolWindowTabUnselectedHovered.Background
                Else
                    textColor = DockPane.DockPanel.Theme.ColorPalette.ToolWindowTabUnselected.Text
                    backgroundColor = DockPane.DockPanel.Theme.ColorPalette.MainWindowActive.Background
                End If

                g.FillRectangle(DockPane.DockPanel.Theme.PaintingService.GetBrush(backgroundColor), rect)
                g.DrawLine(DockPane.DockPanel.Theme.PaintingService.GetPen(borderColor), rect.Left, rect.Top, rect.Right, rect.Top)
                TextRenderer.DrawText(g, tab.Content.DockHandler.TabText, TextFont, rectText, textColor, ToolWindowTextFormat)
            End If

            If rectTab.Contains(rectIcon) Then g.DrawIcon(tab.Content.DockHandler.Icon, rectIcon)
        End Sub

        Private Sub DrawTab_Document(g As Graphics, tab As TabVS2013)
            Dim rect = tab.Rectangle.Value
            If tab.TabWidth = 0 Then Return

            Dim rectCloseButton = GetCloseButtonRect(rect)
            Dim rectIcon As Rectangle = New Rectangle(rect.X + DocumentIconGapLeft, rect.Y + rect.Height - DocumentIconGapBottom - DocumentIconHeight, DocumentIconWidth, DocumentIconHeight)
            Dim rectText As Rectangle = If(EnableHighDpi = True, New Rectangle(rect.X + DocumentIconGapLeft, rect.Y + rect.Height - DocumentIconGapBottom - TextFont.Height, DocumentIconWidth, TextFont.Height), rectIcon)
            If DockPane.DockPanel.ShowDocumentIcon Then
                rectText.X += rectIcon.Width + DocumentIconGapRight
                rectText.Y = rect.Y
                rectText.Width = rect.Width - rectIcon.Width - DocumentIconGapLeft - DocumentIconGapRight - DocumentTextGapRight - rectCloseButton.Width
                rectText.Height = rect.Height
            Else
                rectText.Width = rect.Width - DocumentIconGapLeft - DocumentTextGapRight - rectCloseButton.Width
            End If

            Dim rectTab = RtlTransform(Me, rect)
            Dim rectBack = RtlTransform(Me, rect)
            rectBack.Width += DocumentIconGapLeft
            rectBack.X -= DocumentIconGapLeft

            rectText = RtlTransform(Me, rectText)
            rectIcon = RtlTransform(Me, rectIcon)

            Dim activeColor = DockPane.DockPanel.Theme.ColorPalette.TabSelectedActive.Background
            Dim lostFocusColor = DockPane.DockPanel.Theme.ColorPalette.TabSelectedInactive.Background
            Dim inactiveColor = DockPane.DockPanel.Theme.ColorPalette.MainWindowActive.Background
            Dim mouseHoverColor = DockPane.DockPanel.Theme.ColorPalette.TabUnselectedHovered.Background

            Dim activeText = DockPane.DockPanel.Theme.ColorPalette.TabSelectedActive.Text
            Dim lostFocusText = DockPane.DockPanel.Theme.ColorPalette.TabSelectedInactive.Text
            Dim inactiveText = DockPane.DockPanel.Theme.ColorPalette.TabUnselected.Text
            Dim mouseHoverText = DockPane.DockPanel.Theme.ColorPalette.TabUnselectedHovered.Text

            Dim text As Color
            Dim image As Image = Nothing
            Dim paint As Color
            Dim imageService = DockPane.DockPanel.Theme.ImageService
            If DockPane.ActiveContent Is tab.Content Then
                If DockPane.IsActiveDocumentPane Then
                    paint = activeColor
                    text = activeText
                    image = If(IsMouseDown, imageService.TabPressActive_Close, If(rectCloseButton = ActiveClose, imageService.TabHoverActive_Close, imageService.TabActive_Close))
                Else
                    paint = lostFocusColor
                    text = lostFocusText
                    image = If(IsMouseDown, imageService.TabPressLostFocus_Close, If(rectCloseButton = ActiveClose, imageService.TabHoverLostFocus_Close, imageService.TabLostFocus_Close))
                End If
            Else
                If tab.Content Is DockPane.MouseOverTab Then
                    paint = mouseHoverColor
                    text = mouseHoverText
                    image = If(IsMouseDown, imageService.TabPressInactive_Close, If(rectCloseButton = ActiveClose, imageService.TabHoverInactive_Close, imageService.TabInactive_Close))
                Else
                    paint = inactiveColor
                    text = inactiveText
                End If
            End If

            g.FillRectangle(DockPane.DockPanel.Theme.PaintingService.GetBrush(paint), rect)
            TextRenderer.DrawText(g, tab.Content.DockHandler.TabText, TextFont, rectText, text, DocumentTextFormat)
            If image IsNot Nothing Then g.DrawImage(image, rectCloseButton)

            If rectTab.Contains(rectIcon) AndAlso DockPane.DockPanel.ShowDocumentIcon Then g.DrawIcon(tab.Content.DockHandler.Icon, rectIcon)
        End Sub

        Private m_isMouseDown As Boolean = False
        Protected Property IsMouseDown As Boolean
            Get
                Return m_isMouseDown
            End Get
            Private Set(value As Boolean)
                If m_isMouseDown = value Then Return

                m_isMouseDown = value
                Invalidate()
            End Set
        End Property

        Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
            MyBase.OnMouseUp(e)
            If IsMouseDown Then IsMouseDown = False
        End Sub

        Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
            MyBase.OnMouseDown(e)
            ' suspend drag if mouse is down on active close button.
            m_suspendDrag = ActiveCloseHitTest(e.Location)
            If Not IsMouseDown Then IsMouseDown = True
        End Sub

        Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
            If Not m_suspendDrag Then MyBase.OnMouseMove(e)

            Dim index = HitTest(PointToClient(MousePosition))
            Dim toolTip = String.Empty

            Dim tabUpdate = False
            Dim buttonUpdate = False
            If index <> -1 Then
                Dim tab = TryCast(Tabs(index), TabVS2013)
                If Appearance = DockPane.AppearanceStyle.ToolWindow OrElse Appearance = DockPane.AppearanceStyle.Document Then
                    tabUpdate = SetMouseOverTab(If(tab.Content Is DockPane.ActiveContent, Nothing, tab.Content))
                End If

                If Not String.IsNullOrEmpty(tab.Content.DockHandler.ToolTipText) Then
                    toolTip = tab.Content.DockHandler.ToolTipText
                ElseIf tab.MaxWidth > tab.TabWidth Then
                    toolTip = tab.Content.DockHandler.TabText
                End If

                Dim mousePos = PointToClient(MousePosition)
                Dim tabRect = tab.Rectangle.Value
                Dim closeButtonRect = GetCloseButtonRect(tabRect)
                Dim mouseRect = New Rectangle(mousePos, New Size(1, 1))
                buttonUpdate = SetActiveClose(If(closeButtonRect.IntersectsWith(mouseRect), closeButtonRect, Rectangle.Empty))
            Else
                tabUpdate = SetMouseOverTab(Nothing)
                buttonUpdate = SetActiveClose(Rectangle.Empty)
            End If

            If tabUpdate OrElse buttonUpdate Then Invalidate()

            If Not Equals(m_toolTip.GetToolTip(Me), toolTip) Then
                m_toolTip.Active = False
                m_toolTip.SetToolTip(Me, toolTip)
                m_toolTip.Active = True
            End If
        End Sub

        Protected Overrides Sub OnMouseClick(e As MouseEventArgs)
            MyBase.OnMouseClick(e)
            If e.Button <> MouseButtons.Left OrElse Appearance <> DockPane.AppearanceStyle.Document Then Return

            Dim indexHit = HitTest()
            If indexHit > -1 Then TabCloseButtonHit(indexHit)
        End Sub

        Private Sub TabCloseButtonHit(index As Integer)
            Dim mousePos = PointToClient(MousePosition)
            Dim tabRect = GetTabBounds(Tabs(index))
            If tabRect.Contains(ActiveClose) AndAlso ActiveCloseHitTest(mousePos) Then TryCloseTab(index)
        End Sub

        Private Function GetCloseButtonRect(rectTab As Rectangle) As Rectangle
            If Appearance <> DockPane.AppearanceStyle.Document Then
                Return Rectangle.Empty
            End If

            Const gap = 3
            Dim imageSize = If(EnableHighDpi = True, rectTab.Height - gap * 2, 15)
            Return New Rectangle(rectTab.X + rectTab.Width - imageSize - gap - 1, rectTab.Y + gap, imageSize, imageSize)
        End Function

        Private Sub WindowList_Click(sender As Object, e As EventArgs)
            SelectMenu.Items.Clear()
            For Each tab As TabVS2013 In Tabs
                Dim content = tab.Content
                Dim item As ToolStripItem = SelectMenu.Items.Add(content.DockHandler.TabText, content.DockHandler.Icon.ToBitmap())
                item.Tag = tab.Content
                AddHandler item.Click, New EventHandler(AddressOf ContextMenuItem_Click)
            Next

            Dim workingArea = Screen.GetWorkingArea(ButtonWindowList.PointToScreen(New Point(ButtonWindowList.Width / 2, ButtonWindowList.Height / 2)))
            Dim menu = New Rectangle(ButtonWindowList.PointToScreen(New Point(0, ButtonWindowList.Location.Y + ButtonWindowList.Height)), SelectMenu.Size)
            Dim menuMargined = New Rectangle(menu.X - SelectMenuMargin, menu.Y - SelectMenuMargin, menu.Width + SelectMenuMargin, menu.Height + SelectMenuMargin)
            If workingArea.Contains(menuMargined) Then
                SelectMenu.Show(menu.Location)
            Else
                Dim newPoint = menu.Location
                newPoint.X = Balance(SelectMenu.Width, SelectMenuMargin, newPoint.X, workingArea.Left, workingArea.Right)
                newPoint.Y = Balance(SelectMenu.Size.Height, SelectMenuMargin, newPoint.Y, workingArea.Top, workingArea.Bottom)
                Dim button = ButtonWindowList.PointToScreen(New Point(0, ButtonWindowList.Height))
                If newPoint.Y < button.Y Then
                    ' flip the menu up to be above the button.
                    newPoint.Y = button.Y - ButtonWindowList.Height
                    SelectMenu.Show(newPoint, ToolStripDropDownDirection.AboveRight)
                Else
                    SelectMenu.Show(newPoint)
                End If
            End If
        End Sub

        Private Sub ContextMenuItem_Click(sender As Object, e As EventArgs)
            Dim item As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)
            If item IsNot Nothing Then
                Dim content = CType(item.Tag, IDockContent)
                DockPane.ActiveContent = content
            End If
        End Sub

        Private Sub SetInertButtons()
            If Appearance = DockPane.AppearanceStyle.ToolWindow Then
                If m_buttonOverflow IsNot Nothing Then m_buttonOverflow.Left = -m_buttonOverflow.Width

                If m_buttonWindowList IsNot Nothing Then m_buttonWindowList.Left = -m_buttonWindowList.Width
            Else
                ButtonOverflow.Visible = m_documentTabsOverflow
                ButtonOverflow.RefreshChanges()

                ButtonWindowList.Visible = Not m_documentTabsOverflow
                ButtonWindowList.RefreshChanges()
            End If
        End Sub

        Protected Overrides Sub OnLayout(levent As LayoutEventArgs)
            If Appearance = DockPane.AppearanceStyle.Document Then
                LayoutButtons()
                OnRefreshChanges()
            End If

            MyBase.OnLayout(levent)
        End Sub

        Private Sub LayoutButtons()
            Dim rectTabStrip = TabStripRectangle

            ' Set position and size of the buttons
            Dim buttonWidth = ButtonOverflow.Image.Width
            Dim buttonHeight = ButtonOverflow.Image.Height
            Dim height = rectTabStrip.Height - DocumentButtonGapTop - DocumentButtonGapBottom
            If buttonHeight < height Then
                buttonWidth = buttonWidth * height / buttonHeight
                buttonHeight = height
            End If
            Dim buttonSize As Size = New Size(buttonWidth, buttonHeight)

            Dim x = rectTabStrip.X + rectTabStrip.Width - DocumentTabGapLeft - DocumentButtonGapRight - buttonWidth
            Dim y = rectTabStrip.Y + DocumentButtonGapTop
            Dim point As Point = New Point(x, y)
            ButtonOverflow.Bounds = RtlTransform(Me, New Rectangle(point, buttonSize))

            ' If the close button is not visible draw the window list button overtop.
            ' Otherwise it is drawn to the left of the close button.
            ButtonWindowList.Bounds = RtlTransform(Me, New Rectangle(point, buttonSize))
        End Sub

        Private Sub Close_Click(sender As Object, e As EventArgs)
            DockPane.CloseActiveContent()
            If EnableMemoryLeakFix = True Then
                ContentClosed()
            End If
        End Sub

        Protected Overrides Function HitTest(point As Point) As Integer
            If Not TabsRectangle.Contains(point) Then Return -1

            For Each tab In Tabs
                Dim path = GetTabOutline(tab, True, False)
                If path.IsVisible(point) Then Return Tabs.IndexOf(tab)
            Next

            Return -1
        End Function

        Protected Overrides Function MouseDownActivateTest(e As MouseEventArgs) As Boolean
            Dim result = MyBase.MouseDownActivateTest(e)
            If result AndAlso e.Button = MouseButtons.Left AndAlso Appearance = DockPane.AppearanceStyle.Document Then
                ' don't activate if mouse is down on active close button
                result = Not ActiveCloseHitTest(e.Location)
            End If
            Return result
        End Function

        Private Function ActiveCloseHitTest(ptMouse As Point) As Boolean
            Dim result = False
            If Not ActiveClose.IsEmpty Then
                Dim mouseRect = New Rectangle(ptMouse, New Size(1, 1))
                result = ActiveClose.IntersectsWith(mouseRect)
            End If
            Return result
        End Function

        Protected Overrides Function GetTabBounds(tab As Tab) As Rectangle
            Dim path = GetTabOutline(tab, True, False)
            Dim rectangle As RectangleF = path.GetBounds()
            Return New Rectangle(rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height)
        End Function

        Private ReadOnly Property ActiveClose As Rectangle
            Get
                Return _activeClose
            End Get
        End Property

        Private Function SetActiveClose(rectangle As Rectangle) As Boolean
            If _activeClose = rectangle Then Return False

            _activeClose = rectangle
            Return True
        End Function

        Private Function SetMouseOverTab(content As IDockContent) As Boolean
            If DockPane.MouseOverTab Is content Then Return False

            DockPane.MouseOverTab = content
            Return True
        End Function

        Protected Overrides Sub OnMouseLeave(e As EventArgs)
            Dim tabUpdate = SetMouseOverTab(Nothing)
            Dim buttonUpdate = SetActiveClose(Rectangle.Empty)
            If tabUpdate OrElse buttonUpdate Then Invalidate()

            MyBase.OnMouseLeave(e)
        End Sub

        Protected Overrides Sub OnRightToLeftChanged(e As EventArgs)
            MyBase.OnRightToLeftChanged(e)
            PerformLayout()
        End Sub
    End Class
End Namespace
