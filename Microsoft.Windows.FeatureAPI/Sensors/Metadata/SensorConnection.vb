' Copyright (c) Microsoft Corporation.  All rights reserved.

Namespace Sensors
	''' <summary>
	''' Defines types of sensor device connections.
	''' </summary>
	Public Enum SensorConnectionType
		''' <summary>
		''' Invalid value for this enumeration.
		''' </summary>
		Invalid = -1

		''' <summary>
		''' Indicates that the sensor is built into the computer. 
		''' </summary>        
		Integrated = 0

		''' <summary>
		''' Indicates that the sensor is attached to the computer, such as through a peripheral device. 
		''' </summary>
		Attached = 1
		''' <summary>
		''' Indicates that the sensor is connected by external means, such as through a network connection. 
		''' </summary>
		External = 2
	End Enum
End Namespace
