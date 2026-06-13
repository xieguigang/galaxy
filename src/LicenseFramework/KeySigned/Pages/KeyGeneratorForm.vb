Imports System.IO
Imports System.Text
Imports Galaxy.Workbench.DockDocument
Imports LicenseVendor.LicenseFramework.[Shared]

''' <summary>
''' 密钥生成工具的WinForms界面
''' </summary>
Public Class KeyGeneratorForm : Inherits DocumentWindow

    Private WithEvents txtPrivateKey As TextBox
    Private WithEvents txtPublicKey As TextBox

    Dim WithEvents lblTitle As Label
    Dim WithEvents lblPrompt As Label
    Dim WithEvents lblPrompt2 As Label

    WithEvents btnSavePrivate As Button
    WithEvents btnSavePublic As Button
    WithEvents btnGenerate As Button

    Public Sub New()
        Call InitializeComponent()
    End Sub

    Private Sub InitializeComponent()
        lblTitle = New Label()
        btnGenerate = New Button()
        lblPrompt = New Label()
        txtPrivateKey = New TextBox()
        btnSavePrivate = New Button()
        lblPrompt2 = New Label()
        txtPublicKey = New TextBox()
        btnSavePublic = New Button()
        SuspendLayout()
        ' 
        ' lblTitle
        ' 
        lblTitle.AutoSize = True
        lblTitle.Font = New Font("Microsoft YaHei", 14.0F, FontStyle.Bold)
        lblTitle.Location = New Point(20, 15)
        lblTitle.Name = "lblTitle"
        lblTitle.Size = New Size(256, 26)
        lblTitle.TabIndex = 0
        lblTitle.Text = "RSA 2048位密钥对生成工具"
        ' 
        ' btnGenerate
        ' 
        btnGenerate.Location = New Point(20, 55)
        btnGenerate.Name = "btnGenerate"
        btnGenerate.Size = New Size(120, 35)
        btnGenerate.TabIndex = 1
        btnGenerate.Text = "生成密钥对"
        ' 
        ' lblPrompt
        ' 
        lblPrompt.AutoSize = True
        lblPrompt.Location = New Point(20, 105)
        lblPrompt.Name = "lblPrompt"
        lblPrompt.Size = New Size(246, 15)
        lblPrompt.TabIndex = 2
        lblPrompt.Text = "私钥 (Private Key) - 请妥善保管，切勿泄露:"
        ' 
        ' txtPrivateKey
        ' 
        txtPrivateKey.Location = New Point(20, 125)
        txtPrivateKey.Multiline = True
        txtPrivateKey.Name = "txtPrivateKey"
        txtPrivateKey.ReadOnly = True
        txtPrivateKey.ScrollBars = ScrollBars.Vertical
        txtPrivateKey.Size = New Size(650, 120)
        txtPrivateKey.TabIndex = 3
        ' 
        ' btnSavePrivate
        ' 
        btnSavePrivate.Location = New Point(20, 250)
        btnSavePrivate.Name = "btnSavePrivate"
        btnSavePrivate.Size = New Size(100, 30)
        btnSavePrivate.TabIndex = 4
        btnSavePrivate.Text = "保存私钥"
        ' 
        ' lblPrompt2
        ' 
        lblPrompt2.AutoSize = True
        lblPrompt2.Location = New Point(20, 290)
        lblPrompt2.Name = "lblPrompt2"
        lblPrompt2.Size = New Size(230, 15)
        lblPrompt2.TabIndex = 5
        lblPrompt2.Text = "公钥 (Public Key) - 嵌入到客户端程序中:"
        ' 
        ' txtPublicKey
        ' 
        txtPublicKey.Location = New Point(20, 310)
        txtPublicKey.Multiline = True
        txtPublicKey.Name = "txtPublicKey"
        txtPublicKey.ReadOnly = True
        txtPublicKey.ScrollBars = ScrollBars.Vertical
        txtPublicKey.Size = New Size(650, 120)
        txtPublicKey.TabIndex = 6
        ' 
        ' btnSavePublic
        ' 
        btnSavePublic.Location = New Point(20, 435)
        btnSavePublic.Name = "btnSavePublic"
        btnSavePublic.Size = New Size(100, 30)
        btnSavePublic.TabIndex = 7
        btnSavePublic.Text = "保存公钥"
        ' 
        ' KeyGeneratorForm
        ' 
        ClientSize = New Size(709, 487)
        Controls.Add(lblTitle)
        Controls.Add(btnGenerate)
        Controls.Add(lblPrompt)
        Controls.Add(txtPrivateKey)
        Controls.Add(btnSavePrivate)
        Controls.Add(lblPrompt2)
        Controls.Add(txtPublicKey)
        Controls.Add(btnSavePublic)
        Name = "KeyGeneratorForm"
        StartPosition = FormStartPosition.CenterScreen
        Text = "RSA密钥对生成工具"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Private Sub BtnGenerate_Click(sender As Object, e As EventArgs) Handles btnGenerate.Click
        If Workbench.CheckPrivateKey Then
            If MessageBox.Show("目前已经存在有一个应用程序的私钥文件的配置结果，重新生成新的私钥文件将会让当前的软件授权配置失效，需要重新使用新的私钥文件编译后才可以被使用，确认重新生成新的私钥文件？",
                               "确认更换新的私钥文件",
                               MessageBoxButtons.OKCancel,
                               MessageBoxIcon.Warning) = DialogResult.Cancel Then
                Return
            End If
        End If

        Try
            Me.Cursor = Cursors.WaitCursor
            Dim publicKeyXml As String = String.Empty
            Dim privateKeyXml As String = String.Empty

            CryptoHelper.GenerateRsaKeyPair(publicKeyXml, privateKeyXml)

            txtPrivateKey.Text = privateKeyXml
            txtPublicKey.Text = publicKeyXml

            Workbench.WriteLicenseKeys(publicKeyXml, privateKeyXml)
            Me.Cursor = Cursors.Default
            MessageBox.Show("密钥对生成成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MessageBox.Show($"生成密钥失败: {ex.Message}",
                            "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub BtnSavePrivate_Click(sender As Object, e As EventArgs) Handles btnSavePrivate.Click
        If String.IsNullOrEmpty(txtPrivateKey.Text) Then
            Return
        End If

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
        If String.IsNullOrEmpty(txtPublicKey.Text) Then
            Return
        End If

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

    Private Sub KeyGeneratorForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        With Workbench.GetLicenseKey
            txtPrivateKey.Text = .privateKey
            txtPublicKey.Text = .publicKey
        End With
    End Sub
End Class

