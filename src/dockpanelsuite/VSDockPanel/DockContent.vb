Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Diagnostics.CodeAnalysis

Namespace WeifenLuo.WinFormsUI.Docking
    Public Class DockContent
        Inherits Form
        Implements IDockContent
        Public Sub New()
            m_dockHandler = New DockContentHandler(Me, New GetPersistStringCallback(AddressOf GetPersistString))
            AddHandler m_dockHandler.DockStateChanged, New EventHandler(AddressOf DockHandler_DockStateChanged)
            If EnableFontInheritanceFix <> True Then
                'Suggested as a fix by bensty regarding form resize
                AddHandler ParentChanged, New EventHandler(AddressOf DockContent_ParentChanged)
            End If
        End Sub

        'Suggested as a fix by bensty regarding form resize
        Private Sub DockContent_ParentChanged(Sender As Object, e As EventArgs)
            If Parent IsNot Nothing Then Font = Parent.Font
        End Sub

        Private m_dockHandler As DockContentHandler = Nothing
        <Browsable(False)>
        Public ReadOnly Property DockHandler As DockContentHandler Implements IDockContent.DockHandler
            Get
                Return m_dockHandler
            End Get
        End Property

        <LocalizedCategory("Category_Docking")>
        <LocalizedDescription("DockContent_AllowEndUserDocking_Description")>
        <DefaultValue(True)>
        Public Property AllowEndUserDocking As Boolean
            Get
                Return DockHandler.AllowEndUserDocking
            End Get
            Set(value As Boolean)
                DockHandler.AllowEndUserDocking = value
            End Set
        End Property

        <LocalizedCategory("Category_Docking")>
        <LocalizedDescription("DockContent_DockAreas_Description")>
        <DefaultValue(DockAreas.DockLeft Or DockAreas.DockRight Or DockAreas.DockTop Or DockAreas.DockBottom Or DockAreas.Document Or DockAreas.Float)>
        Public Property DockAreas As DockAreas
            Get
                Return DockHandler.DockAreas
            End Get
            Set(value As DockAreas)
                DockHandler.DockAreas = value
            End Set
        End Property

        <LocalizedCategory("Category_Docking")>
        <LocalizedDescription("DockContent_AutoHidePortion_Description")>
        <DefaultValue(0.25)>
        Public Property AutoHidePortion As Double
            Get
                Return DockHandler.AutoHidePortion
            End Get
            Set(value As Double)
                DockHandler.AutoHidePortion = value
            End Set
        End Property

        Private m_tabText As String = Nothing
        <Localizable(True)>
        <LocalizedCategory("Category_Docking")>
        <LocalizedDescription("DockContent_TabText_Description")>
        Public Property TabText As String
            Get
                Return m_tabText
            End Get
            Set(value As String)
                m_tabText = value
                DockHandler.TabText = value
            End Set
        End Property

        Private Function ShouldSerializeTabText() As Boolean
            Return Not Equals(m_tabText, Nothing)
        End Function

        <LocalizedCategory("Category_Docking")>
        <LocalizedDescription("DockContent_CloseButton_Description")>
        <DefaultValue(True)>
        Public Property CloseButton As Boolean
            Get
                Return DockHandler.CloseButton
            End Get
            Set(value As Boolean)
                DockHandler.CloseButton = value
            End Set
        End Property

        <LocalizedCategory("Category_Docking")>
        <LocalizedDescription("DockContent_CloseButtonVisible_Description")>
        <DefaultValue(True)>
        Public Property CloseButtonVisible As Boolean
            Get
                Return DockHandler.CloseButtonVisible
            End Get
            Set(value As Boolean)
                DockHandler.CloseButtonVisible = value
            End Set
        End Property

        <Browsable(False)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public Property DockPanel As DockPanel
            Get
                Return DockHandler.DockPanel
            End Get
            Set(value As DockPanel)
                DockHandler.DockPanel = value
            End Set
        End Property

        <Browsable(False)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public Property DockState As DockState
            Get
                Return DockHandler.DockState
            End Get
            Set(value As DockState)
                DockHandler.DockState = value
            End Set
        End Property

        <Browsable(False)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public Property Pane As DockPane
            Get
                Return DockHandler.Pane
            End Get
            Set(value As DockPane)
                DockHandler.Pane = value
            End Set
        End Property

        <Browsable(False)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public Property IsHidden As Boolean
            Get
                Return DockHandler.IsHidden
            End Get
            Set(value As Boolean)
                DockHandler.IsHidden = value
            End Set
        End Property

        <Browsable(False)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public Property VisibleState As DockState
            Get
                Return DockHandler.VisibleState
            End Get
            Set(value As DockState)
                DockHandler.VisibleState = value
            End Set
        End Property

        <Browsable(False)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public Property IsFloat As Boolean
            Get
                Return DockHandler.IsFloat
            End Get
            Set(value As Boolean)
                DockHandler.IsFloat = value
            End Set
        End Property

        <Browsable(False)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public Property PanelPane As DockPane
            Get
                Return DockHandler.PanelPane
            End Get
            Set(value As DockPane)
                DockHandler.PanelPane = value
            End Set
        End Property

        <Browsable(False)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public Property FloatPane As DockPane
            Get
                Return DockHandler.FloatPane
            End Get
            Set(value As DockPane)
                DockHandler.FloatPane = value
            End Set
        End Property

        <SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")>
        Protected Overridable Function GetPersistString() As String
            Return [GetType]().ToString()
        End Function

        <LocalizedCategory("Category_Docking")>
        <LocalizedDescription("DockContent_HideOnClose_Description")>
        <DefaultValue(False)>
        Public Property HideOnClose As Boolean
            Get
                Return DockHandler.HideOnClose
            End Get
            Set(value As Boolean)
                DockHandler.HideOnClose = value
            End Set
        End Property

        <LocalizedCategory("Category_Docking")>
        <LocalizedDescription("DockContent_ShowHint_Description")>
        <DefaultValue(DockState.Unknown)>
        Public Property ShowHint As DockState
            Get
                Return DockHandler.ShowHint
            End Get
            Set(value As DockState)
                DockHandler.ShowHint = value
            End Set
        End Property

        <Browsable(False)>
        Public ReadOnly Property IsActivated As Boolean
            Get
                Return DockHandler.IsActivated
            End Get
        End Property

        Public Function IsDockStateValid(dockState As DockState) As Boolean
            Return DockHandler.IsDockStateValid(dockState)
        End Function

