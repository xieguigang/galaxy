Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Security.Permissions
Imports System.Diagnostics.CodeAnalysis

Namespace WeifenLuo.WinFormsUI.Docking
    Public Class FloatWindow
        Inherits Form
        Implements INestedPanesContainer, IDockDragSource
        Private m_nestedPanes As NestedPaneCollection
        Friend Const WM_CHECKDISPOSE As Integer = Win32.Msgs.WM_USER + 1

        Friend Protected Sub New(dockPanel As DockPanel, pane As DockPane)
            InternalConstruct(dockPanel, pane, False, Rectangle.Empty)
        End Sub

        Friend Protected Sub New(dockPanel As DockPanel, pane As DockPane, bounds As Rectangle)
            InternalConstruct(dockPanel, pane, True, bounds)
        End Sub

        Private Sub InternalConstruct(dockPanel As DockPanel, pane As DockPane, boundsSpecified As Boolean, bounds As Rectangle)
            If dockPanel Is Nothing Then Throw (New ArgumentNullException(Strings.FloatWindow_Constructor_NullDockPanel))

            m_nestedPanes = New NestedPaneCollection(Me)

            FormBorderStyle = FormBorderStyle.SizableToolWindow
            ShowInTaskbar = False
            If dockPanel.RightToLeft <> MyBase.RightToLeft Then MyBase.RightToLeft = dockPanel.RightToLeft
            If MyBase.RightToLeftLayout <> dockPanel.RightToLeftLayout Then MyBase.RightToLeftLayout = dockPanel.RightToLeftLayout

            SuspendLayout()
            If boundsSpecified Then
                MyBase.Bounds = bounds
                StartPosition = FormStartPosition.Manual
            Else
                StartPosition = FormStartPosition.WindowsDefaultLocation
                Size = dockPanel.DefaultFloatWindowSize
            End If

            m_dockPanel = dockPanel
            Owner = Me.DockPanel.FindForm()
            Me.DockPanel.AddFloatWindow(Me)
            If pane IsNot Nothing Then pane.FloatWindow = Me

            If EnableFontInheritanceFix = True Then
                MyBase.Font = dockPanel.Font
            End If

            ResumeLayout()
        End Sub

        Protected Overrides Sub Dispose(disposing As Boolean)
            If disposing Then
                If DockPanel IsNot Nothing Then DockPanel.RemoveFloatWindow(Me)
                m_dockPanel = Nothing
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private m_allowEndUserDocking As Boolean = True
        Public Property AllowEndUserDocking As Boolean
            Get
                Return m_allowEndUserDocking
            End Get
            Set(value As Boolean)
                m_allowEndUserDocking = value
            End Set
        End Property

        Private m_doubleClickTitleBarToDock As Boolean = True
        Public Property DoubleClickTitleBarToDock As Boolean
            Get
                Return m_doubleClickTitleBarToDock
            End Get
            Set(value As Boolean)
                m_doubleClickTitleBarToDock = value
            End Set
        End Property

        Public ReadOnly Property NestedPanes As NestedPaneCollection Implements INestedPanesContainer.NestedPanes
            Get
                Return m_nestedPanes
            End Get
        End Property

        Public ReadOnly Property VisibleNestedPanes As VisibleNestedPaneCollection Implements INestedPanesContainer.VisibleNestedPanes
            Get
                Return NestedPanes.VisibleNestedPanes
            End Get
        End Property

        Private m_dockPanel As DockPanel
        Public ReadOnly Property DockPanel As DockPanel
            Get
                Return m_dockPanel
            End Get
        End Property

        Public ReadOnly Property DockState As DockState Implements INestedPanesContainer.DockState
            Get
                Return DockState.Float
            End Get
        End Property

        Public ReadOnly Property IsFloat As Boolean Implements INestedPanesContainer.IsFloat
            Get
                Return DockState = DockState.Float
            End Get
        End Property

        Friend Function IsDockStateValid(dockState As DockState) As Boolean
            For Each pane In NestedPanes
                For Each content In pane.Contents
                    If Not DockHelper.IsDockStateValid(dockState, content.DockHandler.DockAreas) Then Return False
                Next
            Next

            Return True
        End Function

        Protected Overrides Sub OnActivated(e As EventArgs)
            DockPanel.FloatWindows.BringWindowToFront(Me)
            MyBase.OnActivated(e)
            ' Propagate the Activated event to the visible panes content objects
            For Each pane In VisibleNestedPanes
                For Each content In pane.Contents
                    content.OnActivated(e)
                Next
            Next
        End Sub

        Protected Overrides Sub OnDeactivate(e As EventArgs)
            MyBase.OnDeactivate(e)
            ' Propagate the Deactivate event to the visible panes content objects
            For Each pane In VisibleNestedPanes
                For Each content In pane.Contents
                    content.OnDeactivate(e)
                Next
            Next
        End Sub

        Protected Overrides Sub OnLayout(levent As LayoutEventArgs)
            VisibleNestedPanes.Refresh()
            RefreshChanges()
            Visible = VisibleNestedPanes.Count > 0
            SetText()

            MyBase.OnLayout(levent)
        End Sub


        <SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId:="System.Windows.Forms.Control.set_Text(System.String)")>
        Friend Sub SetText()
            Dim theOnlyPane = If(VisibleNestedPanes.Count = 1, VisibleNestedPanes(0), Nothing)

            If theOnlyPane Is Nothing OrElse theOnlyPane.ActiveContent Is Nothing Then
                Text = " "  ' use " " instead of string.Empty because the whole title bar will disappear when ControlBox is set to false.
                Icon = Nothing
            Else
                Text = theOnlyPane.ActiveContent.DockHandler.TabText
                Icon = theOnlyPane.ActiveContent.DockHandler.Icon
            End If
        End Sub

        Protected Overrides Sub SetBoundsCore(x As Integer, y As Integer, width As Integer, height As Integer, specified As BoundsSpecified)
            Dim rectWorkArea = SystemInformation.VirtualScreen

            If y + height > rectWorkArea.Bottom Then y -= y + height - rectWorkArea.Bottom

            If y < rectWorkArea.Top Then y += rectWorkArea.Top - y

            MyBase.SetBoundsCore(x, y, width, height, specified)
        End Sub

        <SecurityPermission(SecurityAction.LinkDemand, Flags:=SecurityPermissionFlag.UnmanagedCode)>
        Protected Overrides Sub WndProc(ByRef m As Message)
            Select Case m.Msg
                Case Win32.Msgs.WM_NCLBUTTONDOWN
                    If IsDisposed Then Return

                    Dim result As UInteger = If(IsRunningOnMono, 0UI, SendMessage(Handle, Win32.Msgs.WM_NCHITTEST, 0, CUInt(m.LParam)))
                    If result = 2 AndAlso DockPanel.AllowEndUserDocking AndAlso AllowEndUserDocking Then    ' HITTEST_CAPTION
                        Activate()
                        m_dockPanel.BeginDrag(Me)
                    Else
                        MyBase.WndProc(m)
                    End If

                    Return
                Case Win32.Msgs.WM_NCRBUTTONDOWN
                    Dim result = If(IsRunningOnMono, HitTestCaption(Me), SendMessage(Handle, Win32.Msgs.WM_NCHITTEST, 0, CUInt(m.LParam)))
                    If result = 2 Then  ' HITTEST_CAPTION
                        Dim theOnlyPane = If(VisibleNestedPanes.Count = 1, VisibleNestedPanes(0), Nothing)
                        If theOnlyPane IsNot Nothing AndAlso theOnlyPane.ActiveContent IsNot Nothing Then
                            theOnlyPane.ShowTabPageContextMenu(Me, PointToClient(MousePosition))
                            Return
                        End If
                    End If

                    MyBase.WndProc(m)
                    Return
                Case Win32.Msgs.WM_CLOSE
                    If NestedPanes.Count = 0 Then
                        MyBase.WndProc(m)
                        Return
                    End If
                    For i = NestedPanes.Count - 1 To 0 Step -1
                        Dim contents = NestedPanes(i).Contents
                        For j = contents.Count - 1 To 0 Step -1
                            Dim content = contents(j)
                            If content.DockHandler.DockState <> DockState.Float Then Continue For

                            If Not content.DockHandler.CloseButton Then Continue For

                            If content.DockHandler.HideOnClose Then
                                content.DockHandler.Hide()
                            Else
                                content.DockHandler.Close()
                            End If
                        Next
                    Next
                    Return
                Case Win32.Msgs.WM_NCLBUTTONDBLCLK
                    Dim result = If(Not DoubleClickTitleBarToDock OrElse IsRunningOnMono, HitTestCaption(Me), SendMessage(Handle, Win32.Msgs.WM_NCHITTEST, 0, CUInt(m.LParam)))

                    If result <> 2 Then ' HITTEST_CAPTION
                        MyBase.WndProc(m)
                        Return
                    End If

                    DockPanel.SuspendLayout(True)

                    ' Restore to panel
                    For Each pane In NestedPanes
                        If pane.DockState <> DockState.Float Then Continue For
                        pane.RestoreToPanel()
                    Next


                    DockPanel.ResumeLayout(True, True)
                    Return
                Case WM_CHECKDISPOSE
                    If NestedPanes.Count = 0 Then Dispose()
                    Return
            End Select

            MyBase.WndProc(m)
        End Sub

        Friend Sub RefreshChanges()
            If IsDisposed Then Return

            If VisibleNestedPanes.Count = 0 Then
                ControlBox = True
                Return
            End If

            For i = VisibleNestedPanes.Count - 1 To 0 Step -1
                Dim contents = VisibleNestedPanes(i).Contents
                For j = contents.Count - 1 To 0 Step -1
                    Dim content = contents(j)
                    If content.DockHandler.DockState <> DockState.Float Then Continue For

                    If content.DockHandler.CloseButton AndAlso content.DockHandler.CloseButtonVisible Then
                        ControlBox = True
                        Return
                    End If
                Next
            Next
            'Only if there is a ControlBox do we turn it off
            'old code caused a flash of the window.
            If ControlBox Then ControlBox = False
        End Sub

        Public Overridable ReadOnly Property DisplayingRectangle As Rectangle Implements INestedPanesContainer.DisplayingRectangle
            Get
                Return ClientRectangle
            End Get
        End Property

        Friend Sub TestDrop(dragSource As IDockDragSource, dockOutline As DockOutlineBase)
            If VisibleNestedPanes.Count = 1 Then
                Dim pane = VisibleNestedPanes(0)
                If Not dragSource.CanDockTo(pane) Then Return

                Dim ptMouse = MousePosition
                Dim lParam = MakeLong(ptMouse.X, ptMouse.Y)
                If Not IsRunningOnMono Then
                    If SendMessage(Handle, Win32.Msgs.WM_NCHITTEST, 0, lParam) = CUInt(Win32.HitTest.HTCAPTION) Then
                        dockOutline.Show(VisibleNestedPanes(0), -1)
                    End If
                End If
            End If
        End Sub

