#Region "Microsoft.VisualBasic::500cd72dfefa84c707f0979df79b3f06, mzkit\src\mzkit\mzkit\pages\dockWindow\documents\frmNetworkViewer.vb"

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

'   Total Lines: 82
'    Code Lines: 63
' Comment Lines: 2
'   Blank Lines: 17
'     File Size: 3.36 KB


' Class frmNetworkViewer
' 
'     Sub: Canvas1_DoubleClick, ConfigLayoutToolStripMenuItem_Click, ContextMenuStrip1_Opening, CopyNetworkVisualizeToolStripMenuItem_Click, frmNetworkViewer_Load
'          PhysicalEngineToolStripMenuItem_Click, PinToolStripMenuItem_Click, SetGraph, ShowLabelsToolStripMenuItem_Click, SnapshotToolStripMenuItem_Click
' 
' /********************************************************************************/

#End Region

Imports System.ComponentModel
Imports System.Text
Imports Galaxy.CommonControls
Imports Galaxy.Workbench
Imports Galaxy.Workbench.CommonDialogs
Imports Microsoft.VisualBasic.ApplicationServices
Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model
Imports Microsoft.VisualBasic.Data.visualize.Network
Imports Microsoft.VisualBasic.Data.visualize.Network.Canvas
Imports Microsoft.VisualBasic.Data.visualize.Network.FileStream
Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
Imports Microsoft.VisualBasic.Data.visualize.Network.Layouts.SpringForce
Imports Microsoft.VisualBasic.Drawing
Imports SMRUCC.genomics.Visualize.Cytoscape.CytoscapeGraphView.Serialization
Imports SMRUCC.genomics.Visualize.Cytoscape.CytoscapeGraphView.XGMML
Imports SMRUCC.genomics.Visualize.Cytoscape.CytoscapeGraphView.XGMML.File

