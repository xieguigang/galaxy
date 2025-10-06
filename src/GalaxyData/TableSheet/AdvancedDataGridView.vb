#Region "License"
' Advanced DataGridView
'
' Copyright (c), 2014 Davide Gironi <davide.gironi@gmail.com>
' Original work Copyright (c), 2013 Zuby <zuby@me.com>
'
' Please refer to LICENSE file for licensing information.
#End Region

Imports System.IO
Imports System.Text
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Serialization.JSON

Namespace TableSheet

    <ComponentModel.DesignerCategory("")>
    Public Class AdvancedDataGridView
        Inherits DataGridView

#Region "public enum"

        ''' <summary>
        ''' Filter builder mode
        ''' </summary>
        Public Enum FilterBuilerMode As Byte
            [And] = 0
            [Or] = 1
        End Enum

#End Region


#Region "public events"

        Public Class SortEventArgs
            Inherits EventArgs
            Public Property SortString As String
            Public Property Cancel As Boolean

            Public Sub New()
                SortString = Nothing
                Cancel = False
            End Sub
        End Class

        Public Class FilterEventArgs
            Inherits EventArgs
            Public Property FilterString As String
            Public Property Cancel As Boolean

            Public Sub New()
                FilterString = Nothing
                Cancel = False
            End Sub
        End Class

        Public Event SortStringChanged As EventHandler(Of SortEventArgs)

        Public Event FilterStringChanged As EventHandler(Of FilterEventArgs)

#End Region


#Region "translations"

        ''' <summary>
        ''' Available translation keys
        ''' </summary>
        Public Enum TranslationKey
            ADGVSortDateTimeASC
            ADGVSortDateTimeDESC
            ADGVSortBoolASC
            ADGVSortBoolDESC
            ADGVSortNumASC
            ADGVSortNumDESC
            ADGVSortTextASC
            ADGVSortTextDESC
            ADGVAddCustomFilter
            ADGVCustomFilter
            ADGVClearFilter
            ADGVClearSort
            ADGVButtonFilter
            ADGVButtonUndofilter
            ADGVNodeSelectAll
            ADGVNodeSelectEmpty
            ADGVNodeSelectTrue
            ADGVNodeSelectFalse
            ADGVFilterChecklistDisable
            ADGVEquals
            ADGVDoesNotEqual
            ADGVEarlierThan
            ADGVEarlierThanOrEqualTo
            ADGVLaterThan
            ADGVLaterThanOrEqualTo
            ADGVBetween
            ADGVGreaterThan
            ADGVGreaterThanOrEqualTo
            ADGVLessThan
            ADGVLessThanOrEqualTo
            ADGVBeginsWith
            ADGVDoesNotBeginWith
            ADGVEndsWith
            ADGVDoesNotEndWith
            ADGVContains
            ADGVDoesNotContain
            ADGVIncludeNullValues
            ADGVInvalidValue
            ADGVFilterStringDescription
            ADGVFormTitle
            ADGVLabelColumnNameText
            ADGVLabelAnd
            ADGVButtonOk
            ADGVButtonCancel
        End Enum

        ''' <summary>
        ''' Internationalization strings
        ''' </summary>
        Public Shared Translations As Dictionary(Of String, String) = New Dictionary(Of String, String)() From {
    {TranslationKey.ADGVSortDateTimeASC.ToString(), "Sort Oldest to Newest"},
    {TranslationKey.ADGVSortDateTimeDESC.ToString(), "Sort Newest to Oldest"},
    {TranslationKey.ADGVSortBoolASC.ToString(), "Sort by False/True"},
    {TranslationKey.ADGVSortBoolDESC.ToString(), "Sort by True/False"},
    {TranslationKey.ADGVSortNumASC.ToString(), "Sort Smallest to Largest"},
    {TranslationKey.ADGVSortNumDESC.ToString(), "Sort Largest to Smallest"},
    {TranslationKey.ADGVSortTextASC.ToString(), "Sort А to Z"},
    {TranslationKey.ADGVSortTextDESC.ToString(), "Sort Z to A"},
    {TranslationKey.ADGVAddCustomFilter.ToString(), "Add a Custom Filter"},
    {TranslationKey.ADGVCustomFilter.ToString(), "Custom Filter"},
    {TranslationKey.ADGVClearFilter.ToString(), "Clear Filter"},
    {TranslationKey.ADGVClearSort.ToString(), "Clear Sort"},
    {TranslationKey.ADGVButtonFilter.ToString(), "Filter"},
    {TranslationKey.ADGVButtonUndofilter.ToString(), "Cancel"},
    {TranslationKey.ADGVNodeSelectAll.ToString(), "(Select All)"},
    {TranslationKey.ADGVNodeSelectEmpty.ToString(), "(Blanks)"},
    {TranslationKey.ADGVNodeSelectTrue.ToString(), "True"},
    {TranslationKey.ADGVNodeSelectFalse.ToString(), "False"},
    {TranslationKey.ADGVFilterChecklistDisable.ToString(), "Filter list is disabled"},
    {TranslationKey.ADGVEquals.ToString(), "equals"},
    {TranslationKey.ADGVDoesNotEqual.ToString(), "does not equal"},
    {TranslationKey.ADGVEarlierThan.ToString(), "earlier than"},
    {TranslationKey.ADGVEarlierThanOrEqualTo.ToString(), "earlier than or equal to"},
    {TranslationKey.ADGVLaterThan.ToString(), "later than"},
    {TranslationKey.ADGVLaterThanOrEqualTo.ToString(), "later than or equal to"},
    {TranslationKey.ADGVBetween.ToString(), "between"},
    {TranslationKey.ADGVGreaterThan.ToString(), "greater than"},
    {TranslationKey.ADGVGreaterThanOrEqualTo.ToString(), "greater than or equal to"},
    {TranslationKey.ADGVLessThan.ToString(), "less than"},
    {TranslationKey.ADGVLessThanOrEqualTo.ToString(), "less than or equal to"},
    {TranslationKey.ADGVBeginsWith.ToString(), "begins with"},
    {TranslationKey.ADGVDoesNotBeginWith.ToString(), "does not begin with"},
    {TranslationKey.ADGVEndsWith.ToString(), "ends with"},
    {TranslationKey.ADGVDoesNotEndWith.ToString(), "does not end with"},
    {TranslationKey.ADGVContains.ToString(), "contains"},
    {TranslationKey.ADGVDoesNotContain.ToString(), "does not contain"},
    {TranslationKey.ADGVIncludeNullValues.ToString(), "include empty strings"},
    {TranslationKey.ADGVInvalidValue.ToString(), "Invalid Value"},
    {TranslationKey.ADGVFilterStringDescription.ToString(), "Show rows where value {0} ""{1}"""},
    {TranslationKey.ADGVFormTitle.ToString(), "Custom Filter"},
    {TranslationKey.ADGVLabelColumnNameText.ToString(), "Show rows where value"},
    {TranslationKey.ADGVLabelAnd.ToString(), "And"},
    {TranslationKey.ADGVButtonOk.ToString(), "OK"},
    {TranslationKey.ADGVButtonCancel.ToString(), "Cancel"}
}

#End Region


#Region "class properties and fields"

        Private _sortOrderList As New System.Collections.Generic.List(Of String)()
        Private _filterOrderList As New System.Collections.Generic.List(Of String)()
        Private _filteredColumns As New System.Collections.Generic.List(Of String)()
        Private _menuStripToDispose As New System.Collections.Generic.List(Of MenuStrip)()

        Private _loadedFilter As Boolean = False
        Private _sortString As String = Nothing
        Private _filterString As String = Nothing

        Private _sortStringChangedInvokeBeforeDatasourceUpdate As Boolean = True
        Private _filterStringChangedInvokeBeforeDatasourceUpdate As Boolean = True

        Private _filterBuilerMode As FilterBuilerMode = FilterBuilerMode.And

        Friend _maxFilterButtonImageHeight As Integer = ColumnHeaderCell.FilterButtonImageDefaultSize

        Friend _maxAllCellHeight As Integer = ColumnHeaderCell.FilterButtonImageDefaultSize

#End Region


#Region "constructors"

        ''' <summary>
        ''' AdvancedDataGridView constructor
        ''' </summary>
        Public Sub New()
            MyBase.RightToLeft = RightToLeft.No
        End Sub

        ''' <summary>
        ''' Handle the dispose methods
        ''' </summary>
        ''' <param name="e"></param>
        Protected Overrides Sub OnHandleDestroyed(e As EventArgs)
            For Each column As DataGridViewColumn In Columns
                Dim cell As ColumnHeaderCell = TryCast(column.HeaderCell, ColumnHeaderCell)
                If cell IsNot Nothing Then
                    RemoveHandler cell.SortChanged, AddressOf Cell_SortChanged
                    RemoveHandler cell.FilterChanged, AddressOf Cell_FilterChanged
                    RemoveHandler cell.FilterPopup, AddressOf Cell_FilterPopup
                End If
            Next
            For Each menustrip In _menuStripToDispose
                menustrip.Dispose()
            Next
            _menuStripToDispose.Clear()

            MyBase.OnHandleDestroyed(e)
        End Sub

        ''' <summary>
        ''' Handle the DataSource change
        ''' </summary>
        ''' <param name="e"></param>
        Protected Overrides Sub OnDataSourceChanged(e As EventArgs)
            'dispose unactive menustrips
            For Each column As DataGridViewColumn In Columns
                Dim cell As ColumnHeaderCell = TryCast(column.HeaderCell, ColumnHeaderCell)
                _menuStripToDispose = _menuStripToDispose.Where(Function(f) f IsNot cell.MenuStrip).ToList()
            Next
            For Each menustrip In _menuStripToDispose
                menustrip.Dispose()
            Next
            _menuStripToDispose.Clear()

            'update datatype for active menustrips
            For Each column As DataGridViewColumn In Columns
                Dim cell As ColumnHeaderCell = TryCast(column.HeaderCell, ColumnHeaderCell)
                cell.MenuStrip.SetDataType(column.ValueType)
            Next

            MyBase.OnDataSourceChanged(e)
        End Sub
