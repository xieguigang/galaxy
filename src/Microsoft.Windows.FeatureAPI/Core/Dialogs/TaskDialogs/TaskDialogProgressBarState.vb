'Copyright (c) Microsoft Corporation.  All rights reserved.

Namespace Dialogs
	''' <summary>
	''' Sets the state of a task dialog progress bar.
	''' </summary>        
	Public Enum TaskDialogProgressBarState
		''' <summary>
		''' Uninitialized state, this should never occur.
		''' </summary>
		None = 0

		''' <summary>
		''' Normal state.
		''' </summary>
		Normal = TaskDialogNativeMethods.ProgressBarState.Normal

		''' <summary>
		''' An error occurred.
		''' </summary>
		[Error] = TaskDialogNativeMethods.ProgressBarState.[Error]

		''' <summary>
		''' The progress is paused.
		''' </summary>
		Paused = TaskDialogNativeMethods.ProgressBarState.Paused

		''' <summary>
		''' Displays marquee (indeterminate) style progress
		''' </summary>
		Marquee
	End Enum
End Namespace
