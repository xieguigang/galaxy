'Copyright (c) Microsoft Corporation.  All rights reserved.

Namespace Shell
	''' <summary>
	''' Represents a non filesystem item (e.g. virtual items inside Control Panel)
	''' </summary>
	Public Class ShellNonFileSystemItem
		Inherits ShellObject
		Friend Sub New(shellItem As IShellItem2)
            m_nativeShellItem = shellItem
        End Sub
	End Class
End Namespace
