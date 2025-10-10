Imports System.Drawing
Imports System.Security.Permissions
Imports System.Windows.Forms

Namespace Docking
    Public MustInherit Class DockPaneCaptionBase
        Inherits Control
        Protected Friend Sub New(pane As DockPane)
            m_dockPane = pane

            SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint, True)
            SetStyle(ControlStyles.Selectable, False)
        End Sub

        Private m_dockPane As DockPane
        Public ReadOnly Property DockPane As DockPane
            Get
                Return m_dockPane
            End Get
        End Property

        Protected ReadOnly Property Appearance As DockPane.AppearanceStyle
            Get
                Return DockPane.Appearance
            End Get
        End Property

        Protected ReadOnly Property HasTabPageContextMenu As Boolean
            Get
                Return DockPane.HasTabPageContextMenu
            End Get
        End Property

        Protected Sub ShowTabPageContextMenu(position As Point)
            DockPane.ShowTabPageContextMenu(Me, position)
        End Sub

        Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
            MyBase.OnMouseUp(e)

            If e.Button = MouseButtons.Right Then ShowTabPageContextMenu(New Point(e.X, e.Y))
        End Sub

        Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
            MyBase.OnMouseDown(e)

            If e.Button = MouseButtons.Left AndAlso DockPane.DockPanel.AllowEndUserDocking AndAlso DockPane.AllowDockDragAndDrop AndAlso DockPane.ActiveContent IsNot Nothing AndAlso (Not IsDockStateAutoHide(DockPane.DockState) OrElse CanDragAutoHide) Then
                DockPane.DockPanel.BeginDrag(DockPane)
            End If
        End Sub

        <SecurityPermission(SecurityAction.LinkDemand, Flags:=SecurityPermissionFlag.UnmanagedCode)>
        Protected Overrides Sub WndProc(ByRef m As Message)
            If m.Msg = Win32.Msgs.WM_LBUTTONDBLCLK Then
                If IsDockStateAutoHide(DockPane.DockState) Then
                    DockPane.DockPanel.ActiveAutoHideContent = Nothing
                    Return
                End If

                If DockPane.IsFloat Then
                    DockPane.RestoreToPanel()
                Else
                    DockPane.Float()
                End If
            End If
            MyBase.WndProc(m)
        End Sub

        Friend Sub RefreshChanges()
            If IsDisposed Then Return

            OnRefreshChanges()
        End Sub

        Protected Overridable Sub OnRightToLeftLayoutChanged()
        End Sub

        Protected Overridable Sub OnRefreshChanges()
        End Sub

        Protected Friend MustOverride Function MeasureHeight() As Integer

        ''' <summary>
        ''' Gets a value indicating whether dock panel can be dragged when in auto hide mode. 
        ''' Default is false.
        ''' </summary>
        Protected Overridable ReadOnly Property CanDragAutoHide As Boolean
            Get
                Return False
            End Get
        End Property
    End Class
End Namespace
