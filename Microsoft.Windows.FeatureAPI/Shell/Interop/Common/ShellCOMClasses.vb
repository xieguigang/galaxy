'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Runtime.InteropServices

Namespace Shell
	<ComImport, Guid(ShellIIDGuid.IShellLibrary), CoClass(GetType(ShellLibraryCoClass))> _
	Friend Interface INativeShellLibrary
		Inherits IShellLibrary
	End Interface

	<ComImport, ClassInterface(ClassInterfaceType.None), TypeLibType(TypeLibTypeFlags.FCanCreate), Guid(ShellCLSIDGuid.ShellLibrary)> _
	Friend Class ShellLibraryCoClass
	End Class
End Namespace
