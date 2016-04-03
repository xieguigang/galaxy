Imports System.ComponentModel
Imports System.Windows.Forms

Namespace Controls
	''' <summary>
	''' Material design-like progress bar
	''' </summary>
	Public Class MaterialProgressBar
		Inherits ProgressBar
		Implements IMaterialControl
		''' <summary>
		''' Initializes a new instance of the <see cref="MaterialProgressBar"/> class.
		''' </summary>
		Public Sub New()
			SetStyle(ControlStyles.UserPaint, True)
			SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
		End Sub

		''' <summary>
		''' Gets or sets the depth.
		''' </summary>
		''' <value>
		''' The depth.
		''' </value>
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

		''' <summary>
		''' Gets the skin manager.
		''' </summary>
		''' <value>
		''' The skin manager.
		''' </value>
		<Browsable(False)> _
		Public ReadOnly Property SkinManager() As MaterialSkinManager Implements IMaterialControl.SkinManager
			Get
				Return MaterialSkinManager.Instance
			End Get
		End Property

		''' <summary>
		''' Gets or sets the state of the mouse.
		''' </summary>
		''' <value>
		''' The state of the mouse.
		''' </value>
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

		''' <summary>
		''' Performs the work of setting the specified bounds of this control.
		''' </summary>
		''' <param name="x">The new <see cref="P:System.Windows.Forms.Control.Left" /> property value of the control.</param>
		''' <param name="y">The new <see cref="P:System.Windows.Forms.Control.Top" /> property value of the control.</param>
		''' <param name="width">The new <see cref="P:System.Windows.Forms.Control.Width" /> property value of the control.</param>
		''' <param name="height">The new <see cref="P:System.Windows.Forms.Control.Height" /> property value of the control.</param>
		''' <param name="specified">A bitwise combination of the <see cref="T:System.Windows.Forms.BoundsSpecified" /> values.</param>
		Protected Overrides Sub SetBoundsCore(x As Integer, y As Integer, width As Integer, height As Integer, specified As BoundsSpecified)
			MyBase.SetBoundsCore(x, y, width, 5, specified)
		End Sub

		''' <summary>
		''' Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event.
		''' </summary>
		''' <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
		Protected Overrides Sub OnPaint(e As PaintEventArgs)
			Dim doneProgress = CInt(Math.Truncate(e.ClipRectangle.Width * (CDbl(Value) / Maximum)))
			e.Graphics.FillRectangle(SkinManager.ColorScheme.PrimaryBrush, 0, 0, doneProgress, e.ClipRectangle.Height)
			e.Graphics.FillRectangle(SkinManager.GetDisabledOrHintBrush(), doneProgress, 0, e.ClipRectangle.Width, e.ClipRectangle.Height)
		End Sub
	End Class
End Namespace
