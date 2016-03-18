' Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices

Namespace Sensors
	''' <summary>
	''' A COM interop wrapper for the ISensorCollection interface
	''' </summary>
	<ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("23571E11-E545-4DD8-A337-B89BF44B10DF")> _
	Friend Interface ISensorCollection
		''' <summary>
		''' Get a sensor by index
		''' </summary>
		''' <param name="ulIndex">The index for the sensor to retrieve</param>
		''' <param name="ppSensor">The sensor retrieved</param>
		Sub GetAt(<[In]> ulIndex As UInteger, <Out> ByRef ppSensor As ISensor)

		''' <summary>
		''' Get the sensor count for the collection
		''' </summary>
		''' <param name="pCount">The count returned</param>
		Sub GetCount(<Out> ByRef pCount As UInteger)

		''' <summary>
		''' Add a sensor to the collection
		''' </summary>
		''' <param name="pSensor">The sensor to add</param>
		Sub Add(<[In], MarshalAs(UnmanagedType.IUnknown)> pSensor As ISensor)

		''' <summary>
		''' Remove a sensor from the collection
		''' </summary>
		''' <param name="pSensor">The sensor to remove</param>
		Sub Remove(<[In]> pSensor As ISensor)

		''' <summary>
		''' Remove a sensor from the collection
		''' </summary>
		''' <param name="sensorID">Remove sensor by ID</param>
		Sub RemoveByID(<[In], MarshalAs(UnmanagedType.LPStruct)> sensorID As Guid)

		''' <summary>
		''' Clear the collection
		''' </summary>
		Sub Clear()
	End Interface

	''' <summary>
	''' A COM interop wrapper for the SensorCollection class
	''' </summary>
	<ComImport, Guid("79C43ADB-A429-469F-AA39-2F2B74B75937"), ClassInterface(ClassInterfaceType.None), TypeLibType(TypeLibTypeFlags.FCanCreate)> _
	Friend Class SensorCollection
		Implements ISensorCollection
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub GetAt(<[In]> index As UInteger, <MarshalAs(UnmanagedType.[Interface])> ByRef ppSensor As ISensor) Implements ISensorCollection.GetAt
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub GetCount(ByRef pCount As UInteger) Implements ISensorCollection.GetCount
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub Add(<MarshalAs(UnmanagedType.[Interface])> pSensor As ISensor) Implements ISensorCollection.Add
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub Remove(<MarshalAs(UnmanagedType.[Interface])> pSensor As ISensor) Implements ISensorCollection.Remove
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub RemoveByID(<[In], MarshalAs(UnmanagedType.LPStruct)> sensorId As Guid) Implements ISensorCollection.RemoveByID
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub Clear() Implements ISensorCollection.Clear
        End Sub
    End Class
End Namespace
