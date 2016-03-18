'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.IO
Imports System.Runtime.InteropServices
Imports Microsoft.Windows.Shell.PropertySystem
Imports Microsoft.Windows.Resources
Imports Microsoft.Windows.Internal

Namespace Shell
	''' <summary>
	''' A helper class for Shell Objects
	''' </summary>
	Friend NotInheritable Class ShellHelper
		Private Sub New()
		End Sub
		Friend Shared Function GetParsingName(shellItem As IShellItem) As String
			If shellItem Is Nothing Then
				Return Nothing
			End If

			Dim path As String = Nothing

			Dim pszPath As IntPtr = IntPtr.Zero
			Dim hr As HResult = shellItem.GetDisplayName(ShellNativeMethods.ShellItemDesignNameOptions.DesktopAbsoluteParsing, pszPath)

			If hr <> HResult.Ok AndAlso hr <> HResult.InvalidArguments Then
				Throw New ShellException(LocalizedMessages.ShellHelperGetParsingNameFailed, hr)
			End If

			If pszPath <> IntPtr.Zero Then
				path = Marshal.PtrToStringAuto(pszPath)
				Marshal.FreeCoTaskMem(pszPath)
				pszPath = IntPtr.Zero
			End If

			Return path

		End Function

		Friend Shared Function GetAbsolutePath(path__1 As String) As String
			If Uri.IsWellFormedUriString(path__1, UriKind.Absolute) Then
				Return path__1
			End If
			Return Path.GetFullPath((path__1))
		End Function

		Friend Shared ItemTypePropertyKey As New PropertyKey(New Guid("28636AA6-953D-11D2-B5D6-00C04FD918D0"), 11)

		Friend Shared Function GetItemType(shellItem As IShellItem2) As String
			If shellItem IsNot Nothing Then
				Dim itemType As String = Nothing
				Dim hr As HResult = shellItem.GetString(ItemTypePropertyKey, itemType)
				If hr = HResult.Ok Then
					Return itemType
				End If
			End If

			Return Nothing
		End Function

		Friend Shared Function PidlFromParsingName(name As String) As IntPtr
			Dim pidl As IntPtr

			Dim sfgao As ShellNativeMethods.ShellFileGetAttributesOptions
			Dim retCode As Integer = ShellNativeMethods.SHParseDisplayName(name, IntPtr.Zero, pidl, CType(0, ShellNativeMethods.ShellFileGetAttributesOptions), sfgao)

			Return (If(CoreErrorHelper.Succeeded(retCode), pidl, IntPtr.Zero))
		End Function

		Friend Shared Function PidlFromShellItem(nativeShellItem As IShellItem) As IntPtr
			Dim unknown As IntPtr = Marshal.GetIUnknownForObject(nativeShellItem)
			Return PidlFromUnknown(unknown)
		End Function

		Friend Shared Function PidlFromUnknown(unknown As IntPtr) As IntPtr
			Dim pidl As IntPtr
			Dim retCode As Integer = ShellNativeMethods.SHGetIDListFromObject(unknown, pidl)
			Return (If(CoreErrorHelper.Succeeded(retCode), pidl, IntPtr.Zero))
		End Function

	End Class
End Namespace
