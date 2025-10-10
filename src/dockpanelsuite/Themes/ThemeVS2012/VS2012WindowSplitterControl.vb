Imports System.ComponentModel
Imports System.Drawing.Drawing2D
Imports Microsoft.VisualStudio.WinForms.Docking

Namespace ThemeVS2012
    <ToolboxItem(False)>
    Public Class VS2012WindowSplitterControl
        Inherits SplitterBase
        Private ReadOnly _horizontalBrush As SolidBrush
        Private ReadOnly _backgroundBrush As SolidBrush
        Private _foregroundBrush As PathGradientBrush
        Private ReadOnly _verticalSurroundColors As Color()
        Private ReadOnly _host As ISplitterHost

        Public Sub New(host As ISplitterHost)
            _host = host
            _horizontalBrush = host.DockPanel.Theme.PaintingService.GetBrush(host.DockPanel.Theme.ColorPalette.TabSelectedInactive.Background)
            _backgroundBrush = host.DockPanel.Theme.PaintingService.GetBrush(host.DockPanel.Theme.ColorPalette.MainWindowActive.Background)
            _verticalSurroundColors = {host.DockPanel.Theme.ColorPalette.MainWindowActive.Background}
        End Sub

        Protected Overrides Sub OnSizeChanged(e As EventArgs)
            MyBase.OnSizeChanged(e)
            Dim rect = ClientRectangle
            If rect.Width <= 0 OrElse rect.Height <= 0 Then Return

            If Dock <> DockStyle.Left AndAlso Dock <> DockStyle.Right Then Return

            _foregroundBrush?.Dispose()
            Using path = New GraphicsPath()
                path.AddRectangle(rect)
                _foregroundBrush = New PathGradientBrush(path) With {
    .CenterColor = _horizontalBrush.Color,
    .SurroundColors = _verticalSurroundColors
}
            End Using
        End Sub

        Protected Overrides Sub Dispose(disposing As Boolean)
            If Not IsDisposed AndAlso disposing Then
                _foregroundBrush?.Dispose()
            End If

            MyBase.Dispose(disposing)
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

            If _host.IsDockWindow Then
                Select Case Dock
                    Case DockStyle.Right, DockStyle.Left
                        Diagnostics.Debug.Assert(SplitterSize = rect.Width)
                        e.Graphics.FillRectangle(_backgroundBrush, rect)
                        e.Graphics.FillRectangle(_foregroundBrush, CSng(rect.X + SplitterSize / 2 - 1), rect.Y, CSng(SplitterSize / 3), rect.Height)
                    Case DockStyle.Bottom, DockStyle.Top
                        Diagnostics.Debug.Assert(SplitterSize = rect.Height)
                        e.Graphics.FillRectangle(_horizontalBrush, rect)
                End Select

                Return
            End If

            Select Case _host.DockState
                Case DockState.DockRightAutoHide, DockState.DockLeftAutoHide
                    Diagnostics.Debug.Assert(SplitterSize = rect.Width)
                    e.Graphics.FillRectangle(_backgroundBrush, rect)
                Case DockState.DockBottomAutoHide, DockState.DockTopAutoHide
                    Diagnostics.Debug.Assert(SplitterSize = rect.Height)
                    e.Graphics.FillRectangle(_horizontalBrush, rect)
            End Select
        End Sub
    End Class

End Namespace
