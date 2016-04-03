Imports System.ComponentModel
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Text
Imports System.Windows.Forms
Imports MaterialSkin.Animations

Namespace Controls
	Public Class MaterialContextMenuStrip
		Inherits ContextMenuStrip
		Implements IMaterialControl
		'Properties for managing the material design properties
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


		Friend animationManager As AnimationManager
		Friend animationSource As Point

		Public Delegate Sub ItemClickStart(sender As Object, e As ToolStripItemClickedEventArgs)
		Public Event OnItemClickStart As ItemClickStart

		Public Sub New()
			Renderer = New MaterialToolStripRender()

            animationManager = New AnimationManager(False) With {
             .Increment = 0.07,
             .AnimationType = AnimationType.Linear
            }
            AddHandler animationManager.OnAnimationProgress, Sub(sender) Invalidate()
			AddHandler animationManager.OnAnimationFinished, Sub(sender) OnItemClicked(delayesArgs)

			BackColor = SkinManager.GetApplicationBackgroundColor()
		End Sub

		Protected Overrides Sub OnMouseUp(mea As MouseEventArgs)
			MyBase.OnMouseUp(mea)

			animationSource = mea.Location
		End Sub

		Private delayesArgs As ToolStripItemClickedEventArgs
		Protected Overrides Sub OnItemClicked(e As ToolStripItemClickedEventArgs)
			If e.ClickedItem IsNot Nothing AndAlso Not (TypeOf e.ClickedItem Is ToolStripSeparator) Then
				If e Is delayesArgs Then
					'The event has been fired manualy because the args are the ones we saved for delay
					MyBase.OnItemClicked(e)
				Else
					'Interrupt the default on click, saving the args for the delay which is needed to display the animaton
					delayesArgs = e

					'Fire custom event to trigger actions directly but keep cms open
					RaiseEvent OnItemClickStart(Me, e)

					'Start animation
					animationManager.StartNewAnimation(AnimationDirection.[In])
				End If
			End If
		End Sub
	End Class

	Public Class MaterialToolStripMenuItem
		Inherits ToolStripMenuItem
		Public Sub New()
			AutoSize = False
			Size = New Size(120, 30)
		End Sub

		Protected Overrides Function CreateDefaultDropDown() As ToolStripDropDown
			Dim baseDropDown = MyBase.CreateDefaultDropDown()
			If DesignMode Then
				Return baseDropDown
			End If

			Dim defaultDropDown = New MaterialContextMenuStrip()
			defaultDropDown.Items.AddRange(baseDropDown.Items)

			Return defaultDropDown
		End Function
	End Class

	Friend Class MaterialToolStripRender
		Inherits ToolStripProfessionalRenderer
		Implements IMaterialControl
		'Properties for managing the material design properties
		Public Property Depth() As Integer Implements IMaterialControl.Depth
			Get
				Return m_Depth
			End Get
			Set
				m_Depth = Value
			End Set
		End Property
		Private m_Depth As Integer
		Public ReadOnly Property SkinManager() As MaterialSkinManager Implements IMaterialControl.SkinManager
			Get
				Return MaterialSkinManager.Instance
			End Get
		End Property
		Public Property MouseState() As MouseState Implements IMaterialControl.MouseState
			Get
				Return m_MouseState
			End Get
			Set
				m_MouseState = Value
			End Set
		End Property
		Private m_MouseState As MouseState


		Protected Overrides Sub OnRenderItemText(e As ToolStripItemTextRenderEventArgs)
			Dim g = e.Graphics
			g.TextRenderingHint = TextRenderingHint.AntiAlias

			Dim itemRect = GetItemRect(e.Item)
			Dim textRect = New Rectangle(24, itemRect.Y, itemRect.Width - (24 + 16), itemRect.Height)
            g.DrawString(e.Text, SkinManager.ROBOTO_MEDIUM_10, If(e.Item.Enabled, SkinManager.GetPrimaryTextBrush(), SkinManager.GetDisabledOrHintBrush()), textRect, New StringFormat() With {
             .LineAlignment = StringAlignment.Center
            })
        End Sub

		Protected Overrides Sub OnRenderMenuItemBackground(e As ToolStripItemRenderEventArgs)
			Dim g = e.Graphics
			g.Clear(SkinManager.GetApplicationBackgroundColor())

			'Draw background
			Dim itemRect = GetItemRect(e.Item)
			g.FillRectangle(If(e.Item.Selected AndAlso e.Item.Enabled, SkinManager.GetCmsSelectedItemBrush(), New SolidBrush(SkinManager.GetApplicationBackgroundColor())), itemRect)

			'Ripple animation
			Dim toolStrip = TryCast(e.ToolStrip, MaterialContextMenuStrip)
			If toolStrip IsNot Nothing Then
				Dim animationManager = toolStrip.animationManager
				Dim animationSource = toolStrip.animationSource
				If toolStrip.animationManager.IsAnimating() AndAlso e.Item.Bounds.Contains(animationSource) Then
					For i As Integer = 0 To animationManager.GetAnimationCount() - 1
						Dim animationValue = animationManager.GetProgress(i)
						Dim rippleBrush = New SolidBrush(Color.FromArgb(CInt(Math.Truncate(51 - (animationValue * 50))), Color.Black))
						Dim rippleSize = CInt(Math.Truncate(animationValue * itemRect.Width * 2.5))
						g.FillEllipse(rippleBrush, New Rectangle(animationSource.X - rippleSize \ 2, itemRect.Y - itemRect.Height, rippleSize, itemRect.Height * 3))
					Next
				End If
			End If
		End Sub

		Protected Overrides Sub OnRenderImageMargin(e As ToolStripRenderEventArgs)
			'base.OnRenderImageMargin(e);
		End Sub

		Protected Overrides Sub OnRenderSeparator(e As ToolStripSeparatorRenderEventArgs)
			Dim g = e.Graphics

			g.FillRectangle(New SolidBrush(SkinManager.GetApplicationBackgroundColor()), e.Item.Bounds)
			g.DrawLine(New Pen(SkinManager.GetDividersColor()), New Point(e.Item.Bounds.Left, e.Item.Bounds.Height \ 2), New Point(e.Item.Bounds.Right, e.Item.Bounds.Height \ 2))
		End Sub

		Protected Overrides Sub OnRenderToolStripBorder(e As ToolStripRenderEventArgs)
			Dim g = e.Graphics

			g.DrawRectangle(New Pen(SkinManager.GetDividersColor()), New Rectangle(e.AffectedBounds.X, e.AffectedBounds.Y, e.AffectedBounds.Width - 1, e.AffectedBounds.Height - 1))
		End Sub

		Protected Overrides Sub OnRenderArrow(e As ToolStripArrowRenderEventArgs)
			Dim g = e.Graphics
			Const  ARROW_SIZE As Integer = 4

			Dim arrowMiddle = New Point(e.ArrowRectangle.X + e.ArrowRectangle.Width \ 2, e.ArrowRectangle.Y + e.ArrowRectangle.Height \ 2)
			Dim arrowBrush = If(e.Item.Enabled, SkinManager.GetPrimaryTextBrush(), SkinManager.GetDisabledOrHintBrush())
			Using arrowPath = New GraphicsPath()
                arrowPath.AddLines({New Point(arrowMiddle.X - ARROW_SIZE, arrowMiddle.Y - ARROW_SIZE), New Point(arrowMiddle.X, arrowMiddle.Y), New Point(arrowMiddle.X - ARROW_SIZE, arrowMiddle.Y + ARROW_SIZE)})
                arrowPath.CloseFigure()

				g.FillPath(arrowBrush, arrowPath)
			End Using
		End Sub

		Private Function GetItemRect(item As ToolStripItem) As Rectangle
			Return New Rectangle(0, item.ContentRectangle.Y, item.ContentRectangle.Width + 4, item.ContentRectangle.Height)
		End Function
	End Class
End Namespace
