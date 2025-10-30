'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.ComponentModel
Imports System.Runtime.InteropServices
Imports Microsoft.Windows.Internal

Namespace Dialogs

    ''' <summary>
    ''' Encapsulates the native logic required to create, 
    ''' configure, and show a TaskDialog, 
    ''' via the TaskDialogIndirect() Win32 function.
    ''' </summary>
    ''' <remarks>A new instance of this class should 
    ''' be created for each messagebox show, as
    ''' the HWNDs for TaskDialogs do not remain constant 
    ''' across calls to TaskDialogIndirect.
    ''' </remarks>
    Public Class NativeTaskDialog
        Implements IDisposable

        Private nativeDialogConfig As TaskDialogNativeMethods.TaskDialogConfiguration
        Private settings As NativeTaskDialogSettings
        Private hWndDialog As IntPtr
        Private outerDialog As TaskDialog

        Private updatedStrings As IntPtr() = New IntPtr([Enum].GetNames(GetType(TaskDialogNativeMethods.TaskDialogElements)).Length - 1) {}
        Private buttonArray As IntPtr, radioButtonArray As IntPtr

        ' Flag tracks whether our first radio 
        ' button click event has come through.
        Private firstRadioButtonClicked As Boolean = True

#Region "Constructors"

        ' Configuration is applied at dialog creation time.
        Public Sub New(settings As NativeTaskDialogSettings, outerDialog As TaskDialog)
            nativeDialogConfig = settings.NativeConfiguration
            Me.settings = settings

            ' Wireup dialog proc message loop for this instance.
            nativeDialogConfig.callback = New TaskDialogNativeMethods.TaskDialogCallback(AddressOf DialogProc)

            ShowState = DialogShowState.PreShow

            ' Keep a reference to the outer shell, so we can notify.
            Me.outerDialog = outerDialog
        End Sub

#End Region

#Region "Public Properties"

        Public Property ShowState() As DialogShowState
            Get
                Return m_ShowState
            End Get
            Private Set
                m_ShowState = Value
            End Set
        End Property
        Private m_ShowState As DialogShowState

        Public Property SelectedButtonId() As Integer
            Get
                Return m_SelectedButtonId
            End Get
            Private Set
                m_SelectedButtonId = Value
            End Set
        End Property
        Private m_SelectedButtonId As Integer

        Public Property SelectedRadioButtonId() As Integer
            Get
                Return m_SelectedRadioButtonId
            End Get
            Private Set
                m_SelectedRadioButtonId = Value
            End Set
        End Property
        Private m_SelectedRadioButtonId As Integer

        Public Property CheckBoxChecked() As Boolean
            Get
                Return m_CheckBoxChecked
            End Get
            Private Set
                m_CheckBoxChecked = Value
            End Set
        End Property
        Private m_CheckBoxChecked As Boolean

