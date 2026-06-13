Imports System.Drawing.Drawing2D
Imports LicenseVendor.Database

Public Class FormLicenseList

    Private Async Sub FormLicenseList_Load(sender As Object, e As EventArgs) Handles Me.Load
        TreeListView1 _
            .AddColumn("用户名", Function(m) m.user_name, width:=200) _
            .AddColumn("软件名称", Function(m) m.software_name, width:=200) _
            .AddColumn("许可证数量", Function(m) m.licenses.TryCount, width:=100) _
            .AddColumn("过期时间", Function(m) m.expired, width:=200) _
            .AddColumn("许可证硬件摘要信息", Function(m)
                                        Return m.hardware_checksum
                                    End Function, width:=800)

        TreeListView1.SetIconGetter(AddressOf GetIcon)

        Await LoadLicenseList()
    End Sub

    Public Async Function LoadLicenseList() As Task
        Dim licenses As LicenseUser() = Await Workbench.loadLicenses

        TreeListView1 _
            .SetChildrenGetter(Function(m) m.licenses) _
            .SetHasChildrenGetter(Function(m) m.licenses.TryCount > 0) _
            .SetRoots(licenses)
    End Function

    ' ==================== ICON DRAWING ====================

    ' ==================== ICONS (drawn in code, no resources needed) ====================

    Private Shared ReadOnly Icons As Dictionary(Of String, Image) = New Dictionary(Of String, Image)() From {
        {"license", CreateFolderIcon(Color.FromArgb(244, 180, 76))},
        {"license_old", CreateTeamIcon(Color.FromArgb(32, 162, 152))},
        {"user", CreatePersonIcon(Color.FromArgb(96, 116, 145))},
        {"license_xxx", CreatePersonIcon(Color.FromArgb(173, 126, 188))}
    }

    Private Function GetIcon(m As LicenseUser) As Image
        If Icons.ContainsKey(m.type) Then
            Return Icons(m.type)
        Else
            Return Nothing
        End If
    End Function

    Private Shared Function CreateFolderIcon(color As Color) As Image
        Dim bmp = New Bitmap(16, 16)
        Dim g = Graphics.FromImage(bmp)
        g.SmoothingMode = SmoothingMode.AntiAlias
        Dim brush = New SolidBrush(color)
        Dim darker = New SolidBrush(ControlPaint.Dark(color, 0.1F))
        g.FillRectangle(darker, 1, 3, 7, 4)
        g.FillRectangle(brush, 1, 5, 14, 9)
        Return bmp
    End Function

    Private Shared Function CreateTeamIcon(color As Color) As Image
        Dim bmp = New Bitmap(16, 16)
        Dim g = Graphics.FromImage(bmp)
        g.SmoothingMode = SmoothingMode.AntiAlias
        Dim back = New SolidBrush(ControlPaint.Light(color, 0.6F))
        Dim front = New SolidBrush(color)
        g.FillEllipse(back, 8, 2, 6, 6)
        g.FillEllipse(back, 7, 9, 8, 6)
        g.FillEllipse(front, 2, 3, 7, 7)
        g.FillEllipse(front, 1, 10, 9, 6)
        Return bmp
    End Function

    Private Shared Function CreatePersonIcon(color As Color) As Image
        Dim bmp = New Bitmap(16, 16)
        Dim g = Graphics.FromImage(bmp)
        g.SmoothingMode = SmoothingMode.AntiAlias
        Dim brush = New SolidBrush(color)
        g.FillEllipse(brush, 5, 1, 6, 6)
        g.FillEllipse(brush, 2, 9, 12, 9)
        Return bmp
    End Function
End Class