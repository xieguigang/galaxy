'Copyright (c) Microsoft Corporation.  All rights reserved.
Imports System.Collections.ObjectModel
Imports System.Threading
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Forms.Integration
Imports System.Windows.Threading
Imports Microsoft.Windows.Shell

Namespace Controls.WindowsPresentationFoundation

    Partial Public Class ExplorerBrowser
        Inherits UserControl
        Implements IDisposable
        ''' <summary>
        ''' The underlying WinForms control
        ''' </summary>
        Public Property ExplorerBrowserControl() As Controls.WindowsForms.ExplorerBrowser
            Get
                Return m_ExplorerBrowserControl
            End Get
            Set
                m_ExplorerBrowserControl = Value
            End Set
        End Property
        Private m_ExplorerBrowserControl As Controls.WindowsForms.ExplorerBrowser

        Private m_selectedItems As ObservableCollection(Of ShellObject)
        Private m_items As ObservableCollection(Of ShellObject)
        Private m_navigationLog As ObservableCollection(Of ShellObject)
        Private dtCLRUpdater As New DispatcherTimer()

        Private initialNavigationTarget As ShellObject
        Private initialViewMode As System.Nullable(Of ExplorerBrowserViewMode)

        Private _itemsChanged As New AutoResetEvent(False)
        Private _selectionChanged As New AutoResetEvent(False)
        Private selectionChangeWaitCount As Integer

        ''' <summary>
        ''' Hosts the ExplorerBrowser WinForms wrapper in this control
        ''' </summary>
        Public Sub New()
            InitializeComponent()

            ' the ExplorerBrowser WinForms control
            ExplorerBrowserControl = New Controls.WindowsForms.ExplorerBrowser()

            ' back the dependency collection properties with instances
            SelectedItems = m_selectedItems.DirectCopy(New ObservableCollection(Of ShellObject)())
            Items = m_items.DirectCopy(New ObservableCollection(Of ShellObject)())
            NavigationLog = m_navigationLog.DirectCopy(New ObservableCollection(Of ShellObject)())

            ' hook up events for collection synchronization
            AddHandler ExplorerBrowserControl.ItemsChanged, New EventHandler(AddressOf ItemsChanged)
            AddHandler ExplorerBrowserControl.SelectionChanged, New EventHandler(AddressOf SelectionChanged)
            AddHandler ExplorerBrowserControl.ViewEnumerationComplete, New EventHandler(AddressOf ExplorerBrowserControl_ViewEnumerationComplete)
            AddHandler ExplorerBrowserControl.ViewSelectedItemChanged, New EventHandler(AddressOf ExplorerBrowserControl_ViewSelectedItemChanged)
            AddHandler ExplorerBrowserControl.NavigationLog.NavigationLogChanged, New EventHandler(Of NavigationLogEventArgs)(AddressOf NavigationLogChanged)

            ' host the control           
            Dim host As New WindowsFormsHost()
            Try
                host.Child = ExplorerBrowserControl
                Me.root.Children.Clear()
                Me.root.Children.Add(host)
            Catch
                host.Dispose()
                Throw
            End Try

            AddHandler Loaded, New RoutedEventHandler(AddressOf ExplorerBrowser_Loaded)
        End Sub

        Private Sub ExplorerBrowserControl_ViewSelectedItemChanged(sender As Object, e As EventArgs)
        End Sub

        Private Sub ExplorerBrowserControl_ViewEnumerationComplete(sender As Object, e As EventArgs)
            _itemsChanged.[Set]()
            _selectionChanged.[Set]()
        End Sub

        ''' <summary>
        ''' To avoid the 'Dispatcher processing has been suspended' InvalidOperationException on Win7,
        ''' the ExplorerBorwser native control is initialized after this control is fully loaded.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub ExplorerBrowser_Loaded(sender As Object, e As RoutedEventArgs)
            ' setup timer to update dependency properties from CLR properties of WinForms ExplorerBrowser object
            AddHandler dtCLRUpdater.Tick, New EventHandler(AddressOf UpdateDependencyPropertiesFromCLRPRoperties)
            dtCLRUpdater.Interval = New TimeSpan(100 * 10000)
            ' 100ms
            dtCLRUpdater.Start()

            If initialNavigationTarget IsNot Nothing Then
                ExplorerBrowserControl.Navigate(initialNavigationTarget)
                initialNavigationTarget = Nothing
            End If

            If initialViewMode IsNot Nothing Then
                ExplorerBrowserControl.ContentOptions.ViewMode = CType(initialViewMode, ExplorerBrowserViewMode)
                initialViewMode = Nothing
            End If
        End Sub

        ''' <summary>
        ''' Map changes to the CLR flags to the dependency properties
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub UpdateDependencyPropertiesFromCLRPRoperties(sender As Object, e As EventArgs)
            AlignLeft = ExplorerBrowserControl.ContentOptions.AlignLeft
            AutoArrange = ExplorerBrowserControl.ContentOptions.AutoArrange
            CheckSelect = ExplorerBrowserControl.ContentOptions.CheckSelect
            ExtendedTiles = ExplorerBrowserControl.ContentOptions.ExtendedTiles
            FullRowSelect = ExplorerBrowserControl.ContentOptions.FullRowSelect
            HideFileNames = ExplorerBrowserControl.ContentOptions.HideFileNames
            NoBrowserViewState = ExplorerBrowserControl.ContentOptions.NoBrowserViewState
            NoColumnHeader = ExplorerBrowserControl.ContentOptions.NoColumnHeader
            NoHeaderInAllViews = ExplorerBrowserControl.ContentOptions.NoHeaderInAllViews
            NoIcons = ExplorerBrowserControl.ContentOptions.NoIcons
            NoSubfolders = ExplorerBrowserControl.ContentOptions.NoSubfolders
            SingleClickActivate = ExplorerBrowserControl.ContentOptions.SingleClickActivate
            SingleSelection = ExplorerBrowserControl.ContentOptions.SingleSelection
            ThumbnailSize = ExplorerBrowserControl.ContentOptions.ThumbnailSize
            ViewMode = ExplorerBrowserControl.ContentOptions.ViewMode
            AlwaysNavigate = ExplorerBrowserControl.NavigationOptions.AlwaysNavigate
            NavigateOnce = ExplorerBrowserControl.NavigationOptions.NavigateOnce
            AdvancedQueryPane = ExplorerBrowserControl.NavigationOptions.PaneVisibility.AdvancedQuery
            CommandsPane = ExplorerBrowserControl.NavigationOptions.PaneVisibility.Commands
            CommandsOrganizePane = ExplorerBrowserControl.NavigationOptions.PaneVisibility.CommandsOrganize
            CommandsViewPane = ExplorerBrowserControl.NavigationOptions.PaneVisibility.CommandsView
            DetailsPane = ExplorerBrowserControl.NavigationOptions.PaneVisibility.Details
            NavigationPane = ExplorerBrowserControl.NavigationOptions.PaneVisibility.Navigation
            PreviewPane = ExplorerBrowserControl.NavigationOptions.PaneVisibility.Preview
            QueryPane = ExplorerBrowserControl.NavigationOptions.PaneVisibility.Query
            NavigationLogIndex = ExplorerBrowserControl.NavigationLog.CurrentLocationIndex

            If _itemsChanged.WaitOne(1, False) Then
                m_items.Clear()
                For Each obj As ShellObject In ExplorerBrowserControl.Items
                    m_items.Add(obj)
                Next
            End If

            If _selectionChanged.WaitOne(1, False) Then
                selectionChangeWaitCount = 4
            ElseIf selectionChangeWaitCount > 0 Then
                selectionChangeWaitCount -= 1

                If selectionChangeWaitCount = 0 Then
                    m_selectedItems.Clear()
                    For Each obj As ShellObject In ExplorerBrowserControl.SelectedItems
                        m_selectedItems.Add(obj)
                    Next
                End If
            End If
        End Sub

        ''' <summary>
        ''' Synchronize NavigationLog collection to dependency collection
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="args"></param>
        Private Sub NavigationLogChanged(sender As Object, args As NavigationLogEventArgs)
            m_navigationLog.Clear()
            For Each obj As ShellObject In ExplorerBrowserControl.NavigationLog.Locations
                m_navigationLog.Add(obj)
            Next
        End Sub

        ''' <summary>
        ''' Synchronize SelectedItems collection to dependency collection
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub SelectionChanged(sender As Object, e As EventArgs)
            _selectionChanged.[Set]()
        End Sub

        ' Synchronize ItemsCollection to dependency collection
        Private Sub ItemsChanged(sender As Object, e As EventArgs)
            _itemsChanged.[Set]()
        End Sub

        ''' <summary>
        ''' The items in the ExplorerBrowser window
        ''' </summary>
        Public Property Items() As ObservableCollection(Of ShellObject)
            Get
                Return DirectCast(GetValue(ItemsProperty), ObservableCollection(Of ShellObject))
            End Get
            Set
                SetValue(ItemsPropertyKey, Value)
            End Set
        End Property

        Private Shared ReadOnly ItemsPropertyKey As DependencyPropertyKey = DependencyProperty.RegisterReadOnly("Items", GetType(ObservableCollection(Of ShellObject)), GetType(ExplorerBrowser), New PropertyMetadata(Nothing))

        ''' <summary>
        ''' The items in the ExplorerBrowser window
        ''' </summary>
        Public Shared ReadOnly ItemsProperty As DependencyProperty = ItemsPropertyKey.DependencyProperty

        ''' <summary>
        ''' The selected items in the ExplorerBrowser window
        ''' </summary>
        Public Property SelectedItems() As ObservableCollection(Of ShellObject)
            Get
                Return DirectCast(GetValue(SelectedItemsProperty), ObservableCollection(Of ShellObject))
            End Get
            Friend Set
                SetValue(SelectedItemsPropertyKey, Value)
            End Set
        End Property

        Private Shared ReadOnly SelectedItemsPropertyKey As DependencyPropertyKey = DependencyProperty.RegisterReadOnly("SelectedItems", GetType(ObservableCollection(Of ShellObject)), GetType(ExplorerBrowser), New PropertyMetadata(Nothing))

        ''' <summary>
        ''' The selected items in the ExplorerBrowser window
        ''' </summary>
        Public Property NavigationLog() As ObservableCollection(Of ShellObject)
            Get
                Return DirectCast(GetValue(NavigationLogProperty), ObservableCollection(Of ShellObject))
            End Get
            Friend Set
                SetValue(NavigationLogPropertyKey, Value)
            End Set
        End Property

        Private Shared ReadOnly NavigationLogPropertyKey As DependencyPropertyKey = DependencyProperty.RegisterReadOnly("NavigationLog", GetType(ObservableCollection(Of ShellObject)), GetType(ExplorerBrowser), New PropertyMetadata(Nothing))

        ''' <summary>
        ''' The NavigationLog
        ''' </summary>
        Public Shared ReadOnly NavigationLogProperty As DependencyProperty = NavigationLogPropertyKey.DependencyProperty

        ''' <summary>
        ''' The selected items in the ExplorerBrowser window
        ''' </summary>
        Public Shared ReadOnly SelectedItemsProperty As DependencyProperty = SelectedItemsPropertyKey.DependencyProperty


        ''' <summary>
        ''' The location the explorer browser is navigating to
        ''' </summary>
        Public Property NavigationTarget() As ShellObject
            Get
                Return DirectCast(GetValue(NavigationTargetProperty), ShellObject)
            End Get
            Set
                SetValue(NavigationTargetProperty, Value)
            End Set
        End Property

        ''' <summary>
        ''' The DependencyProperty for the NavigationTarget property
        ''' </summary>
        Public Shared ReadOnly NavigationTargetProperty As DependencyProperty = DependencyProperty.Register("NavigationTarget", GetType(ShellObject), GetType(ExplorerBrowser), New PropertyMetadata(Nothing, AddressOf navigationTargetChanged))

        Private Shared Sub navigationTargetChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
            Dim instance As ExplorerBrowser = TryCast(d, ExplorerBrowser)

            If instance.ExplorerBrowserControl.explorerBrowserControl IsNot Nothing Then
                instance.ExplorerBrowserControl.Navigate(DirectCast(e.NewValue, ShellObject))
            Else
                instance.initialNavigationTarget = DirectCast(e.NewValue, ShellObject)
            End If
        End Sub

        ''' <summary>
        ''' The view should be left-aligned. 
        ''' </summary>
        Public Property AlignLeft() As Boolean
            Get
                Return CBool(GetValue(AlignLeftProperty))
            End Get
            Set
                SetValue(AlignLeftProperty, Value)
            End Set
        End Property

        Friend Shared AlignLeftProperty As DependencyProperty = DependencyProperty.Register("AlignLeft", GetType(Boolean), GetType(ExplorerBrowser), New PropertyMetadata(False, AddressOf OnAlignLeftChanged))

        Private Shared Sub OnAlignLeftChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
            Dim instance As ExplorerBrowser = TryCast(d, ExplorerBrowser)
            If instance.ExplorerBrowserControl IsNot Nothing Then
                instance.ExplorerBrowserControl.ContentOptions.AlignLeft = CBool(e.NewValue)
            End If
        End Sub


        ''' <summary>
        ''' Automatically arrange the elements in the view. 
        ''' </summary>
        Public Property AutoArrange() As Boolean
            Get
                Return CBool(GetValue(AutoArrangeProperty))
            End Get
            Set
                SetValue(AutoArrangeProperty, Value)
            End Set
        End Property

        Friend Shared AutoArrangeProperty As DependencyProperty = DependencyProperty.Register("AutoArrange", GetType(Boolean), GetType(ExplorerBrowser), New PropertyMetadata(False, AddressOf OnAutoArrangeChanged))

        Private Shared Sub OnAutoArrangeChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
            Dim instance As ExplorerBrowser = TryCast(d, ExplorerBrowser)
            If instance.ExplorerBrowserControl IsNot Nothing Then
                instance.ExplorerBrowserControl.ContentOptions.AutoArrange = CBool(e.NewValue)
            End If
        End Sub

        ''' <summary>
        ''' Turns on check mode for the view
        ''' </summary>
        Public Property CheckSelect() As Boolean
            Get
                Return CBool(GetValue(CheckSelectProperty))
            End Get
            Set
                SetValue(CheckSelectProperty, Value)
            End Set
        End Property

        Friend Shared CheckSelectProperty As DependencyProperty = DependencyProperty.Register("CheckSelect", GetType(Boolean), GetType(ExplorerBrowser), New PropertyMetadata(False, AddressOf OnCheckSelectChanged))

        Private Shared Sub OnCheckSelectChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
            Dim instance As ExplorerBrowser = TryCast(d, ExplorerBrowser)
            If instance.ExplorerBrowserControl IsNot Nothing Then
                instance.ExplorerBrowserControl.ContentOptions.CheckSelect = CBool(e.NewValue)
            End If
        End Sub

        ''' <summary>
        ''' When the view is in "tile view mode" the layout of a single item should be extended to the width of the view.
        ''' </summary>
        Public Property ExtendedTiles() As Boolean
            Get
                Return CBool(GetValue(ExtendedTilesProperty))
            End Get
            Set
                SetValue(ExtendedTilesProperty, Value)
            End Set
        End Property

        Friend Shared ExtendedTilesProperty As DependencyProperty = DependencyProperty.Register("ExtendedTiles", GetType(Boolean), GetType(ExplorerBrowser), New PropertyMetadata(False, AddressOf OnExtendedTilesChanged))

        Private Shared Sub OnExtendedTilesChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
            Dim instance As ExplorerBrowser = TryCast(d, ExplorerBrowser)
            If instance.ExplorerBrowserControl IsNot Nothing Then
                instance.ExplorerBrowserControl.ContentOptions.ExtendedTiles = CBool(e.NewValue)
            End If
        End Sub

        ''' <summary>
        ''' When an item is selected, the item and all its sub-items are highlighted.
        ''' </summary>
        Public Property FullRowSelect() As Boolean
            Get
                Return CBool(GetValue(FullRowSelectProperty))
            End Get
            Set
                SetValue(FullRowSelectProperty, Value)
            End Set
        End Property

        Friend Shared FullRowSelectProperty As DependencyProperty = DependencyProperty.Register("FullRowSelect", GetType(Boolean), GetType(ExplorerBrowser), New PropertyMetadata(False, AddressOf OnFullRowSelectChanged))

        Private Shared Sub OnFullRowSelectChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
            Dim instance As ExplorerBrowser = TryCast(d, ExplorerBrowser)
            If instance.ExplorerBrowserControl IsNot Nothing Then
                instance.ExplorerBrowserControl.ContentOptions.FullRowSelect = CBool(e.NewValue)
            End If
        End Sub

        ''' <summary>
        ''' The view should not display file names
        ''' </summary>
        Public Property HideFileNames() As Boolean
            Get
                Return CBool(GetValue(HideFileNamesProperty))
            End Get
            Set
                SetValue(HideFileNamesProperty, Value)
            End Set
        End Property

        Friend Shared HideFileNamesProperty As DependencyProperty = DependencyProperty.Register("HideFileNames", GetType(Boolean), GetType(ExplorerBrowser), New PropertyMetadata(False, AddressOf OnHideFileNamesChanged))

        Private Shared Sub OnHideFileNamesChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
            Dim instance As ExplorerBrowser = TryCast(d, ExplorerBrowser)
            If instance.ExplorerBrowserControl IsNot Nothing Then
                instance.ExplorerBrowserControl.ContentOptions.HideFileNames = CBool(e.NewValue)
            End If
        End Sub

        ''' <summary>
        ''' The view should not save view state in the browser.
        ''' </summary>
        Public Property NoBrowserViewState() As Boolean
            Get
                Return CBool(GetValue(NoBrowserViewStateProperty))
            End Get
            Set
                SetValue(NoBrowserViewStateProperty, Value)
            End Set
        End Property

        Friend Shared NoBrowserViewStateProperty As DependencyProperty = DependencyProperty.Register("NoBrowserViewState", GetType(Boolean), GetType(ExplorerBrowser), New PropertyMetadata(False, AddressOf OnNoBrowserViewStateChanged))

        Private Shared Sub OnNoBrowserViewStateChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
            Dim instance As ExplorerBrowser = TryCast(d, ExplorerBrowser)
            If instance.ExplorerBrowserControl IsNot Nothing Then
                instance.ExplorerBrowserControl.ContentOptions.NoBrowserViewState = CBool(e.NewValue)
            End If
        End Sub

        ''' <summary>
        ''' Do not display a column header in the view in any view mode.
        ''' </summary>
        Public Property NoColumnHeader() As Boolean
            Get
                Return CBool(GetValue(NoColumnHeaderProperty))
            End Get
            Set
                SetValue(NoColumnHeaderProperty, Value)
            End Set
        End Property

        Friend Shared NoColumnHeaderProperty As DependencyProperty = DependencyProperty.Register("NoColumnHeader", GetType(Boolean), GetType(ExplorerBrowser), New PropertyMetadata(False, AddressOf OnNoColumnHeaderChanged))

        Private Shared Sub OnNoColumnHeaderChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
            Dim instance As ExplorerBrowser = TryCast(d, ExplorerBrowser)
            If instance.ExplorerBrowserControl IsNot Nothing Then
                instance.ExplorerBrowserControl.ContentOptions.NoColumnHeader = CBool(e.NewValue)
            End If
        End Sub

        ''' <summary>
        ''' Only show the column header in details view mode.
        ''' </summary>
        Public Property NoHeaderInAllViews() As Boolean
            Get
                Return CBool(GetValue(NoHeaderInAllViewsProperty))
            End Get
            Set
                SetValue(NoHeaderInAllViewsProperty, Value)
            End Set
        End Property

        Friend Shared NoHeaderInAllViewsProperty As DependencyProperty = DependencyProperty.Register("NoHeaderInAllViews", GetType(Boolean), GetType(ExplorerBrowser), New PropertyMetadata(False, AddressOf OnNoHeaderInAllViewsChanged))

        Private Shared Sub OnNoHeaderInAllViewsChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
            Dim instance As ExplorerBrowser = TryCast(d, ExplorerBrowser)
            If instance.ExplorerBrowserControl IsNot Nothing Then
                instance.ExplorerBrowserControl.ContentOptions.NoHeaderInAllViews = CBool(e.NewValue)
            End If
        End Sub

        ''' <summary>
        ''' The view should not display icons. 
        ''' </summary>
        Public Property NoIcons() As Boolean
            Get
                Return CBool(GetValue(NoIconsProperty))
            End Get
            Set
                SetValue(NoIconsProperty, Value)
            End Set
        End Property

        Friend Shared NoIconsProperty As DependencyProperty = DependencyProperty.Register("NoIcons", GetType(Boolean), GetType(ExplorerBrowser), New PropertyMetadata(False, AddressOf OnNoIconsChanged))

        Private Shared Sub OnNoIconsChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
            Dim instance As ExplorerBrowser = TryCast(d, ExplorerBrowser)
            If instance.ExplorerBrowserControl IsNot Nothing Then
                instance.ExplorerBrowserControl.ContentOptions.NoIcons = CBool(e.NewValue)
            End If
        End Sub

        ''' <summary>
        ''' Do not show subfolders. 
        ''' </summary>
        Public Property NoSubfolders() As Boolean
            Get
                Return CBool(GetValue(NoSubfoldersProperty))
            End Get
            Set
                SetValue(NoSubfoldersProperty, Value)
            End Set
        End Property

        Friend Shared NoSubfoldersProperty As DependencyProperty = DependencyProperty.Register("NoSubfolders", GetType(Boolean), GetType(ExplorerBrowser), New PropertyMetadata(False, AddressOf OnNoSubfoldersChanged))

        Private Shared Sub OnNoSubfoldersChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
            Dim instance As ExplorerBrowser = TryCast(d, ExplorerBrowser)
            If instance.ExplorerBrowserControl IsNot Nothing Then
                instance.ExplorerBrowserControl.ContentOptions.NoSubfolders = CBool(e.NewValue)
            End If
        End Sub

        ''' <summary>
        ''' Navigate with a single click
        ''' </summary>
        Public Property SingleClickActivate() As Boolean
            Get
                Return CBool(GetValue(SingleClickActivateProperty))
            End Get
            Set
                SetValue(SingleClickActivateProperty, Value)
            End Set
        End Property

        Friend Shared SingleClickActivateProperty As DependencyProperty = DependencyProperty.Register("SingleClickActivate", GetType(Boolean), GetType(ExplorerBrowser), New PropertyMetadata(False, AddressOf OnSingleClickActivateChanged))

        Private Shared Sub OnSingleClickActivateChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
            Dim instance As ExplorerBrowser = TryCast(d, ExplorerBrowser)
            If instance.ExplorerBrowserControl IsNot Nothing Then
                instance.ExplorerBrowserControl.ContentOptions.SingleClickActivate = CBool(e.NewValue)
            End If
        End Sub

        ''' <summary>
        ''' Do not allow more than a single item to be selected.
        ''' </summary>
        Public Property SingleSelection() As Boolean
            Get
                Return CBool(GetValue(SingleSelectionProperty))
            End Get
            Set
                SetValue(SingleSelectionProperty, Value)
            End Set
        End Property

        Friend Shared SingleSelectionProperty As DependencyProperty = DependencyProperty.Register("SingleSelection", GetType(Boolean), GetType(ExplorerBrowser), New PropertyMetadata(False, AddressOf OnSingleSelectionChanged))

        Private Shared Sub OnSingleSelectionChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
            Dim instance As ExplorerBrowser = TryCast(d, ExplorerBrowser)
            If instance.ExplorerBrowserControl IsNot Nothing Then
                instance.ExplorerBrowserControl.ContentOptions.SingleSelection = CBool(e.NewValue)
            End If
        End Sub

        ''' <summary>
        ''' The size of the thumbnails in the explorer browser
        ''' </summary>
        Public Property ThumbnailSize() As Integer
            Get
                Return CInt(GetValue(ThumbnailSizeProperty))
            End Get
            Set
                SetValue(ThumbnailSizeProperty, Value)
            End Set
        End Property

        Friend Shared ThumbnailSizeProperty As DependencyProperty = DependencyProperty.Register("ThumbnailSize", GetType(Integer), GetType(ExplorerBrowser), New PropertyMetadata(32, AddressOf OnThumbnailSizeChanged))

        Private Shared Sub OnThumbnailSizeChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
            Dim instance As ExplorerBrowser = TryCast(d, ExplorerBrowser)
            If instance.ExplorerBrowserControl IsNot Nothing Then
                instance.ExplorerBrowserControl.ContentOptions.ThumbnailSize = CInt(e.NewValue)
            End If
        End Sub



        ''' <summary>
        ''' The various view modes of the explorer browser control
        ''' </summary>
        Public Property ViewMode() As ExplorerBrowserViewMode
            Get
                Return CType(GetValue(ViewModeProperty), ExplorerBrowserViewMode)
            End Get
            Set
                SetValue(ViewModeProperty, Value)
            End Set
        End Property

        Friend Shared ViewModeProperty As DependencyProperty = DependencyProperty.Register("ViewMode", GetType(ExplorerBrowserViewMode), GetType(ExplorerBrowser), New PropertyMetadata(ExplorerBrowserViewMode.Auto, AddressOf OnViewModeChanged))

        Private Shared Sub OnViewModeChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
            Dim instance As ExplorerBrowser = TryCast(d, ExplorerBrowser)

            If instance.ExplorerBrowserControl IsNot Nothing Then
                If instance.ExplorerBrowserControl.explorerBrowserControl Is Nothing Then
                    instance.initialViewMode = CType(e.NewValue, ExplorerBrowserViewMode)
                Else
                    instance.ExplorerBrowserControl.ContentOptions.ViewMode = CType(e.NewValue, ExplorerBrowserViewMode)
                End If
            End If
        End Sub


        ''' <summary>
        ''' Always navigate, even if you are attempting to navigate to the current folder.
        ''' </summary>
        Public Property AlwaysNavigate() As Boolean
            Get
                Return CBool(GetValue(AlwaysNavigateProperty))
            End Get
            Set
                SetValue(AlwaysNavigateProperty, Value)
            End Set
        End Property

        Friend Shared AlwaysNavigateProperty As DependencyProperty = DependencyProperty.Register("AlwaysNavigate", GetType(Boolean), GetType(ExplorerBrowser), New PropertyMetadata(False, AddressOf OnAlwaysNavigateChanged))

        Private Shared Sub OnAlwaysNavigateChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
            Dim instance As ExplorerBrowser = TryCast(d, ExplorerBrowser)
            If instance.ExplorerBrowserControl IsNot Nothing Then
                instance.ExplorerBrowserControl.NavigationOptions.AlwaysNavigate = CBool(e.NewValue)
            End If
        End Sub

        ''' <summary>
        ''' Do not navigate further than the initial navigation.
        ''' </summary>
        Public Property NavigateOnce() As Boolean
            Get
                Return CBool(GetValue(NavigateOnceProperty))
            End Get
            Set
                SetValue(NavigateOnceProperty, Value)
            End Set
        End Property

        Friend Shared NavigateOnceProperty As DependencyProperty = DependencyProperty.Register("NavigateOnce", GetType(Boolean), GetType(ExplorerBrowser), New PropertyMetadata(False, AddressOf OnNavigateOnceChanged))

        Private Shared Sub OnNavigateOnceChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
            Dim instance As ExplorerBrowser = TryCast(d, ExplorerBrowser)
            If instance.ExplorerBrowserControl IsNot Nothing Then
                instance.ExplorerBrowserControl.NavigationOptions.NavigateOnce = CBool(e.NewValue)
            End If
        End Sub

        ''' <summary>
        ''' Show/Hide the AdvancedQuery pane on subsequent navigation
        ''' </summary>
        Public Property AdvancedQueryPane() As PaneVisibilityState
            Get
                Return CType(GetValue(AdvancedQueryPaneProperty), PaneVisibilityState)
            End Get
            Set
                SetValue(AdvancedQueryPaneProperty, Value)
            End Set
        End Property

        Friend Shared AdvancedQueryPaneProperty As DependencyProperty = DependencyProperty.Register("AdvancedQueryPane", GetType(PaneVisibilityState), GetType(ExplorerBrowser), New PropertyMetadata(PaneVisibilityState.DoNotCare, AddressOf OnAdvancedQueryPaneChanged))

        Private Shared Sub OnAdvancedQueryPaneChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
            Dim instance As ExplorerBrowser = TryCast(d, ExplorerBrowser)
            If instance.ExplorerBrowserControl IsNot Nothing Then
                instance.ExplorerBrowserControl.NavigationOptions.PaneVisibility.AdvancedQuery = CType(e.NewValue, PaneVisibilityState)
            End If
        End Sub

        ''' <summary>
        ''' Show/Hide the Commands pane on subsequent navigation
        ''' </summary>
        Public Property CommandsPane() As PaneVisibilityState
            Get
                Return CType(GetValue(CommandsPaneProperty), PaneVisibilityState)
            End Get
            Set
                SetValue(CommandsPaneProperty, Value)
            End Set
        End Property

        Friend Shared CommandsPaneProperty As DependencyProperty = DependencyProperty.Register("CommandsPane", GetType(PaneVisibilityState), GetType(ExplorerBrowser), New PropertyMetadata(PaneVisibilityState.DoNotCare, AddressOf OnCommandsPaneChanged))

        Private Shared Sub OnCommandsPaneChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
            Dim instance As ExplorerBrowser = TryCast(d, ExplorerBrowser)
            If instance.ExplorerBrowserControl IsNot Nothing Then
                instance.ExplorerBrowserControl.NavigationOptions.PaneVisibility.Commands = CType(e.NewValue, PaneVisibilityState)
            End If
        End Sub

        ''' <summary>
        ''' Show/Hide the Organize menu in the Commands pane on subsequent navigation
        ''' </summary>
        Public Property CommandsOrganizePane() As PaneVisibilityState
            Get
                Return CType(GetValue(CommandsOrganizePaneProperty), PaneVisibilityState)
            End Get
            Set
                SetValue(CommandsOrganizePaneProperty, Value)
            End Set
        End Property

        Friend Shared CommandsOrganizePaneProperty As DependencyProperty = DependencyProperty.Register("CommandsOrganizePane", GetType(PaneVisibilityState), GetType(ExplorerBrowser), New PropertyMetadata(PaneVisibilityState.DoNotCare, AddressOf OnCommandsOrganizePaneChanged))

        Private Shared Sub OnCommandsOrganizePaneChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
            Dim instance As ExplorerBrowser = TryCast(d, ExplorerBrowser)
            If instance.ExplorerBrowserControl IsNot Nothing Then
                instance.ExplorerBrowserControl.NavigationOptions.PaneVisibility.CommandsOrganize = CType(e.NewValue, PaneVisibilityState)
            End If
        End Sub

        ''' <summary>
        ''' Show/Hide the View menu in the Commands pane on subsequent navigation
        ''' </summary>
        Public Property CommandsViewPane() As PaneVisibilityState
            Get
                Return CType(GetValue(CommandsViewPaneProperty), PaneVisibilityState)
            End Get
            Set
                SetValue(CommandsViewPaneProperty, Value)
            End Set
        End Property

        Friend Shared CommandsViewPaneProperty As DependencyProperty = DependencyProperty.Register("CommandsViewPane", GetType(PaneVisibilityState), GetType(ExplorerBrowser), New PropertyMetadata(PaneVisibilityState.DoNotCare, AddressOf OnCommandsViewPaneChanged))

        Private Shared Sub OnCommandsViewPaneChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
            Dim instance As ExplorerBrowser = TryCast(d, ExplorerBrowser)
            If instance.ExplorerBrowserControl IsNot Nothing Then
                instance.ExplorerBrowserControl.NavigationOptions.PaneVisibility.CommandsView = CType(e.NewValue, PaneVisibilityState)
            End If
        End Sub

        ''' <summary>
        ''' Show/Hide the Details pane on subsequent navigation
        ''' </summary>
        Public Property DetailsPane() As PaneVisibilityState
            Get
                Return CType(GetValue(DetailsPaneProperty), PaneVisibilityState)
            End Get
            Set
                SetValue(DetailsPaneProperty, Value)
            End Set
        End Property

        Friend Shared DetailsPaneProperty As DependencyProperty = DependencyProperty.Register("DetailsPane", GetType(PaneVisibilityState), GetType(ExplorerBrowser), New PropertyMetadata(PaneVisibilityState.DoNotCare, AddressOf OnDetailsPaneChanged))

        Private Shared Sub OnDetailsPaneChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
            Dim instance As ExplorerBrowser = TryCast(d, ExplorerBrowser)
            If instance.ExplorerBrowserControl IsNot Nothing Then
                instance.ExplorerBrowserControl.NavigationOptions.PaneVisibility.Details = CType(e.NewValue, PaneVisibilityState)
            End If
        End Sub

        ''' <summary>
        ''' Show/Hide the Navigation pane on subsequent navigation
        ''' </summary>
        Public Property NavigationPane() As PaneVisibilityState
            Get
                Return CType(GetValue(NavigationPaneProperty), PaneVisibilityState)
            End Get
            Set
                SetValue(NavigationPaneProperty, Value)
            End Set
        End Property

        Friend Shared NavigationPaneProperty As DependencyProperty = DependencyProperty.Register("NavigationPane", GetType(PaneVisibilityState), GetType(ExplorerBrowser), New PropertyMetadata(PaneVisibilityState.DoNotCare, AddressOf OnNavigationPaneChanged))

        Private Shared Sub OnNavigationPaneChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
            Dim instance As ExplorerBrowser = TryCast(d, ExplorerBrowser)
            If instance.ExplorerBrowserControl IsNot Nothing Then
                instance.ExplorerBrowserControl.NavigationOptions.PaneVisibility.Navigation = CType(e.NewValue, PaneVisibilityState)
            End If
        End Sub

        ''' <summary>
        ''' Show/Hide the Preview pane on subsequent navigation
        ''' </summary>
        Public Property PreviewPane() As PaneVisibilityState
            Get
                Return CType(GetValue(PreviewPaneProperty), PaneVisibilityState)
            End Get
            Set
                SetValue(PreviewPaneProperty, Value)
            End Set
        End Property

        Friend Shared PreviewPaneProperty As DependencyProperty = DependencyProperty.Register("PreviewPane", GetType(PaneVisibilityState), GetType(ExplorerBrowser), New PropertyMetadata(PaneVisibilityState.DoNotCare, AddressOf OnPreviewPaneChanged))

        Private Shared Sub OnPreviewPaneChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
            Dim instance As ExplorerBrowser = TryCast(d, ExplorerBrowser)
            If instance.ExplorerBrowserControl IsNot Nothing Then
                instance.ExplorerBrowserControl.NavigationOptions.PaneVisibility.Preview = CType(e.NewValue, PaneVisibilityState)
            End If
        End Sub

        ''' <summary>
        ''' Show/Hide the Query pane on subsequent navigation
        ''' </summary>
        Public Property QueryPane() As PaneVisibilityState
            Get
                Return CType(GetValue(QueryPaneProperty), PaneVisibilityState)
            End Get
            Set
                SetValue(QueryPaneProperty, Value)
            End Set
        End Property

        Friend Shared QueryPaneProperty As DependencyProperty = DependencyProperty.Register("QueryPane", GetType(PaneVisibilityState), GetType(ExplorerBrowser), New PropertyMetadata(PaneVisibilityState.DoNotCare, AddressOf OnQueryPaneChanged))

        Private Shared Sub OnQueryPaneChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
            Dim instance As ExplorerBrowser = TryCast(d, ExplorerBrowser)
            If instance.ExplorerBrowserControl IsNot Nothing Then
                instance.ExplorerBrowserControl.NavigationOptions.PaneVisibility.Query = CType(e.NewValue, PaneVisibilityState)
            End If
        End Sub


        ''' <summary>
        ''' Navigation log index
        ''' </summary>
        Public Property NavigationLogIndex() As Integer
            Get
                Return CInt(GetValue(NavigationLogIndexProperty))
            End Get
            Set
                SetValue(NavigationLogIndexProperty, Value)
            End Set
        End Property

        Friend Shared NavigationLogIndexProperty As DependencyProperty = DependencyProperty.Register("NavigationLogIndex", GetType(Integer), GetType(ExplorerBrowser), New PropertyMetadata(0, AddressOf OnNavigationLogIndexChanged))

        Private Shared Sub OnNavigationLogIndexChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
            Dim instance As ExplorerBrowser = TryCast(d, ExplorerBrowser)
            If instance.ExplorerBrowserControl IsNot Nothing Then
                instance.ExplorerBrowserControl.NavigationLog.NavigateLog(CInt(e.NewValue))
            End If
        End Sub


#Region "IDisposable Members"

        ''' <summary>
        ''' Disposes the class
        ''' </summary>        
        Public Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub

        ''' <summary>
        ''' Disposes the browser.
        ''' </summary>
        ''' <param name="disposed"></param>
        Protected Overridable Sub Dispose(disposed As Boolean)
            If disposed Then
                If _itemsChanged IsNot Nothing Then
                    _itemsChanged.Close()
                End If

                If _selectionChanged IsNot Nothing Then
                    _selectionChanged.Close()
                End If
            End If
        End Sub

#End Region
    End Class
End Namespace
