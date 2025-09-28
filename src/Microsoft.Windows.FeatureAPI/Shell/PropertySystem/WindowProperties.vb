'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Windows
Imports System.Windows.Interop
Imports Microsoft.Windows.Taskbar

Namespace Shell.PropertySystem
    ''' <summary>
    ''' Helper class to modify properties for a given window
    ''' </summary>
    Public NotInheritable Class WindowProperties
        Private Sub New()
        End Sub
        ''' <summary>
        ''' Sets a shell property for a given window
        ''' </summary>
        ''' <param name="propKey">The property to set</param>
        ''' <param name="windowHandle">Handle to the window that the property will be set on</param>
        ''' <param name="value">The value to set for the property</param>
        Public Shared Sub SetWindowProperty(windowHandle As IntPtr, propKey As PropertyKey, value As String)
            TaskbarNativeMethods.SetWindowProperty(windowHandle, propKey, value)
        End Sub

        ''' <summary>
        ''' Sets a shell property for a given window
        ''' </summary>
        ''' <param name="propKey">The property to set</param>
        ''' <param name="window">Window that the property will be set on</param>
        ''' <param name="value">The value to set for the property</param>
        Public Shared Sub SetWindowProperty(window As Window, propKey As PropertyKey, value As String)
            TaskbarNativeMethods.SetWindowProperty((New WindowInteropHelper(window)).Handle, propKey, value)
        End Sub
    End Class
End Namespace
