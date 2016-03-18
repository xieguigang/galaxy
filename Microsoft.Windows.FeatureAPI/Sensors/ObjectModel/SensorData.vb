' Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Collections.Generic
Imports System.Runtime.InteropServices
Imports Microsoft.Windows.Shell.PropertySystem
Imports Microsoft.Windows.Internal

Namespace Sensors
	''' <summary>
	''' Defines a mapping of data field identifiers to the data returned in a sensor report.
	''' </summary>    
	Public Class SensorData
		Implements IDictionary(Of Guid, IList(Of Object))
		#Region "implementation"
		Friend Shared Function FromNativeReport(iSensor As ISensor, iReport As ISensorDataReport) As SensorData
			Dim data As New SensorData()

			Dim keyCollection As IPortableDeviceKeyCollection
			Dim valuesCollection As IPortableDeviceValues
			iSensor.GetSupportedDataFields(keyCollection)
			iReport.GetSensorValues(keyCollection, valuesCollection)

			Dim items As UInteger = 0
			keyCollection.GetCount(items)
            For index As UInteger = 0 To items - 1UI
                Dim key As PropertyKey
                Using propValue As New PropVariant()
                    keyCollection.GetAt(index, key)
                    valuesCollection.GetValue(key, propValue)

                    If data.ContainsKey(key.FormatId) Then
                        data(key.FormatId).Add(propValue.Value)
                    Else
                        data.Add(key.FormatId, New List(Of Object)() From {
                            propValue.Value
                        })
                    End If
                End Using
            Next

            If keyCollection IsNot Nothing Then
				Marshal.ReleaseComObject(keyCollection)
				keyCollection = Nothing
			End If

			If valuesCollection IsNot Nothing Then
				Marshal.ReleaseComObject(valuesCollection)
				valuesCollection = Nothing
			End If


			Return data
		End Function
		#End Region

		Private sensorDataDictionary As New Dictionary(Of Guid, IList(Of Object))()

#Region "IDictionary<Guid,IList<object>> Members"

        ''' <summary>
        ''' Adds a data item to the dictionary.
        ''' </summary>
        ''' <param name="key">The data field identifier.</param>
        ''' <param name="value">The data list.</param>
        Public Sub Add(key As Guid, value As IList(Of Object)) Implements IDictionary(Of Guid, IList(Of Object)).Add
            sensorDataDictionary.Add(key, value)
        End Sub
        ''' <summary>
        ''' Determines if a particular data field itentifer is present in the collection.
        ''' </summary>
        ''' <param name="key">The data field identifier.</param>
        ''' <returns><b>true</b> if the data field identifier is present.</returns>
        Public Function ContainsKey(key As Guid) As Boolean Implements IDictionary(Of Guid, IList(Of Object)).ContainsKey
            Return sensorDataDictionary.ContainsKey(key)
        End Function
        ''' <summary>
        ''' Gets the list of the data field identifiers in this collection.
        ''' </summary>
        Public ReadOnly Property Keys() As ICollection(Of Guid) Implements IDictionary(Of Guid, IList(Of Object)).Keys
            Get
                Return sensorDataDictionary.Keys
            End Get
        End Property
        ''' <summary>
        ''' Removes a particular data field identifier from the collection.
        ''' </summary>
        ''' <param name="key">The data field identifier.</param>
        ''' <returns><b>true</b> if the item was removed.</returns>
        Public Function Remove(key As Guid) As Boolean Implements IDictionary(Of Guid, IList(Of Object)).Remove
            Return sensorDataDictionary.Remove(key)
        End Function
        ''' <summary>
        ''' Attempts to get the value of a data item.
        ''' </summary>
        ''' <param name="key">The data field identifier.</param>
        ''' <param name="value">The data list.</param>
        ''' <returns><b>true</b> if able to obtain the value; otherwise <b>false</b>.</returns>
        Public Function TryGetValue(key As Guid, ByRef value As IList(Of Object)) As Boolean Implements IDictionary(Of Guid, IList(Of Object)).TryGetValue
            Return sensorDataDictionary.TryGetValue(key, value)
        End Function
        ''' <summary>
        ''' Gets the list of data values in the dictionary.
        ''' </summary>
        Public ReadOnly Property Values() As ICollection(Of IList(Of Object)) Implements IDictionary(Of Guid, IList(Of Object)).Values
			Get
				Return sensorDataDictionary.Values
			End Get
		End Property
        ''' <summary>
        ''' Gets or sets the index operator for the dictionary by key.
        ''' </summary>
        ''' <param name="key">A GUID.</param>
        ''' <returns>The item at the specified index.</returns>
        Default Public Property Item(key As Guid) As IList(Of Object) Implements IDictionary(Of Guid, IList(Of Object)).Item
            Get
                Return sensorDataDictionary(key)
            End Get
            Set
                sensorDataDictionary(key) = Value
            End Set
        End Property

#End Region

#Region "ICollection<KeyValuePair<Guid,IList<object>>> Members"
        ''' <summary>
        ''' Adds a specified key/value data pair to the collection.
        ''' </summary>
        ''' <param name="item">The item to add.</param>
        Public Sub Add(item As KeyValuePair(Of Guid, IList(Of Object))) Implements IDictionary(Of Guid, IList(Of Object)).Add
            Dim c As ICollection(Of KeyValuePair(Of Guid, IList(Of Object))) = TryCast(sensorDataDictionary, ICollection(Of KeyValuePair(Of Guid, IList(Of Object))))
            c.Add(item)
        End Sub

        ''' <summary>
        ''' Clears the items from the collection.
        ''' </summary>
        Public Sub Clear() Implements ICollection(Of KeyValuePair(Of Guid, IList(Of Object))).Clear
			Dim c As ICollection(Of KeyValuePair(Of Guid, IList(Of Object))) = TryCast(sensorDataDictionary, ICollection(Of KeyValuePair(Of Guid, IList(Of Object))))
			c.Clear()
		End Sub

        ''' <summary>
        ''' Determines if the collection contains the specified key/value pair. 
        ''' </summary>
        ''' <param name="item">The item to locate.</param>
        ''' <returns><b>true</b> if the collection contains the key/value pair; otherwise false.</returns>
        Public Function Contains(item As KeyValuePair(Of Guid, IList(Of Object))) As Boolean Implements IDictionary(Of Guid, IList(Of Object)).Contains
            Dim c As ICollection(Of KeyValuePair(Of Guid, IList(Of Object))) = TryCast(sensorDataDictionary, ICollection(Of KeyValuePair(Of Guid, IList(Of Object))))
            Return c.Contains(item)
        End Function

        ''' <summary>
        ''' Copies an element collection to another collection.
        ''' </summary>
        ''' <param name="array">The destination collection.</param>
        ''' <param name="arrayIndex">The index of the item to copy.</param>
        Public Sub CopyTo(array As KeyValuePair(Of Guid, IList(Of Object))(), arrayIndex As Integer) Implements IDictionary(Of Guid, IList(Of Object)).CopyTo
            Dim c As ICollection(Of KeyValuePair(Of Guid, IList(Of Object))) = TryCast(sensorDataDictionary, ICollection(Of KeyValuePair(Of Guid, IList(Of Object))))
            c.CopyTo(array, arrayIndex)
        End Sub

        ''' <summary>
        ''' Returns the number of items in the collection.
        ''' </summary>
        Public ReadOnly Property Count() As Integer Implements ICollection(Of KeyValuePair(Of Guid, IList(Of Object))).Count
			Get
				Return sensorDataDictionary.Count
			End Get
		End Property

		''' <summary>
		''' Gets a value that determines if the collection is read-only.
		''' </summary>
		Public ReadOnly Property IsReadOnly() As Boolean Implements ICollection(Of KeyValuePair(Of Guid, IList(Of Object))).IsReadOnly
			Get
				Return True
			End Get
		End Property

        ''' <summary>
        ''' Removes a particular item from the collection.
        ''' </summary>
        ''' <param name="item">The item to remove.</param>
        ''' <returns><b>true</b> if successful; otherwise <b>false</b></returns>
        Public Function Remove(item As KeyValuePair(Of Guid, IList(Of Object))) As Boolean Implements IDictionary(Of Guid, IList(Of Object)).Remove
            Dim c As ICollection(Of KeyValuePair(Of Guid, IList(Of Object))) = TryCast(sensorDataDictionary, ICollection(Of KeyValuePair(Of Guid, IList(Of Object))))
            Return c.Remove(item)
        End Function

#End Region

#Region "IEnumerable<KeyValuePair<Guid,IList<object>>> Members"
        ''' <summary>
        ''' Returns an enumerator for the collection.
        ''' </summary>
        ''' <returns>An enumerator.</returns>
        Public Function GetEnumerator() As IEnumerator(Of KeyValuePair(Of Guid, IList(Of Object))) Implements IEnumerable(Of KeyValuePair(Of Guid, IList(Of Object))).GetEnumerator
			Return TryCast(sensorDataDictionary, IEnumerator(Of KeyValuePair(Of Guid, IList(Of Object))))
		End Function

		#End Region

		#Region "IEnumerable Members"
		''' <summary>
		''' Returns an enumerator for the collection.
		''' </summary>
		''' <returns>An enumerator.</returns>
		Private Function System_Collections_IEnumerable_GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
			Return TryCast(sensorDataDictionary, System.Collections.IEnumerator)
		End Function

		#End Region
	End Class
End Namespace
