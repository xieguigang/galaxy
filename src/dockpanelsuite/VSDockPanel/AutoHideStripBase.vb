Imports System.Diagnostics.CodeAnalysis
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms

Namespace Docking

    Public MustInherit Class AutoHideStripBase : Inherits Control

        Private _DockPanel As DockPanel,
            _PanesTop As AutoHideStripBase.PaneCollection,
            _PanesBottom As AutoHideStripBase.PaneCollection,
            _PanesLeft As AutoHideStripBase.PaneCollection,
            _PanesRight As AutoHideStripBase.PaneCollection

        <SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")>
        Protected Class Tab
            Implements IDisposable
            Private m_content As IDockContent

            Protected Friend Sub New(content As IDockContent)
                m_content = content
            End Sub

            Protected Overrides Sub Finalize()
                Dispose(False)
            End Sub

            Public ReadOnly Property Content As IDockContent
                Get
                    Return m_content
                End Get
            End Property

            Public Sub Dispose() Implements IDisposable.Dispose
                Dispose(True)
                GC.SuppressFinalize(Me)
            End Sub

            Protected Overridable Sub Dispose(disposing As Boolean)
            End Sub
        End Class

        <SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")>
        Protected NotInheritable Class TabCollection
            Implements IEnumerable(Of Tab)
#Region "IEnumerable Members"
            Private Iterator Function GetEnumerator1() As IEnumerator(Of Tab) Implements IEnumerable(Of Tab).GetEnumerator
                For i = 0 To Count - 1
                    Yield Me(i)
                Next
            End Function

            Private Iterator Function GetEnumerator2() As IEnumerator Implements IEnumerable.GetEnumerator
                For i = 0 To Count - 1
                    Yield Me(i)
                Next
            End Function
