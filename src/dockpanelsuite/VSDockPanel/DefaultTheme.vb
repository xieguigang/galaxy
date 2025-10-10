Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms

Namespace WeifenLuo.WinFormsUI.Docking
    Friend Class DefaultTheme
        Inherits ThemeBase
        Friend Sub New()
            Extender.AutoHideStripFactory = New DefaultAutoHideStripFactory()
            Extender.AutoHideWindowFactory = New DefaultAutoHideWindowFactory()
            Extender.WindowSplitterControlFactory = New DefaultWindowSplitterControlFactory()
            Extender.DockWindowFactory = New DefaultDockWindowFactory()
            Extender.DockPaneSplitterControlFactory = New DefaultDockPaneSplitterControlFactory()
            Extender.DockPaneCaptionFactory = New DefaultDockPaneCaptionFactory()
            Extender.DockPaneStripFactory = New DefaultDockPaneStripFactory()
            Extender.DockIndicatorFactory = New DefaultDockIndicatorFactory()
            Extender.DockOutlineFactory = New DefaultDockOutlineFactory()
            Extender.PaneIndicatorFactory = New DefaultPaneIndicatorFactory()
            Extender.PanelIndicatorFactory = New DefaultPanelIndicatorFactory()
        End Sub

        Private Class DefaultAutoHideStripFactory
            Implements DockPanelExtender.IAutoHideStripFactory
            Public Function CreateAutoHideStrip(panel As DockPanel) As AutoHideStripBase Implements DockPanelExtender.IAutoHideStripFactory.CreateAutoHideStrip
                Return New DefaultAutoHideStripBase(panel)
            End Function

            Friend Class DefaultAutoHideStripBase
                Inherits AutoHideStripBase
                Public Sub New(panel As DockPanel)
                    MyBase.New(panel)
                End Sub

                Protected Overrides Function GetTabBounds(tab As Tab) As Rectangle
                    Return Rectangle.Empty
                End Function

                Protected Overrides Function HitTest(point As Point) As IDockContent
                    Return Nothing
                End Function

                Protected Friend Overrides Function MeasureHeight() As Integer
                    Return 0
                End Function
            End Class

        End Class

        Private Class DefaultAutoHideWindowFactory
            Implements DockPanelExtender.IAutoHideWindowFactory
            Public Function CreateAutoHideWindow(panel As DockPanel) As DockPanel.AutoHideWindowControl Implements DockPanelExtender.IAutoHideWindowFactory.CreateAutoHideWindow
                Return New DefaultAutoHideWindowControl(panel)
            End Function

            Private Class DefaultAutoHideWindowControl
                Inherits DockPanel.AutoHideWindowControl
                Public Sub New(dockPanel As DockPanel)
                    MyBase.New(dockPanel)
                End Sub
            End Class
        End Class

        Private Class DefaultWindowSplitterControlFactory
            Implements DockPanelExtender.IWindowSplitterControlFactory
            Public Function CreateSplitterControl(host As ISplitterHost) As SplitterBase Implements DockPanelExtender.IWindowSplitterControlFactory.CreateSplitterControl
                Return New SplitterBase()
            End Function
        End Class

        Private Class DefaultDockWindowFactory
            Implements DockPanelExtender.IDockWindowFactory
            Public Function CreateDockWindow(dockPanel As DockPanel, dockState As DockState) As DockWindow Implements DockPanelExtender.IDockWindowFactory.CreateDockWindow
                Return New DockWindow(dockPanel, dockState)
            End Function
        End Class

        Private Class DefaultDockPaneSplitterControlFactory
            Implements DockPanelExtender.IDockPaneSplitterControlFactory
            Public Function CreateSplitterControl(pane As DockPane) As DockPane.SplitterControlBase Implements DockPanelExtender.IDockPaneSplitterControlFactory.CreateSplitterControl
                Return New DockPane.SplitterControlBase(pane)
            End Function
        End Class

        Private Class DefaultDockPaneCaptionFactory
            Implements DockPanelExtender.IDockPaneCaptionFactory
            Public Function CreateDockPaneCaption(pane As DockPane) As DockPaneCaptionBase Implements DockPanelExtender.IDockPaneCaptionFactory.CreateDockPaneCaption
                Return New DefaultDockPaneCaption(pane)
            End Function

            Private Class DefaultDockPaneCaption
                Inherits DockPaneCaptionBase
                Public Sub New(pane As DockPane)
                    MyBase.New(pane)
                End Sub

                Protected Friend Overrides Function MeasureHeight() As Integer
                    Return 0
                End Function
            End Class
        End Class

        Private Class DefaultDockPaneStripFactory
            Implements DockPanelExtender.IDockPaneStripFactory
            Public Function CreateDockPaneStrip(pane As DockPane) As DockPaneStripBase Implements DockPanelExtender.IDockPaneStripFactory.CreateDockPaneStrip
                Return New DefaultDockPaneStrip(pane)
            End Function

            Private Class DefaultDockPaneStrip
                Inherits DockPaneStripBase
                Public Sub New(pane As DockPane)
                    MyBase.New(pane)
                End Sub

                Public Overrides Function GetOutline(index As Integer) As GraphicsPath
                    Return New GraphicsPath()
                End Function

                Protected Overrides Function GetTabBounds(tab As Tab) As Rectangle
                    Return Rectangle.Empty
                End Function

                Protected Friend Overrides Sub EnsureTabVisible(content As IDockContent)
                End Sub

                Protected Friend Overrides Function HitTest(point As Point) As Integer
                    Return -1
                End Function

                Protected Friend Overrides Function MeasureHeight() As Integer
                    Return 0
                End Function
            End Class
        End Class

        Private Class DefaultDockIndicatorFactory
            Implements DockPanelExtender.IDockIndicatorFactory
            Public Function CreateDockIndicator(dockDragHandler As DockPanel.DockDragHandler) As DockPanel.DockDragHandler.DockIndicator Implements DockPanelExtender.IDockIndicatorFactory.CreateDockIndicator
                Return New DockPanel.DockDragHandler.DockIndicator(dockDragHandler)
            End Function
        End Class

        Private Class DefaultDockOutlineFactory
            Implements DockPanelExtender.IDockOutlineFactory
            Public Function CreateDockOutline() As DockOutlineBase Implements DockPanelExtender.IDockOutlineFactory.CreateDockOutline
                Return New DefaultDockOutline()
            End Function

            Private Class DefaultDockOutline
                Inherits DockOutlineBase
                Protected Overrides Sub OnClose()
                End Sub

                Protected Overrides Sub OnShow()
                End Sub
            End Class
        End Class

        Private Class DefaultPaneIndicatorFactory
            Implements DockPanelExtender.IPaneIndicatorFactory
            Public Function CreatePaneIndicator(theme As ThemeBase) As DockPanel.IPaneIndicator Implements DockPanelExtender.IPaneIndicatorFactory.CreatePaneIndicator
                Return New DefaultPaneIndicator(theme)
            End Function

            Private Class DefaultPaneIndicator
                Implements DockPanel.IPaneIndicator
                Private theme As ThemeBase

                Public Sub New(theme As ThemeBase)
                    Me.theme = theme
                End Sub

                Public Property Location As Point Implements DockPanel.IPaneIndicator.Location
                    Get
                        Throw New NotImplementedException()
                    End Get
                    Set(value As Point)
                        Throw New NotImplementedException()
                    End Set
                End Property
                Public Property Visible As Boolean Implements DockPanel.IPaneIndicator.Visible
                    Get
                        Throw New NotImplementedException()
                    End Get
                    Set(value As Boolean)
                        Throw New NotImplementedException()
                    End Set
                End Property

                Public ReadOnly Property Left As Integer Implements DockPanel.IPaneIndicator.Left
                    Get
                        Throw New NotImplementedException()
                    End Get
                End Property

                Public ReadOnly Property Top As Integer Implements DockPanel.IPaneIndicator.Top
                    Get
                        Throw New NotImplementedException()
                    End Get
                End Property

                Public ReadOnly Property Right As Integer Implements DockPanel.IPaneIndicator.Right
                    Get
                        Throw New NotImplementedException()
                    End Get
                End Property

                Public ReadOnly Property Bottom As Integer Implements DockPanel.IPaneIndicator.Bottom
                    Get
                        Throw New NotImplementedException()
                    End Get
                End Property

                Public ReadOnly Property ClientRectangle As Rectangle Implements DockPanel.IPaneIndicator.ClientRectangle
                    Get
                        Throw New NotImplementedException()
                    End Get
                End Property

                Public ReadOnly Property Width As Integer Implements DockPanel.IPaneIndicator.Width
                    Get
                        Throw New NotImplementedException()
                    End Get
                End Property

                Public ReadOnly Property Height As Integer Implements DockPanel.IPaneIndicator.Height
                    Get
                        Throw New NotImplementedException()
                    End Get
                End Property

                Public ReadOnly Property DisplayingGraphicsPath As GraphicsPath Implements DockPanel.IPaneIndicator.DisplayingGraphicsPath
                    Get
                        Throw New NotImplementedException()
                    End Get
                End Property

                Public Property Status As DockStyle Implements DockPanel.IHitTest.Status
                    Get
                        Throw New NotImplementedException()
                    End Get
                    Set(value As DockStyle)
                        Throw New NotImplementedException()
                    End Set
                End Property

                Public Function HitTest(pt As Point) As DockStyle Implements DockPanel.IHitTest.HitTest
                    Throw New NotImplementedException()
                End Function
            End Class
        End Class

        Private Class DefaultPanelIndicatorFactory
            Implements DockPanelExtender.IPanelIndicatorFactory
            Public Function CreatePanelIndicator(style As DockStyle, theme As ThemeBase) As DockPanel.IPanelIndicator Implements DockPanelExtender.IPanelIndicatorFactory.CreatePanelIndicator
                Return New DefaultPanelIndicator(style, theme)
            End Function

            Private Class DefaultPanelIndicator
                Implements DockPanel.IPanelIndicator
                Private style As DockStyle
                Private theme As ThemeBase

                Public Sub New(style As DockStyle, theme As ThemeBase)
                    Me.style = style
                    Me.theme = theme
                End Sub

                Public Property Location As Point Implements DockPanel.IPanelIndicator.Location
                    Get
                        Throw New NotImplementedException()
                    End Get
                    Set(value As Point)
                        Throw New NotImplementedException()
                    End Set
                End Property
                Public Property Visible As Boolean Implements DockPanel.IPanelIndicator.Visible
                    Get
                        Throw New NotImplementedException()
                    End Get
                    Set(value As Boolean)
                        Throw New NotImplementedException()
                    End Set
                End Property

                Public ReadOnly Property Bounds As Rectangle Implements DockPanel.IPanelIndicator.Bounds
                    Get
                        Throw New NotImplementedException()
                    End Get
                End Property

                Public ReadOnly Property Width As Integer Implements DockPanel.IPanelIndicator.Width
                    Get
                        Throw New NotImplementedException()
                    End Get
                End Property

                Public ReadOnly Property Height As Integer Implements DockPanel.IPanelIndicator.Height
                    Get
                        Throw New NotImplementedException()
                    End Get
                End Property

                Public Property Status As DockStyle Implements DockPanel.IHitTest.Status
                    Get
                        Throw New NotImplementedException()
                    End Get
                    Set(value As DockStyle)
                        Throw New NotImplementedException()
                    End Set
                End Property

                Public Function HitTest(pt As Point) As DockStyle Implements DockPanel.IHitTest.HitTest
                    Throw New NotImplementedException()
                End Function
            End Class
        End Class
    End Class
End Namespace
