Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports Microsoft.Windows.Internal
Imports System.Runtime.InteropServices
Imports System.Runtime.InteropServices.ComTypes

Namespace Shell.Interop

	Friend NotInheritable Class ShellObjectWatcherNativeMethods
		Private Sub New()
		End Sub
		' must be 0
		<DllImport("Ole32.dll")> _
		Public Shared Function CreateBindCtx(reserved As Integer, <Out> ByRef bindCtx As IBindCtx) As HResult
		End Function

		<DllImport("User32.dll", CharSet := CharSet.Unicode, SetLastError := True)> _
		Public Shared Function RegisterClassEx(ByRef windowClass As WindowClassEx) As UInteger
		End Function

		'string className, //optional
		'window name
		<DllImport("User32.dll", CharSet := CharSet.Unicode, SetLastError := True)> _
		Public Shared Function CreateWindowEx(extendedStyle As Integer, <MarshalAs(UnmanagedType.LPWStr)> className As String, <MarshalAs(UnmanagedType.LPWStr)> windowName As String, style As Integer, x As Integer, y As Integer, _
			width As Integer, height As Integer, parentHandle As IntPtr, menuHandle As IntPtr, instanceHandle As IntPtr, additionalData As IntPtr) As IntPtr
		End Function

		<DllImport("User32.dll")> _
		Public Shared Function GetMessage(<Out> ByRef message As Message, windowHandle As IntPtr, filterMinMessage As UInteger, filterMaxMessage As UInteger) As <MarshalAs(UnmanagedType.Bool)> Boolean
		End Function

		<DllImport("User32.dll")> _
		Public Shared Function DefWindowProc(hwnd As IntPtr, msg As UInteger, wparam As IntPtr, lparam As IntPtr) As Integer
		End Function

		<DllImport("User32.dll")> _
		Public Shared Sub DispatchMessage(<[In]> ByRef message As Message)
		End Sub

		Public Delegate Function WndProcDelegate(hwnd As IntPtr, msg As UInteger, wparam As IntPtr, lparam As IntPtr) As Integer
	End Class

	<StructLayout(LayoutKind.Sequential, CharSet := CharSet.Unicode)> _
	Friend Structure WindowClassEx
		Friend Size As UInteger
		Friend Style As UInteger

		Friend WndProc As ShellObjectWatcherNativeMethods.WndProcDelegate

		Friend ExtraClassBytes As Integer
		Friend ExtraWindowBytes As Integer
		Friend InstanceHandle As IntPtr
		Friend IconHandle As IntPtr
		Friend CursorHandle As IntPtr
		Friend BackgroundBrushHandle As IntPtr

		Friend MenuName As String
		Friend ClassName As String

		Friend SmallIconHandle As IntPtr
	End Structure

	''' <summary>
	''' Wraps the native Windows MSG structure.
	''' </summary>
	Public Structure Message
		Private m_windowHandle As IntPtr
		Private m_msg As UInteger
		Private m_wparam As IntPtr
		Private m_lparam As IntPtr
		Private m_time As Integer
		Private m_point As NativePoint

		''' <summary>
		''' Gets the window handle
		''' </summary>
		Public ReadOnly Property WindowHandle() As IntPtr
			Get
				Return m_windowHandle
			End Get
		End Property

		''' <summary>
		''' Gets the window message
		''' </summary>
		Public ReadOnly Property Msg() As UInteger
			Get
				Return m_msg
			End Get
		End Property

		''' <summary>
		''' Gets the WParam
		''' </summary>
		Public ReadOnly Property WParam() As IntPtr
			Get
				Return m_wparam
			End Get
		End Property

		''' <summary>
		''' Gets the LParam
		''' </summary>
		Public ReadOnly Property LParam() As IntPtr
			Get
				Return m_lparam
			End Get
		End Property

		''' <summary>
		''' Gets the time
		''' </summary>
		Public ReadOnly Property Time() As Integer
			Get
				Return m_time
			End Get
		End Property

		''' <summary>
		''' Gets the point
		''' </summary>
		Public ReadOnly Property Point() As NativePoint
			Get
				Return m_point
			End Get
		End Property

		''' <summary>
		''' Creates a new instance of the Message struct
		''' </summary>
		''' <param name="windowHandle">Window handle</param>
		''' <param name="msg">Message</param>
		''' <param name="wparam">WParam</param>
		''' <param name="lparam">LParam</param>
		''' <param name="time">Time</param>
		''' <param name="point">Point</param>
		Friend Sub New(windowHandle As IntPtr, msg As UInteger, wparam As IntPtr, lparam As IntPtr, time As Integer, point As NativePoint)
			Me.New()
			Me.m_windowHandle = windowHandle
			Me.m_msg = msg
			Me.m_wparam = wparam
			Me.m_lparam = lparam
			Me.m_time = time
			Me.m_point = point
		End Sub

		''' <summary>
		''' Determines if two messages are equal.
		''' </summary>
		''' <param name="first">First message</param>
		''' <param name="second">Second message</param>
		''' <returns>True if first and second message are equal; false otherwise.</returns>
		Public Shared Operator =(first As Message, second As Message) As Boolean
			Return first.WindowHandle = second.WindowHandle AndAlso first.Msg = second.Msg AndAlso first.WParam = second.WParam AndAlso first.LParam = second.LParam AndAlso first.Time = second.Time AndAlso first.Point = second.Point
		End Operator

		''' <summary>
		''' Determines if two messages are not equal.
		''' </summary>
		''' <param name="first">First message</param>
		''' <param name="second">Second message</param>
		''' <returns>True if first and second message are not equal; false otherwise.</returns>
		Public Shared Operator <>(first As Message, second As Message) As Boolean
			Return Not (first = second)
		End Operator

		''' <summary>
		''' Determines if this message is equal to another.
		''' </summary>
		''' <param name="obj">Another message</param>
		''' <returns>True if this message is equal argument; false otherwise.</returns>
		Public Overrides Function Equals(obj As Object) As Boolean
			Return If((obj IsNot Nothing AndAlso TypeOf obj Is Message), Me = CType(obj, Message), False)
		End Function

		''' <summary>
		''' Gets a hash code for the message.
		''' </summary>
		''' <returns>Hash code for this message.</returns>
		Public Overrides Function GetHashCode() As Integer
			Dim hash As Integer = WindowHandle.GetHashCode()
			hash = hash * 31 + Msg.GetHashCode()
			hash = hash * 31 + WParam.GetHashCode()
			hash = hash * 31 + LParam.GetHashCode()
			hash = hash * 31 + Time.GetHashCode()
			hash = hash * 31 + Point.GetHashCode()
			Return hash
		End Function
	End Structure

End Namespace
