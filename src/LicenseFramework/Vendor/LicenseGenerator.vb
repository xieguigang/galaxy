Imports System
Imports System.Text
Imports System.IO
Imports LicenseFramework.Shared

Namespace LicenseFramework.Vendor

    ''' <summary>
    ''' 许可证生成器，厂商端核心组件
    ''' 
    ''' 许可证格式: Base64(JSON数据) + "." + Base64(RSA签名)
    ''' </summary>
    ''' <remarks>
    ''' ---------------------------------------------------------------
    ''' 许可证生成模块 (LicenseGenerator.vb) - 厂商端
    ''' 根据客户提供的硬件指纹哈希，生成签名的许可证数据
    ''' ---------------------------------------------------------------
    ''' </remarks>
    Public Class LicenseGenerator

        Private _privateKeyXml As String

        Public Sub New(privateKeyXml As String)
            If String.IsNullOrEmpty(privateKeyXml) Then
                Throw New ArgumentNullException(NameOf(privateKeyXml), "RSA私钥不能为空")
            End If
            _privateKeyXml = privateKeyXml
        End Sub

        ''' <summary>
        ''' 从文件加载RSA私钥构造实例
        ''' </summary>
        Public Shared Function FromKeyFile(privateKeyFilePath As String) As LicenseGenerator
            If Not File.Exists(privateKeyFilePath) Then
                Throw New FileNotFoundException("私钥文件不存在", privateKeyFilePath)
            End If
            Dim privateKeyXml As String = File.ReadAllText(privateKeyFilePath, Encoding.UTF8)
            Return New LicenseGenerator(privateKeyXml)
        End Function

        ''' <summary>
        ''' 生成签名许可证字符串
        ''' </summary>
        Public Function GenerateSignedLicense(licenseData As LicenseData) As String
            ' 序列化为JSON
            Dim jsonSerializer As New System.Web.Script.Serialization.JavaScriptSerializer()
            Dim jsonString As String = jsonSerializer.Serialize(licenseData)

            ' 使用RSA私钥签名
            Dim signature As String = CryptoHelper.SignString(jsonString, _privateKeyXml)

            ' 组合为 Base64(JSON).Base64(Signature) 格式
            Dim jsonBase64 As String = Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonString))
            Return jsonBase64 & "." & signature
        End Function

        ''' <summary>
        ''' 从指纹文件生成许可证
        ''' </summary>
        Public Function GenerateLicenseFromFingerprintFile(fingerprintFilePath As String,
                                                            productName As String,
                                                            productVersion As String,
                                                            customerName As String,
                                                            licenseType As LicenseType,
                                                            expiryDays As Integer) As String
            Dim fingerprint As String = ReadFingerprintFromFile(fingerprintFilePath)
            Return GenerateLicense(fingerprint, productName, productVersion,
                                    customerName, licenseType, expiryDays)
        End Function

        ''' <summary>
        ''' 从硬件指纹哈希直接生成许可证
        ''' </summary>
        Public Function GenerateLicense(hardwareFingerprint As String,
                                         productName As String,
                                         productVersion As String,
                                         customerName As String,
                                         licenseType As LicenseType,
                                         expiryDays As Integer) As String
            Dim licenseData As New LicenseData() With {
                .HardwareFingerprint = hardwareFingerprint,
                .ProductName = productName,
                .ProductVersion = productVersion,
                .CustomerName = customerName,
                .LicenseType = licenseType,
                .IssueDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                .MaxActivationCount = 1
            }

            If expiryDays > 0 Then
                licenseData.ExpiryDate = DateTime.UtcNow.AddDays(expiryDays).ToString("yyyy-MM-ddTHH:mm:ssZ")
            End If

            Return GenerateSignedLicense(licenseData)
        End Function

        ''' <summary>
        ''' 生成许可证并保存到文件
        ''' </summary>
        Public Sub GenerateAndSaveLicense(licenseData As LicenseData, outputPath As String)
            Dim signedLicense As String = GenerateSignedLicense(licenseData)
            File.WriteAllText(outputPath, signedLicense, Encoding.UTF8)
        End Sub

        ''' <summary>
        ''' 从指纹文件中提取硬件指纹哈希
        ''' </summary>
        Private Function ReadFingerprintFromFile(filePath As String) As String
            If Not File.Exists(filePath) Then
                Throw New FileNotFoundException("指纹文件不存在", filePath)
            End If

            Dim content As String = File.ReadAllText(filePath, Encoding.UTF8)

            ' 尝试解析标准格式
            Dim startMarker As String = "硬件指纹哈希 (Hardware Fingerprint Hash):"
            Dim endMarker As String = "----------------------------------------"

            Dim startIndex As Integer = content.IndexOf(startMarker)
            If startIndex >= 0 Then
                startIndex += startMarker.Length
                Dim endIndex As Integer = content.IndexOf(endMarker, startIndex)
                If endIndex >= 0 Then
                    Return content.Substring(startIndex, endIndex - startIndex).Trim()
                End If
            End If

            ' 如果不是标准格式，尝试直接读取
            Dim trimmed As String = content.Trim()
            If trimmed.Length = 64 Then
                Return trimmed
            End If

            Throw New FormatException("无法从文件中解析硬件指纹哈希")
        End Function

    End Class

End Namespace
