Imports System.IO
Imports System.Text
Imports System.Windows.Forms
Imports LicenseVendor.LicenseFramework.[Shared]

''' <summary>
''' 密钥生成工具的WinForms界面
''' </summary>
Public Class KeyGeneratorForm
    Inherits Form

    Private txtPrivateKey As New TextBox()
    Private txtPublicKey As New TextBox()

    Public Sub New()
        Me.Text = "RSA密钥对生成工具"
        Me.Size = New Drawing.Size(700, 500)
        Me.StartPosition = FormStartPosition.CenterScreen

        Dim lblTitle As New Label With {
            .Text = "RSA 2048位密钥对生成工具",
            .Font = New Drawing.Font("Microsoft YaHei", 14, Drawing.FontStyle.Bold),
            .Location = New Drawing.Point(20, 15),
            .AutoSize = True
        }
        Me.Controls.Add(lblTitle)

        Dim btnGenerate As New Button With {
            .Text = "生成密钥对",
            .Size = New Drawing.Size(120, 35),
            .Location = New Drawing.Point(20, 55)
        }
        AddHandler btnGenerate.Click, AddressOf BtnGenerate_Click
        Me.Controls.Add(btnGenerate)

        ' 私钥
        Me.Controls.Add(New Label With {
            .Text = "私钥 (Private Key) - 请妥善保管，切勿泄露:",
            .Location = New Drawing.Point(20, 105),
            .AutoSize = True
        })

        txtPrivateKey.Multiline = True
        txtPrivateKey.Size = New Drawing.Size(650, 120)
        txtPrivateKey.Location = New Drawing.Point(20, 125)
        txtPrivateKey.ReadOnly = True
        txtPrivateKey.ScrollBars = ScrollBars.Vertical
        Me.Controls.Add(txtPrivateKey)

        Dim btnSavePrivate As New Button With {
            .Text = "保存私钥",
            .Size = New Drawing.Size(100, 30),
            .Location = New Drawing.Point(20, 250)
        }
        AddHandler btnSavePrivate.Click, AddressOf BtnSavePrivate_Click
        Me.Controls.Add(btnSavePrivate)

        ' 公钥
        Me.Controls.Add(New Label With {
            .Text = "公钥 (Public Key) - 嵌入到客户端程序中:",
            .Location = New Drawing.Point(20, 290),
            .AutoSize = True
        })

        txtPublicKey.Multiline = True
        txtPublicKey.Size = New Drawing.Size(650, 120)
        txtPublicKey.Location = New Drawing.Point(20, 310)
        txtPublicKey.ReadOnly = True
        txtPublicKey.ScrollBars = ScrollBars.Vertical
        Me.Controls.Add(txtPublicKey)

        Dim btnSavePublic As New Button With {
            .Text = "保存公钥",
            .Size = New Drawing.Size(100, 30),
            .Location = New Drawing.Point(20, 435)
        }
        AddHandler btnSavePublic.Click, AddressOf BtnSavePublic_Click
        Me.Controls.Add(btnSavePublic)
    End Sub

    Private Sub BtnGenerate_Click(sender As Object, e As EventArgs)
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

    Private Sub BtnSavePrivate_Click(sender As Object, e As EventArgs)
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

    Private Sub BtnSavePublic_Click(sender As Object, e As EventArgs)
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

