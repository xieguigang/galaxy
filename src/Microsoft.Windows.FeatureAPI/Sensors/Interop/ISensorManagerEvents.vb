' Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Runtime.InteropServices

Namespace Sensors
	''' <summary>
	''' A COM interop events interface for the ISensorManager object
	''' </summary>
	<ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("9B3B0B86-266A-4AAD-B21F-FDE5501001B7")> _
	Friend Interface ISensorManagerEvents
		Sub OnSensorEnter(<[In], MarshalAs(UnmanagedType.[Interface])> pSensor As ISensor, <[In], MarshalAs(UnmanagedType.U4)> state As NativeSensorState)
	End Interface

	''' <summary>
	''' A COM interop events interface for the ISensor object
	''' </summary>
	<ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("5D8DCC91-4641-47E7-B7C3-B74F48A6C391")> _
	Friend Interface ISensorEvents
		Sub OnStateChanged(<[In], MarshalAs(UnmanagedType.[Interface])> sensor As ISensor, <[In], MarshalAs(UnmanagedType.U4)> state As NativeSensorState)

		Sub OnDataUpdated(<[In], MarshalAs(UnmanagedType.[Interface])> sensor As ISensor, <[In], MarshalAs(UnmanagedType.[Interface])> newData As ISensorDataReport)

		Sub OnEvent(<[In], MarshalAs(UnmanagedType.[Interface])> sensor As ISensor, <[In], MarshalAs(UnmanagedType.LPStruct)> eventID As Guid, <[In], MarshalAs(UnmanagedType.[Interface])> newData As ISensorDataReport)

		Sub OnLeave(<[In], MarshalAs(UnmanagedType.LPStruct)> sensorID As Guid)
	End Interface

End Namespace
