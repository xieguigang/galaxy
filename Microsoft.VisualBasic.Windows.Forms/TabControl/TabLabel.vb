Public Class TabLabel

    ''' <summary>
    ''' 和当前的这个标签所相关的Tab页面
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property TabPage As TabPage
    Public Overloads ReadOnly Property Container As TabControl

    Sub New(page As TabPage, container As TabControl)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.TabPage = page
        Me.Container = container
    End Sub

    Public Overloads Sub [Select]()
        Call Container.Select(Me)
    End Sub

    Public Overrides Function ToString() As String
        Return Text
    End Function
End Class
