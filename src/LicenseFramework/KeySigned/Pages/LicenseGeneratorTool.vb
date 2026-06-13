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

    Private txtPrivateKey As New TextBox()
    Private txtFingerprint As New TextBox()
    Private txtLicenseOutput As New TextBox()
    Private cmbLicenseType As New ComboBox()
    Private numExpiryDays As New NumericUpDown()
    Private txtProductName As New TextBox()
    Private txtProductVersion As New TextBox()
    Private txtCustomerName As New TextBox()

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




        ' 标题
        Me.Controls.Add(New Label With {
            .Text = "许可证生成工具",
            .Font = New Drawing.Font("Microsoft YaHei", 14, Drawing.FontStyle.Bold),
            .Location = New Drawing.Point(20, 15),
            .AutoSize = True
        })


        ' 私钥
        Me.Controls.Add(New Label With {
            .Text = "RSA私钥文件:",
            .Location = New Drawing.Point(20, 55),
            .AutoSize = True
        })

        txtPrivateKey.Location = New Drawing.Point(20, 77)
        txtPrivateKey.Size = New Drawing.Size(580, 25)
        txtPrivateKey.ReadOnly = True
        Me.Controls.Add(txtPrivateKey)

        Dim btnLoadKey As New Button With {
            .Text = "加载...",
            .Size = New Drawing.Size(100, 25),
            .Location = New Drawing.Point(610, 77)
        }
        AddHandler btnLoadKey.Click, AddressOf BtnLoadKey_Click
        Me.Controls.Add(btnLoadKey)


        ' 指纹文件
        Me.Controls.Add(New Label With {
            .Text = "客户硬件指纹文件:",
            .Location = New Drawing.Point(20, 112),
            .AutoSize = True
        })


        txtFingerprint.Location = New Drawing.Point(20, 135)
        txtFingerprint.Size = New Drawing.Size(580, 25)
        txtFingerprint.ReadOnly = True
        Me.Controls.Add(txtFingerprint)

        Dim btnLoadFp As New Button With {
            .Text = "加载...",
            .Size = New Drawing.Size(100, 25),
            .Location = New Drawing.Point(610, 135)
        }
        AddHandler btnLoadFp.Click, AddressOf BtnLoadFingerprint_Click
        Me.Controls.Add(btnLoadFp)


        ' 产品名称
        Me.Controls.Add(New Label With {
            .Text = "产品名称:",
            .Location = New Drawing.Point(20, 170),
            .AutoSize = True
        })
        txtProductName.Location = New Drawing.Point(120, 170)
        txtProductName.Size = New Drawing.Size(250, 25)
        Me.Controls.Add(txtProductName)


        ' 产品版本
        Me.Controls.Add(New Label With {
            .Text = "产品版本:",
            .Location = New Drawing.Point(20, 205),
            .AutoSize = True
        })
        txtProductVersion.Location = New Drawing.Point(120, 205)
        txtProductVersion.Size = New Drawing.Size(250, 25)
        Me.Controls.Add(txtProductVersion)


        ' 客户名称
        Me.Controls.Add(New Label With {
            .Text = "客户名称:",
            .Location = New Drawing.Point(20, 240),
            .AutoSize = True
        })
        txtCustomerName.Location = New Drawing.Point(120, 240)
        txtCustomerName.Size = New Drawing.Size(250, 25)
        Me.Controls.Add(txtCustomerName)


        ' 许可证类型
        Me.Controls.Add(New Label With {
            .Text = "许可证类型:",
            .Location = New Drawing.Point(20, 275),
            .AutoSize = True
        })
        cmbLicenseType.Location = New Drawing.Point(120, 275)
        cmbLicenseType.Size = New Drawing.Size(250, 25)
        cmbLicenseType.DropDownStyle = ComboBoxStyle.DropDownList
        cmbLicenseType.Items.AddRange({"Trial - 试用版", "Standard - 标准版",
                                        "Professional - 专业版", "Enterprise - 企业版"})
        cmbLicenseType.SelectedIndex = 1
        Me.Controls.Add(cmbLicenseType)


        ' 有效期
        Me.Controls.Add(New Label With {
            .Text = "有效期(天,0=永久):",
            .Location = New Drawing.Point(20, 310),
            .AutoSize = True
        })
        numExpiryDays.Location = New Drawing.Point(170, 310)
        numExpiryDays.Size = New Drawing.Size(100, 25)
        numExpiryDays.Minimum = 0
        numExpiryDays.Maximum = 3650
        numExpiryDays.Value = 365
        Me.Controls.Add(numExpiryDays)


        ' 生成按钮
        Dim btnGenerate As New Button With {
            .Text = "生成许可证",
            .Size = New Drawing.Size(200, 40),
            .Location = New Drawing.Point(20, 350),
            .Font = New Drawing.Font("Microsoft YaHei", 10, Drawing.FontStyle.Bold)
        }
        AddHandler btnGenerate.Click, AddressOf BtnGenerate_Click
        Me.Controls.Add(btnGenerate)


        ' 输出
        Me.Controls.Add(New Label With {
            .Text = "生成的许可证:",
            .Location = New Drawing.Point(20, 400),
            .AutoSize = True
        })


        txtLicenseOutput.Location = New Drawing.Point(20, 422)
        txtLicenseOutput.Size = New Drawing.Size(690, 80)
        txtLicenseOutput.Multiline = True
        txtLicenseOutput.ReadOnly = True
        txtLicenseOutput.ScrollBars = ScrollBars.Vertical
        Me.Controls.Add(txtLicenseOutput)


        ' 保存按钮
        Dim btnSave As New Button With {
            .Text = "保存许可证文件",
            .Size = New Drawing.Size(200, 35),
            .Location = New Drawing.Point(20, 515)
        }
        AddHandler btnSave.Click, AddressOf BtnSaveLicense_Click
        Me.Controls.Add(btnSave)

        ResumeLayout(False)
    End Sub

    Private Sub BtnLoadKey_Click(sender As Object, e As EventArgs)
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

    Private Sub BtnLoadFingerprint_Click(sender As Object, e As EventArgs)
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

    Private Sub BtnGenerate_Click(sender As Object, e As EventArgs)
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

    Private Sub BtnSaveLicense_Click(sender As Object, e As EventArgs)
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

