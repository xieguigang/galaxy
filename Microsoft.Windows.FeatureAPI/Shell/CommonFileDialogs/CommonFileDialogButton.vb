'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Diagnostics

Namespace Dialogs.Controls
	''' <summary>
	''' Creates the push button controls used by the Common File Dialog.
	''' </summary>
	Public Class CommonFileDialogButton
		Inherits CommonFileDialogProminentControl
		''' <summary>
		''' Initializes a new instance of this class.
		''' </summary>
		Public Sub New()
			MyBase.New(String.Empty)
		End Sub

		''' <summary>
		''' Initializes a new instance of this class with the text only.
		''' </summary>
		''' <param name="text">The text to display for this control.</param>
		Public Sub New(text As String)
			MyBase.New(text)
		End Sub

		''' <summary>
		''' Initializes a new instance of this class with the specified name and text.
		''' </summary>
		''' <param name="name">The name of this control.</param>
		''' <param name="text">The text to display for this control.</param>
		Public Sub New(name As String, text As String)
			MyBase.New(name, text)
		End Sub

		''' <summary>
		''' Attach the PushButton control to the dialog object
		''' </summary>
		''' <param name="dialog">Target dialog</param>
		Friend Overrides Sub Attach(dialog As IFileDialogCustomize)
			System.Diagnostics.Debug.Assert(dialog IsNot Nothing, "CommonFileDialogButton.Attach: dialog parameter can not be null")

			' Add a push button control
			dialog.AddPushButton(Me.Id, Me.Text)

			' Make this control prominent if needed
			If IsProminent Then
				dialog.MakeProminent(Me.Id)
			End If

			' Sync unmanaged properties with managed properties
			SyncUnmanagedProperties()
		End Sub

        ''' <summary>
        ''' Occurs when the user clicks the control. This event is routed from COM via the event sink.
        ''' </summary>
        Public Event Click As EventHandler

        Friend Sub RaiseClickEvent()
			' Make sure that this control is enabled and has a specified delegate
			If Enabled Then
                RaiseEvent Click(Me, EventArgs.Empty)
            End If
		End Sub
	End Class
End Namespace
