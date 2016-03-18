'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports System.Text
Imports Microsoft.Windows.Shell
Imports Microsoft.Windows.Internal
Imports Microsoft.Windows.Resources

Namespace Controls.WindowsForms
    ''' <summary>
    ''' This class is a wrapper around the Windows Explorer Browser control.
    ''' </summary>
    Public NotInheritable Class ExplorerBrowser
        Inherits System.Windows.Forms.UserControl
        Implements Controls.IServiceProvider
        Implements IExplorerPaneVisibility
        Implements IExplorerBrowserEvents
        Implements ICommDlgBrowser3
        Implements IMessageFilter
#Region "properties"
        ''' <summary>
        ''' Options that control how the ExplorerBrowser navigates
        ''' </summary>
        Public Property NavigationOptions() As ExplorerBrowserNavigationOptions
            Get
                Return m_NavigationOptions
            End Get
            Private Set
                m_NavigationOptions = Value
            End Set
        End Property
        Private m_NavigationOptions As ExplorerBrowserNavigationOptions

        ''' <summary>
        ''' Options that control how the content of the ExplorerBorwser looks
        ''' </summary>
        Public Property ContentOptions() As ExplorerBrowserContentOptions
            Get
                Return m_ContentOptions
            End Get
            Private Set
                m_ContentOptions = Value
            End Set
        End Property
        Private m_ContentOptions As ExplorerBrowserContentOptions

        Private shellItemsArray As IShellItemArray
        Private itemsCollection As ShellObjectCollection
        ''' <summary>
        ''' The set of ShellObjects in the Explorer Browser
        ''' </summary>
        Public ReadOnly Property Items() As ShellObjectCollection
            Get
                If shellItemsArray IsNot Nothing Then
                    Marshal.ReleaseComObject(shellItemsArray)
                End If

                If itemsCollection IsNot Nothing Then
                    itemsCollection.Dispose()
                    itemsCollection = Nothing
                End If

                shellItemsArray = GetItemsArray()
                itemsCollection = New ShellObjectCollection(shellItemsArray, True)

                Return itemsCollection
            End Get
        End Property

        Private selectedShellItemsArray As IShellItemArray
        Private selectedItemsCollection As ShellObjectCollection
        ''' <summary>
        ''' The set of selected ShellObjects in the Explorer Browser
        ''' </summary>
        Public ReadOnly Property SelectedItems() As ShellObjectCollection
            Get
                If selectedShellItemsArray IsNot Nothing Then
                    Marshal.ReleaseComObject(selectedShellItemsArray)
                End If

                If selectedItemsCollection IsNot Nothing Then
                    selectedItemsCollection.Dispose()
                    selectedItemsCollection = Nothing
                End If

                selectedShellItemsArray = GetSelectedItemsArray()
                selectedItemsCollection = New ShellObjectCollection(selectedShellItemsArray, True)

                Return selectedItemsCollection
            End Get
        End Property

        ''' <summary>
        ''' Contains the navigation history of the ExplorerBrowser
        ''' </summary>
        Public Property NavigationLog() As ExplorerBrowserNavigationLog
            Get
                Return m_NavigationLog
            End Get
            Private Set
                m_NavigationLog = Value
            End Set
        End Property
        Private m_NavigationLog As ExplorerBrowserNavigationLog

        ''' <summary>
        ''' The name of the property bag used to persist changes to the ExplorerBrowser's view state.
        ''' </summary>
        Public Property PropertyBagName() As String
            Get
                Return m_propertyBagName
            End Get
            Set
                m_propertyBagName = Value
                If explorerBrowserControl IsNot Nothing Then
                    explorerBrowserControl.SetPropertyBag(m_propertyBagName)
                End If
            End Set
        End Property

#End Region

#Region "operations"
        ''' <summary>
        ''' Clears the Explorer Browser of existing content, fills it with
        ''' content from the specified container, and adds a new point to the Travel Log.
        ''' </summary>
        ''' <param name="shellObject">The shell container to navigate to.</param>
        ''' <exception cref="System.Runtime.InteropServices.COMException">Will throw if navigation fails for any other reason.</exception>
        Public Sub Navigate(shellObject As ShellObject)
            If shellObject Is Nothing Then
                Throw New ArgumentNullException("shellObject")
            End If

            If explorerBrowserControl Is Nothing Then
                antecreationNavigationTarget = shellObject
            Else
                Dim hr As HResult = explorerBrowserControl.BrowseToObject(shellObject.NativeShellItem, 0)
                If hr <> HResult.Ok Then
                    If (hr = HResult.ResourceInUse OrElse hr = HResult.Canceled) AndAlso _NavigationFailed IsNot Nothing Then
                        Dim args As New NavigationFailedEventArgs()
                        args.FailedLocation = shellObject
                        RaiseEvent NavigationFailed(Me, args)
                    Else
                        Throw New CommonControlException(LocalizedMessages.ExplorerBrowserBrowseToObjectFailed, hr)
                    End If
                End If
            End If
        End Sub

        ''' <summary>
        ''' Navigates within the navigation log. This does not change the set of 
        ''' locations in the navigation log.
        ''' </summary>
        ''' <param name="direction">Forward of Backward</param>
        ''' <returns>True if the navigation succeeded, false if it failed for any reason.</returns>
        Public Function NavigateLogLocation(direction As NavigationLogDirection) As Boolean
            Return NavigationLog.NavigateLog(direction)
        End Function

        ''' <summary>
        ''' Navigate within the navigation log. This does not change the set of 
        ''' locations in the navigation log.
        ''' </summary>
        ''' <param name="navigationLogIndex">An index into the navigation logs Locations collection.</param>
        ''' <returns>True if the navigation succeeded, false if it failed for any reason.</returns>
        Public Function NavigateLogLocation(navigationLogIndex As Integer) As Boolean
            Return NavigationLog.NavigateLog(navigationLogIndex)
        End Function
#End Region

#Region "events"

        ''' <summary>
        ''' Fires when the SelectedItems collection changes. 
        ''' </summary>
        Public Event SelectionChanged As EventHandler

        Dim _ItemsChanged As EventHandler

        ''' <summary>
        ''' Fires when the Items colection changes. 
        ''' </summary>
        Public Custom Event ItemsChanged As EventHandler
            AddHandler(value As EventHandler)
                _ItemsChanged = value
            End AddHandler
            RemoveHandler(value As EventHandler)
                _ItemsChanged = Nothing
            End RemoveHandler
            RaiseEvent(sender As Object, e As EventArgs)
                _ItemsChanged.Invoke(sender, e)
            End RaiseEvent
        End Event

        Dim _NavigationPending As EventHandler(Of NavigationPendingEventArgs)

        ''' <summary>
        ''' Fires when a navigation has been initiated, but is not yet complete.
        ''' </summary>
        Public Custom Event NavigationPending As EventHandler(Of NavigationPendingEventArgs)
            AddHandler(value As EventHandler(Of NavigationPendingEventArgs))
                _NavigationPending = value
            End AddHandler
            RemoveHandler(value As EventHandler(Of NavigationPendingEventArgs))
                _NavigationPending = Nothing
            End RemoveHandler
            RaiseEvent(sender As Object, e As NavigationPendingEventArgs)
                Call _NavigationPending.Invoke(sender, e)
            End RaiseEvent
        End Event

        Dim _NavigationComplete As EventHandler(Of NavigationCompleteEventArgs)

        ''' <summary>
        ''' Fires when a navigation has been 'completed': no NavigationPending listener 
        ''' has cancelled, and the ExplorerBorwser has created a new view. The view 
        ''' will be populated with new items asynchronously, and ItemsChanged will be 
        ''' fired to reflect this some time later.
        ''' </summary>
        Public Custom Event NavigationComplete As EventHandler(Of NavigationCompleteEventArgs)
            AddHandler(value As EventHandler(Of NavigationCompleteEventArgs))
                _NavigationComplete = value
            End AddHandler
            RemoveHandler(value As EventHandler(Of NavigationCompleteEventArgs))
                _NavigationComplete = Nothing
            End RemoveHandler
            RaiseEvent(sender As Object, e As NavigationCompleteEventArgs)
                Call _NavigationComplete.Invoke(sender, e)
            End RaiseEvent
        End Event

        Dim _NavigationFailed As EventHandler(Of NavigationFailedEventArgs)

        ''' <summary>
        ''' Fires when either a NavigationPending listener cancels the navigation, or
        ''' if the operating system determines that navigation is not possible.
        ''' </summary>
        Public Custom Event NavigationFailed As EventHandler(Of NavigationFailedEventArgs)
            AddHandler(value As EventHandler(Of NavigationFailedEventArgs))
                _NavigationFailed = value
            End AddHandler
            RemoveHandler(value As EventHandler(Of NavigationFailedEventArgs))
                _NavigationFailed = Nothing
            End RemoveHandler
            RaiseEvent(sender As Object, e As NavigationFailedEventArgs)
                Call _NavigationFailed.Invoke(sender, e)
            End RaiseEvent
        End Event

        Dim _ViewEnumerationComplete As EventHandler

        ''' <summary>
        ''' Fires when the ExplorerBorwser view has finished enumerating files.
        ''' </summary>
        Public Custom Event ViewEnumerationComplete As EventHandler
            AddHandler(value As EventHandler)
                _ViewEnumerationComplete = value
            End AddHandler
            RemoveHandler(value As EventHandler)
                _ViewEnumerationComplete = Nothing
            End RemoveHandler
            RaiseEvent(sender As Object, e As EventArgs)
                Call _ViewEnumerationComplete.Invoke(sender, e)
            End RaiseEvent
        End Event

        Dim _ViewSelectedItemChanged As EventHandler

        ''' <summary>
        ''' Fires when the item selected in the view has changed (i.e., a rename ).
        ''' This is not the same as SelectionChanged.
        ''' </summary>
        Public Custom Event ViewSelectedItemChanged As EventHandler
            AddHandler(value As EventHandler)
                _ViewSelectedItemChanged = value
            End AddHandler
            RemoveHandler(value As EventHandler)
                _ViewSelectedItemChanged = Nothing
            End RemoveHandler
            RaiseEvent(sender As Object, e As EventArgs)
                Call _ViewSelectedItemChanged.Invoke(sender, e)
            End RaiseEvent
        End Event

#End Region

#Region "implementation"

#Region "construction"
        Friend explorerBrowserControl As ExplorerBrowserClass

        ' for the IExplorerBrowserEvents Advise call
        Friend eventsCookie As UInteger

        ' name of the property bag that contains the view state options of the browser
        Private m_propertyBagName As String = GetType(ExplorerBrowser).FullName

        ''' <summary>
        ''' Initializes the ExplorerBorwser WinForms wrapper.
        ''' </summary>
        Public Sub New()
            MyBase.New()
            NavigationOptions = New ExplorerBrowserNavigationOptions(Me)
            ContentOptions = New ExplorerBrowserContentOptions(Me)
            NavigationLog = New ExplorerBrowserNavigationLog(Me)
        End Sub

