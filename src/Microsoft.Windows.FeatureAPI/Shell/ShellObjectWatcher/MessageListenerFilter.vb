Imports System.Collections.Generic
Imports System.Linq

Namespace Shell
	Friend NotInheritable Class MessageListenerFilter
		Private Sub New()
		End Sub
		Private Shared ReadOnly _registerLock As New Object()
		Private Shared _packages As New List(Of RegisteredListener)()

		Public Shared Function Register(callback As Action(Of WindowMessageEventArgs)) As MessageListenerFilterRegistrationResult
			SyncLock _registerLock
				Dim message As UInteger = 0
				Dim package = _packages.FirstOrDefault(Function(x) x.TryRegister(callback, message))
				If package Is Nothing Then
					package = New RegisteredListener()
					If Not package.TryRegister(callback, message) Then
						' this should never happen
						Throw New ShellException(LocalizedMessages.MessageListenerFilterUnableToRegister)
					End If
					_packages.Add(package)
				End If

				Return New MessageListenerFilterRegistrationResult(package.Listener.WindowHandle, message)
			End SyncLock
		End Function

		Public Shared Sub Unregister(listenerHandle As IntPtr, message As UInteger)
			SyncLock _registerLock
				Dim package = _packages.FirstOrDefault(Function(x) x.Listener.WindowHandle = listenerHandle)
				If package Is Nothing OrElse Not package.Callbacks.Remove(message) Then
					Throw New ArgumentException(LocalizedMessages.MessageListenerFilterUnknownListenerHandle)
				End If

				If package.Callbacks.Count = 0 Then
					package.Listener.Dispose()
					_packages.Remove(package)
				End If
			End SyncLock
		End Sub

        Private Class RegisteredListener
            Public Property Callbacks() As Dictionary(Of UInteger, Action(Of WindowMessageEventArgs))
                Get
                    Return m_Callbacks
                End Get
                Private Set
                    m_Callbacks = Value
                End Set
            End Property
            Private m_Callbacks As Dictionary(Of UInteger, Action(Of WindowMessageEventArgs))

            Public Property Listener() As MessageListener
                Get
                    Return m_Listener
                End Get
                Private Set
                    m_Listener = Value
                End Set
            End Property
            Private m_Listener As MessageListener

            Public Sub New()
                Callbacks = New Dictionary(Of UInteger, Action(Of WindowMessageEventArgs))()
                Listener = New MessageListener()
                AddHandler Listener.MessageReceived, AddressOf MessageReceived
            End Sub

            Private Sub MessageReceived(sender As Object, e As WindowMessageEventArgs)
                Dim action As Action(Of WindowMessageEventArgs)
                If Callbacks.TryGetValue(e.Message.Msg, action) Then
                    action(e)
                End If
            End Sub

            Private _lastMessage As UInteger = MessageListener.BaseUserMessage
            Public Function TryRegister(callback As Action(Of WindowMessageEventArgs), ByRef message As UInteger) As Boolean
                message = 0
                If Callbacks.Count < UShort.MaxValue - MessageListener.BaseUserMessage Then
                    Dim i As UInteger = _lastMessage + 1UI
                    While i <> _lastMessage
                        If i > UShort.MaxValue Then
                            i = MessageListener.BaseUserMessage
                        End If

                        If Not Callbacks.ContainsKey(i) Then
							_lastMessage = message.InlineCopy(i)
							Callbacks.Add(i, callback)
                            Return True
                        End If
                        i += 1UI
                    End While
                End If
                Return False
            End Function
        End Class
    End Class

	''' <summary>
	''' The result of registering with the MessageListenerFilter
	''' </summary>
	Friend Class MessageListenerFilterRegistrationResult
		Friend Sub New(handle As IntPtr, msg As UInteger)
			WindowHandle = handle
			Message = msg
		End Sub

		''' <summary>
		''' Gets the window handle to which the callback was registered.
		''' </summary>
		Public Property WindowHandle() As IntPtr
			Get
				Return m_WindowHandle
			End Get
			Private Set
				m_WindowHandle = Value
			End Set
		End Property
		Private m_WindowHandle As IntPtr

		''' <summary>
		''' Gets the message for which the callback was registered.
		''' </summary>
		Public Property Message() As UInteger
			Get
				Return m_Message
			End Get
			Private Set
				m_Message = Value
			End Set
		End Property
		Private m_Message As UInteger
	End Class


End Namespace
