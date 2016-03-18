' Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Collections.Generic
Imports System.Reflection
Imports System.Threading
Imports Microsoft.Windows.Resources
Imports Microsoft.Windows.Internal

Namespace Sensors
	''' <summary>
	''' Specifies the types of change in sensor availability.
	''' </summary>
	Public Enum SensorAvailabilityChange
		''' <summary>
		''' A sensor has been added.
		''' </summary>
		Addition
		''' <summary>
		''' A sensor has been removed.
		''' </summary>
		Removal
	End Enum

	''' <summary>
	''' Defines the data passed to the SensorsChangedHandler.
	''' </summary>
	Public Class SensorsChangedEventArgs
		Inherits EventArgs
		''' <summary>
		''' The type of change. 
		''' </summary>
		Public Property Change() As SensorAvailabilityChange
			Get
				Return m_Change
			End Get
			Set
				m_Change = Value
			End Set
		End Property
		Private m_Change As SensorAvailabilityChange

		''' <summary>
		''' The ID of the sensor that changed.
		''' </summary>
		Public Property SensorId() As Guid
			Get
				Return m_SensorId
			End Get
			Set
				m_SensorId = Value
			End Set
		End Property
		Private m_SensorId As Guid

		Friend Sub New(sensorId__1 As Guid, change__2 As SensorAvailabilityChange)
			SensorId = sensorId__1
			Change = change__2
		End Sub
	End Class

	''' <summary>
	''' Represents the method that will handle the system sensor list change.
	''' </summary>
	''' <param name="e">The event data.</param>
	Public Delegate Sub SensorsChangedEventHandler(e As SensorsChangedEventArgs)

	''' <summary>
	''' Manages the sensors conected to the system.
	''' </summary>
	Public NotInheritable Class SensorManager
		Private Sub New()
		End Sub
		#Region "Public Methods"
		''' <summary>
		''' Retireves a collection of all sensors.
		''' </summary>
		''' <returns>A list of all sensors.</returns>
		<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")> _
		Public Shared Function GetAllSensors() As SensorList(Of Sensor)
			Return GetSensorsByCategoryId(SensorCategories.All)
		End Function

		''' <summary>
		''' Retrieves a collection of sensors filtered by category ID.
		''' </summary>
		''' <param name="category">The category ID of the requested sensors.</param>
		''' <returns>A list of sensors of the specified category ID.</returns>
		Public Shared Function GetSensorsByCategoryId(category As Guid) As SensorList(Of Sensor)
			Dim sensorCollection As ISensorCollection = Nothing
			Dim hr As HResult = sensorManager.GetSensorsByCategory(category, sensorCollection)
			If hr = HResult.ElementNotFound Then
				Throw New SensorPlatformException(LocalizedMessages.SensorsNotFound)
			End If

			Return nativeSensorCollectionToSensorCollection(Of Sensor)(sensorCollection)
		End Function

		''' <summary>
		''' Returns a collection of sensors filtered by type ID.
		''' </summary>
		''' <param name="typeId">The type ID of the sensors requested.</param>
		''' <returns>A list of sensors of the spefified type ID.</returns>
		Public Shared Function GetSensorsByTypeId(typeId As Guid) As SensorList(Of Sensor)
			Dim sensorCollection As ISensorCollection = Nothing
			Dim hr As HResult = sensorManager.GetSensorsByType(typeId, sensorCollection)
			If hr = HResult.ElementNotFound Then
				Throw New SensorPlatformException(LocalizedMessages.SensorsNotFound)
			End If
			Return nativeSensorCollectionToSensorCollection(Of Sensor)(sensorCollection)
		End Function

		''' <summary>
		''' Returns a strongly typed collection of specific sensor types.
		''' </summary>
		''' <typeparam name="T">The type of the sensors to retrieve.</typeparam>
		''' <returns>A strongly types list of sensors.</returns>        
		Public Shared Function GetSensorsByTypeId(Of T As Sensor)() As SensorList(Of T)
			Dim attrs As Object() = GetType(T).GetCustomAttributes(GetType(SensorDescriptionAttribute), True)
			If attrs IsNot Nothing AndAlso attrs.Length > 0 Then
				Dim sda As SensorDescriptionAttribute = TryCast(attrs(0), SensorDescriptionAttribute)

				Dim nativeSensorCollection As ISensorCollection = Nothing
				Dim hr As HResult = sensorManager.GetSensorsByType(sda.SensorTypeGuid, nativeSensorCollection)
				If hr = HResult.ElementNotFound Then
					Throw New SensorPlatformException(LocalizedMessages.SensorsNotFound)
				End If
				Return nativeSensorCollectionToSensorCollection(Of T)(nativeSensorCollection)
			End If

			Return New SensorList(Of T)()
		End Function

		''' <summary>
		''' Returns a specific sensor by sensor ID.
		''' </summary>
		''' <typeparam name="T">A strongly typed sensor.</typeparam>
		''' <param name="sensorId">The unique identifier of the sensor.</param>
		''' <returns>A particular sensor.</returns>        
		Public Shared Function GetSensorBySensorId(Of T As Sensor)(sensorId As Guid) As T
			Dim nativeSensor As ISensor = Nothing
			Dim hr As HResult = sensorManager.GetSensorByID(sensorId, nativeSensor)
			If hr = HResult.ElementNotFound Then
				Throw New SensorPlatformException(LocalizedMessages.SensorsNotFound)
			End If

			If nativeSensor IsNot Nothing Then
				Return GetSensorWrapperInstance(Of T)(nativeSensor)
			End If

			Return Nothing
		End Function

		''' <summary>
		''' Opens a system dialog box to request user permission to access sensor data.
		''' </summary>
		''' <param name="parentWindowHandle">The handle to a window that can act as a parent to the permissions dialog box.</param>
		''' <param name="modal">Specifies whether the window should be modal.</param>
		''' <param name="sensors">The sensors for which to request permission.</param>
		Public Shared Sub RequestPermission(parentWindowHandle As IntPtr, modal As Boolean, sensors As SensorList(Of Sensor))
			If sensors Is Nothing OrElse sensors.Count = 0 Then
				Throw New ArgumentException(LocalizedMessages.SensorManagerEmptySensorsCollection, "sensors")
			End If

			Dim sensorCollection As ISensorCollection = New SensorCollection()

			For Each sensor As Sensor In sensors
				sensorCollection.Add(sensor.internalObject)
			Next

			sensorManager.RequestPermissions(parentWindowHandle, sensorCollection, modal)
		End Sub

		#End Region

		#Region "Public Events"
		''' <summary>
		''' Occurs when the system's list of sensors changes.
		''' </summary>
		<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification := "The event is raised from a static method, and so providing the instance of the sender is not possible")> _
		Public Shared Event SensorsChanged As SensorsChangedEventHandler
		#End Region

		#Region "implementation"
		Private Shared sensorManager As NativeISensorManager = New NativeSensorManager()
		Private Shared sensorManagerEventSink As New nativeSensorManagerEventSink()

		''' <summary>
		''' Sensor type GUID -> .NET Type + report type
		''' </summary>
		Private Shared guidToSensorDescr As New Dictionary(Of Guid, SensorTypeData)()

		''' <summary>
		''' .NET type -> type GUID.
		''' </summary>      
		Private Shared sensorTypeToGuid As New Dictionary(Of Type, Guid)()

		<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")> _
		Shared Sub New()
			CoreHelpers.ThrowIfNotWin7()

			BuildSensorTypeMap()
			Thread.MemoryBarrier()
			sensorManager.SetEventSink(sensorManagerEventSink)
		End Sub

		Friend Shared Function nativeSensorCollectionToSensorCollection(Of S As Sensor)(nativeCollection As ISensorCollection) As SensorList(Of S)
			Dim sensors As New SensorList(Of S)()

			If nativeCollection IsNot Nothing Then
				Dim sensorCount As UInteger = 0
				nativeCollection.GetCount(sensorCount)

                For i As UInteger = 0 To sensorCount - 1UI
                    Dim iSensor As ISensor = Nothing
                    nativeCollection.GetAt(i, iSensor)
                    Dim sensor As S = GetSensorWrapperInstance(Of S)(iSensor)
                    If sensor IsNot Nothing Then
                        sensor.internalObject = iSensor
                        sensors.Add(sensor)
                    End If
                Next
            End If

			Return sensors
		End Function

        ''' <summary>
        ''' Notification that the list of sensors has changed
        ''' </summary>
        Friend Shared Sub OnSensorsChanged(sensorId As Guid, change As SensorAvailabilityChange)
            RaiseEvent SensorsChanged(New SensorsChangedEventArgs(sensorId, change))
        End Sub

        ''' <summary>
        ''' Interrogates assemblies currently loaded into the AppDomain for classes deriving from <see cref="Sensor"/>.
        ''' Builds data structures which map those types to sensor type GUIDs. 
        ''' </summary>
        Private Shared Sub BuildSensorTypeMap()
			Dim loadedAssemblies As Assembly() = AppDomain.CurrentDomain.GetAssemblies()

			For Each asm As Assembly In loadedAssemblies
				Try
					Dim exportedTypes As Type() = asm.GetExportedTypes()
					For Each t As Type In exportedTypes
						If t.IsSubclassOf(GetType(Sensor)) AndAlso t.IsPublic AndAlso Not t.IsAbstract AndAlso Not t.IsGenericType Then
							Dim attrs As Object() = t.GetCustomAttributes(GetType(SensorDescriptionAttribute), True)
							If attrs IsNot Nothing AndAlso attrs.Length > 0 Then
								Dim sda As SensorDescriptionAttribute = DirectCast(attrs(0), SensorDescriptionAttribute)
								Dim stm As New SensorTypeData(t, sda)

								guidToSensorDescr.Add(sda.SensorTypeGuid, stm)
								sensorTypeToGuid.Add(t, sda.SensorTypeGuid)
							End If
						End If
					Next
						' GetExportedTypes can throw this if dynamic assemblies are loaded 
						' via Reflection.Emit.
				Catch generatedExceptionName As System.NotSupportedException
						' GetExportedTypes can throw this if a loaded asembly is not in the 
						' current directory or path.
				Catch generatedExceptionName As System.IO.FileNotFoundException
				End Try
			Next
		End Sub

		''' <summary>
		''' Returns an instance of a sensor wrapper appropritate for the given sensor COM interface.
		''' If no appropriate sensor wrapper type could be found, the object created will be of the base-class type <see cref="Sensor"/>.
		''' </summary>
		''' <param name="nativeISensor">The underlying sensor COM interface.</param>
		''' <returns>A wrapper instance.</returns>
		Private Shared Function GetSensorWrapperInstance(Of S As Sensor)(nativeISensor As ISensor) As S
			Dim sensorTypeGuid As Guid
			nativeISensor.[GetType](sensorTypeGuid)

			Dim stm As SensorTypeData
			Dim sensorClassType As Type = If(guidToSensorDescr.TryGetValue(sensorTypeGuid, stm), stm.SensorType, GetType(UnknownSensor))

			Try
				Dim sensor As S = DirectCast(Activator.CreateInstance(sensorClassType), S)
				sensor.internalObject = nativeISensor
				Return sensor
			Catch generatedExceptionName As InvalidCastException
				Return Nothing
			End Try
		End Function

		#End Region
	End Class

	#Region "helper classes"
	''' <summary>
	''' Data associated with a sensor type GUID.
	''' </summary>
	Friend Structure SensorTypeData
		Private m_sensorType As Type
		Private sda As SensorDescriptionAttribute

		Public Sub New(sensorClassType As Type, sda As SensorDescriptionAttribute)
			Me.m_sensorType = sensorClassType
			Me.sda = sda
		End Sub

		Public ReadOnly Property SensorType() As Type
			Get
				Return Me.m_sensorType
			End Get
		End Property

		<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")> _
		Public ReadOnly Property Attr() As SensorDescriptionAttribute
			Get
				Return Me.sda
			End Get
		End Property
	End Structure

	Friend Class nativeSensorManagerEventSink
		Implements ISensorManagerEvents
		#Region "nativeISensorManagerEvents Members"

		Public Sub OnSensorEnter(nativeSensor As ISensor, state As NativeSensorState) Implements ISensorManagerEvents.OnSensorEnter
			If state = NativeSensorState.Ready Then
				Dim sensorId As Guid
				Dim hr As HResult = nativeSensor.GetID(sensorId)
				If hr = HResult.Ok Then
					SensorManager.OnSensorsChanged(sensorId, SensorAvailabilityChange.Addition)
				End If
			End If
		End Sub

		#End Region
	End Class
	#End Region
End Namespace
