#Region "License"
' Advanced DataGridView
'
' Copyright (c), 2014 Davide Gironi <davide.gironi@gmail.com>
' Original work Copyright (c), 2013 Zuby <zuby@me.com>
'
' Please refer to LICENSE file for licensing information.
#End Region

Imports System
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Globalization
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports System.Windows.Forms.VisualStyles

Namespace TableSheet

    <ComponentModel.DesignerCategory("")>
    Partial Friend Class MenuStrip
        Inherits ContextMenuStrip

        ''' <summary>
        ''' Get the DataType for the MenuStrip Filter
        ''' </summary>
        Private _DataType As System.Type

#Region "public enum"

        ''' <summary>
        ''' MenuStrip Filter type
        ''' </summary>
        Public Enum FilterType As Byte
            None = 0
            Custom = 1
            CheckList = 2
            Loaded = 3
        End Enum


        ''' <summary>
        ''' MenuStrip Sort type
        ''' </summary>
        Public Enum SortType As Byte
            None = 0
            ASC = 1
            DESC = 2
        End Enum

#End Region


#Region "public constants"

        ''' <summary>
        ''' Default checklist filter node behaviour 
        ''' </summary>
        Public Const DefaultCheckTextFilterRemoveNodesOnSearch As Boolean = True

        ''' <summary>
        ''' Default max filter checklist max nodes
        ''' </summary>
        Public Const DefaultMaxChecklistNodes As Integer = 10000

        ''' <summary>
        ''' Default number of nodes to enable the TextChanged delay on text filter
        ''' </summary>
        Public Const DefaultTextFilterTextChangedDelayNodes As Integer = 1000

        ''' <summary>
        ''' Number of nodes to disable the text filter TextChanged delay
        ''' </summary>
        Public Const TextFilterTextChangedDelayNodesDisabled As Integer = -1

        ''' <summary>
        ''' Default delay milliseconds for TextChanged delay on text filter
        ''' </summary>
        Public Const DefaultTextFilterTextChangedDelayMs As Integer = 300

#End Region


#Region "class properties"

        Private _activeFilterType As FilterType = FilterType.None
        Private _activeSortType As SortType = SortType.None
        Private _loadedNodes As TreeNodeItemSelector() = New TreeNodeItemSelector() {}
        Private _startingNodes As TreeNodeItemSelector() = New TreeNodeItemSelector() {}
        Private _removedNodes As TreeNodeItemSelector() = New TreeNodeItemSelector() {}
        Private _removedsessionNodes As TreeNodeItemSelector() = New TreeNodeItemSelector() {}
        Private _sortString As String = Nothing
        Private _filterString As String = Nothing
        Private Shared _resizeStartPoint As Point = New Point(1, 1)
        Private _resizeEndPoint As Point = New Point(-1, -1)
        Private _checkTextFilterChangedEnabled As Boolean = True
        Private _checkTextFilterRemoveNodesOnSearch As Boolean = DefaultCheckTextFilterRemoveNodesOnSearch
        Private _maxChecklistNodes As Integer = DefaultMaxChecklistNodes
        Private _filterclick As Boolean = False
        Private _textFilterTextChangedTimer As Timer
        Private _textFilterTextChangedDelayNodes As Integer = DefaultTextFilterTextChangedDelayNodes
        Private _textFilterTextChangedDelayMs As Integer = DefaultTextFilterTextChangedDelayMs

#End Region


#Region "costructors"

        ''' <summary>
        ''' MenuStrip constructor
        ''' </summary>
        ''' <param name="dataType"></param>
        Public Sub New(dataType As Type)
            MyBase.New()
            'initialize components
            InitializeComponent()

            'set type
            SetDataType(dataType)

            'set component translations
            cancelSortMenuItem.Text = AdvancedDataGridView.Translations(AdvancedDataGridView.TranslationKey.ADGVClearSort.ToString())
            cancelFilterMenuItem.Text = AdvancedDataGridView.Translations(AdvancedDataGridView.TranslationKey.ADGVClearFilter.ToString())
            customFilterMenuItem.Text = AdvancedDataGridView.Translations(AdvancedDataGridView.TranslationKey.ADGVAddCustomFilter.ToString())
            button_filter.Text = AdvancedDataGridView.Translations(AdvancedDataGridView.TranslationKey.ADGVButtonFilter.ToString())
            button_undofilter.Text = AdvancedDataGridView.Translations(AdvancedDataGridView.TranslationKey.ADGVButtonUndofilter.ToString())

            'set default NOT IN logic
            IsFilterNOTINLogicEnabled = False

            'set enablers default
            IsSortEnabled = True
            IsFilterEnabled = True
            IsFilterChecklistEnabled = True
            IsFilterDateAndTimeEnabled = True
            FilterTextFocusOnShow = False

            'resize before hitting ResizeBox so the grip works correctly
            Dim scalingfactor As Single = GetScalingFactor()
            MyBase.MinimumSize = New Size(Scale(PreferredSize.Width, scalingfactor), Scale(PreferredSize.Height, scalingfactor))
            'once the size is set resize the ones that wont change      
            resizeBoxControlHost.Height = Scale(resizeBoxControlHost.Height, scalingfactor)
            resizeBoxControlHost.Width = Scale(resizeBoxControlHost.Width, scalingfactor)
            toolStripSeparator1MenuItem.Height = Scale(toolStripSeparator1MenuItem.Height, scalingfactor)
            toolStripSeparator2MenuItem.Height = Scale(toolStripSeparator2MenuItem.Height, scalingfactor)
            toolStripSeparator3MenuItem.Height = Scale(toolStripSeparator3MenuItem.Height, scalingfactor)
            sortASCMenuItem.Height = Scale(sortASCMenuItem.Height, scalingfactor)
            sortDESCMenuItem.Height = Scale(sortDESCMenuItem.Height, scalingfactor)
            cancelSortMenuItem.Height = Scale(cancelSortMenuItem.Height, scalingfactor)
            cancelFilterMenuItem.Height = Scale(cancelFilterMenuItem.Height, scalingfactor)
            customFilterMenuItem.Height = Scale(customFilterMenuItem.Height, scalingfactor)
            customFilterLastFiltersListMenuItem.Height = Scale(customFilterLastFiltersListMenuItem.Height, scalingfactor)
            checkTextFilterControlHost.Height = Scale(checkTextFilterControlHost.Height, scalingfactor)
            button_filter.Width = Scale(button_filter.Width, scalingfactor)
            button_filter.Height = Scale(button_filter.Height, scalingfactor)
            button_undofilter.Width = Scale(button_undofilter.Width, scalingfactor)
            button_undofilter.Height = Scale(button_undofilter.Height, scalingfactor)
            'resize
            ResizeBox(MyBase.MinimumSize.Width, MyBase.MinimumSize.Height)

            _textFilterTextChangedTimer = New Timer()
            _textFilterTextChangedTimer.Interval = _textFilterTextChangedDelayMs
            AddHandler _textFilterTextChangedTimer.Tick, New EventHandler(AddressOf CheckTextFilterTextChangedTimer_Tick)
        End Sub

        ''' <summary>
        ''' Closed event
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub MenuStrip_Closed(sender As Object, e As EventArgs)
            ResizeClean()

            If _checkTextFilterRemoveNodesOnSearch AndAlso Not _filterclick Then
                _loadedNodes = DuplicateNodes(_startingNodes)
            End If

            _startingNodes = New TreeNodeItemSelector() {}

            _checkTextFilterChangedEnabled = False
            checkTextFilter.Text = ""
            _checkTextFilterChangedEnabled = True
        End Sub

        ''' <summary>
        ''' LostFocust event
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub MenuStrip_LostFocus(sender As Object, e As EventArgs)
            If Not ContainsFocus Then Close()
        End Sub

        ''' <summary>
        ''' Control removed event
        ''' </summary>
        ''' <param name="e"></param>
        Protected Overrides Sub OnControlRemoved(e As ControlEventArgs)
            _loadedNodes = New TreeNodeItemSelector() {}
            _startingNodes = New TreeNodeItemSelector() {}
            _removedNodes = New TreeNodeItemSelector() {}
            _removedsessionNodes = New TreeNodeItemSelector() {}
            If _textFilterTextChangedTimer IsNot Nothing Then _textFilterTextChangedTimer.Stop()

            MyBase.OnControlRemoved(e)
        End Sub

        ''' <summary>
        ''' Get all images for checkList
        ''' </summary>
        ''' <returns></returns>
        Private Shared Function GetCheckListStateImages() As ImageList
            Dim images As ImageList = New ImageList()
            Dim unCheckImg As Bitmap = New Bitmap(16, 16)
            Dim checkImg As Bitmap = New Bitmap(16, 16)
            Dim mixedImg As Bitmap = New Bitmap(16, 16)

            Using img As Bitmap = New Bitmap(16, 16)
                Using g = Graphics.FromImage(img)
                    Call CheckBoxRenderer.DrawCheckBox(g, New Point(0, 1), CheckBoxState.UncheckedNormal)
                    unCheckImg = CType(img.Clone(), Bitmap)
                    Call CheckBoxRenderer.DrawCheckBox(g, New Point(0, 1), CheckBoxState.CheckedNormal)
                    checkImg = CType(img.Clone(), Bitmap)
                    Call CheckBoxRenderer.DrawCheckBox(g, New Point(0, 1), CheckBoxState.MixedNormal)
                    mixedImg = CType(img.Clone(), Bitmap)
                End Using
            End Using

            images.Images.Add("uncheck", unCheckImg)
            images.Images.Add("check", checkImg)
            images.Images.Add("mixed", mixedImg)

            Return images
        End Function