#End Region


#Region "translations methods"

        ''' <summary>
        ''' Set translation dictionary
        ''' </summary>
        ''' <param name="translations"></param>
        Public Shared Sub SetTranslations(translations As IDictionary(Of String, String))
            'set localization strings
            If translations IsNot Nothing Then
                For Each translation In translations
                    If AdvancedDataGridView.Translations.ContainsKey(translation.Key) Then AdvancedDataGridView.Translations(translation.Key) = translation.Value
                Next
            End If
        End Sub

        ''' <summary>
        ''' Get translation dictionary
        ''' </summary>
        ''' <returns></returns>
        Public Shared Function GetTranslations() As IDictionary(Of String, String)
            Return Translations
        End Function

        ''' <summary>
        ''' Load translations from file
        ''' </summary>
        ''' <param name="filename"></param>
        ''' <returns></returns>
        Public Shared Function LoadTranslationsFromFile(filename As String) As IDictionary(Of String, String)
            Dim ret As IDictionary(Of String, String) = New Dictionary(Of String, String)()

            If Not String.IsNullOrEmpty(filename) Then
                'deserialize the file
                Try
                    Dim jsontext = File.ReadAllText(filename)
                    Dim translations As Dictionary(Of String, String) = jsontext.LoadJSON(Of Dictionary(Of String, String))

                    For Each translation In translations
                        If Not ret.ContainsKey(translation.Key) AndAlso AdvancedDataGridView.Translations.ContainsKey(translation.Key) Then ret.Add(translation.Key, translation.Value)
                    Next

                Catch
                End Try
            End If

            'add default translations if not in files
            For Each translation In GetTranslations()
                If Not ret.ContainsKey(translation.Key) Then ret.Add(translation.Key, translation.Value)
            Next

            Return ret
        End Function

#End Region


#Region "public Helper methods"

        ''' <summary>
        ''' Set AdvancedDataGridView the Double Buffered
        ''' </summary>
        Public Sub SetDoubleBuffered()
            DoubleBuffered = True
        End Sub

#End Region


