#Region "License"
' Advanced DataGridView
'
' Original work Copyright (c), 2013 Zuby <zuby@me.com> 
' Modified work Copyright (c), 2014 Davide Gironi <davide.gironi@gmail.com>
'
' Please refer to LICENSE file for licensing information.
#End Region

Imports System.ComponentModel

Namespace TableSheet

    <DesignerCategory("")>
    Friend Class ColumnHeaderCell : Inherits DataGridViewColumnHeaderCell

        ''' <summary>
        ''' Get the MenuStrip for this ColumnHeaderCell
        ''' </summary>
        Private _MenuStrip As MenuStrip

#Region "public events"

        Public Event FilterPopup As ColumnHeaderCellEventHandler
        Public Event SortChanged As ColumnHeaderCellEventHandler
        Public Event FilterChanged As ColumnHeaderCellEventHandler

#End Region


#Region "constants"

        ''' <summary>
        ''' Default behaviour for Date and Time filter
        ''' </summary>
        Private Const FilterDateAndTimeDefaultEnabled As Boolean = False

        ''' <summary>
        ''' Default filter button image width and height
        ''' </summary>
        Public Const FilterButtonImageDefaultSize As Integer = 23

#End Region


#Region "class properties and fields"

        ReadOnly m_dataGridView As AdvancedDataGridView

        Private _filterImage As Image = My.Resources.Resources.ColumnHeader_UnFiltered
        Private _filterButtonImageSize As Size = New Size(FilterButtonImageDefaultSize, FilterButtonImageDefaultSize)
        Private _filterButtonPressed As Boolean = False
        Private _filterButtonOver As Boolean = False
        Private _filterButtonOffsetBounds As Rectangle = Rectangle.Empty
        Private _filterButtonImageBounds As Rectangle = Rectangle.Empty
        Private _filterButtonMargin As Padding = New Padding(3, 4, 3, 4)
        Private _filterEnabled As Boolean = False

        Public Property MenuStrip As MenuStrip
            Get
                Return _MenuStrip
            End Get
            Private Set(value As MenuStrip)
                _MenuStrip = value
            End Set
        End Property

#End Region


#Region "constructors"

        ''' <summary>
        ''' ColumnHeaderCell constructor
        ''' </summary>
        ''' <param name="dataGridView"></param>
        ''' <param name="oldCell"></param>
        ''' <param name="filterEnabled"></param>
        Public Sub New(dataGridView As AdvancedDataGridView, oldCell As DataGridViewColumnHeaderCell, filterEnabled As Boolean)
            MyBase.New()

            m_dataGridView = dataGridView
            Tag = oldCell.Tag
            ErrorText = oldCell.ErrorText
            ToolTipText = oldCell.ToolTipText
            Value = oldCell.Value
            ValueType = oldCell.ValueType
            MyBase.ContextMenuStrip = oldCell.ContextMenuStrip
            Style = oldCell.Style
            _filterEnabled = filterEnabled

            If oldCell.Size.Height > dataGridView.MaxAllCellHeight Then dataGridView.MaxAllCellHeight = oldCell.Size.Height
            Dim filterButtonImageHeight = If(dataGridView.MaxFilterButtonImageHeight < dataGridView.MaxAllCellHeight, dataGridView.MaxFilterButtonImageHeight, dataGridView.MaxAllCellHeight)
            _filterButtonImageSize = New Size(Math.Round(filterButtonImageHeight * 0.8), Math.Round(filterButtonImageHeight * 0.8))

            Dim oldCellt As ColumnHeaderCell = TryCast(oldCell, ColumnHeaderCell)
            If oldCellt IsNot Nothing AndAlso oldCellt.MenuStrip IsNot Nothing Then
                MenuStrip = oldCellt.MenuStrip
                _filterImage = oldCellt._filterImage
                _filterButtonPressed = oldCellt._filterButtonPressed
                _filterButtonOver = oldCellt._filterButtonOver
                _filterButtonOffsetBounds = oldCellt._filterButtonOffsetBounds
                _filterButtonImageBounds = oldCellt._filterButtonImageBounds
                AddHandler MenuStrip.FilterChanged, New EventHandler(AddressOf MenuStrip_FilterChanged)
                AddHandler MenuStrip.SortChanged, New EventHandler(AddressOf MenuStrip_SortChanged)
            Else
                MenuStrip = New MenuStrip(oldCell.OwningColumn.ValueType)
                AddHandler MenuStrip.FilterChanged, New EventHandler(AddressOf MenuStrip_FilterChanged)
                AddHandler MenuStrip.SortChanged, New EventHandler(AddressOf MenuStrip_SortChanged)
            End If

            IsFilterDateAndTimeEnabled = FilterDateAndTimeDefaultEnabled
            IsSortEnabled = True
            IsFilterEnabled = True
            IsFilterChecklistEnabled = True
        End Sub
        Protected Overrides Sub Finalize()
            If MenuStrip IsNot Nothing Then
                RemoveHandler MenuStrip.FilterChanged, AddressOf MenuStrip_FilterChanged
                RemoveHandler MenuStrip.SortChanged, AddressOf MenuStrip_SortChanged
            End If
        End Sub

#End Region


#Region "public methods"

        ''' <summary>
        ''' Get or Set the Filter and Sort enabled status
        ''' </summary>
        Public Property FilterAndSortEnabled As Boolean
            Get
                Return _filterEnabled
            End Get
            Set(value As Boolean)
                If Not value Then
                    _filterButtonPressed = False
                    _filterButtonOver = False
                End If

                If value <> _filterEnabled Then
                    _filterEnabled = value
                    Dim refreshed = False
                    If MenuStrip.FilterString.Length > 0 Then
                        MenuStrip_FilterChanged(Me, New EventArgs())
                        refreshed = True
                    End If
                    If MenuStrip.SortString.Length > 0 Then
                        MenuStrip_SortChanged(Me, New EventArgs())
                        refreshed = True
                    End If
                    If Not refreshed Then RepaintCell()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Set or Unset the Filter and Sort to Loaded mode
        ''' </summary>
        ''' <param name="enabled"></param>
        Public Sub SetLoadedMode(enabled As Boolean)
            MenuStrip.SetLoadedMode(enabled)
            RefreshImage()
            RepaintCell()
        End Sub

        ''' <summary>
        ''' Clean Sort
        ''' </summary>
        Public Sub CleanSort()
            If MenuStrip IsNot Nothing AndAlso FilterAndSortEnabled Then
                MenuStrip.CleanSort()
                RefreshImage()
                RepaintCell()
            End If
        End Sub

        ''' <summary>
        ''' Clean Filter
        ''' </summary>
        Public Sub CleanFilter()
            If MenuStrip IsNot Nothing AndAlso FilterAndSortEnabled Then
                MenuStrip.CleanFilter()
                RefreshImage()
                RepaintCell()
            End If
        End Sub

        ''' <summary>
        ''' Sort ASC
        ''' </summary>
        Public Sub SortASC()
            If MenuStrip IsNot Nothing AndAlso FilterAndSortEnabled Then
                MenuStrip.SortASC()
            End If
        End Sub

        ''' <summary>
        ''' Sort DESC
        ''' </summary>
        Public Sub SortDESC()
            If MenuStrip IsNot Nothing AndAlso FilterAndSortEnabled Then
                MenuStrip.SortDESC()
            End If
        End Sub

        ''' <summary>
        ''' Clone the ColumnHeaderCell
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function Clone() As Object
            Return New ColumnHeaderCell(DataGridView, Me, FilterAndSortEnabled)
        End Function

        ''' <summary>
        ''' Get the MenuStrip SortType
        ''' </summary>
        Public ReadOnly Property ActiveSortType As MenuStrip.SortType
            Get
                If MenuStrip IsNot Nothing AndAlso FilterAndSortEnabled Then
                    Return MenuStrip.ActiveSortType
                Else
                    Return MenuStrip.SortType.None
                End If
            End Get
        End Property

        ''' <summary>
        ''' Get the MenuStrip FilterType
        ''' </summary>
        Public ReadOnly Property ActiveFilterType As MenuStrip.FilterType
            Get
                If MenuStrip IsNot Nothing AndAlso FilterAndSortEnabled Then
                    Return MenuStrip.ActiveFilterType
                Else
                    Return MenuStrip.FilterType.None
                End If
            End Get
        End Property

        ''' <summary>
        ''' Get the Sort string
        ''' </summary>
        Public ReadOnly Property SortString As String
            Get
                If MenuStrip IsNot Nothing AndAlso FilterAndSortEnabled Then
                    Return MenuStrip.SortString
                Else
                    Return ""
                End If
            End Get
        End Property

        ''' <summary>
        ''' Get the Filter string
        ''' </summary>
        Public ReadOnly Property FilterString As String
            Get
                If MenuStrip IsNot Nothing AndAlso FilterAndSortEnabled Then
                    Return MenuStrip.FilterString
                Else
                    Return ""
                End If
            End Get
        End Property

        ''' <summary>
        ''' Get the Minimum size
        ''' </summary>
        Public ReadOnly Property MinimumSize As Size
            Get
                Return New Size(_filterButtonImageSize.Width + _filterButtonMargin.Left + _filterButtonMargin.Right, _filterButtonImageSize.Height + _filterButtonMargin.Bottom + _filterButtonMargin.Top)
            End Get
        End Property

        ''' <summary>
        ''' Get or Set the Sort enabled status
        ''' </summary>
        Public Property IsSortEnabled As Boolean
            Get
                Return MenuStrip.IsSortEnabled
            End Get
            Set(value As Boolean)
                MenuStrip.IsSortEnabled = value
            End Set
        End Property

        ''' <summary>
        ''' Get or Set the Filter enabled status
        ''' </summary>
        Public Property IsFilterEnabled As Boolean
            Get
                Return MenuStrip.IsFilterEnabled
            End Get
            Set(value As Boolean)
                MenuStrip.IsFilterEnabled = value
            End Set
        End Property

        ''' <summary>
        ''' Get or Set the Filter enabled status
        ''' </summary>
        Public Property IsFilterChecklistEnabled As Boolean
            Get
                Return MenuStrip.IsFilterChecklistEnabled
            End Get
            Set(value As Boolean)
                MenuStrip.IsFilterChecklistEnabled = value
            End Set
        End Property

        ''' <summary>
        ''' Get or Set the FilterDateAndTime enabled status
        ''' </summary>
        Public Property IsFilterDateAndTimeEnabled As Boolean
            Get
                Return MenuStrip.IsFilterDateAndTimeEnabled
            End Get
            Set(value As Boolean)
                MenuStrip.IsFilterDateAndTimeEnabled = value
            End Set
        End Property

        ''' <summary>
        ''' Get or Set the NOT IN logic for Filter
        ''' </summary>
        Public Property IsMenuStripFilterNOTINLogicEnabled As Boolean
            Get
                Return MenuStrip.IsFilterNOTINLogicEnabled
            End Get
            Set(value As Boolean)
                MenuStrip.IsFilterNOTINLogicEnabled = value
            End Set
        End Property

        ''' <summary>
        ''' Set the text filter search nodes behaviour
        ''' </summary>
        Public Property DoesTextFilterRemoveNodesOnSearch As Boolean
            Get
                Return MenuStrip.DoesTextFilterRemoveNodesOnSearch
            End Get
            Set(value As Boolean)
                MenuStrip.DoesTextFilterRemoveNodesOnSearch = value
            End Set
        End Property

        ''' <summary>
        ''' Number of nodes to enable the TextChanged delay on text filter
        ''' </summary>
        Public Property TextFilterTextChangedDelayNodes As Integer
            Get
                Return MenuStrip.TextFilterTextChangedDelayNodes
            End Get
            Set(value As Integer)
                MenuStrip.TextFilterTextChangedDelayNodes = value
            End Set
        End Property

        ''' <summary>
        ''' Get or Set the Filter Text focus OnShow
        ''' </summary>
        Public Property FilterTextFocusOnShow As Boolean
            Get
                Return MenuStrip.FilterTextFocusOnShow
            End Get
            Set(value As Boolean)
                MenuStrip.FilterTextFocusOnShow = value
            End Set
        End Property

        ''' <summary>
        ''' Enabled or disable Sort capabilities
        ''' </summary>
        ''' <param name="enabled"></param>
        Public Sub SetSortEnabled(enabled As Boolean)
            If MenuStrip IsNot Nothing Then
                MenuStrip.IsSortEnabled = enabled
                MenuStrip.SetSortEnabled(enabled)
            End If
        End Sub

        ''' <summary>
        ''' Enable or disable Filter capabilities
        ''' </summary>
        ''' <param name="enabled"></param>
        Public Sub SetFilterEnabled(enabled As Boolean)
            If MenuStrip IsNot Nothing Then
                MenuStrip.IsFilterEnabled = enabled
                MenuStrip.SetFilterEnabled(enabled)
            End If
        End Sub

        ''' <summary>
        ''' Enable or disable Filter checklist capabilities
        ''' </summary>
        ''' <param name="enabled"></param>
        Public Sub SetFilterChecklistEnabled(enabled As Boolean)
            If MenuStrip IsNot Nothing Then
                MenuStrip.IsFilterChecklistEnabled = enabled
                MenuStrip.SetFilterChecklistEnabled(enabled)
            End If
        End Sub

        ''' <summary>
        ''' Set Filter checklist nodes max
        ''' </summary>
        ''' <param name="maxnodes"></param>
        Public Sub SetFilterChecklistNodesMax(maxnodes As Integer)
            If maxnodes >= 0 Then
                MenuStrip.MaxChecklistNodes = maxnodes
            End If
        End Sub

        ''' <summary>
        ''' Enable or disable Filter checklist nodes max
        ''' </summary>
        ''' <param name="enabled"></param>
        Public Sub EnabledFilterChecklistNodesMax(enabled As Boolean)
            If MenuStrip.MaxChecklistNodes = 0 AndAlso enabled Then
                MenuStrip.MaxChecklistNodes = MenuStrip.DefaultMaxChecklistNodes
            ElseIf MenuStrip.MaxChecklistNodes <> 0 AndAlso Not enabled Then
                MenuStrip.MaxChecklistNodes = 0
            End If
        End Sub

        ''' <summary>
        ''' Enable or disable Filter custom capabilities
        ''' </summary>
        ''' <param name="enabled"></param>
        Public Sub SetFilterCustomEnabled(enabled As Boolean)
            If MenuStrip IsNot Nothing Then
                MenuStrip.IsFilterCustomEnabled = enabled
                MenuStrip.SetFilterCustomEnabled(enabled)
            End If
        End Sub

        ''' <summary>
        ''' Enable or disable Text filter on checklist remove node mode
        ''' </summary>
        ''' <param name="enabled"></param>
        Public Sub SetChecklistTextFilterRemoveNodesOnSearchMode(enabled As Boolean)
            If MenuStrip IsNot Nothing Then
                MenuStrip.SetChecklistTextFilterRemoveNodesOnSearchMode(enabled)
            End If
        End Sub

        ''' <summary>
        ''' Disable text filter TextChanged delay
        ''' </summary>
        Public Sub SetTextFilterTextChangedDelayNodesDisabled()
            If MenuStrip IsNot Nothing Then
                MenuStrip.SetTextFilterTextChangedDelayNodesDisabled()
            End If
        End Sub

        ''' <summary>
        ''' Set text filter TextChanged delay milliseconds
        ''' </summary>
        Public Sub SetTextFilterTextChangedDelayMs(milliseconds As Integer)
            If MenuStrip IsNot Nothing Then
                MenuStrip.TextFilterTextChangedDelayMs = milliseconds
            End If
        End Sub

#End Region


#Region "menustrip events"

        ''' <summary>
        ''' OnFilterChanged event
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub MenuStrip_FilterChanged(sender As Object, e As EventArgs)
            RefreshImage()
            RepaintCell()
            If FilterAndSortEnabled AndAlso FilterChangedEvent IsNot Nothing Then RaiseEvent FilterChanged(Me, New ColumnHeaderCellEventArgs(MenuStrip, OwningColumn))
        End Sub

        ''' <summary>
        ''' OnSortChanged event
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub MenuStrip_SortChanged(sender As Object, e As EventArgs)
            RefreshImage()
            RepaintCell()
            If FilterAndSortEnabled AndAlso SortChangedEvent IsNot Nothing Then RaiseEvent SortChanged(Me, New ColumnHeaderCellEventArgs(MenuStrip, OwningColumn))
        End Sub

        ''' <summary>
        ''' Clean attached events
        ''' </summary>
        Public Sub CleanEvents()
            RemoveHandler MenuStrip.FilterChanged, AddressOf MenuStrip_FilterChanged
            RemoveHandler MenuStrip.SortChanged, AddressOf MenuStrip_SortChanged
        End Sub


