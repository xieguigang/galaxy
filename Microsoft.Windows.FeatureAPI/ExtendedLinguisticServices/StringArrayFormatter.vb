' Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Text

Namespace ExtendedLinguisticServices

	''' <summary>
	''' Converts registry-formatted data into string arrays.
	''' </summary>
	Public Class StringArrayFormatter
		Implements IMappingFormatter(Of String())
		Private Shared ReadOnly Separator As Char() = New Char() {ControlChars.NullChar}

		''' <summary>
		''' Converts a single <see cref="MappingDataRange">MappingDataRange</see> into a string array by splitting on each null character and
		''' removing the empty entries.
		''' </summary>
		''' <param name="dataRange">The <see cref="MappingDataRange">MappingDataRange</see> to convert</param>
		''' <returns>The resulting string array</returns>
		Public Function Format(dataRange As MappingDataRange) As String() Implements IMappingFormatter(Of String()).Format
			If dataRange Is Nothing Then
				Throw New ArgumentNullException("dataRange")
			End If

			Dim data As Byte() = dataRange.GetData()
			Dim resultText As String = Encoding.Unicode.GetString(data)
			Return resultText.Split(Separator, StringSplitOptions.RemoveEmptyEntries)
		End Function

		''' <summary>
		''' Uses <see cref="Format(MappingDataRange)">Format</see> to format all the ranges of the supplied
		''' <see cref="MappingPropertyBag">MappingPropertyBag</see>.
		''' </summary>
		''' <param name="bag">The property bag to convert.</param>
		''' <returns>An array of string arrays, one per <see cref="MappingDataRange">MappingDataRange</see>.</returns>
		Public Function FormatAll(bag As MappingPropertyBag) As String()() Implements IMappingFormatter(Of String()).FormatAll
			If bag Is Nothing Then
				Throw New ArgumentNullException("bag")
			End If

			Dim dataRanges As MappingDataRange() = bag.GetResultRanges()
			Dim results As String()() = New String(dataRanges.Length - 1)() {}
			For i As Integer = 0 To results.Length - 1
				results(i) = Format(dataRanges(i))
			Next
			Return results
		End Function
	End Class

End Namespace