#Region "public Filter and Sort methods"

        ''' <summary>
        ''' SortStringChanged event called before DataSource update after sort changed is triggered
        ''' </summary>
        Public Property SortStringChangedInvokeBeforeDatasourceUpdate As Boolean
            Get
                Return _sortStringChangedInvokeBeforeDatasourceUpdate
            End Get
            Set(value As Boolean)
                _sortStringChangedInvokeBeforeDatasourceUpdate = value
            End Set
        End Property

        ''' <summary>
        ''' FilterStringChanged event called before DataSource update after sort changed is triggered
        ''' </summary>
        Public Property FilterStringChangedInvokeBeforeDatasourceUpdate As Boolean
            Get
                Return _filterStringChangedInvokeBeforeDatasourceUpdate
            End Get
            Set(value As Boolean)
                _filterStringChangedInvokeBeforeDatasourceUpdate = value
            End Set
        End Property

        ''' <summary>
        ''' Disable a Filter and Sort on a DataGridViewColumn
        ''' </summary>
        ''' <param name="column"></param>
        Public Sub DisableFilterAndSort(column As DataGridViewColumn)
            If Columns.Contains(column) Then
                Dim cell As ColumnHeaderCell = TryCast(column.HeaderCell, ColumnHeaderCell)
                If cell IsNot Nothing Then
                    If cell.FilterAndSortEnabled = True AndAlso (cell.SortString.Length > 0 OrElse cell.FilterString.Length > 0) Then
                        CleanFilter(True)
                        cell.FilterAndSortEnabled = False
                    Else
                        cell.FilterAndSortEnabled = False
                    End If
                    _filterOrderList.Remove(column.Name)
                    _sortOrderList.Remove(column.Name)
                    _filteredColumns.Remove(column.Name)
                End If
            End If
        End Sub

        ''' <summary>
        ''' Enable a Filter and Sort on a DataGridViewColumn
        ''' </summary>
        ''' <param name="column"></param>
        Public Sub EnableFilterAndSort(column As DataGridViewColumn)
            If Columns.Contains(column) Then
                Dim cell As ColumnHeaderCell = TryCast(column.HeaderCell, ColumnHeaderCell)
                If cell IsNot Nothing Then
                    If Not cell.FilterAndSortEnabled AndAlso (cell.FilterString.Length > 0 OrElse cell.SortString.Length > 0) Then CleanFilter(True)

                    cell.FilterAndSortEnabled = True
                    _filteredColumns.Remove(column.Name)

                    SetFilterDateAndTimeEnabled(column, cell.IsFilterDateAndTimeEnabled)
                    SetSortEnabled(column, cell.IsSortEnabled)
                    SetFilterEnabled(column, cell.IsFilterEnabled)
                Else
                    column.SortMode = DataGridViewColumnSortMode.Programmatic
                    cell = New ColumnHeaderCell(Me, column.HeaderCell, True)
                    AddHandler cell.SortChanged, New ColumnHeaderCellEventHandler(AddressOf Cell_SortChanged)
                    AddHandler cell.FilterChanged, New ColumnHeaderCellEventHandler(AddressOf Cell_FilterChanged)
                    AddHandler cell.FilterPopup, New ColumnHeaderCellEventHandler(AddressOf Cell_FilterPopup)
                    column.MinimumWidth = cell.MinimumSize.Width
                    If ColumnHeadersHeight < cell.MinimumSize.Height Then ColumnHeadersHeight = cell.MinimumSize.Height
                    column.HeaderCell = cell
                End If
            End If
        End Sub

        ''' <summary>
        ''' Enabled or disable Filter and Sort capabilities on a DataGridViewColumn
        ''' </summary>
        ''' <param name="column"></param>
        ''' <param name="enabled"></param>
        Public Sub SetFilterAndSortEnabled(column As DataGridViewColumn, enabled As Boolean)
            If enabled Then
                EnableFilterAndSort(column)
            Else
                DisableFilterAndSort(column)
            End If
        End Sub

        ''' <summary>
        ''' Disable a Filter checklist on a DataGridViewColumn
        ''' </summary>
        ''' <param name="column"></param>
        Public Sub DisableFilterChecklist(column As DataGridViewColumn)
            If Columns.Contains(column) Then
                Dim cell As ColumnHeaderCell = TryCast(column.HeaderCell, ColumnHeaderCell)
                If cell IsNot Nothing Then
                    cell.SetFilterChecklistEnabled(False)
                End If
            End If
        End Sub

        ''' <summary>
        ''' Enable a Filter checklist on a DataGridViewColumn
        ''' </summary>
        ''' <param name="column"></param>
        Public Sub EnableFilterChecklist(column As DataGridViewColumn)
            If Columns.Contains(column) Then
                Dim cell As ColumnHeaderCell = TryCast(column.HeaderCell, ColumnHeaderCell)
                If cell IsNot Nothing Then
                    cell.SetFilterChecklistEnabled(True)
                End If
            End If
        End Sub

        ''' <summary>
        ''' Enabled or disable Filter checklist capabilities on a DataGridViewColumn
        ''' </summary>
        ''' <param name="column"></param>
        ''' <param name="enabled"></param>
        Public Sub SetFilterChecklistEnabled(column As DataGridViewColumn, enabled As Boolean)
            If enabled Then
                EnableFilterChecklist(column)
            Else
                DisableFilterChecklist(column)
            End If
        End Sub

        ''' <summary>
        ''' Set Filter checklist nodes max on a DataGridViewColumn
        ''' </summary>
        ''' <param name="column"></param>
        ''' <param name="maxnodes"></param>
        Public Sub SetFilterChecklistNodesMax(column As DataGridViewColumn, maxnodes As Integer)
            If Columns.Contains(column) Then
                Dim cell As ColumnHeaderCell = TryCast(column.HeaderCell, ColumnHeaderCell)
                If cell IsNot Nothing Then
                    cell.SetFilterChecklistNodesMax(maxnodes)
                End If
            End If
        End Sub

        ''' <summary>
        ''' Set Filter checklist nodes max
        ''' </summary>
        ''' <param name="maxnodes"></param>
        Public Sub SetFilterChecklistNodesMax(maxnodes As Integer)
            For Each c As ColumnHeaderCell In FilterableCells
                c.SetFilterChecklistNodesMax(maxnodes)
            Next
        End Sub

        ''' <summary>
        ''' Enable or disable Filter checklist nodes max on a DataGridViewColumn
        ''' </summary>
        ''' <param name="column"></param>
        ''' <param name="enabled"></param>
        Public Sub EnabledFilterChecklistNodesMax(column As DataGridViewColumn, enabled As Boolean)
            If Columns.Contains(column) Then
                Dim cell As ColumnHeaderCell = TryCast(column.HeaderCell, ColumnHeaderCell)
                If cell IsNot Nothing Then
                    cell.EnabledFilterChecklistNodesMax(enabled)
                End If
            End If
        End Sub

        ''' <summary>
        ''' Enable or disable Filter checklist nodes max
        ''' </summary>
        ''' <param name="enabled"></param>
        Public Sub EnabledFilterChecklistNodesMax(enabled As Boolean)
            For Each c As ColumnHeaderCell In FilterableCells
                c.EnabledFilterChecklistNodesMax(enabled)
            Next
        End Sub

        ''' <summary>
        ''' Disable a Filter custom on a DataGridViewColumn
        ''' </summary>
        ''' <param name="column"></param>
        Public Sub DisableFilterCustom(column As DataGridViewColumn)
            If Columns.Contains(column) Then
                Dim cell As ColumnHeaderCell = TryCast(column.HeaderCell, ColumnHeaderCell)
                If cell IsNot Nothing Then
                    cell.SetFilterCustomEnabled(False)
                End If
            End If
        End Sub

        ''' <summary>
        ''' Enable a Filter custom on a DataGridViewColumn
        ''' </summary>
        ''' <param name="column"></param>
        Public Sub EnableFilterCustom(column As DataGridViewColumn)
            If Columns.Contains(column) Then
                Dim cell As ColumnHeaderCell = TryCast(column.HeaderCell, ColumnHeaderCell)
                If cell IsNot Nothing Then
                    cell.SetFilterCustomEnabled(True)
                End If
            End If
        End Sub

        ''' <summary>
        ''' Enabled or disable Filter custom capabilities on a DataGridViewColumn
        ''' </summary>
        ''' <param name="column"></param>
        ''' <param name="enabled"></param>
        Public Sub SetFilterCustomEnabled(column As DataGridViewColumn, enabled As Boolean)
            If enabled Then
                EnableFilterCustom(column)
            Else
                DisableFilterCustom(column)
            End If
        End Sub

        ''' <summary>
        ''' Set nodes to enable TextChanged delay on filter checklist on a DataGridViewColumn
        ''' </summary>
        ''' <param name="column"></param>
        ''' <param name="numnodes"></param>
        Public Sub SetFilterChecklistTextFilterTextChangedDelayNodes(column As DataGridViewColumn, numnodes As Integer)
            If Columns.Contains(column) Then
                Dim cell As ColumnHeaderCell = TryCast(column.HeaderCell, ColumnHeaderCell)
                If cell IsNot Nothing Then
                    cell.TextFilterTextChangedDelayNodes = numnodes
                End If
            End If
        End Sub

        ''' <summary>
        ''' Set nodes to enable TextChanged delay on filter checklist
        ''' </summary>
        ''' <param name="numnodes"></param>
        Public Sub SetFilterChecklistTextFilterTextChangedDelayNodes(numnodes As Integer)
            For Each c As ColumnHeaderCell In FilterableCells
                c.TextFilterTextChangedDelayNodes = numnodes
            Next
        End Sub

        ''' <summary>
        ''' Disable TextChanged delay on filter checklist on a DataGridViewColumn
        ''' </summary>
        ''' <param name="column"></param>
        Public Sub SetFilterChecklistTextFilterTextChangedDelayDisabled(column As DataGridViewColumn)
            If Columns.Contains(column) Then
                Dim cell As ColumnHeaderCell = TryCast(column.HeaderCell, ColumnHeaderCell)
                If cell IsNot Nothing Then
                    cell.SetTextFilterTextChangedDelayNodesDisabled()
                End If
            End If
        End Sub

        ''' <summary>
        ''' Disable TextChanged delay on filter checklist
        ''' </summary>
        Public Sub SetFilterChecklistTextFilterTextChangedDelayDisabled()
            For Each c As ColumnHeaderCell In FilterableCells
                c.SetTextFilterTextChangedDelayNodesDisabled()
            Next
        End Sub

        ''' <summary>
        ''' Set TextChanged delay milliseconds on filter checklist on a DataGridViewColumn
        ''' </summary>
        ''' <param name="column"></param>
        Public Sub SetFilterChecklistTextFilterTextChangedDelayMs(column As DataGridViewColumn, milliseconds As Integer)
            If Columns.Contains(column) Then
                Dim cell As ColumnHeaderCell = TryCast(column.HeaderCell, ColumnHeaderCell)
                If cell IsNot Nothing Then
                    cell.SetTextFilterTextChangedDelayMs(milliseconds)
                End If
            End If
        End Sub

        ''' <summary>
        ''' Set TextChanged delay milliseconds on filter checklist
        ''' </summary>
        Public Sub SetFilterChecklistTextFilterTextChangedDelayMs(milliseconds As Integer)
            For Each c As ColumnHeaderCell In FilterableCells
                c.SetTextFilterTextChangedDelayMs(milliseconds)
            Next
        End Sub

        ''' <summary>
        ''' Load a Filter and Sort preset
        ''' </summary>
        ''' <param name="filter"></param>
        ''' <param name="sorting"></param>
        Public Sub LoadFilterAndSort(filter As String, sorting As String)
            For Each c As ColumnHeaderCell In FilterableCells
                c.SetLoadedMode(True)
            Next

            _filteredColumns.Clear()

            _filterOrderList.Clear()
            _sortOrderList.Clear()

            If Not Equals(filter, Nothing) Then FilterString = filter
            If Not Equals(sorting, Nothing) Then SortString = sorting

            _loadedFilter = True
        End Sub

        ''' <summary>
        ''' Clean Filter and Sort
        ''' </summary>
        Public Sub CleanFilterAndSort()
            For Each c As ColumnHeaderCell In FilterableCells
                c.SetLoadedMode(False)
            Next

            _filteredColumns.Clear()
            _filterOrderList.Clear()
            _sortOrderList.Clear()

            _loadedFilter = False

            CleanFilter()
            CleanSort()
        End Sub

        ''' <summary>
        ''' Set the NOTIN Logic for checkbox filter
        ''' </summary>
        ''' <param name="enabled"></param>
        Public Sub SetMenuStripFilterNOTINLogic(enabled As Boolean)
            For Each c As ColumnHeaderCell In FilterableCells
                c.IsMenuStripFilterNOTINLogicEnabled = enabled
            Next
        End Sub

        ''' <summary>
        ''' Get or Set Filter and Sort status
        ''' </summary>
        Public Property FilterAndSortEnabled As Boolean
            Get
                Return _filterAndSortEnabled
            End Get
            Set(value As Boolean)
                _filterAndSortEnabled = value
            End Set
        End Property
        Private _filterAndSortEnabled As Boolean = True

        ''' <summary>
        ''' Set the Filter Text focus OnShow
        ''' </summary>
        ''' <param name="enabled"></param>
        Public Sub SetFilterTextFocusOnShow(enabled As Boolean)
            For Each c As ColumnHeaderCell In FilterableCells
                c.FilterTextFocusOnShow = enabled
            Next
        End Sub

#End Region


#Region "public Sort methods"

        ''' <summary>
        ''' Get the Sort string
        ''' </summary>
        Public Property SortString As String
            Get
                Return If(Not String.IsNullOrEmpty(_sortString), _sortString, "")
            End Get
            Private Set(value As String)
                Dim old = value
                If Not Equals(old, _sortString) Then
                    _sortString = value

                    TriggerSortStringChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Trigger the sort string changed method
        ''' </summary>
        Public Sub TriggerSortStringChanged()
            'call event handler if one is attached
            Dim sortEventArgs As SortEventArgs = New SortEventArgs With {
    .SortString = _sortString,
    .Cancel = False
}
            'invoke SortStringChanged
            If _sortStringChangedInvokeBeforeDatasourceUpdate Then
                RaiseEvent SortStringChanged(Me, sortEventArgs)
            End If
            Dim bindingsource As New Value(Of BindingSource)
            Dim dataview As New Value(Of DataView)
            Dim datatable As New Value(Of DataTable)

            'sort datasource
            If sortEventArgs.Cancel = False Then

                If (bindingsource = TryCast(DataSource, BindingSource)) IsNot Nothing Then
                    CType(bindingsource, BindingSource).Sort = sortEventArgs.SortString
                ElseIf (dataview = TryCast(DataSource, DataView)) IsNot Nothing Then
                    CType(dataview, DataView).Sort = sortEventArgs.SortString
                ElseIf (datatable = TryCast(DataSource, DataTable)) IsNot Nothing Then
                    If CType(datatable, DataTable).DefaultView IsNot Nothing Then
                        CType(datatable, DataTable).DefaultView.Sort = sortEventArgs.SortString
                    End If
                End If
            End If
            'invoke SortStringChanged
            If Not _sortStringChangedInvokeBeforeDatasourceUpdate Then
                RaiseEvent SortStringChanged(Me, sortEventArgs)
            End If
        End Sub

        ''' <summary>
        ''' Enabled or disable Sort capabilities for a DataGridViewColumn
        ''' </summary>
        ''' <param name="column"></param>
        ''' <param name="enabled"></param>
        Public Sub SetSortEnabled(column As DataGridViewColumn, enabled As Boolean)
            If Columns.Contains(column) Then
                Dim cell As ColumnHeaderCell = TryCast(column.HeaderCell, ColumnHeaderCell)
                If cell IsNot Nothing Then
                    cell.SetSortEnabled(enabled)
                End If
            End If
        End Sub

        ''' <summary>
        ''' Sort ASC
        ''' </summary>
        Public Sub SortASC(column As DataGridViewColumn)
            If Columns.Contains(column) Then
                Dim cell As ColumnHeaderCell = TryCast(column.HeaderCell, ColumnHeaderCell)
                If cell IsNot Nothing Then
                    cell.SortASC()
                End If
            End If
        End Sub

        ''' <summary>
        ''' Sort ASC
        ''' </summary>
        Public Sub SortDESC(column As DataGridViewColumn)
            If Columns.Contains(column) Then
                Dim cell As ColumnHeaderCell = TryCast(column.HeaderCell, ColumnHeaderCell)
                If cell IsNot Nothing Then
                    cell.SortDESC()
                End If
            End If
        End Sub

        ''' <summary>
        ''' Clean all Sort on specific column
        ''' </summary>
        ''' <param name="column"></param>
        ''' <param name="fireEvent"></param>
        Public Sub CleanSort(column As DataGridViewColumn, fireEvent As Boolean)
            If Columns.Contains(column) Then
                Dim cell As ColumnHeaderCell = TryCast(column.HeaderCell, ColumnHeaderCell)
                If cell IsNot Nothing AndAlso FilterableCells.Contains(cell) Then
                    cell.CleanSort()
                    'remove column from sorted list
                    _sortOrderList.Remove(column.Name)
                End If
            End If

            If fireEvent Then
                SortString = BuildSortString()
            Else
                _sortString = BuildSortString()
            End If
        End Sub

        ''' <summary>
        ''' Clean all Sort on specific column
        ''' </summary>
        ''' <param name="column"></param>
        Public Sub CleanSort(column As DataGridViewColumn)
            CleanSort(column, True)
        End Sub

        ''' <summary>
        ''' Clean all Sort on all columns
        ''' </summary>
        ''' <param name="fireEvent"></param>
        Public Sub CleanSort(fireEvent As Boolean)
            For Each c As ColumnHeaderCell In FilterableCells
                c.CleanSort()
            Next
            _sortOrderList.Clear()

            If fireEvent Then
                SortString = Nothing
            Else
                _sortString = Nothing
            End If
        End Sub

        ''' <summary>
        ''' Clean all Sort on all columns
        ''' </summary>
        Public Sub CleanSort()
            CleanSort(True)
        End Sub

