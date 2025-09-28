' Copyright (c) Microsoft Corporation.  All rights reserved.


Namespace Sensors
	''' <summary>
	''' An attribute which is applied on <see cref="Sensor"/>-derived types. Provides essential metadata
	''' such as the GUID of the sensor type for which this wrapper was written.
	''' </summary>    
	<AttributeUsage(AttributeTargets.[Class])> _
	Public NotInheritable Class SensorDescriptionAttribute
		Inherits Attribute
		Private _sensorType As Guid

		''' <summary>
		''' Constructs the attribue with a string represening the sensor type GUID and the type of the data report class.
		''' </summary>
		''' <param name="sensorType">String representing the sensor type GUID.</param>
		Public Sub New(sensorType As String)
			' will throw if invalid format
			_sensorType = New Guid(sensorType)
		End Sub

		''' <summary>
		''' Gets a string representing the sensor type GUID.
		''' </summary>
		Public ReadOnly Property SensorType() As String
			Get
				Return _sensorType.ToString()
			End Get
		End Property

		''' <summary>
		''' Gets the GUID of the sensor type.
		''' </summary>
		Public ReadOnly Property SensorTypeGuid() As Guid
			Get
				Return _sensorType
			End Get
		End Property
	End Class
End Namespace
