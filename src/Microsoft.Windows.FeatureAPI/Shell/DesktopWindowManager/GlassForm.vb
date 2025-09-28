Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.Windows.Internal


Namespace Shell
	''' <summary>
	''' Windows Glass Form
	''' Inherit from this form to be able to enable glass on Windows Form
	''' </summary>
	Public Class GlassForm
		Inherits Form
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
        ''' Makes the background of current window transparent
        ''' </summary>
        Public Sub SetAeroGlassTransparency()
			Me.BackColor = Color.Transparent
		End Sub

		''' <summary>
		''' Excludes a Control from the AeroGlass frame.
		''' </summary>
		''' <param name="control">The control to exclude.</param>
		''' <remarks>Many non-WPF rendered controls (i.e., the ExplorerBrowser control) will not 
		''' render properly on top of an AeroGlass frame. </remarks>
		Public Sub ExcludeControlFromAeroGlass(control As Control)
			If control Is Nothing Then
				Throw New ArgumentNullException("control")
			End If

			If AeroGlassCompositionEnabled Then
				Dim clientScreen As Rectangle = Me.RectangleToScreen(Me.ClientRectangle)
				Dim controlScreen As Rectangle = control.RectangleToScreen(control.ClientRectangle)

				Dim margins As New Margins()
				margins.LeftWidth = controlScreen.Left - clientScreen.Left
				margins.RightWidth = clientScreen.Right - controlScreen.Right
				margins.TopHeight = controlScreen.Top - clientScreen.Top
				margins.BottomHeight = clientScreen.Bottom - controlScreen.Bottom

				' Extend the Frame into client area
				DesktopWindowManagerNativeMethods.DwmExtendFrameIntoClientArea(Handle, margins)
			End If
		End Sub

		''' <summary>
		''' Resets the AeroGlass exclusion area.
		''' </summary>
		Public Sub ResetAeroGlass()
			If Me.Handle <> IntPtr.Zero Then
				Dim margins As New Margins(True)
				DesktopWindowManagerNativeMethods.DwmExtendFrameIntoClientArea(Me.Handle, margins)
			End If
		End Sub
		#End Region

		#Region "implementation"
		''' <summary>
		''' Catches the DWM messages to this window and fires the appropriate event.
		''' </summary>
		''' <param name="m"></param>

		Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
			If m.Msg = DWMMessages.WM_DWMCOMPOSITIONCHANGED OrElse m.Msg = DWMMessages.WM_DWMNCRENDERINGCHANGED Then
                If _AeroGlassCompositionChanged IsNot Nothing Then
                    _AeroGlassCompositionChanged.Invoke(Me, New AeroGlassCompositionChangedEventArgs(AeroGlassCompositionEnabled))
                End If
            End If

			MyBase.WndProc(m)
		End Sub

		''' <summary>
		''' Initializes the Form for AeroGlass
		''' </summary>
		''' <param name="e">The arguments for this event</param>
		Protected Overrides Sub OnLoad(e As EventArgs)
			MyBase.OnLoad(e)
			ResetAeroGlass()
		End Sub

		''' <summary>
		''' Overide OnPaint to paint the background as black.
		''' </summary>
		''' <param name="e">PaintEventArgs</param>
		Protected Overrides Sub OnPaint(e As PaintEventArgs)
			MyBase.OnPaint(e)

			If DesignMode = False Then
				If AeroGlassCompositionEnabled AndAlso e IsNot Nothing Then
					' Paint the all the regions black to enable glass
					e.Graphics.FillRectangle(Brushes.Black, Me.ClientRectangle)
				End If
			End If
		End Sub

		#End Region
	End Class
End Namespace
