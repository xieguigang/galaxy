'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Runtime.InteropServices
Imports System.Text

Namespace Internal
    ''' <summary>
    ''' Wrappers for Native Methods and Structs.
    ''' This type is intended for internal use only
    ''' </summary>    
    Public Module CoreNativeMethods

#Region "General Definitions"

        ''' <summary>
        ''' Places (posts) a message in the message queue associated with the thread that created
        ''' the specified window and returns without waiting for the thread to process the message.
        ''' </summary>
        ''' <param name="windowHandle">Handle to the window whose window procedure will receive the message. 
        ''' If this parameter is HWND_BROADCAST, the message is sent to all top-level windows in the system, 
        ''' including disabled or invisible unowned windows, overlapped windows, and pop-up windows; 
        ''' but the message is not sent to child windows.
        ''' </param>
        ''' <param name="message">Specifies the message to be sent.</param>
        ''' <param name="wparam">Specifies additional message-specific information.</param>
        ''' <param name="lparam">Specifies additional message-specific information.</param>
        <DllImport("user32.dll", CharSet:=CharSet.Auto, PreserveSig:=False, SetLastError:=True)>
        Public Sub PostMessage(windowHandle As IntPtr, message As WindowMessage, wparam As IntPtr, lparam As IntPtr)
        End Sub

        ''' <summary>
        ''' Sends the specified message to a window or windows. The SendMessage function calls 
        ''' the window procedure for the specified window and does not return until the window 
        ''' procedure has processed the message. 
        ''' </summary>
        ''' <param name="windowHandle">Handle to the window whose window procedure will receive the message. 
        ''' If this parameter is HWND_BROADCAST, the message is sent to all top-level windows in the system, 
        ''' including disabled or invisible unowned windows, overlapped windows, and pop-up windows; 
        ''' but the message is not sent to child windows.
        ''' </param>
        ''' <param name="message">Specifies the message to be sent.</param>
        ''' <param name="wparam">Specifies additional message-specific information.</param>
        ''' <param name="lparam">Specifies additional message-specific information.</param>
        ''' <returns>A return code specific to the message being sent.</returns>     
        <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
        Public Function SendMessage(windowHandle As IntPtr, message As WindowMessage, wparam As IntPtr, lparam As IntPtr) As IntPtr
        End Function

        ''' <summary>
        ''' Sends the specified message to a window or windows. The SendMessage function calls 
        ''' the window procedure for the specified window and does not return until the window 
        ''' procedure has processed the message. 
        ''' </summary>
        ''' <param name="windowHandle">Handle to the window whose window procedure will receive the message. 
        ''' If this parameter is HWND_BROADCAST, the message is sent to all top-level windows in the system, 
        ''' including disabled or invisible unowned windows, overlapped windows, and pop-up windows; 
        ''' but the message is not sent to child windows.
        ''' </param>
        ''' <param name="message">Specifies the message to be sent.</param>
        ''' <param name="wparam">Specifies additional message-specific information.</param>
        ''' <param name="lparam">Specifies additional message-specific information.</param>
        ''' <returns>A return code specific to the message being sent.</returns>        
        <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
        Public Function SendMessage(windowHandle As IntPtr, message As UInteger, wparam As IntPtr, lparam As IntPtr) As IntPtr
        End Function

        ''' <summary>
        ''' Sends the specified message to a window or windows. The SendMessage function calls 
        ''' the window procedure for the specified window and does not return until the window 
        ''' procedure has processed the message. 
        ''' </summary>
        ''' <param name="windowHandle">Handle to the window whose window procedure will receive the message. 
        ''' If this parameter is HWND_BROADCAST, the message is sent to all top-level windows in the system, 
        ''' including disabled or invisible unowned windows, overlapped windows, and pop-up windows; 
        ''' but the message is not sent to child windows.
        ''' </param>
        ''' <param name="message">Specifies the message to be sent.</param>
        ''' <param name="wparam">Specifies additional message-specific information.</param>
        ''' <param name="lparam">Specifies additional message-specific information.</param>
        ''' <returns>A return code specific to the message being sent.</returns>
        <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
        Public Function SendMessage(windowHandle As IntPtr, message As UInteger, wparam As IntPtr, <MarshalAs(UnmanagedType.LPWStr)> lparam As String) As IntPtr
        End Function

        ''' <summary>
        ''' Sends the specified message to a window or windows. The SendMessage function calls 
        ''' the window procedure for the specified window and does not return until the window 
        ''' procedure has processed the message. 
        ''' </summary>
        ''' <param name="windowHandle">Handle to the window whose window procedure will receive the message. 
        ''' If this parameter is HWND_BROADCAST, the message is sent to all top-level windows in the system, 
        ''' including disabled or invisible unowned windows, overlapped windows, and pop-up windows; 
        ''' but the message is not sent to child windows.
        ''' </param>
        ''' <param name="message">Specifies the message to be sent.</param>
        ''' <param name="wparam">Specifies additional message-specific information.</param>
        ''' <param name="lparam">Specifies additional message-specific information.</param>
        ''' <returns>A return code specific to the message being sent.</returns>
        Public Function SendMessage(windowHandle As IntPtr, message As UInteger, wparam As Integer, lparam As String) As IntPtr
            Return SendMessage(windowHandle, message, CType(wparam, IntPtr), lparam)
        End Function

        ''' <summary>
        ''' Sends the specified message to a window or windows. The SendMessage function calls 
        ''' the window procedure for the specified window and does not return until the window 
        ''' procedure has processed the message. 
        ''' </summary>
        ''' <param name="windowHandle">Handle to the window whose window procedure will receive the message. 
        ''' If this parameter is HWND_BROADCAST, the message is sent to all top-level windows in the system, 
        ''' including disabled or invisible unowned windows, overlapped windows, and pop-up windows; 
        ''' but the message is not sent to child windows.
        ''' </param>
        ''' <param name="message">Specifies the message to be sent.</param>
        ''' <param name="wparam">Specifies additional message-specific information.</param>
        ''' <param name="lparam">Specifies additional message-specific information.</param>
        ''' <returns>A return code specific to the message being sent.</returns>

        <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
        Public Function SendMessage(windowHandle As IntPtr, message As UInteger, ByRef wparam As Integer, <MarshalAs(UnmanagedType.LPWStr)> lparam As StringBuilder) As IntPtr
        End Function

        ' Various helpers for forcing binding to proper 
        ' version of Comctl32 (v6).
        <DllImport("kernel32.dll", SetLastError:=True, ThrowOnUnmappableChar:=True, BestFitMapping:=False)>
        Public Function LoadLibrary(<MarshalAs(UnmanagedType.LPStr)> fileName As String) As IntPtr
        End Function

        <DllImport("gdi32.dll")>
        Public Function DeleteObject(graphicsObjectHandle As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Unicode)>
        Public Function LoadString(instanceHandle As IntPtr, id As Integer, buffer As StringBuilder, bufferSize As Integer) As Integer
        End Function

        <DllImport("Kernel32.dll", EntryPoint:="LocalFree")>
        Public Function LocalFree(ByRef guid As Guid) As IntPtr
        End Function

        ''' <summary>
        ''' Destroys an icon and frees any memory the icon occupied.
        ''' </summary>
        ''' <param name="hIcon">Handle to the icon to be destroyed. The icon must not be in use. </param>
        ''' <returns>If the function succeeds, the return value is nonzero. If the function fails, the return value is zero. To get extended error information, call GetLastError. </returns>
        <DllImport("user32.dll", SetLastError:=True)>
        Public Function DestroyIcon(hIcon As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

#End Region

#Region "Window Handling"

        <DllImport("user32.dll", SetLastError:=True, EntryPoint:="DestroyWindow", CallingConvention:=CallingConvention.StdCall)>
        Public Function DestroyWindow(handle As IntPtr) As Integer
        End Function

#End Region

#Region "General Declarations"

        ' Various important window messages
        Public Const UserMessage As Integer = &H400
        Public Const EnterIdleMessage As Integer = &H121

        ' FormatMessage constants and structs.
        Public Const FormatMessageFromSystem As Integer = &H1000

        ' App recovery and restart return codes
        Public Const ResultFailed As UInteger = &H80004005UI
        Public Const ResultInvalidArgument As UInteger = &H80070057UI
        Public Const ResultFalse As UInteger = 1
        Public Const ResultNotFound As UInteger = &H80070490UI

        ''' <summary>
        ''' Gets the HiWord
        ''' </summary>
        ''' <param name="value">The value to get the hi word from.</param>
        ''' <param name="size">Size</param>
        ''' <returns>The upper half of the dword.</returns>        
        Public Function GetHiWord(value As Long, size As Integer) As Integer
            Return CShort(value >> size)
        End Function

        ''' <summary>
        ''' Gets the LoWord
        ''' </summary>
        ''' <param name="value">The value to get the low word from.</param>
        ''' <returns>The lower half of the dword.</returns>
        Public Function GetLoWord(value As Long) As Integer
            Return CShort(value And &HFFFF)
        End Function

