' Copyright (c) Microsoft Corporation.  All rights reserved.

Namespace Shell
	''' <summary>
	''' Represents a Non FileSystem folder (e.g. My Computer, Control Panel)
	''' </summary>
	Public Class ShellNonFileSystemFolder
		Inherits ShellFolder
		#Region "Internal Constructors"

				' Empty            
		Friend Sub New()
		End Sub

		Friend Sub New(shellItem As IShellItem2)
            m_nativeShellItem = shellItem
        End Sub

		#End Region

	End Class
End Namespace
