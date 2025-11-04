Public Class ChartPad

    Dim WithEvents ggplot As New ggviewer.PlotView

    Private Sub ChartPad_Load(sender As Object, e As EventArgs) Handles Me.Load
        ggplot.Dock = DockStyle.Fill
        TabPage1.Controls.Add(ggplot)
    End Sub
End Class
