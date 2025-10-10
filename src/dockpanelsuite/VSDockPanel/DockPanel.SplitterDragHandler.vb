Imports System.Drawing

Namespace WeifenLuo.WinFormsUI.Docking
    Partial Class DockPanel
        Private NotInheritable Class SplitterDragHandler
            Inherits DragHandler
            Private Class SplitterOutline
                Public Sub New()
                    m_dragForm = New DragForm()
                    SetDragForm(Rectangle.Empty)
                    DragForm.BackColor = Color.Black
                    DragForm.Opacity = 0.7
                    DragForm.Show(False)
                End Sub

                Private m_dragForm As DragForm
                Private ReadOnly Property DragForm As DragForm
                    Get
                        Return m_dragForm
                    End Get
                End Property

                Public Sub Show(rect As Rectangle)
                    SetDragForm(rect)
                End Sub

                Public Sub Close()
                    DragForm.Bounds = Rectangle.Empty
                    DragForm.Close()
                End Sub

                Private Sub SetDragForm(rect As Rectangle)
                    DragForm.Bounds = rect
                    If rect = Rectangle.Empty Then
                        DragForm.Region = New Region(Rectangle.Empty)
                    ElseIf DragForm.Region IsNot Nothing Then
                        DragForm.Region = Nothing
                    End If
                End Sub
            End Class

            Public Sub New(dockPanel As DockPanel)
                MyBase.New(dockPanel)
            End Sub

            Public Overloads Property DragSource As ISplitterDragSource
                Get
                    Return TryCast(MyBase.DragSource, ISplitterDragSource)
                End Get
                Private Set(value As ISplitterDragSource)
                    MyBase.DragSource = value
                End Set
            End Property

            Private m_outline As SplitterOutline
            Private Property Outline As SplitterOutline
                Get
                    Return m_outline
                End Get
                Set(value As SplitterOutline)
                    m_outline = value
                End Set
            End Property

            Private m_rectSplitter As Rectangle
            Private Property RectSplitter As Rectangle
                Get
                    Return m_rectSplitter
                End Get
                Set(value As Rectangle)
                    m_rectSplitter = value
                End Set
            End Property

            Public Overloads Sub BeginDrag(dragSource As ISplitterDragSource, rectSplitter As Rectangle)
                Me.DragSource = dragSource
                Me.RectSplitter = rectSplitter

                If Not MyBase.BeginDrag() Then
                    Me.DragSource = Nothing
                    Return
                End If

                Outline = New SplitterOutline()
                Outline.Show(rectSplitter)
                Me.DragSource.BeginDrag(rectSplitter)
            End Sub

            Protected Overrides Sub OnDragging()
                Outline.Show(GetSplitterOutlineBounds(MousePosition))
            End Sub

            Protected Overrides Sub OnEndDrag(abort As Boolean)
                DockPanel.SuspendLayout(True)

                Outline.Close()

                If Not abort Then DragSource.MoveSplitter(GetMovingOffset(MousePosition))

                DragSource.EndDrag()
                DockPanel.ResumeLayout(True, True)
            End Sub

            Private Function GetMovingOffset(ptMouse As Point) As Integer
                Dim rect = GetSplitterOutlineBounds(ptMouse)
                If DragSource.IsVertical Then
                    Return rect.X - RectSplitter.X
                Else
                    Return rect.Y - RectSplitter.Y
                End If
            End Function

            Private Function GetSplitterOutlineBounds(ptMouse As Point) As Rectangle
                Dim rectLimit = DragSource.DragLimitBounds

                Dim rect = RectSplitter
                If rectLimit.Width <= 0 OrElse rectLimit.Height <= 0 Then Return rect

                If DragSource.IsVertical Then
                    rect.X += ptMouse.X - StartMousePosition.X
                    rect.Height = rectLimit.Height
                Else
                    rect.Y += ptMouse.Y - StartMousePosition.Y
                    rect.Width = rectLimit.Width
                End If

                If rect.Left < rectLimit.Left Then rect.X = rectLimit.X
                If rect.Top < rectLimit.Top Then rect.Y = rectLimit.Y
                If rect.Right > rectLimit.Right Then rect.X -= rect.Right - rectLimit.Right
                If rect.Bottom > rectLimit.Bottom Then rect.Y -= rect.Bottom - rectLimit.Bottom

                Return rect
            End Function
        End Class

        Private m_splitterDragHandler As SplitterDragHandler = Nothing
        Private Function GetSplitterDragHandler() As SplitterDragHandler
            If m_splitterDragHandler Is Nothing Then m_splitterDragHandler = New SplitterDragHandler(Me)
            Return m_splitterDragHandler
        End Function

        Public Sub BeginDrag(dragSource As ISplitterDragSource, rectSplitter As Rectangle)
            GetSplitterDragHandler().BeginDrag(dragSource, rectSplitter)
        End Sub
    End Class
End Namespace
