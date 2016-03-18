' Copyright (c) Microsoft Corporation.  All rights reserved.


Namespace ExtendedLinguisticServices

	''' <summary>
	''' A <see cref="MappingAsyncResult">MappingAsyncResult</see> subclass to be used only for asynchronous calls to text recognition.
	''' <seealso cref="MappingService.BeginDoAction">MappingService.BeginDoAction</seealso>
	''' </summary>
	Public Class MappingRecognizeAsyncResult
		Inherits MappingAsyncResult
		Friend Sub New(callerData As Object, asyncCallback As AsyncCallback, text__1 As String, length__2 As Integer, index__3 As Integer, options__4 As MappingOptions)
			MyBase.New(callerData, asyncCallback)
			Text = text__1
			Length = length__2
			Index = index__3
			Options = options__4
		End Sub

		''' <summary>
		''' Gets the text parameter for MappingService.RecognizeText or MappingService.BeginRecognizeText.
		''' </summary>        
		Public Property Text() As String
			Get
				Return m_Text
			End Get
			Private Set
				m_Text = Value
			End Set
		End Property
		Private m_Text As String

		''' <summary>
		''' Gets the length parameter for MappingService.RecognizeText or MappingService.BeginRecognizeText.
		''' </summary>        
		Public Property Length() As Integer
			Get
				Return m_Length
			End Get
			Private Set
				m_Length = Value
			End Set
		End Property
		Private m_Length As Integer

		''' <summary>
		''' Gets the index parameter for MappingService.RecognizeText or MappingService.BeginRecognizeText.
		''' </summary>        
		Public Property Index() As Integer
			Get
				Return m_Index
			End Get
			Private Set
				m_Index = Value
			End Set
		End Property
		Private m_Index As Integer

		''' <summary>
		''' Gets the options parameter for MappingService.RecognizeText or MappingService.BeginRecognizeText.
		''' </summary>        
		Public Property Options() As MappingOptions
			Get
				Return m_Options
			End Get
			Private Set
				m_Options = Value
			End Set
		End Property
		Private m_Options As MappingOptions
	End Class

End Namespace
