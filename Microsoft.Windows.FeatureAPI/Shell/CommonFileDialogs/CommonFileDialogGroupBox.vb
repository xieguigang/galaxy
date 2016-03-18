'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Collections.ObjectModel
Imports System.Diagnostics
Imports System.Windows.Markup

Namespace Dialogs.Controls
	''' <summary>
	''' Represents a group box control for the Common File Dialog.
	''' </summary>note
	<ContentProperty("Items")> _
	Public Class CommonFileDialogGroupBox
		Inherits CommonFileDialogProminentControl
		Private m_items As Collection(Of DialogControl)
		''' <summary>
		''' Gets the collection of controls for this group box.
		''' </summary>
		Public ReadOnly Property Items() As Collection(Of DialogControl)
			Get
				Return m_items
			End Get
		End Property

		''' <summary>
		''' Creates a new instance of this class.
		''' </summary>
		Public Sub New()
			MyBase.New(String.Empty)
			Initialize()
		End Sub

		''' <summary>
		''' Create a new instance of this class with the specified text.
		''' </summary>
		''' <param name="text">The text to display for this control.</param>
		Public Sub New(text As String)
			MyBase.New(text)
			Initialize()
		End Sub

		''' <summary>
		''' Creates a new instance of this class with the specified name and text.
		''' </summary>
		''' <param name="name">The name of this control.</param>
		''' <param name="text">The text to display for this control.</param>
		Public Sub New(name As String, text As String)
			MyBase.New(name, text)
			Initialize()
		End Sub

		''' <summary>
		''' Initializes the item collection for this class.
		''' </summary>
		Private Sub Initialize()
			m_items = New Collection(Of DialogControl)()
		End Sub

		''' <summary>
		''' Attach the GroupBox control to the dialog object
		''' </summary>
		''' <param name="dialog">Target dialog</param>
		Friend Overrides Sub Attach(dialog As IFileDialogCustomize)
			Debug.Assert(dialog IsNot Nothing, "CommonFileDialogGroupBox.Attach: dialog parameter can not be null")

			' Start a visual group
			dialog.StartVisualGroup(Me.Id, Me.Text)

			' Add child controls
			For Each item As CommonFileDialogControl In Me.m_items
				item.HostingDialog = HostingDialog
				item.Attach(dialog)
			Next

			' End visual group
			dialog.EndVisualGroup()

			' Make this control prominent if needed
			If IsProminent Then
				dialog.MakeProminent(Me.Id)
			End If

			' Sync unmanaged properties with managed properties
			SyncUnmanagedProperties()
		End Sub

	End Class
End Namespace
