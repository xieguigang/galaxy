Imports System.Windows.Forms
Imports Galaxy.Workbench
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

    Sub New()
        Call CommonRuntime.SetSeed(&HA7)

        ' ===== 第一步：初始化授权管理器 =====
        licenseManager = New LicenseManager(
            publicKeyXml:=GetEmbeddedPublicKey(),
            productName:="我的商业软件",
            productVersion:="1.0.0",
            serverUrl:="https://license.example.com/api/activate",
            hmacKey:=GetEmbeddedHmacKey()
        )
    End Sub

    ''' <summary>
    ''' 获取嵌入的RSA公钥
    ''' 实际项目中应将公钥硬编码或混淆后嵌入
    ''' </summary>
    Private Function GetEmbeddedPublicKey() As String
        ' 此处替换为实际的RSA公钥XML
        ' 使用KeyGenerator工具生成密钥对后，将公钥内容粘贴到这里
        Return CommonRuntime.AssembleKey("m/X05uzC3vHGy9LCmZvqyMPSy9LUmdbgyvXN7sDRkObf9tbgkd7ViOKfz/WS7d3WwcHe7Nbm4cLX6vLoy/fR1dHQlvXK3dbu857dzuvdwt3X8tXNzffL7fWT5cP98czK1vX+wMTt1tP",
           "h4eHU88Dd/9PgxeX/18vWz8bewp/F1cLN7ZDgyuv23svVk8bo7sDX4+P2y+Xm5t32ld+M6pXB3cuT8+bA6+jN/fbh9c/t/9Xs5Jb/z92Xn5De4sXVxMPQyNPDn8Tmn9bi7pbS1cLk6P",
           "/U3uSI4PTGxMrV9+DV5sPul/fe5Z+Vw4yUl5bI/5CfxczT1MzQwO/p1tXG6PSSw+XenpDP08/r6uziiJbD8v/B9ODw4vHT/dHg9t+e7pWR95by4+z+9cbNnuyT0YyM493w0tfE6sDUl",
           "vH/6JT+4//NlIjI7erPy+PzlNLs7OXAys7E1dXl5sb938rK4cLN1OaV/pT91uv2iNXB9pqam4jqyMPSy9LUmZvi39fIycLJ05nm9ublm4ji39fIycLJ05mbiPX05uzC3vHGy9LCmQ==")
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

    Public Sub SetLicenseStatus()
        DirectCast(CommonRuntime.AppHost, FormMain).ToolStripStatusLabel3.Text = LicenseData.SimpleDescription(Workbench.GetCurrentLicense)
    End Sub
End Module