#End Region

#Region "message handlers"

        ''' <summary>
        ''' Displays a placeholder for the explorer browser in design mode
        ''' </summary>
        ''' <param name="e">Contains information about the paint event.</param>
        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            If DesignMode AndAlso e IsNot Nothing Then
                Using linGrBrush As New LinearGradientBrush(ClientRectangle, Color.Aqua, Color.CadetBlue, LinearGradientMode.ForwardDiagonal)
                    e.Graphics.FillRectangle(linGrBrush, ClientRectangle)
                End Using

                Using font As New Font("Garamond", 30)
                    Using sf As New StringFormat()
                        sf.Alignment = StringAlignment.Center
                        sf.LineAlignment = StringAlignment.Center
                        e.Graphics.DrawString("ExplorerBrowserControl", font, Brushes.White, ClientRectangle, sf)
                    End Using
                End Using
            End If

            MyBase.OnPaint(e)
        End Sub

        Private antecreationNavigationTarget As ShellObject
        Private viewEvents As ExplorerBrowserViewEvents

        ''' <summary>
        ''' Creates and initializes the native ExplorerBrowser control
        ''' </summary>
        Protected Overrides Sub OnCreateControl()
            MyBase.OnCreateControl()

            If Me.DesignMode = False Then
                explorerBrowserControl = New ExplorerBrowserClass()

                ' hooks up IExplorerPaneVisibility and ICommDlgBrowser event notifications
                ExplorerBrowserNativeMethods.IUnknown_SetSite(explorerBrowserControl, Me)

                ' hooks up IExplorerBrowserEvents event notification
                explorerBrowserControl.Advise(Marshal.GetComInterfaceForObject(Me, GetType(IExplorerBrowserEvents)), eventsCookie)

                ' sets up ExplorerBrowser view connection point events
                viewEvents = New ExplorerBrowserViewEvents(Me)

                Dim rect As New NativeRect()
                rect.Top = ClientRectangle.Top
                rect.Left = ClientRectangle.Left
                rect.Right = ClientRectangle.Right
                rect.Bottom = ClientRectangle.Bottom

                explorerBrowserControl.Initialize(Me.Handle, rect, Nothing)

                ' Force an initial show frames so that IExplorerPaneVisibility works the first time it is set.
                ' This also enables the control panel to be browsed to. If it is not set, then navigating to 
                ' the control panel succeeds, but no items are visible in the view.
                explorerBrowserControl.SetOptions(ExplorerBrowserOptions.ShowFrames)

                explorerBrowserControl.SetPropertyBag(m_propertyBagName)

                If antecreationNavigationTarget IsNot Nothing Then
                    BeginInvoke(New MethodInvoker(Sub()
                                                      Navigate(antecreationNavigationTarget)
                                                      antecreationNavigationTarget = Nothing

                                                  End Sub))
                End If
            End If

            Application.AddMessageFilter(Me)
        End Sub

        ''' <summary>
        ''' Sizes the native control to match the WinForms control wrapper.
        ''' </summary>
        ''' <param name="e">Contains information about the size changed event.</param>
        Protected Overrides Sub OnSizeChanged(e As EventArgs)
            If explorerBrowserControl IsNot Nothing Then
                Dim rect As New NativeRect()
                rect.Top = ClientRectangle.Top
                rect.Left = ClientRectangle.Left
                rect.Right = ClientRectangle.Right
                rect.Bottom = ClientRectangle.Bottom

                Dim ptr As IntPtr = IntPtr.Zero
                explorerBrowserControl.SetRect(ptr, rect)
            End If

            MyBase.OnSizeChanged(e)
        End Sub

        ''' <summary>
        ''' Cleans up the explorer browser events+object when the window is being taken down.
        ''' </summary>
        ''' <param name="e">An EventArgs that contains event data.</param>
        Protected Overrides Sub OnHandleDestroyed(e As EventArgs)
            If explorerBrowserControl IsNot Nothing Then
                ' unhook events
                viewEvents.DisconnectFromView()
                explorerBrowserControl.Unadvise(eventsCookie)
                ExplorerBrowserNativeMethods.IUnknown_SetSite(explorerBrowserControl, Nothing)

                ' destroy the explorer browser control
                explorerBrowserControl.Destroy()

                ' release com reference to it
                Marshal.ReleaseComObject(explorerBrowserControl)
                explorerBrowserControl = Nothing
            End If

            MyBase.OnHandleDestroyed(e)
        End Sub
#End Region

#Region "object interfaces"

#Region "IServiceProvider"
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="guidService">calling service</param>
        ''' <param name="riid">requested interface guid</param>
        ''' <param name="ppvObject">caller-allocated memory for interface pointer</param>
        ''' <returns></returns>
        Private Function Microsoft_WindowsAPICodePack_Controls_IServiceProvider_QueryService(ByRef guidService As Guid, ByRef riid As Guid, ByRef ppvObject As IntPtr) As HResult Implements Controls.IServiceProvider.QueryService
            Dim hr As HResult = HResult.Ok

            If guidService.CompareTo(New Guid(ExplorerBrowserIIDGuid.IExplorerPaneVisibility)) = 0 Then
                ' Responding to this SID allows us to control the visibility of the 
                ' explorer browser panes
                ppvObject = Marshal.GetComInterfaceForObject(Me, GetType(IExplorerPaneVisibility))
                hr = HResult.Ok
            ElseIf guidService.CompareTo(New Guid(ExplorerBrowserIIDGuid.ICommDlgBrowser)) = 0 Then
                If riid.CompareTo(New Guid(ExplorerBrowserIIDGuid.ICommDlgBrowser)) = 0 Then
                    ppvObject = Marshal.GetComInterfaceForObject(Me, GetType(ICommDlgBrowser3))
                    hr = HResult.Ok
                    ' The below lines are commented out to decline requests for the ICommDlgBrowser2 interface.
                    ' This interface is incorrectly marshaled back to unmanaged, and causes an exception.
                    ' There is a bug for this, I have not figured the underlying cause.
                    ' Remove this comment and uncomment the following code to enable the ICommDlgBrowser2 interface
                    'else if (riid.CompareTo(new Guid(ExplorerBrowserIIDGuid.ICommDlgBrowser2)) == 0)
                    '{
                    '    ppvObject = Marshal.GetComInterfaceForObject(this, typeof(ICommDlgBrowser3));
                    '    hr = HResult.Ok;                    
                    '}
                ElseIf riid.CompareTo(New Guid(ExplorerBrowserIIDGuid.ICommDlgBrowser3)) = 0 Then
                    ppvObject = Marshal.GetComInterfaceForObject(Me, GetType(ICommDlgBrowser3))
                    hr = HResult.Ok
                Else
                    ppvObject = IntPtr.Zero
                    hr = HResult.NoInterface
                End If
            Else
                Dim nullObj As IntPtr = IntPtr.Zero
                ppvObject = nullObj
                hr = HResult.NoInterface
            End If

            Return hr
        End Function