Public Class FormNetworkViewer

    Public getImage As Func(Of Graph.Node, Image)
    Public showTarget As Action(Of Graph.Node)

    Private Sub frmNetworkViewer_Load(sender As Object, e As EventArgs) Handles Me.Load
        Text = "Network Canvas"
        TabText = "Network Canvas"

        Call ApplyVsTheme(ContextMenuStrip1)

        ToolStripStatusLabel1.Text = ""
        ToolStripStatusLabel2.Text = ""
    End Sub

    Public Sub SetGraph(g As NetworkGraph, layout As ForceDirectedArgs)
        If layout Is Nothing Then
            layout = ForceDirectedArgs.DefaultNew
        End If

        Canvas1.Graph() = g
        Canvas1.SetFDGParams(layout)
    End Sub

    Private Sub PhysicalEngineToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PhysicalEngineToolStripMenuItem.Click
        If PhysicalEngineToolStripMenuItem.Checked Then
            ' turn on engine
            PhysicalEngineToolStripMenuItem.Text = "Physical Engine (On)"
        Else
            ' turn off engine
            PhysicalEngineToolStripMenuItem.Text = "Physical Engine (Off)"
        End If

        Canvas1.SetPhysical(PhysicalEngineToolStripMenuItem.Checked)
    End Sub

    Private Sub ConfigLayoutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConfigLayoutToolStripMenuItem.Click
        Dim settings As New InputNetworkLayout
        Dim range As New DoubleRange(settings.TrackBar1.Minimum, settings.TrackBar1.Maximum)

        If range.IsInside(Canvas1.ViewDistance) Then
            settings.TrackBar1.Value = Canvas1.ViewDistance
        End If

        CommonRuntime.LogText($"view distance: {Canvas1.ViewDistance}")

        Call InputDialog.Input(Of InputNetworkLayout)(
            Sub(config)
                Canvas1.SetFDGParams(config.GraphLayout)
                Canvas1.ViewDistance = config.TrackBar1.Value
            End Sub, config:=settings)
    End Sub

    Private Sub ShowLabelsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ShowLabelsToolStripMenuItem.Click
        Canvas1.ShowLabel = ShowLabelsToolStripMenuItem.Checked
    End Sub

    Private Sub SnapshotToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SnapshotToolStripMenuItem.Click
        Using file As New SaveFileDialog With {.Filter = "Image File(*.bmp)|*.bmp|Vector Image(*.svg)|*.svg"}
            If file.ShowDialog = DialogResult.OK Then
                If file.FileName.ExtensionSuffix("svg") Then
                    Call Canvas1 _
                        .WriteLayout _
                        .ToSVG(size:=Size, viewDistance:=Canvas1.ViewDistance) _
                        .SaveAsXml(file.FileName)
                Else
                    Call Canvas1.GetSnapshot.SaveAs(file.FileName)
                End If
            End If
        End Using
    End Sub

    Private Sub CopyNetworkVisualizeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyNetworkVisualizeToolStripMenuItem.Click
        Clipboard.SetImage(Canvas1.GetSnapshot)
    End Sub

    Private Sub Canvas1_DoubleClick(sender As Object, e As EventArgs) Handles Canvas1.DoubleClick
        Dim tempfile As String = TempFileSystem.GetAppSysTempFile(ext:=".bmp")

        Call Canvas1.GetSnapshot.SaveAs(tempfile)
        Call Process.Start(tempfile)
    End Sub

    Private Sub PinToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PinToolStripMenuItem.Click
        Dim target As Graph.Node = Canvas1.GetTargetNode(PointToClient(Cursor.Position))

        If Not target Is Nothing Then
            target.pinned = PinToolStripMenuItem.Checked
        End If
    End Sub

    Private Sub ContextMenuStrip1_Opening(sender As Object, e As CancelEventArgs) Handles ContextMenuStrip1.Opening
        Dim target As Graph.Node = Canvas1.GetTargetNode(PointToClient(Cursor.Position))

        If Not target Is Nothing Then
            PinToolStripMenuItem.Checked = target.pinned
        End If
    End Sub

    Private Sub Canvas1_MouseHover(sender As Object, e As EventArgs) Handles Canvas1.MouseHover
        Dim target As Graph.Node = Canvas1.GetTargetNode(PointToClient(Cursor.Position))

        If target IsNot Nothing AndAlso Not getImage Is Nothing Then
            ' display target ms plot
            Dim tt As New ToolTipWithPictureOrGif
            Dim win As IWin32Window = Me
            Dim MousePosition = Cursor.Position
            Dim msPlot As Image = getImage(target)

            tt.Binding(Me.Canvas1, msPlot)
            tt.Show(target.data.label, win, MousePosition)
        End If
    End Sub

    Private Sub DToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DToolStripMenuItem.Click
        If Not Canvas1.Graph Is Nothing Then
            Canvas1.Graph(space3D:=DToolStripMenuItem.Checked) = Canvas1.Graph
        End If
    End Sub

    Private Sub Canvas1_MouseMove(sender As Object, e As MouseEventArgs) Handles Canvas1.MouseMove
        Dim mouseStatus = ToolStripStatusLabel1
        Dim graphStatus = ToolStripStatusLabel2
        Dim pos As Point = PointToClient(Cursor.Position)

        If DToolStripMenuItem.Checked Then
            ' 3d mode
            mouseStatus.Text = $"[{pos.X},{pos.Y}] View distance: {Canvas1.ViewDistance}"
        Else
            mouseStatus.Text = $"[{pos.X},{pos.Y}]"
        End If

        Dim target As Graph.Node = Canvas1.GetTargetNode(PointToClient(Cursor.Position))

        If Not target Is Nothing Then
            graphStatus.Text = target.data.label
        End If
    End Sub

    Private Sub Canvas1_MouseClick(sender As Object, e As MouseEventArgs) Handles Canvas1.MouseClick
        Dim target As Graph.Node = Canvas1.GetTargetNode(PointToClient(Cursor.Position))

        If target IsNot Nothing AndAlso Not showTarget Is Nothing Then
            ' show target ms
            Call showTarget(target)
        End If
    End Sub

    Private Sub NetworkGraphToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NetworkGraphToolStripMenuItem.Click
        If Not Canvas1.Graph Is Nothing Then
            ' edge table
            Dim edges = Canvas1.Graph.CreateGraphTable({"*"}, DToolStripMenuItem.Checked).ToArray

            Call ExcelPad.ShowTable(edges, "Network Graph")
        End If
    End Sub

    Private Sub NodeMetadataToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NodeMetadataToolStripMenuItem.Click
        If Not Canvas1.Graph Is Nothing Then
            ' nodes table
            Dim nodes = Canvas1.WriteLayout.CreateNodesMetaData({"*"}, DToolStripMenuItem.Checked).ToArray

            Call ExcelPad.ShowTable(nodes, "Node Metadata")
        End If
    End Sub

    Private Sub ExportCytoscapeToolStripMenuItem_DisplayStyleChanged(sender As Object, e As EventArgs) Handles ExportCytoscapeToolStripMenuItem.DisplayStyleChanged

    End Sub

    Private Sub ExportCytoscapeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExportCytoscapeToolStripMenuItem.Click
        If Canvas1.Graph Is Nothing Then
            Return
        End If

        Using file As New SaveFileDialog With {.Filter = "Cytoscape Model(*.xgmml)|*.xgmml"}
            If file.ShowDialog = DialogResult.OK Then
                Dim g As NetworkGraph = Canvas1.WriteLayout
                Dim edges = g.CreateGraphTable({"*"}, False).ToArray
                Dim nodes = g.CreateNodesMetaData({"*"}, False).ToArray
                Dim cy3 As XGMMLgraph = ExportToFile.Export(nodes, edges, title:="View Network Graph")

                Call RDFXml.WriteXml(cy3, Encoding.UTF8, path:=file.FileName)
            End If
        End Using
    End Sub
End Class