#End Region

#Region "GDI and DWM Declarations"

        ''' <summary>
        ''' A Wrapper for a SIZE struct
        ''' </summary>
        <StructLayout(LayoutKind.Sequential)>
        Public Structure Size
            Private m_width As Integer
            Private m_height As Integer

            ''' <summary>
            ''' Width
            ''' </summary>
            Public Property Width() As Integer
                Get
                    Return m_width
                End Get
                Set
                    m_width = Value
                End Set
            End Property

            ''' <summary>
            ''' Height
            ''' </summary>
            Public Property Height() As Integer
                Get
                    Return m_height
                End Get
                Set
                    m_height = Value
                End Set
            End Property
        End Structure

        ' Enable/disable non-client rendering based on window style.
        Public Const DWMNCRP_USEWINDOWSTYLE As Integer = 0

        ' Disabled non-client rendering; window style is ignored.
        Public Const DWMNCRP_DISABLED As Integer = 1

        ' Enabled non-client rendering; window style is ignored.
        Public Const DWMNCRP_ENABLED As Integer = 2

        ' Enable/disable non-client rendering Use DWMNCRP_* values.
        Public Const DWMWA_NCRENDERING_ENABLED As Integer = 1

        ' Non-client rendering policy.
        Public Const DWMWA_NCRENDERING_POLICY As Integer = 2

        ' Potentially enable/forcibly disable transitions 0 or 1.
        Public Const DWMWA_TRANSITIONS_FORCEDISABLED As Integer = 3

#End Region

#Region "Windows OS structs and consts"

        Public Const StatusAccessDenied As UInteger = &HC0000022UI

        Public Delegate Function WNDPROC(hWnd As IntPtr, uMessage As UInteger, wParam As IntPtr, lParam As IntPtr) As Integer

#End Region
    End Module
End Namespace
