Imports System.Runtime.InteropServices

Namespace Shell.PropertySystem

	''' <summary>
	''' An exception thrown when an error occurs while dealing with the Property System API.
	''' </summary>
	<Serializable> _
	Public Class PropertySystemException
		Inherits ExternalException
		''' <summary>
		''' Default constructor.
		''' </summary>
		Public Sub New()
		End Sub

		''' <summary>
		''' Initializes an excpetion with a custom message.
		''' </summary>
		''' <param name="message"></param>
		Public Sub New(message As String)
			MyBase.New(message)
		End Sub

		''' <summary>
		''' Initializes an exception with custom message and inner exception.
		''' </summary>
		''' <param name="message"></param>
		''' <param name="innerException"></param>
		Public Sub New(message As String, innerException As Exception)
			MyBase.New(message, innerException)
		End Sub

		''' <summary>
		''' Initializes an exception with custom message and error code.
		''' </summary>
		''' <param name="message"></param>
		''' <param name="errorCode"></param>
		Public Sub New(message As String, errorCode As Integer)
			MyBase.New(message, errorCode)
		End Sub

		''' <summary>
		''' Initializes an exception from serialization info and a context.
		''' </summary>
		''' <param name="info"></param>
		''' <param name="context"></param>
		Protected Sub New(info As System.Runtime.Serialization.SerializationInfo, context As System.Runtime.Serialization.StreamingContext)
			MyBase.New(info, context)
		End Sub

	End Class
End Namespace
