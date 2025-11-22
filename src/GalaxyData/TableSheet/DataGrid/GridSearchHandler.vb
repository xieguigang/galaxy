Imports System.Runtime.CompilerServices
Imports Galaxy.Data.TableSheet.Events

<Assembly: InternalsVisibleTo("ExcelPad")>

Namespace TableSheet

    Friend Class GridSearchHandler

        ReadOnly AdvancedDataGridView1 As AdvancedDataGridView

        Sub New(grid As AdvancedDataGridView)
            Me.AdvancedDataGridView1 = grid
        End Sub

        ''' <summary>
        ''' Handles AdvancedDataGridViewSearchToolBar1.Search
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Public Sub AdvancedDataGridViewSearchToolBar1_Search(sender As Object, e As AdvancedDataGridViewSearchToolBarSearchEventArgs)
            Dim restartsearch = True
            Dim startColumn = 0
            Dim startRow = 0

            If Not e.FromBegin Then
                Dim endcol = AdvancedDataGridView1.CurrentCell.ColumnIndex + 1 >= AdvancedDataGridView1.ColumnCount
                Dim endrow = AdvancedDataGridView1.CurrentCell.RowIndex + 1 >= AdvancedDataGridView1.RowCount

                If endcol AndAlso endrow Then
                    startColumn = AdvancedDataGridView1.CurrentCell.ColumnIndex
                    startRow = AdvancedDataGridView1.CurrentCell.RowIndex
                Else
                    startColumn = If(endcol, 0, AdvancedDataGridView1.CurrentCell.ColumnIndex + 1)
                    startRow = AdvancedDataGridView1.CurrentCell.RowIndex + If(endcol, 1, 0)
                End If
            End If

            Dim c = AdvancedDataGridView1.FindCell(e.ValueToSearch, If(e.ColumnToSearch IsNot Nothing, e.ColumnToSearch.Name, Nothing), startRow, startColumn, e.WholeWord, e.CaseSensitive)

            If c Is Nothing AndAlso restartsearch Then
                c = AdvancedDataGridView1.FindCell(e.ValueToSearch, If(e.ColumnToSearch IsNot Nothing, e.ColumnToSearch.Name, Nothing), 0, 0, e.WholeWord, e.CaseSensitive)
            End If

            If c IsNot Nothing Then
                AdvancedDataGridView1.CurrentCell = c
            End If
        End Sub
    End Class
End Namespace