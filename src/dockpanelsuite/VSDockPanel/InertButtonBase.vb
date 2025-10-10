Imports System
Imports System.Windows.Forms
Imports System.Drawing
Imports System.Drawing.Imaging

Namespace WeifenLuo.WinFormsUI.Docking
    Public MustInherit Class InertButtonBase
        Inherits Control
        Protected Sub New()
            SetStyle(ControlStyles.SupportsTransparentBackColor, True)
            MyBase.BackColor = Color.Transparent
        End Sub

        Public MustOverride ReadOnly Property HoverImage As Bitmap

        Public MustOverride ReadOnly Property PressImage As Bitmap

        Public MustOverride ReadOnly Property Image As Bitmap

        Private m_isMouseOver As Boolean = False
        Protected Property IsMouseOver As Boolean
            Get
                Return m_isMouseOver
            End Get
            Private Set(value As Boolean)
                If m_isMouseOver = value Then Return

                m_isMouseOver = value
                Invalidate()
            End Set
        End Property

        Private m_isMouseDown As Boolean = False
        Protected Property IsMouseDown As Boolean
            Get
                Return m_isMouseDown
            End Get
            Private Set(value As Boolean)
                If m_isMouseDown = value Then Return

                m_isMouseDown = value
                Invalidate()
            End Set
        End Property

        Protected Overrides ReadOnly Property DefaultSize As Size
            Get
                Return New Size(16, 15)
            End Get
        End Property

        Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
            MyBase.OnMouseMove(e)
            Dim over = ClientRectangle.Contains(e.X, e.Y)
            If IsMouseOver <> over Then IsMouseOver = over
        End Sub

        Protected Overrides Sub OnMouseEnter(e As EventArgs)
            MyBase.OnMouseEnter(e)
            If Not IsMouseOver Then IsMouseOver = True
        End Sub

        Protected Overrides Sub OnMouseLeave(e As EventArgs)
            MyBase.OnMouseLeave(e)
            If IsMouseOver Then IsMouseOver = False
        End Sub

        Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
            MyBase.OnMouseLeave(e)
            If Not IsMouseDown Then IsMouseDown = True
        End Sub

        Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
            MyBase.OnMouseLeave(e)
            If IsMouseDown Then IsMouseDown = False
        End Sub

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            If HoverImage IsNot Nothing Then
                If IsMouseOver AndAlso Enabled Then
                    e.Graphics.DrawImage(If(IsMouseDown, PressImage, HoverImage), If(EnableHighDpi = True, ClientRectangle, New Rectangle(0, 0, Image.Width, Image.Height)))
                Else
                    e.Graphics.DrawImage(Image, If(EnableHighDpi = True, ClientRectangle, New Rectangle(0, 0, Image.Width, Image.Height)))
                End If

                MyBase.OnPaint(e)
                Return
            End If

            If IsMouseOver AndAlso Enabled Then
                Using pen As Pen = New Pen(MyBase.ForeColor)
                    e.Graphics.DrawRectangle(pen, Rectangle.Inflate(ClientRectangle, -1, -1))
                End Using
            End If

            Using imageAttributes As ImageAttributes = New ImageAttributes()
                Dim colorMap = New ColorMap(1) {}
                colorMap(0) = New ColorMap()
                colorMap(0).OldColor = Color.FromArgb(0, 0, 0)
                colorMap(0).NewColor = MyBase.ForeColor
                colorMap(1) = New ColorMap()
                colorMap(1).OldColor = Image.GetPixel(0, 0)
                colorMap(1).NewColor = Color.Transparent

                imageAttributes.SetRemapTable(colorMap)

                e.Graphics.DrawImage(Image, New Rectangle(0, 0, Image.Width, Image.Height), 0, 0, Image.Width, Image.Height, GraphicsUnit.Pixel, imageAttributes)
            End Using

            MyBase.OnPaint(e)
        End Sub

        Public Sub RefreshChanges()
            If IsDisposed Then Return

            Dim mouseOver = ClientRectangle.Contains(PointToClient(MousePosition))
            If mouseOver <> IsMouseOver Then IsMouseOver = mouseOver

            OnRefreshChanges()
        End Sub

        Protected Overridable Sub OnRefreshChanges()
        End Sub
    End Class
End Namespace
