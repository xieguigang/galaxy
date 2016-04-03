Imports System.ComponentModel
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Text
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.Windows.Forms.Animations

Namespace Controls

    Public Class MaterialCheckBox
        Inherits CheckBox
        Implements IMaterialControl

        <Browsable(False)>
        Public Property Depth() As Integer Implements IMaterialControl.Depth

        <Browsable(False)>
        Public ReadOnly Property SkinManager() As MaterialSkinManager Implements IMaterialControl.SkinManager
            Get
                Return MaterialSkinManager.Instance
            End Get
        End Property

        <Browsable(False)>
        Public Property MouseState() As MouseState Implements IMaterialControl.MouseState

        <Browsable(False)>
        Public Property MouseLocation() As Point

        Private m_ripple As Boolean

        <Category("Behavior")>
        Public Property Ripple() As Boolean
            Get
                Return m_ripple
            End Get
            Set
                m_ripple = Value
                AutoSize = AutoSize
                'Make AutoSize directly set the bounds.
                If Value Then
                    Margin = New Padding(0)
                End If

                Invalidate()
            End Set
        End Property

        Private ReadOnly animationManager As AnimationManager
        Private ReadOnly rippleAnimationManager As AnimationManager

        Private Const CHECKBOX_SIZE As Integer = 18
        Private Const CHECKBOX_SIZE_HALF As Integer = CHECKBOX_SIZE \ 2
        Private Const CHECKBOX_INNER_BOX_SIZE As Integer = CHECKBOX_SIZE - 4

        Private boxOffset As Integer
        Private boxRectangle As Rectangle

        Public Sub New()
            animationManager = New AnimationManager() With {
                .AnimationType = AnimationType.EaseInOut,
                .Increment = 0.05
            }
            rippleAnimationManager = New AnimationManager(False) With {
                .AnimationType = AnimationType.Linear,
                .Increment = 0.1,
                .SecondaryIncrement = 0.08
            }
            AddHandler animationManager.OnAnimationProgress, Sub(sender) Invalidate()
            AddHandler rippleAnimationManager.OnAnimationProgress, Sub(sender) Invalidate()
            AddHandler CheckedChanged, Sub(sender, args) animationManager.StartNewAnimation(If(Checked, AnimationDirection.[In], AnimationDirection.Out))

            Ripple = True
            MouseLocation = New Point(-1, -1)
        End Sub

        Protected Overrides Sub OnSizeChanged(e As EventArgs)
            MyBase.OnSizeChanged(e)

            boxOffset = Height \ 2 - 9
            boxRectangle = New Rectangle(boxOffset, boxOffset, CHECKBOX_SIZE - 1, CHECKBOX_SIZE - 1)
        End Sub

        Public Overrides Function GetPreferredSize(proposedSize As Size) As Size
            Dim w As Integer = boxOffset + CHECKBOX_SIZE + 2 + CInt(Math.Truncate(CreateGraphics().MeasureString(Text, SkinManager.ROBOTO_MEDIUM_10).Width))
            Return If(Ripple, New Size(w, 30), New Size(w, 20))
        End Function

        Private Shared ReadOnly CHECKMARK_LINE As Point() = {New Point(3, 8), New Point(7, 12), New Point(14, 5)}
        Private Const TEXT_OFFSET As Integer = 22

        Protected Overrides Sub OnPaint(pevent As PaintEventArgs)
            Dim g As Graphics = pevent.Graphics
            g.SmoothingMode = SmoothingMode.AntiAlias
            g.TextRenderingHint = TextRenderingHint.AntiAlias

            ' clear the control
            g.Clear(Parent.BackColor)

            Dim CHECKBOX_CENTER As Integer = boxOffset + CHECKBOX_SIZE_HALF - 1

            Dim animationProgress As Double = animationManager.GetProgress()

            Dim colorAlpha As Integer = If(Enabled, CInt(Math.Truncate(animationProgress * 255.0)), SkinManager.GetCheckBoxOffDisabledColor().A)
            Dim backgroundAlpha As Integer = If(Enabled, CInt(Math.Truncate(SkinManager.GetCheckboxOffColor().A * (1.0 - animationProgress))), SkinManager.GetCheckBoxOffDisabledColor().A)

            Dim brush As SolidBrush = New SolidBrush(Color.FromArgb(colorAlpha, If(Enabled, SkinManager.ColorScheme.AccentColor, SkinManager.GetCheckBoxOffDisabledColor())))
            Dim brush3 As SolidBrush = New SolidBrush(If(Enabled, SkinManager.ColorScheme.AccentColor, SkinManager.GetCheckBoxOffDisabledColor()))
            Dim pen = New Pen(brush.Color)

            ' draw ripple animation
            If Ripple AndAlso rippleAnimationManager.IsAnimating() Then
                For i As Integer = 0 To rippleAnimationManager.GetAnimationCount() - 1
                    Dim animationValue = rippleAnimationManager.GetProgress(i)
                    Dim animationSource = New Point(CHECKBOX_CENTER, CHECKBOX_CENTER)
                    Dim rippleBrush = New SolidBrush(Color.FromArgb(CInt(Math.Truncate((animationValue * 40))), If(CBool(rippleAnimationManager.GetData(i)(0)), Color.Black, brush.Color)))
                    Dim rippleHeight = If((Height Mod 2 = 0), Height - 3, Height - 2)
                    Dim rippleSize = If((rippleAnimationManager.GetDirection(i) = AnimationDirection.InOutIn), CInt(Math.Truncate(rippleHeight * (0.8 + (0.2 * animationValue)))), rippleHeight)
                    Using path = DrawHelper.CreateRoundRect(animationSource.X - rippleSize \ 2, animationSource.Y - rippleSize \ 2, rippleSize, rippleSize, rippleSize \ 2)
                        g.FillPath(rippleBrush, path)
                    End Using

                    rippleBrush.Dispose()
                Next
            End If

            brush3.Dispose()

            Dim checkMarkLineFill = New Rectangle(boxOffset, boxOffset, CInt(Math.Truncate(17.0 * animationProgress)), 17)
            Using checkmarkPath = DrawHelper.CreateRoundRect(boxOffset, boxOffset, 17, 17, 1.0F)
                Dim brush2 As New SolidBrush(DrawHelper.BlendColor(Parent.BackColor, If(Enabled, SkinManager.GetCheckboxOffColor(), SkinManager.GetCheckBoxOffDisabledColor()), backgroundAlpha))
                Dim pen2 As New Pen(brush2.Color)
                g.FillPath(brush2, checkmarkPath)
                g.DrawPath(pen2, checkmarkPath)

                g.FillRectangle(New SolidBrush(Parent.BackColor), boxOffset + 2, boxOffset + 2, CHECKBOX_INNER_BOX_SIZE - 1, CHECKBOX_INNER_BOX_SIZE - 1)
                g.DrawRectangle(New Pen(Parent.BackColor), boxOffset + 2, boxOffset + 2, CHECKBOX_INNER_BOX_SIZE - 1, CHECKBOX_INNER_BOX_SIZE - 1)

                brush2.Dispose()
                pen2.Dispose()

                If Enabled Then
                    g.FillPath(brush, checkmarkPath)
                    g.DrawPath(pen, checkmarkPath)
                ElseIf Checked Then
                    g.SmoothingMode = SmoothingMode.None
                    g.FillRectangle(brush, boxOffset + 2, boxOffset + 2, CHECKBOX_INNER_BOX_SIZE, CHECKBOX_INNER_BOX_SIZE)
                    g.SmoothingMode = SmoothingMode.AntiAlias
                End If

                g.DrawImageUnscaledAndClipped(DrawCheckMarkBitmap(), checkMarkLineFill)
            End Using

            ' draw checkbox text
            Dim stringSize As SizeF = g.MeasureString(Text, SkinManager.ROBOTO_MEDIUM_10)
            g.DrawString(Text, SkinManager.ROBOTO_MEDIUM_10, If(Enabled, SkinManager.GetPrimaryTextBrush(), SkinManager.GetDisabledOrHintBrush()), boxOffset + TEXT_OFFSET, Height \ 2 - stringSize.Height / 2)

            ' dispose used paint objects
            pen.Dispose()
            brush.Dispose()
        End Sub

        Private Function DrawCheckMarkBitmap() As Bitmap
            Dim checkMark = New Bitmap(CHECKBOX_SIZE, CHECKBOX_SIZE)
            Dim g = Graphics.FromImage(checkMark)

            ' clear everything, transparent
            g.Clear(Color.Transparent)

            ' draw the checkmark lines
            Using pen = New Pen(Parent.BackColor, 2)
                g.DrawLines(pen, CHECKMARK_LINE)
            End Using

            Return checkMark
        End Function

        Public Overrides Property AutoSize() As Boolean
            Get
                Return MyBase.AutoSize
            End Get
            Set
                MyBase.AutoSize = Value
                If Value Then
                    Size = New Size(10, 10)
                End If
            End Set
        End Property

        Private Function IsMouseInCheckArea() As Boolean
            Return boxRectangle.Contains(MouseLocation)
        End Function

        Protected Overrides Sub OnCreateControl()
            MyBase.OnCreateControl()
            Font = SkinManager.ROBOTO_MEDIUM_10

            If DesignMode Then
                Return
            End If

            MouseState = MouseState.OUT
            AddHandler MouseEnter, Sub(sender, args) MouseState = MouseState.HOVER
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
