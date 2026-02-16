' Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Runtime.InteropServices
Imports System.Threading
Imports Microsoft.Windows.Internal

Namespace Shell
	Friend NotInheritable Class ShellObjectFactory
		Private Sub New()
		End Sub
		''' <summary>
		''' Creates a ShellObject given a native IShellItem interface
		''' </summary>
		''' <param name="nativeShellItem"></param>
		''' <returns>A newly constructed ShellObject object</returns>
		Friend Shared Function Create(nativeShellItem As IShellItem) As ShellObject
			' Sanity check
			System.Diagnostics.Debug.Assert(nativeShellItem IsNot Nothing, "nativeShellItem should not be null")

			' Need to make sure we're running on Vista or higher
			If Not CoreHelpers.RunningOnVista Then
				Throw New PlatformNotSupportedException(GlobalLocalizedMessages.ShellObjectFactoryPlatformNotSupported)
			End If

			' A lot of APIs need IShellItem2, so just keep a copy of it here
			Dim nativeShellItem2 As IShellItem2 = TryCast(nativeShellItem, IShellItem2)

			' Get the System.ItemType property
			Dim itemType As String = ShellHelper.GetItemType(nativeShellItem2)

			If Not String.IsNullOrEmpty(itemType) Then
				itemType = itemType.ToUpperInvariant()
			End If

			' Get some IShellItem attributes
			Dim sfgao As ShellNativeMethods.ShellFileGetAttributesOptions
			nativeShellItem2.GetAttributes(ShellNativeMethods.ShellFileGetAttributesOptions.FileSystem Or ShellNativeMethods.ShellFileGetAttributesOptions.Folder, sfgao)

			' Is this item a FileSystem item?
			Dim isFileSystem As Boolean = (sfgao And ShellNativeMethods.ShellFileGetAttributesOptions.FileSystem) <> 0

			' Is this item a Folder?
			Dim isFolder As Boolean = (sfgao And ShellNativeMethods.ShellFileGetAttributesOptions.Folder) <> 0

			' Shell Library
			Dim shellLibrary__1 As ShellLibrary = Nothing

			' Create the right type of ShellObject based on the above information 

			' 1. First check if this is a Shell Link
			If itemType = ".lnk" Then
				Return New ShellLink(nativeShellItem2)
			' 2. Check if this is a container or a single item (entity)
			ElseIf isFolder Then
				' 3. If this is a folder, check for types: Shell Library, Shell Folder or Search Container
				If itemType = ".library-ms" AndAlso shellLibrary__1.InlineCopy(ShellLibrary.FromShellItem(nativeShellItem2, True)) IsNot Nothing Then
					' we already created this above while checking for Library
					Return shellLibrary__1
				ElseIf itemType = ".searchconnector-ms" Then
					Return New ShellSearchConnector(nativeShellItem2)
				ElseIf itemType = ".search-ms" Then
					Return New ShellSavedSearchCollection(nativeShellItem2)
				End If

				' 4. It's a ShellFolder
				If isFileSystem Then
					' 5. Is it a (File-System / Non-Virtual) Known Folder
					If Not IsVirtualKnownFolder(nativeShellItem2) Then
						'needs to check if it is a known folder and not virtual
						Dim kf As New FileSystemKnownFolder(nativeShellItem2)
						Return kf
					End If

					Return New ShellFileSystemFolder(nativeShellItem2)
				End If

				' 5. Is it a (Non File-System / Virtual) Known Folder
				If IsVirtualKnownFolder(nativeShellItem2) Then
					'needs to check if known folder is virtual
					Dim kf As New NonFileSystemKnownFolder(nativeShellItem2)
					Return kf
				End If

				Return New ShellNonFileSystemFolder(nativeShellItem2)
			End If

			' 6. If this is an entity (single item), check if its filesystem or not
			If isFileSystem Then
				Return New ShellFile(nativeShellItem2)
			End If

			Return New ShellNonFileSystemItem(nativeShellItem2)
		End Function

		' This is a work around for the STA thread bug.  This will execute the call on a non-sta thread, then return the result
		Private Shared Function IsVirtualKnownFolder(nativeShellItem2 As IShellItem2) As Boolean
			Dim pidl As IntPtr = IntPtr.Zero
			Try
				Dim nativeFolder As IKnownFolderNative = Nothing
				Dim definition As New KnownFoldersSafeNativeMethods.NativeFolderDefinition()

				' We found a bug where the enumeration of shell folders was
				' not reliable when called from a STA thread - it would return
				' different results the first time vs the other times.
				'
				' This is a work around.  We call FindFolderFromIDList on a
				' worker MTA thread instead of the main STA thread.
				'
				' Ultimately, it would be a very good idea to replace the 'getting shell object' logic
				' to get a list of pidl's in 1 step, then look up their information in a 2nd, rather than
				' looking them up as we get them.  This would replace the need for the work around.
				Dim padlock As New Object()
				SyncLock padlock
					Dim unknown As IntPtr = Marshal.GetIUnknownForObject(nativeShellItem2)

                    ThreadPool.QueueUserWorkItem(Sub(obj)
                                                     SyncLock padlock
                                                         pidl = ShellHelper.PidlFromUnknown(unknown)

                                                         Call New KnownFolderManagerClass().FindFolderFromIDList(pidl, nativeFolder)

                                                         If nativeFolder IsNot Nothing Then
                                                             nativeFolder.GetFolderDefinition(definition)
                                                         End If

                                                         Monitor.Pulse(padlock)
                                                     End SyncLock
                                                 End Sub)
                    Monitor.Wait(padlock)
                End SyncLock

				Return nativeFolder IsNot Nothing AndAlso definition.category = FolderCategory.Virtual
			Finally
				ShellNativeMethods.ILFree(pidl)
			End Try
		End Function

		''' <summary>
		''' Creates a ShellObject given a parsing name
		''' </summary>
		''' <param name="parsingName"></param>
		''' <returns>A newly constructed ShellObject object</returns>
		Friend Shared Function Create(parsingName As String) As ShellObject
			If String.IsNullOrEmpty(parsingName) Then
				Throw New ArgumentNullException("parsingName")
			End If

			' Create a native shellitem from our path
			Dim nativeShellItem As IShellItem2
			Dim guid As New Guid(ShellIIDGuid.IShellItem2)
			Dim retCode As Integer = ShellNativeMethods.SHCreateItemFromParsingName(parsingName, IntPtr.Zero, guid, nativeShellItem)

			If Not CoreErrorHelper.Succeeded(retCode) Then
				Throw New ShellException(GlobalLocalizedMessages.ShellObjectFactoryUnableToCreateItem, Marshal.GetExceptionForHR(retCode))
			End If
			Return ShellObjectFactory.Create(nativeShellItem)
		End Function

		''' <summary>
		''' Constructs a new Shell object from IDList pointer
		''' </summary>
		''' <param name="idListPtr"></param>
		''' <returns></returns>
		Friend Shared Function Create(idListPtr As IntPtr) As ShellObject
			' Throw exception if not running on Win7 or newer.
			CoreHelpers.ThrowIfNotVista()

			Dim guid As New Guid(ShellIIDGuid.IShellItem2)

			Dim nativeShellItem As IShellItem2
			Dim retCode As Integer = ShellNativeMethods.SHCreateItemFromIDList(idListPtr, guid, nativeShellItem)

			If Not CoreErrorHelper.Succeeded(retCode) Then
				Return Nothing
			End If
			Return ShellObjectFactory.Create(nativeShellItem)
		End Function

		''' <summary>
		''' Constructs a new Shell object from IDList pointer
		''' </summary>
		''' <param name="idListPtr"></param>
		''' <param name="parent"></param>
		''' <returns></returns>
		Friend Shared Function Create(idListPtr As IntPtr, parent As ShellContainer) As ShellObject
			Dim nativeShellItem As IShellItem

			Dim retCode As Integer = ShellNativeMethods.SHCreateShellItem(IntPtr.Zero, parent.NativeShellFolder, idListPtr, nativeShellItem)

			If Not CoreErrorHelper.Succeeded(retCode) Then
				Return Nothing
			End If

			Return ShellObjectFactory.Create(nativeShellItem)
		End Function

    End Class
End Namespace
