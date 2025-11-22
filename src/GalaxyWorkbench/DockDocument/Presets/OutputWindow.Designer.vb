Namespace DockDocument.Presets

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class OutputWindow
        Inherits ToolWindow

        'Form overrides dispose to clean up the component list.
        <System.Diagnostics.DebuggerNonUserCode()>
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Try
                If disposing AndAlso components IsNot Nothing Then
                    components.Dispose()
                End If
            Finally
                MyBase.Dispose(disposing)
            End Try
        End Sub

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(OutputWindow))
            Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
            Me.ToolStripLabel1 = New System.Windows.Forms.ToolStripLabel()
            Me.ToolStripComboBox1 = New System.Windows.Forms.ToolStripComboBox()
            Me.ToolStripButton1 = New System.Windows.Forms.ToolStripButton()
            Me.DataGridView1 = New System.Windows.Forms.DataGridView()
            Me.Column4 = New System.Windows.Forms.DataGridViewImageColumn()
            Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
            Me.CopyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.CopyMessageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.TextBox1 = New System.Windows.Forms.TextBox()
            Me.ToolStrip1.SuspendLayout()
            CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.ContextMenuStrip1.SuspendLayout()
            Me.SuspendLayout()
            '
            'ToolStrip1
            '
            Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripLabel1, Me.ToolStripComboBox1, Me.ToolStripButton1})
            Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
            Me.ToolStrip1.Name = "ToolStrip1"
            Me.ToolStrip1.Size = New System.Drawing.Size(1069, 25)
            Me.ToolStrip1.TabIndex = 0
            Me.ToolStrip1.Text = "ToolStrip1"
            '
            'ToolStripLabel1
            '
            Me.ToolStripLabel1.Image = CType(resources.GetObject("ToolStripLabel1.Image"), System.Drawing.Image)
            Me.ToolStripLabel1.Name = "ToolStripLabel1"
            Me.ToolStripLabel1.Size = New System.Drawing.Size(110, 22)
            Me.ToolStripLabel1.Text = "Output Content:"
            '
            'ToolStripComboBox1
            '
            Me.ToolStripComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.ToolStripComboBox1.Items.AddRange(New Object() {"Action Output", "Log Text"})
            Me.ToolStripComboBox1.Name = "ToolStripComboBox1"
            Me.ToolStripComboBox1.Size = New System.Drawing.Size(150, 25)
            '
            'ToolStripButton1
            '
            Me.ToolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.ToolStripButton1.Image = CType(resources.GetObject("ToolStripButton1.Image"), System.Drawing.Image)
            Me.ToolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.ToolStripButton1.Name = "ToolStripButton1"
            Me.ToolStripButton1.Size = New System.Drawing.Size(23, 22)
            Me.ToolStripButton1.Text = "Save"
            '
            'DataGridView1
            '
            Me.DataGridView1.BackgroundColor = System.Drawing.Color.WhiteSmoke
            Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column4, Me.Column1, Me.Column2, Me.Column3})
            Me.DataGridView1.ContextMenuStrip = Me.ContextMenuStrip1
            DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
            DataGridViewCellStyle2.Font = New System.Drawing.Font("Cambria", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
            DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
            Me.DataGridView1.DefaultCellStyle = DataGridViewCellStyle2
            Me.DataGridView1.Location = New System.Drawing.Point(85, 179)
            Me.DataGridView1.Name = "DataGridView1"
            Me.DataGridView1.RowTemplate.Height = 23
            Me.DataGridView1.Size = New System.Drawing.Size(590, 275)
            Me.DataGridView1.TabIndex = 1
            '
            'Column4
            '
            Me.Column4.HeaderText = ""
            Me.Column4.Name = "Column4"
            Me.Column4.ReadOnly = True
            Me.Column4.Width = 32
            '
            'Column1
            '
            Me.Column1.HeaderText = "Time"
            Me.Column1.Name = "Column1"
            Me.Column1.ReadOnly = True
            '
            'Column2
            '
            Me.Column2.HeaderText = "Action"
            Me.Column2.Name = "Column2"
            Me.Column2.ReadOnly = True
            '
            'Column3
            '
            Me.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
            Me.Column3.HeaderText = "Message"
            Me.Column3.Name = "Column3"
            Me.Column3.ReadOnly = True
            '
            'ContextMenuStrip1
            '
            Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CopyToolStripMenuItem, Me.CopyMessageToolStripMenuItem})
            Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
            Me.ContextMenuStrip1.Size = New System.Drawing.Size(152, 48)
            '
            'CopyToolStripMenuItem
            '
            Me.CopyToolStripMenuItem.Name = "CopyToolStripMenuItem"
            Me.CopyToolStripMenuItem.Size = New System.Drawing.Size(151, 22)
            Me.CopyToolStripMenuItem.Text = "Copy"
            '
            'CopyMessageToolStripMenuItem
            '
            Me.CopyMessageToolStripMenuItem.Name = "CopyMessageToolStripMenuItem"
            Me.CopyMessageToolStripMenuItem.Size = New System.Drawing.Size(151, 22)
            Me.CopyMessageToolStripMenuItem.Text = "Copy Message"
            '
            'TextBox1
            '
            Me.TextBox1.BackColor = System.Drawing.Color.White
            Me.TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.TextBox1.Location = New System.Drawing.Point(716, 147)
            Me.TextBox1.Multiline = True
            Me.TextBox1.Name = "TextBox1"
            Me.TextBox1.ReadOnly = True
            Me.TextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both
            Me.TextBox1.Size = New System.Drawing.Size(303, 307)
            Me.TextBox1.TabIndex = 2
            '
            'OutputWindow
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(1069, 550)
            Me.Controls.Add(Me.TextBox1)
            Me.Controls.Add(Me.DataGridView1)
            Me.Controls.Add(Me.ToolStrip1)
            Me.DockAreas = CType((((((Microsoft.VisualStudio.WinForms.Docking.DockAreas.Float Or Microsoft.VisualStudio.WinForms.Docking.DockAreas.DockLeft) _
            Or Microsoft.VisualStudio.WinForms.Docking.DockAreas.DockRight) _
            Or Microsoft.VisualStudio.WinForms.Docking.DockAreas.DockTop) _
            Or Microsoft.VisualStudio.WinForms.Docking.DockAreas.DockBottom) _
            Or Microsoft.VisualStudio.WinForms.Docking.DockAreas.Document), Microsoft.VisualStudio.WinForms.Docking.DockAreas)
            Me.DoubleBuffered = True
            Me.Name = "OutputWindow"
            Me.ShowHint = Microsoft.VisualStudio.WinForms.Docking.DockState.Unknown
            Me.Text = "Output"
            Me.ToolStrip1.ResumeLayout(False)
            Me.ToolStrip1.PerformLayout()
            CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ContextMenuStrip1.ResumeLayout(False)
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

        Friend WithEvents ToolStrip1 As ToolStrip
        Friend WithEvents ToolStripLabel1 As ToolStripLabel
        Friend WithEvents ToolStripComboBox1 As ToolStripComboBox
        Friend WithEvents ToolStripButton1 As ToolStripButton
        Friend WithEvents DataGridView1 As DataGridView
        Friend WithEvents TextBox1 As TextBox
        Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
        Friend WithEvents CopyToolStripMenuItem As ToolStripMenuItem
        Friend WithEvents CopyMessageToolStripMenuItem As ToolStripMenuItem
        Friend WithEvents Column4 As DataGridViewImageColumn
        Friend WithEvents Column1 As DataGridViewTextBoxColumn
        Friend WithEvents Column2 As DataGridViewTextBoxColumn
        Friend WithEvents Column3 As DataGridViewTextBoxColumn
    End Class
End Namespace