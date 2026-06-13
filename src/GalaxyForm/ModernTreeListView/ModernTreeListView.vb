Imports System.ComponentModel
Imports System.Drawing.Drawing2D

Namespace ModernTreeListView

    ''' <summary>
    ''' A modern, high-quality hybrid Tree + ListView control for WinForms (.NET 9+).
    ''' Supports hierarchical data, columns, lazy loading for large datasets, excellent in-place editing,
    ''' clickable column sorting, multi-selection, checkboxes, filtering, type-ahead search, per-node icons,
    ''' tree connector lines, hover highlighting, and light/dark themes.
    ''' </summary>
    ''' <typeparam name="TModel">The type of the data model for each node. For best results with immutable records,
    ''' prefer using <see cref="ReplaceModel"/> after creating an updated instance (see remarks on SetCellValueSetter).</typeparam>
    ''' <remarks>
    ''' https://github.com/kushanp/ModernTreeListView
    ''' </remarks>
    Public NotInheritable Class ModernTreeListView(Of TModel)
        Inherits System.Windows.Forms.Control
        ' Layout constants - modern defaults
        Private Const DefaultRowHeight As Integer = 26
        Private Const DefaultHeaderHeight As Integer = 32
        Private Const IndentSize As Integer = 18
        Private Const ExpanderSize As Integer = 11
        Private Const CheckBoxSize As Integer = 14
        Private Const IconSize As Integer = 16
        Private Const CellPadding As Integer = 6
        Private Const ExpanderMargin As Integer = 3
        Private Const ResizeGripWidth As Integer = 6

        ' State
        Private ReadOnly _columns As List(Of TreeListColumn(Of TModel)) = New List(Of TreeListColumn(Of TModel))()
        Private ReadOnly _rootNodes As List(Of TreeNode) = New List(Of TreeNode)()
        Private ReadOnly _visibleRows As List(Of VisibleRow) = New List(Of VisibleRow)()

        Private _childrenGetter As Func(Of TModel, IEnumerable(Of TModel))
        Private _hasChildrenGetter As Func(Of TModel, Boolean)
        Private _setCellValue As Action(Of TModel, TreeListColumn(Of TModel), Object)
        Private _iconGetter As Func(Of TModel, Drawing.Image)

        ' Selection state (multi-select aware; _selectedNode is the focused node)
        Private ReadOnly _selectedNodes As HashSet(Of TreeNode) = New HashSet(Of TreeNode)()
        Private _selectedNode As TreeNode
        Private _selectedIndex As Integer = -1
        Private _anchorIndex As Integer = -1
        Private _multiSelect As Boolean = True

        Private _rowHeight As Integer = DefaultRowHeight
        Private _headerHeight As Integer = DefaultHeaderHeight

        Private _vOffset As Integer
        Private _hOffset As Integer

        Private _vScrollBar As System.Windows.Forms.VScrollBar = Nothing
        Private _hScrollBar As System.Windows.Forms.HScrollBar = Nothing

        ' Hover / tooltip state
        Private _hoverRowIndex As Integer = -1
        Private _toolTip As System.Windows.Forms.ToolTip = Nothing
        Private _currentToolTipText As String = String.Empty

        ' Column resizing state
        Private _resizingColumnIndex As Integer = -1
        Private _resizeStartX As Integer
        Private _resizeStartWidth As Integer

        ' Hint for which column to start editing on F2/Enter (remembers last interacted column)
        Private _currentEditColumnHint As Integer

        ' Sorting state
        Private _sortColumnIndex As Integer = -1
        Private _sortOrder As SortOrder = SortOrder.None

        ' Filtering state
        Private _filter As Func(Of TModel, Boolean)
        Private _filterMatch As Dictionary(Of TreeNode, Boolean)

        ' Type-ahead search state
        Private _typeAheadPrefix As String = String.Empty
        Private _typeAheadLastTick As Long
        Private Const TypeAheadResetMs As Integer = 1000

        ' Editing state - excellent in-place editing support
        Private _activeEditor As System.Windows.Forms.Control
        Private _editingRowIndex As Integer = -1
        Private _editingColIndex As Integer = -1
        Private _editingNode As TreeNode
        Private _editingOriginalValue As Object
        Private _inEndEdit As Boolean
        Private _suppressFocusCommit As Boolean ' true while a DateTimePicker dropdown is open

        ' Display options
        Private _showCheckBoxes As Boolean
        Private _showTreeLines As Boolean
        Private _autoFillLastColumn As Boolean

        ' Theme
        Private _theme As TreeListTheme = Nothing

        ' Modern visual configuration (clean, minimal)
        ' These are hidden from the WinForms designer because this is a generic control
        ' primarily configured via code / fluent API. Change values programmatically
        ' or use ApplyTheme() with a TreeListTheme preset.
        <Browsable(False)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public Property HeaderBackColor As Drawing.Color

        <Browsable(False)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public Property HeaderForeColor As Drawing.Color

        <Browsable(False)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public Property RowBackColor As Drawing.Color

        <Browsable(False)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public Property AlternatingRowBackColor As Drawing.Color

        <Browsable(False)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public Property SelectionBackColor As Drawing.Color

        <Browsable(False)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public Property SelectionForeColor As Drawing.Color

        <Browsable(False)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public Property GridLineColor As Drawing.Color

        <Browsable(False)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public Property ExpanderColor As Drawing.Color

        <Browsable(False)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public Property TreeLineColor As Drawing.Color

        <Browsable(False)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public Property HoverBackColor As Drawing.Color

        <Browsable(False)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public Property EditorBackColor As Drawing.Color

        <Browsable(False)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public Property EditorForeColor As Drawing.Color

        <Browsable(False)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public Property FocusCueColor As Drawing.Color

        <Browsable(True)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public Property ShowAlternatingRows As Boolean = True

        <Browsable(True)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public Property ShowGridLines As Boolean = False

        <Browsable(True)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public Property FullRowSelect As Boolean = True

        ''' <summary>
        ''' When true, draws connector lines between parent and child rows in the tree column.
        ''' </summary>
        <Browsable(True)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public Property ShowTreeLines As Boolean
            Get
                Return _showTreeLines
            End Get
            Set(value As Boolean)
                If _showTreeLines = value Then Return
                _showTreeLines = value
                Invalidate()
            End Set
        End Property

        ''' <summary>
        ''' When true, shows a checkbox for each row in the tree column.
        ''' Use Space to toggle all selected rows, or click the checkbox directly.
        ''' </summary>
        <Browsable(True)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public Property ShowCheckBoxes As Boolean
            Get
                Return _showCheckBoxes
            End Get
            Set(value As Boolean)
                If _showCheckBoxes = value Then Return
                _showCheckBoxes = value
                Invalidate()
            End Set
        End Property

        ''' <summary>
        ''' When true, the last column stretches to fill any remaining client width
        ''' (it never shrinks below its configured width).
        ''' </summary>
        <Browsable(True)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public Property AutoFillLastColumn As Boolean
            Get
                Return _autoFillLastColumn
            End Get
            Set(value As Boolean)
                If _autoFillLastColumn = value Then Return
                _autoFillLastColumn = value
                UpdateScrollbars()
                Invalidate()
            End Set
        End Property

        ''' <summary>
        ''' Enables multi-selection via Ctrl+Click (toggle), Shift+Click (range) and Shift+Arrow keys.
        ''' When disabled, selection collapses to the focused row. Default: true.
        ''' </summary>
        <Browsable(False)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public Property MultiSelect As Boolean
            Get
                Return _multiSelect
            End Get
            Set(value As Boolean)
                If _multiSelect = value Then Return
                _multiSelect = value
                If Not value AndAlso _selectedNodes.Count > 1 Then
                    _selectedNodes.Clear()
                    If _selectedNode IsNot Nothing Then _selectedNodes.Add(_selectedNode)
                    RaiseEvent SelectionChanged(Me, EventArgs.Empty)
                    Invalidate()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the active theme. Setting this applies all theme colors at once (same as <see cref="ApplyTheme"/>).
        ''' </summary>
        <Browsable(False)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public Property Theme As TreeListTheme
            Get
                Return _theme
            End Get
            Set(value As TreeListTheme)
                ApplyTheme(value)
            End Set
        End Property

        ' Public events
        Public Event CellEditCommitted As EventHandler(Of CellEditEventArgs(Of TModel))
        Public Event CellEditCanceled As EventHandler(Of CellEditEventArgs(Of TModel))
        Public Event SelectionChanged As EventHandler
        Public Event NodeExpanded As EventHandler(Of TreeNodeEventArgs(Of TModel))
        Public Event NodeCollapsed As EventHandler(Of TreeNodeEventArgs(Of TModel))

        ''' <summary>
        ''' Raised when the user changes the sort column or direction (including clearing the sort).
        ''' </summary>
        Public Event ColumnSortChanged As EventHandler

        ''' <summary>
        ''' Raised whenever a node's checked state changes (checkbox click, Space key, or <see cref="SetChecked"/>).
        ''' </summary>
        Public Event CheckedChanged As EventHandler(Of TreeNodeEventArgs(Of TModel))

        Public Sub New()
            SetStyle(System.Windows.Forms.ControlStyles.AllPaintingInWmPaint Or System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer Or System.Windows.Forms.ControlStyles.ResizeRedraw Or System.Windows.Forms.ControlStyles.UserPaint Or System.Windows.Forms.ControlStyles.Selectable, True)

            DoubleBuffered = True
            Font = New Drawing.Font("Segoe UI", 9.5F, Drawing.FontStyle.Regular)

            TabStop = True

            _toolTip = New System.Windows.Forms.ToolTip With {
                    .InitialDelay = 400,
                    .ReshowDelay = 100,
                    .AutoPopDelay = 8000
                }

            ApplyTheme(TreeListTheme.Light)
            InitializeScrollBars()
        End Sub

        Private Sub InitializeScrollBars()
            _vScrollBar = New System.Windows.Forms.VScrollBar With {
        .Visible = False,
        .Width = System.Windows.Forms.SystemInformation.VerticalScrollBarWidth,
        .Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right Or System.Windows.Forms.AnchorStyles.Bottom
    }
            _hScrollBar = New System.Windows.Forms.HScrollBar With {
        .Visible = False,
        .Height = System.Windows.Forms.SystemInformation.HorizontalScrollBarHeight,
        .Anchor = System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right Or System.Windows.Forms.AnchorStyles.Bottom
    }

            Controls.Add(_vScrollBar)
            Controls.Add(_hScrollBar)

            AddHandler _vScrollBar.Scroll, AddressOf OnVScroll
            AddHandler _hScrollBar.Scroll, AddressOf OnHScroll
        End Sub

        Private Sub OnVScroll(sender As Object, e As System.Windows.Forms.ScrollEventArgs)
            CancelEdit() ' editing position would be invalid
            _vOffset = _vScrollBar.Value
            _hoverRowIndex = -1
            Invalidate()
        End Sub

        Private Sub OnHScroll(sender As Object, e As System.Windows.Forms.ScrollEventArgs)
            CancelEdit()
            _hOffset = _hScrollBar.Value
            Invalidate()
        End Sub

        ' ==================== FLUENT API ====================

        ''' <summary>
        ''' Adds a column with fluent configuration support.
        ''' </summary>
        Public Function AddColumn(title As String, getter As Func(Of TModel, Object), Optional width As Integer = 140, Optional configure As Action(Of TreeListColumn(Of TModel)) = Nothing) As ModernTreeListView(Of TModel)
            Dim column = New TreeListColumn(Of TModel)(title, getter, width)
            configure?.Invoke(column)
            _columns.Add(column)
            UpdateScrollbars()
            Invalidate()
            Return Me
        End Function

        ''' <summary>
        ''' Clears all columns.
        ''' </summary>
        Public Function ClearColumns() As ModernTreeListView(Of TModel)
            _columns.Clear()
            UpdateScrollbars()
            Invalidate()
            Return Me
        End Function

        ''' <summary>
        ''' Sets the root models (top level nodes). Children are loaded on demand via the registered children getter.
        ''' This is the primary entry point for populating the control.
        ''' </summary>
        Public Function SetRoots(roots As IEnumerable(Of TModel)) As ModernTreeListView(Of TModel)
            LoadRoots(roots)
            Return Me
        End Function

        ''' <summary>
        ''' Sets the delegate used to retrieve children for any model (enables lazy loading / virtual trees).
        ''' </summary>
        Public Function SetChildrenGetter(getter As Func(Of TModel, IEnumerable(Of TModel))) As ModernTreeListView(Of TModel)
            _childrenGetter = getter
            ' Note: does not auto-reload existing expanded nodes; call Rebuild() if needed
            Return Me
        End Function

        ''' <summary>
        ''' Optional: provides a fast path to know whether a node has children without enumerating (large/virtual datasets).
        ''' </summary>
        Public Function SetHasChildrenGetter(getter As Func(Of TModel, Boolean)) As ModernTreeListView(Of TModel)
            _hasChildrenGetter = getter
            Return Me
        End Function

        ''' <summary>
        ''' Optional: provides a 16x16 icon for each node, drawn in the tree column before the text.
        ''' Return null for nodes without an icon. The control does not take ownership of the images.
        ''' </summary>
        Public Function SetIconGetter(getter As Func(Of TModel, Drawing.Image)) As ModernTreeListView(Of TModel)
            _iconGetter = getter
            Invalidate()
            Return Me
        End Function

        ''' <summary>
        ''' Sets the action invoked when the user commits an in-place edit.
        ''' The action is responsible for writing the value back to the model (or data source).
        ''' </summary>
        ''' <remarks>
        ''' <para><b>Records / immutable models:</b> Because records are immutable, the setter cannot mutate the instance.
        ''' Preferred pattern: create a new record (e.g. <c>model with { Name = (string)newValue }</c>),
        ''' update your source collections so that future <see cref="Reload"/> / children getters see consistent data,
        ''' then call <see cref="ReplaceModel"/> to swap the reference inside the control's tree nodes while preserving
        ''' expand/selection state. See the demo for a complete example.</para>
        ''' <para>For mutable POCOs you can mutate directly and call <see cref="RefreshObject"/> or <see cref="Rebuild"/> as needed.</para>
        ''' </remarks>
        Public Function SetCellValueSetter(setter As Action(Of TModel, TreeListColumn(Of TModel), Object)) As ModernTreeListView(Of TModel)
            _setCellValue = setter
            Return Me
        End Function

        ''' <summary>
        ''' Applies a row filter. A node remains visible when it matches the predicate or any of its descendants do;
        ''' ancestors of matches are automatically expanded so matches become visible.
        ''' </summary>
        ''' <remarks>
        ''' Filtering needs to evaluate descendants, so children are loaded on demand for the whole tree.
        ''' For very large virtual trees consider filtering at the data source instead.
        ''' </remarks>
        Public Function SetFilter(predicate As Func(Of TModel, Boolean)) As ModernTreeListView(Of TModel)
            ArgumentNullException.ThrowIfNull(predicate)

            CancelEdit()
            _filter = predicate
            RecomputeFilterCache()
            AutoExpandFilterMatches()
            RebuildVisibleRows()
            UpdateScrollbars()
            Invalidate()
            Return Me
        End Function

        ''' <summary>
        ''' Removes any active filter. The expansion state created by the filter is kept.
        ''' </summary>
        Public Sub ClearFilter()
            If _filter Is Nothing Then Return

            CancelEdit()
            _filter = Nothing
            _filterMatch = Nothing
            RebuildVisibleRows()
            UpdateScrollbars()
            Invalidate()
        End Sub

        ''' <summary>
        ''' Rebuilds the visible row list from the current expanded state.
        ''' Call this after you have mutated the underlying data (add/remove children, reordered items, etc.)
        ''' while the control's nodes are already loaded. Does not reload children from getters.
        ''' </summary>
        Public Sub Rebuild()
            CancelEdit()
            RebuildVisibleRows()
            UpdateScrollbars()
            Invalidate()
        End Sub

        ''' <summary>
        ''' Fully reloads the tree from the current root models using the registered children getter.
        ''' All expanded state is lost; nodes will be re-expanded on demand when the user expands them again.
        ''' Use when your root collection or children source has changed structurally.
        ''' </summary>
        Public Sub Reload()
            CancelEdit()
            Dim currentRoots = _rootNodes.[Select](Function(n) n.Model).ToList()
            LoadRoots(currentRoots)
        End Sub

        ''' <summary>
        ''' Replaces a model instance in the tree with a different instance (primary use case: immutable records).
        ''' The node keeps its parent, children, expanded state and position in the tree. Selection is preserved.
        ''' After calling this you typically also want to update the same logical item inside your own source collections
        ''' so that <see cref="Reload"/> and future children enumeration remain consistent.
        ''' </summary>
        ''' <param name="oldModel">The model reference currently held by a tree node.</param>
        ''' <param name="newModel">The new model instance that should take its place for display and future operations.</param>
        Public Sub ReplaceModel(oldModel As TModel, newModel As TModel)
            Dim node = FindNode(oldModel)
            If node Is Nothing Then Return

            node.ReplaceModelReference(newModel)

            RebuildVisibleRows()
            UpdateScrollbars()
            Invalidate()
        End Sub

        ''' <summary>
        ''' Invalidates and redraws the row for the specified model if it is currently visible.
        ''' Useful after external changes to a mutable model's display properties when you do not want a full <see cref="Rebuild"/>.
        ''' For immutable records, prefer <see cref="ReplaceModel"/> followed by this (or just ReplaceModel).
        ''' </summary>
        Public Sub RefreshObject(model As TModel)
            Dim node = FindNode(model)
            If node Is Nothing Then Return

            Dim idx = _visibleRows.FindIndex(Function(vr) vr.Node Is node)
            If idx >= 0 Then
                InvalidateRow(idx)
            End If
        End Sub

        ''' <summary>
        ''' Sorts the tree by the specified column using the given direction.
        ''' Sorting is applied to the siblings of each level (per-parent), is stable, and respects the current expanded state.
        ''' </summary>
        ''' <param name="columnIndex">The column to sort by.</param>
        ''' <param name="order">The desired sort direction. Use <see cref="SortOrder.None"/> to clear sorting for this column.</param>
        Public Sub Sort(columnIndex As Integer, order As SortOrder)
            If columnIndex < 0 OrElse columnIndex >= _columns.Count OrElse order = SortOrder.None Then
                ClearSortInternal()
                Return
            End If

            _sortColumnIndex = columnIndex
            _sortOrder = order

            RaiseEvent ColumnSortChanged(Me, EventArgs.Empty)
            RebuildVisibleRows()
            UpdateScrollbars()
            Invalidate()
        End Sub

        ''' <summary>
        ''' Clears any active column sort and restores the original sibling insertion order.
        ''' </summary>
        Public Sub ClearSort()
            ClearSortInternal()
        End Sub

        Private Sub ClearSortInternal()
            If _sortColumnIndex = -1 AndAlso _sortOrder = SortOrder.None Then Return

            _sortColumnIndex = -1
            _sortOrder = SortOrder.None

            RaiseEvent ColumnSortChanged(Me, EventArgs.Empty)
            RebuildVisibleRows()
            UpdateScrollbars()
            Invalidate()
        End Sub

        ''' <summary>
        ''' Applies the given theme: copies all theme colors onto the control's individual color properties.
        ''' Individual properties remain settable afterwards for fine-tuning.
        ''' </summary>
        Public Sub ApplyTheme(theme As TreeListTheme)
            ArgumentNullException.ThrowIfNull(theme)

            _theme = theme

            BackColor = theme.BackColor
            ForeColor = theme.ForeColor
            HeaderBackColor = theme.HeaderBackColor
            HeaderForeColor = theme.HeaderForeColor
            RowBackColor = theme.RowBackColor
            AlternatingRowBackColor = theme.AlternatingRowBackColor
            SelectionBackColor = theme.SelectionBackColor
            SelectionForeColor = theme.SelectionForeColor
            GridLineColor = theme.GridLineColor
            ExpanderColor = theme.ExpanderColor
            TreeLineColor = theme.TreeLineColor
            HoverBackColor = theme.HoverBackColor
            EditorBackColor = theme.EditorBackColor
            EditorForeColor = theme.EditorForeColor
            FocusCueColor = theme.FocusCueColor

            If _toolTip IsNot Nothing Then
                _toolTip.BackColor = theme.RowBackColor
                _toolTip.ForeColor = theme.ForeColor
            End If

            Invalidate()
        End Sub

        ' ==================== PROPERTIES ====================

        Public ReadOnly Property Columns As IReadOnlyList(Of TreeListColumn(Of TModel))
            Get
                Return _columns
            End Get
        End Property

        ''' <summary>
        ''' The model of the focused row, or default when nothing is selected.
        ''' With multi-selection enabled, use <see cref="SelectedModels"/> for the full set.
        ''' </summary>
        Public ReadOnly Property SelectedModel As TModel
            Get
                Dim n = _selectedNode

                If n IsNot Nothing Then
                    Return n.Model
                Else
                    Return Nothing
                End If
            End Get
        End Property

        ''' <summary>
        ''' All currently selected models, in visible row order.
        ''' </summary>
        <Browsable(False)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public ReadOnly Property SelectedModels As IReadOnlyList(Of TModel)
            Get
                If _selectedNodes.Count = 0 Then Return New List(Of TModel)()
                Dim list = New List(Of TModel)(_selectedNodes.Count)
                For Each vr In _visibleRows
                    If _selectedNodes.Contains(vr.Node) Then list.Add(vr.Node.Model)
                Next
                Return list
            End Get
        End Property

        ''' <summary>
        ''' All currently checked models (requires <see cref="ShowCheckBoxes"/> or programmatic <see cref="SetChecked"/> calls).
        ''' Only loaded nodes are considered.
        ''' </summary>
        <Browsable(False)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public ReadOnly Property CheckedModels As IReadOnlyList(Of TModel)
            Get
                Dim list As List(Of TModel) = New List(Of TModel)()

                For Each root In _rootNodes
                    Walk(list, root)
                Next
                Return list
            End Get
        End Property

        Private Sub Walk(list As List(Of TModel), n As TreeNode)
            If n.IsChecked Then list.Add(n.Model)
            If n.ChildrenLoaded Then
                For Each c In n.Children
                    Walk(list, c)
                Next
            End If
        End Sub

        Public ReadOnly Property SelectedRowIndex As Integer
            Get
                Return _selectedIndex
            End Get
        End Property

        <Browsable(False)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public Property RowHeight As Integer
            Get
                Return _rowHeight
            End Get
            Set(value As Integer)
                _rowHeight = Math.Max(16, value)
                UpdateScrollbars()
                Invalidate()
            End Set
        End Property

        <Browsable(False)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public Property HeaderHeight As Integer
            Get
                Return _headerHeight
            End Get
            Set(value As Integer)
                _headerHeight = Math.Max(18, value)
                UpdateScrollbars()
                Invalidate()
            End Set
        End Property

        ''' <summary>
        ''' Gets the zero-based index of the column currently used for sorting, or null if no column sort is active.
        ''' </summary>
        <Browsable(False)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public ReadOnly Property SortColumn As Integer?
            Get
                Return If(_sortColumnIndex >= 0, _sortColumnIndex, Nothing)
            End Get
        End Property

        ''' <summary>
        ''' Gets the current sort direction. When <see cref="SortColumn"/> is null this is <see cref="SortOrder.None"/>.
        ''' </summary>
        <Browsable(False)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public ReadOnly Property SortOrder As SortOrder
            Get
                Return _sortOrder
            End Get
        End Property

        ' ==================== DATA LOADING ====================

        Private Sub LoadRoots(roots As IEnumerable(Of TModel))
            CancelEdit()
            _rootNodes.Clear()
            _visibleRows.Clear()
            _selectedNodes.Clear()
            _selectedNode = Nothing
            _selectedIndex = -1
            _anchorIndex = -1
            _hoverRowIndex = -1

            If roots IsNot Nothing Then
                Dim idx = 0
                For Each model In roots
                    _rootNodes.Add(New TreeNode(model, parent:=Nothing) With {
                            .OriginalIndex = Math.Min(Threading.Interlocked.Increment(idx), idx - 1)
                        })
                Next
            End If

            RebuildVisibleRows()
            UpdateScrollbars()
            Invalidate()
        End Sub

        Private Sub RebuildVisibleRows()
            _visibleRows.Clear()
            _hoverRowIndex = -1

            If _filter IsNot Nothing Then
                RecomputeFilterCache()
            Else
                _filterMatch = Nothing
            End If

            Dim lineage = New List(Of Boolean)()
            Dim orderedRoots = OrderSiblings(_rootNodes).Where(New Func(Of TreeNode, Boolean)(AddressOf IsNodeVisibleUnderFilter)).ToList()
            For i = 0 To orderedRoots.Count - 1
                lineage.Add(i < orderedRoots.Count - 1)
                AppendVisible(orderedRoots(i), level:=0, lineage)
                lineage.RemoveAt(lineage.Count - 1)
            Next

            ' Prune selection: only nodes that are still visible remain selected
            If _selectedNodes.Count > 0 Then
                Dim visibleSet = New HashSet(Of TreeNode)()
                For Each vr In _visibleRows
                    visibleSet.Add(vr.Node)
                Next
                _selectedNodes.RemoveWhere(Function(n) Not visibleSet.Contains(n))
            End If

            If _selectedNode IsNot Nothing Then
                _selectedIndex = _visibleRows.FindIndex(Function(vr) vr.Node Is _selectedNode)
                If _selectedIndex < 0 Then
                    ' Focused node vanished; fall back to the first remaining selected node (if any)
                    Dim fallback = _visibleRows.FindIndex(Function(vr) _selectedNodes.Contains(vr.Node))
                    _selectedNode = If(fallback >= 0, _visibleRows(fallback).Node, Nothing)
                    _selectedIndex = fallback
                End If
            Else
                _selectedIndex = -1
            End If

            If _anchorIndex >= _visibleRows.Count Then _anchorIndex = _visibleRows.Count - 1
        End Sub

        Private Sub AppendVisible(node As TreeNode, level As Integer, lineage As List(Of Boolean))
            _visibleRows.Add(New VisibleRow(node, level, lineage.ToArray()))

            If Not node.IsExpanded Then Return

            node.EnsureChildrenLoaded(_childrenGetter)
            Dim children = OrderSiblings(node.Children).Where(New Func(Of TreeNode, Boolean)(AddressOf IsNodeVisibleUnderFilter)).ToList()
            For i = 0 To children.Count - 1
                lineage.Add(i < children.Count - 1)
                AppendVisible(children(i), level + 1, lineage)
                lineage.RemoveAt(lineage.Count - 1)
            Next
        End Sub

        Private Function IsNodeVisibleUnderFilter(node As TreeNode) As Boolean
            If _filterMatch Is Nothing Then
                Return True
            End If

            Dim match As Boolean
            Dim existed = _filterMatch.TryGetValue(node, match)

            Return existed AndAlso match
        End Function

        Private Function OrderSiblings(nodes As List(Of TreeNode)) As IEnumerable(Of TreeNode)
            If _sortOrder = SortOrder.None OrElse _sortColumnIndex < 0 OrElse _sortColumnIndex >= _columns.Count OrElse nodes.Count <= 1 Then Return nodes

            Dim col = _columns(_sortColumnIndex)

            Return If(_sortOrder = SortOrder.Ascending, nodes.OrderBy(Function(n) col.Getter(n.Model), SortValueComparer.Instance).ThenBy(Function(n) n.OriginalIndex), nodes.OrderByDescending(Function(n) col.Getter(n.Model), SortValueComparer.Instance).ThenBy(Function(n) n.OriginalIndex))
        End Function

        Private Function NodeHasChildren(node As TreeNode) As Boolean
            If node.ChildrenLoaded Then
                If _filterMatch IsNot Nothing Then Return node.Children.Any(New Func(Of TreeNode, Boolean)(AddressOf IsNodeVisibleUnderFilter))
                Return node.Children.Count > 0
            End If

            If _hasChildrenGetter IsNot Nothing Then Return _hasChildrenGetter(node.Model)

            ' Optimistic: allow expand attempt (getter will decide at load time)
            Return _childrenGetter IsNot Nothing
        End Function

        ' ==================== FILTERING ====================

        Private Sub RecomputeFilterCache()
            _filterMatch = New Dictionary(Of TreeNode, Boolean)()
            For Each root In _rootNodes
                ComputeFilterMatch(root)
            Next
        End Sub

        Private Function ComputeFilterMatch(node As TreeNode) As Boolean
            Dim match As Boolean
            Try
                match = _filter(node.Model)
            Catch
                match = False
            End Try

            node.EnsureChildrenLoaded(_childrenGetter)
            For Each child In node.Children
                If ComputeFilterMatch(child) Then match = True
            Next

            _filterMatch(node) = match
            Return match
        End Function

        Private Sub AutoExpandFilterMatches()
            If _filterMatch Is Nothing Then Return



            For Each root In _rootNodes
                Walk(root)
            Next
        End Sub

        Private Sub Walk(node As TreeNode)
            If Not node.ChildrenLoaded Then Return
            Dim anyChildMatches = False
            Dim m As Boolean = Nothing
            For Each child In node.Children
                If _filterMatch.TryGetValue(child, m) AndAlso m Then anyChildMatches = True
                Walk(child)
            Next
            If anyChildMatches Then node.IsExpanded = True
        End Sub

        ' ==================== SCROLLING ====================

        Private Function GetColumnWidth(columnIndex As Integer) As Integer
            Dim col = _columns(columnIndex)
            If _autoFillLastColumn AndAlso columnIndex = _columns.Count - 1 Then
                Dim others = 0
                For c = 0 To _columns.Count - 1 - 1
                    others += _columns(c).Width
                Next

                Dim avail = ClientSize.Width - If(_vScrollBar?.Visible = True, _vScrollBar.Width, 0) - others
                Return Math.Max(col.Width, avail)
            End If
            Return col.Width
        End Function

        Private Function GetTotalColumnsWidth() As Integer
            Dim total = 0
            For c = 0 To _columns.Count - 1
                total += GetColumnWidth(c)
            Next
            Return total
        End Function

        Private Sub UpdateScrollbars()
            If _vScrollBar Is Nothing OrElse _hScrollBar Is Nothing Then Return

            Dim clientWidth = ClientSize.Width
            Dim clientHeight = ClientSize.Height
            Dim header = _headerHeight
            Dim viewHeight = Math.Max(0, clientHeight - header)

            ' Vertical
            Dim totalHeight = _visibleRows.Count * _rowHeight
            Dim needV = totalHeight > viewHeight AndAlso viewHeight > 0

            _vScrollBar.Visible = needV
            If needV Then
                _vScrollBar.Left = clientWidth - _vScrollBar.Width
                _vScrollBar.Top = header
                _vScrollBar.Height = viewHeight

                Dim large = Math.Max(_rowHeight, viewHeight)
                _vScrollBar.LargeChange = large
                _vScrollBar.SmallChange = _rowHeight

                Dim maxVal = Math.Max(0, totalHeight - viewHeight)
                _vScrollBar.Maximum = maxVal + large - 1
                _vScrollBar.Value = Math.Min(_vOffset, maxVal)
                _vOffset = _vScrollBar.Value
            Else
                _vOffset = 0
            End If

            ' Horizontal (computed after vertical so AutoFillLastColumn accounts for the vertical scrollbar)
            Dim totalWidth As Integer = GetTotalColumnsWidth()
            Dim needH = totalWidth > clientWidth AndAlso clientWidth > 0

            _hScrollBar.Visible = needH
            If needH Then
                Dim bottom = clientHeight - _hScrollBar.Height
                _hScrollBar.Left = 0
                _hScrollBar.Top = bottom
                _hScrollBar.Width = clientWidth - If(needV, _vScrollBar.Width, 0)

                Dim large As Integer = Math.Max(10, clientWidth / 3)
                _hScrollBar.LargeChange = large
                _hScrollBar.SmallChange = 20

                Dim maxVal = Math.Max(0, totalWidth - clientWidth)
                _hScrollBar.Maximum = maxVal + large - 1
                _hScrollBar.Value = Math.Min(_hOffset, maxVal)
                _hOffset = _hScrollBar.Value
            Else
                _hOffset = 0
            End If
        End Sub

        Private Sub EnsureRowVisible(rowIndex As Integer)
            If rowIndex < 0 OrElse rowIndex >= _visibleRows.Count Then Return

            Dim rowTop = rowIndex * _rowHeight
            Dim rowBottom = rowTop + _rowHeight
            Dim viewTop = _vOffset
            Dim viewHeight = Math.Max(0, ClientSize.Height - _headerHeight)
            Dim viewBottom = viewTop + viewHeight

            If rowTop < viewTop Then
                _vOffset = rowTop
            ElseIf rowBottom > viewBottom Then
                _vOffset = rowBottom - viewHeight
            End If

            If _vScrollBar.Visible Then
                Dim maxVal = Math.Max(0, _visibleRows.Count * _rowHeight - viewHeight)
                _vScrollBar.Value = Math.Clamp(_vOffset, 0, maxVal)
                _vOffset = _vScrollBar.Value
            End If

            Invalidate()
        End Sub

        ' ==================== HIT TESTING ====================

        Friend Structure HitTestResult
            Public RowIndex As Integer
            Public ColumnIndex As Integer
            Public IsExpander As Boolean
            Public IsCheckBox As Boolean
            Public IsHeader As Boolean
            Public IsValid As Boolean

            Public Sub New(RowIndex As Integer, ColumnIndex As Integer, IsExpander As Boolean, IsCheckBox As Boolean, IsHeader As Boolean, IsValid As Boolean)
                Me.RowIndex = RowIndex
                Me.ColumnIndex = ColumnIndex
                Me.IsValid = IsValid
                Me.IsExpander = IsExpander
                Me.IsCheckBox = IsCheckBox
                Me.IsHeader = IsHeader
            End Sub
        End Structure

        Private Function HitTest(x As Integer, y As Integer) As HitTestResult
            If y < 0 Then Return Nothing

            If y < _headerHeight Then
                ' Header
                Dim colX = -_hOffset
                For c = 0 To _columns.Count - 1
                    Dim w = GetColumnWidth(c)
                    If x >= colX AndAlso x < colX + w Then
                        Return New HitTestResult(-1, c, False, False, True, True)
                    End If
                    colX += w
                Next
                Return New HitTestResult(-1, -1, False, False, True, True)
            End If

            Dim firstRow As Integer = _vOffset / _rowHeight
            Dim rowIndex As Integer = firstRow + (y - _headerHeight + _vOffset Mod _rowHeight) / _rowHeight

            If rowIndex < 0 OrElse rowIndex >= _visibleRows.Count Then Return Nothing

            Dim vrow = _visibleRows(rowIndex)

            ' Compute column
            Dim cellX = -_hOffset
            Dim colIndex = -1
            Dim isExpander = False
            Dim isCheckBox = False

            For c = 0 To _columns.Count - 1
                Dim w = GetColumnWidth(c)
                If x >= cellX AndAlso x < cellX + w Then
                    colIndex = c
                    If c = 0 Then
                        Dim rowTop = _headerHeight + rowIndex * _rowHeight - _vOffset
                        Dim cellRect = New Drawing.Rectangle(cellX, rowTop, w, _rowHeight)
                        Dim layout = GetTreeCellLayout(vrow, cellRect)

                        If layout.HasChildren Then
                            Dim expRect = layout.ExpanderRect
                            expRect.Inflate(3, 3)
                            If expRect.Contains(x, y) Then isExpander = True
                        End If

                        If Not isExpander AndAlso _showCheckBoxes Then
                            Dim cbRect = layout.CheckBoxRect
                            cbRect.Inflate(2, 2)
                            If cbRect.Contains(x, y) Then isCheckBox = True
                        End If
                    End If
                    Exit For
                End If
                cellX += w
            Next

            Return New HitTestResult(rowIndex, colIndex, isExpander, isCheckBox, False, True)
        End Function

        ' ==================== SELECTION ====================

        Private Sub SelectSingle(rowIndex As Integer)
            If rowIndex < 0 OrElse rowIndex >= _visibleRows.Count Then
                ClearSelection()
                Return
            End If

            Dim node = _visibleRows(rowIndex).Node
            _anchorIndex = rowIndex

            Dim unchanged = _selectedNode Is node AndAlso _selectedIndex = rowIndex AndAlso _selectedNodes.Count = 1 AndAlso _selectedNodes.Contains(node)
            If unchanged Then Return

            _selectedNodes.Clear()
            _selectedNodes.Add(node)
            _selectedNode = node
            _selectedIndex = rowIndex
            _currentEditColumnHint = 0

            RaiseEvent SelectionChanged(Me, EventArgs.Empty)
            Invalidate()
        End Sub

        Private Sub ToggleRowSelection(rowIndex As Integer)
            If rowIndex < 0 OrElse rowIndex >= _visibleRows.Count Then Return

            Dim node = _visibleRows(rowIndex).Node
            _anchorIndex = rowIndex

            If _selectedNodes.Add(node) Then
                _selectedNode = node
                _selectedIndex = rowIndex
            Else
                _selectedNodes.Remove(node)
                If _selectedNode Is node Then
                    Dim fallback = _visibleRows.FindIndex(Function(vr) _selectedNodes.Contains(vr.Node))
                    _selectedNode = If(fallback >= 0, _visibleRows(fallback).Node, Nothing)
                    _selectedIndex = fallback
                End If
            End If

            _currentEditColumnHint = 0
            RaiseEvent SelectionChanged(Me, EventArgs.Empty)
            Invalidate()
        End Sub

        Private Sub SelectRange(anchorIndex As Integer, focusIndex As Integer)
            If _visibleRows.Count = 0 Then Return

            anchorIndex = Math.Clamp(anchorIndex, 0, _visibleRows.Count - 1)
            focusIndex = Math.Clamp(focusIndex, 0, _visibleRows.Count - 1)

            Dim lo = Math.Min(anchorIndex, focusIndex)
            Dim hi = Math.Max(anchorIndex, focusIndex)

            Dim newSet = New HashSet(Of TreeNode)()
            For i = lo To hi
                newSet.Add(_visibleRows(i).Node)
            Next

            Dim newFocused = _visibleRows(focusIndex).Node
            Dim changed = _selectedNode IsNot newFocused OrElse Not _selectedNodes.SetEquals(newSet)

            _selectedNodes.Clear()
            _selectedNodes.UnionWith(newSet)
            _selectedNode = newFocused
            _selectedIndex = focusIndex
            _anchorIndex = anchorIndex
            _currentEditColumnHint = 0

            If changed Then RaiseEvent SelectionChanged(Me, EventArgs.Empty)
            Invalidate()
        End Sub

        Private Sub SelectAllRows()
            If _visibleRows.Count = 0 Then Return

            _selectedNodes.Clear()
            For Each vr In _visibleRows
                _selectedNodes.Add(vr.Node)
            Next

            If _selectedNode Is Nothing OrElse _selectedIndex < 0 Then
                _selectedNode = _visibleRows(0).Node
                _selectedIndex = 0
            End If
            If _anchorIndex < 0 Then _anchorIndex = 0

            RaiseEvent SelectionChanged(Me, EventArgs.Empty)
            Invalidate()
        End Sub

        Private Sub MoveFocusTo(rowIndex As Integer, extendRange As Boolean)
            If rowIndex < 0 OrElse rowIndex >= _visibleRows.Count Then Return

            If extendRange AndAlso _multiSelect AndAlso _anchorIndex >= 0 Then
                SelectRange(_anchorIndex, rowIndex)
            Else
                SelectSingle(rowIndex)
            End If

            EnsureRowVisible(rowIndex)
        End Sub

        ''' <summary>
        ''' Selects the given model (expands all ancestor nodes as needed so the item becomes visible and selected).
        ''' </summary>
        Public Sub SelectModel(model As TModel)
            ' Find the node in the currently visible tree (depth-first search)
            Dim node = FindNode(model)
            If node Is Nothing Then Return

            ' Ensure all ancestors are expanded so the node becomes visible
            Me.ExpandAncestors(node)

            RebuildVisibleRows()
            UpdateScrollbars()

            Dim idx = _visibleRows.FindIndex(Function(vr) vr.Node Is node)
            If idx >= 0 Then
                SelectSingle(idx)
                EnsureRowVisible(idx)
            End If
        End Sub

        ''' <summary>
        ''' Clears the current selection.
        ''' </summary>
        Public Sub ClearSelection()
            If _selectedNode Is Nothing AndAlso _selectedNodes.Count = 0 Then Return
            _selectedNodes.Clear()
            _selectedNode = Nothing
            _selectedIndex = -1
            _anchorIndex = -1
            RaiseEvent SelectionChanged(Me, EventArgs.Empty)
            Invalidate()
        End Sub

        Private Function FindNode(model As TModel) As TreeNode
            For Each root In _rootNodes
                Dim found = FindNodeRecursive(root, model)
                If found IsNot Nothing Then Return found
            Next
            Return Nothing
        End Function

        Private Function FindNodeRecursive(node As TreeNode, model As TModel) As TreeNode
            If EqualityComparer(Of TModel).Default.Equals(node.Model, model) Then Return node

            ' Only search loaded children (avoids forcing load of entire tree)
            If node.ChildrenLoaded Then
                For Each child In node.Children
                    Dim found = FindNodeRecursive(child, model)
                    If found IsNot Nothing Then Return found
                Next
            End If
            Return Nothing
        End Function

        Private Sub ExpandAncestors(node As TreeNode)
            Dim current = node.Parent
            While current IsNot Nothing
                If Not current.IsExpanded Then
                    current.EnsureChildrenLoaded(_childrenGetter)
                    current.IsExpanded = True
                    RaiseEvent NodeExpanded(Me, New TreeNodeEventArgs(Of TModel)(current.Model, current))
                End If
                current = current.Parent
            End While
        End Sub

        ' ==================== CHECKBOXES ====================

        ''' <summary>
        ''' Sets the checked state of the node for the given model. Raises <see cref="CheckedChanged"/> when the state changes.
        ''' </summary>
        Public Sub SetChecked(model As TModel, isChecked As Boolean)
            Dim node = FindNode(model)
            If node Is Nothing Then Return

            Me.SetCheckedCore(node, isChecked)
            Dim idx = _visibleRows.FindIndex(Function(vr) vr.Node Is node)
            If idx >= 0 Then InvalidateRow(idx)
        End Sub

        Private Sub SetCheckedCore(node As TreeNode, isChecked As Boolean)
            If node.IsChecked = isChecked Then Return
            node.IsChecked = isChecked
            RaiseEvent CheckedChanged(Me, New TreeNodeEventArgs(Of TModel)(node.Model, node))
        End Sub

        Private Sub ToggleCheckedForSelection()
            If _selectedNodes.Count = 0 Then Return

            Dim target = Not If(_selectedNode, Enumerable.First(_selectedNodes)).IsChecked
            For Each node In _selectedNodes
                SetCheckedCore(node, target)
            Next

            Invalidate()
        End Sub

        ' ==================== EXPAND / COLLAPSE ====================

        Private Sub ToggleExpand(node As TreeNode)
            If Not NodeHasChildren(node) Then Return

            If Not node.IsExpanded Then
                node.EnsureChildrenLoaded(_childrenGetter)
                node.IsExpanded = True
                RaiseEvent NodeExpanded(Me, New TreeNodeEventArgs(Of TModel)(node.Model, node))
            Else
                node.IsExpanded = False
                RaiseEvent NodeCollapsed(Me, New TreeNodeEventArgs(Of TModel)(node.Model, node))
            End If

            RebuildVisibleRows()
            UpdateScrollbars()
            Invalidate()
        End Sub

        ''' <summary>
        ''' Expands the node for the given model (loads children on demand if necessary).
        ''' </summary>
        Public Sub Expand(model As TModel)
            Dim node = FindNode(model)
            If node Is Nothing OrElse node.IsExpanded Then Return

            Me.ExpandAncestors(node)
            node.EnsureChildrenLoaded(_childrenGetter)
            node.IsExpanded = True
            RaiseEvent NodeExpanded(Me, New TreeNodeEventArgs(Of TModel)(node.Model, node))

            RebuildVisibleRows()
            UpdateScrollbars()
            Invalidate()
        End Sub

        ''' <summary>
        ''' Collapses the node for the given model.
        ''' </summary>
        Public Sub Collapse(model As TModel)
            Dim node = FindNode(model)
            If node Is Nothing OrElse Not node.IsExpanded Then Return

            node.IsExpanded = False
            RaiseEvent NodeCollapsed(Me, New TreeNodeEventArgs(Of TModel)(node.Model, node))

            RebuildVisibleRows()
            UpdateScrollbars()
            Invalidate()
        End Sub

        ''' <summary>
        ''' Expands every node in the tree, loading children on demand as needed.
        ''' Per-node <see cref="NodeExpanded"/> events are not raised for bulk expansion.
        ''' </summary>
        Public Sub ExpandAll()
            CancelEdit()
            For Each root In _rootNodes
                ExpandNodeRecursive(root)
            Next

            RebuildVisibleRows()
            UpdateScrollbars()
            Invalidate()
        End Sub

        ''' <summary>
        ''' Collapses every loaded node in the tree.
        ''' Per-node <see cref="NodeCollapsed"/> events are not raised for bulk collapse.
        ''' </summary>
        Public Sub CollapseAll()
            CancelEdit()
            For Each root In _rootNodes
                CollapseNodeRecursive(root)
            Next

            RebuildVisibleRows()
            UpdateScrollbars()
            Invalidate()
        End Sub

        ''' <summary>
        ''' Expands the node for the given model and its entire subtree (loads children on demand).
        ''' Bound to the * key for the selected row.
        ''' </summary>
        Public Sub ExpandSubtree(model As TModel)
            Dim node = FindNode(model)
            If node Is Nothing Then Return

            CancelEdit()
            Me.ExpandAncestors(node)
            Me.ExpandNodeRecursive(node)

            RebuildVisibleRows()
            UpdateScrollbars()
            Invalidate()
        End Sub

        Private Sub ExpandNodeRecursive(node As TreeNode)
            node.EnsureChildrenLoaded(_childrenGetter)
            If node.Children.Count = 0 Then Return

            node.IsExpanded = True
            For Each child In node.Children
                ExpandNodeRecursive(child)
            Next
        End Sub

        Private Shared Sub CollapseNodeRecursive(node As TreeNode)
            node.IsExpanded = False
            If node.ChildrenLoaded Then
                For Each child In node.Children
                    CollapseNodeRecursive(child)
                Next
            End If
        End Sub

        ' ==================== IN-PLACE EDITING (Excellent support) ====================

        ''' <summary>
        ''' Begins in-place editing for the cell at the given visible row and column index.
        ''' The default editor is a TextBox; DateTime values get a DateTimePicker, bool values get a CheckBox.
        ''' Columns can supply a custom editor via <see cref="TreeListColumn(Of TModel).EditorFactory"/>.
        ''' Commit with Enter or by losing focus; cancel with Escape. Tab / Shift+Tab move to the next / previous editable cell.
        ''' </summary>
        Public Sub BeginEdit(rowIndex As Integer, columnIndex As Integer)
            If rowIndex < 0 OrElse rowIndex >= _visibleRows.Count Then Return
            If columnIndex < 0 OrElse columnIndex >= _columns.Count Then Return

            Dim column = _columns(columnIndex)
            If Not column.IsEditable Then Return

            CancelEdit()

            Dim vrow = _visibleRows(rowIndex)
            Dim node = vrow.Node

            _editingRowIndex = rowIndex
            _editingColIndex = columnIndex
            _editingNode = node

            Dim currentValue = column.Getter(node.Model)
            _editingOriginalValue = currentValue

            Dim editor = If(column.EditorFactory IsNot Nothing, column.EditorFactory(node.Model, currentValue), CreateDefaultEditor(column, node.Model, currentValue))
            If editor Is Nothing Then
                EndEdit()
                Return
            End If

            Dim cellRect = GetCellRectangle(rowIndex, columnIndex)
            If columnIndex = 0 Then
                ' Do not cover the expander / checkbox / icon area of the tree cell
                Dim layout = GetTreeCellLayout(vrow, cellRect)
                Dim delta = Math.Max(0, layout.ContentLeft - cellRect.Left)
                cellRect.X += delta
                cellRect.Width = Math.Max(20, cellRect.Width - delta)
            End If

            ' Inset slightly for modern look
            cellRect.Inflate(-1, -1)
            If cellRect.Width < 20 Then cellRect.Width = 20

            editor.Bounds = cellRect
            editor.Font = Font
            editor.Tag = New EditTag(column, node, currentValue)

            _activeEditor = editor
            _suppressFocusCommit = False
            Controls.Add(editor)
            editor.BringToFront()
            editor.Focus()

            Dim tb As System.Windows.Forms.TextBox = TryCast(editor, System.Windows.Forms.TextBox)
            If tb IsNot Nothing Then tb.SelectAll()

            Dim dtp As System.Windows.Forms.DateTimePicker = TryCast(editor, System.Windows.Forms.DateTimePicker)

            If dtp IsNot Nothing Then
                AddHandler dtp.DropDown, AddressOf Editor_DropDown
                AddHandler dtp.CloseUp, AddressOf Editor_CloseUp
            End If

            AddHandler editor.PreviewKeyDown, AddressOf Me.Editor_PreviewKeyDown
            AddHandler editor.KeyDown, AddressOf Me.Editor_KeyDown
            AddHandler editor.LostFocus, AddressOf Me.Editor_LostFocus

            Invalidate() ' in case we want to highlight editing cell
        End Sub

        Private Function CreateDefaultEditor(column As TreeListColumn(Of TModel), model As TModel, currentValue As Object) As System.Windows.Forms.Control
            ' Improved default editor with basic type awareness for a better out-of-the-box experience.
            ' DateTime and DateTime? -> DateTimePicker (excellent for dates)
            ' bool / bool? -> CheckBox
            ' Everything else (strings, numbers, etc.) -> TextBox.
            ' Keyboard: arrows, Home, End etc. work inside the editors because we only intercept Enter/Escape/Tab.

            ' DateTime (non-nullable or nullable with value)
            Dim dtVal As Date? = If(TypeOf currentValue Is Date, CDate(currentValue), Nothing)

            If dtVal IsNot Nothing Then
                Return New System.Windows.Forms.DateTimePicker With {
        .Format = System.Windows.Forms.DateTimePickerFormat.Short,
        .Value = dtVal,
        .CalendarMonthBackground = EditorBackColor,
        .CalendarForeColor = EditorForeColor
    }
            End If

            ' bool
            Dim boolVal As Boolean? = If(TypeOf currentValue Is Boolean, CBool(currentValue), Nothing)

            If boolVal IsNot Nothing Then
                Return New System.Windows.Forms.CheckBox With {
        .Checked = boolVal,
        .BackColor = EditorBackColor,
        .ForeColor = EditorForeColor,
        .Text = column.Title,
        .AutoSize = True
    }
            End If

            ' Default: TextBox (great for strings, numbers, GUIDs, etc.)
            ' Right-align for common numeric types (detected from current value for a nicer look).
            Dim text = If(currentValue?.ToString(), String.Empty)
            Dim numeric = IsNumericType(currentValue)

            Dim tb = New System.Windows.Forms.TextBox With {
        .Text = text,
        .BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle,
        .BackColor = EditorBackColor,
        .ForeColor = EditorForeColor,
        .Padding = New System.Windows.Forms.Padding(2),
        .TextAlign = If(numeric, System.Windows.Forms.HorizontalAlignment.Right, System.Windows.Forms.HorizontalAlignment.Left)
    }

            Return tb
        End Function

        Private Shared Function IsNumericType(value As Object) As Boolean
            If value Is Nothing Then Return False
            Dim t = value.GetType()
            t = If(Nullable.GetUnderlyingType(t), t)
            Return t Is GetType(SByte) OrElse t Is GetType(Byte) OrElse t Is GetType(Short) OrElse t Is GetType(UShort) OrElse t Is GetType(Integer) OrElse t Is GetType(UInteger) OrElse t Is GetType(Long) OrElse t Is GetType(ULong) OrElse t Is GetType(Single) OrElse t Is GetType(Double) OrElse t Is GetType(Decimal)
        End Function

        Private Sub Editor_PreviewKeyDown(sender As Object, e As System.Windows.Forms.PreviewKeyDownEventArgs)
            ' Tab is normally swallowed as a dialog/navigation key before KeyDown fires.
            ' Declaring it an input key lets Editor_KeyDown handle Tab / Shift+Tab cell navigation.
            If e.KeyCode = System.Windows.Forms.Keys.Tab Then e.IsInputKey = True
        End Sub

        Private Sub Editor_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs)
            If e.KeyCode = System.Windows.Forms.Keys.Enter Then
                e.Handled = True
                e.SuppressKeyPress = True
                CommitEdit()
            ElseIf e.KeyCode = System.Windows.Forms.Keys.Escape Then
                e.Handled = True
                e.SuppressKeyPress = True
                CancelEdit()
            ElseIf e.KeyCode = System.Windows.Forms.Keys.Tab Then
                e.Handled = True
                e.SuppressKeyPress = True

                ' Capture position before commit resets editing state
                Dim row = _editingRowIndex
                Dim col = _editingColIndex
                Dim direction = If(e.Shift, -1, +1)

                CommitEdit()
                MoveToAdjacentEditableCell(row, col, direction)
            End If
        End Sub

        Private Sub Editor_DropDown(sender As Object, e As EventArgs)
            ' The DateTimePicker's calendar dropdown takes focus; do not treat that as "editing finished".
            _suppressFocusCommit = True
        End Sub

        Private Sub Editor_CloseUp(sender As Object, e As EventArgs)
            _suppressFocusCommit = False
        End Sub

        Private Sub Editor_LostFocus(sender As Object, e As EventArgs)
            If _suppressFocusCommit Then Return

            ' Commit on focus lost (standard for excellent editing experience)
            If _activeEditor IsNot Nothing Then
                CommitEdit()
            End If
        End Sub

        Private Sub CommitEdit()
            If _inEndEdit Then Return
            If _activeEditor Is Nothing OrElse _editingNode Is Nothing OrElse _editingColIndex < 0 Then
                EndEdit()
                Return
            End If

            _inEndEdit = True

            Dim column = _columns(_editingColIndex)
            Dim model = _editingNode.Model

            Dim newValue = Me.ExtractValueFromEditor(_activeEditor, column)

            Dim args = New CellEditEventArgs(Of TModel)(model, column, newValue, _editingOriginalValue, _editingRowIndex, _editingColIndex)

            Try
                RaiseEvent CellEditCommitted(Me, args)

                If Not args.Cancel Then
                    ' Apply via registered setter if present (user is responsible for updating model)
                    _setCellValue?.Invoke(model, column, newValue)

                    ' If the edit was on a visible column, we may want to refresh row
                    InvalidateRow(_editingRowIndex)
                Else
                    RaiseEvent CellEditCanceled(Me, args)
                End If

            Finally
                EndEdit()
                _inEndEdit = False
            End Try
        End Sub

        Private Sub CancelEdit()
            If _inEndEdit Then Return
            If _activeEditor Is Nothing Then Return

            _inEndEdit = True
            Try
                If _editingNode IsNot Nothing AndAlso _editingColIndex >= 0 Then
                    Dim args = New CellEditEventArgs(Of TModel)(_editingNode.Model, _columns(_editingColIndex), Nothing, _editingOriginalValue, _editingRowIndex, _editingColIndex) With {
.Cancel = True
}

                    RaiseEvent CellEditCanceled(Me, args)
                End If

            Finally
                EndEdit()
                _inEndEdit = False
            End Try
        End Sub

        Private Sub EndEdit()

            Dim dtp As System.Windows.Forms.DateTimePicker = TryCast(_activeEditor, System.Windows.Forms.DateTimePicker)
            If _activeEditor IsNot Nothing Then
                RemoveHandler _activeEditor.LostFocus, AddressOf Me.Editor_LostFocus
                RemoveHandler _activeEditor.KeyDown, AddressOf Me.Editor_KeyDown
                RemoveHandler _activeEditor.PreviewKeyDown, AddressOf Me.Editor_PreviewKeyDown

                If dtp IsNot Nothing Then
                    RemoveHandler dtp.DropDown, AddressOf Editor_DropDown
                    RemoveHandler dtp.CloseUp, AddressOf Editor_CloseUp
                End If

                If Controls.Contains(_activeEditor) Then Controls.Remove(_activeEditor)

                _activeEditor.Dispose()
                _activeEditor = Nothing
            End If

            _editingRowIndex = -1
            _editingColIndex = -1
            _editingNode = Nothing
            _editingOriginalValue = Nothing
            _suppressFocusCommit = False

            Focus()
            Invalidate()
        End Sub

        Private Function ExtractValueFromEditor(editor As System.Windows.Forms.Control, column As TreeListColumn(Of TModel)) As Object
            If column.EditorValueExtractor IsNot Nothing Then Return column.EditorValueExtractor(editor)

            ' For default TextBox we return the string. Setter can parse.
            Return Nothing
        End Function

        Private Function FirstEditableColumn() As Integer
            For c = 0 To _columns.Count - 1
                If _columns(c).IsEditable Then Return c
            Next
            Return -1
        End Function

        Private Sub MoveToAdjacentEditableCell(fromRow As Integer, fromCol As Integer, direction As Integer)
            If fromRow < 0 OrElse fromCol < 0 OrElse _columns.Count = 0 Then Return

            Dim r = fromRow
            Dim c = fromCol

            While True
                c += direction
                If c >= _columns.Count Then
                    c = 0
                    r += 1
                ElseIf c < 0 Then
                    c = _columns.Count - 1
                    r -= 1
                End If

                If r < 0 OrElse r >= _visibleRows.Count Then Return

                If _columns(c).IsEditable Then
                    SelectSingle(r)
                    EnsureRowVisible(r)
                    BeginEdit(r, c)
                    Return
                End If

                ' Terminates: r strictly progresses every _columns.Count iterations
            End While
        End Sub

        Private Sub InvalidateRow(rowIndex As Integer)
            If rowIndex < 0 Then Return
            Dim y = _headerHeight + rowIndex * _rowHeight - _vOffset
            Invalidate(New Drawing.Rectangle(0, y, ClientSize.Width, _rowHeight))
        End Sub

        ' ==================== GEOMETRY HELPERS ====================

        Private Function GetCellRectangle(rowIndex As Integer, columnIndex As Integer) As Drawing.Rectangle
            If rowIndex < 0 OrElse columnIndex < 0 OrElse columnIndex >= _columns.Count Then Return Drawing.Rectangle.Empty

            Dim colX = -_hOffset
            For c = 0 To columnIndex - 1
                colX += GetColumnWidth(c)
            Next

            Dim colWidth = GetColumnWidth(columnIndex)

            Dim rowY = _headerHeight + rowIndex * _rowHeight - _vOffset

            Return New Drawing.Rectangle(colX, rowY, colWidth, _rowHeight)
        End Function

        Private Function GetColumnStartX(columnIndex As Integer) As Integer
            Dim x = -_hOffset
            For c = 0 To columnIndex - 1
                x += GetColumnWidth(c)
            Next
            Return x
        End Function

        Friend Structure TreeCellLayout

            Public HasChildren As Boolean
            Public ExpanderRect As Drawing.Rectangle
            Public CheckBoxRect As Drawing.Rectangle
            Public IconRect As Drawing.Rectangle
            Public Icon As Drawing.Image
            Public ContentLeft As Integer

            Public Sub New(HasChildren As Boolean, ExpanderRect As Drawing.Rectangle, CheckBoxRect As Drawing.Rectangle, IconRect As Drawing.Rectangle, Icon As Drawing.Image, ContentLeft As Integer)
                Me.HasChildren = HasChildren
                Me.ExpanderRect = ExpanderRect
                Me.CheckBoxRect = CheckBoxRect
                Me.IconRect = IconRect
                Me.Icon = Icon
                Me.ContentLeft = ContentLeft
            End Sub
        End Structure

        Private Function GetTreeCellLayout(vrow As VisibleRow, cellRect As Drawing.Rectangle) As TreeCellLayout
            Dim hasChildren = NodeHasChildren(vrow.Node)
            Dim x = cellRect.Left + vrow.Level * IndentSize + ExpanderMargin

            Dim expanderRect = Drawing.Rectangle.Empty
            If hasChildren Then
                expanderRect = New Drawing.Rectangle(x, cellRect.Top + (cellRect.Height - ExpanderSize) / 2, ExpanderSize, ExpanderSize)
            End If
            x += If(hasChildren, ExpanderSize + 4, 4)

            Dim checkRect = Drawing.Rectangle.Empty
            If _showCheckBoxes Then
                checkRect = New Drawing.Rectangle(x, cellRect.Top + (cellRect.Height - CheckBoxSize) / 2, CheckBoxSize, CheckBoxSize)
                x += CheckBoxSize + 5
            End If

            Dim icon As Drawing.Image = Nothing
            Dim iconRect = Drawing.Rectangle.Empty
            If _iconGetter IsNot Nothing Then
                icon = _iconGetter(vrow.Node.Model)
                If icon IsNot Nothing Then
                    iconRect = New Drawing.Rectangle(x, cellRect.Top + (cellRect.Height - IconSize) / 2, IconSize, IconSize)
                    x += IconSize + 4
                End If
            End If

            Return New TreeCellLayout(hasChildren, expanderRect, checkRect, iconRect, icon, x)
        End Function

        ' ==================== COLUMN AUTO-FIT ====================

        ''' <summary>
        ''' Resizes the column so its header and all currently realized rows fit without truncation.
        ''' Also triggered by double-clicking a column divider in the header.
        ''' </summary>
        Public Sub AutoFitColumn(columnIndex As Integer)
            If columnIndex < 0 OrElse columnIndex >= _columns.Count Then Return

            Dim col = _columns(columnIndex)

            ' Header text + room for the sort glyph
            Dim max = System.Windows.Forms.TextRenderer.MeasureText(col.Title, Font).Width + CellPadding * 2 + 18

            For Each vrow In _visibleRows
                Dim text = GetDisplayText(vrow.Node.Model, col)
                If text.Length = 0 Then Continue For

                Dim w = System.Windows.Forms.TextRenderer.MeasureText(text, Font).Width + CellPadding * 2
                If columnIndex = 0 Then
                    w += vrow.Level * IndentSize + ExpanderMargin + ExpanderSize + 4
                    If _showCheckBoxes Then w += CheckBoxSize + 5
                    If _iconGetter IsNot Nothing Then w += IconSize + 4
                End If
                If w > max Then max = w
            Next

            col.Width = Math.Max(col.MinWidth, max)
            UpdateScrollbars()
            Invalidate()
        End Sub

        ' ==================== PAINTING (Modern clean look) ====================

        Protected Overrides Sub OnPaint(e As System.Windows.Forms.PaintEventArgs)
            Dim g = e.Graphics
            g.Clear(BackColor)

            Dim width = ClientSize.Width
            Dim height = ClientSize.Height

            ' 1. Header
            DrawHeader(g, width)

            ' 2. Rows
            DrawRows(g, width, height)

            ' 3. Borders / finishing
            Dim borderPen = New Drawing.Pen(GridLineColor)
            g.DrawLine(borderPen, 0, _headerHeight - 1, width, _headerHeight - 1)

            ' Focus cue on whole control when focused (subtle)
            If Focused AndAlso _selectedIndex >= 0 Then
                Dim focusPen = New Drawing.Pen(FocusCueColor) With {
                        .DashStyle = DashStyle.Dot
                    }
                g.DrawRectangle(focusPen, 0, 0, width - 1, height - 1)
            End If
        End Sub

        Private Sub DrawHeader(g As Drawing.Graphics, clientWidth As Integer)
            Dim headerRect = New Drawing.Rectangle(0, 0, clientWidth, _headerHeight)

            Dim headerBrush = New Drawing.SolidBrush(HeaderBackColor)
            g.FillRectangle(headerBrush, headerRect)

            Dim linePen = New Drawing.Pen(GridLineColor)

            Dim x = -_hOffset

            For c = 0 To _columns.Count - 1
                Dim col = _columns(c)
                Dim colWidth = GetColumnWidth(c)
                Dim colRect = New Drawing.Rectangle(x, 0, colWidth, _headerHeight)

                If colRect.Right > 0 AndAlso colRect.Left < clientWidth Then
                    ' Column separator (subtle)
                    g.DrawLine(linePen, colRect.Right - 1, 4, colRect.Right - 1, _headerHeight - 5)

                    ' Title + optional sort indicator
                    Dim isSortedCol = _sortColumnIndex = c AndAlso _sortOrder <> SortOrder.None
                    Dim sortGlyph = If(isSortedCol, If(_sortOrder = SortOrder.Ascending, "▲", "▼"), "")

                    Dim textRightPadding = If(isSortedCol, 18, CellPadding)
                    Dim textRect = New Drawing.Rectangle(colRect.Left + CellPadding, colRect.Top, Math.Max(4, colRect.Width - CellPadding - textRightPadding), colRect.Height)

                    System.Windows.Forms.TextRenderer.DrawText(g, col.Title, Font, textRect, HeaderForeColor, System.Windows.Forms.TextFormatFlags.VerticalCenter Or System.Windows.Forms.TextFormatFlags.Left Or System.Windows.Forms.TextFormatFlags.EndEllipsis Or System.Windows.Forms.TextFormatFlags.PreserveGraphicsClipping)

                    If isSortedCol Then
                        ' Draw sort indicator on the right side of the header cell (subtle, modern)
                        Dim glyphRect = New Drawing.Rectangle(colRect.Right - 16, colRect.Top, 14, colRect.Height)

                        System.Windows.Forms.TextRenderer.DrawText(g, sortGlyph, Font, glyphRect, HeaderForeColor, System.Windows.Forms.TextFormatFlags.VerticalCenter Or System.Windows.Forms.TextFormatFlags.HorizontalCenter Or System.Windows.Forms.TextFormatFlags.PreserveGraphicsClipping)
                    End If
                End If

                x += colWidth
            Next

            ' Right edge line if needed
            g.DrawLine(linePen, 0, _headerHeight - 1, clientWidth, _headerHeight - 1)
        End Sub

        Private Sub DrawRows(g As Drawing.Graphics, clientWidth As Integer, clientHeight As Integer)
            If _visibleRows.Count = 0 Then Return

            Dim firstRow As Integer = Math.Max(0, _vOffset / _rowHeight)
            Dim rowPixelY = _headerHeight - _vOffset Mod _rowHeight

            Dim gridPen = New Drawing.Pen(GridLineColor)

            Dim r = firstRow

            While r < _visibleRows.Count AndAlso rowPixelY < clientHeight
                Dim vrow = _visibleRows(r)
                Dim isSelected = _selectedNodes.Contains(vrow.Node)
                Dim isHover = r = _hoverRowIndex AndAlso Not isSelected
                Dim isAlt = ShowAlternatingRows AndAlso r Mod 2 = 1

                Dim rowRect = New Drawing.Rectangle(0, rowPixelY, clientWidth, _rowHeight)

                ' Row background (selection > hover > alternating > normal)
                Dim bg = If(isSelected, SelectionBackColor, If(isHover, HoverBackColor, If(isAlt, AlternatingRowBackColor, RowBackColor)))
                Using b = New Drawing.SolidBrush(bg)
                    g.FillRectangle(b, rowRect)
                End Using

                ' Draw cells
                Dim cellX = -_hOffset
                For c = 0 To _columns.Count - 1
                    Dim col = _columns(c)
                    Dim colW = GetColumnWidth(c)
                    Dim cellRect = New Drawing.Rectangle(cellX, rowPixelY, colW, _rowHeight)

                    If cellRect.Right > 0 AndAlso cellRect.Left < clientWidth Then
                        If c = 0 Then
                            DrawTreeCell(g, cellRect, vrow, col, isSelected)
                        Else
                            DrawDataCell(g, cellRect, vrow, col, isSelected)
                        End If
                    End If

                    ' Vertical grid line
                    If ShowGridLines Then
                        g.DrawLine(gridPen, cellRect.Right - 1, rowPixelY + 2, cellRect.Right - 1, rowPixelY + _rowHeight - 3)
                    End If

                    cellX += colW
                Next

                ' Bottom grid line for row
                If ShowGridLines Then
                    g.DrawLine(gridPen, 0, rowPixelY + _rowHeight - 1, clientWidth, rowPixelY + _rowHeight - 1)
                End If

                rowPixelY += _rowHeight
                r += 1
            End While
        End Sub

        Private Sub DrawTreeCell(g As Drawing.Graphics, cellRect As Drawing.Rectangle, vrow As VisibleRow, column As TreeListColumn(Of TModel), isSelected As Boolean)
            Dim node = vrow.Node
            Dim layout = GetTreeCellLayout(vrow, cellRect)

            If _showTreeLines AndAlso vrow.Level > 0 Then
                DrawTreeLines(g, cellRect, vrow, layout)
            End If

            If layout.HasChildren Then
                DrawModernExpander(g, layout.ExpanderRect, node.IsExpanded, isSelected)
            End If

            If _showCheckBoxes Then
                DrawCheckBox(g, layout.CheckBoxRect, node.IsChecked, isSelected)
            End If

            If layout.Icon IsNot Nothing Then
                g.DrawImage(layout.Icon, layout.IconRect)
            End If

            ' Text
            Dim text = GetDisplayText(node.Model, column)
            Dim textColor = If(isSelected, SelectionForeColor, ForeColor)

            Dim textRect = New Drawing.Rectangle(layout.ContentLeft, cellRect.Top, Math.Max(4, cellRect.Right - layout.ContentLeft - CellPadding), cellRect.Height)

            System.Windows.Forms.TextRenderer.DrawText(g, text, Font, textRect, textColor, System.Windows.Forms.TextFormatFlags.VerticalCenter Or System.Windows.Forms.TextFormatFlags.Left Or System.Windows.Forms.TextFormatFlags.EndEllipsis Or System.Windows.Forms.TextFormatFlags.PreserveGraphicsClipping)
        End Sub

        Private Sub DrawTreeLines(g As Drawing.Graphics, cellRect As Drawing.Rectangle, vrow As VisibleRow, layout As TreeCellLayout)
            Dim pen = New Drawing.Pen(TreeLineColor)
            Dim midY As Integer = cellRect.Top + cellRect.Height / 2

            ' Pass-through vertical lines for ancestors that have following siblings
            ' (level 0 is skipped: no root-level connector lines for a cleaner look)
            For i = 1 To vrow.Level - 1
                If vrow.AncestorsHaveNext(i) Then
                    Dim lx As Integer = cellRect.Left + i * IndentSize + ExpanderMargin + ExpanderSize / 2
                    g.DrawLine(pen, lx, cellRect.Top, lx, cellRect.Bottom)
                End If
            Next

            ' This node's own connector elbow
            Dim ex As Integer = cellRect.Left + vrow.Level * IndentSize + ExpanderMargin + ExpanderSize / 2
            g.DrawLine(pen, ex, cellRect.Top, ex, midY)
            If vrow.AncestorsHaveNext(vrow.Level) Then
                g.DrawLine(pen, ex, midY, ex, cellRect.Bottom)
            End If

            ' Horizontal stub to the content for leaf nodes (parents have the chevron at the junction)
            If Not layout.HasChildren Then
                g.DrawLine(pen, CSng(ex), CSng(midY), CSng(ex + ExpanderSize / 2 + 3), CSng(midY))
            End If
        End Sub

        Private Sub DrawDataCell(g As Drawing.Graphics, cellRect As Drawing.Rectangle, vrow As VisibleRow, column As TreeListColumn(Of TModel), isSelected As Boolean)
            Dim text = GetDisplayText(vrow.Node.Model, column)
            Dim textColor = If(isSelected, SelectionForeColor, ForeColor)

            ' Respect column alignment (simple mapping)
            Dim flags = System.Windows.Forms.TextFormatFlags.VerticalCenter Or System.Windows.Forms.TextFormatFlags.EndEllipsis Or System.Windows.Forms.TextFormatFlags.PreserveGraphicsClipping
            If column.Alignment = System.Windows.Forms.HorizontalAlignment.Right Then
                flags = flags Or System.Windows.Forms.TextFormatFlags.Right
            ElseIf column.Alignment = System.Windows.Forms.HorizontalAlignment.Center Then
                flags = flags Or System.Windows.Forms.TextFormatFlags.HorizontalCenter
            Else
                flags = flags Or System.Windows.Forms.TextFormatFlags.Left
            End If

            Dim textRect = New Drawing.Rectangle(cellRect.Left + CellPadding, cellRect.Top, Math.Max(4, cellRect.Width - CellPadding * 2), cellRect.Height)

            System.Windows.Forms.TextRenderer.DrawText(g, text, Font, textRect, textColor, flags)
        End Sub

        Private Sub DrawModernExpander(g As Drawing.Graphics, rect As Drawing.Rectangle, expanded As Boolean, selected As Boolean)
            ' Clean modern chevron/triangle style (no box)
            Dim color = If(selected, SelectionForeColor, ExpanderColor)
            Dim pen = New Drawing.Pen(color, 1.6F)

            Dim cx As Integer = rect.Left + rect.Width / 2
            Dim cy As Integer = rect.Top + rect.Height / 2
            Dim sz = 3

            If expanded Then
                ' Down chevron
                g.DrawLine(pen, cx - sz, cy - 1, cx, cy + sz - 1)
                g.DrawLine(pen, cx, cy + sz - 1, cx + sz, cy - 1)
            Else
                ' Right chevron
                g.DrawLine(pen, cx - 1, cy - sz, cx + sz - 1, cy)
                g.DrawLine(pen, cx + sz - 1, cy, cx - 1, cy + sz)
            End If
        End Sub

        Private Sub DrawCheckBox(g As Drawing.Graphics, rect As Drawing.Rectangle, isChecked As Boolean, isSelected As Boolean)
            Dim borderColor = If(isSelected, SelectionForeColor, ExpanderColor)

            Using fill = New Drawing.SolidBrush(If(isSelected, Drawing.Color.FromArgb(40, 255, 255, 255), RowBackColor))
                g.FillRectangle(fill, rect)
            End Using
            Using pen = New Drawing.Pen(borderColor, 1.2F)
                g.DrawRectangle(pen, rect.X, rect.Y, rect.Width - 1, rect.Height - 1)
            End Using

            If isChecked Then
                Dim markColor = If(isSelected, SelectionForeColor, SelectionBackColor)
                Dim markPen = New Drawing.Pen(markColor, 1.8F)
                Dim oldMode = g.SmoothingMode
                g.SmoothingMode = SmoothingMode.AntiAlias
                g.DrawLine(markPen, CSng(rect.X + 3), CSng(rect.Y + rect.Height / 2), CSng(rect.X + rect.Width / 2 - 1), CSng(rect.Y + rect.Height - 4))
                g.DrawLine(markPen, CSng(rect.X + rect.Width / 2 - 1), CSng(rect.Y + rect.Height - 4), CSng(rect.X + rect.Width - 3), CSng(rect.Y + 3))
                g.SmoothingMode = oldMode
            End If
        End Sub

        Private Function GetDisplayText(model As TModel, column As TreeListColumn(Of TModel)) As String
            Dim value = column.Getter(model)
            If column.Formatter IsNot Nothing Then Return If(column.Formatter(value), String.Empty)
            Return If(value Is Nothing, "", value.ToString())
        End Function

        ' ==================== INPUT HANDLING ====================

        Protected Overrides Sub OnMouseDown(e As System.Windows.Forms.MouseEventArgs)
            MyBase.OnMouseDown(e)
            Focus()

            CancelEdit() ' any pending edit commit/cancel before new action

            Dim hit = HitTest(e.X, e.Y)

            If hit.IsHeader Then
                HandleHeaderMouseDown(e, hit)
                Return
            End If

            If hit.RowIndex >= 0 AndAlso hit.IsValid Then
                Dim vrow = _visibleRows(hit.RowIndex)

                If hit.IsExpander Then
                    ToggleExpand(vrow.Node)
                    Return
                End If

                If hit.IsCheckBox Then
                    SetCheckedCore(vrow.Node, Not vrow.Node.IsChecked)
                    InvalidateRow(hit.RowIndex)
                    Return
                End If

                Dim ctrl = (ModifierKeys And System.Windows.Forms.Keys.Control) = System.Windows.Forms.Keys.Control
                Dim shift = (ModifierKeys And System.Windows.Forms.Keys.Shift) = System.Windows.Forms.Keys.Shift

                If e.Button = System.Windows.Forms.MouseButtons.Right Then
                    ' Right-click selects the row only when it is not already part of the selection
                    ' (so context menus can operate on a multi-selection)
                    If Not _selectedNodes.Contains(vrow.Node) Then SelectSingle(hit.RowIndex)
                ElseIf _multiSelect AndAlso shift AndAlso _anchorIndex >= 0 Then
                    SelectRange(_anchorIndex, hit.RowIndex)
                ElseIf _multiSelect AndAlso ctrl Then
                    ToggleRowSelection(hit.RowIndex)
                Else
                    SelectSingle(hit.RowIndex)
                End If

                ' For single click on data cell we just select (excellent editing uses F2 / double-click)
                _currentEditColumnHint = If(hit.ColumnIndex >= 0, hit.ColumnIndex, 0)
            End If
        End Sub

        Protected Overrides Sub OnMouseDoubleClick(e As System.Windows.Forms.MouseEventArgs)
            MyBase.OnMouseDoubleClick(e)

            If e.Y >= 0 AndAlso e.Y < _headerHeight Then
                ' Double-click on a column divider auto-fits that column
                Dim x = -_hOffset
                For c = 0 To _columns.Count - 1
                    x += GetColumnWidth(c)
                    If Math.Abs(e.X - x) <= ResizeGripWidth Then
                        _resizingColumnIndex = -1
                        Cursor = System.Windows.Forms.Cursors.Default
                        AutoFitColumn(c)
                        Return
                    End If
                Next
                Return
            End If

            Dim hit = HitTest(e.X, e.Y)
            If hit.RowIndex >= 0 AndAlso Not hit.IsExpander AndAlso Not hit.IsCheckBox AndAlso hit.IsValid Then
                Dim col = If(hit.ColumnIndex >= 0, hit.ColumnIndex, 0)
                BeginEdit(hit.RowIndex, col)
            End If
        End Sub

        Private Sub HandleHeaderMouseDown(e As System.Windows.Forms.MouseEventArgs, hit As HitTestResult)
            If e.Button <> System.Windows.Forms.MouseButtons.Left OrElse hit.ColumnIndex < 0 Then Return

            ' Check for column resize grip
            Dim colRight = GetColumnStartX(hit.ColumnIndex) + GetColumnWidth(hit.ColumnIndex)

            If Math.Abs(e.X - colRight) <= ResizeGripWidth Then
                _resizingColumnIndex = hit.ColumnIndex
                _resizeStartX = e.X
                _resizeStartWidth = GetColumnWidth(hit.ColumnIndex)
                Cursor = System.Windows.Forms.Cursors.VSplit
            Else
                ' Header click (not on grip) -> toggle sort for this column
                ToggleSortOnColumn(hit.ColumnIndex)
            End If
        End Sub

        Private Sub ToggleSortOnColumn(columnIndex As Integer)
            If columnIndex < 0 OrElse columnIndex >= _columns.Count Then Return

            Dim nextOrder As SortOrder

            If _sortColumnIndex <> columnIndex Then
                ' Different column: start with ascending
                nextOrder = SortOrder.Ascending
            Else
                ' Same column: cycle Asc -> Desc -> None
                Select Case _sortOrder
                    Case SortOrder.None
                        nextOrder = SortOrder.Ascending
                    Case SortOrder.Ascending
                        nextOrder = SortOrder.Descending
                    Case Else
                        nextOrder = SortOrder.None
                End Select
            End If

            Sort(columnIndex, nextOrder)
        End Sub

        Protected Overrides Sub OnMouseMove(e As System.Windows.Forms.MouseEventArgs)
            MyBase.OnMouseMove(e)

            If _resizingColumnIndex >= 0 Then
                Dim col = _columns(_resizingColumnIndex)
                Dim delta = e.X - _resizeStartX
                col.Width = Math.Max(col.MinWidth, _resizeStartWidth + delta)
                UpdateScrollbars()
                Invalidate()
                Return
            End If

            ' Update cursor for resize affordance in header
            If e.Y < _headerHeight Then
                SetHoverRow(-1)
                UpdateToolTip(Nothing)

                Dim hit = HitTest(e.X, e.Y)
                If hit.ColumnIndex >= 0 Then
                    Dim colRight = GetColumnStartX(hit.ColumnIndex) + GetColumnWidth(hit.ColumnIndex)
                    If Math.Abs(e.X - colRight) <= 5 Then
                        Cursor = System.Windows.Forms.Cursors.VSplit
                        Return
                    End If
                End If

                Cursor = System.Windows.Forms.Cursors.Default
                Return
            End If

            Cursor = System.Windows.Forms.Cursors.Default

            Dim rowHit = HitTest(e.X, e.Y)
            SetHoverRow(If(Not rowHit.IsHeader AndAlso rowHit.IsValid, rowHit.RowIndex, -1))
            UpdateToolTip(rowHit)
        End Sub

        Protected Overrides Sub OnMouseLeave(e As EventArgs)
            MyBase.OnMouseLeave(e)
            SetHoverRow(-1)
            UpdateToolTip(Nothing)
        End Sub

        Private Sub SetHoverRow(rowIndex As Integer)
            If rowIndex = _hoverRowIndex Then Return
            Dim old = _hoverRowIndex
            _hoverRowIndex = rowIndex
            If old >= 0 Then InvalidateRow(old)
            If rowIndex >= 0 Then InvalidateRow(rowIndex)
        End Sub

        Private Sub UpdateToolTip(hit As HitTestResult)
            Dim text = String.Empty

            If hit.IsValid AndAlso Not hit.IsHeader AndAlso hit.RowIndex >= 0 AndAlso hit.RowIndex < _visibleRows.Count AndAlso hit.ColumnIndex >= 0 AndAlso hit.ColumnIndex < _columns.Count Then
                Dim vrow = _visibleRows(hit.RowIndex)
                Dim column = _columns(hit.ColumnIndex)
                Dim display = GetDisplayText(vrow.Node.Model, column)

                If display.Length > 0 Then
                    Dim cellRect = GetCellRectangle(hit.RowIndex, hit.ColumnIndex)
                    Dim available As Integer
                    If hit.ColumnIndex = 0 Then
                        Dim layout = GetTreeCellLayout(vrow, cellRect)
                        available = cellRect.Right - layout.ContentLeft - CellPadding
                    Else
                        available = cellRect.Width - CellPadding * 2
                    End If

                    Dim needed = System.Windows.Forms.TextRenderer.MeasureText(display, Font).Width
                    If needed > available Then text = display
                End If
            End If

            If Not Equals(text, _currentToolTipText) Then
                _currentToolTipText = text
                _toolTip.SetToolTip(Me, If(text.Length = 0, Nothing, text))
            End If
        End Sub

        Protected Overrides Sub OnMouseUp(e As System.Windows.Forms.MouseEventArgs)
            MyBase.OnMouseUp(e)

            If _resizingColumnIndex >= 0 Then
                _resizingColumnIndex = -1
                Cursor = System.Windows.Forms.Cursors.Default
                UpdateScrollbars()
                Invalidate()
            End If
        End Sub

        Protected Overrides Sub OnMouseWheel(e As System.Windows.Forms.MouseEventArgs)
            MyBase.OnMouseWheel(e)

            ' Shift + wheel scrolls horizontally
            If (ModifierKeys And System.Windows.Forms.Keys.Shift) = System.Windows.Forms.Keys.Shift Then
                If _hScrollBar.Visible Then
                    CancelEdit()
                    Dim maxVal As Integer = Math.Max(0, GetTotalColumnsWidth() - ClientSize.Width)
                    _hOffset = Math.Clamp(_hOffset - Math.Sign(e.Delta) * 48, 0, maxVal)
                    _hScrollBar.Value = Math.Min(_hOffset, maxVal)
                    _hoverRowIndex = -1
                    Invalidate()
                End If
                Return
            End If

            If _vScrollBar.Visible Then
                CancelEdit()
                Dim newVal As Integer = _vOffset - e.Delta / 2 ' natural feel
                Dim maxVal = Math.Max(0, _visibleRows.Count * _rowHeight - Math.Max(1, ClientSize.Height - _headerHeight))
                _vOffset = Math.Clamp(newVal, 0, maxVal)

                If _vScrollBar.Visible Then
                    _vScrollBar.Value = _vOffset
                End If

                _hoverRowIndex = -1
                Invalidate()
            End If
        End Sub

        Protected Overrides Function IsInputKey(keyData As System.Windows.Forms.Keys) As Boolean
            ' Navigation keys must reach OnKeyDown instead of being treated as dialog keys
            Select Case keyData And System.Windows.Forms.Keys.KeyCode
                Case System.Windows.Forms.Keys.Up, System.Windows.Forms.Keys.Down, System.Windows.Forms.Keys.Left, System.Windows.Forms.Keys.Right, System.Windows.Forms.Keys.Home, System.Windows.Forms.Keys.End, System.Windows.Forms.Keys.PageUp, System.Windows.Forms.Keys.PageDown, System.Windows.Forms.Keys.Enter
                    Return True
            End Select
            Return MyBase.IsInputKey(keyData)
        End Function

        Protected Overrides Sub OnKeyDown(e As System.Windows.Forms.KeyEventArgs)
            MyBase.OnKeyDown(e)

            If _visibleRows.Count = 0 Then Return

            Select Case e.KeyCode
                Case System.Windows.Forms.Keys.F2, System.Windows.Forms.Keys.Enter
                    If _selectedIndex >= 0 Then
                        Dim col As Integer = If(_currentEditColumnHint >= 0 AndAlso _currentEditColumnHint < _columns.Count AndAlso _columns(_currentEditColumnHint).IsEditable, _currentEditColumnHint, FirstEditableColumn())
                        If col >= 0 Then BeginEdit(_selectedIndex, col)
                        e.Handled = True
                        e.SuppressKeyPress = True
                    End If

                Case System.Windows.Forms.Keys.Up
                    If _selectedIndex > 0 Then
                        MoveFocusTo(_selectedIndex - 1, e.Shift)
                    ElseIf _selectedIndex < 0 Then
                        MoveFocusTo(0, False)
                    End If
                    e.Handled = True

                Case System.Windows.Forms.Keys.Down
                    If _selectedIndex >= 0 AndAlso _selectedIndex < _visibleRows.Count - 1 Then
                        MoveFocusTo(_selectedIndex + 1, e.Shift)
                    ElseIf _selectedIndex = -1 Then
                        MoveFocusTo(0, False)
                    End If
                    e.Handled = True

                Case System.Windows.Forms.Keys.Left
                    If _selectedIndex >= 0 Then
                        Dim node = _visibleRows(_selectedIndex).Node
                        If node.IsExpanded Then
                            ToggleExpand(node)
                        ElseIf node.Parent IsNot Nothing Then
                            ' Go to parent
                            Dim parentIdx = _visibleRows.FindIndex(Function(vr) vr.Node Is node.Parent)
                            If parentIdx >= 0 Then
                                SelectSingle(parentIdx)
                                EnsureRowVisible(parentIdx)
                            End If
                        End If
                    End If
                    e.Handled = True

                Case System.Windows.Forms.Keys.Right
                    If _selectedIndex >= 0 Then
                        Dim node = _visibleRows(_selectedIndex).Node
                        If Not node.IsExpanded AndAlso NodeHasChildren(node) Then
                            ToggleExpand(node)
                        ElseIf node.IsExpanded AndAlso node.Children.Count > 0 Then
                            ' Go to first child
                            Dim childIdx = _visibleRows.FindIndex(Function(vr) vr.Node.Parent Is node)
                            If childIdx >= 0 Then
                                SelectSingle(childIdx)
                                EnsureRowVisible(childIdx)
                            End If
                        End If
                    End If
                    e.Handled = True

                Case System.Windows.Forms.Keys.PageUp
                    Dim page As Integer = Math.Max(1, (ClientSize.Height - _headerHeight) / _rowHeight)
                    Dim newIdx = Math.Max(0, _selectedIndex - page)
                    MoveFocusTo(newIdx, e.Shift)
                    e.Handled = True

                Case System.Windows.Forms.Keys.PageDown
                    Dim page As Integer = Math.Max(1, (ClientSize.Height - _headerHeight) / _rowHeight)
                    Dim newIdx = Math.Min(_visibleRows.Count - 1, _selectedIndex + page)
                    MoveFocusTo(newIdx, e.Shift)
                    e.Handled = True

                Case System.Windows.Forms.Keys.Home
                    MoveFocusTo(0, e.Shift)
                    e.Handled = True

                Case System.Windows.Forms.Keys.End
                    MoveFocusTo(_visibleRows.Count - 1, e.Shift)
                    e.Handled = True

                Case System.Windows.Forms.Keys.Multiply
                    If _selectedIndex >= 0 Then
                        ExpandSubtree(_visibleRows(_selectedIndex).Node.Model)
                    End If
                    e.Handled = True
                    e.SuppressKeyPress = True

                Case System.Windows.Forms.Keys.Add
                    If _selectedIndex >= 0 Then
                        Dim node = _visibleRows(_selectedIndex).Node
                        If Not node.IsExpanded AndAlso NodeHasChildren(node) Then ToggleExpand(node)
                    End If
                    e.Handled = True
                    e.SuppressKeyPress = True

                Case System.Windows.Forms.Keys.Subtract
                    If _selectedIndex >= 0 Then
                        Dim node = _visibleRows(_selectedIndex).Node
                        If node.IsExpanded Then ToggleExpand(node)
                    End If
                    e.Handled = True
                    e.SuppressKeyPress = True

                Case System.Windows.Forms.Keys.Space
                    If _showCheckBoxes AndAlso _selectedNodes.Count > 0 Then
                        ToggleCheckedForSelection()
                        e.Handled = True
                        e.SuppressKeyPress = True
                    End If

                Case System.Windows.Forms.Keys.A
                    If e.Control AndAlso _multiSelect Then
                        SelectAllRows()
                        e.Handled = True
                        e.SuppressKeyPress = True
                    End If
            End Select
        End Sub

        Protected Overrides Sub OnKeyPress(e As System.Windows.Forms.KeyPressEventArgs)
            MyBase.OnKeyPress(e)

            ' Type-ahead search on the tree (first) column
            If e.Handled OrElse _columns.Count = 0 OrElse _visibleRows.Count = 0 Then Return

            Dim ch = e.KeyChar
            If Char.IsControl(ch) OrElse Not Char.IsLetterOrDigit(ch) Then Return

            Dim now = Environment.TickCount64
            If now - _typeAheadLastTick > TypeAheadResetMs Then _typeAheadPrefix = String.Empty
            _typeAheadLastTick = now
            _typeAheadPrefix += ch

            ' A fresh single-char prefix searches from the next row; a growing prefix re-matches the current row first
            Dim start = If(_typeAheadPrefix.Length = 1, _selectedIndex + 1, Math.Max(0, _selectedIndex))
            If start < 0 Then start = 0

            Dim firstColumn = _columns(0)
            For offset As Integer = 0 To _visibleRows.Count - 1
                Dim idx = (start + offset) Mod _visibleRows.Count
                Dim text = GetDisplayText(_visibleRows(idx).Node.Model, firstColumn)
                If text.StartsWith(_typeAheadPrefix, StringComparison.CurrentCultureIgnoreCase) Then
                    SelectSingle(idx)
                    EnsureRowVisible(idx)
                    Exit For
                End If
            Next

            e.Handled = True
        End Sub

        ' ==================== LAYOUT & RESIZE ====================

        Protected Overrides Sub OnResize(e As EventArgs)
            MyBase.OnResize(e)
            UpdateScrollbars()
        End Sub

        Protected Overrides Sub OnLayout(levent As System.Windows.Forms.LayoutEventArgs)
            MyBase.OnLayout(levent)
            UpdateScrollbars()
        End Sub

        ' ==================== FOCUS ====================

        Protected Overrides Sub OnGotFocus(e As EventArgs)
            MyBase.OnGotFocus(e)
            Invalidate()
        End Sub

        Protected Overrides Sub OnLostFocus(e As EventArgs)
            MyBase.OnLostFocus(e)
            Invalidate()
        End Sub

        ' ==================== CLEANUP ====================

        Protected Overrides Sub Dispose(disposing As Boolean)
            If disposing Then
                _activeEditor?.Dispose()
                _toolTip?.Dispose()
                ' Scrollbars are disposed by Controls collection
            End If
            MyBase.Dispose(disposing)
        End Sub

        ' ==================== INTERNAL TYPES ====================

        Friend NotInheritable Class TreeNode
            Private _Model As TModel, _ChildrenLoaded As Boolean

            Public Property Model As TModel
                Get
                    Return _Model
                End Get
                Private Set(value As TModel)
                    _Model = value
                End Set
            End Property
            Public ReadOnly Property Parent As TreeNode
            Public ReadOnly Property Children As List(Of TreeNode) = New List(Of TreeNode)()
            Public Property IsExpanded As Boolean
            Public Property IsChecked As Boolean

            Public Property ChildrenLoaded As Boolean
                Get
                    Return _ChildrenLoaded
                End Get
                Private Set(value As Boolean)
                    _ChildrenLoaded = value
                End Set
            End Property

            ''' <summary>
            ''' Stable sibling index assigned at load time. Used as a tie-breaker for stable sorting.
            ''' </summary>
            Public Property OriginalIndex As Integer

            Public Sub New(model As TModel, parent As TreeNode)
                Me.Model = model
                Me.Parent = parent
            End Sub

            Public Sub EnsureChildrenLoaded(getter As Func(Of TModel, IEnumerable(Of TModel)))
                If ChildrenLoaded OrElse getter Is Nothing Then Return

                Me.Children.Clear()
                Dim children = getter(Model)
                If children IsNot Nothing Then
                    Dim idx = 0
                    For Each child In children
                        Dim n = New TreeNode(child, Me)
                        n.OriginalIndex = Math.Min(Threading.Interlocked.Increment(idx), idx - 1)
                        Me.Children.Add(n)
                    Next
                End If
                ChildrenLoaded = True
            End Sub

            ''' <summary>
            ''' Internal helper for ReplaceModel (immutable record support).
            ''' </summary>
            Friend Sub ReplaceModelReference(newModel As TModel)
                Model = newModel
            End Sub
        End Class

        ''' <summary>
        ''' Robust value comparer for sorting heterogeneous column data (strings, numbers, dates, nulls).
        ''' </summary>
        Private NotInheritable Class SortValueComparer
            Implements IComparer(Of Object)
            Public Shared ReadOnly Instance As SortValueComparer = New SortValueComparer()

            Public Function Compare(x As Object, y As Object) As Integer Implements IComparer(Of Object).Compare
                If ReferenceEquals(x, y) Then Return 0
                If x Is Nothing Then Return -1
                If y Is Nothing Then Return 1

                ' Fast path for identical runtime types that implement IComparable
                Dim cx As IComparable = TryCast(x, IComparable)

                If x.GetType() Is y.GetType() AndAlso cx IsNot Nothing Then
                    Try
                        Return cx.CompareTo(y)
                    Catch
                    End Try
                End If

                ' Try IComparable on either side (different numeric types, DateTime vs string, etc.)
                Dim cx2 As IComparable = TryCast(x, IComparable)


                If cx2 IsNot Nothing Then
                    Try
                        Return cx2.CompareTo(y)
                    Catch
                    End Try
                End If
                Dim cy2 As IComparable = TryCast(y, IComparable)

                If cy2 IsNot Nothing Then
                    Try
                        Return -cy2.CompareTo(x)
                    Catch
                    End Try
                End If

                ' Fallback: culture-insensitive string compare of ToString representations
                Return String.Compare(x.ToString(), y.ToString(), StringComparison.CurrentCultureIgnoreCase)
            End Function
        End Class

        ''' <summary>
        ''' A realized (visible) row. AncestorsHaveNext[i] indicates whether the chain node at depth i
        ''' (i == Level is the row's own node) has a following sibling - used to draw tree connector lines.
        ''' </summary>
        Friend Structure VisibleRow

            Public Node As TreeNode
            Public Level As Integer
            Public AncestorsHaveNext As Boolean()

            Public Sub New(Node As TreeNode, Level As Integer, AncestorsHaveNext As Boolean())
                Me.Node = Node
                Me.Level = Level
                Me.AncestorsHaveNext = AncestorsHaveNext
            End Sub
        End Structure

        ' Helper tag for editors (extensibility point)
        Friend Structure EditTag

            Public Column As TreeListColumn(Of TModel)
            Public Node As TreeNode
            Public OriginalValue As Object

            Public Sub New(Column As TreeListColumn(Of TModel), Node As TreeNode, OriginalValue As Object)
                Me.Column = Column
                Me.Node = Node
                Me.OriginalValue = OriginalValue
            End Sub
        End Structure
    End Class

    ''' <summary>
    ''' Defines a column in the ModernTreeListView.
    ''' </summary>
    Public NotInheritable Class TreeListColumn(Of TModel)
        Public Property Title As String
        Public Property Width As Integer
        Public Property Getter As Func(Of TModel, Object)
        Public Property Formatter As Func(Of Object, String)
        Public Property Alignment As System.Windows.Forms.HorizontalAlignment = System.Windows.Forms.HorizontalAlignment.Left

        ''' <summary>
        ''' Optional minimum width when the user resizes the column.
        ''' </summary>
        Public Property MinWidth As Integer = 36

        ''' <summary>
        ''' Whether cells in this column can be edited in place. Default: true.
        ''' </summary>
        Public Property IsEditable As Boolean = True

        ''' <summary>
        ''' Optional factory creating a custom in-place editor for this column.
        ''' Receives the model and the current cell value; return the (unparented) editor control.
        ''' Pair with <see cref="EditorValueExtractor"/> to read the value back on commit.
        ''' </summary>
        Public Property EditorFactory As Func(Of TModel, Object, System.Windows.Forms.Control)

        ''' <summary>
        ''' Optional delegate extracting the committed value from the editor control.
        ''' When null, built-in extraction is used (TextBox.Text, CheckBox.Checked, DateTimePicker.Value, ...).
        ''' </summary>
        Public Property EditorValueExtractor As Func(Of System.Windows.Forms.Control, Object)

        Friend Sub New(title As String, getter As Func(Of TModel, Object), width As Integer)
            Me.Title = title
            Me.Getter = getter
            Me.Width = Math.Max(width, MinWidth)
        End Sub
    End Class

    ''' <summary>
    ''' A bundle of all visual colors used by <see cref="ModernTreeListView(Of TModel)"/>.
    ''' Apply via <c>ApplyTheme</c> or the <c>Theme</c> property; use <see cref="Light"/> / <see cref="Dark"/> presets as starting points.
    ''' </summary>
    Public NotInheritable Class TreeListTheme
        Public Property BackColor As Drawing.Color
        Public Property ForeColor As Drawing.Color
        Public Property HeaderBackColor As Drawing.Color
        Public Property HeaderForeColor As Drawing.Color
        Public Property RowBackColor As Drawing.Color
        Public Property AlternatingRowBackColor As Drawing.Color
        Public Property SelectionBackColor As Drawing.Color
        Public Property SelectionForeColor As Drawing.Color
        Public Property GridLineColor As Drawing.Color
        Public Property ExpanderColor As Drawing.Color
        Public Property TreeLineColor As Drawing.Color
        Public Property HoverBackColor As Drawing.Color
        Public Property EditorBackColor As Drawing.Color
        Public Property EditorForeColor As Drawing.Color
        Public Property FocusCueColor As Drawing.Color

        ''' <summary>
        ''' Clean light preset (the control's defaults).
        ''' </summary>
        Public Shared ReadOnly Property Light As TreeListTheme
            Get
                Return New TreeListTheme() With {
                    .BackColor = Drawing.Color.White,
                    .ForeColor = Drawing.Color.FromArgb(33, 37, 41),
                    .HeaderBackColor = Drawing.Color.FromArgb(247, 248, 250),
                    .HeaderForeColor = Drawing.Color.FromArgb(52, 58, 64),
                    .RowBackColor = Drawing.Color.White,
                    .AlternatingRowBackColor = Drawing.Color.FromArgb(250, 251, 252),
                    .SelectionBackColor = Drawing.Color.FromArgb(0, 120, 212),
                    .SelectionForeColor = Drawing.Color.White,
                    .GridLineColor = Drawing.Color.FromArgb(234, 236, 239),
                    .ExpanderColor = Drawing.Color.FromArgb(108, 117, 125),
                    .TreeLineColor = Drawing.Color.FromArgb(206, 212, 218),
                    .HoverBackColor = Drawing.Color.FromArgb(241, 243, 245),
                    .EditorBackColor = Drawing.Color.White,
                    .EditorForeColor = Drawing.Color.FromArgb(33, 37, 41),
                    .FocusCueColor = Drawing.Color.FromArgb(100, 0, 120, 212)
                }
            End Get
        End Property

        ''' <summary>
        ''' Modern dark preset (VS Code-like palette).
        ''' </summary>
        Public Shared ReadOnly Property Dark As TreeListTheme
            Get
                Return New TreeListTheme() With {
                    .BackColor = Drawing.Color.FromArgb(30, 30, 30),
                    .ForeColor = Drawing.Color.FromArgb(232, 232, 232),
                    .HeaderBackColor = Drawing.Color.FromArgb(45, 45, 48),
                    .HeaderForeColor = Drawing.Color.FromArgb(208, 212, 217),
                    .RowBackColor = Drawing.Color.FromArgb(37, 37, 38),
                    .AlternatingRowBackColor = Drawing.Color.FromArgb(42, 42, 43),
                    .SelectionBackColor = Drawing.Color.FromArgb(10, 93, 171),
                    .SelectionForeColor = Drawing.Color.White,
                    .GridLineColor = Drawing.Color.FromArgb(63, 65, 68),
                    .ExpanderColor = Drawing.Color.FromArgb(160, 166, 173),
                    .TreeLineColor = Drawing.Color.FromArgb(74, 77, 82),
                    .HoverBackColor = Drawing.Color.FromArgb(51, 52, 55),
                    .EditorBackColor = Drawing.Color.FromArgb(45, 45, 48),
                    .EditorForeColor = Drawing.Color.FromArgb(232, 232, 232),
                    .FocusCueColor = Drawing.Color.FromArgb(100, 86, 156, 214)
                }
            End Get
        End Property
    End Class

    ''' <summary>
    ''' Event arguments for cell edit commit/cancel.
    ''' </summary>
    Public NotInheritable Class CellEditEventArgs(Of TModel)
        Inherits EventArgs
        Public ReadOnly Property Model As TModel
        Public ReadOnly Property Column As TreeListColumn(Of TModel)
        Public Property ProposedValue As Object
        Public ReadOnly Property OriginalValue As Object
        Public ReadOnly Property RowIndex As Integer
        Public ReadOnly Property ColumnIndex As Integer
        Public Property Cancel As Boolean

        Public Sub New(model As TModel, column As TreeListColumn(Of TModel), proposedValue As Object, originalValue As Object, rowIndex As Integer, columnIndex As Integer)
            Me.Model = model
            Me.Column = column
            Me.ProposedValue = proposedValue
            Me.OriginalValue = originalValue
            Me.RowIndex = rowIndex
            Me.ColumnIndex = columnIndex
        End Sub
    End Class

    ''' <summary>
    ''' Event args for tree node expand/collapse/check notifications.
    ''' </summary>
    Public NotInheritable Class TreeNodeEventArgs(Of TModel)
        Inherits EventArgs
        Public ReadOnly Property Model As TModel
        Friend ReadOnly Property Node As Object ' internal for future use

        Public Sub New(model As TModel, node As Object)
            Me.Model = model
            Me.Node = node
        End Sub
    End Class

    ''' <summary>
    ''' Specifies the sort direction for a column in the tree list view.
    ''' </summary>
    Public Enum SortOrder
        ''' <summary>No sorting applied (original sibling order is used).</summary>
        None
        ''' <summary>Sort in ascending order.</summary>
        Ascending
        ''' <summary>Sort in descending order.</summary>
        Descending
    End Enum
End Namespace
