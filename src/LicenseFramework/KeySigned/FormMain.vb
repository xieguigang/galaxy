Imports KeySigned.LicenseFramework.Vendor

Public Class FormMain

    Private Sub ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem1.Click
        Call New KeyGeneratorForm().ShowDialog()
    End Sub

    Private Sub LicenseGeneratorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LicenseGeneratorToolStripMenuItem.Click
        Call New LicenseGeneratorForm().ShowDialog()
    End Sub
End Class
