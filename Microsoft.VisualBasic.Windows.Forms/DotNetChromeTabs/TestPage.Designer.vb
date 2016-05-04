Partial Class TestPage
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

	#Region "Component Designer generated code"

	''' <summary> 
	''' Required method for Designer support - do not modify 
	''' the contents of this method with the code editor.
	''' </summary>
	Private Sub InitializeComponent()
		Me.label1 = New System.Windows.Forms.Label()
		Me.SuspendLayout()
		' 
		' label1
		' 
		Me.label1.AutoSize = True
		Me.label1.Location = New System.Drawing.Point(3, 0)
		Me.label1.Name = "label1"
		Me.label1.Size = New System.Drawing.Size(96, 13)
		Me.label1.TabIndex = 0
		Me.label1.Text = "This is a test page."
		' 
		' TestPage
		' 
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.Controls.Add(Me.label1)
		Me.Name = "TestPage"
		Me.Size = New System.Drawing.Size(298, 239)
		AddHandler Me.Load, New System.EventHandler(AddressOf Me.TestPage_Load)
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub

	#End Region

	Private label1 As System.Windows.Forms.Label
End Class
