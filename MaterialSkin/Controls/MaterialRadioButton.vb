Imports System.ComponentModel
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Text
Imports System.Windows.Forms
Imports MaterialSkin.Animations

Namespace Controls
	Public Class MaterialRadioButton
		Inherits RadioButton
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
		<Browsable(False)> _
		Public Property MouseLocation() As Point
			Get
				Return m_MouseLocation
			End Get
			Set
				m_MouseLocation = Value
			End Set
		End Property
		Private m_MouseLocation As Point

		Private m_ripple As Boolean
		<Category("Behavior")> _
		Public Property Ripple() As Boolean
			Get
				Return m_ripple
			End Get
			Set
				m_ripple = value
				AutoSize = AutoSize
				'Make AutoSize directly set the bounds.
				If value Then
					Margin = New Padding(0)
				End If

				Invalidate()
			End Set
		End Property

		' animation managers
		Private ReadOnly animationManager As AnimationManager
		Private ReadOnly rippleAnimationManager As AnimationManager

		' size related variables which should be recalculated onsizechanged
		Private radioButtonBounds As Rectangle
		Private boxOffset As Integer

		' size constants
		Private Const RADIOBUTTON_SIZE As Integer = 19
		Private Const RADIOBUTTON_SIZE_HALF As Integer = RADIOBUTTON_SIZE \ 2
		Private Const RADIOBUTTON_OUTER_CIRCLE_WIDTH As Integer = 2
		Private Const RADIOBUTTON_INNER_CIRCLE_SIZE As Integer = RADIOBUTTON_SIZE - (2 * RADIOBUTTON_OUTER_CIRCLE_WIDTH)

		Public Sub New()
			SetStyle(ControlStyles.DoubleBuffer Or ControlStyles.OptimizedDoubleBuffer, True)

            animationManager = New AnimationManager() With {
             .AnimationType = AnimationType.EaseInOut,
             .Increment = 0.06
            }
            rippleAnimationManager = New AnimationManager(False) With {
             .AnimationType = AnimationType.Linear,
             .Increment = 0.1,
             .SecondaryIncrement = 0.08
            }
            AddHandler animationManager.OnAnimationProgress, Sub(sender) Invalidate()
            AddHandler rippleAnimationManager.OnAnimationProgress, Sub(sender) Invalidate()

            AddHandler CheckedChanged, Sub(sender, args) animationManager.StartNewAnimation(If(Checked, AnimationDirection.[In], AnimationDirection.Out))

            AddHandler SizeChanged, AddressOf OnSizeChanged

            Ripple = True
            MouseLocation = New Point(-1, -1)
        End Sub
        Private Overloads Sub OnSizeChanged(sender As Object, eventArgs As EventArgs)
            boxOffset = Height \ 2 - CInt(Math.Truncate(Math.Ceiling(RADIOBUTTON_SIZE / 2.0)))
            radioButtonBounds = New Rectangle(boxOffset, boxOffset, RADIOBUTTON_SIZE, RADIOBUTTON_SIZE)
        End Sub

        Public Overrides Function GetPreferredSize(proposedSize As Size) As Size
            Dim width As Integer = boxOffset + 20 + CInt(Math.Truncate(CreateGraphics().MeasureString(Text, SkinManager.ROBOTO_MEDIUM_10).Width))
            Return If(Ripple, New Size(width, 30), New Size(width, 20))
        End Function

        Protected Overrides Sub OnPaint(pevent As PaintEventArgs)
            Dim g As Graphics = pevent.Graphics
            g.SmoothingMode = SmoothingMode.AntiAlias
            g.TextRenderingHint = TextRenderingHint.AntiAlias

            ' clear the control
            g.Clear(Parent.BackColor)

            Dim RADIOBUTTON_CENTER = boxOffset + RADIOBUTTON_SIZE_HALF

            Dim animationProgress = animationManager.GetProgress()

            Dim colorAlpha As Integer = If(Enabled, CInt(Math.Truncate(animationProgress * 255.0)), SkinManager.GetCheckBoxOffDisabledColor().A)
            Dim backgroundAlpha As Integer = If(Enabled, CInt(Math.Truncate(SkinManager.GetCheckboxOffColor().A * (1.0 - animationProgress))), SkinManager.GetCheckBoxOffDisabledColor().A)
            Dim animationSize As Single = CSng(animationProgress * 8.0F)
            Dim animationSizeHalf As Single = animationSize / 2
            animationSize = CSng(animationProgress * 9.0F)

            Dim brush As SolidBrush = New SolidBrush(Color.FromArgb(colorAlpha, If(Enabled, SkinManager.ColorScheme.AccentColor, SkinManager.GetCheckBoxOffDisabledColor())))
            Dim pen As Pen = New Pen(brush.Color)

            ' draw ripple animation
            If Ripple AndAlso rippleAnimationManager.IsAnimating() Then
                For i As Integer = 0 To rippleAnimationManager.GetAnimationCount() - 1
                    Dim animationValue = rippleAnimationManager.GetProgress(i)
                    Dim animationSource = New Point(RADIOBUTTON_CENTER, RADIOBUTTON_CENTER)
                    Dim rippleBrush = New SolidBrush(Color.FromArgb(CInt(Math.Truncate((animationValue * 40))), If(CBool(rippleAnimationManager.GetData(i)(0)), Color.Black, brush.Color)))
                    Dim rippleHeight = If((Height Mod 2 = 0), Height - 3, Height - 2)
                    Dim rippleSize = If((rippleAnimationManager.GetDirection(i) = AnimationDirection.InOutIn), CInt(Math.Truncate(rippleHeight * (0.8 + (0.2 * animationValue)))), rippleHeight)
                    Using path = DrawHelper.CreateRoundRect(animationSource.X - rippleSize \ 2, animationSource.Y - rippleSize \ 2, rippleSize, rippleSize, rippleSize \ 2)
                        g.FillPath(rippleBrush, path)
                    End Using

                    rippleBrush.Dispose()
                Next
            End If

            ' draw radiobutton circle
            Dim uncheckedColor As Color = DrawHelper.BlendColor(Parent.BackColor, If(Enabled, SkinManager.GetCheckboxOffColor(), SkinManager.GetCheckBoxOffDisabledColor()), backgroundAlpha)

            Using path = DrawHelper.CreateRoundRect(boxOffset, boxOffset, RADIOBUTTON_SIZE, RADIOBUTTON_SIZE, 9.0F)
                g.FillPath(New SolidBrush(uncheckedColor), path)

                If Enabled Then
                    g.FillPath(brush, path)
                End If
            End Using

            g.FillEllipse(New SolidBrush(Parent.BackColor), RADIOBUTTON_OUTER_CIRCLE_WIDTH + boxOffset, RADIOBUTTON_OUTER_CIRCLE_WIDTH + boxOffset, RADIOBUTTON_INNER_CIRCLE_SIZE, RADIOBUTTON_INNER_CIRCLE_SIZE)

            If Checked Then
                Using path = DrawHelper.CreateRoundRect(RADIOBUTTON_CENTER - animationSizeHalf, RADIOBUTTON_CENTER - animationSizeHalf, animationSize, animationSize, 4.0F)
                    g.FillPath(brush, path)
                End Using
            End If
            Dim stringSize As SizeF = g.MeasureString(Text, SkinManager.ROBOTO_MEDIUM_10)
            g.DrawString(Text, SkinManager.ROBOTO_MEDIUM_10, If(Enabled, SkinManager.GetPrimaryTextBrush(), SkinManager.GetDisabledOrHintBrush()), boxOffset + 22, Height \ 2 - stringSize.Height / 2)

            brush.Dispose()
            pen.Dispose()
        End Sub

        Private Function IsMouseInCheckArea() As Boolean
            Return radioButtonBounds.Contains(MouseLocation)
        End Function

        Protected Overrides Sub OnCreateControl()
            MyBase.OnCreateControl()
            Font = SkinManager.ROBOTO_MEDIUM_10

            If DesignMode Then
                Return
            End If

            MouseState = MouseState.OUT
            AddHandler MouseEnter, Sub(sender, args)
                                       MouseState = MouseState.HOVER

                                   End Sub
            AddHandler MouseLeave, Sub(sender, args)
                                       MouseLocation = New Point(-1, -1)
                                       MouseState = MouseState.OUT

                                   End Sub
            AddHandler MouseDown, Sub(sender, args)
                                      MouseState = MouseState.DOWN

                                      If Ripple AndAlso args.Button = MouseButtons.Left AndAlso IsMouseInCheckArea() Then
                                          rippleAnimationManager.SecondaryIncrement = 0
                                          rippleAnimationManager.StartNewAnimation(AnimationDirection.InOutIn, New Object() {Checked})
                                      End If

                                  End Sub
            AddHandler MouseUp, Sub(sender, args)
                                    MouseState = MouseState.HOVER
                                    rippleAnimationManager.SecondaryIncrement = 0.08

                                End Sub
            AddHandler MouseMove, Sub(sender, args)
                                      MouseLocation = args.Location
                                      Cursor = If(IsMouseInCheckArea(), Cursors.Hand, Cursors.[Default])

                                  End Sub
        End Sub
    End Class
End Namespace
