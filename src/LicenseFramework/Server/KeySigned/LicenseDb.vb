Imports System.Runtime.CompilerServices
Imports KeySigned.license_svrModel
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Uri

Public Class LicenseDb : Inherits db_mysql

    Public ReadOnly Property license As TableModel(Of license)
        Get
            Return m_license
        End Get
    End Property

    Public ReadOnly Property user As TableModel(Of user)
        Get
            Return m_user
        End Get
    End Property

    Public Sub New(mysqli As ConnectionUri)
        MyBase.New(mysqli)
    End Sub

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function GetUser(user As String, password As String) As license_svrModel.user
        Return Me.user _
            .where(field("email") = user,
                   field("passwd") = password) _
            .find(Of user)
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function CheckLicense(user As user, hashcode As String) As license
        Return Me.license _
            .where(field("user_id") = user.id,
                   field("fingerprint") = hashcode) _
            .find(Of license)
    End Function
End Class
