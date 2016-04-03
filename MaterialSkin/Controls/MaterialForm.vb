Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing
Imports System.Drawing.Text
Imports System.Linq
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.Windows.Forms

Namespace Controls
	'Microsoft.VisualBasic.Windows.Forms.
	Public Class MaterialForm
		Inherits Microsoft.VisualBasic.Windows.Forms.Form
		Implements IMaterialControl
		<Browsable(False)> _
		Public Property Depth() As Integer Implements IMaterialControl.Depth
			Get
				Return m_Depth
			End Get
			Set
				m_Depth = Value
			End Set
		End Property
		Private m_Depth As Integer
		<Browsable(False)> _
		Public ReadOnly Property SkinManager() As MaterialSkinManager Implements IMaterialControl.SkinManager
			Get
				Return MaterialSkinManager.Instance
			End Get
		End Property
		<Browsable(False)> _
		Public Property MouseState() As MouseState Implements IMaterialControl.MouseState
			Get
				Return m_MouseState
			End Get
			Set
				m_MouseState = Value
			End Set
		End Property
		Private m_MouseState As MouseState
		Public Shadows Property FormBorderStyle() As FormBorderStyle
			Get
				Return MyBase.FormBorderStyle
			End Get
			Set
				MyBase.FormBorderStyle = value
			End Set
		End Property
		Public Property Sizable() As Boolean
			Get
				Return m_Sizable
			End Get
			Set
				m_Sizable = Value
			End Set
		End Property
		Private m_Sizable As Boolean

        <DllImport("user32.dll")> _
		Public Shared Function TrackPopupMenuEx(hmenu As IntPtr, fuFlags As UInteger, x As Integer, y As Integer, hwnd As IntPtr, lptpm As IntPtr) As Integer
		End Function

		<DllImport("user32.dll")> _
		Public Shared Function GetSystemMenu(hWnd As IntPtr, bRevert As Boolean) As IntPtr
		End Function

		<DllImport("user32.dll")> _
		Public Shared Function MonitorFromWindow(hwnd As IntPtr, dwFlags As UInteger) As IntPtr
		End Function

		<DllImport("User32.dll", CharSet := CharSet.Auto)> _
		Public Shared Function GetMonitorInfo(hmonitor As HandleRef, <[In], Out> info As MONITORINFOEX) As Boolean
		End Function

		Public Const WM_NCLBUTTONDOWN As Integer = &Ha1
		Public Const HT_CAPTION As Integer = &H2
		Public Const WM_MOUSEMOVE As Integer = &H200
		Public Const WM_LBUTTONDOWN As Integer = &H201
		Public Const WM_LBUTTONUP As Integer = &H202
		Public Const WM_LBUTTONDBLCLK As Integer = &H203
		Public Const WM_RBUTTONDOWN As Integer = &H204
		Private Const HTBOTTOMLEFT As Integer = 16
		Private Const HTBOTTOMRIGHT As Integer = 17
		Private Const HTLEFT As Integer = 10
		Private Const HTRIGHT As Integer = 11
		Private Const HTBOTTOM As Integer = 15
		Private Const HTTOP As Integer = 12
		Private Const HTTOPLEFT As Integer = 13
		Private Const HTTOPRIGHT As Integer = 14
		Private Const BORDER_WIDTH As Integer = 7
		Private resizeDir As ResizeDirection
        Private buttonState As ButtonStates = ButtonStates.None

        Private Const WMSZ_TOP As Integer = 3
		Private Const WMSZ_TOPLEFT As Integer = 4
		Private Const WMSZ_TOPRIGHT As Integer = 5
		Private Const WMSZ_LEFT As Integer = 1
		Private Const WMSZ_RIGHT As Integer = 2
		Private Const WMSZ_BOTTOM As Integer = 6
		Private Const WMSZ_BOTTOMLEFT As Integer = 7
		Private Const WMSZ_BOTTOMRIGHT As Integer = 8

		Private ReadOnly resizingLocationsToCmd As New Dictionary(Of Integer, Integer)() From { _
			{HTTOP, WMSZ_TOP}, _
			{HTTOPLEFT, WMSZ_TOPLEFT}, _
			{HTTOPRIGHT, WMSZ_TOPRIGHT}, _
			{HTLEFT, WMSZ_LEFT}, _
			{HTRIGHT, WMSZ_RIGHT}, _
			{HTBOTTOM, WMSZ_BOTTOM}, _
			{HTBOTTOMLEFT, WMSZ_BOTTOMLEFT}, _
			{HTBOTTOMRIGHT, WMSZ_BOTTOMRIGHT} _
		}

		Private Const STATUS_BAR_BUTTON_WIDTH As Integer = STATUS_BAR_HEIGHT
		Private Const STATUS_BAR_HEIGHT As Integer = 24
		Private Const ACTION_BAR_HEIGHT As Integer = 40

		Private Const TPM_LEFTALIGN As UInteger = &H0
		Private Const TPM_RETURNCMD As UInteger = &H100

		Private Const WM_SYSCOMMAND As Integer = &H112
		Private Const WS_MINIMIZEBOX As Integer = &H20000
		Private Const WS_SYSMENU As Integer = &H80000

		Private Const MONITOR_DEFAULTTONEAREST As Integer = 2

		<StructLayout(LayoutKind.Sequential, CharSet := CharSet.Auto, Pack := 4)> _
		Public Class MONITORINFOEX
			Public cbSize As Integer = Marshal.SizeOf(GetType(MONITORINFOEX))
			Public rcMonitor As New RECT()
			Public rcWork As New RECT()
			Public dwFlags As Integer = 0
			<MarshalAs(UnmanagedType.ByValArray, SizeConst := 32)> _
			Public szDevice As Char() = New Char(31) {}
		End Class

		<StructLayout(LayoutKind.Sequential)> _
		Public Structure RECT
			Public left As Integer
			Public top As Integer
			Public right As Integer
			Public bottom As Integer

			Public Function Width() As Integer
				Return right - left
			End Function

			Public Function Height() As Integer
				Return bottom - top
			End Function
		End Structure

        Private Shadows Enum ResizeDirection
            BottomLeft
            Left
            Right
            BottomRight
            Bottom
            None
        End Enum

        Private Enum ButtonStates
            XOver
            MaxOver
            MinOver
            XDown
            MaxDown
            MinDown
            None
        End Enum

        Private ReadOnly resizeCursors As Cursor() = {Cursors.SizeNESW, Cursors.SizeWE, Cursors.SizeNWSE, Cursors.SizeWE, Cursors.SizeNS}

		Private minButtonBounds As Rectangle
		Private maxButtonBounds As Rectangle
		Private xButtonBounds As Rectangle
		Private actionBarBounds As Rectangle
		Private statusBarBounds As Rectangle

		Private Maximized As Boolean
		Private previousSize As Size
		Private previousLocation As Point
		Private headerMouseDown As Boolean

		Public Sub New()
			' FormBorderStyle = FormBorderStyle.None;
			Sizable = True
			DoubleBuffered = True
			SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.ResizeRedraw, True)

			' This enables the form to trigger the MouseMove event even when mouse is over another control
			Application.AddMessageFilter(New MouseMessageFilter())
			AddHandler MouseMessageFilter.MouseMove, AddressOf OnGlobalMouseMove
		End Sub

		Protected Overrides Sub WndProc(ByRef m As Message)
			MyBase.WndProc(m)
			If DesignMode OrElse IsDisposed Then
				Return
			End If

			If m.Msg = WM_LBUTTONDBLCLK Then
				MaximizeWindow(Not Maximized)
			ElseIf m.Msg = WM_MOUSEMOVE AndAlso Maximized AndAlso (statusBarBounds.Contains(PointToClient(Cursor.Position)) OrElse actionBarBounds.Contains(PointToClient(Cursor.Position))) AndAlso Not (minButtonBounds.Contains(PointToClient(Cursor.Position)) OrElse maxButtonBounds.Contains(PointToClient(Cursor.Position)) OrElse xButtonBounds.Contains(PointToClient(Cursor.Position))) Then
				If headerMouseDown Then
					Maximized = False
					headerMouseDown = False

					Dim mousePoint As Point = PointToClient(Cursor.Position)
					If mousePoint.X < Width / 2 Then
						Location = If(mousePoint.X < previousSize.Width \ 2, New Point(Cursor.Position.X - mousePoint.X, Cursor.Position.Y - mousePoint.Y), New Point(Cursor.Position.X - previousSize.Width \ 2, Cursor.Position.Y - mousePoint.Y))
					Else
						Location = If(Width - mousePoint.X < previousSize.Width \ 2, New Point(Cursor.Position.X - previousSize.Width + Width - mousePoint.X, Cursor.Position.Y - mousePoint.Y), New Point(Cursor.Position.X - previousSize.Width \ 2, Cursor.Position.Y - mousePoint.Y))
					End If

					Size = previousSize
					ReleaseCapture()
					SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0)
				End If
			ElseIf m.Msg = WM_LBUTTONDOWN AndAlso (statusBarBounds.Contains(PointToClient(Cursor.Position)) OrElse actionBarBounds.Contains(PointToClient(Cursor.Position))) AndAlso Not (minButtonBounds.Contains(PointToClient(Cursor.Position)) OrElse maxButtonBounds.Contains(PointToClient(Cursor.Position)) OrElse xButtonBounds.Contains(PointToClient(Cursor.Position))) Then
				If Not Maximized Then
					ReleaseCapture()
					SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0)
				Else
					headerMouseDown = True
				End If
			ElseIf m.Msg = WM_RBUTTONDOWN Then
				Dim cursorPos As Point = PointToClient(Cursor.Position)

				If statusBarBounds.Contains(cursorPos) AndAlso Not minButtonBounds.Contains(cursorPos) AndAlso Not maxButtonBounds.Contains(cursorPos) AndAlso Not xButtonBounds.Contains(cursorPos) Then
					' Show default system menu when right clicking titlebar
					Dim id As Integer = TrackPopupMenuEx(GetSystemMenu(Handle, False), TPM_LEFTALIGN Or TPM_RETURNCMD, Cursor.Position.X, Cursor.Position.Y, Handle, IntPtr.Zero)

					' Pass the command as a WM_SYSCOMMAND message
					SendMessage(Handle, WM_SYSCOMMAND, id, 0)
				End If
			ElseIf m.Msg = WM_NCLBUTTONDOWN Then
				' This re-enables resizing by letting the application know when the
				' user is trying to resize a side. This is disabled by default when using WS_SYSMENU.
				If Not Sizable Then
					Return
				End If

				Dim bFlag As Byte = 0

				' Get which side to resize from
				If resizingLocationsToCmd.ContainsKey(CInt(m.WParam)) Then
					bFlag = CByte(resizingLocationsToCmd(CInt(m.WParam)))
				End If

				If bFlag <> 0 Then
					SendMessage(Handle, WM_SYSCOMMAND, &Hf000 Or bFlag, CInt(m.LParam))
				End If
			ElseIf m.Msg = WM_LBUTTONUP Then
				headerMouseDown = False
			End If
		End Sub

		Protected Overrides ReadOnly Property CreateParams() As CreateParams
			Get
				Dim par As CreateParams = MyBase.CreateParams
				' WS_SYSMENU: Trigger the creation of the system menu
				' WS_MINIMIZEBOX: Allow minimizing from taskbar
				par.Style = par.Style Or WS_MINIMIZEBOX Or WS_SYSMENU
				' Turn on the WS_MINIMIZEBOX style flag
				Return par
			End Get
		End Property

		Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
			If DesignMode Then
				Return
			End If
			UpdateButtons(e)

			If e.Button = MouseButtons.Left AndAlso Not Maximized Then
				ResizeForm(resizeDir)
			End If
			MyBase.OnMouseDown(e)
		End Sub

		Protected Overrides Sub OnMouseLeave(e As EventArgs)
			MyBase.OnMouseLeave(e)
			If DesignMode Then
				Return
			End If
            buttonState = ButtonStates.None
            Invalidate()
		End Sub

		Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
			MyBase.OnMouseMove(e)

			If DesignMode Then
				Return
			End If

			If Sizable Then
				'True if the mouse is hovering over a child control
				Dim isChildUnderMouse As Boolean = GetChildAtPoint(e.Location) IsNot Nothing

				If e.Location.X < BORDER_WIDTH AndAlso e.Location.Y > Height - BORDER_WIDTH AndAlso Not isChildUnderMouse AndAlso Not Maximized Then
					resizeDir = ResizeDirection.BottomLeft
					Cursor = Cursors.SizeNESW
				ElseIf e.Location.X < BORDER_WIDTH AndAlso Not isChildUnderMouse AndAlso Not Maximized Then
					resizeDir = ResizeDirection.Left
					Cursor = Cursors.SizeWE
				ElseIf e.Location.X > Width - BORDER_WIDTH AndAlso e.Location.Y > Height - BORDER_WIDTH AndAlso Not isChildUnderMouse AndAlso Not Maximized Then
					resizeDir = ResizeDirection.BottomRight
					Cursor = Cursors.SizeNWSE
				ElseIf e.Location.X > Width - BORDER_WIDTH AndAlso Not isChildUnderMouse AndAlso Not Maximized Then
					resizeDir = ResizeDirection.Right
					Cursor = Cursors.SizeWE
				ElseIf e.Location.Y > Height - BORDER_WIDTH AndAlso Not isChildUnderMouse AndAlso Not Maximized Then
					resizeDir = ResizeDirection.Bottom
					Cursor = Cursors.SizeNS
				Else
					resizeDir = ResizeDirection.None

					'Only reset the cursor when needed, this prevents it from flickering when a child control changes the cursor to its own needs
					If resizeCursors.Contains(Cursor) Then
						Cursor = Cursors.[Default]
					End If
				End If
			End If

			UpdateButtons(e)
		End Sub

		Protected Sub OnGlobalMouseMove(sender As Object, e As MouseEventArgs)
			If Not IsDisposed Then
				' Convert to client position and pass to Form.MouseMove
				Dim clientCursorPos As Point = PointToClient(e.Location)
				Dim new_e As New MouseEventArgs(MouseButtons.None, 0, clientCursorPos.X, clientCursorPos.Y, 0)
				OnMouseMove(new_e)
			End If
		End Sub

		Private Sub UpdateButtons(e As MouseEventArgs, Optional up As Boolean = False)
			If DesignMode Then
				Return
			End If
            Dim oldState As ButtonStates = buttonState
            Dim showMin As Boolean = MinimizeBox AndAlso ControlBox
			Dim showMax As Boolean = MaximizeBox AndAlso ControlBox

			If e.Button = MouseButtons.Left AndAlso Not up Then
				If showMin AndAlso Not showMax AndAlso maxButtonBounds.Contains(e.Location) Then
                    buttonState = ButtonStates.MinDown
                ElseIf showMin AndAlso showMax AndAlso minButtonBounds.Contains(e.Location) Then
                    buttonState = ButtonStates.MinDown
                ElseIf showMax AndAlso maxButtonBounds.Contains(e.Location) Then
                    buttonState = ButtonStates.MaxDown
                ElseIf ControlBox AndAlso xButtonBounds.Contains(e.Location) Then
                    buttonState = ButtonStates.XDown
                Else
                    buttonState = ButtonStates.None
                End If
			Else
				If showMin AndAlso Not showMax AndAlso maxButtonBounds.Contains(e.Location) Then
                    buttonState = ButtonStates.MinOver

                    If oldState = ButtonStates.MinDown Then
                        WindowState = FormWindowState.Minimized
                    End If
                ElseIf showMin AndAlso showMax AndAlso minButtonBounds.Contains(e.Location) Then
                    buttonState = ButtonStates.MinOver

                    If oldState = ButtonStates.MinDown Then
                        WindowState = FormWindowState.Minimized
                    End If
                ElseIf MaximizeBox AndAlso ControlBox AndAlso maxButtonBounds.Contains(e.Location) Then
                    buttonState = ButtonStates.MaxOver

                    If oldState = ButtonStates.MaxDown Then
                        MaximizeWindow(Not Maximized)

                    End If
                ElseIf ControlBox AndAlso xButtonBounds.Contains(e.Location) Then
                    buttonState = ButtonStates.XOver

                    If oldState = ButtonStates.XDown Then
                        Close()
                    End If
                Else
                    buttonState = ButtonStates.None
                End If
			End If

			If oldState <> buttonState Then
				Invalidate()
			End If
		End Sub

		Private Sub MaximizeWindow(maximize As Boolean)
			If Not MaximizeBox OrElse Not ControlBox Then
				Return
			End If

			Maximized = maximize

			If maximize Then
				Dim monitorHandle As IntPtr = MonitorFromWindow(Handle, MONITOR_DEFAULTTONEAREST)
				Dim monitorInfo As New MONITORINFOEX()
				GetMonitorInfo(New HandleRef(Nothing, monitorHandle), monitorInfo)
				previousSize = Size
				previousLocation = Location
				Size = New Size(monitorInfo.rcWork.Width(), monitorInfo.rcWork.Height())
				Location = New Point(monitorInfo.rcWork.left, monitorInfo.rcWork.top)
			Else
				Size = previousSize
				Location = previousLocation
			End If

		End Sub

		Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
			If DesignMode Then
				Return
			End If
			UpdateButtons(e, True)

			MyBase.OnMouseUp(e)
			ReleaseCapture()
		End Sub

		Private Sub ResizeForm(direction As ResizeDirection)
			If DesignMode Then
				Return
			End If
			Dim dir As Integer = -1
			Select Case direction
				Case ResizeDirection.BottomLeft
					dir = HTBOTTOMLEFT
					Exit Select
				Case ResizeDirection.Left
					dir = HTLEFT
					Exit Select
				Case ResizeDirection.Right
					dir = HTRIGHT
					Exit Select
				Case ResizeDirection.BottomRight
					dir = HTBOTTOMRIGHT
					Exit Select
				Case ResizeDirection.Bottom
					dir = HTBOTTOM
					Exit Select
			End Select

			ReleaseCapture()
			If dir <> -1 Then
				SendMessage(Handle, WM_NCLBUTTONDOWN, dir, 0)
			End If
		End Sub

		Protected Overrides Sub OnResize(e As EventArgs)
			MyBase.OnResize(e)

			minButtonBounds = New Rectangle((Width - SkinManager.FORM_PADDING \ 2) - 3 * STATUS_BAR_BUTTON_WIDTH, 0, STATUS_BAR_BUTTON_WIDTH, STATUS_BAR_HEIGHT)
			maxButtonBounds = New Rectangle((Width - SkinManager.FORM_PADDING \ 2) - 2 * STATUS_BAR_BUTTON_WIDTH, 0, STATUS_BAR_BUTTON_WIDTH, STATUS_BAR_HEIGHT)
			xButtonBounds = New Rectangle((Width - SkinManager.FORM_PADDING \ 2) - STATUS_BAR_BUTTON_WIDTH, 0, STATUS_BAR_BUTTON_WIDTH, STATUS_BAR_HEIGHT)
			statusBarBounds = New Rectangle(0, 0, Width, STATUS_BAR_HEIGHT)
			actionBarBounds = New Rectangle(0, STATUS_BAR_HEIGHT, Width, ACTION_BAR_HEIGHT)
		End Sub

		Protected Overrides Sub OnPaint(e As PaintEventArgs)
            Dim g As Graphics = e.Graphics
            g.TextRenderingHint = TextRenderingHint.AntiAlias

			g.Clear(SkinManager.GetApplicationBackgroundColor())
			g.FillRectangle(SkinManager.ColorScheme.DarkPrimaryBrush, statusBarBounds)
			g.FillRectangle(SkinManager.ColorScheme.PrimaryBrush, actionBarBounds)

			'Draw border
			Using borderPen = New Pen(SkinManager.GetDividersColor(), 1)
				g.DrawLine(borderPen, New Point(0, actionBarBounds.Bottom), New Point(0, Height - 2))
				g.DrawLine(borderPen, New Point(Width - 1, actionBarBounds.Bottom), New Point(Width - 1, Height - 2))
				g.DrawLine(borderPen, New Point(0, Height - 1), New Point(Width - 1, Height - 1))
			End Using

			' Determine whether or not we even should be drawing the buttons.
			Dim showMin As Boolean = MinimizeBox AndAlso ControlBox
			Dim showMax As Boolean = MaximizeBox AndAlso ControlBox
			Dim hoverBrush = SkinManager.GetFlatButtonHoverBackgroundBrush()
			Dim downBrush = SkinManager.GetFlatButtonPressedBackgroundBrush()

            ' When MaximizeButton == false, the minimize button will be painted in its place
            If buttonState = ButtonStates.MinOver AndAlso showMin Then
                g.FillRectangle(hoverBrush, If(showMax, minButtonBounds, maxButtonBounds))
            End If

            If buttonState = ButtonStates.MinDown AndAlso showMin Then
                g.FillRectangle(downBrush, If(showMax, minButtonBounds, maxButtonBounds))
            End If

            If buttonState = ButtonStates.MaxOver AndAlso showMax Then
                g.FillRectangle(hoverBrush, maxButtonBounds)
            End If

            If buttonState = ButtonStates.MaxDown AndAlso showMax Then
                g.FillRectangle(downBrush, maxButtonBounds)
            End If

            If buttonState = ButtonStates.XOver AndAlso ControlBox Then
                g.FillRectangle(hoverBrush, xButtonBounds)
            End If

            If buttonState = ButtonStates.XDown AndAlso ControlBox Then
                g.FillRectangle(downBrush, xButtonBounds)
            End If

            Using formButtonsPen = New Pen(SkinManager.ACTION_BAR_TEXT_SECONDARY, 2)
				' Minimize button.
				If showMin Then
					Dim x As Integer = If(showMax, minButtonBounds.X, maxButtonBounds.X)
					Dim y As Integer = If(showMax, minButtonBounds.Y, maxButtonBounds.Y)

					g.DrawLine(formButtonsPen, x + CInt(Math.Truncate(minButtonBounds.Width * 0.33)), y + CInt(Math.Truncate(minButtonBounds.Height * 0.66)), x + CInt(Math.Truncate(minButtonBounds.Width * 0.66)), y + CInt(Math.Truncate(minButtonBounds.Height * 0.66)))
				End If

				' Maximize button
				If showMax Then
					g.DrawRectangle(formButtonsPen, maxButtonBounds.X + CInt(Math.Truncate(maxButtonBounds.Width * 0.33)), maxButtonBounds.Y + CInt(Math.Truncate(maxButtonBounds.Height * 0.36)), CInt(Math.Truncate(maxButtonBounds.Width * 0.39)), CInt(Math.Truncate(maxButtonBounds.Height * 0.31)))
				End If

				' Close button
				If ControlBox Then
					g.DrawLine(formButtonsPen, xButtonBounds.X + CInt(Math.Truncate(xButtonBounds.Width * 0.33)), xButtonBounds.Y + CInt(Math.Truncate(xButtonBounds.Height * 0.33)), xButtonBounds.X + CInt(Math.Truncate(xButtonBounds.Width * 0.66)), xButtonBounds.Y + CInt(Math.Truncate(xButtonBounds.Height * 0.66)))

					g.DrawLine(formButtonsPen, xButtonBounds.X + CInt(Math.Truncate(xButtonBounds.Width * 0.66)), xButtonBounds.Y + CInt(Math.Truncate(xButtonBounds.Height * 0.33)), xButtonBounds.X + CInt(Math.Truncate(xButtonBounds.Width * 0.33)), xButtonBounds.Y + CInt(Math.Truncate(xButtonBounds.Height * 0.66)))
				End If
			End Using

            'Form title
            g.DrawString(Text, SkinManager.ROBOTO_MEDIUM_12, SkinManager.ColorScheme.TextBrush, New Rectangle(SkinManager.FORM_PADDING, STATUS_BAR_HEIGHT, Width, ACTION_BAR_HEIGHT), New StringFormat() With {
             .LineAlignment = StringAlignment.Center
            })
        End Sub
	End Class

	Public Class MouseMessageFilter
		Implements IMessageFilter
		Private Const WM_MOUSEMOVE As Integer = &H200

		Public Shared Event MouseMove As MouseEventHandler

		Public Function PreFilterMessage(ByRef m As Message) As Boolean Implements IMessageFilter.PreFilterMessage

            If m.Msg = WM_MOUSEMOVE Then

                Dim x As Integer = Control.MousePosition.X, y As Integer = Control.MousePosition.Y

                RaiseEvent MouseMove(Nothing, New MouseEventArgs(MouseButtons.None, 0, x, y, 0))
            End If
            Return False
		End Function
	End Class
End Namespace
