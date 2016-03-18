'Copyright (c) Microsoft Corporation.  All rights reserved.

Namespace Internal
	''' <summary>
	''' Safe Icon Handle
	''' </summary>
	Public Class SafeIconHandle
		Inherits ZeroInvalidHandle
		''' <summary>
		''' Release the handle
		''' </summary>
		''' <returns>true if handled is release successfully, false otherwise</returns>
		Protected Overrides Function ReleaseHandle() As Boolean
			If CoreNativeMethods.DestroyIcon(handle) Then
				Return True
			Else
				Return False
			End If
		End Function
	End Class
End Namespace
