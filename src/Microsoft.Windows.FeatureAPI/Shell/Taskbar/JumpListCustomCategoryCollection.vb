'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Collections
Imports System.Collections.Generic
Imports System.Collections.Specialized

Namespace Taskbar
	''' <summary>
	''' Represents a collection of custom categories
	''' </summary>
	Friend Class JumpListCustomCategoryCollection
		Implements ICollection(Of JumpListCustomCategory)
		Implements INotifyCollectionChanged
		Private categories As New List(Of JumpListCustomCategory)()

        ''' <summary>
        ''' Event to trigger anytime this collection is modified
        ''' </summary>
        Public Event CollectionChanged As NotifyCollectionChangedEventHandler Implements INotifyCollectionChanged.CollectionChanged

        ''' <summary>
        ''' Determines if this collection is read-only
        ''' </summary>
        Public Property IsReadOnly() As Boolean Implements ICollection(Of JumpListCustomCategory).IsReadOnly
			Get
				Return m_IsReadOnly
			End Get
			Set
				m_IsReadOnly = Value
			End Set
		End Property
		Private m_IsReadOnly As Boolean

		''' <summary>
		''' The number of items in this collection
		''' </summary>
		Public ReadOnly Property Count() As Integer Implements ICollection(Of JumpListCustomCategory).Count
			Get
				Return categories.Count
			End Get
		End Property

        ''' <summary>
        ''' Add the specified category to this collection
        ''' </summary>
        ''' <param name="category">Category to add</param>
        Public Sub Add(category As JumpListCustomCategory) Implements ICollection(Of JumpListCustomCategory).Add
            If category Is Nothing Then
                Throw New ArgumentNullException("category")
            End If
            categories.Add(category)

            ' Trigger CollectionChanged event
            RaiseEvent CollectionChanged(Me, New NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, category))

            ' Make sure that a collection changed event is fire if this category
            ' or it's corresponding jumplist is modified
            AddHandler category.CollectionChanged, Sub(sender, e) RaiseEvent CollectionChanged(sender, e)
            AddHandler category.JumpListItems.CollectionChanged, Sub(sender, e) RaiseEvent CollectionChanged(sender, e)
        End Sub

        ''' <summary>
        ''' Remove the specified category from this collection
        ''' </summary>
        ''' <param name="category">Category item to remove</param>
        ''' <returns>True if item was removed.</returns>
        Public Function Remove(category As JumpListCustomCategory) As Boolean Implements ICollection(Of JumpListCustomCategory).Remove
            Dim removed As Boolean = categories.Remove(category)

            If removed = True Then
                ' Trigger CollectionChanged event
                RaiseEvent CollectionChanged(Me, New NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, 0))
            End If

            Return removed
        End Function

        ''' <summary>
        ''' Clear all items from the collection
        ''' </summary>
        Public Sub Clear() Implements ICollection(Of JumpListCustomCategory).Clear
			categories.Clear()

			RaiseEvent CollectionChanged(Me, New NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset))
		End Sub

        ''' <summary>
        ''' Determine if this collection contains the specified item
        ''' </summary>
        ''' <param name="category">Category to search for</param>
        ''' <returns>True if category was found</returns>
        Public Function Contains(category As JumpListCustomCategory) As Boolean Implements ICollection(Of JumpListCustomCategory).Contains
            Return categories.Contains(category)
        End Function

        ''' <summary>
        ''' Copy this collection to a compatible one-dimensional array,
        ''' starting at the specified index of the target array
        ''' </summary>
        ''' <param name="array">Array to copy to</param>
        ''' <param name="index">Index of target array to start copy</param>
        Public Sub CopyTo(array As JumpListCustomCategory(), index As Integer) Implements ICollection(Of JumpListCustomCategory).CopyTo
            categories.CopyTo(array, index)
        End Sub

        ''' <summary>
        ''' Returns an enumerator that iterates through this collection.
        ''' </summary>
        ''' <returns>Enumerator to iterate through this collection.</returns>
        Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
			Return categories.GetEnumerator()
		End Function

        ''' <summary>
        ''' Returns an enumerator that iterates through this collection.
        ''' </summary>
        ''' <returns>Enumerator to iterate through this collection.</returns>
        Private Function IEnumerable_GetEnumerator1() As IEnumerator(Of JumpListCustomCategory) Implements IEnumerable(Of JumpListCustomCategory).GetEnumerator
            Return categories.GetEnumerator()
        End Function
    End Class
End Namespace
