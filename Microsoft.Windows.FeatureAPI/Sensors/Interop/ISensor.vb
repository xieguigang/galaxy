' Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Runtime.InteropServices
Imports Microsoft.Windows.Internal
Imports Microsoft.Windows.Shell.PropertySystem


Namespace Sensors

    ''' <summary>
    ''' A COM interop wrapper for the ISensor interface.
    ''' </summary>
    <ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("5FA08F80-2657-458E-AF75-46F73FA6AC5C")>
    Friend Interface ISensor
        ''' <summary>
        ''' Unique ID of sensor within the sensors platform
        ''' </summary>
        ''' <param name="id">The unique ID to be returned</param>
        <PreserveSig>
        Function GetID(ByRef id As Guid) As HResult

        ''' <summary>
        ''' Category of sensor Ex: Location
        ''' </summary>
        ''' <param name="sensorCategory">The sensor category to be returned</param>
        <PreserveSig>
        Function GetCategory(ByRef sensorCategory As Guid) As HResult

        ''' <summary>
        ''' Specific type of sensor: Ex: IPLocationSensor
        ''' </summary>
        ''' <param name="sensorType">The sensor Type to be returned</param>
        <PreserveSig>
        Function [GetType](ByRef sensorType As Guid) As HResult

        ''' <summary>
        ''' Human readable name for sensor
        ''' </summary>
        ''' <param name="friendlyName">The friendly name for the sensor</param>
        <PreserveSig>
        Function GetFriendlyName(<Out, MarshalAs(UnmanagedType.BStr)> ByRef friendlyName As String) As HResult

        ''' <summary>
        ''' Sensor metadata: make, model, serial number, etc
        ''' </summary>
        ''' <param name="key">The PROPERTYKEY for the property to be retrieved</param>
        ''' <param name="property">The property returned</param>
        <PreserveSig>
        Function GetProperty(<[In]> ByRef key As PropertyKey, <Out> [property] As PropVariant) As HResult

        ''' <summary>
        ''' Bulk Sensor metadata query: make, model, serial number, etc
        ''' </summary>
        ''' <param name="keys">The PROPERTYKEY collection for the properties to be retrieved</param>
        ''' <param name="properties">The properties returned</param>
        <PreserveSig>
        Function GetProperties(<[In], MarshalAs(UnmanagedType.[Interface])> keys As IPortableDeviceKeyCollection, <Out, MarshalAs(UnmanagedType.[Interface])> ByRef properties As IPortableDeviceValues) As HResult

        ''' <summary>
        ''' Get the array of SensorDataField objects that describe the individual values that can be reported by the sensor
        ''' </summary>
        ''' <param name="dataFields">A collection of PROPERTYKEY structures representing the data values reported by the sensor</param>
        <PreserveSig>
        Function GetSupportedDataFields(<Out, MarshalAs(UnmanagedType.[Interface])> ByRef dataFields As IPortableDeviceKeyCollection) As HResult

        ''' <summary>
        ''' Bulk Sensor metadata set for settable properties
        ''' </summary>
        ''' <param name="properties">The properties to be set</param>
        ''' <param name="results">The PROPERTYKEY/HRESULT pairs indicating success/failure for each property set</param>
        <PreserveSig>
        Function SetProperties(<[In], MarshalAs(UnmanagedType.[Interface])> properties As IPortableDeviceValues, <Out, MarshalAs(UnmanagedType.[Interface])> ByRef results As IPortableDeviceValues) As HResult

        ''' <summary>
        ''' Reports whether or not a sensor can deliver the requested data type
        ''' </summary>
        ''' <param name="key">The GUID to find matching PROPERTYKEY structures for</param>
        ''' <param name="isSupported">A collection of PROPERTYKEY structures representing the data values</param>
        Sub SupportsDataField(<[In]> key As PropertyKey, <Out, MarshalAs(UnmanagedType.VariantBool)> ByRef isSupported As Boolean)

        ''' <summary>
        ''' Get the sensor state
        ''' </summary>
        ''' <param name="state">The SensorState returned</param>
        Sub GetState(<Out, MarshalAs(UnmanagedType.U4)> ByRef state As NativeSensorState)

        ''' <summary>
        ''' Get the most recent ISensorDataReport for the sensor
        ''' </summary>
        ''' <param name="dataReport">The data report returned</param>
        <PreserveSig>
        Function GetData(<Out, MarshalAs(UnmanagedType.[Interface])> ByRef dataReport As ISensorDataReport) As HResult

        ''' <summary>
        ''' Reports whether or not a sensor supports the specified event.
        ''' </summary>
        ''' <param name="eventGuid">The event identifier</param>
        ''' <param name="isSupported">true if the event is supported, otherwise false</param>
        Sub SupportsEvent(<[In], MarshalAs(UnmanagedType.LPStruct)> eventGuid As Guid, <Out, MarshalAs(UnmanagedType.VariantBool)> ByRef isSupported As Boolean)

        ''' <summary>
        ''' Reports the set of event interests.
        ''' </summary>
        ''' <param name="pValues"></param>
        ''' <param name="count"></param>
        Sub GetEventInterest(ByRef pValues As IntPtr, <Out> ByRef count As UInteger)

        ''' <summary>
        ''' Sets the set of event interests
        ''' </summary>
        ''' <param name="pValues">The array of GUIDs representing sensor events of interest</param>
        ''' <param name="count">The number of guids included</param>
        <PreserveSig>
        Function SetEventInterest(<[In], MarshalAs(UnmanagedType.LPArray, SizeParamIndex:=1)> pValues As Guid(), <[In]> count As UInteger) As HResult

        ''' <summary>
        ''' Subscribe to ISensor events
        ''' </summary>
        ''' <param name="pEvents">An interface pointer to the callback object created for events</param>
        Sub SetEventSink(<[In], MarshalAs(UnmanagedType.[Interface])> pEvents As ISensorEvents)
    End Interface
End Namespace
