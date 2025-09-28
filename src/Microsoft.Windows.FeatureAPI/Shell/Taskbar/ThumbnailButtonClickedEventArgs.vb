' Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Windows

Namespace Taskbar
	''' <summary>
	''' Event args for TabbedThumbnailButton.Click event
	''' </summary>
	Public Class ThumbnailButtonClickedEventArgs
		Inherits EventArgs
        ''' <summary>
        ''' Creates a Event Args for the TabbedThumbnailButton.Click event
        ''' </summary>
        ''' <param name="windowHandle__1">Window handle for the control/window related to the event</param>
        ''' <param name="button">Thumbnail toolbar button that was clicked</param>
        Public Sub New(windowHandle__1 As IntPtr, button As ThumbnailToolBarButton)
			ThumbnailButton = button
			WindowHandle = windowHandle__1
			WindowsControl = Nothing
		End Sub

        ''' <summary>
        ''' Creates a Event Args for the TabbedThumbnailButton.Click event
        ''' </summary>
        ''' <param name="windowsControl__1">WPF Control (UIElement) related to the event</param>
        ''' <param name="button">Thumbnail toolbar button that was clicked</param>
        Public Sub New(windowsControl__1 As UIElement, button As ThumbnailToolBarButton)
			ThumbnailButton = button
			WindowHandle = IntPtr.Zero
			WindowsControl = windowsControl__1
		End Sub

		''' <summary>
		''' Gets the Window handle for the specific control/window that is related to this event.
		''' </summary>
		''' <remarks>For WPF Controls (UIElement) the WindowHandle will be IntPtr.Zero. 
		''' Check the WindowsControl property to get the specific control associated with this event.</remarks>
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
		''' Gets the WPF Control (UIElement) that is related to this event. This property may be null
		''' for non-WPF applications.
		''' </summary>
		Public Property WindowsControl() As UIElement
			Get
				Return m_WindowsControl
			End Get
			Private Set
				m_WindowsControl = Value
			End Set
		End Property
		Private m_WindowsControl As UIElement

		''' <summary>
		''' Gets the ThumbnailToolBarButton that was clicked
		''' </summary>
		Public Property ThumbnailButton() As ThumbnailToolBarButton
			Get
				Return m_ThumbnailButton
			End Get
			Private Set
				m_ThumbnailButton = Value
			End Set
		End Property
		Private m_ThumbnailButton As ThumbnailToolBarButton
	End Class
End Namespace
