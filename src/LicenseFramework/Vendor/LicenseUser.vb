Namespace Database

    Public Class SoftwareLicense

        Public Property user_name As String
        Public Property organization As String
        Public Property expired As Date
        Public Property hardware_checksum As String

    End Class

    Public Class LicenseUser

        Public Property user_name As String
        Public Property software_name As String
        Public Property expired As Date
        Public Property hardware_checksum As String
        Public Property type As String = "user|license"
        Public Property licenses As LicenseUser()

    End Class

End Namespace