#Region "Microsoft.VisualBasic::2bf889b9fdb2fdbe4073b82cee9ff964, mzkit\src\mzkit\mzkit\pages\dockWindow\documents\frmTableViewer.Designer.vb"

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

'   Total Lines: 128
'    Code Lines: 92
' Comment Lines: 32
'   Blank Lines: 4
'     File Size: 7.02 KB


' Class frmTableViewer
' 
'     Sub: Dispose, InitializeComponent
' 
' /********************************************************************************/

#End Region

Imports Galaxy.Data.TableSheet
Imports Galaxy.Workbench.DockDocument

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormExcelPad
    Inherits DocumentWindow
    ' Inherits Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        components = New ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormExcelPad))
        ContextMenuStrip1 = New ContextMenuStrip(components)
        ViewToolStripMenuItem = New ToolStripMenuItem()
        ToolStripMenuItem1 = New ToolStripSeparator()
        VisualizeToolStripMenuItem = New ToolStripMenuItem()
        ActionsToolStripMenuItem = New ToolStripMenuItem()
        SendToToolStripMenuItem = New ToolStripMenuItem()

        SendToREnvironmentToolStripMenuItem = New ToolStripMenuItem()
        ToolStripMenuItem2 = New ToolStripSeparator()
        TransposeToolStripMenuItem = New ToolStripMenuItem()
        CopyToolStripMenuItem = New ToolStripMenuItem()
        ExportTableToolStripMenuItem = New ToolStripMenuItem()
        AdvancedDataGridView1 = New AdvancedDataGridView()
        AdvancedDataGridViewSearchToolBar1 = New AdvancedDataGridViewSearchToolBar()
        ContextMenuStrip1.SuspendLayout()
        CType(AdvancedDataGridView1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' ContextMenuStrip1
        ' 
        ContextMenuStrip1.Items.AddRange(New ToolStripItem() {ViewToolStripMenuItem, ToolStripMenuItem1, VisualizeToolStripMenuItem, ActionsToolStripMenuItem, SendToToolStripMenuItem, ToolStripMenuItem2, TransposeToolStripMenuItem, CopyToolStripMenuItem, ExportTableToolStripMenuItem})
        ContextMenuStrip1.Name = "ContextMenuStrip1"
        ContextMenuStrip1.Size = New Size(181, 192)
        ' 
        ' ViewToolStripMenuItem
        ' 
        ViewToolStripMenuItem.Image = CType(resources.GetObject("ViewToolStripMenuItem.Image"), Image)
        ViewToolStripMenuItem.Name = "ViewToolStripMenuItem"
        ViewToolStripMenuItem.Size = New Size(180, 22)
        ViewToolStripMenuItem.Text = "View"
        ' 
        ' ToolStripMenuItem1
        ' 
        ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        ToolStripMenuItem1.Size = New Size(177, 6)
        ' 
        ' VisualizeToolStripMenuItem
        ' 
        VisualizeToolStripMenuItem.Image = CType(resources.GetObject("VisualizeToolStripMenuItem.Image"), Image)
        VisualizeToolStripMenuItem.Name = "VisualizeToolStripMenuItem"
        VisualizeToolStripMenuItem.Size = New Size(180, 22)
        VisualizeToolStripMenuItem.Text = "Visualize"
        ' 
        ' ActionsToolStripMenuItem
        ' 
        ActionsToolStripMenuItem.Image = CType(resources.GetObject("ActionsToolStripMenuItem.Image"), Image)
        ActionsToolStripMenuItem.Name = "ActionsToolStripMenuItem"
        ActionsToolStripMenuItem.Size = New Size(180, 22)
        ActionsToolStripMenuItem.Text = "Actions"
        ' 
        ' SendToToolStripMenuItem
        ' 
        SendToToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {SendToREnvironmentToolStripMenuItem})
        SendToToolStripMenuItem.Name = "SendToToolStripMenuItem"
        SendToToolStripMenuItem.Size = New Size(180, 22)
        SendToToolStripMenuItem.Text = "Send To"
        ' 
        ' MSImagingIonListToolStripMenuItem
        ' 

        ' 
        ' SendToREnvironmentToolStripMenuItem
        ' 
        SendToREnvironmentToolStripMenuItem.Name = "SendToREnvironmentToolStripMenuItem"
        SendToREnvironmentToolStripMenuItem.Size = New Size(181, 22)
        SendToREnvironmentToolStripMenuItem.Text = "R# Environment"
        ' 
        ' ToolStripMenuItem2
        ' 
        ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        ToolStripMenuItem2.Size = New Size(177, 6)
        ' 
        ' TransposeToolStripMenuItem
        ' 
        TransposeToolStripMenuItem.Name = "TransposeToolStripMenuItem"
        TransposeToolStripMenuItem.Size = New Size(180, 22)
        TransposeToolStripMenuItem.Text = "Transpose"
        ' 
        ' CopyToolStripMenuItem
        ' 
        CopyToolStripMenuItem.Image = CType(resources.GetObject("CopyToolStripMenuItem.Image"), Image)
        CopyToolStripMenuItem.Name = "CopyToolStripMenuItem"
        CopyToolStripMenuItem.Size = New Size(180, 22)
        CopyToolStripMenuItem.Text = "Copy"
        ' 
        ' ExportTableToolStripMenuItem
        ' 
        ExportTableToolStripMenuItem.Image = CType(resources.GetObject("ExportTableToolStripMenuItem.Image"), Image)
        ExportTableToolStripMenuItem.Name = "ExportTableToolStripMenuItem"
        ExportTableToolStripMenuItem.Size = New Size(180, 22)
        ExportTableToolStripMenuItem.Text = "Export Table"
        ' 
        ' AdvancedDataGridView1
        ' 
        AdvancedDataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        AdvancedDataGridView1.ContextMenuStrip = ContextMenuStrip1
        AdvancedDataGridView1.Dock = DockStyle.Fill
        AdvancedDataGridView1.FilterAndSortEnabled = True
        AdvancedDataGridView1.FilterStringChangedInvokeBeforeDatasourceUpdate = True
        AdvancedDataGridView1.Location = New Point(0, 27)
        AdvancedDataGridView1.MaxFilterButtonImageHeight = 23
        AdvancedDataGridView1.Name = "AdvancedDataGridView1"
        AdvancedDataGridView1.RightToLeft = RightToLeft.No
        AdvancedDataGridView1.RowTemplate.Height = 23
        AdvancedDataGridView1.Size = New Size(834, 468)
        AdvancedDataGridView1.SortStringChangedInvokeBeforeDatasourceUpdate = True
        AdvancedDataGridView1.TabIndex = 1
        ' 
        ' AdvancedDataGridViewSearchToolBar1
        ' 
        AdvancedDataGridViewSearchToolBar1.AllowMerge = False
        AdvancedDataGridViewSearchToolBar1.GripStyle = ToolStripGripStyle.Hidden
        AdvancedDataGridViewSearchToolBar1.Location = New Point(0, 0)
        AdvancedDataGridViewSearchToolBar1.MaximumSize = New Size(0, 27)
        AdvancedDataGridViewSearchToolBar1.MinimumSize = New Size(0, 27)
        AdvancedDataGridViewSearchToolBar1.Name = "AdvancedDataGridViewSearchToolBar1"
        AdvancedDataGridViewSearchToolBar1.RenderMode = ToolStripRenderMode.Professional
        AdvancedDataGridViewSearchToolBar1.Size = New Size(834, 27)
        AdvancedDataGridViewSearchToolBar1.TabIndex = 2
        AdvancedDataGridViewSearchToolBar1.Text = "AdvancedDataGridViewSearchToolBar1"
        ' 
        ' FormExcelPad
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(834, 495)
        Controls.Add(AdvancedDataGridView1)
        Controls.Add(AdvancedDataGridViewSearchToolBar1)
        DockAreas = Microsoft.VisualStudio.WinForms.Docking.DockAreas.Float Or Microsoft.VisualStudio.WinForms.Docking.DockAreas.DockLeft Or Microsoft.VisualStudio.WinForms.Docking.DockAreas.DockRight Or Microsoft.VisualStudio.WinForms.Docking.DockAreas.DockTop Or Microsoft.VisualStudio.WinForms.Docking.DockAreas.DockBottom Or Microsoft.VisualStudio.WinForms.Docking.DockAreas.Document
        DoubleBuffered = True
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        Name = "FormExcelPad"
        ShowHint = Microsoft.VisualStudio.WinForms.Docking.DockState.Unknown
        TabPageContextMenuStrip = DockContextMenuStrip1
        ContextMenuStrip1.ResumeLayout(False)
        CType(AdvancedDataGridView1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()

    End Sub
    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents ViewToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As ToolStripSeparator
    Friend WithEvents VisualizeToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SendToREnvironmentToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ActionsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AdvancedDataGridView1 As AdvancedDataGridView
    Friend WithEvents AdvancedDataGridViewSearchToolBar1 As AdvancedDataGridViewSearchToolBar

    Friend WithEvents ToolStripMenuItem2 As ToolStripSeparator
    Friend WithEvents ExportTableToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CopyToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents TransposeToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SendToToolStripMenuItem As ToolStripMenuItem

End Class
