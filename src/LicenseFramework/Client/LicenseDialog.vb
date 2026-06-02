Imports System.Drawing
Imports System.Windows.Forms
Imports LicenseVendor.LicenseFramework.Shared

Namespace LicenseFramework.Client

    ''' <summary>
    ''' 授权对话框窗体
    ''' </summary>
    '''
    ''' ---------------------------------------------------------------
    ''' 授权对话框 (LicenseDialog.vb) - 客户端UI组件
    ''' 提供可视化的授权管理界面
    ''' ---------------------------------------------------------------
    '''
    Public Class LicenseDialog
        Inherits Form

        Private _offlineProvider As OfflineLicenseProvider
        Private _onlineProvider As OnlineLicenseProvider
        Private _productName As String
        Private _productVersion As String
        Private _initialResult As LicenseValidationResult

        Public Property IsAuthorized As Boolean = False

        Public Sub New()
            InitializeComponent()
            Call UpdateUI()
        End Sub

        Public Sub SetLicenseData(offlineProvider As OfflineLicenseProvider,
                        onlineProvider As OnlineLicenseProvider,
                        productName As String,
                        productVersion As String,
                        initialResult As LicenseValidationResult)

            _offlineProvider = offlineProvider
            _onlineProvider = onlineProvider
            _productName = productName
            _productVersion = productVersion
            _initialResult = initialResult

            Call UpdateUI()
        End Sub

        Private Sub UpdateUI()
            lblStatus.Text = If(_initialResult?.IsValid, "当前状态: 已授权", "当前状态: 未授权")
            lblDetail.Text = If(_initialResult?.IsValid,
                           $"许可证类型: {_initialResult?.License?.LicenseType.ToString()}" & Environment.NewLine &
                           $"到期时间: {_initialResult?.License?.ExpiryDate}",
                           If(_initialResult?.Message, "需要激活软件许可证"))

        End Sub

        Dim WithEvents lblStatus As Label
        Dim WithEvents lblDetail As New Label
        Dim WithEvents btnExport As New Button
        Dim WithEvents lblExportHint As New Label
        Dim WithEvents btnImport As New Button
        Dim WithEvents btnOnline As New Button
        Dim WithEvents btnClose As New Button

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
            lblStatus = New Label With {
                .Text = "当前状态: 未授权",
                .Font = New Font("Microsoft YaHei", 12, FontStyle.Bold),
                .ForeColor = If(_initialResult?.IsValid, Color.Green, Color.Red),
                .Location = New Point(20, 20),
                .AutoSize = True
            }
            mainPanel.Controls.Add(lblStatus)

            ' 详细信息
            lblDetail = New Label With {
                .Text = "需要激活软件许可证",
                .Location = New Point(20, 60),
                .Size = New Size(460, 50)
            }
            mainPanel.Controls.Add(lblDetail)

            ' 导出指纹按钮
            btnExport = New Button With {
                .Text = "1. 导出硬件指纹文件",
                .Size = New Size(460, 40),
                .Location = New Point(20, 120)
            }

            mainPanel.Controls.Add(btnExport)

            lblExportHint = New Label With {
                .Text = "将指纹文件发送给软件供应商，获取许可证文件",
                .ForeColor = Color.Gray,
                .Location = New Point(20, 162),
                .AutoSize = True
            }
            mainPanel.Controls.Add(lblExportHint)

            ' 导入许可证按钮
            btnImport = New Button With {
                .Text = "2. 导入许可证文件（离线激活）",
                .Size = New Size(460, 40),
                .Location = New Point(20, 190)
            }

            mainPanel.Controls.Add(btnImport)

            ' 在线激活按钮
            btnOnline = New Button With {
                .Text = "3. 在线激活",
                .Size = New Size(460, 40),
                .Location = New Point(20, 250),
                .Enabled = (_onlineProvider IsNot Nothing)
            }

            mainPanel.Controls.Add(btnOnline)

            ' 关闭按钮
            btnClose = New Button With {
                .Text = "关闭",
                .Size = New Size(100, 35),
                .Location = New Point(380, 320)
            }

            mainPanel.Controls.Add(btnClose)

            Me.Controls.Add(mainPanel)
        End Sub

        Private Sub BtnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
            _offlineProvider.ExportFingerprintWithDialog(_productName, _productVersion)
        End Sub

        Private Sub BtnImport_Click(sender As Object, e As EventArgs) Handles btnImport.Click
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

        Private Sub BtnOnline_Click(sender As Object, e As EventArgs) Handles btnOnline.Click
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

        Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
            Me.Close()
        End Sub
    End Class

End Namespace
