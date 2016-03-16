<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TabControl
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.FlowLayoutPanelLabelsContainer = New System.Windows.Forms.FlowLayoutPanel()
        Me.PanelPagesContainer = New System.Windows.Forms.Panel()
        Me.TabIndicator1 = New Microsoft.VisualBasic.Windows.Forms.TabIndicator()
        Me.SuspendLayout()
        '
        'FlowLayoutPanelLabelsContainer
        '
        Me.FlowLayoutPanelLabelsContainer.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.FlowLayoutPanelLabelsContainer.Dock = System.Windows.Forms.DockStyle.Top
        Me.FlowLayoutPanelLabelsContainer.Location = New System.Drawing.Point(0, 0)
        Me.FlowLayoutPanelLabelsContainer.Name = "FlowLayoutPanelLabelsContainer"
        Me.FlowLayoutPanelLabelsContainer.Size = New System.Drawing.Size(688, 40)
        Me.FlowLayoutPanelLabelsContainer.TabIndex = 0
        '
        'PanelPagesContainer
        '
        Me.PanelPagesContainer.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PanelPagesContainer.Location = New System.Drawing.Point(0, 46)
        Me.PanelPagesContainer.Name = "PanelPagesContainer"
        Me.PanelPagesContainer.Size = New System.Drawing.Size(688, 412)
        Me.PanelPagesContainer.TabIndex = 1
        '
        'TabIndicator1
        '
        Me.TabIndicator1.DefaultDock = 0
        Me.TabIndicator1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabIndicator1.LabelWidth = 63
        Me.TabIndicator1.Location = New System.Drawing.Point(0, 40)
        Me.TabIndicator1.Name = "TabIndicator1"
        Me.TabIndicator1.PointerDock = 0
        Me.TabIndicator1.Size = New System.Drawing.Size(688, 6)
        Me.TabIndicator1.Speed = 30
        Me.TabIndicator1.TabIndex = 2
        '
        'TabControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.TabIndicator1)
        Me.Controls.Add(Me.PanelPagesContainer)
        Me.Controls.Add(Me.FlowLayoutPanelLabelsContainer)
        Me.Name = "TabControl"
        Me.Size = New System.Drawing.Size(688, 458)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents FlowLayoutPanelLabelsContainer As FlowLayoutPanel
    Friend WithEvents PanelPagesContainer As Panel
    Friend WithEvents TabIndicator1 As TabIndicator
End Class
