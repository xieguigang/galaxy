'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Collections.ObjectModel
Imports System.Diagnostics
Imports System.Windows.Markup

Namespace Dialogs.Controls
	''' <summary>
	''' Defines the menu controls for the Common File Dialog.
	''' </summary>
	<ContentProperty("Items")> _
	Public Class CommonFileDialogMenu
		Inherits CommonFileDialogProminentControl
		Private m_items As New Collection(Of CommonFileDialogMenuItem)()
		''' <summary>
		''' Gets the collection of CommonFileDialogMenuItem objects.
		''' </summary>
		Public ReadOnly Property Items() As Collection(Of CommonFileDialogMenuItem)
			Get
				Return m_items
			End Get
		End Property

		''' <summary>
		''' Creates a new instance of this class.
		''' </summary>
		Public Sub New()
			MyBase.New()
		End Sub

		''' <summary>
		''' Creates a new instance of this class with the specified text.
		''' </summary>
		''' <param name="text">The text to display for this control.</param>
		Public Sub New(text As String)
			MyBase.New(text)
		End Sub

		''' <summary>
		''' Creates a new instance of this class with the specified name and text.
		''' </summary>
		''' <param name="name">The name of this control.</param>
		''' <param name="text">The text to display for this control.</param>
		Public Sub New(name As String, text As String)
			MyBase.New(name, text)
		End Sub

		''' <summary>
		''' Attach the Menu control to the dialog object.
		''' </summary>
		''' <param name="dialog">the target dialog</param>
		Friend Overrides Sub Attach(dialog As IFileDialogCustomize)
			Debug.Assert(dialog IsNot Nothing, "CommonFileDialogMenu.Attach: dialog parameter can not be null")

			' Add the menu control
			dialog.AddMenu(Me.Id, Me.Text)

			' Add the menu items
			For Each item As CommonFileDialogMenuItem In Me.m_items
				dialog.AddControlItem(Me.Id, item.Id, item.Text)
			Next

			' Make prominent as needed
			If IsProminent Then
				dialog.MakeProminent(Me.Id)
			End If

			' Sync unmanaged properties with managed properties
			SyncUnmanagedProperties()
		End Sub
	End Class

	''' <summary>
	''' Creates the CommonFileDialogMenuItem items for the Common File Dialog.
	''' </summary>
	Public Class CommonFileDialogMenuItem
		Inherits CommonFileDialogControl
		''' <summary>
		''' Creates a new instance of this class.
		''' </summary>
		Public Sub New()
			MyBase.New(String.Empty)
		End Sub

		''' <summary>
		''' Creates a new instance of this class with the specified text.
		''' </summary>
		''' <param name="text">The text to display for this control.</param>
		Public Sub New(text As String)
			MyBase.New(text)
		End Sub

        ''' <summary>
        ''' Occurs when a user clicks a menu item.
        ''' </summary>
        Public Event Click As EventHandler

        Friend Sub RaiseClickEvent()
			' Make sure that this control is enabled and has a specified delegate
			If Enabled Then
				RaiseEvent Click(Me, EventArgs.Empty)
			End If
		End Sub

		''' <summary>
		''' Attach this control to the dialog object
		''' </summary>
		''' <param name="dialog">Target dialog</param>
		Friend Overrides Sub Attach(dialog As IFileDialogCustomize)
			' Items are added via the menu itself
		End Sub
	End Class
End Namespace
