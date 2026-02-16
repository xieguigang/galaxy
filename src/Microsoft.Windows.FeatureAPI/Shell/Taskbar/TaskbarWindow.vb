' Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Windows

Namespace Taskbar
	Friend Class TaskbarWindow
		Implements IDisposable
		Friend Property TabbedThumbnailProxyWindow() As TabbedThumbnailProxyWindow
			Get
				Return m_TabbedThumbnailProxyWindow
			End Get
			Set
				m_TabbedThumbnailProxyWindow = Value
			End Set
		End Property
		Private m_TabbedThumbnailProxyWindow As TabbedThumbnailProxyWindow

		Friend Property ThumbnailToolbarProxyWindow() As ThumbnailToolbarProxyWindow
			Get
				Return m_ThumbnailToolbarProxyWindow
			End Get
			Set
				m_ThumbnailToolbarProxyWindow = Value
			End Set
		End Property
		Private m_ThumbnailToolbarProxyWindow As ThumbnailToolbarProxyWindow

		Friend Property EnableTabbedThumbnails() As Boolean
			Get
				Return m_EnableTabbedThumbnails
			End Get
			Set
				m_EnableTabbedThumbnails = Value
			End Set
		End Property
		Private m_EnableTabbedThumbnails As Boolean

		Friend Property EnableThumbnailToolbars() As Boolean
			Get
				Return m_EnableThumbnailToolbars
			End Get
			Set
				m_EnableThumbnailToolbars = Value
			End Set
		End Property
		Private m_EnableThumbnailToolbars As Boolean

		Friend Property UserWindowHandle() As IntPtr
			Get
				Return m_UserWindowHandle
			End Get
			Set
				m_UserWindowHandle = Value
			End Set
		End Property
		Private m_UserWindowHandle As IntPtr

		Friend Property WindowsControl() As UIElement
			Get
				Return m_WindowsControl
			End Get
			Set
				m_WindowsControl = Value
			End Set
		End Property
		Private m_WindowsControl As UIElement

		Private _tabbedThumbnailPreview As TabbedThumbnail
		Friend Property TabbedThumbnail() As TabbedThumbnail
			Get
				Return _tabbedThumbnailPreview
			End Get
			Set
				If _tabbedThumbnailPreview IsNot Nothing Then
					Throw New InvalidOperationException(GlobalLocalizedMessages.TaskbarWindowValueSet)
				End If

				TabbedThumbnailProxyWindow = New TabbedThumbnailProxyWindow(value)
				_tabbedThumbnailPreview = value
				_tabbedThumbnailPreview.TaskbarWindow = Me
			End Set
		End Property

		Private _thumbnailButtons As ThumbnailToolBarButton()
		Friend Property ThumbnailButtons() As ThumbnailToolBarButton()
			Get
				Return _thumbnailButtons
			End Get
			Set
				_thumbnailButtons = value
				UpdateHandles()
			End Set
		End Property

		Private Sub UpdateHandles()
			For Each button As ThumbnailToolBarButton In _thumbnailButtons
				button.WindowHandle = WindowToTellTaskbarAbout
				button.AddedToTaskbar = False
			Next
		End Sub


		' TODO: Verify the logic of this property. There are situations where this will throw InvalidOperationException when it shouldn't.
		Friend ReadOnly Property WindowToTellTaskbarAbout() As IntPtr
			Get
				If EnableThumbnailToolbars AndAlso Not EnableTabbedThumbnails AndAlso ThumbnailToolbarProxyWindow IsNot Nothing Then
					Return ThumbnailToolbarProxyWindow.WindowToTellTaskbarAbout
				ElseIf Not EnableThumbnailToolbars AndAlso EnableTabbedThumbnails AndAlso TabbedThumbnailProxyWindow IsNot Nothing Then
					Return TabbedThumbnailProxyWindow.WindowToTellTaskbarAbout
				' Bug: What should happen when TabedThumbnailProxyWindow IS null, but it is enabled?
				' This occurs during the TabbedThumbnailProxyWindow constructor at line 31.   
				ElseIf EnableTabbedThumbnails AndAlso EnableThumbnailToolbars AndAlso TabbedThumbnailProxyWindow IsNot Nothing Then
					Return TabbedThumbnailProxyWindow.WindowToTellTaskbarAbout
				End If

				Throw New InvalidOperationException()
			End Get
		End Property

		Friend Sub SetTitle(title As String)
			If TabbedThumbnailProxyWindow Is Nothing Then
				Throw New InvalidOperationException(GlobalLocalizedMessages.TasbarWindowProxyWindowSet)
			End If
			TabbedThumbnailProxyWindow.Text = title
		End Sub

		Friend Sub New(userWindowHandle__1 As IntPtr, ParamArray buttons As ThumbnailToolBarButton())
			If userWindowHandle__1 = IntPtr.Zero Then
				Throw New ArgumentException(GlobalLocalizedMessages.CommonFileDialogInvalidHandle, "userWindowHandle")
			End If

			If buttons Is Nothing OrElse buttons.Length = 0 Then
				Throw New ArgumentException(GlobalLocalizedMessages.TaskbarWindowEmptyButtonArray, "buttons")
			End If

			' Create our proxy window
			ThumbnailToolbarProxyWindow = New ThumbnailToolbarProxyWindow(userWindowHandle__1, buttons)
			ThumbnailToolbarProxyWindow.TaskbarWindow = Me

			' Set our current state
			EnableThumbnailToolbars = True
			EnableTabbedThumbnails = False

			'
			Me.ThumbnailButtons = buttons
			UserWindowHandle = userWindowHandle__1
			WindowsControl = Nothing
		End Sub

		Friend Sub New(windowsControl__1 As System.Windows.UIElement, ParamArray buttons As ThumbnailToolBarButton())
			If windowsControl__1 Is Nothing Then
				Throw New ArgumentNullException("windowsControl")
			End If

			If buttons Is Nothing OrElse buttons.Length = 0 Then
				Throw New ArgumentException(GlobalLocalizedMessages.TaskbarWindowEmptyButtonArray, "buttons")
			End If

			' Create our proxy window
			ThumbnailToolbarProxyWindow = New ThumbnailToolbarProxyWindow(windowsControl__1, buttons)
			ThumbnailToolbarProxyWindow.TaskbarWindow = Me

			' Set our current state
			EnableThumbnailToolbars = True
			EnableTabbedThumbnails = False

			Me.ThumbnailButtons = buttons
			UserWindowHandle = IntPtr.Zero
			WindowsControl = windowsControl__1
		End Sub

		Friend Sub New(preview As TabbedThumbnail)
			If preview Is Nothing Then
				Throw New ArgumentNullException("preview")
			End If

			' Create our proxy window
			' Bug: This is only called in this constructor.  Which will cause the property 
			' to fail if TaskbarWindow is initialized from a different constructor.
			TabbedThumbnailProxyWindow = New TabbedThumbnailProxyWindow(preview)

			' set our current state
			EnableThumbnailToolbars = False
			EnableTabbedThumbnails = True

			' copy values
			UserWindowHandle = preview.WindowHandle
			WindowsControl = preview.WindowsControl
			TabbedThumbnail = preview
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
				If _tabbedThumbnailPreview IsNot Nothing Then
					_tabbedThumbnailPreview.Dispose()
				End If
				_tabbedThumbnailPreview = Nothing

				If ThumbnailToolbarProxyWindow IsNot Nothing Then
					ThumbnailToolbarProxyWindow.Dispose()
				End If
				ThumbnailToolbarProxyWindow = Nothing

				If TabbedThumbnailProxyWindow IsNot Nothing Then
					TabbedThumbnailProxyWindow.Dispose()
				End If
				TabbedThumbnailProxyWindow = Nothing

				' Don't dispose the thumbnail buttons as they might be used in another window.
				' Setting them to null will indicate we don't need use anymore.
				_thumbnailButtons = Nothing
			End If
		End Sub

		#End Region
	End Class
End Namespace
