Imports LicenseVendor.Database

Public Class FormLicenseList

    Private Async Sub FormLicenseList_Load(sender As Object, e As EventArgs) Handles Me.Load
        TreeListView1 _
            .AddColumn("用户名", Function(m) m.user_name, width:=200) _
            .AddColumn("软件名称", Function(m) m.software_name, width:=200) _
            .AddColumn("许可证数量", Function(m) m.licenses.TryCount, width:=100) _
            .AddColumn("过期时间", Function(m) m.expired, width:=200) _
            .AddColumn("许可证硬件摘要信息", Function(m) m.hardware_checksum, width:=800)

        Await LoadLicenseList()
    End Sub

    Public Async Function LoadLicenseList() As Task
        Dim licenses As LicenseUser() = Await Workbench.loadLicenses

        TreeListView1 _
            .SetChildrenGetter(Function(m) m.licenses) _
            .SetHasChildrenGetter(Function(m) m.licenses.TryCount > 0) _
            .SetRoots(licenses)
    End Function
End Class