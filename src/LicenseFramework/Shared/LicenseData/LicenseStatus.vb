Namespace LicenseFramework.Shared

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
End Namespace