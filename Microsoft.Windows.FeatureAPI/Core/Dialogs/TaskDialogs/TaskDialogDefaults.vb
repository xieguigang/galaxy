'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports Microsoft.Windows.Resources

Namespace Dialogs
	Friend NotInheritable Class TaskDialogDefaults
		Private Sub New()
		End Sub
		Public Shared ReadOnly Property Caption() As String
			Get
				Return "LocalizedMessages.TaskDialogDefaultCaption"
			End Get
		End Property
		Public Shared ReadOnly Property MainInstruction() As String
			Get
				Return "LocalizedMessages.TaskDialogDefaultMainInstruction"
			End Get
		End Property
		Public Shared ReadOnly Property Content() As String
			Get
				Return "LocalizedMessages.TaskDialogDefaultContent"
			End Get
		End Property

		Public Const ProgressBarMinimumValue As Integer = 0
		Public Const ProgressBarMaximumValue As Integer = 100

		Public Const IdealWidth As Integer = 0

        ' For generating control ID numbers that won't 
        ' collide with the standard button return IDs.
        Public Const MinimumDialogControlId As Integer = TaskDialogNativeMethods.TaskDialogCommonButtonReturnIds.Close + 1
    End Class
End Namespace
