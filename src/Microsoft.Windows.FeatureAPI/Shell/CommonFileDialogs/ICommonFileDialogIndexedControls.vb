'Copyright (c) Microsoft Corporation.  All rights reserved.


Namespace Dialogs.Controls
	''' <summary>
	''' Specifies a property, event and method that indexed controls need
	''' to implement.
	''' </summary>
	''' 
	''' <remarks>
	''' not sure where else to put this, so leaving here for now.
	''' </remarks>
	Interface ICommonFileDialogIndexedControls
		Property SelectedIndex() As Integer

		Event SelectedIndexChanged As EventHandler

		Sub RaiseSelectedIndexChangedEvent()
	End Interface
End Namespace
