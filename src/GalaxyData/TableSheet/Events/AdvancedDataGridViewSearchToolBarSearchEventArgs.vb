Namespace TableSheet.Events

    Public Delegate Sub AdvancedDataGridViewSearchToolBarSearchEventHandler(sender As Object, e As AdvancedDataGridViewSearchToolBarSearchEventArgs)

    Public Class AdvancedDataGridViewSearchToolBarSearchEventArgs : Inherits EventArgs

        Private _ValueToSearch As String, _ColumnToSearch As System.Windows.Forms.DataGridViewColumn, _CaseSensitive As Boolean, _WholeWord As Boolean, _FromBegin As Boolean

        Public Property ValueToSearch As String
            Get
                Return _ValueToSearch
            End Get
            Private Set(value As String)
                _ValueToSearch = value
            End Set
        End Property

        Public Property ColumnToSearch As DataGridViewColumn
            Get
                Return _ColumnToSearch
            End Get
            Private Set(value As DataGridViewColumn)
                _ColumnToSearch = value
            End Set
        End Property

        Public Property CaseSensitive As Boolean
            Get
                Return _CaseSensitive
            End Get
            Private Set(value As Boolean)
                _CaseSensitive = value
            End Set
        End Property

        Public Property WholeWord As Boolean
            Get
                Return _WholeWord
            End Get
            Private Set(value As Boolean)
                _WholeWord = value
            End Set
        End Property

        Public Property FromBegin As Boolean
            Get
                Return _FromBegin
            End Get
            Private Set(value As Boolean)
                _FromBegin = value
            End Set
        End Property

        Public Sub New(Value As String, Column As DataGridViewColumn, [Case] As Boolean, Whole As Boolean, fromBegin As Boolean)
            ValueToSearch = Value
            ColumnToSearch = Column
            CaseSensitive = [Case]
            WholeWord = Whole
            Me.FromBegin = fromBegin
        End Sub
    End Class
End Namespace