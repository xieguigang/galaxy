'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports Microsoft.Windows.Shell
Imports Microsoft.Windows.Internal
Imports System.Text
Imports Microsoft.Windows.Shell.Interop

Namespace Controls

	Friend Enum ShellViewGetItemObject
		Background = &H0
		Selection = &H1
		AllView = &H2
		Checked = &H3
		TypeMask = &HF
        ViewOrderFlag = &H80000000UI
    End Enum

	<Flags> _
	Friend Enum FolderOptions
		AutoArrange = &H1
		AbbreviatedNames = &H2
		SnapToGrid = &H4
		OwnerData = &H8
		BestFitWindow = &H10
		Desktop = &H20
		SingleSelection = &H40
		NoSubfolders = &H80
		Transparent = &H100
		NoClientEdge = &H200
		NoScroll = &H400
		AlignLeft = &H800
		NoIcons = &H1000
		ShowSelectionAlways = &H2000
		NoVisible = &H4000
		SingleClickActivate = &H8000
		NoWebView = &H10000
		HideFilenames = &H20000
		CheckSelect = &H40000
		NoEnumRefresh = &H80000
		NoGrouping = &H100000
		FullRowSelect = &H200000
		NoFilters = &H400000
		NoColumnHeaders = &H800000
		NoHeaderInAllViews = &H1000000
		ExtendedTiles = &H2000000
		TriCheckSelect = &H4000000
		AutoCheckSelect = &H8000000
		NoBrowserViewState = &H10000000
		SubsetGroups = &H20000000
		UseSearchFolders = &H40000000
        AllowRightToLeftReading = &H80000000UI
    End Enum

	Friend Enum FolderViewMode
		Auto = -1
		First = 1
		Icon = 1
		SmallIcon = 2
		List = 3
		Details = 4
		Thumbnail = 5
		Tile = 6
		Thumbstrip = 7
		Content = 8
		Last = 8
	End Enum

	Friend Enum ExplorerPaneState
		DoNotCare = &H0
		DefaultOn = &H1
		DefaultOff = &H2
		StateMask = &Hffff
		InitialState = &H10000
		Force = &H20000
	End Enum

	<StructLayout(LayoutKind.Sequential, Pack := 4)> _
	Friend Class FolderSettings
		Public ViewMode As FolderViewMode
		Public Options As FolderOptions
	End Class

	<Flags> _
	Friend Enum ExplorerBrowserOptions
		NavigateOnce = &H1
		ShowFrames = &H2
		AlwaysNavigate = &H4
		NoTravelLog = &H8
		NoWrapperWindow = &H10
		HtmlSharepointView = &H20
	End Enum

	Friend Enum CommDlgBrowserStateChange
		SetFocus = 0
		KillFocus = 1
		SelectionChange = 2
		Rename = 3
		StateChange = 4
	End Enum

	Friend Enum CommDlgBrowserNotifyType
		Done = 1
		Start = 2
	End Enum

	Friend Enum CommDlgBrowser2ViewFlags
		ShowAllFiles = &H1
		IsFileSave = &H2
		AllowPreviewPane = &H4
		NoSelectVerb = &H8
		NoIncludeItem = &H10
		IsFolderPicker = &H20
	End Enum

    ' Disable warning if a method declaration hides another inherited from a parent COM interface
    ' To successfully import a COM interface, all inherited methods need to be declared again with 
    ' the exception of those already declared in "IUnknown"
    '#Pragma warning disable 108


    <ComImport, TypeLibType(TypeLibTypeFlags.FCanCreate), ClassInterface(ClassInterfaceType.None), Guid(ExplorerBrowserCLSIDGuid.ExplorerBrowser)> _
	Friend Class ExplorerBrowserClass
		Implements IExplorerBrowser
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub Initialize(hwndParent As IntPtr, <[In]> ByRef prc As NativeRect, <[In]> pfs As FolderSettings) Implements IExplorerBrowser.Initialize
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub Destroy() Implements IExplorerBrowser.Destroy
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub SetRect(<[In], Out> ByRef phdwp As IntPtr, rcBrowser As NativeRect) Implements IExplorerBrowser.SetRect
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub SetPropertyBag(<MarshalAs(UnmanagedType.LPWStr)> pszPropertyBag As String) Implements IExplorerBrowser.SetPropertyBag
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub SetEmptyText(<MarshalAs(UnmanagedType.LPWStr)> pszEmptyText As String) Implements IExplorerBrowser.SetEmptyText
        End Sub

        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Function SetFolderSettings(pfs As FolderSettings) As HResult Implements IExplorerBrowser.SetFolderSettings
        End Function

        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Function Advise(psbe As IntPtr, ByRef pdwCookie As UInteger) As HResult Implements IExplorerBrowser.Advise
        End Function

        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Function Unadvise(dwCookie As UInteger) As HResult Implements IExplorerBrowser.Unadvise
        End Function

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub SetOptions(<[In]> dwFlag As ExplorerBrowserOptions) Implements IExplorerBrowser.SetOptions
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub GetOptions(ByRef pdwFlag As ExplorerBrowserOptions) Implements IExplorerBrowser.GetOptions
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub BrowseToIDList(pidl As IntPtr, uFlags As UInteger) Implements IExplorerBrowser.BrowseToIDList
        End Sub

        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Function BrowseToObject(<MarshalAs(UnmanagedType.IUnknown)> punk As Object, uFlags As UInteger) As HResult Implements IExplorerBrowser.BrowseToObject
        End Function

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub FillFromObject(<MarshalAs(UnmanagedType.IUnknown)> punk As Object, dwFlags As Integer) Implements IExplorerBrowser.FillFromObject
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub RemoveAll() Implements IExplorerBrowser.RemoveAll
        End Sub

        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Function GetCurrentView(ByRef riid As Guid, ByRef ppv As IntPtr) As HResult Implements IExplorerBrowser.GetCurrentView
        End Function
    End Class


	<ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid(ExplorerBrowserIIDGuid.IExplorerBrowser)> _
	Friend Interface IExplorerBrowser
        ''' <summary>
        ''' Prepares the browser to be navigated.
        ''' </summary>
        ''' <param name="hwndParent">A handle to the owner window or control.</param>
        ''' <param name="prc">A pointer to a RECT containing the coordinates of the bounding rectangle 
        ''' the browser will occupy. The coordinates are relative to hwndParent. If this parameter is NULL,
        ''' then method IExplorerBrowser::SetRect should subsequently be called.</param>
        ''' <param name="pfs">A pointer to a FOLDERSETTINGS structure that determines how the folder will be
        ''' displayed in the view. If this parameter is NULL, then method IExplorerBrowser::SetFolderSettings
        ''' should be called, otherwise, the default view settings for the folder are used.</param>

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub Initialize(hwndParent As IntPtr, <[In]> ByRef prc As NativeRect, <[In]> pfs As FolderSettings)

        ''' <summary>
        ''' Destroys the browser.
        ''' </summary>

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub Destroy()

        ''' <summary>
        ''' Sets the size and position of the view windows created by the browser.
        ''' </summary>
        ''' <param name="phdwp">A pointer to a DeferWindowPos handle. This paramater can be NULL.</param>
        ''' <param name="rcBrowser">The coordinates that the browser will occupy.</param>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub SetRect(<[In], Out> ByRef phdwp As IntPtr, rcBrowser As NativeRect)

        ''' <summary>
        ''' Sets the name of the property bag.
        ''' </summary>
        ''' <param name="pszPropertyBag">A pointer to a constant, null-terminated, Unicode string that contains
        ''' the name of the property bag. View state information that is specific to the application of the 
        ''' client is stored (persisted) using this name.</param>

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub SetPropertyBag(<MarshalAs(UnmanagedType.LPWStr)> pszPropertyBag As String)

        ''' <summary>
        ''' Sets the default empty text.
        ''' </summary>
        ''' <param name="pszEmptyText">A pointer to a constant, null-terminated, Unicode string that contains 
        ''' the empty text.</param>

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub SetEmptyText(<MarshalAs(UnmanagedType.LPWStr)> pszEmptyText As String)

		''' <summary>
		''' Sets the folder settings for the current view.
		''' </summary>
		''' <param name="pfs">A pointer to a FOLDERSETTINGS structure that contains the folder settings 
		''' to be applied.</param>
		''' <returns></returns>
		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function SetFolderSettings(pfs As FolderSettings) As HResult

		''' <summary>
		''' Initiates a connection with IExplorerBrowser for event callbacks.
		''' </summary>
		''' <param name="psbe">A pointer to the IExplorerBrowserEvents interface of the object to be 
		''' advised of IExplorerBrowser events</param>
		''' <param name="pdwCookie">When this method returns, contains a token that uniquely identifies 
		''' the event listener. This allows several event listeners to be subscribed at a time.</param>
		''' <returns></returns>
		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function Advise(psbe As IntPtr, ByRef pdwCookie As UInteger) As HResult

		''' <summary>
		''' Terminates an advisory connection.
		''' </summary>
		''' <param name="dwCookie">A connection token previously returned from IExplorerBrowser::Advise.
		''' Identifies the connection to be terminated.</param>
		''' <returns></returns>
		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function Unadvise(<[In]> dwCookie As UInteger) As HResult

        ''' <summary>
        ''' Sets the current browser options.
        ''' </summary>
        ''' <param name="dwFlag">One or more EXPLORER_BROWSER_OPTIONS flags to be set.</param>

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub SetOptions(<[In]> dwFlag As ExplorerBrowserOptions)

        ''' <summary>
        ''' Gets the current browser options.
        ''' </summary>
        ''' <param name="pdwFlag">When this method returns, contains the current EXPLORER_BROWSER_OPTIONS 
        ''' for the browser.</param>

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub GetOptions(ByRef pdwFlag As ExplorerBrowserOptions)

        ''' <summary>
        ''' Browses to a pointer to an item identifier list (PIDL)
        ''' </summary>
        ''' <param name="pidl">A pointer to a const ITEMIDLIST (item identifier list) that specifies an object's 
        ''' location as the destination to navigate to. This parameter can be NULL.</param>
        ''' <param name="uFlags">A flag that specifies the category of the pidl. This affects how 
        ''' navigation is accomplished</param>

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub BrowseToIDList(pidl As IntPtr, uFlags As UInteger)

		''' <summary>
		''' Browse to an object
		''' </summary>
		''' <param name="punk">A pointer to an object to browse to. If the object cannot be browsed, 
		''' an error value is returned.</param>
		''' <param name="uFlags">A flag that specifies the category of the pidl. This affects how 
		''' navigation is accomplished. </param>
		''' <returns></returns>
		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function BrowseToObject(<MarshalAs(UnmanagedType.IUnknown)> punk As Object, uFlags As UInteger) As HResult

        ''' <summary>
        ''' Creates a results folder and fills it with items.
        ''' </summary>
        ''' <param name="punk">An interface pointer on the source object that will fill the IResultsFolder</param>
        ''' <param name="dwFlags">One of the EXPLORER_BROWSER_FILL_FLAGS</param>

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub FillFromObject(<MarshalAs(UnmanagedType.IUnknown)> punk As Object, dwFlags As Integer)

        ''' <summary>
        ''' Removes all items from the results folder.
        ''' </summary>

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub RemoveAll()

		''' <summary>
		''' Gets an interface for the current view of the browser.
		''' </summary>
		''' <param name="riid">A reference to the desired interface ID.</param>
		''' <param name="ppv">When this method returns, contains the interface pointer requested in riid. 
		''' This will typically be IShellView or IShellView2. </param>
		''' <returns></returns>
		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function GetCurrentView(ByRef riid As Guid, ByRef ppv As IntPtr) As HResult
	End Interface

	<ComImport, Guid(ExplorerBrowserIIDGuid.IServiceProvider), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
	Friend Interface IServiceProvider
		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall)> _
		Function QueryService(ByRef guidService As Guid, ByRef riid As Guid, ByRef ppvObject As IntPtr) As HResult
	End Interface

	<ComImport, Guid(ExplorerBrowserIIDGuid.IFolderView), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
	Friend Interface IFolderView
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub GetCurrentViewMode(<Out> ByRef pViewMode As UInteger)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub SetCurrentViewMode(ViewMode As UInteger)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub GetFolder(ByRef riid As Guid, <MarshalAs(UnmanagedType.IUnknown)> ByRef ppv As Object)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub Item(iItemIndex As Integer, ByRef ppidl As IntPtr)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub ItemCount(uFlags As UInteger, ByRef pcItems As Integer)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub Items(uFlags As UInteger, ByRef riid As Guid, <Out, MarshalAs(UnmanagedType.IUnknown)> ByRef ppv As Object)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub GetSelectionMarkedItem(ByRef piItem As Integer)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub GetFocusedItem(ByRef piItem As Integer)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub GetItemPosition(pidl As IntPtr, ByRef ppt As NativePoint)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub GetSpacing(<Out> ByRef ppt As NativePoint)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub GetDefaultSpacing(ByRef ppt As NativePoint)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub GetAutoArrange()

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub SelectItem(iItem As Integer, dwFlags As UInteger)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub SelectAndPositionItems(cidl As UInteger, apidl As IntPtr, ByRef apt As NativePoint, dwFlags As UInteger)
	End Interface

	<ComImport, Guid(ExplorerBrowserIIDGuid.IFolderView2), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
	Friend Interface IFolderView2
		Inherits IFolderView
        ' IFolderView
        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Function GetCurrentViewMode(ByRef pViewMode As UInteger) As HResult

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub SetCurrentViewMode(ViewMode As UInteger)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub GetFolder(ByRef riid As Guid, <MarshalAs(UnmanagedType.IUnknown)> ByRef ppv As Object)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub Item(iItemIndex As Integer, ByRef ppidl As IntPtr)

        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Function ItemCount(uFlags As UInteger, ByRef pcItems As Integer) As HResult

        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Function Items(uFlags As UInteger, ByRef riid As Guid, <Out, MarshalAs(UnmanagedType.IUnknown)> ByRef ppv As Object) As HResult

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub GetSelectionMarkedItem(ByRef piItem As Integer)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub GetFocusedItem(ByRef piItem As Integer)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub GetItemPosition(pidl As IntPtr, ByRef ppt As NativePoint)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub GetSpacing(<Out> ByRef ppt As NativePoint)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub GetDefaultSpacing(ByRef ppt As NativePoint)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub GetAutoArrange()

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub SelectItem(iItem As Integer, dwFlags As UInteger)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub SelectAndPositionItems(cidl As UInteger, apidl As IntPtr, ByRef apt As NativePoint, dwFlags As UInteger)

        ' IFolderView2
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub SetGroupBy(key As IntPtr, fAscending As Boolean)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub GetGroupBy(ByRef pkey As IntPtr, ByRef pfAscending As Boolean)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub SetViewProperty(pidl As IntPtr, propkey As IntPtr, propvar As Object)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub GetViewProperty(pidl As IntPtr, propkey As IntPtr, ByRef ppropvar As Object)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub SetTileViewProperties(pidl As IntPtr, <MarshalAs(UnmanagedType.LPWStr)> pszPropList As String)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub SetExtendedTileViewProperties(pidl As IntPtr, <MarshalAs(UnmanagedType.LPWStr)> pszPropList As String)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub SetText(iType As Integer, <MarshalAs(UnmanagedType.LPWStr)> pwszText As String)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub SetCurrentFolderFlags(dwMask As UInteger, dwFlags As UInteger)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub GetCurrentFolderFlags(ByRef pdwFlags As UInteger)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub GetSortColumnCount(ByRef pcColumns As Integer)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub SetSortColumns(rgSortColumns As IntPtr, cColumns As Integer)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub GetSortColumns(ByRef rgSortColumns As IntPtr, cColumns As Integer)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub GetItem(iItem As Integer, ByRef riid As Guid, <MarshalAs(UnmanagedType.IUnknown)> ByRef ppv As Object)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub GetVisibleItem(iStart As Integer, fPrevious As Boolean, ByRef piItem As Integer)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub GetSelectedItem(iStart As Integer, ByRef piItem As Integer)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub GetSelection(fNoneImpliesFolder As Boolean, ByRef ppsia As IShellItemArray)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub GetSelectionState(pidl As IntPtr, ByRef pdwFlags As UInteger)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub InvokeVerbOnSelection(<[In], MarshalAs(UnmanagedType.LPWStr)> pszVerb As String)

		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function SetViewModeAndIconSize(uViewMode As Integer, iImageSize As Integer) As HResult

		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function GetViewModeAndIconSize(ByRef puViewMode As Integer, ByRef piImageSize As Integer) As HResult

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub SetGroupSubsetCount(cVisibleRows As UInteger)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub GetGroupSubsetCount(ByRef pcVisibleRows As UInteger)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub SetRedraw(fRedrawOn As Boolean)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub IsMoveInSameFolder()

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub DoRename()
	End Interface

	<ComImport, Guid(ExplorerBrowserIIDGuid.IExplorerPaneVisibility), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
	Friend Interface IExplorerPaneVisibility
		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function GetPaneState(ByRef explorerPane As Guid, ByRef peps As ExplorerPaneState) As HResult
	End Interface

	<ComImport, Guid(ExplorerBrowserIIDGuid.IExplorerBrowserEvents), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
	Friend Interface IExplorerBrowserEvents
		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function OnNavigationPending(pidlFolder As IntPtr) As HResult

		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function OnViewCreated(<MarshalAs(UnmanagedType.IUnknown)> psv As Object) As HResult

		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function OnNavigationComplete(pidlFolder As IntPtr) As HResult

		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function OnNavigationFailed(pidlFolder As IntPtr) As HResult
	End Interface

	#Region "Unused - Keeping for debugging bug #885228"

	'[ComImport,
	' Guid(ExplorerBrowserIIDGuid.ICommDlgBrowser),
	' InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	'internal interface ICommDlgBrowser
	'{
	'    [PreserveSig]
	'    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	'    HResult OnDefaultCommand(IntPtr ppshv);

	'    [PreserveSig]
	'    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	'    HResult OnStateChange(
	'        IntPtr ppshv,
	'        CommDlgBrowserStateChange uChange);

	'    [PreserveSig]
	'    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	'    HResult IncludeObject(
	'        IntPtr ppshv,
	'        IntPtr pidl);
	'}

	'[ComImport,
	' Guid(ExplorerBrowserIIDGuid.ICommDlgBrowser2),
	' InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	'internal interface ICommDlgBrowser2
	'{
	'    // dlg

	'    [PreserveSig]
	'    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	'    HResult OnDefaultCommand(IntPtr ppshv);

	'    [PreserveSig]
	'    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	'    HResult OnStateChange(
	'        IntPtr ppshv,
	'        CommDlgBrowserStateChange uChange);

	'    [PreserveSig]
	'    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	'    HResult IncludeObject(
	'        IntPtr ppshv,
	'        IntPtr pidl);

	'    // dlg2

	'    [PreserveSig]
	'    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	'    HResult GetDefaultMenuText(
	'        [In] IShellView shellView,
	'        StringBuilder buffer, //A pointer to a buffer that is used by the Shell browser to return the default shortcut menu text.
	'        [In] int bufferMaxLength); //should be max size = 260?

	'    [PreserveSig]
	'    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	'    HResult GetViewFlags(CommDlgBrowser2ViewFlags pdwFlags);


	'    [PreserveSig]
	'    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	'    HResult Notify(
	'        IntPtr pshv,
	'        CommDlgBrowserNotifyType notifyType);
	'}

	#End Region

	<ComImport, Guid(ExplorerBrowserIIDGuid.ICommDlgBrowser3), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
	Friend Interface ICommDlgBrowser3
		' dlg1
		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function OnDefaultCommand(ppshv As IntPtr) As HResult

		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function OnStateChange(ppshv As IntPtr, uChange As CommDlgBrowserStateChange) As HResult

		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function IncludeObject(ppshv As IntPtr, pidl As IntPtr) As HResult

		' dlg2
		'A pointer to a buffer that is used by the Shell browser to return the default shortcut menu text.
		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function GetDefaultMenuText(shellView As IShellView, buffer As IntPtr, bufferMaxLength As Integer) As HResult
		'should be max size = 260?
		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function GetViewFlags(<Out> ByRef pdwFlags As UInteger) As HResult
		' CommDlgBrowser2ViewFlags 

		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function Notify(pshv As IntPtr, notifyType As CommDlgBrowserNotifyType) As HResult

		' dlg3
		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function GetCurrentFilter(pszFileSpec As StringBuilder, cchFileSpec As Integer) As HResult

		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function OnColumnClicked(ppshv As IShellView, iColumn As Integer) As HResult

		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function OnPreViewCreated(ppshv As IShellView) As HResult
	End Interface

	<ComImport, Guid(ExplorerBrowserIIDGuid.IInputObject), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
	Friend Interface IInputObject
		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function UIActivateIO(fActivate As Boolean, ByRef pMsg As System.Windows.Forms.Message) As HResult

		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function HasFocusIO() As HResult

		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function TranslateAcceleratorIO(ByRef pMsg As System.Windows.Forms.Message) As HResult

	End Interface

	<ComImport, Guid(ExplorerBrowserIIDGuid.IShellView), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
	Friend Interface IShellView
		' IOleWindow
		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function GetWindow(ByRef phwnd As IntPtr) As HResult

		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function ContextSensitiveHelp(fEnterMode As Boolean) As HResult

		' IShellView
		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function TranslateAccelerator(pmsg As IntPtr) As HResult

		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function EnableModeless(fEnable As Boolean) As HResult

		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function UIActivate(uState As UInteger) As HResult

		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function Refresh() As HResult

		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function CreateViewWindow(<MarshalAs(UnmanagedType.IUnknown)> psvPrevious As Object, pfs As IntPtr, <MarshalAs(UnmanagedType.IUnknown)> psb As Object, prcView As IntPtr, ByRef phWnd As IntPtr) As HResult

		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function DestroyViewWindow() As HResult

		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function GetCurrentInfo(ByRef pfs As IntPtr) As HResult

		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function AddPropertySheetPages(dwReserved As UInteger, pfn As IntPtr, lparam As UInteger) As HResult

		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function SaveViewState() As HResult

		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function SelectItem(pidlItem As IntPtr, uFlags As UInteger) As HResult

		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function GetItemObject(uItem As ShellViewGetItemObject, ByRef riid As Guid, <MarshalAs(UnmanagedType.IUnknown)> ByRef ppv As Object) As HResult
	End Interface

    '#Pragma warning restore 108

End Namespace
