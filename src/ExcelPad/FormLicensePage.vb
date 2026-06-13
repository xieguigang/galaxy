Imports System.Windows.Forms
Imports Galaxy.Workbench.LicenseFramework.[Shared]

''' <summary>
''' 主窗体 - 演示授权框架集成
''' 
''' 集成步骤：
''' 1. 在程序启动时创建LicenseManager实例
''' 2. 调用Validate()或ValidateWithDialog()进行授权验证
''' 3. 验证通过后显示主界面，验证失败则退出或限制功能
''' </summary>
'''
''' ---------------------------------------------------------------
''' WinForms集成示例 (MainForm.vb)
''' 演示如何在WinForms程序中集成授权验证框架
''' ---------------------------------------------------------------
'''
Public Class FormLicensePage

    Private Sub BtnLicense_Click(sender As Object, e As EventArgs) Handles btnLicense.Click
        Workbench.OpenLicenseDialog()
    End Sub

    Private Sub FormLicensePage_Load(sender As Object, e As EventArgs) Handles Me.Load
        lblStatus.Text = LicenseData.SimpleDescription(Workbench.GetCurrentLicense)

        ' 授权信息
        If Workbench.IsLicensed Then
            Dim license As LicenseData = Workbench.GetCurrentLicense
            Dim lblInfo As New Label With {
                .Text = $"许可证类型: {license.LicenseType.ToString()}" & Environment.NewLine &
                        $"客户名称: {license.CustomerName}" & Environment.NewLine &
                        $"授权日期: {license.IssueDate}" & Environment.NewLine &
                        $"到期日期: {If(String.IsNullOrEmpty(license.ExpiryDate), "永久", license.ExpiryDate)}",
                .Location = New System.Drawing.Point(20, 70),
                .AutoSize = True
            }
            mainPanel.Controls.Add(lblInfo)
        End If
    End Sub

    Private Sub FormLicensePage_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Call Workbench.SetLicenseStatus()
    End Sub
End Class