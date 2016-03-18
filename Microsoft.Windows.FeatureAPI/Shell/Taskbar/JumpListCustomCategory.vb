'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Collections.Generic
Imports System.Collections.Specialized
Imports System.Linq

Namespace Taskbar
	''' <summary>
	''' Represents a custom category on the taskbar's jump list
	''' </summary>
	Public Class JumpListCustomCategory
		Private m_name As String

		Friend Property JumpListItems() As JumpListItemCollection(Of IJumpListItem)
			Get
				Return m_JumpListItems
			End Get
			Private Set
				m_JumpListItems = Value
			End Set
		End Property
		Private m_JumpListItems As JumpListItemCollection(Of IJumpListItem)

		''' <summary>
		''' Category name
		''' </summary>
		Public Property Name() As String
			Get
				Return m_name
			End Get
			Set
				If value <> m_name Then
					m_name = Value
                    RaiseEvent CollectionChanged(Me, New NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset))
                End If
			End Set
		End Property


		''' <summary>
		''' Add JumpList items for this category
		''' </summary>
		''' <param name="items">The items to add to the JumpList.</param>
		Public Sub AddJumpListItems(ParamArray items As IJumpListItem())
			If items IsNot Nothing Then
				For Each item As IJumpListItem In items
					JumpListItems.Add(item)
				Next
			End If
		End Sub

        ''' <summary>
        ''' Event that is triggered when the jump list collection is modified
        ''' </summary>
        Friend Event CollectionChanged As NotifyCollectionChangedEventHandler

        ''' <summary>
        ''' Creates a new custom category instance
        ''' </summary>
        ''' <param name="categoryName">Category name</param>
        Public Sub New(categoryName As String)
			Name = categoryName

			JumpListItems = New JumpListItemCollection(Of IJumpListItem)()
			AddHandler JumpListItems.CollectionChanged, AddressOf OnJumpListCollectionChanged
		End Sub

		Friend Sub OnJumpListCollectionChanged(sender As Object, args As NotifyCollectionChangedEventArgs)
            RaiseEvent CollectionChanged(Me, args)
        End Sub


		Friend Sub RemoveJumpListItem(path As String)
            Dim itemsToRemove As New List(Of IJumpListItem)(From i In JumpListItems Where String.Equals(path, i.Path, StringComparison.OrdinalIgnoreCase) Select i)

            ' Remove matching items
            For i As Integer = 0 To itemsToRemove.Count - 1
				JumpListItems.Remove(itemsToRemove(i))
			Next
		End Sub
	End Class
End Namespace
