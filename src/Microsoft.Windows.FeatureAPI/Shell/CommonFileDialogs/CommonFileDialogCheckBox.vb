'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Diagnostics

Namespace Dialogs.Controls
	''' <summary>
	''' Creates the check button controls used by the Common File Dialog.
	''' </summary>
	Public Class CommonFileDialogCheckBox
		Inherits CommonFileDialogProminentControl
		Private m_isChecked As Boolean
		''' <summary>
		''' Gets or sets the state of the check box.
		''' </summary>
		Public Property IsChecked() As Boolean
			Get
				Return m_isChecked
			End Get
			Set
				' Check if property has changed
				If m_isChecked <> value Then
					m_isChecked = value
					ApplyPropertyChange("IsChecked")
				End If
			End Set
		End Property

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
		''' Creates a new instance of this class with the specified text and check state.
		''' </summary>
		''' <param name="text">The text to display for this control.</param>
		''' <param name="isChecked">The check state of this control.</param>
		Public Sub New(text As String, isChecked As Boolean)
			MyBase.New(text)
			Me.m_isChecked = isChecked
		End Sub

		''' <summary>
		''' Creates a new instance of this class with the specified name, text and check state.
		''' </summary>
		''' <param name="name">The name of this control.</param>
		''' <param name="text">The text to display for this control.</param>
		''' <param name="isChecked">The check state of this control.</param>
		Public Sub New(name As String, text As String, isChecked As Boolean)
			MyBase.New(name, text)
			Me.m_isChecked = isChecked
		End Sub

        Dim _CheckedChanged As EventHandler

        ''' <summary>
        ''' Occurs when the user changes the check state.
        ''' </summary>
        Public Custom Event CheckedChanged As EventHandler
            AddHandler(value As EventHandler)
                _CheckedChanged = value
            End AddHandler
            RemoveHandler(value As EventHandler)
                _CheckedChanged = Nothing
            End RemoveHandler
            RaiseEvent(sender As Object, e As EventArgs)
                Call _CheckedChanged.Invoke(sender, e)
            End RaiseEvent
        End Event

        Friend Sub RaiseCheckedChangedEvent()
			' Make sure that this control is enabled and has a specified delegate
			If Enabled Then
                Me._CheckedChanged(Me, EventArgs.Empty)
            End If
		End Sub

		''' <summary>
		''' Attach the CheckButton control to the dialog object.
		''' </summary>
		''' <param name="dialog">the target dialog</param>
		Friend Overrides Sub Attach(dialog As IFileDialogCustomize)
			System.Diagnostics.Debug.Assert(dialog IsNot Nothing, "CommonFileDialogCheckBox.Attach: dialog parameter can not be null")

			' Add a check button control
			dialog.AddCheckButton(Me.Id, Me.Text, Me.m_isChecked)

			' Make this control prominent if needed
			If IsProminent Then
				dialog.MakeProminent(Me.Id)
			End If

			' Make sure this property is set
			ApplyPropertyChange("IsChecked")

			' Sync unmanaged properties with managed properties
			SyncUnmanagedProperties()
		End Sub
	End Class
End Namespace
