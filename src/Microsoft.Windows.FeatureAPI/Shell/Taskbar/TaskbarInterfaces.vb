'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports Microsoft.Windows.Shell
Imports Microsoft.Windows.Shell.PropertySystem
Namespace Taskbar
	''' <summary>
	''' Interface for jump list items
	''' </summary>
	Public Interface IJumpListItem
		''' <summary>
		''' Gets or sets this item's path
		''' </summary>
		Property Path() As String
	End Interface

	''' <summary>
	''' Interface for jump list tasks
	''' </summary>
	Public MustInherit Class JumpListTask
		Friend MustOverride ReadOnly Property NativeShellLink() As IShellLinkW
	End Class
End Namespace
