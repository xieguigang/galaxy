Imports Microsoft.VisualBasic.Windows.Forms.Gtk.CSSEngine.Serialization

Namespace Gtk.CSSEngine.Controls

    Public MustInherit Class Control

        <[Property]("background-color")> Public Property backColor As Color
    End Class

    Public Class Label : Inherits Control

    End Class

    <GtkObject(".button")>
    Public Class Button : Inherits Control

    End Class
End Namespace