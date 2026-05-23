'''
''' ---------------------------------------------------------------
''' 授权对话框 (LicenseDialog.vb) - 客户端UI组件
''' 提供可视化的授权管理界面
''' ---------------------------------------------------------------
'''
Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports LicenseFramework.Shared

Namespace LicenseFramework.Client

    ''' <summary>
    ''' 授权对话框窗体
    ''' </summary>
    Public Class LicenseDialog
        Inherits Form

        Private _offlineProvider As OfflineLicenseProvider
        Private _onlineProvider As OnlineLicenseProvider
        Private _productName As String
        Private _productVersion As String
        Private _initialResult As LicenseValidationResult

        Public Property IsAuthorized As Boolean = False

        Public Sub New(offlineProvider As OfflineLicenseProvider,
                        onlineProvider As OnlineLicenseProvider,
                        productName As String,
                        productVersion As String,
                        initialResult As LicenseValidationResult)
            _offlineProvider = offlineProvider
            _onlineProvider = onlineProvider
            _productName = productName
            _productVersion = productVersion
            _initialResult = initialResult

            InitializeComponent()
        End Sub

        Private Sub InitializeComponent()
            Me.Text = "软件授权管理"
            Me.Size = New Size(520, 420)
            Me.StartPosition = FormStartPosition.CenterParent
            Me.FormBorderStyle = FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False

            Dim mainPanel As New Panel With {
                .Dock = DockStyle.Fill,
                .Padding = New Padding(20)
            }

            ' 状态标签
            Dim lblStatus As New Label With {
                .Text = If(_initialResult?.IsValid, "当前状态: 已授权", "当前状态: 未授权"),
                .Font = New Font("Microsoft YaHei", 12, FontStyle.Bold),
                .ForeColor = If(_initialResult?.IsValid, Color.Green, Color.Red),
                .Location = New Point(20, 20),
                .AutoSize = True
            }
            mainPanel.Controls.Add(lblStatus)

            ' 详细信息
            Dim lblDetail As New Label With {
                .Text = If(_initialResult?.IsValid,
                           $"许可证类型: {_initialResult?.License?.LicenseType.ToString()}" & Environment.NewLine &
                           $"到期时间: {_initialResult?.License?.ExpiryDate}",
                           _initialResult?.Message ?? "需要激活软件许可证"),
                .Location = New Point(20, 60),
                .Size = New Size(460, 50)
            }
            mainPanel.Controls.Add(lblDetail)

            ' 导出指纹按钮
            Dim btnExport As New Button With {
                .Text = "1. 导出硬件指纹文件",
                .Size = New Size(460, 40),
                .Location = New Point(20, 120)
            }
            AddHandler btnExport.Click, AddressOf BtnExport_Click
            mainPanel.Controls.Add(btnExport)

            Dim lblExportHint As New Label With {
                .Text = "将指纹文件发送给软件供应商，获取许可证文件",
                .ForeColor = Color.Gray,
                .Location = New Point(20, 162),
                .AutoSize = True
            }
            mainPanel.Controls.Add(lblExportHint)

            ' 导入许可证按钮
            Dim btnImport As New Button With {
                .Text = "2. 导入许可证文件（离线激活）",
                .Size = New Size(460, 40),
                .Location = New Point(20, 190)
            }
            AddHandler btnImport.Click, AddressOf BtnImport_Click
            mainPanel.Controls.Add(btnImport)

            ' 在线激活按钮
            Dim btnOnline As New Button With {
                .Text = "3. 在线激活",
                .Size = New Size(460, 40),
                .Location = New Point(20, 250),
                .Enabled = (_onlineProvider IsNot Nothing)
            }
            AddHandler btnOnline.Click, AddressOf BtnOnline_Click
            mainPanel.Controls.Add(btnOnline)

            ' 关闭按钮
            Dim btnClose As New Button With {
                .Text = "关闭",
                .Size = New Size(100, 35),
                .Location = New Point(380, 320)
            }
            AddHandler btnClose.Click, Sub() Me.Close()
            mainPanel.Controls.Add(btnClose)

            Me.Controls.Add(mainPanel)
        End Sub

        Private Sub BtnExport_Click(sender As Object, e As EventArgs)
            _offlineProvider.ExportFingerprintWithDialog(_productName, _productVersion)
        End Sub

        Private Sub BtnImport_Click(sender As Object, e As EventArgs)
            Dim result As LicenseValidationResult = _offlineProvider.ImportLicenseWithDialog()

            If result.IsValid Then
                IsAuthorized = True
                MessageBox.Show("许可证导入成功，软件已授权!", "授权成功",
                                MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.Close()
            Else
                MessageBox.Show($"许可证导入失败:" & Environment.NewLine & result.Message,
                                "导入失败", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        End Sub

        Private Sub BtnOnline_Click(sender As Object, e As EventArgs)
            If _onlineProvider Is Nothing Then
                MessageBox.Show("未配置在线授权服务", "提示",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Try
                Me.Cursor = Cursors.WaitCursor
                Dim result As LicenseValidationResult = _onlineProvider.RequestOnlineLicense(
                    _productName, _productVersion)

                If result.IsValid Then
                    IsAuthorized = True
                    Me.Cursor = Cursors.Default
                    MessageBox.Show("在线激活成功，软件已授权!",
                                    "授权成功", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Me.Close()
                Else
                    Me.Cursor = Cursors.Default
                    MessageBox.Show($"在线激活失败:" & Environment.NewLine & result.Message,
                                    "激活失败", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If
            Catch ex As Exception
                Me.Cursor = Cursors.Default
                MessageBox.Show($"在线激活异常: {ex.Message}",
                                "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

    End Class

End Namespace
