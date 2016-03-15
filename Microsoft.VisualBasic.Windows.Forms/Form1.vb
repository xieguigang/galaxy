Imports Microsoft.VisualBasic.Windows.Forms.Gtk.CSSEngine

Public Class Form1

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        Dim css As CSSFile = CSSParser.ParseDoc("E:\Microsoft.VisualBasic.Windows.Forms\Arc-OSX\gtk-3.0\gtk-contained.css", "")
        css.ApplyWinForm(Me)
    End Sub
End Class
