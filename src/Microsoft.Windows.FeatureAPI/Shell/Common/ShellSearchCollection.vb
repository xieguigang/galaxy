'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports Microsoft.Windows.Internal

Namespace Shell
	''' <summary>
	''' Represents the base class for all search-related classes.
	''' </summary>
	Public Class ShellSearchCollection
		Inherits ShellContainer
		Friend Sub New()
		End Sub

		Friend Sub New(shellItem As IShellItem2)
			MyBase.New(shellItem)
		End Sub
	End Class
End Namespace
