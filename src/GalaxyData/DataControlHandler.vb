#Region "Microsoft.VisualBasic::82cb33f4d127560d66c1c4263033494b, mzkit\src\mzkit\mzkit\DataControlHandler.vb"

' Author:
' 
'       xieguigang (gg.xie@bionovogene.com, BioNovoGene Co., LTD.)
' 
' Copyright (c) 2018 gg.xie@bionovogene.com, BioNovoGene Co., LTD.
' 
' 
' MIT License
' 
' 
' Permission is hereby granted, free of charge, to any person obtaining a copy
' of this software and associated documentation files (the "Software"), to deal
' in the Software without restriction, including without limitation the rights
' to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
' copies of the Software, and to permit persons to whom the Software is
' furnished to do so, subject to the following conditions:
' 
' The above copyright notice and this permission notice shall be included in all
' copies or substantial portions of the Software.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
' IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
' FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
' LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
' SOFTWARE.



' /********************************************************************************/

' Summaries:


' Code Statistics:

'   Total Lines: 84
'    Code Lines: 60
' Comment Lines: 7
'   Blank Lines: 17
'     File Size: 2.91 KB


' Module DataControlHandler
' 
'     Sub: PasteTextData, SaveDataGrid, WriteTableToFile
' 
' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports BioNovoGene.mzkit_win32.My
Imports Microsoft.VisualBasic.Language
Imports REnv = SMRUCC.Rsharp.Runtime

Module DataControlHandler

    <Extension>
    Public Function getFieldVector(table As DataTable, fieldRef As String) As Array
        Dim fieldNames As New List(Of String)
        Dim tag As String

        For i As Integer = 0 To table.Columns.Count - 1
            tag = table.Columns.Item(i).ColumnName
            fieldNames.Add(tag)
        Next

        Dim ordinal As Integer = fieldNames.IndexOf(fieldRef)
        Dim vec = table.getFieldVector(ordinal)

        Return vec
    End Function

    <Extension>
    Public Function getFieldVector(table As DataTable, fieldRef As Integer) As Array
        Dim array As New List(Of Object)
        Dim row As DataRow
        Dim val As Object

        For index As Integer = 0 To table.Rows.Count - 2
            row = table.Rows.Item(index)
            val = row.Item(fieldRef)

            If Convert.IsDBNull(val) Then
                Call array.Add(Nothing)
            Else
                Call array.Add(val)
            End If
        Next

        Return REnv.TryCastGenericArray(array.ToArray, MyApplication.REngine.globalEnvir)
    End Function

    ''' <summary>
    ''' found based on the <see cref="DataGridViewColumn.Name"/>
    ''' </summary>
    ''' <param name="AdvancedDataGridView1"></param>
    ''' <param name="fieldRef"></param>
    ''' <returns></returns>
    <Extension>
    Public Function getFieldVector(AdvancedDataGridView1 As DataGridView, fieldRef As String) As Array
        Dim fieldNames As New List(Of String)

        For Each col As DataGridViewColumn In AdvancedDataGridView1.Columns
            Call fieldNames.Add(col.Name)
        Next

        Dim i As Integer = fieldNames.IndexOf(fieldRef)
        Dim vec = AdvancedDataGridView1.getFieldVector(i)

        Return vec
    End Function

    <Extension>
    Public Function getFieldVector(AdvancedDataGridView1 As DataGridView, i As Integer) As Array
        Dim array As New List(Of Object)
        Dim val As Object

        For Each row As DataGridViewRow In AdvancedDataGridView1.Rows
            val = row.Cells(i).Value

            If Convert.IsDBNull(val) Then
                Call array.Add(Nothing)
            Else
                Call array.Add(val)
            End If
        Next

        Return REnv.TryCastGenericArray(array.ToArray, MyApplication.REngine.globalEnvir)
    End Function
End Module
