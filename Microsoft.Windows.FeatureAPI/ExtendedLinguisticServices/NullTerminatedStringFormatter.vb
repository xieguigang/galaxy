' Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Text

Namespace ExtendedLinguisticServices

	''' <summary>
	''' Converts byte arrays containing Unicode null-terminated strings into .NET string objects.
	''' </summary>
	Public Class NullTerminatedStringFormatter
		Implements IMappingFormatter(Of String)
		''' <summary>
		''' Converts a single <see cref="MappingDataRange">MappingDataRange</see> into a string, stripping the trailing null character.
		''' If the string doesn't contain null characters, the empty string is returned.
		''' </summary>
		''' <param name="dataRange">The <see cref="MappingDataRange">MappingDataRange</see> to convert</param>
		''' <returns>The resulting string</returns>
		Public Function Format(dataRange As MappingDataRange) As String Implements IMappingFormatter(Of String).Format
			If dataRange Is Nothing Then
				Throw New ArgumentNullException("dataRange")
			End If

			Dim data As Byte() = dataRange.GetData()
			If (data.Length And 1) <> 0 Then
				Throw New LinguisticException(LinguisticException.InvalidArgs)
			End If

			Dim nullIndex As Integer = data.Length
			For i As Integer = 0 To data.Length - 1 Step 2
				If data(i) = 0 AndAlso data(i + 1) = 0 Then
					nullIndex = i
					Exit For
				End If
			Next

			Dim resultText As String = Encoding.Unicode.GetString(data, 0, nullIndex)
			Return resultText
		End Function

		''' <summary>
		''' Uses <see cref="Format(MappingDataRange)">Format</see> to format all the ranges of the supplied
		''' <see cref="MappingPropertyBag">MappingPropertyBag</see>.
		''' </summary>
		''' <param name="bag">The property bag to convert.</param>
		''' <returns>An array of strings, one per <see cref="MappingDataRange">MappingDataRange</see>.</returns>
		Public Function FormatAll(bag As MappingPropertyBag) As String() Implements IMappingFormatter(Of String).FormatAll
			If bag Is Nothing Then
				Throw New ArgumentNullException("bag")
			End If

			Dim dataRanges As MappingDataRange() = bag.GetResultRanges()
			Dim results As String() = New String(dataRanges.Length - 1) {}
			For i As Integer = 0 To results.Length - 1
				results(i) = Format(dataRanges(i))
			Next
			Return results
		End Function
	End Class

End Namespace
