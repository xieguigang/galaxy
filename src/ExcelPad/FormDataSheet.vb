Imports System.Data
Imports System.Runtime.CompilerServices
Imports Galaxy.Data.TableSheet
Imports Microsoft.VisualBasic.Data.Framework.IO
Imports Microsoft.VisualBasic.Data.Framework.StorageProvider

Public Class FormDataSheet

    Dim loader As GridLoaderHandler
    Dim search As GridSearchHandler

    Private Sub FormDataSheet_Load(sender As Object, e As EventArgs) Handles Me.Load
        loader = New GridLoaderHandler(table, toolbar)
        search = New GridSearchHandler(table)

        Call ApplyVsTheme(toolbar)
    End Sub

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Overloads Sub LoadData(table As DataFrameResolver)
        Call loader.LoadTable(Sub(tbl) LoadData(tbl, table))
    End Sub

    Private Overloads Sub LoadData(tbl As DataTable, table As DataFrameResolver)
        Dim nsize As Integer = table.HeadTitles.Length

        For Each col As String In table.HeadTitles
            Call tbl.Columns.Add(col, GetType(String))
        Next

        For Each row As RowObject In table.Rows
            Dim data As Object() = New Object(nsize - 1) {}

            For i As Integer = 0 To data.Length - 1
                data(i) = row(i)
            Next

            Call tbl.Rows.Add(data)
        Next
    End Sub
End Class