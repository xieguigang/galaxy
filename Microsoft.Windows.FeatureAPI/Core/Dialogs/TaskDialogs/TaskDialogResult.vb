'Copyright (c) Microsoft Corporation.  All rights reserved.

Namespace Dialogs
	''' <summary>
	''' Indicates the various buttons and options clicked by the user on the task dialog.
	''' </summary>        
	Public Enum TaskDialogResult
		''' <summary>
		''' No button was selected.
		''' </summary>
		None = &H0

		''' <summary>
		''' "OK" button was clicked
		''' </summary>
		Ok = &H1

		''' <summary>
		''' "Yes" button was clicked
		''' </summary>
		Yes = &H2

		''' <summary>
		''' "No" button was clicked
		''' </summary>
		No = &H4

		''' <summary>
		''' "Cancel" button was clicked
		''' </summary>
		Cancel = &H8

		''' <summary>
		''' "Retry" button was clicked
		''' </summary>
		Retry = &H10

		''' <summary>
		''' "Close" button was clicked
		''' </summary>
		Close = &H20

		''' <summary>
		''' A custom button was clicked.
		''' </summary>
		CustomButtonClicked = &H100
	End Enum
End Namespace
