Imports System.Windows.Forms
Imports RibbonLib

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormMain
    Inherits System.Windows.Forms.Form

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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        m_dockPanel = New Microsoft.VisualStudio.WinForms.Docking.DockPanel()
        StatusStrip1 = New StatusStrip()
        ToolStripStatusLabel1 = New ToolStripStatusLabel()
        Ribbon1 = New Ribbon()
        StatusStrip1.SuspendLayout()
        SuspendLayout()
        ' 
        ' m_dockPanel
        ' 
        m_dockPanel.Dock = DockStyle.Fill
        m_dockPanel.Location = New System.Drawing.Point(0, 116)
        m_dockPanel.Margin = New Padding(4, 5, 4, 5)
        m_dockPanel.Name = "m_dockPanel"
        m_dockPanel.Size = New System.Drawing.Size(933, 499)
        m_dockPanel.TabIndex = 0
        ' 
        ' StatusStrip1
        ' 
        StatusStrip1.Items.AddRange(New ToolStripItem() {ToolStripStatusLabel1})
        StatusStrip1.Location = New System.Drawing.Point(0, 615)
        StatusStrip1.Name = "StatusStrip1"
        StatusStrip1.Padding = New Padding(1, 0, 16, 0)
        StatusStrip1.Size = New System.Drawing.Size(933, 22)
        StatusStrip1.TabIndex = 1
        StatusStrip1.Text = "StatusStrip1"
        ' 
        ' ToolStripStatusLabel1
        ' 
        ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        ToolStripStatusLabel1.Size = New System.Drawing.Size(48, 17)
        ToolStripStatusLabel1.Text = "Ready!"
        ' 
        ' Ribbon1
        ' 
        Ribbon1.Location = New System.Drawing.Point(0, 0)
        Ribbon1.Name = "Ribbon1"
        Ribbon1.ResourceIdentifier = Nothing
        Ribbon1.ResourceName = "ExcelPad.RibbonMarkup.ribbon"
        Ribbon1.ShortcutTableResourceName = Nothing
        Ribbon1.Size = New System.Drawing.Size(933, 116)
        Ribbon1.TabIndex = 2
        ' 
        ' FormMain
        ' 
        AutoScaleDimensions = New System.Drawing.SizeF(7F, 17F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New System.Drawing.Size(933, 637)
        Controls.Add(m_dockPanel)
        Controls.Add(Ribbon1)
        Controls.Add(StatusStrip1)
        Margin = New Padding(4, 5, 4, 5)
        Name = "FormMain"
        Text = "Excel Pad"
        StatusStrip1.ResumeLayout(False)
        StatusStrip1.PerformLayout()
        ResumeLayout(False)
        PerformLayout()

    End Sub
    Friend WithEvents m_dockPanel As Microsoft.VisualStudio.WinForms.Docking.DockPanel
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents ToolStripStatusLabel1 As ToolStripStatusLabel
    Friend WithEvents Ribbon1 As Ribbon
End Class
