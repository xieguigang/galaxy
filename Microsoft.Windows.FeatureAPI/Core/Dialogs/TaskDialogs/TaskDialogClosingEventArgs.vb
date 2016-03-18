'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.ComponentModel

Namespace Dialogs
	''' <summary>
	''' Data associated with <see cref="TaskDialog.Closing"/> event.
	''' </summary>
	Public Class TaskDialogClosingEventArgs
		Inherits CancelEventArgs
		Private m_taskDialogResult As TaskDialogResult
		''' <summary>
		''' Gets or sets the standard button that was clicked.
		''' </summary>
		Public Property TaskDialogResult() As TaskDialogResult
			Get
				Return m_taskDialogResult
			End Get
			Set
				m_taskDialogResult = value
			End Set
		End Property

		Private m_customButton As String
		''' <summary>
		''' Gets or sets the text of the custom button that was clicked.
		''' </summary>
		Public Property CustomButton() As String
			Get
				Return m_customButton
			End Get
			Set
				m_customButton = value
			End Set
		End Property


	End Class
End Namespace
