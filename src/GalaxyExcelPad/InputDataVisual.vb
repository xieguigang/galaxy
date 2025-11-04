#Region "Microsoft.VisualBasic::35e892bfadafafab59e4e8d78773f46e, mzkit\src\mzkit\mzkit\forms\Inputs\InputDataVisual.vb"

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

'   Total Lines: 83
'    Code Lines: 65
' Comment Lines: 0
'   Blank Lines: 18
'     File Size: 2.87 KB


' Class InputDataVisual
' 
'     Function: getSerials, GetX, GetY
' 
'     Sub: Button1_Click, Button2_Click, DoPlot, SetAxis
' 
' /********************************************************************************/

#End Region

Imports BioNovoGene.mzkit_win32.My
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.ComponentModel.DataStructures
Imports Microsoft.VisualBasic.Data.ChartPlots
Imports Microsoft.VisualBasic.Data.ChartPlots.BarPlot.Data
Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Colors
Imports Microsoft.VisualBasic.Imaging.Math2D
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Scripting.Runtime
Imports any = Microsoft.VisualBasic.Scripting

Public Class InputDataVisual

    Dim fields As Dictionary(Of String, Type)

    ''' <summary>
    ''' set field names
    ''' </summary>
    ''' <param name="fields"></param>
    Public Sub SetAxis(fields As Dictionary(Of String, Type))
        For Each item As KeyValuePair(Of String, Type) In fields
            ListBox1.Items.Add(item.Key)
            CheckedListBox1.Items.Add(item.Key)
            cbColorGroups.Items.Add(item.Key)
        Next

        ListBox1.SelectedIndex = 0
        CheckedListBox1.SelectedIndex = 0

        Me.ComboBox1.SelectedIndex = 0
        Me.fields = fields

        For Each app As SummaryPlot In SummaryPlot.PlotApps
            If app.Test(fields.Keys) Then
                Call ComboBox2.Items.Add(app.appName)
            End If
        Next
    End Sub

    Public Function GetX() As String
        Return ListBox1.SelectedItem.ToString
    End Function

    Public Iterator Function GetY() As IEnumerable(Of String)
        For Each check In CheckedListBox1.CheckedItems
            Yield check.ToString
        Next
    End Function

    Private Function getCategorySerials(x As String(), getVector As Func(Of String, Array)) As BarDataGroup
        Dim colors As String() = Globals.Settings.viewer.colorSet
        Dim idx As i32 = Scan0
        Dim grid = MyApplication.host.mzkitTool.DataGridView1
        Dim getXName As String = GetX()
        Dim yList As New List(Of Array)
        Dim samples As New List(Of BarDataSample)

        If colors.IsNullOrEmpty Then
            colors = Designer.GetColors("paper", 12) _
                .Select(Function(c) c.ToHtmlColor) _
                .ToArray
        End If

        Dim colorList As NamedValue(Of Color)() = Designer _
            .CubicSpline(colors.Select(Function(c) c.TranslateColor), n:=x.Length) _
            .Take(x.Length) _
            .Select(Function(c, i)
                        Return New NamedValue(Of Color) With {
                            .Name = x(i),
                            .Value = c
                        }
                    End Function) _
            .ToArray

        Call grid.Rows.Clear()
        Call grid.Columns.Clear()

        Call grid.Columns.Add(getXName, getXName)

        For Each name As String In GetY()
            Dim y As Array = getVector(name)

            yList.Add(y)
            samples += New BarDataSample With {
                .data = y.AsObjectEnumerator _
                    .Take(x.Length) _
                    .Select(Function(o) Val(o)) _
                    .ToArray,
                .tag = name
            }
        Next

#Disable Warning
        For i As Integer = 0 To x.Length - 1
            Dim row As Object() = {x.GetValue(i)} _
                .JoinIterates(yList.Select(Function(yi) yi.GetValue(i))) _
                .ToArray

            Call grid.Rows.Add(row)
        Next
