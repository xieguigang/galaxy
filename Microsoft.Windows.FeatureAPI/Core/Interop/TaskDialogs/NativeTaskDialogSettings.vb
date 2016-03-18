'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Collections.Generic
Imports System.Runtime.InteropServices

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
            Get
                Return m_ProgressBarMinimum
            End Get
            Set
                m_ProgressBarMinimum = Value
            End Set
        End Property
        Private m_ProgressBarMinimum As Integer
        Public Property ProgressBarMaximum() As Integer
            Get
                Return m_ProgressBarMaximum
            End Get
            Set
                m_ProgressBarMaximum = Value
            End Set
        End Property
        Private m_ProgressBarMaximum As Integer
        Public Property ProgressBarValue() As Integer
            Get
                Return m_ProgressBarValue
            End Get
            Set
                m_ProgressBarValue = Value
            End Set
        End Property
        Private m_ProgressBarValue As Integer
        Public Property ProgressBarState() As TaskDialogProgressBarState
            Get
                Return m_ProgressBarState
            End Get
            Set
                m_ProgressBarState = Value
            End Set
        End Property
        Private m_ProgressBarState As TaskDialogProgressBarState
        Public Property InvokeHelp() As Boolean
            Get
                Return m_InvokeHelp
            End Get
            Set
                m_InvokeHelp = Value
            End Set
        End Property
        Private m_InvokeHelp As Boolean
        Public Property NativeConfiguration() As TaskDialogNativeMethods.TaskDialogConfiguration
            Get
                Return m_NativeConfiguration
            End Get
            Private Set
                m_NativeConfiguration = Value
            End Set
        End Property
        Private m_NativeConfiguration As TaskDialogNativeMethods.TaskDialogConfiguration
        Public Property Buttons() As TaskDialogNativeMethods.TaskDialogButton()
            Get
                Return m_Buttons
            End Get
            Set
                m_Buttons = Value
            End Set
        End Property
        Private m_Buttons As TaskDialogNativeMethods.TaskDialogButton()
        Public Property RadioButtons() As TaskDialogNativeMethods.TaskDialogButton()
            Get
                Return m_RadioButtons
            End Get
            Set
                m_RadioButtons = Value
            End Set
        End Property
        Private m_RadioButtons As TaskDialogNativeMethods.TaskDialogButton()
        Public Property ElevatedButtons() As List(Of Integer)
            Get
                Return m_ElevatedButtons
            End Get
            Set
                m_ElevatedButtons = Value
            End Set
        End Property
        Private m_ElevatedButtons As List(Of Integer)
    End Class
End Namespace
