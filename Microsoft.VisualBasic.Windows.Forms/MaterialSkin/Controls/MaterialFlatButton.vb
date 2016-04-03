Imports System.ComponentModel
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Text
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.Windows.Forms.Animations

Namespace Controls
    Public Class MaterialFlatButton
        Inherits Button
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

        Public Property Primary() As Boolean

        Private ReadOnly animationManager As AnimationManager
        Private ReadOnly hoverAnimationManager As AnimationManager

        Private textSize As SizeF

        Public Sub New()
            Primary = False

            animationManager = New AnimationManager(False) With {
                .Increment = 0.03,
                .AnimationType = AnimationType.EaseOut
            }
            hoverAnimationManager = New AnimationManager() With {
                .Increment = 0.07,
                .AnimationType = AnimationType.Linear
            }

            AddHandler hoverAnimationManager.OnAnimationProgress, Sub(sender) Invalidate()
            AddHandler animationManager.OnAnimationProgress, Sub(sender) Invalidate()

            AutoSizeMode = AutoSizeMode.GrowAndShrink
            AutoSize = True
            Margin = New Padding(4, 6, 4, 6)
            Padding = New Padding(0)
        End Sub

        Public Overrides Property Text() As String
            Get
                Return MyBase.Text
            End Get
            Set
                MyBase.Text = Value
                textSize = CreateGraphics().MeasureString(Value.ToUpper(), SkinManager.ROBOTO_MEDIUM_10)
                If AutoSize Then
                    Size = GetPreferredSize()
                End If
                Invalidate()
            End Set
        End Property

        Protected Overrides Sub OnPaint(pevent As PaintEventArgs)
            Dim g = pevent.Graphics
            g.TextRenderingHint = TextRenderingHint.AntiAlias

            g.Clear(Parent.BackColor)

            'Hover
            Dim c As Color = SkinManager.GetFlatButtonHoverBackgroundColor()
            Using b As Brush = New SolidBrush(Color.FromArgb(CInt(Math.Truncate(hoverAnimationManager.GetProgress() * c.A)), c.RemoveAlpha()))
                g.FillRectangle(b, ClientRectangle)
            End Using

            'Ripple
            If animationManager.IsAnimating() Then
                g.SmoothingMode = SmoothingMode.AntiAlias
                For i As Integer = 0 To animationManager.GetAnimationCount() - 1
                    Dim animationValue = animationManager.GetProgress(i)
                    Dim animationSource = animationManager.GetSource(i)

                    Using rippleBrush As Brush = New SolidBrush(Color.FromArgb(CInt(Math.Truncate(101 - (animationValue * 100))), Color.Black))
                        Dim rippleSize = CInt(Math.Truncate(animationValue * Width * 2))
                        g.FillEllipse(rippleBrush, New Rectangle(animationSource.X - rippleSize \ 2, animationSource.Y - rippleSize \ 2, rippleSize, rippleSize))
                    End Using
                Next
                g.SmoothingMode = SmoothingMode.None
            End If
            g.DrawString(Text.ToUpper(), SkinManager.ROBOTO_MEDIUM_10, If(Enabled, (If(Primary, SkinManager.ColorScheme.PrimaryBrush, SkinManager.GetPrimaryTextBrush())), SkinManager.GetFlatButtonDisabledTextBrush()), ClientRectangle, New StringFormat() With {
             .Alignment = StringAlignment.Center,
             .LineAlignment = StringAlignment.Center
            })
        End Sub

        Private Overloads Function GetPreferredSize() As Size
            Return GetPreferredSize(New Size(0, 0))
        End Function

        Public Overrides Function GetPreferredSize(proposedSize As Size) As Size
            Return New Size(CInt(Math.Truncate(textSize.Width)) + 8, 36)
        End Function

        Protected Overrides Sub OnCreateControl()
            MyBase.OnCreateControl()
            If DesignMode Then
                Return
            End If

            MouseState = MouseState.OUT
            AddHandler MouseEnter, Sub(sender, args)
                                       MouseState = MouseState.HOVER
                                       hoverAnimationManager.StartNewAnimation(AnimationDirection.[In])
                                       Invalidate()
                                   End Sub
            AddHandler MouseLeave, Sub(sender, args)
                                       MouseState = MouseState.OUT
                                       hoverAnimationManager.StartNewAnimation(AnimationDirection.Out)
                                       Invalidate()
                                   End Sub
            AddHandler MouseDown, Sub(sender, args)
                                      If args.Button = MouseButtons.Left Then
                                          MouseState = MouseState.DOWN

                                          animationManager.StartNewAnimation(AnimationDirection.[In], args.Location)
                                          Invalidate()
                                      End If
                                  End Sub
            AddHandler MouseUp, Sub(sender, args)
                                    MouseState = MouseState.HOVER
                                    Invalidate()
                                End Sub
        End Sub
    End Class
End Namespace
