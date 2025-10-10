Imports System.ComponentModel
Imports Microsoft.VisualStudio.WinForms.Docking

Namespace ThemeVS2013
    <ToolboxItem(False)>
    Public Class VS2013WindowSplitterControl
        Inherits SplitterBase
        Private _horizontalBrush As SolidBrush
        Private ReadOnly _host As ISplitterHost

        Public Sub New(host As ISplitterHost)
            _host = host
        End Sub

        Protected Overrides ReadOnly Property SplitterSize As Integer
            Get
                Return If(_host.IsDockWindow, _host.DockPanel.Theme.Measures.SplitterSize, _host.DockPanel.Theme.Measures.AutoHideSplitterSize)
            End Get
        End Property

        Protected Overrides Sub StartDrag()
            _host.DockPanel.BeginDrag(_host, _host.DragControl.RectangleToScreen(Bounds))
        End Sub

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            MyBase.OnPaint(e)

            Dim rect = ClientRectangle

            If rect.Width <= 0 OrElse rect.Height <= 0 Then Return

            _horizontalBrush = _host.DockPanel.Theme.PaintingService.GetBrush(_host.DockPanel.Theme.ColorPalette.MainWindowActive.Background)
            If _host.IsDockWindow Then
                Select Case Dock
                    Case DockStyle.Right, DockStyle.Left
                        Diagnostics.Debug.Assert(SplitterSize = rect.Width)
                        e.Graphics.FillRectangle(_horizontalBrush, rect)
                    Case DockStyle.Bottom, DockStyle.Top
                        Diagnostics.Debug.Assert(SplitterSize = rect.Height)
                        e.Graphics.FillRectangle(_horizontalBrush, rect)
                End Select

                Return
            End If

            Select Case _host.DockState
                Case DockState.DockRightAutoHide, DockState.DockLeftAutoHide
                    Diagnostics.Debug.Assert(SplitterSize = rect.Width)
                    e.Graphics.FillRectangle(_horizontalBrush, rect)
                Case DockState.DockBottomAutoHide, DockState.DockTopAutoHide
                    Diagnostics.Debug.Assert(SplitterSize = rect.Height)
                    e.Graphics.FillRectangle(_horizontalBrush, rect)
            End Select
        End Sub
    End Class
End Namespace
