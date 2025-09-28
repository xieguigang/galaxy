'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Runtime.InteropServices
Imports Microsoft.Windows.Internal

Namespace Taskbar
	<ComImportAttribute> _
	<GuidAttribute("6332DEBF-87B5-4670-90C0-5E57B408A49E")> _
	<InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)> _
	Friend Interface ICustomDestinationList
		Sub SetAppID(<MarshalAs(UnmanagedType.LPWStr)> pszAppID As String)
		<PreserveSig> _
		Function BeginList(ByRef cMaxSlots As UInteger, ByRef riid As Guid, <Out, MarshalAs(UnmanagedType.[Interface])> ByRef ppvObject As Object) As HResult
		<PreserveSig> _
		Function AppendCategory(<MarshalAs(UnmanagedType.LPWStr)> pszCategory As String, <MarshalAs(UnmanagedType.[Interface])> poa As IObjectArray) As HResult
		Sub AppendKnownCategory(<MarshalAs(UnmanagedType.I4)> category As KnownDestinationCategory)
		<PreserveSig> _
		Function AddUserTasks(<MarshalAs(UnmanagedType.[Interface])> poa As IObjectArray) As HResult
		Sub CommitList()
		Sub GetRemovedDestinations(ByRef riid As Guid, <Out, MarshalAs(UnmanagedType.[Interface])> ByRef ppvObject As Object)
		Sub DeleteList(<MarshalAs(UnmanagedType.LPWStr)> pszAppID As String)
		Sub AbortList()
	End Interface

	<GuidAttribute("77F10CF0-3DB5-4966-B520-B7C54FD35ED6")> _
	<ClassInterfaceAttribute(ClassInterfaceType.None)> _
	<ComImportAttribute> _
	Friend Class CDestinationList
	End Class

	<ComImportAttribute> _
	<GuidAttribute("92CA9DCD-5622-4BBA-A805-5E9F541BD8C9")> _
	<InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)> _
	Friend Interface IObjectArray
		Sub GetCount(ByRef cObjects As UInteger)
		Sub GetAt(iIndex As UInteger, ByRef riid As Guid, <Out, MarshalAs(UnmanagedType.[Interface])> ByRef ppvObject As Object)
	End Interface

	<ComImportAttribute> _
	<GuidAttribute("5632B1A4-E38A-400A-928A-D4CD63230295")> _
	<InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)> _
	Friend Interface IObjectCollection
		' IObjectArray
		<PreserveSig> _
		Sub GetCount(ByRef cObjects As UInteger)
		<PreserveSig> _
		Sub GetAt(iIndex As UInteger, ByRef riid As Guid, <Out, MarshalAs(UnmanagedType.[Interface])> ByRef ppvObject As Object)

		' IObjectCollection
		Sub AddObject(<MarshalAs(UnmanagedType.[Interface])> pvObject As Object)
		Sub AddFromArray(<MarshalAs(UnmanagedType.[Interface])> poaSource As IObjectArray)
		Sub RemoveObject(uiIndex As UInteger)
		Sub Clear()
	End Interface

	<GuidAttribute("2D3468C1-36A7-43B6-AC24-D3F02FD9607A")> _
	<ClassInterfaceAttribute(ClassInterfaceType.None)> _
	<ComImportAttribute> _
	Friend Class CEnumerableObjectCollection
	End Class

	<ComImportAttribute> _
	<GuidAttribute("c43dc798-95d1-4bea-9030-bb99e2983a1a")> _
	<InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)> _
	Friend Interface ITaskbarList4
		' ITaskbarList
		<PreserveSig> _
		Sub HrInit()
		<PreserveSig> _
		Sub AddTab(hwnd As IntPtr)
		<PreserveSig> _
		Sub DeleteTab(hwnd As IntPtr)
		<PreserveSig> _
		Sub ActivateTab(hwnd As IntPtr)
		<PreserveSig> _
		Sub SetActiveAlt(hwnd As IntPtr)

		' ITaskbarList2
		<PreserveSig> _
		Sub MarkFullscreenWindow(hwnd As IntPtr, <MarshalAs(UnmanagedType.Bool)> fFullscreen As Boolean)

		' ITaskbarList3
		<PreserveSig> _
		Sub SetProgressValue(hwnd As IntPtr, ullCompleted As UInt64, ullTotal As UInt64)
		<PreserveSig> _
		Sub SetProgressState(hwnd As IntPtr, tbpFlags As TaskbarProgressBarStatus)
		<PreserveSig> _
		Sub RegisterTab(hwndTab As IntPtr, hwndMDI As IntPtr)
		<PreserveSig> _
		Sub UnregisterTab(hwndTab As IntPtr)
		<PreserveSig> _
		Sub SetTabOrder(hwndTab As IntPtr, hwndInsertBefore As IntPtr)
		<PreserveSig> _
		Sub SetTabActive(hwndTab As IntPtr, hwndInsertBefore As IntPtr, dwReserved As UInteger)
		<PreserveSig> _
		Function ThumbBarAddButtons(hwnd As IntPtr, cButtons As UInteger, <MarshalAs(UnmanagedType.LPArray)> pButtons As ThumbButton()) As HResult
		<PreserveSig> _
		Function ThumbBarUpdateButtons(hwnd As IntPtr, cButtons As UInteger, <MarshalAs(UnmanagedType.LPArray)> pButtons As ThumbButton()) As HResult
		<PreserveSig> _
		Sub ThumbBarSetImageList(hwnd As IntPtr, himl As IntPtr)
		<PreserveSig> _
		Sub SetOverlayIcon(hwnd As IntPtr, hIcon As IntPtr, <MarshalAs(UnmanagedType.LPWStr)> pszDescription As String)
		<PreserveSig> _
		Sub SetThumbnailTooltip(hwnd As IntPtr, <MarshalAs(UnmanagedType.LPWStr)> pszTip As String)
		<PreserveSig> _
		Sub SetThumbnailClip(hwnd As IntPtr, prcClip As IntPtr)

		' ITaskbarList4
		Sub SetTabProperties(hwndTab As IntPtr, stpFlags As SetTabPropertiesOption)
	End Interface

	<GuidAttribute("56FDF344-FD6D-11d0-958A-006097C9A090")> _
	<ClassInterfaceAttribute(ClassInterfaceType.None)> _
	<ComImportAttribute> _
	Friend Class CTaskbarList
	End Class
End Namespace
