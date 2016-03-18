' Copyright (c) Microsoft Corporation.  All rights reserved.


Namespace ExtendedLinguisticServices

	''' <summary>
	''' Contains options used to enumerate ELS services.
	''' </summary>
	Public Class MappingEnumOptions
		Friend _guid As Nullable(Of Guid)
		Friend _win32EnumOption As Win32EnumOptions

		''' <summary>
		''' Public constructor. Initializes an empty instance of <see cref="MappingEnumOptions">MappingEnumOptions</see>.
		''' </summary>
		Public Sub New()
			_win32EnumOption._size = InteropTools.SizeOfWin32EnumOptions
		End Sub

		''' <summary>
		''' Optional. A service category, for example, "Transliteration". The application must set this member to null
		''' if the service category is not a search criterion.
		''' </summary>
		Public Property Category() As String
			Get
				Return _win32EnumOption._category
			End Get
			Set
				_win32EnumOption._category = value
			End Set
		End Property

		''' <summary>
		''' Optional. An input language string, following the IETF naming convention, that identifies the input language
		''' that services should accept. The application can set this member to null if the supported input language is
		''' not a search criterion.
		''' </summary>
		Public Property InputLanguage() As String
			Get
				Return _win32EnumOption._inputLanguage
			End Get
			Set
				_win32EnumOption._inputLanguage = value
			End Set
		End Property

		''' <summary>
		''' Optional. An output language string, following the IETF naming convention, that identifies the output language
		''' that services use to retrieve results. The application can set this member to null if the output language is
		''' not a search criterion.
		''' </summary>
		Public Property OutputLanguage() As String
			Get
				Return _win32EnumOption._outputLanguage
			End Get
			Set
				_win32EnumOption._outputLanguage = value
			End Set
		End Property

		''' <summary>
		''' Optional. A standard Unicode script name that can be accepted by services. The application set this member to
		''' null if the input script is not a search criterion.
		''' </summary>
		Public Property InputScript() As String
			Get
				Return _win32EnumOption._inputScript
			End Get
			Set
				_win32EnumOption._inputScript = value
			End Set
		End Property

		''' <summary>
		''' Optional. A standard Unicode script name used by services. The application can set this member to
		''' null if the output script is not a search criterion.
		''' </summary>
		Public Property OutputScript() As String
			Get
				Return _win32EnumOption._outputScript
			End Get
			Set
				_win32EnumOption._outputScript = value
			End Set
		End Property

		''' <summary>
		''' Optional. A string, following the format of the MIME content types, that identifies the format that the
		''' services should be able to interpret when the application passes data. Examples of content types are
		''' "text/plain", "text/html", and "text/css". The application can set this member to null if the input content
		''' type is not a search criterion.
		'''
		''' <note>In Windows 7, the ELS services support only the content type "text/plain". A content type specification
		''' can be found at the IANA website: http://www.iana.org/assignments/media-types/text/ </note>
		''' </summary>
		Public Property InputContentType() As String
			Get
				Return _win32EnumOption._inputContentType
			End Get
			Set
				_win32EnumOption._inputContentType = value
			End Set
		End Property

		''' <summary>
		''' Optional. A string, following the format of the MIME content types, that identifies the format in which the
		''' services retrieve data. The application can set this member to null if the output content type is not a search
		''' criterion.
		''' </summary>
		Public Property OutputContentType() As String
			Get
				Return _win32EnumOption._outputContentType
			End Get
			Set
				_win32EnumOption._outputContentType = value
			End Set
		End Property

		''' <summary>
		''' Optional. A globally unique identifier (guid) structure for a specific service. The application must
		''' avoid setting this member at all if the guid is not a search criterion.
		''' </summary>
		Public Property Guid() As Nullable(Of Guid)
			Get
				Return _guid
			End Get
			Set
				_guid = value
			End Set
		End Property
	End Class

	''' <summary>
	''' Contains options for text recognition. The values stored in this structure affect the behavior and results
	''' of MappingRecognizeText.
	''' </summary>
	Public Class MappingOptions
		Friend _win32Options As Win32Options

		''' <summary>
		''' Public constructor. Initializes an empty instance of MappingOptions.
		''' </summary>
		Public Sub New()
			_win32Options._size = InteropTools.SizeOfWin32Options
		End Sub

		''' <summary>
		''' Optional. An input language string, following the IETF naming convention, that identifies the input language
		''' that the service should be able to accept. The application can set this member to null to indicate that
		''' the service is free to interpret the input as any input language it supports.
		''' </summary>
		Public Property InputLanguage() As String
			Get
				Return _win32Options._inputLanguage
			End Get
			Set
				_win32Options._inputLanguage = value
			End Set
		End Property

		''' <summary>
		''' Optional. An output language string, following the IETF naming convention, that identifies the output language
		''' that the service should be able to use to produce results. The application can set this member to null if
		''' the service should decide the output language.
		''' </summary>
		Public Property OutputLanguage() As String
			Get
				Return _win32Options._outputLanguage
			End Get
			Set
				_win32Options._outputLanguage = value
			End Set
		End Property

		''' <summary>
		''' Optional. A standard Unicode script name that should be accepted by the service. The application can set this
		''' member to null to let the service decide how handle the input.
		''' </summary>
		Public Property InputScript() As String
			Get
				Return _win32Options._inputScript
			End Get
			Set
				_win32Options._inputScript = value
			End Set
		End Property

		''' <summary>
		''' Optional. A standard Unicode script name that the service should use to retrieve results. The application can
		''' set this member to null to let the service decide the output script.
		''' </summary>
		Public Property OutputScript() As String
			Get
				Return _win32Options._outputScript
			End Get
			Set
				_win32Options._outputScript = value
			End Set
		End Property

		''' <summary>
		''' Optional. A string, following the format of the MIME content types, that identifies the format that the service
		''' should be able to interpret when the application passes data. Examples of content types are "text/plain",
		''' "text/html", and "text/css". The application can set this member to null to indicate the "text/plain"
		''' content type.
		'''
		''' <note>In Windows 7, the ELS services support only the content type "text/plain". A content type specification
		''' can be found at the IANA website: http://www.iana.org/assignments/media-types/text/ </note>
		''' </summary>
		Public Property InputContentType() As String
			Get
				Return _win32Options._inputContentType
			End Get
			Set
				_win32Options._inputContentType = value
			End Set
		End Property

		''' <summary>
		''' Optional. A string, following the format of the MIME content types, that identifies the format in which the
		''' service should retrieve data. The application can set this member to NULL to let the service decide the output
		''' content type.
		''' </summary>
		Public Property OutputContentType() As String
			Get
				Return _win32Options._outputContentType
			End Get
			Set
				_win32Options._outputContentType = value
			End Set
		End Property

		''' <summary>
		''' Optional. Private flag that a service provider defines to affect service behavior. Services can interpret this
		''' flag as they require.
		'''
		''' <note>For Windows 7, none of the available ELS services support flags.</note>
		''' </summary>
		<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId := "Flag")> _
		Public Property ServiceFlag() As Integer
			Get
				Return CInt(_win32Options._serviceFlag)
			End Get
			Set
				_win32Options._serviceFlag = CUInt(value)
			End Set
		End Property

	End Class

End Namespace
