Imports System.Windows.Forms
Imports Galaxy.Data.TableSheet
Imports Galaxy.Workbench.DockDocument

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormDataSheet
    Inherits DocumentWindow

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
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
        table = New AdvancedDataGridView()
        toolbar = New AdvancedDataGridViewSearchToolBar()
        CType(table, System.ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' table
        ' 
        table.BackgroundColor = Drawing.Color.Gainsboro
        table.Dock = DockStyle.Fill
        table.FilterAndSortEnabled = True
        table.FilterStringChangedInvokeBeforeDatasourceUpdate = True
        table.Location = New System.Drawing.Point(0, 27)
        table.MaxFilterButtonImageHeight = 23
        table.MultiSelect = False
        table.Name = "table"
        table.RightToLeft = RightToLeft.No
        table.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        table.Size = New System.Drawing.Size(800, 423)
        table.SortStringChangedInvokeBeforeDatasourceUpdate = True
        table.TabIndex = 1
        ' 
        ' toolbar
        ' 
        toolbar.AllowMerge = False
        toolbar.GripStyle = ToolStripGripStyle.Hidden
        toolbar.Location = New System.Drawing.Point(0, 0)
        toolbar.MaximumSize = New System.Drawing.Size(0, 27)
        toolbar.MinimumSize = New System.Drawing.Size(0, 27)
        toolbar.Name = "toolbar"
        toolbar.RenderMode = ToolStripRenderMode.Professional
        toolbar.Size = New System.Drawing.Size(800, 27)
        toolbar.TabIndex = 2
        ' 
        ' FormDataSheet
        ' 
        AutoScaleDimensions = New System.Drawing.SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New System.Drawing.Size(800, 450)
        Controls.Add(table)
        Controls.Add(toolbar)
        DockAreas = Microsoft.VisualStudio.WinForms.Docking.DockAreas.Float Or Microsoft.VisualStudio.WinForms.Docking.DockAreas.DockLeft Or Microsoft.VisualStudio.WinForms.Docking.DockAreas.DockRight Or Microsoft.VisualStudio.WinForms.Docking.DockAreas.DockTop Or Microsoft.VisualStudio.WinForms.Docking.DockAreas.DockBottom Or Microsoft.VisualStudio.WinForms.Docking.DockAreas.Document
        DoubleBuffered = True
        Name = "FormDataSheet"
        ShowHint = Microsoft.VisualStudio.WinForms.Docking.DockState.Unknown
        TabPageContextMenuStrip = DockContextMenuStrip1
        CType(table, System.ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Dim WithEvents table As AdvancedDataGridView
    Dim WithEvents toolbar As AdvancedDataGridViewSearchToolBar
End Class
