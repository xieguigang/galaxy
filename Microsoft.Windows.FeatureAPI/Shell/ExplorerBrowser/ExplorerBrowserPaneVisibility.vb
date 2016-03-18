'Copyright (c) Microsoft Corporation.  All rights reserved.

Namespace Controls
	''' <summary>
	''' Controls the visibility of the various ExplorerBrowser panes on subsequent navigation
	''' </summary>
	Public Class ExplorerBrowserPaneVisibility
		''' <summary>
		''' The pane on the left side of the Windows Explorer window that hosts the folders tree and Favorites.
		''' </summary>        
		Public Property Navigation() As PaneVisibilityState
			Get
				Return m_Navigation
			End Get
			Set
				m_Navigation = Value
			End Set
		End Property
		Private m_Navigation As PaneVisibilityState


		''' <summary>
		''' Commands module along the top of the Windows Explorer window.
		''' </summary>        
		Public Property Commands() As PaneVisibilityState
			Get
				Return m_Commands
			End Get
			Set
				m_Commands = Value
			End Set
		End Property
		Private m_Commands As PaneVisibilityState

		''' <summary>
		''' Organize menu within the commands module.
		''' </summary>
		Public Property CommandsOrganize() As PaneVisibilityState
			Get
				Return m_CommandsOrganize
			End Get
			Set
				m_CommandsOrganize = Value
			End Set
		End Property
		Private m_CommandsOrganize As PaneVisibilityState


		''' <summary>
		''' View menu within the commands module.
		''' </summary>
		Public Property CommandsView() As PaneVisibilityState
			Get
				Return m_CommandsView
			End Get
			Set
				m_CommandsView = Value
			End Set
		End Property
		Private m_CommandsView As PaneVisibilityState


		''' <summary>
		''' Pane showing metadata along the bottom of the Windows Explorer window.
		''' </summary>
		Public Property Details() As PaneVisibilityState
			Get
				Return m_Details
			End Get
			Set
				m_Details = Value
			End Set
		End Property
		Private m_Details As PaneVisibilityState


		''' <summary>
		''' Pane on the right of the Windows Explorer window that shows a large reading preview of the file.
		''' </summary>
		Public Property Preview() As PaneVisibilityState
			Get
				Return m_Preview
			End Get
			Set
				m_Preview = Value
			End Set
		End Property
		Private m_Preview As PaneVisibilityState


		''' <summary>
		''' Quick filter buttons to aid in a search.
		''' </summary>
		Public Property Query() As PaneVisibilityState
			Get
				Return m_Query
			End Get
			Set
				m_Query = Value
			End Set
		End Property
		Private m_Query As PaneVisibilityState


		''' <summary>
		''' Additional fields and options to aid in a search.
		''' </summary>
		Public Property AdvancedQuery() As PaneVisibilityState
			Get
				Return m_AdvancedQuery
			End Get
			Set
				m_AdvancedQuery = Value
			End Set
		End Property
		Private m_AdvancedQuery As PaneVisibilityState

	End Class
End Namespace
