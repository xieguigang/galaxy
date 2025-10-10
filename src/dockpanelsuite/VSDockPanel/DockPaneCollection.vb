Imports System.Collections.ObjectModel
Imports System.Collections.Generic

Namespace Docking
    Public Class DockPaneCollection
        Inherits ReadOnlyCollection(Of DockPane)
        Friend Sub New()
            MyBase.New(New List(Of DockPane)())
        End Sub

        Friend Function Add(pane As DockPane) As Integer
            If Items.Contains(pane) Then Return Items.IndexOf(pane)

            Items.Add(pane)
            Return Count - 1
        End Function

        Friend Sub AddAt(pane As DockPane, index As Integer)
            If index < 0 OrElse index > Items.Count - 1 Then Return

            If Contains(pane) Then Return

            Items.Insert(index, pane)
        End Sub

        Friend Sub Dispose()
            If EnableNestedDisposalFix = True Then
                Dim collection As List(Of DockPane) = New List(Of DockPane)(Items)
                For Each dockPane In collection
                    dockPane.Close()
                Next

                collection.Clear()
                Return
            End If

            For i = Count - 1 To 0 Step -1
                Me(i).Close()
            Next
        End Sub

        Friend Sub Remove(pane As DockPane)
            Items.Remove(pane)
        End Sub
    End Class
End Namespace
