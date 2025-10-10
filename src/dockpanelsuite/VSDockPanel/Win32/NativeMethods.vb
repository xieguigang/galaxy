Imports System.Diagnostics.CodeAnalysis
Imports System.Drawing
Imports System.Runtime.InteropServices

Namespace Win32

    Friend Module NativeMethods

        ' <MarshalAs(UnmanagedType.Bool)>
        <DllImport("User32.dll", CharSet:=CharSet.Auto)>
        Public Function DragDetect(hWnd As IntPtr, pt As Point) As Boolean
        End Function

        <DllImport("User32.dll", CharSet:=CharSet.Auto)>
        Public Function GetFocus() As IntPtr
        End Function

        <DllImport("User32.dll", CharSet:=CharSet.Auto)>
        Public Function SetFocus(hWnd As IntPtr) As IntPtr
        End Function

        ' <MarshalAs(UnmanagedType.Bool)>
        <DllImport("User32.dll", CharSet:=CharSet.Auto)>
        Public Function PostMessage(hWnd As IntPtr, Msg As Integer, wParam As UInteger, lParam As UInteger) As Boolean
        End Function

        <DllImport("User32.dll", CharSet:=CharSet.Auto)>
        Public Function SendMessage(hWnd As IntPtr, Msg As Integer, wParam As UInteger, lParam As UInteger) As UInteger
        End Function

        <DllImport("User32.dll", CharSet:=CharSet.Auto)>
        Public Function SetWindowPos(hWnd As IntPtr, hWndAfter As IntPtr, X As Integer, Y As Integer, Width As Integer, Height As Integer, flags As FlagsSetWindowPos) As Integer
        End Function

        <DllImport("user32.dll", CharSet:=CharSet.Auto)>
        Public Function GetWindowLong(hWnd As IntPtr, Index As Integer) As Integer
        End Function

        <DllImport("user32.dll", CharSet:=CharSet.Auto)>
        Public Function SetWindowLong(hWnd As IntPtr, Index As Integer, Value As Integer) As Integer
        End Function

        <DllImport("user32.dll", CharSet:=CharSet.Auto)>
        Public Function ShowScrollBar(hWnd As IntPtr, wBar As Integer, bShow As Integer) As Integer
        End Function

        '*********************************
        ' FxCop bug, suppress the message
        '*********************************
        <DllImport("user32.dll", CharSet:=CharSet.Auto)>
        <SuppressMessage("Microsoft.Portability", "CA1901:PInvokeDeclarationsShouldBePortable", MessageId:="0")>
        Public Function WindowFromPoint(point As Point) As IntPtr
        End Function

        <DllImport("Kernel32.dll", CharSet:=CharSet.Auto)>
        Public Function GetCurrentThreadId() As Integer
        End Function

        Public Delegate Function HookProc(code As Integer, wParam As IntPtr, lParam As IntPtr) As IntPtr

        <DllImport("user32.dll")>
        Public Function SetWindowsHookEx(code As HookType, func As HookProc, hInstance As IntPtr, threadID As Integer) As IntPtr
        End Function

        <DllImport("user32.dll")>
        Public Function UnhookWindowsHookEx(hhook As IntPtr) As Integer
        End Function

        <DllImport("user32.dll")>
        Public Function CallNextHookEx(hhook As IntPtr, code As Integer, wParam As IntPtr, lParam As IntPtr) As IntPtr
        End Function
    End Module
End Namespace