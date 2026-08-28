Imports System.Runtime.InteropServices

Namespace ApplicationServices
	''' <summary>
	''' This exception is thrown when there are problems with registering, unregistering or updating
	''' applications using Application Restart Recovery.
	''' </summary>
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

	End Class
End Namespace
