Namespace LicenseFramework.Shared

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

End Namespace