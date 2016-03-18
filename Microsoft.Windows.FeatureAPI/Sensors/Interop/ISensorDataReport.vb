' Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Globalization
Imports System.Runtime.InteropServices
Imports Microsoft.Windows.Internal
Imports Microsoft.Windows.Shell.PropertySystem

Namespace Sensors
    ''' <summary>
    ''' The SystemTime structure represents a date and time using individual members for 
    ''' the month, day, year, weekday, hour, minute, second, and millisecond.
    ''' </summary>
    <StructLayout(LayoutKind.Sequential)>
    Friend Structure SystemTime
        Friend Year As UShort
        Friend Month As UShort
        Friend DayOfWeek As UShort
        Friend Day As UShort
        Friend Hour As UShort
        Friend Minute As UShort
        Friend Second As UShort
        Friend Millisecond As UShort

        ''' <summary>
        ''' Gets the <see cref="DateTime"/> representation of this object.
        ''' </summary>
        Public ReadOnly Property DateTime() As DateTime
            Get
                Return New DateTime(Year, Month, Day, Hour, Minute, Second,
                    Millisecond)
            End Get
        End Property

        Public Shared Widening Operator CType(systemTime As SystemTime) As DateTime
            Return systemTime.DateTime
        End Operator

        Public Overrides Function ToString() As String
            Return String.Format(CultureInfo.InvariantCulture.NumberFormat, "{0:D2}/{1:D2}/{2:D4}, {3:D2}:{4:D2}:{5:D2}.{6}", Month, Day, Year, Hour,
                Minute, Second, Millisecond)
        End Function
    End Structure

    ''' <summary>
    ''' COM interop wrapper for the ISensorDataReport interface.
    ''' </summary>
    <ComImport, Guid("0AB9DF9B-C4B5-4796-8898-0470706A2E1D"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Friend Interface ISensorDataReport
        ''' <summary>
        ''' Get the timestamp for the data report
        ''' </summary>
        ''' <param name="timeStamp">The timestamp returned for the data report</param>
        Sub GetTimestamp(ByRef timeStamp As SystemTime)

        ''' <summary>
        ''' Get a single value reported by the sensor
        ''' </summary>
        ''' <param name="propKey">The data field ID of interest</param>
        ''' <param name="propValue">The data returned</param>
        Sub GetSensorValue(<[In]> ByRef propKey As PropertyKey, <Out> propValue As PropVariant)

        ''' <summary>
        ''' Get multiple values reported by a sensor
        ''' </summary>
        ''' <param name="keys">The collection of keys for values to obtain data for</param>
        ''' <param name="values">The values returned by the query</param>
        Sub GetSensorValues(<[In], MarshalAs(UnmanagedType.[Interface])> keys As IPortableDeviceKeyCollection, <Out, MarshalAs(UnmanagedType.[Interface])> ByRef values As IPortableDeviceValues)
    End Interface
End Namespace
