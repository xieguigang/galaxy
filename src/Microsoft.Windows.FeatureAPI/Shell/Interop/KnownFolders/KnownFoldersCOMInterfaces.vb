'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports Microsoft.Windows.Internal

Namespace Shell
    ' Disable warning if a method declaration hides another inherited from a parent COM interface
    ' To successfully import a COM interface, all inherited methods need to be declared again with 
    ' the exception of those already declared in "IUnknown"
    '#Pragma warning disable 0108


    <ComImport, Guid(KnownFoldersIIDGuid.IKnownFolder), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
	Friend Interface IKnownFolderNative
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function GetId() As Guid

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function GetCategory() As FolderCategory

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		<PreserveSig> _
		Function GetShellItem(<[In]> i As Integer, ByRef interfaceGuid As Guid, <Out, MarshalAs(UnmanagedType.[Interface])> ByRef shellItem As IShellItem2) As HResult

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function GetPath(<[In]> [option] As Integer) As <MarshalAs(UnmanagedType.LPWStr)> String

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub SetPath(<[In]> i As Integer, <[In]> path As String)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub GetIDList(<[In]> i As Integer, <Out> ByRef itemIdentifierListPointer As IntPtr)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function GetFolderType() As Guid

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function GetRedirectionCapabilities() As RedirectionCapability

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub GetFolderDefinition(<Out, MarshalAs(UnmanagedType.Struct)> ByRef definition As KnownFoldersSafeNativeMethods.NativeFolderDefinition)

	End Interface

	<ComImport, Guid(KnownFoldersIIDGuid.IKnownFolderManager), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
	Friend Interface IKnownFolderManager
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub FolderIdFromCsidl(csidl As Integer, <Out> ByRef knownFolderID As Guid)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub FolderIdToCsidl(<[In], MarshalAs(UnmanagedType.LPStruct)> id As Guid, <Out> ByRef csidl As Integer)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub GetFolderIds(<Out> ByRef folders As IntPtr, <Out> ByRef count As UInt32)

		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function GetFolder(<[In], MarshalAs(UnmanagedType.LPStruct)> id As Guid, <Out, MarshalAs(UnmanagedType.[Interface])> ByRef knownFolder As IKnownFolderNative) As HResult

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub GetFolderByName(canonicalName As String, <Out, MarshalAs(UnmanagedType.[Interface])> ByRef knownFolder As IKnownFolderNative)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub RegisterFolder(<[In], MarshalAs(UnmanagedType.LPStruct)> knownFolderGuid As Guid, <[In]> ByRef knownFolderDefinition As KnownFoldersSafeNativeMethods.NativeFolderDefinition)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub UnregisterFolder(<[In], MarshalAs(UnmanagedType.LPStruct)> knownFolderGuid As Guid)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub FindFolderFromPath(<[In], MarshalAs(UnmanagedType.LPWStr)> path As String, <[In]> mode As Integer, <Out, MarshalAs(UnmanagedType.[Interface])> ByRef knownFolder As IKnownFolderNative)

		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function FindFolderFromIDList(pidl As IntPtr, <Out, MarshalAs(UnmanagedType.[Interface])> ByRef knownFolder As IKnownFolderNative) As HResult

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub Redirect()
	End Interface

	<ComImport> _
	<Guid("4df0c730-df9d-4ae3-9153-aa6b82e9795a")> _
	Friend Class KnownFolderManagerClass
		Implements IKnownFolderManager

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub FolderIdFromCsidl(csidl As Integer, <Out> ByRef knownFolderID As Guid) Implements IKnownFolderManager.FolderIdFromCsidl
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub FolderIdToCsidl(<[In], MarshalAs(UnmanagedType.LPStruct)> id As Guid, <Out> ByRef csidl As Integer) Implements IKnownFolderManager.FolderIdToCsidl
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub GetFolderIds(<Out> ByRef folders As IntPtr, <Out> ByRef count As UInt32) Implements IKnownFolderManager.GetFolderIds
        End Sub

        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Function GetFolder(<[In], MarshalAs(UnmanagedType.LPStruct)> id As Guid, <Out, MarshalAs(UnmanagedType.[Interface])> ByRef knownFolder As IKnownFolderNative) As HResult Implements IKnownFolderManager.GetFolder
        End Function

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub GetFolderByName(canonicalName As String, <Out, MarshalAs(UnmanagedType.[Interface])> ByRef knownFolder As IKnownFolderNative) Implements IKnownFolderManager.GetFolderByName
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub RegisterFolder(<[In], MarshalAs(UnmanagedType.LPStruct)> knownFolderGuid As Guid, <[In]> ByRef knownFolderDefinition As KnownFoldersSafeNativeMethods.NativeFolderDefinition) Implements IKnownFolderManager.RegisterFolder
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub UnregisterFolder(<[In], MarshalAs(UnmanagedType.LPStruct)> knownFolderGuid As Guid) Implements IKnownFolderManager.UnregisterFolder
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub FindFolderFromPath(<[In], MarshalAs(UnmanagedType.LPWStr)> path As String, <[In]> mode As Integer, <Out, MarshalAs(UnmanagedType.[Interface])> ByRef knownFolder As IKnownFolderNative) Implements IKnownFolderManager.FindFolderFromPath
        End Sub

        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Function FindFolderFromIDList(pidl As IntPtr, <Out, MarshalAs(UnmanagedType.[Interface])> ByRef knownFolder As IKnownFolderNative) As HResult Implements IKnownFolderManager.FindFolderFromIDList
        End Function

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub Redirect() Implements IKnownFolderManager.Redirect
        End Sub
    End Class
End Namespace
