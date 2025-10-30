Imports System.Runtime.InteropServices
Imports Microsoft.Windows.Internal

Namespace Shell

    ''' <summary>
    ''' An exception thrown when an error occurs while dealing with ShellObjects.
    ''' </summary>
    <Serializable>
    Public Class ShellException
        Inherits ExternalException
        ''' <summary>
        ''' Default constructor.
        ''' </summary>
        Public Sub New()
        End Sub

        ''' <summary>
        ''' Initializes a new exception using an HResult
        ''' </summary>
        ''' <param name="result">HResult error</param>
        Friend Sub New(result As HResult)
            Me.New(CInt(result))
        End Sub

        ''' <summary>
        ''' Initializes an excpetion with a custom message.
        ''' </summary>
        ''' <param name="message">Custom message</param>
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub

        ''' <summary>
        ''' Initializes an exception with custom message and inner exception.
        ''' </summary>
        ''' <param name="message">Custom message</param>
        ''' <param name="innerException">The original exception that preceded this exception</param>
        Public Sub New(message As String, innerException As Exception)
            MyBase.New(message, innerException)
        End Sub

        ''' <summary>
        ''' Initializes an exception with custom message and error code.
        ''' </summary>
        ''' <param name="message">Custom message</param>
        ''' <param name="errorCode">HResult error code</param>
        Public Sub New(message As String, errorCode As Integer)
            MyBase.New(message, errorCode)
        End Sub

        ''' <summary>
        ''' Initializes an exception with custom message and error code.
        ''' </summary>
        ''' <param name="message"></param>
        ''' <param name="errorCode"></param>
        Friend Sub New(message As String, errorCode As HResult)
            Me.New(message, CInt(errorCode))
        End Sub

        ''' <summary>
        ''' Initializes an exception with custom message and inner exception.
        ''' </summary>
        ''' <param name="errorCode">HRESULT of an operation</param>        
        Public Sub New(errorCode As Integer)
            MyBase.New(LocalizedMessages.ShellExceptionDefaultText, errorCode)
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
