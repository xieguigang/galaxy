' Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Collections.Generic

Namespace Sensors
	''' <summary>
	''' Defines a strongly typed list of sensors.
	''' </summary>
	''' <typeparam name="TSensor">The type of sensor in the list.</typeparam>        
	Public Class SensorList(Of TSensor As Sensor)
		Implements IList(Of TSensor)
		Private sensorList As New List(Of TSensor)()

#Region "IList<S> Members"

        ''' <summary>
        ''' Returns a sensor at a particular index.
        ''' </summary>
        ''' <param name="item">The sensor item.</param>
        ''' <returns>The index of the sensor.</returns>
        Public Function IndexOf(item As TSensor) As Integer Implements IList(Of TSensor).IndexOf
            Return sensorList.IndexOf(item)
        End Function

        ''' <summary>
        ''' Inserts a sensor at a specific location in the list.
        ''' </summary>
        ''' <param name="index">The index to insert the sensor.</param>
        ''' <param name="item">The sensor to insert.</param>
        Public Sub Insert(index As Integer, item As TSensor) Implements IList(Of TSensor).Insert
            sensorList.Insert(index, item)
        End Sub

        ''' <summary>
        ''' Removes a sensor at a specific location in the list.
        ''' </summary>
        ''' <param name="index">The index of the sensor to remove.</param>
        Public Sub RemoveAt(index As Integer) Implements IList(Of TSensor).RemoveAt
            sensorList.RemoveAt(index)
        End Sub

        ''' <summary>
        ''' Gets or sets a sensor at a specified location in the list.
        ''' </summary>
        ''' <param name="index">The index of the sensor in the list.</param>
        ''' <returns>The sensor.</returns>
        Default Public Property Item(index As Integer) As TSensor Implements IList(Of TSensor).Item
            Get
                Return sensorList(index)
            End Get
            Set
                sensorList(index) = Value
            End Set
        End Property

#End Region

#Region "ICollection<S> Members"

        ''' <summary>
        ''' Adds a sensor to the end of the list.
        ''' </summary>
        ''' <param name="item">The sensor item.</param>
        Public Sub Add(item As TSensor) Implements ICollection(Of TSensor).Add
            sensorList.Add(item)
        End Sub

        ''' <summary>
        ''' Clears the list of sensors.
        ''' </summary>
        Public Sub Clear() Implements ICollection(Of TSensor).Clear
            sensorList.Clear()
        End Sub

        ''' <summary>
        ''' Determines if the list contains a specified sensor.
        ''' </summary>
        ''' <param name="item">The sensor to locate.</param>
        ''' <returns><b>true</b> if the list contains the sensor; otherwise <b>false</b>.</returns>
        Public Function Contains(item As TSensor) As Boolean Implements ICollection(Of TSensor).Contains
            Return sensorList.Contains(item)
        End Function

        ''' <summary>
        ''' Copies a sensor to a new list.
        ''' </summary>
        ''' <param name="array">The destination list.</param>
        ''' <param name="arrayIndex">The index of the item to copy.</param>
        Public Sub CopyTo(array As TSensor(), arrayIndex As Integer) Implements ICollection(Of TSensor).CopyTo
            sensorList.CopyTo(array, arrayIndex)
        End Sub

        ''' <summary>
        ''' Gets the number of items in the list.
        ''' </summary>
        Public ReadOnly Property Count() As Integer Implements ICollection(Of TSensor).Count
            Get
                Return sensorList.Count
            End Get
        End Property

        ''' <summary>
        ''' Gets a value that determines if the list is read-only.
        ''' </summary>
        Public ReadOnly Property IsReadOnly() As Boolean Implements ICollection(Of TSensor).IsReadOnly
            Get
                Return TryCast(sensorList, ICollection(Of TSensor)).IsReadOnly
            End Get
        End Property

        ''' <summary>
        ''' Removes a specific sensor from the list.
        ''' </summary>
        ''' <param name="item">The sensor to remove.</param>
        ''' <returns><b>true</b> if the sensor was removed from the list; otherwise <b>false</b>.</returns>
        Public Function Remove(item As TSensor) As Boolean Implements ICollection(Of TSensor).Remove
            Return sensorList.Remove(item)


        End Function

#End Region

#Region "IEnumerable<S> Members"

        ''' <summary>
        ''' Returns an enumerator for the list.
        ''' </summary>
        ''' <returns>An enumerator.</returns>
        Public Function GetEnumerator() As IEnumerator(Of TSensor) Implements IEnumerable(Of TSensor).GetEnumerator
            Return (sensorList.GetEnumerator())
        End Function

#End Region

#Region "IEnumerable Members"

        ''' <summary>
        ''' Returns an enumerator for the list.
        ''' </summary>
        ''' <returns>An enumerator.</returns>
        Private Function System_Collections_IEnumerable_GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
			Return sensorList.GetEnumerator()
		End Function

		#End Region
	End Class
End Namespace
