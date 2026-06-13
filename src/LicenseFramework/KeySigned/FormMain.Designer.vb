Imports RibbonLib

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormMain
    Inherits System.Windows.Forms.Form

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
        m_dockPanel = New Microsoft.VisualStudio.WinForms.Docking.DockPanel()
        Ribbon1 = New Ribbon()
        StatusStrip1 = New StatusStrip()
        ToolStripStatusLabel1 = New ToolStripStatusLabel()
        StatusStrip1.SuspendLayout()
        SuspendLayout()
        ' 
        ' m_dockPanel
        ' 
        m_dockPanel.Dock = DockStyle.Fill
        m_dockPanel.Location = New Point(0, 116)
        m_dockPanel.Margin = New Padding(4)
        m_dockPanel.Name = "m_dockPanel"
        m_dockPanel.Size = New Size(800, 312)
        m_dockPanel.TabIndex = 3
        ' 
        ' Ribbon1
        ' 
        Ribbon1.Location = New Point(0, 0)
        Ribbon1.Name = "Ribbon1"
        Ribbon1.ResourceIdentifier = Nothing
        Ribbon1.ResourceName = "KeySigned.RibbonMarkup.ribbon"
        Ribbon1.ShortcutTableResourceName = Nothing
        Ribbon1.Size = New Size(800, 116)
        Ribbon1.TabIndex = 5
        ' 
        ' StatusStrip1
        ' 
        StatusStrip1.Items.AddRange(New ToolStripItem() {ToolStripStatusLabel1})
        StatusStrip1.Location = New Point(0, 428)
        StatusStrip1.Name = "StatusStrip1"
        StatusStrip1.Padding = New Padding(1, 0, 16, 0)
        StatusStrip1.Size = New Size(800, 22)
        StatusStrip1.TabIndex = 4
        StatusStrip1.Text = "StatusStrip1"
        ' 
        ' ToolStripStatusLabel1
        ' 
        ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        ToolStripStatusLabel1.Size = New Size(42, 17)
        ToolStripStatusLabel1.Text = "Ready!"
        ' 
        ' FormMain
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(800, 450)
        Controls.Add(m_dockPanel)
        Controls.Add(Ribbon1)
        Controls.Add(StatusStrip1)
        Name = "FormMain"
        Text = "软件授权管理工具"
        StatusStrip1.ResumeLayout(False)
        StatusStrip1.PerformLayout()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents m_dockPanel As Microsoft.VisualStudio.WinForms.Docking.DockPanel
    Friend WithEvents Ribbon1 As Ribbon
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents ToolStripStatusLabel1 As ToolStripStatusLabel

End Class
