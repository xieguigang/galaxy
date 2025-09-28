' Copyright (c) Microsoft Corporation.  All rights reserved.


Namespace ExtendedLinguisticServices

	''' <summary>
	''' This class serves as the result status of asynchronous calls to ELS and
	''' as the result status of linguistic exceptions.
	''' </summary>
	Public Structure MappingResultState
		Private _hResult As Integer
		Private _errorMessage As String

		Friend Sub New(hResult As Integer, errorMessage As String)
			_hResult = hResult
			_errorMessage = errorMessage
		End Sub

		''' <summary>
		''' Gets a human-readable description of the HResult error code,
		''' as constructed by <see cref="System.ComponentModel.Win32Exception">Win32Exception</see>.
		''' </summary>
		Public ReadOnly Property ErrorMessage() As String
			Get
				Return _errorMessage
			End Get
		End Property

		''' <summary>
		''' Gets the HResult error code associated with this structure.
		''' </summary>
		Public ReadOnly Property HResult() As Integer
			Get
				Return _hResult
			End Get
		End Property

		''' <summary>
		''' Uses the HResult param as the object hashcode.
		''' </summary>
		''' <returns>An integer hashcode</returns>
		Public Overrides Function GetHashCode() As Integer
			Return _hResult
		End Function

		''' <summary>
		''' Compares an Object for value equality.
		''' </summary>
		''' <param name="obj">Object to compare.</param>
		''' <returns>True if obj is equal to this instance, false otherwise.</returns>
		Public Overrides Function Equals(obj As [Object]) As Boolean
			If obj Is Nothing Then
				Return False
			End If

			If [Object].ReferenceEquals(obj, Me) Then
				Return True
			End If

			If TypeOf obj Is MappingResultState Then
				Return Equals(CType(obj, MappingResultState))
			End If

			Return False
		End Function

		''' <summary>
		''' Compares a <see cref="MappingResultState">MappingResultState</see> obj for value equality.
		''' </summary>
		''' <param name="obj"><see cref="MappingResultState">MappingResultState</see> to compare.</param>
		''' <returns>True if obj is equal to this instance, false otherwise.</returns>
		Public Overloads Function Equals(obj As MappingResultState) As Boolean
			Return obj._hResult = Me._hResult
		End Function

		''' <summary>
		''' Compares two <see cref="MappingResultState">MappingResultState</see> objs for value equality.
		''' </summary>
		''' <param name="one">First <see cref="MappingResultState">MappingResultState</see> to compare.</param>
		''' <param name="two">Second <see cref="MappingResultState">MappingResultState</see> to compare.</param>
		''' <returns>True if the two <see cref="MappingResultState">MappingResultStates</see> are equal, false otherwise.</returns>
		Public Shared Operator =(one As MappingResultState, two As MappingResultState) As Boolean
			Return one.Equals(two)
		End Operator

		''' <summary>
		''' Compares two <see cref="MappingResultState">MappingResultState</see> objs against value equality.
		''' </summary>
		''' <param name="one">First <see cref="MappingResultState">MappingResultState</see> to compare.</param>
		''' <param name="two">Second <see cref="MappingResultState">MappingResultState</see> to compare.</param>
		''' <returns>True if the two <see cref="MappingResultState">MappingResultStates</see> are not equal, false otherwise.</returns>
		Public Shared Operator <>(one As MappingResultState, two As MappingResultState) As Boolean
			Return Not one.Equals(two)
		End Operator
	End Structure

End Namespace
