Public Class FormMain

    Private Sub ToolStripMenuItem1_Click(sender As Object, e As EventArgs)
        Call New KeyGeneratorForm().ShowDialog
    End Sub

    Private Sub LicenseGeneratorToolStripMenuItem_Click(sender As Object, e As EventArgs)
        Call New LicenseGeneratorForm().ShowDialog
    End Sub
End Class
