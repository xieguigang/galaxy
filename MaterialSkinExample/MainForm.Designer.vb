Imports System.Drawing
Imports System.Windows.Forms
Imports MaterialSkin
Imports MaterialSkin.Controls

Partial Class MainForm
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
		Dim resources As New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
		Me.materialFlatButton2 = New MaterialSkin.Controls.MaterialFlatButton()
		Me.materialFlatButton3 = New MaterialSkin.Controls.MaterialFlatButton()
		Me.materialFlatButton1 = New MaterialSkin.Controls.MaterialFlatButton()
		Me.materialDivider1 = New MaterialSkin.Controls.MaterialDivider()
		Me.materialRadioButton4 = New MaterialSkin.Controls.MaterialRadioButton()
		Me.materialLabel1 = New MaterialSkin.Controls.MaterialLabel()
		Me.materialRadioButton3 = New MaterialSkin.Controls.MaterialRadioButton()
		Me.materialRadioButton2 = New MaterialSkin.Controls.MaterialRadioButton()
		Me.materialCheckbox4 = New MaterialSkin.Controls.MaterialCheckBox()
		Me.materialCheckbox3 = New MaterialSkin.Controls.MaterialCheckBox()
		Me.materialCheckbox2 = New MaterialSkin.Controls.MaterialCheckBox()
		Me.materialCheckbox1 = New MaterialSkin.Controls.MaterialCheckBox()
		Me.materialSingleLineTextField2 = New MaterialSkin.Controls.MaterialSingleLineTextField()
		Me.materialSingleLineTextField1 = New MaterialSkin.Controls.MaterialSingleLineTextField()
		Me.materialButton1 = New MaterialSkin.Controls.MaterialRaisedButton()
		Me.materialRadioButton1 = New MaterialSkin.Controls.MaterialRadioButton()
		Me.materialTabSelector1 = New MaterialSkin.Controls.MaterialTabSelector()
		Me.materialTabControl1 = New MaterialSkin.Controls.MaterialTabControl()
		Me.tabPage1 = New System.Windows.Forms.TabPage()
		Me.materialSingleLineTextField3 = New MaterialSkin.Controls.MaterialSingleLineTextField()
		Me.materialRaisedButton1 = New MaterialSkin.Controls.MaterialRaisedButton()
		Me.tabPage2 = New System.Windows.Forms.TabPage()
		Me.materialCheckBox6 = New MaterialSkin.Controls.MaterialCheckBox()
		Me.materialCheckBox5 = New MaterialSkin.Controls.MaterialCheckBox()
		Me.tabPage3 = New System.Windows.Forms.TabPage()
		Me.tabPage4 = New System.Windows.Forms.TabPage()
		Me.materialListView1 = New MaterialSkin.Controls.MaterialListView()
		Me.columnHeader1 = DirectCast(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me.columnHeader2 = DirectCast(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me.columnHeader3 = DirectCast(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me.columnHeader4 = DirectCast(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me.tabPage5 = New System.Windows.Forms.TabPage()
		Me.materialContextMenuStrip1 = New MaterialSkin.Controls.MaterialContextMenuStrip()
		Me.item1ToolStripMenuItem = New MaterialSkin.Controls.MaterialToolStripMenuItem()
		Me.subItem1ToolStripMenuItem = New MaterialSkin.Controls.MaterialToolStripMenuItem()
		Me.subItem2ToolStripMenuItem = New MaterialSkin.Controls.MaterialToolStripMenuItem()
		Me.disabledItemToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
		Me.item2ToolStripMenuItem = New MaterialSkin.Controls.MaterialToolStripMenuItem()
		Me.toolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
		Me.item3ToolStripMenuItem = New MaterialSkin.Controls.MaterialToolStripMenuItem()
		Me.materialProgressBar1 = New MaterialSkin.Controls.MaterialProgressBar()
		Me.materialTabControl1.SuspendLayout()
		Me.tabPage1.SuspendLayout()
		Me.tabPage2.SuspendLayout()
		Me.tabPage3.SuspendLayout()
		Me.tabPage4.SuspendLayout()
		Me.tabPage5.SuspendLayout()
		Me.materialContextMenuStrip1.SuspendLayout()
		Me.SuspendLayout()
		' 
		' materialFlatButton2
		' 
		Me.materialFlatButton2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.materialFlatButton2.AutoSize = True
		Me.materialFlatButton2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
		Me.materialFlatButton2.Depth = 0
		Me.materialFlatButton2.Location = New System.Drawing.Point(417, 386)
		Me.materialFlatButton2.Margin = New System.Windows.Forms.Padding(4, 6, 4, 6)
		Me.materialFlatButton2.MouseState = MaterialSkin.MouseState.HOVER
		Me.materialFlatButton2.Name = "materialFlatButton2"
		Me.materialFlatButton2.Primary = False
		Me.materialFlatButton2.Size = New System.Drawing.Size(91, 36)
		Me.materialFlatButton2.TabIndex = 13
		Me.materialFlatButton2.Text = "Secondary"
		Me.materialFlatButton2.UseVisualStyleBackColor = True
		' 
		' materialFlatButton3
		' 
		Me.materialFlatButton3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.materialFlatButton3.AutoSize = True
		Me.materialFlatButton3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
		Me.materialFlatButton3.Depth = 0
		Me.materialFlatButton3.Enabled = False
		Me.materialFlatButton3.Location = New System.Drawing.Point(334, 386)
		Me.materialFlatButton3.Margin = New System.Windows.Forms.Padding(4, 6, 4, 6)
		Me.materialFlatButton3.MouseState = MaterialSkin.MouseState.HOVER
		Me.materialFlatButton3.Name = "materialFlatButton3"
		Me.materialFlatButton3.Primary = False
		Me.materialFlatButton3.Size = New System.Drawing.Size(75, 36)
		Me.materialFlatButton3.TabIndex = 19
		Me.materialFlatButton3.Text = "DISABLED"
		Me.materialFlatButton3.UseVisualStyleBackColor = True
		' 
		' materialFlatButton1
		' 
		Me.materialFlatButton1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.materialFlatButton1.AutoSize = True
		Me.materialFlatButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
		Me.materialFlatButton1.Depth = 0
		Me.materialFlatButton1.Location = New System.Drawing.Point(516, 386)
		Me.materialFlatButton1.Margin = New System.Windows.Forms.Padding(4, 6, 4, 6)
		Me.materialFlatButton1.MouseState = MaterialSkin.MouseState.HOVER
		Me.materialFlatButton1.Name = "materialFlatButton1"
		Me.materialFlatButton1.Primary = True
		Me.materialFlatButton1.Size = New System.Drawing.Size(71, 36)
		Me.materialFlatButton1.TabIndex = 1
		Me.materialFlatButton1.Text = "Primary"
		Me.materialFlatButton1.UseVisualStyleBackColor = True
		' 
		' materialDivider1
		' 
		Me.materialDivider1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.materialDivider1.BackColor = System.Drawing.Color.FromArgb(CInt(CByte(31)), CInt(CByte(0)), CInt(CByte(0)), CInt(CByte(0)))
		Me.materialDivider1.Depth = 0
		Me.materialDivider1.Location = New System.Drawing.Point(0, 379)
		Me.materialDivider1.Margin = New System.Windows.Forms.Padding(0)
		Me.materialDivider1.MouseState = MaterialSkin.MouseState.HOVER
		Me.materialDivider1.Name = "materialDivider1"
		Me.materialDivider1.Size = New System.Drawing.Size(618, 1)
		Me.materialDivider1.TabIndex = 16
		Me.materialDivider1.Text = "materialDivider1"
		' 
		' materialRadioButton4
		' 
		Me.materialRadioButton4.AutoSize = True
		Me.materialRadioButton4.Checked = True
		Me.materialRadioButton4.Cursor = System.Windows.Forms.Cursors.[Default]
		Me.materialRadioButton4.Depth = 0
		Me.materialRadioButton4.Enabled = False
		Me.materialRadioButton4.Font = New System.Drawing.Font("Roboto", 10F)
		Me.materialRadioButton4.Location = New System.Drawing.Point(0, 98)
		Me.materialRadioButton4.Margin = New System.Windows.Forms.Padding(0)
		Me.materialRadioButton4.MouseLocation = New System.Drawing.Point(-1, -1)
		Me.materialRadioButton4.MouseState = MaterialSkin.MouseState.HOVER
		Me.materialRadioButton4.Name = "materialRadioButton4"
		Me.materialRadioButton4.Ripple = True
		Me.materialRadioButton4.Size = New System.Drawing.Size(163, 30)
		Me.materialRadioButton4.TabIndex = 15
		Me.materialRadioButton4.TabStop = True
		Me.materialRadioButton4.Text = "materialRadioButton4"
		Me.materialRadioButton4.UseVisualStyleBackColor = True
		' 
		' materialLabel1
		' 
		Me.materialLabel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.materialLabel1.Depth = 0
		Me.materialLabel1.Font = New System.Drawing.Font("Roboto", 11F)
		Me.materialLabel1.ForeColor = System.Drawing.Color.FromArgb(CInt(CByte(222)), CInt(CByte(0)), CInt(CByte(0)), CInt(CByte(0)))
		Me.materialLabel1.Location = New System.Drawing.Point(-4, 117)
		Me.materialLabel1.MouseState = MaterialSkin.MouseState.HOVER
		Me.materialLabel1.Name = "materialLabel1"
		Me.materialLabel1.Size = New System.Drawing.Size(573, 64)
		Me.materialLabel1.TabIndex = 14
		Me.materialLabel1.Text = resources.GetString("materialLabel1.Text")
		' 
		' materialRadioButton3
		' 
		Me.materialRadioButton3.AutoSize = True
		Me.materialRadioButton3.Cursor = System.Windows.Forms.Cursors.[Default]
		Me.materialRadioButton3.Depth = 0
		Me.materialRadioButton3.Font = New System.Drawing.Font("Roboto", 10F)
		Me.materialRadioButton3.Location = New System.Drawing.Point(0, 68)
		Me.materialRadioButton3.Margin = New System.Windows.Forms.Padding(0)
		Me.materialRadioButton3.MouseLocation = New System.Drawing.Point(-1, -1)
		Me.materialRadioButton3.MouseState = MaterialSkin.MouseState.HOVER
		Me.materialRadioButton3.Name = "materialRadioButton3"
		Me.materialRadioButton3.Ripple = True
		Me.materialRadioButton3.Size = New System.Drawing.Size(163, 30)
		Me.materialRadioButton3.TabIndex = 11
		Me.materialRadioButton3.Text = "materialRadioButton3"
		Me.materialRadioButton3.UseVisualStyleBackColor = True
		' 
		' materialRadioButton2
		' 
		Me.materialRadioButton2.AutoSize = True
		Me.materialRadioButton2.Cursor = System.Windows.Forms.Cursors.[Default]
		Me.materialRadioButton2.Depth = 0
		Me.materialRadioButton2.Font = New System.Drawing.Font("Roboto", 10F)
		Me.materialRadioButton2.Location = New System.Drawing.Point(0, 38)
		Me.materialRadioButton2.Margin = New System.Windows.Forms.Padding(0)
		Me.materialRadioButton2.MouseLocation = New System.Drawing.Point(-1, -1)
		Me.materialRadioButton2.MouseState = MaterialSkin.MouseState.HOVER
		Me.materialRadioButton2.Name = "materialRadioButton2"
		Me.materialRadioButton2.Ripple = True
		Me.materialRadioButton2.Size = New System.Drawing.Size(163, 30)
		Me.materialRadioButton2.TabIndex = 10
		Me.materialRadioButton2.Text = "materialRadioButton2"
		Me.materialRadioButton2.UseVisualStyleBackColor = True
		' 
		' materialCheckbox4
		' 
		Me.materialCheckbox4.AutoSize = True
		Me.materialCheckbox4.Depth = 0
		Me.materialCheckbox4.Font = New System.Drawing.Font("Roboto", 10F)
		Me.materialCheckbox4.Location = New System.Drawing.Point(0, 98)
		Me.materialCheckbox4.Margin = New System.Windows.Forms.Padding(0)
		Me.materialCheckbox4.MouseLocation = New System.Drawing.Point(-1, -1)
		Me.materialCheckbox4.MouseState = MaterialSkin.MouseState.HOVER
		Me.materialCheckbox4.Name = "materialCheckbox4"
		Me.materialCheckbox4.Ripple = True
		Me.materialCheckbox4.Size = New System.Drawing.Size(149, 30)
		Me.materialCheckbox4.TabIndex = 7
		Me.materialCheckbox4.Text = "materialCheckbox4"
		Me.materialCheckbox4.UseVisualStyleBackColor = True
		' 
		' materialCheckbox3
		' 
		Me.materialCheckbox3.AutoSize = True
		Me.materialCheckbox3.Cursor = System.Windows.Forms.Cursors.[Default]
		Me.materialCheckbox3.Depth = 0
		Me.materialCheckbox3.Font = New System.Drawing.Font("Roboto", 10F)
		Me.materialCheckbox3.Location = New System.Drawing.Point(0, 68)
		Me.materialCheckbox3.Margin = New System.Windows.Forms.Padding(0)
		Me.materialCheckbox3.MouseLocation = New System.Drawing.Point(-1, -1)
		Me.materialCheckbox3.MouseState = MaterialSkin.MouseState.HOVER
		Me.materialCheckbox3.Name = "materialCheckbox3"
		Me.materialCheckbox3.Ripple = True
		Me.materialCheckbox3.Size = New System.Drawing.Size(149, 30)
		Me.materialCheckbox3.TabIndex = 6
		Me.materialCheckbox3.Text = "materialCheckbox3"
		Me.materialCheckbox3.UseVisualStyleBackColor = True
		' 
		' materialCheckbox2
		' 
		Me.materialCheckbox2.AutoSize = True
		Me.materialCheckbox2.Checked = True
		Me.materialCheckbox2.CheckState = System.Windows.Forms.CheckState.Checked
		Me.materialCheckbox2.Depth = 0
		Me.materialCheckbox2.Font = New System.Drawing.Font("Roboto", 10F)
		Me.materialCheckbox2.Location = New System.Drawing.Point(0, 38)
		Me.materialCheckbox2.Margin = New System.Windows.Forms.Padding(0)
		Me.materialCheckbox2.MouseLocation = New System.Drawing.Point(-1, -1)
		Me.materialCheckbox2.MouseState = MaterialSkin.MouseState.HOVER
		Me.materialCheckbox2.Name = "materialCheckbox2"
		Me.materialCheckbox2.Ripple = True
		Me.materialCheckbox2.Size = New System.Drawing.Size(149, 30)
		Me.materialCheckbox2.TabIndex = 5
		Me.materialCheckbox2.Text = "materialCheckbox2"
		Me.materialCheckbox2.UseVisualStyleBackColor = True
		' 
		' materialCheckbox1
		' 
		Me.materialCheckbox1.AutoSize = True
		Me.materialCheckbox1.Checked = True
		Me.materialCheckbox1.CheckState = System.Windows.Forms.CheckState.Checked
		Me.materialCheckbox1.Cursor = System.Windows.Forms.Cursors.[Default]
		Me.materialCheckbox1.Depth = 0
		Me.materialCheckbox1.Font = New System.Drawing.Font("Roboto", 10F)
		Me.materialCheckbox1.Location = New System.Drawing.Point(0, 8)
		Me.materialCheckbox1.Margin = New System.Windows.Forms.Padding(0)
		Me.materialCheckbox1.MouseLocation = New System.Drawing.Point(-1, -1)
		Me.materialCheckbox1.MouseState = MaterialSkin.MouseState.HOVER
		Me.materialCheckbox1.Name = "materialCheckbox1"
		Me.materialCheckbox1.Ripple = True
		Me.materialCheckbox1.Size = New System.Drawing.Size(149, 30)
		Me.materialCheckbox1.TabIndex = 4
		Me.materialCheckbox1.Text = "materialCheckbox1"
		Me.materialCheckbox1.UseVisualStyleBackColor = True
		' 
		' materialSingleLineTextField2
		' 
		Me.materialSingleLineTextField2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.materialSingleLineTextField2.Depth = 0
		Me.materialSingleLineTextField2.Hint = "Another example hint"
		Me.materialSingleLineTextField2.Location = New System.Drawing.Point(0, 51)
		Me.materialSingleLineTextField2.MaxLength = 32767
		Me.materialSingleLineTextField2.MouseState = MaterialSkin.MouseState.HOVER
		Me.materialSingleLineTextField2.Name = "materialSingleLineTextField2"
		Me.materialSingleLineTextField2.PasswordChar = ControlChars.NullChar
		Me.materialSingleLineTextField2.SelectedText = ""
		Me.materialSingleLineTextField2.SelectionLength = 0
		Me.materialSingleLineTextField2.SelectionStart = 0
		Me.materialSingleLineTextField2.Size = New System.Drawing.Size(569, 23)
		Me.materialSingleLineTextField2.TabIndex = 3
		Me.materialSingleLineTextField2.TabStop = False
		Me.materialSingleLineTextField2.UseSystemPasswordChar = False
		' 
		' materialSingleLineTextField1
		' 
		Me.materialSingleLineTextField1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.materialSingleLineTextField1.Depth = 0
		Me.materialSingleLineTextField1.Hint = "This is a hint"
		Me.materialSingleLineTextField1.Location = New System.Drawing.Point(0, 14)
		Me.materialSingleLineTextField1.MaxLength = 32767
		Me.materialSingleLineTextField1.MouseState = MaterialSkin.MouseState.HOVER
		Me.materialSingleLineTextField1.Name = "materialSingleLineTextField1"
		Me.materialSingleLineTextField1.PasswordChar = ControlChars.NullChar
		Me.materialSingleLineTextField1.SelectedText = ""
		Me.materialSingleLineTextField1.SelectionLength = 0
		Me.materialSingleLineTextField1.SelectionStart = 0
		Me.materialSingleLineTextField1.Size = New System.Drawing.Size(569, 23)
		Me.materialSingleLineTextField1.TabIndex = 2
		Me.materialSingleLineTextField1.TabStop = False
		Me.materialSingleLineTextField1.UseSystemPasswordChar = False
		' 
		' materialButton1
		' 
		Me.materialButton1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.materialButton1.Depth = 0
		Me.materialButton1.Location = New System.Drawing.Point(249, 187)
		Me.materialButton1.MouseState = MaterialSkin.MouseState.HOVER
		Me.materialButton1.Name = "materialButton1"
		Me.materialButton1.Primary = True
		Me.materialButton1.Size = New System.Drawing.Size(135, 36)
		Me.materialButton1.TabIndex = 0
		Me.materialButton1.Text = "Change Theme"
		Me.materialButton1.UseVisualStyleBackColor = True
		AddHandler Me.materialButton1.Click, New System.EventHandler(AddressOf Me.materialButton1_Click)
		' 
		' materialRadioButton1
		' 
		Me.materialRadioButton1.AutoSize = True
		Me.materialRadioButton1.Cursor = System.Windows.Forms.Cursors.[Default]
		Me.materialRadioButton1.Depth = 0
		Me.materialRadioButton1.Font = New System.Drawing.Font("Roboto", 10F)
		Me.materialRadioButton1.Location = New System.Drawing.Point(0, 8)
		Me.materialRadioButton1.Margin = New System.Windows.Forms.Padding(0)
		Me.materialRadioButton1.MouseLocation = New System.Drawing.Point(-1, -1)
		Me.materialRadioButton1.MouseState = MaterialSkin.MouseState.HOVER
		Me.materialRadioButton1.Name = "materialRadioButton1"
		Me.materialRadioButton1.Ripple = True
		Me.materialRadioButton1.Size = New System.Drawing.Size(163, 30)
		Me.materialRadioButton1.TabIndex = 9
		Me.materialRadioButton1.Text = "materialRadioButton1"
		Me.materialRadioButton1.UseVisualStyleBackColor = True
		' 
		' materialTabSelector1
		' 
		Me.materialTabSelector1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.materialTabSelector1.BaseTabControl = Me.materialTabControl1
		Me.materialTabSelector1.Depth = 0
		Me.materialTabSelector1.Location = New System.Drawing.Point(0, 64)
		Me.materialTabSelector1.MouseState = MaterialSkin.MouseState.HOVER
		Me.materialTabSelector1.Name = "materialTabSelector1"
		Me.materialTabSelector1.Size = New System.Drawing.Size(618, 48)
		Me.materialTabSelector1.TabIndex = 17
		Me.materialTabSelector1.Text = "materialTabSelector1"
		' 
		' materialTabControl1
		' 
		Me.materialTabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.materialTabControl1.Controls.Add(Me.tabPage1)
		Me.materialTabControl1.Controls.Add(Me.tabPage2)
		Me.materialTabControl1.Controls.Add(Me.tabPage3)
		Me.materialTabControl1.Controls.Add(Me.tabPage4)
		Me.materialTabControl1.Controls.Add(Me.tabPage5)
		Me.materialTabControl1.Depth = 0
		Me.materialTabControl1.Location = New System.Drawing.Point(14, 111)
		Me.materialTabControl1.MouseState = MaterialSkin.MouseState.HOVER
		Me.materialTabControl1.Name = "materialTabControl1"
		Me.materialTabControl1.SelectedIndex = 0
		Me.materialTabControl1.Size = New System.Drawing.Size(577, 256)
		Me.materialTabControl1.TabIndex = 18
		' 
		' tabPage1
		' 
		Me.tabPage1.BackColor = System.Drawing.Color.White
		Me.tabPage1.Controls.Add(Me.materialSingleLineTextField3)
		Me.tabPage1.Controls.Add(Me.materialRaisedButton1)
		Me.tabPage1.Controls.Add(Me.materialSingleLineTextField1)
		Me.tabPage1.Controls.Add(Me.materialSingleLineTextField2)
		Me.tabPage1.Controls.Add(Me.materialButton1)
		Me.tabPage1.Controls.Add(Me.materialLabel1)
		Me.tabPage1.Location = New System.Drawing.Point(4, 22)
		Me.tabPage1.Name = "tabPage1"
		Me.tabPage1.Padding = New System.Windows.Forms.Padding(3)
		Me.tabPage1.Size = New System.Drawing.Size(569, 230)
		Me.tabPage1.TabIndex = 0
		Me.tabPage1.Text = "tabPage1"
		' 
		' materialSingleLineTextField3
		' 
		Me.materialSingleLineTextField3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.materialSingleLineTextField3.Depth = 0
		Me.materialSingleLineTextField3.Hint = "This is a password"
		Me.materialSingleLineTextField3.Location = New System.Drawing.Point(0, 88)
		Me.materialSingleLineTextField3.MaxLength = 32767
		Me.materialSingleLineTextField3.MouseState = MaterialSkin.MouseState.HOVER
		Me.materialSingleLineTextField3.Name = "materialSingleLineTextField3"
		Me.materialSingleLineTextField3.PasswordChar = ControlChars.NullChar
		Me.materialSingleLineTextField3.SelectedText = ""
		Me.materialSingleLineTextField3.SelectionLength = 0
		Me.materialSingleLineTextField3.SelectionStart = 0
		Me.materialSingleLineTextField3.Size = New System.Drawing.Size(569, 23)
		Me.materialSingleLineTextField3.TabIndex = 4
		Me.materialSingleLineTextField3.TabStop = False
		Me.materialSingleLineTextField3.UseSystemPasswordChar = True
		' 
		' materialRaisedButton1
		' 
		Me.materialRaisedButton1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.materialRaisedButton1.Depth = 0
		Me.materialRaisedButton1.Location = New System.Drawing.Point(390, 187)
		Me.materialRaisedButton1.MouseState = MaterialSkin.MouseState.HOVER
		Me.materialRaisedButton1.Name = "materialRaisedButton1"
		Me.materialRaisedButton1.Primary = True
		Me.materialRaisedButton1.Size = New System.Drawing.Size(179, 36)
		Me.materialRaisedButton1.TabIndex = 21
		Me.materialRaisedButton1.Text = "Change color scheme"
		Me.materialRaisedButton1.UseVisualStyleBackColor = True
		AddHandler Me.materialRaisedButton1.Click, New System.EventHandler(AddressOf Me.materialRaisedButton1_Click)
		' 
		' tabPage2
		' 
		Me.tabPage2.BackColor = System.Drawing.Color.White
		Me.tabPage2.Controls.Add(Me.materialCheckBox6)
		Me.tabPage2.Controls.Add(Me.materialCheckBox5)
		Me.tabPage2.Controls.Add(Me.materialCheckbox3)
		Me.tabPage2.Controls.Add(Me.materialCheckbox1)
		Me.tabPage2.Controls.Add(Me.materialCheckbox2)
		Me.tabPage2.Controls.Add(Me.materialCheckbox4)
		Me.tabPage2.Location = New System.Drawing.Point(4, 22)
		Me.tabPage2.Name = "tabPage2"
		Me.tabPage2.Padding = New System.Windows.Forms.Padding(3)
		Me.tabPage2.Size = New System.Drawing.Size(569, 230)
		Me.tabPage2.TabIndex = 1
		Me.tabPage2.Text = "tabPage2"
		' 
		' materialCheckBox6
		' 
		Me.materialCheckBox6.AutoSize = True
		Me.materialCheckBox6.Cursor = System.Windows.Forms.Cursors.[Default]
		Me.materialCheckBox6.Depth = 0
		Me.materialCheckBox6.Enabled = False
		Me.materialCheckBox6.Font = New System.Drawing.Font("Roboto", 10F)
		Me.materialCheckBox6.Location = New System.Drawing.Point(0, 158)
		Me.materialCheckBox6.Margin = New System.Windows.Forms.Padding(0)
		Me.materialCheckBox6.MouseLocation = New System.Drawing.Point(-1, -1)
		Me.materialCheckBox6.MouseState = MaterialSkin.MouseState.HOVER
		Me.materialCheckBox6.Name = "materialCheckBox6"
		Me.materialCheckBox6.Ripple = True
		Me.materialCheckBox6.Size = New System.Drawing.Size(150, 30)
		Me.materialCheckBox6.TabIndex = 9
		Me.materialCheckBox6.Text = "materialCheckBox6"
		Me.materialCheckBox6.UseVisualStyleBackColor = True
		' 
		' materialCheckBox5
		' 
		Me.materialCheckBox5.AutoSize = True
		Me.materialCheckBox5.Checked = True
		Me.materialCheckBox5.CheckState = System.Windows.Forms.CheckState.Checked
		Me.materialCheckBox5.Cursor = System.Windows.Forms.Cursors.[Default]
		Me.materialCheckBox5.Depth = 0
		Me.materialCheckBox5.Enabled = False
		Me.materialCheckBox5.Font = New System.Drawing.Font("Roboto", 10F)
		Me.materialCheckBox5.Location = New System.Drawing.Point(0, 128)
		Me.materialCheckBox5.Margin = New System.Windows.Forms.Padding(0)
		Me.materialCheckBox5.MouseLocation = New System.Drawing.Point(-1, -1)
		Me.materialCheckBox5.MouseState = MaterialSkin.MouseState.HOVER
		Me.materialCheckBox5.Name = "materialCheckBox5"
		Me.materialCheckBox5.Ripple = True
		Me.materialCheckBox5.Size = New System.Drawing.Size(150, 30)
		Me.materialCheckBox5.TabIndex = 8
		Me.materialCheckBox5.Text = "materialCheckBox5"
		Me.materialCheckBox5.UseVisualStyleBackColor = True
		' 
		' tabPage3
		' 
		Me.tabPage3.BackColor = System.Drawing.Color.White
		Me.tabPage3.Controls.Add(Me.materialRadioButton4)
		Me.tabPage3.Controls.Add(Me.materialRadioButton1)
		Me.tabPage3.Controls.Add(Me.materialRadioButton2)
		Me.tabPage3.Controls.Add(Me.materialRadioButton3)
		Me.tabPage3.Location = New System.Drawing.Point(4, 22)
		Me.tabPage3.Name = "tabPage3"
		Me.tabPage3.Padding = New System.Windows.Forms.Padding(3)
		Me.tabPage3.Size = New System.Drawing.Size(569, 230)
		Me.tabPage3.TabIndex = 2
		Me.tabPage3.Text = "MaterialTabPage3"
		' 
		' tabPage4
		' 
		Me.tabPage4.Controls.Add(Me.materialListView1)
		Me.tabPage4.Location = New System.Drawing.Point(4, 22)
		Me.tabPage4.Name = "tabPage4"
		Me.tabPage4.Padding = New System.Windows.Forms.Padding(3)
		Me.tabPage4.Size = New System.Drawing.Size(569, 230)
		Me.tabPage4.TabIndex = 3
		Me.tabPage4.Text = "ListView"
		Me.tabPage4.UseVisualStyleBackColor = True
		' 
		' materialListView1
		' 
		Me.materialListView1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.materialListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.columnHeader1, Me.columnHeader2, Me.columnHeader3, Me.columnHeader4})
		Me.materialListView1.Depth = 0
		Me.materialListView1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.materialListView1.Font = New System.Drawing.Font("Roboto", 24F)
		Me.materialListView1.FullRowSelect = True
		Me.materialListView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
		Me.materialListView1.Location = New System.Drawing.Point(3, 3)
		Me.materialListView1.MouseLocation = New System.Drawing.Point(0, 0)
		Me.materialListView1.MouseState = MaterialSkin.MouseState.HOVER
		Me.materialListView1.Name = "materialListView1"
		Me.materialListView1.OwnerDraw = True
		Me.materialListView1.Size = New System.Drawing.Size(563, 224)
		Me.materialListView1.TabIndex = 0
		Me.materialListView1.UseCompatibleStateImageBehavior = False
		Me.materialListView1.View = System.Windows.Forms.View.Details
		' 
		' columnHeader1
		' 
		Me.columnHeader1.Text = "Dessert (100g serving)"
		Me.columnHeader1.Width = 200
		' 
		' columnHeader2
		' 
		Me.columnHeader2.Text = "Calories"
		Me.columnHeader2.Width = 101
		' 
		' columnHeader3
		' 
		Me.columnHeader3.Text = "Fat (g)"
		Me.columnHeader3.Width = 94
		' 
		' columnHeader4
		' 
		Me.columnHeader4.Text = "Protein (g)"
		Me.columnHeader4.Width = 154
		' 
		' tabPage5
		' 
		Me.tabPage5.Controls.Add(Me.materialProgressBar1)
		Me.tabPage5.Location = New System.Drawing.Point(4, 22)
		Me.tabPage5.Name = "tabPage5"
		Me.tabPage5.Padding = New System.Windows.Forms.Padding(3)
		Me.tabPage5.Size = New System.Drawing.Size(569, 230)
		Me.tabPage5.TabIndex = 4
		Me.tabPage5.Text = "tabPage5"
		Me.tabPage5.UseVisualStyleBackColor = True
		' 
		' materialContextMenuStrip1
		' 
		Me.materialContextMenuStrip1.BackColor = System.Drawing.Color.White
		Me.materialContextMenuStrip1.Depth = 0
		Me.materialContextMenuStrip1.Font = New System.Drawing.Font("Microsoft Sans Serif", 11F)
		Me.materialContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.item1ToolStripMenuItem, Me.disabledItemToolStripMenuItem, Me.item2ToolStripMenuItem, Me.toolStripSeparator1, Me.item3ToolStripMenuItem})
		Me.materialContextMenuStrip1.Margin = New System.Windows.Forms.Padding(16, 8, 16, 8)
		Me.materialContextMenuStrip1.MouseState = MaterialSkin.MouseState.HOVER
		Me.materialContextMenuStrip1.Name = "materialContextMenuStrip1"
		Me.materialContextMenuStrip1.Size = New System.Drawing.Size(166, 130)
		' 
		' item1ToolStripMenuItem
		' 
		Me.item1ToolStripMenuItem.AutoSize = False
		Me.item1ToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.subItem1ToolStripMenuItem, Me.subItem2ToolStripMenuItem})
		Me.item1ToolStripMenuItem.Name = "item1ToolStripMenuItem"
		Me.item1ToolStripMenuItem.Size = New System.Drawing.Size(170, 30)
		Me.item1ToolStripMenuItem.Text = "Item 1"
		' 
		' subItem1ToolStripMenuItem
		' 
		Me.subItem1ToolStripMenuItem.AutoSize = False
		Me.subItem1ToolStripMenuItem.Name = "subItem1ToolStripMenuItem"
		Me.subItem1ToolStripMenuItem.Size = New System.Drawing.Size(152, 30)
		Me.subItem1ToolStripMenuItem.Text = "SubItem 1"
		' 
		' subItem2ToolStripMenuItem
		' 
		Me.subItem2ToolStripMenuItem.AutoSize = False
		Me.subItem2ToolStripMenuItem.Name = "subItem2ToolStripMenuItem"
		Me.subItem2ToolStripMenuItem.Size = New System.Drawing.Size(152, 30)
		Me.subItem2ToolStripMenuItem.Text = "SubItem 2"
		' 
		' disabledItemToolStripMenuItem
		' 
		Me.disabledItemToolStripMenuItem.AutoSize = False
		Me.disabledItemToolStripMenuItem.Enabled = False
		Me.disabledItemToolStripMenuItem.Name = "disabledItemToolStripMenuItem"
		Me.disabledItemToolStripMenuItem.Size = New System.Drawing.Size(170, 30)
		Me.disabledItemToolStripMenuItem.Text = "Disabled item"
		' 
		' item2ToolStripMenuItem
		' 
		Me.item2ToolStripMenuItem.AutoSize = False
		Me.item2ToolStripMenuItem.Name = "item2ToolStripMenuItem"
		Me.item2ToolStripMenuItem.Size = New System.Drawing.Size(170, 30)
		Me.item2ToolStripMenuItem.Text = "Item 2"
		' 
		' toolStripSeparator1
		' 
		Me.toolStripSeparator1.Name = "toolStripSeparator1"
		Me.toolStripSeparator1.Size = New System.Drawing.Size(162, 6)
		' 
		' item3ToolStripMenuItem
		' 
		Me.item3ToolStripMenuItem.AutoSize = False
		Me.item3ToolStripMenuItem.Name = "item3ToolStripMenuItem"
		Me.item3ToolStripMenuItem.Size = New System.Drawing.Size(170, 30)
		Me.item3ToolStripMenuItem.Text = "Item 3"
		' 
		' materialProgressBar1
		' 
		Me.materialProgressBar1.Depth = 0
		Me.materialProgressBar1.Location = New System.Drawing.Point(16, 208)
		Me.materialProgressBar1.MouseState = MaterialSkin.MouseState.HOVER
		Me.materialProgressBar1.Name = "materialProgressBar1"
		Me.materialProgressBar1.Size = New System.Drawing.Size(534, 5)
		Me.materialProgressBar1.TabIndex = 0
		Me.materialProgressBar1.Value = 45
		' 
		' MainForm
		' 
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.BackColor = System.Drawing.Color.White
		Me.ClientSize = New System.Drawing.Size(610, 429)
		Me.ContextMenuStrip = Me.materialContextMenuStrip1
		Me.Controls.Add(Me.materialFlatButton3)
		Me.Controls.Add(Me.materialFlatButton2)
		Me.Controls.Add(Me.materialTabSelector1)
		Me.Controls.Add(Me.materialTabControl1)
		Me.Controls.Add(Me.materialDivider1)
		Me.Controls.Add(Me.materialFlatButton1)
		Me.Name = "MainForm"
		Me.Text = "MaterialSkin Demo"
		Me.materialTabControl1.ResumeLayout(False)
		Me.tabPage1.ResumeLayout(False)
		Me.tabPage2.ResumeLayout(False)
		Me.tabPage2.PerformLayout()
		Me.tabPage3.ResumeLayout(False)
		Me.tabPage3.PerformLayout()
		Me.tabPage4.ResumeLayout(False)
		Me.tabPage5.ResumeLayout(False)
		Me.materialContextMenuStrip1.ResumeLayout(False)
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub

	#End Region

	Private materialButton1 As MaterialSkin.Controls.MaterialRaisedButton
	Private materialFlatButton1 As MaterialSkin.Controls.MaterialFlatButton
	Private materialSingleLineTextField1 As MaterialSkin.Controls.MaterialSingleLineTextField
	Private materialSingleLineTextField2 As MaterialSkin.Controls.MaterialSingleLineTextField
	Private materialCheckbox1 As MaterialSkin.Controls.MaterialCheckBox
	Private materialCheckbox2 As MaterialSkin.Controls.MaterialCheckBox
	Private materialCheckbox3 As MaterialSkin.Controls.MaterialCheckBox
	Private materialCheckbox4 As MaterialSkin.Controls.MaterialCheckBox
	Private materialRadioButton1 As MaterialSkin.Controls.MaterialRadioButton
	Private materialRadioButton2 As MaterialRadioButton
	Private materialRadioButton3 As MaterialRadioButton
	Private materialFlatButton2 As MaterialFlatButton
	Private materialLabel1 As MaterialLabel
	Private materialRadioButton4 As MaterialRadioButton
	Private materialDivider1 As MaterialDivider
	Private materialTabSelector1 As MaterialTabSelector
	Private materialTabControl1 As MaterialTabControl
	Private tabPage1 As System.Windows.Forms.TabPage
	Private tabPage2 As System.Windows.Forms.TabPage
	Private tabPage3 As System.Windows.Forms.TabPage
	Private materialCheckBox5 As MaterialCheckBox
	Private materialContextMenuStrip1 As MaterialContextMenuStrip
	Private item1ToolStripMenuItem As MaterialSkin.Controls.MaterialToolStripMenuItem
	Private subItem1ToolStripMenuItem As MaterialSkin.Controls.MaterialToolStripMenuItem
	Private subItem2ToolStripMenuItem As MaterialSkin.Controls.MaterialToolStripMenuItem
	Private item2ToolStripMenuItem As MaterialSkin.Controls.MaterialToolStripMenuItem
	Private item3ToolStripMenuItem As MaterialSkin.Controls.MaterialToolStripMenuItem
	Private toolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
	Private disabledItemToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
	Private materialCheckBox6 As MaterialCheckBox
	Private materialRaisedButton1 As MaterialRaisedButton
	Private materialFlatButton3 As MaterialFlatButton
	Private materialSingleLineTextField3 As MaterialSingleLineTextField
	Private tabPage4 As TabPage
	Private materialListView1 As MaterialListView
	Private columnHeader1 As ColumnHeader
	Private columnHeader2 As ColumnHeader
	Private columnHeader3 As ColumnHeader
	Private columnHeader4 As ColumnHeader
	Private tabPage5 As TabPage
	Private materialProgressBar1 As MaterialProgressBar
End Class
