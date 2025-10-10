Imports System
Imports System.Drawing
Imports System.Windows.Forms

Namespace WeifenLuo.WinFormsUI.Docking
    Public Interface IDockContent
        Inherits IContextMenuStripHost
        ReadOnly Property DockHandler As DockContentHandler
        Sub OnActivated(e As EventArgs)
        Sub OnDeactivate(e As EventArgs)
    End Interface

    Public Interface IContextMenuStripHost
        Sub ApplyTheme()
    End Interface

    Public Interface INestedPanesContainer
        ReadOnly Property DockState As DockState
        ReadOnly Property DisplayingRectangle As Rectangle
        ReadOnly Property NestedPanes As NestedPaneCollection
        ReadOnly Property VisibleNestedPanes As VisibleNestedPaneCollection
        ReadOnly Property IsFloat As Boolean
    End Interface

    Public Interface IDragSource
        ReadOnly Property DragControl As Control
    End Interface

    Public Interface IDockDragSource
        Inherits IDragSource
        Function BeginDrag(ptMouse As Point) As Rectangle
        Sub EndDrag()
        Function IsDockStateValid(dockState As DockState) As Boolean
        Function CanDockTo(pane As DockPane) As Boolean
        Sub FloatAt(floatWindowBounds As Rectangle)
        Sub DockTo(pane As DockPane, dockStyle As DockStyle, contentIndex As Integer)
        Sub DockTo(panel As DockPanel, dockStyle As DockStyle)
    End Interface

    Public Interface ISplitterDragSource
        Inherits IDragSource
        Sub BeginDrag(rectSplitter As Rectangle)
        Sub EndDrag()
        ReadOnly Property IsVertical As Boolean
        ReadOnly Property DragLimitBounds As Rectangle
        Sub MoveSplitter(offset As Integer)
    End Interface

    Public Interface ISplitterHost
        Inherits ISplitterDragSource
        ReadOnly Property DockPanel As DockPanel
        ReadOnly Property DockState As DockState
        ReadOnly Property IsDockWindow As Boolean
    End Interface
End Namespace
