Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Security.Permissions
Imports System.Diagnostics.CodeAnalysis

Namespace Docking
    <ToolboxItem(False)>
    Public Partial Class DockPane
        Inherits UserControl
        Implements IDockDragSource
        Public Enum AppearanceStyle
            ToolWindow
            Document
        End Enum

        Private Enum HitTestArea
            Caption
            TabStrip
            Content
            None
        End Enum

        Private Structure HitTestResult
            Public HitArea As HitTestArea
            Public Index As Integer

            Public Sub New(hitTestArea As HitTestArea, index As Integer)
                HitArea = hitTestArea
                Me.Index = index
            End Sub
        End Structure

        Private m_captionControl As DockPaneCaptionBase
        Private ReadOnly Property CaptionControl As DockPaneCaptionBase
            Get
                Return m_captionControl
            End Get
        End Property

        Private m_tabStripControl As DockPaneStripBase

        Public ReadOnly Property TabStripControl As DockPaneStripBase
            Get
                Return m_tabStripControl
            End Get
        End Property

        Friend Protected Sub New(content As IDockContent, visibleState As DockState, show As Boolean)
            InternalConstruct(content, visibleState, False, Rectangle.Empty, Nothing, DockAlignment.Right, 0.5, show)
        End Sub

        <SuppressMessage("Microsoft.Naming", "CA1720:AvoidTypeNamesInParameters", MessageId:="1#")>
        Friend Protected Sub New(content As IDockContent, floatWindow As FloatWindow, show As Boolean)
            If floatWindow Is Nothing Then Throw New ArgumentNullException(NameOf(floatWindow))

            InternalConstruct(content, DockState.Float, False, Rectangle.Empty, floatWindow.NestedPanes.GetDefaultPreviousPane(Me), DockAlignment.Right, 0.5, show)
        End Sub

        Friend Protected Sub New(content As IDockContent, previousPane As DockPane, alignment As DockAlignment, proportion As Double, show As Boolean)
            If previousPane Is Nothing Then Throw (New ArgumentNullException(NameOf(previousPane)))
            InternalConstruct(content, previousPane.DockState, False, Rectangle.Empty, previousPane, alignment, proportion, show)
        End Sub

        <SuppressMessage("Microsoft.Naming", "CA1720:AvoidTypeNamesInParameters", MessageId:="1#")>
        Friend Protected Sub New(content As IDockContent, floatWindowBounds As Rectangle, show As Boolean)
            InternalConstruct(content, DockState.Float, True, floatWindowBounds, Nothing, DockAlignment.Right, 0.5, show)
        End Sub

        Private Sub InternalConstruct(content As IDockContent, dockState As DockState, flagBounds As Boolean, floatWindowBounds As Rectangle, prevPane As DockPane, alignment As DockAlignment, proportion As Double, show As Boolean)
            If dockState = DockState.Hidden OrElse dockState = DockState.Unknown Then Throw New ArgumentException(Strings.DockPane_SetDockState_InvalidState)

            If content Is Nothing Then Throw New ArgumentNullException(Strings.DockPane_Constructor_NullContent)

            If content.DockHandler.DockPanel Is Nothing Then Throw New ArgumentException(Strings.DockPane_Constructor_NullDockPanel)

            SuspendLayout()
            SetStyle(ControlStyles.Selectable, False)

            m_isFloat = dockState = DockState.Float

            m_contents = New DockContentCollection()
            m_displayingContents = New DockContentCollection(Me)
            m_dockPanel = content.DockHandler.DockPanel
            m_dockPanel.AddPane(Me)

            m_splitter = content.DockHandler.DockPanel.Theme.Extender.DockPaneSplitterControlFactory.CreateSplitterControl(Me)

            m_nestedDockingStatus = New NestedDockingStatus(Me)

            m_captionControl = DockPanel.Theme.Extender.DockPaneCaptionFactory.CreateDockPaneCaption(Me)
            m_tabStripControl = DockPanel.Theme.Extender.DockPaneStripFactory.CreateDockPaneStrip(Me)
            Controls.AddRange(New Control() {m_captionControl, m_tabStripControl})

            DockPanel.SuspendLayout(True)
            If flagBounds Then
                FloatWindow = DockPanel.Theme.Extender.FloatWindowFactory.CreateFloatWindow(DockPanel, Me, floatWindowBounds)
            ElseIf prevPane IsNot Nothing Then
                DockTo(prevPane.NestedPanesContainer, prevPane, alignment, proportion)
            End If

            SetDockState(dockState)
            If show Then
                content.DockHandler.Pane = Me
            ElseIf IsFloat Then
                content.DockHandler.FloatPane = Me
            Else
                content.DockHandler.PanelPane = Me
            End If

            ResumeLayout()
            DockPanel.ResumeLayout(True, True)
        End Sub

        Private m_isDisposing As Boolean

        Protected Overrides Sub Dispose(disposing As Boolean)
            If disposing Then
                ' IMPORTANT: avoid nested call into this method on Mono. 
                ' https://github.com/dockpanelsuite/dockpanelsuite/issues/16
                If IsRunningOnMono Then
                    If m_isDisposing Then Return

                    m_isDisposing = True
                End If

                m_dockState = DockState.Unknown

                If NestedPanesContainer IsNot Nothing Then NestedPanesContainer.NestedPanes.Remove(Me)

                If DockPanel IsNot Nothing Then
                    DockPanel.RemovePane(Me)
                    m_dockPanel = Nothing
                End If

                Splitter.Dispose()
                If m_autoHidePane IsNot Nothing Then m_autoHidePane.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private m_activeContent As IDockContent = Nothing
        Public Overridable Property ActiveContent As IDockContent
            Get
                Return m_activeContent
            End Get
            Set(value As IDockContent)
                If ActiveContent Is value Then Return

                If value IsNot Nothing Then
                    If Not DisplayingContents.Contains(value) Then Throw (New InvalidOperationException(Strings.DockPane_ActiveContent_InvalidValue))
                Else
                    If DisplayingContents.Count <> 0 Then Throw (New InvalidOperationException(Strings.DockPane_ActiveContent_InvalidValue))
                End If

                Dim oldValue = m_activeContent

                If DockPanel.ActiveAutoHideContent Is oldValue Then DockPanel.ActiveAutoHideContent = Nothing

                m_activeContent = value

                If DockPanel.DocumentStyle = DocumentStyle.DockingMdi AndAlso DockState = DockState.Document Then
                    If m_activeContent IsNot Nothing Then m_activeContent.DockHandler.Form.BringToFront()
                Else
                    If m_activeContent IsNot Nothing Then m_activeContent.DockHandler.SetVisible()
                    If oldValue IsNot Nothing AndAlso DisplayingContents.Contains(oldValue) Then oldValue.DockHandler.SetVisible()
                    If IsActivated AndAlso m_activeContent IsNot Nothing Then m_activeContent.DockHandler.Activate()
                End If

                If FloatWindow IsNot Nothing Then FloatWindow.SetText()

                If DockPanel.DocumentStyle = DocumentStyle.DockingMdi AndAlso DockState = DockState.Document Then
                    RefreshChanges(False)  ' delayed layout to reduce screen flicker
                Else
                    RefreshChanges()
                End If

                If m_activeContent IsNot Nothing Then TabStripControl.EnsureTabVisible(m_activeContent)
            End Set
        End Property

        Friend Sub ClearLastActiveContent()
            m_activeContent = Nothing
        End Sub

        Private m_allowDockDragAndDrop As Boolean = True
        Public Overridable Property AllowDockDragAndDrop As Boolean
            Get
                Return m_allowDockDragAndDrop
            End Get
            Set(value As Boolean)
                m_allowDockDragAndDrop = value
            End Set
        End Property

        Private m_autoHidePane As IDisposable = Nothing
        Friend Property AutoHidePane As IDisposable
            Get
                Return m_autoHidePane
            End Get
            Set(value As IDisposable)
                m_autoHidePane = value
            End Set
        End Property

        Private m_autoHideTabs As Object = Nothing
        Friend Property AutoHideTabs As Object
            Get
                Return m_autoHideTabs
            End Get
            Set(value As Object)
                m_autoHideTabs = value
            End Set
        End Property

        Private ReadOnly Property TabPageContextMenu As Object
            Get
                Dim content = ActiveContent

                If content Is Nothing Then Return Nothing

                If content.DockHandler.TabPageContextMenuStrip IsNot Nothing Then
