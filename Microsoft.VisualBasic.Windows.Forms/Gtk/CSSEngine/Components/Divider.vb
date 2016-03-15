Imports System.Drawing
Imports System.Windows.Forms

Namespace Gtk.CSSEngine.Components

    Public Class DividerLabel
        Inherits System.Windows.Forms.Label

        Dim m_spacing As Integer
        Dim m_borderStyle As Border3DStyle = Border3DStyle.Etched

        <System.ComponentModel.Category("Appearance")>
        Public Property LineStyle() As Border3DStyle
            Get
                Return m_borderStyle
            End Get
            Set(Value As Border3DStyle)
                If Value <> m_borderStyle Then
                    m_borderStyle = Value
                    Me.Invalidate()
                End If
            End Set
        End Property

        <System.ComponentModel.Category("Appearance")>
        Public Property Spacing() As Integer
            Get
                Return m_spacing
            End Get
            Set(Value As Integer)
                If Value <> m_spacing Then
                    m_spacing = Value
                    Me.Invalidate()
                End If

            End Set
        End Property

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            Dim g As Graphics = e.Graphics
            Dim f As Font = Me.Font
            Dim b As Brush = New SolidBrush(Me.ForeColor)

            Dim sf As StringFormat = StringFormat.GenericTypographic
            Dim labelBounds As New RectangleF(0, 0, Me.Width, Me.Height)
            Dim textSize As SizeF = g.MeasureString(Me.Text, f, Me.Width)

            g.DrawString(Me.Text, f, b, 0, 0, sf)

            If textSize.Width + Spacing < Me.Width Then
                Dim startingPoint As New Point(CInt(textSize.Width) + Spacing, CInt(CLng(textSize.Height) \ 2))
                ControlPaint.DrawBorder3D(g, startingPoint.X, startingPoint.Y, Me.Width - startingPoint.X, 5, m_borderStyle, Border3DSide.Top)
            End If

        End Sub

        Public Sub New()
            Me.SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
            Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)
            Me.SetStyle(ControlStyles.ResizeRedraw, True)
            Me.AutoSize = False
        End Sub
    End Class
End Namespace