' Copyright (c) Microsoft Corporation.  All rights reserved.


Namespace Sensors
	''' <summary>
	''' Represents a generic ambient light sensor.
	''' </summary>
	<SensorDescription("97F115C8-599A-4153-8894-D2D12899918A")> _
	Public Class AmbientLightSensor
		Inherits Sensor
		''' <summary>
		''' Gets an array representing the light response curve.
		''' </summary>
		''' <returns>Array representing the light response curve.</returns>
		Public Function GetLightResponseCurve() As UInteger()
			Return DirectCast(GetProperty(SensorPropertyKeys.SensorPropertyLightResponseCurve), UInteger())
		End Function

		''' <summary>
		''' Gets the current luminous intensity of the sensor.
		''' </summary>
		Public ReadOnly Property CurrentLuminousIntensity() As LuminousIntensity
			Get
				Return New LuminousIntensity(Me.DataReport)
			End Get
		End Property

	End Class

	''' <summary>
	''' Defines a luminous intensity measurement. 
	''' </summary>
	Public Class LuminousIntensity
        ''' <summary>
        ''' Initializes a sensor report to obtain a luminous intensity value.
        ''' </summary>
        ''' <param name="report">The report name.</param>
        Public Sub New(report As SensorReport)
            If report Is Nothing Then
                Throw New ArgumentNullException("report")
            End If

            If report.Values IsNot Nothing AndAlso report.Values.ContainsKey(SensorPropertyKeys.SensorDataTypeLightLux.FormatId) Then
                Intensity = CSng(report.Values(SensorPropertyKeys.SensorDataTypeLightLux.FormatId)(0))
            End If
        End Sub
        ''' <summary>
        ''' Gets the intensity of the light in lumens.
        ''' </summary>
        Public Property Intensity() As Single
			Get
				Return m_Intensity
			End Get
			Private Set
				m_Intensity = Value
			End Set
		End Property
		Private m_Intensity As Single
	End Class
End Namespace
