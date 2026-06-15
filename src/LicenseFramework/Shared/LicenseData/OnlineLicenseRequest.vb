Namespace LicenseFramework.Shared

    ''' <summary>
    ''' 在线授权请求结构
    ''' </summary>
    Public Class OnlineLicenseRequest
        Public Property HardwareFingerprint As String = String.Empty
        Public Property ProductName As String = String.Empty
        Public Property ProductVersion As String = String.Empty
        Public Property RequestTimestamp As String = String.Empty
        Public Property RequestSignature As String = String.Empty
        Public Property User As String
        Public Property Password As String
    End Class
End Namespace