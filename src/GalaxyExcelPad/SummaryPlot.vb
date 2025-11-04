Imports System.Text
Imports Galaxy.Data
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.Serialization.JSON

Public MustInherit Class SummaryPlot

    Public MustOverride ReadOnly Property requiredFields As Dictionary(Of String(), String)
    Public MustOverride ReadOnly Property appName As String

    Public Function Test(fieldNames As IEnumerable(Of String)) As Boolean
        Dim nameIndex As Index(Of String) = fieldNames.Indexing

        For Each names As String() In requiredFields.Keys
            Dim hit As Boolean = False

            For Each name As String In names
                If name Like nameIndex Then
                    hit = True
                    Exit For
                End If
            Next

            If Not hit Then
                Return False
            End If
        Next

        Return True
    End Function

    Shared ReadOnly PlotApps As New Dictionary(Of String, SummaryPlot)

    Public Shared Function getApp(name As String) As SummaryPlot
        Return PlotApps(name)
    End Function

    Public Shared Sub Register(plot As SummaryPlot)
        PlotApps(plot.appName) = plot
    End Sub

    Public Shared Function EnumeratePlots() As IEnumerable(Of SummaryPlot)
        Return PlotApps.Values
    End Function

    Public MustOverride Function Plot(table As DataTable) As Object

    Protected Function getFieldVector(table As DataTable, aliasNames As String()) As Array
        For Each name As String In aliasNames
            If table.Columns.Contains(name) Then
                Return table.getFieldVector(name)
            End If
        Next

        Throw New InvalidProgramException(aliasNames.GetJson)
    End Function

    Public Overrides Function ToString() As String
        Dim sb As New StringBuilder(appName & vbCrLf)
        sb.AppendLine()

        For Each item In requiredFields
            Call sb.AppendLine($"{item.Key.JoinBy(", ")}: {item.Value}")
        Next

        Return sb.ToString
    End Function
End Class
