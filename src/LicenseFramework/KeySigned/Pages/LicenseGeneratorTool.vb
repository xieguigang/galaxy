Imports System.IO
Imports System.Text
Imports Galaxy.Workbench.CommonDialogs
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
Public Class LicenseGeneratorForm : Inherits InputDialog

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
    Friend WithEvents GroupBox1 As GroupBox
    Private WithEvents lblPrompt1 As Label
    Dim WithEvents btnSave As Button

    Public Sub New()
        Call InitializeComponent()
    End Sub

    Private Sub InitializeComponent()
        lblPrompt2 = New Label()
        txtPrivateKey = New TextBox()
        btnLoadKey = New Button()
        lblPrompt3 = New Label()
        txtFingerprint = New TextBox()
        btnLoadFp = New Button()
        lblPrompt4 = New Label()
        txtProductName = New TextBox()
        lblPrompt5 = New Label()
        txtProductVersion = New TextBox()
        lblPrompt6 = New Label()
        txtCustomerName = New TextBox()
        lblPrompt7 = New Label()
        cmbLicenseType = New ComboBox()
        lblPrompt8 = New Label()
        numExpiryDays = New NumericUpDown()
        btnGenerate = New Button()
        lblPrompt9 = New Label()
        txtLicenseOutput = New TextBox()
        btnSave = New Button()
        GroupBox1 = New GroupBox()
        lblPrompt1 = New Label()
        CType(numExpiryDays, ComponentModel.ISupportInitialize).BeginInit()
        GroupBox1.SuspendLayout()
        SuspendLayout()
        ' 
        ' lblPrompt2
        ' 
        lblPrompt2.AutoSize = True
        lblPrompt2.Location = New Point(17, 39)
        lblPrompt2.Name = "lblPrompt2"
        lblPrompt2.Size = New Size(83, 15)
        lblPrompt2.TabIndex = 1
        lblPrompt2.Text = "RSA私钥文件:"
        ' 
        ' txtPrivateKey
        ' 
        txtPrivateKey.Location = New Point(17, 61)
        txtPrivateKey.Name = "txtPrivateKey"
        txtPrivateKey.ReadOnly = True
        txtPrivateKey.Size = New Size(580, 23)
        txtPrivateKey.TabIndex = 2
        ' 
        ' btnLoadKey
        ' 
        btnLoadKey.BackColor = SystemColors.Control
        btnLoadKey.Location = New Point(607, 61)
        btnLoadKey.Name = "btnLoadKey"
        btnLoadKey.Size = New Size(100, 25)
        btnLoadKey.TabIndex = 3
        btnLoadKey.Text = "加载..."
        btnLoadKey.UseVisualStyleBackColor = False
        ' 
        ' lblPrompt3
        ' 
        lblPrompt3.AutoSize = True
        lblPrompt3.Location = New Point(17, 96)
        lblPrompt3.Name = "lblPrompt3"
        lblPrompt3.Size = New Size(114, 15)
        lblPrompt3.TabIndex = 4
        lblPrompt3.Text = "客户硬件指纹文件:"
        ' 
        ' txtFingerprint
        ' 
        txtFingerprint.Location = New Point(17, 119)
        txtFingerprint.Name = "txtFingerprint"
        txtFingerprint.ReadOnly = True
        txtFingerprint.Size = New Size(580, 23)
        txtFingerprint.TabIndex = 5
        ' 
        ' btnLoadFp
        ' 
        btnLoadFp.BackColor = SystemColors.Control
        btnLoadFp.Location = New Point(607, 119)
        btnLoadFp.Name = "btnLoadFp"
        btnLoadFp.Size = New Size(100, 25)
        btnLoadFp.TabIndex = 6
        btnLoadFp.Text = "加载..."
        btnLoadFp.UseVisualStyleBackColor = False
        ' 
        ' lblPrompt4
        ' 
        lblPrompt4.AutoSize = True
        lblPrompt4.Location = New Point(17, 154)
        lblPrompt4.Name = "lblPrompt4"
        lblPrompt4.Size = New Size(62, 15)
        lblPrompt4.TabIndex = 7
        lblPrompt4.Text = "产品名称:"
        ' 
        ' txtProductName
        ' 
        txtProductName.Location = New Point(117, 154)
        txtProductName.Name = "txtProductName"
        txtProductName.Size = New Size(250, 23)
        txtProductName.TabIndex = 8
        ' 
        ' lblPrompt5
        ' 
        lblPrompt5.AutoSize = True
        lblPrompt5.Location = New Point(17, 189)
        lblPrompt5.Name = "lblPrompt5"
        lblPrompt5.Size = New Size(62, 15)
        lblPrompt5.TabIndex = 9
        lblPrompt5.Text = "产品版本:"
        ' 
        ' txtProductVersion
        ' 
        txtProductVersion.Location = New Point(117, 189)
        txtProductVersion.Name = "txtProductVersion"
        txtProductVersion.Size = New Size(250, 23)
        txtProductVersion.TabIndex = 10
        ' 
        ' lblPrompt6
        ' 
        lblPrompt6.AutoSize = True
        lblPrompt6.Location = New Point(17, 224)
        lblPrompt6.Name = "lblPrompt6"
        lblPrompt6.Size = New Size(62, 15)
        lblPrompt6.TabIndex = 11
        lblPrompt6.Text = "客户名称:"
        ' 
        ' txtCustomerName
        ' 
        txtCustomerName.Location = New Point(117, 224)
        txtCustomerName.Name = "txtCustomerName"
        txtCustomerName.Size = New Size(250, 23)
        txtCustomerName.TabIndex = 12
        ' 
        ' lblPrompt7
        ' 
        lblPrompt7.AutoSize = True
        lblPrompt7.Location = New Point(17, 259)
        lblPrompt7.Name = "lblPrompt7"
        lblPrompt7.Size = New Size(75, 15)
        lblPrompt7.TabIndex = 13
        lblPrompt7.Text = "许可证类型:"
        ' 
        ' cmbLicenseType
        ' 
        cmbLicenseType.DropDownStyle = ComboBoxStyle.DropDownList
        cmbLicenseType.Items.AddRange(New Object() {"Trial - 试用版", "Standard - 标准版", "Professional - 专业版", "Enterprise - 企业版"})
        cmbLicenseType.Location = New Point(117, 259)
        cmbLicenseType.Name = "cmbLicenseType"
        cmbLicenseType.Size = New Size(250, 23)
        cmbLicenseType.TabIndex = 14
        ' 
        ' lblPrompt8
        ' 
        lblPrompt8.AutoSize = True
        lblPrompt8.Location = New Point(17, 294)
        lblPrompt8.Name = "lblPrompt8"
        lblPrompt8.Size = New Size(113, 15)
        lblPrompt8.TabIndex = 15
        lblPrompt8.Text = "有效期(天,0=永久):"
        ' 
        ' numExpiryDays
        ' 
        numExpiryDays.Location = New Point(167, 294)
        numExpiryDays.Maximum = New Decimal(New Integer() {3650, 0, 0, 0})
        numExpiryDays.Name = "numExpiryDays"
        numExpiryDays.Size = New Size(100, 23)
        numExpiryDays.TabIndex = 16
        numExpiryDays.Value = New Decimal(New Integer() {365, 0, 0, 0})
        ' 
        ' btnGenerate
        ' 
        btnGenerate.Font = New Font("Microsoft YaHei", 10F, FontStyle.Bold)
        btnGenerate.Location = New Point(17, 334)
        btnGenerate.Name = "btnGenerate"
        btnGenerate.Size = New Size(200, 40)
        btnGenerate.TabIndex = 17
        btnGenerate.Text = "生成许可证"
        ' 
        ' lblPrompt9
        ' 
        lblPrompt9.AutoSize = True
        lblPrompt9.Location = New Point(17, 384)
        lblPrompt9.Name = "lblPrompt9"
        lblPrompt9.Size = New Size(88, 15)
        lblPrompt9.TabIndex = 18
        lblPrompt9.Text = "生成的许可证:"
        ' 
        ' txtLicenseOutput
        ' 
        txtLicenseOutput.Location = New Point(17, 406)
        txtLicenseOutput.Multiline = True
        txtLicenseOutput.Name = "txtLicenseOutput"
        txtLicenseOutput.ReadOnly = True
        txtLicenseOutput.ScrollBars = ScrollBars.Vertical
        txtLicenseOutput.Size = New Size(690, 80)
        txtLicenseOutput.TabIndex = 19
        ' 
        ' btnSave
        ' 
        btnSave.Location = New Point(17, 499)
        btnSave.Name = "btnSave"
        btnSave.Size = New Size(200, 35)
        btnSave.TabIndex = 20
        btnSave.Text = "保存许可证文件"
        ' 
        ' GroupBox1
        ' 
        GroupBox1.Controls.Add(btnSave)
        GroupBox1.Controls.Add(lblPrompt2)
        GroupBox1.Controls.Add(txtLicenseOutput)
        GroupBox1.Controls.Add(txtPrivateKey)
        GroupBox1.Controls.Add(lblPrompt9)
        GroupBox1.Controls.Add(btnLoadKey)
        GroupBox1.Controls.Add(btnGenerate)
        GroupBox1.Controls.Add(lblPrompt3)
        GroupBox1.Controls.Add(numExpiryDays)
        GroupBox1.Controls.Add(txtFingerprint)
        GroupBox1.Controls.Add(lblPrompt8)
        GroupBox1.Controls.Add(btnLoadFp)
        GroupBox1.Controls.Add(cmbLicenseType)
        GroupBox1.Controls.Add(lblPrompt4)
        GroupBox1.Controls.Add(lblPrompt7)
        GroupBox1.Controls.Add(txtProductName)
        GroupBox1.Controls.Add(txtCustomerName)
        GroupBox1.Controls.Add(lblPrompt5)
        GroupBox1.Controls.Add(lblPrompt6)
        GroupBox1.Controls.Add(txtProductVersion)
        GroupBox1.Location = New Point(12, 62)
        GroupBox1.Name = "GroupBox1"
        GroupBox1.Size = New Size(728, 549)
        GroupBox1.TabIndex = 21
        GroupBox1.TabStop = False
        GroupBox1.Text = "许可证生成工具"
        ' 
        ' lblPrompt1
        ' 
        lblPrompt1.AutoSize = True
        lblPrompt1.Font = New Font("Microsoft YaHei", 14F, FontStyle.Bold)
        lblPrompt1.Location = New Point(12, 18)
        lblPrompt1.Name = "lblPrompt1"
        lblPrompt1.Size = New Size(183, 26)
        lblPrompt1.TabIndex = 0
        lblPrompt1.Text = "软件许可证生成工具"
        ' 
        ' LicenseGeneratorForm
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        ClientSize = New Size(751, 621)
        Controls.Add(lblPrompt1)
        Controls.Add(GroupBox1)
        Name = "LicenseGeneratorForm"
        Text = "许可证生成工具 - 厂商端"
        CType(numExpiryDays, ComponentModel.ISupportInitialize).EndInit()
        GroupBox1.ResumeLayout(False)
        GroupBox1.PerformLayout()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Private Sub BtnLoadKey_Click(sender As Object, e As EventArgs) Handles btnLoadKey.Click
        Using dlg As New OpenFileDialog With {
            .Filter = "XML文件|*.xml|所有文件|*.*",
            .Title = "选择RSA私钥文件"
        }
            If dlg.ShowDialog = DialogResult.OK Then
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
            If dlg.ShowDialog = DialogResult.OK Then
                Try
                    Dim content = File.ReadAllText(dlg.FileName, Encoding.UTF8)
                    txtFingerprint.Text = dlg.FileName

                    ' 尝试从文件中提取指纹哈希
                    Dim startMarker = "硬件指纹哈希 (Hardware Fingerprint Hash):"
                    Dim idx = content.IndexOf(startMarker)
                    If idx >= 0 Then
                        idx += startMarker.Length
                        Dim endIdx = content.IndexOf("---", idx)
                        If endIdx >= 0 Then
                            _currentFingerprint = content.Substring(idx, endIdx - idx).Trim
                        End If
                    Else
                        _currentFingerprint = content.Trim
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
            Cursor = Cursors.WaitCursor

            Dim generator As New LicenseGenerator(_privateKeyXml)

            Dim licenseType As LicenseType = cmbLicenseType.SelectedIndex
            Dim expiryDays As Integer = numExpiryDays.Value

            Dim signedLicense = generator.GenerateLicense(
                _currentFingerprint,
                txtProductName.Text,
                txtProductVersion.Text,
                txtCustomerName.Text,
                licenseType,
                expiryDays)

            txtLicenseOutput.Text = signedLicense

            Cursor = Cursors.Default
            MessageBox.Show("许可证生成成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            Cursor = Cursors.Default
            MessageBox.Show($"生成许可证失败: {ex.Message}", "错误",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub BtnSaveLicense_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If String.IsNullOrEmpty(txtLicenseOutput.Text) Then Return

        Using dlg As New SaveFileDialog With {
            .Filter = "许可证文件|*.lic|文本文件|*.txt|所有文件|*.*",
            .FileName = $"license_{txtCustomerName.Text}_{Date.Now:yyyyMMdd}.lic",
            .Title = "保存许可证文件"
        }
            If dlg.ShowDialog = DialogResult.OK Then
                File.WriteAllText(dlg.FileName, txtLicenseOutput.Text, Encoding.UTF8)
                MessageBox.Show("许可证文件已保存", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End Using
    End Sub

    Private Sub LicenseGeneratorForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        txtPrivateKey.Text = Workbench.private_key
        _privateKeyXml = Workbench.private_key.ReadAllText
    End Sub
End Class

