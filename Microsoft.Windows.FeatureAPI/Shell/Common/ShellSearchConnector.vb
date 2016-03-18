'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports Microsoft.Windows.Internal

Namespace Shell
	''' <summary>
	''' A Serch Connector folder in the Shell Namespace
	''' </summary>
	Public NotInheritable Class ShellSearchConnector
		Inherits ShellSearchCollection

		#Region "Internal Constructor"

		Friend Sub New()
			CoreHelpers.ThrowIfNotWin7()
		End Sub

		Friend Sub New(shellItem As IShellItem2)
			Me.New()
            m_nativeShellItem = shellItem
        End Sub

		#End Region

		''' <summary>
		''' Indicates whether this feature is supported on the current platform.
		''' </summary>
		Public Shared Shadows ReadOnly Property IsPlatformSupported() As Boolean
			Get
				' We need Windows 7 onwards ...
				Return CoreHelpers.RunningOnWin7
			End Get
		End Property
	End Class
End Namespace
