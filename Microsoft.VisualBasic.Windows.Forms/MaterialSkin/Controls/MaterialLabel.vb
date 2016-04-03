Imports System.ComponentModel
Imports System.Windows.Forms

Namespace Controls
	Public Class MaterialLabel
		Inherits Label
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
		Protected Overrides Sub OnCreateControl()
			MyBase.OnCreateControl()

			ForeColor = SkinManager.GetPrimaryTextColor()
			Font = SkinManager.ROBOTO_REGULAR_11

			AddHandler BackColorChanged, Sub(sender, args) InlineAssignHelper(ForeColor, SkinManager.GetPrimaryTextColor())
		End Sub
		Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
			target = value
			Return value
		End Function
	End Class
End Namespace