#End Region

        Friend Sub NativeShow()
            ' Applies config struct and other settings, then
            ' calls main Win32 function.
            If settings Is Nothing Then
                Throw New InvalidOperationException(LocalizedMessages.NativeTaskDialogConfigurationError)
            End If

            ' Do a last-minute parse of the various dialog control lists,  
            ' and only allocate the memory at the last minute.

            MarshalDialogControlStructs()

            ' Make the call and show the dialog.
            ' NOTE: this call is BLOCKING, though the thread 
            ' WILL re-enter via the DialogProc.
            Try
                ShowState = DialogShowState.Showing

                Dim selectedButtonId__1 As Integer
                Dim selectedRadioButtonId__2 As Integer
                Dim checkBoxChecked__3 As Boolean

                ' Here is the way we use "vanilla" P/Invoke to call TaskDialogIndirect().  
                Dim hresult__4 As HResult = TaskDialogNativeMethods.TaskDialogIndirect(nativeDialogConfig, selectedButtonId__1, selectedRadioButtonId__2, checkBoxChecked__3)

                If CoreErrorHelper.Failed(hresult__4) Then
                    Dim msg As String
                    Select Case hresult__4
                        Case HResult.InvalidArguments
                            msg = LocalizedMessages.NativeTaskDialogInternalErrorArgs
                            Exit Select
                        Case HResult.OutOfMemory
                            msg = LocalizedMessages.NativeTaskDialogInternalErrorComplex
                            Exit Select
                        Case Else
                            msg = String.Format(System.Globalization.CultureInfo.InvariantCulture, LocalizedMessages.NativeTaskDialogInternalErrorUnexpected, hresult__4)
                            Exit Select
                    End Select
                    Dim e As Exception = Marshal.GetExceptionForHR(CInt(hresult__4))
                    Throw New Win32Exception(msg, e)
                End If

                SelectedButtonId = selectedButtonId__1
                SelectedRadioButtonId = selectedRadioButtonId__2
                CheckBoxChecked = checkBoxChecked__3
            Catch exc As EntryPointNotFoundException
                Throw New NotSupportedException(LocalizedMessages.NativeTaskDialogVersionError, exc)
            Finally
                ShowState = DialogShowState.Closed
            End Try
        End Sub

        ' The new task dialog does not support the existing 
        ' Win32 functions for closing (e.g. EndDialog()); instead,
        ' a "click button" message is sent. In this case, we're 
        ' abstracting out to say that the TaskDialog consumer can
        ' simply call "Close" and we'll "click" the cancel button. 
        ' Note that the cancel button doesn't actually
        ' have to exist for this to work.
        Friend Sub NativeClose(result As TaskDialogResult)
            ShowState = DialogShowState.Closing

            Dim id As Integer
            Select Case result
                Case TaskDialogResult.Close
                    id = CInt(TaskDialogNativeMethods.TaskDialogCommonButtonReturnIds.Close)
                    Exit Select
                Case TaskDialogResult.CustomButtonClicked
                    id = DialogsDefaults.MinimumDialogControlId
                    ' custom buttons
                    Exit Select
                Case TaskDialogResult.No
                    id = CInt(TaskDialogNativeMethods.TaskDialogCommonButtonReturnIds.No)
                    Exit Select
                Case TaskDialogResult.Ok
                    id = CInt(TaskDialogNativeMethods.TaskDialogCommonButtonReturnIds.Ok)
                    Exit Select
                Case TaskDialogResult.Retry
                    id = CInt(TaskDialogNativeMethods.TaskDialogCommonButtonReturnIds.Retry)
                    Exit Select
                Case TaskDialogResult.Yes
                    id = CInt(TaskDialogNativeMethods.TaskDialogCommonButtonReturnIds.Yes)
                    Exit Select
                Case Else
                    id = CInt(TaskDialogNativeMethods.TaskDialogCommonButtonReturnIds.Cancel)
                    Exit Select
            End Select

            SendMessageHelper(TaskDialogNativeMethods.TaskDialogMessages.ClickButton, id, 0)
        End Sub

