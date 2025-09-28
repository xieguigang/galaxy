'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Windows.Forms
Imports Microsoft.Windows.Resources
Imports Microsoft.Windows.Internal

Namespace Taskbar
	Friend Class ThumbnailToolbarProxyWindow
		Inherits NativeWindow
		Implements IDisposable
		Private _thumbnailButtons As ThumbnailToolBarButton()
		Private _internalWindowHandle As IntPtr

		Friend Property WindowsControl() As System.Windows.UIElement
			Get
				Return m_WindowsControl
			End Get
			Set
				m_WindowsControl = Value
			End Set
		End Property
		Private m_WindowsControl As System.Windows.UIElement

		Friend ReadOnly Property WindowToTellTaskbarAbout() As IntPtr
			Get
				Return If(_internalWindowHandle <> IntPtr.Zero, _internalWindowHandle, Me.Handle)
			End Get
		End Property

		Friend Property TaskbarWindow() As TaskbarWindow
			Get
				Return m_TaskbarWindow
			End Get
			Set
				m_TaskbarWindow = Value
			End Set
		End Property
		Private m_TaskbarWindow As TaskbarWindow

		Friend Sub New(windowHandle As IntPtr, buttons As ThumbnailToolBarButton())
			If windowHandle = IntPtr.Zero Then
				Throw New ArgumentException(LocalizedMessages.CommonFileDialogInvalidHandle, "windowHandle")
			End If
			If buttons IsNot Nothing AndAlso buttons.Length = 0 Then
				Throw New ArgumentException(LocalizedMessages.ThumbnailToolbarManagerNullEmptyArray, "buttons")
			End If

			_internalWindowHandle = windowHandle
			_thumbnailButtons = buttons

			' Set the window handle on the buttons (for future updates)
			Array.ForEach(_thumbnailButtons, New Action(Of ThumbnailToolBarButton)(AddressOf UpdateHandle))

			' Assign the window handle (coming from the user) to this native window
			' so we can intercept the window messages sent from the taskbar to this window.
			Me.AssignHandle(windowHandle)
		End Sub

		Friend Sub New(windowsControl__1 As System.Windows.UIElement, buttons As ThumbnailToolBarButton())
			If windowsControl__1 Is Nothing Then
				Throw New ArgumentNullException("windowsControl")
			End If
			If buttons IsNot Nothing AndAlso buttons.Length = 0 Then
				Throw New ArgumentException(LocalizedMessages.ThumbnailToolbarManagerNullEmptyArray, "buttons")
			End If

			_internalWindowHandle = IntPtr.Zero
			WindowsControl = windowsControl__1
			_thumbnailButtons = buttons

			' Set the window handle on the buttons (for future updates)
			Array.ForEach(_thumbnailButtons, New Action(Of ThumbnailToolBarButton)(AddressOf UpdateHandle))
		End Sub

		Private Sub UpdateHandle(button As ThumbnailToolBarButton)
			button.WindowHandle = _internalWindowHandle
			button.AddedToTaskbar = False
		End Sub

		Protected Overrides Sub WndProc(ByRef m As Message)
			Dim handled As Boolean = False

			handled = TaskbarWindowManager.DispatchMessage(m, Me.TaskbarWindow)

			' If it's a WM_Destroy message, then also forward it to the base class (our native window)
			If (m.Msg = CInt(WindowMessage.Destroy)) OrElse (m.Msg = CInt(WindowMessage.NCDestroy)) OrElse ((m.Msg = CInt(WindowMessage.SystemCommand)) AndAlso (CInt(m.WParam) = TabbedThumbnailNativeMethods.ScClose)) Then
				MyBase.WndProc(m)
			ElseIf Not handled Then
				MyBase.WndProc(m)
			End If
		End Sub

		#Region "IDisposable Members"

		''' <summary>
		''' 
		''' </summary>
		Protected Overrides Sub Finalize()
			Try
				Dispose(False)
			Finally
				MyBase.Finalize()
			End Try
		End Sub

		''' <summary>
		''' Release the native objects.
		''' </summary>
		Public Sub Dispose() Implements IDisposable.Dispose
			Dispose(True)
			GC.SuppressFinalize(Me)
		End Sub

		Public Sub Dispose(disposing As Boolean)
			If disposing Then
				' Dispose managed resources

				' Don't dispose the thumbnail buttons
				' as they might be used in another window.
				' Setting them to null will indicate we don't need use anymore.
				_thumbnailButtons = Nothing
			End If
		End Sub

		#End Region

	End Class
End Namespace
