'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Security.Permissions
Namespace Internal
	''' <summary>
	''' Safe Window Handle
	''' </summary>
	Public Class SafeWindowHandle
		Inherits ZeroInvalidHandle
		''' <summary>
		''' Release the handle
		''' </summary>
		''' <returns>true if handled is release successfully, false otherwise</returns>
		Protected Overrides Function ReleaseHandle() As Boolean
			If IsInvalid Then
				Return True
			End If

			If CoreNativeMethods.DestroyWindow(handle) <> 0 Then
				Return True
			Else
				Return False
			End If
		End Function
	End Class
End Namespace