#End Region


#Region "public events"

        ''' <summary>
        ''' The current Sorting in changed
        ''' </summary>
        Public Event SortChanged As EventHandler

        ''' <summary>
        ''' The current Filter is changed
        ''' </summary>
        Public Event FilterChanged As EventHandler

#End Region


#Region "public getter and setters"

        ''' <summary>
        ''' Set the max checklist nodes
        ''' </summary>
        Public Property MaxChecklistNodes As Integer
            Get
                Return _maxChecklistNodes
            End Get
            Set(value As Integer)
                _maxChecklistNodes = value
            End Set
        End Property

        ''' <summary>
        ''' Get the current MenuStripSortType type
        ''' </summary>
        Public ReadOnly Property ActiveSortType As SortType
            Get
                Return _activeSortType
            End Get
        End Property

        ''' <summary>
        ''' Get the current MenuStripFilterType type
        ''' </summary>
        Public ReadOnly Property ActiveFilterType As FilterType
            Get
                Return _activeFilterType
            End Get
        End Property

        Public Property DataType As Type
            Get
                Return _DataType
            End Get
            Private Set(value As Type)
                _DataType = value
            End Set
        End Property

        ''' <summary>
        ''' Get or Set the Filter Sort enabled
        ''' </summary>
        Public Property IsSortEnabled As Boolean

        ''' <summary>
        ''' Get or Set the Filter enabled
        ''' </summary>
        Public Property IsFilterEnabled As Boolean

        ''' <summary>
        ''' Get or Set the Filter Checklist enabled
        ''' </summary>
        Public Property IsFilterChecklistEnabled As Boolean

        ''' <summary>
        ''' Get or Set the Filter Custom enabled
        ''' </summary>
        Public Property IsFilterCustomEnabled As Boolean

        ''' <summary>
        ''' Get or Set the Filter DateAndTime enabled
        ''' </summary>
        Public Property IsFilterDateAndTimeEnabled As Boolean

        ''' <summary>
        ''' Get or Set the NOT IN logic for Filter
        ''' </summary>
        Public Property IsFilterNOTINLogicEnabled As Boolean

        ''' <summary>
        ''' Set the text filter search nodes behaviour
        ''' </summary>
        Public Property DoesTextFilterRemoveNodesOnSearch As Boolean
            Get
                Return _checkTextFilterRemoveNodesOnSearch
            End Get
            Set(value As Boolean)
                _checkTextFilterRemoveNodesOnSearch = value
            End Set
        End Property

        ''' <summary>
        ''' Number of nodes to enable the TextChanged delay on text filter
        ''' </summary>
        Public Property TextFilterTextChangedDelayNodes As Integer
            Get
                Return _textFilterTextChangedDelayNodes
            End Get
            Set(value As Integer)
                _textFilterTextChangedDelayNodes = value
            End Set
        End Property

        ''' <summary>
        ''' Delay milliseconds for TextChanged delay on text filter
        ''' </summary>
        Public Property TextFilterTextChangedDelayMs As Integer
            Get
                Return _textFilterTextChangedDelayMs
            End Get
            Set(value As Integer)
                _textFilterTextChangedDelayMs = value
            End Set
        End Property

        ''' <summary>
        ''' Get or Set the Filter Text focus OnShow
        ''' </summary>
        Public Property FilterTextFocusOnShow As Boolean

#End Region


#Region "public enablers"

        ''' <summary>
        ''' Enabled or disable Sorting capabilities
        ''' </summary>
        ''' <param name="enabled"></param>
        Public Sub SetSortEnabled(enabled As Boolean)
            IsSortEnabled = enabled

            sortASCMenuItem.Enabled = enabled
            sortDESCMenuItem.Enabled = enabled
            cancelSortMenuItem.Enabled = enabled
        End Sub

        ''' <summary>
        ''' Enable or disable Filter capabilities
        ''' </summary>
        ''' <param name="enabled"></param>
        Public Sub SetFilterEnabled(enabled As Boolean)
            IsFilterEnabled = enabled

            cancelFilterMenuItem.Enabled = enabled
            customFilterLastFiltersListMenuItem.Enabled = enabled AndAlso DataType IsNot GetType(Boolean)
            button_filter.Enabled = enabled
            button_undofilter.Enabled = enabled
            checkList.Enabled = enabled
            checkTextFilter.Enabled = enabled
        End Sub

        ''' <summary>
        ''' Enable or disable Filter checklist capabilities
        ''' </summary>
        ''' <param name="enabled"></param>
        Public Sub SetFilterChecklistEnabled(enabled As Boolean)
            If Not IsFilterEnabled Then enabled = False

            IsFilterChecklistEnabled = enabled
            checkList.Enabled = enabled
            checkTextFilter.ReadOnly = Not enabled

            If Not IsFilterChecklistEnabled Then
                ChecklistClearNodes()
                Dim disablednode As TreeNodeItemSelector = TreeNodeItemSelector.CreateNode(AdvancedDataGridView.Translations(AdvancedDataGridView.TranslationKey.ADGVFilterChecklistDisable.ToString()) & "            ", Nothing, CheckState.Checked, TreeNodeItemSelector.CustomNodeType.SelectAll)
                disablednode.NodeFont = New Font(checkList.Font, FontStyle.Bold)
                ChecklistAddNode(disablednode)
                ChecklistReloadNodes()
            End If
        End Sub

        ''' <summary>
        ''' Enable or disable Filter custom capabilities
        ''' </summary>
        ''' <param name="enabled"></param>
        Public Sub SetFilterCustomEnabled(enabled As Boolean)
            If Not IsFilterEnabled Then enabled = False

            IsFilterCustomEnabled = enabled
            customFilterMenuItem.Enabled = enabled
            customFilterLastFiltersListMenuItem.Enabled = enabled

            If Not IsFilterCustomEnabled Then
                UnCheckCustomFilters()
            End If
        End Sub

        ''' <summary>
        ''' Disable text filter TextChanged delay
        ''' </summary>
        Public Sub SetTextFilterTextChangedDelayNodesDisabled()
            _textFilterTextChangedDelayNodes = TextFilterTextChangedDelayNodesDisabled
        End Sub

#End Region


#Region "preset loader"

        Public Sub SetLoadedMode(enabled As Boolean)
            customFilterMenuItem.Enabled = Not enabled
            cancelFilterMenuItem.Enabled = enabled
            If enabled Then
                _activeFilterType = FilterType.Loaded
                _sortString = Nothing
                _filterString = Nothing
                customFilterLastFiltersListMenuItem.Checked = False
                For i = 2 To customFilterLastFiltersListMenuItem.DropDownItems.Count - 1 - 1
                    TryCast(customFilterLastFiltersListMenuItem.DropDownItems(i), ToolStripMenuItem).Checked = False
                Next

                ChecklistClearNodes()
                Dim allnode As TreeNodeItemSelector = TreeNodeItemSelector.CreateNode(AdvancedDataGridView.Translations(AdvancedDataGridView.TranslationKey.ADGVNodeSelectAll.ToString()) & "            ", Nothing, CheckState.Indeterminate, TreeNodeItemSelector.CustomNodeType.SelectAll)
                allnode.NodeFont = New Font(checkList.Font, FontStyle.Bold)
                ChecklistAddNode(allnode)
                ChecklistReloadNodes()

                SetSortEnabled(False)
                SetFilterEnabled(False)
            Else
                _activeFilterType = FilterType.None

                SetSortEnabled(True)
                SetFilterEnabled(True)
            End If
        End Sub

#End Region


