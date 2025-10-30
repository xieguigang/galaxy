'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Collections.Generic
Imports Microsoft.Windows.Internal

Namespace Dialogs
    ''' <summary>
    ''' Encapsulates a new-to-Vista Win32 TaskDialog window 
    ''' - a powerful successor to the MessageBox available
    ''' in previous versions of Windows.
    ''' </summary>
    Public NotInheritable Class TaskDialog
        Implements IDialogControlHost
        Implements IDisposable
        ' Global instance of TaskDialog, to be used by static Show() method.
        ' As most parameters of a dialog created via static Show() will have
        ' identical parameters, we'll create one TaskDialog and treat it
        ' as a NativeTaskDialog generator for all static Show() calls.
        Private Shared staticDialog As TaskDialog

        ' Main current native dialog.
        Private nativeDialog As NativeTaskDialog

        Private buttons As New List(Of TaskDialogButtonBase)()
        Private radioButtons As New List(Of TaskDialogButtonBase)()
        Private commandLinks As New List(Of TaskDialogButtonBase)()
        Private ownerWindow As IntPtr

#Region "Public Properties"

        Dim _Tick As EventHandler(Of TaskDialogTickEventArgs)

        ''' <summary>
        ''' Occurs when a progress bar changes.
        ''' </summary>
        Public Custom Event Tick As EventHandler(Of TaskDialogTickEventArgs)
            AddHandler(value As EventHandler(Of TaskDialogTickEventArgs))
                _Tick = value
            End AddHandler
            RemoveHandler(value As EventHandler(Of TaskDialogTickEventArgs))
                _Tick = Nothing
            End RemoveHandler
            RaiseEvent(sender As Object, e As TaskDialogTickEventArgs)
                _Tick.Invoke(sender, e)
            End RaiseEvent
        End Event

        ''' <summary>
        ''' Occurs when a user clicks a hyperlink.
        ''' </summary>
        Public Event HyperlinkClick As EventHandler(Of TaskDialogHyperlinkClickedEventArgs)

        Dim _Closing As EventHandler(Of TaskDialogClosingEventArgs)

        ''' <summary>
        ''' Occurs when the TaskDialog is closing.
        ''' </summary>
        Public Custom Event Closing As EventHandler(Of TaskDialogClosingEventArgs)
            AddHandler(value As EventHandler(Of TaskDialogClosingEventArgs))
                _Closing = value
            End AddHandler
            RemoveHandler(value As EventHandler(Of TaskDialogClosingEventArgs))
                _Closing = Nothing
            End RemoveHandler
            RaiseEvent(sender As Object, e As TaskDialogClosingEventArgs)
                Call _Closing.Invoke(sender, e)
            End RaiseEvent
        End Event

        Dim _HelpInvoked As EventHandler

        ''' <summary>
        ''' Occurs when a user clicks on Help.
        ''' </summary>
        Public Custom Event HelpInvoked As EventHandler
            AddHandler(value As EventHandler)
                _HelpInvoked = value
            End AddHandler
            RemoveHandler(value As EventHandler)
                _HelpInvoked = Nothing
            End RemoveHandler
            RaiseEvent(sender As Object, e As EventArgs)
                _HelpInvoked.Invoke(sender, e)
            End RaiseEvent
        End Event

        ''' <summary>
        ''' Occurs when the TaskDialog is opened.
        ''' </summary>
        Public Event Opened As EventHandler

        ''' <summary>
        ''' Gets or sets a value that contains the owner window's handle.
        ''' </summary>
        Public Property OwnerWindowHandle() As IntPtr
            Get
                Return ownerWindow
            End Get
            Set
                ThrowIfDialogShowing("LocalizedMessages.OwnerCannotBeChanged")
                ownerWindow = Value
            End Set
        End Property

        ' Main content (maps to MessageBox's "message"). 
        Private m_text As String
        ''' <summary>
        ''' Gets or sets a value that contains the message text.
        ''' </summary>
        Public Property Text() As String
            Get
                Return m_text
            End Get
            Set
                ' Set local value, then update native dialog if showing.
                m_text = Value
                If NativeDialogShowing Then
                    nativeDialog.UpdateText(m_text)
                End If
            End Set
        End Property

        Private m_instructionText As String
        ''' <summary>
        ''' Gets or sets a value that contains the instruction text.
        ''' </summary>
        Public Property InstructionText() As String
            Get
                Return m_instructionText
            End Get
            Set
                ' Set local value, then update native dialog if showing.
                m_instructionText = Value
                If NativeDialogShowing Then
                    nativeDialog.UpdateInstruction(m_instructionText)
                End If
            End Set
        End Property

        Private m_caption As String
        ''' <summary>
        ''' Gets or sets a value that contains the caption text.
        ''' </summary>
        Public Property Caption() As String
            Get
                Return m_caption
            End Get
            Set
                ThrowIfDialogShowing("LocalizedMessages.CaptionCannotBeChanged")
                m_caption = Value
            End Set
        End Property

        Private m_footerText As String
        ''' <summary>
        ''' Gets or sets a value that contains the footer text.
        ''' </summary>
        Public Property FooterText() As String
            Get
                Return m_footerText
            End Get
            Set
                ' Set local value, then update native dialog if showing.
                m_footerText = Value
                If NativeDialogShowing Then
                    nativeDialog.UpdateFooterText(m_footerText)
                End If
            End Set
        End Property

        Private checkBoxText As String
        ''' <summary>
        ''' Gets or sets a value that contains the footer check box text.
        ''' </summary>
        Public Property FooterCheckBoxText() As String
            Get
                Return checkBoxText
            End Get
            Set
                ThrowIfDialogShowing(LocalizedMessages.CheckBoxCannotBeChanged)
                checkBoxText = Value
            End Set
        End Property

        Private m_detailsExpandedText As String
        ''' <summary>
        ''' Gets or sets a value that contains the expanded text in the details section.
        ''' </summary>
        Public Property DetailsExpandedText() As String
            Get
                Return m_detailsExpandedText
            End Get
            Set
                ' Set local value, then update native dialog if showing.
                m_detailsExpandedText = Value
                If NativeDialogShowing Then
                    nativeDialog.UpdateExpandedText(m_detailsExpandedText)
                End If
            End Set
        End Property

        Private m_detailsExpanded As Boolean
        ''' <summary>
        ''' Gets or sets a value that determines if the details section is expanded.
        ''' </summary>
        Public Property DetailsExpanded() As Boolean
            Get
                Return m_detailsExpanded
            End Get
            Set
                ThrowIfDialogShowing(LocalizedMessages.ExpandingStateCannotBeChanged)
                m_detailsExpanded = Value
            End Set
        End Property

        Private m_detailsExpandedLabel As String
        ''' <summary>
        ''' Gets or sets a value that contains the expanded control text.
        ''' </summary>
        Public Property DetailsExpandedLabel() As String
            Get
                Return m_detailsExpandedLabel
            End Get
            Set
                ThrowIfDialogShowing(LocalizedMessages.ExpandedLabelCannotBeChanged)
                m_detailsExpandedLabel = Value
            End Set
        End Property

        Private m_detailsCollapsedLabel As String
        ''' <summary>
        ''' Gets or sets a value that contains the collapsed control text.
        ''' </summary>
        Public Property DetailsCollapsedLabel() As String
            Get
                Return m_detailsCollapsedLabel
            End Get
            Set
                ThrowIfDialogShowing(LocalizedMessages.CollapsedTextCannotBeChanged)
                m_detailsCollapsedLabel = Value
            End Set
        End Property

        Private m_cancelable As Boolean
        ''' <summary>
        ''' Gets or sets a value that determines if Cancelable is set.
        ''' </summary>
        Public Property Cancelable() As Boolean
            Get
                Return m_cancelable
            End Get
            Set
                ThrowIfDialogShowing("LocalizedMessages.CancelableCannotBeChanged")
                m_cancelable = Value
            End Set
        End Property

        Private m_icon As TaskDialogStandardIcon
        ''' <summary>
        ''' Gets or sets a value that contains the TaskDialog main icon.
        ''' </summary>
        Public Property Icon() As TaskDialogStandardIcon
            Get
                Return m_icon
            End Get
            Set
                ' Set local value, then update native dialog if showing.
                m_icon = Value
                If NativeDialogShowing Then
                    nativeDialog.UpdateMainIcon(m_icon)
                End If
            End Set
        End Property

        Private m_footerIcon As TaskDialogStandardIcon
        ''' <summary>
        ''' Gets or sets a value that contains the footer icon.
        ''' </summary>
        Public Property FooterIcon() As TaskDialogStandardIcon
            Get
                Return m_footerIcon
            End Get
            Set
                ' Set local value, then update native dialog if showing.
                m_footerIcon = Value
                If NativeDialogShowing Then
                    nativeDialog.UpdateFooterIcon(m_footerIcon)
                End If
            End Set
        End Property

        Private m_standardButtons As TaskDialogStandardButtons = TaskDialogStandardButtons.None
        ''' <summary>
        ''' Gets or sets a value that contains the standard buttons.
        ''' </summary>
        Public Property StandardButtons() As TaskDialogStandardButtons
            Get
                Return m_standardButtons
            End Get
            Set
                ThrowIfDialogShowing(LocalizedMessages.StandardButtonsCannotBeChanged)
                m_standardButtons = Value
            End Set
        End Property

        Private m_controls As DialogControlCollection(Of TaskDialogControl)
        ''' <summary>
        ''' Gets a value that contains the TaskDialog controls.
        ''' </summary>
        Public ReadOnly Property Controls() As DialogControlCollection(Of TaskDialogControl)
            ' "Show protection" provided by collection itself, 
            ' as well as individual controls.
            Get
                Return m_controls
            End Get
        End Property

        Private m_hyperlinksEnabled As Boolean
        ''' <summary>
        ''' Gets or sets a value that determines if hyperlinks are enabled.
        ''' </summary>
        Public Property HyperlinksEnabled() As Boolean
            Get
                Return m_hyperlinksEnabled
            End Get
            Set
                ThrowIfDialogShowing(LocalizedMessages.HyperlinksCannotBetSet)
                m_hyperlinksEnabled = Value
            End Set
        End Property

        Private m_footerCheckBoxChecked As System.Nullable(Of Boolean) = Nothing
        ''' <summary>
        ''' Gets or sets a value that indicates if the footer checkbox is checked.
        ''' </summary>
        Public Property FooterCheckBoxChecked() As System.Nullable(Of Boolean)
            Get
                Return m_footerCheckBoxChecked.GetValueOrDefault(False)
            End Get
            Set
                ' Set local value, then update native dialog if showing.
                m_footerCheckBoxChecked = Value
                If NativeDialogShowing Then
                    nativeDialog.UpdateCheckBoxChecked(m_footerCheckBoxChecked.Value)
                End If
            End Set
        End Property

        Private m_expansionMode As TaskDialogExpandedDetailsLocation
        ''' <summary>
        ''' Gets or sets a value that contains the expansion mode for this dialog.
        ''' </summary>
        Public Property ExpansionMode() As TaskDialogExpandedDetailsLocation
            Get
                Return m_expansionMode
            End Get
            Set
                ThrowIfDialogShowing(LocalizedMessages.ExpandedDetailsCannotBeChanged)
                m_expansionMode = Value
            End Set
        End Property

        Private m_startupLocation As TaskDialogStartupLocation
        ''' <summary>
        ''' Gets or sets a value that contains the startup location.
        ''' </summary>
        Public Property StartupLocation() As TaskDialogStartupLocation
            Get
                Return m_startupLocation
            End Get
            Set
                ThrowIfDialogShowing(LocalizedMessages.StartupLocationCannotBeChanged)
                m_startupLocation = Value
            End Set
        End Property

        Private m_progressBar As TaskDialogProgressBar
        ''' <summary>
        ''' Gets or sets the progress bar on the taskdialog. ProgressBar a visual representation 
        ''' of the progress of a long running operation.
        ''' </summary>
        Public Property ProgressBar() As TaskDialogProgressBar
            Get
                Return m_progressBar
            End Get
            Set
                ThrowIfDialogShowing(LocalizedMessages.ProgressBarCannotBeChanged)
                If Value IsNot Nothing Then
                    If Value.HostingDialog IsNot Nothing Then
                        Throw New InvalidOperationException(LocalizedMessages.ProgressBarCannotBeHostedInMultipleDialogs)
                    End If

                    Value.HostingDialog = Me
                End If
                m_progressBar = Value
            End Set
        End Property

#End Region

#Region "Constructors"

        ''' <summary>
        ''' Creates a basic TaskDialog window 
        ''' </summary>
        Public Sub New()
            CoreHelpers.ThrowIfNotVista()

            ' Initialize various data structs.
            m_controls = New DialogControlCollection(Of TaskDialogControl)(Me)
        End Sub

