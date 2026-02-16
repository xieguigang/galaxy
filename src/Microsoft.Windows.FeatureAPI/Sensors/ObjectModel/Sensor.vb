' Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Collections.Generic
Imports System.Linq
Imports System.Runtime.InteropServices
Imports Microsoft.Windows.Internal
Imports Microsoft.Windows.Shell.PropertySystem

Namespace Sensors
    ''' <summary>
    ''' Represents the method that will handle the DataReportChanged event.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Public Delegate Sub DataReportChangedEventHandler(sender As Sensor, e As EventArgs)

    ''' <summary>
    ''' Represents the method that will handle the StatChanged event.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Public Delegate Sub StateChangedEventHandler(sender As Sensor, e As EventArgs)

    ''' <summary>
    ''' Defines a general wrapper for a sensor.
    ''' </summary>
    Public Class Sensor
        Implements ISensorEvents
        ''' <summary>
        ''' Occurs when the DataReport member changes.
        ''' </summary>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification:="The event is raised by a static method, so passing in the sender instance is not possible")>
        Public Event DataReportChanged As DataReportChangedEventHandler

        ''' <summary>
        ''' Occurs when the State member changes.
        ''' </summary>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification:="The event is raised by a static method, so passing in the sender instance is not possible")>
        Public Event StateChanged As StateChangedEventHandler


#Region "Public properties"

        ''' <summary>
        ''' Gets a value that specifies the most recent data reported by the sensor.
        ''' </summary>        
        Public Property DataReport() As SensorReport
            Get
                Return m_DataReport
            End Get
            Private Set
                m_DataReport = Value
            End Set
        End Property
        Private m_DataReport As SensorReport

        ''' <summary>
        ''' Gets a value that specifies the GUID for the sensor instance.
        ''' </summary>
        Public ReadOnly Property SensorId() As System.Nullable(Of Guid)
            Get
                If m_sensorId Is Nothing Then
                    Dim id As Guid
                    Dim hr As HResult = nativeISensor.GetID(id)
                    If hr = HResult.Ok Then
                        m_sensorId = id
                    End If
                End If
                Return m_sensorId
            End Get
        End Property
        Private m_sensorId As System.Nullable(Of Guid)

        ''' <summary>
        ''' Gets a value that specifies the GUID for the sensor category.
        ''' </summary>
        Public ReadOnly Property CategoryId() As System.Nullable(Of Guid)
            Get
                If m_categoryId Is Nothing Then
                    Dim id As Guid
                    Dim hr As HResult = nativeISensor.GetCategory(id)
                    If hr = HResult.Ok Then
                        m_categoryId = id
                    End If
                End If

                Return m_categoryId
            End Get
        End Property
        Private m_categoryId As System.Nullable(Of Guid)

        ''' <summary>
        ''' Gets a value that specifies the GUID for the sensor type.
        ''' </summary>
        Public ReadOnly Property TypeId() As System.Nullable(Of Guid)
            Get
                If m_typeId Is Nothing Then
                    Dim id As Guid
                    Dim hr As HResult = nativeISensor.[GetType](id)
                    If hr = HResult.Ok Then
                        m_typeId = id
                    End If
                End If

                Return m_typeId
            End Get
        End Property
        Private m_typeId As System.Nullable(Of Guid)

        ''' <summary>
        ''' Gets a value that specifies the sensor's friendly name.
        ''' </summary>
        Public ReadOnly Property FriendlyName() As String
            Get
                If m_friendlyName Is Nothing Then
                    Dim name As String
                    Dim hr As HResult = nativeISensor.GetFriendlyName(name)
                    If hr = HResult.Ok Then
                        m_friendlyName = name
                    End If
                End If
                Return m_friendlyName
            End Get
        End Property
        Private m_friendlyName As String

        ''' <summary>
        ''' Gets a value that specifies the sensor's current state.
        ''' </summary>
        Public ReadOnly Property State() As SensorState
            Get
                Dim state__1 As NativeSensorState
                nativeISensor.GetState(state__1)
                Return CType(state__1, SensorState)
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets a value that specifies the report interval.
        ''' </summary>
        Public Property ReportInterval() As UInteger
            Get
                Return CUInt(GetProperty(SensorPropertyKeys.SensorPropertyCurrentReportInterval))
            End Get
            Set
                SetProperties(New DataFieldInfo() {New DataFieldInfo(SensorPropertyKeys.SensorPropertyCurrentReportInterval, Value)})
            End Set
        End Property

        ''' <summary>
        ''' Gets a value that specifies the minimum report interval.
        ''' </summary>
        Public ReadOnly Property MinimumReportInterval() As UInteger
            Get
                Return CUInt(GetProperty(SensorPropertyKeys.SensorPropertyMinReportInterval))
            End Get
        End Property

        ''' <summary>
        ''' Gets a value that specifies the manufacturer of the sensor.
        ''' </summary>
        Public ReadOnly Property Manufacturer() As String
            Get
                If m_manufacturer Is Nothing Then
                    m_manufacturer = DirectCast(GetProperty(SensorPropertyKeys.SensorPropertyManufacturer), String)
                End If
                Return m_manufacturer
            End Get
        End Property
        Private m_manufacturer As String

        ''' <summary>
        ''' Gets a value that specifies the sensor's model.
        ''' </summary>
        Public ReadOnly Property Model() As String
            Get
                If m_model Is Nothing Then
                    m_model = DirectCast(GetProperty(SensorPropertyKeys.SensorPropertyModel), String)
                End If
                Return m_model
            End Get
        End Property
        Private m_model As String

        ''' <summary>
        ''' Gets a value that specifies the sensor's serial number.
        ''' </summary>
        Public ReadOnly Property SerialNumber() As String
            Get
                If m_serialNumber Is Nothing Then
                    m_serialNumber = DirectCast(GetProperty(SensorPropertyKeys.SensorPropertySerialNumber), String)
                End If
                Return m_serialNumber
            End Get
        End Property
        Private m_serialNumber As String

        ''' <summary>
        ''' Gets a value that specifies the sensor's description.
        ''' </summary>
        Public ReadOnly Property Description() As String
            Get
                If m_description Is Nothing Then
                    m_description = DirectCast(GetProperty(SensorPropertyKeys.SensorPropertyDescription), String)
                End If

                Return m_description
            End Get
        End Property
        Private m_description As String

        ''' <summary>
        ''' Gets a value that specifies the sensor's connection type.
        ''' </summary>
        Public ReadOnly Property ConnectionType() As System.Nullable(Of SensorConnectionType)
            Get
                If m_connectionType Is Nothing Then
                    m_connectionType = CType(GetProperty(SensorPropertyKeys.SensorPropertyConnectionType), SensorConnectionType)
                End If
                Return m_connectionType
            End Get
        End Property
        Private m_connectionType As System.Nullable(Of SensorConnectionType)

        ''' <summary>
        ''' Gets a value that specifies the sensor's device path.
        ''' </summary>
        Public ReadOnly Property DevicePath() As String
            Get
                If m_devicePath Is Nothing Then
                    m_devicePath = DirectCast(GetProperty(SensorPropertyKeys.SensorPropertyDeviceId), String)
                End If

                Return m_devicePath
            End Get
        End Property
        Private m_devicePath As String

        ''' <summary>
        ''' Gets or sets a value that specifies whether the data should be automatically updated.   
        ''' If the value is not set, call TryUpdateDataReport or UpdateDataReport to update the DataReport member.
        ''' </summary>        
        Public Property AutoUpdateDataReport() As Boolean
            Get
                Return IsEventInterestSet(EventInterestTypes.DataUpdated)
            End Get
            Set
                If Value Then
                    SetEventInterest(EventInterestTypes.DataUpdated)
                Else
                    ClearEventInterest(EventInterestTypes.DataUpdated)
                End If
            End Set
        End Property

#End Region

#Region "public methods"
        ''' <summary>
        ''' Attempts a synchronous data update from the sensor.
        ''' </summary>
        ''' <returns><b>true</b> if the request was successful; otherwise <b>false</b>.</returns>
        Public Function TryUpdateData() As Boolean
            Dim hr As HResult = InternalUpdateData()
            Return (hr = HResult.Ok)
        End Function

        ''' <summary>
        ''' Requests a synchronous data update from the sensor. The method throws an exception if the request fails.
        ''' </summary>
        Public Sub UpdateData()
            Dim hr As HResult = InternalUpdateData()
            If hr <> HResult.Ok Then
                Throw New SensorPlatformException(GlobalLocalizedMessages.SensorsNotFound, Marshal.GetExceptionForHR(CInt(hr)))
            End If
        End Sub

        Friend Function InternalUpdateData() As HResult

            Dim iReport As ISensorDataReport
            Dim hr As HResult = nativeISensor.GetData(iReport)
            If hr = HResult.Ok Then
                Try
                    DataReport = SensorReport.FromNativeReport(Me, iReport)
                    RaiseEvent DataReportChanged(Me, EventArgs.Empty)
                Finally
                    Marshal.ReleaseComObject(iReport)
                End Try
            End If
            Return hr
        End Function

        ''' <summary>
        ''' Returns a string that represents the current object.
        ''' </summary>
        ''' <returns>A string that represents the current object.</returns>
        Public Overrides Function ToString() As String
            Return String.Format(System.Globalization.CultureInfo.InvariantCulture, GlobalLocalizedMessages.SensorGetString, Me.SensorId, Me.TypeId, Me.CategoryId, Me.FriendlyName)
        End Function


        ''' <summary>
        ''' Retrieves a property value by the property key.
        ''' </summary>
        ''' <param name="propKey">A property key.</param>
        ''' <returns>A property value.</returns>        
        Public Function GetProperty(propKey As PropertyKey) As Object
            Using pv As New PropVariant()
                Dim hr As HResult = nativeISensor.GetProperty(propKey, pv)
                If hr <> HResult.Ok Then
                    Dim e As Exception = Marshal.GetExceptionForHR(CInt(hr))
                    If hr = HResult.ElementNotFound Then
                        Throw New ArgumentOutOfRangeException(GlobalLocalizedMessages.SensorPropertyNotFound, e)
                    Else
                        Throw e
                    End If
                End If
                Return pv.Value
            End Using
        End Function

        ''' <summary>
        ''' Retrieves a property value by the property index.
        ''' Assumes the GUID component in the property key takes the sensor's type GUID.
        ''' </summary>
        ''' <param name="propIndex">A property index.</param>
        ''' <returns>A property value.</returns>
        Public Function GetProperty(propIndex As Integer) As Object
            Dim propKey As New PropertyKey(SensorPropertyKeys.SensorPropertyCommonGuid, propIndex)
            Return GetProperty(propKey)
        End Function

        ''' <summary>
        ''' Retrieves the values of multiple properties by property key.
        ''' </summary>
        ''' <param name="propKeys">An array of properties to retrieve.</param>
        ''' <returns>A dictionary that contains the property keys and values.</returns>
        Public Function GetProperties(propKeys As PropertyKey()) As IDictionary(Of PropertyKey, Object)
            If propKeys Is Nothing OrElse propKeys.Length = 0 Then
                Throw New ArgumentException(GlobalLocalizedMessages.SensorEmptyProperties, "propKeys")
            End If

            Dim keyCollection As IPortableDeviceKeyCollection = New PortableDeviceKeyCollection()
            Try
                Dim valuesCollection As IPortableDeviceValues

                For i As Integer = 0 To propKeys.Length - 1
                    Dim propKey As PropertyKey = propKeys(i)
                    keyCollection.Add(propKey)
                Next

                Dim data As New Dictionary(Of PropertyKey, Object)()
                Dim hr As HResult = nativeISensor.GetProperties(keyCollection, valuesCollection)
                If CoreErrorHelper.Succeeded(hr) AndAlso valuesCollection IsNot Nothing Then
                    Try

                        Dim count As UInteger = 0
                        valuesCollection.GetCount(count)
                        Dim Len As UInteger = count - 1UI

                        For i As UInteger = 0 To Len
                            Dim propKey As New PropertyKey()
                            Using propVal As New PropVariant()
                                valuesCollection.GetAt(i, propKey, propVal)
                                data.Add(propKey, propVal.Value)
                            End Using
                        Next
                    Finally
                        Marshal.ReleaseComObject(valuesCollection)
                        valuesCollection = Nothing
                    End Try
                End If

                Return data
            Finally
                Marshal.ReleaseComObject(keyCollection)
                keyCollection = Nothing
            End Try
        End Function

        ''' <summary>
        ''' Returns a list of supported properties for the sensor.
        ''' </summary>
        ''' <returns>A strongly typed list of supported properties.</returns>        
        Public Function GetSupportedProperties() As IList(Of PropertyKey)
            If nativeISensor Is Nothing Then
                Throw New SensorPlatformException(GlobalLocalizedMessages.SensorNotInitialized)
            End If

            Dim list As New List(Of PropertyKey)()
            Dim collection As IPortableDeviceKeyCollection
            Dim hr As HResult = nativeISensor.GetSupportedDataFields(collection)
            If hr = HResult.Ok Then
                Try
                    Dim elements As UInteger = 0
                    collection.GetCount(elements)
                    If elements = 0 Then
                        Return Nothing
                    End If

                    For element As UInteger = 0 To elements - 1UI
                        Dim key As PropertyKey
                        hr = collection.GetAt(element, key)
                        If hr = HResult.Ok Then
                            list.Add(key)
                        End If
                    Next
                Finally
                    Marshal.ReleaseComObject(collection)
                    collection = Nothing
                End Try
            End If
            Return list
        End Function


        ''' <summary>
        ''' Retrieves the values of multiple properties by their index.
        ''' Assumes that the GUID component of the property keys is the sensor's type GUID.
        ''' </summary>
        ''' <param name="propIndexes">The indexes of the properties to retrieve.</param>
        ''' <returns>An array that contains the property values.</returns>
        ''' <remarks>
        ''' The returned array will contain null values for some properties if the values could not be retrieved.
        ''' </remarks>        
        Public Function GetProperties(ParamArray propIndexes As Integer()) As Object()
            If propIndexes Is Nothing OrElse propIndexes.Length = 0 Then
                Throw New ArgumentNullException("propIndexes")
            End If

            Dim keyCollection As IPortableDeviceKeyCollection = New PortableDeviceKeyCollection()
            Try
                Dim valuesCollection As IPortableDeviceValues
                Dim propKeyToIdx As New Dictionary(Of PropertyKey, Integer)()

                For i As Integer = 0 To propIndexes.Length - 1
                    Dim propKey As New PropertyKey(TypeId.Value, propIndexes(i))
                    keyCollection.Add(propKey)
                    propKeyToIdx.Add(propKey, i)
                Next

                Dim data As Object() = New Object(propIndexes.Length - 1) {}
                Dim hr As HResult = nativeISensor.GetProperties(keyCollection, valuesCollection)
                If hr = HResult.Ok Then
                    Try
                        If valuesCollection Is Nothing Then
                            Return data
                        End If

                        Dim count As UInteger = 0
                        valuesCollection.GetCount(count)

                        For i As UInteger = 0 To count - 1UI
                            Dim propKey As New PropertyKey()
                            Using propVal As New PropVariant()
                                valuesCollection.GetAt(i, propKey, propVal)

                                Dim idx As Integer = propKeyToIdx(propKey)
                                data(idx) = propVal.Value
                            End Using
                        Next
                    Finally
                        Marshal.ReleaseComObject(valuesCollection)
                        valuesCollection = Nothing
                    End Try
                End If
                Return data
            Finally
                Marshal.ReleaseComObject(keyCollection)
            End Try
        End Function

        ''' <summary>
        ''' Sets the values of multiple properties.
        ''' </summary>
        ''' <param name="data">An array that contains the property keys and values.</param>
        ''' <returns>A dictionary of the new values for the properties. Actual values may not match the requested values.</returns>                
        Public Function SetProperties(data As DataFieldInfo()) As IDictionary(Of PropertyKey, Object)
            If data Is Nothing OrElse data.Length = 0 Then
                Throw New ArgumentException(GlobalLocalizedMessages.SensorEmptyData, "data")
            End If

            Dim pdv As IPortableDeviceValues = New PortableDeviceValues()

            For i As Integer = 0 To data.Length - 1
                Dim propKey As PropertyKey = data(i).Key
                Dim value As Object = data(i).Value
                If value Is Nothing Then
                    Throw New ArgumentException(String.Format(System.Globalization.CultureInfo.InvariantCulture, GlobalLocalizedMessages.SensorNullValueAtIndex, i), "data")
                End If

                Try
                    ' new PropVariant will throw an ArgumentException if the value can 
                    ' not be converted to an appropriate PropVariant.
                    Using pv As PropVariant = PropVariant.FromObject(value)
                        pdv.SetValue(propKey, pv)
                    End Using
                Catch generatedExceptionName As ArgumentException
                    Dim buffer As Byte()
                    If TypeOf value Is Guid Then
                        Dim guid As Guid = CType(value, Guid)
                        pdv.SetGuidValue(propKey, guid)
                    ElseIf buffer.InlineCopy(TryCast(value, Byte())) IsNot Nothing Then
                        pdv.SetBufferValue(propKey, buffer, CUInt(buffer.Length))
                    Else
                        pdv.SetIUnknownValue(propKey, value)
                    End If
                End Try
            Next

            Dim results As New Dictionary(Of PropertyKey, Object)()
            Dim pdv2 As IPortableDeviceValues = Nothing
            Dim hr As HResult = nativeISensor.SetProperties(pdv, pdv2)
            If hr = HResult.Ok Then
                Try
                    Dim count As UInteger = 0
                    pdv2.GetCount(count)

                    For i As UInteger = 0 To count - 1UI
                        Dim propKey As New PropertyKey()
                        Using propVal As New PropVariant()
                            pdv2.GetAt(i, propKey, propVal)
                            results.Add(propKey, propVal.Value)
                        End Using
                    Next
                Finally
                    Marshal.ReleaseComObject(pdv2)
                    pdv2 = Nothing
                End Try
            End If

            Return results
        End Function
