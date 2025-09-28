'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Runtime.InteropServices
Imports System.Text

Namespace Shell
	Friend NotInheritable Class ShellNativeMethods
		Private Sub New()
		End Sub
		#Region "Shell Enums"

		<Flags> _
		Friend Enum FileOpenOptions
			OverwritePrompt = &H2
			StrictFileTypes = &H4
			NoChangeDirectory = &H8
			PickFolders = &H20
			' Ensure that items returned are filesystem items.
			ForceFilesystem = &H40
			' Allow choosing items that have no storage.
			AllNonStorageItems = &H80
			NoValidate = &H100
			AllowMultiSelect = &H200
			PathMustExist = &H800
			FileMustExist = &H1000
			CreatePrompt = &H2000
			ShareAware = &H4000
			NoReadOnlyReturn = &H8000
			NoTestFileCreate = &H10000
			HideMruPlaces = &H20000
			HidePinnedPlaces = &H40000
			NoDereferenceLinks = &H100000
			DontAddToRecent = &H2000000
			ForceShowHidden = &H10000000
			DefaultNoMiniMode = &H20000000
		End Enum
		Friend Enum ControlState
			Inactive = &H0
			Enable = &H1
			Visible = &H2
		End Enum
		Friend Enum ShellItemDesignNameOptions
			Normal = &H0
            ' SIGDN_NORMAL
            ParentRelativeParsing = &H80018001UI
            ' SIGDN_INFOLDER | SIGDN_FORPARSING
            DesktopAbsoluteParsing = &H80028000UI
            ' SIGDN_FORPARSING
            ParentRelativeEditing = &H80031001UI
            ' SIGDN_INFOLDER | SIGDN_FOREDITING
            DesktopAbsoluteEditing = &H8004C000UI
            ' SIGDN_FORPARSING | SIGDN_FORADDRESSBAR
            FileSystemPath = &H80058000UI
            ' SIGDN_FORPARSING
            Url = &H80068000UI
            ' SIGDN_FORPARSIN
            ParentRelativeForAddressBar = &H8007C001UI
            ' SIGDN_INFOLDER | SIGDN_FORPARSING | SIGDN_FORADDRESSBAR
            ParentRelative = &H80080001UI
            ' SIGDN_INFOLDER
        End Enum

		''' <summary>
		''' Indicate flags that modify the property store object retrieved by methods 
		''' that create a property store, such as IShellItem2::GetPropertyStore or 
		''' IPropertyStoreFactory::GetPropertyStore.
		''' </summary>
		<Flags> _
		Friend Enum GetPropertyStoreOptions
			''' <summary>
			''' Meaning to a calling process: Return a read-only property store that contains all 
			''' properties. Slow items (offline files) are not opened. 
			''' Combination with other flags: Can be overridden by other flags.
			''' </summary>
			[Default] = 0

			''' <summary>
			''' Meaning to a calling process: Include only properties directly from the property
			''' handler, which opens the file on the disk, network, or device. Meaning to a file 
			''' folder: Only include properties directly from the handler.
			''' 
			''' Meaning to other folders: When delegating to a file folder, pass this flag on 
			''' to the file folder; do not do any multiplexing (MUX). When not delegating to a 
			''' file folder, ignore this flag instead of returning a failure code.
			''' 
			''' Combination with other flags: Cannot be combined with GPS_TEMPORARY, 
			''' GPS_FASTPROPERTIESONLY, or GPS_BESTEFFORT.
			''' </summary>
			HandlePropertiesOnly = &H1

			''' <summary>
			''' Meaning to a calling process: Can write properties to the item. 
			''' Note: The store may contain fewer properties than a read-only store. 
			''' 
			''' Meaning to a file folder: ReadWrite.
			''' 
			''' Meaning to other folders: ReadWrite. Note: When using default MUX, 
			''' return a single unmultiplexed store because the default MUX does not support ReadWrite.
			''' 
			''' Combination with other flags: Cannot be combined with GPS_TEMPORARY, GPS_FASTPROPERTIESONLY, 
			''' GPS_BESTEFFORT, or GPS_DELAYCREATION. Implies GPS_HANDLERPROPERTIESONLY.
			''' </summary>
			ReadWrite = &H2

			''' <summary>
			''' Meaning to a calling process: Provides a writable store, with no initial properties, 
			''' that exists for the lifetime of the Shell item instance; basically, a property bag 
			''' attached to the item instance. 
			''' 
			''' Meaning to a file folder: Not applicable. Handled by the Shell item.
			''' 
			''' Meaning to other folders: Not applicable. Handled by the Shell item.
			''' 
			''' Combination with other flags: Cannot be combined with any other flag. Implies GPS_READWRITE
			''' </summary>
			Temporary = &H4

			''' <summary>
			''' Meaning to a calling process: Provides a store that does not involve reading from the 
			''' disk or network. Note: Some values may be different, or missing, compared to a store 
			''' without this flag. 
			''' 
			''' Meaning to a file folder: Include the "innate" and "fallback" stores only. Do not load the handler.
			''' 
			''' Meaning to other folders: Include only properties that are available in memory or can 
			''' be computed very quickly (no properties from disk, network, or peripheral IO devices). 
			''' This is normally only data sources from the IDLIST. When delegating to other folders, pass this flag on to them.
			''' 
			''' Combination with other flags: Cannot be combined with GPS_TEMPORARY, GPS_READWRITE, 
			''' GPS_HANDLERPROPERTIESONLY, or GPS_DELAYCREATION.
			''' </summary>
			FastPropertiesOnly = &H8

			''' <summary>
			''' Meaning to a calling process: Open a slow item (offline file) if necessary. 
			''' Meaning to a file folder: Retrieve a file from offline storage, if necessary. 
			''' Note: Without this flag, the handler is not created for offline files.
			''' 
			''' Meaning to other folders: Do not return any properties that are very slow.
			''' 
			''' Combination with other flags: Cannot be combined with GPS_TEMPORARY or GPS_FASTPROPERTIESONLY.
			''' </summary>
			OpensLowItem = &H10

			''' <summary>
			''' Meaning to a calling process: Delay memory-intensive operations, such as file access, until 
			''' a property is requested that requires such access. 
			''' 
			''' Meaning to a file folder: Do not create the handler until needed; for example, either 
			''' GetCount/GetAt or GetValue, where the innate store does not satisfy the request. 
			''' Note: GetValue might fail due to file access problems.
			''' 
			''' Meaning to other folders: If the folder has memory-intensive properties, such as 
			''' delegating to a file folder or network access, it can optimize performance by 
			''' supporting IDelayedPropertyStoreFactory and splitting up its properties into a 
			''' fast and a slow store. It can then use delayed MUX to recombine them.
			''' 
			''' Combination with other flags: Cannot be combined with GPS_TEMPORARY or 
			''' GPS_READWRITE
			''' </summary>
			DelayCreation = &H20

			''' <summary>
			''' Meaning to a calling process: Succeed at getting the store, even if some 
			''' properties are not returned. Note: Some values may be different, or missing,
			''' compared to a store without this flag. 
			''' 
			''' Meaning to a file folder: Succeed and return a store, even if the handler or 
			''' innate store has an error during creation. Only fail if substores fail.
			''' 
			''' Meaning to other folders: Succeed on getting the store, even if some properties 
			''' are not returned.
			''' 
			''' Combination with other flags: Cannot be combined with GPS_TEMPORARY, 
			''' GPS_READWRITE, or GPS_HANDLERPROPERTIESONLY.
			''' </summary>
			BestEffort = &H40

			''' <summary>
			''' Mask for valid GETPROPERTYSTOREFLAGS values.
			''' </summary>
			MaskValid = &Hff
		End Enum

		Friend Enum ShellItemAttributeOptions
			' if multiple items and the attirbutes together.
			[And] = &H1
			' if multiple items or the attributes together.
			[Or] = &H2
			' Call GetAttributes directly on the 
			' ShellFolder for multiple attributes.
			AppCompat = &H3

			' A mask for SIATTRIBFLAGS_AND, SIATTRIBFLAGS_OR, and SIATTRIBFLAGS_APPCOMPAT. Callers normally do not use this value.
			Mask = &H3

			' Windows 7 and later. Examine all items in the array to compute the attributes. 
			' Note that this can result in poor performance over large arrays and therefore it 
			' should be used only when needed. Cases in which you pass this flag should be extremely rare.
			AllItems = &H4000
		End Enum

		Friend Enum FileDialogEventShareViolationResponse
			[Default] = &H0
			Accept = &H1
			Refuse = &H2
		End Enum
		Friend Enum FileDialogEventOverwriteResponse
			[Default] = &H0
			Accept = &H1
			Refuse = &H2
		End Enum
		Friend Enum FileDialogAddPlacement
			Bottom = &H0
			Top = &H1
		End Enum

		<Flags> _
		Friend Enum SIIGBF
			ResizeToFit = &H0
			BiggerSizeOk = &H1
			MemoryOnly = &H2
			IconOnly = &H4
			ThumbnailOnly = &H8
			InCacheOnly = &H10
		End Enum

		<Flags> _
		Friend Enum ThumbnailOptions
			Extract = &H0
			InCacheOnly = &H1
			FastExtract = &H2
			ForceExtraction = &H4
			SlowReclaim = &H8
			ExtractDoNotCache = &H20
		End Enum

		<Flags> _
		Friend Enum ThumbnailCacheOptions
			[Default] = &H0
			LowQuality = &H1
			Cached = &H2
		End Enum

		<Flags> _
		Friend Enum ShellFileGetAttributesOptions
			''' <summary>
			''' The specified items can be copied.
			''' </summary>
			CanCopy = &H1

			''' <summary>
			''' The specified items can be moved.
			''' </summary>
			CanMove = &H2

			''' <summary>
			''' Shortcuts can be created for the specified items. This flag has the same value as DROPEFFECT. 
			''' The normal use of this flag is to add a Create Shortcut item to the shortcut menu that is displayed 
			''' during drag-and-drop operations. However, SFGAO_CANLINK also adds a Create Shortcut item to the Microsoft 
			''' Windows Explorer's File menu and to normal shortcut menus. 
			''' If this item is selected, your application's IContextMenu::InvokeCommand is invoked with the lpVerb 
			''' member of the CMINVOKECOMMANDINFO structure set to "link." Your application is responsible for creating the link.
			''' </summary>
			CanLink = &H4

			''' <summary>
			''' The specified items can be bound to an IStorage interface through IShellFolder::BindToObject.
			''' </summary>
			Storage = &H8

			''' <summary>
			''' The specified items can be renamed.
			''' </summary>
			CanRename = &H10

			''' <summary>
			''' The specified items can be deleted.
			''' </summary>
			CanDelete = &H20

			''' <summary>
			''' The specified items have property sheets.
			''' </summary>
			HasPropertySheet = &H40

			''' <summary>
			''' The specified items are drop targets.
			''' </summary>
			DropTarget = &H100

			''' <summary>
			''' This flag is a mask for the capability flags.
			''' </summary>
			CapabilityMask = &H177

			''' <summary>
			''' Windows 7 and later. The specified items are system items.
			''' </summary>
			System = &H1000

			''' <summary>
			''' The specified items are encrypted.
			''' </summary>
			Encrypted = &H2000

			''' <summary>
			''' Indicates that accessing the object = through IStream or other storage interfaces, 
			''' is a slow operation. 
			''' Applications should avoid accessing items flagged with SFGAO_ISSLOW.
			''' </summary>
			IsSlow = &H4000

			''' <summary>
			''' The specified items are ghosted icons.
			''' </summary>
			Ghosted = &H8000

			''' <summary>
			''' The specified items are shortcuts.
			''' </summary>
			Link = &H10000

			''' <summary>
			''' The specified folder objects are shared.
			''' </summary>    
			Share = &H20000

			''' <summary>
			''' The specified items are read-only. In the case of folders, this means 
			''' that new items cannot be created in those folders.
			''' </summary>
			[ReadOnly] = &H40000

			''' <summary>
			''' The item is hidden and should not be displayed unless the 
			''' Show hidden files and folders option is enabled in Folder Settings.
			''' </summary>
			Hidden = &H80000

			''' <summary>
			''' This flag is a mask for the display attributes.
			''' </summary>
			DisplayAttributeMask = &Hfc000

			''' <summary>
			''' The specified folders contain one or more file system folders.
			''' </summary>
			FileSystemAncestor = &H10000000

			''' <summary>
			''' The specified items are folders.
			''' </summary>
			Folder = &H20000000

			''' <summary>
			''' The specified folders or file objects are part of the file system 
			''' that is, they are files, directories, or root directories).
			''' </summary>
			FileSystem = &H40000000

            ''' <summary>
            ''' The specified folders have subfolders = and are, therefore, 
            ''' expandable in the left pane of Windows Explorer).
            ''' </summary>
            HasSubFolder = &H80000000UI

            ''' <summary>
            ''' This flag is a mask for the contents attributes.
            ''' </summary>
            ContentsMask = &H80000000UI

            ''' <summary>
            ''' When specified as input, SFGAO_VALIDATE instructs the folder to validate that the items 
            ''' pointed to by the contents of apidl exist. If one or more of those items do not exist, 
            ''' IShellFolder::GetAttributesOf returns a failure code. 
            ''' When used with the file system folder, SFGAO_VALIDATE instructs the folder to discard cached 
            ''' properties retrieved by clients of IShellFolder2::GetDetailsEx that may 
            ''' have accumulated for the specified items.
            ''' </summary>
            Validate = &H1000000

			''' <summary>
			''' The specified items are on removable media or are themselves removable devices.
			''' </summary>
			Removable = &H2000000

			''' <summary>
			''' The specified items are compressed.
			''' </summary>
			Compressed = &H4000000

			''' <summary>
			''' The specified items can be browsed in place.
			''' </summary>
			Browsable = &H8000000

			''' <summary>
			''' The items are nonenumerated items.
			''' </summary>
			Nonenumerated = &H100000

			''' <summary>
			''' The objects contain new content.
			''' </summary>
			NewContent = &H200000

			''' <summary>
			''' It is possible to create monikers for the specified file objects or folders.
			''' </summary>
			CanMoniker = &H400000

			''' <summary>
			''' Not supported.
			''' </summary>
			HasStorage = &H400000

			''' <summary>
			''' Indicates that the item has a stream associated with it that can be accessed 
			''' by a call to IShellFolder::BindToObject with IID_IStream in the riid parameter.
			''' </summary>
			Stream = &H400000

			''' <summary>
			''' Children of this item are accessible through IStream or IStorage. 
			''' Those children are flagged with SFGAO_STORAGE or SFGAO_STREAM.
			''' </summary>
			StorageAncestor = &H800000

			''' <summary>
			''' This flag is a mask for the storage capability attributes.
			''' </summary>
			StorageCapabilityMask = &H70c50008

            ''' <summary>
            ''' Mask used by PKEY_SFGAOFlags to remove certain values that are considered 
            ''' to cause slow calculations or lack context. 
            ''' Equal to SFGAO_VALIDATE | SFGAO_ISSLOW | SFGAO_HASSUBFOLDER.
            ''' </summary>
            PkeyMask = &H81044000UI
        End Enum

		<Flags> _
		Friend Enum ShellFolderEnumerationOptions As UShort
			CheckingForChildren = &H10
			Folders = &H20
			NonFolders = &H40
			IncludeHidden = &H80
			InitializeOnFirstNext = &H100
			NetPrinterSearch = &H200
			Shareable = &H400
			Storage = &H800
			NavigationEnum = &H1000
			FastItems = &H2000
			FlatList = &H4000
			EnableAsync = &H8000
		End Enum

		#End Region

		#Region "Shell Structs"

		<StructLayout(LayoutKind.Sequential, CharSet := CharSet.Auto)> _
		Friend Structure FilterSpec
			<MarshalAs(UnmanagedType.LPWStr)> _
			Friend Name As String
			<MarshalAs(UnmanagedType.LPWStr)> _
			Friend Spec As String

			Friend Sub New(name__1 As String, spec__2 As String)
				Name = name__1
				Spec = spec__2
			End Sub
		End Structure

		<StructLayout(LayoutKind.Sequential, CharSet := CharSet.Auto)> _
		Friend Structure ThumbnailId
			<MarshalAs(UnmanagedType.LPArray, SizeParamIndex := 16)> _
			Private rgbKey As Byte
		End Structure

		#End Region

		#Region "Shell Helper Methods"

		<DllImport("shell32.dll", CharSet := CharSet.Unicode, SetLastError := True)> _
		Friend Shared Function SHCreateShellItemArrayFromDataObject(pdo As System.Runtime.InteropServices.ComTypes.IDataObject, ByRef riid As Guid, <MarshalAs(UnmanagedType.[Interface])> ByRef iShellItemArray As IShellItemArray) As Integer
		End Function

		' The following parameter is not used - binding context.
		<DllImport("shell32.dll", CharSet := CharSet.Unicode, SetLastError := True)> _
		Friend Shared Function SHCreateItemFromParsingName(<MarshalAs(UnmanagedType.LPWStr)> path As String, pbc As IntPtr, ByRef riid As Guid, <MarshalAs(UnmanagedType.[Interface])> ByRef shellItem As IShellItem2) As Integer
		End Function

		' The following parameter is not used - binding context.
		<DllImport("shell32.dll", CharSet := CharSet.Unicode, SetLastError := True)> _
		Friend Shared Function SHCreateItemFromParsingName(<MarshalAs(UnmanagedType.LPWStr)> path As String, pbc As IntPtr, ByRef riid As Guid, <MarshalAs(UnmanagedType.[Interface])> ByRef shellItem As IShellItem) As Integer
		End Function

		<DllImport("shlwapi.dll", CharSet := CharSet.Unicode, SetLastError := True)> _
		Friend Shared Function PathParseIconLocation(<MarshalAs(UnmanagedType.LPWStr)> ByRef pszIconFile As String) As Integer
		End Function


		'PCIDLIST_ABSOLUTE
		<DllImport("shell32.dll", CharSet := CharSet.Unicode, SetLastError := True)> _
		Friend Shared Function SHCreateItemFromIDList(pidl As IntPtr, ByRef riid As Guid, <MarshalAs(UnmanagedType.[Interface])> ByRef ppv As IShellItem2) As Integer
		End Function

		<DllImport("shell32.dll", CharSet := CharSet.Unicode, SetLastError := True)> _
		Friend Shared Function SHParseDisplayName(<MarshalAs(UnmanagedType.LPWStr)> pszName As String, pbc As IntPtr, ByRef ppidl As IntPtr, sfgaoIn As ShellFileGetAttributesOptions, ByRef psfgaoOut As ShellFileGetAttributesOptions) As Integer
		End Function

		<DllImport("shell32.dll", CharSet := CharSet.Unicode, SetLastError := True)> _
		Friend Shared Function SHGetIDListFromObject(iUnknown As IntPtr, ByRef ppidl As IntPtr) As Integer
		End Function

		<DllImport("shell32.dll", CharSet := CharSet.Unicode, SetLastError := True)> _
		Friend Shared Function SHGetDesktopFolder(<MarshalAs(UnmanagedType.[Interface])> ByRef ppshf As IShellFolder) As Integer
		End Function

		<DllImport("shell32.dll", CharSet := CharSet.Unicode, SetLastError := True)> _
		Friend Shared Function SHCreateShellItem(pidlParent As IntPtr, <[In], MarshalAs(UnmanagedType.[Interface])> psfParent As IShellFolder, pidl As IntPtr, <MarshalAs(UnmanagedType.[Interface])> ByRef ppsi As IShellItem) As Integer
		End Function

		<DllImport("shell32.dll", CharSet := CharSet.Unicode, SetLastError := True)> _
		Friend Shared Function ILGetSize(pidl As IntPtr) As UInteger
		End Function

		<DllImport("shell32.dll", CharSet := CharSet.None)> _
		Public Shared Sub ILFree(pidl As IntPtr)
		End Sub

		<DllImport("gdi32.dll")> _
		Friend Shared Function DeleteObject(hObject As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
		End Function

		#End Region

		#Region "Shell Library Enums"

		Friend Enum LibraryFolderFilter
			ForceFileSystem = 1
			StorageItems = 2
			AllItems = 3
		End Enum

		<Flags> _
		Friend Enum LibraryOptions
			[Default] = 0
			PinnedToNavigationPane = &H1
			MaskAll = &H1
		End Enum

		Friend Enum DefaultSaveFolderType
			Detect = 1
			[Private] = 2
			[Public] = 3
		End Enum

		Friend Enum LibrarySaveOptions
			FailIfThere = 0
			OverrideExisting = 1
			MakeUniqueName = 2
		End Enum

		Friend Enum LibraryManageDialogOptions
			[Default] = 0
			NonIndexableLocationWarning = 1
		End Enum


		#End Region

		#Region "Shell Library Helper Methods"

		<DllImport("Shell32", CharSet := CharSet.Unicode, CallingConvention := CallingConvention.Winapi, SetLastError := True)> _
		Friend Shared Function SHShowManageLibraryUI(<[In], MarshalAs(UnmanagedType.[Interface])> library As IShellItem, <[In]> hwndOwner As IntPtr, <[In]> title As String, <[In]> instruction As String, <[In]> lmdOptions As LibraryManageDialogOptions) As Integer
		End Function

		#End Region

		#Region "Command Link Definitions"

		Friend Const CommandLink As Integer = &He
		Friend Const SetNote As UInteger = &H1609
		Friend Const GetNote As UInteger = &H160a
		Friend Const GetNoteLength As UInteger = &H160b
		Friend Const SetShield As UInteger = &H160c

		#End Region

		#Region "Shell notification definitions"
		Friend Const MaxPath As Integer = 260

		<DllImport("shell32.dll")> _
		Friend Shared Function SHGetPathFromIDListW(pidl As IntPtr, <MarshalAs(UnmanagedType.LPWStr)> pszPath As StringBuilder) As <MarshalAs(UnmanagedType.Bool)> Boolean
		End Function

		<StructLayout(LayoutKind.Sequential)> _
		Friend Structure ShellNotifyStruct
			Friend item1 As IntPtr
			Friend item2 As IntPtr
		End Structure

		<StructLayout(LayoutKind.Sequential)> _
		Friend Structure SHChangeNotifyEntry
			Friend pIdl As IntPtr

			<MarshalAs(UnmanagedType.Bool)> _
			Friend recursively As Boolean
		End Structure

		<DllImport("shell32.dll")> _
		Friend Shared Function SHChangeNotifyRegister(windowHandle As IntPtr, sources As ShellChangeNotifyEventSource, events As ShellObjectChangeTypes, message As UInteger, entries As Integer, ByRef changeNotifyEntry As SHChangeNotifyEntry) As UInteger
		End Function

		<DllImport("shell32.dll")> _
		Friend Shared Function SHChangeNotification_Lock(windowHandle As IntPtr, processId As Integer, ByRef pidl As IntPtr, ByRef lEvent As UInteger) As IntPtr
		End Function

		<DllImport("shell32.dll")> _
		Friend Shared Function SHChangeNotification_Unlock(hLock As IntPtr) As <MarshalAs(UnmanagedType.Bool)> [Boolean]
		End Function

		<DllImport("shell32.dll")> _
		Friend Shared Function SHChangeNotifyDeregister(hNotify As UInteger) As <MarshalAs(UnmanagedType.Bool)> [Boolean]
		End Function

		<Flags> _
		Friend Enum ShellChangeNotifyEventSource
			InterruptLevel = &H1
			ShellLevel = &H2
			RecursiveInterrupt = &H1000
			NewDelivery = &H8000
		End Enum



		#End Region

		Friend Const InPlaceStringTruncated As Integer = &H401a0
	End Class
End Namespace
