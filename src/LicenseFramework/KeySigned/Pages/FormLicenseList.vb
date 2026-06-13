Imports LicenseVendor

Public Class FormLicenseList

    Private Async Sub FormLicenseList_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim licenses As LicenseUser() = Await Workbench.loadLicenses

        TreeListView1 _
            .AddColumn("User", Function(m) m.user_name, width:=300) _
            .AddColumn("Expired Date", Function(m) m.expired, width:=200) _
            .AddColumn("Fingerprint", Function(m) m.hardware_checksum, width:=1000)
    End Sub
End Class