#If NET35 Or NET40
                else if (content.DockHandler.TabPageContextMenu != null)
                    return content.DockHandler.TabPageContextMenu;
#End If
                    Return content.DockHandler.TabPageContextMenuStrip
                Else
                    Return Nothing
                End If
            End Get
        End Property

        Friend ReadOnly Property HasTabPageContextMenu As Boolean
            Get
                Return TabPageContextMenu IsNot Nothing
            End Get
        End Property

        Friend Sub ShowTabPageContextMenu(control As Control, position As Point)
            Dim menu = TabPageContextMenu

            If menu Is Nothing Then Return

            Dim contextMenuStrip As ContextMenuStrip = TryCast(menu, ContextMenuStrip)
            If contextMenuStrip IsNot Nothing Then
                contextMenuStrip.Show(control, position)
                Return
            End If
#If NET35 Or NET40
            ContextMenu contextMenu = menu as ContextMenu;
            if (contextMenu != null)
                contextMenu.Show(this, position);
#End If
        End Sub

        Private ReadOnly Property CaptionRectangle As Rectangle
            Get
                If Not HasCaption Then Return Rectangle.Empty

                Dim rectWindow = DisplayingRectangle
                Dim x, y, width As Integer
                x = rectWindow.X
                y = rectWindow.Y
                width = rectWindow.Width
                Dim height As Integer = CaptionControl.MeasureHeight()

                Return New Rectangle(x, y, width, height)
            End Get
        End Property

        Friend Protected Overridable ReadOnly Property ContentRectangle As Rectangle
            Get
                Dim rectWindow = DisplayingRectangle
                Dim rectCaption = CaptionRectangle
                Dim rectTabStrip = TabStripRectangle

                Dim x = rectWindow.X

                Dim y = rectWindow.Y + If(rectCaption.IsEmpty, 0, rectCaption.Height)
                If DockState = DockState.Document AndAlso DockPanel.DocumentTabStripLocation = DocumentTabStripLocation.Top Then y += rectTabStrip.Height

                Dim width = rectWindow.Width
                Dim height = rectWindow.Height - rectCaption.Height - rectTabStrip.Height

                Return New Rectangle(x, y, width, height)
            End Get
        End Property

        Friend ReadOnly Property TabStripRectangle As Rectangle
            Get
                If Appearance = AppearanceStyle.ToolWindow Then
                    Return TabStripRectangle_ToolWindow
                Else
                    Return TabStripRectangle_Document
                End If
            End Get
        End Property

        Private ReadOnly Property TabStripRectangle_ToolWindow As Rectangle
            Get
                If DisplayingContents.Count <= 1 OrElse IsAutoHide Then Return Rectangle.Empty

                Dim rectWindow = DisplayingRectangle

                Dim width = rectWindow.Width
                Dim height As Integer = TabStripControl.MeasureHeight()
                Dim x = rectWindow.X
                Dim y = rectWindow.Bottom - height
                Dim rectCaption = CaptionRectangle
                If rectCaption.Contains(x, y) Then y = rectCaption.Y + rectCaption.Height

                Return New Rectangle(x, y, width, height)
            End Get
        End Property

        Private ReadOnly Property TabStripRectangle_Document As Rectangle
            Get
                If DisplayingContents.Count = 0 Then Return Rectangle.Empty

                If DisplayingContents.Count = 1 AndAlso DockPanel.DocumentStyle = DocumentStyle.DockingSdi Then Return Rectangle.Empty

                Dim rectWindow = DisplayingRectangle
                Dim x = rectWindow.X
                Dim width = rectWindow.Width
                Dim height As Integer = TabStripControl.MeasureHeight()

                Dim y = 0
                If DockPanel.DocumentTabStripLocation = DocumentTabStripLocation.Bottom Then
                    y = rectWindow.Height - height
                Else
                    y = rectWindow.Y
                End If

                Return New Rectangle(x, y, width, height)
            End Get
        End Property

        Public Overridable ReadOnly Property CaptionText As String
            Get
                Return If(ActiveContent Is Nothing, String.Empty, ActiveContent.DockHandler.TabText)
            End Get
        End Property

        Private m_contents As DockContentCollection
        Public ReadOnly Property Contents As DockContentCollection
            Get
                Return m_contents
            End Get
        End Property

        Private m_displayingContents As DockContentCollection
        Public ReadOnly Property DisplayingContents As DockContentCollection
            Get
                Return m_displayingContents
            End Get
        End Property

        Private m_dockPanel As DockPanel
        Public ReadOnly Property DockPanel As DockPanel
            Get
                Return m_dockPanel
            End Get
        End Property

        Private ReadOnly Property HasCaption As Boolean
            Get
                If DockState = DockState.Document OrElse DockState = DockState.Hidden OrElse DockState = DockState.Unknown OrElse DockState = DockState.Float AndAlso FloatWindow.VisibleNestedPanes.Count <= 1 Then
                    Return False
                Else
                    Return True
                End If
            End Get
        End Property

        Private m_isActivated As Boolean = False
        Public ReadOnly Property IsActivated As Boolean
            Get
                Return m_isActivated
            End Get
        End Property
        Friend Sub SetIsActivated(value As Boolean)
            If m_isActivated = value Then Return

            m_isActivated = value
            If DockState <> DockState.Document Then RefreshChanges(False)
            OnIsActivatedChanged(EventArgs.Empty)
        End Sub

        Private m_isActiveDocumentPane As Boolean = False
        Public ReadOnly Property IsActiveDocumentPane As Boolean
            Get
                Return m_isActiveDocumentPane
            End Get
        End Property

        Friend Sub SetIsActiveDocumentPane(value As Boolean)
            If m_isActiveDocumentPane = value Then Return

            m_isActiveDocumentPane = value
            If DockState = DockState.Document Then RefreshChanges()
            OnIsActiveDocumentPaneChanged(EventArgs.Empty)
        End Sub

        Public ReadOnly Property IsActivePane As Boolean
            Get
                Return Me Is DockPanel.ActivePane
            End Get
        End Property

        Public Function IsDockStateValid(dockState As DockState) As Boolean
            For Each content In Contents
                If Not content.DockHandler.IsDockStateValid(dockState) Then Return False
            Next

            Return True
        End Function

        Public ReadOnly Property IsAutoHide As Boolean
            Get
                Return IsDockStateAutoHide(DockState)
            End Get
        End Property

        Public ReadOnly Property Appearance As AppearanceStyle
            Get
                Return If(DockState = DockState.Document, AppearanceStyle.Document, AppearanceStyle.ToolWindow)
            End Get
        End Property

        Public ReadOnly Property DisplayingRectangle As Rectangle
            Get
                Return ClientRectangle
            End Get
        End Property

        Public Sub Activate()
            If IsDockStateAutoHide(DockState) AndAlso DockPanel.ActiveAutoHideContent IsNot ActiveContent Then
                DockPanel.ActiveAutoHideContent = ActiveContent
            ElseIf Not IsActivated AndAlso ActiveContent IsNot Nothing Then
                ActiveContent.DockHandler.Activate()
            End If
        End Sub

        Friend Sub AddContent(content As IDockContent)
            If Contents.Contains(content) Then Return

            Contents.Add(content)
        End Sub

        Friend Sub Close()
            Dispose()
        End Sub

        Public Sub CloseActiveContent()
            CloseContent(ActiveContent)
        End Sub

        Friend Sub CloseContent(content As IDockContent)
            If content Is Nothing Then Return

            If Not content.DockHandler.CloseButton Then Return

            Dim dockPanel = Me.DockPanel

            dockPanel.SuspendLayout(True)

            Try
                If content.DockHandler.HideOnClose Then
                    content.DockHandler.Hide()
                    NestedDockingStatus.NestedPanes.SwitchPaneWithFirstChild(Me)
                Else
                    content.DockHandler.Close()
                    ' TODO: fix layout here for #519
                End If

            Finally
                dockPanel.ResumeLayout(True, True)
            End Try
        End Sub

        Private Function GetHitTest(ptMouse As Point) As HitTestResult
            Dim ptMouseClient = PointToClient(ptMouse)

            Dim rectCaption = CaptionRectangle
            If rectCaption.Contains(ptMouseClient) Then Return New HitTestResult(HitTestArea.Caption, -1)

            Dim rectContent = ContentRectangle
            If rectContent.Contains(ptMouseClient) Then Return New HitTestResult(HitTestArea.Content, -1)

            Dim rectTabStrip = TabStripRectangle
            If rectTabStrip.Contains(ptMouseClient) Then Return New HitTestResult(HitTestArea.TabStrip, TabStripControl.HitTest(TabStripControl.PointToClient(ptMouse)))

            Return New HitTestResult(HitTestArea.None, -1)
        End Function

        Private m_isHidden As Boolean = True
        Public ReadOnly Property IsHidden As Boolean
            Get
                Return m_isHidden
            End Get
        End Property
        Private Sub SetIsHidden(value As Boolean)
            If m_isHidden = value Then Return

            m_isHidden = value
            If IsDockStateAutoHide(DockState) Then
                DockPanel.RefreshAutoHideStrip()
                DockPanel.PerformLayout()
            ElseIf NestedPanesContainer IsNot Nothing Then
                CType(NestedPanesContainer, Control).PerformLayout()
            End If
        End Sub

        Protected Overrides Sub OnLayout(e As LayoutEventArgs)
            SetIsHidden(DisplayingContents.Count = 0)
            If Not IsHidden Then
                CaptionControl.Bounds = CaptionRectangle
                TabStripControl.Bounds = TabStripRectangle

                SetContentBounds()

                For Each content In Contents
                    If DisplayingContents.Contains(content) Then
                        If content.DockHandler.FlagClipWindow AndAlso content.DockHandler.Form.Visible Then content.DockHandler.FlagClipWindow = False
                    End If
                Next
            End If

            MyBase.OnLayout(e)
        End Sub

        Friend Sub SetContentBounds()
            Dim rectContent = ContentRectangle
            If DockState = DockState.Document AndAlso DockPanel.DocumentStyle = DocumentStyle.DockingMdi Then rectContent = DockPanel.RectangleToMdiClient(RectangleToScreen(rectContent))

            Dim rectInactive As Rectangle = New Rectangle(-rectContent.Width, rectContent.Y, rectContent.Width, rectContent.Height)
            For Each content In Contents
                If content.DockHandler.Pane Is Me Then
                    If content Is ActiveContent Then
                        content.DockHandler.Form.Bounds = rectContent
                    Else
                        content.DockHandler.Form.Bounds = rectInactive
                    End If
                End If
            Next
        End Sub

        Friend Sub RefreshChanges()
            RefreshChanges(True)
        End Sub

        Private Sub RefreshChanges(performLayout As Boolean)
            If IsDisposed Then Return

            CaptionControl.RefreshChanges()
            TabStripControl.RefreshChanges()
            If DockState = DockState.Float AndAlso FloatWindow IsNot Nothing Then FloatWindow.RefreshChanges()
            If IsDockStateAutoHide(DockState) AndAlso DockPanel IsNot Nothing Then
                DockPanel.RefreshAutoHideStrip()
                DockPanel.PerformLayout()
            End If

            If performLayout Then MyBase.PerformLayout()
        End Sub

        Friend Sub RemoveContent(content As IDockContent)
            If Not Contents.Contains(content) Then Return

            Contents.Remove(content)
        End Sub

        Public Sub SetContentIndex(content As IDockContent, index As Integer)
            Dim oldIndex = Contents.IndexOf(content)
            If oldIndex = -1 Then Throw (New ArgumentException(Strings.DockPane_SetContentIndex_InvalidContent))

            If index < 0 OrElse index > Contents.Count - 1 Then
                If index <> -1 Then Throw (New ArgumentOutOfRangeException(Strings.DockPane_SetContentIndex_InvalidIndex))
            End If

            If oldIndex = index Then Return
            If oldIndex = Contents.Count - 1 AndAlso index = -1 Then Return

            Contents.Remove(content)
            If index = -1 Then
                Contents.Add(content)
            ElseIf oldIndex < index Then
                Contents.AddAt(content, index - 1)
            Else
                Contents.AddAt(content, index)
            End If

            RefreshChanges()
        End Sub

        Private Sub SetParent()
            If DockState = DockState.Unknown OrElse DockState = DockState.Hidden Then
                SetParent(Nothing)
                Splitter.Parent = Nothing
            ElseIf DockState = DockState.Float Then
                SetParent(FloatWindow)
                Splitter.Parent = FloatWindow
            ElseIf IsDockStateAutoHide(DockState) Then
                SetParent(DockPanel.AutoHideControl)
                Splitter.Parent = Nothing
            Else
                SetParent(DockPanel.DockWindows(DockState))
                Splitter.Parent = Parent
            End If
        End Sub

        Private Sub SetParent(value As Control)
            If Parent Is value Then Return

            '!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            ' Workaround of .Net Framework bug:
            ' Change the parent of a control with focus may result in the first
            ' MDI child form get activated. 
            '!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            Dim contentFocused As IDockContent = GetFocusedContent()
            If contentFocused IsNot Nothing Then DockPanel.SaveFocus()

            '!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

            Parent = value

            '!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            ' Workaround of .Net Framework bug:
            ' Change the parent of a control with focus may result in the first
            ' MDI child form get activated. 
            '!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            If contentFocused IsNot Nothing Then contentFocused.DockHandler.Activate()
            '!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        End Sub

        Public Overloads Sub Show()
            Activate()
        End Sub

        Friend Sub TestDrop(dragSource As IDockDragSource, dockOutline As DockOutlineBase)
            If Not dragSource.CanDockTo(Me) Then Return

            Dim ptMouse = MousePosition

            Dim hitTestResult = GetHitTest(ptMouse)
            If hitTestResult.HitArea = HitTestArea.Caption Then
                dockOutline.Show(Me, -1)
            ElseIf hitTestResult.HitArea = HitTestArea.TabStrip AndAlso hitTestResult.Index <> -1 Then
                dockOutline.Show(Me, hitTestResult.Index)
            End If
        End Sub

        Friend Sub ValidateActiveContent()
            If ActiveContent Is Nothing Then
                If DisplayingContents.Count <> 0 Then ActiveContent = DisplayingContents(0)
                Return
            End If

            If DisplayingContents.IndexOf(ActiveContent) >= 0 Then Return

            Dim prevVisible As IDockContent = Nothing
            For i = Contents.IndexOf(ActiveContent) - 1 To 0 Step -1
                If Contents(i).DockHandler.DockState = DockState Then
                    prevVisible = Contents(i)
                    Exit For
                End If
            Next

            Dim nextVisible As IDockContent = Nothing
            For i = Contents.IndexOf(ActiveContent) + 1 To Contents.Count - 1
                If Contents(i).DockHandler.DockState = DockState Then
                    nextVisible = Contents(i)
                    Exit For
                End If
            Next

            If prevVisible IsNot Nothing Then
                ActiveContent = prevVisible
            ElseIf nextVisible IsNot Nothing Then
                ActiveContent = nextVisible
            Else
                ActiveContent = Nothing
            End If
        End Sub

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

        Private Shared ReadOnly IsActivatedChangedEvent As Object = New Object()
        Public Custom Event IsActivatedChanged As EventHandler
            AddHandler(value As EventHandler)
                Events.AddHandler(IsActivatedChangedEvent, value)
            End AddHandler
            RemoveHandler(value As EventHandler)
                Events.RemoveHandler(IsActivatedChangedEvent, value)
            End RemoveHandler
            RaiseEvent(sender As Object, e As EventArgs)
            End RaiseEvent
        End Event
        Protected Overridable Sub OnIsActivatedChanged(e As EventArgs)
            Dim handler = CType(Events(IsActivatedChangedEvent), EventHandler)
            If handler IsNot Nothing Then handler(Me, e)
        End Sub

        Private Shared ReadOnly IsActiveDocumentPaneChangedEvent As Object = New Object()
        Public Custom Event IsActiveDocumentPaneChanged As EventHandler
            AddHandler(value As EventHandler)
                Events.AddHandler(IsActiveDocumentPaneChangedEvent, value)
            End AddHandler
            RemoveHandler(value As EventHandler)
                Events.RemoveHandler(IsActiveDocumentPaneChangedEvent, value)
            End RemoveHandler
            RaiseEvent(sender As Object, e As EventArgs)
            End RaiseEvent
        End Event
        Protected Overridable Sub OnIsActiveDocumentPaneChanged(e As EventArgs)
            Dim handler = CType(Events(IsActiveDocumentPaneChangedEvent), EventHandler)
            If handler IsNot Nothing Then handler(Me, e)
        End Sub

        Public Property DockWindow As DockWindow
            Get
                Return If(m_nestedDockingStatus.NestedPanes Is Nothing, Nothing, TryCast(m_nestedDockingStatus.NestedPanes.Container, DockWindow))
            End Get
            Set(value As DockWindow)
                Dim oldValue = DockWindow
                If oldValue Is value Then Return

                DockTo(value)
            End Set
        End Property

        Public Property FloatWindow As FloatWindow
            Get
                Return If(m_nestedDockingStatus.NestedPanes Is Nothing, Nothing, TryCast(m_nestedDockingStatus.NestedPanes.Container, FloatWindow))
            End Get
            Set(value As FloatWindow)
                Dim oldValue = FloatWindow
                If oldValue Is value Then Return

                DockTo(value)
            End Set
        End Property

        Private m_nestedDockingStatus As NestedDockingStatus
        Public ReadOnly Property NestedDockingStatus As NestedDockingStatus
            Get
                Return m_nestedDockingStatus
            End Get
        End Property

        Private m_isFloat As Boolean
        Public ReadOnly Property IsFloat As Boolean
            Get
                Return m_isFloat
            End Get
        End Property

        Public ReadOnly Property NestedPanesContainer As INestedPanesContainer
            Get
                If NestedDockingStatus.NestedPanes Is Nothing Then
                    Return Nothing
                Else
                    Return NestedDockingStatus.NestedPanes.Container
                End If
            End Get
        End Property

        Private m_dockState As DockState = DockState.Unknown
        Public Property DockState As DockState
            Get
                Return m_dockState
            End Get
            Set(value As DockState)
                SetDockState(value)
            End Set
        End Property

        Public Function SetDockState(value As DockState) As DockPane
            If value = DockState.Unknown OrElse value = DockState.Hidden Then Throw New InvalidOperationException(Strings.DockPane_SetDockState_InvalidState)

            If value = DockState.Float = IsFloat Then
                InternalSetDockState(value)
                Return Me
            End If

            If DisplayingContents.Count = 0 Then Return Nothing

            Dim firstContent As IDockContent = Nothing
            For i = 0 To DisplayingContents.Count - 1
                Dim content = DisplayingContents(i)
                If content.DockHandler.IsDockStateValid(value) Then
                    firstContent = content
                    Exit For
                End If
            Next
            If firstContent Is Nothing Then Return Nothing

            firstContent.DockHandler.DockState = value
            Dim pane = firstContent.DockHandler.Pane
            DockPanel.SuspendLayout(True)
            For i = 0 To DisplayingContents.Count - 1
                Dim content = DisplayingContents(i)
                If content.DockHandler.IsDockStateValid(value) Then content.DockHandler.Pane = pane
            Next
            DockPanel.ResumeLayout(True, True)
            Return pane
        End Function

        Private Sub InternalSetDockState(value As DockState)
            If m_dockState = value Then Return

            Dim oldDockState = m_dockState
            Dim oldContainer = NestedPanesContainer

            m_dockState = value

            SuspendRefreshStateChange()

            Dim contentFocused As IDockContent = GetFocusedContent()
            If contentFocused IsNot Nothing Then DockPanel.SaveFocus()

            If Not IsFloat Then
                DockWindow = DockPanel.DockWindows(DockState)
            ElseIf FloatWindow Is Nothing Then
                FloatWindow = DockPanel.Theme.Extender.FloatWindowFactory.CreateFloatWindow(DockPanel, Me)
            End If

            If contentFocused IsNot Nothing Then
                If Not IsRunningOnMono Then
                    DockPanel.ContentFocusManager.Activate(contentFocused)
                End If
            End If

            ResumeRefreshStateChange(oldContainer, oldDockState)
        End Sub

        Private m_countRefreshStateChange As Integer = 0
        Private Sub SuspendRefreshStateChange()
            m_countRefreshStateChange += 1
            DockPanel.SuspendLayout(True)
        End Sub

        Private Sub ResumeRefreshStateChange()
            m_countRefreshStateChange -= 1
            Diagnostics.Debug.Assert(m_countRefreshStateChange >= 0)
            DockPanel.ResumeLayout(True, True)
        End Sub

        Private ReadOnly Property IsRefreshStateChangeSuspended As Boolean
            Get
                Return m_countRefreshStateChange <> 0
            End Get
        End Property

        Private Sub ResumeRefreshStateChange(oldContainer As INestedPanesContainer, oldDockState As DockState)
            ResumeRefreshStateChange()
            RefreshStateChange(oldContainer, oldDockState)
        End Sub

        Private Sub RefreshStateChange(oldContainer As INestedPanesContainer, oldDockState As DockState)
            If IsRefreshStateChangeSuspended Then Return

            SuspendRefreshStateChange()

            DockPanel.SuspendLayout(True)

            Dim contentFocused As IDockContent = GetFocusedContent()
            If contentFocused IsNot Nothing Then DockPanel.SaveFocus()
            SetParent()

            If ActiveContent IsNot Nothing Then ActiveContent.DockHandler.SetDockState(ActiveContent.DockHandler.IsHidden, DockState, ActiveContent.DockHandler.Pane)
            For Each content In Contents
                If content.DockHandler.Pane Is Me Then content.DockHandler.SetDockState(content.DockHandler.IsHidden, DockState, content.DockHandler.Pane)
            Next

            If oldContainer IsNot Nothing Then
                Dim oldContainerControl = CType(oldContainer, Control)
                If oldContainer.DockState = oldDockState AndAlso Not oldContainerControl.IsDisposed Then oldContainerControl.PerformLayout()
            End If
            If IsDockStateAutoHide(oldDockState) Then DockPanel.RefreshActiveAutoHideContent()

            If NestedPanesContainer.DockState = DockState Then CType(NestedPanesContainer, Control).PerformLayout()
            If IsDockStateAutoHide(DockState) Then DockPanel.RefreshActiveAutoHideContent()

            If IsDockStateAutoHide(oldDockState) OrElse IsDockStateAutoHide(DockState) Then
                DockPanel.RefreshAutoHideStrip()
                DockPanel.PerformLayout()
            End If

            ResumeRefreshStateChange()

            If contentFocused IsNot Nothing Then contentFocused.DockHandler.Activate()

            DockPanel.ResumeLayout(True, True)

            If oldDockState <> DockState Then OnDockStateChanged(EventArgs.Empty)
        End Sub

        Private Function GetFocusedContent() As IDockContent
            Dim contentFocused As IDockContent = Nothing
            For Each content In Contents
                If content.DockHandler.Form.ContainsFocus Then
                    contentFocused = content
                    Exit For
                End If
            Next

            Return contentFocused
        End Function

        Public Function DockTo(container As INestedPanesContainer) As DockPane
            If container Is Nothing Then Throw New InvalidOperationException(Strings.DockPane_DockTo_NullContainer)

            Dim alignment As DockAlignment
            If container.DockState = DockState.DockLeft OrElse container.DockState = DockState.DockRight Then
                alignment = DockAlignment.Bottom
            Else
                alignment = DockAlignment.Right
            End If

            Return DockTo(container, container.NestedPanes.GetDefaultPreviousPane(Me), alignment, 0.5)
        End Function

        Public Function DockTo(container As INestedPanesContainer, previousPane As DockPane, alignment As DockAlignment, proportion As Double) As DockPane
            If container Is Nothing Then Throw New InvalidOperationException(Strings.DockPane_DockTo_NullContainer)

            If container.IsFloat = IsFloat Then
                InternalAddToDockList(container, previousPane, alignment, proportion)
                Return Me
            End If

            Dim firstContent = GetFirstContent(container.DockState)
            If firstContent Is Nothing Then Return Nothing

            Dim pane As DockPane
            DockPanel.DummyContent.DockPanel = DockPanel
            If container.IsFloat Then
                pane = DockPanel.Theme.Extender.DockPaneFactory.CreateDockPane(DockPanel.DummyContent, CType(container, FloatWindow), True)
            Else
                pane = DockPanel.Theme.Extender.DockPaneFactory.CreateDockPane(DockPanel.DummyContent, container.DockState, True)
            End If

            pane.DockTo(container, previousPane, alignment, proportion)
            SetVisibleContentsToPane(pane)
            DockPanel.DummyContent.DockPanel = Nothing

            Return pane
        End Function

        Private Sub SetVisibleContentsToPane(pane As DockPane)
            SetVisibleContentsToPane(pane, ActiveContent)
        End Sub

        Private Sub SetVisibleContentsToPane(pane As DockPane, activeContent As IDockContent)
            For i = 0 To DisplayingContents.Count - 1
                Dim content = DisplayingContents(i)

                If content IsNot Nothing Then
                    If content.DockHandler.IsDockStateValid(pane.DockState) Then
                        content.DockHandler.Pane = pane
                        i -= 1
                    End If
                End If
            Next

            If activeContent.DockHandler.Pane Is pane Then pane.ActiveContent = activeContent
        End Sub

        Private Sub InternalAddToDockList(container As INestedPanesContainer, prevPane As DockPane, alignment As DockAlignment, proportion As Double)
            If container.DockState = DockState.Float <> IsFloat Then Throw New InvalidOperationException(Strings.DockPane_DockTo_InvalidContainer)

            Dim count = container.NestedPanes.Count
            If container.NestedPanes.Contains(Me) Then count -= 1
            If prevPane Is Nothing AndAlso count > 0 Then Throw New InvalidOperationException(Strings.DockPane_DockTo_NullPrevPane)

            If prevPane IsNot Nothing AndAlso Not container.NestedPanes.Contains(prevPane) Then Throw New InvalidOperationException(Strings.DockPane_DockTo_NoPrevPane)

            If prevPane Is Me Then Throw New InvalidOperationException(Strings.DockPane_DockTo_SelfPrevPane)

            Dim oldContainer = NestedPanesContainer
            Dim oldDockState = DockState
            container.NestedPanes.Add(Me)
            NestedDockingStatus.SetStatus(container.NestedPanes, prevPane, alignment, proportion)

            If IsDockWindowState(DockState) Then m_dockState = container.DockState

            RefreshStateChange(oldContainer, oldDockState)
        End Sub

        Public Sub SetNestedDockingProportion(proportion As Double)
            NestedDockingStatus.SetStatus(NestedDockingStatus.NestedPanes, NestedDockingStatus.PreviousPane, NestedDockingStatus.Alignment, proportion)
            If NestedPanesContainer IsNot Nothing Then CType(NestedPanesContainer, Control).PerformLayout()
        End Sub

        Public Function Float() As DockPane
            DockPanel.SuspendLayout(True)

            Dim activeContent = Me.ActiveContent

            Dim floatPane As DockPane = GetFloatPaneFromContents()
            If floatPane Is Nothing Then
                Dim firstContent = GetFirstContent(DockState.Float)
                If firstContent Is Nothing Then
                    DockPanel.ResumeLayout(True, True)
                    Return Nothing
                End If

                floatPane = DockPanel.Theme.Extender.DockPaneFactory.CreateDockPane(firstContent, DockState.Float, True)
            End If

            SetVisibleContentsToPane(floatPane, activeContent)
            If EnableFloatSplitterFix = True Then
                If IsHidden Then
                    NestedDockingStatus.NestedPanes.SwitchPaneWithFirstChild(Me)
                End If
            End If

            DockPanel.ResumeLayout(True, True)
            Return floatPane
        End Function

        Private Function GetFloatPaneFromContents() As DockPane
            Dim floatPane As DockPane = Nothing
            For i = 0 To DisplayingContents.Count - 1
                Dim content = DisplayingContents(i)
                If Not content.DockHandler.IsDockStateValid(DockState.Float) Then Continue For

                If floatPane IsNot Nothing AndAlso content.DockHandler.FloatPane IsNot floatPane Then
                    Return Nothing
                Else
                    floatPane = content.DockHandler.FloatPane
                End If
            Next

            Return floatPane
        End Function

        Private Function GetFirstContent(dockState As DockState) As IDockContent
            For i = 0 To DisplayingContents.Count - 1
                Dim content = DisplayingContents(i)
                If content.DockHandler.IsDockStateValid(dockState) Then Return content
            Next
            Return Nothing
        End Function

        Public Sub RestoreToPanel()
            DockPanel.SuspendLayout(True)

            Dim activeContent = DockPanel.ActiveContent

            For i = DisplayingContents.Count - 1 To 0 Step -1
                Dim content = DisplayingContents(i)
                If content.DockHandler.CheckDockState(False) <> DockState.Unknown Then content.DockHandler.IsFloat = False
            Next

            DockPanel.ResumeLayout(True, True)
        End Sub

        <SecurityPermission(SecurityAction.LinkDemand, Flags:=SecurityPermissionFlag.UnmanagedCode)>
        Protected Overrides Sub WndProc(ByRef m As Message)
            If m.Msg = Win32.Msgs.WM_MOUSEACTIVATE Then Activate()

            MyBase.WndProc(m)
        End Sub

