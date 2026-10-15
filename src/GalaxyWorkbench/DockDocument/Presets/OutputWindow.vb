Imports System.IO
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ApplicationServices.Debugging.Logging
Imports Microsoft.VisualBasic.Serialization.JSON

Namespace DockDocument.Presets

    Public Class OutputWindow

        ''' <summary>
        ''' the fixed grid (DataGridView) output, always present at combo index 0
        ''' </summary>
        Private Const ActionOutputName As String = "Action Output"

        ''' <summary>
        ''' default configurable text (TextBox) output name
        ''' </summary>
        Public Const LogTextName As String = "Log Text"

        ''' <summary>
        ''' configurable text outputs: each name maps to its buffered lines.
        ''' </summary>
        Private ReadOnly _textOutputs As New Dictionary(Of String, List(Of String))

        ''' <summary>
        ''' lock guarding access to <see cref="_textOutputs"/> from background threads
        ''' </summary>
        Private ReadOnly _textLock As New Object

        Private Sub OutputWindow_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            If Not _textOutputs.ContainsKey(LogTextName) Then
                _textOutputs.Add(LogTextName, New List(Of String))
            End If

            Call SyncComboBox()
            Call ApplyVsTheme(ToolStrip1, ContextMenuStrip1)

            ToolStripComboBox1.SelectedIndex = 0
        End Sub

        ''' <summary>
        ''' rebuild the dropdown: the fixed grid output plus every configurable text output.
        ''' preserves the current selection where possible.
        ''' </summary>
        Private Sub SyncComboBox()
            Dim current As String = CStr(ToolStripComboBox1.SelectedItem)

            ToolStripComboBox1.Items.Clear()
            ToolStripComboBox1.Items.Add(ActionOutputName)

            SyncLock _textLock
                For Each name As String In _textOutputs.Keys
                    ToolStripComboBox1.Items.Add(name)
                Next
            End SyncLock

            If current Is Nothing Then
                ToolStripComboBox1.SelectedIndex = 0
            Else
                Dim idx As Integer = ToolStripComboBox1.Items.IndexOf(current)
                ToolStripComboBox1.SelectedIndex = If(idx >= 0, idx, 0)
            End If
        End Sub

        Private Sub ToolStripComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ToolStripComboBox1.SelectedIndexChanged
            If ToolStripComboBox1.SelectedItem Is Nothing Then
                Return
            End If

            Dim name As String = CStr(ToolStripComboBox1.SelectedItem)

            If name = ActionOutputName Then
                TextBox1.Visible = False
                TextBox1.Dock = DockStyle.None
                DataGridView1.Visible = True
                DataGridView1.Dock = DockStyle.Fill
            Else
                DataGridView1.Visible = False
                DataGridView1.Dock = DockStyle.None
                TextBox1.Visible = True
                TextBox1.Dock = DockStyle.Fill

                ' restore the buffered lines of the selected text output
                Call RestoreTextBox(name)
            End If
        End Sub

        ''' <summary>
        ''' render the buffered <see cref="List(Of String)"/> of a text output back into the TextBox.
        ''' </summary>
        Private Sub RestoreTextBox(name As String)
            Dim lines As List(Of String) = Nothing

            SyncLock _textLock
                If _textOutputs.TryGetValue(name, lines) Then
                    lines = New List(Of String)(_textOutputs(name))
                End If
            End SyncLock

            If lines Is Nothing Then
                TextBox1.Clear()
            Else
                TextBox1.Text = String.Join(vbCrLf, lines)
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
        ''' thread-safe append a line to the default "Log Text" output.
        ''' </summary>
        ''' <param name="text"></param>
        Public Sub AppendLine(text As String)
            Call AppendTo(LogTextName, text)
        End Sub

        ''' <summary>
        ''' add a new configurable text (TextBox) output. If the name already exists it is ignored.
        ''' the new name is synchronised into the dropdown automatically.
        ''' </summary>
        ''' <param name="name"></param>
        Public Sub AddTextOutput(name As String)
            If String.IsNullOrEmpty(name) OrElse name = ActionOutputName Then
                Return
            End If

            Dim added As Boolean = False

            SyncLock _textLock
                If Not _textOutputs.ContainsKey(name) Then
                    _textOutputs.Add(name, New List(Of String))
                    added = True
                End If
            End SyncLock

            If added Then
                Call DirectCast(CommonRuntime.AppHost, Form).Invoke(Sub() SyncComboBox())
            End If
        End Sub

        ''' <summary>
        ''' thread-safe append a line to a specific configurable text output.
        ''' the named output is created on demand if it does not exist yet.
        ''' if the output is currently selected, the line is also shown live in the TextBox.
        ''' </summary>
        ''' <param name="name"></param>
        ''' <param name="text"></param>
        Public Sub AppendTo(name As String, text As String)
            If String.IsNullOrEmpty(name) OrElse name = ActionOutputName Then
                Return
            End If

            Dim created As Boolean = False

            SyncLock _textLock
                If Not _textOutputs.ContainsKey(name) Then
                    _textOutputs.Add(name, New List(Of String))
                    created = True
                End If

                _textOutputs(name).Add(text)
            End SyncLock

            If created Then
                Call DirectCast(CommonRuntime.AppHost, Form).Invoke(Sub() SyncComboBox())
            End If

            If CStr(ToolStripComboBox1.SelectedItem) = name Then
                Call DirectCast(CommonRuntime.AppHost, Form).Invoke(Sub() TextBox1.AppendText(text & vbCrLf))
            End If
        End Sub

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Sub AddLog(action$, message$, Optional time As Date? = Nothing, Optional level As MSG_TYPES = MSG_TYPES.INF)
            Call AddLog(If(time, Now), action, message, level)
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

            Call DirectCast(CommonRuntime.AppHost, Form).Invoke(
                Sub()
                    Dim offset As Integer = DataGridView1.Rows.Add(icon, time.ToString("HH:mm:ss"), action, message)
                    Dim row As DataGridViewRow = DataGridView1.Rows(offset)

                    row.Tag = level.Description
                    DataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit)
                End Sub)
        End Sub

        Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
            Dim selected As String = If(ToolStripComboBox1.SelectedItem Is Nothing, "", CStr(ToolStripComboBox1.SelectedItem))

            If selected = ActionOutputName Then
                ' export table
                Using file As New SaveFileDialog With {.Filter = "CSV file(*.csv)|*.csv"}
                    If file.ShowDialog = DialogResult.OK Then
                        Using s As StreamWriter = file.FileName.OpenWriter
                            Call s.WriteLine(",level,time,action,message")

                            For i As Integer = 0 To DataGridView1.Rows.Count - 1
                                Dim level As String = CStr(DataGridView1.Rows(i).Tag)
                                Dim time As String = CStr(DataGridView1.Rows(i).Cells(1).Value)
                                Dim action As String = CStr(DataGridView1.Rows(i).Cells(2).Value)
                                Dim message As String = CStr(DataGridView1.Rows(i).Cells(3).Value)

                                Call s.WriteLine($"{i + 1},{level},{time},""{action}"",""{message}""")
                            Next

                            Call s.Flush()
                        End Using
                    End If
                End Using
            Else
                ' export log text
                Using file As New SaveFileDialog With {.Filter = "Log file(*.log;*.txt)|*.log;*.txt"}
                    If file.ShowDialog = DialogResult.OK Then
                        Call TextBox1.Text.SaveTo(file.FileName)
                    End If
                End Using
            End If
        End Sub
    End Class
End Namespace