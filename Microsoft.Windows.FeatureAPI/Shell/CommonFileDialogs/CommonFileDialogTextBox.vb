'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Diagnostics

Namespace Dialogs.Controls
	''' <summary>
	'''  Defines the text box controls in the Common File Dialog.
	''' </summary>
	Public Class CommonFileDialogTextBox
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
		''' Creates a new instance of this class with the specified name and text.
		''' </summary>
		''' <param name="name">The name of this control.</param>
		''' <param name="text">The text to display for this control.</param>
		Public Sub New(name As String, text As String)
			MyBase.New(name, text)
		End Sub

		Friend Property Closed() As Boolean
			Get
				Return m_Closed
			End Get
			Set
				m_Closed = Value
			End Set
		End Property
		Private m_Closed As Boolean

		''' <summary>
		''' Gets or sets a value for the text string contained in the CommonFileDialogTextBox.
		''' </summary>
		Public Overrides Property Text() As String
			Get
				If Not Closed Then
					SyncValue()
				End If

				Return MyBase.Text
			End Get

			Set
				If customizedDialog IsNot Nothing Then
					customizedDialog.SetEditBoxText(Me.Id, value)
				End If

				MyBase.Text = value
			End Set
		End Property

		''' <summary>
		''' Holds an instance of the customized (/native) dialog and should
		''' be null until after the Attach() call is made.
		''' </summary>
		Private customizedDialog As IFileDialogCustomize

		''' <summary>
		''' Attach the TextBox control to the dialog object
		''' </summary>
		''' <param name="dialog">Target dialog</param>
		Friend Overrides Sub Attach(dialog As IFileDialogCustomize)
			Debug.Assert(dialog IsNot Nothing, "CommonFileDialogTextBox.Attach: dialog parameter can not be null")

			' Add a text entry control
			dialog.AddEditBox(Me.Id, Me.Text)

			' Set to local instance in order to gate access to same.
			customizedDialog = dialog

			' Sync unmanaged properties with managed properties
			SyncUnmanagedProperties()

			Closed = False
		End Sub

		Friend Sub SyncValue()
			' Make sure that the local native dialog instance is NOT 
			' null. If it's null, just return the "textValue" var,
			' otherwise, use the native call to get the text value, 
			' setting the textValue member variable then return it.

			If customizedDialog IsNot Nothing Then
				Dim textValue As String
				customizedDialog.GetEditBoxText(Me.Id, textValue)

				MyBase.Text = textValue
			End If
		End Sub
	End Class
End Namespace
