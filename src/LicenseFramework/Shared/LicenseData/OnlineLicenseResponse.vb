Namespace LicenseFramework.Shared

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