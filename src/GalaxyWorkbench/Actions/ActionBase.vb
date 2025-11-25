Imports System.Data

Namespace Actions

    ''' <summary>
    ''' run a specific action on a data table
    ''' </summary>
    Public MustInherit Class ActionBase

        Public MustOverride ReadOnly Property Description As String

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="fieldName">
        ''' the main data field name in <paramref name="table"/>
        ''' </param>
        ''' <param name="data">the corresponding column value of the data field <paramref name="fieldName"/></param>
        ''' <param name="table">the data table for get additional field values.</param>
        Public MustOverride Sub RunAction(fieldName As String, data As Array, table As DataTable)

        Protected Shared Iterator Function GetFieldNames(table As DataTable) As IEnumerable(Of String)
            For i As Integer = 0 To table.Columns.Count - 1
                Yield table.Columns.Item(i).ColumnName
            Next
        End Function

    End Class
End Namespace