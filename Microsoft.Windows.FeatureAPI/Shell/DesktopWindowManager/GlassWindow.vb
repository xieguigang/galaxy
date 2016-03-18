Imports System.Windows.Interop
Imports System.Windows.Media
Imports Microsoft.Windows.Internal
Imports Microsoft.Windows.Shell.Interop
Imports System.Windows

Namespace Shell

	''' <summary>
	''' WPF Glass Window
	''' Inherit from this window class to enable glass on a WPF window
	''' </summary>
	Public Class GlassWindow
		Inherits Window
		#Region "properties"

		''' <summary>
		''' Get determines if AeroGlass is enabled on the desktop. Set enables/disables AreoGlass on the desktop.
		''' </summary>
		Public Shared Property AeroGlassCompositionEnabled() As Boolean
			Get
				Return DesktopWindowManagerNativeMethods.DwmIsCompositionEnabled()
			End Get
			Set
				DesktopWindowManagerNativeMethods.DwmEnableComposition(If(value, CompositionEnable.Enable, CompositionEnable.Disable))
			End Set
		End Property

#End Region

#Region "events"

        Dim _AeroGlassCompositionChanged As EventHandler(Of AeroGlassCompositionChangedEventArgs)

        ''' <summary>
        ''' Fires when the availability of Glass effect changes.
        ''' </summary>
        Public Custom Event AeroGlassCompositionChanged As EventHandler(Of AeroGlassCompositionChangedEventArgs)
            AddHandler(value As EventHandler(Of AeroGlassCompositionChangedEventArgs))
                _AeroGlassCompositionChanged = value
            End AddHandler
            RemoveHandler(value As EventHandler(Of AeroGlassCompositionChangedEventArgs))
                _AeroGlassCompositionChanged = Nothing
            End RemoveHandler
            RaiseEvent(sender As Object, e As AeroGlassCompositionChangedEventArgs)
                Call _AeroGlassCompositionChanged.Invoke(sender, e)
            End RaiseEvent
        End Event

#End Region

#Region "operations"

        ''' <summary>
        ''' Makes the background of current window transparent from both Wpf and Windows Perspective
        ''' </summary>
        Public Sub SetAeroGlassTransparency()
			' Set the Background to transparent from Win32 perpective 
			HwndSource.FromHwnd(windowHandle).CompositionTarget.BackgroundColor = System.Windows.Media.Colors.Transparent

			' Set the Background to transparent from WPF perpective 
			Me.Background = Brushes.Transparent
		End Sub

		''' <summary>
		''' Excludes a UI element from the AeroGlass frame.
		''' </summary>
		''' <param name="element">The element to exclude.</param>
		''' <remarks>Many non-WPF rendered controls (i.e., the ExplorerBrowser control) will not 
		''' render properly on top of an AeroGlass frame. </remarks>
		Public Sub ExcludeElementFromAeroGlass(element As FrameworkElement)
			If AeroGlassCompositionEnabled AndAlso element IsNot Nothing Then
				' calculate total size of window nonclient area
				Dim hwndSource As HwndSource = TryCast(PresentationSource.FromVisual(Me), HwndSource)
				Dim windowRect As NativeRect
				Dim clientRect As NativeRect
				DesktopWindowManagerNativeMethods.GetWindowRect(hwndSource.Handle, windowRect)
				DesktopWindowManagerNativeMethods.GetClientRect(hwndSource.Handle, clientRect)
				Dim nonClientSize As New Size(CDbl(windowRect.Right - windowRect.Left) - CDbl(clientRect.Right - clientRect.Left), CDbl(windowRect.Bottom - windowRect.Top) - CDbl(clientRect.Bottom - clientRect.Top))

				' calculate size of element relative to nonclient area
				Dim transform As GeneralTransform = element.TransformToAncestor(Me)
				Dim topLeftFrame As Point = transform.Transform(New Point(0, 0))
				Dim bottomRightFrame As Point = transform.Transform(New Point(element.ActualWidth + nonClientSize.Width, element.ActualHeight + nonClientSize.Height))

				' Create a margin structure
				Dim margins As New Margins()
				margins.LeftWidth = CInt(Math.Truncate(topLeftFrame.X))
				margins.RightWidth = CInt(Math.Truncate(Me.ActualWidth - bottomRightFrame.X))
				margins.TopHeight = CInt(Math.Truncate(topLeftFrame.Y))
				margins.BottomHeight = CInt(Math.Truncate(Me.ActualHeight - bottomRightFrame.Y))

				' Extend the Frame into client area
				DesktopWindowManagerNativeMethods.DwmExtendFrameIntoClientArea(windowHandle, margins)
			End If
		End Sub

		''' <summary>
		''' Resets the AeroGlass exclusion area.
		''' </summary>
		Public Sub ResetAeroGlass()
			Dim margins As New Margins(True)
			DesktopWindowManagerNativeMethods.DwmExtendFrameIntoClientArea(windowHandle, margins)
		End Sub

		#End Region

		#Region "implementation"
		Private windowHandle As IntPtr

		Private Function WndProc(hwnd As IntPtr, msg As Integer, wParam As IntPtr, lParam As IntPtr, ByRef handled As Boolean) As IntPtr
			If msg = DWMMessages.WM_DWMCOMPOSITIONCHANGED OrElse msg = DWMMessages.WM_DWMNCRENDERINGCHANGED Then
                If _AeroGlassCompositionChanged IsNot Nothing Then
                    _AeroGlassCompositionChanged.Invoke(Me, New AeroGlassCompositionChangedEventArgs(AeroGlassCompositionEnabled))
                End If

                handled = True
			End If
			Return IntPtr.Zero
		End Function

		''' <summary>
		''' OnSourceInitialized
		''' Override SourceInitialized to initialize windowHandle for this window.
		''' A valid windowHandle is available only after the sourceInitialized is completed
		''' </summary>
		''' <param name="e">EventArgs</param>
		Protected Overrides Sub OnSourceInitialized(e As EventArgs)
			MyBase.OnSourceInitialized(e)
			Dim interopHelper As New WindowInteropHelper(Me)
			Me.windowHandle = interopHelper.Handle

			' add Window Proc hook to capture DWM messages
			Dim source As HwndSource = HwndSource.FromHwnd(windowHandle)
			source.AddHook(New HwndSourceHook(AddressOf WndProc))

			ResetAeroGlass()
		End Sub

		#End Region
	End Class
End Namespace
