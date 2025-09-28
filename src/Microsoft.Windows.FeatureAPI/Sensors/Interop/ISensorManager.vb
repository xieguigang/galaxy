' Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports Microsoft.Windows.Internal

Namespace Sensors
    ''' <summary>
    ''' A COM interop wrapper for the SensorsManager class
    ''' </summary>
    ''' <remarks>
    ''' See Sensor API documentation in Windows 7 SDK
    ''' </remarks>
    <ComImport, GuidAttribute("77A1C827-FCD2-4689-8915-9D613CC5FA3E"), ClassInterfaceAttribute(ClassInterfaceType.None)>
    Friend Class NativeSensorManager
        Implements NativeISensorManager
        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Function GetSensorsByCategory(<[In], MarshalAs(UnmanagedType.LPStruct)> sensorCategory As Guid, <Out, MarshalAs(UnmanagedType.[Interface])> ByRef ppSensorsFound As ISensorCollection) As HResult Implements NativeISensorManager.GetSensorsByCategory
        End Function

        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Function GetSensorsByType(<[In], MarshalAs(UnmanagedType.LPStruct)> sensorType As Guid, <Out, MarshalAs(UnmanagedType.[Interface])> ByRef ppSensorsFound As ISensorCollection) As HResult Implements NativeISensorManager.GetSensorsByType
        End Function

        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Function GetSensorByID(<[In], MarshalAs(UnmanagedType.LPStruct)> sensorID As Guid, <Out, MarshalAs(UnmanagedType.[Interface])> ByRef ppSensor As ISensor) As HResult Implements NativeISensorManager.GetSensorByID
        End Function

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub SetEventSink(<Out, MarshalAs(UnmanagedType.[Interface])> pEvents As ISensorManagerEvents) Implements NativeISensorManager.SetEventSink
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub RequestPermissions(hParent As IntPtr, <[In], MarshalAs(UnmanagedType.[Interface])> pSensors As ISensorCollection, <[In], MarshalAs(UnmanagedType.Bool)> modal As Boolean) Implements NativeISensorManager.RequestPermissions
        End Sub
    End Class

    ''' <summary>
    ''' A COM interop wrapper for the ISensorsManager interface
    ''' </summary>
    <ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("BD77DB67-45A8-42DC-8D00-6DCF15F8377A")>
    Friend Interface NativeISensorManager
        ''' <summary>
        ''' Get a collection of related sensors by category, Ex: Light
        ''' </summary>
        ''' <param name="sensorCategory">The category of sensors to find</param>
        ''' <param name="ppSensorsFound">The collection of sensors found</param>
        <PreserveSig>
        Function GetSensorsByCategory(<[In], MarshalAs(UnmanagedType.LPStruct)> sensorCategory As Guid, <Out, MarshalAs(UnmanagedType.[Interface])> ByRef ppSensorsFound As ISensorCollection) As HResult

        ''' <summary>
        ''' Get sensors by type, Ex: Ambient Light
        ''' </summary>
        ''' <param name="sensorType">The type of sensors to find</param>
        ''' <param name="ppSensorsFound">The collection of sensors found</param>
        <PreserveSig>
        Function GetSensorsByType(<[In], MarshalAs(UnmanagedType.LPStruct)> sensorType As Guid, <Out, MarshalAs(UnmanagedType.[Interface])> ByRef ppSensorsFound As ISensorCollection) As HResult

        ''' <summary>
        ''' Get a unique instance of a sensor
        ''' </summary>
        ''' <param name="sensorID">The unique ID of a sensor installed on a system</param>
        ''' <param name="ppSensor">The ISensor interface pointer for the sensor found, or null if no sensor was found</param>
        <PreserveSig>
        Function GetSensorByID(<[In], MarshalAs(UnmanagedType.LPStruct)> sensorID As Guid, <Out, MarshalAs(UnmanagedType.[Interface])> ByRef ppSensor As ISensor) As HResult

        ''' <summary>
        ''' Subscribe to ISensors events
        ''' </summary>
        ''' <param name="pEvents">An interface pointer to the callback object created</param>
        Sub SetEventSink(<[In], MarshalAs(UnmanagedType.[Interface])> pEvents As ISensorManagerEvents)

        ''' <summary>
        ''' Opens a system dialog box to request user permission to access sensor data
        ''' </summary>
        ''' <param name="parent">HWND handle to a window that can act as a parent to the permissions dialog box</param>
        ''' <param name="sensors">Pointer to the ISensorCollection interface that contains the list of sensors for which permission is being requested</param>
        ''' <param name="modal">BOOL that specifies the dialog box mode</param>
        Sub RequestPermissions(<[In]> parent As IntPtr, <[In], MarshalAs(UnmanagedType.[Interface])> sensors As ISensorCollection, <[In], MarshalAs(UnmanagedType.Bool)> modal As Boolean)
    End Interface
End Namespace
