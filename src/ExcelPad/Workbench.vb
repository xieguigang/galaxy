Imports System.Windows.Forms
Imports Galaxy.Workbench.LicenseFramework.Client
Imports Galaxy.Workbench.LicenseFramework.[Shared]

Module Workbench

    ReadOnly licenseManager As LicenseManager

    Public Function IsLicensed() As Boolean
        If licenseManager Is Nothing Then
            Return False
        Else
            Return licenseManager.IsLicensed
        End If
    End Function

    Public Function GetCurrentLicense() As LicenseData
        If licenseManager Is Nothing Then
            Return Nothing
        Else
            Return licenseManager.CurrentLicense
        End If
    End Function

    ''' <summary>
    ''' 获取嵌入的RSA公钥
    ''' 实际项目中应将公钥硬编码或混淆后嵌入
    ''' </summary>
    Private Function GetEmbeddedPublicKey() As String
        ' 此处替换为实际的RSA公钥XML
        ' 使用KeyGenerator工具生成密钥对后，将公钥内容粘贴到这里
        Return "<RSAKeyValue><Modulus>qGmRjIgv7AxQqG6yr/E8hR5JzqffyKqAFepMUOlPvrvw1RmzqIT9ziLzezpUrjjPlJR4BdZVkmqRYgcJqtFFFsTgzXtGbBXplqhaye8brejJ7GmLQylr4aOIgpDDQlBAAzQ2x+M2fzl4TAgLOjZQFRhJXrKC1Xhz087yEbrcdwotd8cA8qEI1ureCOXsyC/GSacmrPGrAdI0PyB82d+301oX78bktskwgHNqraOS5dBy97hthLMKE/1dUXfSGWEVtZvGQx9I26P1UDKYRaj9K4v++DzWupcMgs1VXO3YDXj3/oJMhlDT3uKKBgmicrrBAaZxmmFejsA2Y3ZqLQ/rfQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>"
    End Function

    ''' <summary>
    ''' 获取嵌入的HMAC密钥
    ''' 实际项目中应混淆后嵌入
    ''' </summary>
    Private Function GetEmbeddedHmacKey() As String
        ' 此处替换为实际的HMAC密钥（Base64编码）
        Return "YOUR_HMAC_KEY_BASE64_HERE"
    End Function

    Public Function CheckLicense() As Boolean
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
            Dim dialogResult As LicenseValidationResult = licenseManager.ValidateWithDialog

            If Not licenseManager.IsLicensed Then
                ' 用户未完成授权，退出程序
                MessageBox.Show("软件未授权，程序将退出。",
                                "授权验证", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return False
            End If
        End If

        Return True
    End Function

    Public Sub OpenLicenseDialog()
        Call licenseManager.OpenLicenseDialog()
    End Sub
End Module
