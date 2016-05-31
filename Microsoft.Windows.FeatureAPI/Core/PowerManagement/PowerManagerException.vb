Imports System.Collections.Generic
Imports System.Text
Imports System.Runtime.InteropServices

Namespace ApplicationServices

    ''' <summary>
    ''' This exception is thrown when there are problems with getting piece of data within PowerManager.
    ''' </summary>
    <Serializable>
    Public Class PowerManagerException
        Inherits Exception

        ''' <summary>
        ''' Default constructor.
        ''' </summary>
        Public Sub New()
        End Sub

        ''' <summary>
        ''' Initializes an excpetion with a custom message.
        ''' </summary>
        ''' <param name="message">A custom message for the exception.</param>
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub

        ''' <summary>
        ''' Initializes an exception with custom message and inner exception.
        ''' </summary>
        ''' <param name="message">A custom message for the exception.</param>
        ''' <param name="innerException">An inner exception on which to base this exception.</param>
        Public Sub New(message As String, innerException As Exception)
            MyBase.New(message, innerException)
        End Sub

        ''' <summary>
        ''' Initializes an exception from serialization info and a context.
        ''' </summary>
        ''' <param name="info">SerializationInfo for the exception.</param>
        ''' <param name="context">StreamingContext for the exception.</param>
        Protected Sub New(info As System.Runtime.Serialization.SerializationInfo, context As System.Runtime.Serialization.StreamingContext)
            MyBase.New(info, context)
        End Sub

    End Class
End Namespace
