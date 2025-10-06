Namespace TableSheet.Events

    Friend Delegate Sub ColumnHeaderCellEventHandler(sender As Object, e As ColumnHeaderCellEventArgs)
    Friend Class ColumnHeaderCellEventArgs
        Inherits EventArgs

        Private _FilterMenu As MenuStrip, _Column As System.Windows.Forms.DataGridViewColumn

        Public Property FilterMenu As MenuStrip
            Get
                Return _FilterMenu
            End Get
            Private Set(value As MenuStrip)
                _FilterMenu = value
            End Set
        End Property

        Public Property Column As DataGridViewColumn
            Get
                Return _Column
            End Get
            Private Set(value As DataGridViewColumn)
                _Column = value
            End Set
        End Property

        Public Sub New(filterMenu As MenuStrip, column As DataGridViewColumn)
            Me.FilterMenu = filterMenu
            Me.Column = column
        End Sub
    End Class
End Namespace