#End Region


#Region "public Filter methods"

        ''' <summary>
        ''' Get the Filter string
        ''' </summary>
        Public Property FilterString As String
            Get
                Return If(Not String.IsNullOrEmpty(_filterString), _filterString, "")
            End Get
            Private Set(value As String)
                Dim old = value
                If Not Equals(old, _filterString) Then
                    _filterString = value

                    TriggerFilterStringChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Trigger the filter string changed method
        ''' </summary>
        Public Sub TriggerFilterStringChanged()
            'call event handler if one is attached
            Dim filterEventArgs As New FilterEventArgs With {
                .FilterString = _filterString,
                .Cancel = False
            }
            'invoke FilterStringChanged
            If _filterStringChangedInvokeBeforeDatasourceUpdate Then
                RaiseEvent FilterStringChanged(Me, filterEventArgs)
            End If
            Dim bindingsource As New Value(Of BindingSource)
            Dim dataview As New Value(Of DataView)
            Dim datatable As New Value(Of DataTable)

            'filter datasource
            If filterEventArgs.Cancel = False Then
                If (bindingsource = TryCast(DataSource, BindingSource)) IsNot Nothing Then
                    CType(bindingsource, BindingSource).Filter = filterEventArgs.FilterString
                ElseIf (dataview = TryCast(DataSource, DataView)) IsNot Nothing Then
                    CType(dataview, DataView).RowFilter = filterEventArgs.FilterString
                ElseIf (datatable = TryCast(DataSource, DataTable)) IsNot Nothing Then
                    If CType(datatable, DataTable).DefaultView IsNot Nothing Then
                        CType(datatable, DataTable).DefaultView.RowFilter = filterEventArgs.FilterString
                    End If
                End If
            End If
            'invoke FilterStringChanged
            If Not _filterStringChangedInvokeBeforeDatasourceUpdate Then
                RaiseEvent FilterStringChanged(Me, filterEventArgs)
            End If
        End Sub

        ''' <summary>
        ''' Set FilterDateAndTime status for a DataGridViewColumn
        ''' </summary>
        ''' <param name="column"></param>
        ''' <param name="enabled"></param>
        Public Sub SetFilterDateAndTimeEnabled(column As DataGridViewColumn, enabled As Boolean)
            If Columns.Contains(column) Then
                Dim cell As ColumnHeaderCell = TryCast(column.HeaderCell, ColumnHeaderCell)
                If cell IsNot Nothing Then
                    cell.IsFilterDateAndTimeEnabled = enabled
                End If
            End If
        End Sub

        ''' <summary>
        ''' Enable or disable Filter capabilities for a DataGridViewColumn
        ''' </summary>
        ''' <param name="column"></param>
        ''' <param name="enabled"></param>
        Public Sub SetFilterEnabled(column As DataGridViewColumn, enabled As Boolean)
            If Columns.Contains(column) Then
                Dim cell As ColumnHeaderCell = TryCast(column.HeaderCell, ColumnHeaderCell)
                If cell IsNot Nothing Then
                    cell.SetFilterEnabled(enabled)
                End If
            End If
        End Sub

        ''' <summary>
        ''' Enable or disable Text filter on checklist remove node mode for a DataGridViewColumn
        ''' </summary>
        ''' <param name="column"></param>
        ''' <param name="enabled"></param>
        Public Sub SetChecklistTextFilterRemoveNodesOnSearchMode(column As DataGridViewColumn, enabled As Boolean)
            If Columns.Contains(column) Then
                Dim cell As ColumnHeaderCell = TryCast(column.HeaderCell, ColumnHeaderCell)
                If cell IsNot Nothing Then
                    cell.SetChecklistTextFilterRemoveNodesOnSearchMode(enabled)
                End If
            End If
        End Sub

        ''' <summary>
        ''' Clean Filter on specific column
        ''' </summary>
        ''' <param name="column"></param>
        ''' <param name="fireEvent"></param>
        Public Sub CleanFilter(column As DataGridViewColumn, fireEvent As Boolean)
            If Columns.Contains(column) Then
                Dim cell As ColumnHeaderCell = TryCast(column.HeaderCell, ColumnHeaderCell)
                If cell IsNot Nothing Then
                    cell.CleanFilter()
                    'remove column from filtered list
                    _filterOrderList.Remove(column.Name)
                End If
            End If

            If fireEvent Then
                FilterString = BuildFilterString()
            Else
                _filterString = BuildFilterString()
            End If
        End Sub

        ''' <summary>
        ''' Clean Filter on specific column
        ''' </summary>
        ''' <param name="column"></param>
        Public Sub CleanFilter(column As DataGridViewColumn)
            CleanFilter(column, True)
        End Sub

        ''' <summary>
        ''' Clean Filter on all columns
        ''' </summary>
        ''' <param name="fireEvent"></param>
        Public Sub CleanFilter(fireEvent As Boolean)
            For Each c As ColumnHeaderCell In FilterableCells
                c.CleanFilter()
            Next
            _filterOrderList.Clear()

            If fireEvent Then
                FilterString = Nothing
            Else
                _filterString = Nothing
            End If
        End Sub

        ''' <summary>
        ''' Clean all Sort on all columns
        ''' </summary>
        Public Sub CleanFilter()
            CleanFilter(True)
        End Sub

        ''' <summary>
        ''' Set the text filter search nodes behaviour
        ''' </summary>
        Public Sub SetTextFilterRemoveNodesOnSearch(column As DataGridViewColumn, enabled As Boolean)
            If Columns.Contains(column) Then
                Dim cell As ColumnHeaderCell = TryCast(column.HeaderCell, ColumnHeaderCell)
                If cell IsNot Nothing Then cell.DoesTextFilterRemoveNodesOnSearch = enabled
            End If
        End Sub

        ''' <summary>
        ''' Get the text filter search nodes behaviour
        ''' </summary>
        Public Function GetTextFilterRemoveNodesOnSearch(column As DataGridViewColumn) As Boolean?
            Dim ret As Boolean? = Nothing
            If Columns.Contains(column) Then
                Dim cell As ColumnHeaderCell = TryCast(column.HeaderCell, ColumnHeaderCell)
                If cell IsNot Nothing Then ret = cell.DoesTextFilterRemoveNodesOnSearch
            End If
            Return ret
        End Function


        ''' <summary>
        ''' Return the filtered strings by column name
        ''' </summary>
        ''' <returns></returns>
        Public Function GetColumnsFilteredStrings() As Dictionary(Of String, String)
            Dim ret As Dictionary(Of String, String) = New Dictionary(Of String, String)()

            For Each filterOrder In _filterOrderList
                Dim Column = Columns(filterOrder)

                If Column IsNot Nothing Then
                    Dim cell As ColumnHeaderCell = TryCast(Column.HeaderCell, ColumnHeaderCell)
                    If cell IsNot Nothing Then
                        If cell.FilterAndSortEnabled AndAlso cell.ActiveFilterType <> MenuStrip.FilterType.None Then
                            If Not ret.ContainsKey(Column.DataPropertyName) Then
                                ret.Add(Column.DataPropertyName, cell.FilterString)
                            End If
                        End If
                    End If
                End If
            Next
            Return ret
        End Function

        ''' <summary>
        ''' Set the filter builder mode
        ''' </summary>
        Public Sub SetFilterBuilderMode(filterBuilerMode As FilterBuilerMode)
            _filterBuilerMode = filterBuilerMode
        End Sub

        ''' <summary>
        ''' Get the filter builder mode
        ''' </summary>
        Public Function GetFilterBuilderMode() As FilterBuilerMode
            Return _filterBuilerMode
        End Function

#End Region


#Region "public Find methods"

        ''' <summary>
        ''' Find the Cell with the given value
        ''' </summary>
        ''' <param name="valueToFind"></param>
        ''' <param name="columnName"></param>
        ''' <param name="rowIndex"></param>
        ''' <param name="columnIndex"></param>
        ''' <param name="isWholeWordSearch"></param>
        ''' <param name="isCaseSensitive"></param>
        ''' <returns></returns>
        Public Function FindCell(valueToFind As String, columnName As String, rowIndex As Integer, columnIndex As Integer, isWholeWordSearch As Boolean, isCaseSensitive As Boolean) As DataGridViewCell
            If Not Equals(valueToFind, Nothing) AndAlso RowCount > 0 AndAlso ColumnCount > 0 AndAlso (Equals(columnName, Nothing) OrElse Columns.Contains(columnName) AndAlso Columns(columnName).Visible) Then
                rowIndex = Math.Max(0, rowIndex)

                If Not isCaseSensitive Then valueToFind = valueToFind.ToLower()

                If Not Equals(columnName, Nothing) Then
                    Dim c = Columns(columnName).Index
                    If columnIndex > c Then rowIndex += 1
                    For r = rowIndex To RowCount - 1
                        Dim value As String = Rows(r).Cells(c).FormattedValue.ToString()
                        If Not isCaseSensitive Then value = value.ToLower()

                        If Not isWholeWordSearch AndAlso value.Contains(valueToFind) OrElse value.Equals(valueToFind) Then Return Rows(r).Cells(c)
                    Next
                Else
                    columnIndex = Math.Max(0, columnIndex)

                    For r As Integer = rowIndex To RowCount - 1
                        For c As Integer = columnIndex To ColumnCount - 1
                            If Not Rows(r).Cells(c).Visible Then Continue For

                            Dim value As String = Rows(r).Cells(c).FormattedValue.ToString()
                            If Not isCaseSensitive Then value = value.ToLower()

                            If Not isWholeWordSearch AndAlso value.Contains(valueToFind) OrElse value.Equals(valueToFind) Then
                                Return Rows(r).Cells(c)
                            End If
                        Next

                        columnIndex = 0
                    Next
                End If
            End If

            Return Nothing
        End Function

#End Region


#Region "public Cell methods"

        ''' <summary>
        ''' Show a menu strip
        ''' </summary>
        ''' <param name="column"></param>
        Public Sub ShowMenuStrip(column As DataGridViewColumn)
            If Columns.Contains(column) Then
                Dim cell As ColumnHeaderCell = TryCast(column.HeaderCell, ColumnHeaderCell)
                If cell IsNot Nothing Then
                    Cell_FilterPopup(cell, New ColumnHeaderCellEventArgs(cell.MenuStrip, column))
                End If
            End If
        End Sub

        ''' <summary>
        ''' Get or Set the max filter button image height
        ''' </summary>
        Public Property MaxFilterButtonImageHeight As Integer
            Get
                Return _maxFilterButtonImageHeight
            End Get
            Set(value As Integer)
                _maxFilterButtonImageHeight = If(value > ColumnHeaderCell.FilterButtonImageDefaultSize, value, ColumnHeaderCell.FilterButtonImageDefaultSize)
            End Set
        End Property

