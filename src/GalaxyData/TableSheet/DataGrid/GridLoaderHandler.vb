Imports System.Data
Imports System.Runtime.CompilerServices
Imports Galaxy.Data.TableSheet.Events

Namespace TableSheet

    Public Class GridLoaderHandler

        Dim memoryData As New DataSet
        Dim table As DataTable

        Dim AdvancedDataGridView1 As AdvancedDataGridView
        Dim BindingSource1 As BindingSource
        Dim dataSearch As GridSearchHandler

        Dim WithEvents AdvancedDataGridViewSearchToolBar1 As AdvancedDataGridViewSearchToolBar

        Sub New(grid As AdvancedDataGridView, toolbar As AdvancedDataGridViewSearchToolBar)
            Me.BindingSource1 = New BindingSource
            Me.AdvancedDataGridViewSearchToolBar1 = toolbar
            Me.AdvancedDataGridView1 = grid
            Me.dataSearch = New GridSearchHandler(grid)
        End Sub

        Public Sub ClearData()
            If Not table Is Nothing Then
                Dim copy = table

                Call LoadTable(
                    Sub(tbl)
                        For Each col As DataColumn In copy.Columns
                            Call tbl.Columns.Add(col.ColumnName, col.DataType)
                        Next
                    End Sub)
            Else
                AdvancedDataGridView1.DataSource = Nothing
                AdvancedDataGridView1.Rows.Clear()
            End If
        End Sub

        Public Sub LoadTable(apply As Action(Of DataTable))
            memoryData = New DataSet
            table = memoryData.Tables.Add("memoryData")

            Try
                Call Me.AdvancedDataGridView1.Columns.Clear()
                Call Me.AdvancedDataGridView1.Rows.Clear()
            Catch ex As Exception

            End Try

            Call apply(table)
            Call AdvancedDataGridView1.SetDoubleBuffered()

            For Each column As DataGridViewColumn In AdvancedDataGridView1.Columns
                'Select Case table.Columns.Item(column.HeaderText).DataType
                '    Case GetType(String)
                '        AdvancedDataGridView1.SetSortEnabled(column, True)
                '    Case GetType(Double)
                '    Case GetType(Integer)
                '    Case Else
                '        ' do nothing 
                'End Select

                ' AdvancedDataGridView1.ShowMenuStrip(column)
            Next

            BindingSource1.DataSource = memoryData
            BindingSource1.DataMember = table.TableName

            AdvancedDataGridView1.DataSource = BindingSource1
            AdvancedDataGridViewSearchToolBar1.SetColumns(AdvancedDataGridView1.Columns)
        End Sub

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Private Sub AdvancedDataGridViewSearchToolBar1_Search(sender As Object, e As AdvancedDataGridViewSearchToolBarSearchEventArgs) Handles AdvancedDataGridViewSearchToolBar1.Search
            Call dataSearch.AdvancedDataGridViewSearchToolBar1_Search(sender, e)
        End Sub

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Shared Function Search(table As AdvancedDataGridView) As AdvancedDataGridViewSearchToolBarSearchEventHandler
            Return AddressOf New GridSearchHandler(table).AdvancedDataGridViewSearchToolBar1_Search
        End Function
    End Class
End Namespace