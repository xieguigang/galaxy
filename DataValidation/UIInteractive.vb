Imports System.Runtime.CompilerServices
Imports System.Windows.Forms

Public Module UIInteractive

    <Extension>
    Public Sub SetAutoSelectRow(dv As DataGridView)
        AddHandler dv.CellClick,
            Sub(sender, evt)
                If evt.RowIndex >= 0 AndAlso evt.RowIndex < dv.Rows.Count Then
                    dv.Rows(evt.RowIndex).Selected = True
                End If
            End Sub
    End Sub
End Module
