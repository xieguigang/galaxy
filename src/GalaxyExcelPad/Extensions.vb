Imports Galaxy.Workbench
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.ComponentModel.Collection.Generic
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Data.Framework
Imports Microsoft.VisualBasic.Data.Framework.IO
Imports Microsoft.VisualBasic.Language

<HideModuleName>
Public Module Extensions

    Public Sub ShowTable(Of T As {INamedValue, DynamicPropertyBase(Of String)})(table As IEnumerable(Of T), title As String)
        Call ShowTable(DataFrameResolver.CreateObject(table.ToCsvDoc), title)
    End Sub

    Public Sub ShowTable(dataframe As DataFrameResolver, title As String)
        Call CommonRuntime _
            .ShowDocument(Of FormExcelPad)(title:=title) _
            .LoadTable(Sub(grid)
                           Call loadInternal(grid, dataframe)
                       End Sub)
    End Sub

    Private Sub loadInternal(grid As DataTable, dataframe As DataFrameResolver)
        Dim schema As New List(Of Type)
        Dim i As i32 = Scan0

        For Each name As String In dataframe.HeadTitles
            Dim v As String() = dataframe.Column(++i).ToArray
            Dim type As Type = v.SampleForType

            Call schema.Add(type)
            grid.Columns.Add(name, type)
        Next

        For Each item As RowObject In dataframe.Rows
            Dim values As Object() = item _
                .Select(Function(str, idx) As Object
                            Select Case schema(idx)
                                Case GetType(Double) : Return Val(str)
                                Case GetType(Integer) : Return str.ParseInteger
                                Case GetType(Boolean) : Return str.ParseBoolean
                                Case GetType(Date) : Return str.ParseDate
                                Case Else
                                    Return CObj(str)
                            End Select
                        End Function) _
                .ToArray
            Dim row = grid.Rows.Add(values)
        Next
    End Sub
End Module