#Region "IDockDragSource Members"

#Region "IDragSource Members"

        Private ReadOnly Property DragControl As Control Implements IDragSource.DragControl
            Get
                Return Me
            End Get
        End Property

#End Region

        Private Function IsDockStateValid1(dockState As DockState) As Boolean Implements IDockDragSource.IsDockStateValid
            Return IsDockStateValid(dockState)
        End Function

        Private Function CanDockTo(pane As DockPane) As Boolean Implements IDockDragSource.CanDockTo
            If Not IsDockStateValid(pane.DockState) Then Return False

            If pane.FloatWindow Is Me Then Return False

            Return True
        End Function

        Private m_preDragExStyle As Integer

        Private Function BeginDrag(ptMouse As Point) As Rectangle Implements IDockDragSource.BeginDrag
            m_preDragExStyle = GetWindowLong(Handle, Win32.GetWindowLongIndex.GWL_EXSTYLE)
            SetWindowLong(Handle, Win32.GetWindowLongIndex.GWL_EXSTYLE, m_preDragExStyle Or (Win32.WindowExStyles.WS_EX_TRANSPARENT Or Win32.WindowExStyles.WS_EX_LAYERED))
            Return Bounds
        End Function

        Private Sub EndDrag() Implements IDockDragSource.EndDrag
            SetWindowLong(Handle, Win32.GetWindowLongIndex.GWL_EXSTYLE, m_preDragExStyle)

            Invalidate(True)
            SendMessage(Handle, Win32.Msgs.WM_NCPAINT, 1, 0)
        End Sub

        Public Sub FloatAt(floatWindowBounds As Rectangle) Implements IDockDragSource.FloatAt
            Bounds = floatWindowBounds
        End Sub

        Public Sub DockTo(pane As DockPane, dockStyle As DockStyle, contentIndex As Integer) Implements IDockDragSource.DockTo
            If dockStyle = DockStyle.Fill Then
                For i = NestedPanes.Count - 1 To 0 Step -1
                    Dim paneFrom = NestedPanes(i)
                    For j = paneFrom.Contents.Count - 1 To 0 Step -1
                        Dim c = paneFrom.Contents(j)
                        c.DockHandler.Pane = pane
                        If contentIndex <> -1 Then pane.SetContentIndex(c, contentIndex)
                        c.DockHandler.Activate()
                    Next
                Next
            Else
                Dim alignment = DockAlignment.Left
                If dockStyle = DockStyle.Left Then
                    alignment = DockAlignment.Left
                ElseIf dockStyle = DockStyle.Right Then
                    alignment = DockAlignment.Right
                ElseIf dockStyle = DockStyle.Top Then
                    alignment = DockAlignment.Top
                ElseIf dockStyle = DockStyle.Bottom Then
                    alignment = DockAlignment.Bottom
                End If

                MergeNestedPanes(VisibleNestedPanes, pane.NestedPanesContainer.NestedPanes, pane, alignment, 0.5)
            End If
        End Sub

        Public Sub DockTo(panel As DockPanel, dockStyle As DockStyle) Implements IDockDragSource.DockTo
            If panel IsNot DockPanel Then Throw New ArgumentException(Strings.IDockDragSource_DockTo_InvalidPanel, "panel")

            Dim nestedPanesTo As NestedPaneCollection = Nothing

            If dockStyle = DockStyle.Top Then
                nestedPanesTo = DockPanel.DockWindows(DockState.DockTop).NestedPanes
            ElseIf dockStyle = DockStyle.Bottom Then
                nestedPanesTo = DockPanel.DockWindows(DockState.DockBottom).NestedPanes
            ElseIf dockStyle = DockStyle.Left Then
                nestedPanesTo = DockPanel.DockWindows(DockState.DockLeft).NestedPanes
            ElseIf dockStyle = DockStyle.Right Then
                nestedPanesTo = DockPanel.DockWindows(DockState.DockRight).NestedPanes
            ElseIf dockStyle = DockStyle.Fill Then
                nestedPanesTo = DockPanel.DockWindows(DockState.Document).NestedPanes
            End If

            Dim prevPane As DockPane = Nothing
            For i = nestedPanesTo.Count - 1 To 0 Step -1
                If nestedPanesTo(i) IsNot VisibleNestedPanes(0) Then prevPane = nestedPanesTo(i)
            Next
            MergeNestedPanes(VisibleNestedPanes, nestedPanesTo, prevPane, DockAlignment.Left, 0.5)
        End Sub

        Private Shared Sub MergeNestedPanes(nestedPanesFrom As VisibleNestedPaneCollection, nestedPanesTo As NestedPaneCollection, prevPane As DockPane, alignment As DockAlignment, proportion As Double)
            If nestedPanesFrom.Count = 0 Then Return

            Dim count = nestedPanesFrom.Count
            Dim panes = New DockPane(count - 1) {}
            Dim prevPanes = New DockPane(count - 1) {}
            Dim alignments = New DockAlignment(count - 1) {}
            Dim proportions = New Double(count - 1) {}

            For i = 0 To count - 1
                panes(i) = nestedPanesFrom(i)
                prevPanes(i) = nestedPanesFrom(i).NestedDockingStatus.PreviousPane
                alignments(i) = nestedPanesFrom(i).NestedDockingStatus.Alignment
                proportions(i) = nestedPanesFrom(i).NestedDockingStatus.Proportion
            Next

            Dim pane = panes(0).DockTo(nestedPanesTo.Container, prevPane, alignment, proportion)
            panes(0).DockState = nestedPanesTo.DockState

            For i = 1 To count - 1
                For j = i To count - 1
                    If prevPanes(j) Is panes(i - 1) Then prevPanes(j) = pane
                Next
                pane = panes(i).DockTo(nestedPanesTo.Container, prevPanes(i), alignments(i), proportions(i))
                panes(i).DockState = nestedPanesTo.DockState
            Next
        End Sub

#End Region
    End Class
End Namespace