#End Region


#Region "internal Cell methods"

        ''' <summary>
        ''' Get or Set the max filter button image height of all cells
        ''' </summary>
        Friend Property MaxAllCellHeight As Integer
            Get
                Return _maxAllCellHeight
            End Get
            Set(value As Integer)
                _maxAllCellHeight = If(value > ColumnHeaderCell.FilterButtonImageDefaultSize, value, ColumnHeaderCell.FilterButtonImageDefaultSize)
            End Set
        End Property

#End Region


#Region "cells methods"

        ''' <summary>
        ''' Get all columns
        ''' </summary>
        Private ReadOnly Property FilterableCells As IEnumerable(Of ColumnHeaderCell)
            Get
                Return From c As DataGridViewColumn
                       In Columns
                       Where c.HeaderCell IsNot Nothing AndAlso TypeOf c.HeaderCell Is ColumnHeaderCell
                       Select TryCast(c.HeaderCell, ColumnHeaderCell)
            End Get
        End Property

#End Region


#Region "column events"

        ''' <summary>
        ''' Overriden  OnColumnAdded event
        ''' </summary>
        ''' <param name="e"></param>
        Protected Overrides Sub OnColumnAdded(e As DataGridViewColumnEventArgs)
            e.Column.SortMode = DataGridViewColumnSortMode.Programmatic
            Dim cell As ColumnHeaderCell = New ColumnHeaderCell(Me, e.Column.HeaderCell, FilterAndSortEnabled)
            AddHandler cell.SortChanged, New ColumnHeaderCellEventHandler(AddressOf Cell_SortChanged)
            AddHandler cell.FilterChanged, New ColumnHeaderCellEventHandler(AddressOf Cell_FilterChanged)
            AddHandler cell.FilterPopup, New ColumnHeaderCellEventHandler(AddressOf Cell_FilterPopup)
            e.Column.MinimumWidth = cell.MinimumSize.Width
            If ColumnHeadersHeight < cell.MinimumSize.Height Then ColumnHeadersHeight = cell.MinimumSize.Height
            e.Column.HeaderCell = cell

            MyBase.OnColumnAdded(e)
        End Sub

        ''' <summary>
        ''' Overridden OnColumnRemoved event
        ''' </summary>
        ''' <param name="e"></param>
        Protected Overrides Sub OnColumnRemoved(e As DataGridViewColumnEventArgs)
            _filteredColumns.Remove(e.Column.Name)
            _filterOrderList.Remove(e.Column.Name)
            _sortOrderList.Remove(e.Column.Name)

            Dim cell As ColumnHeaderCell = TryCast(e.Column.HeaderCell, ColumnHeaderCell)
            If cell IsNot Nothing Then
                RemoveHandler cell.SortChanged, AddressOf Cell_SortChanged
                RemoveHandler cell.FilterChanged, AddressOf Cell_FilterChanged
                RemoveHandler cell.FilterPopup, AddressOf Cell_FilterPopup

                cell.CleanEvents()
                If Not e.Column.IsDataBound Then
                    cell.MenuStrip.Dispose()
                Else
                    _menuStripToDispose.Add(cell.MenuStrip)
                End If
            End If
            MyBase.OnColumnRemoved(e)
        End Sub

