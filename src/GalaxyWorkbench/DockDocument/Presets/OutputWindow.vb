Imports Microsoft.VisualBasic.ApplicationServices.Debugging.Logging
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
                {"level", CStr(target.Tag)},
                {"time", CStr(target.Cells(1).Value)},
                {"action", CStr(target.Cells(2).Value)},
                {"message", CStr(target.Cells(3).Value)}
            }

            Clipboard.SetText(json.GetJson)
        End Sub

        Private Sub CopyMessageToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyMessageToolStripMenuItem.Click
            If DataGridView1.SelectedRows.Count = 0 Then
                Return
            End If

            Dim target As DataGridViewRow = DataGridView1.SelectedRows(0)
            Dim msg As String = CStr(target.Cells(3).Value)

            Clipboard.SetText(msg)
        End Sub

        ''' <summary>
        ''' thread-safe append log text
        ''' </summary>
        ''' <param name="text"></param>
        Public Sub AppendLine(text As String)
            Call Invoke(Sub() TextBox1.AppendText(text & vbCrLf))
        End Sub

        ''' <summary>
        ''' thread-safe add log entry
        ''' </summary>
        ''' <param name="time"></param>
        ''' <param name="action"></param>
        ''' <param name="message"></param>
        ''' <param name="level"></param>
        Public Sub AddLog(time As Date, action As String, message As String, Optional level As MSG_TYPES = MSG_TYPES.INF)
            Dim icon As Image = Nothing

            Select Case level
                Case MSG_TYPES.DEBUG : icon = My.Resources.Icons8.icons8_debug_96
                Case MSG_TYPES.ERR : icon = My.Resources.Icons8.icons8_error_96
                Case MSG_TYPES.INF : icon = My.Resources.Icons8.icons8_information_96
                Case MSG_TYPES.WRN : icon = My.Resources.Icons8.icons8_warning_96
                Case Else
                    icon = My.Resources.Icons8.icons8_done_144
            End Select

            Call Invoke(Sub()
                            Dim offset As Integer = DataGridView1.Rows.Add(icon, time.ToString("HH:mm:ss"), action, message)
                            Dim row As DataGridViewRow = DataGridView1.Rows(offset)

                            row.Tag = level.Description
                            DataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit)
                        End Sub)
        End Sub
    End Class
End Namespace