#Region "public show methods"

        ''' <summary>
        ''' Show the menuStrip
        ''' </summary>
        ''' <param name="control"></param>
        ''' <param name="x"></param>
        ''' <param name="y"></param>
        ''' <param name="columnName"></param>
        Public Overloads Sub Show(control As Control, x As Integer, y As Integer, columnName As String)
            _removedNodes = New TreeNodeItemSelector() {}
            _removedsessionNodes = New TreeNodeItemSelector() {}

            'add nodes
            Dim dataGridView As DataGridView = TryCast(control, DataGridView)
            Dim vals As IEnumerable(Of DataGridViewCell) = If(dataGridView IsNot Nothing, GetValuesForFilter(dataGridView, columnName), Enumerable.Empty(Of DataGridViewCell)())
            BuildNodes(vals, dataGridView, columnName)
            'set the starting nodes
            _startingNodes = DuplicateNodes(_loadedNodes)

            If _activeFilterType = FilterType.Custom Then SetNodesCheckState(_loadedNodes, False)
            MyBase.Show(control, x, y)

            'force checklist refresh
            checkList.BeginUpdate()
            checkList.EndUpdate()

            _filterclick = False

            _checkTextFilterChangedEnabled = False
            checkTextFilter.Text = ""
            _checkTextFilterChangedEnabled = True

            If FilterTextFocusOnShow Then checkTextFilter.Focus()
        End Sub

        ''' <summary>
        ''' Show the menuStrip
        ''' </summary>
        ''' <param name="control"></param>
        ''' <param name="x"></param>
        ''' <param name="y"></param>
        ''' <param name="_restoreFilter"></param>
        Public Overloads Sub Show(control As Control, x As Integer, y As Integer, _restoreFilter As Boolean)
            _checkTextFilterChangedEnabled = False
            checkTextFilter.Text = ""
            _checkTextFilterChangedEnabled = True
            If _restoreFilter OrElse _checkTextFilterRemoveNodesOnSearch Then
                'reset the starting nodes
                _startingNodes = DuplicateNodes(_loadedNodes)
            End If
            'reset removed nodes
            If _checkTextFilterRemoveNodesOnSearch Then
                _removedNodes = _loadedNodes.Where(Function(n) n.CheckState = CheckState.Unchecked AndAlso n.NodeType = TreeNodeItemSelector.CustomNodeType.Default).ToArray()
                _removedsessionNodes = _removedNodes
            End If

            ChecklistReloadNodes()

            MyBase.Show(control, x, y)

            _filterclick = False

            If FilterTextFocusOnShow Then checkTextFilter.Focus()
        End Sub

        ''' <summary>
        ''' Get values used for Show method
        ''' </summary>
        ''' <param name="grid"></param>
        ''' <param name="columnName"></param>
        ''' <returns></returns>
        Public Shared Function GetValuesForFilter(grid As DataGridView, columnName As String) As IEnumerable(Of DataGridViewCell)
            Return From nulls As DataGridViewRow In grid.Rows Select nulls.Cells(columnName)
        End Function

#End Region


#Region "public sort methods"

        ''' <summary>
        ''' Sort ASC
        ''' </summary>
        Public Sub SortASC()
            SortASCMenuItem_Click(Me, Nothing)
        End Sub

        ''' <summary>
        ''' Sort DESC
        ''' </summary>
        Public Sub SortDESC()
            SortDESCMenuItem_Click(Me, Nothing)
        End Sub

        ''' <summary>
        ''' Get the Sorting String
        ''' </summary>
        Public Property SortString As String
            Get
                Return If(Not String.IsNullOrEmpty(_sortString), _sortString, "")
            End Get
            Private Set(value As String)
                cancelSortMenuItem.Enabled = Not Equals(value, Nothing) AndAlso value.Length > 0
                _sortString = value
            End Set
        End Property

        ''' <summary>
        ''' Clean the Sorting
        ''' </summary>
        Public Sub CleanSort()
            sortASCMenuItem.Checked = False
            sortDESCMenuItem.Checked = False
            _activeSortType = SortType.None
            SortString = Nothing
        End Sub

#End Region


#Region "public filter methods"

        ''' <summary>
        ''' Get the Filter string
        ''' </summary>
        Public Property FilterString As String
            Get
                Return If(Not String.IsNullOrEmpty(_filterString), _filterString, "")
            End Get
            Private Set(value As String)
                cancelFilterMenuItem.Enabled = Not Equals(value, Nothing) AndAlso value.Length > 0
                _filterString = value
            End Set
        End Property

        ''' <summary>
        ''' Clean the Filter
        ''' </summary>
        Public Sub CleanFilter()
            If _checkTextFilterRemoveNodesOnSearch Then
                _removedNodes = New TreeNodeItemSelector() {}
                _removedsessionNodes = New TreeNodeItemSelector() {}
            End If

            For i = 2 To customFilterLastFiltersListMenuItem.DropDownItems.Count - 1 - 1
                TryCast(customFilterLastFiltersListMenuItem.DropDownItems(i), ToolStripMenuItem).Checked = False
            Next
            _activeFilterType = FilterType.None
            SetNodesCheckState(_loadedNodes, True)
            FilterString = Nothing
            customFilterLastFiltersListMenuItem.Checked = False
            button_filter.Enabled = True
        End Sub

        ''' <summary>
        ''' Set the text filter on checklist remove node mode
        ''' </summary>
        ''' <param name="enabled"></param>
        Public Sub SetChecklistTextFilterRemoveNodesOnSearchMode(enabled As Boolean)
            If _checkTextFilterRemoveNodesOnSearch <> enabled Then
                _checkTextFilterRemoveNodesOnSearch = enabled
                CleanFilter()
            End If
        End Sub

#End Region


