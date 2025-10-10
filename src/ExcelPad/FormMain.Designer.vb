<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
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
        Me.components = New System.ComponentModel.Container()
        Me.VisualStudioToolStripExtender1 = New Microsoft.VisualStudio.WinForms.Docking.VisualStudioToolStripExtender(Me.components)
        Me.DockPanel1 = New Microsoft.VisualStudio.WinForms.Docking.DockPanel()
        Me.SuspendLayout()
        '
        'VisualStudioToolStripExtender1
        '
        Me.VisualStudioToolStripExtender1.DefaultRenderer = Nothing
        '
        'DockPanel1
        '
        Me.DockPanel1.Location = New System.Drawing.Point(155, 144)
        Me.DockPanel1.Name = "DockPanel1"
        Me.DockPanel1.Size = New System.Drawing.Size(200, 100)
        Me.DockPanel1.TabIndex = 0
        '
        'FormMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.DockPanel1)
        Me.Name = "FormMain"
        Me.Text = "Form1"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents VisualStudioToolStripExtender1 As Microsoft.VisualStudio.WinForms.Docking.VisualStudioToolStripExtender
    Friend WithEvents DockPanel1 As Microsoft.VisualStudio.WinForms.Docking.DockPanel
End Class
