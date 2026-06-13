Imports Galaxy.CommonControls.ModernTreeListView
Imports Galaxy.Workbench.DockDocument
Imports LicenseVendor

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormLicenseList
    Inherits DocumentWindow

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
        TreeListView1 = New ModernTreeListView(Of LicenseUser)()
        SuspendLayout()
        ' 
        ' TreeListView1
        ' 
        TreeListView1.BackColor = Color.White
        TreeListView1.Dock = DockStyle.Fill
        TreeListView1.Font = New Font("Segoe UI", 9.5F)
        TreeListView1.ForeColor = Color.FromArgb(CByte(33), CByte(37), CByte(41))
        TreeListView1.Location = New Point(0, 0)
        TreeListView1.Name = "TreeListView1"
        TreeListView1.Size = New Size(939, 537)
        TreeListView1.TabIndex = 0
        ' 
        ' FormLicenseList
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(939, 537)
        Controls.Add(TreeListView1)
        Name = "FormLicenseList"
        Text = "客户列表"
        ResumeLayout(False)
    End Sub

    Dim WithEvents TreeListView1 As ModernTreeListView(Of LicenseUser)
End Class