#End Region

#Region "IExplorerPaneVisibility"
        ''' <summary>
        ''' Controls the visibility of the explorer borwser panes
        ''' </summary>
        ''' <param name="explorerPane">a guid identifying the pane</param>
        ''' <param name="peps">the pane state desired</param>
        ''' <returns></returns>
        Private Function IExplorerPaneVisibility_GetPaneState(ByRef explorerPane As Guid, ByRef peps As ExplorerPaneState) As HResult Implements IExplorerPaneVisibility.GetPaneState
            Select Case explorerPane.ToString()
                Case ExplorerBrowserViewPanes.AdvancedQuery
                    peps = VisibilityToPaneState(NavigationOptions.PaneVisibility.AdvancedQuery)
                    Exit Select
                Case ExplorerBrowserViewPanes.Commands
                    peps = VisibilityToPaneState(NavigationOptions.PaneVisibility.Commands)
                    Exit Select
                Case ExplorerBrowserViewPanes.CommandsOrganize
                    peps = VisibilityToPaneState(NavigationOptions.PaneVisibility.CommandsOrganize)
                    Exit Select
                Case ExplorerBrowserViewPanes.CommandsView
                    peps = VisibilityToPaneState(NavigationOptions.PaneVisibility.CommandsView)
                    Exit Select
                Case ExplorerBrowserViewPanes.Details
                    peps = VisibilityToPaneState(NavigationOptions.PaneVisibility.Details)
                    Exit Select
                Case ExplorerBrowserViewPanes.Navigation
                    peps = VisibilityToPaneState(NavigationOptions.PaneVisibility.Navigation)
                    Exit Select
                Case ExplorerBrowserViewPanes.Preview
                    peps = VisibilityToPaneState(NavigationOptions.PaneVisibility.Preview)
                    Exit Select
                Case ExplorerBrowserViewPanes.Query
                    peps = VisibilityToPaneState(NavigationOptions.PaneVisibility.Query)
                    Exit Select
                Case Else
