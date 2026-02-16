'Copyright (c) Microsoft Corporation.  All rights reserved.

Namespace Dialogs
	Friend NotInheritable Class DialogsDefaults
		Private Sub New()
		End Sub
		Friend Shared ReadOnly Property Caption() As String
			Get
				Return GlobalLocalizedMessages.DialogDefaultCaption
			End Get
		End Property
		Friend Shared ReadOnly Property MainInstruction() As String
			Get
				Return GlobalLocalizedMessages.DialogDefaultMainInstruction
			End Get
		End Property
		Friend Shared ReadOnly Property Content() As String
			Get
				Return GlobalLocalizedMessages.DialogDefaultContent
			End Get
		End Property

		Friend Const ProgressBarStartingValue As Integer = 0
		Friend Const ProgressBarMinimumValue As Integer = 0
		Friend Const ProgressBarMaximumValue As Integer = 100

		Friend Const IdealWidth As Integer = 0

		' For generating control ID numbers that won't 
		' collide with the standard button return IDs.
		Friend Const MinimumDialogControlId As Integer = TaskDialogNativeMethods.TaskDialogCommonButtonReturnIds.Close + 1
	End Class
End Namespace