#End Region

#Region "overridable methods"
        ''' <summary>
        ''' Initializes the Sensor wrapper after it has been bound to the native ISensor interface
        ''' and is ready for subsequent initialization.
        ''' </summary>
        Protected Overridable Sub Initialize()
        End Sub

#End Region

#Region "ISensorEvents Members"

        Private Sub ISensorEvents_OnStateChanged(sensor As ISensor, state As NativeSensorState) Implements ISensorEvents.OnStateChanged
            RaiseEvent StateChanged(Me, EventArgs.Empty)
        End Sub

        Private Sub ISensorEvents_OnDataUpdated(sensor As ISensor, newData As ISensorDataReport) Implements ISensorEvents.OnDataUpdated
            DataReport = SensorReport.FromNativeReport(Me, newData)
            RaiseEvent DataReportChanged(Me, EventArgs.Empty)
        End Sub

        Private Sub ISensorEvents_OnEvent(sensor As ISensor, eventID As Guid, newData As ISensorDataReport) Implements ISensorEvents.OnEvent
        End Sub

        Private Sub ISensorEvents_OnLeave(sensorIdArgs As Guid) Implements ISensorEvents.OnLeave
            SensorManager.OnSensorsChanged(sensorIdArgs, SensorAvailabilityChange.Removal)
        End Sub

