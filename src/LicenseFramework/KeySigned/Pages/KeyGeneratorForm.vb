Imports System.IO
Imports System.Text
Imports System.Windows.Forms
Imports LicenseVendor.LicenseFramework.[Shared]

''' <summary>
''' 密钥生成工具的WinForms界面
''' </summary>
Public Class KeyGeneratorForm
    Inherits Form

    Private WithEvents txtPrivateKey As TextBox
    Private WithEvents txtPublicKey As TextBox

    Dim WithEvents lblTitle As Label
    WithEvents btnGenerate As Button
    Dim WithEvents lblPrompt As Label
    Dim WithEvents lblPrompt2 As Label
    WithEvents btnSavePrivate As Button
    WithEvents btnSavePublic As Button

    Public Sub New()
        Call InitializeComponent()
    End Sub

    Private Sub InitializeComponent()
        Me.Text = "RSA密钥对生成工具"
        Me.Size = New Drawing.Size(700, 500)
        Me.StartPosition = FormStartPosition.CenterScreen

        lblTitle = New Label
        lblTitle.Text = "RSA 2048位密钥对生成工具"
        lblTitle.Font = New Drawing.Font("Microsoft YaHei", 14, Drawing.FontStyle.Bold)
        lblTitle.Location = New Drawing.Point(20, 15)
        lblTitle.AutoSize = True



        btnGenerate = New Button
        btnGenerate.Text = "生成密钥对"
        btnGenerate.Size = New Drawing.Size(120, 35)
        btnGenerate.Location = New Drawing.Point(20, 55)




        lblPrompt = New Label
        lblPrompt.Text = "私钥 (Private Key) - 请妥善保管，切勿泄露:"
        lblPrompt.Location = New Drawing.Point(20, 105)
        lblPrompt.AutoSize = True

        ' 私钥


        txtPrivateKey = New TextBox

        txtPrivateKey.Multiline = True
        txtPrivateKey.Size = New Drawing.Size(650, 120)
        txtPrivateKey.Location = New Drawing.Point(20, 125)
        txtPrivateKey.ReadOnly = True
        txtPrivateKey.ScrollBars = ScrollBars.Vertical


        btnSavePrivate = New Button
        btnSavePrivate.Text = "保存私钥"
        btnSavePrivate.Size = New Drawing.Size(100, 30)
        btnSavePrivate.Location = New Drawing.Point(20, 250)



        lblPrompt2 = New Label
        lblPrompt2.Text = "公钥 (Public Key) - 嵌入到客户端程序中:"
        lblPrompt2.Location = New Drawing.Point(20, 290)
        lblPrompt2.AutoSize = True

        ' 公钥


        txtPublicKey = New TextBox

        txtPublicKey.Multiline = True
        txtPublicKey.Size = New Drawing.Size(650, 120)
        txtPublicKey.Location = New Drawing.Point(20, 310)
        txtPublicKey.ReadOnly = True
        txtPublicKey.ScrollBars = ScrollBars.Vertical


        btnSavePublic = New Button
        btnSavePublic.Text = "保存公钥"
        btnSavePublic.Size = New Drawing.Size(100, 30)
        btnSavePublic.Location = New Drawing.Point(20, 435)

        Me.Controls.Add(lblTitle)
        Me.Controls.Add(btnGenerate)
        Me.Controls.Add(lblPrompt)
        Me.Controls.Add(txtPrivateKey)
        Me.Controls.Add(btnSavePrivate)
        Me.Controls.Add(lblPrompt2)
        Me.Controls.Add(txtPublicKey)
        Me.Controls.Add(btnSavePublic)
    End Sub

    Private Sub BtnGenerate_Click(sender As Object, e As EventArgs) Handles btnGenerate.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Dim publicKeyXml As String = String.Empty
            Dim privateKeyXml As String = String.Empty

            CryptoHelper.GenerateRsaKeyPair(publicKeyXml, privateKeyXml)

            txtPrivateKey.Text = privateKeyXml
            txtPublicKey.Text = publicKeyXml

            Me.Cursor = Cursors.Default
            MessageBox.Show("密钥对生成成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MessageBox.Show($"生成密钥失败: {ex.Message}",
                            "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub BtnSavePrivate_Click(sender As Object, e As EventArgs) Handles btnSavePrivate.Click
        If String.IsNullOrEmpty(txtPrivateKey.Text) Then Return
        Using dlg As New SaveFileDialog With {
            .Filter = "XML文件|*.xml",
            .FileName = "private_key.xml",
            .Title = "保存私钥文件"
        }
            If dlg.ShowDialog() = DialogResult.OK Then
                File.WriteAllText(dlg.FileName, txtPrivateKey.Text, Encoding.UTF8)
                MessageBox.Show("私钥已保存", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End Using
    End Sub

    Private Sub BtnSavePublic_Click(sender As Object, e As EventArgs) Handles btnSavePublic.Click
        If String.IsNullOrEmpty(txtPublicKey.Text) Then Return
        Using dlg As New SaveFileDialog With {
            .Filter = "XML文件|*.xml",
            .FileName = "public_key.xml",
            .Title = "保存公钥文件"
        }
            If dlg.ShowDialog() = DialogResult.OK Then
                File.WriteAllText(dlg.FileName, txtPublicKey.Text, Encoding.UTF8)
                MessageBox.Show("公钥已保存", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End Using
    End Sub

End Class

