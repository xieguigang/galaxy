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

    Public ReadOnly Property CanvasSize As Size
        Get
            Return ggplot.Size
        End Get
    End Property

    Private Sub ChartPad_Load(sender As Object, e As EventArgs) Handles Me.Load
        ggplot.Dock = DockStyle.Fill
        TabPage1.Controls.Add(ggplot)
    End Sub
End Class
