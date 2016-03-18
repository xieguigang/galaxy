'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Runtime.InteropServices
Imports System.Security

Namespace Shell
	''' <summary>
	''' Internal class that contains interop declarations for 
	''' functions that are considered benign but that
	''' are performance critical. 
	''' </summary>
	''' <remarks>
	''' Functions that are benign but not performance critical 
	''' should be located in the NativeMethods class.
	''' </remarks>
	<SuppressUnmanagedCodeSecurity> _
	Friend NotInheritable Class KnownFoldersSafeNativeMethods
		Private Sub New()
		End Sub
		#Region "KnownFolders"

		<StructLayout(LayoutKind.Sequential)> _
		Friend Structure NativeFolderDefinition
			Friend category As FolderCategory
			Friend name As IntPtr
			Friend description As IntPtr
			Friend parentId As Guid
			Friend relativePath As IntPtr
			Friend parsingName As IntPtr
			Friend tooltip As IntPtr
			Friend localizedName As IntPtr
			Friend icon As IntPtr
			Friend security As IntPtr
			Friend attributes As UInt32
			Friend definitionOptions As DefinitionOptions
			Friend folderTypeId As Guid
		End Structure

		#End Region
	End Class
End Namespace
