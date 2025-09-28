'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Collections

Namespace Taskbar
	''' <summary>
	''' Event arguments for when the user is notified of items
	''' that have been removed from the taskbar destination list
	''' </summary>
	Public Class UserRemovedJumpListItemsEventArgs
		Inherits EventArgs
		Private ReadOnly _removedItems As IEnumerable

		Friend Sub New(RemovedItems As IEnumerable)
			_removedItems = RemovedItems
		End Sub

		''' <summary>
		''' The collection of removed items based on path.
		''' </summary>
		Public ReadOnly Property RemovedItems() As IEnumerable
			Get
				Return _removedItems
			End Get
		End Property
	End Class
End Namespace
