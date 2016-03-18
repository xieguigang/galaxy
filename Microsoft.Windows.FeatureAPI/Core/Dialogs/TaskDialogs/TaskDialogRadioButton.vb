'Copyright (c) Microsoft Corporation.  All rights reserved.

Namespace Dialogs
	''' <summary>
	''' Defines a radio button that can be hosted in by a 
	''' <see cref="TaskDialog"/> object.
	''' </summary>
	Public Class TaskDialogRadioButton
		Inherits TaskDialogButtonBase
		''' <summary>
		''' Creates a new instance of this class.
		''' </summary>
		Public Sub New()
		End Sub

		''' <summary>
		''' Creates a new instance of this class with
		''' the specified name and text.
		''' </summary>
		''' <param name="name">The name for this control.</param>
		''' <param name="text">The value for this controls 
		''' <see cref="P:Dialogs.TaskDialogButtonBase.Text"/> property.</param>
		Public Sub New(name As String, text As String)
			MyBase.New(name, text)
		End Sub
	End Class
End Namespace
