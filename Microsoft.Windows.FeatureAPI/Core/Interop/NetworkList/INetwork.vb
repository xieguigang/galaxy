'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Collections
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices

Namespace Net

    <ComImport>
    <TypeLibType(CShort(&H1040))>
    <Guid("DCB00002-570F-4A9B-8D69-199FDBA5723B")>
    Public Interface INetwork
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetName() As <MarshalAs(UnmanagedType.BStr)> String

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub SetName(<[In], MarshalAs(UnmanagedType.BStr)> szNetworkNewName As String)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetDescription() As <MarshalAs(UnmanagedType.BStr)> String

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub SetDescription(<[In], MarshalAs(UnmanagedType.BStr)> szDescription As String)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetNetworkId() As Guid

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetDomainType() As DomainType

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetNetworkConnections() As <MarshalAs(UnmanagedType.[Interface])> IEnumerable

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetTimeCreatedAndConnected(ByRef pdwLowDateTimeCreated As UInteger, ByRef pdwHighDateTimeCreated As UInteger, ByRef pdwLowDateTimeConnected As UInteger, ByRef pdwHighDateTimeConnected As UInteger)

        ReadOnly Property IsConnectedToInternet() As Boolean

        ReadOnly Property IsConnected() As Boolean

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetConnectivity() As ConnectivityStates

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetCategory() As NetworkCategory

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub SetCategory(<[In]> NewCategory As NetworkCategory)
    End Interface
End Namespace
