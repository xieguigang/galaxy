'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.IO
Imports System.Linq
Imports System.Runtime.InteropServices
Imports System.Threading
Imports Microsoft.Windows.Resources
Imports Microsoft.Windows.Internal

Namespace Shell
	''' <summary>
	''' A Shell Library in the Shell Namespace
	''' </summary>
	Public NotInheritable Class ShellLibrary
		Inherits ShellContainer
		Implements IList(Of ShellFileSystemFolder)
		#Region "Private Fields"

		Private nativeShellLibrary As INativeShellLibrary
		Private knownFolder As IKnownFolder

		Private Shared FolderTypesGuids As Guid() = {New Guid(ShellKFIDGuid.GenericLibrary), New Guid(ShellKFIDGuid.DocumentsLibrary), New Guid(ShellKFIDGuid.MusicLibrary), New Guid(ShellKFIDGuid.PicturesLibrary), New Guid(ShellKFIDGuid.VideosLibrary)}

		#End Region

		#Region "Private Constructor"

		Private Sub New()
			CoreHelpers.ThrowIfNotWin7()
		End Sub

		'Construct the ShellLibrary object from a native Shell Library
		Private Sub New(nativeShellLibrary As INativeShellLibrary)
			Me.New()
			Me.nativeShellLibrary = nativeShellLibrary
		End Sub

		''' <summary>
		''' Creates a shell library in the Libraries Known Folder, 
		''' using the given IKnownFolder
		''' </summary>
		''' <param name="sourceKnownFolder">KnownFolder from which to create the new Shell Library</param>
		''' <param name="isReadOnly">If <B>true</B> , opens the library in read-only mode.</param>
		Private Sub New(sourceKnownFolder As IKnownFolder, isReadOnly As Boolean)
			Me.New()
			System.Diagnostics.Debug.Assert(sourceKnownFolder IsNot Nothing)

			' Keep a reference locally
			knownFolder = sourceKnownFolder

			nativeShellLibrary = DirectCast(New ShellLibraryCoClass(), INativeShellLibrary)

			Dim flags As AccessModes = If(isReadOnly, AccessModes.Read, AccessModes.ReadWrite)

            ' Get the IShellItem2
            MyBase.m_nativeShellItem = DirectCast(sourceKnownFolder, ShellObject).NativeShellItem2

            Dim guid As Guid = sourceKnownFolder.FolderId

			' Load the library from the IShellItem2
			Try
				nativeShellLibrary.LoadLibraryFromKnownFolder(guid, flags)
			Catch generatedExceptionName As InvalidCastException
				Throw New ArgumentException(LocalizedMessages.ShellLibraryInvalidLibrary, "sourceKnownFolder")
			Catch generatedExceptionName As NotImplementedException
				Throw New ArgumentException(LocalizedMessages.ShellLibraryInvalidLibrary, "sourceKnownFolder")
			End Try
		End Sub

		#End Region

		#Region "Public Constructors"

		''' <summary>
		''' Creates a shell library in the Libraries Known Folder, 
		''' using the given shell library name.
		''' </summary>
		''' <param name="libraryName">The name of this library</param>
		''' <param name="overwrite">Allow overwriting an existing library; if one exists with the same name</param>
		Public Sub New(libraryName As String, overwrite As Boolean)
			Me.New()
			If String.IsNullOrEmpty(libraryName) Then
				Throw New ArgumentException(LocalizedMessages.ShellLibraryEmptyName, "libraryName")
			End If

			Me.Name = libraryName
			Dim guid As New Guid(ShellKFIDGuid.Libraries)

			Dim flags As ShellNativeMethods.LibrarySaveOptions = If(overwrite, ShellNativeMethods.LibrarySaveOptions.OverrideExisting, ShellNativeMethods.LibrarySaveOptions.FailIfThere)

			nativeShellLibrary = DirectCast(New ShellLibraryCoClass(), INativeShellLibrary)
            nativeShellLibrary.SaveInKnownFolder(guid, libraryName, flags, DirectCast(NativeShellItem, IShellItem2))
        End Sub

		''' <summary>
		''' Creates a shell library in a given Known Folder, 
		''' using the given shell library name.
		''' </summary>
		''' <param name="libraryName">The name of this library</param>
		''' <param name="sourceKnownFolder">The known folder</param>
		''' <param name="overwrite">Override an existing library with the same name</param>
		Public Sub New(libraryName As String, sourceKnownFolder As IKnownFolder, overwrite As Boolean)
			Me.New()
			If String.IsNullOrEmpty(libraryName) Then
				Throw New ArgumentException(LocalizedMessages.ShellLibraryEmptyName, "libraryName")
			End If

			knownFolder = sourceKnownFolder

			Me.Name = libraryName
			Dim guid As Guid = knownFolder.FolderId

			Dim flags As ShellNativeMethods.LibrarySaveOptions = If(overwrite, ShellNativeMethods.LibrarySaveOptions.OverrideExisting, ShellNativeMethods.LibrarySaveOptions.FailIfThere)

			nativeShellLibrary = DirectCast(New ShellLibraryCoClass(), INativeShellLibrary)
            nativeShellLibrary.SaveInKnownFolder(guid, libraryName, flags, DirectCast(NativeShellItem, IShellItem2))
        End Sub

		''' <summary>
		''' Creates a shell library in a given local folder, 
		''' using the given shell library name.
		''' </summary>
		''' <param name="libraryName">The name of this library</param>
		''' <param name="folderPath">The path to the local folder</param>
		''' <param name="overwrite">Override an existing library with the same name</param>
		Public Sub New(libraryName As String, folderPath As String, overwrite As Boolean)
			Me.New()
			If String.IsNullOrEmpty(libraryName) Then
				Throw New ArgumentException(LocalizedMessages.ShellLibraryEmptyName, "libraryName")
			End If

			If Not Directory.Exists(folderPath) Then
				Throw New DirectoryNotFoundException(LocalizedMessages.ShellLibraryFolderNotFound)
			End If

			Me.Name = libraryName

			Dim flags As ShellNativeMethods.LibrarySaveOptions = If(overwrite, ShellNativeMethods.LibrarySaveOptions.OverrideExisting, ShellNativeMethods.LibrarySaveOptions.FailIfThere)

			Dim guid As New Guid(ShellIIDGuid.IShellItem)

			Dim shellItemIn As IShellItem
			ShellNativeMethods.SHCreateItemFromParsingName(folderPath, IntPtr.Zero, guid, shellItemIn)

			nativeShellLibrary = DirectCast(New ShellLibraryCoClass(), INativeShellLibrary)
            nativeShellLibrary.Save(shellItemIn, libraryName, flags, DirectCast(NativeShellItem, IShellItem2))
        End Sub

#End Region

#Region "Public Properties"

        ''' <summary>
        ''' The name of the library, every library must 
        ''' have a name
        ''' </summary>
        ''' <exception cref="COMException">Will throw if no Icon is set</exception>
        Public Overrides Property Name() As String
            Get
                If MyBase.Name Is Nothing AndAlso NativeShellItem IsNot Nothing Then
                    MyBase.Name = System.IO.Path.GetFileNameWithoutExtension(ShellHelper.GetParsingName(NativeShellItem))
                End If

                Return MyBase.Name
            End Get
            Protected Set(value As String)
                MyBase.Name = value
            End Set
        End Property

        ''' <summary>
        ''' The Resource Reference to the icon.
        ''' </summary>
        Public Property IconResourceId() As IconReference
			Get
				Dim iconRef As String
				nativeShellLibrary.GetIcon(iconRef)
				Return New IconReference(iconRef)
			End Get

			Set
				nativeShellLibrary.SetIcon(value.ReferencePath)
				nativeShellLibrary.Commit()
			End Set
		End Property

		''' <summary>
		''' One of predefined Library types
		''' </summary>
		''' <exception cref="COMException">Will throw if no Library Type is set</exception>
		Public Property LibraryType() As LibraryFolderType
			Get
				Dim folderTypeGuid As Guid
				nativeShellLibrary.GetFolderType(folderTypeGuid)

				Return GetFolderTypefromGuid(folderTypeGuid)
			End Get

			Set
				Dim guid As Guid = FolderTypesGuids(CInt(value))
				nativeShellLibrary.SetFolderType(guid)
				nativeShellLibrary.Commit()
			End Set
		End Property

		''' <summary>
		''' The Guid of the Library type
		''' </summary>
		''' <exception cref="COMException">Will throw if no Library Type is set</exception>
		Public ReadOnly Property LibraryTypeId() As Guid
			Get
				Dim folderTypeGuid As Guid
				nativeShellLibrary.GetFolderType(folderTypeGuid)

				Return folderTypeGuid
			End Get
		End Property

		Private Shared Function GetFolderTypefromGuid(folderTypeGuid As Guid) As LibraryFolderType
			For i As Integer = 0 To FolderTypesGuids.Length - 1
				If folderTypeGuid.Equals(FolderTypesGuids(i)) Then
					Return CType(i, LibraryFolderType)
				End If
			Next
			Throw New ArgumentOutOfRangeException("folderTypeGuid", LocalizedMessages.ShellLibraryInvalidFolderType)
		End Function

		''' <summary>
		''' By default, this folder is the first location 
		''' added to the library. The default save folder 
		''' is both the default folder where files can 
		''' be saved, and also where the library XML 
		''' file will be saved, if no other path is specified
		''' </summary>
		Public Property DefaultSaveFolder() As String
			Get
				Dim guid As New Guid(ShellIIDGuid.IShellItem)

				Dim saveFolderItem As IShellItem

				nativeShellLibrary.GetDefaultSaveFolder(ShellNativeMethods.DefaultSaveFolderType.Detect, guid, saveFolderItem)

				Return ShellHelper.GetParsingName(saveFolderItem)
			End Get
			Set
				If String.IsNullOrEmpty(value) Then
					Throw New ArgumentNullException("value")
				End If

				If Not Directory.Exists(value) Then
					Throw New DirectoryNotFoundException(LocalizedMessages.ShellLibraryDefaultSaveFolderNotFound)
				End If

				Dim fullPath As String = New DirectoryInfo(value).FullName

				Dim guid As New Guid(ShellIIDGuid.IShellItem)
				Dim saveFolderItem As IShellItem

				ShellNativeMethods.SHCreateItemFromParsingName(fullPath, IntPtr.Zero, guid, saveFolderItem)

				nativeShellLibrary.SetDefaultSaveFolder(ShellNativeMethods.DefaultSaveFolderType.Detect, saveFolderItem)

				nativeShellLibrary.Commit()
			End Set
		End Property

		''' <summary>
		''' Whether the library will be pinned to the 
		''' Explorer Navigation Pane
		''' </summary>
		Public Property IsPinnedToNavigationPane() As Boolean
			Get
				Dim flags As ShellNativeMethods.LibraryOptions = ShellNativeMethods.LibraryOptions.PinnedToNavigationPane

				nativeShellLibrary.GetOptions(flags)

				Return ((flags And ShellNativeMethods.LibraryOptions.PinnedToNavigationPane) = ShellNativeMethods.LibraryOptions.PinnedToNavigationPane)
			End Get
			Set
				Dim flags As ShellNativeMethods.LibraryOptions = ShellNativeMethods.LibraryOptions.[Default]

				If value Then
					flags = flags Or ShellNativeMethods.LibraryOptions.PinnedToNavigationPane
				Else
					flags = flags And Not ShellNativeMethods.LibraryOptions.PinnedToNavigationPane
				End If

				nativeShellLibrary.SetOptions(ShellNativeMethods.LibraryOptions.PinnedToNavigationPane, flags)
				nativeShellLibrary.Commit()
			End Set
		End Property

		#End Region

		#Region "Public Methods"

		''' <summary>
		''' Close the library, and release its associated file system resources
		''' </summary>
		Public Sub Close()
			Me.Dispose()
		End Sub

		#End Region

		#Region "Internal Properties"

		Friend Const FileExtension As String = ".library-ms"

		Friend Overrides ReadOnly Property NativeShellItem() As IShellItem
			Get
				Return NativeShellItem2
			End Get
		End Property

		Friend Overrides ReadOnly Property NativeShellItem2() As IShellItem2
			Get
                Return DirectCast(NativeShellItem, IShellItem2)
            End Get
		End Property

		#End Region

		#Region "Static Shell Library methods"

		''' <summary>
		''' Get a the known folder FOLDERID_Libraries 
		''' </summary>
		Public Shared ReadOnly Property LibrariesKnownFolder() As IKnownFolder
			Get
				CoreHelpers.ThrowIfNotWin7()
				Return KnownFolderHelper.FromKnownFolderId(New Guid(ShellKFIDGuid.Libraries))
			End Get
		End Property

		''' <summary>
		''' Load the library using a number of options
		''' </summary>
		''' <param name="libraryName">The name of the library</param>
		''' <param name="isReadOnly">If <B>true</B>, loads the library in read-only mode.</param>
		''' <returns>A ShellLibrary Object</returns>
		Public Shared Function Load(libraryName As String, isReadOnly As Boolean) As ShellLibrary
			CoreHelpers.ThrowIfNotWin7()

			Dim kf As IKnownFolder = KnownFolders.Libraries
			Dim librariesFolderPath As String = If((kf IsNot Nothing), kf.Path, String.Empty)

			Dim guid As New Guid(ShellIIDGuid.IShellItem)
			Dim nativeShellItem As IShellItem
			Dim shellItemPath As String = System.IO.Path.Combine(librariesFolderPath, libraryName & FileExtension)
			Dim hr As Integer = ShellNativeMethods.SHCreateItemFromParsingName(shellItemPath, IntPtr.Zero, guid, nativeShellItem)

			If Not CoreErrorHelper.Succeeded(hr) Then
				Throw New ShellException(hr)
			End If

			Dim nativeShellLibrary As INativeShellLibrary = DirectCast(New ShellLibraryCoClass(), INativeShellLibrary)
			Dim flags As AccessModes = If(isReadOnly, AccessModes.Read, AccessModes.ReadWrite)
			nativeShellLibrary.LoadLibraryFromItem(nativeShellItem, flags)

			Dim library As New ShellLibrary(nativeShellLibrary)
			Try
                library.m_nativeShellItem = DirectCast(nativeShellItem, IShellItem2)
                library.Name = libraryName

				Return library
			Catch
				library.Dispose()
				Throw
			End Try
		End Function

		''' <summary>
		''' Load the library using a number of options
		''' </summary>
		''' <param name="libraryName">The name of the library.</param>
		''' <param name="folderPath">The path to the library.</param>
		''' <param name="isReadOnly">If <B>true</B>, opens the library in read-only mode.</param>
		''' <returns>A ShellLibrary Object</returns>
		Public Shared Function Load(libraryName As String, folderPath As String, isReadOnly As Boolean) As ShellLibrary
			CoreHelpers.ThrowIfNotWin7()

			' Create the shell item path
			Dim shellItemPath As String = System.IO.Path.Combine(folderPath, libraryName & FileExtension)
			Dim item As ShellFile = ShellFile.FromFilePath(shellItemPath)

			Dim nativeShellItem As IShellItem = item.NativeShellItem
			Dim nativeShellLibrary As INativeShellLibrary = DirectCast(New ShellLibraryCoClass(), INativeShellLibrary)
			Dim flags As AccessModes = If(isReadOnly, AccessModes.Read, AccessModes.ReadWrite)
			nativeShellLibrary.LoadLibraryFromItem(nativeShellItem, flags)

			Dim library As New ShellLibrary(nativeShellLibrary)
			Try
                library.m_nativeShellItem = DirectCast(nativeShellItem, IShellItem2)
                library.Name = libraryName

				Return library
			Catch
				library.Dispose()
				Throw
			End Try
		End Function

		''' <summary>
		''' Load the library using a number of options
		''' </summary>
		''' <param name="nativeShellItem">IShellItem</param>
		''' <param name="isReadOnly">read-only flag</param>
		''' <returns>A ShellLibrary Object</returns>
		Friend Shared Function FromShellItem(nativeShellItem As IShellItem, isReadOnly As Boolean) As ShellLibrary
			CoreHelpers.ThrowIfNotWin7()

			Dim nativeShellLibrary As INativeShellLibrary = DirectCast(New ShellLibraryCoClass(), INativeShellLibrary)

			Dim flags As AccessModes = If(isReadOnly, AccessModes.Read, AccessModes.ReadWrite)

			nativeShellLibrary.LoadLibraryFromItem(nativeShellItem, flags)

			Dim library As New ShellLibrary(nativeShellLibrary)
            library.m_nativeShellItem = DirectCast(nativeShellItem, IShellItem2)

            Return library
		End Function

		''' <summary>
		''' Load the library using a number of options
		''' </summary>
		''' <param name="sourceKnownFolder">A known folder.</param>
		''' <param name="isReadOnly">If <B>true</B>, opens the library in read-only mode.</param>
		''' <returns>A ShellLibrary Object</returns>
		Public Shared Function Load(sourceKnownFolder As IKnownFolder, isReadOnly As Boolean) As ShellLibrary
			CoreHelpers.ThrowIfNotWin7()
			Return New ShellLibrary(sourceKnownFolder, isReadOnly)
		End Function

		Private Shared Sub ShowManageLibraryUI(shellLibrary As ShellLibrary, windowHandle As IntPtr, title As String, instruction As String, allowAllLocations As Boolean)
			Dim hr As Integer = 0

            Dim staWorker As New Thread(
                Sub() hr = ShellNativeMethods.SHShowManageLibraryUI(
                    shellLibrary.NativeShellItem,
                    windowHandle,
                    title,
                    instruction,
                    If(allowAllLocations, ShellNativeMethods.LibraryManageDialogOptions.NonIndexableLocationWarning, ShellNativeMethods.LibraryManageDialogOptions.[Default])))

            staWorker.SetApartmentState(ApartmentState.STA)
			staWorker.Start()
			staWorker.Join()

			If Not CoreErrorHelper.Succeeded(hr) Then
				Throw New ShellException(hr)
			End If
		End Sub

		''' <summary>
		''' Shows the library management dialog which enables users to mange the library folders and default save location.
		''' </summary>
		''' <param name="libraryName">The name of the library</param>
		''' <param name="folderPath">The path to the library.</param>
		''' <param name="windowHandle">The parent window,or IntPtr.Zero for no parent</param>
		''' <param name="title">A title for the library management dialog, or null to use the library name as the title</param>
		''' <param name="instruction">An optional help string to display for the library management dialog</param>
		''' <param name="allowAllLocations">If true, do not show warning dialogs about locations that cannot be indexed</param>
		''' <remarks>If the library is already open in read-write mode, the dialog will not save the changes.</remarks>
		Public Shared Sub ShowManageLibraryUI(libraryName As String, folderPath As String, windowHandle As IntPtr, title As String, instruction As String, allowAllLocations As Boolean)
			' this method is not safe for MTA consumption and will blow
			' Access Violations if called from an MTA thread so we wrap this
			' call up into a Worker thread that performs all operations in a
			' single threaded apartment
			Using shellLibrary__1 As ShellLibrary = ShellLibrary.Load(libraryName, folderPath, True)
				ShowManageLibraryUI(shellLibrary__1, windowHandle, title, instruction, allowAllLocations)
			End Using
		End Sub

		''' <summary>
		''' Shows the library management dialog which enables users to mange the library folders and default save location.
		''' </summary>
		''' <param name="libraryName">The name of the library</param>
		''' <param name="windowHandle">The parent window,or IntPtr.Zero for no parent</param>
		''' <param name="title">A title for the library management dialog, or null to use the library name as the title</param>
		''' <param name="instruction">An optional help string to display for the library management dialog</param>
		''' <param name="allowAllLocations">If true, do not show warning dialogs about locations that cannot be indexed</param>
		''' <remarks>If the library is already open in read-write mode, the dialog will not save the changes.</remarks>
		Public Shared Sub ShowManageLibraryUI(libraryName As String, windowHandle As IntPtr, title As String, instruction As String, allowAllLocations As Boolean)
			' this method is not safe for MTA consumption and will blow
			' Access Violations if called from an MTA thread so we wrap this
			' call up into a Worker thread that performs all operations in a
			' single threaded apartment
			Using shellLibrary__1 As ShellLibrary = ShellLibrary.Load(libraryName, True)
				ShowManageLibraryUI(shellLibrary__1, windowHandle, title, instruction, allowAllLocations)
			End Using
		End Sub

		''' <summary>
		''' Shows the library management dialog which enables users to mange the library folders and default save location.
		''' </summary>
		''' <param name="sourceKnownFolder">A known folder.</param>
		''' <param name="windowHandle">The parent window,or IntPtr.Zero for no parent</param>
		''' <param name="title">A title for the library management dialog, or null to use the library name as the title</param>
		''' <param name="instruction">An optional help string to display for the library management dialog</param>
		''' <param name="allowAllLocations">If true, do not show warning dialogs about locations that cannot be indexed</param>
		''' <remarks>If the library is already open in read-write mode, the dialog will not save the changes.</remarks>
		Public Shared Sub ShowManageLibraryUI(sourceKnownFolder As IKnownFolder, windowHandle As IntPtr, title As String, instruction As String, allowAllLocations As Boolean)
			' this method is not safe for MTA consumption and will blow
			' Access Violations if called from an MTA thread so we wrap this
			' call up into a Worker thread that performs all operations in a
			' single threaded apartment
			Using shellLibrary__1 As ShellLibrary = ShellLibrary.Load(sourceKnownFolder, True)
				ShowManageLibraryUI(shellLibrary__1, windowHandle, title, instruction, allowAllLocations)
			End Using
		End Sub

#End Region

#Region "Collection Members"

        ''' <summary>
        ''' Add a new FileSystemFolder or SearchConnector
        ''' </summary>
        ''' <param name="item">The folder to add to the library.</param>
        Public Sub Add(item As ShellFileSystemFolder) Implements IList(Of ShellFileSystemFolder).Add
            If item Is Nothing Then
                Throw New ArgumentNullException("item")
            End If

            nativeShellLibrary.AddFolder(item.NativeShellItem)
            nativeShellLibrary.Commit()
        End Sub

        ''' <summary>
        ''' Add an existing folder to this library
        ''' </summary>
        ''' <param name="folderPath">The path to the folder to be added to the library.</param>
        Public Sub Add(folderPath As String)
			If Not Directory.Exists(folderPath) Then
				Throw New DirectoryNotFoundException(LocalizedMessages.ShellLibraryFolderNotFound)
			End If

			Add(ShellFileSystemFolder.FromFolderPath(folderPath))
		End Sub

		''' <summary>
		''' Clear all items of this Library 
		''' </summary>
		Public Sub Clear() Implements ICollection(Of ShellFileSystemFolder).Clear
			Dim list As List(Of ShellFileSystemFolder) = ItemsList
			For Each folder As ShellFileSystemFolder In list
				nativeShellLibrary.RemoveFolder(folder.NativeShellItem)
			Next

			nativeShellLibrary.Commit()
		End Sub

        ''' <summary>
        ''' Remove a folder or search connector
        ''' </summary>
        ''' <param name="item">The item to remove.</param>
        ''' <returns><B>true</B> if the item was removed.</returns>
        Public Function Remove(item As ShellFileSystemFolder) As Boolean Implements IList(Of ShellFileSystemFolder).Remove
            If item Is Nothing Then
                Throw New ArgumentNullException("item")
            End If

            Try
                nativeShellLibrary.RemoveFolder(item.NativeShellItem)
                nativeShellLibrary.Commit()
            Catch generatedExceptionName As COMException
                Return False
            End Try

            Return True
        End Function

        ''' <summary>
        ''' Remove a folder or search connector
        ''' </summary>
        ''' <param name="folderPath">The path of the item to remove.</param>
        ''' <returns><B>true</B> if the item was removed.</returns>
        Public Function Remove(folderPath As String) As Boolean
			Dim item As ShellFileSystemFolder = ShellFileSystemFolder.FromFolderPath(folderPath)
			Return Remove(item)
		End Function

		#End Region

		#Region "Disposable Pattern"

		''' <summary>
		''' Release resources
		''' </summary>
		''' <param name="disposing">Indicates that this was called from Dispose(), rather than from the finalizer.</param>
		Protected Overrides Sub Dispose(disposing As Boolean)
			If nativeShellLibrary IsNot Nothing Then
				Marshal.ReleaseComObject(nativeShellLibrary)
				nativeShellLibrary = Nothing
			End If

			MyBase.Dispose(disposing)
		End Sub

		''' <summary>
		''' Release resources
		''' </summary>
		Protected Overrides Sub Finalize()
			Try
				Dispose(False)
			Finally
				MyBase.Finalize()
			End Try
		End Sub

		#End Region

		#Region "Private Properties"

		Private ReadOnly Property ItemsList() As List(Of ShellFileSystemFolder)
			Get
				Return GetFolders()
			End Get
		End Property

		Private Function GetFolders() As List(Of ShellFileSystemFolder)
			Dim list As New List(Of ShellFileSystemFolder)()
			Dim itemArray As IShellItemArray

			Dim shellItemArrayGuid As New Guid(ShellIIDGuid.IShellItemArray)

			Dim hr As HResult = nativeShellLibrary.GetFolders(ShellNativeMethods.LibraryFolderFilter.AllItems, shellItemArrayGuid, itemArray)

			If Not CoreErrorHelper.Succeeded(hr) Then
				Return list
			End If

			Dim count As UInteger
			itemArray.GetCount(count)

            For i As UInteger = 0 To count - 1UI
                Dim shellItem As IShellItem
                itemArray.GetItemAt(i, shellItem)
                list.Add(New ShellFileSystemFolder(TryCast(shellItem, IShellItem2)))
            Next

            If itemArray IsNot Nothing Then
				Marshal.ReleaseComObject(itemArray)
				itemArray = Nothing
			End If

			Return list
		End Function

		#End Region

		#Region "IEnumerable<ShellFileSystemFolder> Members"

		''' <summary>
		''' Retrieves the collection enumerator.
		''' </summary>
		''' <returns>The enumerator.</returns>
		Public Shadows Function GetEnumerator() As IEnumerator(Of ShellFileSystemFolder) Implements IEnumerable(Of ShellFileSystemFolder).GetEnumerator
			Return ItemsList.GetEnumerator()
		End Function

		#End Region

		#Region "IEnumerable Members"

		''' <summary>
		''' Retrieves the collection enumerator.
		''' </summary>
		''' <returns>The enumerator.</returns>
		Private Function System_Collections_IEnumerable_GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
			Return ItemsList.GetEnumerator()
		End Function

		#End Region

		#Region "ICollection<ShellFileSystemFolder> Members"


		''' <summary>
		''' Determines if an item with the specified path exists in the collection.
		''' </summary>
		''' <param name="fullPath">The path of the item.</param>
		''' <returns><B>true</B> if the item exists in the collection.</returns>
		Public Function Contains(fullPath As String) As Boolean
			If String.IsNullOrEmpty(fullPath) Then
				Throw New ArgumentNullException("fullPath")
			End If

			Return ItemsList.Any(Function(folder) String.Equals(fullPath, folder.Path, StringComparison.OrdinalIgnoreCase))
		End Function

        ''' <summary>
        ''' Determines if a folder exists in the collection.
        ''' </summary>
        ''' <param name="item">The folder.</param>
        ''' <returns><B>true</B>, if the folder exists in the collection.</returns>
        Public Function Contains(item As ShellFileSystemFolder) As Boolean Implements IList(Of ShellFileSystemFolder).Contains
            If item Is Nothing Then
                Throw New ArgumentNullException("item")
            End If

            Return ItemsList.Any(Function(folder) String.Equals(item.Path, folder.Path, StringComparison.OrdinalIgnoreCase))
        End Function

#End Region

#Region "IList<FileSystemFolder> Members"

        ''' <summary>
        ''' Searches for the specified FileSystemFolder and returns the zero-based index of the
        ''' first occurrence within Library list.
        ''' </summary>
        ''' <param name="item">The item to search for.</param>
        ''' <returns>The index of the item in the collection, or -1 if the item does not exist.</returns>
        Public Function IndexOf(item As ShellFileSystemFolder) As Integer Implements IList(Of ShellFileSystemFolder).IndexOf
            Return ItemsList.IndexOf(item)
        End Function

        ''' <summary>
        ''' Inserts a FileSystemFolder at the specified index.
        ''' </summary>
        ''' <param name="index">The index to insert at.</param>
        ''' <param name="item">The FileSystemFolder to insert.</param>
        Private Sub IList_Insert(index As Integer, item As ShellFileSystemFolder) Implements IList(Of ShellFileSystemFolder).Insert
			' Index related options are not supported by IShellLibrary doesn't support them.
			Throw New NotImplementedException()
		End Sub

		''' <summary>
		''' Removes an item at the specified index.
		''' </summary>
		''' <param name="index">The index to remove.</param>
		Private Sub IList_RemoveAt(index As Integer) Implements IList(Of ShellFileSystemFolder).RemoveAt
			' Index related options are not supported by IShellLibrary doesn't support them.
			Throw New NotImplementedException()
		End Sub

		''' <summary>
		''' Retrieves the folder at the specified index
		''' </summary>
		''' <param name="index">The index of the folder to retrieve.</param>
		''' <returns>A folder.</returns>
		Public Default Property Item(index As Integer) As ShellFileSystemFolder Implements IList(Of ShellFileSystemFolder).Item
			Get
				Return ItemsList(index)
			End Get
			Set
				' Index related options are not supported by IShellLibrary
				' doesn't support them.
				Throw New NotImplementedException()
			End Set
		End Property
		#End Region

		#Region "ICollection<ShellFileSystemFolder> Members"

		''' <summary>
		''' Copies the collection to an array.
		''' </summary>
		''' <param name="array">The array to copy to.</param>
		''' <param name="arrayIndex">The index in the array at which to start the copy.</param>
		Private Sub ICollection_CopyTo(array As ShellFileSystemFolder(), arrayIndex As Integer) Implements ICollection(Of ShellFileSystemFolder).CopyTo
			Throw New NotImplementedException()
		End Sub

		''' <summary>
		''' The count of the items in the list.
		''' </summary>
		Public ReadOnly Property Count() As Integer Implements ICollection(Of ShellFileSystemFolder).Count
			Get
				Return ItemsList.Count
			End Get
		End Property

		''' <summary>
		''' Indicates whether this list is read-only or not.
		''' </summary>
		Public ReadOnly Property IsReadOnly() As Boolean Implements ICollection(Of ShellFileSystemFolder).IsReadOnly
			Get
				Return False
			End Get
		End Property

		#End Region

		''' <summary>
		''' Indicates whether this feature is supported on the current platform.
		''' </summary>
		Public Shared Shadows ReadOnly Property IsPlatformSupported() As Boolean
			Get
				' We need Windows 7 onwards ...
				Return CoreHelpers.RunningOnWin7
			End Get
		End Property
	End Class

End Namespace