#If NET35 Or NET40
        /// <summary>
        /// Context menu.
        /// </summary>
        /// <remarks>
        /// This property should be obsolete as it does not support theming. Please use <see cref="TabPageContextMenuStrip"/> instead.
        /// </remarks>
        [LocalizedCategory("Category_Docking")]
        [LocalizedDescription("DockContent_TabPageContextMenu_Description")]
        [DefaultValue(null)]
        public ContextMenu TabPageContextMenu
        {
            get { return DockHandler.TabPageContextMenu; }
            set { DockHandler.TabPageContextMenu = value; }
        }
#End If
        ''' <summary>
        ''' Context menu strip.
        ''' </summary>
        <LocalizedCategory("Category_Docking")>
        <LocalizedDescription("DockContent_TabPageContextMenuStrip_Description")>
        Public Property TabPageContextMenuStrip As ContextMenuStrip
            Get
                Return DockHandler.TabPageContextMenuStrip
            End Get
            Set(value As ContextMenuStrip)
                DockHandler.TabPageContextMenuStrip = value
            End Set
        End Property

        Private Sub ApplyTheme() Implements IContextMenuStripHost.ApplyTheme
            DockHandler.ApplyTheme()

            If DockPanel IsNot Nothing Then
                If MainMenuStrip IsNot Nothing Then DockPanel.Theme.ApplyTo(MainMenuStrip)
                If MyBase.ContextMenuStrip IsNot Nothing Then DockPanel.Theme.ApplyTo(MyBase.ContextMenuStrip)
            End If
        End Sub

        <Localizable(True)>
        <Category("Appearance")>
        <LocalizedDescription("DockContent_ToolTipText_Description")>
        Public Property ToolTipText As String
            Get
                Return DockHandler.ToolTipText
            End Get
            Set(value As String)
                DockHandler.ToolTipText = value
            End Set
        End Property

        Public Overloads Sub Activate()
            DockHandler.Activate()
        End Sub

        Public Overloads Sub Hide()
            DockHandler.Hide()
        End Sub

        Public Overloads Sub Show()
            DockHandler.Show()
        End Sub

        Public Overloads Sub Show(dockPanel As DockPanel)
            DockHandler.Show(dockPanel)
        End Sub

        Public Overloads Sub Show(dockPanel As DockPanel, dockState As DockState)
            DockHandler.Show(dockPanel, dockState)
        End Sub

        <SuppressMessage("Microsoft.Naming", "CA1720:AvoidTypeNamesInParameters")>
        Public Overloads Sub Show(dockPanel As DockPanel, floatWindowBounds As Rectangle)
            DockHandler.Show(dockPanel, floatWindowBounds)
        End Sub

        Public Overloads Sub Show(pane As DockPane, beforeContent As IDockContent)
            DockHandler.Show(pane, beforeContent)
        End Sub

        Public Overloads Sub Show(previousPane As DockPane, alignment As DockAlignment, proportion As Double)
            DockHandler.Show(previousPane, alignment, proportion)
        End Sub

        <SuppressMessage("Microsoft.Naming", "CA1720:AvoidTypeNamesInParameters")>
        Public Sub FloatAt(floatWindowBounds As Rectangle)
            DockHandler.FloatAt(floatWindowBounds)
        End Sub

        Public Sub DockTo(paneTo As DockPane, dockStyle As DockStyle, contentIndex As Integer)
            DockHandler.DockTo(paneTo, dockStyle, contentIndex)
        End Sub

        Public Sub DockTo(panel As DockPanel, dockStyle As DockStyle)
            DockHandler.DockTo(panel, dockStyle)
        End Sub

#Region "IDockContent Members"
        Private Overloads Sub OnActivated(e As EventArgs) Implements IDockContent.OnActivated
            MyBase.OnActivated(e)
        End Sub

        Private Overloads Sub OnDeactivate(e As EventArgs) Implements IDockContent.OnDeactivate
            MyBase.OnDeactivate(e)
        End Sub
#End Region

#Region "Events"
        Private Sub DockHandler_DockStateChanged(sender As Object, e As EventArgs)
            OnDockStateChanged(e)
        End Sub

        Private Shared ReadOnly DockStateChangedEvent As Object = New Object()
        <LocalizedCategory("Category_PropertyChanged")>
        <LocalizedDescription("Pane_DockStateChanged_Description")>
        Public Custom Event DockStateChanged As EventHandler
            AddHandler(value As EventHandler)
                Events.AddHandler(DockStateChangedEvent, value)
            End AddHandler
            RemoveHandler(value As EventHandler)
                Events.RemoveHandler(DockStateChangedEvent, value)
            End RemoveHandler
            <LocalizedCategory("Category_PropertyChanged")>
            <LocalizedDescription("Pane_DockStateChanged_Description")>
            RaiseEvent(sender As Object, e As EventArgs)
            End RaiseEvent
        End Event
        Protected Overridable Sub OnDockStateChanged(e As EventArgs)
            CType(Events(DockStateChangedEvent), EventHandler)?.Invoke(Me, e)
        End Sub
#End Region

        ''' <summary>
        ''' Overridden to avoid resize issues with nested controls
        ''' </summary>
        ''' <remarks>
        ''' http://blogs.msdn.com/b/alejacma/archive/2008/11/20/controls-won-t-get-resized-once-the-nesting-hierarchy-of-windows-exceeds-a-certain-depth-x64.aspx
        ''' http://support.microsoft.com/kb/953934
        ''' </remarks>
        Protected Overrides Sub OnSizeChanged(e As EventArgs)
            If DockPanel IsNot Nothing AndAlso DockPanel.SupportDeeplyNestedContent AndAlso IsHandleCreated Then
                BeginInvoke(CType(Sub() MyBase.OnSizeChanged(e), MethodInvoker))
            Else
                MyBase.OnSizeChanged(e)
            End If
        End Sub
    End Class
End Namespace
