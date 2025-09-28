'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Collections
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices

Namespace Net
    <ComImport>
    <Guid("DCB00000-570F-4A9B-8D69-199FDBA5723B")>
    <TypeLibType(CShort(&H1040))>
    Public Interface INetworkListManager
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetNetworks(<[In]> Flags As NetworkConnectivityLevels) As <MarshalAs(UnmanagedType.[Interface])> IEnumerable

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetNetwork(<[In]> gdNetworkId As Guid) As <MarshalAs(UnmanagedType.[Interface])> INetwork

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetNetworkConnections() As <MarshalAs(UnmanagedType.[Interface])> IEnumerable

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetNetworkConnection(<[In]> gdNetworkConnectionId As Guid) As <MarshalAs(UnmanagedType.[Interface])> INetworkConnection

        ReadOnly Property IsConnectedToInternet() As Boolean

        ReadOnly Property IsConnected() As Boolean

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetConnectivity() As ConnectivityStates
    End Interface
End Namespace
