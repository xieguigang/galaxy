'Copyright (c) Microsoft Corporation.  All rights reserved.


Namespace Dialogs
	''' <summary>
	''' Identifies one of the standard buttons that 
	''' can be displayed via TaskDialog.
	''' </summary>
	<Flags> _
	Public Enum TaskDialogStandardButtons
		''' <summary>
		''' No buttons on the dialog.
		''' </summary>
		None = &H0

		''' <summary>
		''' An "OK" button.
		''' </summary>
		Ok = &H1

		''' <summary>
		''' A "Yes" button.
		''' </summary>
		Yes = &H2

		''' <summary>
		''' A "No" button.
		''' </summary>
		No = &H4

		''' <summary>
		''' A "Cancel" button.
		''' </summary>
		Cancel = &H8

		''' <summary>
		''' A "Retry" button.
		''' </summary>
		Retry = &H10

		''' <summary>
		''' A "Close" button.
		''' </summary>
		Close = &H20
	End Enum
End Namespace
