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

    Protected Overrides Sub OnPrint(e As PaintEventArgs)
        MyBase.OnPrint(e)
        e.Graphics.DrawString(Text, Font, New SolidBrush(ForeColor), New Point(5, 5))
    End Sub

    Private Sub TabLabel_MouseClick(sender As Object, e As MouseEventArgs) Handles Me.MouseClick
        Call [Select]()
        Call Container.TabIndicator1.SetPage(Location.X)
    End Sub
End Class
