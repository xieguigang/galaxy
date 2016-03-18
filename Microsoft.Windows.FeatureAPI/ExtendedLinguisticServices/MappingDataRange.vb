' Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Collections.Generic
Imports System.Runtime.InteropServices

Namespace ExtendedLinguisticServices

	''' <summary>
	''' Contains text recognition results for a recognized text subrange. An array of structures of this type
	''' is retrieved by an ELS service in a <see cref="MappingPropertyBag">MappingPropertyBag</see> structure.
	''' </summary>
	Public Class MappingDataRange
		Friend _win32DataRange As Win32DataRange

		Friend Sub New()
		End Sub

		''' <summary>
		''' Index of the beginning of the subrange in the text, where 0 indicates the first character of the string
		''' passed to MappingService.RecognizeText or
		''' MappingService.BeginRecognizeText, instead of an offset to the
		''' index passed to the function in the index parameter. The value should be less than the entire length
		''' of the text.
		''' </summary>
		Public ReadOnly Property StartIndex() As Integer
			Get
				Return CInt(_win32DataRange._startIndex)
			End Get
		End Property

		''' <summary>
		''' Index of the end of the subrange in the text, where 0 indicates the first character at of the string
		''' passed to MappingService.RecognizeText or
		''' MappingService.BeginRecognizeText, instead of an offset to the
		''' index passed to the function in the index parameter. The value should be less than the entire length
		''' of the text.
		''' </summary>
		Public ReadOnly Property EndIndex() As Integer
			Get
				Return CInt(_win32DataRange._endIndex)
			End Get
		End Property

		''' <summary>
		''' The data retrieved as service output associated with the subrange. This data must be of the format indicated
		''' by the content type supplied in the <see cref="ContentType">ContentType</see> property.
		''' </summary>
		Public Function GetData() As Byte()
			Dim data As Byte() = New Byte(CInt(_win32DataRange._dataSize) - 1) {}
			If _win32DataRange._dataSize = 0 Then
				Return data
			End If
			If _win32DataRange._data = IntPtr.Zero Then
				Throw New LinguisticException(LinguisticException.InvalidData)
			End If
			Marshal.Copy(_win32DataRange._data, data, 0, CInt(_win32DataRange._dataSize))
			Return data
		End Function

		''' <summary>
		''' A string specifying the MIME content type of the data returned by <see cref="GetData()">GetData()</see>. Examples of
		''' content types are "text/plain", "text/html", and "text/css".
		'''
		''' <note>In Windows 7, the ELS services support only the content type "text/plain". A content type specification
		''' can be found at the IANA website: http://www.iana.org/assignments/media-types/text/ </note>
		''' </summary>
		Public ReadOnly Property ContentType() As String
			Get
				Return _win32DataRange._contentType
			End Get
		End Property

		''' <summary>
		''' Available action IDs for this data range. Usable for calling <see cref="MappingService.DoAction">MappingService.DoAction</see> or
		''' <see cref="MappingService.BeginDoAction">MappingService.BeginDoAction</see>.
		'''
		''' <note>In Windows 7, the ELS services do not expose any actions.</note>
		''' </summary>
		Public ReadOnly Property ActionIds() As IEnumerable(Of String)
			Get
				Dim actionIDs__1 As String() = InteropTools.UnpackStringArray(_win32DataRange._actionIDs, _win32DataRange._actionsCount)
				Return actionIDs__1
			End Get
		End Property

		''' <summary>
		''' Available action display names for this data range. These strings can be localized.
		'''
		''' <note>In Windows 7, the ELS services do not expose any actions.</note>
		''' </summary>
		Public ReadOnly Property ActionDisplayNames() As IEnumerable(Of String)
			Get
				Dim actionDisplayNames__1 As String() = InteropTools.UnpackStringArray(_win32DataRange._actionDisplayNames, _win32DataRange._actionsCount)
				Return actionDisplayNames__1
			End Get
		End Property

        ''' <summary>
        ''' Formats the low-level data contained in this <see cref="MappingDataRange">MappingDataRange</see> using an implementation of the
        ''' <see cref="IMappingFormatter(Of T)">IMappingFormatter</see> interface.
        ''' </summary>
        ''' <typeparam name="T">The type with which <see cref="IMappingFormatter(Of T)">IMappingFormatter</see> is parameterized.</typeparam>
        ''' <param name="formatter">The formatter to be used in the formatting.</param>
        ''' <returns>A formatted version of this <see cref="MappingDataRange">MappingDataRange</see>.</returns>
        Public Function FormatData(Of T)(formatter As IMappingFormatter(Of T)) As T
			If formatter Is Nothing Then
				Throw New ArgumentNullException("formatter")
			End If
			Return formatter.Format(Me)
		End Function
	End Class

End Namespace
