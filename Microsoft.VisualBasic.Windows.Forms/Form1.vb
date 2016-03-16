Imports Microsoft.VisualBasic.Windows.Forms.Gtk.CSSEngine
Imports Microsoft.VisualBasic.Windows.Forms.Gtk.CSSEngine.Serialization

Public Class Form1

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        Dim css As CSSFile = CSSParser.ParseDoc("E:\Microsoft.VisualBasic.Windows.Forms\Arc-OSX\gtk-3.0\gtk-contained.css", "")

        Dim btn = css.Fill(Of Gtk.Controls.Button)



        Dim engine As LegacyEngine = New LegacyEngine(css)
        engine.ApplyWinForm(Me)
    End Sub
End Class
