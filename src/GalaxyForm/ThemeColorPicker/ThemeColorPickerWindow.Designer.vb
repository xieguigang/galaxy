Namespace ThemeColorPicker
    Partial Class ThemeColorPickerWindow
        ''' <summary>
        ''' Required designer variable.
        ''' </summary>
        Private components As System.ComponentModel.IContainer = Nothing

        ''' <summary>
        ''' Clean up any resources being used.
        ''' </summary>
        ''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        Protected Overrides Sub Dispose(disposing As Boolean)
            If disposing AndAlso components IsNot Nothing Then
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ThemeColorPickerWindow))
            themeColorPicker1 = New ThemeColorPicker()
            tableLayoutPanel1 = New TableLayoutPanel()
            tableLayoutPanel1.SuspendLayout()
            SuspendLayout()
            ' 
            ' themeColorPicker1
            ' 
            themeColorPicker1.BackgroundImage = CType(resources.GetObject("themeColorPicker1.BackgroundImage"), Drawing.Image)
            themeColorPicker1.BackgroundImageLayout = ImageLayout.None
            themeColorPicker1.Location = New Drawing.Point(0, 0)
            themeColorPicker1.Margin = New Padding(0)
            themeColorPicker1.Name = "themeColorPicker1"
            themeColorPicker1.Color = Drawing.Color.Empty
            themeColorPicker1.Size = New Drawing.Size(174, 166)
            themeColorPicker1.TabIndex = 0
            AddHandler themeColorPicker1.ColorSelected, AddressOf themeColorPicker1_ColorSelected
            ' 
            ' tableLayoutPanel1
            ' 
            tableLayoutPanel1.AutoSize = True
            tableLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink
            tableLayoutPanel1.ColumnCount = 1
            tableLayoutPanel1.ColumnStyles.Add(New ColumnStyle())
            tableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 20.0F))
            tableLayoutPanel1.Controls.Add(themeColorPicker1, 0, 0)
            tableLayoutPanel1.Dock = DockStyle.Fill
            tableLayoutPanel1.Location = New Drawing.Point(0, 0)
            tableLayoutPanel1.Name = "tableLayoutPanel1"
            tableLayoutPanel1.RowCount = 1
            tableLayoutPanel1.RowStyles.Add(New RowStyle())
            tableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Absolute, 20.0F))
            tableLayoutPanel1.Size = New Drawing.Size(176, 168)
            tableLayoutPanel1.TabIndex = 1
            ' 
            ' ThemeColorPickerWindow
            ' 
            AutoScaleMode = AutoScaleMode.None
            AutoSize = True
            AutoSizeMode = AutoSizeMode.GrowAndShrink
            ClientSize = New Drawing.Size(176, 168)
            ControlBox = False
            Controls.Add(tableLayoutPanel1)
            FormBorderStyle = FormBorderStyle.FixedToolWindow
            MaximizeBox = False
            MinimizeBox = False
            Name = "ThemeColorPickerWindow"
            ShowIcon = False
            ShowInTaskbar = False
            StartPosition = FormStartPosition.Manual
            Text = "Color"
            tableLayoutPanel1.ResumeLayout(False)
            ResumeLayout(False)
            PerformLayout()

        End Sub

#End Region

        Public themeColorPicker1 As ThemeColorPicker
        Private tableLayoutPanel1 As TableLayoutPanel
    End Class
End Namespace