#Region "checklist filter methods"

        ''' <summary>
        ''' Clear checklist loaded nodes
        ''' </summary>
        Private Sub ChecklistClearNodes()
            _loadedNodes = New TreeNodeItemSelector() {}
        End Sub

        ''' <summary>
        ''' Add a node to checklist nodes
        ''' </summary>
        ''' <param name="node"></param>
        Private Sub ChecklistAddNode(node As TreeNodeItemSelector)
            _loadedNodes = _loadedNodes.Concat(New TreeNodeItemSelector() {node}).ToArray()
        End Sub

        ''' <summary>
        ''' Load checklist nodes
        ''' </summary>
        Private Sub ChecklistReloadNodes()
            checkList.BeginUpdate()
            checkList.Nodes.Clear()
            Dim nodecount = 0
            For Each node In _loadedNodes
                If node.NodeType = TreeNodeItemSelector.CustomNodeType.Default Then
                    If _maxChecklistNodes = 0 Then
                        If Not _removedNodes.Contains(node) Then checkList.Nodes.Add(node)
                    Else
                        If nodecount < _maxChecklistNodes AndAlso Not _removedNodes.Contains(node) Then
                            checkList.Nodes.Add(node)
                        ElseIf nodecount = _maxChecklistNodes Then
                            checkList.Nodes.Add("...")
                        End If
                        If Not _removedNodes.Contains(node) OrElse nodecount = _maxChecklistNodes Then nodecount += 1

                    End If
                Else
                    checkList.Nodes.Add(node)
                End If

            Next
            checkList.EndUpdate()
        End Sub

        ''' <summary>
        ''' Get checklist nodes
        ''' </summary>
        ''' <returns></returns>
        Private Function ChecklistNodes() As TreeNodeCollection
            Return checkList.Nodes
        End Function

        ''' <summary>
        ''' Set the Filter String using checkList selected Nodes
        ''' </summary>
        Private Sub SetCheckListFilter()
            UnCheckCustomFilters()

            Dim selectAllNode As TreeNodeItemSelector = GetSelectAllNode()
            customFilterLastFiltersListMenuItem.Checked = False

            If selectAllNode IsNot Nothing AndAlso selectAllNode.Checked AndAlso String.IsNullOrEmpty(checkTextFilter.Text) Then
                CancelFilterMenuItem_Click(Nothing, New EventArgs())
            Else
                Dim oldfilter = FilterString
                FilterString = ""
                _activeFilterType = FilterType.CheckList

                If _loadedNodes.Length > 1 Then
                    selectAllNode = GetSelectEmptyNode()
                    If selectAllNode IsNot Nothing AndAlso selectAllNode.Checked Then FilterString = "[{0}] IS NULL"

                    If _loadedNodes.Length > 2 OrElse selectAllNode Is Nothing Then
                        Dim filter As String = BuildNodesFilterString((If(IsFilterNOTINLogicEnabled AndAlso DataType IsNot GetType(Date) AndAlso DataType IsNot GetType(TimeSpan) AndAlso DataType IsNot GetType(Boolean), _loadedNodes.AsParallel().Cast(Of TreeNodeItemSelector)().Where(Function(n) n.NodeType <> TreeNodeItemSelector.CustomNodeType.SelectAll AndAlso n.NodeType <> TreeNodeItemSelector.CustomNodeType.SelectEmpty AndAlso n.CheckState = CheckState.Unchecked), _loadedNodes.AsParallel().Cast(Of TreeNodeItemSelector)().Where(Function(n) n.NodeType <> TreeNodeItemSelector.CustomNodeType.SelectAll AndAlso n.NodeType <> TreeNodeItemSelector.CustomNodeType.SelectEmpty AndAlso n.CheckState <> CheckState.Unchecked))))
                        filter = filter.Replace("{", "{{").Replace("}", "}}")

                        If filter.Length > 0 Then
                            If FilterString.Length > 0 Then FilterString += " OR "

                            If DataType Is GetType(Boolean) Then
                                FilterString += "[{0}] =" & filter
                            ElseIf DataType Is GetType(Integer) OrElse DataType Is GetType(Long) OrElse DataType Is GetType(Short) OrElse DataType Is GetType(UInteger) OrElse DataType Is GetType(ULong) OrElse DataType Is GetType(UShort) OrElse DataType Is GetType(Decimal) OrElse DataType Is GetType(Byte) OrElse DataType Is GetType(SByte) OrElse DataType Is GetType(String) Then
                                If IsFilterNOTINLogicEnabled Then
                                    FilterString += "[{0}] NOT IN (" & filter & ")"
                                Else
                                    FilterString += "[{0}] IN (" & filter & ")"
                                End If
                            ElseIf DataType Is GetType(Bitmap) Then
                            Else
                                If IsFilterNOTINLogicEnabled Then
                                    FilterString += "Convert([{0}],System.String) NOT IN (" & filter & ")"
                                Else
                                    FilterString += "Convert([{0}],System.String) IN (" & filter & ")"
                                End If
                            End If
                        End If
                    End If
                End If

                If Not Equals(oldfilter, FilterString) AndAlso FilterChangedEvent IsNot Nothing Then RaiseEvent FilterChanged(Me, New EventArgs())
            End If
        End Sub

        ''' <summary>
        ''' Build a Filter string based on selectd Nodes
        ''' </summary>
        ''' <param name="nodes"></param>
        ''' <returns></returns>
        Private Function BuildNodesFilterString(nodes As IEnumerable(Of TreeNodeItemSelector)) As String
            Dim sb As StringBuilder = New StringBuilder("")

            Dim appx = ", "

            If nodes IsNot Nothing AndAlso nodes.Any() Then
                If DataType Is GetType(Date) Then
                    For Each n In nodes
                        If n.Checked AndAlso (n.Nodes.AsParallel().Cast(Of TreeNodeItemSelector)().Where(Function(sn) sn.CheckState <> CheckState.Unchecked).Count() = 0) Then
                            Dim dt As Date = n.Value
                            sb.Append("'" & Convert.ToString(If(IsFilterDateAndTimeEnabled, dt, dt.Date), CultureInfo.CurrentCulture) & "'" & appx)
                        ElseIf n.CheckState <> CheckState.Unchecked AndAlso n.Nodes.Count > 0 Then
                            Dim subnode As String = BuildNodesFilterString(n.Nodes.AsParallel().Cast(Of TreeNodeItemSelector)().Where(Function(sn) sn.CheckState <> CheckState.Unchecked))
                            If subnode.Length > 0 Then sb.Append(subnode & appx)
                        End If
                    Next
                ElseIf DataType Is GetType(TimeSpan) Then
                    For Each n In nodes
                        If n.Checked AndAlso (n.Nodes.AsParallel().Cast(Of TreeNodeItemSelector)().Where(Function(sn) sn.CheckState <> CheckState.Unchecked).Count() = 0) Then
                            Dim ts As TimeSpan = n.Value
                            sb.Append("'P" & (If(ts.Days > 0, ts.Days.ToString() & "D", "")) & If(ts.TotalHours > 0, "T", "") & (If(ts.Hours > 0, ts.Hours.ToString() & "H", "")) & (If(ts.Minutes > 0, ts.Minutes.ToString() & "M", "")) & (If(ts.Seconds > 0, ts.Seconds.ToString() & "S", "")) & "'" & appx)
                        ElseIf n.CheckState <> CheckState.Unchecked AndAlso n.Nodes.Count > 0 Then
                            Dim subnode As String = BuildNodesFilterString(n.Nodes.AsParallel().Cast(Of TreeNodeItemSelector)().Where(Function(sn) sn.CheckState <> CheckState.Unchecked))
                            If subnode.Length > 0 Then sb.Append(subnode & appx)
                        End If
                    Next
                ElseIf DataType Is GetType(Boolean) Then
                    For Each n In nodes
                        sb.Append(n.Value.ToString())
                        Exit For
                    Next
                ElseIf DataType Is GetType(Integer) OrElse DataType Is GetType(Long) OrElse DataType Is GetType(Short) OrElse DataType Is GetType(UInteger) OrElse DataType Is GetType(ULong) OrElse DataType Is GetType(UShort) OrElse DataType Is GetType(Byte) OrElse DataType Is GetType(SByte) Then
                    For Each n In nodes
                        sb.Append(n.Value.ToString() & appx)
                    Next
                ElseIf DataType Is GetType(Single) OrElse DataType Is GetType(Double) OrElse DataType Is GetType(Decimal) Then
                    For Each n In nodes
                        sb.Append(n.Value.ToString().Replace(",", ".") & appx)
                    Next
                ElseIf DataType Is GetType(Bitmap) Then
                Else
                    For Each n In nodes
                        sb.Append("'" & FormatFilterString(n.Value.ToString()) & "'" & appx)
                    Next
                End If
            End If

            If sb.Length > appx.Length AndAlso DataType IsNot GetType(Boolean) Then sb.Remove(sb.Length - appx.Length, appx.Length)

            Return sb.ToString()
        End Function

        ''' <summary>
        ''' Format a text Filter string
        ''' </summary>
        ''' <param name="text"></param>
        ''' <returns></returns>
        Private Shared Function FormatFilterString(text As String) As String
            Return text.Replace("'", "''")
        End Function

        ''' <summary>
        ''' Add nodes to checkList
        ''' </summary>
        ''' <param name="vals"></param>
        ''' <param name="dataGridView"></param>
        ''' <param name="columnName"></param>
        Private Sub BuildNodes(vals As IEnumerable(Of DataGridViewCell), dataGridView As DataGridView, columnName As String)
            If Not IsFilterChecklistEnabled Then Return

            ChecklistClearNodes()

            If vals IsNot Nothing Then
                'add select all node
                Dim allnode As TreeNodeItemSelector = TreeNodeItemSelector.CreateNode(AdvancedDataGridView.Translations(AdvancedDataGridView.TranslationKey.ADGVNodeSelectAll.ToString()) & "            ", Nothing, CheckState.Checked, TreeNodeItemSelector.CustomNodeType.SelectAll)
                allnode.NodeFont = New Font(checkList.Font, FontStyle.Bold)
                ChecklistAddNode(allnode)

                If vals.Any() Then
                    Dim nonulls = vals.Where(Function(c) c.Value IsNot Nothing AndAlso c.Value IsNot DBNull.Value)

                    'add select empty node
                    If vals.Count() <> nonulls.Count() Then
                        Dim nullnode As TreeNodeItemSelector = TreeNodeItemSelector.CreateNode(AdvancedDataGridView.Translations(AdvancedDataGridView.TranslationKey.ADGVNodeSelectEmpty.ToString()) & "               ", Nothing, CheckState.Checked, TreeNodeItemSelector.CustomNodeType.SelectEmpty)
                        nullnode.NodeFont = New Font(checkList.Font, FontStyle.Bold)
                        ChecklistAddNode(nullnode)
                    End If

                    'add datetime nodes
                    If DataType Is GetType(Date) Then
                        Dim years = nonulls.GroupBy(Function(year) CDate(year.Value).Year)

                        For Each year As IGrouping(Of Integer, DataGridViewCell) In years
                            Dim yearnode As TreeNodeItemSelector = TreeNodeItemSelector.CreateNode(year.Key.ToString(), year.Key, CheckState.Checked, TreeNodeItemSelector.CustomNodeType.DateTimeNode)
                            ChecklistAddNode(yearnode)

                            Dim months = year.GroupBy(Function(month) CDate(month.Value).Month)

                            For Each month As IGrouping(Of Integer, DataGridViewCell) In months
                                Dim monthnode = yearnode.CreateChildNode(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month.Key), month.Key)

                                Dim days = month.GroupBy(Function(day) CDate(day.Value).Day)

                                For Each day In days
                                    Dim daysnode As TreeNodeItemSelector

                                    If Not IsFilterDateAndTimeEnabled Then
                                        daysnode = monthnode.CreateChildNode(day.Key.ToString("D2"), Enumerable.First(day).Value)
                                    Else
                                        daysnode = monthnode.CreateChildNode(day.Key.ToString("D2"), day.Key)

                                        Dim hours = day.GroupBy(Function(hour) CDate(hour.Value).Hour)

                                        For Each hour As IGrouping(Of Integer, DataGridViewCell) In hours
                                            Dim hoursnode = daysnode.CreateChildNode(hour.Key.ToString("D2") & " " & "h", hour.Key)

                                            Dim mins = hour.GroupBy(Function(min) CDate(min.Value).Minute)

                                            For Each min In mins
                                                Dim minsnode = hoursnode.CreateChildNode(min.Key.ToString("D2") & " " & "m", min.Key)

                                                Dim secs = min.GroupBy(Function(sec) CDate(sec.Value).Second)

                                                For Each sec In secs
                                                    Dim secsnode = minsnode.CreateChildNode(sec.Key.ToString("D2") & " " & "s", Enumerable.First(sec).Value)
                                                Next
                                            Next
                                        Next
                                    End If
                                Next
                            Next
                        Next

                        'add timespan nodes
                    ElseIf DataType Is GetType(TimeSpan) Then
                        Dim days = nonulls.GroupBy(Function(day) CType(day.Value, TimeSpan).Days)

                        For Each day In days
                            Dim daysnode = TreeNodeItemSelector.CreateNode(day.Key.ToString("D2"), day.Key, CheckState.Checked, TreeNodeItemSelector.CustomNodeType.DateTimeNode)
                            ChecklistAddNode(daysnode)

                            Dim hours = day.GroupBy(Function(hour) CType(hour.Value, TimeSpan).Hours)

                            For Each hour As IGrouping(Of Integer, DataGridViewCell) In hours
                                Dim hoursnode = daysnode.CreateChildNode(hour.Key.ToString("D2") & " " & "h", hour.Key)

                                Dim mins = hour.GroupBy(Function(min) CType(min.Value, TimeSpan).Minutes)

                                For Each min In mins
                                    Dim minsnode = hoursnode.CreateChildNode(min.Key.ToString("D2") & " " & "m", min.Key)

                                    Dim secs = min.GroupBy(Function(sec) CType(sec.Value, TimeSpan).Seconds)

                                    For Each sec In secs
                                        Dim secsnode = minsnode.CreateChildNode(sec.Key.ToString("D2") & " " & "s", Enumerable.First(sec).Value)
                                    Next
                                Next
                            Next
                        Next

                        'add boolean nodes
                    ElseIf DataType Is GetType(Boolean) Then
                        Dim values = nonulls.Where(Function(c) CBool(c.Value) = True)

                        If values.Count() <> nonulls.Count() Then
                            Dim node As TreeNodeItemSelector = TreeNodeItemSelector.CreateNode(AdvancedDataGridView.Translations(AdvancedDataGridView.TranslationKey.ADGVNodeSelectFalse.ToString()), False, CheckState.Checked, TreeNodeItemSelector.CustomNodeType.Default)
                            ChecklistAddNode(node)
                        End If

                        If values.Any() Then
                            Dim node As TreeNodeItemSelector = TreeNodeItemSelector.CreateNode(AdvancedDataGridView.Translations(AdvancedDataGridView.TranslationKey.ADGVNodeSelectTrue.ToString()), True, CheckState.Checked, TreeNodeItemSelector.CustomNodeType.Default)
                            ChecklistAddNode(node)
                        End If

                        'ignore image nodes

                        'add string nodes
                    ElseIf DataType Is GetType(Bitmap) Then
                    Else
                        'get custom font by columnName
                        Dim nodeFont As Font = Nothing
                        If dataGridView IsNot Nothing AndAlso Not String.IsNullOrEmpty(columnName) Then nodeFont = dataGridView.Columns(columnName).DefaultCellStyle.Font

                        For Each v In nonulls.GroupBy(Function(c) c.Value).OrderBy(Function(g) g.Key)
                            Dim node As TreeNodeItemSelector = TreeNodeItemSelector.CreateNode(Enumerable.First(v).FormattedValue.ToString(), v.Key, CheckState.Checked, TreeNodeItemSelector.CustomNodeType.Default)
                            If nodeFont IsNot Nothing Then node.NodeFont = nodeFont
                            ChecklistAddNode(node)
                        Next
                    End If
                End If
            End If

            ChecklistReloadNodes()
        End Sub

        ''' <summary>
        ''' Check if filter buttons needs to be enabled
        ''' </summary>
        Private Sub CheckFilterButtonEnabled()
            button_filter.Enabled = HasNodesChecked(_loadedNodes)
        End Sub

        ''' <summary>
        ''' Check if selected nodes exists
        ''' </summary>
        ''' <param name="nodes"></param>
        ''' <returns></returns>
        Private Function HasNodesChecked(nodes As TreeNodeItemSelector()) As Boolean
            Dim state = False
            If Not String.IsNullOrEmpty(checkTextFilter.Text) Then
                state = nodes.Any(Function(n) n.CheckState = CheckState.Checked AndAlso n.Text.ToLower().Contains(checkTextFilter.Text.ToLower()))
            Else
                state = nodes.Any(Function(n) n.CheckState = CheckState.Checked)
            End If

            If state Then Return state

            For Each node In nodes
                For Each nodesel As TreeNodeItemSelector In node.Nodes
                    state = HasNodesChecked(New TreeNodeItemSelector() {nodesel})
                    If state Then Exit For
                Next
                If state Then Exit For
            Next

            Return state
        End Function

        ''' <summary>
        ''' Check
        ''' </summary>
        ''' <param name="node"></param>
        Private Sub NodeCheckChange(node As TreeNodeItemSelector)
            If node.CheckState = CheckState.Checked Then
                node.CheckState = CheckState.Unchecked
            Else
                node.CheckState = CheckState.Checked
            End If

            If node.NodeType = TreeNodeItemSelector.CustomNodeType.SelectAll Then
                SetNodesCheckState(_loadedNodes, node.Checked)
            Else
                If node.Nodes.Count > 0 Then
                    For Each subnode As TreeNodeItemSelector In node.Nodes
                        SetNodesCheckState(New TreeNodeItemSelector() {subnode}, node.Checked)
                    Next
                End If

                'refresh nodes
                Dim state As CheckState = UpdateNodesCheckState(ChecklistNodes())
                GetSelectAllNode().CheckState = state
            End If
        End Sub

        ''' <summary>
        ''' Set Nodes CheckState
        ''' </summary>
        ''' <param name="nodes"></param>
        ''' <param name="isChecked"></param>
        Private Sub SetNodesCheckState(nodes As TreeNodeItemSelector(), isChecked As Boolean)
            For Each node In nodes
                node.Checked = isChecked
                If node.Nodes IsNot Nothing AndAlso node.Nodes.Count > 0 Then
                    For Each subnode As TreeNodeItemSelector In node.Nodes
                        SetNodesCheckState(New TreeNodeItemSelector() {subnode}, isChecked)
                    Next
                End If

            Next
        End Sub

        ''' <summary>
        ''' Update Nodes CheckState recursively
        ''' </summary>
        ''' <param name="nodes"></param>
        ''' <returns></returns>
        Private Function UpdateNodesCheckState(nodes As TreeNodeCollection) As CheckState
            Dim result = CheckState.Unchecked
            Dim isFirstNode = True
            Dim isAllNodesSomeCheckState = True

            For Each n In nodes.OfType(Of TreeNodeItemSelector)()
                If n.NodeType = TreeNodeItemSelector.CustomNodeType.SelectAll Then Continue For

                If n.Nodes.Count > 0 Then
                    n.CheckState = UpdateNodesCheckState(n.Nodes)
                End If

                If isFirstNode Then
                    result = n.CheckState
                    isFirstNode = False
                Else
                    If result <> n.CheckState Then isAllNodesSomeCheckState = False
                End If
            Next

            If isAllNodesSomeCheckState Then
                Return result
            Else
                Return CheckState.Indeterminate
            End If
        End Function

        ''' <summary>
        ''' Get the SelectAll Node
        ''' </summary>
        ''' <returns></returns>
        Private Function GetSelectAllNode() As TreeNodeItemSelector
            Dim result As TreeNodeItemSelector = Nothing
            Dim i = 0
            For Each n In ChecklistNodes().OfType(Of TreeNodeItemSelector)()
                If n.NodeType = TreeNodeItemSelector.CustomNodeType.SelectAll Then
                    result = n
                    Exit For
                ElseIf i > 2 Then
                    Exit For
                Else
                    i += 1
                End If
            Next

            Return result
        End Function

        ''' <summary>
        ''' Get the SelectEmpty Node
        ''' </summary>
        ''' <returns></returns>
        Private Function GetSelectEmptyNode() As TreeNodeItemSelector
            Dim result As TreeNodeItemSelector = Nothing
            Dim i = 0
            For Each n In ChecklistNodes().OfType(Of TreeNodeItemSelector)()
                If n.NodeType = TreeNodeItemSelector.CustomNodeType.SelectEmpty Then
                    result = n
                    Exit For
                ElseIf i > 2 Then
                    Exit For
                Else
                    i += 1
                End If
            Next

            Return result
        End Function

        ''' <summary>
        ''' Duplicate Nodes
        ''' </summary>
        Private Shared Function DuplicateNodes(nodes As TreeNodeItemSelector()) As TreeNodeItemSelector()
            Dim ret = New TreeNodeItemSelector(nodes.Length - 1) {}
            Dim i = 0
            For Each n In nodes
                ret(i) = n.Clone()
                i += 1
            Next
            Return ret
        End Function

