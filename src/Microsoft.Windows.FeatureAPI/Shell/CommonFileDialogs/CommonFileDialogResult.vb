'Copyright (c) Microsoft Corporation.  All rights reserved.

Namespace Dialogs
	''' <summary>
	''' Specifies identifiers to indicate the return value of a CommonFileDialog dialog.
	''' </summary>
	Public Enum CommonFileDialogResult
		''' <summary>
		''' Default value for enumeration, a dialog box should never return this value.
		''' </summary>
		None = 0

		''' <summary>
		''' The dialog box return value is OK (usually sent from a button labeled OK or Save).
		''' </summary>
		Ok = 1

		''' <summary>
		''' The dialog box return value is Cancel (usually sent from a button labeled Cancel).
		''' </summary>
		Cancel = 2
	End Enum
End Namespace
