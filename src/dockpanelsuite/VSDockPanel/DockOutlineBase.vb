Imports System.Drawing
Imports System.Windows.Forms

Namespace Docking
    Public MustInherit Class DockOutlineBase
        Public Sub New()
            Init()
        End Sub

        Private Sub Init()
            SetValues(Rectangle.Empty, Nothing, DockStyle.None, -1)
            SaveOldValues()
        End Sub

        Private m_oldFloatWindowBounds As Rectangle
        Protected ReadOnly Property OldFloatWindowBounds As Rectangle
            Get
                Return m_oldFloatWindowBounds
            End Get
        End Property

        Private m_oldDockTo As Control
        Protected ReadOnly Property OldDockTo As Control
            Get
                Return m_oldDockTo
            End Get
        End Property

        Private m_oldDock As DockStyle
        Protected ReadOnly Property OldDock As DockStyle
            Get
                Return m_oldDock
            End Get
        End Property

        Private m_oldContentIndex As Integer
        Protected ReadOnly Property OldContentIndex As Integer
            Get
                Return m_oldContentIndex
            End Get
        End Property

        Protected ReadOnly Property SameAsOldValue As Boolean
            Get
                Return FloatWindowBounds = OldFloatWindowBounds AndAlso DockTo Is OldDockTo AndAlso Dock = OldDock AndAlso ContentIndex = OldContentIndex
            End Get
        End Property

        Private m_floatWindowBounds As Rectangle
        Public ReadOnly Property FloatWindowBounds As Rectangle
            Get
                Return m_floatWindowBounds
            End Get
        End Property

        Private m_dockTo As Control
        Public ReadOnly Property DockTo As Control
            Get
                Return m_dockTo
            End Get
        End Property

        Private m_dock As DockStyle
        Public ReadOnly Property Dock As DockStyle
            Get
                Return m_dock
            End Get
        End Property

        Private m_contentIndex As Integer
        Public ReadOnly Property ContentIndex As Integer
            Get
                Return m_contentIndex
            End Get
        End Property

        Public ReadOnly Property FlagFullEdge As Boolean
            Get
                Return m_contentIndex <> 0
            End Get
        End Property

        Private m_flagTestDrop As Boolean = False
        Public Property FlagTestDrop As Boolean
            Get
                Return m_flagTestDrop
            End Get
            Set(value As Boolean)
                m_flagTestDrop = value
            End Set
        End Property

        Private Sub SaveOldValues()
            m_oldDockTo = m_dockTo
            m_oldDock = m_dock
            m_oldContentIndex = m_contentIndex
            m_oldFloatWindowBounds = m_floatWindowBounds
        End Sub

        Protected MustOverride Sub OnShow()

        Protected MustOverride Sub OnClose()

        Private Sub SetValues(floatWindowBounds As Rectangle, dockTo As Control, dock As DockStyle, contentIndex As Integer)
            m_floatWindowBounds = floatWindowBounds
            m_dockTo = dockTo
            m_dock = dock
            m_contentIndex = contentIndex
            FlagTestDrop = True
        End Sub

        Private Sub TestChange()
            If m_floatWindowBounds <> m_oldFloatWindowBounds OrElse m_dockTo IsNot m_oldDockTo OrElse m_dock <> m_oldDock OrElse m_contentIndex <> m_oldContentIndex Then OnShow()
        End Sub

        Public Sub Show()
            SaveOldValues()
            SetValues(Rectangle.Empty, Nothing, DockStyle.None, -1)
            TestChange()
        End Sub

        Public Sub Show(pane As DockPane, dock As DockStyle)
            SaveOldValues()
            SetValues(Rectangle.Empty, pane, dock, -1)
            TestChange()
        End Sub

        Public Sub Show(pane As DockPane, contentIndex As Integer)
            SaveOldValues()
            SetValues(Rectangle.Empty, pane, DockStyle.Fill, contentIndex)
            TestChange()
        End Sub

        Public Sub Show(dockPanel As DockPanel, dock As DockStyle, fullPanelEdge As Boolean)
            SaveOldValues()
            SetValues(Rectangle.Empty, dockPanel, dock, If(fullPanelEdge, -1, 0))
            TestChange()
        End Sub

        Public Sub Show(floatWindowBounds As Rectangle)
            SaveOldValues()
            SetValues(floatWindowBounds, Nothing, DockStyle.None, -1)
            TestChange()
        End Sub

        Public Sub Close()
            OnClose()
        End Sub
    End Class
End Namespace