#If LOG_UNKNOWN_PANES Then
					System.Diagnostics.Debugger.Log(4, "ExplorerBrowser", "unknown pane view state. id=" & explorerPane.ToString())
#End If
                    peps = VisibilityToPaneState(PaneVisibilityState.Show)
                    Exit Select
            End Select

            Return HResult.Ok
        End Function

        Private Shared Function VisibilityToPaneState(visibility As PaneVisibilityState) As ExplorerPaneState
            Select Case visibility
                Case PaneVisibilityState.DoNotCare
                    Return ExplorerPaneState.DoNotCare

                Case PaneVisibilityState.Hide
                    Return ExplorerPaneState.DefaultOff Or ExplorerPaneState.Force

                Case PaneVisibilityState.Show
                    Return ExplorerPaneState.DefaultOn Or ExplorerPaneState.Force
                Case Else

                    Throw New ArgumentException("unexpected PaneVisibilityState")
            End Select
        End Function

#End Region

#Region "IExplorerBrowserEvents"
        Private Function IExplorerBrowserEvents_OnNavigationPending(pidlFolder As IntPtr) As HResult Implements IExplorerBrowserEvents.OnNavigationPending
            Dim canceled As Boolean = False

            If _NavigationPending IsNot Nothing Then
                Dim args As New NavigationPendingEventArgs()

                ' For some special items (like network machines), ShellObject.FromIDList
                ' might return null
                args.PendingLocation = ShellObjectFactory.Create(pidlFolder)

                If args.PendingLocation IsNot Nothing Then
                    For Each del As [Delegate] In _NavigationPending.GetInvocationList()
                        del.DynamicInvoke(New Object() {Me, args})
                        If args.Cancel Then
                            canceled = True
                        End If
                    Next
                End If
            End If

            Return If(canceled, HResult.Canceled, HResult.Ok)
        End Function

        Private Function IExplorerBrowserEvents_OnViewCreated(psv As Object) As HResult Implements IExplorerBrowserEvents.OnViewCreated
            viewEvents.ConnectToView(DirectCast(psv, IShellView))

            Return HResult.Ok
        End Function

        Private Function IExplorerBrowserEvents_OnNavigationComplete(pidlFolder As IntPtr) As HResult Implements IExplorerBrowserEvents.OnNavigationComplete
            ' view mode may change 
            ContentOptions.folderSettings.ViewMode = GetCurrentViewMode()

            If _NavigationComplete IsNot Nothing Then
                Dim args As New NavigationCompleteEventArgs()
                args.NewLocation = ShellObjectFactory.Create(pidlFolder)
                RaiseEvent NavigationComplete(Me, args)
            End If
            Return HResult.Ok
        End Function

        Private Function IExplorerBrowserEvents_OnNavigationFailed(pidlFolder As IntPtr) As HResult Implements IExplorerBrowserEvents.OnNavigationFailed
            If _NavigationFailed IsNot Nothing Then
                Dim args As New NavigationFailedEventArgs()
                args.FailedLocation = ShellObjectFactory.Create(pidlFolder)
                RaiseEvent NavigationFailed(Me, args)
            End If
            Return HResult.Ok
        End Function
