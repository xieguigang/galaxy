'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports Microsoft.Windows.Internal

Namespace Shell
	''' <summary>
	''' Represents a saved search
	''' </summary>
	Public Class ShellSavedSearchCollection
		Inherits ShellSearchCollection
		Friend Sub New(shellItem As IShellItem2)
			MyBase.New(shellItem)
			CoreHelpers.ThrowIfNotVista()
		End Sub
	End Class
End Namespace
