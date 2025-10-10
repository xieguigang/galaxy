Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports System.Diagnostics.CodeAnalysis

Namespace Docking
    Friend Interface IContentFocusManager
        Sub Activate(content As IDockContent)
        Sub GiveUpFocus(content As IDockContent)
        Sub AddToList(content As IDockContent)
        Sub RemoveFromList(content As IDockContent)
    End Interface

    Partial Class DockPanel
        Private Interface IFocusManager
            Sub SuspendFocusTracking()
            Sub ResumeFocusTracking()
            ReadOnly Property IsFocusTrackingSuspended As Boolean
            ReadOnly Property ActiveContent As IDockContent
            ReadOnly Property ActivePane As DockPane
            ReadOnly Property ActiveDocument As IDockContent
            ReadOnly Property ActiveDocumentPane As DockPane
        End Interface

        Private Class FocusManagerImpl
            Inherits Component
            Implements IContentFocusManager, IFocusManager
            Private Class HookEventArgs
                Inherits EventArgs
                <SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>
                Public HookCode As Integer
                <SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>
                Public wParam As IntPtr
                Public lParam As IntPtr
            End Class

            Private Class LocalWindowsHook
                Implements IDisposable
                ' Internal properties
                Private m_hHook As IntPtr = IntPtr.Zero
                Private m_filterFunc As HookProc = Nothing
                Private m_hookType As Win32.HookType

                ' Event delegate
                Public Delegate Sub HookEventHandler(sender As Object, e As HookEventArgs)

                ' Event: HookInvoked 
                Public Event HookInvoked As HookEventHandler
                Protected Sub OnHookInvoked(e As HookEventArgs)
                    RaiseEvent HookInvoked(Me, e)
                End Sub

                Public Sub New(hook As Win32.HookType)
                    m_hookType = hook
                    m_filterFunc = New HookProc(AddressOf CoreHookProc)
                End Sub

                ' Default filter function
                Public Function CoreHookProc(code As Integer, wParam As IntPtr, lParam As IntPtr) As IntPtr
                    If code < 0 Then Return CallNextHookEx(m_hHook, code, wParam, lParam)

                    ' Let clients determine what to do
                    Dim e As HookEventArgs = New HookEventArgs()
                    e.HookCode = code
                    e.wParam = wParam
                    e.lParam = lParam
                    OnHookInvoked(e)

                    ' Yield to the next hook in the chain
                    Return CallNextHookEx(m_hHook, code, wParam, lParam)
                End Function

                ' Install the hook
                Public Sub Install()
                    If m_hHook <> IntPtr.Zero Then Uninstall()

                    Dim threadId As Integer = GetCurrentThreadId()
                    m_hHook = SetWindowsHookEx(m_hookType, m_filterFunc, IntPtr.Zero, threadId)
                End Sub

                ' Uninstall the hook
                Public Sub Uninstall()
                    If m_hHook <> IntPtr.Zero Then
                        UnhookWindowsHookEx(m_hHook)
                        m_hHook = IntPtr.Zero
                    End If
                End Sub

                Protected Overrides Sub Finalize()
                    Dispose(False)
                End Sub

                Public Sub Dispose() Implements IDisposable.Dispose
                    Dispose(True)
                    GC.SuppressFinalize(Me)
                End Sub

                Protected Overridable Sub Dispose(disposing As Boolean)
                    Uninstall()
                End Sub
            End Class

            ' Use a static instance of the windows hook to prevent stack overflows in the windows kernel.
            <ThreadStatic>
            Private Shared sm_localWindowsHook As LocalWindowsHook
            <ThreadStatic>
            Private Shared _referenceCount As Integer = 0

            Private ReadOnly m_hookEventHandler As LocalWindowsHook.HookEventHandler

            Public Sub New(dockPanel As DockPanel)
                m_dockPanel = dockPanel
                If IsRunningOnMono Then Return
                m_hookEventHandler = New LocalWindowsHook.HookEventHandler(AddressOf HookEventHandler)

                ' Ensure the windows hook has been created for this thread
                If sm_localWindowsHook Is Nothing Then
                    sm_localWindowsHook = New LocalWindowsHook(Win32.HookType.WH_CALLWNDPROCRET)
                    Call sm_localWindowsHook.Install()
                End If

                AddHandler sm_localWindowsHook.HookInvoked, m_hookEventHandler
                Threading.Interlocked.Increment(_referenceCount)
            End Sub

            Private m_dockPanel As DockPanel
            Public ReadOnly Property DockPanel As DockPanel
                Get
                    Return m_dockPanel
                End Get
            End Property

            Private m_disposed As Boolean = False
            Protected Overrides Sub Dispose(disposing As Boolean)
                If Not m_disposed AndAlso disposing Then
                    If Not IsRunningOnMono Then
                        RemoveHandler sm_localWindowsHook.HookInvoked, m_hookEventHandler
                    End If

                    Threading.Interlocked.Decrement(_referenceCount)

                    If _referenceCount = 0 AndAlso sm_localWindowsHook IsNot Nothing Then
                        Call sm_localWindowsHook.Dispose()
                        sm_localWindowsHook = Nothing
                    End If

                    m_disposed = True
                End If

                MyBase.Dispose(disposing)
            End Sub

            Private m_contentActivating As IDockContent = Nothing
            Private Property ContentActivating As IDockContent
                Get
                    Return m_contentActivating
                End Get
                Set(value As IDockContent)
                    m_contentActivating = value
                End Set
            End Property

            Public Sub Activate(content As IDockContent) Implements IContentFocusManager.Activate
                If IsFocusTrackingSuspended Then
                    ContentActivating = content
                    Return
                End If

                If content Is Nothing Then Return
                Dim handler = content.DockHandler
                If handler.Form.IsDisposed Then Return ' Should not reach here, but better than throwing an exception
                If ContentContains(content, handler.ActiveWindowHandle) Then
                    If Not IsRunningOnMono Then
                        SetFocus(handler.ActiveWindowHandle)
                    End If
                End If

                If handler.Form.ContainsFocus Then Return

                If handler.Form.SelectNextControl(handler.Form.ActiveControl, True, True, True, True) Then Return

                If IsRunningOnMono Then Return

                ' Since DockContent Form is not selectalbe, use Win32 SetFocus instead
                SetFocus(handler.Form.Handle)
            End Sub

            Private m_listContent As List(Of IDockContent) = New List(Of IDockContent)()
            Private ReadOnly Property ListContent As List(Of IDockContent)
                Get
                    Return m_listContent
                End Get
            End Property
            Public Sub AddToList(content As IDockContent) Implements IContentFocusManager.AddToList
                If ListContent.Contains(content) OrElse IsInActiveList(content) Then Return

                ListContent.Add(content)
            End Sub

            Public Sub RemoveFromList(content As IDockContent) Implements IContentFocusManager.RemoveFromList
                If IsInActiveList(content) Then RemoveFromActiveList(content)
                If ListContent.Contains(content) Then ListContent.Remove(content)
            End Sub

            Private m_lastActiveContent As IDockContent = Nothing
            Private Property LastActiveContent As IDockContent
                Get
                    Return m_lastActiveContent
                End Get
                Set(value As IDockContent)
                    m_lastActiveContent = value
                End Set
            End Property

            Private Function IsInActiveList(content As IDockContent) As Boolean
                Return Not (content.DockHandler.NextActive Is Nothing AndAlso LastActiveContent IsNot content)
            End Function

            Private Sub AddLastToActiveList(content As IDockContent)
                Dim last = LastActiveContent
                If last Is content Then Return

                Dim handler = content.DockHandler

                If IsInActiveList(content) Then RemoveFromActiveList(content)

                handler.PreviousActive = last
                handler.NextActive = Nothing
                LastActiveContent = content
                If last IsNot Nothing Then last.DockHandler.NextActive = LastActiveContent
            End Sub

            Private Sub RemoveFromActiveList(content As IDockContent)
                If LastActiveContent Is content Then LastActiveContent = content.DockHandler.PreviousActive

                Dim prev = content.DockHandler.PreviousActive
                Dim [next] = content.DockHandler.NextActive
                If prev IsNot Nothing Then prev.DockHandler.NextActive = [next]
                If [next] IsNot Nothing Then [next].DockHandler.PreviousActive = prev

                content.DockHandler.PreviousActive = Nothing
                content.DockHandler.NextActive = Nothing
            End Sub

            Public Sub GiveUpFocus(content As IDockContent) Implements IContentFocusManager.GiveUpFocus
                Dim handler = content.DockHandler
                If Not handler.Form.ContainsFocus Then Return

                If IsFocusTrackingSuspended Then DockPanel.DummyControl.Focus()

                If LastActiveContent Is content Then
                    Dim prev = handler.PreviousActive
                    If prev IsNot Nothing Then
                        Activate(prev)
                    ElseIf ListContent.Count > 0 Then
                        Activate(ListContent(ListContent.Count - 1))
                    End If
                ElseIf LastActiveContent IsNot Nothing Then
                    Activate(LastActiveContent)
                ElseIf ListContent.Count > 0 Then
                    Activate(ListContent(ListContent.Count - 1))
                End If
            End Sub

            Private Shared Function ContentContains(content As IDockContent, hWnd As IntPtr) As Boolean
                Dim control = FromChildHandle(hWnd)
                Dim parent = control

                While parent IsNot Nothing
                    If parent Is content.DockHandler.Form Then Return True
                    parent = parent.Parent
                End While

                Return False
            End Function

            Private m_countSuspendFocusTracking As UInteger = 0
            Public Sub SuspendFocusTracking() Implements IFocusManager.SuspendFocusTracking
                If m_disposed Then Return

                If m_countSuspendFocusTracking = 0 Then
                    m_countSuspendFocusTracking += 1UI
                    If Not IsRunningOnMono Then RemoveHandler sm_localWindowsHook.HookInvoked, m_hookEventHandler
                End If
            End Sub

            Public Sub ResumeFocusTracking() Implements IFocusManager.ResumeFocusTracking
                If m_disposed OrElse m_countSuspendFocusTracking = 0 Then Return

                m_countSuspendFocusTracking -= 1UI

                If m_countSuspendFocusTracking = 0 Then
                    If ContentActivating IsNot Nothing Then
                        Activate(ContentActivating)
                        ContentActivating = Nothing
                    End If

                    If Not IsRunningOnMono Then AddHandler sm_localWindowsHook.HookInvoked, m_hookEventHandler

                    If Not InRefreshActiveWindow Then RefreshActiveWindow()
                End If
            End Sub

            Public ReadOnly Property IsFocusTrackingSuspended As Boolean Implements IFocusManager.IsFocusTrackingSuspended
                Get
                    Return m_countSuspendFocusTracking <> 0
                End Get
            End Property

            ' Windows hook event handler
            Private Sub HookEventHandler(sender As Object, e As HookEventArgs)
                Dim msg As Win32.Msgs = Marshal.ReadInt32(e.lParam, IntPtr.Size * 3)

                If msg = Win32.Msgs.WM_KILLFOCUS Then
                    Dim wParam = Marshal.ReadIntPtr(e.lParam, IntPtr.Size * 2)
                    Dim pane = GetPaneFromHandle(wParam)
                    If pane Is Nothing Then RefreshActiveWindow()
                ElseIf msg = Win32.Msgs.WM_SETFOCUS OrElse msg = Win32.Msgs.WM_MDIACTIVATE Then
                    RefreshActiveWindow()
                End If
            End Sub

            Private Function GetPaneFromHandle(hWnd As IntPtr) As DockPane
                Dim control = FromChildHandle(hWnd)

                Dim content As IDockContent = Nothing
                Dim pane As DockPane = Nothing
                While control IsNot Nothing
                    content = TryCast(control, IDockContent)
                    If content IsNot Nothing Then content.DockHandler.ActiveWindowHandle = hWnd

                    If content IsNot Nothing AndAlso content.DockHandler.DockPanel Is DockPanel Then Return content.DockHandler.Pane

                    pane = TryCast(control, DockPane)
                    If pane IsNot Nothing AndAlso pane.DockPanel Is DockPanel Then Exit While
                    control = control.Parent
                End While

                Return pane
            End Function

            Private m_inRefreshActiveWindow As Boolean = False
            Private ReadOnly Property InRefreshActiveWindow As Boolean
                Get
                    Return m_inRefreshActiveWindow
                End Get
            End Property

            Private Sub RefreshActiveWindow()
                If DockPanel.Theme Is Nothing Then
                    Return
                End If

                SuspendFocusTracking()
                m_inRefreshActiveWindow = True

                Dim oldActivePane = ActivePane
                Dim oldActiveContent = ActiveContent
                Dim oldActiveDocument = ActiveDocument

                SetActivePane()
                SetActiveContent()
                SetActiveDocumentPane()
                SetActiveDocument()
                DockPanel.AutoHideWindow.RefreshActivePane()

                ResumeFocusTracking()
                m_inRefreshActiveWindow = False

                If oldActiveContent IsNot ActiveContent Then DockPanel.OnActiveContentChanged(EventArgs.Empty)
                If oldActiveDocument IsNot ActiveDocument Then DockPanel.OnActiveDocumentChanged(EventArgs.Empty)
                If oldActivePane IsNot ActivePane Then DockPanel.OnActivePaneChanged(EventArgs.Empty)
            End Sub

            Private m_activePane As DockPane = Nothing
            Public ReadOnly Property ActivePane As DockPane Implements IFocusManager.ActivePane
                Get
                    Return m_activePane
                End Get
            End Property

            Private Sub SetActivePane()
                Dim value As DockPane = If(IsRunningOnMono, Nothing, GetPaneFromHandle(GetFocus()))
                If m_activePane Is value Then Return

                If m_activePane IsNot Nothing Then m_activePane.SetIsActivated(False)

                m_activePane = value

                If m_activePane IsNot Nothing Then m_activePane.SetIsActivated(True)
            End Sub

            Private m_activeContent As IDockContent = Nothing
            Public ReadOnly Property ActiveContent As IDockContent Implements IFocusManager.ActiveContent
                Get
                    Return m_activeContent
                End Get
            End Property

            Friend Sub SetActiveContent()
                Dim value = If(ActivePane Is Nothing, Nothing, ActivePane.ActiveContent)

                If m_activeContent Is value Then Return

                If m_activeContent IsNot Nothing Then m_activeContent.DockHandler.IsActivated = False

                m_activeContent = value

                If m_activeContent IsNot Nothing Then
                    m_activeContent.DockHandler.IsActivated = True
                    If Not IsDockStateAutoHide(m_activeContent.DockHandler.DockState) Then AddLastToActiveList(m_activeContent)
                End If
            End Sub

            Private m_activeDocumentPane As DockPane = Nothing
            Public ReadOnly Property ActiveDocumentPane As DockPane Implements IFocusManager.ActiveDocumentPane
                Get
                    Return m_activeDocumentPane
                End Get
            End Property

            Private Sub SetActiveDocumentPane()
                Dim value As DockPane = Nothing

                If ActivePane IsNot Nothing AndAlso ActivePane.DockState = DockState.Document Then value = ActivePane

                If value Is Nothing AndAlso DockPanel.DockWindows IsNot Nothing Then
                    If ActiveDocumentPane Is Nothing Then
                        value = DockPanel.DockWindows(DockState.Document).DefaultPane
                    ElseIf ActiveDocumentPane.DockPanel IsNot DockPanel OrElse ActiveDocumentPane.DockState <> DockState.Document Then
                        value = DockPanel.DockWindows(DockState.Document).DefaultPane
                    Else
                        value = ActiveDocumentPane
                    End If
                End If

                If m_activeDocumentPane Is value Then Return

                If m_activeDocumentPane IsNot Nothing Then m_activeDocumentPane.SetIsActiveDocumentPane(False)

                m_activeDocumentPane = value

                If m_activeDocumentPane IsNot Nothing Then m_activeDocumentPane.SetIsActiveDocumentPane(True)
            End Sub

            Private m_activeDocument As IDockContent = Nothing
            Public ReadOnly Property ActiveDocument As IDockContent Implements IFocusManager.ActiveDocument
                Get
                    Return m_activeDocument
                End Get
            End Property

            Private Sub SetActiveDocument()
                Dim value = If(ActiveDocumentPane Is Nothing, Nothing, ActiveDocumentPane.ActiveContent)

                If m_activeDocument Is value Then Return

                m_activeDocument = value
            End Sub
        End Class

        Private ReadOnly Property FocusManager As IFocusManager
            Get
                Return m_focusManager
            End Get
        End Property

        Friend ReadOnly Property ContentFocusManager As IContentFocusManager
            Get
                Return m_focusManager
            End Get
        End Property

        Friend Sub SaveFocus()
            DummyControl.Focus()
        End Sub

        <Browsable(False)>
        Public ReadOnly Property ActiveContent As IDockContent
            Get
                Return FocusManager.ActiveContent
            End Get
        End Property

        <Browsable(False)>
        Public ReadOnly Property ActivePane As DockPane
            Get
                Return FocusManager.ActivePane
            End Get
        End Property

        <Browsable(False)>
        Public ReadOnly Property ActiveDocument As IDockContent
            Get
                Return FocusManager.ActiveDocument
            End Get
        End Property

        <Browsable(False)>
        Public ReadOnly Property ActiveDocumentPane As DockPane
            Get
                Return FocusManager.ActiveDocumentPane
            End Get
        End Property

        Private Shared ReadOnly ActiveDocumentChangedEvent As Object = New Object()
        <LocalizedCategory("Category_PropertyChanged")>
        <LocalizedDescription("DockPanel_ActiveDocumentChanged_Description")>
        Public Custom Event ActiveDocumentChanged As EventHandler
            AddHandler(value As EventHandler)
                Events.AddHandler(ActiveDocumentChangedEvent, value)
            End AddHandler
            RemoveHandler(value As EventHandler)
                Events.RemoveHandler(ActiveDocumentChangedEvent, value)
            End RemoveHandler
            <LocalizedCategory("Category_PropertyChanged")>
            <LocalizedDescription("DockPanel_ActiveDocumentChanged_Description")>
            RaiseEvent(sender As Object, e As EventArgs)
            End RaiseEvent
        End Event
        Protected Overridable Sub OnActiveDocumentChanged(e As EventArgs)
            Dim handler = CType(Events(ActiveDocumentChangedEvent), EventHandler)
            If handler IsNot Nothing Then handler(Me, e)
        End Sub

        Private Shared ReadOnly ActiveContentChangedEvent As Object = New Object()
        <LocalizedCategory("Category_PropertyChanged")>
        <LocalizedDescription("DockPanel_ActiveContentChanged_Description")>
        Public Custom Event ActiveContentChanged As EventHandler
            AddHandler(value As EventHandler)
                Events.AddHandler(ActiveContentChangedEvent, value)
            End AddHandler
            RemoveHandler(value As EventHandler)
                Events.RemoveHandler(ActiveContentChangedEvent, value)
            End RemoveHandler
            <LocalizedCategory("Category_PropertyChanged")>
            <LocalizedDescription("DockPanel_ActiveContentChanged_Description")>
            RaiseEvent(sender As Object, e As EventArgs)
            End RaiseEvent
        End Event

        Protected Sub OnActiveContentChanged(e As EventArgs)
            Dim handler = CType(Events(ActiveContentChangedEvent), EventHandler)
            If handler IsNot Nothing Then handler(Me, e)
        End Sub

        Private Shared ReadOnly DocumentDraggedEvent As Object = New Object()
        <LocalizedCategory("Category_PropertyChanged")>
        <LocalizedDescription("DockPanel_ActiveContentChanged_Description")>
        Public Custom Event DocumentDragged As EventHandler
            AddHandler(value As EventHandler)
                Events.AddHandler(DocumentDraggedEvent, value)
            End AddHandler
            RemoveHandler(value As EventHandler)
                Events.RemoveHandler(DocumentDraggedEvent, value)
            End RemoveHandler
            <LocalizedCategory("Category_PropertyChanged")>
            <LocalizedDescription("DockPanel_ActiveContentChanged_Description")>
            RaiseEvent(sender As Object, e As EventArgs)
            End RaiseEvent
        End Event

        Friend Sub OnDocumentDragged()
            Dim handler = CType(Events(DocumentDraggedEvent), EventHandler)
            If handler IsNot Nothing Then handler(Me, EventArgs.Empty)
        End Sub

        Private Shared ReadOnly ActivePaneChangedEvent As Object = New Object()
        <LocalizedCategory("Category_PropertyChanged")>
        <LocalizedDescription("DockPanel_ActivePaneChanged_Description")>
        Public Custom Event ActivePaneChanged As EventHandler
            AddHandler(value As EventHandler)
                Events.AddHandler(ActivePaneChangedEvent, value)
            End AddHandler
            RemoveHandler(value As EventHandler)
                Events.RemoveHandler(ActivePaneChangedEvent, value)
            End RemoveHandler
            <LocalizedCategory("Category_PropertyChanged")>
            <LocalizedDescription("DockPanel_ActivePaneChanged_Description")>
            RaiseEvent(sender As Object, e As EventArgs)
            End RaiseEvent
        End Event
        Protected Overridable Sub OnActivePaneChanged(e As EventArgs)
            Dim handler = CType(Events(ActivePaneChangedEvent), EventHandler)
            If handler IsNot Nothing Then handler(Me, e)
        End Sub
    End Class
End Namespace
