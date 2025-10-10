Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.Drawing

Namespace WeifenLuo.WinFormsUI.Docking
    Public NotInheritable Class NestedPaneCollection
        Inherits ReadOnlyCollection(Of DockPane)
        Private m_container As INestedPanesContainer
        Private m_visibleNestedPanes As VisibleNestedPaneCollection

        Friend Sub New(container As INestedPanesContainer)
            MyBase.New(New List(Of DockPane)())
            m_container = container
            m_visibleNestedPanes = New VisibleNestedPaneCollection(Me)
        End Sub

        Public ReadOnly Property Container As INestedPanesContainer
            Get
                Return m_container
            End Get
        End Property

        Public ReadOnly Property VisibleNestedPanes As VisibleNestedPaneCollection
            Get
                Return m_visibleNestedPanes
            End Get
        End Property

        Public ReadOnly Property DockState As DockState
            Get
                Return Container.DockState
            End Get
        End Property

        Public ReadOnly Property IsFloat As Boolean
            Get
                Return DockState = DockState.Float
            End Get
        End Property

        Friend Sub Add(pane As DockPane)
            If pane Is Nothing Then Return

            Dim oldNestedPanes = If(pane.NestedPanesContainer Is Nothing, Nothing, pane.NestedPanesContainer.NestedPanes)
            If oldNestedPanes IsNot Nothing Then oldNestedPanes.InternalRemove(pane)
            Items.Add(pane)
            If oldNestedPanes IsNot Nothing Then oldNestedPanes.CheckFloatWindowDispose()
        End Sub

        Private Sub CheckFloatWindowDispose()
            If Count <> 0 OrElse Container.DockState <> DockState.Float Then Return

            Dim floatWindow = CType(Container, FloatWindow)
            If floatWindow.Disposing OrElse floatWindow.IsDisposed Then Return

            If IsRunningOnMono Then Return

            PostMessage(CType(Container, FloatWindow).Handle, FloatWindow.WM_CHECKDISPOSE, 0, 0)
        End Sub

        ''' <summary>
        ''' Switches a pane with its first child in the pane hierarchy. (The actual hiding happens elsewhere.)
        ''' </summary>
        ''' <param name="pane">Pane to switch</param>
        Public Sub SwitchPaneWithFirstChild(pane As DockPane)
            If Not Contains(pane) Then Return

            Dim statusPane = pane.NestedDockingStatus
            Dim lastNestedPane As DockPane = Nothing
            For i = Count - 1 To IndexOf(pane) + 1 Step -1
                If Me(i).NestedDockingStatus.PreviousPane Is pane Then
                    lastNestedPane = Me(i)
                    Exit For
                End If
            Next

            If lastNestedPane IsNot Nothing Then
                Dim indexLastNestedPane = IndexOf(lastNestedPane)
                Items(IndexOf(pane)) = lastNestedPane
                Items(indexLastNestedPane) = pane
                Dim lastNestedDock = lastNestedPane.NestedDockingStatus

                Dim newAlignment As DockAlignment
                If lastNestedDock.Alignment = DockAlignment.Left Then
                    newAlignment = DockAlignment.Right
                ElseIf lastNestedDock.Alignment = DockAlignment.Right Then
                    newAlignment = DockAlignment.Left
                ElseIf lastNestedDock.Alignment = DockAlignment.Top Then
                    newAlignment = DockAlignment.Bottom
                Else
                    newAlignment = DockAlignment.Top
                End If
                Dim newProportion = 1 - lastNestedDock.Proportion

                lastNestedDock.SetStatus(Me, statusPane.PreviousPane, statusPane.Alignment, statusPane.Proportion)
                For i = indexLastNestedPane - 1 To IndexOf(lastNestedPane) + 1 Step -1
                    Dim status = Me(i).NestedDockingStatus
                    If status.PreviousPane Is pane Then status.SetStatus(Me, lastNestedPane, status.Alignment, status.Proportion)
                Next

                statusPane.SetStatus(Me, lastNestedPane, newAlignment, newProportion)
            End If
        End Sub

        Friend Sub Remove(pane As DockPane)
            InternalRemove(pane)
            CheckFloatWindowDispose()
        End Sub

        Private Sub InternalRemove(pane As DockPane)
            If Not Contains(pane) Then Return

            Dim statusPane = pane.NestedDockingStatus
            Dim lastNestedPane As DockPane = Nothing
            For i = Count - 1 To IndexOf(pane) + 1 Step -1
                If Me(i).NestedDockingStatus.PreviousPane Is pane Then
                    lastNestedPane = Me(i)
                    Exit For
                End If
            Next

            If lastNestedPane IsNot Nothing Then
                Dim indexLastNestedPane = IndexOf(lastNestedPane)
                Items.Remove(lastNestedPane)
                Items(IndexOf(pane)) = lastNestedPane
                Dim lastNestedDock = lastNestedPane.NestedDockingStatus
                lastNestedDock.SetStatus(Me, statusPane.PreviousPane, statusPane.Alignment, statusPane.Proportion)
                For i = indexLastNestedPane - 1 To IndexOf(lastNestedPane) + 1 Step -1
                    Dim status = Me(i).NestedDockingStatus
                    If status.PreviousPane Is pane Then status.SetStatus(Me, lastNestedPane, status.Alignment, status.Proportion)
                Next
            Else
                Items.Remove(pane)
            End If

            statusPane.SetStatus(Nothing, Nothing, DockAlignment.Left, 0.5)
            statusPane.SetDisplayingStatus(False, Nothing, DockAlignment.Left, 0.5)
            statusPane.SetDisplayingBounds(Rectangle.Empty, Rectangle.Empty, Rectangle.Empty)
        End Sub

        Public Function GetDefaultPreviousPane(pane As DockPane) As DockPane
            For i = Count - 1 To 0 Step -1
                If Me(i) IsNot pane Then Return Me(i)
            Next

            Return Nothing
        End Function
    End Class
End Namespace
