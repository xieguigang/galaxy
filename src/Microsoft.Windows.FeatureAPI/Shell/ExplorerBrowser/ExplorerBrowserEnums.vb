'Copyright (c) Microsoft Corporation.  All rights reserved.


Namespace Controls
	''' <summary>
	''' Indicates the viewing mode of the explorer browser
	''' </summary>
	Public Enum ExplorerBrowserViewMode
		''' <summary>
		''' Choose the best view mode for the folder
		''' </summary>
		Auto = -1

		''' <summary>
		''' (New for Windows7)
		''' </summary>
		Content = 8

		''' <summary>
		''' Object names and other selected information, such as the size or date last updated, are shown.
		''' </summary>
		Details = 4

		''' <summary>
		''' The view should display medium-size icons.
		''' </summary>
		Icon = 1

		''' <summary>
		''' Object names are displayed in a list view.
		''' </summary>
		List = 3

		''' <summary>
		''' The view should display small icons. 
		''' </summary>
		SmallIcon = 2

		''' <summary>
		''' The view should display thumbnail icons. 
		''' </summary>
		Thumbnail = 5

		''' <summary>
		''' The view should display icons in a filmstrip format.
		''' </summary>
		ThumbStrip = 7

		''' <summary>
		''' The view should display large icons. 
		''' </summary>
		Tile = 6
	End Enum

	''' <summary>
	''' Specifies the options that control subsequent navigation.
	''' Typically use one, or a bitwise combination of these
	''' flags to specify how the explorer browser navigates.
	''' </summary>
	<Flags> _
	Public Enum ExplorerBrowserNavigateOptions
		''' <summary>
		''' Always navigate, even if you are attempting to navigate to the current folder.
		''' </summary>
		AlwaysNavigate = &H4

		''' <summary>
		''' Do not navigate further than the initial navigation.
		''' </summary>
		NavigateOnce = &H1
	End Enum

	''' <summary>
	''' Indicates the content options of the explorer browser.
	''' Typically use one, or a bitwise combination of these
	''' flags to specify how conent should appear in the
	''' explorer browser control
	''' </summary>
	<Flags> _
	Public Enum ExplorerBrowserContentSectionOptions
		''' <summary>
		''' No options.
		''' </summary>
		None = 0
		''' <summary>
		''' The view should be left-aligned. 
		''' </summary>
		AlignLeft = &H800
		''' <summary>
		''' Automatically arrange the elements in the view. 
		''' </summary>
		AutoArrange = &H1
		''' <summary>
		''' Turns on check mode for the view
		''' </summary>
		CheckSelect = &H8040000
		''' <summary>
		''' When the view is set to "Tile" the layout of a single item should be extended to the width of the view.
		''' </summary>
		ExtendedTiles = &H2000000
		''' <summary>
		''' When an item is selected, the item and all its sub-items are highlighted.
		''' </summary>
		FullRowSelect = &H200000
		''' <summary>
		''' The view should not display file names
		''' </summary>
		HideFileNames = &H20000
		''' <summary>
		''' The view should not save view state in the browser.
		''' </summary>
		NoBrowserViewState = &H10000000
		''' <summary>
		''' Do not display a column header in the view in any view mode.
		''' </summary>
		NoColumnHeader = &H800000
		''' <summary>
		''' Only show the column header in details view mode.
		''' </summary>
		NoHeaderInAllViews = &H1000000
		''' <summary>
		''' The view should not display icons. 
		''' </summary>
		NoIcons = &H1000
		''' <summary>
		''' Do not show subfolders. 
		''' </summary>
		NoSubfolders = &H80
		''' <summary>
		''' Navigate with a single click
		''' </summary>
		SingleClickActivate = &H8000
		''' <summary>
		''' Do not allow more than a single item to be selected.
		''' </summary>
		SingleSelection = &H40
	End Enum

	''' <summary>
	''' Indicates the visibility state of an ExplorerBrowser pane
	''' </summary>
	Public Enum PaneVisibilityState
		''' <summary>
		''' Allow the explorer browser to determine if this pane is displayed.
		''' </summary>
		DoNotCare
		''' <summary>
		''' Hide the pane
		''' </summary>
		Hide
		''' <summary>
		''' Show the pane
		''' </summary>
		Show
	End Enum
End Namespace
