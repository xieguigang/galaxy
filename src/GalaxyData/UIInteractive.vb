Imports System.Runtime.CompilerServices
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.Text

Public Module UIInteractive

    ''' <summary>
    ''' Auto select the row on cell click
    ''' </summary>
    ''' <param name="dv"></param>
    <Extension>
    Public Sub SetAutoSelectRow(dv As DataGridView)
        AddHandler dv.CellClick,
            Sub(sender, evt)
                If evt.RowIndex >= 0 AndAlso evt.RowIndex < dv.Rows.Count Then
                    dv.Rows(evt.RowIndex).Selected = True
                End If
            End Sub
    End Sub

    ''' <summary>
    ''' Paste the text data to the table control
    ''' </summary>
    ''' <param name="table"></param>
    <Extension>
    Public Sub PasteTextData(table As DataGridView)
        Dim text As String = Strings.Trim(Clipboard.GetText).Trim(ASCII.CR, ASCII.LF, ASCII.TAB)

        If table.SelectedCells.Count = 0 Then
            Return
        End If

        Dim i As Integer = table.SelectedCells.Item(Scan0).RowIndex
        Dim j As Integer = table.SelectedCells.Item(Scan0).ColumnIndex

        If text.Contains(vbCr) OrElse text.Contains(vbLf) Then
            Dim colCells As String() = text.LineTokens

            If i + colCells.Length >= table.Rows.Count Then
                Dim n As Integer = table.Rows.Count

                For rid As Integer = 0 To (colCells.Length + i) - n
                    table.Rows.Add()
                Next
            End If

            For ii As Integer = 0 To colCells.Length - 1
                table.Rows(ii + i).Cells(j).Value = colCells(ii)
            Next
        Else
            Dim rowCells As String() = text.Split(ASCII.TAB)

            For ci As Integer = 0 To rowCells.Length - 1
                table.Rows(i).Cells(j + ci).Value = rowCells(ci)
            Next
        End If
    End Sub
End Module
