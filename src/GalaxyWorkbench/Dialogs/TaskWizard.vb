Public Class TaskWizard

    Public Property TaskName As String
        Get
            Return GroupBox1.Text
        End Get
        Set(value As String)
            GroupBox1.Text = value
        End Set
    End Property

End Class