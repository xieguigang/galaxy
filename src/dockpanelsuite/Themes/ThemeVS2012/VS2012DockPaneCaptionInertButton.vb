Imports System.ComponentModel
Imports Microsoft.VisualStudio.WinForms.Docking

Namespace ThemeVS2012
    <ToolboxItem(False)>
    Public Class VS2012DockPaneCaptionInertButton
        Inherits InertButtonBase
        Private _hovered As Bitmap
        Private _normal As Bitmap
        Private _active As Bitmap
        Private _pressed As Bitmap
        Private _hoveredActive As Bitmap
        Private _hoveredAutoHide As Bitmap
        Private _autoHide As Bitmap
        Private _pressedAutoHide As Bitmap

        Public Sub New(dockPaneCaption As DockPaneCaptionBase, hovered As Bitmap, normal As Bitmap, pressed As Bitmap, hoveredActive As Bitmap, active As Bitmap, Optional hoveredAutoHide As Bitmap = Nothing, Optional autoHide As Bitmap = Nothing, Optional pressedAutoHide As Bitmap = Nothing)
            m_dockPaneCaption = dockPaneCaption
            _hovered = hovered
            _normal = normal
            _pressed = pressed
            _hoveredActive = hoveredActive
            _active = active
            _hoveredAutoHide = If(hoveredAutoHide, hoveredActive)
            _autoHide = If(autoHide, active)
            _pressedAutoHide = If(pressedAutoHide, pressed)
            RefreshChanges()
        End Sub

        Private m_dockPaneCaption As DockPaneCaptionBase
        Private ReadOnly Property DockPaneCaption As DockPaneCaptionBase
            Get
                Return m_dockPaneCaption
            End Get
        End Property

        Public ReadOnly Property IsAutoHide As Boolean
            Get
                Return DockPaneCaption.DockPane.IsAutoHide
            End Get
        End Property

        Public ReadOnly Property IsActive As Boolean
            Get
                Return DockPaneCaption.DockPane.IsActivePane
            End Get
        End Property

        Public Overrides ReadOnly Property Image As Bitmap
            Get
                Return If(IsActive, If(IsAutoHide, _autoHide, _active), _normal)
            End Get
        End Property

        Public Overrides ReadOnly Property HoverImage As Bitmap
            Get
                Return If(IsActive, If(IsAutoHide, _hoveredAutoHide, _hoveredActive), _hovered)
            End Get
        End Property

        Public Overrides ReadOnly Property PressImage As Bitmap
            Get
                Return If(IsAutoHide, _pressedAutoHide, _pressed)
            End Get
        End Property
    End Class
End Namespace