#End Region

#Region "Implementation"
        Private nativeISensor As ISensor
        Friend Property internalObject() As ISensor
            Get
                Return nativeISensor
            End Get
            Set
                nativeISensor = Value
                SetEventInterest(EventInterestTypes.StateChanged)
                nativeISensor.SetEventSink(Me)
                Initialize()
            End Set
        End Property

        ''' <summary>
        ''' Informs the sensor driver of interest in a specific type of event.
        ''' </summary>
        ''' <param name="eventType">The type of event of interest.</param>        
        Protected Sub SetEventInterest(eventType As Guid)
            If Me.nativeISensor Is Nothing Then
                Throw New SensorPlatformException(GlobalLocalizedMessages.SensorNotInitialized)
            End If

            Dim interestingEvents As Guid() = GetInterestingEvents()

            If interestingEvents.Any(Function(g) g = eventType) Then
                Return
            End If

            Dim interestCount As Integer = interestingEvents.Length

            Dim newEventInterest As Guid() = New Guid(interestCount) {}
            interestingEvents.CopyTo(newEventInterest, 0)
            newEventInterest(interestCount) = eventType

            Dim hr As HResult = Me.nativeISensor.SetEventInterest(newEventInterest, CUInt(interestCount + 1))
            If hr <> HResult.Ok Then
                Throw Marshal.GetExceptionForHR(CInt(hr))
            End If
        End Sub

        ''' <summary>
        '''  Informs the sensor driver to clear a specific type of event.
        ''' </summary>
        ''' <param name="eventType">The type of event of interest.</param>
        Protected Sub ClearEventInterest(eventType As Guid)
            If Me.nativeISensor Is Nothing Then
                Throw New SensorPlatformException(GlobalLocalizedMessages.SensorNotInitialized)
            End If

            If IsEventInterestSet(eventType) Then
                Dim interestingEvents As Guid() = GetInterestingEvents()
                Dim interestCount As Integer = interestingEvents.Length

                Dim newEventInterest As Guid() = New Guid(interestCount - 2) {}

                Dim eventIndex As Integer = 0
                For Each g As Guid In interestingEvents
                    If g <> eventType Then
                        newEventInterest(eventIndex) = g
                        eventIndex += 1
                    End If
                Next

                Me.nativeISensor.SetEventInterest(newEventInterest, CUInt(interestCount - 1))
            End If

        End Sub

        ''' <summary>
        ''' Determines whether the sensor driver will file events for a particular type of event.
        ''' </summary>
        ''' <param name="eventType">The type of event, as a GUID.</param>
        ''' <returns><b>true</b> if the sensor will report interest in the specified event.</returns>
        Protected Function IsEventInterestSet(eventType As Guid) As Boolean
            If Me.nativeISensor Is Nothing Then
                Throw New SensorPlatformException(GlobalLocalizedMessages.SensorNotInitialized)
            End If

            Return GetInterestingEvents().Any(Function(g) g.CompareTo(eventType) = 0)
        End Function

        Private Function GetInterestingEvents() As Guid()
            Dim values As IntPtr
            Dim interestCount As UInteger
            Me.nativeISensor.GetEventInterest(values, interestCount)
            Dim interestingEvents As Guid() = New Guid(CInt(interestCount - 1)) {}
            For index As Integer = 0 To interestingEvents.Length - 1
                interestingEvents(index) = CType(Marshal.PtrToStructure(values, GetType(Guid)), Guid)
                values = IncrementIntPtr(values, Marshal.SizeOf(GetType(Guid)))
            Next
            Return interestingEvents
        End Function

        Private Shared Function IncrementIntPtr(source As IntPtr, increment As Integer) As IntPtr
            If IntPtr.Size = 8 Then
                Dim p As Int64 = source.ToInt64()
                p += increment
                Return New IntPtr(p)
            ElseIf IntPtr.Size = 4 Then
                Dim p As Int32 = source.ToInt32()
                p += increment
                Return New IntPtr(p)
            Else
                Throw New SensorPlatformException(GlobalLocalizedMessages.SensorUnexpectedPointerSize)
            End If
        End Function

#End Region
    End Class

#Region "Helper types"

    ''' <summary>
    ''' Defines a structure that contains the property ID (key) and value.
    ''' </summary>
    Public Structure DataFieldInfo
        Implements IEquatable(Of DataFieldInfo)
        Private _propKey As PropertyKey
        Private _value As Object

        ''' <summary>
        ''' Initializes the structure.
        ''' </summary>
        ''' <param name="propKey">A property ID (key).</param>
        ''' <param name="value">A property value. The type must be valid for the property ID.</param>
        Public Sub New(propKey As PropertyKey, value As Object)
            _propKey = propKey
            _value = value
        End Sub

        ''' <summary>
        ''' Gets the property's key.
        ''' </summary>
        Public ReadOnly Property Key() As PropertyKey
            Get
                Return _propKey
            End Get
        End Property

        ''' <summary>
        ''' Gets the property's value.
        ''' </summary>
        Public ReadOnly Property Value() As Object
            Get
                Return _value
            End Get
        End Property

        ''' <summary>
        ''' Returns the hash code for a particular DataFieldInfo structure.
        ''' </summary>
        ''' <returns>A hash code.</returns>
        Public Overrides Function GetHashCode() As Integer
            Dim valHashCode As Integer = If(_value IsNot Nothing, _value.GetHashCode(), 0)
            Return _propKey.GetHashCode() Xor valHashCode
        End Function

        ''' <summary>
        ''' Determines if this object and another object are equal.
        ''' </summary>
        ''' <param name="obj">The object to compare.</param>
        ''' <returns><b>true</b> if this instance and another object are equal; otherwise <b>false</b>.</returns>
        Public Overrides Function Equals(obj As Object) As Boolean
            If obj Is Nothing Then
                Return False
            End If

            If Not (TypeOf obj Is DataFieldInfo) Then
                Return False
            End If

            Dim other As DataFieldInfo = CType(obj, DataFieldInfo)
            Return _value.Equals(other._value) AndAlso _propKey.Equals(other._propKey)
        End Function

