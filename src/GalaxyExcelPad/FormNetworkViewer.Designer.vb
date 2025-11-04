#Region "Microsoft.VisualBasic::c2a17ed3eb78b014d632c065e5456ea0, mzkit\src\mzkit\mzkit\pages\dockWindow\documents\frmNetworkViewer.Designer.vb"

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

'   Total Lines: 138
'    Code Lines: 95
' Comment Lines: 38
'   Blank Lines: 5
'     File Size: 7.31 KB


' Class frmNetworkViewer
' 
'     Sub: Dispose, InitializeComponent
' 
' /********************************************************************************/

#End Region

Imports Galaxy.CommonControls
Imports Galaxy.Workbench.DockDocument

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormNetworkViewer
    Inherits DocumentWindow
    ' Inherits Form

    'Form 重写 Dispose，以清理组件列表。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。  
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormNetworkViewer))
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.PinToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ShowLabelsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PhysicalEngineToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConfigLayoutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.OpenGraphTableToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NetworkGraphToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NodeMetadataToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SnapshotToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CopyNetworkVisualizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExportCytoscapeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolTipWithPictureOrGif1 = New ToolTipWithPictureOrGif()
        Me.Canvas1 = New Microsoft.VisualBasic.Data.visualize.Network.Canvas.Canvas
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel2 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PinToolStripMenuItem, Me.ToolStripMenuItem2, Me.ShowLabelsToolStripMenuItem, Me.PhysicalEngineToolStripMenuItem, Me.DToolStripMenuItem, Me.ConfigLayoutToolStripMenuItem, Me.ToolStripMenuItem1, Me.OpenGraphTableToolStripMenuItem, Me.SnapshotToolStripMenuItem, Me.CopyNetworkVisualizeToolStripMenuItem, Me.ExportCytoscapeToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(199, 214)
        '
        'PinToolStripMenuItem
        '
        Me.PinToolStripMenuItem.CheckOnClick = True
        Me.PinToolStripMenuItem.Image = CType(resources.GetObject("PinToolStripMenuItem.Image"), System.Drawing.Image)
        Me.PinToolStripMenuItem.Name = "PinToolStripMenuItem"
        Me.PinToolStripMenuItem.Size = New System.Drawing.Size(198, 22)
        Me.PinToolStripMenuItem.Text = "Pin"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(195, 6)
        '
        'ShowLabelsToolStripMenuItem
        '
        Me.ShowLabelsToolStripMenuItem.CheckOnClick = True
        Me.ShowLabelsToolStripMenuItem.Name = "ShowLabelsToolStripMenuItem"
        Me.ShowLabelsToolStripMenuItem.Size = New System.Drawing.Size(198, 22)
        Me.ShowLabelsToolStripMenuItem.Text = "Show Labels"
        '
        'PhysicalEngineToolStripMenuItem
        '
        Me.PhysicalEngineToolStripMenuItem.Checked = True
        Me.PhysicalEngineToolStripMenuItem.CheckOnClick = True
        Me.PhysicalEngineToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.PhysicalEngineToolStripMenuItem.Name = "PhysicalEngineToolStripMenuItem"
        Me.PhysicalEngineToolStripMenuItem.Size = New System.Drawing.Size(198, 22)
        Me.PhysicalEngineToolStripMenuItem.Text = "Physical Engine (On)"
        '
        'DToolStripMenuItem
        '
        Me.DToolStripMenuItem.CheckOnClick = True
        Me.DToolStripMenuItem.Name = "DToolStripMenuItem"
        Me.DToolStripMenuItem.Size = New System.Drawing.Size(198, 22)
        Me.DToolStripMenuItem.Text = "3D"
        '
        'ConfigLayoutToolStripMenuItem
        '
        Me.ConfigLayoutToolStripMenuItem.Name = "ConfigLayoutToolStripMenuItem"
        Me.ConfigLayoutToolStripMenuItem.Size = New System.Drawing.Size(198, 22)
        Me.ConfigLayoutToolStripMenuItem.Text = "Config Layout"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(195, 6)
        '
        'OpenGraphTableToolStripMenuItem
        '
        Me.OpenGraphTableToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NetworkGraphToolStripMenuItem, Me.NodeMetadataToolStripMenuItem})
        Me.OpenGraphTableToolStripMenuItem.Name = "OpenGraphTableToolStripMenuItem"
        Me.OpenGraphTableToolStripMenuItem.Size = New System.Drawing.Size(198, 22)
        Me.OpenGraphTableToolStripMenuItem.Text = "Open Graph Table"
        '
        'NetworkGraphToolStripMenuItem
        '
        Me.NetworkGraphToolStripMenuItem.Name = "NetworkGraphToolStripMenuItem"
        Me.NetworkGraphToolStripMenuItem.Size = New System.Drawing.Size(156, 22)
        Me.NetworkGraphToolStripMenuItem.Text = "Network Graph"
        '
        'NodeMetadataToolStripMenuItem
        '
        Me.NodeMetadataToolStripMenuItem.Name = "NodeMetadataToolStripMenuItem"
        Me.NodeMetadataToolStripMenuItem.Size = New System.Drawing.Size(156, 22)
        Me.NodeMetadataToolStripMenuItem.Text = "Node Metadata"
        '
        'SnapshotToolStripMenuItem
        '
        Me.SnapshotToolStripMenuItem.Image = CType(resources.GetObject("SnapshotToolStripMenuItem.Image"), System.Drawing.Image)
        Me.SnapshotToolStripMenuItem.Name = "SnapshotToolStripMenuItem"
        Me.SnapshotToolStripMenuItem.Size = New System.Drawing.Size(198, 22)
        Me.SnapshotToolStripMenuItem.Text = "Snapshot"
        '
        'CopyNetworkVisualizeToolStripMenuItem
        '
        Me.CopyNetworkVisualizeToolStripMenuItem.Name = "CopyNetworkVisualizeToolStripMenuItem"
        Me.CopyNetworkVisualizeToolStripMenuItem.Size = New System.Drawing.Size(198, 22)
        Me.CopyNetworkVisualizeToolStripMenuItem.Text = "Copy Network Visualize"
        '
        'ExportCytoscapeToolStripMenuItem
        '
        Me.ExportCytoscapeToolStripMenuItem.Image = CType(resources.GetObject("ExportCytoscapeToolStripMenuItem.Image"), System.Drawing.Image)
        Me.ExportCytoscapeToolStripMenuItem.Name = "ExportCytoscapeToolStripMenuItem"
        Me.ExportCytoscapeToolStripMenuItem.Size = New System.Drawing.Size(198, 22)
        Me.ExportCytoscapeToolStripMenuItem.Text = "Export Cytoscape"
        '
        'ToolTipWithPictureOrGif1
        '
        Me.ToolTipWithPictureOrGif1.OwnerDraw = True

        'Canvas1
        '
        Me.Canvas1.AutoRotate = False
        Me.Canvas1.BackColor = System.Drawing.Color.FromArgb(CType(CType(223, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Canvas1.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Canvas1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Canvas1.DynamicsRadius = False
        Me.Canvas1.Font = New System.Drawing.Font("Microsoft YaHei UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Canvas1.Location = New System.Drawing.Point(0, 0)
        Me.Canvas1.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Canvas1.Name = "Canvas1"
        Me.Canvas1.ShowLabel = False
        Me.Canvas1.Size = New System.Drawing.Size(888, 520)
        Me.Canvas1.TabIndex = 1
        Me.Canvas1.ViewDistance = -450.0R

        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1, Me.ToolStripStatusLabel2})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 498)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(888, 22)
        Me.StatusStrip1.TabIndex = 1
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(119, 17)
        Me.ToolStripStatusLabel1.Text = "ToolStripStatusLabel1"
        '
        'ToolStripStatusLabel2
        '
        Me.ToolStripStatusLabel2.Name = "ToolStripStatusLabel2"
        Me.ToolStripStatusLabel2.Size = New System.Drawing.Size(119, 17)
        Me.ToolStripStatusLabel2.Text = "ToolStripStatusLabel2"
        '
        'frmNetworkViewer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(888, 520)
        Me.Controls.Add(Canvas1)
        Me.Controls.Add(Me.StatusStrip1)
        Me.DoubleBuffered = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmNetworkViewer"
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents PhysicalEngineToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents Canvas1 As Microsoft.VisualBasic.Data.visualize.Network.Canvas.Canvas
    Friend WithEvents ConfigLayoutToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ShowLabelsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As ToolStripSeparator
    Friend WithEvents SnapshotToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CopyNetworkVisualizeToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PinToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As ToolStripSeparator
    Friend WithEvents DToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolTipWithPictureOrGif1 As ToolTipWithPictureOrGif
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents ToolStripStatusLabel1 As ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabel2 As ToolStripStatusLabel
    Friend WithEvents OpenGraphTableToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents NetworkGraphToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents NodeMetadataToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExportCytoscapeToolStripMenuItem As ToolStripMenuItem
End Class
