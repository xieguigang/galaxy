Imports System.Windows.Forms
Imports System.Drawing

Namespace WeifenLuo.WinFormsUI.Docking
    Partial Class DockPanel
        ''' <summary>
        ''' DragHandlerBase is the base class for drag handlers. The derived class should:
        '''   1. Define its public method BeginDrag. From within this public BeginDrag method,
        '''      DragHandlerBase.BeginDrag should be called to initialize the mouse capture
        '''      and message filtering.
        '''   2. Override the OnDragging and OnEndDrag methods.
        ''' </summary>
        Public MustInherit Class DragHandlerBase
            Inherits NativeWindow
            Implements IMessageFilter
            Protected Sub New()
            End Sub

            Protected MustOverride ReadOnly Property DragControl As Control

            Private m_startMousePosition As Point = Point.Empty
            Protected Property StartMousePosition As Point
                Get
                    Return m_startMousePosition
                End Get
                Private Set(value As Point)
                    m_startMousePosition = value
                End Set
            End Property

            Protected Function BeginDrag() As Boolean
                If DragControl Is Nothing Then Return False

                StartMousePosition = MousePosition

                If Not IsRunningOnMono Then
                    If Not DragDetect(DragControl.Handle, StartMousePosition) Then
                        Return False
                    End If
                End If

                DragControl.FindForm().Capture = True
                AssignHandle(DragControl.FindForm().Handle)
                If EnableActiveXFix = False Then
                    Application.AddMessageFilter(Me)
                End If

                Return True
            End Function

            Protected MustOverride Sub OnDragging()

            Protected MustOverride Sub OnEndDrag(abort As Boolean)

            Private Sub EndDrag(abort As Boolean)
                MyBase.ReleaseHandle()

                If EnableActiveXFix = False Then
                    Application.RemoveMessageFilter(Me)
                End If

                DragControl.FindForm().Capture = False

                OnEndDrag(abort)
            End Sub

            Private Function PreFilterMessage(ByRef m As Message) As Boolean Implements IMessageFilter.PreFilterMessage
                If EnableActiveXFix = False Then
                    If m.Msg = Win32.Msgs.WM_MOUSEMOVE Then
                        OnDragging()
                    ElseIf m.Msg = Win32.Msgs.WM_LBUTTONUP Then
                        EndDrag(False)
                    ElseIf m.Msg = Win32.Msgs.WM_CAPTURECHANGED Then
                        EndDrag(Not IsRunningOnMono)
                    ElseIf m.Msg = Win32.Msgs.WM_KEYDOWN AndAlso CInt(m.WParam) = Keys.Escape Then
                        EndDrag(True)
                    End If
                End If

                Return OnPreFilterMessage(m)
            End Function

            Protected Overridable Function OnPreFilterMessage(ByRef m As Message) As Boolean
                If EnableActiveXFix = True Then
                    If m.Msg = Win32.Msgs.WM_MOUSEMOVE Then
                        OnDragging()
                    ElseIf m.Msg = Win32.Msgs.WM_LBUTTONUP Then
                        EndDrag(False)
                    ElseIf m.Msg = Win32.Msgs.WM_CAPTURECHANGED Then
                        EndDrag(Not IsRunningOnMono)
                    ElseIf m.Msg = Win32.Msgs.WM_KEYDOWN AndAlso CInt(m.WParam) = Keys.Escape Then
                        EndDrag(True)
                    End If
                End If

                Return False
            End Function

            Protected NotOverridable Overrides Sub WndProc(ByRef m As Message)
                If EnableActiveXFix = True Then
                    'Manually pre-filter message, rather than using
                    'Application.AddMessageFilter(this).  This fixes
                    'the docker control for ActiveX objects
                    OnPreFilterMessage(m)
                End If

                If m.Msg = Win32.Msgs.WM_CANCELMODE OrElse m.Msg = Win32.Msgs.WM_CAPTURECHANGED Then EndDrag(True)

                MyBase.WndProc(m)
            End Sub
        End Class

        Public MustInherit Class DragHandler
            Inherits DragHandlerBase
            Private m_dockPanel As DockPanel

            Protected Sub New(dockPanel As DockPanel)
                m_dockPanel = dockPanel
            End Sub

            Public ReadOnly Property DockPanel As DockPanel
                Get
                    Return m_dockPanel
                End Get
            End Property

            Private m_dragSource As IDragSource
            Protected Property DragSource As IDragSource
                Get
                    Return m_dragSource
                End Get
                Set(value As IDragSource)
                    m_dragSource = value
                End Set
            End Property

            Protected NotOverridable Overrides ReadOnly Property DragControl As Control
                Get
                    Return If(DragSource Is Nothing, Nothing, DragSource.DragControl)
                End Get
            End Property

            Protected NotOverridable Overrides Function OnPreFilterMessage(ByRef m As Message) As Boolean
                If (m.Msg = Win32.Msgs.WM_KEYDOWN OrElse m.Msg = Win32.Msgs.WM_KEYUP) AndAlso (CInt(m.WParam) = Keys.ControlKey OrElse CInt(m.WParam) = Keys.ShiftKey) Then OnDragging()

                Return MyBase.OnPreFilterMessage(m)
            End Function
        End Class
    End Class
End Namespace
