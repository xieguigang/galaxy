' Copyright (c) Microsoft Corporation.  All rights reserved.


Namespace ExtendedLinguisticServices

	''' <summary>
	''' This class contains constants describing the existing ELS services for Windows 7.
	''' </summary>
	Public NotInheritable Class MappingAvailableServices
		Private Sub New()
		End Sub
		''' <summary>
		''' The guid of the Microsoft Language Detection service.
		''' </summary>
		Public Shared ReadOnly LanguageDetection As New Guid("{CF7E00B1-909B-4d95-A8F4-611F7C377702}")

		''' <summary>
		''' The guid of the Microsoft Script Detection service.
		''' </summary>
		Public Shared ReadOnly ScriptDetection As New Guid("{2D64B439-6CAF-4f6b-B688-E5D0F4FAA7D7}")

		''' <summary>
		''' The guid of the Microsoft Traditional Chinese to Simplified Chinese Transliteration service.
		''' </summary>        
		Public Shared ReadOnly TransliterationHantToHans As New Guid("{A3A8333B-F4FC-42f6-A0C4-0462FE7317CB}")

		''' <summary>
		''' The guid of the Microsoft Simplified Chinese to Traditional Chinese Transliteration service.
		''' </summary>
		Public Shared ReadOnly TransliterationHansToHant As New Guid("{3CACCDC8-5590-42dc-9A7B-B5A6B5B3B63B}")

		''' <summary>
		''' The guid of the Microsoft Malayalam to Latin Transliteration service.
		''' </summary>
		Public Shared ReadOnly TransliterationMalayalamToLatin As New Guid("{D8B983B1-F8BF-4a2b-BCD5-5B5EA20613E1}")

		''' <summary>
		''' The guid of the Microsoft Devanagari to Latin Transliteration service.
		''' </summary>        
		Public Shared ReadOnly TransliterationDevanagariToLatin As New Guid("{C4A4DCFE-2661-4d02-9835-F48187109803}")

		''' <summary>
		''' The guid of the Microsoft Cyrillic to Latin Transliteration service.
		''' </summary>
		Public Shared ReadOnly TransliterationCyrillicToLatin As New Guid("{3DD12A98-5AFD-4903-A13F-E17E6C0BFE01}")

		''' <summary>
		''' The guid of the Microsoft Bengali to Latin Transliteration service.
		''' </summary>
		Public Shared ReadOnly TransliterationBengaliToLatin As New Guid("{F4DFD825-91A4-489f-855E-9AD9BEE55727}")
	End Class

End Namespace
