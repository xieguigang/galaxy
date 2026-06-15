Imports System.IO
Imports System.Net
Imports System.Text
Imports Galaxy.Workbench.LicenseFramework.Shared
Imports Microsoft.VisualBasic.Serialization.JSON

Namespace LicenseFramework.Client

    ''' <summary>
    ''' 在线授权提供程序
    ''' 
    ''' 在线授权流程：
    ''' 1. 采集硬件信息 → 生成硬件指纹哈希
    ''' 2. 构建在线授权请求（含HMAC签名）
    ''' 3. HTTP POST发送到授权服务器
    ''' 4. 服务器验证并返回签名许可证
    ''' 5. 客户端验证返回的许可证
    ''' 6. 验证通过后缓存许可证
    ''' </summary>
    '''
    ''' ---------------------------------------------------------------
    ''' 在线授权提供程序 (OnlineLicenseProvider.vb) - 客户端
    ''' 处理在线授权：发送硬件指纹、接收许可证、缓存管理
    ''' ---------------------------------------------------------------
    '''
    Public Class OnlineLicenseProvider

        Private _hwCollector As New HardwareInfoCollector()
        Private _fpGenerator As New FingerprintGenerator()
        Private _validator As LicenseValidator
        Private _serverUrl As String
        Private _hmacKey As String
        Private _timeoutMs As Integer
        Private _offlineProvider As OfflineLicenseProvider

        Public Sub New(publicKeyXml As String, serverUrl As String, hmacKey As String,
                       offlineProvider As OfflineLicenseProvider,
                       Optional timeoutMs As Integer = 15000)

            _validator = New LicenseValidator(publicKeyXml)
            _serverUrl = serverUrl
            _hmacKey = hmacKey
            _timeoutMs = timeoutMs
            _offlineProvider = offlineProvider
        End Sub

        ''' <summary>
        ''' 请求在线授权
        ''' </summary>
        Public Function RequestOnlineLicense(productName As String, productVersion As String, userName As String) As LicenseValidationResult
            Try
                ' 第一步：采集硬件指纹
                Dim hwInfo As HardwareInfo = _hwCollector.Collect()
                Dim fingerprint As String = _fpGenerator.GenerateFingerprint(hwInfo)

                ' 第二步：构建请求
                Dim request As New OnlineLicenseRequest() With {
                    .HardwareFingerprint = fingerprint,
                    .ProductName = productName,
                    .ProductVersion = productVersion,
                    .RequestTimestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                    .User = userName
                }

                ' 第三步：计算HMAC签名
                Dim signData As String = $"{request.HardwareFingerprint}|{request.ProductName}|{request.ProductVersion}|{request.RequestTimestamp}"
                request.RequestSignature = CryptoHelper.ComputeHmac(signData, _hmacKey)

                ' 第四步：序列化请求为JSON
                Dim jsonPayload As String = request.GetJson

                ' 第五步：发送HTTP POST请求
                Dim responseJson As String = SendHttpPostRequest(_serverUrl, jsonPayload)

                ' 第六步：解析响应
                Dim response As OnlineLicenseResponse = responseJson.LoadJSON(Of OnlineLicenseResponse)

                If response Is Nothing OrElse response.StatusCode <> 0 Then
                    Dim errMsg As String = If(response?.StatusMessage, "服务器返回无效响应")
                    Return LicenseValidationResult.Fail(LicenseStatus.OnlineVerificationFailed, errMsg)
                End If

                ' 第七步：验证返回的许可证
                Dim result As LicenseValidationResult = _validator.Validate(response.SignedLicense, fingerprint)

                If result.IsValid Then
                    ' 验证通过，缓存许可证
                    If _offlineProvider IsNot Nothing Then
                        _offlineProvider.CacheLicenseString(response.SignedLicense)
                    End If
                End If

                Return result

            Catch ex As WebException
                Return LicenseValidationResult.Fail(LicenseStatus.OnlineVerificationFailed,
                                                    $"无法连接到授权服务器: {ex.Message}")
            Catch ex As Exception
                Return LicenseValidationResult.Fail(LicenseStatus.OnlineVerificationFailed, ex.Message)
            End Try
        End Function

        ''' <summary>
        ''' 发送HTTP POST请求
        ''' </summary>
        Private Function SendHttpPostRequest(url As String, jsonPayload As String) As String
            Dim request As HttpWebRequest = CType(WebRequest.Create(url), HttpWebRequest)
            request.Method = "POST"
            request.ContentType = "application/json"
            request.Timeout = _timeoutMs
            request.ReadWriteTimeout = _timeoutMs

            ' 写入请求体
            Dim payloadBytes As Byte() = Encoding.UTF8.GetBytes(jsonPayload)
            request.ContentLength = payloadBytes.Length

            Using requestStream As Stream = request.GetRequestStream()
                requestStream.Write(payloadBytes, 0, payloadBytes.Length)
            End Using

            ' 读取响应
            Using response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
                Using responseStream As Stream = response.GetResponseStream()
                    Using reader As New StreamReader(responseStream, Encoding.UTF8)
                        Return reader.ReadToEnd()
                    End Using
                End Using
            End Using
        End Function

        ''' <summary>
        ''' 异步请求在线授权
        ''' </summary>
        Public Async Function RequestOnlineLicenseAsync(productName As String, productVersion As String, userName As String) As Task(Of LicenseValidationResult)
            Return Await Task.Factory.StartNew(Function() RequestOnlineLicense(productName, productVersion, userName))
        End Function

    End Class

End Namespace
