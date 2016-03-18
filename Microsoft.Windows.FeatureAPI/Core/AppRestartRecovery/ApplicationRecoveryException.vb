Imports System.Runtime.InteropServices

Namespace ApplicationServices
	''' <summary>
	''' This exception is thrown when there are problems with registering, unregistering or updating
	''' applications using Application Restart Recovery.
	''' </summary>
	<Serializable> _
	Public Class ApplicationRecoveryException
		Inherits ExternalException
		''' <summary>
		''' Default constructor.
		''' </summary>
		Public Sub New()
		End Sub

		''' <summary>
		''' Initializes an exception with a custom message.
		''' </summary>
		''' <param name="message">A custom message for the exception.</param>
		Public Sub New(message As String)
			MyBase.New(message)
		End Sub

		''' <summary>
		''' Initializes an exception with custom message and inner exception.
		''' </summary>
		''' <param name="message">A custom message for the exception.</param>
		''' <param name="innerException">Inner exception.</param>
		Public Sub New(message As String, innerException As Exception)
				' Empty
			MyBase.New(message, innerException)
		End Sub

		''' <summary>
		''' Initializes an exception with custom message and error code.
		''' </summary>
		''' <param name="message">A custom message for the exception.</param>
		''' <param name="errorCode">An error code (hresult) from which to generate the exception.</param>
		Public Sub New(message As String, errorCode As Integer)
			MyBase.New(message, errorCode)
		End Sub

		''' <summary>
		''' Initializes an exception from serialization info and a context.
		''' </summary>
		''' <param name="info">Serialization info from which to create exception.</param>
		''' <param name="context">Streaming context from which to create exception.</param>
		Protected Sub New(info As System.Runtime.Serialization.SerializationInfo, context As System.Runtime.Serialization.StreamingContext)
				' Empty
			MyBase.New(info, context)
		End Sub

	End Class
End Namespace