#Enable Warning

        Return New BarDataGroup With {
            .Samples = samples.ToArray,
            .Serials = colorList.ToArray
        }
    End Function

    Private Iterator Function getSerials(x As Array, getVector As Func(Of String, Array)) As IEnumerable(Of SerialData)
        Dim colors As String() = Globals.Settings.viewer.colorSet
        Dim idx As i32 = Scan0
        Dim grid = MyApplication.host.mzkitTool.DataGridView1
        Dim getXName As String = GetX()
        Dim yList As New List(Of Array)

        If colors.IsNullOrEmpty Then
            colors = Designer.GetColors("paper", 12) _
                .Select(Function(c) c.ToHtmlColor) _
                .ToArray
        End If

        Dim memoryData As New DataSet
        Dim table As DataTable = memoryData.Tables.Add("memoryData")

        Try
            Call grid.Columns.Clear()
            Call grid.Rows.Clear()
        Catch ex As Exception

        End Try

        table.Columns.Add(getXName, GetType(Double))

        Dim yNames = GetY.ToArray

        If yNames.Length = 1 Then
            Dim group_maps As New Dictionary(Of String, (List(Of PointF), String))

            If cbColorGroups.SelectedIndex <> -1 Then
                Dim tags As Array = getVector(any.ToString(cbColorGroups.SelectedItem))
                Dim tag As String
                Dim colorLoop As LoopArray(Of String) = colors
                Dim y As Array = getVector(yNames(0))
                Dim points = x _
                    .AsObjectEnumerator _
                    .Take(x.Length - 1) _
                    .Select(Function(xi, i) New PointF(xi, y(i))) _
                    .OrderByDescending(Function(p) p.X) _
                    .ToArray
                Dim obj As Object

                For i As Integer = 0 To points.Length - 1
                    obj = tags(i)
                    tag = any.ToString(obj)

                    If Not group_maps.ContainsKey(tag) Then
                        group_maps(tag) = (New List(Of PointF), ++colorLoop)
                    End If

                    group_maps(tag).Item1.Add(points(i))
                Next

                For Each group In group_maps
                    Dim s = Scatter.FromPoints(group.Value.Item1, lineColor:=group.Value.Item2)

                    Call table.Columns.Add(group.Key, GetType(Double))
                    Call yList.Add(group.Value.Item1.Select(Function(p) p.Y).ToArray)

                    Yield s
                Next
            Else
                GoTo SingleS
            End If
        Else
SingleS:    For Each name As String In GetY()
                Dim y As Array = getVector(name)
                Dim points = x _
                    .AsObjectEnumerator _
                    .Take(x.Length - 1) _
                    .Select(Function(xi, i) New PointF(xi, y(i))) _
                    .OrderByDescending(Function(p) p.X) _
                    .ToArray
                Dim s = Scatter.FromPoints(points, lineColor:=colors(++idx))

                Call table.Columns.Add(name, GetType(Double))
                Call yList.Add(y)

                Yield s
            Next
        End If

        If yList.All(Function(yi) yi.Length = x.Length) Then
#Disable Warning
            ' 20230216
            ' 如果y group的每组元素的数量和x都一样，才显示绘图矩阵
            For i As Integer = 0 To x.Length - 2
                Dim row As Object() = {x.GetValue(i)} _
                    .JoinIterates(yList.Select(Function(yi) yi.GetValue(i))) _
                    .ToArray

                Call table.Rows.Add(row)
            Next
