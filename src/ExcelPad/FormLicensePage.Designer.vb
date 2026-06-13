Imports System.Windows.Forms
Imports Galaxy.Workbench.LicenseFramework.[Shared]

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormLicensePage
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        components = New System.ComponentModel.Container
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Text = "我的商业软件 v1.0.0"
        Me.Size = New System.Drawing.Size(800, 600)
        Me.StartPosition = FormStartPosition.CenterScreen

        ' 状态栏
        Dim statusPanel As New Panel With {
            .Dock = DockStyle.Bottom,
            .Height = 30,
            .BackColor = System.Drawing.SystemColors.Control
        }

        Dim lblStatus As New Label With {
            .Text = If(Workbench.IsLicensed,
                       $"已授权 - {Workbench.GetCurrentLicense()?.LicenseType.ToString()} - 到期: {Workbench.GetCurrentLicense()?.ExpiryDate}",
                       "未授权"),
            .Dock = DockStyle.Fill,
            .TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        }
        statusPanel.Controls.Add(lblStatus)
        Me.Controls.Add(statusPanel)

        ' 主内容
        Dim mainPanel As New Panel With {
            .Dock = DockStyle.Fill
        }

        Dim lblWelcome As New Label With {
            .Text = "欢迎使用我的商业软件！",
            .Font = New System.Drawing.Font("Microsoft YaHei", 16, System.Drawing.FontStyle.Bold),
            .Location = New System.Drawing.Point(20, 20),
            .AutoSize = True
        }
        mainPanel.Controls.Add(lblWelcome)

        ' 授权信息
        If Workbench.IsLicensed Then
            Dim license As LicenseData = Workbench.GetCurrentLicense
            Dim lblInfo As New Label With {
                .Text = $"许可证类型: {license.LicenseType.ToString()}" & Environment.NewLine &
                        $"客户名称: {license.CustomerName}" & Environment.NewLine &
                        $"授权日期: {license.IssueDate}" & Environment.NewLine &
                        $"到期日期: {If(String.IsNullOrEmpty(license.ExpiryDate), "永久", license.ExpiryDate)}",
                .Location = New System.Drawing.Point(20, 70),
                .AutoSize = True
            }
            mainPanel.Controls.Add(lblInfo)
        End If

        ' 授权管理按钮
        btnLicense = New Button With {
            .Text = "授权管理",
            .Size = New System.Drawing.Size(120, 35),
            .Location = New System.Drawing.Point(20, 200)
        }

        mainPanel.Controls.Add(btnLicense)

        Me.Controls.Add(mainPanel)
    End Sub

    Dim WithEvents btnLicense As New Button
End Class
