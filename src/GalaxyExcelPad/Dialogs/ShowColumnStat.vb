#Region "Microsoft.VisualBasic::926335061b59b96b3d25e51f6e0a535a, mzkit\src\mzkit\mzkit\forms\Inputs\ShowColumnStat.vb"

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

'   Total Lines: 3
'    Code Lines: 2
' Comment Lines: 0
'   Blank Lines: 1
'     File Size: 40.00 B


' Class ShowColumnStat
' 
' 
' 
' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Data.ChartPlots
Imports Microsoft.VisualBasic.Data.ChartPlots.BarPlot
Imports Microsoft.VisualBasic.Data.ChartPlots.BarPlot.Data
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Colors
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Math.Distributions
Imports SMRUCC.Rsharp.Runtime.Vectorization

Public Class ShowColumnStat

    Friend vectors As New Dictionary(Of String, Array)

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.DialogResult = DialogResult.OK
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Dim key As String = ComboBox1.SelectedItem.ToString
        Dim vec As Array = vectors(key)

        If TypeOf vec Is String() OrElse TypeOf vec Is Char() Then
            Dim chrs As String() = CLRVector.asCharacter(vec)
            Dim factors = chrs.GroupBy(Function(c) c).ToArray

            If factors.Length = chrs.Length Then
                Return
            ElseIf chrs.All(Function(si) si.StringEmpty OrElse si.IsNumeric) Then
                GoTo IS_NUMERIC
            End If

            DataGridView1.Rows.Clear()

            For Each factor In factors
                DataGridView1.Rows.Add(factor.Key, factor.Count)
            Next

            DataGridView1.Rows.Add("size", chrs.Length)

            ' plot bar plot to show counts
            Dim serials = Designer.GetColors("paper", factors.Length).Select(Function(c, i) New NamedValue(Of Color)(factors(i).Key, c)).ToArray
            Dim bar As New BarDataGroup With {
                .Serials = serials,
                .Samples = {New BarDataSample With {
                    .tag = "Factors",
                    .data = factors.Select(Function(f) CDbl(f.Count)).ToArray
                }}
            }

            PictureBox1.BackgroundImage = bar.Plot(size:=New Size(2700, 2100), dpi:=300).AsGDIImage

        ElseIf TypeOf vec Is Boolean() Then
            Dim flags As Boolean() = vec.AsObjectEnumerator.Select(Function(b) CBool(b)).ToArray
            Dim groups = flags.GroupBy(Function(b) b).ToArray
            Dim tcount = groups.Where(Function(t) t.Key).Count
            Dim fcount = groups.Where(Function(t) Not t.Key).Count

            DataGridView1.Rows.Clear()
            DataGridView1.Rows.Add("True", tcount)
            DataGridView1.Rows.Add("False", fcount)
            DataGridView1.Rows.Add("size", flags.Length)

            ' plot bar plot to show counts
            Dim bar As New BarDataGroup With {
                .Serials = {New NamedValue(Of Color)("True", Color.Red), New NamedValue(Of Color)("False", Color.Blue)},
                .Samples = {New BarDataSample With {
                    .data = {tcount, fcount},
                    .tag = "Flags"
                }}
            }

            PictureBox1.BackgroundImage = bar.Plot(size:=New Size(2700, 2100), dpi:=300).AsGDIImage

        ElseIf TypeOf vec Is Date() Then
            ' do nothing?
        Else
IS_NUMERIC:    ' is numeric
            Dim num As Double() = CLRVector.asNumeric(vec)
            Dim sample As New SampleDistribution(num)

            DataGridView1.Rows.Clear()
            DataGridView1.Rows.Add("mean", sample.average)
            DataGridView1.Rows.Add("min", sample.min)
            DataGridView1.Rows.Add("max", sample.max)
            DataGridView1.Rows.Add("min(CI95%)", sample.CI95Range.Min)
            DataGridView1.Rows.Add("max(CI95%)", sample.CI95Range.Max)
            DataGridView1.Rows.Add("mode", sample.mode)
            DataGridView1.Rows.Add("outlier lower bound", sample.outlierBoundary.Min)
            DataGridView1.Rows.Add("outlier upper bound", sample.outlierBoundary.Max)
            DataGridView1.Rows.Add("quantile 0%", sample.quantile(0))
            DataGridView1.Rows.Add("quantile 25%", sample.quantile(1))
            DataGridView1.Rows.Add("quantile 50%", sample.quantile(2))
            DataGridView1.Rows.Add("quantile 75%", sample.quantile(3))
            DataGridView1.Rows.Add("quantile 100%", sample.quantile(4))
            DataGridView1.Rows.Add("size", sample.size)

            ' plot violin plot of current data vector
            Dim data As New NamedCollection(Of Double) With {
                .name = key,
                .value = num
            }

            PictureBox1.BackgroundImage = ViolinPlot.Plot(
                dataset:={data},
                margin:="padding:300px 150px 500px 600px;",
                yTickFormat:="G2",
                title:=key,
                ytickFontCSS:="font-style: normal; font-size: 20; font-family: " & FontFace.BookmanOldStyle & ";",
                titleFontCSS:="font-style: strong; font-size: 48; font-family: " & FontFace.BookmanOldStyle & ";",
                ppi:=300,
                showStats:=False,
                removesOutliers:=False
            ).AsGDIImage
        End If
    End Sub
End Class
