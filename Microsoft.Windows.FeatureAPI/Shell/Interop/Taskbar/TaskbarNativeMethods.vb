'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Runtime.InteropServices
Imports Microsoft.Windows.Shell
Imports Microsoft.Windows.Shell.PropertySystem
Imports Microsoft.Windows.Internal

Namespace Taskbar
	#Region "Enums"
	Friend Enum KnownDestinationCategory
		Frequent = 1
		Recent
	End Enum

	Friend Enum ShellAddToRecentDocs
		Pidl = &H1
		PathA = &H2
		PathW = &H3
		AppIdInfo = &H4
		' indicates the data type is a pointer to a SHARDAPPIDINFO structure
		AppIdInfoIdList = &H5
		' indicates the data type is a pointer to a SHARDAPPIDINFOIDLIST structure
		Link = &H6
		' indicates the data type is a pointer to an IShellLink instance
		AppIdInfoLink = &H7
		' indicates the data type is a pointer to a SHARDAPPIDINFOLINK structure 
	End Enum

	Friend Enum TaskbarProgressBarStatus
		NoProgress = 0
		Indeterminate = &H1
		Normal = &H2
		[Error] = &H4
		Paused = &H8
	End Enum

	Friend Enum TaskbarActiveTabSetting
		UseMdiThumbnail = &H1
		UseMdiLivePreview = &H2
	End Enum

	Friend Enum ThumbButtonMask
		Bitmap = &H1
		Icon = &H2
		Tooltip = &H4
		THB_FLAGS = &H8
	End Enum

	<Flags> _
	Friend Enum ThumbButtonOptions
		Enabled = &H0
		Disabled = &H1
		DismissOnClick = &H2
		NoBackground = &H4
		Hidden = &H8
		NonInteractive = &H10
	End Enum

	Friend Enum SetTabPropertiesOption
		None = &H0
		UseAppThumbnailAlways = &H1
		UseAppThumbnailWhenActive = &H2
		UseAppPeekAlways = &H4
		UseAppPeekWhenActive = &H8
	End Enum

	#End Region

	#Region "Structs"

	<StructLayout(LayoutKind.Sequential, CharSet := CharSet.Auto)> _
	Friend Structure ThumbButton
		''' <summary>
		''' WPARAM value for a THUMBBUTTON being clicked.
		''' </summary>
		Friend Const Clicked As Integer = &H1800

		<MarshalAs(UnmanagedType.U4)> _
		Friend Mask As ThumbButtonMask
		Friend Id As UInteger
		Friend Bitmap As UInteger
		Friend Icon As IntPtr
		<MarshalAs(UnmanagedType.ByValTStr, SizeConst := 260)> _
		Friend Tip As String
		<MarshalAs(UnmanagedType.U4)> _
		Friend Flags As ThumbButtonOptions
	End Structure
	#End Region

	Friend NotInheritable Class TaskbarNativeMethods
		Private Sub New()
		End Sub
		Friend NotInheritable Class TaskbarGuids
			Private Sub New()
			End Sub
			Friend Shared IObjectArray As New Guid("92CA9DCD-5622-4BBA-A805-5E9F541BD8C9")
			Friend Shared IUnknown As New Guid("00000000-0000-0000-C000-000000000046")
		End Class

		Friend Const WmCommand As Integer = &H111

		' Register Window Message used by Shell to notify that the corresponding taskbar button has been added to the taskbar.
		Friend Shared ReadOnly WmTaskbarButtonCreated As UInteger = RegisterWindowMessage("TaskbarButtonCreated")

		Friend Const WmDwmSendIconThumbnail As UInteger = &H323
		Friend Const WmDwmSendIconicLivePreviewBitmap As UInteger = &H326

		#Region "Methods"

		<DllImport("shell32.dll")> _
		Friend Shared Sub SetCurrentProcessExplicitAppUserModelID(<MarshalAs(UnmanagedType.LPWStr)> AppID As String)
		End Sub

		<DllImport("shell32.dll")> _
		Friend Shared Sub GetCurrentProcessExplicitAppUserModelID(<Out, MarshalAs(UnmanagedType.LPWStr)> ByRef AppID As String)
		End Sub

		<DllImport("shell32.dll")> _
		Friend Shared Sub SHAddToRecentDocs(flags As ShellAddToRecentDocs, <MarshalAs(UnmanagedType.LPWStr)> path As String)
		End Sub

		Friend Shared Sub SHAddToRecentDocs(path As String)
			SHAddToRecentDocs(ShellAddToRecentDocs.PathW, path)
		End Sub

		<DllImport("user32.dll", EntryPoint := "RegisterWindowMessage", SetLastError := True, CharSet := CharSet.Unicode)> _
		Friend Shared Function RegisterWindowMessage(<MarshalAs(UnmanagedType.LPWStr)> lpString As String) As UInteger
		End Function


		'IID_IPropertyStore
		<DllImport("shell32.dll")> _
		Public Shared Function SHGetPropertyStoreForWindow(hwnd As IntPtr, ByRef iid As Guid, <Out, MarshalAs(UnmanagedType.[Interface])> ByRef propertyStore As IPropertyStore) As Integer
		End Function

		''' <summary>
		''' Sets the window's application id by its window handle.
		''' </summary>
		''' <param name="hwnd">The window handle.</param>
		''' <param name="appId">The application id.</param>
		Friend Shared Sub SetWindowAppId(hwnd As IntPtr, appId As String)
			SetWindowProperty(hwnd, SystemProperties.System.AppUserModel.ID, appId)
		End Sub

		Friend Shared Sub SetWindowProperty(hwnd As IntPtr, propkey As PropertyKey, value As String)
			' Get the IPropertyStore for the given window handle
			Dim propStore As IPropertyStore = GetWindowPropertyStore(hwnd)

			' Set the value
			Using pv As New PropVariant(value)
				Dim result As HResult = propStore.SetValue(propkey, pv)
				If Not CoreErrorHelper.Succeeded(result) Then
					Throw New ShellException(result)
				End If
			End Using


			' Dispose the IPropertyStore and PropVariant
			Marshal.ReleaseComObject(propStore)
		End Sub

		Friend Shared Function GetWindowPropertyStore(hwnd As IntPtr) As IPropertyStore
			Dim propStore As IPropertyStore
			Dim guid As New Guid(ShellIIDGuid.IPropertyStore)
			Dim rc As Integer = SHGetPropertyStoreForWindow(hwnd, guid, propStore)
			If rc <> 0 Then
				Throw Marshal.GetExceptionForHR(rc)
			End If
			Return propStore
		End Function

		#End Region
	End Class

	''' <summary>
	''' Thumbnail Alpha Types
	''' </summary>
	Public Enum ThumbnailAlphaType
		''' <summary>
		''' Let the system decide.
		''' </summary>
		Unknown = 0

		''' <summary>
		''' No transparency
		''' </summary>
		NoAlphaChannel = 1

		''' <summary>
		''' Has transparency
		''' </summary>
		HasAlphaChannel = 2
	End Enum
End Namespace
