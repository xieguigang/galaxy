Imports System.ComponentModel
Imports System.Windows.Forms

Namespace Controls
	Public NotInheritable Class MaterialDivider
		Inherits Control
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

		Public Sub New()
			SetStyle(ControlStyles.SupportsTransparentBackColor, True)
			Height = 1
			BackColor = SkinManager.GetDividersColor()
		End Sub
	End Class
End Namespace
