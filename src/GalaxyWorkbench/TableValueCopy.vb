Imports System.Data

Public Class TableValueCopy

    ReadOnly matrix As DataGridView

    Sub New(matrix As DataGridView)
        Me.matrix = matrix
    End Sub

    ''' <summary>
    ''' make value copy from the <see cref="DataGridView"/> control to a given <see cref="DataTable"/>.
    ''' </summary>
    ''' <param name="grid"></param>
    Public Sub LoadTable(grid As DataTable)
        For i As Integer = 0 To matrix.Columns.Count - 1
            Call grid.Columns.Add(matrix.Columns(i).HeaderText, GetType(Object))
        Next

        For j As Integer = 0 To matrix.Rows.Count - 1
            Dim rowObj = matrix.Rows(j)
            Dim row As New List(Of Object)

            For i As Integer = 0 To rowObj.Cells.Count - 1
                Call row.Add(rowObj.Cells(i).Value)
            Next

            If row.All(Function(o) o Is Nothing OrElse IsDBNull(o)) Then
                Continue For
            End If

            Call grid.Rows.Add(row.ToArray)
        Next
    End Sub
End Class