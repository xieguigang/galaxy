' Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Text

Namespace ExtendedLinguisticServices

	''' <summary>
	''' Converts byte arrays into Unicode (UTF-16) strings.
	''' </summary>
	Public Class StringFormatter
		Implements IMappingFormatter(Of String)
		''' <summary>
		''' Converts a single <see cref="MappingDataRange">MappingDataRange</see> into a string.
		''' </summary>
		''' <param name="dataRange">The <see cref="MappingDataRange">MappingDataRange</see> to convert</param>
		''' <returns>The resulting string</returns>
		Public Function Format(dataRange As MappingDataRange) As String Implements IMappingFormatter(Of String).Format
			If dataRange Is Nothing Then
				Throw New ArgumentNullException("dataRange")
			End If

			Dim data As Byte() = dataRange.GetData()
			Dim resultText As String = Encoding.Unicode.GetString(data)
			Return resultText
		End Function

		''' <summary>
		''' Uses <see cref="Format(MappingDataRange)">Format</see> to format all the ranges of the supplied
		''' MappingPropertyBag.
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
