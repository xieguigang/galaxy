Namespace Database

    Public Class LicenseTable

        Public Property user_name As String
        Public Property expired As Date
        Public Property hardware_checksum As String

    End Class

    Public Class LicenseUser

        Public Property user_name As String
        Public Property licenses As SoftwareLicense()

    End Class

    Public Class SoftwareLicense

        Public Property expired As Date
        Public Property hardware_checksum As String

    End Class
End Namespace