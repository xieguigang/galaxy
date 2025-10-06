
Imports System.ComponentModel
Imports System.Windows.Forms

Namespace Zuby.ADGV
    Partial Class AdvancedDataGridViewSearchToolBar
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
            button_close = New ToolStripButton()
            label_search = New ToolStripLabel()
            comboBox_columns = New ToolStripComboBox()
            textBox_search = New ToolStripTextBox()
            button_frombegin = New ToolStripButton()
            button_casesensitive = New ToolStripButton()
            button_search = New ToolStripButton()
            button_wholeword = New ToolStripButton()
            separator_search = New ToolStripSeparator()
            SuspendLayout()
            ' 
            ' button_close
            ' 
            button_close.DisplayStyle = ToolStripItemDisplayStyle.Image
            button_close.Image = My.Resources.Resources.SearchToolBar_ButtonCaseSensitive
            button_close.ImageScaling = ToolStripItemImageScaling.None
            button_close.ImageTransparentColor = Drawing.Color.Magenta
            button_close.Name = "button_close"
            button_close.Overflow = ToolStripItemOverflow.Never
            button_close.Size = New Drawing.Size(23, 24)
            AddHandler button_close.Click, New EventHandler(AddressOf button_close_Click)
            ' 
            ' label_search
            ' 
            label_search.Name = "label_search"
            label_search.Size = New Drawing.Size(45, 15)

            ' 
            ' comboBox_columns
            ' 
            comboBox_columns.AutoSize = False
            comboBox_columns.AutoToolTip = True
            comboBox_columns.DropDownStyle = ComboBoxStyle.DropDownList
            comboBox_columns.FlatStyle = FlatStyle.Standard
            comboBox_columns.IntegralHeight = False
            comboBox_columns.Margin = New Padding(0, 2, 8, 2)
            comboBox_columns.MaxDropDownItems = 12
            comboBox_columns.Name = "comboBox_columns"
            comboBox_columns.Size = New Drawing.Size(150, 23)
            ' 
            ' textBox_search
            ' 
            textBox_search.AutoSize = False
            textBox_search.ForeColor = Drawing.Color.LightGray
            textBox_search.Margin = New Padding(0, 2, 8, 2)
            textBox_search.Name = "textBox_search"
            textBox_search.Overflow = ToolStripItemOverflow.Never
            textBox_search.Size = New Drawing.Size(100, 23)
            AddHandler textBox_search.Enter, New EventHandler(AddressOf textBox_search_Enter)
            AddHandler textBox_search.Leave, New EventHandler(AddressOf textBox_search_Leave)
            AddHandler textBox_search.KeyDown, New KeyEventHandler(AddressOf textBox_search_KeyDown)
            AddHandler textBox_search.TextChanged, New EventHandler(AddressOf textBox_search_TextChanged)
            ' 
            ' button_frombegin
            ' 
            button_frombegin.CheckOnClick = True
            button_frombegin.DisplayStyle = ToolStripItemDisplayStyle.Image
            button_frombegin.Image = My.Resources.Resources.SearchToolBar_ButtonFromBegin
            button_frombegin.ImageScaling = ToolStripItemImageScaling.None
            button_frombegin.ImageTransparentColor = Drawing.Color.Magenta
            button_frombegin.Name = "button_frombegin"
            button_frombegin.Size = New Drawing.Size(23, 20)
            ' 
            ' button_casesensitive
            ' 
            button_casesensitive.CheckOnClick = True
            button_casesensitive.DisplayStyle = ToolStripItemDisplayStyle.Image
            button_casesensitive.Image = My.Resources.Resources.SearchToolBar_ButtonCaseSensitive
            button_casesensitive.ImageScaling = ToolStripItemImageScaling.None
            button_casesensitive.ImageTransparentColor = Drawing.Color.Magenta
            button_casesensitive.Name = "button_casesensitive"
            button_casesensitive.Size = New Drawing.Size(23, 20)
            ' 
            ' button_search
            ' 
            button_search.DisplayStyle = ToolStripItemDisplayStyle.Image
            button_search.Image = My.Resources.Resources.SearchToolBar_ButtonSearch
            button_search.ImageScaling = ToolStripItemImageScaling.None
            button_search.ImageTransparentColor = Drawing.Color.Magenta
            button_search.Name = "button_search"
            button_search.Overflow = ToolStripItemOverflow.Never
            button_search.Size = New Drawing.Size(23, 24)
            AddHandler button_search.Click, New EventHandler(AddressOf button_search_Click)
            ' 
            ' button_wholeword
            ' 
            button_wholeword.CheckOnClick = True
            button_wholeword.DisplayStyle = ToolStripItemDisplayStyle.Image
            button_wholeword.Image = My.Resources.Resources.SearchToolBar_ButtonWholeWord
            button_wholeword.ImageScaling = ToolStripItemImageScaling.None
            button_wholeword.ImageTransparentColor = Drawing.Color.Magenta
            button_wholeword.Margin = New Padding(1, 1, 1, 2)
            button_wholeword.Name = "button_wholeword"
            button_wholeword.Size = New Drawing.Size(23, 20)
            ' 
            ' separator_search
            ' 
            separator_search.AutoSize = False
            separator_search.Name = "separator_search"
            separator_search.Size = New Drawing.Size(10, 25)
            ' 
            ' AdvancedDataGridViewSearchToolBar
            ' 
            AllowMerge = False
            GripStyle = ToolStripGripStyle.Hidden
            Items.AddRange(New ToolStripItem() {button_close, label_search, comboBox_columns, textBox_search, button_frombegin, button_wholeword, button_casesensitive, separator_search, button_search})
            MaximumSize = New Drawing.Size(0, 27)
            MinimumSize = New Drawing.Size(0, 27)
            RenderMode = ToolStripRenderMode.Professional
            Size = New Drawing.Size(0, 27)
            AddHandler Resize, New EventHandler(AddressOf ResizeMe)
            ResumeLayout(False)
            PerformLayout()

        End Sub

#End Region

        Private button_close As ToolStripButton
        Private label_search As ToolStripLabel
        Private comboBox_columns As ToolStripComboBox
        Private textBox_search As ToolStripTextBox
        Private button_frombegin As ToolStripButton
        Private button_casesensitive As ToolStripButton
        Private button_search As ToolStripButton
        Private button_wholeword As ToolStripButton
        Private separator_search As ToolStripSeparator
    End Class
End Namespace
