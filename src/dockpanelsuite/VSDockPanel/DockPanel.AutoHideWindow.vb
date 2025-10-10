Imports System
Imports System.Windows.Forms
Imports System.Drawing
Imports System.ComponentModel

Namespace WeifenLuo.WinFormsUI.Docking
    Partial Class DockPanel
        <ToolboxItem(False)>
        Public Class AutoHideWindowControl
            Inherits Panel
            Implements ISplitterHost
            Private _m_splitter As WeifenLuo.WinFormsUI.Docking.SplitterBase
            Protected Class SplitterControl
                Inherits SplitterBase
                Public Sub New(autoHideWindow As AutoHideWindowControl)
                    m_autoHideWindow = autoHideWindow
                End Sub

                Private m_autoHideWindow As AutoHideWindowControl
                Private ReadOnly Property AutoHideWindow As AutoHideWindowControl
                    Get
                        Return m_autoHideWindow
                    End Get
                End Property

                Protected Overrides ReadOnly Property SplitterSize As Integer
                    Get
                        Return AutoHideWindow.DockPanel.Theme.Measures.AutoHideSplitterSize
                    End Get
                End Property

                Protected Overrides Sub StartDrag()
                    AutoHideWindow.DockPanel.BeginDrag(AutoHideWindow, AutoHideWindow.RectangleToScreen(Bounds))
                End Sub
            End Class

#Region "consts"
            Private Const ANIMATE_TIME As Integer = 100    ' in mini-seconds
