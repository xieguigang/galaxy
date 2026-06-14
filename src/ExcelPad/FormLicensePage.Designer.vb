Imports System.Windows.Forms
Imports Galaxy.Workbench.DockDocument
Imports Galaxy.Workbench.LicenseFramework.[Shared]

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormLicensePage : Inherits DocumentWindow

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
        mainPanel = New Panel()
        lblWelcome = New Label()
        btnLicense = New Button()
        mainPanel.SuspendLayout()
        SuspendLayout()
        ' 
        ' mainPanel
        ' 
        mainPanel.Controls.Add(lblWelcome)
        mainPanel.Controls.Add(btnLicense)
        mainPanel.Dock = DockStyle.Fill
        mainPanel.Location = New System.Drawing.Point(0, 0)
        mainPanel.Name = "mainPanel"
        mainPanel.Size = New System.Drawing.Size(784, 561)
        mainPanel.TabIndex = 1
        ' 
        ' lblWelcome
        ' 
        lblWelcome.AutoSize = True
        lblWelcome.Font = New System.Drawing.Font("Microsoft YaHei", 16F, Drawing.FontStyle.Bold)
        lblWelcome.Location = New System.Drawing.Point(20, 20)
        lblWelcome.Name = "lblWelcome"
        lblWelcome.Size = New System.Drawing.Size(255, 30)
        lblWelcome.TabIndex = 0
        lblWelcome.Text = "欢迎使用我的商业软件！"
        ' 
        ' btnLicense
        ' 
        btnLicense.Location = New System.Drawing.Point(20, 200)
        btnLicense.Name = "btnLicense"
        btnLicense.Size = New System.Drawing.Size(120, 35)
        btnLicense.TabIndex = 1
        btnLicense.Text = "授权管理"
        ' 
        ' FormLicensePage
        ' 
        AutoScaleDimensions = New System.Drawing.SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New System.Drawing.Size(784, 561)
        Controls.Add(mainPanel)
        DockAreas = Microsoft.VisualStudio.WinForms.Docking.DockAreas.Float Or Microsoft.VisualStudio.WinForms.Docking.DockAreas.DockLeft Or Microsoft.VisualStudio.WinForms.Docking.DockAreas.DockRight Or Microsoft.VisualStudio.WinForms.Docking.DockAreas.DockTop Or Microsoft.VisualStudio.WinForms.Docking.DockAreas.DockBottom Or Microsoft.VisualStudio.WinForms.Docking.DockAreas.Document
        DoubleBuffered = True
        Name = "FormLicensePage"
        ShowHint = Microsoft.VisualStudio.WinForms.Docking.DockState.Unknown
        StartPosition = FormStartPosition.CenterScreen
        TabPageContextMenuStrip = DockContextMenuStrip1
        Text = "我的商业软件 v1.0.0"
        mainPanel.ResumeLayout(False)
        mainPanel.PerformLayout()
        ResumeLayout(False)
    End Sub

    Dim WithEvents btnLicense As Button
    Dim WithEvents mainPanel As Panel
    Dim WithEvents lblWelcome As Label

End Class
