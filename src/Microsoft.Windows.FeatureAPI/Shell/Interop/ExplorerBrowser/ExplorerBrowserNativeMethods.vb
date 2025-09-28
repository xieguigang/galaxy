'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Runtime.InteropServices
Imports System.Security
Imports Microsoft.Windows.Internal

Namespace Controls
	''' <summary>
	''' Internal class that contains interop declarations for 
	''' functions that are not benign and are performance critical. 
	''' </summary>
	<SuppressUnmanagedCodeSecurity> _
	Friend NotInheritable Class ExplorerBrowserNativeMethods
		Private Sub New()
		End Sub
		<DllImport("SHLWAPI.DLL", CharSet := CharSet.Unicode, SetLastError := True)> _
		Friend Shared Function IUnknown_SetSite(<[In], MarshalAs(UnmanagedType.IUnknown)> punk As Object, <[In], MarshalAs(UnmanagedType.IUnknown)> punkSite As Object) As HResult
		End Function


		<DllImport("SHLWAPI.DLL", CharSet := CharSet.Unicode, SetLastError := True)> _
		Friend Shared Function ConnectToConnectionPoint(<[In], MarshalAs(UnmanagedType.IUnknown)> punk As Object, ByRef riidEvent As Guid, <MarshalAs(UnmanagedType.Bool)> fConnect As Boolean, <[In], MarshalAs(UnmanagedType.IUnknown)> punkTarget As Object, ByRef pdwCookie As UInteger, ByRef ppcpOut As IntPtr) As HResult
		End Function

	End Class
End Namespace
