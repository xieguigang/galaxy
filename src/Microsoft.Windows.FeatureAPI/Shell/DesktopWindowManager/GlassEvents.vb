
Namespace Shell
	''' <summary>
	''' Event argument for The GlassAvailabilityChanged event
	''' </summary>
	Public Class AeroGlassCompositionChangedEventArgs
		Inherits EventArgs
		Friend Sub New(avialbility As Boolean)
			GlassAvailable = avialbility
		End Sub

		''' <summary>
		''' The new GlassAvailable state
		''' </summary>
		Public Property GlassAvailable() As Boolean
			Get
				Return m_GlassAvailable
			End Get
			Private Set
				m_GlassAvailable = Value
			End Set
		End Property
		Private m_GlassAvailable As Boolean

	End Class

End Namespace
