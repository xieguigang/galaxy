Interface IMaterialControl
	Property Depth() As Integer
	ReadOnly Property SkinManager() As MaterialSkinManager
	Property MouseState() As MouseState

End Interface

Public Enum MouseState
	HOVER
	DOWN
	OUT
End Enum
