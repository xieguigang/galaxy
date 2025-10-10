Imports System.Collections.Generic
Imports System.Collections.ObjectModel

Namespace WeifenLuo.WinFormsUI.Docking
    Public Class FloatWindowCollection
        Inherits ReadOnlyCollection(Of FloatWindow)
        Friend Sub New()
            MyBase.New(New List(Of FloatWindow)())
        End Sub

        Friend Function Add(fw As FloatWindow) As Integer
            If Items.Contains(fw) Then Return Items.IndexOf(fw)

            Items.Add(fw)
            Return Count - 1
        End Function

        Friend Sub Dispose()
            For i = Count - 1 To 0 Step -1
                Me(i).Close()
            Next
        End Sub

        Friend Sub Remove(fw As FloatWindow)
            Items.Remove(fw)
        End Sub

        Friend Sub BringWindowToFront(fw As FloatWindow)
            Items.Remove(fw)
            Items.Add(fw)
        End Sub
    End Class
End Namespace
