'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports Microsoft.Windows.Shell
Imports Microsoft.Windows.Shell.PropertySystem
Imports Microsoft.Windows.Internal

Namespace Dialogs
    ' Disable warning if a method declaration hides another inherited from a parent COM interface
    ' To successfully import a COM interface, all inherited methods need to be declared again with 
    ' the exception of those already declared in "IUnknown"
    '#Pragma warning disable 0108

    <ComImport, Guid(ShellIIDGuid.IFileDialog), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
	Friend Interface IFileDialog
		Inherits IModalWindow
        ' Defined on IModalWindow - repeated here due to requirements of COM interop layer.
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), PreserveSig>
        Overloads Function Show(<[In]> parent As IntPtr) As Integer

        ' IFileDialog-Specific interface members.

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub SetFileTypes(<[In]> cFileTypes As UInteger, <[In], MarshalAs(UnmanagedType.LPArray)> rgFilterSpec As ShellNativeMethods.FilterSpec())

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub SetFileTypeIndex(<[In]> iFileType As UInteger)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub GetFileTypeIndex(ByRef piFileType As UInteger)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub Advise(<[In], MarshalAs(UnmanagedType.[Interface])> pfde As IFileDialogEvents, ByRef pdwCookie As UInteger)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub Unadvise(<[In]> dwCookie As UInteger)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub SetOptions(<[In]> fos As ShellNativeMethods.FileOpenOptions)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub GetOptions(ByRef pfos As ShellNativeMethods.FileOpenOptions)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub SetDefaultFolder(<[In], MarshalAs(UnmanagedType.[Interface])> psi As IShellItem)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub SetFolder(<[In], MarshalAs(UnmanagedType.[Interface])> psi As IShellItem)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub GetFolder(<MarshalAs(UnmanagedType.[Interface])> ByRef ppsi As IShellItem)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub GetCurrentSelection(<MarshalAs(UnmanagedType.[Interface])> ByRef ppsi As IShellItem)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub SetFileName(<[In], MarshalAs(UnmanagedType.LPWStr)> pszName As String)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub GetFileName(<MarshalAs(UnmanagedType.LPWStr)> ByRef pszName As String)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub SetTitle(<[In], MarshalAs(UnmanagedType.LPWStr)> pszTitle As String)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub SetOkButtonLabel(<[In], MarshalAs(UnmanagedType.LPWStr)> pszText As String)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub SetFileNameLabel(<[In], MarshalAs(UnmanagedType.LPWStr)> pszLabel As String)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub GetResult(<MarshalAs(UnmanagedType.[Interface])> ByRef ppsi As IShellItem)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub AddPlace(<[In], MarshalAs(UnmanagedType.[Interface])> psi As IShellItem, fdap As ShellNativeMethods.FileDialogAddPlacement)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub SetDefaultExtension(<[In], MarshalAs(UnmanagedType.LPWStr)> pszDefaultExtension As String)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub Close(<MarshalAs(UnmanagedType.[Error])> hr As Integer)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub SetClientGuid(<[In]> ByRef guid As Guid)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub ClearClientData()

		' Not supported:  IShellItemFilter is not defined, converting to IntPtr.
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub SetFilter(<MarshalAs(UnmanagedType.[Interface])> pFilter As IntPtr)
	End Interface

	<ComImport, Guid(ShellIIDGuid.IFileOpenDialog), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
	Friend Interface IFileOpenDialog
		Inherits IFileDialog
        ' Defined on IModalWindow - repeated here due to requirements of COM interop layer.
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), PreserveSig>
        Overloads Function Show(<[In]> parent As IntPtr) As Integer

        ' Defined on IFileDialog - repeated here due to requirements of COM interop layer.
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub SetFileTypes(<[In]> cFileTypes As UInteger, <[In]> ByRef rgFilterSpec As ShellNativeMethods.FilterSpec)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub SetFileTypeIndex(<[In]> iFileType As UInteger)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub GetFileTypeIndex(ByRef piFileType As UInteger)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub Advise(<[In], MarshalAs(UnmanagedType.[Interface])> pfde As IFileDialogEvents, ByRef pdwCookie As UInteger)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub Unadvise(<[In]> dwCookie As UInteger)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub SetOptions(<[In]> fos As ShellNativeMethods.FileOpenOptions)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub GetOptions(ByRef pfos As ShellNativeMethods.FileOpenOptions)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub SetDefaultFolder(<[In], MarshalAs(UnmanagedType.[Interface])> psi As IShellItem)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub SetFolder(<[In], MarshalAs(UnmanagedType.[Interface])> psi As IShellItem)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub GetFolder(<MarshalAs(UnmanagedType.[Interface])> ByRef ppsi As IShellItem)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub GetCurrentSelection(<MarshalAs(UnmanagedType.[Interface])> ByRef ppsi As IShellItem)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub SetFileName(<[In], MarshalAs(UnmanagedType.LPWStr)> pszName As String)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub GetFileName(<MarshalAs(UnmanagedType.LPWStr)> ByRef pszName As String)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub SetTitle(<[In], MarshalAs(UnmanagedType.LPWStr)> pszTitle As String)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub SetOkButtonLabel(<[In], MarshalAs(UnmanagedType.LPWStr)> pszText As String)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub SetFileNameLabel(<[In], MarshalAs(UnmanagedType.LPWStr)> pszLabel As String)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub GetResult(<MarshalAs(UnmanagedType.[Interface])> ByRef ppsi As IShellItem)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub AddPlace(<[In], MarshalAs(UnmanagedType.[Interface])> psi As IShellItem, fdap As ShellNativeMethods.FileDialogAddPlacement)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub SetDefaultExtension(<[In], MarshalAs(UnmanagedType.LPWStr)> pszDefaultExtension As String)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub Close(<MarshalAs(UnmanagedType.[Error])> hr As Integer)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub SetClientGuid(<[In]> ByRef guid As Guid)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub ClearClientData()

        ' Not supported:  IShellItemFilter is not defined, converting to IntPtr.
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub SetFilter(<MarshalAs(UnmanagedType.[Interface])> pFilter As IntPtr)

        ' Defined by IFileOpenDialog.
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub GetResults(<MarshalAs(UnmanagedType.[Interface])> ByRef ppenum As IShellItemArray)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub GetSelectedItems(<MarshalAs(UnmanagedType.[Interface])> ByRef ppsai As IShellItemArray)
	End Interface

	<ComImport, Guid(ShellIIDGuid.IFileSaveDialog), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
	Friend Interface IFileSaveDialog
		Inherits IFileDialog
        ' Defined on IModalWindow - repeated here due to requirements of COM interop layer.
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), PreserveSig>
        Overloads Function Show(<[In]> parent As IntPtr) As Integer

        ' Defined on IFileDialog - repeated here due to requirements of COM interop layer.
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub SetFileTypes(<[In]> cFileTypes As UInteger, <[In]> ByRef rgFilterSpec As ShellNativeMethods.FilterSpec)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub SetFileTypeIndex(<[In]> iFileType As UInteger)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub GetFileTypeIndex(ByRef piFileType As UInteger)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub Advise(<[In], MarshalAs(UnmanagedType.[Interface])> pfde As IFileDialogEvents, ByRef pdwCookie As UInteger)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub Unadvise(<[In]> dwCookie As UInteger)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub SetOptions(<[In]> fos As ShellNativeMethods.FileOpenOptions)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub GetOptions(ByRef pfos As ShellNativeMethods.FileOpenOptions)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub SetDefaultFolder(<[In], MarshalAs(UnmanagedType.[Interface])> psi As IShellItem)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub SetFolder(<[In], MarshalAs(UnmanagedType.[Interface])> psi As IShellItem)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub GetFolder(<MarshalAs(UnmanagedType.[Interface])> ByRef ppsi As IShellItem)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub GetCurrentSelection(<MarshalAs(UnmanagedType.[Interface])> ByRef ppsi As IShellItem)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub SetFileName(<[In], MarshalAs(UnmanagedType.LPWStr)> pszName As String)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub GetFileName(<MarshalAs(UnmanagedType.LPWStr)> ByRef pszName As String)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub SetTitle(<[In], MarshalAs(UnmanagedType.LPWStr)> pszTitle As String)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub SetOkButtonLabel(<[In], MarshalAs(UnmanagedType.LPWStr)> pszText As String)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub SetFileNameLabel(<[In], MarshalAs(UnmanagedType.LPWStr)> pszLabel As String)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub GetResult(<MarshalAs(UnmanagedType.[Interface])> ByRef ppsi As IShellItem)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub AddPlace(<[In], MarshalAs(UnmanagedType.[Interface])> psi As IShellItem, fdap As ShellNativeMethods.FileDialogAddPlacement)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub SetDefaultExtension(<[In], MarshalAs(UnmanagedType.LPWStr)> pszDefaultExtension As String)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub Close(<MarshalAs(UnmanagedType.[Error])> hr As Integer)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub SetClientGuid(<[In]> ByRef guid As Guid)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub ClearClientData()

        ' Not supported:  IShellItemFilter is not defined, converting to IntPtr.
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub SetFilter(<MarshalAs(UnmanagedType.[Interface])> pFilter As IntPtr)

        ' Defined by IFileSaveDialog interface.

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub SetSaveAsItem(<[In], MarshalAs(UnmanagedType.[Interface])> psi As IShellItem)

		' Not currently supported: IPropertyStore.
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub SetProperties(<[In], MarshalAs(UnmanagedType.[Interface])> pStore As IntPtr)

		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function SetCollectedProperties(<[In]> pList As IPropertyDescriptionList, <[In]> fAppendDefault As Boolean) As Integer

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		<PreserveSig> _
		Function GetProperties(ByRef ppStore As IPropertyStore) As HResult

		' Not currently supported: IPropertyStore, IFileOperationProgressSink.
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub ApplyProperties(<[In], MarshalAs(UnmanagedType.[Interface])> psi As IShellItem, <[In], MarshalAs(UnmanagedType.[Interface])> pStore As IntPtr, <[In], ComAliasName("ShellObjects.wireHWND")> ByRef hwnd As IntPtr, <[In], MarshalAs(UnmanagedType.[Interface])> pSink As IntPtr)
	End Interface

	<ComImport, Guid(ShellIIDGuid.IFileDialogEvents), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
	Friend Interface IFileDialogEvents
		' NOTE: some of these callbacks are cancelable - returning S_FALSE means that 
		' the dialog should not proceed (e.g. with closing, changing folder); to 
		' support this, we need to use the PreserveSig attribute to enable us to return
		' the proper HRESULT.

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime), PreserveSig> _
		Function OnFileOk(<[In], MarshalAs(UnmanagedType.[Interface])> pfd As IFileDialog) As HResult

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime), PreserveSig> _
		Function OnFolderChanging(<[In], MarshalAs(UnmanagedType.[Interface])> pfd As IFileDialog, <[In], MarshalAs(UnmanagedType.[Interface])> psiFolder As IShellItem) As HResult

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub OnFolderChange(<[In], MarshalAs(UnmanagedType.[Interface])> pfd As IFileDialog)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub OnSelectionChange(<[In], MarshalAs(UnmanagedType.[Interface])> pfd As IFileDialog)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub OnShareViolation(<[In], MarshalAs(UnmanagedType.[Interface])> pfd As IFileDialog, <[In], MarshalAs(UnmanagedType.[Interface])> psi As IShellItem, ByRef pResponse As ShellNativeMethods.FileDialogEventShareViolationResponse)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub OnTypeChange(<[In], MarshalAs(UnmanagedType.[Interface])> pfd As IFileDialog)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub OnOverwrite(<[In], MarshalAs(UnmanagedType.[Interface])> pfd As IFileDialog, <[In], MarshalAs(UnmanagedType.[Interface])> psi As IShellItem, ByRef pResponse As ShellNativeMethods.FileDialogEventOverwriteResponse)
	End Interface

	<ComImport, Guid(ShellIIDGuid.IFileDialogCustomize), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
	Friend Interface IFileDialogCustomize
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub EnableOpenDropDown(<[In]> dwIDCtl As Integer)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub AddMenu(<[In]> dwIDCtl As Integer, <[In], MarshalAs(UnmanagedType.LPWStr)> pszLabel As String)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub AddPushButton(<[In]> dwIDCtl As Integer, <[In], MarshalAs(UnmanagedType.LPWStr)> pszLabel As String)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub AddComboBox(<[In]> dwIDCtl As Integer)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub AddRadioButtonList(<[In]> dwIDCtl As Integer)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub AddCheckButton(<[In]> dwIDCtl As Integer, <[In], MarshalAs(UnmanagedType.LPWStr)> pszLabel As String, <[In]> bChecked As Boolean)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub AddEditBox(<[In]> dwIDCtl As Integer, <[In], MarshalAs(UnmanagedType.LPWStr)> pszText As String)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub AddSeparator(<[In]> dwIDCtl As Integer)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub AddText(<[In]> dwIDCtl As Integer, <[In], MarshalAs(UnmanagedType.LPWStr)> pszText As String)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub SetControlLabel(<[In]> dwIDCtl As Integer, <[In], MarshalAs(UnmanagedType.LPWStr)> pszLabel As String)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub GetControlState(<[In]> dwIDCtl As Integer, <Out> ByRef pdwState As ShellNativeMethods.ControlState)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub SetControlState(<[In]> dwIDCtl As Integer, <[In]> dwState As ShellNativeMethods.ControlState)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub GetEditBoxText(<[In]> dwIDCtl As Integer, <MarshalAs(UnmanagedType.LPWStr)> ByRef ppszText As String)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub SetEditBoxText(<[In]> dwIDCtl As Integer, <[In], MarshalAs(UnmanagedType.LPWStr)> pszText As String)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub GetCheckButtonState(<[In]> dwIDCtl As Integer, <Out> ByRef pbChecked As Boolean)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub SetCheckButtonState(<[In]> dwIDCtl As Integer, <[In]> bChecked As Boolean)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub AddControlItem(<[In]> dwIDCtl As Integer, <[In]> dwIDItem As Integer, <[In], MarshalAs(UnmanagedType.LPWStr)> pszLabel As String)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub RemoveControlItem(<[In]> dwIDCtl As Integer, <[In]> dwIDItem As Integer)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub RemoveAllControlItems(<[In]> dwIDCtl As Integer)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub GetControlItemState(<[In]> dwIDCtl As Integer, <[In]> dwIDItem As Integer, <Out> ByRef pdwState As ShellNativeMethods.ControlState)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub SetControlItemState(<[In]> dwIDCtl As Integer, <[In]> dwIDItem As Integer, <[In]> dwState As ShellNativeMethods.ControlState)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub GetSelectedControlItem(<[In]> dwIDCtl As Integer, <Out> ByRef pdwIDItem As Integer)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub SetSelectedControlItem(<[In]> dwIDCtl As Integer, <[In]> dwIDItem As Integer)
		' Not valid for OpenDropDown.
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub StartVisualGroup(<[In]> dwIDCtl As Integer, <[In], MarshalAs(UnmanagedType.LPWStr)> pszLabel As String)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub EndVisualGroup()

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub MakeProminent(<[In]> dwIDCtl As Integer)
	End Interface

	<ComImport, Guid(ShellIIDGuid.IFileDialogControlEvents), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
	Friend Interface IFileDialogControlEvents
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub OnItemSelected(<[In], MarshalAs(UnmanagedType.[Interface])> pfdc As IFileDialogCustomize, <[In]> dwIDCtl As Integer, <[In]> dwIDItem As Integer)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub OnButtonClicked(<[In], MarshalAs(UnmanagedType.[Interface])> pfdc As IFileDialogCustomize, <[In]> dwIDCtl As Integer)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub OnCheckButtonToggled(<[In], MarshalAs(UnmanagedType.[Interface])> pfdc As IFileDialogCustomize, <[In]> dwIDCtl As Integer, <[In]> bChecked As Boolean)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub OnControlActivating(<[In], MarshalAs(UnmanagedType.[Interface])> pfdc As IFileDialogCustomize, <[In]> dwIDCtl As Integer)
	End Interface
    ' Restore the warning
    '#Pragma warning restore 0108

End Namespace
