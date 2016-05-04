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

Imports Microsoft.Windows.Dialogs
Imports Microsoft.Windows.Taskbar

Namespace ChromeTabControl

    ''' <summary>
    ''' Pages that work with the TabControl.
    ''' </summary>
    <System.ComponentModel.ToolboxItem(False)>
    Public Class TabPage
        Inherits UserControl

        Friend _tabControl As ChromeTabControl

        ''' <summary>
        ''' Creates a new tab page.
        ''' </summary>
        Public Sub New()
            Me.DoubleBuffered = True
            Me.AutoScroll = True
        End Sub

        ''' <summary>
        ''' Overrides the OnScroll event for this conrol.
        ''' </summary>
        ''' <param name="se">The ScrollEventArgs for this event.</param>
        Protected Overrides Sub OnScroll(se As ScrollEventArgs)
            MyBase.OnScroll(se)
            Invalidate()
        End Sub

        ''' <summary>
        ''' Overrides the OnLoad event for this control.
        ''' </summary>
        ''' <param name="e">The EventArgs for this event.</param>
        Protected Overrides Sub OnLoad(e As EventArgs)
            Focus()
            MyBase.OnLoad(e)
        End Sub

        ''' <summary>
        ''' Gets triggered when this page is selected in the tab control.
        ''' </summary>
        Friend Overridable Sub OnSelected()
        End Sub

        ''' <summary>
        ''' Gets triggered when this page is no longer selected in the tab control.
        ''' </summary>
        Friend Overridable Sub OnDeselected()
        End Sub

        ''' <summary>
        ''' Sets the owner of this tab.
        ''' </summary>
        ''' <param name="tabcontrol">The owner of this tab.</param>
        Friend Sub SetOwnerTabControl(tabcontrol As ChromeTabControl)
            _tabControl = tabcontrol
        End Sub

        ''' <summary>
        ''' Updates the parent control's areas.
        ''' </summary>
        Private Sub Changed()
            If (_tabControl IsNot Nothing) Then
                _tabControl.UpdateAreas()
            End If
        End Sub

        ''' <summary>
        ''' Invalidates the parent control.
        ''' </summary>
        Private Sub RefreshControl()
            If (_tabControl IsNot Nothing) Then
                _tabControl.Invalidate()
            End If
        End Sub

        ''' <summary>
        ''' Closes this page.
        ''' </summary>
        Public Sub Close()
            If _tabControl Is Nothing Then
                Return
            End If
            Dim index As Integer = _tabControl.TabPages.IndexOf(Me)
            If index < 0 Then
                Return
            End If
            _tabControl.TabPages.RemoveAt(index)
        End Sub

        ''' <summary>
        ''' Gets called when the tab is being closed. Return true to cancel.
        ''' </summary>
        ''' <returns>True to allow the closing. False to not allow it.</returns>
        Friend Overridable Function TabClosingAllowed() As Boolean
            Return True
        End Function

        ''' <summary>
        ''' Makes this tab the selected tab in the control.
        ''' </summary>
        Public Sub EnsureVisible()
            If _tabControl Is Nothing Then
                Return
            End If
            Dim index As Integer = _tabControl.TabPages.IndexOf(Me)
            If index < 0 Then
                Return
            End If
            _tabControl.TabPages.SelectedIndex = index
        End Sub

        ''' <summary>
        ''' Whether or not there can only be one instance of a tab.
        ''' </summary>
        Public Property SingleInstance() As Boolean
            Get
                Return m_singleInstance
            End Get
            Set
                m_singleInstance = Value
            End Set
        End Property
        Private m_singleInstance As Boolean = True

        ''' <summary>
        ''' Gets called when a new instance of this tab page is attempted to be opened.
        ''' </summary>
        Protected Overridable Function NewInstanceAttempted(newInstance As TabPage) As Boolean
            Return False
        End Function

        ''' <summary>
        ''' Gets called when the parent TabControl is disposing.
        ''' </summary>
        Protected Overloads Sub Dispose()
        End Sub

        ''' <summary>
        ''' Gets called when the parent TabControl is disposing.
        ''' </summary>
        Protected Overrides Sub Dispose(disposing As Boolean)
            MyBase.Dispose(disposing)
            Dispose()
        End Sub

        ''' <summary>
        ''' The animator that animates the tab.
        ''' </summary>
        Public ReadOnly Property Animator() As TabAnimator
            Get
                If _animator Is Nothing Then
                    _animator = New TabAnimator(Me)
                End If
                Return _animator
            End Get
        End Property
        Private _animator As TabAnimator

        ''' <summary>
        ''' Whether or not this page can be closed.
        ''' </summary>
        Public Property CanClose() As Boolean
            Get
                Return _canClose
            End Get
            Set
                Dim flag As Boolean = False
                If Value <> _canClose Then
                    flag = True
                End If
                _canClose = Value
                If flag Then
                    RefreshControl()
                End If
            End Set
        End Property
        Private _canClose As Boolean = True

        ''' <summary>
        ''' Whether or not this page can be reopened.
        ''' </summary>
        Public Property CanReopen() As Boolean
            Get
                Return _canReopen
            End Get
            Set
                _canReopen = Value
            End Set
        End Property
        Private _canReopen As Boolean = True

        ''' <summary>
        ''' Whether or not this page can be pinned.
        ''' </summary>
        Public Property CanPin() As Boolean
            Get
                Return _canPin
            End Get
            Set
                _canPin = Value
            End Set
        End Property
        Private _canPin As Boolean = True

        ''' <summary>
        ''' The title of the page that shows up on the tab.
        ''' </summary>
        Public Property Title() As String
            Get
                Return _title
            End Get
            Set
                Dim flag As Boolean = False
                If Value <> _title Then
                    flag = True
                End If
                _title = Value
                If flag Then
                    RefreshControl()
                End If
            End Set
        End Property
        Private _title As String = "No Title"

        ''' <summary>
        ''' The current width in pixels of the tab on top.
        ''' </summary>
        Public Property TabWidth() As Single
            Get
                Return _width
            End Get
            Set
                _width = Value
                Changed()
            End Set
        End Property
        Private _width As Single = -1

        ''' <summary>
        ''' The full sized image for this page.
        ''' </summary>
        Public Property Image() As Bitmap
            Get
                Return _image
            End Get
            Set
                _image = Value
                _image16 = Utils.ResizeBitmap(_image, New Size(16, 16))
                _image16Transparent = Utils.SetBitmapOpacity(_image16, 80)
                RefreshControl()
            End Set
        End Property
        Private _image As Bitmap

        ''' <summary>
        ''' The 16x16 image for this page
        ''' </summary>
        Public ReadOnly Property Image16() As Bitmap
            Get
                Return _image16
            End Get
        End Property
        Private _image16 As Bitmap

        ''' <summary>
        ''' The slightly transparent 16x16 image for this page.
        ''' </summary>
        Public ReadOnly Property Image16Transparent() As Bitmap
            Get
                Return _image16Transparent
            End Get
        End Property
        Private _image16Transparent As Bitmap

        ''' <summary>
        ''' Whether or not this tab is selectable or not.
        ''' </summary>
        Public Property TabSelectable() As Boolean
            Get
                Return _tabEnabled
            End Get
            Set
                Dim flag As Boolean = False
                If Value <> _tabEnabled Then
                    flag = True
                End If
                _tabEnabled = Value
                If flag Then
                    If (_tabControl IsNot Nothing) Then
                        _tabControl.ReclipMouse()
                    End If
                End If
            End Set
        End Property
        Private _tabEnabled As Boolean = True

        ''' <summary>
        ''' Whether or not this tab is pinned.
        ''' </summary>
        Public Property Pinned() As Boolean
            Get
                Return _pinned
            End Get
            Set
                If Value Then

                    ' If we're pinned or we cannot pin, let's get out of here
                    If Pinned OrElse Not CanPin Then
                        Return
                    End If

                    ' Move this to the first open pin spot
                    If (_tabControl IsNot Nothing) Then

                        ' Let's find the spot to move it to
                        Dim i As Integer = 0
                        While i < _tabControl.TabPages.Count
                            If _tabControl.TabPages(i).Pinned Then
                                If i = _tabControl.TabPages.IndexOf(Me) Then
                                    Exit While
                                End If
                                i += 1
                            Else
                                Exit While
                            End If
                        End While

                        ' Now let's move it
                        _tabControl.TabPages.MoveItem(_tabControl.TabPages.IndexOf(Me), i)
                    End If

                    ' We're pinned, let's do the animation
                    _pinned = True
                    If (_tabControl IsNot Nothing) Then
                        Animator.Enable(TabAnimator.AnimationType.ToPin)
                        While Animator.Working
                            Application.DoEvents()
                        End While
                        RefreshControl()
                    Else
                        Me.TabWidth = TabAnimator.PIN_SIZE

                    End If
                Else

                    ' If we're already not pinned, let's get out of here
                    If Not Pinned Then
                        Return
                    End If

                    ' Move this past any pinned tabs
                    If (_tabControl IsNot Nothing) Then

                        ' Let's find the spot to move to
                        Dim i As Integer = _tabControl.TabPages.IndexOf(Me)
                        Dim cur As Integer = i
                        While i < _tabControl.TabPages.Count
                            If i + 1 < _tabControl.TabPages.Count Then
                                If _tabControl.TabPages(i + 1).Pinned Then
                                    i += 1
                                Else
                                    Exit While
                                End If
                            Else
                                Exit While
                            End If
                        End While

                        ' Now let's move it
                        _tabControl.TabPages.MoveItem(cur, i)
                    End If

                    ' We're no longer pinned, let's animate it
                    _pinned = False
                    If (_tabControl IsNot Nothing) Then
                        Animator.Enable(TabAnimator.AnimationType.ToFull)
                        While Animator.Working
                            Application.DoEvents()
                        End While
                        RefreshControl()
                    Else
                        Me.TabWidth = -1

                    End If
                End If
            End Set
        End Property
        Private _pinned As Boolean = False

        ''' <summary>
        ''' Whether or not this tab is draggable.
        ''' </summary>
        Public Property TabDraggable() As Boolean
            Get
                Return _tabDraggable
            End Get
            Set
                _tabDraggable = Value
            End Set
        End Property
        Private _tabDraggable As Boolean = True

        ''' <summary>
        ''' Used for animating tabs.
        ''' </summary>
        Public Class TabAnimator

            ''' <summary>
            ''' The tab we're going to animate.
            ''' </summary>
            Private _tab As TabPage

            ''' <summary>
            ''' The timer in charge of iterating.
            ''' </summary>
            Private _timer As Timer

            ''' <summary>
            ''' The size of a pinned tab.
            ''' </summary>
            Public Const PIN_SIZE As Single = 40

            ' Some instance variables
            Private _wasTabEnabled As Boolean = True
            Private _step As Integer = 20
            Private _type As AnimationType

            ''' <summary>
            ''' Creates a new TabAnimator given a tab.
            ''' </summary>
            ''' <param name="tab">The tab to animate.</param>
            Public Sub New(tab As TabPage)
                _tab = tab
                _timer = New Timer()
                _timer.Interval = 1
                AddHandler _timer.Tick, New EventHandler(AddressOf Tick)
            End Sub

            ''' <summary>
            ''' The different animation types.
            ''' </summary>
            Public Enum AnimationType As Byte
                ToZero = 0
                ToFull = 1
                ToPin = 2
            End Enum

            ''' <summary>
            ''' Starts the animation given a type.
            ''' </summary>
            ''' <param name="type">The animation type to start.</param>
            Public Sub Enable(type As AnimationType)
                If _timer.Enabled Then
                    Finish()
                End If
                _type = type
                _wasTabEnabled = _tab.TabSelectable
                _tab.TabSelectable = False
                _timer.Interval = 1
                _timer.Start()
            End Sub

            ''' <summary>
            ''' Finishes an animation.
            ''' </summary>
            Public Sub Finish()
                _timer.[Stop]()
                _tab.TabSelectable = _wasTabEnabled
            End Sub

            ''' <summary>
            ''' Whether or not this animator is animating.
            ''' </summary>
            Public ReadOnly Property Working() As Boolean
                Get
                    Return _timer.Enabled
                End Get
            End Property

            ''' <summary>
            ''' Gets triggered when the timer updates.
            ''' </summary>
            ''' <param name="sender">The sender object.</param>
            ''' <param name="e">The event args.</param>
            Private Sub Tick(sender As [Object], e As EventArgs)
                If _type = AnimationType.ToFull Then
                    If _tab.TabWidth = -1 Then
                        Finish()
                    End If
                    If _tab._tabControl Is Nothing Then
                        Finish()
                    End If
                    If _tab.TabWidth < _tab._tabControl.TabWidth Then
                        If _tab._tabControl.TabWidth - _tab.TabWidth < _step Then
                            _tab.TabWidth += _tab._tabControl.TabWidth - _tab.TabWidth
                        Else
                            _tab.TabWidth += _step
                        End If
                    Else
                        _tab.TabWidth = -1
                        Finish()

                    End If
                ElseIf _type = AnimationType.ToZero Then
                    If _tab._tabControl Is Nothing Then
                        Finish()
                    End If
                    If _tab.TabWidth = -1 Then
                        _tab.TabWidth = _tab._tabControl.TabWidth - _step
                    End If
                    If _tab.TabWidth > 0 Then
                        If _tab.TabWidth - _step < 0 Then
                            _tab.TabWidth = 0
                        Else
                            _tab.TabWidth -= _step
                        End If
                    Else
                        _tab.TabWidth = 0
                        Finish()

                    End If
                ElseIf _type = AnimationType.ToPin Then
                    If _tab._tabControl Is Nothing Then
                        Finish()
                    End If
                    If _tab.TabWidth = -1 Then
                        _tab.TabWidth = _tab._tabControl.TabWidth
                    End If
                    If _tab.TabWidth > PIN_SIZE Then
                        If _tab.TabWidth - _step < PIN_SIZE Then
                            _tab.TabWidth = PIN_SIZE
                            Finish()
                        Else
                            _tab.TabWidth -= _step
                        End If
                    ElseIf _tab.TabWidth < PIN_SIZE Then
                        If _tab.TabWidth + _step > PIN_SIZE Then
                            _tab.TabWidth = PIN_SIZE
                            Finish()
                        Else
                            _tab.TabWidth += _step
                        End If
                    End If
                Else
                    Finish()
                End If
            End Sub

        End Class

        ''' <summary>
        ''' Used for tab dragging.
        ''' Keeps necessary information in the drag event.
        ''' </summary>
        Public Class DragShell

            Private _tab As TabPage
            Public Sub New(tab1 As TabPage)
                _tab = tab1
            End Sub

            Public ReadOnly Property Tab() As TabPage
                Get
                    Return _tab
                End Get
            End Property

        End Class

        ''' <summary>
        ''' Collection of tab pages that syncs with the tab control.
        ''' </summary>
        Public Class TabPageCollection
            Implements IList(Of TabPage)

            Private Const REOPEN_LIST_MAX As Integer = 3
            Private _list As New List(Of TabPage)()
            Private _reopenList As New List(Of TabPage)()
            Private _reopenListIndexes As New List(Of Integer)()

            ' Events
            Public Delegate Sub TabPageCollectionEvent()
            Public Event NoItemsLeft As TabPageCollectionEvent

            Private _tabControl As ChromeTabControl
            Friend Sub Changed()
                CheckList()
                If (_tabControl IsNot Nothing) Then
                    _tabControl.UpdateAreas()
                End If
            End Sub

            Private Sub CheckList()
                If _list.Count > 0 AndAlso _selectedIndex = -1 Then
                    SelectedIndex = 0
                End If
                If _list.Count = 0 AndAlso _selectedIndex <> -1 Then
                    If (_tabControl IsNot Nothing) Then
                        _tabControl.SetCanvas(Nothing)
                    End If
                End If
            End Sub

            Private Sub CheckReopenList()
                Dim i As Integer = 0
                While i < _reopenList.Count
                    If _reopenList(i).CanReopen = False OrElse i >= REOPEN_LIST_MAX Then
                        _reopenList(i).Dispose()
                        _reopenList.RemoveAt(i)
                        _reopenListIndexes.RemoveAt(i)
                    Else
                        i += 1
                    End If
                End While
            End Sub

            ''' <summary>
            ''' Gets the index of an already opened instance of a tab. -1 for not found.
            ''' </summary>
            Private Function GetInstanceIndices(page As TabPage, tlist As List(Of TabPage)) As Integer()
                Dim rtn As New List(Of Integer)()
                If Not page.SingleInstance Then
                    Return rtn.ToArray()
                End If
                For i As Integer = 0 To _list.Count - 1
                    If tlist(i).Name = page.Name AndAlso tlist(i).[GetType]() Is page.[GetType]() AndAlso tlist(i).SingleInstance Then
                        rtn.Add(i)
                    End If
                Next
                Return rtn.ToArray()
            End Function
            Private Function CheckInstance(page As TabPage) As Boolean
                Dim indices As Integer() = GetInstanceIndices(page, _list)
                For i As Integer = 0 To indices.Length - 1
                    If indices(i) > -1 Then
                        SelectedIndex = indices(i)
                        Dim answer As Boolean = _list(indices(i)).NewInstanceAttempted(page)
                        If answer = False Then
                            Return True
                        End If
                    End If
                Next
                Return False
            End Function

            ''' <summary>
            ''' Whether or not there is a tab available to be reopened.
            ''' </summary>
            ''' <returns>True if a tab is ready.</returns>
            Public Function HasTabToReopen() As Boolean
                CheckReopenList()
                If _reopenList.Count > 0 Then
                    Return True
                End If
                Return False
            End Function

            ''' <summary>
            ''' Reopened the last closed tab.
            ''' </summary>
            Public Sub ReopenTab()
                If HasTabToReopen() = False Then
                    Return
                End If
                Dim index As Integer = _reopenListIndexes(0)
                If index > _list.Count Then
                    index = _list.Count
                End If
                If index < 0 Then
                    index = 0
                End If
                Insert(index, _reopenList(0))
                _reopenList.RemoveAt(0)
                _reopenListIndexes.RemoveAt(0)
                _selectedIndex = -1
                'force a recanvas
                SelectedIndex = index
            End Sub

            ''' <summary>
            ''' Returns the tab that is next on line to be reopened.
            ''' </summary>
            ''' <returns></returns>
            Public Function GetTabToReopen() As TabPage
                If HasTabToReopen() = False Then
                    Return Nothing
                End If
                Return _reopenList(0)
            End Function

            ''' <summary>
            ''' The index of the currently selected tab.
            ''' </summary>
            Public Property SelectedIndex() As Integer
                Get
                    Return _selectedIndex
                End Get
                Set
                    If Value > _list.Count - 1 Then
                        Value = _list.Count - 1
                    End If
                    If Value < -1 Then
                        Value = -1
                    End If
                    If Value = _selectedIndex Then
                        Return
                    End If
                    Dim flag As Boolean = False
                    If Value <> _selectedIndex Then
                        flag = True
                    End If
                    _selectedIndex = Value
                    If flag Then
                        If (_tabControl IsNot Nothing) Then
                            _tabControl.Invalidate()
                            If Value > -1 Then
                                _tabControl.SetCanvas(_list(Value))
                            Else
                                _tabControl.SetCanvas(DirectCast(Nothing, TabPage))
                            End If
                        End If
                    End If
                End Set
            End Property
            Private _selectedIndex As Integer = -1

            Public Sub New(owner As ChromeTabControl)
                _tabControl = owner
            End Sub

            ''' <summary>
            ''' Adds a page to the list and animates it in.
            ''' </summary>
            ''' <param name="newTab">The item to open.</param>
            Public Sub Add(newTab As TabPage) Implements IList(Of TabPage).Add
                If CheckInstance(newTab) Then
                    Return
                End If
                newTab.SetOwnerTabControl(_tabControl)
                newTab.TabWidth = 0
                _list.Add(newTab)
                Changed()
                newTab.Animator.Enable(TabPage.TabAnimator.AnimationType.ToFull)
                SelectedIndex = _list.Count - 1

                ' Add thumbnail toolbar buttons
                '   TaskbarManager.Instance.ThumbnailToolBars.AddButtons(newTab.Handle)

                ' Add a new preview
                '  Dim preview As New TabbedThumbnail(_tabControl.Handle, newTab.Handle)

                ' Event handlers for this preview
                '  AddHandler preview.TabbedThumbnailActivated, AddressOf preview_TabbedThumbnailActivated
                '  AddHandler preview.TabbedThumbnailClosed, AddressOf preview_TabbedThumbnailClosed
                '   AddHandler preview.TabbedThumbnailMaximized, AddressOf preview_TabbedThumbnailMaximized
                '  AddHandler preview.TabbedThumbnailMinimized, AddressOf preview_TabbedThumbnailMinimized

                '  TaskbarManager.Instance.TabbedThumbnail.AddThumbnailPreview(preview)

                ' Select the tab in the application UI as well as taskbar tabbed thumbnail list
                '   TaskbarManager.Instance.TabbedThumbnail.SetActiveTab(newTab)
            End Sub

            ''' <summary>
            ''' Adds a page to the list but doesn't animate it when it opens.
            ''' </summary>
            ''' <param name="item">The item to open.</param>
            Public Sub AddNoAnimate(item As TabPage)
                If CheckInstance(item) Then
                    Return
                End If
                item.SetOwnerTabControl(_tabControl)
                _list.Add(item)
                Changed()
            End Sub

            ''' <summary>
            ''' Should never really be called.
            ''' </summary>
            Public Sub Clear() Implements ICollection(Of TabPage).Clear
                _list.Clear()
                _reopenList.Clear()
                _reopenListIndexes.Clear()
                Changed()
                RaiseEvent NoItemsLeft()
            End Sub

            Public Function Contains(item As TabPage) As Boolean Implements IList(Of TabPage).Contains
                Return _list.Contains(item)
            End Function

            Public Sub CopyTo(array As TabPage(), arrayIndex As Integer) Implements IList(Of TabPage).CopyTo
                _list.CopyTo(array, arrayIndex)
            End Sub

            Public ReadOnly Property Count() As Integer Implements ICollection(Of TabPage).Count
                Get
                    Return _list.Count
                End Get
            End Property

            Public ReadOnly Property IsReadOnly() As Boolean Implements ICollection(Of TabPage).IsReadOnly
                Get
                    Return False
                End Get
            End Property

            Public Function Remove(item As TabPage) As Boolean Implements IList(Of TabPage).Remove
                Dim index As Integer = _list.IndexOf(item)
                If index < 0 Then
                    Return False
                End If
                If Not _list(index).CanClose Then
                    Return False
                End If
                If Not _list(index).TabClosingAllowed() Then
                    Return False
                End If
                RemoveAt(index)
                If _list.Count = 0 Then
                    RaiseEvent NoItemsLeft()
                End If
                Return True
            End Function

            Public Sub OverrideTab(indexToOverride As Integer, newPage As TabPage)
                If CheckInstance(newPage) Then
                    Return
                End If
                newPage.SetOwnerTabControl(_tabControl)
                _list(indexToOverride) = newPage
                If indexToOverride = SelectedIndex Then
                    _tabControl.SetCanvas(_list(indexToOverride))
                End If
                _tabControl.Invalidate()
            End Sub

            Public Function GetEnumerator() As System.Collections.Generic.IEnumerator(Of TabPage) Implements IEnumerable(Of TabPage).GetEnumerator
                Return _list.GetEnumerator()
            End Function

            Public Function IndexOf(item As TabPage) As Integer Implements IList(Of TabPage).IndexOf
                Return _list.IndexOf(item)
            End Function

            Public Sub Insert(index As Integer, item As TabPage) Implements IList(Of TabPage).Insert
                If CheckInstance(item) Then
                    Return
                End If
                item.SetOwnerTabControl(_tabControl)
                item.TabWidth = 0
                _list.Insert(index, item)
                Changed()
                item.Animator.Enable(TabPage.TabAnimator.AnimationType.ToFull)
            End Sub

            Default Public Property Item(index As Integer) As TabPage Implements IList(Of TabPage).Item
                Get
                    Return _list(index)
                End Get
                Set
                    Value.SetOwnerTabControl(_tabControl)
                    _list(index) = Value
                    Changed()
                End Set
            End Property

            Public Sub RemoveAt(index As Integer) Implements IList(Of TabPage).RemoveAt
                'Don't allow the removal of pinned tabs
                If _list(index).Pinned Then
                    Return
                End If

                ' Don't try to close it if it doesn't want to
                If Not _list(index).CanClose Then
                    Return
                End If

                ' Don't remove if the tab cancels the close
                If Not _list(index).TabClosingAllowed() Then
                    Return
                End If

                'Calculate and change the selected index according to which is closing
                Dim indexToChangeTo As Integer = _selectedIndex
                If _selectedIndex > -1 Then
                    If _selectedIndex = index Then
                        If index = 0 Then
                            If _list.Count > 1 Then
                                indexToChangeTo = _selectedIndex
                                SelectedIndex += 1
                            Else
                                indexToChangeTo = -1
                                SelectedIndex = -1
                            End If
                        ElseIf index = _list.Count - 1 Then
                            SelectedIndex -= 1
                            indexToChangeTo = _selectedIndex
                        Else
                            indexToChangeTo = _selectedIndex
                            SelectedIndex += 1
                        End If
                    ElseIf _selectedIndex > index Then
                        indexToChangeTo = _selectedIndex - 1
                    End If
                Else
                    indexToChangeTo = -1
                End If

                'Do the animation and actual removal
                _list(index).Animator.Enable(TabPage.TabAnimator.AnimationType.ToZero)
                Try
                    While _list(index).Animator.Working
                        Application.DoEvents()
                    End While
                Catch
                End Try


                'Add to the reopen list if needed
                If index < _list.Count Then
                    If _list(index).CanReopen Then
                        Dim tab As TabPage = _list(index)
                        _list.RemoveAt(index)
                        tab.SetOwnerTabControl(Nothing)
                        tab.TabWidth = -1
                        tab.Pinned = False
                        _reopenList.Insert(0, tab)
                        _reopenListIndexes.Insert(0, index)
                    Else
                        _list(index).Dispose()
                        _list.RemoveAt(index)
                    End If
                End If

                'Change the selected index and dont refresh until finished
                If indexToChangeTo <> _selectedIndex Then
                    _selectedIndex = indexToChangeTo
                End If

                If _list.Count = 0 Then
                    RaiseEvent NoItemsLeft()
                End If

                Changed()

            End Sub

            Public Function GetEnumerator1() As System.Collections.IEnumerator
                Return _list.GetEnumerator()
            End Function
            Private Function System_Collections_IEnumerable_GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
                Return GetEnumerator1()
            End Function

            ''' <summary>
            ''' Moves a page from one index to another.
            ''' </summary>
            ''' <param name="oldIndex">The index it is currently at.</param>
            ''' <param name="newIndex">The new index you want it to be at.</param>
            Public Sub MoveItem(oldIndex As Integer, newIndex As Integer)
                If oldIndex = newIndex Then
                    Return
                End If
                If newIndex > _list.Count - 1 Then
                    newIndex = _list.Count - 1
                End If
                If newIndex < 0 Then
                    newIndex = 0
                End If
                Dim curselected As Integer = _selectedIndex
                Dim temp As TabPage = _list(oldIndex)
                _list.RemoveAt(oldIndex)
                _list.Insert(newIndex, temp)
                If _selectedIndex = oldIndex Then
                    _selectedIndex = newIndex
                ElseIf _selectedIndex > oldIndex Then
                    If newIndex >= _selectedIndex Then
                        _selectedIndex -= 1
                    End If
                ElseIf _selectedIndex < oldIndex Then
                    If newIndex <= _selectedIndex Then
                        _selectedIndex += 1
                    End If
                End If
                Changed()
            End Sub

        End Class

    End Class
End Namespace