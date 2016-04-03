Imports System.ComponentModel
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Text
Imports System.Windows.Forms
Imports MaterialSkin.Animations

Namespace Controls
	Public Class MaterialRaisedButton
		Inherits Button
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
		Public Property Primary() As Boolean
			Get
				Return m_Primary
			End Get
			Set
				m_Primary = Value
			End Set
		End Property
		Private m_Primary As Boolean

		Private ReadOnly animationManager As AnimationManager

		Public Sub New()
			Primary = True

			animationManager = New AnimationManager(False) With { _
			 .Increment = 0.03, _
			 .AnimationType = AnimationType.EaseOut _
			}
            AddHandler animationManager.OnAnimationProgress, Sub(sender) Invalidate()
        End Sub

		Protected Overrides Sub OnMouseUp(mevent As MouseEventArgs)
			MyBase.OnMouseUp(mevent)

			animationManager.StartNewAnimation(AnimationDirection.[In], mevent.Location)
		End Sub

		Protected Overrides Sub OnPaint(pevent As PaintEventArgs)
			Dim g = pevent.Graphics
			g.SmoothingMode = SmoothingMode.AntiAlias
			g.TextRenderingHint = TextRenderingHint.AntiAlias

			g.Clear(Parent.BackColor)

			Using backgroundPath = DrawHelper.CreateRoundRect(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1, ClientRectangle.Height - 1, 1F)
				g.FillPath(If(Primary, SkinManager.ColorScheme.PrimaryBrush, SkinManager.GetRaisedButtonBackgroundBrush()), backgroundPath)
			End Using

			If animationManager.IsAnimating() Then
				For i As Integer = 0 To animationManager.GetAnimationCount() - 1
					Dim animationValue = animationManager.GetProgress(i)
					Dim animationSource = animationManager.GetSource(i)
					Dim rippleBrush = New SolidBrush(Color.FromArgb(CInt(Math.Truncate(51 - (animationValue * 50))), Color.White))
					Dim rippleSize = CInt(Math.Truncate(animationValue * Width * 2))
					g.FillEllipse(rippleBrush, New Rectangle(animationSource.X - rippleSize \ 2, animationSource.Y - rippleSize \ 2, rippleSize, rippleSize))
				Next
			End If

			g.DrawString(Text.ToUpper(), SkinManager.ROBOTO_MEDIUM_10, SkinManager.GetRaisedButtonTextBrush(Primary), ClientRectangle, New StringFormat() With { _
			 .Alignment = StringAlignment.Center, _
			 .LineAlignment = StringAlignment.Center _
			})
		End Sub
	End Class
End Namespace
