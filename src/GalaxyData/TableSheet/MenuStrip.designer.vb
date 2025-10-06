
Imports System.ComponentModel
Imports System.Windows.Forms

Namespace Zuby.ADGV
    Partial Class MenuStrip
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
            sortASCMenuItem = New ToolStripMenuItem()
            sortDESCMenuItem = New ToolStripMenuItem()
            cancelSortMenuItem = New ToolStripMenuItem()
            toolStripSeparator1MenuItem = New ToolStripSeparator()
            cancelFilterMenuItem = New ToolStripMenuItem()
            customFilterLastFiltersListMenuItem = New ToolStripMenuItem()
            customFilterMenuItem = New ToolStripMenuItem()
            toolStripSeparator2MenuItem = New ToolStripSeparator()
            customFilterLastFilter1MenuItem = New ToolStripMenuItem()
            customFilterLastFilter2MenuItem = New ToolStripMenuItem()
            customFilterLastFilter3MenuItem = New ToolStripMenuItem()
            customFilterLastFilter4MenuItem = New ToolStripMenuItem()
            customFilterLastFilter5MenuItem = New ToolStripMenuItem()
            toolStripSeparator3MenuItem = New ToolStripSeparator()
            checkList = New TreeView()
            button_filter = New Button()
            button_undofilter = New Button()
            checkFilterListPanel = New Panel()
            checkFilterListButtonsPanel = New Panel()
            checkFilterListButtonsControlHost = New ToolStripControlHost(checkFilterListButtonsPanel)
            checkFilterListControlHost = New ToolStripControlHost(checkFilterListPanel)
            checkTextFilter = New TextBox()
            checkTextFilterControlHost = New ToolStripControlHost(checkTextFilter)
            resizeBoxControlHost = New ToolStripControlHost(New Control())
            SuspendLayout()
            '
            ' MenuStrip
            '
            BackColor = Drawing.SystemColors.ControlLightLight
            AutoSize = False
            Padding = New Padding(0)
            Margin = New Padding(0)
            Size = New Drawing.Size(287, 370)
            AddHandler Closed, New ToolStripDropDownClosedEventHandler(AddressOf MenuStrip_Closed)
            AddHandler LostFocus, New EventHandler(AddressOf MenuStrip_LostFocus)
            Items.AddRange(New ToolStripItem() {sortASCMenuItem, sortDESCMenuItem, cancelSortMenuItem, toolStripSeparator1MenuItem, cancelFilterMenuItem, customFilterLastFiltersListMenuItem, toolStripSeparator3MenuItem, checkTextFilterControlHost, checkFilterListControlHost, checkFilterListButtonsControlHost, resizeBoxControlHost})
            '
            ' sortASCMenuItem
            '
            sortASCMenuItem.Name = "sortASCMenuItem"
            sortASCMenuItem.AutoSize = False
            sortASCMenuItem.Size = New Drawing.Size(Width - 1, 22)
            AddHandler sortASCMenuItem.Click, New EventHandler(AddressOf SortASCMenuItem_Click)
            AddHandler sortASCMenuItem.MouseEnter, New EventHandler(AddressOf SortASCMenuItem_MouseEnter)
            sortASCMenuItem.ImageScaling = ToolStripItemImageScaling.None
            '
            ' sortDESCMenuItem
            '
            sortDESCMenuItem.Name = "sortDESCMenuItem"
            sortDESCMenuItem.AutoSize = False
            sortDESCMenuItem.Size = New Drawing.Size(Width - 1, 22)
            AddHandler sortDESCMenuItem.Click, New EventHandler(AddressOf SortDESCMenuItem_Click)
            AddHandler sortDESCMenuItem.MouseEnter, New EventHandler(AddressOf SortDESCMenuItem_MouseEnter)
            sortDESCMenuItem.ImageScaling = ToolStripItemImageScaling.None
            '
            ' cancelSortMenuItem
            '
            cancelSortMenuItem.Name = "cancelSortMenuItem"
            cancelSortMenuItem.Enabled = False
            cancelSortMenuItem.AutoSize = False
            cancelSortMenuItem.Size = New Drawing.Size(Width - 1, 22)
            cancelSortMenuItem.Text = "Clear Sort"
            AddHandler cancelSortMenuItem.Click, New EventHandler(AddressOf CancelSortMenuItem_Click)
            AddHandler cancelSortMenuItem.MouseEnter, New EventHandler(AddressOf CancelSortMenuItem_MouseEnter)
            '
            ' toolStripSeparator1MenuItem
            '
            toolStripSeparator1MenuItem.Name = "toolStripSeparator1MenuItem"
            toolStripSeparator1MenuItem.Size = New Drawing.Size(Width - 4, 6)
            '
            ' cancelFilterMenuItem
            '
            cancelFilterMenuItem.Name = "cancelFilterMenuItem"
            cancelFilterMenuItem.Enabled = False
            cancelFilterMenuItem.AutoSize = False
            cancelFilterMenuItem.Size = New Drawing.Size(Width - 1, 22)
            cancelFilterMenuItem.Text = "Clear Filter"
            AddHandler cancelFilterMenuItem.Click, New EventHandler(AddressOf CancelFilterMenuItem_Click)
            AddHandler cancelFilterMenuItem.MouseEnter, New EventHandler(AddressOf CancelFilterMenuItem_MouseEnter)
            '
            ' toolStripMenuItem2
            '
            toolStripSeparator2MenuItem.Name = "toolStripSeparator2MenuItem"
            toolStripSeparator2MenuItem.Size = New Drawing.Size(149, 6)
            toolStripSeparator2MenuItem.Visible = False
            '
            ' customFilterMenuItem
            '
            customFilterMenuItem.Name = "customFilterMenuItem"
            customFilterMenuItem.Size = New Drawing.Size(152, 22)
            customFilterMenuItem.Text = "Add a Custom Filter"
            AddHandler customFilterMenuItem.Click, New EventHandler(AddressOf CustomFilterMenuItem_Click)
            '
            ' customFilterLastFilter1MenuItem
            '
            customFilterLastFilter1MenuItem.Name = "customFilterLastFilter1MenuItem"
            customFilterLastFilter1MenuItem.Size = New Drawing.Size(152, 22)
            customFilterLastFilter1MenuItem.Tag = "0"
            customFilterLastFilter1MenuItem.Text = Nothing
            customFilterLastFilter1MenuItem.Visible = False
            AddHandler customFilterLastFilter1MenuItem.VisibleChanged, New EventHandler(AddressOf CustomFilterLastFilter1MenuItem_VisibleChanged)
            AddHandler customFilterLastFilter1MenuItem.Click, New EventHandler(AddressOf CustomFilterLastFilterMenuItem_Click)
            AddHandler customFilterLastFilter1MenuItem.TextChanged, New EventHandler(AddressOf CustomFilterLastFilterMenuItem_TextChanged)
            '
            ' customFilterLastFilter2MenuItem
            '
            customFilterLastFilter2MenuItem.Name = "customFilterLastFilter2MenuItem"
            customFilterLastFilter2MenuItem.Size = New Drawing.Size(152, 22)
            customFilterLastFilter2MenuItem.Tag = "1"
            customFilterLastFilter2MenuItem.Text = Nothing
            customFilterLastFilter2MenuItem.Visible = False
            AddHandler customFilterLastFilter2MenuItem.Click, New EventHandler(AddressOf CustomFilterLastFilterMenuItem_Click)
            AddHandler customFilterLastFilter2MenuItem.TextChanged, New EventHandler(AddressOf CustomFilterLastFilterMenuItem_TextChanged)
            '
            ' customFilterLastFilter3MenuItem
            '
            customFilterLastFilter3MenuItem.Name = "customFilterLastFilter3MenuItem"
            customFilterLastFilter3MenuItem.Size = New Drawing.Size(152, 22)
            customFilterLastFilter3MenuItem.Tag = "2"
            customFilterLastFilter3MenuItem.Text = Nothing
            customFilterLastFilter3MenuItem.Visible = False
            AddHandler customFilterLastFilter3MenuItem.Click, New EventHandler(AddressOf CustomFilterLastFilterMenuItem_Click)
            AddHandler customFilterLastFilter3MenuItem.TextChanged, New EventHandler(AddressOf CustomFilterLastFilterMenuItem_TextChanged)
            '
            ' customFilterLastFilter3MenuItem
            '
            customFilterLastFilter4MenuItem.Name = "lastfilter4MenuItem"
            customFilterLastFilter4MenuItem.Size = New Drawing.Size(152, 22)
            customFilterLastFilter4MenuItem.Tag = "3"
            customFilterLastFilter4MenuItem.Text = Nothing
            customFilterLastFilter4MenuItem.Visible = False
            AddHandler customFilterLastFilter4MenuItem.Click, New EventHandler(AddressOf CustomFilterLastFilterMenuItem_Click)
            AddHandler customFilterLastFilter4MenuItem.TextChanged, New EventHandler(AddressOf CustomFilterLastFilterMenuItem_TextChanged)
            '
            ' customFilterLastFilter5MenuItem
            '
            customFilterLastFilter5MenuItem.Name = "customFilterLastFilter5MenuItem"
            customFilterLastFilter5MenuItem.Size = New Drawing.Size(152, 22)
            customFilterLastFilter5MenuItem.Tag = "4"
            customFilterLastFilter5MenuItem.Text = Nothing
            customFilterLastFilter5MenuItem.Visible = False
            AddHandler customFilterLastFilter5MenuItem.Click, New EventHandler(AddressOf CustomFilterLastFilterMenuItem_Click)
            AddHandler customFilterLastFilter5MenuItem.TextChanged, New EventHandler(AddressOf CustomFilterLastFilterMenuItem_TextChanged)
            '
            ' customFilterLastFiltersListMenuItem
            '
            customFilterLastFiltersListMenuItem.Name = "customFilterLastFiltersListMenuItem"
            customFilterLastFiltersListMenuItem.AutoSize = False
            customFilterLastFiltersListMenuItem.Size = New Drawing.Size(Width - 1, 22)
            customFilterLastFiltersListMenuItem.Image = My.Resources.Resources.ColumnHeader_Filtered
            customFilterLastFiltersListMenuItem.ImageScaling = ToolStripItemImageScaling.None
            customFilterLastFiltersListMenuItem.DropDownItems.AddRange(New ToolStripItem() {customFilterMenuItem, toolStripSeparator2MenuItem, customFilterLastFilter1MenuItem, customFilterLastFilter2MenuItem, customFilterLastFilter3MenuItem, customFilterLastFilter4MenuItem, customFilterLastFilter5MenuItem})
            AddHandler customFilterLastFiltersListMenuItem.MouseEnter, New EventHandler(AddressOf CustomFilterLastFiltersListMenuItem_MouseEnter)
            AddHandler customFilterLastFiltersListMenuItem.Paint, New PaintEventHandler(AddressOf CustomFilterLastFiltersListMenuItem_Paint)
            '
            ' toolStripMenuItem3
            '
            toolStripSeparator3MenuItem.Name = "toolStripSeparator3MenuItem"
            toolStripSeparator3MenuItem.Size = New Drawing.Size(Width - 4, 6)
            '
            ' button_filter
            '
            button_filter.Name = "button_filter"
            button_filter.BackColor = DefaultBackColor
            button_filter.UseVisualStyleBackColor = True
            button_filter.Margin = New Padding(0)
            button_filter.Size = New Drawing.Size(75, 23)
            button_filter.Text = "Filter"
            AddHandler button_filter.Click, New EventHandler(AddressOf Button_ok_Click)
            button_filter.Location = New Drawing.Point(checkFilterListButtonsPanel.Width - 164, 0)
            '
            ' button_undofilter
            '
            button_undofilter.Name = "button_undofilter"
            button_undofilter.BackColor = DefaultBackColor
            button_undofilter.UseVisualStyleBackColor = True
            button_undofilter.Margin = New Padding(0)
            button_undofilter.Size = New Drawing.Size(75, 23)
            button_undofilter.Text = "Cancel"
            AddHandler button_undofilter.Click, New EventHandler(AddressOf Button_cancel_Click)
            button_undofilter.Location = New Drawing.Point(checkFilterListButtonsPanel.Width - 79, 0)
            '
            ' resizeBoxControlHost
            '
            resizeBoxControlHost.Name = "resizeBoxControlHost"
            resizeBoxControlHost.Control.Cursor = Cursors.SizeNWSE
            resizeBoxControlHost.AutoSize = False
            resizeBoxControlHost.Padding = New Padding(0)
            resizeBoxControlHost.Margin = New Padding(Width - 45, 0, 0, 0)
            resizeBoxControlHost.Size = New Drawing.Size(10, 10)
            AddHandler resizeBoxControlHost.Paint, New PaintEventHandler(AddressOf ResizeBoxControlHost_Paint)
            AddHandler resizeBoxControlHost.MouseDown, New MouseEventHandler(AddressOf ResizeBoxControlHost_MouseDown)
            AddHandler resizeBoxControlHost.MouseUp, New MouseEventHandler(AddressOf ResizeBoxControlHost_MouseUp)
            AddHandler resizeBoxControlHost.MouseMove, New MouseEventHandler(AddressOf ResizeBoxControlHost_MouseMove)
            '
            ' checkFilterListControlHost
            '
            checkFilterListControlHost.Name = "checkFilterListControlHost"
            checkFilterListControlHost.AutoSize = False
            checkFilterListControlHost.Size = New Drawing.Size(Width - 35, 194)
            checkFilterListControlHost.Padding = New Padding(0)
            checkFilterListControlHost.Margin = New Padding(0)
            '
            ' checkTextFilterControlHost
            '
            checkTextFilterControlHost.Name = "checkTextFilterControlHost"
            checkTextFilterControlHost.AutoSize = False
            checkTextFilterControlHost.Size = New Drawing.Size(Width - 35, 20)
            checkTextFilterControlHost.Padding = New Padding(4, 0, 4, 0)
            checkTextFilterControlHost.Margin = New Padding(0)
            '
            ' checkFilterListButtonsControlHost
            '
            checkFilterListButtonsControlHost.Name = "checkFilterListButtonsControlHost"
            checkFilterListButtonsControlHost.AutoSize = False
            checkFilterListButtonsControlHost.Size = New Drawing.Size(Width - 35, 24)
            checkFilterListButtonsControlHost.Padding = New Padding(0)
            checkFilterListButtonsControlHost.Margin = New Padding(0)
            '
            ' checkFilterListPanel
            '
            checkFilterListPanel.Name = "checkFilterListPanel"
            checkFilterListPanel.AutoSize = False
            checkFilterListPanel.Size = checkFilterListControlHost.Size
            checkFilterListPanel.Padding = New Padding(0)
            checkFilterListPanel.Margin = New Padding(0)
            checkFilterListPanel.BackColor = BackColor
            checkFilterListPanel.BorderStyle = BorderStyle.None
            checkFilterListPanel.Controls.Add(checkList)
            '
            ' checkList
            '
            checkList.Name = "checkList"
            checkList.AutoSize = False
            checkList.Padding = New Padding(0)
            checkList.Margin = New Padding(0)
            checkList.Bounds = New Drawing.Rectangle(4, 4, checkFilterListPanel.Width - 8, checkFilterListPanel.Height - 8)
            checkList.StateImageList = GetCheckListStateImages()
            checkList.CheckBoxes = False
            AddHandler checkList.MouseLeave, New EventHandler(AddressOf CheckList_MouseLeave)
            AddHandler checkList.NodeMouseClick, New TreeNodeMouseClickEventHandler(AddressOf CheckList_NodeMouseClick)
            AddHandler checkList.KeyDown, New KeyEventHandler(AddressOf CheckList_KeyDown)
            AddHandler checkList.MouseEnter, AddressOf CheckList_MouseEnter
            AddHandler checkList.NodeMouseDoubleClick, AddressOf CheckList_NodeMouseDoubleClick
            '
            ' checkTextFilter
            '
            checkTextFilter.Name = "checkTextFilter"
            checkTextFilter.Padding = New Padding(0)
            checkTextFilter.Margin = New Padding(0)
            checkTextFilter.Size = checkTextFilterControlHost.Size
            checkTextFilter.Dock = DockStyle.Fill
            AddHandler checkTextFilter.TextChanged, New EventHandler(AddressOf CheckTextFilter_TextChanged)
            '
            ' checkFilterListButtonsPanel
            '
            checkFilterListButtonsPanel.Name = "checkFilterListButtonsPanel"
            checkFilterListButtonsPanel.AutoSize = False
            checkFilterListButtonsPanel.Size = checkFilterListButtonsControlHost.Size
            checkFilterListButtonsPanel.Padding = New Padding(0)
            checkFilterListButtonsPanel.Margin = New Padding(0)
            checkFilterListButtonsPanel.BackColor = BackColor
            checkFilterListButtonsPanel.BorderStyle = BorderStyle.None
            checkFilterListButtonsPanel.Controls.AddRange(New Control() {button_filter, button_undofilter})
            ResumeLayout(False)
            PerformLayout()

        End Sub

