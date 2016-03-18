'Copyright (c) Microsoft Corporation.  All rights reserved.

Namespace Dialogs
	''' <summary>
	''' Declares the abstract base class for all custom task dialog controls.
	''' </summary>
	Public MustInherit Class TaskDialogControl
		Inherits DialogControl
		''' <summary>
		''' Creates a new instance of a task dialog control.
		''' </summary>
		Protected Sub New()
		End Sub
		''' <summary>
		''' Creates a new instance of a task dialog control with the specified name.
		''' </summary>
		''' <param name="name">The name for this control.</param>
		Protected Sub New(name As String)
			MyBase.New(name)
		End Sub
	End Class
End Namespace
