Imports System
Imports System.Windows.Forms
Imports System.Windows.Forms.Design
Imports System.Drawing

Namespace ThemeColorPicker
    <ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.All)>
    Public Class ThemeColorPickerToolStripSplitButton
        Inherits ToolStripSplitButton

        ''' <summary>
        ''' Occur when a color is selected.
        ''' </summary>
        Public Event ColorSelected(sender As Object, e As ColorSelectedArg)

        Private _imageWidth As Integer = 32
        Private _imageHeight As Integer = 16

        Public Property ImageWidth As Integer
            Get
                Return _imageWidth
            End Get
            Set(value As Integer)
                _imageWidth = value
            End Set
        End Property
        Public Property ImageHeight As Integer
            Get
                Return _imageHeight
            End Get
            Set(value As Integer)
                _imageHeight = value
            End Set
        End Property

        Private _img As Image
        Private _c As Color = Color.White
        Public Property Color As Color
            Get
                Return _c
            End Get
            Set(value As Color)
                _c = value
                If GetCurrentParent() IsNot Nothing Then
                    GetCurrentParent().SuspendLayout()
                End If
                DrawImage()
                If GetCurrentParent() IsNot Nothing Then
                    GetCurrentParent().ResumeLayout(False)
                    GetCurrentParent().PerformLayout()
                End If
                If ColorSelectedEvent IsNot Nothing Then
                    RaiseEvent ColorSelected(Me, New ColorSelectedArg(_c))
                End If
            End Set
        End Property

        Public Overrides Property Image As Image
            Get
                If _img Is Nothing Then
                    DrawImage()
                End If
                Return _img
            End Get
            Set(value As Image)
            End Set
        End Property

        Public Overrides Property DisplayStyle As ToolStripItemDisplayStyle
            Get
                Return ToolStripItemDisplayStyle.Image
            End Get
            Set(value As ToolStripItemDisplayStyle)
                ' do nothing
            End Set
        End Property
        Protected Overrides ReadOnly Property DefaultDisplayStyle As ToolStripItemDisplayStyle
            Get
                Return ToolStripItemDisplayStyle.Image
            End Get
        End Property

        Private f As ThemeColorPickerWindow
        Public Sub New()
            AutoSize = True
            ImageScaling = ToolStripItemImageScaling.None
            f = New ThemeColorPickerWindow(New Point(Bounds.Location.X, Bounds.Location.Y + Bounds.Height), FormBorderStyle.None, ThemeColorPickerWindow.Action.HideWindow, ThemeColorPickerWindow.Action.HideWindow)
            AddHandler f.ColorSelected, AddressOf f_ColorSelected
            AddHandler DropDownOpening, New EventHandler(AddressOf ThemeColorPickerToolStripSpliButton_DropDownOpening)
        End Sub

        Private Sub DrawImage()
            _img = New Bitmap(ImageWidth, ImageHeight)

            Using gfx = Graphics.FromImage(_img)
                Using brush As SolidBrush = New SolidBrush(Color.Black)
                    gfx.FillRectangle(brush, 0, 0, ImageWidth, ImageHeight)
                    brush.Color = Color
                    gfx.FillRectangle(brush, 1, 1, ImageWidth - 2, ImageHeight - 2)
                End Using
            End Using
        End Sub

        Private Sub f_ColorSelected(sender As Object, e As ColorSelectedArg)
            Color = e.Color
        End Sub

        Private Sub ThemeColorPickerToolStripSpliButton_DropDownOpening(sender As Object, e As EventArgs)
            Dim pt As Point = GetCurrentParent().PointToScreen(New Point(Bounds.Location.X, Bounds.Location.Y + Bounds.Height))
            f.Location = pt
            f.Show()
        End Sub

        Protected Overrides Sub OnButtonClick(e As EventArgs)
            Color = Color
            MyBase.OnButtonClick(e)
        End Sub
    End Class
End Namespace
