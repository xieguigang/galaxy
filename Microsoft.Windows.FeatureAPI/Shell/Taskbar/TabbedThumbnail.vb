'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Drawing
Imports System.IO
Imports System.Threading
Imports System.Windows
Imports System.Windows.Forms
Imports System.Windows.Interop
Imports System.Windows.Media.Imaging
Imports Microsoft.Windows.Shell
Imports Microsoft.Windows.Resources
Imports Microsoft.Windows.Internal

Namespace Taskbar
	''' <summary>
	''' Represents a tabbed thumbnail on the taskbar for a given window or a control.
	''' </summary>
	Public Class TabbedThumbnail
		Implements IDisposable
		#Region "Internal members"

		' Control properties
		Friend Property WindowHandle() As IntPtr
			Get
				Return m_WindowHandle
			End Get
			Set
				m_WindowHandle = Value
			End Set
		End Property
		Private m_WindowHandle As IntPtr
		Friend Property ParentWindowHandle() As IntPtr
			Get
				Return m_ParentWindowHandle
			End Get
			Set
				m_ParentWindowHandle = Value
			End Set
		End Property
		Private m_ParentWindowHandle As IntPtr

		' WPF properties
		Friend Property WindowsControl() As UIElement
			Get
				Return m_WindowsControl
			End Get
			Set
				m_WindowsControl = Value
			End Set
		End Property
		Private m_WindowsControl As UIElement
		Friend Property WindowsControlParentWindow() As Window
			Get
				Return m_WindowsControlParentWindow
			End Get
			Set
				m_WindowsControlParentWindow = Value
			End Set
		End Property
		Private m_WindowsControlParentWindow As Window

		Private _taskbarWindow As TaskbarWindow
		Friend Property TaskbarWindow() As TaskbarWindow
			Get
				Return _taskbarWindow
			End Get
			Set
				_taskbarWindow = value

				' If we have a TaskbarWindow assigned, set it's icon
				If _taskbarWindow IsNot Nothing AndAlso _taskbarWindow.TabbedThumbnailProxyWindow IsNot Nothing Then
					_taskbarWindow.TabbedThumbnailProxyWindow.Icon = Icon
				End If
			End Set
		End Property

		Private _addedToTaskbar As Boolean
		Friend Property AddedToTaskbar() As Boolean
			Get
				Return _addedToTaskbar
			End Get
			Set
				_addedToTaskbar = value

				' The user has updated the clipping region, so invalidate our existing preview
				If ClippingRectangle IsNot Nothing Then
					TaskbarWindowManager.InvalidatePreview(Me.TaskbarWindow)
				End If
			End Set
		End Property

		Friend Property RemovedFromTaskbar() As Boolean
			Get
				Return m_RemovedFromTaskbar
			End Get
			Set
				m_RemovedFromTaskbar = Value
			End Set
		End Property
		Private m_RemovedFromTaskbar As Boolean

#End Region

#Region "Constructors"

        ''' <summary>
        ''' Creates a new TabbedThumbnail with the given window handle of the parent and
        ''' a child control/window's handle (e.g. TabPage or Panel)
        ''' </summary>
        ''' <param name="parentWindowHandle__1">Window handle of the parent window. 
        ''' This window has to be a top-level window and the handle cannot be null or IntPtr.Zero</param>
        ''' <param name="windowHandle__2">Window handle of the child control or window for which a tabbed 
        ''' thumbnail needs to be displayed</param>
        Public Sub New(parentWindowHandle__1 As IntPtr, windowHandle__2 As IntPtr)
			If parentWindowHandle__1 = IntPtr.Zero Then
				Throw New ArgumentException(LocalizedMessages.TabbedThumbnailZeroParentHandle, "parentWindowHandle")
			End If
			If windowHandle__2 = IntPtr.Zero Then
				Throw New ArgumentException(LocalizedMessages.TabbedThumbnailZeroChildHandle, "windowHandle")
			End If

			WindowHandle = windowHandle__2
			ParentWindowHandle = parentWindowHandle__1
		End Sub

        ''' <summary>
        ''' Creates a new TabbedThumbnail with the given window handle of the parent and
        ''' a child control (e.g. TabPage or Panel)
        ''' </summary>
        ''' <param name="parentWindowHandle__1">Window handle of the parent window. 
        ''' This window has to be a top-level window and the handle cannot be null or IntPtr.Zero</param>
        ''' <param name="control">Child control for which a tabbed thumbnail needs to be displayed</param>
        ''' <remarks>This method can also be called when using a WindowsFormHost control in a WPF application.
        '''  Call this method with the main WPF Window's handle, and windowsFormHost.Child control.</remarks>
        Public Sub New(parentWindowHandle__1 As IntPtr, control As Control)
			If parentWindowHandle__1 = IntPtr.Zero Then
				Throw New ArgumentException(LocalizedMessages.TabbedThumbnailZeroParentHandle, "parentWindowHandle")
			End If
			If control Is Nothing Then
				Throw New ArgumentNullException("control")
			End If

			WindowHandle = control.Handle
			ParentWindowHandle = parentWindowHandle__1
		End Sub

        ''' <summary>
        ''' Creates a new TabbedThumbnail with the given window handle of the parent and
        ''' a WPF child Window. For WindowsFormHost control, use TabbedThumbnail(IntPtr, Control) overload and pass
        ''' the WindowsFormHost.Child as the second parameter.
        ''' </summary>
        ''' <param name="parentWindow">Parent window for the UIElement control. 
        ''' This window has to be a top-level window and the handle cannot be null</param>
        ''' <param name="windowsControl__1">WPF Control (UIElement) for which a tabbed thumbnail needs to be displayed</param>
        ''' <param name="peekOffset__2">Offset point used for displaying the peek bitmap. This setting is
        ''' recomended for hidden WPF controls as it is difficult to calculate their offset.</param>
        Public Sub New(parentWindow As Window, windowsControl__1 As UIElement, peekOffset__2 As Vector)
			If windowsControl__1 Is Nothing Then
				Throw New ArgumentNullException("windowsControl")
			End If
			If parentWindow Is Nothing Then
				Throw New ArgumentNullException("parentWindow")
			End If

			WindowHandle = IntPtr.Zero

			WindowsControl = windowsControl__1
			WindowsControlParentWindow = parentWindow
			ParentWindowHandle = (New WindowInteropHelper(parentWindow)).Handle
			PeekOffset = peekOffset__2
		End Sub

		#End Region

		#Region "Public Properties"

		Private _title As String = String.Empty
		''' <summary>
		''' Title for the window shown as the taskbar thumbnail.
		''' </summary>
		Public Property Title() As String
			Get
				Return _title
			End Get
			Set
				If _title <> value Then
					_title = value
					RaiseEvent TitleChanged(Me, EventArgs.Empty)
				End If
			End Set
		End Property

		Private _tooltip As String = String.Empty
		''' <summary>
		''' Tooltip to be shown for this thumbnail on the taskbar. 
		''' By default this is full title of the window shown on the taskbar.
		''' </summary>
		Public Property Tooltip() As String
			Get
				Return _tooltip
			End Get
			Set
				If _tooltip <> value Then
					_tooltip = value
					RaiseEvent TooltipChanged(Me, EventArgs.Empty)
				End If
			End Set
		End Property

        ''' <summary>
        ''' Sets the window icon for this thumbnail preview
        ''' </summary>
        ''' <param name="icon__1">System.Drawing.Icon for the window/control associated with this preview</param>
        Public Sub SetWindowIcon(icon__1 As Icon)
			Icon = icon__1

			' If we have a TaskbarWindow assigned, set its icon
			If TaskbarWindow IsNot Nothing AndAlso TaskbarWindow.TabbedThumbnailProxyWindow IsNot Nothing Then
				TaskbarWindow.TabbedThumbnailProxyWindow.Icon = Icon
			End If
		End Sub

		''' <summary>
		''' Sets the window icon for this thumbnail preview
		''' </summary>
		''' <param name="iconHandle">Icon handle (hIcon) for the window/control associated with this preview</param>
		''' <remarks>This method will not release the icon handle. It is the caller's responsibility to release the icon handle.</remarks>
		Public Sub SetWindowIcon(iconHandle As IntPtr)
			Icon = If(iconHandle <> IntPtr.Zero, System.Drawing.Icon.FromHandle(iconHandle), Nothing)

			If TaskbarWindow IsNot Nothing AndAlso TaskbarWindow.TabbedThumbnailProxyWindow IsNot Nothing Then
				TaskbarWindow.TabbedThumbnailProxyWindow.Icon = Icon
			End If
		End Sub

		Private _clippingRectangle As System.Nullable(Of Rectangle)
		''' <summary>
		''' Specifies that only a portion of the window's client area
		''' should be used in the window's thumbnail.
		''' <para>A value of null will clear the clipping area and use the default thumbnail.</para>
		''' </summary>
		Public Property ClippingRectangle() As System.Nullable(Of Rectangle)
			Get
				Return _clippingRectangle
			End Get
			Set
				_clippingRectangle = value

				' The user has updated the clipping region, so invalidate our existing preview
				TaskbarWindowManager.InvalidatePreview(Me.TaskbarWindow)
			End Set
		End Property

		Friend Property CurrentHBitmap() As IntPtr
			Get
				Return m_CurrentHBitmap
			End Get
			Set
				m_CurrentHBitmap = Value
			End Set
		End Property
		Private m_CurrentHBitmap As IntPtr

		Friend Property Icon() As Icon
			Get
				Return m_Icon
			End Get
			Private Set
				m_Icon = Value
			End Set
		End Property
		Private m_Icon As Icon

		''' <summary>
		''' Override the thumbnail and peek bitmap. 
		''' By providing this bitmap manually, Thumbnail Window manager will provide the 
		''' Desktop Window Manager (DWM) this bitmap instead of rendering one automatically.
		''' Use this property to update the bitmap whenever the control is updated and the user
		''' needs to be shown a new thumbnail on the taskbar preview (or aero peek).
		''' </summary>
		''' <param name="bitmap">The image to use.</param>
		''' <remarks>
		''' If the bitmap doesn't have the right dimensions, the DWM may scale it or not 
		''' render certain areas as appropriate - it is the user's responsibility
		''' to render a bitmap with the proper dimensions.
		''' </remarks>
		Public Sub SetImage(bitmap As Bitmap)
			If bitmap IsNot Nothing Then
				SetImage(bitmap.GetHbitmap())
			Else
				SetImage(IntPtr.Zero)
			End If
		End Sub

		''' <summary>
		''' Override the thumbnail and peek bitmap. 
		''' By providing this bitmap manually, Thumbnail Window manager will provide the 
		''' Desktop Window Manager (DWM) this bitmap instead of rendering one automatically.
		''' Use this property to update the bitmap whenever the control is updated and the user
		''' needs to be shown a new thumbnail on the taskbar preview (or aero peek).
		''' </summary>
		''' <param name="bitmapSource">The image to use.</param>
		''' <remarks>
		''' If the bitmap doesn't have the right dimensions, the DWM may scale it or not 
		''' render certain areas as appropriate - it is the user's responsibility
		''' to render a bitmap with the proper dimensions.
		''' </remarks>
		Public Sub SetImage(bitmapSource As BitmapSource)
			If bitmapSource Is Nothing Then
				SetImage(IntPtr.Zero)
				Return
			End If

			Dim encoder As New BmpBitmapEncoder()
			encoder.Frames.Add(BitmapFrame.Create(bitmapSource))

			Using memoryStream As New MemoryStream()
				encoder.Save(memoryStream)
				memoryStream.Position = 0

				Using bmp As New Bitmap(memoryStream)
					SetImage(bmp.GetHbitmap())
				End Using
			End Using
		End Sub

		''' <summary>
		''' Override the thumbnail and peek bitmap. 
		''' By providing this bitmap manually, Thumbnail Window manager will provide the 
		''' Desktop Window Manager (DWM) this bitmap instead of rendering one automatically.
		''' Use this property to update the bitmap whenever the control is updated and the user
		''' needs to be shown a new thumbnail on the taskbar preview (or aero peek).
		''' </summary>
		''' <param name="hBitmap">A bitmap handle for the image to use.
		''' <para>When the TabbedThumbnail is finalized, this class will delete the provided hBitmap.</para></param>
		''' <remarks>
		''' If the bitmap doesn't have the right dimensions, the DWM may scale it or not 
		''' render certain areas as appropriate - it is the user's responsibility
		''' to render a bitmap with the proper dimensions.
		''' </remarks>
		Friend Sub SetImage(hBitmap As IntPtr)
			' Before we set a new bitmap, dispose the old one
			If CurrentHBitmap <> IntPtr.Zero Then
				ShellNativeMethods.DeleteObject(CurrentHBitmap)
			End If

			' Set the new bitmap
			CurrentHBitmap = hBitmap

			' Let DWM know to invalidate its cached thumbnail/preview and ask us for a new one            
			TaskbarWindowManager.InvalidatePreview(TaskbarWindow)
		End Sub

		''' <summary>
		''' Specifies whether a standard window frame will be displayed
		''' around the bitmap.  If the bitmap represents a top-level window,
		''' you would probably set this flag to <b>true</b>.  If the bitmap
		''' represents a child window (or a frameless window), you would
		''' probably set this flag to <b>false</b>.
		''' </summary>
		Public Property DisplayFrameAroundBitmap() As Boolean
			Get
				Return m_DisplayFrameAroundBitmap
			End Get
			Set
				m_DisplayFrameAroundBitmap = Value
			End Set
		End Property
		Private m_DisplayFrameAroundBitmap As Boolean

		''' <summary>
		''' Invalidate any existing thumbnail preview. Calling this method
		''' will force DWM to request a new bitmap next time user previews the thumbnails
		''' or requests Aero peek preview.
		''' </summary>
		Public Sub InvalidatePreview()
			' clear current image and invalidate
			SetImage(IntPtr.Zero)
		End Sub

		''' <summary>
		''' Gets or sets the offset used for displaying the peek bitmap. This setting is
		''' recomended for hidden WPF controls as it is difficult to calculate their offset.
		''' </summary>
		Public Property PeekOffset() As System.Nullable(Of Vector)
			Get
				Return m_PeekOffset
			End Get
			Set
				m_PeekOffset = Value
			End Set
		End Property
		Private m_PeekOffset As System.Nullable(Of Vector)

		#End Region



		#Region "Events"

		''' <summary>
		''' This event is raised when the Title property changes.
		''' </summary>
		Public Event TitleChanged As EventHandler

		''' <summary>
		''' This event is raised when the Tooltip property changes.
		''' </summary>
		Public Event TooltipChanged As EventHandler

        Dim _TabbedThumbnailClosed As EventHandler(Of TabbedThumbnailClosedEventArgs)

        ''' <summary>
        ''' The event that occurs when a tab is closed on the taskbar thumbnail preview.
        ''' </summary>
        Public Custom Event TabbedThumbnailClosed As EventHandler(Of TabbedThumbnailClosedEventArgs)
            AddHandler(value As EventHandler(Of TabbedThumbnailClosedEventArgs))
                Me._TabbedThumbnailClosed = value
            End AddHandler
            RemoveHandler(value As EventHandler(Of TabbedThumbnailClosedEventArgs))
                Me._TabbedThumbnailClosed = Nothing
            End RemoveHandler
            RaiseEvent(sender As Object, e As TabbedThumbnailClosedEventArgs)
                Call _TabbedThumbnailClosed.Invoke(sender, e)
            End RaiseEvent
        End Event

        Dim _TabbedThumbnailMaximized As EventHandler(Of TabbedThumbnailEventArgs)

        ''' <summary>
        ''' The event that occurs when a tab is maximized via the taskbar thumbnail preview (context menu).
        ''' </summary>
        Public Custom Event TabbedThumbnailMaximized As EventHandler(Of TabbedThumbnailEventArgs)
            AddHandler(value As EventHandler(Of TabbedThumbnailEventArgs))
                Me._TabbedThumbnailMaximized = value
            End AddHandler
            RemoveHandler(value As EventHandler(Of TabbedThumbnailEventArgs))
                Me._TabbedThumbnailMaximized = Nothing
            End RemoveHandler
            RaiseEvent(sender As Object, e As TabbedThumbnailEventArgs)
                Call Me._TabbedThumbnailMaximized.Invoke(sender, e)
            End RaiseEvent
        End Event

        Dim _TabbedThumbnailMinimized As EventHandler(Of TabbedThumbnailEventArgs)

        ''' <summary>
        ''' The event that occurs when a tab is minimized via the taskbar thumbnail preview (context menu).
        ''' </summary>
        Public Custom Event TabbedThumbnailMinimized As EventHandler(Of TabbedThumbnailEventArgs)
            AddHandler(value As EventHandler(Of TabbedThumbnailEventArgs))
                Me._TabbedThumbnailMinimized = value
            End AddHandler
            RemoveHandler(value As EventHandler(Of TabbedThumbnailEventArgs))
                Me._TabbedThumbnailMinimized = Nothing
            End RemoveHandler
            RaiseEvent(sender As Object, e As TabbedThumbnailEventArgs)
                Call Me._TabbedThumbnailMinimized.Invoke(Nothing, Nothing)
            End RaiseEvent
        End Event

        Dim _TabbedThumbnailActivated As EventHandler(Of TabbedThumbnailEventArgs)

        ''' <summary>
        ''' The event that occurs when a tab is activated (clicked) on the taskbar thumbnail preview.
        ''' </summary>
        Public Custom Event TabbedThumbnailActivated As EventHandler(Of TabbedThumbnailEventArgs)
            AddHandler(value As EventHandler(Of TabbedThumbnailEventArgs))
                Me._TabbedThumbnailActivated = value
            End AddHandler
            RemoveHandler(value As EventHandler(Of TabbedThumbnailEventArgs))
                Me._TabbedThumbnailActivated = Nothing
            End RemoveHandler
            RaiseEvent(sender As Object, e As TabbedThumbnailEventArgs)
                Call Me._TabbedThumbnailActivated.Invoke(sender, e)
            End RaiseEvent
        End Event

        ''' <summary>
        ''' The event that occurs when a thumbnail or peek bitmap is requested by the user.
        ''' </summary>
        Public Event TabbedThumbnailBitmapRequested As EventHandler(Of TabbedThumbnailBitmapRequestedEventArgs)


		Friend Sub OnTabbedThumbnailMaximized()
            If _TabbedThumbnailMaximized IsNot Nothing Then
                RaiseEvent TabbedThumbnailMaximized(Me, GetTabbedThumbnailEventArgs())
            Else
                ' No one is listening to these events.
                ' Forward the message to the main window
                CoreNativeMethods.SendMessage(ParentWindowHandle, WindowMessage.SystemCommand, New IntPtr(TabbedThumbnailNativeMethods.ScMaximize), IntPtr.Zero)
			End If
		End Sub

		Friend Sub OnTabbedThumbnailMinimized()
            If _TabbedThumbnailMinimized IsNot Nothing Then
                RaiseEvent TabbedThumbnailMinimized(Me, GetTabbedThumbnailEventArgs())
            Else
                ' No one is listening to these events.
                ' Forward the message to the main window
                CoreNativeMethods.SendMessage(ParentWindowHandle, WindowMessage.SystemCommand, New IntPtr(TabbedThumbnailNativeMethods.ScMinimize), IntPtr.Zero)
			End If

		End Sub

		''' <summary>
		''' Returns true if the thumbnail was removed from the taskbar; false if it was not.
		''' </summary>
		''' <returns>Returns true if the thumbnail was removed from the taskbar; false if it was not.</returns>
		Friend Function OnTabbedThumbnailClosed() As Boolean
            Dim closedHandler As EventHandler(Of TabbedThumbnailClosedEventArgs) = _TabbedThumbnailClosed
            If closedHandler IsNot Nothing Then
				Dim closingEvent = GetTabbedThumbnailClosingEventArgs()

				closedHandler(Me, closingEvent)

				If closingEvent.Cancel Then
					Return False
				End If
			Else
				' No one is listening to these events. Forward the message to the main window
				CoreNativeMethods.SendMessage(ParentWindowHandle, WindowMessage.NCDestroy, IntPtr.Zero, IntPtr.Zero)
			End If

			' Remove it from the internal list as well as the taskbar
			TaskbarManager.Instance.TabbedThumbnail.RemoveThumbnailPreview(Me)
			Return True
		End Function

		Friend Sub OnTabbedThumbnailActivated()
            If _TabbedThumbnailActivated IsNot Nothing Then
                RaiseEvent TabbedThumbnailActivated(Me, GetTabbedThumbnailEventArgs())
            Else
                ' No one is listening to these events.
                ' Forward the message to the main window
                CoreNativeMethods.SendMessage(ParentWindowHandle, WindowMessage.ActivateApplication, New IntPtr(1), New IntPtr(Thread.CurrentThread.GetHashCode()))
			End If
		End Sub

        Friend Sub OnTabbedThumbnailBitmapRequested()
            Dim eventArgs As TabbedThumbnailBitmapRequestedEventArgs = Nothing

            If Me.WindowHandle <> IntPtr.Zero Then
                eventArgs = New TabbedThumbnailBitmapRequestedEventArgs(Me.WindowHandle)
            ElseIf Me.WindowsControl IsNot Nothing Then
                eventArgs = New TabbedThumbnailBitmapRequestedEventArgs(Me.WindowsControl)
            End If

            RaiseEvent TabbedThumbnailBitmapRequested(Me, eventArgs)
        End Sub

        Private Function GetTabbedThumbnailClosingEventArgs() As TabbedThumbnailClosedEventArgs
			Dim eventArgs As TabbedThumbnailClosedEventArgs = Nothing

			If Me.WindowHandle <> IntPtr.Zero Then
				eventArgs = New TabbedThumbnailClosedEventArgs(Me.WindowHandle)
			ElseIf Me.WindowsControl IsNot Nothing Then
				eventArgs = New TabbedThumbnailClosedEventArgs(Me.WindowsControl)
			End If

			Return eventArgs
		End Function

		Private Function GetTabbedThumbnailEventArgs() As TabbedThumbnailEventArgs
			Dim eventArgs As TabbedThumbnailEventArgs = Nothing

			If Me.WindowHandle <> IntPtr.Zero Then
				eventArgs = New TabbedThumbnailEventArgs(Me.WindowHandle)
			ElseIf Me.WindowsControl IsNot Nothing Then
				eventArgs = New TabbedThumbnailEventArgs(Me.WindowsControl)
			End If

			Return eventArgs
		End Function

		#End Region

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

		''' <summary>
		''' Release the native objects.
		''' </summary>
		''' <param name="disposing"></param>
		Protected Overridable Sub Dispose(disposing As Boolean)
			If disposing Then
				_taskbarWindow = Nothing

				If Icon IsNot Nothing Then
					Icon.Dispose()
				End If
				Icon = Nothing

				_title = Nothing
				_tooltip = Nothing
				WindowsControl = Nothing
			End If

			If CurrentHBitmap <> IntPtr.Zero Then
				ShellNativeMethods.DeleteObject(CurrentHBitmap)
				CurrentHBitmap = IntPtr.Zero
			End If
		End Sub

		#End Region
	End Class
End Namespace
