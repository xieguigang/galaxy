Imports Microsoft.VisualBasic.Linq

Public Class TabControl

    Public ReadOnly Property TabPages As IEnumerable(Of TabPage)
        Get
            Return __labels.ToArray(Function(x) x.TabPage)
        End Get
    End Property

    ReadOnly __labels As New List(Of TabLabel)

    Public Property TabLabelSkin As Gtk.CSSEngine.Models.VisualObject
    Public ReadOnly Property Current As TabLabel

    Public Function Add(Name As String, page As TabPage) As TabLabel
        Dim label As New TabLabel(page, container:=Me) With {
            .Text = Name
        }
        Call __labels.Add(label)
        Call PanelPagesContainer.Controls.Add(page)

        page.Location = New Point
        page.Size = PanelPagesContainer.Size

        Return label
    End Function

    Public Overloads Sub [Select](label As TabLabel)
        If Not Current Is Nothing AndAlso Current.Equals(label) Then
            Return
        End If
    End Sub
End Class
