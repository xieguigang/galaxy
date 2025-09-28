'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Collections
Imports System.Collections.Generic
Imports System.Collections.Specialized

Namespace Taskbar
	''' <summary>
	''' Represents a collection of jump list items.
	''' </summary>
	''' <typeparam name="T">The type of elements in this collection.</typeparam>
	Friend Class JumpListItemCollection(Of T)
		Implements ICollection(Of T)
		Implements INotifyCollectionChanged
		Private items As New List(Of T)()

        ''' <summary>
        ''' Occurs anytime a change is made to the underlying collection.
        ''' </summary>
        Public Event CollectionChanged As NotifyCollectionChangedEventHandler Implements INotifyCollectionChanged.CollectionChanged

        ''' <summary>
        ''' Gets or sets a value that determines if this collection is read-only.
        ''' </summary>
        Public Property IsReadOnly() As Boolean Implements ICollection(Of T).IsReadOnly
			Get
				Return m_IsReadOnly
			End Get
			Set
				m_IsReadOnly = Value
			End Set
		End Property
		Private m_IsReadOnly As Boolean

		''' <summary>
		''' Gets a count of the items currently in this collection.
		''' </summary>
		Public ReadOnly Property Count() As Integer Implements ICollection(Of T).Count
			Get
				Return items.Count
			End Get
		End Property

		''' <summary>
		''' Adds the specified item to this collection.
		''' </summary>
		''' <param name="item">The item to add.</param>
		Public Sub Add(item As T) Implements ICollection(Of T).Add
			items.Add(item)

			' Trigger CollectionChanged event
			RaiseEvent CollectionChanged(Me, New NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item))
		End Sub

		''' <summary>
		''' Removes the first instance of the specified item from the collection.
		''' </summary>
		''' <param name="item">The item to remove.</param>
		''' <returns><b>true</b> if an item was removed, otherwise <b>false</b> if no items were removed.</returns>
		Public Function Remove(item As T) As Boolean Implements ICollection(Of T).Remove
			Dim removed As Boolean = items.Remove(item)

			If removed = True Then
				' Trigger CollectionChanged event
				RaiseEvent CollectionChanged(Me, New NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, 0))
			End If

			Return removed
		End Function

		''' <summary>
		''' Clears all items from this collection.
		''' </summary>
		Public Sub Clear() Implements ICollection(Of T).Clear
			items.Clear()

			' Trigger CollectionChanged event
			RaiseEvent CollectionChanged(Me, New NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset))
		End Sub

		''' <summary>
		''' Determines if this collection contains the specified item.
		''' </summary>
		''' <param name="item">The search item.</param>
		''' <returns><b>true</b> if an item was found, otherwise <b>false</b>.</returns>
		Public Function Contains(item As T) As Boolean Implements ICollection(Of T).Contains
			Return items.Contains(item)
		End Function

		''' <summary>
		''' Copies this collection to a compatible one-dimensional array,
		''' starting at the specified index of the target array.
		''' </summary>
		''' <param name="array">The array name.</param>
		''' <param name="index">The index of the starting element.</param>
		Public Sub CopyTo(array As T(), index As Integer) Implements ICollection(Of T).CopyTo
			items.CopyTo(array, index)
		End Sub

		''' <summary>
		''' Returns an enumerator that iterates through a collection.
		''' </summary>
		''' <returns>An enumerator to iterate through this collection.</returns>
		Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
			Return items.GetEnumerator()
		End Function

        ''' <summary>
        ''' Returns an enumerator that iterates through a collection of a specified type.
        ''' </summary>
        ''' <returns>An enumerator to iterate through this collection.</returns>
        Private Function IEnumerable_GetEnumerator1() As IEnumerator(Of T) Implements IEnumerable(Of T).GetEnumerator
            Return items.GetEnumerator()
        End Function
    End Class
End Namespace