#Enable Warning
        End If

        MyApplication.host.mzkitTool.BindingSource1.DataSource = memoryData
        MyApplication.host.mzkitTool.BindingSource1.DataMember = table.TableName
        MyApplication.host.mzkitTool.DataGridView1.DataSource = MyApplication.host.mzkitTool.BindingSource1
    End Function

    Public Sub DoPlot(x As Array, table As DataTable, getVector As Func(Of String, Array))
        Dim plot As Image

        If (TabControl1.SelectedTab Is TabPage2) Then
            Dim Rplot = doSummaryPlot(table)

            If TypeOf Rplot Is NetworkGraph Then
                Dim g As NetworkGraph = Rplot
                Dim viewer As frmNetworkViewer = VisualStudio.ShowDocument(Of frmNetworkViewer)(title:=ComboBox2.SelectedItem.ToString)

                viewer.SetGraph(g, layout:=Globals.Settings.network.layout)
                viewer.Show(MyApplication.host.m_dockPanel)

                Return
            Else
                plot = Rplot
            End If
        Else
            plot = doGeneralPlot(x, getVector)
        End If

        ' nothing will be returns if user cancel
        If Not plot Is Nothing Then
            MyApplication.host.mzkitTool.ShowPlotImage(plot, ImageLayout.Zoom)
            MyApplication.host.ShowMzkitToolkit()
        End If
    End Sub

    Private Function doSummaryPlot(table As DataTable) As Object
        Dim name As String = ComboBox2.SelectedItem.ToString
        Dim app As SummaryPlot = SummaryPlot.getApp(name)

        Return app.Plot(table)
    End Function

    Private Function doGeneralPlot(x As Array, getVector As Func(Of String, Array)) As Image
        Dim size As String = MyApplication.host.mzkitTool.PictureBox1 _
            .Size _
            .Scale(1.75) _
            .ToArray(reverse:=True) _
            .JoinBy(",")
        Dim padding As String = "padding:200px 300px 200px 200px;"

        Select Case ComboBox1.SelectedItem.ToString
            Case "Scatter"
                Dim groups = getSerials(x, getVector).ToArray
                Return Scatter.Plot(groups, size:=size, drawLine:=False, padding:=padding).AsGDIImage
            Case "Line"
                Dim groups = getSerials(x, getVector).ToArray
                Return Scatter.Plot(groups, size:=size, drawLine:=True, padding:=padding).AsGDIImage
            Case "BarPlot"
                Dim catNames As String() = x _
                    .AsObjectEnumerator() _
                    .Take(x.Length - 1) _
                    .Select(Function(o) any.ToString(o)) _
                    .ToArray

                padding = "padding:200px 600px 200px 200px;"

                Return BarPlot.BarPlotAPI _
                    .Plot(
                        data:=getCategorySerials(catNames, getVector),
                        size:=size.SizeParser,
                        padding:=padding,
                        dpi:=100
                    ) _
                    .AsGDIImage

            Case "BoxPlot", "ViolinPlot"
                Throw New NotImplementedException
            Case "Histogram"
                Dim xvec As Double() = x _
                    .AsObjectEnumerator _
                    .Select(Function(xi) CDbl(xi)) _
                    .ToArray

                Return BarPlot.Histogram.Histogram.HistogramPlot(xvec, CSng((xvec.Max - xvec.Min) / 64), size:=size, padding:=padding).AsGDIImage
            Case Else
                Throw New NotImplementedException
        End Select
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ' do data validation
        If (TabControl1.SelectedTab Is TabPage2) Then
            If ComboBox2.SelectedIndex = -1 Then
                MessageBox.Show("Please select a summary data plot!", "Data visualization", buttons:=MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            Else
                ' check success
            End If
        Else

        End If

        Me.DialogResult = DialogResult.OK
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.DialogResult = DialogResult.Cancel
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.SelectedIndex > 0 Then
            If ComboBox1.SelectedItem.ToString = "Histogram" Then
                lblMsg.Text = "Histogram plot did'nt needs the data selection of Y data."
            Else
                lblMsg.Text = ""
            End If
        Else
            lblMsg.Text = ""
        End If
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        If ComboBox2.SelectedIndex > -1 Then
            TextBox1.Text = SummaryPlot.getApp(ComboBox2.SelectedItem.ToString).ToString
        End If
    End Sub

    Private Sub cbColorGroups_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbColorGroups.SelectedIndexChanged

    End Sub
End Class
