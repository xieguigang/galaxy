Imports System.Collections.Generic
Imports System.Runtime.InteropServices
Imports System.Threading
Imports Microsoft.Windows.Internal
Imports Microsoft.Windows.Shell.Interop
Imports Microsoft.Windows.Resources

Namespace Shell
	Friend Class MessageListener
		Implements IDisposable
		Public Const CreateWindowMessage As UInteger = CUInt(WindowMessage.User) + 1
		Public Const DestroyWindowMessage As UInteger = CUInt(WindowMessage.User) + 2
		Public Const BaseUserMessage As UInteger = CUInt(WindowMessage.User) + 5

		Private Const MessageWindowClassName As String = "MessageListenerClass"

		Private Shared ReadOnly _threadlock As New Object()
		Private Shared _atom As UInteger
		Private Shared _windowThread As Thread = Nothing
		Private Shared _running As Boolean = False

        Private Shared _wndProc As ShellObjectWatcherNativeMethods.WndProcDelegate =
            New ShellObjectWatcherNativeMethods.WndProcDelegate(AddressOf MessageListener.WndProc)
        ' Dictionary relating window's hwnd to its message window
        Private Shared _listeners As New Dictionary(Of IntPtr, MessageListener)()
		Private Shared _firstWindowHandle As IntPtr = IntPtr.Zero

		Private Shared ReadOnly _crossThreadWindowLock As New Object()
		Private Shared _tempHandle As IntPtr = IntPtr.Zero

        Friend _MessageReceived As EventHandler(Of WindowMessageEventArgs)

        Public Custom Event MessageReceived As EventHandler(Of WindowMessageEventArgs)
            AddHandler(value As EventHandler(Of WindowMessageEventArgs))
                _MessageReceived = value
            End AddHandler
            RemoveHandler(value As EventHandler(Of WindowMessageEventArgs))
                _MessageReceived = Nothing
            End RemoveHandler
            RaiseEvent(sender As Object, e As WindowMessageEventArgs)
                _MessageReceived.Invoke(sender, e)
            End RaiseEvent
        End Event

        Public Sub New()
			SyncLock _threadlock
				If _windowThread Is Nothing Then
					_windowThread = New Thread(AddressOf ThreadMethod)
					_windowThread.SetApartmentState(ApartmentState.STA)
					_windowThread.Name = "ShellObjectWatcherMessageListenerHelperThread"

					SyncLock _crossThreadWindowLock
						_windowThread.Start()
						Monitor.Wait(_crossThreadWindowLock)
					End SyncLock

					_firstWindowHandle = WindowHandle
				Else
					CrossThreadCreateWindow()
				End If

				If WindowHandle = IntPtr.Zero Then
					Throw New ShellException(LocalizedMessages.MessageListenerCannotCreateWindow, Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error()))
				End If

				_listeners.Add(WindowHandle, Me)
			End SyncLock
		End Sub

		Private Sub CrossThreadCreateWindow()
			If _firstWindowHandle = IntPtr.Zero Then
				Throw New InvalidOperationException(LocalizedMessages.MessageListenerNoWindowHandle)
			End If

			SyncLock _crossThreadWindowLock
				CoreNativeMethods.PostMessage(_firstWindowHandle, CType(CreateWindowMessage, WindowMessage), IntPtr.Zero, IntPtr.Zero)
				Monitor.Wait(_crossThreadWindowLock)
			End SyncLock

			WindowHandle = _tempHandle
		End Sub

		Private Shared Sub RegisterWindowClass()
			Dim classEx As New WindowClassEx()
			classEx.ClassName = MessageWindowClassName
            classEx.WndProc = _wndProc

            classEx.Size = CUInt(Marshal.SizeOf(GetType(WindowClassEx)))

			Dim atom = ShellObjectWatcherNativeMethods.RegisterClassEx(classEx)
			If atom = 0 Then
				Throw New ShellException(LocalizedMessages.MessageListenerClassNotRegistered, Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error()))
			End If
			_atom = atom
		End Sub

		Private Shared Function CreateWindow() As IntPtr
			'extended style
			'class name
			'title
			'style
			' x,y,width,height
			' -3 = Message-Only window
			Dim handle As IntPtr = ShellObjectWatcherNativeMethods.CreateWindowEx(0, MessageWindowClassName, "MessageListenerWindow", 0, 0, 0, _
				0, 0, New IntPtr(-3), IntPtr.Zero, IntPtr.Zero, IntPtr.Zero)

			Return handle
		End Function

		Private Sub ThreadMethod()
		' Message Loop
			SyncLock _crossThreadWindowLock
				_running = True
				If _atom = 0 Then
					RegisterWindowClass()
				End If
				WindowHandle = CreateWindow()

				Monitor.Pulse(_crossThreadWindowLock)
			End SyncLock

			While _running
				Dim msg As Message
				If ShellObjectWatcherNativeMethods.GetMessage(msg, IntPtr.Zero, 0, 0) Then
					ShellObjectWatcherNativeMethods.DispatchMessage(msg)
				End If
			End While
		End Sub

		Private Shared Function WndProc(hwnd As IntPtr, msg As UInteger, wparam As IntPtr, lparam As IntPtr) As Integer
			Select Case msg
				Case CreateWindowMessage
					SyncLock _crossThreadWindowLock
						_tempHandle = CreateWindow()
						Monitor.Pulse(_crossThreadWindowLock)
					End SyncLock
					Exit Select
				Case CUInt(WindowMessage.Destroy)
					Exit Select
				Case Else
					Dim listener As MessageListener
					If _listeners.TryGetValue(hwnd, listener) Then
						Dim message As New Message(hwnd, msg, wparam, lparam, 0, New NativePoint())
                        Call listener._MessageReceived(listener, New WindowMessageEventArgs(message))
                    End If
					Exit Select
			End Select

			Return ShellObjectWatcherNativeMethods.DefWindowProc(hwnd, msg, wparam, lparam)
		End Function

		Public Property WindowHandle() As IntPtr
			Get
				Return m_WindowHandle
			End Get
			Private Set
				m_WindowHandle = Value
			End Set
		End Property
		Private m_WindowHandle As IntPtr
		Public Shared ReadOnly Property Running() As Boolean
			Get
				Return _running
			End Get
		End Property

		#Region "IDisposable Members"

		Protected Overrides Sub Finalize()
			Try
				Dispose(False)
			Finally
				MyBase.Finalize()
			End Try
		End Sub

		Public Sub Dispose() Implements IDisposable.Dispose
			Dispose(True)
			GC.SuppressFinalize(Me)
		End Sub

		Protected Overridable Sub Dispose(disposing As Boolean)
			If disposing Then
				SyncLock _threadlock
					_listeners.Remove(WindowHandle)
					If _listeners.Count = 0 Then
						CoreNativeMethods.PostMessage(WindowHandle, WindowMessage.Destroy, IntPtr.Zero, IntPtr.Zero)
					End If
				End SyncLock
			End If
		End Sub

		#End Region
	End Class


	''' <summary>
	''' Encapsulates the data about a window message 
	''' </summary>
	Public Class WindowMessageEventArgs
		Inherits EventArgs
		''' <summary>
		''' Received windows message.
		''' </summary>
		Public Property Message() As Message
			Get
				Return m_Message
			End Get
			Private Set
				m_Message = Value
			End Set
		End Property
		Private m_Message As Message

		Friend Sub New(msg As Message)
			Message = msg
		End Sub
	End Class


End Namespace
