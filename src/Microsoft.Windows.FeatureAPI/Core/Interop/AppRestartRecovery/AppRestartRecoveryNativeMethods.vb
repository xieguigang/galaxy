'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Runtime.InteropServices
Imports Microsoft.Windows.Internal

Namespace ApplicationServices
    Public Module AppRestartRecoveryNativeMethods

#Region "Application Restart and Recovery Definitions"

        Public Delegate Function InternalRecoveryCallback(state As IntPtr) As UInt32

        Private m_internalCallback As New InternalRecoveryCallback(AddressOf InternalRecoveryHandler)

        Public ReadOnly Property InternalCallback() As InternalRecoveryCallback
            Get
                Return m_internalCallback
            End Get
        End Property

        Private Function InternalRecoveryHandler(parameter As IntPtr) As UInt32
            Dim cancelled As Boolean = False
            ApplicationRecoveryInProgress(cancelled)

            Dim handle As GCHandle = GCHandle.FromIntPtr(parameter)
            Dim data As RecoveryData = TryCast(handle.Target, RecoveryData)
            data.Invoke()
            handle.Free()

            Return (0)
        End Function

        <DllImport("kernel32.dll")>
        Public Sub ApplicationRecoveryFinished(<MarshalAs(UnmanagedType.Bool)> success As Boolean)
        End Sub

        <DllImport("kernel32.dll")>
        <PreserveSig>
        Public Function ApplicationRecoveryInProgress(<Out, MarshalAs(UnmanagedType.Bool)> ByRef canceled As Boolean) As HResult
        End Function

        <DllImport("kernel32.dll", CharSet:=CharSet.Unicode)>
        <PreserveSig>
        Public Function RegisterApplicationRecoveryCallback(callback As InternalRecoveryCallback, param As IntPtr, pingInterval As UInteger, flags As UInteger) As HResult
        End Function
        ' Unused.
        <DllImport("kernel32.dll")>
        <PreserveSig>
        Public Function RegisterApplicationRestart(<MarshalAs(UnmanagedType.BStr)> commandLineArgs As String, flags As RestartRestrictions) As HResult
        End Function

        <DllImport("kernel32.dll")>
        <PreserveSig>
        Public Function UnregisterApplicationRecoveryCallback() As HResult
        End Function

        <DllImport("kernel32.dll")>
        <PreserveSig>
        Public Function UnregisterApplicationRestart() As HResult
        End Function

#End Region
    End Module
End Namespace