#End Region

#Region "Static Show Methods"

        ''' <summary>
        ''' Creates and shows a task dialog with the specified message text.
        ''' </summary>
        ''' <param name="text">The text to display.</param>
        ''' <returns>The dialog result.</returns>
        Public Shared Function Show(text As String) As TaskDialogResult
            Return ShowCoreStatic(text, TaskDialogDefaults.MainInstruction, TaskDialogDefaults.Caption)
        End Function

        ''' <summary>
        ''' Creates and shows a task dialog with the specified supporting text and main instruction.
        ''' </summary>
        ''' <param name="text">The supporting text to display.</param>
        ''' <param name="instructionText">The main instruction text to display.</param>
        ''' <returns>The dialog result.</returns>
        Public Shared Function Show(text As String, instructionText As String) As TaskDialogResult
            Return ShowCoreStatic(text, instructionText, TaskDialogDefaults.Caption)
        End Function

        ''' <summary>
        ''' Creates and shows a task dialog with the specified supporting text, main instruction, and dialog caption.
        ''' </summary>
        ''' <param name="text">The supporting text to display.</param>
        ''' <param name="instructionText">The main instruction text to display.</param>
        ''' <param name="caption">The caption for the dialog.</param>
        ''' <returns>The dialog result.</returns>
        Public Shared Function Show(text As String, instructionText As String, caption As String) As TaskDialogResult
            Return ShowCoreStatic(text, instructionText, caption)
        End Function
