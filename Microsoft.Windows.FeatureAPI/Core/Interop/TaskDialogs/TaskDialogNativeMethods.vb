'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Runtime.InteropServices
Imports Microsoft.Windows.Internal

Namespace Dialogs

    ''' <summary>
    ''' Internal class containing most native interop declarations used
    ''' throughout the library.
    ''' Functions that are not performance intensive belong in this class.
    ''' </summary>

    Public Module TaskDialogNativeMethods

#Region "TaskDialog Definitions"

        <DllImport("Comctl32.dll", SetLastError:=True)>
        Public Function TaskDialogIndirect(<[In]> taskConfig As TaskDialogNativeMethods.TaskDialogConfiguration, <Out> ByRef button As Integer, <Out> ByRef radioButton As Integer, <MarshalAs(UnmanagedType.Bool), Out> ByRef verificationFlagChecked As Boolean) As HResult
        End Function

        ' Main task dialog configuration struct.
        ' NOTE: Packing must be set to 4 to make this work on 64-bit platforms.
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto, Pack:=4)>
        Public Class TaskDialogConfiguration
            Public size As UInteger
            Public parentHandle As IntPtr
            Public instance As IntPtr
            Public taskDialogFlags As TaskDialogOptions
            Public commonButtons As TaskDialogCommonButtons
            <MarshalAs(UnmanagedType.LPWStr)>
            Public windowTitle As String
            Public mainIcon As IconUnion
            ' NOTE: 32-bit union field, holds pszMainIcon as well
            <MarshalAs(UnmanagedType.LPWStr)>
            Public mainInstruction As String
            <MarshalAs(UnmanagedType.LPWStr)>
            Public content As String
            Public buttonCount As UInteger
            Public buttons As IntPtr
            ' Ptr to TASKDIALOG_BUTTON structs
            Public defaultButtonIndex As Integer
            Public radioButtonCount As UInteger
            Public radioButtons As IntPtr
            ' Ptr to TASKDIALOG_BUTTON structs
            Public defaultRadioButtonIndex As Integer
            <MarshalAs(UnmanagedType.LPWStr)>
            Public verificationText As String
            <MarshalAs(UnmanagedType.LPWStr)>
            Public expandedInformation As String
            <MarshalAs(UnmanagedType.LPWStr)>
            Public expandedControlText As String
            <MarshalAs(UnmanagedType.LPWStr)>
            Public collapsedControlText As String
            Public footerIcon As IconUnion
            ' NOTE: 32-bit union field, holds pszFooterIcon as well
            <MarshalAs(UnmanagedType.LPWStr)>
            Public footerText As String
            Public callback As TaskDialogCallback
            Public callbackData As IntPtr
            Public width As UInteger
        End Class

        Public Const TaskDialogIdealWidth As Integer = 0
        ' Value for TASKDIALOGCONFIG.cxWidth
        Public Const TaskDialogButtonShieldIcon As Integer = 1

        ' NOTE: We include a "spacer" so that the struct size varies on 
        ' 64-bit architectures.
        <StructLayout(LayoutKind.Explicit, CharSet:=CharSet.Auto)>
        Public Structure IconUnion
            Public Sub New(i As Integer)
                m_mainIcon = i
                spacer = IntPtr.Zero
            End Sub

            <FieldOffset(0)>
            Private m_mainIcon As Integer

            ' This field is used to adjust the length of the structure on 32/64bit OS.
            <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>
            <FieldOffset(0)>
            Private spacer As IntPtr

            ''' <summary>
            ''' Gets the handle to the Icon
            ''' </summary>
            Public ReadOnly Property MainIcon() As Integer
                Get
                    Return m_mainIcon
                End Get
            End Property
        End Structure

        ' NOTE: Packing must be set to 4 to make this work on 64-bit platforms.
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto, Pack:=4)>
        Public Structure TaskDialogButton
            Public Sub New(buttonId As Integer, text As String)
                Me.buttonId = buttonId
                buttonText = text
            End Sub

            Public buttonId As Integer
            <MarshalAs(UnmanagedType.LPWStr)>
            Public buttonText As String
        End Structure

        ' Task Dialog - identifies common buttons.
        <Flags>
        Public Enum TaskDialogCommonButtons
            Ok = &H1
            ' selected control return value IDOK
            Yes = &H2
            ' selected control return value IDYES
            No = &H4
            ' selected control return value IDNO
            Cancel = &H8
            ' selected control return value IDCANCEL
            Retry = &H10
            ' selected control return value IDRETRY
            Close = &H20
            ' selected control return value IDCLOSE
        End Enum

        ' Identify button *return values* - note that, unfortunately, these are different
        ' from the inbound button values.
        Public Enum TaskDialogCommonButtonReturnIds
            Ok = 1
            Cancel = 2
            Abort = 3
            Retry = 4
            Ignore = 5
            Yes = 6
            No = 7
            Close = 8
        End Enum

        Public Enum TaskDialogElements
            Content
            ExpandedInformation
            Footer
            MainInstruction
        End Enum

        Public Enum TaskDialogIconElement
            Main
            Footer
        End Enum

        ' Task Dialog - flags
        <Flags>
        Public Enum TaskDialogOptions
            None = 0
            EnableHyperlinks = &H1
            UseMainIcon = &H2
            UseFooterIcon = &H4
            AllowCancel = &H8
            UseCommandLinks = &H10
            UseNoIconCommandLinks = &H20
            ExpandFooterArea = &H40
            ExpandedByDefault = &H80
            CheckVerificationFlag = &H100
            ShowProgressBar = &H200
            ShowMarqueeProgressBar = &H400
            UseCallbackTimer = &H800
            PositionRelativeToWindow = &H1000
            RightToLeftLayout = &H2000
            NoDefaultRadioButton = &H4000
        End Enum

        Public Enum TaskDialogMessages
            NavigatePage = CoreNativeMethods.UserMessage + 101
            ClickButton = CoreNativeMethods.UserMessage + 102
            ' wParam = Button ID
            SetMarqueeProgressBar = CoreNativeMethods.UserMessage + 103
            ' wParam = 0 (nonMarque) wParam != 0 (Marquee)
            SetProgressBarState = CoreNativeMethods.UserMessage + 104
            ' wParam = new progress state
            SetProgressBarRange = CoreNativeMethods.UserMessage + 105
            ' lParam = MAKELPARAM(nMinRange, nMaxRange)
            SetProgressBarPosition = CoreNativeMethods.UserMessage + 106
            ' wParam = new position
            SetProgressBarMarquee = CoreNativeMethods.UserMessage + 107
            ' wParam = 0 (stop marquee), wParam != 0 (start marquee), lparam = speed (milliseconds between repaints)
            SetElementText = CoreNativeMethods.UserMessage + 108
            ' wParam = element (TASKDIALOG_ELEMENTS), lParam = new element text (LPCWSTR)
            ClickRadioButton = CoreNativeMethods.UserMessage + 110
            ' wParam = Radio Button ID
            EnableButton = CoreNativeMethods.UserMessage + 111
            ' lParam = 0 (disable), lParam != 0 (enable), wParam = Button ID
            EnableRadioButton = CoreNativeMethods.UserMessage + 112
            ' lParam = 0 (disable), lParam != 0 (enable), wParam = Radio Button ID
            ClickVerification = CoreNativeMethods.UserMessage + 113
            ' wParam = 0 (unchecked), 1 (checked), lParam = 1 (set key focus)
            UpdateElementText = CoreNativeMethods.UserMessage + 114
            ' wParam = element (TASKDIALOG_ELEMENTS), lParam = new element text (LPCWSTR)
            SetButtonElevationRequiredState = CoreNativeMethods.UserMessage + 115
            ' wParam = Button ID, lParam = 0 (elevation not required), lParam != 0 (elevation required)
            UpdateIcon = CoreNativeMethods.UserMessage + 116
            ' wParam = icon element (TASKDIALOG_ICON_ELEMENTS), lParam = new icon (hIcon if TDF_USE_HICON_* was set, PCWSTR otherwise)
        End Enum

        Public Enum TaskDialogNotifications
            Created = 0
            Navigated = 1
            ButtonClicked = 2
            ' wParam = Button ID
            HyperlinkClicked = 3
            ' lParam = (LPCWSTR)pszHREF
            Timer = 4
            ' wParam = Milliseconds since dialog created or timer reset
            Destroyed = 5
            RadioButtonClicked = 6
            ' wParam = Radio Button ID
            Constructed = 7
            VerificationClicked = 8
            ' wParam = 1 if checkbox checked, 0 if not, lParam is unused and always 0
            Help = 9
            ExpandButtonClicked = 10
            ' wParam = 0 (dialog is now collapsed), wParam != 0 (dialog is now expanded)
        End Enum

        ' Used in the various SET_DEFAULT* TaskDialog messages
        Public Const NoDefaultButtonSpecified As Integer = 0

        ' Task Dialog config and related structs (for TaskDialogIndirect())
        Public Delegate Function TaskDialogCallback(hwnd As IntPtr, message As UInteger, wparam As IntPtr, lparam As IntPtr, referenceData As IntPtr) As Integer

        Public Enum ProgressBarState
            Normal = &H1
            [Error] = &H2
            Paused = &H3
        End Enum

        Public Enum TaskDialogIcons
            Warning = 65535
            [Error] = 65534
            Information = 65533
            Shield = 65532
        End Enum

#End Region
    End Module
End Namespace
