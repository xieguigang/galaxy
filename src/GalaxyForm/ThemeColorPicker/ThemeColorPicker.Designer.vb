Namespace ThemeColorPicker
    Partial Class ThemeColorPicker
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

#Region "Component Designer generated code"

        ''' <summary> 
        ''' Required method for Designer support - do not modify 
        ''' the contents of this method with the code editor.
        ''' </summary>
        Private Sub InitializeComponent()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ThemeColorPicker))
            p01 = New Panel()
            p02 = New Panel()
            p03 = New Panel()
            p04 = New Panel()
            p05 = New Panel()
            p06 = New Panel()
            p07 = New Panel()
            pnMoreColor = New Panel()
            SuspendLayout()
            ' 
            ' p01
            ' 
            p01.BackColor = Drawing.Color.White
            p01.Cursor = Cursors.Hand
            p01.Location = New Drawing.Point(4, 20)
            p01.Margin = New Padding(0)
            p01.Name = "p01"
            p01.Size = New Drawing.Size(13, 13)
            p01.TabIndex = 0
            AddHandler p01.MouseClick, New MouseEventHandler(AddressOf p_MouseClick)
            ' 
            ' p02
            ' 
            p02.BackColor = Drawing.Color.Transparent
            p02.Cursor = Cursors.Hand
            p02.Location = New Drawing.Point(4, 39)
            p02.Margin = New Padding(0)
            p02.Name = "p02"
            p02.Size = New Drawing.Size(13, 13)
            p02.TabIndex = 1
            AddHandler p02.MouseClick, New MouseEventHandler(AddressOf p_MouseClick)
            ' 
            ' p03
            ' 
            p03.BackColor = Drawing.Color.Transparent
            p03.Cursor = Cursors.Hand
            p03.Location = New Drawing.Point(4, 52)
            p03.Margin = New Padding(0)
            p03.Name = "p03"
            p03.Size = New Drawing.Size(13, 13)
            p03.TabIndex = 2
            AddHandler p03.MouseClick, New MouseEventHandler(AddressOf p_MouseClick)
            ' 
            ' p04
            ' 
            p04.BackColor = Drawing.Color.Transparent
            p04.Cursor = Cursors.Hand
            p04.Location = New Drawing.Point(4, 65)
            p04.Margin = New Padding(0)
            p04.Name = "p04"
            p04.Size = New Drawing.Size(13, 13)
            p04.TabIndex = 3
            AddHandler p04.MouseClick, New MouseEventHandler(AddressOf p_MouseClick)
            ' 
            ' p05
            ' 
            p05.BackColor = Drawing.Color.Transparent
            p05.Cursor = Cursors.Hand
            p05.Location = New Drawing.Point(4, 77)
            p05.Margin = New Padding(0)
            p05.Name = "p05"
            p05.Size = New Drawing.Size(13, 13)
            p05.TabIndex = 4
            AddHandler p05.MouseClick, New MouseEventHandler(AddressOf p_MouseClick)
            ' 
            ' p06
            ' 
            p06.BackColor = Drawing.Color.Transparent
            p06.Cursor = Cursors.Hand
            p06.Location = New Drawing.Point(4, 89)
            p06.Margin = New Padding(0)
            p06.Name = "p06"
            p06.Size = New Drawing.Size(13, 13)
            p06.TabIndex = 5
            AddHandler p06.MouseClick, New MouseEventHandler(AddressOf p_MouseClick)
            ' 
            ' p07
            ' 
            p07.BackColor = Drawing.Color.Transparent
            p07.Cursor = Cursors.Hand
            p07.Location = New Drawing.Point(4, 126)
            p07.Margin = New Padding(0)
            p07.Name = "p07"
            p07.Size = New Drawing.Size(13, 13)
            p07.TabIndex = 6
            AddHandler p07.MouseClick, New MouseEventHandler(AddressOf p_MouseClick)
            ' 
            ' pnMoreColor
            ' 
            pnMoreColor.BackColor = Drawing.Color.Transparent
            pnMoreColor.Cursor = Cursors.Hand
            pnMoreColor.Location = New Drawing.Point(4, 144)
            pnMoreColor.Name = "pnMoreColor"
            pnMoreColor.Size = New Drawing.Size(167, 19)
            pnMoreColor.TabIndex = 8
            AddHandler pnMoreColor.MouseClick, New MouseEventHandler(AddressOf pnMoreColor_MouseClick)
            ' 
            ' ThemeColorPicker
            ' 
            AutoScaleMode = AutoScaleMode.None
            BackgroundImage = CType(resources.GetObject("$this.BackgroundImage"), Drawing.Image)
            BackgroundImageLayout = ImageLayout.None
            Controls.Add(pnMoreColor)
            Controls.Add(p07)
            Controls.Add(p06)
            Controls.Add(p05)
            Controls.Add(p04)
            Controls.Add(p03)
            Controls.Add(p02)
            Controls.Add(p01)
            Name = "ThemeColorPicker"
            Size = New Drawing.Size(174, 166)
            ResumeLayout(False)

        End Sub

#End Region

        Private p01 As Panel
        Private p02 As Panel
        Private p03 As Panel
        Private p04 As Panel
        Private p05 As Panel
        Private p06 As Panel
        Private p07 As Panel
        Private pnMoreColor As Panel
    End Class
End Namespace
