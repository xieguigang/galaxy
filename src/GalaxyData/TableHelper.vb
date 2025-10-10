Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Text
Imports Microsoft.VisualBasic.Data.Framework.IO
Imports Microsoft.VisualBasic.MIME.Office.Excel.XLSX.Writer
Imports Microsoft.VisualBasic.Text
Imports any = Microsoft.VisualBasic.Scripting

Public Module TableHelper

    ''' <summary>
    ''' Helper function for get dataframe from datagrid view table object
    ''' </summary>
    ''' <param name="table2"></param>
    ''' <param name="saveHeader"></param>
    ''' <returns></returns>
    <Extension>
    Public Function GetDataFrame(table2 As DataGridView,
                                 Optional saveHeader As Boolean = True,
                                 Optional warning As Action(Of String) = Nothing) As DataFrameResolver
        Dim row As New RowObject
        Dim src As BindingSource = table2.DataSource
        Dim headers As New List(Of String)
        Dim df As New List(Of RowObject)

        If saveHeader Then
            For i As Integer = 0 To table2.Columns.Count - 1
                Call headers.Add(table2.Columns(i).HeaderText)
            Next
        End If

        If src Is Nothing Then
            For j As Integer = 0 To table2.Rows.Count - 1
                Dim rowObj As DataGridViewRow = table2.Rows(j)

                row = New RowObject

                For i As Integer = 0 To rowObj.Cells.Count - 1
                    Call row.Add(any.ToString(rowObj.Cells.Item(i).Value))
                Next

                Call df.Add(row)
            Next
        Else
            Dim ds As System.Data.DataSet = src.DataSource
            Dim table As DataTable = ds.Tables.Item(src.DataMember)

            For j As Integer = 0 To table.Rows.Count - 1
                Dim rowObj As DataRow = table.Rows(j)

                row = New RowObject

                Try
                    Call row.AddRange(rowObj.ItemArray.Select(AddressOf any.ToString))
                    Call df.Add(row)
                Catch ex As Exception
                    If Not warning Is Nothing Then
                        Call warning(ex.ToString)
                    End If
                End Try
            Next
        End If

        If headers.IsNullOrEmpty Then
            headers = New List(Of String)(df.FirstOrDefault)
            df.RemoveAt(Scan0)
        End If

        Return DataFrameResolver.CreateObject(headers, df)
    End Function

    ''' <summary>
    ''' write table in tsv file format
    ''' </summary>
    ''' <param name="table2"></param>
    ''' <param name="writeTsv">
    ''' target file or the <see cref="StringBuilder"/> for copy to clipboard
    ''' </param>
    <Extension>
    Public Sub WriteTableToFile(table2 As DataGridView, writeTsv As TextWriter,
                                Optional saveHeader As Boolean = True,
                                Optional sep As Char = ASCII.TAB,
                                Optional warning As Action(Of String) = Nothing)

        Dim row As New RowObject
        Dim src As BindingSource = table2.DataSource

        If saveHeader Then
            For i As Integer = 0 To table2.Columns.Count - 1
                Call row.Add(table2.Columns(i).HeaderText)
            Next

            Call writeTsv.WriteLine(row.PopLine(sep))
        End If

        If src Is Nothing Then
            For j As Integer = 0 To table2.Rows.Count - 1
                Dim rowObj As DataGridViewRow = table2.Rows(j)

                For i As Integer = 0 To rowObj.Cells.Count - 1
                    Call row.Add(any.ToString(rowObj.Cells.Item(i).Value))
                Next

                Call writeTsv.WriteLine(row.PopLine(sep))
            Next
        Else
            Dim ds As System.Data.DataSet = src.DataSource
            Dim table As DataTable = ds.Tables.Item(src.DataMember)

            For j As Integer = 0 To table.Rows.Count - 1
                Dim rowObj As DataRow = table.Rows(j)

                Try
                    Call row.AddRange(rowObj.ItemArray.Select(AddressOf any.ToString))
                    Call writeTsv.WriteLine(row.PopLine(sep))
                Catch ex As Exception
                    If Not warning Is Nothing Then
                        Call warning(ex.ToString)
                    End If
                End Try
            Next
        End If

        Call writeTsv.Flush()
    End Sub

    ''' <summary>
    ''' save data grid as excel table file
    ''' </summary>
    ''' <param name="table"></param>
    ''' <param name="title">
    ''' ``%s`` is the place holder for file name
    ''' </param>
    <Extension>
    Public Sub SaveDataGrid(table As DataGridView, title$)
        Using file As New SaveFileDialog With {
            .Filter = "Microsoft Excel Table(*.xls;*.xlsx)|*.xls;*.xlsx|Comma data sheet(*.csv)|*.csv",
            .Title = $"Save Table File({title})"
        }
            If file.ShowDialog = DialogResult.OK Then
                If file.FileName.ExtensionSuffix("xlsx") Then
                    Dim df As DataFrameResolver = table.GetDataFrame()
                    Dim book As New Workbook("Sheet1")
                    Dim sheet = book.CurrentWorksheet

                    For Each col As String In df.HeadTitles
                        Call sheet.AddNextCell(col)
                    Next

                    Call sheet.GoToNextRow()

                    For Each row As RowObject In df.Rows
                        For Each cell As String In row
                            Call sheet.AddNextCell(cell)
                        Next

                        Call sheet.GoToNextRow()
                    Next

                    Call book.SaveAs(file.FileName)
                Else
                    Using writeTsv As StreamWriter = file.FileName.OpenWriter(encoding:=Encodings.GB2312)
                        Call table.WriteTableToFile(writeTsv, sep:=If(file.FileName.ExtensionSuffix("csv"), ","c, ASCII.TAB))
                        Call MessageBox.Show(title.Replace("%s", file.FileName), "Export Table", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End Using
                End If
            End If
        End Using
    End Sub
End Module
