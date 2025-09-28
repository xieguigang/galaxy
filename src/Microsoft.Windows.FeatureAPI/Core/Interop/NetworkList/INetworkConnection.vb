'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Collections
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices

Namespace Net
    <ComImport>
    <TypeLibType(CShort(&H1040))>
    <Guid("DCB00005-570F-4A9B-8D69-199FDBA5723B")>
    Public Interface INetworkConnection
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetNetwork() As <MarshalAs(UnmanagedType.[Interface])> INetwork

        ReadOnly Property IsConnectedToInternet() As Boolean

        ReadOnly Property IsConnected() As Boolean

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetConnectivity() As ConnectivityStates

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetConnectionId() As Guid

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetAdapterId() As Guid

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetDomainType() As DomainType
    End Interface
End Namespace