#End Region

            Private m_timerMouseTrack As Timer

            Protected Property m_splitter As SplitterBase
                Get
                    Return _m_splitter
                End Get
                Private Set(value As SplitterBase)
                    _m_splitter = value
                End Set
            End Property

            Public Sub New(dockPanel As DockPanel)
                m_dockPanel = dockPanel

                m_timerMouseTrack = New Timer()
                AddHandler m_timerMouseTrack.Tick, New EventHandler(AddressOf TimerMouseTrack_Tick)

                Visible = False
                m_splitter = Me.DockPanel.Theme.Extender.WindowSplitterControlFactory.CreateSplitterControl(Me)
                Controls.Add(m_splitter)
            End Sub

            Protected Overrides Sub Dispose(disposing As Boolean)
                If disposing Then
                    m_timerMouseTrack.Dispose()
                End If
                MyBase.Dispose(disposing)
            End Sub

            Public ReadOnly Property IsDockWindow As Boolean Implements ISplitterHost.IsDockWindow
                Get
                    Return False
                End Get
            End Property

            Private m_dockPanel As DockPanel = Nothing
            Public ReadOnly Property DockPanel As DockPanel Implements ISplitterHost.DockPanel
                Get
                    Return m_dockPanel
                End Get
            End Property

            Private m_activePane As DockPane = Nothing
            Public ReadOnly Property ActivePane As DockPane
                Get
                    Return m_activePane
                End Get
            End Property

            Private Sub SetActivePane()
                Dim value = If(ActiveContent Is Nothing, Nothing, ActiveContent.DockHandler.Pane)

                If value Is m_activePane Then Return

                m_activePane = value
            End Sub

            Private Shared ReadOnly AutoHideActiveContentChangedEvent As Object = New Object()
            Public Custom Event ActiveContentChanged As EventHandler
                AddHandler(value As EventHandler)
                    Events.AddHandler(AutoHideActiveContentChangedEvent, value)
                End AddHandler
                RemoveHandler(value As EventHandler)
                    Events.RemoveHandler(AutoHideActiveContentChangedEvent, value)
                End RemoveHandler
                RaiseEvent(sender As Object, e As EventArgs)
                End RaiseEvent
            End Event

            Protected Overridable Sub OnActiveContentChanged(e As EventArgs)
                Dim handler = CType(Events(ActiveContentChangedEvent), EventHandler)
                If handler IsNot Nothing Then handler(Me, e)
            End Sub

            Private m_activeContent As IDockContent = Nothing
            Public Property ActiveContent As IDockContent
                Get
                    Return m_activeContent
                End Get
                Set(value As IDockContent)
                    If value Is m_activeContent Then Return

                    If value IsNot Nothing Then
                        If Not IsDockStateAutoHide(value.DockHandler.DockState) OrElse value.DockHandler.DockPanel IsNot DockPanel Then Throw (New InvalidOperationException(Strings.DockPanel_ActiveAutoHideContent_InvalidValue))
                    End If

                    DockPanel.SuspendLayout(True)

                    If m_activeContent IsNot Nothing Then
                        If m_activeContent.DockHandler.Form.ContainsFocus Then
                            If Not IsRunningOnMono Then
                                DockPanel.ContentFocusManager.GiveUpFocus(m_activeContent)
                            End If
                        End If

                        AnimateWindow(False)
                    End If

                    m_activeContent = value
                    SetActivePane()
                    If ActivePane IsNot Nothing Then ActivePane.ActiveContent = m_activeContent

                    If m_activeContent IsNot Nothing Then AnimateWindow(True)

                    DockPanel.ResumeLayout(True, True)
                    DockPanel.RefreshAutoHideStrip()

                    SetTimerMouseTrack()

                    OnActiveContentChanged(EventArgs.Empty)
                End Set
            End Property

            Public ReadOnly Property DockState As DockState Implements ISplitterHost.DockState
                Get
                    Return If(ActiveContent Is Nothing, DockState.Unknown, ActiveContent.DockHandler.DockState)
                End Get
            End Property

            Private m_flagAnimate As Boolean = True
            Private Property FlagAnimate As Boolean
                Get
                    Return m_flagAnimate
                End Get
                Set(value As Boolean)
                    m_flagAnimate = value
                End Set
            End Property

            Private m_flagDragging As Boolean = False
            Friend Property FlagDragging As Boolean
                Get
                    Return m_flagDragging
                End Get
                Set(value As Boolean)
                    If m_flagDragging = value Then Return

                    m_flagDragging = value
                    SetTimerMouseTrack()
                End Set
            End Property

            Private Sub AnimateWindow(show As Boolean)
                If Not FlagAnimate AndAlso Visible <> show Then
                    Visible = show
                    Return
                End If

                Parent.SuspendLayout()

                Dim rectSource = GetRectangle(Not show)
                Dim rectTarget = GetRectangle(show)
                Dim dxLoc, dyLoc As Integer
                Dim dWidth, dHeight As Integer
                dHeight = 0
                dWidth = 0
                dyLoc = 0
                dxLoc = 0
                If DockState = DockState.DockTopAutoHide Then
                    dHeight = If(show, 1, -1)
                ElseIf DockState = DockState.DockLeftAutoHide Then
                    dWidth = If(show, 1, -1)
                ElseIf DockState = DockState.DockRightAutoHide Then
                    dxLoc = If(show, -1, 1)
                    dWidth = If(show, 1, -1)
                ElseIf DockState = DockState.DockBottomAutoHide Then
                    dyLoc = If(show, -1, 1)
                    dHeight = If(show, 1, -1)
                End If

                If show Then
                    Bounds = DockPanel.GetAutoHideWindowBounds(New Rectangle(-rectTarget.Width, -rectTarget.Height, rectTarget.Width, rectTarget.Height))
                    If Visible = False Then Visible = True
                    PerformLayout()
                End If

                SuspendLayout()

                LayoutAnimateWindow(rectSource)
                If Visible = False Then Visible = True

                Dim speedFactor = 1
                Dim totalPixels = If(rectSource.Width <> rectTarget.Width, Math.Abs(rectSource.Width - rectTarget.Width), Math.Abs(rectSource.Height - rectTarget.Height))
                Dim remainPixels = totalPixels
                Dim startingTime = Date.Now
                While rectSource <> rectTarget
                    Dim startPerMove = Date.Now

                    rectSource.X += dxLoc * speedFactor
                    rectSource.Y += dyLoc * speedFactor
                    rectSource.Width += dWidth * speedFactor
                    rectSource.Height += dHeight * speedFactor
                    If Math.Sign(rectTarget.X - rectSource.X) <> Math.Sign(dxLoc) Then rectSource.X = rectTarget.X
                    If Math.Sign(rectTarget.Y - rectSource.Y) <> Math.Sign(dyLoc) Then rectSource.Y = rectTarget.Y
                    If Math.Sign(rectTarget.Width - rectSource.Width) <> Math.Sign(dWidth) Then rectSource.Width = rectTarget.Width
                    If Math.Sign(rectTarget.Height - rectSource.Height) <> Math.Sign(dHeight) Then rectSource.Height = rectTarget.Height

                    LayoutAnimateWindow(rectSource)
                    If Parent IsNot Nothing Then Parent.Update()

                    remainPixels -= speedFactor

                    While True
                        Dim time As TimeSpan = New TimeSpan(0, 0, 0, 0, ANIMATE_TIME)
                        Dim elapsedPerMove = Date.Now - startPerMove
                        Dim elapsedTime = Date.Now - startingTime
                        If CInt((time - elapsedTime).TotalMilliseconds) <= 0 Then
                            speedFactor = remainPixels
                            Exit While
                        Else
                            speedFactor = CInt(remainPixels * CInt(elapsedPerMove.TotalMilliseconds) / CInt((time - elapsedTime).TotalMilliseconds))
                        End If
                        If speedFactor >= 1 Then Exit While
                    End While
                End While
                ResumeLayout()
                Parent.ResumeLayout()
            End Sub

            Private Sub LayoutAnimateWindow(rect As Rectangle)
                Bounds = DockPanel.GetAutoHideWindowBounds(rect)

                Dim rectClient = ClientRectangle

                If DockState = DockState.DockLeftAutoHide Then
                    ActivePane.Location = New Point(rectClient.Right - 2 - DockPanel.Theme.Measures.AutoHideSplitterSize - ActivePane.Width, ActivePane.Location.Y)
                ElseIf DockState = DockState.DockTopAutoHide Then
                    ActivePane.Location = New Point(ActivePane.Location.X, rectClient.Bottom - 2 - DockPanel.Theme.Measures.AutoHideSplitterSize - ActivePane.Height)
                End If
            End Sub

            Private Function GetRectangle(show As Boolean) As Rectangle
                If DockState = DockState.Unknown Then Return Rectangle.Empty

                Dim rect = DockPanel.AutoHideWindowRectangle

                If show Then Return rect

                If DockState = DockState.DockLeftAutoHide Then
                    rect.Width = 0
                ElseIf DockState = DockState.DockRightAutoHide Then
                    rect.X += rect.Width
                    rect.Width = 0
                ElseIf DockState = DockState.DockTopAutoHide Then
                    rect.Height = 0
                Else
                    rect.Y += rect.Height
                    rect.Height = 0
                End If

                Return rect
            End Function

            Private Sub SetTimerMouseTrack()
                If ActivePane Is Nothing OrElse ActivePane.IsActivated OrElse FlagDragging Then
                    m_timerMouseTrack.Enabled = False
                    Return
                End If

                ' start the timer
                Dim hovertime = SystemInformation.MouseHoverTime

                ' assign a default value 400 in case of setting Timer.Interval invalid value exception
                If hovertime <= 0 Then hovertime = 400

                m_timerMouseTrack.Interval = 2 * hovertime
                m_timerMouseTrack.Enabled = True
            End Sub

            Protected Overridable ReadOnly Property DisplayingRectangle As Rectangle
                Get
                    Dim rect = ClientRectangle

                    ' exclude the border and the splitter
                    If DockState = DockState.DockBottomAutoHide Then
                        rect.Y += 2 + DockPanel.Theme.Measures.AutoHideSplitterSize
                        rect.Height -= 2 + DockPanel.Theme.Measures.AutoHideSplitterSize
                    ElseIf DockState = DockState.DockRightAutoHide Then
                        rect.X += 2 + DockPanel.Theme.Measures.AutoHideSplitterSize
                        rect.Width -= 2 + DockPanel.Theme.Measures.AutoHideSplitterSize
                    ElseIf DockState = DockState.DockTopAutoHide Then
                        rect.Height -= 2 + DockPanel.Theme.Measures.AutoHideSplitterSize
                    ElseIf DockState = DockState.DockLeftAutoHide Then
                        rect.Width -= 2 + DockPanel.Theme.Measures.AutoHideSplitterSize
                    End If

                    Return rect
                End Get
            End Property

            Public Sub RefreshActiveContent()
                If ActiveContent Is Nothing Then Return

                If Not IsDockStateAutoHide(ActiveContent.DockHandler.DockState) Then
                    FlagAnimate = False
                    ActiveContent = Nothing
                    FlagAnimate = True
                End If
            End Sub

            Public Sub RefreshActivePane()
                SetTimerMouseTrack()
            End Sub

            Private Sub TimerMouseTrack_Tick(sender As Object, e As EventArgs)
                If IsDisposed Then Return

                If ActivePane Is Nothing OrElse ActivePane.IsActivated Then
                    m_timerMouseTrack.Enabled = False
                    Return
                End If

                Dim pane = ActivePane
                Dim ptMouseInAutoHideWindow = PointToClient(MousePosition)
                Dim ptMouseInDockPanel = DockPanel.PointToClient(MousePosition)

                Dim rectTabStrip = DockPanel.GetTabStripRectangle(pane.DockState)

                If Not ClientRectangle.Contains(ptMouseInAutoHideWindow) AndAlso Not rectTabStrip.Contains(ptMouseInDockPanel) Then
                    ActiveContent = Nothing
                    m_timerMouseTrack.Enabled = False
                End If
            End Sub

