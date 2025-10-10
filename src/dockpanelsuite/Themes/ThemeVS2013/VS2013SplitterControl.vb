Imports System.ComponentModel
Imports Microsoft.VisualStudio.WinForms.Docking

Namespace ThemeVS2013
    <ToolboxItem(False)>
    Friend Class VS2013SplitterControl
        Inherits DockPane.SplitterControlBase
        Private ReadOnly _horizontalBrush As SolidBrush
        Private ReadOnly Property SplitterSize As Integer

        Public Sub New(pane As DockPane)
            MyBase.New(pane)
            _horizontalBrush = pane.DockPanel.Theme.PaintingService.GetBrush(pane.DockPanel.Theme.ColorPalette.MainWindowActive.Background)
            SplitterSize = pane.DockPanel.Theme.Measures.SplitterSize
        End Sub

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            MyBase.OnPaint(e)

            Dim rect = ClientRectangle
            If rect.Width <= 0 OrElse rect.Height <= 0 Then Return

            Select Case Alignment
                Case DockAlignment.Right, DockAlignment.Left
                    Diagnostics.Debug.Assert(SplitterSize = rect.Width)
                    e.Graphics.FillRectangle(_horizontalBrush, rect)
                Case DockAlignment.Bottom, DockAlignment.Top
                    Diagnostics.Debug.Assert(SplitterSize = rect.Height)
                    e.Graphics.FillRectangle(_horizontalBrush, rect)
            End Select
        End Sub
    End Class
End Namespace