#Region "Main Dialog Proc"

        Private Function DialogProc(windowHandle As IntPtr, message As UInteger, wparam As IntPtr, lparam As IntPtr, referenceData As IntPtr) As Integer
            ' Fetch the HWND - it may be the first time we're getting it.
            hWndDialog = windowHandle

            ' Big switch on the various notifications the 
            ' dialog proc can get.
            Select Case CType(message, TaskDialogNativeMethods.TaskDialogNotifications)
                Case TaskDialogNativeMethods.TaskDialogNotifications.Created
                    Dim result As Integer = PerformDialogInitialization()
                    outerDialog.RaiseOpenedEvent()
                    Return result
                Case TaskDialogNativeMethods.TaskDialogNotifications.ButtonClicked
                    Return HandleButtonClick(CInt(wparam))
                Case TaskDialogNativeMethods.TaskDialogNotifications.RadioButtonClicked
                    Return HandleRadioButtonClick(CInt(wparam))
                Case TaskDialogNativeMethods.TaskDialogNotifications.HyperlinkClicked
                    Return HandleHyperlinkClick(lparam)
                Case TaskDialogNativeMethods.TaskDialogNotifications.Help
                    Return HandleHelpInvocation()
                Case TaskDialogNativeMethods.TaskDialogNotifications.Timer
                    Return HandleTick(CInt(wparam))
                Case TaskDialogNativeMethods.TaskDialogNotifications.Destroyed
                    Return PerformDialogCleanup()
                Case Else
                    Exit Select
            End Select
            Return CInt(HResult.Ok)
        End Function

        ' Once the task dialog HWND is open, we need to send 
        ' additional messages to configure it.
        Private Function PerformDialogInitialization() As Integer
            ' Initialize Progress or Marquee Bar.
            If IsOptionSet(TaskDialogNativeMethods.TaskDialogOptions.ShowProgressBar) Then
                UpdateProgressBarRange()

                ' The order of the following is important - 
                ' state is more important than value, 
                ' and non-normal states turn off the bar value change 
                ' animation, which is likely the intended
                ' and preferable behavior.
                UpdateProgressBarState(settings.ProgressBarState)
                UpdateProgressBarValue(settings.ProgressBarValue)

                ' Due to a bug that wasn't fixed in time for RTM of Vista,
                ' second SendMessage is required if the state is non-Normal.
                UpdateProgressBarValue(settings.ProgressBarValue)
            ElseIf IsOptionSet(TaskDialogNativeMethods.TaskDialogOptions.ShowMarqueeProgressBar) Then
                ' TDM_SET_PROGRESS_BAR_MARQUEE is necessary 
                ' to cause the marquee to start animating.
                ' Note that this internal task dialog setting is 
                ' round-tripped when the marquee is
                ' is set to different states, so it never has to 
                ' be touched/sent again.
                SendMessageHelper(TaskDialogNativeMethods.TaskDialogMessages.SetProgressBarMarquee, 1, 0)
                UpdateProgressBarState(settings.ProgressBarState)
            End If

            If settings.ElevatedButtons IsNot Nothing AndAlso settings.ElevatedButtons.Count > 0 Then
                For Each id As Integer In settings.ElevatedButtons
                    UpdateElevationIcon(id, True)
                Next
            End If

            Return CoreErrorHelper.Ignored
        End Function

        Private Function HandleButtonClick(id As Integer) As Integer
            ' First we raise a Click event, if there is a custom button
            ' However, we implement Close() by sending a cancel button, so 
            ' we don't want to raise a click event in response to that.
            If ShowState <> DialogShowState.Closing Then
                outerDialog.RaiseButtonClickEvent(id)
            End If

            ' Once that returns, we raise a Closing event for the dialog
            ' The Win32 API handles button clicking-and-closing 
            ' as an atomic action,
            ' but it is more .NET friendly to split them up.
            ' Unfortunately, we do NOT have the return values at this stage.
            If id < DialogsDefaults.MinimumDialogControlId Then
                Return outerDialog.RaiseClosingEvent(id)
            End If

            Return CInt(HResult.[False])
        End Function

        Private Function HandleRadioButtonClick(id As Integer) As Integer
            ' When the dialog sets the radio button to default, 
            ' it (somewhat confusingly)issues a radio button clicked event
            '  - we mask that out - though ONLY if
            ' we do have a default radio button
            If firstRadioButtonClicked AndAlso Not IsOptionSet(TaskDialogNativeMethods.TaskDialogOptions.NoDefaultRadioButton) Then
                firstRadioButtonClicked = False
            Else
                outerDialog.RaiseButtonClickEvent(id)
            End If

            ' Note: we don't raise Closing, as radio 
            ' buttons are non-committing buttons
            Return CoreErrorHelper.Ignored
        End Function

        Private Function HandleHyperlinkClick(href As IntPtr) As Integer
            Dim link As String = Marshal.PtrToStringUni(href)
            outerDialog.RaiseHyperlinkClickEvent(link)

            Return CoreErrorHelper.Ignored
        End Function


        Private Function HandleTick(ticks As Integer) As Integer
            outerDialog.RaiseTickEvent(ticks)
            Return CoreErrorHelper.Ignored
        End Function

        Private Function HandleHelpInvocation() As Integer
            outerDialog.RaiseHelpInvokedEvent()
            Return CoreErrorHelper.Ignored
        End Function

        ' There should be little we need to do here, 
        ' as the use of the NativeTaskDialog is
        ' that it is instantiated for a single show, then disposed of.
        Private Function PerformDialogCleanup() As Integer
            firstRadioButtonClicked = True

            Return CoreErrorHelper.Ignored
        End Function

#End Region

