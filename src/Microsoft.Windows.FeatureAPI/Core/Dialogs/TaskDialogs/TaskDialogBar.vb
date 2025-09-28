'Copyright (c) Microsoft Corporation.  All rights reserved.


Namespace Dialogs
	''' <summary>
	''' Defines a common class for all task dialog bar controls, such as the progress and marquee bars.
	''' </summary>
	Public Class TaskDialogBar
		Inherits TaskDialogControl
		''' <summary>
		''' Creates a new instance of this class.
		''' </summary>
		Public Sub New()
		End Sub
		''' <summary>
		''' Creates a new instance of this class with the specified name.
		''' </summary>
		''' <param name="name">The name for this control.</param>
		Protected Sub New(name As String)
			MyBase.New(name)
		End Sub

		Private m_state As TaskDialogProgressBarState
		''' <summary>
		''' Gets or sets the state of the progress bar.
		''' </summary>
		Public Property State() As TaskDialogProgressBarState
			Get
				Return m_state
			End Get
			Set
				CheckPropertyChangeAllowed("State")
				m_state = value
				ApplyPropertyChange("State")
			End Set
		End Property
		''' <summary>
		''' Resets the state of the control to normal.
		''' </summary>
		Protected Friend Overridable Sub Reset()
			m_state = TaskDialogProgressBarState.Normal
		End Sub
	End Class
End Namespace
