
Imports System.ComponentModel
Imports System.Windows.Forms

Namespace TableSheet
    Partial Class FormCustomFilter
        ''' <summary>
        ''' Required designer variable.
        ''' </summary>
        Private components As IContainer = Nothing

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
            components = New Container()
            button_ok = New Button()
            button_cancel = New Button()
            label_columnName = New Label()
            comboBox_filterType = New ComboBox()
            label_and = New Label()
            errorProvider = New ErrorProvider(components)
            CType(errorProvider, ISupportInitialize).BeginInit()
            SuspendLayout()
            ' 
            ' button_ok
            ' 
            button_ok.DialogResult = DialogResult.OK
            button_ok.Location = New Drawing.Point(40, 139)
            button_ok.Name = "button_ok"
            button_ok.Size = New Drawing.Size(75, 23)
            button_ok.TabIndex = 0
            button_ok.Text = "OK"
            button_ok.UseVisualStyleBackColor = True
            AddHandler button_ok.Click, New EventHandler(AddressOf button_ok_Click)
            ' 
            ' button_cancel
            ' 
            button_cancel.DialogResult = DialogResult.Cancel
            button_cancel.Location = New Drawing.Point(121, 139)
            button_cancel.Name = "button_cancel"
            button_cancel.Size = New Drawing.Size(75, 23)
            button_cancel.TabIndex = 1
            button_cancel.Text = "Cancel"
            button_cancel.UseVisualStyleBackColor = True
            AddHandler button_cancel.Click, New EventHandler(AddressOf button_cancel_Click)
            ' 
            ' label_columnName
            ' 
            label_columnName.AutoSize = True
            label_columnName.Location = New Drawing.Point(4, 9)
            label_columnName.Name = "label_columnName"
            label_columnName.Size = New Drawing.Size(120, 13)
            label_columnName.TabIndex = 2
            label_columnName.Text = "Show rows where value"
            ' 
            ' comboBox_filterType
            ' 
            comboBox_filterType.DropDownStyle = ComboBoxStyle.DropDownList
            comboBox_filterType.FormattingEnabled = True
            comboBox_filterType.Location = New Drawing.Point(7, 25)
            comboBox_filterType.Name = "comboBox_filterType"
            comboBox_filterType.Size = New Drawing.Size(189, 21)
            comboBox_filterType.TabIndex = 3
            AddHandler comboBox_filterType.SelectedIndexChanged, New EventHandler(AddressOf comboBox_filterType_SelectedIndexChanged)
            ' 
            ' label_and
            ' 
            label_and.AutoSize = True
            label_and.Location = New Drawing.Point(7, 89)
            label_and.Name = "label_and"
            label_and.Size = New Drawing.Size(26, 13)
            label_and.TabIndex = 6
            label_and.Text = "And"
            label_and.Visible = False
            ' 
            ' errorProvider
            ' 
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink
            errorProvider.ContainerControl = Me
            ' 
            ' FormCustomFilter
            ' 
            AutoScaleDimensions = New Drawing.SizeF(6.0F, 13.0F)
            AutoScaleMode = AutoScaleMode.Font
            CancelButton = button_cancel
            ClientSize = New Drawing.Size(205, 169)
            Controls.Add(label_and)
            Controls.Add(label_columnName)
            Controls.Add(comboBox_filterType)
            Controls.Add(button_cancel)
            Controls.Add(button_ok)
            FormBorderStyle = FormBorderStyle.FixedToolWindow
            MaximizeBox = False
            MinimizeBox = False
            Name = "FormCustomFilter"
            ShowIcon = False
            ShowInTaskbar = False
            StartPosition = FormStartPosition.CenterParent
            Text = "Custom Filter"
            TopMost = True
            AddHandler Load, New EventHandler(AddressOf FormCustomFilter_Load)
            CType(errorProvider, ISupportInitialize).EndInit()
            ResumeLayout(False)
            PerformLayout()

        End Sub

#End Region

        Private button_ok As Button
        Private button_cancel As Button
        Private label_columnName As Label
        Private comboBox_filterType As ComboBox
        Private label_and As Label
        Private errorProvider As ErrorProvider
    End Class
End Namespace
