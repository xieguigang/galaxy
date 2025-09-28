'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports Microsoft.Windows.Resources

Namespace Dialogs
	''' <summary>
	''' Provides a visual representation of the progress of a long running operation.
	''' </summary>
	Public Class TaskDialogProgressBar
		Inherits TaskDialogBar
		''' <summary>
		''' Creates a new instance of this class.
		''' </summary>
		Public Sub New()
		End Sub

		''' <summary>
		''' Creates a new instance of this class with the specified name.
		''' And using the default values: Min = 0, Max = 100, Current = 0
		''' </summary>
		''' <param name="name">The name of the control.</param>        
		Public Sub New(name As String)
			MyBase.New(name)
		End Sub

        ''' <summary>
        ''' Creates a new instance of this class with the specified 
        ''' minimum, maximum and current values.
        ''' </summary>
        ''' <param name="minimum__1">The minimum value for this control.</param>
        ''' <param name="maximum__2">The maximum value for this control.</param>
        ''' <param name="value__3">The current value for this control.</param>        
        Public Sub New(minimum__1 As Integer, maximum__2 As Integer, value__3 As Integer)
			Minimum = minimum__1
			Maximum = maximum__2
			Value = value__3
		End Sub

		Private _minimum As Integer
		Private _value As Integer
		Private _maximum As Integer = TaskDialogDefaults.ProgressBarMaximumValue

		''' <summary>
		''' Gets or sets the minimum value for the control.
		''' </summary>                
		Public Property Minimum() As Integer
			Get
				Return _minimum
			End Get
			Set
				CheckPropertyChangeAllowed("Minimum")

				' Check for positive numbers
				If value < 0 Then
					Throw New System.ArgumentException(LocalizedMessages.TaskDialogProgressBarMinValueGreaterThanZero, "value")
				End If

				' Check if min / max differ
				If value >= Maximum Then
					Throw New System.ArgumentException(LocalizedMessages.TaskDialogProgressBarMinValueLessThanMax, "value")
				End If

				_minimum = value
				ApplyPropertyChange("Minimum")
			End Set
		End Property
		''' <summary>
		''' Gets or sets the maximum value for the control.
		''' </summary>
		Public Property Maximum() As Integer
			Get
				Return _maximum
			End Get
			Set
				CheckPropertyChangeAllowed("Maximum")

				' Check if min / max differ
				If value < Minimum Then
					Throw New System.ArgumentException(LocalizedMessages.TaskDialogProgressBarMaxValueGreaterThanMin, "value")
				End If
				_maximum = value
				ApplyPropertyChange("Maximum")
			End Set
		End Property
		''' <summary>
		''' Gets or sets the current value for the control.
		''' </summary>
		Public Property Value() As Integer
			Get
				Return Me._value
			End Get
			Set
				CheckPropertyChangeAllowed("Value")
				' Check for positive numbers
				If value < Minimum OrElse value > Maximum Then
					Throw New System.ArgumentException(LocalizedMessages.TaskDialogProgressBarValueInRange, "value")
				End If
				Me._value = value
				ApplyPropertyChange("Value")
			End Set
		End Property

		''' <summary>
		''' Verifies that the progress bar's value is between its minimum and maximum.
		''' </summary>
		Friend ReadOnly Property HasValidValues() As Boolean
			Get
				Return _minimum <= _value AndAlso _value <= _maximum
			End Get
		End Property

		''' <summary>
		''' Resets the control to its minimum value.
		''' </summary>
		Protected Friend Overrides Sub Reset()
			MyBase.Reset()
			_value = _minimum
		End Sub
	End Class
End Namespace
