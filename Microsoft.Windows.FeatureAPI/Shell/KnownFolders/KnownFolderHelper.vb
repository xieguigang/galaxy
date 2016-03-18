' Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Diagnostics
Imports Microsoft.Windows.Resources
Imports Microsoft.Windows.Internal

Namespace Shell
	''' <summary>
	''' Creates the helper class for known folders.
	''' </summary>
	Public NotInheritable Class KnownFolderHelper
		Private Sub New()
		End Sub
		''' <summary>
		''' Returns the native known folder (IKnownFolderNative) given a PID list
		''' </summary>
		''' <param name="pidl"></param>
		''' <returns></returns>
		Friend Shared Function FromPIDL(pidl As IntPtr) As IKnownFolderNative
			Dim knownFolderManager As New KnownFolderManagerClass()

			Dim knownFolder As IKnownFolderNative
			Dim hr As HResult = knownFolderManager.FindFolderFromIDList(pidl, knownFolder)

			Return If((hr = HResult.Ok), knownFolder, Nothing)
		End Function

		''' <summary>
		''' Returns a known folder given a globally unique identifier.
		''' </summary>
		''' <param name="knownFolderId">A GUID for the requested known folder.</param>
		''' <returns>A known folder representing the specified name.</returns>
		''' <exception cref="System.ArgumentException">Thrown if the given Known Folder ID is invalid.</exception>
		Public Shared Function FromKnownFolderId(knownFolderId As Guid) As IKnownFolder
			Dim knownFolderNative As IKnownFolderNative
			Dim knownFolderManager As New KnownFolderManagerClass()

			Dim hr As HResult = knownFolderManager.GetFolder(knownFolderId, knownFolderNative)
			If hr <> HResult.Ok Then
				Throw New ShellException(hr)
			End If

			Dim kf As IKnownFolder = GetKnownFolder(knownFolderNative)
			If kf Is Nothing Then
				Throw New ArgumentException(LocalizedMessages.KnownFolderInvalidGuid, "knownFolderId")
			End If
			Return kf
		End Function

		''' <summary>
		''' Returns a known folder given a globally unique identifier.
		''' </summary>
		''' <param name="knownFolderId">A GUID for the requested known folder.</param>
		''' <returns>A known folder representing the specified name. Returns null if Known Folder is not found or could not be created.</returns>
		Friend Shared Function FromKnownFolderIdInternal(knownFolderId As Guid) As IKnownFolder
			Dim knownFolderNative As IKnownFolderNative
			Dim knownFolderManager As IKnownFolderManager = DirectCast(New KnownFolderManagerClass(), IKnownFolderManager)

			Dim hr As HResult = knownFolderManager.GetFolder(knownFolderId, knownFolderNative)

			Return If((hr = HResult.Ok), GetKnownFolder(knownFolderNative), Nothing)
		End Function

		''' <summary>
		''' Given a native KnownFolder (IKnownFolderNative), create the right type of
		''' IKnownFolder object (FileSystemKnownFolder or NonFileSystemKnownFolder)
		''' </summary>
		''' <param name="knownFolderNative">Native Known Folder</param>
		''' <returns></returns>
		Private Shared Function GetKnownFolder(knownFolderNative As IKnownFolderNative) As IKnownFolder
			Debug.Assert(knownFolderNative IsNot Nothing, "Native IKnownFolder should not be null.")

			' Get the native IShellItem2 from the native IKnownFolder
			Dim shellItem As IShellItem2
			Dim guid As New Guid(ShellIIDGuid.IShellItem2)
			Dim hr As HResult = knownFolderNative.GetShellItem(0, guid, shellItem)

			If Not CoreErrorHelper.Succeeded(hr) Then
				Return Nothing
			End If

			Dim isFileSystem As Boolean = False

			' If we have a valid IShellItem, try to get the FileSystem attribute.
			If shellItem IsNot Nothing Then
				Dim sfgao As ShellNativeMethods.ShellFileGetAttributesOptions
				shellItem.GetAttributes(ShellNativeMethods.ShellFileGetAttributesOptions.FileSystem, sfgao)

				' Is this item a FileSystem item?
				isFileSystem = (sfgao And ShellNativeMethods.ShellFileGetAttributesOptions.FileSystem) <> 0
			End If

			' If it's FileSystem, create a FileSystemKnownFolder, else NonFileSystemKnownFolder
			If isFileSystem Then
				Dim kf As New FileSystemKnownFolder(knownFolderNative)
				Return kf
			End If

			Dim knownFsFolder As New NonFileSystemKnownFolder(knownFolderNative)
			Return knownFsFolder
		End Function

		''' <summary>
		''' Returns the known folder given its canonical name.
		''' </summary>
		''' <param name="canonicalName">A non-localized canonical name for the known folder, such as MyComputer.</param>
		''' <returns>A known folder representing the specified name.</returns>
		''' <exception cref="System.ArgumentException">Thrown if the given canonical name is invalid or if the KnownFolder could not be created.</exception>
		Public Shared Function FromCanonicalName(canonicalName As String) As IKnownFolder
			Dim knownFolderNative As IKnownFolderNative
			Dim knownFolderManager As IKnownFolderManager = DirectCast(New KnownFolderManagerClass(), IKnownFolderManager)

			knownFolderManager.GetFolderByName(canonicalName, knownFolderNative)
			Dim kf As IKnownFolder = KnownFolderHelper.GetKnownFolder(knownFolderNative)

			If kf Is Nothing Then
				Throw New ArgumentException(LocalizedMessages.ShellInvalidCanonicalName, "canonicalName")
			End If
			Return kf
		End Function

		''' <summary>
		''' Returns a known folder given its shell path, such as <c>C:\users\public\documents</c> or 
		''' <c>::{645FF040-5081-101B-9F08-00AA002F954E}</c> for the Recycle Bin.
		''' </summary>
		''' <param name="path">The path for the requested known folder; either a physical path or a virtual path.</param>
		''' <returns>A known folder representing the specified name.</returns>
		Public Shared Function FromPath(path As String) As IKnownFolder
			Return KnownFolderHelper.FromParsingName(path)
		End Function

		''' <summary>
		''' Returns a known folder given its shell namespace parsing name, such as 
		''' <c>::{645FF040-5081-101B-9F08-00AA002F954E}</c> for the Recycle Bin.
		''' </summary>
		''' <param name="parsingName">The parsing name (or path) for the requested known folder.</param>
		''' <returns>A known folder representing the specified name.</returns>
		''' <exception cref="System.ArgumentException">Thrown if the given parsing name is invalid.</exception>
		Public Shared Function FromParsingName(parsingName As String) As IKnownFolder
			If parsingName Is Nothing Then
				Throw New ArgumentNullException("parsingName")
			End If

			Dim pidl As IntPtr = IntPtr.Zero
			Dim pidl2 As IntPtr = IntPtr.Zero

			Try
				pidl = ShellHelper.PidlFromParsingName(parsingName)

				If pidl = IntPtr.Zero Then
					Throw New ArgumentException(LocalizedMessages.KnownFolderParsingName, "parsingName")
				End If

				' It's probably a special folder, try to get it                
				Dim knownFolderNative As IKnownFolderNative = KnownFolderHelper.FromPIDL(pidl)
				If knownFolderNative IsNot Nothing Then
					Dim kf As IKnownFolder = KnownFolderHelper.GetKnownFolder(knownFolderNative)
					If kf Is Nothing Then
						Throw New ArgumentException(LocalizedMessages.KnownFolderParsingName, "parsingName")
					End If
					Return kf
				End If

				' No physical storage was found for this known folder
				' We'll try again with a different name

				' try one more time with a trailing \0
				pidl2 = ShellHelper.PidlFromParsingName(parsingName.PadRight(1, ControlChars.NullChar))

				If pidl2 = IntPtr.Zero Then
					Throw New ArgumentException(LocalizedMessages.KnownFolderParsingName, "parsingName")
				End If

				Dim kf2 As IKnownFolder = KnownFolderHelper.GetKnownFolder(KnownFolderHelper.FromPIDL(pidl))
				If kf2 Is Nothing Then
					Throw New ArgumentException(LocalizedMessages.KnownFolderParsingName, "parsingName")
				End If

				Return kf2
			Finally
				ShellNativeMethods.ILFree(pidl)
				ShellNativeMethods.ILFree(pidl2)
			End Try

		End Function
	End Class
End Namespace
