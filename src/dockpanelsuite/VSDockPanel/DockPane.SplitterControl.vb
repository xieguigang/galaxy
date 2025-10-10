Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms

Namespace WeifenLuo.WinFormsUI.Docking
    Partial Class DockPane
        <ToolboxItem(False)>
        Public Class SplitterControlBase
            Inherits Control
            Implements ISplitterDragSource
            Private m_pane As DockPane

            Public Sub New(pane As DockPane)
                SetStyle(ControlStyles.Selectable, False)
                m_pane = pane
            End Sub

            Public ReadOnly Property DockPane As DockPane
                Get
                    Return m_pane
                End Get
            End Property

            Private m_alignment As DockAlignment
            Public Property Alignment As DockAlignment
                Get
                    Return m_alignment
                End Get
                Set(value As DockAlignment)
                    m_alignment = value
                    If m_alignment = DockAlignment.Left OrElse m_alignment = DockAlignment.Right Then
                        MyBase.Cursor = Cursors.VSplit
                    ElseIf m_alignment = DockAlignment.Top OrElse m_alignment = DockAlignment.Bottom Then
                        MyBase.Cursor = Cursors.HSplit
                    Else
                        MyBase.Cursor = Cursors.Default
                    End If

                    If DockPane.DockState = DockState.Document Then Invalidate()
                End Set
            End Property

            Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
                MyBase.OnMouseDown(e)

                If e.Button <> MouseButtons.Left Then Return

                DockPane.DockPanel.BeginDrag(Me, Parent.RectangleToScreen(Bounds))
            End Sub

#Region "ISplitterDragSource Members"

            Private Sub BeginDrag(rectSplitter As Rectangle) Implements ISplitterDragSource.BeginDrag
            End Sub

            Private Sub EndDrag() Implements ISplitterDragSource.EndDrag
            End Sub

            Private ReadOnly Property IsVertical As Boolean Implements ISplitterDragSource.IsVertical
                Get
                    Dim status = DockPane.NestedDockingStatus
                    Return status.DisplayingAlignment = DockAlignment.Left OrElse status.DisplayingAlignment = DockAlignment.Right
                End Get
            End Property

            Private ReadOnly Property DragLimitBounds As Rectangle Implements ISplitterDragSource.DragLimitBounds
                Get
                    Dim status = DockPane.NestedDockingStatus
                    Dim rectLimit = Parent.RectangleToScreen(status.LogicalBounds)
                    If CType(Me, ISplitterDragSource).IsVertical Then
                        rectLimit.X += MinSize
                        rectLimit.Width -= 2 * MinSize
                    Else
                        rectLimit.Y += MinSize
                        rectLimit.Height -= 2 * MinSize
                    End If

                    Return rectLimit
                End Get
            End Property

            Private Sub MoveSplitter(offset As Integer) Implements ISplitterDragSource.MoveSplitter
                Dim status = DockPane.NestedDockingStatus
                Dim proportion = status.Proportion
                If status.LogicalBounds.Width <= 0 OrElse status.LogicalBounds.Height <= 0 Then
                    Return
                ElseIf status.DisplayingAlignment = DockAlignment.Left Then
                    proportion += offset / status.LogicalBounds.Width
                ElseIf status.DisplayingAlignment = DockAlignment.Right Then
                    proportion -= offset / status.LogicalBounds.Width
                ElseIf status.DisplayingAlignment = DockAlignment.Top Then
                    proportion += offset / status.LogicalBounds.Height
                Else
                    proportion -= offset / status.LogicalBounds.Height
                End If

                DockPane.SetNestedDockingProportion(proportion)
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

        Private m_splitter As SplitterControlBase
        Private ReadOnly Property Splitter As SplitterControlBase
            Get
                Return m_splitter
            End Get
        End Property

        Friend WriteOnly Property SplitterBounds As Rectangle
            Set(value As Rectangle)
                Splitter.Bounds = value
            End Set
        End Property

        Friend WriteOnly Property SplitterAlignment As DockAlignment
            Set(value As DockAlignment)
                Splitter.Alignment = value
            End Set
        End Property
    End Class
End Namespace
