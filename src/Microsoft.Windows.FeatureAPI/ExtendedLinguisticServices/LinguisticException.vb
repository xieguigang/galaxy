' Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.ComponentModel
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Threading
Imports System.Runtime.ConstrainedExecution
Imports System.Collections.Generic
Imports System.Runtime.Serialization

Namespace ExtendedLinguisticServices

	''' <summary>
	''' This exception is thrown by the managed wrappers of ELS when the underlying
	''' unmanaged implementation returns an HResult which is not S_OK (0).
	''' This exception is also thrown when the managed wrapper detects an exceptional
	''' condition which causes it to fail. Please note that other .NET exceptions are also
	''' possible to be thrown from the ELS managed wrappers.
	''' </summary>
	<Serializable> _
	Public Class LinguisticException
		Inherits Win32Exception
		' Common HResult values.
		Friend Const InvalidArgs As UInteger = &H80070057UI
		Friend Const Fail As UInteger = &H80004005UI
		Friend Const InvalidData As UInteger = &H8007000dUI

		''' <summary>
		''' Initializes a new instance of the <see cref="LinguisticException">LinguisticException</see> class
		''' with the last Win32 error that occurred.
		''' </summary>
		Public Sub New()
		End Sub

		Friend Sub New(hResult__1 As UInt32)
			MyBase.New(CInt(hResult__1))
			HResult = CInt(hResult__1)
		End Sub

		''' <summary>
		''' Initializes a new instance of the <see cref="LinguisticException">LinguisticException</see> class
		'''  with the specified context and the serialization information.
		''' </summary>
		''' <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo">SerializationInfo</see> associated with this exception.</param>
		''' <param name="context">A <see cref="System.Runtime.Serialization.StreamingContext">StreamingContext</see> that represents the context of this exception.</param>
		Protected Sub New(info As SerializationInfo, context As StreamingContext)
			MyBase.New(info, context)
		End Sub

		''' <summary>
		''' Initializes a new instance of the <see cref="LinguisticException">LinguisticException</see> 
		''' class with the specified detailed description.
		''' </summary>
		''' <param name="message">A detailed description of the error.</param>
		Public Sub New(message As String)
			MyBase.New(message)
		End Sub

		''' <summary>
		''' Initializes a new instance of the <see cref="LinguisticException">LinguisticException</see> class
		''' with the specified detailed description and the specified exception.
		''' </summary>
		''' <param name="message">A detailed description of the error.</param>
		''' <param name="innerException">A reference to the inner exception that is the cause of this exception.</param>
		Public Sub New(message As String, innerException As Exception)
			MyBase.New(message, innerException)
		End Sub

		''' <summary>
		''' Gets the MappingResultState describing the error condition for this exception.
		''' </summary>
		Public ReadOnly Property ResultState() As MappingResultState
			Get
				Return New MappingResultState(HResult, Message)
			End Get
		End Property
	End Class

End Namespace
