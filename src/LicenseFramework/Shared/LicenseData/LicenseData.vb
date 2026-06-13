Namespace LicenseFramework.Shared

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

End Namespace
