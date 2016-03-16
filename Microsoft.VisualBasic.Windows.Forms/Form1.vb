Imports Microsoft.VisualBasic.Windows.Forms.Gtk.CSSEngine
Imports Microsoft.VisualBasic.Windows.Forms.Gtk.CSSEngine.Serialization

Public Class Form1

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        Dim css As CSSFile = CSSParser.ParseDoc("E:\Microsoft.VisualBasic.Windows.Forms\Arc-OSX\gtk-3.0\gtk-contained.css", "")

        Dim btn = New Models.CSSFile(css).Fill(Of Gtk.Controls.Button)



        '   Dim engine As LegacyEngine = New LegacyEngine(css)
        '    engine.ApplyWinForm(Me)
    End Sub

    Private Sub Form1_Leave(sender As Object, e As EventArgs) Handles Me.Leave

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim page = TabControl1.Add("test 1233")
        page = TabControl1.Add("test 1233")
        page = TabControl1.Add("test 1233 fffff")
        page = TabControl1.Add("test 12     @33")
        page = TabControl1.Add("test 1233")
        page = TabControl1.Add("test 1233 fffff")
        page = TabControl1.Add("test 12     @33")
        page = TabControl1.Add("test 1233")
        page = TabControl1.Add("test 1233 fffff")
        page = TabControl1.Add("test 12     @33")
    End Sub
End Class
