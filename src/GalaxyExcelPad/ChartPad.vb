Public Class ChartPad

    Dim WithEvents ggplot As New ggviewer.PlotView

    ''' <summary>
    ''' get/set of the ggplot chart plot image
    ''' </summary>
    ''' <returns></returns>
    Public Overrides Property BackgroundImage As Image
        Get
            Return ggplot.BackgroundImage
        End Get
        Set(value As Image)
            ggplot.BackgroundImage = value
        End Set
    End Property

    Public Overrides Property BackgroundImageLayout As ImageLayout
        Get
            Return ggplot.BackgroundImageLayout
        End Get
        Set(value As ImageLayout)
            ggplot.BackgroundImageLayout = value
        End Set
    End Property

    Public ReadOnly Property CanvasSize As Size
        Get
            Return ggplot.Size
        End Get
    End Property

    Private Sub ChartPad_Load(sender As Object, e As EventArgs) Handles Me.Load
        ggplot.Dock = DockStyle.Fill
        TabPage1.Controls.Add(ggplot)
    End Sub

    Public ReadOnly Property DataSource As BindingSource
        Get
            Return BindingSource1
        End Get
    End Property

    Public ReadOnly Property Table As DataGridView
        Get
            Return DataGridView1
        End Get
    End Property

    Public Sub ShowRPlot()
        TabControl1.SelectedTab = TabPage1
    End Sub

    Public Sub ShowMatrix()
        TabControl1.SelectedTab = TabPage2
    End Sub
End Class