#Region "Update members"

        Friend Sub UpdateProgressBarValue(i As Integer)
            AssertCurrentlyShowing()
            SendMessageHelper(TaskDialogNativeMethods.TaskDialogMessages.SetProgressBarPosition, i, 0)
        End Sub

        Friend Sub UpdateProgressBarRange()
            AssertCurrentlyShowing()

            ' Build range LPARAM - note it is in REVERSE intuitive order.
            Dim range As Long = NativeTaskDialog.MakeLongLParam(settings.ProgressBarMaximum, settings.ProgressBarMinimum)

            SendMessageHelper(TaskDialogNativeMethods.TaskDialogMessages.SetProgressBarRange, 0, range)
        End Sub

        Friend Sub UpdateProgressBarState(state As TaskDialogProgressBarState)
            AssertCurrentlyShowing()
            SendMessageHelper(TaskDialogNativeMethods.TaskDialogMessages.SetProgressBarState, CInt(state), 0)
        End Sub

        Friend Sub UpdateText(text As String)
            UpdateTextCore(text, TaskDialogNativeMethods.TaskDialogElements.Content)
        End Sub

        Friend Sub UpdateInstruction(instruction As String)
            UpdateTextCore(instruction, TaskDialogNativeMethods.TaskDialogElements.MainInstruction)
        End Sub

        Friend Sub UpdateFooterText(footerText As String)
            UpdateTextCore(footerText, TaskDialogNativeMethods.TaskDialogElements.Footer)
        End Sub

        Friend Sub UpdateExpandedText(expandedText As String)
            UpdateTextCore(expandedText, TaskDialogNativeMethods.TaskDialogElements.ExpandedInformation)
        End Sub

        Private Sub UpdateTextCore(s As String, element As TaskDialogNativeMethods.TaskDialogElements)
            AssertCurrentlyShowing()

            FreeOldString(element)
            SendMessageHelper(TaskDialogNativeMethods.TaskDialogMessages.SetElementText, CInt(element), CLng(MakeNewString(s, element)))
        End Sub

        Friend Sub UpdateMainIcon(mainIcon As TaskDialogStandardIcon)
            UpdateIconCore(mainIcon, TaskDialogNativeMethods.TaskDialogIconElement.Main)
        End Sub

        Friend Sub UpdateFooterIcon(footerIcon As TaskDialogStandardIcon)
            UpdateIconCore(footerIcon, TaskDialogNativeMethods.TaskDialogIconElement.Footer)
        End Sub

        Private Sub UpdateIconCore(icon As TaskDialogStandardIcon, element As TaskDialogNativeMethods.TaskDialogIconElement)
            AssertCurrentlyShowing()
            SendMessageHelper(TaskDialogNativeMethods.TaskDialogMessages.UpdateIcon, CInt(element), CLng(icon))
        End Sub

        Friend Sub UpdateCheckBoxChecked(cbc As Boolean)
            AssertCurrentlyShowing()
            SendMessageHelper(TaskDialogNativeMethods.TaskDialogMessages.ClickVerification, (If(cbc, 1, 0)), 1)
        End Sub

        Friend Sub UpdateElevationIcon(buttonId As Integer, showIcon As Boolean)
            AssertCurrentlyShowing()
            SendMessageHelper(TaskDialogNativeMethods.TaskDialogMessages.SetButtonElevationRequiredState, buttonId, Convert.ToInt32(showIcon))
        End Sub

        Friend Sub UpdateButtonEnabled(buttonID As Integer, enabled As Boolean)
            AssertCurrentlyShowing()
            SendMessageHelper(TaskDialogNativeMethods.TaskDialogMessages.EnableButton, buttonID, If(enabled = True, 1, 0))
        End Sub

        Friend Sub UpdateRadioButtonEnabled(buttonID As Integer, enabled As Boolean)
            AssertCurrentlyShowing()
            SendMessageHelper(TaskDialogNativeMethods.TaskDialogMessages.EnableRadioButton, buttonID, If(enabled = True, 1, 0))
        End Sub

        Friend Sub AssertCurrentlyShowing()
            System.Diagnostics.Debug.Assert(ShowState = DialogShowState.Showing, "Update*() methods should only be called while native dialog is showing")
        End Sub

#End Region