#End Region


#Region "rows events"

        ''' <summary>
        ''' Overridden OnRowsAdded event
        ''' </summary>
        ''' <param name="e"></param>
        Protected Overrides Sub OnRowsAdded(e As DataGridViewRowsAddedEventArgs)
            If e.RowIndex >= 0 Then _filteredColumns.Clear()
            MyBase.OnRowsAdded(e)
        End Sub

        ''' <summary>
        ''' Overridden OnRowsRemoved event
        ''' </summary>
        ''' <param name="e"></param>
        Protected Overrides Sub OnRowsRemoved(e As DataGridViewRowsRemovedEventArgs)
            If e.RowIndex >= 0 Then _filteredColumns.Clear()
            MyBase.OnRowsRemoved(e)
        End Sub

#End Region


#Region "cell events"

        ''' <summary>
        ''' Overridden OnCellValueChanged event
        ''' </summary>
        ''' <param name="e"></param>
        Protected Overrides Sub OnCellValueChanged(e As DataGridViewCellEventArgs)
            If e.RowIndex >= 0 AndAlso e.ColumnIndex >= 0 Then
                _filteredColumns.Remove(Columns(e.ColumnIndex).Name)
            End If

            MyBase.OnCellValueChanged(e)
        End Sub

#End Region


#Region "filter events"

        ''' <summary>
        ''' Build the complete Filter string
        ''' </summary>
        ''' <returns></returns>
        Private Function BuildFilterString() As String
            Dim sb As New StringBuilder("")
            Dim appx = ""

            For Each filterOrder In _filterOrderList
                Dim Column = Columns(filterOrder)

                If Column IsNot Nothing Then
                    Dim cell As ColumnHeaderCell = TryCast(Column.HeaderCell, ColumnHeaderCell)
                    If cell IsNot Nothing Then
                        If cell.FilterAndSortEnabled AndAlso cell.ActiveFilterType <> MenuStrip.FilterType.None Then
                            sb.AppendFormat(appx & "(" & cell.FilterString & ")", Column.DataPropertyName)
                            If _filterBuilerMode = FilterBuilerMode.And Then
                                appx = " AND "
                            ElseIf _filterBuilerMode = FilterBuilerMode.Or Then
                                appx = " OR "
                            End If
                        End If
                    End If
                End If
            Next
            Return sb.ToString()
        End Function

        ''' <summary>
        ''' FilterPopup event
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub Cell_FilterPopup(sender As Object, e As ColumnHeaderCellEventArgs)
            If Columns.Contains(e.Column) Then
                Dim filterMenu = e.FilterMenu
                Dim column = e.Column

                Dim rect = GetCellDisplayRectangle(column.Index, -1, True)

                If _filteredColumns.Contains(column.Name) Then
                    filterMenu.Show(Me, rect.Left, rect.Bottom, False)
                Else
                    _filteredColumns.Add(column.Name)
                    If _filterOrderList.Count() > 0 AndAlso Equals(_filterOrderList.Last(), column.Name) Then
                        filterMenu.Show(Me, rect.Left, rect.Bottom, True)
                    Else
                        filterMenu.Show(Me, rect.Left, rect.Bottom, column.Name)
                    End If
                End If
            End If
        End Sub

        ''' <summary>
        ''' FilterChanged event
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub Cell_FilterChanged(sender As Object, e As ColumnHeaderCellEventArgs)
            If Columns.Contains(e.Column) Then
                Dim filterMenu = e.FilterMenu
                Dim column = e.Column

                _filterOrderList.Remove(column.Name)
                If filterMenu.ActiveFilterType <> MenuStrip.FilterType.None Then
                    _filterOrderList.Add(column.Name)
                End If

                FilterString = BuildFilterString()

                If _loadedFilter Then
                    _loadedFilter = False
                    For Each c As ColumnHeaderCell In FilterableCells.Where(Function(f) f.MenuStrip IsNot filterMenu)
                        c.SetLoadedMode(False)
                    Next
                End If
            End If
        End Sub

