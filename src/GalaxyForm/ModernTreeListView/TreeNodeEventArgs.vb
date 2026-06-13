Namespace ModernTreeListView

    ''' <summary>
    ''' Event args for tree node expand/collapse/check notifications.
    ''' </summary>
    Public NotInheritable Class TreeNodeEventArgs(Of TModel)
        Inherits EventArgs
        Public ReadOnly Property Model As TModel
        Friend ReadOnly Property Node As Object ' internal for future use

        Public Sub New(model As TModel, node As Object)
            Me.Model = model
            Me.Node = node
        End Sub
    End Class

End Namespace