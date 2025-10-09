Imports Microsoft.VisualBasic.Imaging

Public Class ColorComboBox : Inherits ComboBox

    Sub New()
        Call MyBase.New()

        DrawMode = DrawMode.OwnerDrawFixed
        DropDownStyle = ComboBoxStyle.DropDownList

    End Sub

    Public ReadOnly Iterator Property Colors As IEnumerable(Of Color)
        Get
            If getColor Is Nothing Then
                For i As Integer = 0 To Items.Count - 1
                    Yield DirectCast(Items(i), Color)
                Next
            Else
                For i As Integer = 0 To Items.Count - 1
                    Yield getColor(Items(i))
                Next
            End If
        End Get
    End Property

    Public getColor As Func(Of Object, Color)
    Public getLabel As Func(Of Object, String)

    Private Sub ColorComboBox_DrawItem(sender As Object, e As DrawItemEventArgs) Handles Me.DrawItem
        e.DrawBackground()
        e.DrawFocusRectangle()

        If e.Index < 0 Then
            Return
        Else
            Try
                Call DrawItemInternal(e)
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub DrawItemInternal(e As DrawItemEventArgs)
        Dim color As Color = If(
            getColor Is Nothing,
            DirectCast(Items(e.Index), Color),
            getColor(Items(e.Index))
        )
        Dim brush As New SolidBrush(color)
        Dim g = e.Graphics
        Dim rect = e.Bounds

        rect.Inflate(-2, -2)

        Dim rectColor As New Rectangle(rect.Location, New Size(20, rect.Height))
        Dim label As String = If(
            getLabel Is Nothing,
            color.ToHtmlColor,
            getLabel(Items(e.Index))
        )

        g.DrawRectangle(New Pen(e.ForeColor), rectColor)
        g.FillRectangle(brush, rectColor)
        g.DrawString(label, e.Font, New SolidBrush(e.ForeColor), rect.X + 22, rect.Y)
    End Sub
End Class
