Imports System.Windows.Forms
Imports System.Drawing
Imports System.Drawing.Drawing2D

Namespace Docking
    Partial Class DockPanel
#Region "IHitTest"
        Public Interface IHitTest
            Function HitTest(pt As Point) As DockStyle
            Property Status As DockStyle
        End Interface

        Public Interface IPaneIndicator
            Inherits IHitTest
            Property Location As Point
            Property Visible As Boolean
            ReadOnly Property Left As Integer
            ReadOnly Property Top As Integer
            ReadOnly Property Right As Integer
            ReadOnly Property Bottom As Integer
            ReadOnly Property ClientRectangle As Rectangle
            ReadOnly Property Width As Integer
            ReadOnly Property Height As Integer
            ReadOnly Property DisplayingGraphicsPath As GraphicsPath
        End Interface

        Public Interface IPanelIndicator
            Inherits IHitTest
            Property Location As Point
            Property Visible As Boolean
            ReadOnly Property Bounds As Rectangle
            ReadOnly Property Width As Integer
            ReadOnly Property Height As Integer
        End Interface

        Public Structure HotSpotIndex
            Public Sub New(x As Integer, y As Integer, dockStyle As DockStyle)
                m_x = x
                m_y = y
                m_dockStyle = dockStyle
            End Sub

            Private m_x As Integer
            Public ReadOnly Property X As Integer
                Get
                    Return m_x
                End Get
            End Property

            Private m_y As Integer
            Public ReadOnly Property Y As Integer
                Get
                    Return m_y
                End Get
            End Property

            Private m_dockStyle As DockStyle
            Public ReadOnly Property DockStyle As DockStyle
                Get
                    Return m_dockStyle
                End Get
            End Property
        End Structure

#End Region

        Public NotInheritable Class DockDragHandler
            Inherits DragHandler
            Public Class DockIndicator
                Inherits DragForm
#Region "consts"
                Private _PanelIndicatorMargin As Integer = 10
