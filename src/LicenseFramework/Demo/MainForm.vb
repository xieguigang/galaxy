Imports System.Windows.Forms
Imports LicenseVendor.LicenseFramework.Client
Imports LicenseVendor.LicenseFramework.Shared

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
Public Class MainForm
    Inherits Form

    Private _licenseManager As LicenseManager

    Public Shared Sub Main()
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)

        ' ===== 第一步：初始化授权管理器 =====
        Dim licenseManager As New LicenseManager(
            publicKeyXml:=GetEmbeddedPublicKey(),
            productName:="我的商业软件",
            productVersion:="1.0.0",
            serverUrl:="https://license.example.com/api/activate",
            hmacKey:=GetEmbeddedHmacKey()
        )

        ' ===== 第二步：执行授权验证 =====
        Dim result As LicenseValidationResult = licenseManager.Validate()

        If Not result.IsValid Then
            ' ===== 第三步：验证失败，显示授权对话框 =====
            Dim dialogResult As LicenseValidationResult = licenseManager.ValidateWithDialog(Nothing)

            If Not licenseManager.IsLicensed Then
                ' 用户未完成授权，退出程序
                MessageBox.Show("软件未授权，程序将退出。",
                                "授权验证", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If
        End If

        ' ===== 第四步：授权通过，显示主界面 =====
        Application.Run(New MainForm(licenseManager))
    End Sub

    ''' <summary>
    ''' 获取嵌入的RSA公钥
    ''' 实际项目中应将公钥硬编码或混淆后嵌入
    ''' </summary>
    Private Shared Function GetEmbeddedPublicKey() As String
        ' 此处替换为实际的RSA公钥XML
        ' 使用KeyGenerator工具生成密钥对后，将公钥内容粘贴到这里
        Return "<RSAKeyValue><Modulus>qGmRjIgv7AxQqG6yr/E8hR5JzqffyKqAFepMUOlPvrvw1RmzqIT9ziLzezpUrjjPlJR4BdZVkmqRYgcJqtFFFsTgzXtGbBXplqhaye8brejJ7GmLQylr4aOIgpDDQlBAAzQ2x+M2fzl4TAgLOjZQFRhJXrKC1Xhz087yEbrcdwotd8cA8qEI1ureCOXsyC/GSacmrPGrAdI0PyB82d+301oX78bktskwgHNqraOS5dBy97hthLMKE/1dUXfSGWEVtZvGQx9I26P1UDKYRaj9K4v++DzWupcMgs1VXO3YDXj3/oJMhlDT3uKKBgmicrrBAaZxmmFejsA2Y3ZqLQ/rfQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>"
    End Function

    ''' <summary>
    ''' 获取嵌入的HMAC密钥
    ''' 实际项目中应混淆后嵌入
    ''' </summary>
    Private Shared Function GetEmbeddedHmacKey() As String
        ' 此处替换为实际的HMAC密钥（Base64编码）
        Return "YOUR_HMAC_KEY_BASE64_HERE"
    End Function

    Public Sub New(licenseManager As LicenseManager)
        _licenseManager = licenseManager
        InitializeComponents()
    End Sub

    Private Sub InitializeComponents()
        Me.Text = "我的商业软件 v1.0.0"
        Me.Size = New System.Drawing.Size(800, 600)
        Me.StartPosition = FormStartPosition.CenterScreen

        ' 状态栏
        Dim statusPanel As New Panel With {
            .Dock = DockStyle.Bottom,
            .Height = 30,
            .BackColor = System.Drawing.SystemColors.Control
        }

        Dim lblStatus As New Label With {
            .Text = If(_licenseManager.IsLicensed,
                       $"已授权 - {_licenseManager.CurrentLicense?.LicenseType.ToString()} - 到期: {_licenseManager.CurrentLicense?.ExpiryDate}",
                       "未授权"),
            .Dock = DockStyle.Fill,
            .TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        }
        statusPanel.Controls.Add(lblStatus)
        Me.Controls.Add(statusPanel)

        ' 主内容
        Dim mainPanel As New Panel With {
            .Dock = DockStyle.Fill
        }

        Dim lblWelcome As New Label With {
            .Text = "欢迎使用我的商业软件！",
            .Font = New System.Drawing.Font("Microsoft YaHei", 16, System.Drawing.FontStyle.Bold),
            .Location = New System.Drawing.Point(20, 20),
            .AutoSize = True
        }
        mainPanel.Controls.Add(lblWelcome)

        ' 授权信息
        If _licenseManager.IsLicensed Then
            Dim license As LicenseData = _licenseManager.CurrentLicense
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

        ' 授权管理按钮
        btnLicense = New Button With {
            .Text = "授权管理",
            .Size = New System.Drawing.Size(120, 35),
            .Location = New System.Drawing.Point(20, 200)
        }

        mainPanel.Controls.Add(btnLicense)

        Me.Controls.Add(mainPanel)
    End Sub

    Dim WithEvents btnLicense As New Button

    Private Sub BtnLicense_Click(sender As Object, e As EventArgs) Handles btnLicense.Click
        _licenseManager.OpenLicenseDialog(Me)
    End Sub

End Class
