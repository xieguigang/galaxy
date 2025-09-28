'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Runtime.InteropServices
Imports Microsoft.Windows.Internal
Imports Microsoft.Windows.Shell.Interop
Imports Microsoft.Windows.Shell

Namespace Taskbar
	Friend NotInheritable Class TabbedThumbnailNativeMethods
		Private Sub New()
		End Sub
		Friend Const DisplayFrame As Integer = &H1

		Friend Const ForceIconicRepresentation As Integer = 7
		Friend Const HasIconicBitmap As Integer = 10

		Friend Const WmDwmSendIconicThumbnail As UInteger = &H323
		Friend Const WmDwmSendIconicLivePreviewBitmap As UInteger = &H326

		Friend Const WaActive As UInteger = 1
		Friend Const WaClickActive As UInteger = 2

		Friend Const ScClose As Integer = &Hf060
		Friend Const ScMaximize As Integer = &Hf030
		Friend Const ScMinimize As Integer = &Hf020

		Friend Const MsgfltAdd As UInteger = 1
		Friend Const MsgfltRemove As UInteger = 2

		<DllImport("dwmapi.dll")> _
		Friend Shared Function DwmSetIconicThumbnail(hwnd As IntPtr, hbitmap As IntPtr, flags As UInteger) As Integer
		End Function

		<DllImport("dwmapi.dll")> _
		Friend Shared Function DwmInvalidateIconicBitmaps(hwnd As IntPtr) As Integer
		End Function

		<DllImport("dwmapi.dll")> _
		Friend Shared Function DwmSetIconicLivePreviewBitmap(hwnd As IntPtr, hbitmap As IntPtr, ByRef ptClient As NativePoint, flags As UInteger) As Integer
		End Function

		<DllImport("dwmapi.dll")> _
		Friend Shared Function DwmSetIconicLivePreviewBitmap(hwnd As IntPtr, hbitmap As IntPtr, ptClient As IntPtr, flags As UInteger) As Integer
		End Function

		'DWMWA_* values.
		<DllImport("dwmapi.dll", PreserveSig := True)> _
		Friend Shared Function DwmSetWindowAttribute(hwnd As IntPtr, dwAttributeToSet As UInteger, pvAttributeValue As IntPtr, cbAttribute As UInteger) As Integer
		End Function

		<DllImport("user32.dll")> _
		Friend Shared Function GetWindowRect(hwnd As IntPtr, ByRef rect As NativeRect) As <MarshalAs(UnmanagedType.Bool)> Boolean
		End Function

		<DllImport("user32.dll")> _
		Friend Shared Function GetClientRect(hwnd As IntPtr, ByRef rect As NativeRect) As <MarshalAs(UnmanagedType.Bool)> Boolean
		End Function

		Friend Shared Function GetClientSize(hwnd As IntPtr, ByRef size As System.Drawing.Size) As Boolean
			Dim rect As New NativeRect()
			If Not GetClientRect(hwnd, rect) Then
				size = New System.Drawing.Size(-1, -1)
				Return False
			End If
			size = New System.Drawing.Size(rect.Right, rect.Bottom)
			Return True
		End Function

		<DllImport("user32.dll")> _
		Friend Shared Function ClientToScreen(hwnd As IntPtr, ByRef point As NativePoint) As <MarshalAs(UnmanagedType.Bool)> Boolean
		End Function


		<DllImport("gdi32.dll")> _
		Friend Shared Function StretchBlt(hDestDC As IntPtr, destX As Integer, destY As Integer, destWidth As Integer, destHeight As Integer, hSrcDC As IntPtr, _
			srcX As Integer, srcY As Integer, srcWidth As Integer, srcHeight As Integer, operation As UInteger) As <MarshalAs(UnmanagedType.Bool)> Boolean
		End Function

		<DllImport("user32.dll")> _
		Friend Shared Function GetWindowDC(hwnd As IntPtr) As IntPtr
		End Function

		<DllImport("user32.dll")> _
		Friend Shared Function ReleaseDC(hwnd As IntPtr, hdc As IntPtr) As Integer
		End Function

		<DllImport("user32.dll", SetLastError := True)> _
		Friend Shared Function ChangeWindowMessageFilter(message As UInteger, dwFlag As UInteger) As IntPtr
		End Function

		''' <summary>
		''' Sets the specified iconic thumbnail for the specified window.
		''' This is typically done in response to a DWM message.
		''' </summary>
		''' <param name="hwnd">The window handle.</param>
		''' <param name="hBitmap">The thumbnail bitmap.</param>
		Friend Shared Sub SetIconicThumbnail(hwnd As IntPtr, hBitmap As IntPtr)
			Dim rc As Integer = DwmSetIconicThumbnail(hwnd, hBitmap, DisplayFrame)
			If rc <> 0 Then
				Throw Marshal.GetExceptionForHR(rc)
			End If
		End Sub

        ''' <summary>
        ''' Sets the specified peek (live preview) bitmap for the specified
        ''' window.  This is typically done in response to a DWM message.
        ''' </summary>
        ''' <param name="hwnd">The window handle.</param>
        ''' <param name="bitmap">The thumbnail bitmap.</param>
        ''' <param name="displayFrame__1">Whether to display a standard window
        ''' frame around the bitmap.</param>
        Friend Shared Sub SetPeekBitmap(hwnd As IntPtr, bitmap As IntPtr, displayFrame__1 As Boolean)
            Dim rc As Integer = DwmSetIconicLivePreviewBitmap(hwnd, bitmap, IntPtr.Zero, If(displayFrame__1, CUInt(DisplayFrame), CUInt(0)))
            If rc <> 0 Then
				Throw Marshal.GetExceptionForHR(rc)
			End If
		End Sub

        ''' <summary>
        ''' Sets the specified peek (live preview) bitmap for the specified
        ''' window.  This is typically done in response to a DWM message.
        ''' </summary>
        ''' <param name="hwnd">The window handle.</param>
        ''' <param name="bitmap">The thumbnail bitmap.</param>
        ''' <param name="offset">The client area offset at which to display
        ''' the specified bitmap.  The rest of the parent window will be
        ''' displayed as "remembered" by the DWM.</param>
        ''' <param name="displayFrame__1">Whether to display a standard window
        ''' frame around the bitmap.</param>
        Friend Shared Sub SetPeekBitmap(hwnd As IntPtr, bitmap As IntPtr, offset As System.Drawing.Point, displayFrame__1 As Boolean)
			Dim nativePoint = New NativePoint(offset.X, offset.Y)
            Dim rc As Integer = DwmSetIconicLivePreviewBitmap(hwnd, bitmap, nativePoint, If(displayFrame__1, CUInt(DisplayFrame), CUInt(0)))

            If rc <> 0 Then
				Dim e As Exception = Marshal.GetExceptionForHR(rc)

						' Ignore argument exception as it's not really recommended to be throwing
						' exception when rendering the peek bitmap. If it's some other kind of exception,
						' then throw it.
				If TypeOf e Is ArgumentException Then
				Else
					Throw e
				End If
			End If
		End Sub

		''' <summary>
		''' Call this method to either enable custom previews on the taskbar (second argument as true)
		''' or to disable (second argument as false). If called with True, the method will call DwmSetWindowAttribute
		''' for the specific window handle and let DWM know that we will be providing a custom bitmap for the thumbnail
		''' as well as Aero peek.
		''' </summary>
		''' <param name="hwnd"></param>
		''' <param name="enable"></param>
		Friend Shared Sub EnableCustomWindowPreview(hwnd As IntPtr, enable As Boolean)
			Dim t As IntPtr = Marshal.AllocHGlobal(4)
			Marshal.WriteInt32(t, If(enable, 1, 0))

			Try
				Dim rc As Integer = DwmSetWindowAttribute(hwnd, HasIconicBitmap, t, 4)
				If rc <> 0 Then
					Throw Marshal.GetExceptionForHR(rc)
				End If

				rc = DwmSetWindowAttribute(hwnd, ForceIconicRepresentation, t, 4)
				If rc <> 0 Then
					Throw Marshal.GetExceptionForHR(rc)
				End If
			Finally
				Marshal.FreeHGlobal(t)
			End Try
		End Sub

	End Class
End Namespace