#End Region

            Friend Sub New(pane As DockPane)
                m_dockPane = pane
            End Sub

            Private m_dockPane As DockPane = Nothing
            Public ReadOnly Property DockPane As DockPane
                Get
                    Return m_dockPane
                End Get
            End Property

            Public ReadOnly Property DockPanel As DockPanel
                Get
                    Return DockPane.DockPanel
                End Get
            End Property

            Public ReadOnly Property Count As Integer
                Get
                    Return DockPane.DisplayingContents.Count
                End Get
            End Property

            Default Public ReadOnly Property Item(index As Integer) As Tab
                Get
                    Dim content = DockPane.DisplayingContents(index)
                    If content Is Nothing Then Throw New ArgumentOutOfRangeException(NameOf(index))
                    If content.DockHandler.AutoHideTab Is Nothing Then content.DockHandler.AutoHideTab = DockPanel.AutoHideStripControl.CreateTab(content)
                    Return TryCast(content.DockHandler.AutoHideTab, Tab)
                End Get
            End Property

            Public Function Contains(tab As Tab) As Boolean
                Return IndexOf(tab) <> -1
            End Function

            Public Function Contains(content As IDockContent) As Boolean
                Return IndexOf(content) <> -1
            End Function

            Public Function IndexOf(tab As Tab) As Integer
                If tab Is Nothing Then Return -1

                Return IndexOf(tab.Content)
            End Function

            Public Function IndexOf(content As IDockContent) As Integer
                Return DockPane.DisplayingContents.IndexOf(content)
            End Function
        End Class

        <SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")>
        Protected Class Pane
            Implements IDisposable
            Private m_dockPane As DockPane

            Protected Friend Sub New(dockPane As DockPane)
                m_dockPane = dockPane
            End Sub

            Protected Overrides Sub Finalize()
                Dispose(False)
            End Sub

            Public ReadOnly Property DockPane As DockPane
                Get
                    Return m_dockPane
                End Get
            End Property

            Public ReadOnly Property AutoHideTabs As TabCollection
                Get
                    If DockPane.AutoHideTabs Is Nothing Then DockPane.AutoHideTabs = New TabCollection(DockPane)
                    Return TryCast(DockPane.AutoHideTabs, TabCollection)
                End Get
            End Property

            Public Sub Dispose() Implements IDisposable.Dispose
                Dispose(True)
                GC.SuppressFinalize(Me)
            End Sub

            Protected Overridable Sub Dispose(disposing As Boolean)
            End Sub
        End Class

        <SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")>
        Protected NotInheritable Class PaneCollection
            Implements IEnumerable(Of Pane)
            Private Class AutoHideState
                Public m_dockState As DockState
                Public m_selected As Boolean = False

                Public Sub New(dockState As DockState)
                    m_dockState = dockState
                End Sub

                Public ReadOnly Property DockState As DockState
                    Get
                        Return m_dockState
                    End Get
                End Property

                Public Property Selected As Boolean
                    Get
                        Return m_selected
                    End Get
                    Set(value As Boolean)
                        m_selected = value
                    End Set
                End Property
            End Class

            Private Class AutoHideStateCollection
                Private m_states As AutoHideState()

                Public Sub New()
                    m_states = {New AutoHideState(DockState.DockTopAutoHide), New AutoHideState(DockState.DockBottomAutoHide), New AutoHideState(DockState.DockLeftAutoHide), New AutoHideState(DockState.DockRightAutoHide)}
                End Sub

                Default Public ReadOnly Property Item(dockState As DockState) As AutoHideState
                    Get
                        For i = 0 To m_states.Length - 1
                            If m_states(i).DockState = dockState Then Return m_states(i)
                        Next
                        Throw New ArgumentOutOfRangeException(NameOf(dockState))
                    End Get
                End Property

                Public Function ContainsPane(pane As DockPane) As Boolean
                    If pane.IsHidden Then Return False

                    For i = 0 To m_states.Length - 1
                        If m_states(i).DockState = pane.DockState AndAlso m_states(i).Selected Then Return True
                    Next
                    Return False
                End Function
            End Class

            Friend Sub New(panel As DockPanel, dockState As DockState)
                m_dockPanel = panel
                m_states = New AutoHideStateCollection()
                States(DockState.DockTopAutoHide).Selected = dockState = DockState.DockTopAutoHide
                States(DockState.DockBottomAutoHide).Selected = dockState = DockState.DockBottomAutoHide
                States(DockState.DockLeftAutoHide).Selected = dockState = DockState.DockLeftAutoHide
                States(DockState.DockRightAutoHide).Selected = dockState = DockState.DockRightAutoHide
            End Sub

            Private m_dockPanel As DockPanel
            Public ReadOnly Property DockPanel As DockPanel
                Get
                    Return m_dockPanel
                End Get
            End Property

            Private m_states As AutoHideStateCollection
            Private ReadOnly Property States As AutoHideStateCollection
                Get
                    Return m_states
                End Get
            End Property

            Public ReadOnly Property Count As Integer
                Get
                    Dim lCount = 0
                    For Each pane In DockPanel.Panes
                        If States.ContainsPane(pane) Then lCount += 1
                    Next

                    Return lCount
                End Get
            End Property

            Default Public ReadOnly Property Item(index As Integer) As Pane
                Get
                    Dim count = 0
                    For Each pane In DockPanel.Panes
                        If Not States.ContainsPane(pane) Then Continue For

                        If count = index Then
                            If pane.AutoHidePane Is Nothing Then pane.AutoHidePane = DockPanel.AutoHideStripControl.CreatePane(pane)
                            Return TryCast(pane.AutoHidePane, Pane)
                        End If

                        count += 1
                    Next
                    Throw New ArgumentOutOfRangeException(NameOf(index))
                End Get
            End Property

            Public Function Contains(pane As Pane) As Boolean
                Return IndexOf(pane) <> -1
            End Function

            Public Function IndexOf(pane As Pane) As Integer
                If pane Is Nothing Then Return -1

                Dim index = 0
                For Each dockPane In DockPanel.Panes
                    If Not States.ContainsPane(pane.DockPane) Then Continue For

                    If pane Is dockPane.AutoHidePane Then Return index

                    index += 1
                Next
                Return -1
            End Function

#Region "IEnumerable Members"

            Private Iterator Function GetEnumerator3() As IEnumerator(Of Pane) Implements IEnumerable(Of Pane).GetEnumerator
                For i = 0 To Count - 1
                    Yield Me(i)
                Next
            End Function

            Private Iterator Function GetEnumerator4() As IEnumerator Implements IEnumerable.GetEnumerator
                For i = 0 To Count - 1
                    Yield Me(i)
                Next
            End Function

