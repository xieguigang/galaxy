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
        Friend WithEvents GroupBox1 As GroupBox
        Friend WithEvents GroupBox2 As GroupBox
        Friend WithEvents Label1 As Label
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
            lblStatus = New Label()
            lblDetail = New Label()
            btnExport = New Button()
            lblExportHint = New Label()
            btnImport = New Button()
            btnOnline = New Button()
            btnClose = New Button()
            GroupBox1 = New GroupBox()
            GroupBox2 = New GroupBox()
            Label1 = New Label()
            GroupBox1.SuspendLayout()
            GroupBox2.SuspendLayout()
            SuspendLayout()
            ' 
            ' lblStatus
            ' 
            lblStatus.Location = New Point(18, 33)
            lblStatus.Name = "lblStatus"
            lblStatus.Size = New Size(441, 40)
            lblStatus.TabIndex = 0
            lblStatus.Text = "当前状态: 未授权"
            ' 
            ' lblDetail
            ' 
            lblDetail.Location = New Point(18, 85)
            lblDetail.Name = "lblDetail"
            lblDetail.Size = New Size(441, 49)
            lblDetail.TabIndex = 1
            lblDetail.Text = "需要激活软件许可证"
            ' 
            ' btnExport
            ' 
            btnExport.Location = New Point(24, 35)
            btnExport.Name = "btnExport"
            btnExport.Size = New Size(215, 32)
            btnExport.TabIndex = 2
            btnExport.Text = "1. 导出硬件指纹文件"
            ' 
            ' lblExportHint
            ' 
            lblExportHint.Location = New Point(261, 35)
            lblExportHint.Name = "lblExportHint"
            lblExportHint.Size = New Size(164, 40)
            lblExportHint.TabIndex = 3
            lblExportHint.Text = "将指纹文件发送给软件供应商，获取许可证文件"
            ' 
            ' btnImport
            ' 
            btnImport.Location = New Point(24, 90)
            btnImport.Name = "btnImport"
            btnImport.Size = New Size(215, 34)
            btnImport.TabIndex = 4
            btnImport.Text = "2. 导入许可证文件（离线激活）"
            ' 
            ' btnOnline
            ' 
            btnOnline.Location = New Point(42, 325)
            btnOnline.Name = "btnOnline"
            btnOnline.Size = New Size(215, 36)
            btnOnline.TabIndex = 5
            btnOnline.Text = "3. 在线激活"
            ' 
            ' btnClose
            ' 
            btnClose.Location = New Point(380, 392)
            btnClose.Name = "btnClose"
            btnClose.Size = New Size(91, 31)
            btnClose.TabIndex = 6
            btnClose.Text = "关闭"
            ' 
            ' GroupBox1
            ' 
            GroupBox1.Controls.Add(lblExportHint)
            GroupBox1.Controls.Add(btnExport)
            GroupBox1.Controls.Add(btnImport)
            GroupBox1.Location = New Point(18, 150)
            GroupBox1.Name = "GroupBox1"
            GroupBox1.Size = New Size(441, 143)
            GroupBox1.TabIndex = 1
            GroupBox1.TabStop = False
            GroupBox1.Text = "离线激活"
            ' 
            ' GroupBox2
            ' 
            GroupBox2.Controls.Add(Label1)
            GroupBox2.Controls.Add(lblStatus)
            GroupBox2.Controls.Add(btnOnline)
            GroupBox2.Controls.Add(GroupBox1)
            GroupBox2.Controls.Add(lblDetail)
            GroupBox2.Location = New Point(12, 12)
            GroupBox2.Name = "GroupBox2"
            GroupBox2.Size = New Size(469, 370)
            GroupBox2.TabIndex = 7
            GroupBox2.TabStop = False
            GroupBox2.Text = "软件授权激活"
            ' 
            ' Label1
            ' 
            Label1.AutoSize = True
            Label1.Location = New Point(42, 302)
            Label1.Name = "Label1"
            Label1.Size = New Size(72, 15)
            Label1.TabIndex = 6
            Label1.Text = "或者选择："
            ' 
            ' LicenseDialog
            ' 
            BackColor = Color.White
            ClientSize = New Size(490, 434)
            Controls.Add(GroupBox2)
            Controls.Add(btnClose)
            FormBorderStyle = FormBorderStyle.FixedDialog
            MaximizeBox = False
            MinimizeBox = False
            Name = "LicenseDialog"
            StartPosition = FormStartPosition.CenterParent
            Text = "软件授权管理"
            GroupBox1.ResumeLayout(False)
            GroupBox2.ResumeLayout(False)
            GroupBox2.PerformLayout()
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