#End Region


#Region "sort events"

        ''' <summary>
        ''' Build the complete Sort string
        ''' </summary>
        ''' <returns></returns>
        Private Function BuildSortString() As String
            Dim sb As StringBuilder = New StringBuilder("")
            Dim appx = ""

            For Each sortOrder As String In _sortOrderList
                Dim column = Columns(sortOrder)

                If column IsNot Nothing Then
                    Dim cell As ColumnHeaderCell = TryCast(column.HeaderCell, ColumnHeaderCell)
                    If cell IsNot Nothing Then
                        If cell.FilterAndSortEnabled AndAlso cell.ActiveSortType <> MenuStrip.SortType.None Then
                            sb.AppendFormat(appx & cell.SortString, column.DataPropertyName)
                            appx = ", "
                        End If
                    End If
                End If
            Next

            Return sb.ToString()
        End Function

        ''' <summary>
        ''' SortChanged event
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub Cell_SortChanged(sender As Object, e As ColumnHeaderCellEventArgs)
            If Columns.Contains(e.Column) Then
                Dim filterMenu = e.FilterMenu
                Dim column = e.Column

                _sortOrderList.Remove(column.Name)
                If filterMenu.ActiveSortType <> MenuStrip.SortType.None Then _sortOrderList.Add(column.Name)
                SortString = BuildSortString()
            End If
        End Sub

#End Region

    End Class
End Namespace
