'Copyright (c) Microsoft Corporation.  All rights reserved.


Namespace Dialogs
	''' <summary>
	''' The event data for a TaskDialogTick event.
	''' </summary>
	Public Class TaskDialogTickEventArgs
		Inherits EventArgs
        ''' <summary>
        ''' Initializes the data associated with the TaskDialog tick event.
        ''' </summary>
        ''' <param name="ticks__1">The total number of ticks since the control was activated.</param>
        Public Sub New(ticks__1 As Integer)
			Ticks = ticks__1
		End Sub

		''' <summary>
		''' Gets a value that determines the current number of ticks.
		''' </summary>
		Public Property Ticks() As Integer
			Get
				Return m_Ticks
			End Get
			Private Set
				m_Ticks = Value
			End Set
		End Property
		Private m_Ticks As Integer
	End Class
End Namespace