#End Region

        Private sortASCMenuItem As ToolStripMenuItem
        Private sortDESCMenuItem As ToolStripMenuItem
        Private cancelSortMenuItem As ToolStripMenuItem
        Private toolStripSeparator1MenuItem As ToolStripSeparator
        Private toolStripSeparator2MenuItem As ToolStripSeparator
        Private toolStripSeparator3MenuItem As ToolStripSeparator
        Private cancelFilterMenuItem As ToolStripMenuItem
        Private customFilterLastFiltersListMenuItem As ToolStripMenuItem
        Private customFilterMenuItem As ToolStripMenuItem
        Private customFilterLastFilter1MenuItem As ToolStripMenuItem
        Private customFilterLastFilter2MenuItem As ToolStripMenuItem
        Private customFilterLastFilter3MenuItem As ToolStripMenuItem
        Private customFilterLastFilter4MenuItem As ToolStripMenuItem
        Private customFilterLastFilter5MenuItem As ToolStripMenuItem
        Private checkList As TreeView
        Private button_filter As Button
        Private button_undofilter As Button
        Private checkFilterListControlHost As ToolStripControlHost
        Private checkFilterListButtonsControlHost As ToolStripControlHost
        Private resizeBoxControlHost As ToolStripControlHost
        Private checkFilterListPanel As Panel
        Private checkFilterListButtonsPanel As Panel
        Private checkTextFilter As TextBox
        Private checkTextFilterControlHost As ToolStripControlHost
    End Class
End Namespace