#End Region

#Region "ICommDlgBrowser"
        Private Function ICommDlgBrowser3_OnDefaultCommand(ppshv As IntPtr) As HResult Implements ICommDlgBrowser3.OnDefaultCommand
            Return HResult.[False]
            'return HResult.Ok;
        End Function

        Private Function ICommDlgBrowser3_OnStateChange(ppshv As IntPtr, uChange As CommDlgBrowserStateChange) As HResult Implements ICommDlgBrowser3.OnStateChange
            If uChange = CommDlgBrowserStateChange.SelectionChange Then
                FireSelectionChanged()
            End If

            Return HResult.Ok
        End Function

        Private Function ICommDlgBrowser3_IncludeObject(ppshv As IntPtr, pidl As IntPtr) As HResult Implements ICommDlgBrowser3.IncludeObject
            ' items in the view have changed, so the collections need updating
            FireContentChanged()

            Return HResult.Ok
        End Function

#End Region

#Region "ICommDlgBrowser2 Members"

        ' The below methods can be called into, but marshalling the response causes an exception to be
        ' thrown from unmanaged code.  At this time, I decline calls requesting the ICommDlgBrowser2
        ' interface.  This is logged as a bug, but moved to less of a priority, as it only affects being
        ' able to change the default action text for remapping the default action.

        Private Function ICommDlgBrowser3_GetDefaultMenuText(shellView As IShellView, text As IntPtr, cchMax As Integer) As HResult Implements ICommDlgBrowser3.GetDefaultMenuText
            Return HResult.[False]
            'return HResult.Ok;
            'OK if new
            'False if default
            'other if error
        End Function

        Private Function ICommDlgBrowser3_GetViewFlags(ByRef pdwFlags As UInteger) As HResult Implements ICommDlgBrowser3.GetViewFlags
            'var flags = CommDlgBrowser2ViewFlags.NoSelectVerb;
            'Marshal.WriteInt32(pdwFlags, 0);
            pdwFlags = CUInt(CommDlgBrowser2ViewFlags.ShowAllFiles)
            Return HResult.Ok
        End Function

        Private Function ICommDlgBrowser3_Notify(pshv As IntPtr, notifyType As CommDlgBrowserNotifyType) As HResult Implements ICommDlgBrowser3.Notify
            Return HResult.Ok
        End Function

