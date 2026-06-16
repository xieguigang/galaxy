Imports Oracle.LinuxCompatibility.MySQL
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Uri

Namespace license_svrModel

Public MustInherit Class db_mysql : Inherits IDatabase
Protected ReadOnly m_license As TableModel(Of license)
Protected ReadOnly m_user As TableModel(Of user)
Protected Sub New(mysqli As ConnectionUri)
Call MyBase.New(mysqli)

Me.m_license = model(Of license)()
Me.m_user = model(Of user)()
End Sub
End Class

End Namespace