#End Region

#Region "Instance Show Methods"

        ''' <summary>
        ''' Creates and shows a task dialog.
        ''' </summary>
        ''' <returns>The dialog result.</returns>
        Public Function Show() As TaskDialogResult
            Return ShowCore()
        End Function
#End Region

#Region "Core Show Logic"

        ' CORE SHOW METHODS:
        ' All static Show() calls forward here - 
        ' it is responsible for retrieving
        ' or creating our cached TaskDialog instance, getting it configured,
        ' and in turn calling the appropriate instance Show.

        Private Shared Function ShowCoreStatic(text As String, instructionText As String, caption As String) As TaskDialogResult
            CoreHelpers.ThrowIfNotVista()

            ' If no instance cached yet, create it.
            If staticDialog Is Nothing Then
                ' New TaskDialog will automatically pick up defaults when 
                ' a new config structure is created as part of ShowCore().
                staticDialog = New TaskDialog()
            End If

            ' Set the few relevant properties, 
            ' and go with the defaults for the others.
            staticDialog.Text = text
            staticDialog.InstructionText = instructionText
            staticDialog.Caption = caption

            Return staticDialog.Show()
        End Function

        Private Function ShowCore() As TaskDialogResult
            Dim result As TaskDialogResult

            Try
                ' Populate control lists, based on current 
                ' contents - note we are somewhat late-bound 
                ' on our control lists, to support XAML scenarios.
                SortDialogControls()

                ' First, let's make sure it even makes 
                ' sense to try a show.
                ValidateCurrentDialogSettings()

                ' Create settings object for new dialog, 
                ' based on current state.
                Dim settings As New NativeTaskDialogSettings()
                ApplyCoreSettings(settings)
                ApplySupplementalSettings(settings)

                ' Show the dialog.
                ' NOTE: this is a BLOCKING call; the dialog proc callbacks
                ' will be executed by the same thread as the 
                ' Show() call before the thread of execution 
                ' contines to the end of this method.
                nativeDialog = New NativeTaskDialog(settings, Me)
                nativeDialog.NativeShow()

                ' Build and return dialog result to public API - leaving it
                ' null after an exception is thrown is fine in this case
                result = ConstructDialogResult(nativeDialog)
                m_footerCheckBoxChecked = nativeDialog.CheckBoxChecked
            Finally
                CleanUp()
                nativeDialog = Nothing
            End Try

            Return result
        End Function

        ' Helper that looks at the current state of the TaskDialog and verifies
        ' that there aren't any abberant combinations of properties.
        ' NOTE that this method is designed to throw 
        ' rather than return a bool.
        Private Sub ValidateCurrentDialogSettings()
            If m_footerCheckBoxChecked.HasValue AndAlso m_footerCheckBoxChecked.Value = True AndAlso String.IsNullOrEmpty(checkBoxText) Then
                Throw New InvalidOperationException(LocalizedMessages.TaskDialogCheckBoxTextRequiredToEnableCheckBox)
            End If

            ' Progress bar validation.
            ' Make sure the progress bar values are valid.
            ' the Win32 API will valiantly try to rationalize 
            ' bizarre min/max/value combinations, but we'll save
            ' it the trouble by validating.
            If m_progressBar IsNot Nothing AndAlso Not m_progressBar.HasValidValues Then
                Throw New InvalidOperationException(LocalizedMessages.TaskDialogProgressBarValueInRange)
            End If

            ' Validate Buttons collection.
            ' Make sure we don't have buttons AND 
            ' command-links - the Win32 API treats them as different
            ' flavors of a single button struct.
            If buttons.Count > 0 AndAlso commandLinks.Count > 0 Then
                Throw New NotSupportedException(LocalizedMessages.TaskDialogSupportedButtonsAndLinks)
            End If
            If buttons.Count > 0 AndAlso m_standardButtons <> TaskDialogStandardButtons.None Then
                Throw New NotSupportedException(LocalizedMessages.TaskDialogSupportedButtonsAndButtons)
            End If
        End Sub

        ' Analyzes the final state of the NativeTaskDialog instance and creates the 
        ' final TaskDialogResult that will be returned from the public API
        Private Shared Function ConstructDialogResult(native As NativeTaskDialog) As TaskDialogResult
            System.Diagnostics.Debug.Assert(native.ShowState = DialogShowState.Closed, "dialog result being constructed for unshown dialog.")

            Dim result As TaskDialogResult = TaskDialogResult.Cancel

            Dim standardButton As TaskDialogStandardButtons = MapButtonIdToStandardButton(native.SelectedButtonId)

            ' If returned ID isn't a standard button, let's fetch 
            If standardButton = TaskDialogStandardButtons.None Then
                result = TaskDialogResult.CustomButtonClicked
            Else
                result = CType(standardButton, TaskDialogResult)
            End If

            Return result
        End Function

        ''' <summary>
        ''' Close TaskDialog
        ''' </summary>
        ''' <exception cref="InvalidOperationException">if TaskDialog is not showing.</exception>
        Public Sub Close()
            If Not NativeDialogShowing Then
                Throw New InvalidOperationException(LocalizedMessages.TaskDialogCloseNonShowing)
            End If

            nativeDialog.NativeClose(TaskDialogResult.Cancel)
            ' TaskDialog's own cleanup code - 
            ' which runs post show - will handle disposal of native dialog.
        End Sub

        ''' <summary>
        ''' Close TaskDialog with a given TaskDialogResult
        ''' </summary>
        ''' <param name="closingResult">TaskDialogResult to return from the TaskDialog.Show() method</param>
        ''' <exception cref="InvalidOperationException">if TaskDialog is not showing.</exception>
        Public Sub Close(closingResult As TaskDialogResult)
            If Not NativeDialogShowing Then
                Throw New InvalidOperationException(LocalizedMessages.TaskDialogCloseNonShowing)
            End If

            nativeDialog.NativeClose(closingResult)
            ' TaskDialog's own cleanup code - 
            ' which runs post show - will handle disposal of native dialog.
        End Sub

#End Region

#Region "Configuration Construction"

        Private Sub ApplyCoreSettings(settings As NativeTaskDialogSettings)
            ApplyGeneralNativeConfiguration(settings.NativeConfiguration)
            ApplyTextConfiguration(settings.NativeConfiguration)
            ApplyOptionConfiguration(settings.NativeConfiguration)
            ApplyControlConfiguration(settings)
        End Sub

        Private Sub ApplyGeneralNativeConfiguration(dialogConfig As TaskDialogNativeMethods.TaskDialogConfiguration)
            ' If an owner wasn't specifically specified, 
            ' we'll use the app's main window.
            If ownerWindow <> IntPtr.Zero Then
                dialogConfig.parentHandle = ownerWindow
            End If

            ' Other miscellaneous sets.
            dialogConfig.mainIcon = New TaskDialogNativeMethods.IconUnion(CInt(m_icon))
            dialogConfig.footerIcon = New TaskDialogNativeMethods.IconUnion(CInt(m_footerIcon))
            dialogConfig.commonButtons = CType(m_standardButtons, TaskDialogNativeMethods.TaskDialogCommonButtons)
        End Sub

        ''' <summary>
        ''' Sets important text properties.
        ''' </summary>
        ''' <param name="dialogConfig">An instance of a <see cref="TaskDialogNativeMethods.TaskDialogConfiguration"/> object.</param>
        Private Sub ApplyTextConfiguration(dialogConfig As TaskDialogNativeMethods.TaskDialogConfiguration)
            ' note that nulls or empty strings are fine here.
            dialogConfig.content = m_text
            dialogConfig.windowTitle = m_caption
            dialogConfig.mainInstruction = m_instructionText
            dialogConfig.expandedInformation = m_detailsExpandedText
            dialogConfig.expandedControlText = m_detailsExpandedLabel
            dialogConfig.collapsedControlText = m_detailsCollapsedLabel
            dialogConfig.footerText = m_footerText
            dialogConfig.verificationText = checkBoxText
        End Sub

        Private Sub ApplyOptionConfiguration(dialogConfig As TaskDialogNativeMethods.TaskDialogConfiguration)
            ' Handle options - start with no options set.
            Dim options As TaskDialogNativeMethods.TaskDialogOptions = TaskDialogNativeMethods.TaskDialogOptions.None
            If m_cancelable Then
                options = options Or TaskDialogNativeMethods.TaskDialogOptions.AllowCancel
            End If
            If m_footerCheckBoxChecked.HasValue AndAlso m_footerCheckBoxChecked.Value Then
                options = options Or TaskDialogNativeMethods.TaskDialogOptions.CheckVerificationFlag
            End If
            If m_hyperlinksEnabled Then
                options = options Or TaskDialogNativeMethods.TaskDialogOptions.EnableHyperlinks
            End If
            If m_detailsExpanded Then
                options = options Or TaskDialogNativeMethods.TaskDialogOptions.ExpandedByDefault
            End If
            If _Tick IsNot Nothing Then
                options = options Or TaskDialogNativeMethods.TaskDialogOptions.UseCallbackTimer
            End If
            If m_startupLocation = TaskDialogStartupLocation.CenterOwner Then
                options = options Or TaskDialogNativeMethods.TaskDialogOptions.PositionRelativeToWindow
            End If

            ' Note: no validation required, as we allow this to 
            ' be set even if there is no expanded information 
            ' text because that could be added later.
            ' Default for Win32 API is to expand into (and after) 
            ' the content area.
            If m_expansionMode = TaskDialogExpandedDetailsLocation.ExpandFooter Then
                options = options Or TaskDialogNativeMethods.TaskDialogOptions.ExpandFooterArea
            End If

            ' Finally, apply options to config.
            dialogConfig.taskDialogFlags = options
        End Sub

        ' Builds the actual configuration 
        ' that the NativeTaskDialog (and underlying Win32 API)
        ' expects, by parsing the various control 
        ' lists, marshalling to the unmanaged heap, etc.

        Private Sub ApplyControlConfiguration(settings As NativeTaskDialogSettings)
            ' Deal with progress bars/marquees.
            If m_progressBar IsNot Nothing Then
                If m_progressBar.State = TaskDialogProgressBarState.Marquee Then
                    settings.NativeConfiguration.taskDialogFlags = settings.NativeConfiguration.taskDialogFlags Or TaskDialogNativeMethods.TaskDialogOptions.ShowMarqueeProgressBar
                Else
                    settings.NativeConfiguration.taskDialogFlags = settings.NativeConfiguration.taskDialogFlags Or TaskDialogNativeMethods.TaskDialogOptions.ShowProgressBar
                End If
            End If

            ' Build the native struct arrays that NativeTaskDialog 
            ' needs - though NTD will handle
            ' the heavy lifting marshalling to make sure 
            ' all the cleanup is centralized there.
            If buttons.Count > 0 OrElse commandLinks.Count > 0 Then
                ' These are the actual arrays/lists of 
                ' the structs that we'll copy to the 
                ' unmanaged heap.
                Dim sourceList As List(Of TaskDialogButtonBase) = (If(buttons.Count > 0, buttons, commandLinks))
                settings.Buttons = BuildButtonStructArray(sourceList)

                ' Apply option flag that forces all 
                ' custom buttons to render as command links.
                If commandLinks.Count > 0 Then
                    settings.NativeConfiguration.taskDialogFlags = settings.NativeConfiguration.taskDialogFlags Or TaskDialogNativeMethods.TaskDialogOptions.UseCommandLinks
                End If

                ' Set default button and add elevation icons 
                ' to appropriate buttons.
                settings.NativeConfiguration.defaultButtonIndex = FindDefaultButtonId(sourceList)

                ApplyElevatedIcons(settings, sourceList)
            End If

            If radioButtons.Count > 0 Then
                settings.RadioButtons = BuildButtonStructArray(radioButtons)

                ' Set default radio button - radio buttons don't support.
                Dim defaultRadioButton As Integer = FindDefaultButtonId(radioButtons)
                settings.NativeConfiguration.defaultRadioButtonIndex = defaultRadioButton

                If defaultRadioButton = TaskDialogNativeMethods.NoDefaultButtonSpecified Then
                    settings.NativeConfiguration.taskDialogFlags = settings.NativeConfiguration.taskDialogFlags Or TaskDialogNativeMethods.TaskDialogOptions.NoDefaultRadioButton
                End If
            End If
        End Sub

        Private Shared Function BuildButtonStructArray(controls As List(Of TaskDialogButtonBase)) As TaskDialogNativeMethods.TaskDialogButton()
            Dim buttonStructs As TaskDialogNativeMethods.TaskDialogButton()
            Dim button As TaskDialogButtonBase

            Dim totalButtons As Integer = controls.Count
            buttonStructs = New TaskDialogNativeMethods.TaskDialogButton(totalButtons - 1) {}
            For i As Integer = 0 To totalButtons - 1
                button = controls(i)
                buttonStructs(i) = New TaskDialogNativeMethods.TaskDialogButton(button.Id, button.ToString())
            Next
            Return buttonStructs
        End Function

        ' Searches list of controls and returns the ID of 
        ' the default control, or 0 if no default was specified.
        Private Shared Function FindDefaultButtonId(controls As List(Of TaskDialogButtonBase)) As Integer
            Dim defaults As List(Of TaskDialogButtonBase) = controls.FindAll(Function(control) control.[Default])

            If defaults.Count = 1 Then
                Return defaults(0).Id
            ElseIf defaults.Count > 1 Then
                Throw New InvalidOperationException(LocalizedMessages.TaskDialogOnlyOneDefaultControl)
            End If

            Return TaskDialogNativeMethods.NoDefaultButtonSpecified
        End Function

        Private Shared Sub ApplyElevatedIcons(settings As NativeTaskDialogSettings, controls As List(Of TaskDialogButtonBase))
            For Each control As TaskDialogButton In controls
                If control.UseElevationIcon Then
                    If settings.ElevatedButtons Is Nothing Then
                        settings.ElevatedButtons = New List(Of Integer)()
                    End If
                    settings.ElevatedButtons.Add(control.Id)
                End If
            Next
        End Sub

        Private Sub ApplySupplementalSettings(settings As NativeTaskDialogSettings)
            If m_progressBar IsNot Nothing Then
                If m_progressBar.State <> TaskDialogProgressBarState.Marquee Then
                    settings.ProgressBarMinimum = m_progressBar.Minimum
                    settings.ProgressBarMaximum = m_progressBar.Maximum
                    settings.ProgressBarValue = m_progressBar.Value
                    settings.ProgressBarState = m_progressBar.State
                End If
            End If

            If _HelpInvoked IsNot Nothing Then
                settings.InvokeHelp = True
            End If
        End Sub

        ' Here we walk our controls collection and 
        ' sort the various controls by type.         
        Private Sub SortDialogControls()
            For Each control As TaskDialogControl In m_controls
                Dim buttonBase As TaskDialogButtonBase = TryCast(control, TaskDialogButtonBase)
                Dim commandLink As TaskDialogCommandLink = TryCast(control, TaskDialogCommandLink)

                If buttonBase IsNot Nothing AndAlso String.IsNullOrEmpty(buttonBase.Text) AndAlso commandLink IsNot Nothing AndAlso String.IsNullOrEmpty(commandLink.Instruction) Then
                    Throw New InvalidOperationException(LocalizedMessages.TaskDialogButtonTextEmpty)
                End If

                Dim radButton As TaskDialogRadioButton
                Dim progBar As TaskDialogProgressBar

                ' Loop through child controls 
                ' and sort the controls based on type.
                If commandLink IsNot Nothing Then
                    commandLinks.Add(commandLink)
                ElseIf radButton.DirectCopy(TryCast(control, TaskDialogRadioButton)) IsNot Nothing Then
                    If radioButtons Is Nothing Then
                        radioButtons = New List(Of TaskDialogButtonBase)()
                    End If
                    radioButtons.Add(radButton)
                ElseIf buttonBase IsNot Nothing Then
                    If buttons Is Nothing Then
                        buttons = New List(Of TaskDialogButtonBase)()
                    End If
                    buttons.Add(buttonBase)
                ElseIf progBar.DirectCopy(TryCast(control, TaskDialogProgressBar)) IsNot Nothing Then
                    m_progressBar = progBar
                Else
                    Throw New InvalidOperationException(LocalizedMessages.TaskDialogUnkownControl)
                End If
            Next
        End Sub

#End Region

#Region "Helpers"

        ' Helper to map the standard button IDs returned by 
        ' TaskDialogIndirect to the standard button ID enum - 
        ' note that we can't just cast, as the Win32
        ' typedefs differ incoming and outgoing.

        Private Shared Function MapButtonIdToStandardButton(id As Integer) As TaskDialogStandardButtons
            Select Case CType(id, TaskDialogNativeMethods.TaskDialogCommonButtonReturnIds)
                Case TaskDialogNativeMethods.TaskDialogCommonButtonReturnIds.Ok
                    Return TaskDialogStandardButtons.Ok
                Case TaskDialogNativeMethods.TaskDialogCommonButtonReturnIds.Cancel
                    Return TaskDialogStandardButtons.Cancel
                Case TaskDialogNativeMethods.TaskDialogCommonButtonReturnIds.Abort
                    ' Included for completeness in API - 
                    ' we can't pass in an Abort standard button.
                    Return TaskDialogStandardButtons.None
                Case TaskDialogNativeMethods.TaskDialogCommonButtonReturnIds.Retry
                    Return TaskDialogStandardButtons.Retry
                Case TaskDialogNativeMethods.TaskDialogCommonButtonReturnIds.Ignore
                    ' Included for completeness in API - 
                    ' we can't pass in an Ignore standard button.
                    Return TaskDialogStandardButtons.None
                Case TaskDialogNativeMethods.TaskDialogCommonButtonReturnIds.Yes
                    Return TaskDialogStandardButtons.Yes
                Case TaskDialogNativeMethods.TaskDialogCommonButtonReturnIds.No
                    Return TaskDialogStandardButtons.No
                Case TaskDialogNativeMethods.TaskDialogCommonButtonReturnIds.Close
                    Return TaskDialogStandardButtons.Close
                Case Else
                    Return TaskDialogStandardButtons.None
            End Select
        End Function

        Private Sub ThrowIfDialogShowing(message As String)
            If NativeDialogShowing Then
                Throw New NotSupportedException(message)
            End If
        End Sub

        Private ReadOnly Property NativeDialogShowing() As Boolean
            Get
                Return (nativeDialog IsNot Nothing) AndAlso (nativeDialog.ShowState = DialogShowState.Showing OrElse nativeDialog.ShowState = DialogShowState.Closing)
            End Get
        End Property

        ' NOTE: we are going to require names be unique 
        ' across both buttons and radio buttons,
        ' even though the Win32 API allows them to be separate.
        Private Function GetButtonForId(id As Integer) As TaskDialogButtonBase
            Return DirectCast(m_controls.GetControlbyId(id), TaskDialogButtonBase)
        End Function

#End Region

#Region "IDialogControlHost Members"

        ' We're explicitly implementing this interface 
        ' as the user will never need to know about it
        ' or use it directly - it is only for the internal 
        ' implementation of "pseudo controls" within 
        ' the dialogs.

        ' Called whenever controls are being added 
        ' to or removed from the dialog control collection.
        Private Function IDialogControlHost_IsCollectionChangeAllowed() As Boolean Implements IDialogControlHost.IsCollectionChangeAllowed
            ' Only allow additions to collection if dialog is NOT showing.
            Return Not NativeDialogShowing
        End Function

        ' Called whenever controls have been added or removed.
        Private Sub IDialogControlHost_ApplyCollectionChanged() Implements IDialogControlHost.ApplyCollectionChanged
            ' If we're showing, we should never get here - 
            ' the changing notification would have thrown and the 
            ' property would not have been changed.
            System.Diagnostics.Debug.Assert(Not NativeDialogShowing, "Collection changed notification received despite show state of dialog")
        End Sub

        ' Called when a control currently in the collection 
        ' has a property changing - this is 
        ' basically to screen out property changes that 
        ' cannot occur while the dialog is showing
        ' because the Win32 API has no way for us to 
        ' propagate the changes until we re-invoke the Win32 call.
        Private Function IDialogControlHost_IsControlPropertyChangeAllowed(propertyName As String, control As DialogControl) As Boolean Implements IDialogControlHost.IsControlPropertyChangeAllowed
            System.Diagnostics.Debug.Assert(TypeOf control Is TaskDialogControl, "Property changing for a control that is not a TaskDialogControl-derived type")
            System.Diagnostics.Debug.Assert(propertyName <> "Name", "Name changes at any time are not supported - public API should have blocked this")

            Dim canChange As Boolean = False

            If Not NativeDialogShowing Then
                ' Certain properties can't be changed if the dialog is not showing
                ' we need a handle created before we can set these...
                Select Case propertyName
                    Case "Enabled"
                        canChange = False                       
                    Case Else
                        canChange = True                        
                End Select
            Else
                ' If the dialog is showing, we can only 
                ' allow some properties to change.
                Select Case propertyName
                    ' Properties that CAN'T be changed while dialog is showing.
                    Case "Text", "Default"
                        canChange = False                       
					' Properties that CAN be changed while dialog is showing.
					Case "ShowElevationIcon", "Enabled"
						canChange = True						
					Case Else
						System.Diagnostics.Debug.Assert(True, "Unknown property name coming through property changing handler")						
				End Select
			End If
			Return canChange
		End Function

        ' Called when a control currently in the collection 
        ' has a property changed - this handles propagating
        ' the new property values to the Win32 API. 
        ' If there isn't a way to change the Win32 value, then we
        ' should have already screened out the property set 
        ' in NotifyControlPropertyChanging.        
        Private Sub IDialogControlHost_ApplyControlPropertyChange(propertyName As String, control As DialogControl) Implements IDialogControlHost.ApplyControlPropertyChange
            ' We only need to apply changes to the 
            ' native dialog when it actually exists.
            If NativeDialogShowing Then
                Dim button As TaskDialogButton
                Dim radioButton As TaskDialogRadioButton
                If TypeOf control Is TaskDialogProgressBar Then
                    If Not m_progressBar.HasValidValues Then
                        Throw New ArgumentException(LocalizedMessages.TaskDialogProgressBarValueInRange)
                    End If

                    Select Case propertyName
                        Case "State"
                            nativeDialog.UpdateProgressBarState(m_progressBar.State)
                            Exit Select
                        Case "Value"
                            nativeDialog.UpdateProgressBarValue(m_progressBar.Value)
                            Exit Select
                        Case "Minimum", "Maximum"
                            nativeDialog.UpdateProgressBarRange()
                            Exit Select
                        Case Else
                            System.Diagnostics.Debug.Assert(True, "Unknown property being set")
                            
                    End Select
                ElseIf button.DirectCopy(TryCast(control, TaskDialogButton)) IsNot Nothing Then
                    Select Case propertyName
                        Case "ShowElevationIcon"
                            nativeDialog.UpdateElevationIcon(button.Id, button.UseElevationIcon)
                            Exit Select
                        Case "Enabled"
                            nativeDialog.UpdateButtonEnabled(button.Id, button.Enabled)
                            Exit Select
                        Case Else
							System.Diagnostics.Debug.Assert(True, "Unknown property being set")
							Exit Select
                    End Select
                ElseIf radioButton.DirectCopy(TryCast(control, TaskDialogRadioButton)) IsNot Nothing Then
                    Select Case propertyName
						Case "Enabled"
							nativeDialog.UpdateRadioButtonEnabled(radioButton.Id, radioButton.Enabled)
							Exit Select
						Case Else
							System.Diagnostics.Debug.Assert(True, "Unknown property being set")
							Exit Select
					End Select
				Else
					' Do nothing with property change - 
					' note that this shouldn't ever happen, we should have
					' either thrown on the changing event, or we handle above.
					System.Diagnostics.Debug.Assert(True, "Control property changed notification not handled properly - being ignored")
				End If
			End If
		End Sub

#End Region

#Region "Event Percolation Methods"

        ' All Raise*() methods are called by the 
        ' NativeTaskDialog when various pseudo-controls
        ' are triggered.
        Friend Sub RaiseButtonClickEvent(id As Integer)
            ' First check to see if the ID matches a custom button.
            Dim button As TaskDialogButtonBase = GetButtonForId(id)

            ' If a custom button was found, 
            ' raise the event - if not, it's a standard button, and
            ' we don't support custom event handling for the standard buttons
            If button IsNot Nothing Then
                button.RaiseClickEvent()
            End If
        End Sub

        Friend Sub RaiseHyperlinkClickEvent(link As String)
            RaiseEvent HyperlinkClick(Me, New TaskDialogHyperlinkClickedEventArgs(link))
        End Sub

        ' Gives event subscriber a chance to prevent 
        ' the dialog from closing, based on 
        ' the current state of the app and the button 
        ' used to commit. Note that we don't 
        ' have full access at this stage to 
        ' the full dialog state.
        Friend Function RaiseClosingEvent(id As Integer) As Integer
            Dim handler As EventHandler(Of TaskDialogClosingEventArgs) = _Closing
            If handler IsNot Nothing Then
                Dim customButton As TaskDialogButtonBase = Nothing
                Dim e As New TaskDialogClosingEventArgs()

                ' Try to identify the button - is it a standard one?
                Dim buttonClicked As TaskDialogStandardButtons = MapButtonIdToStandardButton(id)

                ' If not, it had better be a custom button...
                If buttonClicked = TaskDialogStandardButtons.None Then
                    customButton = GetButtonForId(id)

                    ' ... or we have a problem.
                    If customButton Is Nothing Then
                        Throw New InvalidOperationException(LocalizedMessages.TaskDialogBadButtonId)
                    End If

                    e.CustomButton = customButton.Name
                    e.TaskDialogResult = TaskDialogResult.CustomButtonClicked
                Else
                    e.TaskDialogResult = CType(buttonClicked, TaskDialogResult)
                End If

                ' Raise the event and determine how to proceed.
                handler(Me, e)
                If e.Cancel Then
                    Return CInt(HResult.[False])
                End If
            End If

            ' It's okay to let the dialog close.
            Return CInt(HResult.Ok)
        End Function

        Friend Sub RaiseHelpInvokedEvent()
            RaiseEvent HelpInvoked(Me, EventArgs.Empty)
        End Sub

        Friend Sub RaiseOpenedEvent()
            RaiseEvent Opened(Me, EventArgs.Empty)
        End Sub

        Friend Sub RaiseTickEvent(ticks As Integer)
            RaiseEvent Tick(Me, New TaskDialogTickEventArgs(ticks))
        End Sub

#End Region

#Region "Cleanup Code"

        ' Cleans up data and structs from a single 
        ' native dialog Show() invocation.
        Private Sub CleanUp()
            ' Reset values that would be considered 
            ' 'volatile' in a given instance.
            If m_progressBar IsNot Nothing Then
                m_progressBar.Reset()
            End If

            ' Clean out sorted control lists - 
            ' though we don't of course clear the main controls collection,
            ' so the controls are still around; we'll 
            ' resort on next show, since the collection may have changed.
            If buttons IsNot Nothing Then
                buttons.Clear()
            End If
            If commandLinks IsNot Nothing Then
                commandLinks.Clear()
            End If
            If radioButtons IsNot Nothing Then
                radioButtons.Clear()
            End If
            m_progressBar = Nothing

            ' Have the native dialog clean up the rest.
            If nativeDialog IsNot Nothing Then
                nativeDialog.Dispose()
            End If
        End Sub


        ' Dispose pattern - cleans up data and structs for 
        ' a) any native dialog currently showing, and
        ' b) anything else that the outer TaskDialog has.
        Private disposed As Boolean

        ''' <summary>
        ''' Dispose TaskDialog Resources
        ''' </summary>
        Public Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub

        ''' <summary>
        ''' TaskDialog Finalizer
        ''' </summary>
        Protected Overrides Sub Finalize()
            Try
                Dispose(False)
            Finally
                MyBase.Finalize()
            End Try
        End Sub

        ''' <summary>
        ''' Dispose TaskDialog Resources
        ''' </summary>
        ''' <param name="disposing">If true, indicates that this is being called via Dispose rather than via the finalizer.</param>
        Public Sub Dispose(disposing As Boolean)
            If Not disposed Then
                disposed = True

                If disposing Then
                    ' Clean up managed resources.
                    If nativeDialog IsNot Nothing AndAlso nativeDialog.ShowState = DialogShowState.Showing Then
                        nativeDialog.NativeClose(TaskDialogResult.Cancel)
                    End If

                    buttons = Nothing
                    radioButtons = Nothing
                    commandLinks = Nothing
                End If

                ' Clean up unmanaged resources SECOND, NTD counts on 
                ' being closed before being disposed.
                If nativeDialog IsNot Nothing Then
                    nativeDialog.Dispose()
                    nativeDialog = Nothing
                End If

                If staticDialog IsNot Nothing Then
                    staticDialog.Dispose()
                    staticDialog = Nothing


                End If
            End If
        End Sub

#End Region

        ''' <summary>
        ''' Indicates whether this feature is supported on the current platform.
        ''' </summary>
        Public Shared ReadOnly Property IsPlatformSupported() As Boolean
            Get
                ' We need Windows Vista onwards ...
                Return CoreHelpers.RunningOnVista
            End Get
        End Property
    End Class
End Namespace
