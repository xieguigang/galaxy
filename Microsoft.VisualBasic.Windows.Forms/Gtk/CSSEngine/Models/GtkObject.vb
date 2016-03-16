Imports Microsoft.VisualBasic.ComponentModel.Collection.Generic

Namespace Gtk.CSSEngine.Models

    Public Class GtkObject : Implements sIdEnumerable

        Public Property Identifier As String Implements sIdEnumerable.Identifier
        Public Property Properties As CSSProperty()

        Sub New(name As String, data As CSSProperty())
            Identifier = name
            Properties = data
        End Sub

        Public Overrides Function ToString() As String
            Return Identifier
        End Function
    End Class
End Namespace