#End Region


#Region "paint methods"

        ''' <summary>
        ''' Repaint the Cell
        ''' </summary>
        Private Sub RepaintCell()
            If Displayed AndAlso MyBase.DataGridView IsNot Nothing Then MyBase.DataGridView.InvalidateCell(Me)
        End Sub

        ''' <summary>
        ''' Refrash the Cell image
        ''' </summary>
        Private Sub RefreshImage()
            If ActiveFilterType = MenuStrip.FilterType.Loaded Then
                _filterImage = My.Resources.Resources.ColumnHeader_SavedFilters
            Else
                If ActiveFilterType = MenuStrip.FilterType.None Then
                    If ActiveSortType = MenuStrip.SortType.None Then
                        _filterImage = My.Resources.Resources.ColumnHeader_UnFiltered
                    ElseIf ActiveSortType = MenuStrip.SortType.ASC Then
                        _filterImage = My.Resources.Resources.ColumnHeader_OrderedASC
                    Else
                        _filterImage = My.Resources.Resources.ColumnHeader_OrderedDESC
                    End If
                Else
                    If ActiveSortType = MenuStrip.SortType.None Then
                        _filterImage = My.Resources.Resources.ColumnHeader_Filtered
                    ElseIf ActiveSortType = MenuStrip.SortType.ASC Then
                        _filterImage = My.Resources.Resources.ColumnHeader_FilteredAndOrderedASC
                    Else
                        _filterImage = My.Resources.Resources.ColumnHeader_FilteredAndOrderedDESC
                    End If
                End If
            End If
        End Sub

        ''' <summary>
        ''' Pain method
        ''' </summary>
        ''' <param name="graphics"></param>
        ''' <param name="clipBounds"></param>
        ''' <param name="cellBounds"></param>
        ''' <param name="rowIndex"></param>
        ''' <param name="cellState"></param>
        ''' <param name="value"></param>
        ''' <param name="formattedValue"></param>
        ''' <param name="errorText"></param>
        ''' <param name="cellStyle"></param>
        ''' <param name="advancedBorderStyle"></param>
        ''' <param name="paintParts"></param>
        Protected Overrides Sub Paint(graphics As Graphics, clipBounds As Rectangle, cellBounds As Rectangle, rowIndex As Integer, cellState As DataGridViewElementStates, value As Object, formattedValue As Object, errorText As String, cellStyle As DataGridViewCellStyle, advancedBorderStyle As DataGridViewAdvancedBorderStyle, paintParts As DataGridViewPaintParts)
            If SortGlyphDirection <> SortOrder.None Then SortGlyphDirection = SortOrder.None

            MyBase.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts)

            ' Don't display a dropdown for Image columns
            If OwningColumn.ValueType Is GetType(Bitmap) Then Return

            If FilterAndSortEnabled AndAlso paintParts.HasFlag(DataGridViewPaintParts.ContentBackground) Then
                _filterButtonOffsetBounds = GetFilterBounds(True)
                _filterButtonImageBounds = GetFilterBounds(False)
                Dim buttonBounds = _filterButtonOffsetBounds
                If clipBounds.IntersectsWith(buttonBounds) Then
                    ControlPaint.DrawBorder(graphics, buttonBounds, Color.Gray, ButtonBorderStyle.Solid)
                    buttonBounds.Inflate(-1, -1)
                    Using b As Brush = New SolidBrush(If(_filterButtonOver, Color.WhiteSmoke, Color.White))
                        graphics.FillRectangle(b, buttonBounds)
                    End Using
                    graphics.DrawImage(_filterImage, buttonBounds)
                End If
            End If
        End Sub

        ''' <summary>
        ''' Get the ColumnHeaderCell Bounds
        ''' </summary>
        ''' <param name="withOffset"></param>
        ''' <returns></returns>
        Private Function GetFilterBounds(Optional withOffset As Boolean = True) As Rectangle
            Dim cell = MyBase.DataGridView.GetCellDisplayRectangle(ColumnIndex, -1, False)

            Dim p As Point = New Point(If(withOffset, cell.Right, cell.Width) - _filterButtonImageSize.Width - _filterButtonMargin.Right, If(withOffset, cell.Bottom + 2, cell.Height) - _filterButtonImageSize.Height - _filterButtonMargin.Bottom)

            Return New Rectangle(p, _filterButtonImageSize)
        End Function

#End Region


#Region "mouse events"

        ''' <summary>
        ''' OnMouseMove event
        ''' </summary>
        ''' <param name="e"></param>
        Protected Overrides Sub OnMouseMove(e As DataGridViewCellMouseEventArgs)
            If FilterAndSortEnabled Then
                If _filterButtonImageBounds.Contains(e.X, e.Y) AndAlso Not _filterButtonOver Then
                    _filterButtonOver = True
                    RepaintCell()
                ElseIf Not _filterButtonImageBounds.Contains(e.X, e.Y) AndAlso _filterButtonOver Then
                    _filterButtonOver = False
                    RepaintCell()
                End If
            End If
            MyBase.OnMouseMove(e)
        End Sub

        ''' <summary>
        ''' OnMouseDown event
        ''' </summary>
        ''' <param name="e"></param>
        Protected Overrides Sub OnMouseDown(e As DataGridViewCellMouseEventArgs)
            If FilterAndSortEnabled AndAlso _filterButtonImageBounds.Contains(e.X, e.Y) Then
                If e.Button = MouseButtons.Left AndAlso Not _filterButtonPressed Then
                    _filterButtonPressed = True
                    _filterButtonOver = True
                    RepaintCell()
                End If
            Else
                MyBase.OnMouseDown(e)
            End If
        End Sub

        ''' <summary>
        ''' OnMouseUp event
        ''' </summary>
        ''' <param name="e"></param>
        Protected Overrides Sub OnMouseUp(e As DataGridViewCellMouseEventArgs)
            If FilterAndSortEnabled AndAlso e.Button = MouseButtons.Left AndAlso _filterButtonPressed Then
                _filterButtonPressed = False
                _filterButtonOver = False
                RepaintCell()
                If _filterButtonImageBounds.Contains(e.X, e.Y) AndAlso FilterPopupEvent IsNot Nothing Then
                    RaiseEvent FilterPopup(Me, New ColumnHeaderCellEventArgs(MenuStrip, OwningColumn))
                End If
            End If
            MyBase.OnMouseUp(e)
        End Sub

        ''' <summary>
        ''' OnMouseLeave event
        ''' </summary>
        ''' <param name="rowIndex"></param>
        Protected Overrides Sub OnMouseLeave(rowIndex As Integer)
            If FilterAndSortEnabled AndAlso _filterButtonOver Then
                _filterButtonOver = False
                RepaintCell()
            End If

            MyBase.OnMouseLeave(rowIndex)
        End Sub

#End Region

    End Class
End Namespace
