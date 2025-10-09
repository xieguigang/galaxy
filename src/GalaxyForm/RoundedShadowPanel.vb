Imports System.Drawing.Drawing2D

Public Class RoundedShadowPanel : Inherits Panel

    Public Property CornerRadius As Integer = 9
    Public Property ShadowSize As Integer = 12
    Public Property ShadowColor As Color = Color.FromArgb(80, 0, 0, 0)
    Public Property FillColor As Color = Color.White

    Dim m_innerPadding As New Padding(20)

    Public Property InnerPadding As Padding
        Get
            Return m_innerPadding
        End Get
        Set(value As Padding)
            m_innerPadding = value
            UpdateEffectivePadding()
        End Set
    End Property

    Public Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.SupportsTransparentBackColor, True)

        BackColor = Color.Transparent
        AutoSize = True
        AutoSizeMode = AutoSizeMode.GrowAndShrink

        Anchor = AnchorStyles.Left Or AnchorStyles.Right

        UpdateEffectivePadding()
    End Sub

    Private Sub UpdateEffectivePadding()
        Padding = New Padding(ShadowSize + m_innerPadding.Left, ShadowSize + m_innerPadding.Top, ShadowSize + m_innerPadding.Right, ShadowSize + m_innerPadding.Bottom)

        Invalidate()
        PerformLayout()
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)

        Dim gfx = e.Graphics
        Dim shadowRect As New RectangleF(CSng(ShadowSize / 2), CSng(ShadowSize / 2), Width - ShadowSize, Height - ShadowSize)

        gfx.SmoothingMode = SmoothingMode.AntiAlias

        Using path = GetRoundedRect(shadowRect, CornerRadius)
            Using brush As New PathGradientBrush(path)
                brush.CenterColor = ShadowColor
                brush.SurroundColors = {Color.FromArgb(0, ShadowColor)}
                gfx.FillPath(brush, path)
            End Using
        End Using

        Dim panelRect As New Rectangle(0, 0, Width - ShadowSize, Height - ShadowSize)

        Using panelPath = GetRoundedRect(panelRect, CornerRadius)
            Using fillBrush As SolidBrush = New SolidBrush(FillColor)
                gfx.FillPath(fillBrush, panelPath)
            End Using
        End Using
    End Sub

    Private Function GetRoundedRect(bounds As RectangleF, radius As Integer) As GraphicsPath
        Dim path As New GraphicsPath()
        Dim d = radius * 2
        path.AddArc(bounds.X, bounds.Y, d, d, 180, 90)
        path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90)
        path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90)
        path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90)
        path.CloseFigure()
        Return path
    End Function
End Class