#Region "ISplitterDragSource Members"

            Private Sub BeginDrag(rectSplitter As Rectangle) Implements ISplitterDragSource.BeginDrag
                FlagDragging = True
            End Sub

            Private Sub EndDrag() Implements ISplitterDragSource.EndDrag
                FlagDragging = False
            End Sub

            Private ReadOnly Property IsVertical As Boolean Implements ISplitterDragSource.IsVertical
                Get
                    Return DockState = DockState.DockLeftAutoHide OrElse DockState = DockState.DockRightAutoHide
                End Get
            End Property

            Private ReadOnly Property DragLimitBounds As Rectangle Implements ISplitterDragSource.DragLimitBounds
                Get
                    Dim rectLimit = DockPanel.DockArea

                    If TryCast(Me, ISplitterDragSource).IsVertical Then
                        rectLimit.X += MinSize
                        rectLimit.Width -= 2 * MinSize
                    Else
                        rectLimit.Y += MinSize
                        rectLimit.Height -= 2 * MinSize
                    End If

                    Return DockPanel.RectangleToScreen(rectLimit)
                End Get
            End Property

            Private Sub MoveSplitter(offset As Integer) Implements ISplitterDragSource.MoveSplitter
                Dim rectDockArea = DockPanel.DockArea
                Dim content = ActiveContent
                If DockState = DockState.DockLeftAutoHide AndAlso rectDockArea.Width > 0 Then
                    If content.DockHandler.AutoHidePortion < 1 Then
                        content.DockHandler.AutoHidePortion += offset / rectDockArea.Width
                    Else
                        content.DockHandler.AutoHidePortion = Width + offset
                    End If
                ElseIf DockState = DockState.DockRightAutoHide AndAlso rectDockArea.Width > 0 Then
                    If content.DockHandler.AutoHidePortion < 1 Then
                        content.DockHandler.AutoHidePortion -= offset / rectDockArea.Width
                    Else
                        content.DockHandler.AutoHidePortion = Width - offset
                    End If
                ElseIf DockState = DockState.DockBottomAutoHide AndAlso rectDockArea.Height > 0 Then
                    If content.DockHandler.AutoHidePortion < 1 Then
                        content.DockHandler.AutoHidePortion -= offset / rectDockArea.Height
                    Else
                        content.DockHandler.AutoHidePortion = Height - offset
                    End If
                ElseIf DockState = DockState.DockTopAutoHide AndAlso rectDockArea.Height > 0 Then
                    If content.DockHandler.AutoHidePortion < 1 Then
                        content.DockHandler.AutoHidePortion += offset / rectDockArea.Height
                    Else
                        content.DockHandler.AutoHidePortion = Height + offset
                    End If
                End If
            End Sub

