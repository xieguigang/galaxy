'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Collections.Generic
Imports System.Runtime.InteropServices
Imports Microsoft.VisualBasic.Serialization

Namespace Dialogs

    '''<summary>
    ''' Encapsulates additional configuration needed by NativeTaskDialog
    ''' that it can't get from the TASKDIALOGCONFIG struct.
    '''</summary>
    Public Class NativeTaskDialogSettings

        Public Sub New()
            NativeConfiguration = New TaskDialogNativeMethods.TaskDialogConfiguration()

            ' Apply standard settings.
            NativeConfiguration.size = CUInt(Marshal.SizeOf(NativeConfiguration))
            NativeConfiguration.parentHandle = IntPtr.Zero
            NativeConfiguration.instance = IntPtr.Zero
            NativeConfiguration.taskDialogFlags = TaskDialogNativeMethods.TaskDialogOptions.AllowCancel
            NativeConfiguration.commonButtons = TaskDialogNativeMethods.TaskDialogCommonButtons.Ok
            NativeConfiguration.mainIcon = New TaskDialogNativeMethods.IconUnion(0)
            NativeConfiguration.footerIcon = New TaskDialogNativeMethods.IconUnion(0)
            NativeConfiguration.width = TaskDialogDefaults.IdealWidth

            ' Zero out all the custom button fields.
            NativeConfiguration.buttonCount = 0
            NativeConfiguration.radioButtonCount = 0
            NativeConfiguration.buttons = IntPtr.Zero
            NativeConfiguration.radioButtons = IntPtr.Zero
            NativeConfiguration.defaultButtonIndex = 0
            NativeConfiguration.defaultRadioButtonIndex = 0

            ' Various text defaults.
            NativeConfiguration.windowTitle = TaskDialogDefaults.Caption
            NativeConfiguration.mainInstruction = TaskDialogDefaults.MainInstruction
            NativeConfiguration.content = TaskDialogDefaults.Content
            NativeConfiguration.verificationText = Nothing
            NativeConfiguration.expandedInformation = Nothing
            NativeConfiguration.expandedControlText = Nothing
            NativeConfiguration.collapsedControlText = Nothing
            NativeConfiguration.footerText = Nothing
        End Sub

        Public Property ProgressBarMinimum() As Integer

        Public Property ProgressBarMaximum() As Integer

        Public Property ProgressBarValue() As Integer

        Public Property ProgressBarState() As TaskDialogProgressBarState

        Public Property InvokeHelp() As Boolean

        Public Property NativeConfiguration() As TaskDialogNativeMethods.TaskDialogConfiguration

        Public Property Buttons() As TaskDialogNativeMethods.TaskDialogButton()

        Public Property RadioButtons() As TaskDialogNativeMethods.TaskDialogButton()

        Public Property ElevatedButtons() As List(Of Integer)

        Public Overrides Function ToString() As String
            Return Me.GetJson
        End Function
    End Class
End Namespace
