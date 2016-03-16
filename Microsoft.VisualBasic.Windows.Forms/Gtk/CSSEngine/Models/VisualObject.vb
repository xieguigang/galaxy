Namespace Gtk.CSSEngine.Models

    Public MustInherit Class VisualObject

        Public MustOverride Function Normal(ctrl As Control) As Image
        Public MustOverride Function Hover(ctrl As Control) As Image
        Public MustOverride Function Press(ctrl As Control) As Image
        Public MustOverride Function Active(ctrl As Control) As Image
        Public MustOverride Function Insensitive(ctrl As Control) As Image


    End Class
End Namespace