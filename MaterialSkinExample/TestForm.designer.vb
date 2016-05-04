Partial Class TestForm
	''' <summary>
	''' Required designer variable.
	''' </summary>
	Private components As System.ComponentModel.IContainer = Nothing

	''' <summary>
	''' Clean up any resources being used.
	''' </summary>
	''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
	Protected Overrides Sub Dispose(disposing As Boolean)
		If disposing AndAlso (components IsNot Nothing) Then
			components.Dispose()
		End If
		MyBase.Dispose(disposing)
	End Sub

	#Region "Windows Form Designer generated code"

	''' <summary>
	''' Required method for Designer support - do not modify
	''' the contents of this method with the code editor.
	''' </summary>
	Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ChromeTabControl1 = New Microsoft.VisualBasic.Windows.Forms.ChromeTabControl.ChromeTabControl()
        Me.SuspendLayout()
        '
        'ChromeTabControl1
        '
        Me.ChromeTabControl1.AllowDrop = True
        Me.ChromeTabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ChromeTabControl1.BackColor = System.Drawing.Color.Transparent
        Me.ChromeTabControl1.Location = New System.Drawing.Point(0, 60)
        Me.ChromeTabControl1.Name = "ChromeTabControl1"
        Me.ChromeTabControl1.NewTabButton = True
        Me.ChromeTabControl1.Size = New System.Drawing.Size(757, 517)
        Me.ChromeTabControl1.TabIndex = 0
        Me.ChromeTabControl1.Text = "ChromeTabControl1"
        '
        'TestForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(756, 576)
        Me.Controls.Add(Me.ChromeTabControl1)
        Me.Cursor = System.Windows.Forms.Cursors.SizeWE
        Me.Name = "TestForm"
        Me.Text = "Chrome Tabs Test"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents ChromeTabControl1 As ChromeTabControl.ChromeTabControl

#End Region
End Class

