'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Diagnostics

Namespace Dialogs.Controls
	''' <summary>
	''' Defines the label controls in the Common File Dialog.
	''' </summary>
	Public Class CommonFileDialogLabel
		Inherits CommonFileDialogControl
		''' <summary>
		''' Creates a new instance of this class.
		''' </summary>
		Public Sub New()
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
		''' Attach this control to the dialog object
		''' </summary>
		''' <param name="dialog">Target dialog</param>
		Friend Overrides Sub Attach(dialog As IFileDialogCustomize)
			Debug.Assert(dialog IsNot Nothing, "CommonFileDialog.Attach: dialog parameter can not be null")

			' Add a text control
			dialog.AddText(Me.Id, Me.Text)

			' Sync unmanaged properties with managed properties
			SyncUnmanagedProperties()
		End Sub
	End Class
End Namespace