#Region "IDockDragSource Members"

#Region "IDragSource Members"

        Private ReadOnly Property DragControl As Control Implements IDragSource.DragControl
            Get
                Return Me
            End Get
        End Property

        Public Property MouseOverTab As IDockContent

#End Region

        Private Function IsDockStateValid1(dockState As DockState) As Boolean Implements IDockDragSource.IsDockStateValid
            Return IsDockStateValid(dockState)
        End Function

        Private Function CanDockTo(pane As DockPane) As Boolean Implements IDockDragSource.CanDockTo
            If Not IsDockStateValid(pane.DockState) Then Return False

            If pane Is Me Then Return False

            Return True
        End Function

        Private Function BeginDrag(ptMouse As Point) As Rectangle Implements IDockDragSource.BeginDrag
            Dim location As Point = PointToScreen(New Point(0, 0))
            Dim size As Size

            Dim floatPane = ActiveContent.DockHandler.FloatPane
            If DockState = DockState.Float OrElse floatPane Is Nothing OrElse floatPane.FloatWindow.NestedPanes.Count <> 1 Then
                size = DockPanel.DefaultFloatWindowSize
            Else
                size = floatPane.FloatWindow.Size
            End If

            If ptMouse.X > location.X + size.Width Then location.X += ptMouse.X - (location.X + size.Width) + DockPanel.Theme.Measures.SplitterSize

            Return New Rectangle(location, size)
        End Function

        Private Sub EndDrag() Implements IDockDragSource.EndDrag
        End Sub

        Public Sub FloatAt(floatWindowBounds As Rectangle) Implements IDockDragSource.FloatAt
            If FloatWindow Is Nothing OrElse FloatWindow.NestedPanes.Count <> 1 Then
                FloatWindow = DockPanel.Theme.Extender.FloatWindowFactory.CreateFloatWindow(DockPanel, Me, floatWindowBounds)
            Else
                FloatWindow.Bounds = floatWindowBounds
            End If

            DockState = DockState.Float

            NestedDockingStatus.NestedPanes.SwitchPaneWithFirstChild(Me)
        End Sub

        Public Sub DockTo(pane As DockPane, dockStyle As DockStyle, contentIndex As Integer) Implements IDockDragSource.DockTo
            If dockStyle = DockStyle.Fill Then
                Dim activeContent = Me.ActiveContent
                For i = Contents.Count - 1 To 0 Step -1
                    Dim c = Contents(i)
                    If c.DockHandler.DockState = DockState Then
                        c.DockHandler.Pane = pane
                        If contentIndex <> -1 Then pane.SetContentIndex(c, contentIndex)
                    End If
                Next
                pane.ActiveContent = activeContent
            Else
                If dockStyle = DockStyle.Left Then
                    DockTo(pane.NestedPanesContainer, pane, DockAlignment.Left, 0.5)
                ElseIf dockStyle = DockStyle.Right Then
                    DockTo(pane.NestedPanesContainer, pane, DockAlignment.Right, 0.5)
                ElseIf dockStyle = DockStyle.Top Then
                    DockTo(pane.NestedPanesContainer, pane, DockAlignment.Top, 0.5)
                ElseIf dockStyle = DockStyle.Bottom Then
                    DockTo(pane.NestedPanesContainer, pane, DockAlignment.Bottom, 0.5)
                End If

                DockState = pane.DockState
            End If
        End Sub

        Public Sub DockTo(panel As DockPanel, dockStyle As DockStyle) Implements IDockDragSource.DockTo
            If panel IsNot DockPanel Then Throw New ArgumentException(Strings.IDockDragSource_DockTo_InvalidPanel, NameOf(panel))

            If dockStyle = DockStyle.Top Then
                DockState = DockState.DockTop
            ElseIf dockStyle = DockStyle.Bottom Then
                DockState = DockState.DockBottom
            ElseIf dockStyle = DockStyle.Left Then
                DockState = DockState.DockLeft
            ElseIf dockStyle = DockStyle.Right Then
                DockState = DockState.DockRight
            ElseIf dockStyle = DockStyle.Fill Then
                DockState = DockState.Document
            End If
        End Sub

#End Region

#Region "cachedLayoutArgs leak workaround"

        ''' <summary>
        ''' There's a bug in the WinForms layout engine
        ''' that can result in a deferred layout to not
        ''' properly clear out the cached layout args after
        ''' the layout operation is performed.
        ''' Specifically, this bug is hit when the bounds of
        ''' the Pane change, initiating a layout on the parent
        ''' (DockWindow) which is where the bug hits.
        ''' To work around it, when a pane loses the DockWindow
        ''' as its parent, that parent DockWindow needs to
        ''' perform a layout to flush the cached args, if they exist.
        ''' </summary>
        Private _lastParentWindow As DockWindow
        Protected Overrides Sub OnParentChanged(e As EventArgs)
            MyBase.OnParentChanged(e)
            Dim newParent = TryCast(Parent, DockWindow)
            If newParent IsNot _lastParentWindow Then
                If _lastParentWindow IsNot Nothing Then _lastParentWindow.PerformLayout()
                _lastParentWindow = newParent
            End If
        End Sub
#End Region
    End Class
End Namespace
