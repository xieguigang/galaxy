'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Collections
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices

Namespace Net
    <ComImport, ClassInterface(CShort(0)), Guid("DCB00C01-570F-4A9B-8D69-199FDBA5723B")>
    <ComSourceInterfaces("Microsoft.Windows.NetworkList.Internal.INetworkEvents" & vbNullChar & "Microsoft.Windows.NetworkList.Internal.INetworkConnectionEvents" & vbNullChar & "Microsoft.Windows.NetworkList.Internal.INetworkListManagerEvents" & vbNullChar), TypeLibType(CShort(2))>
    Public Class NetworkListManagerClass
        Implements INetworkListManager
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(7)>
        Public Overridable Function GetConnectivity() As ConnectivityStates Implements INetworkListManager.GetConnectivity
        End Function

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(2)>
        Public Overridable Function GetNetwork(<[In]> gdNetworkId As Guid) As <MarshalAs(UnmanagedType.[Interface])> INetwork Implements INetworkListManager.GetNetwork
        End Function

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(4)>
        Public Overridable Function GetNetworkConnection(<[In]> gdNetworkConnectionId As Guid) As <MarshalAs(UnmanagedType.[Interface])> INetworkConnection Implements INetworkListManager.GetNetworkConnection
        End Function

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(3)>
        Public Overridable Function GetNetworkConnections() As <MarshalAs(UnmanagedType.[Interface])> IEnumerable Implements INetworkListManager.GetNetworkConnections
        End Function

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(1)>
        Public Overridable Function GetNetworks(<[In]> Flags As NetworkConnectivityLevels) As <MarshalAs(UnmanagedType.[Interface])> IEnumerable Implements INetworkListManager.GetNetworks
        End Function

        <DispId(6)>
        Public Overridable ReadOnly Property IsConnected() As Boolean Implements INetworkListManager.IsConnected

        <DispId(5)>
        Public Overridable ReadOnly Property IsConnectedToInternet() As Boolean Implements INetworkListManager.IsConnectedToInternet
    End Class
End Namespace
