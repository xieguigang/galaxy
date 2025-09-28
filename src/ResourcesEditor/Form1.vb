Public Class Form1

    Private Sub OpenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenToolStripMenuItem.Click
        Using file As New OpenFileDialog With {.Filter = "Visual Basci Project(*.vbproj)|*.vbproj"}
            If file.ShowDialog = DialogResult.OK Then

            End If
        End Using
    End Sub
End Class