#End Region

#Region "ICommDlgBrowser3 Members"

        Private Function ICommDlgBrowser3_GetCurrentFilter(pszFileSpec As StringBuilder, cchFileSpec As Integer) As HResult Implements ICommDlgBrowser3.GetCurrentFilter
            ' If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
            Return HResult.Ok
        End Function

        Private Function ICommDlgBrowser3_OnColumnClicked(ppshv As IShellView, iColumn As Integer) As HResult Implements ICommDlgBrowser3.OnColumnClicked
            ' If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
            Return HResult.Ok
        End Function

        Private Function ICommDlgBrowser3_OnPreViewCreated(ppshv As IShellView) As HResult Implements ICommDlgBrowser3.OnPreViewCreated
            ' If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code
            Return HResult.Ok
        End Function

#End Region

#Region "IMessageFilter Members"

        Private Function IMessageFilter_PreFilterMessage(ByRef m As System.Windows.Forms.Message) As Boolean Implements IMessageFilter.PreFilterMessage
            Dim hr As HResult = HResult.[False]
            If explorerBrowserControl IsNot Nothing Then
                ' translate keyboard input
                hr = DirectCast(explorerBrowserControl, IInputObject).TranslateAcceleratorIO(m)
            End If
            Return (hr = HResult.Ok)
        End Function

