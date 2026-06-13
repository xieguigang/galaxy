Imports System.Text
Imports Microsoft.VisualBasic.Serialization.JSON

#If WORKBENCH Then
Imports Galaxy.Workbench.LicenseFramework.Shared
#End If
#If VENDOR_TOOL Then
Imports LicenseVendor.LicenseFramework.Shared
#End If

Namespace LicenseFramework.Client

    ''' <summary>
    ''' 许可证验证器
    ''' 
    ''' 验证流程：
    ''' 1. 拆分许可证为数据部分和签名部分
    ''' 2. 使用RSA公钥验证签名
    ''' 3. 反序列化JSON数据
    ''' 4. 比对硬件指纹
    ''' 5. 检查过期时间
    ''' </summary>
    '''
    ''' ---------------------------------------------------------------
    ''' 许可证验证模块 (LicenseValidator.vb) - 客户端
    ''' 验证许可证的签名完整性和硬件指纹匹配性
    ''' ---------------------------------------------------------------
    '''
    Public Class LicenseValidator

        Private _publicKeyXml As String

        Public Sub New(publicKeyXml As String)
            If String.IsNullOrEmpty(publicKeyXml) Then
                Throw New ArgumentNullException(NameOf(publicKeyXml), "RSA公钥不能为空")
            End If
            _publicKeyXml = publicKeyXml
        End Sub

        ''' <summary>
        ''' 验证签名许可证字符串（完整验证：签名+指纹+过期）
        ''' </summary>
        Public Function Validate(signedLicense As String, currentHardwareFingerprint As String) As LicenseValidationResult
            Try
                Return ValidateLicenseInternal(signedLicense, currentHardwareFingerprint)
            Catch ex As FormatException
                Return LicenseValidationResult.Fail(LicenseStatus.Malformed, $"许可证格式错误: {ex.Message}")
            Catch ex As Exception
                Return LicenseValidationResult.Fail(LicenseStatus.UnknownError, ex.Message)
            End Try
        End Function

        Private Function ValidateLicenseInternal(signedLicense As String, currentHardwareFingerprint As String) As LicenseValidationResult
            ' 第一步：验证签名
            Dim signatureResult As LicenseValidationResult = ValidateSignatureOnly(signedLicense)
            If Not signatureResult.IsValid Then
                Return signatureResult
            End If

            Dim licenseData As LicenseData = signatureResult.License

            ' 第二步：验证硬件指纹
            If Not String.Equals(licenseData.HardwareFingerprint,
                                  currentHardwareFingerprint,
                                  StringComparison.OrdinalIgnoreCase) Then
                Return LicenseValidationResult.Fail(LicenseStatus.HardwareMismatch,
                                                    "硬件指纹不匹配，此许可证不适用于当前计算机")
            End If

            ' 第三步：验证过期时间
            If licenseData.IsExpired Then
                Return LicenseValidationResult.Fail(LicenseStatus.Expired,
                                                    $"许可证已于 {licenseData.ExpiryDate} 过期")
            End If

            Return LicenseValidationResult.Success(licenseData)
        End Function

        ''' <summary>
        ''' 仅验证签名，不检查硬件指纹和过期时间
        ''' </summary>
        Public Function ValidateSignatureOnly(signedLicense As String) As LicenseValidationResult
            Try
                If String.IsNullOrEmpty(signedLicense) Then
                    Return LicenseValidationResult.Fail(LicenseStatus.Malformed, "许可证字符串为空")
                Else
                    Return ValidateSignatureOnlyInternal(signedLicense)
                End If
            Catch ex As Exception
                Return LicenseValidationResult.Fail(LicenseStatus.UnknownError, ex.Message)
            End Try
        End Function

        Private Function ValidateSignatureOnlyInternal(signedLicense As String) As LicenseValidationResult
            Dim parts() As String = signedLicense.Split("."c)
            If parts.Length <> 2 Then
                Return LicenseValidationResult.Fail(LicenseStatus.Malformed, "许可证格式错误")
            End If

            Dim jsonBytes As Byte() = Convert.FromBase64String(parts(0))
            Dim jsonString As String = Encoding.UTF8.GetString(jsonBytes)

            If Not CryptoHelper.VerifyString(jsonString, parts(1), _publicKeyXml) Then
                Return LicenseValidationResult.Fail(LicenseStatus.InvalidSignature, "签名验证失败，许可证可能被篡改")
            End If

            Dim licenseData As LicenseData = jsonString.LoadJSON(Of LicenseData)

            If licenseData Is Nothing Then
                Return LicenseValidationResult.Fail(LicenseStatus.Malformed, "许可证数据为空")
            End If

            Return LicenseValidationResult.Success(licenseData)
        End Function

    End Class

End Namespace
