Imports Microsoft.VisualBasic.Serialization.JSON

Namespace DockDocument.Presets

    Public Class OutputWindow

        Private Sub OutputWindow_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            ToolStripComboBox1.SelectedIndex = 0

            Call ApplyVsTheme(ToolStrip1, ContextMenuStrip1)
        End Sub

        Private Sub ToolStripComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ToolStripComboBox1.SelectedIndexChanged
            If ToolStripComboBox1.SelectedIndex = 0 Then
                TextBox1.Visible = False
                DataGridView1.Visible = True
            Else
                TextBox1.Visible = True
                DataGridView1.Visible = False
            End If
        End Sub

        Private Sub CopyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyToolStripMenuItem.Click
            If DataGridView1.SelectedRows.Count = 0 Then
                Return
            End If

            Dim target As DataGridViewRow = DataGridView1.SelectedRows(0)
            Dim json As New Dictionary(Of String, String) From {
                {"time", CStr(target.Cells(0).Value)},
                {"action", CStr(target.Cells(1).Value)},
                {"message", CStr(target.Cells(2).Value)}
            }

            Clipboard.SetText(json.GetJson)
        End Sub

        Private Sub CopyMessageToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyMessageToolStripMenuItem.Click
            If DataGridView1.SelectedRows.Count = 0 Then
                Return
            End If

            Dim target As DataGridViewRow = DataGridView1.SelectedRows(0)
            Dim msg As String = CStr(target.Cells(2).Value)

            Clipboard.SetText(msg)
        End Sub

        Public Sub AppendLine(text As String)
            Call Invoke(Sub() TextBox1.AppendText(text & vbCrLf))
        End Sub

        Public Sub AddLog(time As Date, action As String, message As String)
            Call Invoke(Sub() DataGridView1.Rows.Add(time.ToString("HH:mm:ss"), action, message))
        End Sub
    End Class
End Namespace