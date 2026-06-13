Imports System.IO
Imports System.Text
Imports System.Windows.Forms
Imports LicenseVendor.LicenseFramework.Shared
Imports LicenseVendor.LicenseFramework.Vendor

''' <summary>
''' 许可证生成工具的WinForms界面
''' </summary>
'''
''' ---------------------------------------------------------------
''' 许可证生成工具 (LicenseGeneratorTool.vb) - 厂商端WinForms工具
''' 提供可视化的许可证生成界面
''' ---------------------------------------------------------------
'''
Public Class LicenseGeneratorForm
    Inherits Form

    Private _privateKeyXml As String = String.Empty
    Private _currentFingerprint As String = String.Empty

    Private WithEvents txtPrivateKey As TextBox
    Private WithEvents txtFingerprint As TextBox
    Private WithEvents txtLicenseOutput As TextBox
    Private WithEvents cmbLicenseType As ComboBox
    Private WithEvents numExpiryDays As NumericUpDown
    Private WithEvents txtProductName As TextBox
    Private WithEvents txtProductVersion As TextBox
    Private WithEvents txtCustomerName As TextBox

    WithEvents lblPrompt1 As Label
    WithEvents lblPrompt2 As Label
    WithEvents lblPrompt3 As Label
    WithEvents lblPrompt4 As Label
    WithEvents lblPrompt5 As Label
    WithEvents lblPrompt6 As Label
    WithEvents lblPrompt7 As Label
    WithEvents lblPrompt8 As Label
    WithEvents lblPrompt9 As Label

    Dim WithEvents btnLoadKey As Button
    Dim WithEvents btnLoadFp As Button
    Dim WithEvents btnGenerate As Button
    Dim WithEvents btnSave As Button

    Public Sub New()
        Call InitializeComponent()
    End Sub

    Private Sub InitializeComponent()
        SuspendLayout()
        ' 
        ' LicenseGeneratorForm
        ' 
        ClientSize = New Size(693, 392)
        Me.Text = "许可证生成工具 - 厂商端"
        Me.Size = New Drawing.Size(750, 650)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.FormBorderStyle = FormBorderStyle.FixedSingle
        Me.MaximizeBox = False


        lblPrompt1 = New Label
        lblPrompt1.Text = "许可证生成工具"
        lblPrompt1.Font = New Drawing.Font("Microsoft YaHei", 14, Drawing.FontStyle.Bold)
        lblPrompt1.Location = New Drawing.Point(20, 15)
        lblPrompt1.AutoSize = True
        ' 标题
        Me.Controls.Add(lblPrompt1)

        lblPrompt2 = New Label

        lblPrompt2.Text = "RSA私钥文件:"
        lblPrompt2.Location = New Drawing.Point(20, 55)
        lblPrompt2.AutoSize = True
        ' 私钥
        Me.Controls.Add(lblPrompt2)

        txtPrivateKey = New TextBox

        txtPrivateKey.Location = New Drawing.Point(20, 77)
        txtPrivateKey.Size = New Drawing.Size(580, 25)
        txtPrivateKey.ReadOnly = True
        Me.Controls.Add(txtPrivateKey)

        btnLoadKey = New Button
        btnLoadKey.Text = "加载..."
        btnLoadKey.Size = New Drawing.Size(100, 25)
        btnLoadKey.Location = New Drawing.Point(610, 77)


        Me.Controls.Add(btnLoadKey)

        lblPrompt3 = New Label
        lblPrompt3.Text = "客户硬件指纹文件:"
        lblPrompt3.Location = New Drawing.Point(20, 112)
        lblPrompt3.AutoSize = True

        ' 指纹文件
        Me.Controls.Add(lblPrompt3)


        txtFingerprint = New TextBox

        txtFingerprint.Location = New Drawing.Point(20, 135)
        txtFingerprint.Size = New Drawing.Size(580, 25)
        txtFingerprint.ReadOnly = True
        Me.Controls.Add(txtFingerprint)

        btnLoadFp = New Button
        btnLoadFp.Text = "加载..."
        btnLoadFp.Size = New Drawing.Size(100, 25)
        btnLoadFp.Location = New Drawing.Point(610, 135)


        Me.Controls.Add(btnLoadFp)

        lblPrompt4 = New Label
        lblPrompt4.Text = "产品名称:"
        lblPrompt4.Location = New Drawing.Point(20, 170)
        lblPrompt4.AutoSize = True
        ' 产品名称
        Me.Controls.Add(lblPrompt4)

        txtProductName = New TextBox
        txtProductName.Location = New Drawing.Point(120, 170)
        txtProductName.Size = New Drawing.Size(250, 25)
        Me.Controls.Add(txtProductName)

        lblPrompt5 = New Label

        lblPrompt5.Text = "产品版本:"
        lblPrompt5.Location = New Drawing.Point(20, 205)
        lblPrompt5.AutoSize = True
        ' 产品版本
        Me.Controls.Add(lblPrompt5)

        txtProductVersion = New TextBox
        txtProductVersion.Location = New Drawing.Point(120, 205)
        txtProductVersion.Size = New Drawing.Size(250, 25)
        Me.Controls.Add(txtProductVersion)

        lblPrompt6 = New Label

        lblPrompt6.Text = "客户名称:"
        lblPrompt6.Location = New Drawing.Point(20, 240)
        lblPrompt6.AutoSize = True

        ' 客户名称
        Me.Controls.Add(lblPrompt6)

        txtCustomerName = New TextBox
        txtCustomerName.Location = New Drawing.Point(120, 240)
        txtCustomerName.Size = New Drawing.Size(250, 25)
        Me.Controls.Add(txtCustomerName)

        lblPrompt7 = New Label
        lblPrompt7.Text = "许可证类型:"
        lblPrompt7.Location = New Drawing.Point(20, 275)
        lblPrompt7.AutoSize = True

        ' 许可证类型
        Me.Controls.Add(lblPrompt7)

        cmbLicenseType = New ComboBox
        cmbLicenseType.Location = New Drawing.Point(120, 275)
        cmbLicenseType.Size = New Drawing.Size(250, 25)
        cmbLicenseType.DropDownStyle = ComboBoxStyle.DropDownList
        cmbLicenseType.Items.AddRange({"Trial - 试用版", "Standard - 标准版",
                                        "Professional - 专业版", "Enterprise - 企业版"})
        cmbLicenseType.SelectedIndex = 1
        Me.Controls.Add(cmbLicenseType)

        lblPrompt8 = New Label
        lblPrompt8.Text = "有效期(天,0=永久):"
        lblPrompt8.Location = New Drawing.Point(20, 310)
        lblPrompt8.AutoSize = True

        ' 有效期
        Me.Controls.Add(lblPrompt8)

        numExpiryDays = New NumericUpDown
        numExpiryDays.Location = New Drawing.Point(170, 310)
        numExpiryDays.Size = New Drawing.Size(100, 25)
        numExpiryDays.Minimum = 0
        numExpiryDays.Maximum = 3650
        numExpiryDays.Value = 365
        Me.Controls.Add(numExpiryDays)


        ' 生成按钮
        btnGenerate = New Button
        btnGenerate.Text = "生成许可证"
        btnGenerate.Size = New Drawing.Size(200, 40)
        btnGenerate.Location = New Drawing.Point(20, 350)
        btnGenerate.Font = New Drawing.Font("Microsoft YaHei", 10, Drawing.FontStyle.Bold)


        Me.Controls.Add(btnGenerate)

        lblPrompt9 = New Label
        lblPrompt9.Text = "生成的许可证:"
        lblPrompt9.Location = New Drawing.Point(20, 400)
        lblPrompt9.AutoSize = True

        ' 输出
        Me.Controls.Add(lblPrompt9)





        txtLicenseOutput = New TextBox
        txtLicenseOutput.Location = New Drawing.Point(20, 422)
        txtLicenseOutput.Size = New Drawing.Size(690, 80)
        txtLicenseOutput.Multiline = True
        txtLicenseOutput.ReadOnly = True
        txtLicenseOutput.ScrollBars = ScrollBars.Vertical
        Me.Controls.Add(txtLicenseOutput)


        ' 保存按钮
        btnSave = New Button
        btnSave.Text = "保存许可证文件"
        btnSave.Size = New Drawing.Size(200, 35)
        btnSave.Location = New Drawing.Point(20, 515)


        Me.Controls.Add(btnSave)

        ResumeLayout(False)
    End Sub

    Private Sub BtnLoadKey_Click(sender As Object, e As EventArgs) Handles btnLoadKey.Click
        Using dlg As New OpenFileDialog With {
            .Filter = "XML文件|*.xml|所有文件|*.*",
            .Title = "选择RSA私钥文件"
        }
            If dlg.ShowDialog() = DialogResult.OK Then
                _privateKeyXml = File.ReadAllText(dlg.FileName, Encoding.UTF8)
                txtPrivateKey.Text = dlg.FileName
            End If
        End Using
    End Sub

    Private Sub BtnLoadFingerprint_Click(sender As Object, e As EventArgs) Handles btnLoadFp.Click
        Using dlg As New OpenFileDialog With {
            .Filter = "文本文件|*.txt|所有文件|*.*",
            .Title = "选择硬件指纹文件"
        }
            If dlg.ShowDialog() = DialogResult.OK Then
                Try
                    Dim content As String = File.ReadAllText(dlg.FileName, Encoding.UTF8)
                    txtFingerprint.Text = dlg.FileName

                    ' 尝试从文件中提取指纹哈希
                    Dim startMarker As String = "硬件指纹哈希 (Hardware Fingerprint Hash):"
                    Dim idx As Integer = content.IndexOf(startMarker)
                    If idx >= 0 Then
                        idx += startMarker.Length
                        Dim endIdx As Integer = content.IndexOf("---", idx)
                        If endIdx >= 0 Then
                            _currentFingerprint = content.Substring(idx, endIdx - idx).Trim()
                        End If
                    Else
                        _currentFingerprint = content.Trim()
                    End If
                Catch ex As Exception
                    MessageBox.Show($"读取指纹文件失败: {ex.Message}", "错误",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End Using
    End Sub

    Private Sub BtnGenerate_Click(sender As Object, e As EventArgs) Handles btnGenerate.Click
        If String.IsNullOrEmpty(_privateKeyXml) Then
            MessageBox.Show("请先加载RSA私钥文件", "提示",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If String.IsNullOrEmpty(_currentFingerprint) Then
            MessageBox.Show("请先加载硬件指纹文件", "提示",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Me.Cursor = Cursors.WaitCursor

            Dim generator As New LicenseGenerator(_privateKeyXml)

            Dim licenseType As LicenseType = CType(cmbLicenseType.SelectedIndex, LicenseType)
            Dim expiryDays As Integer = CInt(numExpiryDays.Value)

            Dim signedLicense As String = generator.GenerateLicense(
                _currentFingerprint,
                txtProductName.Text,
                txtProductVersion.Text,
                txtCustomerName.Text,
                licenseType,
                expiryDays)

            txtLicenseOutput.Text = signedLicense

            Me.Cursor = Cursors.Default
            MessageBox.Show("许可证生成成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MessageBox.Show($"生成许可证失败: {ex.Message}", "错误",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub BtnSaveLicense_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If String.IsNullOrEmpty(txtLicenseOutput.Text) Then Return

        Using dlg As New SaveFileDialog With {
            .Filter = "许可证文件|*.lic|文本文件|*.txt|所有文件|*.*",
            .FileName = $"license_{txtCustomerName.Text}_{DateTime.Now:yyyyMMdd}.lic",
            .Title = "保存许可证文件"
        }
            If dlg.ShowDialog() = DialogResult.OK Then
                File.WriteAllText(dlg.FileName, txtLicenseOutput.Text, Encoding.UTF8)
                MessageBox.Show("许可证文件已保存", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End Using
    End Sub

End Class

