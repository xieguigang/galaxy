Namespace ModernTreeListView

    Public Class TreeListViewItem(Of T As TreeListViewItem(Of T))

        Public Property Text As String
        Public Property Tag As Object
        Public Property Items As New List(Of T)
        Public Property Parent As TreeListViewItem(Of T)
        Public Property ImageIndex As Integer
        Public Property ToolTipText As String

        Public ReadOnly Property ChildrenCount As Integer
            Get
                Return Items.Count
            End Get
        End Property

    End Class
End Namespace