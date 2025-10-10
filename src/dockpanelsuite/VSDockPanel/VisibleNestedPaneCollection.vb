Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.Drawing

Namespace WeifenLuo.WinFormsUI.Docking
    Public NotInheritable Class VisibleNestedPaneCollection
        Inherits ReadOnlyCollection(Of DockPane)
        Private m_nestedPanes As NestedPaneCollection

        Friend Sub New(nestedPanes As NestedPaneCollection)
            MyBase.New(New List(Of DockPane)())
            m_nestedPanes = nestedPanes
        End Sub

        Public ReadOnly Property NestedPanes As NestedPaneCollection
            Get
                Return m_nestedPanes
            End Get
        End Property

        Public ReadOnly Property Container As INestedPanesContainer
            Get
                Return NestedPanes.Container
            End Get
        End Property

        Public ReadOnly Property DockState As DockState
            Get
                Return NestedPanes.DockState
            End Get
        End Property

        Public ReadOnly Property IsFloat As Boolean
            Get
                Return NestedPanes.IsFloat
            End Get
        End Property

        Friend Sub Refresh()
            Items.Clear()
            For i = 0 To NestedPanes.Count - 1
                Dim pane = NestedPanes(i)
                Dim status = pane.NestedDockingStatus
                status.SetDisplayingStatus(True, status.PreviousPane, status.Alignment, status.Proportion)
                Items.Add(pane)
            Next

            For Each pane In NestedPanes
                If pane.DockState <> DockState OrElse pane.IsHidden Then
                    pane.Bounds = Rectangle.Empty
                    pane.SplitterBounds = Rectangle.Empty
                    Remove(pane)
                End If
            Next

            CalculateBounds()

            For Each pane In Me
                Dim status = pane.NestedDockingStatus
                pane.Bounds = status.PaneBounds
                pane.SplitterBounds = status.SplitterBounds
                pane.SplitterAlignment = status.Alignment
            Next
        End Sub

        Private Sub Remove(pane As DockPane)
            If Not Contains(pane) Then Return

            Dim statusPane = pane.NestedDockingStatus
            Dim lastNestedPane As DockPane = Nothing
            For i = Count - 1 To IndexOf(pane) + 1 Step -1
                If EnableDisplayingPaneFix = True Then
                    If Me(i).NestedDockingStatus.DisplayingPreviousPane Is pane Then
                        lastNestedPane = Me(i)
                        Exit For
                    End If
                Else
                    If Me(i).NestedDockingStatus.PreviousPane Is pane Then
                        lastNestedPane = Me(i)
                        Exit For
                    End If
                End If
            Next

            If lastNestedPane IsNot Nothing Then
                Dim indexLastNestedPane = IndexOf(lastNestedPane)
                Items.Remove(lastNestedPane)
                Items(IndexOf(pane)) = lastNestedPane
                Dim lastNestedDock = lastNestedPane.NestedDockingStatus
                lastNestedDock.SetDisplayingStatus(True, statusPane.DisplayingPreviousPane, statusPane.DisplayingAlignment, statusPane.DisplayingProportion)
                For i = indexLastNestedPane - 1 To IndexOf(lastNestedPane) + 1 Step -1
                    Dim status = Me(i).NestedDockingStatus
                    If EnableDisplayingPaneFix = True Then
                        If status.DisplayingPreviousPane Is pane Then status.SetDisplayingStatus(True, lastNestedPane, status.DisplayingAlignment, status.DisplayingProportion)
                    Else
                        If status.PreviousPane Is pane Then status.SetDisplayingStatus(True, lastNestedPane, status.DisplayingAlignment, status.DisplayingProportion)
                    End If
                Next
            Else
                Items.Remove(pane)
            End If

            statusPane.SetDisplayingStatus(False, Nothing, DockAlignment.Left, 0.5)
        End Sub

        Private Sub CalculateBounds()
            If Count = 0 Then Return

            Me(0).NestedDockingStatus.SetDisplayingBounds(Container.DisplayingRectangle, Container.DisplayingRectangle, Rectangle.Empty)

            For i = 1 To Count - 1
                Dim pane = Me(i)
                Dim status = pane.NestedDockingStatus
                Dim prevPane = status.DisplayingPreviousPane
                Dim statusPrev = prevPane.NestedDockingStatus

                Dim rect = statusPrev.PaneBounds
                Dim bVerticalSplitter = status.DisplayingAlignment = DockAlignment.Left OrElse status.DisplayingAlignment = DockAlignment.Right

                Dim rectThis = rect
                Dim rectPrev = rect
                Dim rectSplitter = rect
                If status.DisplayingAlignment = DockAlignment.Left Then
                    rectThis.Width = CInt(CInt(rect.Width * status.DisplayingProportion) - pane.DockPanel.Theme.Measures.SplitterSize / 2)
                    rectSplitter.X = rectThis.X + rectThis.Width
                    rectSplitter.Width = pane.DockPanel.Theme.Measures.SplitterSize
                    rectPrev.X = rectSplitter.X + rectSplitter.Width
                    rectPrev.Width = rect.Width - rectThis.Width - rectSplitter.Width
                ElseIf status.DisplayingAlignment = DockAlignment.Right Then
                    rectPrev.Width = CInt(rect.Width - CInt(rect.Width * status.DisplayingProportion) - pane.DockPanel.Theme.Measures.SplitterSize / 2)
                    rectSplitter.X = rectPrev.X + rectPrev.Width
                    rectSplitter.Width = pane.DockPanel.Theme.Measures.SplitterSize
                    rectThis.X = rectSplitter.X + rectSplitter.Width
                    rectThis.Width = rect.Width - rectPrev.Width - rectSplitter.Width
                ElseIf status.DisplayingAlignment = DockAlignment.Top Then
                    rectThis.Height = CInt(CInt(rect.Height * status.DisplayingProportion) - pane.DockPanel.Theme.Measures.SplitterSize / 2)
                    rectSplitter.Y = rectThis.Y + rectThis.Height
                    rectSplitter.Height = pane.DockPanel.Theme.Measures.SplitterSize
                    rectPrev.Y = rectSplitter.Y + rectSplitter.Height
                    rectPrev.Height = rect.Height - rectThis.Height - rectSplitter.Height
                ElseIf status.DisplayingAlignment = DockAlignment.Bottom Then
                    rectPrev.Height = CInt(rect.Height - CInt(rect.Height * status.DisplayingProportion) - pane.DockPanel.Theme.Measures.SplitterSize / 2)
                    rectSplitter.Y = rectPrev.Y + rectPrev.Height
                    rectSplitter.Height = pane.DockPanel.Theme.Measures.SplitterSize
                    rectThis.Y = rectSplitter.Y + rectSplitter.Height
                    rectThis.Height = rect.Height - rectPrev.Height - rectSplitter.Height
                Else
                    rectThis = Rectangle.Empty
                End If

                rectSplitter.Intersect(rect)
                rectThis.Intersect(rect)
                rectPrev.Intersect(rect)
                status.SetDisplayingBounds(rect, rectThis, rectSplitter)
                statusPrev.SetDisplayingBounds(statusPrev.LogicalBounds, rectPrev, statusPrev.SplitterBounds)
            Next
        End Sub
    End Class
End Namespace
