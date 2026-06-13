Imports System.ComponentModel
Imports Galaxy.Workbench.LicenseFramework.Client

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
        Workbench.OpenLicenseDialog(Me)
    End Sub
End Class