Imports System.Drawing

Namespace Docking
    Public NotInheritable Class NestedDockingStatus
        Friend Sub New(pane As DockPane)
            m_dockPane = pane
        End Sub

        Private m_dockPane As DockPane = Nothing
        Public ReadOnly Property DockPane As DockPane
            Get
                Return m_dockPane
            End Get
        End Property

        Private m_nestedPanes As NestedPaneCollection = Nothing
        Public ReadOnly Property NestedPanes As NestedPaneCollection
            Get
                Return m_nestedPanes
            End Get
        End Property

        Private m_previousPane As DockPane = Nothing
        Public ReadOnly Property PreviousPane As DockPane
            Get
                Return m_previousPane
            End Get
        End Property

        Private m_alignment As DockAlignment = DockAlignment.Left
        Public ReadOnly Property Alignment As DockAlignment
            Get
                Return m_alignment
            End Get
        End Property

        Private m_proportion As Double = 0.5
        Public ReadOnly Property Proportion As Double
            Get
                Return m_proportion
            End Get
        End Property

        Private m_isDisplaying As Boolean = False
        Public ReadOnly Property IsDisplaying As Boolean
            Get
                Return m_isDisplaying
            End Get
        End Property

        Private m_displayingPreviousPane As DockPane = Nothing
        Public ReadOnly Property DisplayingPreviousPane As DockPane
            Get
                Return m_displayingPreviousPane
            End Get
        End Property

        Private m_displayingAlignment As DockAlignment = DockAlignment.Left
        Public ReadOnly Property DisplayingAlignment As DockAlignment
            Get
                Return m_displayingAlignment
            End Get
        End Property

        Private m_displayingProportion As Double = 0.5
        Public ReadOnly Property DisplayingProportion As Double
            Get
                Return m_displayingProportion
            End Get
        End Property

        Private m_logicalBounds As Rectangle = Rectangle.Empty
        Public ReadOnly Property LogicalBounds As Rectangle
            Get
                Return m_logicalBounds
            End Get
        End Property

        Private m_paneBounds As Rectangle = Rectangle.Empty
        Public ReadOnly Property PaneBounds As Rectangle
            Get
                Return m_paneBounds
            End Get
        End Property

        Private m_splitterBounds As Rectangle = Rectangle.Empty
        Public ReadOnly Property SplitterBounds As Rectangle
            Get
                Return m_splitterBounds
            End Get
        End Property

        Friend Sub SetStatus(nestedPanes As NestedPaneCollection, previousPane As DockPane, alignment As DockAlignment, proportion As Double)
            m_nestedPanes = nestedPanes
            m_previousPane = previousPane
            m_alignment = alignment
            m_proportion = proportion
        End Sub

        Friend Sub SetDisplayingStatus(isDisplaying As Boolean, displayingPreviousPane As DockPane, displayingAlignment As DockAlignment, displayingProportion As Double)
            m_isDisplaying = isDisplaying
            m_displayingPreviousPane = displayingPreviousPane
            m_displayingAlignment = displayingAlignment
            m_displayingProportion = displayingProportion
        End Sub

        Friend Sub SetDisplayingBounds(logicalBounds As Rectangle, paneBounds As Rectangle, splitterBounds As Rectangle)
            m_logicalBounds = logicalBounds
            m_paneBounds = paneBounds
            m_splitterBounds = splitterBounds
        End Sub
    End Class
End Namespace