#Region "IDragSource Members"

            Private ReadOnly Property DragControl As Control Implements IDragSource.DragControl
                Get
                    Return Me
                End Get
            End Property
#End Region

#End Region
        End Class

        Private ReadOnly Property AutoHideWindow As AutoHideWindowControl
            Get
                Return m_autoHideWindow
            End Get
        End Property

        Friend ReadOnly Property AutoHideControl As Control
            Get
                Return m_autoHideWindow
            End Get
        End Property

        Friend Sub RefreshActiveAutoHideContent()
            AutoHideWindow.RefreshActiveContent()
        End Sub

        Friend ReadOnly Property AutoHideWindowRectangle As Rectangle
            Get
                Dim state = AutoHideWindow.DockState
                Dim rectDockArea = DockArea
                If ActiveAutoHideContent Is Nothing Then Return Rectangle.Empty

                If Parent Is Nothing Then Return Rectangle.Empty

                Dim rect = Rectangle.Empty
                Dim autoHideSize = ActiveAutoHideContent.DockHandler.AutoHidePortion
                If state = DockState.DockLeftAutoHide Then
                    If autoHideSize < 1 Then autoHideSize = rectDockArea.Width * autoHideSize
                    If autoHideSize > rectDockArea.Width - MinSize Then autoHideSize = rectDockArea.Width - MinSize
                    rect.X = rectDockArea.X - Theme.Measures.DockPadding
                    rect.Y = rectDockArea.Y
                    rect.Width = CInt(autoHideSize)
                    rect.Height = rectDockArea.Height
                ElseIf state = DockState.DockRightAutoHide Then
                    If autoHideSize < 1 Then autoHideSize = rectDockArea.Width * autoHideSize
                    If autoHideSize > rectDockArea.Width - MinSize Then autoHideSize = rectDockArea.Width - MinSize
                    rect.X = rectDockArea.X + rectDockArea.Width - CInt(autoHideSize) + Theme.Measures.DockPadding
                    rect.Y = rectDockArea.Y
                    rect.Width = CInt(autoHideSize)
                    rect.Height = rectDockArea.Height
                ElseIf state = DockState.DockTopAutoHide Then
                    If autoHideSize < 1 Then autoHideSize = rectDockArea.Height * autoHideSize
                    If autoHideSize > rectDockArea.Height - MinSize Then autoHideSize = rectDockArea.Height - MinSize
                    rect.X = rectDockArea.X
                    rect.Y = rectDockArea.Y - Theme.Measures.DockPadding
                    rect.Width = rectDockArea.Width
                    rect.Height = CInt(autoHideSize)
                ElseIf state = DockState.DockBottomAutoHide Then
                    If autoHideSize < 1 Then autoHideSize = rectDockArea.Height * autoHideSize
                    If autoHideSize > rectDockArea.Height - MinSize Then autoHideSize = rectDockArea.Height - MinSize
                    rect.X = rectDockArea.X
                    rect.Y = rectDockArea.Y + rectDockArea.Height - CInt(autoHideSize) + Theme.Measures.DockPadding
                    rect.Width = rectDockArea.Width
                    rect.Height = CInt(autoHideSize)
                End If

                Return rect
            End Get
        End Property

        Friend Function GetAutoHideWindowBounds(rectAutoHideWindow As Rectangle) As Rectangle
            If DocumentStyle = DocumentStyle.SystemMdi OrElse DocumentStyle = DocumentStyle.DockingMdi Then
                Return If(Parent Is Nothing, Rectangle.Empty, Parent.RectangleToClient(RectangleToScreen(rectAutoHideWindow)))
            Else
                Return rectAutoHideWindow
            End If
        End Function

        Friend Sub RefreshAutoHideStrip()
            AutoHideStripControl.RefreshChanges()
        End Sub
    End Class
End Namespace
