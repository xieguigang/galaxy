Imports Galaxy.Workbench.CommonDialogs

Namespace CommonDialogs

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class InputSelectColorMap
        Inherits InputDialog

        'Form 重写 Dispose，以清理组件列表。
        <System.Diagnostics.DebuggerNonUserCode()>
        Protected Overrides Sub Dispose(disposing As Boolean)
            Try
                If disposing AndAlso components IsNot Nothing Then
                    components.Dispose()
                End If
            Finally
                MyBase.Dispose(disposing)
            End Try
        End Sub

        'Windows 窗体设计器所必需的
        Private components As System.ComponentModel.IContainer

        '注意: 以下过程是 Windows 窗体设计器所必需的
        '可以使用 Windows 窗体设计器修改它。  
        '不要使用代码编辑器修改它。
        <System.Diagnostics.DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.GroupBox1 = New System.Windows.Forms.GroupBox()
            Me.PictureBox1 = New System.Windows.Forms.PictureBox()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.ComboBox1 = New System.Windows.Forms.ComboBox()
            Me.Button1 = New System.Windows.Forms.Button()
            Me.Button2 = New System.Windows.Forms.Button()
            Me.GroupBox1.SuspendLayout()
            CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'GroupBox1
            '
            Me.GroupBox1.Controls.Add(Me.PictureBox1)
            Me.GroupBox1.Controls.Add(Me.Label1)
            Me.GroupBox1.Controls.Add(Me.ComboBox1)
            Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
            Me.GroupBox1.Name = "GroupBox1"
            Me.GroupBox1.Size = New System.Drawing.Size(256, 91)
            Me.GroupBox1.TabIndex = 0
            Me.GroupBox1.TabStop = False
            Me.GroupBox1.Text = "Color Maps"
            '
            'PictureBox1
            '
            Me.PictureBox1.Location = New System.Drawing.Point(18, 54)
            Me.PictureBox1.Name = "PictureBox1"
            Me.PictureBox1.Size = New System.Drawing.Size(224, 22)
            Me.PictureBox1.TabIndex = 2
            Me.PictureBox1.TabStop = False
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.Location = New System.Drawing.Point(16, 26)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(47, 12)
            Me.Label1.TabIndex = 1
            Me.Label1.Text = "Colors:"
            '
            'ComboBox1
            '
            Me.ComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.ComboBox1.FormattingEnabled = True
            Me.ComboBox1.Location = New System.Drawing.Point(84, 23)
            Me.ComboBox1.Name = "ComboBox1"
            Me.ComboBox1.Size = New System.Drawing.Size(158, 20)
            Me.ComboBox1.TabIndex = 0
            '
            'Button1
            '
            Me.Button1.Location = New System.Drawing.Point(179, 111)
            Me.Button1.Name = "Button1"
            Me.Button1.Size = New System.Drawing.Size(75, 23)
            Me.Button1.TabIndex = 1
            Me.Button1.Text = "OK"
            Me.Button1.UseVisualStyleBackColor = True
            '
            'Button2
            '
            Me.Button2.Location = New System.Drawing.Point(96, 111)
            Me.Button2.Name = "Button2"
            Me.Button2.Size = New System.Drawing.Size(75, 23)
            Me.Button2.TabIndex = 2
            Me.Button2.Text = "Cancel"
            Me.Button2.UseVisualStyleBackColor = True
            '
            'InputSelectColorMap
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(281, 144)
            Me.Controls.Add(Me.Button2)
            Me.Controls.Add(Me.Button1)
            Me.Controls.Add(Me.GroupBox1)
            Me.Name = "InputSelectColorMap"
            Me.Text = "Select Color Maps"
            Me.GroupBox1.ResumeLayout(False)
            Me.GroupBox1.PerformLayout()
            CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

        Friend WithEvents GroupBox1 As GroupBox
        Friend WithEvents PictureBox1 As PictureBox
        Friend WithEvents Label1 As Label
        Friend WithEvents ComboBox1 As ComboBox
        Friend WithEvents Button1 As Button
        Friend WithEvents Button2 As Button
    End Class
End Namespace