#Region "Helpers"

        Private Function SendMessageHelper(message As TaskDialogNativeMethods.TaskDialogMessages, wparam As Integer, lparam As Long) As Integer
            ' Be sure to at least assert here - 
            ' messages to invalid handles often just disappear silently
            System.Diagnostics.Debug.Assert(hWndDialog <> Nothing, "HWND for dialog is null during SendMessage")

            Return CInt(CoreNativeMethods.SendMessage(hWndDialog, CUInt(message), CType(wparam, IntPtr), New IntPtr(lparam)))
        End Function

        Private Function IsOptionSet(flag As TaskDialogNativeMethods.TaskDialogOptions) As Boolean
            Return ((nativeDialogConfig.taskDialogFlags And flag) = flag)
        End Function

        ' Allocates a new string on the unmanaged heap, 
        ' and stores the pointer so we can free it later.

        Private Function MakeNewString(text As String, element As TaskDialogNativeMethods.TaskDialogElements) As IntPtr
            Dim newStringPtr As IntPtr = Marshal.StringToHGlobalUni(text)
            updatedStrings(CInt(element)) = newStringPtr
            Return newStringPtr
        End Function

        ' Checks to see if the given element already has an 
        ' updated string, and if so, 
        ' frees it. This is done in preparation for a call to 
        ' MakeNewString(), to prevent
        ' leaks from multiple updates calls on the same element 
        ' within a single native dialog lifetime.
        Private Sub FreeOldString(element As TaskDialogNativeMethods.TaskDialogElements)
            Dim elementIndex As Integer = CInt(element)
            If updatedStrings(elementIndex) <> IntPtr.Zero Then
                Marshal.FreeHGlobal(updatedStrings(elementIndex))
                updatedStrings(elementIndex) = IntPtr.Zero
            End If
        End Sub

        ' Based on the following defines in WinDef.h and WinUser.h:
        ' #define MAKELPARAM(l, h) ((LPARAM)(DWORD)MAKELONG(l, h))
        ' #define MAKELONG(a, b) ((LONG)(((WORD)(((DWORD_PTR)(a)) & 0xffff)) | ((DWORD)((WORD)(((DWORD_PTR)(b)) & 0xffff))) << 16))
        Private Shared Function MakeLongLParam(a As Integer, b As Integer) As Long
            Return (a << 16) + b
        End Function

        ' Builds the actual configuration that the 
        ' NativeTaskDialog (and underlying Win32 API)
        ' expects, by parsing the various control lists, 
        ' marshaling to the unmanaged heap, etc.
        Private Sub MarshalDialogControlStructs()
            If settings.Buttons IsNot Nothing AndAlso settings.Buttons.Length > 0 Then
                buttonArray = AllocateAndMarshalButtons(settings.Buttons)
                settings.NativeConfiguration.buttons = buttonArray
                settings.NativeConfiguration.buttonCount = CUInt(settings.Buttons.Length)
            End If

            If settings.RadioButtons IsNot Nothing AndAlso settings.RadioButtons.Length > 0 Then
                radioButtonArray = AllocateAndMarshalButtons(settings.RadioButtons)
                settings.NativeConfiguration.radioButtons = radioButtonArray
                settings.NativeConfiguration.radioButtonCount = CUInt(settings.RadioButtons.Length)
            End If
        End Sub

        Private Shared Function AllocateAndMarshalButtons(structs As TaskDialogNativeMethods.TaskDialogButton()) As IntPtr
            Dim initialPtr As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(GetType(TaskDialogNativeMethods.TaskDialogButton)) * structs.Length)

            Dim currentPtr As IntPtr = initialPtr
            For Each button As TaskDialogNativeMethods.TaskDialogButton In structs
                Marshal.StructureToPtr(button, currentPtr, False)
                currentPtr = CType(CInt(currentPtr) + Marshal.SizeOf(button), IntPtr)
            Next

            Return initialPtr
        End Function

#End Region

#Region "IDispose Pattern"

        Private disposed As Boolean

        ' Finalizer and IDisposable implementation.
        Public Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub

        Protected Overrides Sub Finalize()
            Try
                Dispose(False)
            Finally
                MyBase.Finalize()
            End Try
        End Sub

        ' Core disposing logic.
        Protected Sub Dispose(disposing As Boolean)
            If Not disposed Then
                disposed = True

                ' Single biggest resource - make sure the dialog 
                ' itself has been instructed to close.

                If ShowState = DialogShowState.Showing Then
                    NativeClose(TaskDialogResult.Cancel)
                End If

                ' Clean up custom allocated strings that were updated
                ' while the dialog was showing. Note that the strings
                ' passed in the initial TaskDialogIndirect call will
                ' be cleaned up automagically by the default 
                ' marshalling logic.

                If updatedStrings IsNot Nothing Then
                    For i As Integer = 0 To updatedStrings.Length - 1
                        If updatedStrings(i) <> IntPtr.Zero Then
                            Marshal.FreeHGlobal(updatedStrings(i))
                            updatedStrings(i) = IntPtr.Zero
                        End If
                    Next
                End If

                ' Clean up the button and radio button arrays, if any.
                If buttonArray <> IntPtr.Zero Then
                    Marshal.FreeHGlobal(buttonArray)
                    buttonArray = IntPtr.Zero
                End If
                If radioButtonArray <> IntPtr.Zero Then
                    Marshal.FreeHGlobal(radioButtonArray)
                    radioButtonArray = IntPtr.Zero
                End If

                ' Clean up managed resources - currently there are none
                ' that are interesting.
                If disposing Then
                End If
            End If
        End Sub

#End Region
    End Class
End Namespace