#End Region

#End Region

#Region "utilities"

        ''' <summary>
        ''' Returns the current view mode of the browser
        ''' </summary>
        ''' <returns></returns>
        Friend Function GetCurrentViewMode() As FolderViewMode
            Dim ifv2 As IFolderView2 = GetFolderView2()
            Dim viewMode As UInteger = 0
            If ifv2 IsNot Nothing Then
                Try
                    Dim hr As HResult = ifv2.GetCurrentViewMode(viewMode)
                    If hr <> HResult.Ok Then
                        Throw New ShellException(hr)
                    End If
                Finally
                    Marshal.ReleaseComObject(ifv2)
                    ifv2 = Nothing
                End Try
            End If
            Return CType(viewMode, FolderViewMode)
        End Function

        ''' <summary>
        ''' Gets the IFolderView2 interface from the explorer browser.
        ''' </summary>
        ''' <returns></returns>
        Friend Function GetFolderView2() As IFolderView2
            Dim iid As New Guid(ExplorerBrowserIIDGuid.IFolderView2)
            Dim view As IntPtr = IntPtr.Zero
            If Me.explorerBrowserControl IsNot Nothing Then
                Dim hr As HResult = Me.explorerBrowserControl.GetCurrentView(iid, view)
                Select Case hr
                    Case HResult.Ok
                        Exit Select

                    Case HResult.NoInterface, HResult.Fail
#If LOG_KNOWN_COM_ERRORS Then
						Debugger.Log(2, "ExplorerBrowser", "Unable to obtain view. Error=" & e.ToString())
