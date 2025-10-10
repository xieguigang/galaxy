Imports System
Imports System.Windows.Forms
Imports System.Drawing
Imports System.ComponentModel
Imports System.Diagnostics.CodeAnalysis

Namespace Docking
    Public Delegate Function GetPersistStringCallback() As String

    Public Class DockContentHandler
        Implements IDisposable, IDockDragSource


        Private _PreviousActive As WeifenLuo.WinFormsUI.Docking.IDockContent, _NextActive As WeifenLuo.WinFormsUI.Docking.IDockContent
        Public Sub New(form As Form)
            Me.New(form, Nothing)
        End Sub

        Public Sub New(form As Form, getPersistStringCallback As GetPersistStringCallback)
            If Not (TypeOf form Is IDockContent) Then Throw New ArgumentException(Strings.DockContent_Constructor_InvalidForm, NameOf(form))

            m_form = form
            Me.GetPersistStringCallback = getPersistStringCallback

            Events = New EventHandlerList()
            AddHandler Me.Form.Disposed, New EventHandler(AddressOf Form_Disposed)
            AddHandler Me.Form.TextChanged, New EventHandler(AddressOf Form_TextChanged)
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub

        Protected Overridable Sub Dispose(disposing As Boolean)
            If disposing Then
                DockPanel = Nothing
                If m_autoHideTab IsNot Nothing Then m_autoHideTab.Dispose()
                If m_tab IsNot Nothing Then m_tab.Dispose()

                RemoveHandler Form.Disposed, New EventHandler(AddressOf Form_Disposed)
                RemoveHandler Form.TextChanged, New EventHandler(AddressOf Form_TextChanged)
                Events.Dispose()
            End If
        End Sub

        Private m_form As Form
        Public ReadOnly Property Form As Form
            Get
                Return m_form
            End Get
        End Property

        Public ReadOnly Property Content As IDockContent
            Get
                Return TryCast(Form, IDockContent)
            End Get
        End Property

        Public Property PreviousActive As IDockContent
            Get
                Return _PreviousActive
            End Get
            Friend Set(value As IDockContent)
                _PreviousActive = value
            End Set
        End Property

        Public Property NextActive As IDockContent
            Get
                Return _NextActive
            End Get
            Friend Set(value As IDockContent)
                _NextActive = value
            End Set
        End Property

        Private ReadOnly Property Events As EventHandlerList

        Public Property AllowEndUserDocking As Boolean = True

        Friend Property SuspendAutoHidePortionUpdates As Boolean = False

        Private m_autoHidePortion As Double = 0.25
        Public Property AutoHidePortion As Double
            Get
                Return m_autoHidePortion
            End Get

            Set(value As Double)
                If value <= 0 Then Throw (New ArgumentOutOfRangeException(Strings.DockContentHandler_AutoHidePortion_OutOfRange))

                If SuspendAutoHidePortionUpdates Then Return

                If Math.Abs(m_autoHidePortion - value) < Double.Epsilon Then Return

                m_autoHidePortion = value

                If DockPanel Is Nothing Then Return

                If DockPanel.ActiveAutoHideContent Is Content Then DockPanel.PerformLayout()
            End Set
        End Property

        Private m_closeButton As Boolean = True
        Public Property CloseButton As Boolean
            Get
                Return m_closeButton
            End Get

            Set(value As Boolean)
                If m_closeButton = value Then Return

                m_closeButton = value
                If IsActiveContentHandler Then Pane.RefreshChanges()
            End Set
        End Property

        Private m_closeButtonVisible As Boolean = True
        ''' <summary>
        ''' Determines whether the close button is visible on the content
        ''' </summary>
        Public Property CloseButtonVisible As Boolean
            Get
                Return m_closeButtonVisible
            End Get

            Set(value As Boolean)
                If m_closeButtonVisible = value Then Return

                m_closeButtonVisible = value
                If IsActiveContentHandler Then Pane.RefreshChanges()
            End Set
        End Property

        Private ReadOnly Property IsActiveContentHandler As Boolean
            Get
                Return Pane IsNot Nothing AndAlso Pane.ActiveContent IsNot Nothing AndAlso Pane.ActiveContent.DockHandler Is Me
            End Get
        End Property

        Private ReadOnly Property DefaultDockState As DockState
            Get
                If ShowHint <> DockState.Unknown AndAlso ShowHint <> DockState.Hidden Then Return ShowHint

                If (DockAreas And DockAreas.Document) <> 0 Then Return DockState.Document
                If (DockAreas And DockAreas.DockRight) <> 0 Then Return DockState.DockRight
                If (DockAreas And DockAreas.DockLeft) <> 0 Then Return DockState.DockLeft
                If (DockAreas And DockAreas.DockBottom) <> 0 Then Return DockState.DockBottom
                If (DockAreas And DockAreas.DockTop) <> 0 Then Return DockState.DockTop

                Return DockState.Unknown
            End Get
        End Property

        Private ReadOnly Property DefaultShowState As DockState
            Get
                If ShowHint <> DockState.Unknown Then Return ShowHint

                If (DockAreas And DockAreas.Document) <> 0 Then Return DockState.Document
                If (DockAreas And DockAreas.DockRight) <> 0 Then Return DockState.DockRight
                If (DockAreas And DockAreas.DockLeft) <> 0 Then Return DockState.DockLeft
                If (DockAreas And DockAreas.DockBottom) <> 0 Then Return DockState.DockBottom
                If (DockAreas And DockAreas.DockTop) <> 0 Then Return DockState.DockTop
                If (DockAreas And DockAreas.Float) <> 0 Then Return DockState.Float

                Return DockState.Unknown
            End Get
        End Property

        Private m_allowedAreas As DockAreas = DockAreas.DockLeft Or DockAreas.DockRight Or DockAreas.DockTop Or DockAreas.DockBottom Or DockAreas.Document Or DockAreas.Float
        Public Property DockAreas As DockAreas
            Get
                Return m_allowedAreas
            End Get
            Set(value As DockAreas)
                If m_allowedAreas = value Then Return

                If Not DockHelper.IsDockStateValid(DockState, value) Then Throw (New InvalidOperationException(Strings.DockContentHandler_DockAreas_InvalidValue))

                m_allowedAreas = value

                If Not DockHelper.IsDockStateValid(ShowHint, m_allowedAreas) Then ShowHint = DockState.Unknown
            End Set
        End Property

        Private m_dockState As DockState = DockState.Unknown
        Public Property DockState As DockState
            Get
                Return m_dockState
            End Get

            Set(value As DockState)
                If m_dockState = value Then Return

                DockPanel.SuspendLayout(True)

                If value = DockState.Hidden Then
                    IsHidden = True
                Else
                    SetDockState(False, value, Pane)
                End If

                DockPanel.ResumeLayout(True, True)
            End Set
        End Property

        Private m_dockPanel As DockPanel = Nothing
        Public Property DockPanel As DockPanel
            Get
                Return m_dockPanel
            End Get

            Set(value As DockPanel)
                If m_dockPanel Is value Then Return

                Pane = Nothing

                If m_dockPanel IsNot Nothing Then m_dockPanel.RemoveContent(Content)

                If m_tab IsNot Nothing Then
                    m_tab.Dispose()
                    m_tab = Nothing
                End If

                If m_autoHideTab IsNot Nothing Then
                    m_autoHideTab.Dispose()
                    m_autoHideTab = Nothing
                End If

                m_dockPanel = value
                If m_dockPanel IsNot Nothing Then
                    m_dockPanel.AddContent(Content)
                    Form.TopLevel = False
                    Form.FormBorderStyle = FormBorderStyle.None
                    Form.ShowInTaskbar = False
                    Form.WindowState = FormWindowState.Normal
                    Content.ApplyTheme()
                    If IsRunningOnMono Then Return

                    SetWindowPos(Form.Handle, IntPtr.Zero, 0, 0, 0, 0, Win32.FlagsSetWindowPos.SWP_NOACTIVATE Or Win32.FlagsSetWindowPos.SWP_NOMOVE Or Win32.FlagsSetWindowPos.SWP_NOSIZE Or Win32.FlagsSetWindowPos.SWP_NOZORDER Or Win32.FlagsSetWindowPos.SWP_NOOWNERZORDER Or Win32.FlagsSetWindowPos.SWP_FRAMECHANGED)
                End If
            End Set
        End Property

        Public ReadOnly Property Icon As Icon
            Get
                Return Form.Icon
            End Get
        End Property

        Public Property Pane As DockPane
            Get
                Return If(IsFloat, FloatPane, PanelPane)
            End Get

            Set(value As DockPane)
                If Pane Is value Then Return

                DockPanel.SuspendLayout(True)

                Dim oldPane = Pane

                SuspendSetDockState()
                FloatPane = If(value Is Nothing, Nothing, If(value.IsFloat, value, FloatPane))
                PanelPane = If(value Is Nothing, Nothing, If(value.IsFloat, PanelPane, value))
                ResumeSetDockState(IsHidden, If(value IsNot Nothing, value.DockState, DockState.Unknown), oldPane)

                DockPanel.ResumeLayout(True, True)
            End Set
        End Property

        Private m_isHidden As Boolean = True
        Public Property IsHidden As Boolean
            Get
                Return m_isHidden
            End Get

            Set(value As Boolean)
                If m_isHidden = value Then Return

                SetDockState(value, VisibleState, Pane)
            End Set
        End Property

        Private m_tabText As String = Nothing
        Public Property TabText As String
            Get
                Return If(Equals(m_tabText, Nothing) OrElse Equals(m_tabText, ""), Form.Text, m_tabText)
            End Get

            Set(value As String)
                If Equals(m_tabText, value) Then Return

                m_tabText = value
                If Pane IsNot Nothing Then Pane.RefreshChanges()
            End Set
        End Property

        Private m_visibleState As DockState = DockState.Unknown
        Public Property VisibleState As DockState
            Get
                Return m_visibleState
            End Get

            Set(value As DockState)
                If m_visibleState = value Then Return

                SetDockState(IsHidden, value, Pane)
            End Set
        End Property

        Private m_isFloat As Boolean = False
        Public Property IsFloat As Boolean
            Get
                Return m_isFloat
            End Get

            Set(value As Boolean)
                If m_isFloat = value Then Return

                Dim visibleState = CheckDockState(value)

                If visibleState = DockState.Unknown Then Throw New InvalidOperationException(Strings.DockContentHandler_IsFloat_InvalidValue)

                SetDockState(IsHidden, visibleState, Pane)
                If EnableFloatSplitterFix = True Then
                    If PanelPane IsNot Nothing AndAlso PanelPane.IsHidden Then
                        PanelPane.NestedDockingStatus.NestedPanes.SwitchPaneWithFirstChild(PanelPane)
                    End If
                End If
            End Set
        End Property

        <SuppressMessage("Microsoft.Naming", "CA1720:AvoidTypeNamesInParameters")>
        Public Function CheckDockState(isFloat As Boolean) As DockState
            Dim dockState As DockState

            If isFloat Then
                If Not IsDockStateValid(DockState.Float) Then
                    dockState = DockState.Unknown
                Else
                    dockState = DockState.Float
                End If
            Else
                dockState = If(PanelPane IsNot Nothing, PanelPane.DockState, DefaultDockState)
                If dockState <> DockState.Unknown AndAlso Not IsDockStateValid(dockState) Then dockState = DockState.Unknown
            End If

            Return dockState
        End Function

        Private m_panelPane As DockPane = Nothing
        Public Property PanelPane As DockPane
            Get
                Return m_panelPane
            End Get

            Set(value As DockPane)
                If m_panelPane Is value Then Return

                If value IsNot Nothing Then
                    If value.IsFloat OrElse value.DockPanel IsNot DockPanel Then Throw New InvalidOperationException(Strings.DockContentHandler_DockPane_InvalidValue)
                End If

                Dim oldPane = Pane

                If m_panelPane IsNot Nothing Then RemoveFromPane(m_panelPane)
                m_panelPane = value
                If m_panelPane IsNot Nothing Then
                    m_panelPane.AddContent(Content)
                    SetDockState(IsHidden, If(IsFloat, DockState.Float, m_panelPane.DockState), oldPane)
                Else
                    SetDockState(IsHidden, DockState.Unknown, oldPane)
                End If
            End Set
        End Property

        Private Sub RemoveFromPane(pane As DockPane)
            pane.RemoveContent(Content)
            SetPane(Nothing)
            If pane.Contents.Count = 0 Then pane.Dispose()
        End Sub

        Private m_floatPane As DockPane = Nothing
        Public Property FloatPane As DockPane
            Get
                Return m_floatPane
            End Get

            Set(value As DockPane)
                If m_floatPane Is value Then Return

                If value IsNot Nothing Then
                    If Not value.IsFloat OrElse value.DockPanel IsNot DockPanel Then Throw New InvalidOperationException(Strings.DockContentHandler_FloatPane_InvalidValue)
                End If

                Dim oldPane = Pane

                If m_floatPane IsNot Nothing Then RemoveFromPane(m_floatPane)
                m_floatPane = value
                If m_floatPane IsNot Nothing Then
                    m_floatPane.AddContent(Content)
                    SetDockState(IsHidden, If(IsFloat, DockState.Float, VisibleState), oldPane)
                Else
                    SetDockState(IsHidden, DockState.Unknown, oldPane)
                End If
            End Set
        End Property

        Private m_countSetDockState As Integer = 0
        Private Sub SuspendSetDockState()
            m_countSetDockState += 1
        End Sub

        Private Sub ResumeSetDockState()
            m_countSetDockState -= 1
            If m_countSetDockState < 0 Then m_countSetDockState = 0
        End Sub

        Friend ReadOnly Property IsSuspendSetDockState As Boolean
            Get
                Return m_countSetDockState <> 0
            End Get
        End Property

        Private Sub ResumeSetDockState(isHidden As Boolean, visibleState As DockState, oldPane As DockPane)
            ResumeSetDockState()
            SetDockState(isHidden, visibleState, oldPane)
        End Sub

        Friend Sub SetDockState(isHidden As Boolean, visibleState As DockState, oldPane As DockPane)
            If IsSuspendSetDockState Then Return

            If Me.DockPanel Is Nothing AndAlso visibleState <> DockState.Unknown Then Throw New InvalidOperationException(Strings.DockContentHandler_SetDockState_NullPanel)

            If visibleState = DockState.Hidden OrElse visibleState <> DockState.Unknown AndAlso Not IsDockStateValid(visibleState) Then Throw New InvalidOperationException(Strings.DockContentHandler_SetDockState_InvalidState)

            Dim dockPanel = Me.DockPanel
            If dockPanel IsNot Nothing Then dockPanel.SuspendLayout(True)

            SuspendSetDockState()

            Dim oldDockState = DockState

            If m_isHidden <> isHidden OrElse oldDockState = DockState.Unknown Then
                m_isHidden = isHidden
            End If
            m_visibleState = visibleState
            m_dockState = If(isHidden, DockState.Hidden, visibleState)

            'Remove hidden content (shown content is added last so removal is done first to invert the operation)
            Dim hidingContent = DockState = DockState.Hidden OrElse DockState = DockState.Unknown OrElse IsDockStateAutoHide(DockState)
            If EnableContentOrderFix = True AndAlso oldDockState <> DockState Then
                If hidingContent Then
                    If Not IsRunningOnMono Then Me.DockPanel.ContentFocusManager.RemoveFromList(Content)
                End If
            End If

            If visibleState = DockState.Unknown Then
                Pane = Nothing
            Else
                m_isFloat = m_visibleState = DockState.Float

                If Pane Is Nothing Then
                    Pane = Me.DockPanel.Theme.Extender.DockPaneFactory.CreateDockPane(Content, visibleState, True)
                ElseIf Pane.DockState <> visibleState Then
                    If Pane.Contents.Count = 1 Then
                        Pane.SetDockState(visibleState)
                    Else
                        Pane = Me.DockPanel.Theme.Extender.DockPaneFactory.CreateDockPane(Content, visibleState, True)
                    End If
                End If
            End If

            If Form.ContainsFocus Then
                If DockState = DockState.Hidden OrElse DockState = DockState.Unknown Then
                    If Not IsRunningOnMono Then
                        Me.DockPanel.ContentFocusManager.GiveUpFocus(Content)
                    End If
                End If
            End If

            SetPaneAndVisible(Pane)

            If oldPane IsNot Nothing AndAlso Not oldPane.IsDisposed AndAlso oldDockState = oldPane.DockState Then RefreshDockPane(oldPane)

            If Pane IsNot Nothing AndAlso DockState = Pane.DockState Then
                If Pane IsNot oldPane OrElse Pane Is oldPane AndAlso oldDockState <> oldPane.DockState Then
                    ' Avoid early refresh of hidden AutoHide panes
                    If (Pane.DockWindow Is Nothing OrElse Pane.DockWindow.Visible OrElse Pane.IsHidden) AndAlso Not Pane.IsAutoHide Then
                        RefreshDockPane(Pane)
                    End If
                End If
            End If

            If oldDockState <> DockState Then
                If EnableContentOrderFix = True Then
                    'Add content that is being shown
                    If Not hidingContent Then
                        If Not IsRunningOnMono Then Me.DockPanel.ContentFocusManager.AddToList(Content)
                    End If
                Else
                    If DockState = DockState.Hidden OrElse DockState = DockState.Unknown OrElse IsDockStateAutoHide(DockState) Then
                        If Not IsRunningOnMono Then
                            Me.DockPanel.ContentFocusManager.RemoveFromList(Content)
                        End If
                    ElseIf Not IsRunningOnMono Then
                        Me.DockPanel.ContentFocusManager.AddToList(Content)
                    End If
                End If

                ResetAutoHidePortion(oldDockState, DockState)
                OnDockStateChanged(EventArgs.Empty)
            End If

            ResumeSetDockState()

            If dockPanel IsNot Nothing Then dockPanel.ResumeLayout(True, True)
        End Sub

        Private Sub ResetAutoHidePortion(oldState As DockState, newState As DockState)
            If oldState = newState OrElse ToggleAutoHideState(oldState) = newState Then Return

            Select Case newState
                Case DockState.DockTop, DockState.DockTopAutoHide
                    AutoHidePortion = DockPanel.DockTopPortion
                Case DockState.DockLeft, DockState.DockLeftAutoHide
                    AutoHidePortion = DockPanel.DockLeftPortion
                Case DockState.DockBottom, DockState.DockBottomAutoHide
                    AutoHidePortion = DockPanel.DockBottomPortion
                Case DockState.DockRight, DockState.DockRightAutoHide
                    AutoHidePortion = DockPanel.DockRightPortion
            End Select
        End Sub

        Private Shared Sub RefreshDockPane(pane As DockPane)
            pane.RefreshChanges()
            pane.ValidateActiveContent()
        End Sub

        Friend ReadOnly Property PersistString As String
            Get
                Return If(GetPersistStringCallback Is Nothing, Form.GetType().ToString(), _GetPersistStringCallback())
            End Get
        End Property

        Public Property GetPersistStringCallback As GetPersistStringCallback

        Public Property HideOnClose As Boolean

        Private m_showHint As DockState = DockState.Unknown
        Public Property ShowHint As DockState
            Get
                Return m_showHint
            End Get

            Set(value As DockState)
                If Not DockHelper.IsDockStateValid(value, DockAreas) Then Throw (New InvalidOperationException(Strings.DockContentHandler_ShowHint_InvalidValue))

                If m_showHint = value Then Return

                m_showHint = value
            End Set
        End Property

        Private m_isActivated As Boolean
        Public Property IsActivated As Boolean
            Get
                Return m_isActivated
            End Get

            Friend Set(value As Boolean)
                If m_isActivated = value Then Return

                m_isActivated = value
            End Set
        End Property

        Public Function IsDockStateValid(dockState As DockState) As Boolean Implements IDockDragSource.IsDockStateValid
            If DockPanel IsNot Nothing AndAlso dockState = DockState.Document AndAlso DockPanel.DocumentStyle = DocumentStyle.SystemMdi Then
                Return False
            Else
                Return DockHelper.IsDockStateValid(dockState, DockAreas)
            End If
        End Function
#If NET35 Or NET40
        public ContextMenu TabPageContextMenu { get; set; }
#End If
        Public Property ToolTipText As String

        Public Sub Activate()
            If DockPanel Is Nothing Then
                Form.Activate()
            ElseIf Pane Is Nothing Then
                Show(DockPanel)
            Else
                IsHidden = False
                Pane.ActiveContent = Content
                If DockState = DockState.Document AndAlso DockPanel.DocumentStyle = DocumentStyle.SystemMdi Then
                    Form.Activate()
                    Return
                ElseIf IsDockStateAutoHide(DockState) Then
                    If DockPanel.ActiveAutoHideContent IsNot Content Then
                        DockPanel.ActiveAutoHideContent = Nothing
                        Return
                    End If
                End If

                If Form.ContainsFocus Then Return

                If IsRunningOnMono Then Return

                DockPanel.ContentFocusManager.Activate(Content)
            End If
        End Sub

        Public Sub GiveUpFocus()
            If Not IsRunningOnMono Then DockPanel.ContentFocusManager.GiveUpFocus(Content)
        End Sub

        Private m_activeWindowHandle As IntPtr = IntPtr.Zero
        Friend Property ActiveWindowHandle As IntPtr
            Get
                Return m_activeWindowHandle
            End Get
            Set(value As IntPtr)
                m_activeWindowHandle = value
            End Set
        End Property

        Public Sub Hide()
            IsHidden = True
        End Sub

        Friend Sub SetPaneAndVisible(pane As DockPane)
            SetPane(pane)
            SetVisible()
        End Sub

        Private Sub SetPane(pane As DockPane)
            If pane IsNot Nothing AndAlso pane.DockState = DockState.Document AndAlso DockPanel.DocumentStyle = DocumentStyle.DockingMdi Then
                If TypeOf Form.Parent Is DockPane Then SetParent(Nothing)
                If Form.MdiParent IsNot DockPanel.ParentForm Then
                    FlagClipWindow = True

                    ' The content form should inherit the font of the dock panel, not the font of
                    ' the dock panel's parent form. However, the content form's font value should
                    ' not be overwritten if it has been explicitly set to a non-default value.
                    If EnableFontInheritanceFix = True AndAlso Form.Font Is Control.DefaultFont Then
                        Form.MdiParent = DockPanel.ParentForm
                        Form.Font = DockPanel.Font
                    Else
                        Form.MdiParent = DockPanel.ParentForm
                    End If
                End If
            Else
                FlagClipWindow = True
                If Form.MdiParent IsNot Nothing Then Form.MdiParent = Nothing
                If Form.TopLevel Then Form.TopLevel = False
                SetParent(pane)
            End If
        End Sub

        Friend Sub SetVisible()
            Dim visible As Boolean

            If IsHidden Then
                visible = False
            ElseIf Pane IsNot Nothing AndAlso Pane.DockState = DockState.Document AndAlso DockPanel.DocumentStyle = DocumentStyle.DockingMdi Then
                visible = True
            ElseIf Pane IsNot Nothing AndAlso Pane.ActiveContent Is Content Then
                visible = True
            ElseIf Pane IsNot Nothing AndAlso Pane.ActiveContent IsNot Content Then
                visible = False
            Else
                visible = Form.Visible
            End If

            If Form.Visible <> visible Then Form.Visible = visible
        End Sub

        Private Sub SetParent(value As Control)
            If Form.Parent Is value Then Return

            '!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            ' Workaround of .Net Framework bug:
            ' Change the parent of a control with focus may result in the first
            ' MDI child form get activated. 
            ' !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            Dim bRestoreFocus = False
            If Form.ContainsFocus Then
                ' Suggested as a fix for a memory leak by bugreports
                If value Is Nothing AndAlso Not IsFloat Then
                    If Not IsRunningOnMono Then
                        DockPanel.ContentFocusManager.GiveUpFocus(Content)
                    End If
                Else
                    DockPanel.SaveFocus()
                    bRestoreFocus = True
                End If
            End If

            ' !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            Dim parentChanged = value IsNot Form.Parent
            Form.Parent = value
            If EnableMainWindowFocusLostFix = True AndAlso parentChanged Then
                Form.Focus()
            End If

            ' !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            ' Workaround of .Net Framework bug:
            ' Change the parent of a control with focus may result in the first
            ' MDI child form get activated. 
            ' !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            If bRestoreFocus AndAlso Not IsRunningOnMono Then Activate()

            ' !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        End Sub

        Public Sub Show()
            If DockPanel Is Nothing Then
                Form.Show()
            Else
                Show(DockPanel)
            End If
        End Sub

        Public Sub Show(dockPanel As DockPanel)
            If dockPanel Is Nothing Then Throw (New ArgumentNullException(Strings.DockContentHandler_Show_NullDockPanel))

            If DockState = DockState.Unknown Then
                Show(dockPanel, DefaultShowState)
            ElseIf Me.DockPanel IsNot dockPanel Then
                Show(dockPanel, If(DockState = DockState.Hidden, m_visibleState, DockState))
            Else
                Activate()
            End If
        End Sub

        Public Sub Show(dockPanel As DockPanel, dockState As DockState)
            If dockPanel Is Nothing Then Throw (New ArgumentNullException(Strings.DockContentHandler_Show_NullDockPanel))

            If dockState = DockState.Unknown OrElse dockState = DockState.Hidden Then Throw (New ArgumentException(Strings.DockContentHandler_Show_InvalidDockState))

            If dockPanel.Theme.GetType() Is GetType(DefaultTheme) Then Throw New ArgumentException(Strings.Theme_NoTheme)

            dockPanel.SuspendLayout(True)

            Me.DockPanel = dockPanel

            If dockState = DockState.Float Then
                If FloatPane Is Nothing Then Pane = Me.DockPanel.Theme.Extender.DockPaneFactory.CreateDockPane(Content, DockState.Float, True)
            ElseIf PanelPane Is Nothing Then
                Dim paneExisting As DockPane = Nothing
                For Each pane As DockPane In Me.DockPanel.Panes
                    If pane.DockState = dockState Then
                        If paneExisting Is Nothing OrElse pane.IsActivated Then paneExisting = pane

                        If pane.IsActivated Then Exit For
                    End If
                Next

                If paneExisting Is Nothing Then
                    Pane = Me.DockPanel.Theme.Extender.DockPaneFactory.CreateDockPane(Content, dockState, True)
                Else
                    Pane = paneExisting
                End If
            End If

            Me.DockState = dockState
            dockPanel.ResumeLayout(True, True) 'we'll resume the layout before activating to ensure that the position
            Activate()                         'and size of the form are finally processed before the form is shown
        End Sub

        <SuppressMessage("Microsoft.Naming", "CA1720:AvoidTypeNamesInParameters")>
        Public Sub Show(dockPanel As DockPanel, floatWindowBounds As Rectangle)
            If dockPanel Is Nothing Then Throw (New ArgumentNullException(Strings.DockContentHandler_Show_NullDockPanel))

            dockPanel.SuspendLayout(True)

            Me.DockPanel = dockPanel
            If FloatPane Is Nothing Then
                IsHidden = True ' to reduce the screen flicker
                FloatPane = Me.DockPanel.Theme.Extender.DockPaneFactory.CreateDockPane(Content, DockState.Float, False)
                FloatPane.FloatWindow.StartPosition = FormStartPosition.Manual
            End If

            FloatPane.FloatWindow.Bounds = floatWindowBounds

            Show(dockPanel, DockState.Float)
            Activate()

            dockPanel.ResumeLayout(True, True)
        End Sub

        Public Sub Show(pane As DockPane, beforeContent As IDockContent)
            If pane Is Nothing Then Throw (New ArgumentNullException(Strings.DockContentHandler_Show_NullPane))

            If beforeContent IsNot Nothing AndAlso pane.Contents.IndexOf(beforeContent) = -1 Then Throw (New ArgumentException(Strings.DockContentHandler_Show_InvalidBeforeContent))

            pane.DockPanel.SuspendLayout(True)

            DockPanel = pane.DockPanel
            Me.Pane = pane
            pane.SetContentIndex(Content, pane.Contents.IndexOf(beforeContent))
            Show()

            pane.DockPanel.ResumeLayout(True, True)
        End Sub

        Public Sub Show(previousPane As DockPane, alignment As DockAlignment, proportion As Double)
            If previousPane Is Nothing Then Throw (New ArgumentException(Strings.DockContentHandler_Show_InvalidPrevPane))

            If IsDockStateAutoHide(previousPane.DockState) Then Throw (New ArgumentException(Strings.DockContentHandler_Show_InvalidPrevPane))

            previousPane.DockPanel.SuspendLayout(True)

            DockPanel = previousPane.DockPanel
            DockPanel.Theme.Extender.DockPaneFactory.CreateDockPane(Content, previousPane, alignment, proportion, True)
            Show()

            previousPane.DockPanel.ResumeLayout(True, True)
        End Sub

        Public Sub Close()
            Dim dockPanel = Me.DockPanel
            If dockPanel IsNot Nothing Then dockPanel.SuspendLayout(True)
            Form.Close()
            If dockPanel IsNot Nothing Then dockPanel.ResumeLayout(True, True)
        End Sub

        Private m_tab As DockPaneStripBase.Tab = Nothing
        Friend Function GetTab(dockPaneStrip As DockPaneStripBase) As DockPaneStripBase.Tab
            If m_tab Is Nothing Then m_tab = dockPaneStrip.CreateTab(Content)

            Return m_tab
        End Function

        Private m_autoHideTab As IDisposable = Nothing
        Friend Property AutoHideTab As IDisposable
            Get
                Return m_autoHideTab
            End Get
            Set(value As IDisposable)
                m_autoHideTab = value
            End Set
        End Property

#Region "Events"
        Private Shared ReadOnly DockStateChangedEvent As Object = New Object()
        Public Custom Event DockStateChanged As EventHandler
            AddHandler(value As EventHandler)
                Events.AddHandler(DockStateChangedEvent, value)
            End AddHandler
            RemoveHandler(value As EventHandler)
                Events.RemoveHandler(DockStateChangedEvent, value)
            End RemoveHandler
            RaiseEvent(sender As Object, e As EventArgs)
            End RaiseEvent
        End Event
        Protected Overridable Sub OnDockStateChanged(e As EventArgs)
            Dim handler = CType(Events(DockStateChangedEvent), EventHandler)
            If handler IsNot Nothing Then handler(Me, e)
        End Sub
#End Region

        Private Sub Form_Disposed(sender As Object, e As EventArgs)
            Dispose()
        End Sub

        Private Sub Form_TextChanged(sender As Object, e As EventArgs)
            If IsDockStateAutoHide(DockState) Then
                DockPanel.RefreshAutoHideStrip()
            ElseIf Pane IsNot Nothing Then
                If Pane.FloatWindow IsNot Nothing Then Pane.FloatWindow.SetText()
                Pane.RefreshChanges()
            End If
        End Sub

        Private m_flagClipWindow As Boolean = False
        Friend Property FlagClipWindow As Boolean
            Get
                Return m_flagClipWindow
            End Get

            Set(value As Boolean)
                If m_flagClipWindow = value Then Return

                m_flagClipWindow = value
                If m_flagClipWindow Then
                    Form.Region = New Region(Rectangle.Empty)
                Else
                    Form.Region = Nothing
                End If
            End Set
        End Property

        Private m_tabPageContextMenuStrip As ContextMenuStrip

        Public Property TabPageContextMenuStrip As ContextMenuStrip
            Get
                Return m_tabPageContextMenuStrip
            End Get

            Set(value As ContextMenuStrip)
                If value Is m_tabPageContextMenuStrip Then Return

                m_tabPageContextMenuStrip = value
                ApplyTheme()
            End Set
        End Property

        Friend Sub ApplyTheme()
            If m_tabPageContextMenuStrip IsNot Nothing AndAlso DockPanel IsNot Nothing Then DockPanel.Theme.ApplyTo(m_tabPageContextMenuStrip)
        End Sub

#Region "IDockDragSource Members"

        Private ReadOnly Property DragControl As Control Implements IDragSource.DragControl
            Get
                Return Form
            End Get
        End Property

        Private Function CanDockTo(pane As DockPane) As Boolean Implements IDockDragSource.CanDockTo
            If Not IsDockStateValid(pane.DockState) Then Return False

            If Me.Pane Is pane AndAlso pane.DisplayingContents.Count = 1 Then Return False

            Return True
        End Function

        Private Function BeginDrag(ptMouse As Point) As Rectangle Implements IDockDragSource.BeginDrag
            Dim size As Size
            Dim floatPane = Me.FloatPane
            If DockState = DockState.Float OrElse floatPane Is Nothing OrElse floatPane.FloatWindow.NestedPanes.Count <> 1 Then
                size = DockPanel.DefaultFloatWindowSize
            Else
                size = floatPane.FloatWindow.Size
            End If

            Dim location As Point
            Dim rectPane = Pane.ClientRectangle
            If DockState = DockState.Document Then
                If Pane.DockPanel.DocumentTabStripLocation = DocumentTabStripLocation.Bottom Then
                    location = New Point(rectPane.Left, rectPane.Bottom - size.Height)
                Else
                    location = New Point(rectPane.Left, rectPane.Top)
                End If
            Else
                location = New Point(rectPane.Left, rectPane.Bottom)
                location.Y -= size.Height
            End If
            location = Pane.PointToScreen(location)

            If ptMouse.X > location.X + size.Width Then location.X += ptMouse.X - (location.X + size.Width) + DockPanel.Theme.Measures.SplitterSize

            Return New Rectangle(location, size)
        End Function

        Private Sub EndDrag() Implements IDockDragSource.EndDrag
        End Sub

        Public Sub FloatAt(floatWindowBounds As Rectangle) Implements IDockDragSource.FloatAt
            ' TODO: where is the pane used?
            Dim pane = DockPanel.Theme.Extender.DockPaneFactory.CreateDockPane(Content, floatWindowBounds, True)
        End Sub

        Public Sub DockTo(pane As DockPane, dockStyle As DockStyle, contentIndex As Integer) Implements IDockDragSource.DockTo
            If dockStyle = DockStyle.Fill Then
                Dim samePane = Me.Pane Is pane
                If Not samePane Then Me.Pane = pane

                Dim visiblePanes = 0
                Dim convertedIndex = 0
                While visiblePanes <= contentIndex AndAlso convertedIndex < Me.Pane.Contents.Count
                    Dim window As DockContent = TryCast(Me.Pane.Contents(convertedIndex), DockContent)
                    If window IsNot Nothing AndAlso Not window.IsHidden Then Threading.Interlocked.Increment(visiblePanes)

                    Threading.Interlocked.Increment(convertedIndex)
                End While

                contentIndex = Math.Min(Math.Max(0, convertedIndex - 1), Me.Pane.Contents.Count - 1)

                If contentIndex = -1 OrElse Not samePane Then
                    pane.SetContentIndex(Content, contentIndex)
                Else
                    Dim contents = pane.Contents
                    Dim oldIndex = contents.IndexOf(Content)
                    Dim newIndex = contentIndex
                    If oldIndex < newIndex Then
                        newIndex += 1
                        If newIndex > contents.Count - 1 Then newIndex = -1
                    End If
                    pane.SetContentIndex(Content, newIndex)
                End If
            Else
                Dim paneFrom = DockPanel.Theme.Extender.DockPaneFactory.CreateDockPane(Content, pane.DockState, True)
                Dim container = pane.NestedPanesContainer
                If dockStyle = DockStyle.Left Then
                    paneFrom.DockTo(container, pane, DockAlignment.Left, 0.5)
                ElseIf dockStyle = DockStyle.Right Then
                    paneFrom.DockTo(container, pane, DockAlignment.Right, 0.5)
                ElseIf dockStyle = DockStyle.Top Then
                    paneFrom.DockTo(container, pane, DockAlignment.Top, 0.5)
                ElseIf dockStyle = DockStyle.Bottom Then
                    paneFrom.DockTo(container, pane, DockAlignment.Bottom, 0.5)
                End If

                paneFrom.DockState = pane.DockState
            End If

            If EnableActivateOnDockFix = True Then Me.Pane.ActiveContent = Content
        End Sub

        Public Sub DockTo(panel As DockPanel, dockStyle As DockStyle) Implements IDockDragSource.DockTo
            If panel IsNot DockPanel Then Throw New ArgumentException(Strings.IDockDragSource_DockTo_InvalidPanel, NameOf(panel))

            Dim pane As DockPane

            If dockStyle = DockStyle.Top Then
                pane = DockPanel.Theme.Extender.DockPaneFactory.CreateDockPane(Content, DockState.DockTop, True)
            ElseIf dockStyle = DockStyle.Bottom Then
                pane = DockPanel.Theme.Extender.DockPaneFactory.CreateDockPane(Content, DockState.DockBottom, True)
            ElseIf dockStyle = DockStyle.Left Then
                pane = DockPanel.Theme.Extender.DockPaneFactory.CreateDockPane(Content, DockState.DockLeft, True)
            ElseIf dockStyle = DockStyle.Right Then
                pane = DockPanel.Theme.Extender.DockPaneFactory.CreateDockPane(Content, DockState.DockRight, True)
            ElseIf dockStyle = DockStyle.Fill Then
                pane = DockPanel.Theme.Extender.DockPaneFactory.CreateDockPane(Content, DockState.Document, True)
            Else
                Return
            End If
        End Sub

#End Region
    End Class
End Namespace
