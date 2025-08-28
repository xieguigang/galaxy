'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Collections
Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.Diagnostics
Imports System.Runtime.InteropServices
Imports Microsoft.Windows.Shell

Namespace Dialogs
	''' <summary>
	''' Creates a Vista or Windows 7 Common File Dialog, allowing the user to select one or more files.
	''' </summary>
	''' 
	Public NotInheritable Class CommonOpenFileDialog
		Inherits CommonFileDialog
		Private openDialogCoClass As NativeFileOpenDialog

		''' <summary>
		''' Creates a new instance of this class.
		''' </summary>
		Public Sub New()
			MyBase.New()
			' For Open file dialog, allow read only files.
			MyBase.EnsureReadOnly = True
		End Sub

		''' <summary>
		''' Creates a new instance of this class with the specified name.
		''' </summary>
		''' <param name="name">The name of this dialog.</param>
		Public Sub New(name As String)
			MyBase.New(name)
			' For Open file dialog, allow read only files.
			MyBase.EnsureReadOnly = True
		End Sub

		#Region "Public API specific to Open"

		''' <summary>
		''' Gets a collection of the selected file names.
		''' </summary>
		''' <remarks>This property should only be used when the
		''' <see cref="CommonOpenFileDialog.Multiselect"/>
		''' property is <b>true</b>.</remarks>
		Public ReadOnly Property FileNames() As IEnumerable(Of String)
			Get
				CheckFileNamesAvailable()
				Return MyBase.FileNameCollection
			End Get
		End Property

		''' <summary>
		''' Gets a collection of the selected items as ShellObject objects.
		''' </summary>
		''' <remarks>This property should only be used when the
		''' <see cref="CommonOpenFileDialog.Multiselect"/>
		''' property is <b>true</b>.</remarks>
		Public ReadOnly Property FilesAsShellObject() As ICollection(Of ShellObject)
			Get
				' Check if we have selected files from the user.              
				CheckFileItemsAvailable()

				' temp collection to hold our shellobjects
				Dim resultItems As ICollection(Of ShellObject) = New Collection(Of ShellObject)()

				' Loop through our existing list of filenames, and try to create a concrete type of
				' ShellObject (e.g. ShellLibrary, FileSystemFolder, ShellFile, etc)
				For Each si As IShellItem In items
					resultItems.Add(ShellObjectFactory.Create(si))
				Next

				Return resultItems
			End Get
		End Property


		Private m_multiselect As Boolean
		''' <summary>
		''' Gets or sets a value that determines whether the user can select more than one file.
		''' </summary>
		Public Property Multiselect() As Boolean
			Get
				Return m_multiselect
			End Get
			Set
				m_multiselect = value
			End Set
		End Property

		Private m_isFolderPicker As Boolean
		''' <summary>
		''' Gets or sets a value that determines whether the user can select folders or files.
		''' Default value is false.
		''' </summary>
		Public Property IsFolderPicker() As Boolean
			Get
				Return m_isFolderPicker
			End Get
			Set
				m_isFolderPicker = value
			End Set
		End Property

		Private allowNonFileSystem As Boolean
		''' <summary>
		''' Gets or sets a value that determines whether the user can select non-filesystem items, 
		''' such as <b>Library</b>, <b>Search Connectors</b>, or <b>Known Folders</b>.
		''' </summary>
		Public Property AllowNonFileSystemItems() As Boolean
			Get
				Return allowNonFileSystem
			End Get
			Set
				allowNonFileSystem = value
			End Set
		End Property
		#End Region

		Friend Overrides Function GetNativeFileDialog() As IFileDialog
			System.Diagnostics.Debug.Assert(openDialogCoClass IsNot Nothing, "Must call Initialize() before fetching dialog interface")

			Return DirectCast(openDialogCoClass, IFileDialog)
		End Function

		Friend Overrides Sub InitializeNativeFileDialog()
			If openDialogCoClass Is Nothing Then
				openDialogCoClass = New NativeFileOpenDialog()
			End If
		End Sub

		Friend Overrides Sub CleanUpNativeFileDialog()
			If openDialogCoClass IsNot Nothing Then
				Marshal.ReleaseComObject(openDialogCoClass)
			End If
		End Sub

		Friend Overrides Sub PopulateWithFileNames(names As Collection(Of String))
			Dim resultsArray As IShellItemArray
			Dim count As UInteger

			openDialogCoClass.GetResults(resultsArray)
			resultsArray.GetCount(count)
            names.Clear()

            Dim Len As Integer = CInt(count) - 1

            For i As Integer = 0 To Len
                names.Add(GetFileNameFromShellItem(GetShellItemAt(resultsArray, i)))
            Next
        End Sub

		Friend Overrides Sub PopulateWithIShellItems(items As Collection(Of IShellItem))
			Dim resultsArray As IShellItemArray
			Dim count As UInteger

			openDialogCoClass.GetResults(resultsArray)
			resultsArray.GetCount(count)
            items.Clear()

            Dim Len = CInt(count) - 1

            For i As Integer = 0 To Len
                items.Add(GetShellItemAt(resultsArray, i))
            Next
        End Sub

		Friend Overrides Function GetDerivedOptionFlags(flags As ShellNativeMethods.FileOpenOptions) As ShellNativeMethods.FileOpenOptions
			If m_multiselect Then
				flags = flags Or ShellNativeMethods.FileOpenOptions.AllowMultiSelect
			End If
			If m_isFolderPicker Then
				flags = flags Or ShellNativeMethods.FileOpenOptions.PickFolders
			End If

			If Not allowNonFileSystem Then
				flags = flags Or ShellNativeMethods.FileOpenOptions.ForceFilesystem
			ElseIf allowNonFileSystem Then
				flags = flags Or ShellNativeMethods.FileOpenOptions.AllNonStorageItems
			End If

			Return flags
		End Function
	End Class
End Namespace
