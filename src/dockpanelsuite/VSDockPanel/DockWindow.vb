Imports System.Windows.Forms
Imports System.Drawing
Imports System.ComponentModel

Namespace WeifenLuo.WinFormsUI.Docking
    ''' <summary>
    ''' Dock window base class.
    ''' </summary>
    <ToolboxItem(False)>
    Public Partial Class DockWindow
        Inherits Panel
        Implements INestedPanesContainer, ISplitterHost
        Private m_dockPanel As DockPanel
        Private m_dockState As DockState
        Private m_splitter As SplitterBase
        Private m_nestedPanes As NestedPaneCollection

        Protected Friend Sub New(dockPanel As DockPanel, dockState As DockState)
            m_nestedPanes = New NestedPaneCollection(Me)
            m_dockPanel = dockPanel
            m_dockState = dockState
            Visible = False

            SuspendLayout()

            If Me.DockState = DockState.DockLeft OrElse Me.DockState = DockState.DockRight OrElse Me.DockState = DockState.DockTop OrElse Me.DockState = DockState.DockBottom Then
                m_splitter = Me.DockPanel.Theme.Extender.WindowSplitterControlFactory.CreateSplitterControl(Me)
                Controls.Add(m_splitter)
            End If

            If Me.DockState = DockState.DockLeft Then
                MyBase.Dock = DockStyle.Left
                m_splitter.Dock = DockStyle.Right
            ElseIf Me.DockState = DockState.DockRight Then
                MyBase.Dock = DockStyle.Right
                m_splitter.Dock = DockStyle.Left
            ElseIf Me.DockState = DockState.DockTop Then
                MyBase.Dock = DockStyle.Top
                m_splitter.Dock = DockStyle.Bottom
            ElseIf Me.DockState = DockState.DockBottom Then
                MyBase.Dock = DockStyle.Bottom
                m_splitter.Dock = DockStyle.Top
            ElseIf Me.DockState = DockState.Document Then
                MyBase.Dock = DockStyle.Fill
            End If

            ResumeLayout()
        End Sub

        Public ReadOnly Property IsDockWindow As Boolean Implements ISplitterHost.IsDockWindow
            Get
                Return True
            End Get
        End Property

        Public ReadOnly Property VisibleNestedPanes As VisibleNestedPaneCollection Implements INestedPanesContainer.VisibleNestedPanes
            Get
                Return NestedPanes.VisibleNestedPanes
            End Get
        End Property

        Public ReadOnly Property NestedPanes As NestedPaneCollection Implements INestedPanesContainer.NestedPanes
            Get
                Return m_nestedPanes
            End Get
        End Property

        Public ReadOnly Property DockPanel As DockPanel Implements ISplitterHost.DockPanel
            Get
                Return m_dockPanel
            End Get
        End Property

        Public ReadOnly Property DockState As DockState Implements INestedPanesContainer.DockState, ISplitterHost.DockState
            Get
                Return m_dockState
            End Get
        End Property

        Public ReadOnly Property IsFloat As Boolean Implements INestedPanesContainer.IsFloat
            Get
                Return DockState = DockState.Float
            End Get
        End Property

        Friend ReadOnly Property DefaultPane As DockPane
            Get
                Return If(VisibleNestedPanes.Count = 0, Nothing, VisibleNestedPanes(0))
            End Get
        End Property

        Public Overridable ReadOnly Property DisplayingRectangle As Rectangle Implements INestedPanesContainer.DisplayingRectangle
            Get
                Dim rect = ClientRectangle
                ' if DockWindow is document, exclude the border
                If DockState = DockState.Document Then
                    rect.X += 1
                    rect.Y += 1
                    rect.Width -= 2
                    rect.Height -= 2
                    ' exclude the splitter
                ElseIf DockState = DockState.DockLeft Then
                    rect.Width -= DockPanel.Theme.Measures.SplitterSize
                ElseIf DockState = DockState.DockRight Then
                    rect.X += DockPanel.Theme.Measures.SplitterSize
                    rect.Width -= DockPanel.Theme.Measures.SplitterSize
                ElseIf DockState = DockState.DockTop Then
                    rect.Height -= DockPanel.Theme.Measures.SplitterSize
                ElseIf DockState = DockState.DockBottom Then
                    rect.Y += DockPanel.Theme.Measures.SplitterSize
                    rect.Height -= DockPanel.Theme.Measures.SplitterSize
                End If

                Return rect
            End Get
        End Property

        Protected Overrides Sub OnLayout(levent As LayoutEventArgs)
            VisibleNestedPanes.Refresh()
            If VisibleNestedPanes.Count = 0 Then
                If Visible Then Visible = False
            ElseIf Not Visible Then
                Visible = True
                VisibleNestedPanes.Refresh()
            End If

            MyBase.OnLayout(levent)
        End Sub

#Region "ISplitterDragSource Members"

        Private Sub BeginDrag(rectSplitter As Rectangle) Implements ISplitterDragSource.BeginDrag
        End Sub

        Private Sub EndDrag() Implements ISplitterDragSource.EndDrag
        End Sub

        Private ReadOnly Property IsVertical As Boolean Implements ISplitterDragSource.IsVertical
            Get
                Return DockState = DockState.DockLeft OrElse DockState = DockState.DockRight
            End Get
        End Property

        Private ReadOnly Property DragLimitBounds As Rectangle Implements ISplitterDragSource.DragLimitBounds
            Get
                Dim rectLimit = DockPanel.DockArea
                Dim location As Point
                If (ModifierKeys And Keys.Shift) = 0 Then
                    location = MyBase.Location
                Else
                    location = DockPanel.DockArea.Location
                End If

                If CType(Me, ISplitterDragSource).IsVertical Then
                    rectLimit.X += MinSize
                    rectLimit.Width -= 2 * MinSize
                    rectLimit.Y = location.Y
                    If (ModifierKeys And Keys.Shift) = 0 Then rectLimit.Height = Height
                Else
                    rectLimit.Y += MinSize
                    rectLimit.Height -= 2 * MinSize
                    rectLimit.X = location.X
                    If (ModifierKeys And Keys.Shift) = 0 Then rectLimit.Width = Width
                End If

                Return DockPanel.RectangleToScreen(rectLimit)
            End Get
        End Property

        Private Sub MoveSplitter(offset As Integer) Implements ISplitterDragSource.MoveSplitter
            If (ModifierKeys And Keys.Shift) <> 0 Then SendToBack()

            Dim rectDockArea = DockPanel.DockArea
            If DockState = DockState.DockLeft AndAlso rectDockArea.Width > 0 Then
                If DockPanel.DockLeftPortion > 1 Then
                    DockPanel.DockLeftPortion = Width + offset
                Else
                    DockPanel.DockLeftPortion += offset / rectDockArea.Width
                End If
            ElseIf DockState = DockState.DockRight AndAlso rectDockArea.Width > 0 Then
                If DockPanel.DockRightPortion > 1 Then
                    DockPanel.DockRightPortion = Width - offset
                Else
                    DockPanel.DockRightPortion -= offset / rectDockArea.Width
                End If
            ElseIf DockState = DockState.DockBottom AndAlso rectDockArea.Height > 0 Then
                If DockPanel.DockBottomPortion > 1 Then
                    DockPanel.DockBottomPortion = Height - offset
                Else
                    DockPanel.DockBottomPortion -= offset / rectDockArea.Height
                End If
            ElseIf DockState = DockState.DockTop AndAlso rectDockArea.Height > 0 Then
                If DockPanel.DockTopPortion > 1 Then
                    DockPanel.DockTopPortion = Height + offset
                Else
                    DockPanel.DockTopPortion += offset / rectDockArea.Height
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
End Namespace
