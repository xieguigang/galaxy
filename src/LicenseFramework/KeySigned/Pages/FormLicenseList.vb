Imports LicenseVendor.Database

Public Class FormLicenseList

    Private Async Sub FormLicenseList_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim licenses As LicenseUser() = Await Workbench.loadLicenses

        TreeListView1 _
            .AddColumn("用户名", Function(m) m.user_name, width:=300) _
            .AddColumn("机构名称", Function(m) m.organization, width:=200) _
            .AddColumn("许可证数量", Function(m) m.licenses.Length, width:=1000) _
            .AddColumn("过期时间", Function(m) m.expired, width:=1000) _
            .AddColumn("许可证硬件摘要信息", Function(m) m.hardware_checksum, width:=1000)
        TreeListView1 _
            .SetChildrenGetter(Function(m) m.licenses) _
            .SetHasChildrenGetter(Function(m) m.licenses.Length > 0) _
            .SetRoots(licenses)
    End Sub
End Class