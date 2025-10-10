Imports System.ComponentModel
Imports System.Diagnostics.CodeAnalysis
Imports Microsoft.VisualStudio.WinForms.Docking

Namespace ThemeVS2013
    <ToolboxItem(False)>
    Public Class VS2013DockPane
        Inherits DockPane
        Public Sub New(content As IDockContent, visibleState As DockState, show As Boolean)
            MyBase.New(content, visibleState, show)
        End Sub

        <SuppressMessage("Microsoft.Naming", "CA1720:AvoidTypeNamesInParameters", MessageId:="1#")>
        Public Sub New(content As IDockContent, floatWindow As FloatWindow, show As Boolean)
            MyBase.New(content, floatWindow, show)
        End Sub

        Public Sub New(content As IDockContent, previousPane As DockPane, alignment As DockAlignment, proportion As Double, show As Boolean)
            MyBase.New(content, previousPane, alignment, proportion, show)
        End Sub

        <SuppressMessage("Microsoft.Naming", "CA1720:AvoidTypeNamesInParameters", MessageId:="1#")>
        Public Sub New(content As IDockContent, floatWindowBounds As Rectangle, show As Boolean)
            MyBase.New(content, floatWindowBounds, show)
        End Sub

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            MyBase.OnPaint(e)
            Dim color = DockPanel.Theme.ColorPalette.ToolWindowBorder
            e.Graphics.FillRectangle(DockPanel.Theme.PaintingService.GetBrush(color), e.ClipRectangle)
        End Sub

        Protected Overrides ReadOnly Property ContentRectangle As Rectangle
            Get
                Dim rect = MyBase.ContentRectangle
                If DockState = DockState.Document OrElse Contents.Count = 1 Then
                    rect.Height -= 1
                End If

                rect.Width -= 2
                rect.X += 1
                Return rect
            End Get
        End Property
    End Class
End Namespace
