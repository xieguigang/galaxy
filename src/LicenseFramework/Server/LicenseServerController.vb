'''
''' ---------------------------------------------------------------
''' 在线授权服务器API控制器 (LicenseServerController.vb)
''' ASP.NET Web API 控制器，处理客户端的在线授权请求
''' ---------------------------------------------------------------
'''
Imports System
Imports System.Web.Http
Imports System.Text
Imports LicenseFramework.Shared

Namespace LicenseFramework.Server

    ''' <summary>
    ''' 在线授权服务器API控制器
    ''' 
    ''' API端点: POST /api/license/activate
    ''' 
    ''' 安全措施：
    ''' 1. HMAC-SHA256请求签名验证
    ''' 2. 时间戳验证（5分钟有效期）
    ''' 3. 请求频率限制
    ''' 4. HTTPS传输（生产环境必须）
    ''' </summary>
    Public Class LicenseServerController
        Inherits ApiController

        Private _privateKeyXml As String
        Private _hmacKey As String
        Private Const TIMESTAMP_TOLERANCE_MINUTES As Integer = 5

        ''' <summary>
        ''' 构造函数
        ''' </summary>
        Public Sub New(privateKeyXml As String, hmacKey As String)
            _privateKeyXml = privateKeyXml
            _hmacKey = hmacKey
        End Function

        ''' <summary>
        ''' 处理在线激活请求
        ''' POST /api/license/activate
        ''' </summary>
        <HttpPost>
        <Route("api/license/activate")>
        Public Function Activate(request As OnlineLicenseRequest) As IHttpActionResult
            Try
                ' 第一步：验证请求签名
                If Not VerifyRequest(request) Then
                    Return BadRequest(New OnlineLicenseResponse With {
                        .StatusCode = -1,
                        .StatusMessage = "请求签名验证失败"
                    })
                End If

                ' 第二步：验证时间戳（防重放攻击）
                If Not VerifyTimestamp(request.RequestTimestamp) Then
                    Return BadRequest(New OnlineLicenseResponse With {
                        .StatusCode = -2,
                        .StatusMessage = "请求已过期"
                    })
                End If

                ' 第三步：查询授权数据库，获取该指纹对应的授权信息
                Dim dbRecord As LicenseDatabaseRecord = QueryLicenseDatabase(request.HardwareFingerprint)

                If dbRecord Is Nothing Then
                    Return BadRequest(New OnlineLicenseResponse With {
                        .StatusCode = -3,
                        .StatusMessage = "未找到授权记录，请联系供应商"
                    })
                End If

                ' 第四步：生成许可证
                Dim generator As New LicenseGenerator(_privateKeyXml)
                Dim licenseData As New LicenseData With {
                    .HardwareFingerprint = request.HardwareFingerprint,
                    .ProductName = request.ProductName,
                    .ProductVersion = request.ProductVersion,
                    .CustomerName = dbRecord.CustomerName,
                    .LicenseType = dbRecord.LicenseType,
                    .IssueDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
                }

                If dbRecord.RemainingDays > 0 Then
                    licenseData.ExpiryDate = DateTime.UtcNow.AddDays(dbRecord.RemainingDays).ToString("yyyy-MM-ddTHH:mm:ssZ")
                End If

                Dim signedLicense As String = generator.GenerateSignedLicense(licenseData)

                ' 第五步：更新激活计数
                UpdateActivationRecord(request.HardwareFingerprint)

                ' 第六步：记录日志
                LogActivation(request.HardwareFingerprint, request.ProductName, True)

                ' 返回成功响应
                Return Ok(New OnlineLicenseResponse With {
                    .StatusCode = 0,
                    .StatusMessage = "OK",
                    .SignedLicense = signedLicense,
                    .ServerTimestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
                })

            Catch ex As Exception
                LogActivation(request?.HardwareFingerprint, request?.ProductName, False)
                Return InternalServerError(New OnlineLicenseResponse With {
                    .StatusCode = -99,
                    .StatusMessage = $"服务器内部错误: {ex.Message}"
                })
            End Try
        End Function

        ''' <summary>
        ''' 验证请求的HMAC签名
        ''' </summary>
        Private Function VerifyRequest(request As OnlineLicenseRequest) As Boolean
            Try
                Dim signData As String = $"{request.HardwareFingerprint}|{request.ProductName}|{request.ProductVersion}|{request.RequestTimestamp}"
                Return CryptoHelper.VerifyHmac(signData, _hmacKey, request.RequestSignature)
            Catch
                Return False
            End Try
        End Function

        ''' <summary>
        ''' 验证请求时间戳是否在有效期内
        ''' </summary>
        Private Function VerifyTimestamp(timestampStr As String) As Boolean
            Try
                Dim requestTime As DateTime = DateTime.Parse(timestampStr)
                Dim diff As TimeSpan = DateTime.UtcNow - requestTime
                Return Math.Abs(diff.TotalMinutes) <= TIMESTAMP_TOLERANCE_MINUTES
            Catch
                Return False
            End Try
        End Function

        ''' <summary>
        ''' 查询授权数据库（示例实现，实际应连接真实数据库）
        ''' </summary>
        Private Function QueryLicenseDatabase(fingerprint As String) As LicenseDatabaseRecord
            ' TODO: 替换为真实的数据库查询逻辑
            ' 示例：返回一个默认的标准版授权
            Return New LicenseDatabaseRecord With {
                .CustomerName = "授权用户",
                .LicenseType = LicenseType.Standard,
                .RemainingDays = 365,
                .IsActivated = True
            }
        End Function

        ''' <summary>
        ''' 更新激活记录
        ''' </summary>
        Private Sub UpdateActivationRecord(fingerprint As String)
            ' TODO: 更新数据库中的激活计数和最后激活时间
        End Sub

        ''' <summary>
        ''' 记录激活日志
        ''' </summary>
        Private Sub LogActivation(fingerprint As String, productName As String, success As Boolean)
            Dim logEntry As String = $"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] " &
                                     $"Fingerprint={fingerprint}, Product={productName}, Success={success}"
            Diagnostics.Debug.WriteLine(logEntry)
        End Sub

        Private Function BadRequest(response As OnlineLicenseResponse) As IHttpActionResult
            Return Content(System.Net.HttpStatusCode.BadRequest, response)
        End Function

        Private Function InternalServerError(response As OnlineLicenseResponse) As IHttpActionResult
            Return Content(System.Net.HttpStatusCode.InternalServerError, response)
        End Function

    End Class

    ''' <summary>
    ''' 授权数据库记录结构（示例）
    ''' </summary>
    Public Class LicenseDatabaseRecord
        Public Property CustomerName As String = String.Empty
        Public Property LicenseType As LicenseType = LicenseType.Standard
        Public Property RemainingDays As Integer = 365
        Public Property IsActivated As Boolean = False
        Public Property ActivationCount As Integer = 0
        Public Property LastActivationTime As DateTime = DateTime.MinValue
    End Class

End Namespace
