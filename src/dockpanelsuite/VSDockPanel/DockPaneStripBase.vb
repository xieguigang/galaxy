Imports System
Imports System.Windows.Forms
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Collections
Imports System.Collections.Generic
Imports System.Security.Permissions
Imports System.Diagnostics.CodeAnalysis

Namespace WeifenLuo.WinFormsUI.Docking
    Public MustInherit Class DockPaneStripBase
        Inherits Control
        <SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")>
        Protected Friend Class Tab
            Implements IDisposable
            Private m_content As IDockContent

            Public Sub New(content As IDockContent)
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

            Public ReadOnly Property ContentForm As Form
                Get
                    Return TryCast(m_content, Form)
                End Get
            End Property

            Public Sub Dispose() Implements IDisposable.Dispose
                Dispose(True)
                GC.SuppressFinalize(Me)
            End Sub

            Protected Overridable Sub Dispose(disposing As Boolean)
            End Sub

            Private _rect As Rectangle?

            Public Property Rectangle As Rectangle?
                Get
                    If _rect IsNot Nothing Then
                        Return _rect
                    End If

                    _rect = Drawing.Rectangle.Empty

                    Return Drawing.Rectangle.Empty
                End Get

                Set(value As Rectangle?)
                    _rect = value
                End Set
            End Property
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

            Private m_dockPane As DockPane
            Public ReadOnly Property DockPane As DockPane
                Get
                    Return m_dockPane
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
                    If content Is Nothing Then Throw (New ArgumentOutOfRangeException(NameOf(index)))
                    Return content.DockHandler.GetTab(DockPane.TabStripControl)
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

                Return DockPane.DisplayingContents.IndexOf(tab.Content)
            End Function

            Public Function IndexOf(content As IDockContent) As Integer
                Return DockPane.DisplayingContents.IndexOf(content)
            End Function
        End Class

        Protected Sub New(pane As DockPane)
            m_dockPane = pane

            SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
            SetStyle(ControlStyles.Selectable, False)
            MyBase.AllowDrop = True
        End Sub

        Private m_dockPane As DockPane
        Protected ReadOnly Property DockPane As DockPane
            Get
                Return m_dockPane
            End Get
        End Property

        Protected ReadOnly Property Appearance As DockPane.AppearanceStyle
            Get
                Return DockPane.Appearance
            End Get
        End Property

        Private m_tabs As TabCollection

        Protected ReadOnly Property Tabs As TabCollection
            Get
                Return If(m_tabs, Function()
                                      m_tabs = New TabCollection(DockPane)
                                      Return m_tabs
                                  End Function())
            End Get
        End Property

        Friend Sub RefreshChanges()
            If IsDisposed Then Return

            OnRefreshChanges()
        End Sub

        Protected Overridable Sub OnRefreshChanges()
        End Sub

        Protected Friend MustOverride Function MeasureHeight() As Integer

        Protected Friend MustOverride Sub EnsureTabVisible(content As IDockContent)

        Protected Function HitTest() As Integer
            Return HitTest(PointToClient(MousePosition))
        End Function

        Protected Friend MustOverride Function HitTest(point As Point) As Integer

        Protected Overridable Function MouseDownActivateTest(e As MouseEventArgs) As Boolean
            Return True
        End Function

        Public MustOverride Function GetOutline(index As Integer) As GraphicsPath

        Protected Friend Overridable Function CreateTab(content As IDockContent) As Tab
            Return New Tab(content)
        End Function

        Private _dragBox As Rectangle = Rectangle.Empty
        Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
            MyBase.OnMouseDown(e)
            Dim index As Integer = HitTest()
            If index <> -1 Then
                If e.Button = MouseButtons.Middle Then
                    ' Close the specified content.
                    TryCloseTab(index)
                Else
                    Dim content = Tabs(index).Content
                    If DockPane.ActiveContent IsNot content Then
                        ' Test if the content should be active
                        If MouseDownActivateTest(e) Then DockPane.ActiveContent = content
                    End If

                End If
            End If

            If e.Button = MouseButtons.Left Then
                Dim dragSize = SystemInformation.DragSize
                _dragBox = New Rectangle(New Point(CInt(e.X - dragSize.Width / 2), CInt(e.Y - dragSize.Height / 2)), dragSize)
            End If
        End Sub

        Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
            MyBase.OnMouseMove(e)

            If e.Button <> MouseButtons.Left OrElse _dragBox.Contains(e.Location) Then Return

            If DockPane.ActiveContent Is Nothing Then Return

            If DockPane.DockPanel.AllowEndUserDocking AndAlso DockPane.AllowDockDragAndDrop AndAlso DockPane.ActiveContent.DockHandler.AllowEndUserDocking Then DockPane.DockPanel.BeginDrag(DockPane.ActiveContent.DockHandler)
        End Sub

        Protected ReadOnly Property HasTabPageContextMenu As Boolean
            Get
                Return DockPane.HasTabPageContextMenu
            End Get
        End Property

        Protected Sub ShowTabPageContextMenu(position As Point)
            DockPane.ShowTabPageContextMenu(Me, position)
        End Sub

        Protected Function TryCloseTab(index As Integer) As Boolean
            If index >= 0 OrElse index < Tabs.Count Then
                ' Close the specified content.
                Dim content = Tabs(index).Content
                DockPane.CloseContent(content)
                If EnableSelectClosestOnClose = True Then SelectClosestPane(index)

                Return True
            End If
            Return False
        End Function

        Private Sub SelectClosestPane(index As Integer)
            If index > 0 AndAlso DockPane.DockPanel.DocumentStyle = DocumentStyle.DockingWindow Then
                index = index - 1

                If index >= 0 OrElse index < Tabs.Count Then DockPane.ActiveContent = Tabs(index).Content

            End If
        End Sub

        Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
            MyBase.OnMouseUp(e)

            If e.Button = MouseButtons.Right Then ShowTabPageContextMenu(New Point(e.X, e.Y))
        End Sub

        <SecurityPermission(SecurityAction.LinkDemand, Flags:=SecurityPermissionFlag.UnmanagedCode)>
        Protected Overrides Sub WndProc(ByRef m As Message)
            If m.Msg = Win32.Msgs.WM_LBUTTONDBLCLK Then
                MyBase.WndProc(m)

                Dim index As Integer = HitTest()
                If DockPane.DockPanel.AllowEndUserDocking AndAlso index <> -1 Then
                    Dim content = Tabs(index).Content
                    If content.DockHandler.CheckDockState(Not content.DockHandler.IsFloat) <> DockState.Unknown Then content.DockHandler.IsFloat = Not content.DockHandler.IsFloat
                End If

                Return
            End If

            MyBase.WndProc(m)
            Return
        End Sub

        Protected Overrides Sub OnDragOver(drgevent As DragEventArgs)
            MyBase.OnDragOver(drgevent)

            Dim index As Integer = HitTest()
            If index <> -1 Then
                Dim content = Tabs(index).Content
                If DockPane.ActiveContent IsNot content Then DockPane.ActiveContent = content
            End If
        End Sub

        Protected Sub ContentClosed()
            If m_tabs.Count = 0 Then
                DockPane.ClearLastActiveContent()
            End If
        End Sub

        Protected MustOverride Function GetTabBounds(tab As Tab) As Rectangle

        Friend Shared Function ToScreen(rectangle As Rectangle, parent As Control) As Rectangle
            If parent Is Nothing Then Return rectangle

            Return New Rectangle(parent.PointToScreen(New Point(rectangle.Left, rectangle.Top)), New Size(rectangle.Width, rectangle.Height))
        End Function

        Protected Overrides Function CreateAccessibilityInstance() As AccessibleObject
            Return New DockPaneStripAccessibleObject(Me)
        End Function

        Public Class DockPaneStripAccessibleObject
            Inherits ControlAccessibleObject
            Private _strip As DockPaneStripBase

            Public Sub New(strip As DockPaneStripBase)
                MyBase.New(strip)
                _strip = strip
            End Sub

            Public Overrides ReadOnly Property Role As AccessibleRole
                Get
                    Return AccessibleRole.PageTabList
                End Get
            End Property

            Public Overrides Function GetChildCount() As Integer
                Return _strip.Tabs.Count
            End Function

            Public Overrides Function GetChild(index As Integer) As AccessibleObject
                Return New DockPaneStripTabAccessibleObject(_strip, _strip.Tabs(index), Me)
            End Function

            Public Overrides Function HitTest(x As Integer, y As Integer) As AccessibleObject
                Dim point As Point = New Point(x, y)
                For Each tab In _strip.Tabs
                    Dim rectangle = _strip.GetTabBounds(tab)
                    If ToScreen(rectangle, _strip).Contains(point) Then Return New DockPaneStripTabAccessibleObject(_strip, tab, Me)
                Next

                Return Nothing
            End Function
        End Class

        Protected Class DockPaneStripTabAccessibleObject
            Inherits AccessibleObject
            Private _strip As DockPaneStripBase
            Private _tab As Tab

            Private _parent As AccessibleObject

            Friend Sub New(strip As DockPaneStripBase, tab As Tab, parent As AccessibleObject)
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
