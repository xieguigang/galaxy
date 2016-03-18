' Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Runtime.InteropServices
Imports System.Windows.Media.Imaging

Namespace Shell
	''' <summary>
	''' Structure used internally to store property values for 
	''' a known folder. This structure holds the information
	''' returned in the FOLDER_DEFINITION structure, and 
	''' resources referenced by fields in NativeFolderDefinition,
	''' such as icon and tool tip.
	''' </summary>
	<StructLayout(LayoutKind.Sequential)> _
	Friend Structure FolderProperties
		Friend name As String
		Friend category As FolderCategory
		Friend canonicalName As String
		Friend description As String
		Friend parentId As Guid
		Friend parent As String
		Friend relativePath As String
		Friend parsingName As String
		Friend tooltipResourceId As String
		Friend tooltip As String
		Friend localizedName As String
		Friend localizedNameResourceId As String
		Friend iconResourceId As String
		Friend icon As BitmapSource
		Friend definitionOptions As DefinitionOptions
		Friend fileAttributes As System.IO.FileAttributes
		Friend folderTypeId As Guid
		Friend folderType As String
		Friend folderId As Guid
		Friend path As String
		Friend pathExists As Boolean
		Friend redirection As RedirectionCapability
		Friend security As String
	End Structure
End Namespace
