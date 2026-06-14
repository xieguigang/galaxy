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
    Friend WithEvents Button1 As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents Label2 As Label
    Private WithEvents TextBox2 As TextBox
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
        Button1 = New Button()
        Label1 = New Label()
        TextBox1 = New TextBox()
        Label2 = New Label()
        TextBox2 = New TextBox()
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
        ' Button1
        ' 
        Button1.Location = New Point(20, 630)
        Button1.Name = "Button1"
        Button1.Size = New Size(97, 30)
        Button1.TabIndex = 8
        Button1.Text = "混淆"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.ForeColor = Color.Red
        Label1.Location = New Point(20, 475)
        Label1.Name = "Label1"
        Label1.Size = New Size(280, 15)
        Label1.TabIndex = 9
        Label1.Text = "重新生成密钥对之前，请先备份旧的密钥对文件"
        ' 
        ' TextBox1
        ' 
        TextBox1.Location = New Point(236, 630)
        TextBox1.Name = "TextBox1"
        TextBox1.Size = New Size(52, 23)
        TextBox1.TabIndex = 10
        TextBox1.Text = "167"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(132, 633)
        Label2.Name = "Label2"
        Label2.Size = New Size(98, 15)
        Label2.TabIndex = 11
        Label2.Text = "异或混淆种子："
        ' 
        ' TextBox2
        ' 
        TextBox2.Location = New Point(20, 504)
        TextBox2.Multiline = True
        TextBox2.Name = "TextBox2"
        TextBox2.ReadOnly = True
        TextBox2.ScrollBars = ScrollBars.Vertical
        TextBox2.Size = New Size(650, 120)
        TextBox2.TabIndex = 12
        ' 
        ' KeyGeneratorForm
        ' 
        AutoScaleDimensions = New SizeF(96.0F, 96.0F)
        ClientSize = New Size(691, 772)
        Controls.Add(TextBox1)
        Controls.Add(TextBox2)
        Controls.Add(Label2)
        Controls.Add(Label1)
        Controls.Add(Button1)
        Controls.Add(lblTitle)
        Controls.Add(btnGenerate)
        Controls.Add(lblPrompt)
        Controls.Add(txtPrivateKey)
        Controls.Add(btnSavePrivate)
        Controls.Add(lblPrompt2)
        Controls.Add(txtPublicKey)
        Controls.Add(btnSavePublic)
        DockAreas = Microsoft.VisualStudio.WinForms.Docking.DockAreas.Float Or Microsoft.VisualStudio.WinForms.Docking.DockAreas.DockLeft Or Microsoft.VisualStudio.WinForms.Docking.DockAreas.DockRight Or Microsoft.VisualStudio.WinForms.Docking.DockAreas.DockTop Or Microsoft.VisualStudio.WinForms.Docking.DockAreas.DockBottom Or Microsoft.VisualStudio.WinForms.Docking.DockAreas.Document
        DoubleBuffered = True
        Name = "KeyGeneratorForm"
        ShowHint = Microsoft.VisualStudio.WinForms.Docking.DockState.Unknown
        StartPosition = FormStartPosition.CenterScreen
        TabPageContextMenuStrip = DockContextMenuStrip1
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

    ''' <summary>
    ''' GenerateObfuscatedKey()
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ' 1. 你的原始公钥
        Dim originalKey As String = txtPublicKey.Text
        ' 2. 定义一个异或密钥 (你可以随意修改这个数值，比如 &HA7, &H5C 等，只要解密时用一样的就行)
        Dim xorKey As Byte = CByte(TextBox1.Text) ' 167
        ' 3. 将字符串转换为字节数组
        Dim keyBytes As Byte() = Encoding.UTF8.GetBytes(originalKey)

        ' 4. 进行 XOR 加密
        For i As Integer = 0 To keyBytes.Length - 1
            keyBytes(i) = keyBytes(i) Xor xorKey
        Next

        ' 5. 转换为 Base64 字符串 (因为XOR后可能产生不可见字符，Base64确保都是可见字符方便硬编码)
        Dim base64Encrypted As String = Convert.ToBase64String(keyBytes)

        ' 6. 拆分成多个片段 (这里拆成4段，你可以自由决定段数和每段长度)
        Dim segLen As Integer = base64Encrypted.Length \ 4
        Dim parts(3) As String
        parts(0) = base64Encrypted.Substring(0, segLen)
        parts(1) = base64Encrypted.Substring(segLen, segLen)
        parts(2) = base64Encrypted.Substring(segLen * 2, segLen)
        parts(3) = base64Encrypted.Substring(segLen * 3)

        Dim sb As New StringBuilder

        ' 7. 输出结果，你将把输出的结果复制到你的正式项目中
        sb.AppendLine("' 请将以下数组和密钥复制到你的正式项目代码中：")
        sb.AppendLine($"Dim xorKey As Byte = &H{xorKey:X2}")
        sb.AppendLine($"Dim keyParts() As String = {{ 
   ""{parts(0)}"", 
   ""{parts(1)}"", 
   ""{parts(2)}"", 
   ""{parts(3)}"" 
}}")

        TextBox2.Text = sb.ToString
    End Sub
End Class

