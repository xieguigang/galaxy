Imports System.Drawing
Imports System.Windows.Forms

Namespace PlugIns

    Friend Class PlugInManagerGUI
        Friend PlugInManager As PlugInManager

        Private Sub PlugInManagerGUI_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
            PlugInManager.DisabledPlugIns = PlugInManager.GetDisabledPlugIns(Me.ListView1, PlugInManager)
        End Sub

        Private Sub PlugInManagerGUI_Load(sender As Object, e As EventArgs) Handles Me.Load
            Call PlugInManager.LoadPlugins(Me.ListView1, Me.ImageList1)
        End Sub
    End Class
End Namespace