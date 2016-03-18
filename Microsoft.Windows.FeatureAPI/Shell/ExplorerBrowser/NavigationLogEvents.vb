'Copyright (c) Microsoft Corporation.  All rights reserved.


Namespace Controls
	''' <summary>
	''' The event argument for NavigationLogChangedEvent
	''' </summary>
	Public Class NavigationLogEventArgs
		Inherits EventArgs
		''' <summary>
		''' Indicates CanNavigateForward has changed
		''' </summary>
		Public Property CanNavigateForwardChanged() As Boolean
			Get
				Return m_CanNavigateForwardChanged
			End Get
			Set
				m_CanNavigateForwardChanged = Value
			End Set
		End Property
		Private m_CanNavigateForwardChanged As Boolean

		''' <summary>
		''' Indicates CanNavigateBackward has changed
		''' </summary>
		Public Property CanNavigateBackwardChanged() As Boolean
			Get
				Return m_CanNavigateBackwardChanged
			End Get
			Set
				m_CanNavigateBackwardChanged = Value
			End Set
		End Property
		Private m_CanNavigateBackwardChanged As Boolean

		''' <summary>
		''' Indicates the Locations collection has changed
		''' </summary>
		Public Property LocationsChanged() As Boolean
			Get
				Return m_LocationsChanged
			End Get
			Set
				m_LocationsChanged = Value
			End Set
		End Property
		Private m_LocationsChanged As Boolean
	End Class
End Namespace
