Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Colors
Imports Microsoft.VisualBasic.Imaging.Driver

Public Class InputSelectColorMap

    Public ReadOnly Property GetColorMap As ScalerPalette
        Get
            Dim name As String = ComboBox1.SelectedItem.ToString
            Dim maps As ScalerPalette = Enums(Of ScalerPalette).Where(Function(c) c.Description = name).FirstOrDefault

            Return maps
        End Get
    End Property

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.DialogResult = DialogResult.Cancel
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.DialogResult = DialogResult.OK
    End Sub

    Private Sub InputSelectColorMap_Load(sender As Object, e As EventArgs) Handles Me.Load
        For Each scaler As ScalerPalette In Enums(Of ScalerPalette)()
            Call ComboBox1.Items.Add(scaler.Description)
        Next

        ComboBox1.SelectedIndex = 0
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Dim name As String = ComboBox1.SelectedItem.ToString
        Dim colors As SolidBrush() = Designer.GetColors(name, 30).Select(Function(c) New SolidBrush(c)).ToArray

        Using g As IGraphics = DriverLoad.CreateDefaultRasterGraphics(PictureBox1.Size, Color.Transparent)
            Dim dx As Double = g.Size.Width / colors.Length
            Dim x As Double
            Dim y As Double = 0
            Dim size As New SizeF(dx, g.Size.Height)
            Dim rect As RectangleF

            For i As Integer = 0 To colors.Length - 1
                x = dx * i + 1
                rect = New RectangleF(New PointF(x, y), size)

                Call g.FillRectangle(colors(i), rect)
            Next

            PictureBox1.BackgroundImage = DirectCast(g, GdiRasterGraphics).ImageResource
        End Using
    End Sub
End Class