#End Region


#Region "checklist filter events"

        ''' <summary>
        ''' CheckList NodeMouseClick event
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub CheckList_NodeMouseClick(sender As Object, e As TreeNodeMouseClickEventArgs)
            Dim HitTestInfo = checkList.HitTest(e.X, e.Y)
            If HitTestInfo IsNot Nothing AndAlso HitTestInfo.Location = TreeViewHitTestLocations.StateImage Then
                'check the node check status
                NodeCheckChange(TryCast(e.Node, TreeNodeItemSelector))
                'set filter button enabled
                CheckFilterButtonEnabled()
            End If
        End Sub

        ''' <summary>
        ''' CheckList KeyDown event
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub CheckList_KeyDown(sender As Object, e As KeyEventArgs)
            If e.KeyCode = Keys.Space Then
                'check the node check status
                NodeCheckChange(TryCast(checkList.SelectedNode, TreeNodeItemSelector))
                'set filter button enabled
                CheckFilterButtonEnabled()
            End If
        End Sub

        ''' <summary>
        ''' CheckList NodeMouseDoubleClick event
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub CheckList_NodeMouseDoubleClick(sender As Object, e As TreeNodeMouseClickEventArgs)
            Dim n As TreeNodeItemSelector = TryCast(e.Node, TreeNodeItemSelector)
            If n IsNot Nothing Then
                'set the new node check status
                SetNodesCheckState(_loadedNodes, False)
                n.CheckState = CheckState.Unchecked
                NodeCheckChange(n)
                'set filter button enabled
                CheckFilterButtonEnabled()
                'do Filter by checkList
                Button_ok_Click(Me, New EventArgs())
            End If
        End Sub

        ''' <summary>
        ''' CheckList MouseEnter event
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub CheckList_MouseEnter(sender As Object, e As EventArgs)
            checkList.Focus()
        End Sub

        ''' <summary>
        ''' CheckList MouseLeave envet
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub CheckList_MouseLeave(sender As Object, e As EventArgs)
            Focus()
        End Sub

        ''' <summary>
        ''' Set the Filter by checkList
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub Button_ok_Click(sender As Object, e As EventArgs)
            _filterclick = True

            SetCheckListFilter()
            Close()
        End Sub

        ''' <summary>
        ''' Undo changed by checkList 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub Button_cancel_Click(sender As Object, e As EventArgs)
            _loadedNodes = DuplicateNodes(_startingNodes)
            Close()
        End Sub

