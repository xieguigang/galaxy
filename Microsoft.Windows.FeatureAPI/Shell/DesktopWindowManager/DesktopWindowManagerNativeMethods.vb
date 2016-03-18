'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Runtime.InteropServices
Imports System.Security
Imports Microsoft.Windows.Shell

Namespace Internal
    Friend NotInheritable Class DWMMessages
        Private Sub New()
        End Sub
        Friend Const WM_DWMCOMPOSITIONCHANGED As Integer = &H31E
        Friend Const WM_DWMNCRENDERINGCHANGED As Integer = &H31F
    End Class

    <StructLayout(LayoutKind.Sequential)>
    Friend Structure Margins
        Public LeftWidth As Integer
        ' width of left border that retains its size
        Public RightWidth As Integer
        ' width of right border that retains its size
        Public TopHeight As Integer
        ' height of top border that retains its size
        Public BottomHeight As Integer
        ' height of bottom border that retains its size
        Public Sub New(fullWindow As Boolean)
            LeftWidth = RightWidth.DirectCopy(TopHeight.DirectCopy(BottomHeight.DirectCopy(If(fullWindow, -1, 0))))
        End Sub
    End Structure

    Friend Enum CompositionEnable
        Disable = 0
        Enable = 1
    End Enum

    ''' <summary>
    ''' Internal class that contains interop declarations for 
    ''' functions that are not benign and are performance critical. 
    ''' </summary>
    <SuppressUnmanagedCodeSecurity>
    Friend NotInheritable Class DesktopWindowManagerNativeMethods
        Private Sub New()
        End Sub
        <DllImport("DwmApi.dll")>
        Friend Shared Function DwmExtendFrameIntoClientArea(hwnd As IntPtr, ByRef m As Margins) As Integer
        End Function

        <DllImport("DwmApi.dll", PreserveSig:=False)>
        Friend Shared Function DwmIsCompositionEnabled() As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        <DllImport("DwmApi.dll")>
        Friend Shared Function DwmEnableComposition(compositionAction As CompositionEnable) As Integer
        End Function

        <DllImport("user32.dll")>
        Friend Shared Function GetWindowRect(hwnd As IntPtr, <Out> ByRef rect As NativeRect) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        <DllImport("user32.dll")>
        Friend Shared Function GetClientRect(hwnd As IntPtr, <Out> ByRef rect As NativeRect) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function
    End Class
End Namespace
