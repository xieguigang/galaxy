' Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Runtime.InteropServices.ComTypes

Namespace Sensors
	''' <summary>
	''' Represents all the data from a single sensor data report.
	''' </summary>
	Public Class SensorReport
		''' <summary>
		''' Gets the time when the data report was generated.
		''' </summary>
		<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId := "TimeStamp")> _
		Public ReadOnly Property TimeStamp() As DateTime
			Get
				Return m_timeStamp
			End Get
		End Property

		''' <summary>
		''' Gets the data values in the report.
		''' </summary>
		Public ReadOnly Property Values() As SensorData
			Get
				Return sensorData
			End Get
		End Property

		''' <summary>
		''' Gets the sensor that is the source of this data report.
		''' </summary>
		Public ReadOnly Property Source() As Sensor
			Get
				Return originator
			End Get
		End Property

		#Region "implementation"
		Private sensorData As SensorData
		Private originator As Sensor
		Private m_timeStamp As New DateTime()

		Friend Shared Function FromNativeReport(originator As Sensor, iReport As ISensorDataReport) As SensorReport

			Dim systemTimeStamp As New SystemTime()
			iReport.GetTimestamp(systemTimeStamp)
			Dim ftTimeStamp As New FILETIME()
			SensorNativeMethods.SystemTimeToFileTime(systemTimeStamp, ftTimeStamp)
			Dim lTimeStamp As Long = (CLng(ftTimeStamp.dwHighDateTime) << 32) + CLng(ftTimeStamp.dwLowDateTime)
			Dim timeStamp As DateTime = DateTime.FromFileTime(lTimeStamp)

			Dim sensorReport As New SensorReport()
			sensorReport.originator = originator
            sensorReport.m_timeStamp = timeStamp
            sensorReport.sensorData = SensorData.FromNativeReport(originator.internalObject, iReport)

			Return sensorReport
		End Function
		#End Region

	End Class
End Namespace
