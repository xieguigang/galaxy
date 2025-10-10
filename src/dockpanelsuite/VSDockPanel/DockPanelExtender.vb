Imports System.Drawing
Imports System.Diagnostics.CodeAnalysis
Imports System.Windows.Forms
Imports WeifenLuo.WinFormsUI.Docking.DockPanel
Imports WeifenLuo.WinFormsUI.Docking.DockPanel.DockDragHandler

Namespace WeifenLuo.WinFormsUI.Docking
    Public NotInheritable Class DockPanelExtender
        <SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")>
        Public Interface IDockPaneFactory
            Function CreateDockPane(content As IDockContent, visibleState As DockState, show As Boolean) As DockPane

            <SuppressMessage("Microsoft.Naming", "CA1720:AvoidTypeNamesInParameters", MessageId:="1#")>
            Function CreateDockPane(content As IDockContent, floatWindow As FloatWindow, show As Boolean) As DockPane

            Function CreateDockPane(content As IDockContent, previousPane As DockPane, alignment As DockAlignment, proportion As Double, show As Boolean) As DockPane

            <SuppressMessage("Microsoft.Naming", "CA1720:AvoidTypeNamesInParameters", MessageId:="1#")>
            Function CreateDockPane(content As IDockContent, floatWindowBounds As Rectangle, show As Boolean) As DockPane
        End Interface

        Public Interface IDockPaneSplitterControlFactory
            Function CreateSplitterControl(pane As DockPane) As DockPane.SplitterControlBase
        End Interface

        Public Interface IWindowSplitterControlFactory
            Function CreateSplitterControl(host As ISplitterHost) As SplitterBase
        End Interface

        <SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")>
        Public Interface IFloatWindowFactory
            Function CreateFloatWindow(dockPanel As DockPanel, pane As DockPane) As FloatWindow
            Function CreateFloatWindow(dockPanel As DockPanel, pane As DockPane, bounds As Rectangle) As FloatWindow
        End Interface

        Public Interface IDockWindowFactory
            Function CreateDockWindow(dockPanel As DockPanel, dockState As DockState) As DockWindow
        End Interface

        <SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")>
        Public Interface IDockPaneCaptionFactory
            Function CreateDockPaneCaption(pane As DockPane) As DockPaneCaptionBase
        End Interface

        <SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")>
        Public Interface IDockPaneStripFactory
            Function CreateDockPaneStrip(pane As DockPane) As DockPaneStripBase
        End Interface

        <SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")>
        Public Interface IAutoHideStripFactory
            Function CreateAutoHideStrip(panel As DockPanel) As AutoHideStripBase
        End Interface

        Public Interface IAutoHideWindowFactory
            Function CreateAutoHideWindow(panel As DockPanel) As AutoHideWindowControl
        End Interface

        Public Interface IPaneIndicatorFactory
            Function CreatePaneIndicator(theme As ThemeBase) As IPaneIndicator
        End Interface

        Public Interface IPanelIndicatorFactory
            Function CreatePanelIndicator(style As DockStyle, theme As ThemeBase) As IPanelIndicator
        End Interface

        Public Interface IDockOutlineFactory
            Function CreateDockOutline() As DockOutlineBase
        End Interface

        Public Interface IDockIndicatorFactory
            Function CreateDockIndicator(dockDragHandler As DockDragHandler) As DockIndicator
        End Interface

#Region "DefaultDockPaneFactory"

        Private Class DefaultDockPaneFactory
            Implements IDockPaneFactory
            Public Function CreateDockPane(content As IDockContent, visibleState As DockState, show As Boolean) As DockPane Implements IDockPaneFactory.CreateDockPane
                Return New DockPane(content, visibleState, show)
            End Function

            Public Function CreateDockPane(content As IDockContent, floatWindow As FloatWindow, show As Boolean) As DockPane Implements IDockPaneFactory.CreateDockPane
                Return New DockPane(content, floatWindow, show)
            End Function

            Public Function CreateDockPane(content As IDockContent, prevPane As DockPane, alignment As DockAlignment, proportion As Double, show As Boolean) As DockPane Implements IDockPaneFactory.CreateDockPane
                Return New DockPane(content, prevPane, alignment, proportion, show)
            End Function

            Public Function CreateDockPane(content As IDockContent, floatWindowBounds As Rectangle, show As Boolean) As DockPane Implements IDockPaneFactory.CreateDockPane
                Return New DockPane(content, floatWindowBounds, show)
            End Function
        End Class

#End Region

#Region "DefaultFloatWindowFactory"

        Private Class DefaultFloatWindowFactory
            Implements IFloatWindowFactory
            Public Function CreateFloatWindow(dockPanel As DockPanel, pane As DockPane) As FloatWindow Implements IFloatWindowFactory.CreateFloatWindow
                Return New FloatWindow(dockPanel, pane)
            End Function

            Public Function CreateFloatWindow(dockPanel As DockPanel, pane As DockPane, bounds As Rectangle) As FloatWindow Implements IFloatWindowFactory.CreateFloatWindow
                Return New FloatWindow(dockPanel, pane, bounds)
            End Function
        End Class

#End Region

        Private m_dockPaneFactory As IDockPaneFactory = Nothing

        Public Property DockPaneFactory As IDockPaneFactory
            Get
                If m_dockPaneFactory Is Nothing Then
                    m_dockPaneFactory = New DefaultDockPaneFactory()
                End If

                Return m_dockPaneFactory
            End Get
            Set(value As IDockPaneFactory)
                m_dockPaneFactory = value
            End Set
        End Property

        Public Property DockPaneSplitterControlFactory As IDockPaneSplitterControlFactory

        Public Property WindowSplitterControlFactory As IWindowSplitterControlFactory

        Private m_floatWindowFactory As IFloatWindowFactory = Nothing

        Public Property FloatWindowFactory As IFloatWindowFactory
            Get
                If m_floatWindowFactory Is Nothing Then
                    m_floatWindowFactory = New DefaultFloatWindowFactory()
                End If

                Return m_floatWindowFactory
            End Get
            Set(value As IFloatWindowFactory)
                m_floatWindowFactory = value
            End Set
        End Property

        Public Property DockWindowFactory As IDockWindowFactory

        Public Property DockPaneCaptionFactory As IDockPaneCaptionFactory

        Public Property DockPaneStripFactory As IDockPaneStripFactory

        Private m_autoHideStripFactory As IAutoHideStripFactory = Nothing

        Public Property AutoHideStripFactory As IAutoHideStripFactory
            Get
                Return m_autoHideStripFactory
            End Get
            Set(value As IAutoHideStripFactory)
                If m_autoHideStripFactory Is value Then
                    Return
                End If

                m_autoHideStripFactory = value
            End Set
        End Property

        Private m_autoHideWindowFactory As IAutoHideWindowFactory

        Public Property AutoHideWindowFactory As IAutoHideWindowFactory
            Get
                Return m_autoHideWindowFactory
            End Get
            Set(value As IAutoHideWindowFactory)
                If m_autoHideWindowFactory Is value Then
                    Return
                End If

                m_autoHideWindowFactory = value
            End Set
        End Property

        Public Property PaneIndicatorFactory As IPaneIndicatorFactory

        Public Property PanelIndicatorFactory As IPanelIndicatorFactory

        Public Property DockOutlineFactory As IDockOutlineFactory

        Public Property DockIndicatorFactory As IDockIndicatorFactory
    End Class
End Namespace