#End Region
        End Class

        Protected Sub New(panel As DockPanel)
            DockPanel = panel
            PanesTop = New PaneCollection(panel, DockState.DockTopAutoHide)
            PanesBottom = New PaneCollection(panel, DockState.DockBottomAutoHide)
            PanesLeft = New PaneCollection(panel, DockState.DockLeftAutoHide)
            PanesRight = New PaneCollection(panel, DockState.DockRightAutoHide)

            SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
            SetStyle(ControlStyles.Selectable, False)
        End Sub

        Protected Property DockPanel As DockPanel
            Get
                Return _DockPanel
            End Get
            Private Set(value As DockPanel)
                _DockPanel = value
            End Set
        End Property

        Protected Property PanesTop As PaneCollection
            Get
                Return _PanesTop
            End Get
            Private Set(value As PaneCollection)
                _PanesTop = value
            End Set
        End Property

        Protected Property PanesBottom As PaneCollection
            Get
                Return _PanesBottom
            End Get
            Private Set(value As PaneCollection)
                _PanesBottom = value
            End Set
        End Property

        Protected Property PanesLeft As PaneCollection
            Get
                Return _PanesLeft
            End Get
            Private Set(value As PaneCollection)
                _PanesLeft = value
            End Set
        End Property

        Protected Property PanesRight As PaneCollection
            Get
                Return _PanesRight
            End Get
            Private Set(value As PaneCollection)
                _PanesRight = value
            End Set
        End Property

        Protected Function GetPanes(dockState As DockState) As PaneCollection
            If dockState = DockState.DockTopAutoHide Then
                Return PanesTop
            ElseIf dockState = DockState.DockBottomAutoHide Then
                Return PanesBottom
            ElseIf dockState = DockState.DockLeftAutoHide Then
                Return PanesLeft
            ElseIf dockState = DockState.DockRightAutoHide Then
                Return PanesRight
            Else
                Throw New ArgumentOutOfRangeException(NameOf(dockState))
            End If
        End Function

        Friend Function GetNumberOfPanes(dockState As DockState) As Integer
            Return GetPanes(dockState).Count
        End Function

        ''' <summary>
        ''' The top left rectangle in auto hide strip area.
        ''' </summary>
        Protected ReadOnly Property RectangleTopLeft As Rectangle
            Get
                Dim standard As Integer = MeasureHeight()
                Dim padding = DockPanel.Theme.Measures.DockPadding
                Dim width = If(PanesLeft.Count > 0, standard, padding)
                Dim height = If(PanesTop.Count > 0, standard, padding)
                Return New Rectangle(0, 0, width, height)
            End Get
        End Property

        ''' <summary>
        ''' The top right rectangle in auto hide strip area.
        ''' </summary>
        Protected ReadOnly Property RectangleTopRight As Rectangle
            Get
                Dim standard As Integer = MeasureHeight()
                Dim padding = DockPanel.Theme.Measures.DockPadding
                Dim width = If(PanesRight.Count > 0, standard, padding)
                Dim height = If(PanesTop.Count > 0, standard, padding)
                Return New Rectangle(MyBase.Width - width, 0, width, height)
            End Get
        End Property

        ''' <summary>
        ''' The bottom left rectangle in auto hide strip area.
        ''' </summary>
        Protected ReadOnly Property RectangleBottomLeft As Rectangle
            Get
                Dim standard As Integer = MeasureHeight()
                Dim padding = DockPanel.Theme.Measures.DockPadding
                Dim width = If(PanesLeft.Count > 0, standard, padding)
                Dim height = If(PanesBottom.Count > 0, standard, padding)
                Return New Rectangle(0, MyBase.Height - height, width, height)
            End Get
        End Property

        ''' <summary>
        ''' The bottom right rectangle in auto hide strip area.
        ''' </summary>
        Protected ReadOnly Property RectangleBottomRight As Rectangle
            Get
                Dim standard As Integer = MeasureHeight()
                Dim padding = DockPanel.Theme.Measures.DockPadding
                Dim width = If(PanesRight.Count > 0, standard, padding)
                Dim height = If(PanesBottom.Count > 0, standard, padding)
                Return New Rectangle(MyBase.Width - width, MyBase.Height - height, width, height)
            End Get
        End Property

        ''' <summary>
        ''' Gets one of the four auto hide strip rectangles.
        ''' </summary>
        ''' <param name="dockState">Dock state.</param>
        ''' <returns>The desired rectangle.</returns>
        ''' <remarks>
        ''' As the corners are represented by <see cref="RectangleTopLeft"/>, <see cref="RectangleTopRight"/>, <see cref="RectangleBottomLeft"/>, and <see cref="RectangleBottomRight"/>,
        ''' the four strips can be easily calculated out as the borders.
        ''' </remarks>
        Protected Friend Function GetTabStripRectangle(dockState As DockState) As Rectangle
            If dockState = DockState.DockTopAutoHide Then Return New Rectangle(RectangleTopLeft.Width, 0, Width - RectangleTopLeft.Width - RectangleTopRight.Width, RectangleTopLeft.Height)

            If dockState = DockState.DockBottomAutoHide Then Return New Rectangle(RectangleBottomLeft.Width, Height - RectangleBottomLeft.Height, Width - RectangleBottomLeft.Width - RectangleBottomRight.Width, RectangleBottomLeft.Height)

            If dockState = DockState.DockLeftAutoHide Then Return New Rectangle(0, RectangleTopLeft.Height, RectangleTopLeft.Width, Height - RectangleTopLeft.Height - RectangleBottomLeft.Height)

            If dockState = DockState.DockRightAutoHide Then Return New Rectangle(Width - RectangleTopRight.Width, RectangleTopRight.Height, RectangleTopRight.Width, Height - RectangleTopRight.Height - RectangleBottomRight.Height)

            Return Rectangle.Empty
        End Function

        Private m_displayingArea As GraphicsPath

        Private ReadOnly Property DisplayingArea As GraphicsPath
            Get
                If m_displayingArea Is Nothing Then m_displayingArea = New GraphicsPath()

                Return m_displayingArea
            End Get
        End Property

        Private Sub SetRegion()
            DisplayingArea.Reset()
            DisplayingArea.AddRectangle(RectangleTopLeft)
            DisplayingArea.AddRectangle(RectangleTopRight)
            DisplayingArea.AddRectangle(RectangleBottomLeft)
            DisplayingArea.AddRectangle(RectangleBottomRight)
            DisplayingArea.AddRectangle(GetTabStripRectangle(DockState.DockTopAutoHide))
            DisplayingArea.AddRectangle(GetTabStripRectangle(DockState.DockBottomAutoHide))
            DisplayingArea.AddRectangle(GetTabStripRectangle(DockState.DockLeftAutoHide))
            DisplayingArea.AddRectangle(GetTabStripRectangle(DockState.DockRightAutoHide))
            Region = New Region(DisplayingArea)
        End Sub

        Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
            MyBase.OnMouseDown(e)

            If e.Button <> MouseButtons.Left Then Return

            Dim content As IDockContent = HitTest()
            If content Is Nothing Then Return

            SetActiveAutoHideContent(content)

            content.DockHandler.Activate()
        End Sub

        Protected Overrides Sub OnMouseHover(e As EventArgs)
            MyBase.OnMouseHover(e)

            If Not DockPanel.ShowAutoHideContentOnHover Then Return

            ' IMPORTANT: VS2003/2005 themes only.
            Dim content As IDockContent = HitTest()
            SetActiveAutoHideContent(content)

            ' requires further tracking of mouse hover behavior,
            ResetMouseEventArgs()
        End Sub

        Private Sub SetActiveAutoHideContent(content As IDockContent)
            If content IsNot Nothing Then
                If DockPanel.ActiveAutoHideContent IsNot content Then
                    DockPanel.ActiveAutoHideContent = content
                ElseIf Not DockPanel.ShowAutoHideContentOnHover Then
                    DockPanel.ActiveAutoHideContent = Nothing
                End If
            End If ' IMPORTANT: Not needed for VS2003/2005 themes.
        End Sub

        Protected Overrides Sub OnLayout(levent As LayoutEventArgs)
            RefreshChanges()
            MyBase.OnLayout(levent)
        End Sub

        Friend Sub RefreshChanges()
            If IsDisposed Then Return

            SetRegion()
            OnRefreshChanges()
        End Sub

        Protected Overridable Sub OnRefreshChanges()
        End Sub

        Protected Friend MustOverride Function MeasureHeight() As Integer

        Private Function HitTest() As IDockContent
            Dim ptMouse = PointToClient(MousePosition)
            Return HitTest(ptMouse)
        End Function

        Protected Overridable Function CreateTab(content As IDockContent) As Tab
            Return New Tab(content)
        End Function

        Protected Overridable Function CreatePane(dockPane As DockPane) As Pane
            Return New Pane(dockPane)
        End Function

        Protected MustOverride Function HitTest(point As Point) As IDockContent

        Protected Overrides Function CreateAccessibilityInstance() As AccessibleObject
            Return New AutoHideStripsAccessibleObject(Me)
        End Function

        Protected MustOverride Function GetTabBounds(tab As Tab) As Rectangle

        Friend Shared Function ToScreen(rectangle As Rectangle, parent As Control) As Rectangle
            If parent Is Nothing Then Return rectangle

            Return New Rectangle(parent.PointToScreen(New Point(rectangle.Left, rectangle.Top)), New Size(rectangle.Width, rectangle.Height))
        End Function

        Public Class AutoHideStripsAccessibleObject
            Inherits ControlAccessibleObject
            Private _strip As AutoHideStripBase

            Public Sub New(strip As AutoHideStripBase)
                MyBase.New(strip)
                _strip = strip
            End Sub

            Public Overrides ReadOnly Property Role As AccessibleRole
                Get
                    Return AccessibleRole.Window
                End Get
            End Property

            Public Overrides Function GetChildCount() As Integer
                ' Top, Bottom, Left, Right
                Return 4
            End Function

            Public Overrides Function GetChild(index As Integer) As AccessibleObject
                Select Case index
                    Case 0
                        Return New AutoHideStripAccessibleObject(_strip, DockState.DockTopAutoHide, Me)
                    Case 1
                        Return New AutoHideStripAccessibleObject(_strip, DockState.DockBottomAutoHide, Me)
                    Case 2
                        Return New AutoHideStripAccessibleObject(_strip, DockState.DockLeftAutoHide, Me)
                    Case Else
                        Return New AutoHideStripAccessibleObject(_strip, DockState.DockRightAutoHide, Me)
                End Select
            End Function

            Public Overrides Function HitTest(x As Integer, y As Integer) As AccessibleObject
                Dim rectangles As Dictionary(Of DockState, Rectangle) = New Dictionary(Of DockState, Rectangle) From {
                    {DockState.DockTopAutoHide, _strip.GetTabStripRectangle(DockState.DockTopAutoHide)},
                    {DockState.DockBottomAutoHide, _strip.GetTabStripRectangle(DockState.DockBottomAutoHide)},
                    {DockState.DockLeftAutoHide, _strip.GetTabStripRectangle(DockState.DockLeftAutoHide)},
                    {DockState.DockRightAutoHide, _strip.GetTabStripRectangle(DockState.DockRightAutoHide)}
                }

                Dim point As Point = _strip.PointToClient(New Point(x, y))
                For Each rectangle In rectangles
                    If rectangle.Value.Contains(point) Then Return New AutoHideStripAccessibleObject(_strip, rectangle.Key, Me)
                Next

                Return Nothing
            End Function
        End Class

        Public Class AutoHideStripAccessibleObject
            Inherits AccessibleObject
            Private _strip As AutoHideStripBase
            Private _state As DockState
            Private _parent As AccessibleObject

            Public Sub New(strip As AutoHideStripBase, state As DockState, parent As AccessibleObject)
                _strip = strip
                _state = state

                _parent = parent
            End Sub

            Public Overrides ReadOnly Property Parent As AccessibleObject
                Get
                    Return _parent
                End Get
            End Property

            Public Overrides ReadOnly Property Role As AccessibleRole
                Get
                    Return AccessibleRole.PageTabList
                End Get
            End Property

            Public Overrides Function GetChildCount() As Integer
                Dim count = 0
                For Each pane In _strip.GetPanes(_state)
                    count += pane.AutoHideTabs.Count
                Next
                Return count
            End Function

            Public Overrides Function GetChild(index As Integer) As AccessibleObject
                Dim tabs As List(Of Tab) = New List(Of Tab)()
                For Each pane In _strip.GetPanes(_state)
                    tabs.AddRange(pane.AutoHideTabs)
                Next

                Return New AutoHideStripTabAccessibleObject(_strip, tabs(index), Me)
            End Function

            Public Overrides ReadOnly Property Bounds As Rectangle
                Get
                    Dim rectangle = _strip.GetTabStripRectangle(_state)
                    Return ToScreen(rectangle, _strip)
                End Get
            End Property
        End Class

        Protected Class AutoHideStripTabAccessibleObject
            Inherits AccessibleObject
            Private _strip As AutoHideStripBase
            Private _tab As Tab

            Private _parent As AccessibleObject

            Friend Sub New(strip As AutoHideStripBase, tab As Tab, parent As AccessibleObject)
                _strip = strip
                _tab = tab

                _parent = parent
            End Sub

            Public Overrides ReadOnly Property Parent As AccessibleObject
                Get
                    Return _parent
                End Get
            End Property

            Public Overrides ReadOnly Property Role As AccessibleRole
                Get
                    Return AccessibleRole.PageTab
                End Get
            End Property

            Public Overrides ReadOnly Property Bounds As Rectangle
                Get
                    Dim rectangle = _strip.GetTabBounds(_tab)
                    Return ToScreen(rectangle, _strip)
                End Get
            End Property

            Public Overrides Property Name As String
                Get
                    Return _tab.Content.DockHandler.TabText
                End Get
                Set(value As String)
                    'base.Name = value;
                End Set
            End Property
        End Class
    End Class
End Namespace