#End Region


#Region "filter methods"

        ''' <summary>
        ''' UnCheck all Custom Filter presets
        ''' </summary>
        Private Sub UnCheckCustomFilters()
            For i = 2 To customFilterLastFiltersListMenuItem.DropDownItems.Count - 1
                TryCast(customFilterLastFiltersListMenuItem.DropDownItems(i), ToolStripMenuItem).Checked = False
            Next
        End Sub

        ''' <summary>
        ''' Set a Custom Filter
        ''' </summary>
        ''' <param name="filtersMenuItemIndex"></param>
        Private Sub SetCustomFilter(filtersMenuItemIndex As Integer)
            If _activeFilterType = FilterType.CheckList Then SetNodesCheckState(_loadedNodes, False)

            Dim filterstring As String = customFilterLastFiltersListMenuItem.DropDownItems(filtersMenuItemIndex).Tag.ToString()
            Dim viewfilterstring = customFilterLastFiltersListMenuItem.DropDownItems(filtersMenuItemIndex).Text

            'do preset jobs
            If filtersMenuItemIndex <> 2 Then
                For i = filtersMenuItemIndex To 3 Step -1
                    customFilterLastFiltersListMenuItem.DropDownItems(i).Text = customFilterLastFiltersListMenuItem.DropDownItems(i - 1).Text
                    customFilterLastFiltersListMenuItem.DropDownItems(i).Tag = customFilterLastFiltersListMenuItem.DropDownItems(i - 1).Tag
                Next

                customFilterLastFiltersListMenuItem.DropDownItems(2).Text = viewfilterstring
                customFilterLastFiltersListMenuItem.DropDownItems(2).Tag = filterstring
            End If

            'uncheck other preset
            For i = 3 To customFilterLastFiltersListMenuItem.DropDownItems.Count - 1
                TryCast(customFilterLastFiltersListMenuItem.DropDownItems(i), ToolStripMenuItem).Checked = False
            Next

            TryCast(customFilterLastFiltersListMenuItem.DropDownItems(2), ToolStripMenuItem).Checked = True
            _activeFilterType = FilterType.Custom

            'get Filter string
            Dim oldfilter = Me.FilterString
            Me.FilterString = filterstring

            'set CheckList nodes
            SetNodesCheckState(_loadedNodes, False)

            customFilterLastFiltersListMenuItem.Checked = True
            button_filter.Enabled = False

            'fire Filter changed
            If Not Equals(oldfilter, Me.FilterString) AndAlso FilterChangedEvent IsNot Nothing Then RaiseEvent FilterChanged(Me, New EventArgs())
        End Sub

#End Region


#Region "filter events"

        ''' <summary>
        ''' Cancel Filter Click event
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub CancelFilterMenuItem_Click(sender As Object, e As EventArgs)
            Dim oldfilter = FilterString

            'clean Filter
            CleanFilter()

            'fire Filter changed
            If Not Equals(oldfilter, FilterString) AndAlso FilterChangedEvent IsNot Nothing Then RaiseEvent FilterChanged(Me, New EventArgs())
        End Sub

        ''' <summary>
        ''' Cancel Filter MouseEnter event
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub CancelFilterMenuItem_MouseEnter(sender As Object, e As EventArgs)
            If TryCast(sender, ToolStripMenuItem).Enabled Then TryCast(sender, ToolStripMenuItem).Select()
        End Sub

        ''' <summary>
        ''' Custom Filter Click event
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub CustomFilterMenuItem_Click(sender As Object, e As EventArgs)
            'ignore image nodes
            If DataType Is GetType(Bitmap) Then Return

            'open a new Custom filter window
            Dim flt As FormCustomFilter = New FormCustomFilter(DataType, IsFilterDateAndTimeEnabled)

            If flt.ShowDialog() = DialogResult.OK Then
                'add the new Filter presets

                Dim filterString = flt.FilterString
                Dim viewFilterString = flt.FilterStringDescription

                Dim index = -1

                For i = 2 To customFilterLastFiltersListMenuItem.DropDownItems.Count - 1
                    If customFilterLastFiltersListMenuItem.DropDown.Items(i).Available Then
                        If Equals(customFilterLastFiltersListMenuItem.DropDownItems(i).Text, viewFilterString) AndAlso Equals(customFilterLastFiltersListMenuItem.DropDownItems(i).Tag.ToString(), filterString) Then
                            index = i
                            Exit For
                        End If
                    Else
                        Exit For
                    End If
                Next

                If index < 2 Then
                    For i = customFilterLastFiltersListMenuItem.DropDownItems.Count - 2 To 2 Step -1
                        If customFilterLastFiltersListMenuItem.DropDownItems(i).Available Then
                            customFilterLastFiltersListMenuItem.DropDownItems(i + 1).Text = customFilterLastFiltersListMenuItem.DropDownItems(i).Text
                            customFilterLastFiltersListMenuItem.DropDownItems(i + 1).Tag = customFilterLastFiltersListMenuItem.DropDownItems(i).Tag
                        End If
                    Next
                    index = 2

                    customFilterLastFiltersListMenuItem.DropDownItems(2).Text = viewFilterString
                    customFilterLastFiltersListMenuItem.DropDownItems(2).Tag = filterString
                End If

                'set the Custom Filter
                SetCustomFilter(index)
            End If
        End Sub

        ''' <summary>
        ''' Custom Filter preset MouseEnter event
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub CustomFilterLastFiltersListMenuItem_MouseEnter(sender As Object, e As EventArgs)
            If TryCast(sender, ToolStripMenuItem).Enabled Then TryCast(sender, ToolStripMenuItem).Select()
        End Sub

        ''' <summary>
        ''' Custom Filter preset MouseEnter event
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub CustomFilterLastFiltersListMenuItem_Paint(sender As Object, e As PaintEventArgs)
            Dim rect As Rectangle = New Rectangle(customFilterLastFiltersListMenuItem.Width - 12, 7, 10, 10)
            ControlPaint.DrawMenuGlyph(e.Graphics, rect, MenuGlyph.Arrow, Color.Black, Color.Transparent)
        End Sub

        ''' <summary>
        ''' Custom Filter preset 1 Visibility changed
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub CustomFilterLastFilter1MenuItem_VisibleChanged(sender As Object, e As EventArgs)
            toolStripSeparator2MenuItem.Visible = Not customFilterLastFilter1MenuItem.Visible
            RemoveHandler TryCast(sender, ToolStripMenuItem).VisibleChanged, AddressOf CustomFilterLastFilter1MenuItem_VisibleChanged
        End Sub

        ''' <summary>
        ''' Custom Filter preset Click event
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub CustomFilterLastFilterMenuItem_Click(sender As Object, e As EventArgs)
            Dim menuitem As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)

            For i = 2 To customFilterLastFiltersListMenuItem.DropDownItems.Count - 1
                If Equals(customFilterLastFiltersListMenuItem.DropDownItems(i).Text, menuitem.Text) AndAlso Equals(customFilterLastFiltersListMenuItem.DropDownItems(i).Tag.ToString(), menuitem.Tag.ToString()) Then
                    'set current filter preset as active
                    SetCustomFilter(i)
                    Exit For
                End If
            Next
        End Sub

        ''' <summary>
        ''' Custom Filter preset TextChanged event
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub CustomFilterLastFilterMenuItem_TextChanged(sender As Object, e As EventArgs)
            TryCast(sender, ToolStripMenuItem).Available = True
            RemoveHandler TryCast(sender, ToolStripMenuItem).TextChanged, AddressOf CustomFilterLastFilterMenuItem_TextChanged
        End Sub

        ''' <summary>
        ''' Text changed timer
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub CheckTextFilterTextChangedTimer_Tick(sender As Object, e As EventArgs)
            Dim timer As Timer = TryCast(sender, Timer)
            If timer Is Nothing Then Return

            CheckTextFilterHandleTextChanged(timer.Tag.ToString())

            timer.Stop()
        End Sub

        ''' <summary>
        ''' Check list filter changer
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub CheckTextFilter_TextChanged(sender As Object, e As EventArgs)
            If Not _checkTextFilterChangedEnabled Then Return

            If _textFilterTextChangedDelayNodes <> TextFilterTextChangedDelayNodesDisabled AndAlso _loadedNodes.Length > _textFilterTextChangedDelayNodes Then
                If _textFilterTextChangedTimer Is Nothing Then
                    _textFilterTextChangedTimer = New Timer()
                    AddHandler _textFilterTextChangedTimer.Tick, New EventHandler(AddressOf CheckTextFilterTextChangedTimer_Tick)
                End If
                _textFilterTextChangedTimer.Stop()
                _textFilterTextChangedTimer.Interval = _textFilterTextChangedDelayMs
                _textFilterTextChangedTimer.Tag = checkTextFilter.Text.ToLower()
                _textFilterTextChangedTimer.Start()
            Else
                CheckTextFilterHandleTextChanged(checkTextFilter.Text.ToLower())
            End If
        End Sub

        ''' <summary>
        ''' Handle check filter text changed
        ''' </summary>
        ''' <param name="text"></param>
        Private Sub CheckTextFilterHandleTextChanged(text As String)
            Dim allnode As TreeNodeItemSelector = TreeNodeItemSelector.CreateNode(AdvancedDataGridView.Translations(AdvancedDataGridView.TranslationKey.ADGVNodeSelectAll.ToString()) & "            ", Nothing, CheckState.Checked, TreeNodeItemSelector.CustomNodeType.SelectAll)
            Dim nullnode As TreeNodeItemSelector = TreeNodeItemSelector.CreateNode(AdvancedDataGridView.Translations(AdvancedDataGridView.TranslationKey.ADGVNodeSelectEmpty.ToString()) & "               ", Nothing, CheckState.Checked, TreeNodeItemSelector.CustomNodeType.SelectEmpty)
            Dim removednodesText = New String() {}
            If _checkTextFilterRemoveNodesOnSearch Then
                removednodesText = _removedsessionNodes.Where(Function(r) Not String.IsNullOrEmpty(r.Text)).[Select](Function(r) r.Text.ToLower()).Distinct().ToArray()
            End If
            For i = _loadedNodes.Length - 1 To 0 Step -1
                Dim node = _loadedNodes(i)
                If Equals(node.Text, allnode.Text) Then
                    node.CheckState = CheckState.Indeterminate
                ElseIf Equals(node.Text, nullnode.Text) Then
                    node.CheckState = CheckState.Unchecked
                Else
                    If node.Text.ToLower().Contains(text) Then
                        node.CheckState = CheckState.Unchecked
                    Else
                        node.CheckState = CheckState.Checked
                    End If
                    If removednodesText.Contains(node.Text.ToLower()) Then node.CheckState = CheckState.Checked
                    NodeCheckChange(node)
                End If
            Next
            'set filter button enabled
            CheckFilterButtonEnabled()
            _removedNodes = _removedsessionNodes
            If _checkTextFilterRemoveNodesOnSearch Then
                For i = _loadedNodes.Length - 1 To 0 Step -1
                    Dim node = _loadedNodes(i)
                    If Not (Equals(node.Text, allnode.Text) OrElse Equals(node.Text, nullnode.Text)) Then
                        If Not node.Text.ToLower().Contains(text) Then
                            _removedNodes = _removedNodes.Concat(New TreeNodeItemSelector() {node}).ToArray()
                        End If
                    End If
                Next
                ChecklistReloadNodes()
            End If
        End Sub

#End Region


#Region "sort events"

        ''' <summary>
        ''' Sort ASC Click event
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub SortASCMenuItem_Click(sender As Object, e As EventArgs)
            'ignore image nodes
            If DataType Is GetType(Bitmap) Then Return

            sortASCMenuItem.Checked = True
            sortDESCMenuItem.Checked = False
            _activeSortType = SortType.ASC

            'get Sort String
            Dim oldsort = SortString
            SortString = "[{0}] ASC"

            'fire Sort Changed
            If Not Equals(oldsort, SortString) AndAlso SortChangedEvent IsNot Nothing Then RaiseEvent SortChanged(Me, New EventArgs())
        End Sub

        ''' <summary>
        ''' Sort ASC MouseEnter event
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub SortASCMenuItem_MouseEnter(sender As Object, e As EventArgs)
            If TryCast(sender, ToolStripMenuItem).Enabled Then TryCast(sender, ToolStripMenuItem).Select()
        End Sub

        ''' <summary>
        ''' Sort DESC Click event
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub SortDESCMenuItem_Click(sender As Object, e As EventArgs)
            'ignore image nodes
            If DataType Is GetType(Bitmap) Then Return

            sortASCMenuItem.Checked = False
            sortDESCMenuItem.Checked = True
            _activeSortType = SortType.DESC

            'get Sort String
            Dim oldsort = SortString
            SortString = "[{0}] DESC"

            'fire Sort Changed
            If Not Equals(oldsort, SortString) AndAlso SortChangedEvent IsNot Nothing Then RaiseEvent SortChanged(Me, New EventArgs())
        End Sub

        ''' <summary>
        ''' Sort DESC MouseEnter event
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub SortDESCMenuItem_MouseEnter(sender As Object, e As EventArgs)
            If TryCast(sender, ToolStripMenuItem).Enabled Then TryCast(sender, ToolStripMenuItem).Select()
        End Sub

        ''' <summary>
        ''' Cancel Sort Click event
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub CancelSortMenuItem_Click(sender As Object, e As EventArgs)
            Dim oldsort = SortString
            'clean Sort
            CleanSort()
            'fire Sort changed
            If Not Equals(oldsort, SortString) AndAlso SortChangedEvent IsNot Nothing Then RaiseEvent SortChanged(Me, New EventArgs())
        End Sub

        ''' <summary>
        ''' Cancel Sort MouseEnter event
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub CancelSortMenuItem_MouseEnter(sender As Object, e As EventArgs)
            If TryCast(sender, ToolStripMenuItem).Enabled Then TryCast(sender, ToolStripMenuItem).Select()
        End Sub

#End Region


#Region "datatype functions"

        ''' <summary>
        ''' Update datatype
        ''' </summary>
        ''' <param name="dataType"></param>
        Friend Sub SetDataType(dataType As Type)
            ' set current datatype
            Me.DataType = dataType

            'set components values
            If dataType Is GetType(Date) OrElse dataType Is GetType(TimeSpan) Then
                customFilterLastFiltersListMenuItem.Text = AdvancedDataGridView.Translations(AdvancedDataGridView.TranslationKey.ADGVCustomFilter.ToString())
                sortASCMenuItem.Text = AdvancedDataGridView.Translations(AdvancedDataGridView.TranslationKey.ADGVSortDateTimeASC.ToString())
                sortDESCMenuItem.Text = AdvancedDataGridView.Translations(AdvancedDataGridView.TranslationKey.ADGVSortDateTimeDESC.ToString())
                sortASCMenuItem.Image = My.Resources.Resources.MenuStrip_OrderASCnum
                sortDESCMenuItem.Image = My.Resources.Resources.MenuStrip_OrderDESCnum
            ElseIf dataType Is GetType(Boolean) Then
                customFilterLastFiltersListMenuItem.Text = AdvancedDataGridView.Translations(AdvancedDataGridView.TranslationKey.ADGVCustomFilter.ToString())
                sortASCMenuItem.Text = AdvancedDataGridView.Translations(AdvancedDataGridView.TranslationKey.ADGVSortBoolASC.ToString())
                sortDESCMenuItem.Text = AdvancedDataGridView.Translations(AdvancedDataGridView.TranslationKey.ADGVSortBoolDESC.ToString())
                sortASCMenuItem.Image = My.Resources.Resources.MenuStrip_OrderASCbool
                sortDESCMenuItem.Image = My.Resources.Resources.MenuStrip_OrderDESCbool
            ElseIf dataType Is GetType(Integer) OrElse dataType Is GetType(Long) OrElse dataType Is GetType(Short) OrElse dataType Is GetType(UInteger) OrElse dataType Is GetType(ULong) OrElse dataType Is GetType(UShort) OrElse dataType Is GetType(Byte) OrElse dataType Is GetType(SByte) OrElse dataType Is GetType(Decimal) OrElse dataType Is GetType(Single) OrElse dataType Is GetType(Double) Then
                customFilterLastFiltersListMenuItem.Text = AdvancedDataGridView.Translations(AdvancedDataGridView.TranslationKey.ADGVCustomFilter.ToString())
                sortASCMenuItem.Text = AdvancedDataGridView.Translations(AdvancedDataGridView.TranslationKey.ADGVSortNumASC.ToString())
                sortDESCMenuItem.Text = AdvancedDataGridView.Translations(AdvancedDataGridView.TranslationKey.ADGVSortNumDESC.ToString())
                sortASCMenuItem.Image = My.Resources.Resources.MenuStrip_OrderASCnum
                sortDESCMenuItem.Image = My.Resources.Resources.MenuStrip_OrderDESCnum
            Else
                customFilterLastFiltersListMenuItem.Text = AdvancedDataGridView.Translations(AdvancedDataGridView.TranslationKey.ADGVCustomFilter.ToString())
                sortASCMenuItem.Text = AdvancedDataGridView.Translations(AdvancedDataGridView.TranslationKey.ADGVSortTextASC.ToString())
                sortDESCMenuItem.Text = AdvancedDataGridView.Translations(AdvancedDataGridView.TranslationKey.ADGVSortTextDESC.ToString())
                sortASCMenuItem.Image = My.Resources.Resources.MenuStrip_OrderASCtxt
                sortDESCMenuItem.Image = My.Resources.Resources.MenuStrip_OrderDESCtxt
            End If

            'set check filter textbox
            If dataType Is GetType(Date) OrElse dataType Is GetType(TimeSpan) OrElse dataType Is GetType(Boolean) Then checkTextFilter.Enabled = False

            'set default components
            customFilterLastFiltersListMenuItem.Enabled = dataType IsNot GetType(Boolean)
            customFilterLastFiltersListMenuItem.Checked = ActiveFilterType = FilterType.Custom
        End Sub

#End Region


#Region "resize methods"

        ''' <summary>
        ''' Get the scaling factor
        ''' </summary>
        ''' <returns></returns>
        Private Function GetScalingFactor() As Single
            Dim ret As Single = 1
            Using Gscale As Graphics = CreateGraphics()
                Try
                    ret = Gscale.DpiX / 96.0F
                Catch
                End Try
            End Using
            Return ret
        End Function

        ''' <summary>
        ''' Scale an item
        ''' </summary>
        ''' <param name="dimesion"></param>
        ''' <param name="factor"></param>
        ''' <returns></returns>
        Private Overloads Shared Function Scale(dimesion As Integer, factor As Single) As Integer
            Return Math.Floor(dimesion * factor)
        End Function

        ''' <summary>
        ''' Resize the box
        ''' </summary>
        ''' <param name="w"></param>
        ''' <param name="h"></param>
        ''' 
        Private Sub ResizeBox(w As Integer, h As Integer)
            sortASCMenuItem.Width = w - 1
            sortDESCMenuItem.Width = w - 1
            cancelSortMenuItem.Width = w - 1
            cancelFilterMenuItem.Width = w - 1
            customFilterMenuItem.Width = w - 1
            customFilterLastFiltersListMenuItem.Width = w - 1
            checkTextFilterControlHost.Width = w - 35

            'scale objects using original width and height
            Dim scalingfactor As Single = GetScalingFactor()
            Dim w2 As Integer = Math.Round(w / scalingfactor, 0)
            Dim h2 As Integer = Math.Round(h / scalingfactor, 0)
            checkFilterListControlHost.Size = New Size(Scale(w2 - 35, scalingfactor), Scale(h2 - 160 - 25, scalingfactor))
            checkFilterListPanel.Size = checkFilterListControlHost.Size
            checkList.Bounds = New Rectangle(Scale(4, scalingfactor), Scale(4, scalingfactor), Scale(w2 - 35 - 8, scalingfactor), Scale(h2 - 160 - 25 - 8, scalingfactor))
            checkFilterListButtonsControlHost.Size = New Size(Scale(w2 - 35, scalingfactor), Scale(24, scalingfactor))
            button_filter.Location = New Point(Scale(w2 - 35 - 164, scalingfactor), 0)
            button_undofilter.Location = New Point(Scale(w2 - 35 - 79, scalingfactor), 0)
            resizeBoxControlHost.Margin = New Padding(Scale(w2 - 46, scalingfactor), 0, 0, 0)

            'get all objects height to make sure we have room for the grip
            Dim finalHeight = sortASCMenuItem.Height + sortDESCMenuItem.Height + cancelSortMenuItem.Height + cancelFilterMenuItem.Height + toolStripSeparator1MenuItem.Height + toolStripSeparator2MenuItem.Height + customFilterLastFiltersListMenuItem.Height + toolStripSeparator3MenuItem.Height + checkFilterListControlHost.Height + checkTextFilterControlHost.Height + checkFilterListButtonsControlHost.Height + resizeBoxControlHost.Height

            ' apply the needed height only when scaled
            If scalingfactor = 1 Then
                Size = New Size(w, h)
            Else
                Size = New Size(w, h + If(finalHeight - h < 0, 0, finalHeight - h))
            End If

        End Sub

        ''' <summary>
        ''' Clean box for Resize
        ''' </summary>
        Private Sub ResizeClean()
            If _resizeEndPoint.X <> -1 Then
                Dim startPoint = PointToScreen(_resizeStartPoint)

                Dim rc As Rectangle = New Rectangle(startPoint.X, startPoint.Y, _resizeEndPoint.X, _resizeEndPoint.Y) With {
    .X = Math.Min(startPoint.X, _resizeEndPoint.X),
    .Width = Math.Abs(startPoint.X - _resizeEndPoint.X),
    .Y = Math.Min(startPoint.Y, _resizeEndPoint.Y),
    .Height = Math.Abs(startPoint.Y - _resizeEndPoint.Y)
}

                ControlPaint.DrawReversibleFrame(rc, Color.Black, FrameStyle.Dashed)

                _resizeEndPoint.X = -1
            End If
        End Sub