#End Region

                Private m_dragHandler As DockDragHandler

                Public Sub New(dragHandler As DockDragHandler)
                    m_dragHandler = dragHandler
                    Controls.AddRange({CType(PaneDiamond, Control), CType(PanelLeft, Control), CType(PanelRight, Control), CType(PanelTop, Control), CType(PanelBottom, Control), CType(PanelFill, Control)})
                    Region = New Region(Rectangle.Empty)
                End Sub

                Private m_paneDiamond As IPaneIndicator = Nothing
                Private ReadOnly Property PaneDiamond As IPaneIndicator
                    Get
                        If m_paneDiamond Is Nothing Then m_paneDiamond = m_dragHandler.DockPanel.Theme.Extender.PaneIndicatorFactory.CreatePaneIndicator(m_dragHandler.DockPanel.Theme)

                        Return m_paneDiamond
                    End Get
                End Property

                Private m_panelLeft As IPanelIndicator = Nothing
                Private ReadOnly Property PanelLeft As IPanelIndicator
                    Get
                        If m_panelLeft Is Nothing Then m_panelLeft = m_dragHandler.DockPanel.Theme.Extender.PanelIndicatorFactory.CreatePanelIndicator(DockStyle.Left, m_dragHandler.DockPanel.Theme)

                        Return m_panelLeft
                    End Get
                End Property

                Private m_panelRight As IPanelIndicator = Nothing
                Private ReadOnly Property PanelRight As IPanelIndicator
                    Get
                        If m_panelRight Is Nothing Then m_panelRight = m_dragHandler.DockPanel.Theme.Extender.PanelIndicatorFactory.CreatePanelIndicator(DockStyle.Right, m_dragHandler.DockPanel.Theme)

                        Return m_panelRight
                    End Get
                End Property

                Private m_panelTop As IPanelIndicator = Nothing
                Private ReadOnly Property PanelTop As IPanelIndicator
                    Get
                        If m_panelTop Is Nothing Then m_panelTop = m_dragHandler.DockPanel.Theme.Extender.PanelIndicatorFactory.CreatePanelIndicator(DockStyle.Top, m_dragHandler.DockPanel.Theme)

                        Return m_panelTop
                    End Get
                End Property

                Private m_panelBottom As IPanelIndicator = Nothing
                Private ReadOnly Property PanelBottom As IPanelIndicator
                    Get
                        If m_panelBottom Is Nothing Then m_panelBottom = m_dragHandler.DockPanel.Theme.Extender.PanelIndicatorFactory.CreatePanelIndicator(DockStyle.Bottom, m_dragHandler.DockPanel.Theme)

                        Return m_panelBottom
                    End Get
                End Property

                Private m_panelFill As IPanelIndicator = Nothing
                Private ReadOnly Property PanelFill As IPanelIndicator
                    Get
                        If m_panelFill Is Nothing Then m_panelFill = m_dragHandler.DockPanel.Theme.Extender.PanelIndicatorFactory.CreatePanelIndicator(DockStyle.Fill, m_dragHandler.DockPanel.Theme)

                        Return m_panelFill
                    End Get
                End Property

                Private m_fullPanelEdge As Boolean = False
                Public Property FullPanelEdge As Boolean
                    Get
                        Return m_fullPanelEdge
                    End Get
                    Set(value As Boolean)
                        If m_fullPanelEdge = value Then Return

                        m_fullPanelEdge = value
                        RefreshChanges()
                    End Set
                End Property

                Public ReadOnly Property DragHandler As DockDragHandler
                    Get
                        Return m_dragHandler
                    End Get
                End Property

                Public ReadOnly Property DockPanel As DockPanel
                    Get
                        Return DragHandler.DockPanel
                    End Get
                End Property

                Private m_dockPane As DockPane = Nothing
                Public Property DockPane As DockPane
                    Get
                        Return m_dockPane
                    End Get
                    Friend Set(value As DockPane)
                        If m_dockPane Is value Then Return

                        Dim oldDisplayingPane = DisplayingPane
                        m_dockPane = value
                        If oldDisplayingPane IsNot DisplayingPane Then RefreshChanges()
                    End Set
                End Property

                Private m_hitTest As IHitTest = Nothing
                Private Property HitTestResult As IHitTest
                    Get
                        Return m_hitTest
                    End Get
                    Set(value As IHitTest)
                        If m_hitTest Is value Then Return

                        If m_hitTest IsNot Nothing Then m_hitTest.Status = DockStyle.None

                        m_hitTest = value
                    End Set
                End Property

                Private ReadOnly Property DisplayingPane As DockPane
                    Get
                        Return If(ShouldPaneDiamondVisible(), DockPane, Nothing)
                    End Get
                End Property

                Private Sub RefreshChanges()
                    If EnablePerScreenDpi = True Then
                        'SHCore.PROCESS_DPI_AWARENESS value;
                        'if (SHCore.GetProcessDpiAwareness(Process.GetCurrentProcess().Handle, out value) == HRESULT.S_OK)
                        '{
                        '    if (value == SHCore.PROCESS_DPI_AWARENESS.PROCESS_SYSTEM_DPI_AWARE)
                        '    {
                        Dim allScreens = Screen.AllScreens
                        Dim mousePos = MousePosition
                        For Each screen In allScreens
                            If screen.Bounds.Contains(mousePos) Then
                                Bounds = screen.Bounds
                            End If
                        Next
                        '    }
                        '}
                    End If

                    Dim region As Region = New Region(Rectangle.Empty)
                    Dim rectDockArea = If(FullPanelEdge, DockPanel.DockArea, DockPanel.DocumentWindowBounds)

                    rectDockArea = RectangleToClient(DockPanel.RectangleToScreen(rectDockArea))
                    If ShouldPanelIndicatorVisible(DockState.DockLeft) Then
                        PanelLeft.Location = New Point(rectDockArea.X + _PanelIndicatorMargin, CInt(rectDockArea.Y + (rectDockArea.Height - PanelRight.Height) / 2))
                        PanelLeft.Visible = True
                        region.Union(PanelLeft.Bounds)
                    Else
                        PanelLeft.Visible = False
                    End If

                    If ShouldPanelIndicatorVisible(DockState.DockRight) Then
                        PanelRight.Location = New Point(rectDockArea.X + rectDockArea.Width - PanelRight.Width - _PanelIndicatorMargin, CInt(rectDockArea.Y + (rectDockArea.Height - PanelRight.Height) / 2))
                        PanelRight.Visible = True
                        region.Union(PanelRight.Bounds)
                    Else
                        PanelRight.Visible = False
                    End If

                    If ShouldPanelIndicatorVisible(DockState.DockTop) Then
                        PanelTop.Location = New Point(CInt(rectDockArea.X + (rectDockArea.Width - PanelTop.Width) / 2), rectDockArea.Y + _PanelIndicatorMargin)
                        PanelTop.Visible = True
                        region.Union(PanelTop.Bounds)
                    Else
                        PanelTop.Visible = False
                    End If

                    If ShouldPanelIndicatorVisible(DockState.DockBottom) Then
                        PanelBottom.Location = New Point(CInt(rectDockArea.X + (rectDockArea.Width - PanelBottom.Width) / 2), rectDockArea.Y + rectDockArea.Height - PanelBottom.Height - _PanelIndicatorMargin)
                        PanelBottom.Visible = True
                        region.Union(PanelBottom.Bounds)
                    Else
                        PanelBottom.Visible = False
                    End If

                    If ShouldPanelIndicatorVisible(DockState.Document) Then
                        Dim rectDocumentWindow = RectangleToClient(DockPanel.RectangleToScreen(DockPanel.DocumentWindowBounds))
                        PanelFill.Location = New Point(CInt(rectDocumentWindow.X + (rectDocumentWindow.Width - PanelFill.Width) / 2), CInt(rectDocumentWindow.Y + (rectDocumentWindow.Height - PanelFill.Height) / 2))
                        PanelFill.Visible = True
                        region.Union(PanelFill.Bounds)
                    Else
                        PanelFill.Visible = False
                    End If

                    If ShouldPaneDiamondVisible() Then
                        Dim rect = RectangleToClient(DockPane.RectangleToScreen(DockPane.ClientRectangle))
                        PaneDiamond.Location = New Point(CInt(rect.Left + (rect.Width - PaneDiamond.Width) / 2), CInt(rect.Top + (rect.Height - PaneDiamond.Height) / 2))
                        PaneDiamond.Visible = True
                        Using graphicsPath As GraphicsPath = TryCast(PaneDiamond.DisplayingGraphicsPath.Clone(), GraphicsPath)
                            Dim pts As Point() = {New Point(PaneDiamond.Left, PaneDiamond.Top), New Point(PaneDiamond.Right, PaneDiamond.Top), New Point(PaneDiamond.Left, PaneDiamond.Bottom)}
                            Using matrix As Matrix = New Matrix(PaneDiamond.ClientRectangle, pts)
                                graphicsPath.Transform(matrix)
                            End Using

                            region.Union(graphicsPath)
                        End Using
                    Else
                        PaneDiamond.Visible = False
                    End If

                    MyBase.Region = region
                End Sub

                Private Function ShouldPanelIndicatorVisible(dockState As DockState) As Boolean
                    If Not Visible Then Return False

                    If DockPanel.DockWindows(dockState).Visible Then Return False

                    Return DragHandler.DragSource.IsDockStateValid(dockState)
                End Function

                Private Function ShouldPaneDiamondVisible() As Boolean
                    If DockPane Is Nothing Then Return False

                    If Not DockPanel.AllowEndUserNestedDocking Then Return False

                    Return DragHandler.DragSource.CanDockTo(DockPane)
                End Function

                Public Overrides Sub Show(bActivate As Boolean)
                    MyBase.Show(bActivate)
                    If EnablePerScreenDpi <> True Then
                        Bounds = SystemInformation.VirtualScreen
                    End If

                    RefreshChanges()
                End Sub

                Public Sub TestDrop()
                    Dim pt = MousePosition
                    DockPane = PaneAtPoint(pt, DockPanel)

                    If TestDrop(PanelLeft, pt) <> DockStyle.None Then
                        HitTestResult = PanelLeft
                    ElseIf TestDrop(PanelRight, pt) <> DockStyle.None Then
                        HitTestResult = PanelRight
                    ElseIf TestDrop(PanelTop, pt) <> DockStyle.None Then
                        HitTestResult = PanelTop
                    ElseIf TestDrop(PanelBottom, pt) <> DockStyle.None Then
                        HitTestResult = PanelBottom
                    ElseIf TestDrop(PanelFill, pt) <> DockStyle.None Then
                        HitTestResult = PanelFill
                    ElseIf TestDrop(PaneDiamond, pt) <> DockStyle.None Then
                        HitTestResult = PaneDiamond
                    Else
                        HitTestResult = Nothing
                    End If

                    If HitTestResult IsNot Nothing Then
                        If TypeOf HitTestResult Is IPaneIndicator Then
                            DragHandler.Outline.Show(DockPane, HitTestResult.Status)
                        Else
                            DragHandler.Outline.Show(DockPanel, HitTestResult.Status, FullPanelEdge)
                        End If
                    End If
                End Sub

                Private Shared Function TestDrop(hitTest As IHitTest, pt As Point) As DockStyle
                    hitTest.Status = hitTest.HitTest(pt)
                    Return hitTest.Status
                End Function
            End Class

            Public Sub New(panel As DockPanel)
                MyBase.New(panel)
            End Sub

            Public Overloads Property DragSource As IDockDragSource
                Get
                    Return TryCast(MyBase.DragSource, IDockDragSource)
                End Get
                Set(value As IDockDragSource)
                    MyBase.DragSource = value
                End Set
            End Property

            Private m_outline As DockOutlineBase
            Public Property Outline As DockOutlineBase
                Get
                    Return m_outline
                End Get
                Private Set(value As DockOutlineBase)
                    m_outline = value
                End Set
            End Property

            Private m_indicator As DockIndicator
            Private Property Indicator As DockIndicator
                Get
                    Return m_indicator
                End Get
                Set(value As DockIndicator)
                    m_indicator = value
                End Set
            End Property

            Private m_floatOutlineBounds As Rectangle
            Private Property FloatOutlineBounds As Rectangle
                Get
                    Return m_floatOutlineBounds
                End Get
                Set(value As Rectangle)
                    m_floatOutlineBounds = value
                End Set
            End Property

            Public Overloads Sub BeginDrag(dragSource As IDockDragSource)
                Me.DragSource = dragSource

                If Not MyBase.BeginDrag() Then
                    Me.DragSource = Nothing
                    Return
                End If

                Outline = DockPanel.Theme.Extender.DockOutlineFactory.CreateDockOutline()
                Indicator = DockPanel.Theme.Extender.DockIndicatorFactory.CreateDockIndicator(Me)
                Indicator.Show(False)

                FloatOutlineBounds = Me.DragSource.BeginDrag(StartMousePosition)
            End Sub

            Protected Overrides Sub OnDragging()
                TestDrop()
            End Sub

            Protected Overrides Sub OnEndDrag(abort As Boolean)
                DockPanel.SuspendLayout(True)

                Outline.Close()
                Indicator.Close()

                EndDrag(abort)

                ' Queue a request to layout all children controls
                DockPanel.PerformMdiClientLayout()

                DockPanel.ResumeLayout(True, True)

                DragSource.EndDrag()

                DragSource = Nothing

                ' Fire notification
                DockPanel.OnDocumentDragged()
            End Sub

            Private Sub TestDrop()
                Outline.FlagTestDrop = False

                Indicator.FullPanelEdge = (ModifierKeys And Keys.Shift) <> 0

                If (ModifierKeys And Keys.Control) = 0 Then
                    Indicator.TestDrop()

                    If Not Outline.FlagTestDrop Then
                        Dim pane = PaneAtPoint(MousePosition, DockPanel)
                        If pane IsNot Nothing AndAlso DragSource.IsDockStateValid(pane.DockState) Then pane.TestDrop(DragSource, Outline)
                    End If

                    If Not Outline.FlagTestDrop AndAlso DragSource.IsDockStateValid(DockState.Float) Then
                        Dim floatWindow = FloatWindowAtPoint(MousePosition, DockPanel)
                        If floatWindow IsNot Nothing Then floatWindow.TestDrop(DragSource, Outline)
                    End If
                Else
                    Indicator.DockPane = PaneAtPoint(MousePosition, DockPanel)
                End If

                If Not Outline.FlagTestDrop Then
                    If DragSource.IsDockStateValid(DockState.Float) Then
                        Dim rect = FloatOutlineBounds
                        rect.Offset(MousePosition.X - StartMousePosition.X, MousePosition.Y - StartMousePosition.Y)
                        Outline.Show(rect)
                    End If
                End If

                If Not Outline.FlagTestDrop Then
                    Cursor.Current = Cursors.No
                    Outline.Show()
                Else
                    Cursor.Current = DragControl.Cursor
                End If
            End Sub

            Private Sub EndDrag(abort As Boolean)
                If abort Then Return

                If Not Outline.FloatWindowBounds.IsEmpty Then
                    DragSource.FloatAt(Outline.FloatWindowBounds)
                ElseIf TypeOf Outline.DockTo Is DockPane Then
                    Dim pane As DockPane = TryCast(Outline.DockTo, DockPane)
                    DragSource.DockTo(pane, Outline.Dock, Outline.ContentIndex)
                ElseIf TypeOf Outline.DockTo Is DockPanel Then
                    Dim panel As DockPanel = TryCast(Outline.DockTo, DockPanel)
                    panel.UpdateDockWindowZOrder(Outline.Dock, Outline.FlagFullEdge)
                    DragSource.DockTo(panel, Outline.Dock)
                End If
            End Sub
        End Class

        Private m_dockDragHandler As DockDragHandler = Nothing
        Private Function GetDockDragHandler() As DockDragHandler
            If m_dockDragHandler Is Nothing Then m_dockDragHandler = New DockDragHandler(Me)
            Return m_dockDragHandler
        End Function

        Friend Sub BeginDrag(dragSource As IDockDragSource)
            GetDockDragHandler().BeginDrag(dragSource)
        End Sub
    End Class
End Namespace
