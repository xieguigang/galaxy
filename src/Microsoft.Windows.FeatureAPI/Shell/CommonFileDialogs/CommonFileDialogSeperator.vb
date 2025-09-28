'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Diagnostics

Namespace Dialogs.Controls
	''' <summary>
	''' Defines the class for the simplest separator controls.
	''' </summary>
	Public Class CommonFileDialogSeparator
		Inherits CommonFileDialogControl
		''' <summary>
		''' Attach the Separator control to the dialog object
		''' </summary>
		''' <param name="dialog">Target dialog</param>
		Friend Overrides Sub Attach(dialog As IFileDialogCustomize)
			System.Diagnostics.Debug.Assert(dialog IsNot Nothing, "CommonFileDialogSeparator.Attach: dialog parameter can not be null")

			' Add a separator
			dialog.AddSeparator(Me.Id)

			' Sync unmanaged properties with managed properties
			SyncUnmanagedProperties()
		End Sub
	End Class
End Namespace
