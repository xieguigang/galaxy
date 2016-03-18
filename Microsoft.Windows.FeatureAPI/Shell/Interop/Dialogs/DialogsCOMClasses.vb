'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Runtime.InteropServices
Imports Microsoft.Windows.Shell

Namespace Dialogs

	' Dummy base interface for CommonFileDialog coclasses.
	Friend Interface NativeCommonFileDialog
	End Interface

	' Coclass interfaces - designed to "look like" the object 
	' in the API, so that the 'new' operator can be used in a 
	' straightforward way. Behind the scenes, the C# compiler
	' morphs all 'new CoClass()' calls to 'new CoClassWrapper()'.

	<ComImport, Guid(ShellIIDGuid.IFileOpenDialog), CoClass(GetType(FileOpenDialogRCW))> _
	Friend Interface NativeFileOpenDialog
		Inherits IFileOpenDialog
	End Interface

	<ComImport, Guid(ShellIIDGuid.IFileSaveDialog), CoClass(GetType(FileSaveDialogRCW))> _
	Friend Interface NativeFileSaveDialog
		Inherits IFileSaveDialog
	End Interface

	' .NET classes representing runtime callable wrappers.
	<ComImport, ClassInterface(ClassInterfaceType.None), TypeLibType(TypeLibTypeFlags.FCanCreate), Guid(ShellCLSIDGuid.FileOpenDialog)> _
	Friend Class FileOpenDialogRCW
	End Class

	<ComImport, ClassInterface(ClassInterfaceType.None), TypeLibType(TypeLibTypeFlags.FCanCreate), Guid(ShellCLSIDGuid.FileSaveDialog)> _
	Friend Class FileSaveDialogRCW
	End Class

End Namespace