#End If
                        Return Nothing
                    Case Else

                        Throw New CommonControlException(LocalizedMessages.ExplorerBrowserFailedToGetView, hr)
                End Select

                Return DirectCast(Marshal.GetObjectForIUnknown(view), IFolderView2)
            End If
            Return Nothing
        End Function

        ''' <summary>
        ''' Gets the selected items in the explorer browser as an IShellItemArray
        ''' </summary>
        ''' <returns></returns>
        Friend Function GetSelectedItemsArray() As IShellItemArray
            Dim iArray As IShellItemArray = Nothing
            Dim iFV2 As IFolderView2 = GetFolderView2()
            If iFV2 IsNot Nothing Then
                Try
                    Dim iidShellItemArray As New Guid(ShellIIDGuid.IShellItemArray)
                    Dim oArray As Object = Nothing
                    Dim hr As HResult = iFV2.Items(CUInt(ShellViewGetItemObject.Selection), iidShellItemArray, oArray)
                    iArray = TryCast(oArray, IShellItemArray)
                    If hr <> HResult.Ok AndAlso hr <> HResult.ElementNotFound AndAlso hr <> HResult.Fail Then
                        Throw New CommonControlException(LocalizedMessages.ExplorerBrowserUnexpectedError, hr)
                    End If
                Finally
                    Marshal.ReleaseComObject(iFV2)
                    iFV2 = Nothing
                End Try
            End If

            Return iArray
        End Function

        Friend Function GetItemsCount() As Integer
            Dim itemsCount As Integer = 0

            Dim iFV2 As IFolderView2 = GetFolderView2()
            If iFV2 IsNot Nothing Then
                Try
                    Dim hr As HResult = iFV2.ItemCount(CUInt(ShellViewGetItemObject.AllView), itemsCount)

                    If hr <> HResult.Ok AndAlso hr <> HResult.ElementNotFound AndAlso hr <> HResult.Fail Then
                        Throw New CommonControlException(LocalizedMessages.ExplorerBrowserItemCount, hr)
                    End If
                Finally
                    Marshal.ReleaseComObject(iFV2)
                    iFV2 = Nothing
                End Try
            End If

            Return itemsCount
        End Function

        Friend Function GetSelectedItemsCount() As Integer
            Dim itemsCount As Integer = 0

            Dim iFV2 As IFolderView2 = GetFolderView2()
            If iFV2 IsNot Nothing Then
                Try
                    Dim hr As HResult = iFV2.ItemCount(CUInt(ShellViewGetItemObject.Selection), itemsCount)

                    If hr <> HResult.Ok AndAlso hr <> HResult.ElementNotFound AndAlso hr <> HResult.Fail Then
                        Throw New CommonControlException(LocalizedMessages.ExplorerBrowserSelectedItemCount, hr)
                    End If
                Finally
                    Marshal.ReleaseComObject(iFV2)
                    iFV2 = Nothing
                End Try
            End If

            Return itemsCount
        End Function

        ''' <summary>
        ''' Gets the items in the ExplorerBrowser as an IShellItemArray
        ''' </summary>
        ''' <returns></returns>
        Friend Function GetItemsArray() As IShellItemArray
            Dim iArray As IShellItemArray = Nothing
            Dim iFV2 As IFolderView2 = GetFolderView2()
            If iFV2 IsNot Nothing Then
                Try
                    Dim iidShellItemArray As New Guid(ShellIIDGuid.IShellItemArray)
                    Dim oArray As Object = Nothing
                    Dim hr As HResult = iFV2.Items(CUInt(ShellViewGetItemObject.AllView), iidShellItemArray, oArray)
                    If hr <> HResult.Ok AndAlso hr <> HResult.Fail AndAlso hr <> HResult.ElementNotFound AndAlso hr <> HResult.InvalidArguments Then
                        Throw New CommonControlException(LocalizedMessages.ExplorerBrowserViewItems, hr)
                    End If

                    iArray = TryCast(oArray, IShellItemArray)
                Finally
                    Marshal.ReleaseComObject(iFV2)
                    iFV2 = Nothing
                End Try
            End If
            Return iArray
        End Function

#End Region

#Region "view event forwarding"
        Friend Sub FireSelectionChanged()
            RaiseEvent SelectionChanged(Me, EventArgs.Empty)
        End Sub

        Friend Sub FireContentChanged()
            If _ItemsChanged IsNot Nothing Then
                _ItemsChanged.Invoke(Me, EventArgs.Empty)
            End If
        End Sub

        Friend Sub FireContentEnumerationComplete()
            If _ViewEnumerationComplete IsNot Nothing Then
                _ViewEnumerationComplete.Invoke(Me, EventArgs.Empty)
            End If
        End Sub

        Friend Sub FireSelectedItemChanged()
            If _ViewSelectedItemChanged IsNot Nothing Then
                _ViewSelectedItemChanged.Invoke(Me, EventArgs.Empty)
            End If
        End Sub
#End Region

#End Region

    End Class

End Namespace
