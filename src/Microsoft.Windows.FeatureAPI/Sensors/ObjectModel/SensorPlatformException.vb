' Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Runtime.Serialization

Namespace Sensors
	''' <summary>
	''' Defines an exception specific to the sensors.
	''' </summary>
	Public Class SensorPlatformException
		Inherits Exception
		''' <summary>
		''' Initializes a new instance of the Sensors.SensorPlatformException 
		''' class with the specified detailed description.
		''' </summary>
		''' <param name="message">A detailed description of the error.</param>
		Public Sub New(message As String)
			MyBase.New(message)
		End Sub

		''' <summary>
		''' Initializes a new instance of the Sensors.SensorPlatformException class
		''' with the last Win32 error that occurred.
		''' </summary>
		Public Sub New()
			MyBase.New()
		End Sub

		''' <summary>
		''' Initializes a new instance of the Sensors.SensorPlatformException class
		''' with the specified detailed description and the specified exception.
		''' </summary>
		''' <param name="message">A detailed description of the error.</param>
		''' <param name="innerException">A reference to the inner exception that is the cause of this exception.</param>
		Public Sub New(message As String, innerException As Exception)
			MyBase.New(message, innerException)
		End Sub
	End Class
End Namespace
