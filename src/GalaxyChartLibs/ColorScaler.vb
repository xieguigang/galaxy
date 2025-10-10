Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Colors
Imports Microsoft.VisualBasic.Imaging.Driver

''' <summary>
''' the common heatmap color scaler control
''' </summary>
Public Class ColorScaler

    Dim colorSet As ScalerPalette = ScalerPalette.FlexImaging
    Dim mapLevels As Integer = 250
    Dim intensityMax As Double = 10 ^ 6
    Dim maxWidthOfColorBar As Integer = 30

    Public ReadOnly Property ColorBarWidth As Integer
        Get
            Return Width - 80
        End Get
    End Property

    Public Property ScalerPalette As ScalerPalette
        Get
            Return colorSet
        End Get
        Set(value As ScalerPalette)
            colorSet = value
            UpdateColors(callEvents:=False)
        End Set
    End Property

    Public Property ScalerLevels As Integer
        Get
            Return mapLevels
        End Get
        Set(value As Integer)
            mapLevels = value
            UpdateColors(callEvents:=False)
        End Set
    End Property

    Public ReadOnly Property MenuUI As ContextMenuStrip
        Get
            Return ContextMenuStrip1
        End Get
    End Property

    ''' <summary>
    ''' range in [0,1]
    ''' </summary>
    ''' <returns></returns>
    Public Property ScalerRange As DoubleRange
        Get
            Dim h As Integer = Height
            Dim up = 1 - picUpperbound.Bottom / h
            Dim lower = 1 - picLowerbound.Top / h

            Return New DoubleRange(lower, up)
        End Get
        Set(value As DoubleRange)
            Dim h As Integer = Height
            Dim upperBottom As Integer = (1 - value.Max) * h
            Dim lowerTop As Integer = (1 - value.Min) * h

            picUpperbound.Location = New Point(1, upperBottom - 10)
            picLowerbound.Location = New Point(1, lowerTop)

            UpdateColors(callEvents:=False)
            ResizeColorBar()
        End Set
    End Property

    Public Event SetRange(range As DoubleRange)
    Public Event RequestSetCustomRange()

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    <DebuggerStepThrough>
    Public Sub SetIntensityMax(max As Double)
        intensityMax = max
    End Sub

    Public Sub ResetScaleRange()
        Dim width = ColorBarWidth

        picUpperbound.Location = New Point(1, 1)
        picUpperbound.Size = New Size(width - 2, 10)
        picLowerbound.Location = New Point(1, Height - 10)
        picLowerbound.Size = New Size(width - 2, 10)

        PictureBox1.Location = New Point(1, picUpperbound.Location.Y + 12)
        PictureBox1.Size = New Size(width - 2, picLowerbound.Top - picUpperbound.Bottom - 5)

        Call UpdateColors(callEvents:=True)
    End Sub

    Public Sub UpdateColors(callEvents As Boolean)
        ' 绘制坐标轴
        BackgroundImage = DrawIntensityAxis()
        ' 绘制颜色条
        PictureBox1.BackgroundImage = DrawByColors(Designer.GetColors(colorSet.Description, mapLevels))

        If callEvents Then
            Try
                RaiseEvent SetRange(ScalerRange)
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Function DrawIntensityAxis() As Image
        Dim w As Double = ColorBarWidth
        Dim axisTicks = New DoubleRange(0, intensityMax).CreateAxisTicks
        Dim height As Double = Me.Height
        Dim d As Double = height / mapLevels
        Dim y As Double = 0

        Using g As IGraphics = DriverLoad.CreateDefaultRasterGraphics(Me.Size, Color.Transparent)
            height -= 20
            ' w -= 20

            ' 绘制坐标轴
            Dim scaleY As New YScaler(False) With {
                .Y = d3js.scale.linear.domain(values:=axisTicks).range(values:={10, height}),
                .region = New Rectangle(0, 0, 0, height)
            }
            Dim a As New PointF(w, 10)
            Dim b As New PointF(w, height)
            Dim pen As New Pen(Color.Black, 3)
            Dim font As New Font(FontFace.MicrosoftYaHeiUI, 7, FontStyle.Italic)
            Dim fh = g.MeasureString("0", font).Height / 2

            g.DrawLine(pen, a, b)
            pen = New Pen(Color.Black, 2)

            For Each tick As Double In axisTicks.Take(axisTicks.Length - 1)
                y = scaleY.TranslateY(tick)
                a = New PointF(w, y)
                b = New PointF(w + 5, y)
                g.DrawLine(pen, a, b)
                g.DrawString(tick.ToString("G3"), font, Brushes.Black, New PointF(w + 8, y - fh))
            Next

            Return DirectCast(g, GdiRasterGraphics).ImageResource
        End Using
    End Function

    Private Function DrawByColors(colors As Color()) As Image
        Dim height As Double = Me.Height
        Dim d As Double = height / colors.Length
        Dim y As Double = 0
        Dim w As Double = Me.Width

        Using g As IGraphics = DriverLoad.CreateDefaultRasterGraphics(Me.Size, Color.Transparent)
            For Each c As Color In colors.Reverse
                Call g.FillRectangle(New SolidBrush(c), New RectangleF(New PointF(0, y), New SizeF(w, d)))
                y += d
            Next

            Return DirectCast(g, GdiRasterGraphics).ImageResource
        End Using
    End Function

    Private Sub ColorScaler_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call ResetScaleRange()
    End Sub

    Private Sub ColorScaler_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        If Me.Width < 100 Then
            Me.Width = 100
        End If

        Dim width = ColorBarWidth

        picUpperbound.Size = New Size(width - 2, 10)
        picLowerbound.Size = New Size(width - 2, 10)

        Call UpdateColors(callEvents:=False)
    End Sub

    Dim moveUp, moveDown As Boolean
    Dim mousePos As Point

    Private Sub picUpperbound_MouseDown(sender As Object, e As MouseEventArgs) Handles picUpperbound.MouseDown
        moveUp = True
        mousePos = e.Location
    End Sub

    Private Sub picLowerbound_MouseDown(sender As Object, e As MouseEventArgs) Handles picLowerbound.MouseDown
        moveDown = True
        mousePos = e.Location
    End Sub

    Private Sub picUpperbound_MouseMove(sender As Object, e As MouseEventArgs) Handles picUpperbound.MouseMove
        If moveUp Then
            Dim oldPos = picUpperbound.Location
            Dim delta = e.Y - mousePos.Y
            Dim newPos As New Point(oldPos.X, oldPos.Y + delta)
            Dim width = ColorBarWidth

            If newPos.Y > picLowerbound.Top OrElse newPos.Y < 10 Then
                Return
            End If

            mousePos = e.Location
            picUpperbound.Location = newPos

            Call ResizeColorBar()
        End If
    End Sub

    Private Sub picLowerbound_MouseMove(sender As Object, e As MouseEventArgs) Handles picLowerbound.MouseMove
        If moveDown Then
            Dim oldPos = picLowerbound.Location
            Dim delta = e.Y - mousePos.Y
            Dim newPos As New Point(oldPos.X, oldPos.Y + delta)
            Dim width = ColorBarWidth

            If newPos.Y < picUpperbound.Bottom OrElse newPos.Y > Height - 10 Then
                Return
            End If

            mousePos = e.Location
            picLowerbound.Location = newPos

            Call ResizeColorBar()
        End If
    End Sub

    Private Sub ResizeColorBar()
        PictureBox1.Location = New Point(1, picUpperbound.Location.Y + 12)
        PictureBox1.Size = New Size(ColorBarWidth - 2, picLowerbound.Top - picUpperbound.Bottom - 5)
    End Sub

    Private Sub picUpperbound_MouseUp(sender As Object, e As MouseEventArgs) Handles picUpperbound.MouseUp
        moveUp = False
        Call UpdateColors(callEvents:=True)
    End Sub

    Private Sub ResetToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ResetToolStripMenuItem.Click
        'ScalerRange = {0.0, 1.0}
        'RaiseEvent SetRange(ScalerRange)
        Call ResetScaleRange()
    End Sub

    Private Sub picLowerbound_MouseUp(sender As Object, e As MouseEventArgs) Handles picLowerbound.MouseUp
        moveDown = False
        Call UpdateColors(callEvents:=True)
    End Sub

    Private Sub SetRangeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SetRangeToolStripMenuItem.Click
        Try
            RaiseEvent RequestSetCustomRange()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub PictureBox1_MouseHover(sender As Object, e As EventArgs) Handles PictureBox1.MouseHover
        Dim h As Double = PictureBox1.PointToClient(MousePosition).Y
        Dim dh As Double = PictureBox1.Height - h
        Dim into As Double = dh / PictureBox1.Height * intensityMax

        Call ToolTip1.SetToolTip(PictureBox1, $"Intensity {into.ToString("G3")}, max intensity {intensityMax.ToString("G3")}")
    End Sub
End Class
