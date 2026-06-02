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
        Friend WithEvents mainPanel As Panel
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
            mainPanel = New Panel()
            lblStatus = New Label()
            lblDetail = New Label()
            btnExport = New Button()
            lblExportHint = New Label()
            btnImport = New Button()
            btnOnline = New Button()
            btnClose = New Button()
            mainPanel.SuspendLayout()
            SuspendLayout()
            ' 
            ' mainPanel
            ' 
            mainPanel.Controls.Add(lblStatus)
            mainPanel.Controls.Add(lblDetail)
            mainPanel.Controls.Add(btnExport)
            mainPanel.Controls.Add(lblExportHint)
            mainPanel.Controls.Add(btnImport)
            mainPanel.Controls.Add(btnOnline)
            mainPanel.Controls.Add(btnClose)
            mainPanel.Dock = DockStyle.Fill
            mainPanel.Location = New Point(0, 0)
            mainPanel.Name = "mainPanel"
            mainPanel.Size = New Size(504, 381)
            mainPanel.TabIndex = 0
            ' 
            ' lblStatus
            ' 
            lblStatus.Location = New Point(147, 83)
            lblStatus.Name = "lblStatus"
            lblStatus.Text = "当前状态: 未授权"
            lblStatus.Size = New Size(100, 23)
            lblStatus.TabIndex = 0
            ' 
            ' lblDetail
            ' 
            lblDetail.Location = New Point(67, 185)
            lblDetail.Name = "lblDetail"
            lblDetail.Text = "需要激活软件许可证"
            lblDetail.Size = New Size(100, 23)
            lblDetail.TabIndex = 1
            ' 
            ' btnExport
            ' 
            btnExport.Location = New Point(220, 238)
            btnExport.Name = "btnExport"
            btnExport.Text = "1. 导出硬件指纹文件"
            btnExport.Size = New Size(75, 23)
            btnExport.TabIndex = 2
            ' 
            ' lblExportHint
            ' 
            lblExportHint.Location = New Point(341, 278)
            lblExportHint.Name = "lblExportHint"
            lblExportHint.Text = "将指纹文件发送给软件供应商，获取许可证文件"
            lblExportHint.Size = New Size(100, 23)
            lblExportHint.TabIndex = 3
            ' 
            ' btnImport
            ' 
            btnImport.Location = New Point(147, 325)
            btnImport.Name = "btnImport"
            btnImport.Text = "2. 导入许可证文件（离线激活）"
            btnImport.Size = New Size(75, 23)
            btnImport.TabIndex = 4
            ' 
            ' btnOnline
            ' 
            btnOnline.Location = New Point(232, 44)
            btnOnline.Name = "btnOnline"
            btnOnline.Text = "3. 在线激活"
            btnOnline.Size = New Size(75, 23)
            btnOnline.TabIndex = 5
            ' 
            ' btnClose
            ' 
            btnClose.Location = New Point(67, 56)
            btnClose.Name = "btnClose"
            btnClose.Text = "关闭"
            btnClose.Size = New Size(75, 23)
            btnClose.TabIndex = 6
            ' 
            ' LicenseDialog
            ' 
            ClientSize = New Size(504, 381)
            Controls.Add(mainPanel)
            FormBorderStyle = FormBorderStyle.FixedDialog
            MaximizeBox = False
            MinimizeBox = False
            Name = "LicenseDialog"
            StartPosition = FormStartPosition.CenterParent
            Text = "软件授权管理"
            mainPanel.ResumeLayout(False)
            ResumeLayout(False)
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
