' Copyright (c) Microsoft Corporation.  All rights reserved.

Namespace Sensors
	''' <summary>
	''' Represents a 3D accelerometer.
	''' </summary>
	<SensorDescription("C2FB0F5F-E2D2-4C78-BCD0-352A9582819D")> _
	Public Class Accelerometer3D
		Inherits Sensor
		''' <summary>
		''' Gets the current acceleration in G's. 
		''' </summary>
		Public ReadOnly Property CurrentAcceleration() As Acceleration3D
			Get
				Return New Acceleration3D(Me.DataReport)
			End Get
		End Property
	End Class

	''' <summary>
	''' Specifies the axis of the acceleration measurement.
	''' </summary>
	Public Enum AccelerationAxis
		''' <summary>
		''' The x-axis.
		''' </summary>        
		XAxis = 0
		''' <summary>
		''' The y-axis.
		''' </summary>        
		YAxis = 1
		''' <summary>
		''' THe z-axis.
		''' </summary>        
		ZAxis = 2
	End Enum

	''' <summary>
	''' Creates an acceleration measurement from the data in the report.
	''' </summary>
	Public Class Acceleration3D
		''' <summary>
		''' Creates a new instance of this class.
		''' </summary>
		''' <param name="report">The sensor report to evaluate.</param>
		Public Sub New(report As SensorReport)
			If report Is Nothing Then
				Throw New ArgumentNullException("report")
			End If

			Me.acceleration(CInt(AccelerationAxis.XAxis)) = CSng(report.Values(SensorPropertyKeys.SensorDataTypeAccelerationXG.FormatId)(0))
			Me.acceleration(CInt(AccelerationAxis.YAxis)) = CSng(report.Values(SensorPropertyKeys.SensorDataTypeAccelerationYG.FormatId)(1))
			Me.acceleration(CInt(AccelerationAxis.ZAxis)) = CSng(report.Values(SensorPropertyKeys.SensorDataTypeAccelerationZG.FormatId)(2))
		End Sub

		''' <summary>
		''' Gets the acceleration reported by the sensor.
		''' </summary>
		''' <param name="axis">The axis of the acceleration.</param>
		''' <returns></returns>
		Public Default ReadOnly Property Item(axis As AccelerationAxis) As Single
			Get
				Return acceleration(CInt(axis))
			End Get
		End Property
		Private acceleration As Single() = New Single(2) {}
	End Class
End Namespace
