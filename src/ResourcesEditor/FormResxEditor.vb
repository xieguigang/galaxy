Public Class FormResxEditor

    Dim vbproj As String

    Private Sub OpenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenToolStripMenuItem.Click
        Using file As New OpenFileDialog With {.Filter = "Visual Basci Project(*.vbproj)|*.vbproj"}
            If file.ShowDialog = DialogResult.OK Then
                vbproj = file.FileName
                loadResourceFile()
            End If
        End Using
    End Sub

    Private Sub loadResourceFile()

    End Sub

    Private Sub FormResxEditor_Load(sender As Object, e As EventArgs) Handles Me.Load
        TextBox1.Visible = False
        TextBox1.Dock = DockStyle.Fill

        PictureBox1.Visible = False
        PictureBox1.Dock = DockStyle.Fill
    End Sub
End Class
