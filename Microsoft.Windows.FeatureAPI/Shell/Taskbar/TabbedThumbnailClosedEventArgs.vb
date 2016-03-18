Imports System.Windows

Namespace Taskbar
	''' <summary>
	''' Event args for when close is selected on a tabbed thumbnail proxy window.
	''' </summary>
	Public Class TabbedThumbnailClosedEventArgs
		Inherits TabbedThumbnailEventArgs
		''' <summary>
		''' Creates a Event Args for a specific tabbed thumbnail event.
		''' </summary>
		''' <param name="windowHandle">Window handle for the control/window related to the event</param>        
		Public Sub New(windowHandle As IntPtr)
			MyBase.New(windowHandle)
		End Sub

		''' <summary>
		''' Creates a Event Args for a specific tabbed thumbnail event.
		''' </summary>
		''' <param name="windowsControl">WPF Control (UIElement) related to the event</param>        
		Public Sub New(windowsControl As UIElement)
			MyBase.New(windowsControl)
		End Sub

		''' <summary>
		''' If set to true, the proxy window will not be removed from the taskbar.
		''' </summary>
		Public Property Cancel() As Boolean
			Get
				Return m_Cancel
			End Get
			Set
				m_Cancel = Value
			End Set
		End Property
		Private m_Cancel As Boolean

	End Class
End Namespace