#End Region


#Region "resize events"

        ''' <summary>
        ''' Resize MouseDown event
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub ResizeBoxControlHost_MouseDown(sender As Object, e As MouseEventArgs)
            If e.Button = MouseButtons.Left Then
                ResizeClean()
            End If
        End Sub

        ''' <summary>
        ''' Resize MouseMove event
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub ResizeBoxControlHost_MouseMove(sender As Object, e As MouseEventArgs)
            If Visible Then
                If e.Button = MouseButtons.Left Then
                    Dim x = e.X
                    Dim y = e.Y

                    ResizeClean()

                    x += Width - resizeBoxControlHost.Width
                    y += Height - resizeBoxControlHost.Height

                    x = Math.Max(x, MyBase.MinimumSize.Width - 1)
                    y = Math.Max(y, MyBase.MinimumSize.Height - 1)

                    Dim StartPoint = PointToScreen(_resizeStartPoint)
                    Dim EndPoint As Point = PointToScreen(New Point(x, y))

                    Dim rc As Rectangle = New Rectangle With {
    .X = Math.Min(StartPoint.X, EndPoint.X),
    .Width = Math.Abs(StartPoint.X - EndPoint.X),
    .Y = Math.Min(StartPoint.Y, EndPoint.Y),
    .Height = Math.Abs(StartPoint.Y - EndPoint.Y)
}

                    ControlPaint.DrawReversibleFrame(rc, Color.Black, FrameStyle.Dashed)

                    _resizeEndPoint.X = EndPoint.X
                    _resizeEndPoint.Y = EndPoint.Y
                End If
            End If
        End Sub

        ''' <summary>
        ''' Resize MouseUp event
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub ResizeBoxControlHost_MouseUp(sender As Object, e As MouseEventArgs)
            If _resizeEndPoint.X <> -1 Then
                ResizeClean()

                If Visible Then
                    If e.Button = MouseButtons.Left Then
                        Dim newWidth = e.X + Width - resizeBoxControlHost.Width
                        Dim newHeight = e.Y + Height - resizeBoxControlHost.Height

                        newWidth = Math.Max(newWidth, MyBase.MinimumSize.Width)
                        newHeight = Math.Max(newHeight, MyBase.MinimumSize.Height)

                        ResizeBox(newWidth, newHeight)
                    End If
                End If
            End If
        End Sub

        ''' <summary>
        ''' Resize Paint event
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub ResizeBoxControlHost_Paint(sender As Object, e As PaintEventArgs)
            e.Graphics.DrawImage(My.Resources.Resources.MenuStrip_ResizeGrip, 0, 0)
        End Sub

#End Region

    End Class
End Namespace