#Region "IEquatable<DataFieldInfo> Members"

        ''' <summary>
        ''' Determines if this key and value pair and another key and value pair are equal.
        ''' </summary>
        ''' <param name="other">The item to compare.</param>
        ''' <returns><b>true</b> if equal; otherwise <b>false</b>.</returns>
        Public Overloads Function Equals(other As DataFieldInfo) As Boolean Implements IEquatable(Of DataFieldInfo).Equals
            Return _value.Equals(other._value) AndAlso _propKey.Equals(other._propKey)
        End Function

#End Region

        ''' <summary>
        ''' DataFieldInfo == operator overload
        ''' </summary>
        ''' <param name="first">The first item to compare.</param>
        ''' <param name="second">The second item to compare.</param>
        ''' <returns><b>true</b> if equal; otherwise <b>false</b>.</returns>
        Public Shared Operator =(first As DataFieldInfo, second As DataFieldInfo) As Boolean
            Return first.Equals(second)
        End Operator

        ''' <summary>
        ''' DataFieldInfo != operator overload
        ''' </summary>
        ''' <param name="first">The first item to compare.</param>
        ''' <param name="second">The second item to comare.</param>
        ''' <returns><b>true</b> if not equal; otherwise <b>false</b>.</returns>
        Public Shared Operator <>(first As DataFieldInfo, second As DataFieldInfo) As Boolean
            Return Not first.Equals(second)
        End Operator
    End Structure

#End Region

End Namespace
