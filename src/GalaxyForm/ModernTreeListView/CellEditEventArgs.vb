Namespace ModernTreeListView

    ''' <summary>
    ''' Event arguments for cell edit commit/cancel.
    ''' </summary>
    Public NotInheritable Class CellEditEventArgs(Of TModel)
        Inherits EventArgs
        Public ReadOnly Property Model As TModel
        Public ReadOnly Property Column As TreeListColumn(Of TModel)
        Public Property ProposedValue As Object
        Public ReadOnly Property OriginalValue As Object
        Public ReadOnly Property RowIndex As Integer
        Public ReadOnly Property ColumnIndex As Integer
        Public Property Cancel As Boolean

        Public Sub New(model As TModel, column As TreeListColumn(Of TModel), proposedValue As Object, originalValue As Object, rowIndex As Integer, columnIndex As Integer)
            Me.Model = model
            Me.Column = column
            Me.ProposedValue = proposedValue
            Me.OriginalValue = originalValue
            Me.RowIndex = rowIndex
            Me.ColumnIndex = columnIndex
        End Sub
    End Class

End Namespace