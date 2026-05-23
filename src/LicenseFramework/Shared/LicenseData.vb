'''
''' ---------------------------------------------------------------
''' 许可证数据模型 (LicenseData.vb)
''' ---------------------------------------------------------------
'''
Imports System

Namespace LicenseFramework.Shared

    Public Enum LicenseType As Integer
        Trial = 0
        Standard = 1
        Professional = 2
        Enterprise = 3
    End Enum

    Public Enum LicenseStatus As Integer
        Valid = 0
        Expired = 1
        HardwareMismatch = 2
        InvalidSignature = 3
        NotFound = 4
        Malformed = 5
        OnlineVerificationFailed = 6
        UnknownError = 7
    End Enum

    ''' <summary>
    ''' 许可证数据实体类
    ''' </summary>
    <Serializable>
    Public Class LicenseData
        Public Property LicenseId As String = Guid.NewGuid().ToString("N")
        Public Property HardwareFingerprint As String = String.Empty
        Public Property ProductName As String = String.Empty
        Public Property ProductVersion As String = String.Empty
        Public Property CustomerName As String = String.Empty
        Public Property LicenseType As LicenseType = LicenseType.Standard
        Public Property IssueDate As String = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
        Public Property ExpiryDate As String = String.Empty
        Public Property MaxActivationCount As Integer = 1
        Public Property Features As String = String.Empty
        Public Property Remarks As String = String.Empty

        Public ReadOnly Property IsExpired As Boolean
            Get
                If String.IsNullOrEmpty(ExpiryDate) Then Return False
                Dim expiry As DateTime
                If DateTime.TryParse(ExpiryDate, expiry) Then
                    Return DateTime.UtcNow > expiry
                End If
                Return False
            End Get
        End Property

        Public ReadOnly Property IsValid As Boolean
            Get
                Return Not String.IsNullOrEmpty(HardwareFingerprint) AndAlso
                       Not String.IsNullOrEmpty(ProductName) AndAlso
                       Not IsExpired
            End Get
        End Property
    End Class

    ''' <summary>
    ''' 许可证验证结果
    ''' </summary>
    Public Class LicenseValidationResult
        Public Property Status As LicenseStatus = LicenseStatus.UnknownError
        Public Property License As LicenseData = Nothing
        Public Property Message As String = String.Empty

        Public ReadOnly Property IsValid As Boolean
            Get
                Return Status = LicenseStatus.Valid AndAlso License IsNot Nothing
            End Get
        End Property

        Public Shared Function Success(license As LicenseData) As LicenseValidationResult
            Return New LicenseValidationResult With {
                .Status = LicenseStatus.Valid,
                .License = license,
                .Message = "许可证验证通过"
            }
        End Function

        Public Shared Function Fail(status As LicenseStatus, message As String) As LicenseValidationResult
            Return New LicenseValidationResult With {
                .Status = status,
                .License = Nothing,
                .Message = message
            }
        End Function
    End Class

    ''' <summary>
    ''' 在线授权请求结构
    ''' </summary>
    Public Class OnlineLicenseRequest
        Public Property HardwareFingerprint As String = String.Empty
        Public Property ProductName As String = String.Empty
        Public Property ProductVersion As String = String.Empty
        Public Property RequestTimestamp As String = String.Empty
        Public Property RequestSignature As String = String.Empty
    End Class

    ''' <summary>
    ''' 在线授权响应结构
    ''' </summary>
    Public Class OnlineLicenseResponse
        Public Property StatusCode As Integer = 0
        Public Property StatusMessage As String = String.Empty
        Public Property SignedLicense As String = String.Empty
        Public Property ServerTimestamp As String = String.Empty
    End Class

End Namespace
