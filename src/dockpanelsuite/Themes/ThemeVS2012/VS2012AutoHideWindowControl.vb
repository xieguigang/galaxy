Imports System.Drawing
Imports System.Windows.Forms
Imports WeifenLuo.WinFormsUI.Docking
Imports System.ComponentModel

Namespace WeifenLuo.WinFormsUI.ThemeVS2012

    <ToolboxItem(False)>
    Friend Class VS2012AutoHideWindowControl
        Inherits DockPanel.AutoHideWindowControl
        Public Sub New(dockPanel As DockPanel)
            MyBase.New(dockPanel)
        End Sub

        Protected Overrides ReadOnly Property DisplayingRectangle As Rectangle
            Get
                Dim rect = ClientRectangle

                ' exclude the border and the splitter
                If DockState = DockState.DockBottomAutoHide Then
                    rect.Y += DockPanel.Theme.Measures.AutoHideSplitterSize
                    rect.Height -= DockPanel.Theme.Measures.AutoHideSplitterSize
                ElseIf DockState = DockState.DockRightAutoHide Then
                    rect.X += DockPanel.Theme.Measures.AutoHideSplitterSize
                    rect.Width -= DockPanel.Theme.Measures.AutoHideSplitterSize
                ElseIf DockState = DockState.DockTopAutoHide Then
                    rect.Height -= DockPanel.Theme.Measures.AutoHideSplitterSize
                ElseIf DockState = DockState.DockLeftAutoHide Then
                    rect.Width -= DockPanel.Theme.Measures.AutoHideSplitterSize
                End If

                Return rect
            End Get
        End Property

        Protected Overrides Sub OnLayout(levent As LayoutEventArgs)
            DockPadding.All = 0
            If DockState = DockState.DockLeftAutoHide Then
                m_splitter.Dock = DockStyle.Right
            ElseIf DockState = DockState.DockRightAutoHide Then
                m_splitter.Dock = DockStyle.Left
            ElseIf DockState = DockState.DockTopAutoHide Then
                m_splitter.Dock = DockStyle.Bottom
            ElseIf DockState = DockState.DockBottomAutoHide Then
                m_splitter.Dock = DockStyle.Top
            End If

            Dim rectDisplaying = DisplayingRectangle
            Dim rectHidden As Rectangle = New Rectangle(-rectDisplaying.Width, rectDisplaying.Y, rectDisplaying.Width, rectDisplaying.Height)
            For Each c As Control In Controls
                Dim pane As DockPane = TryCast(c, DockPane)
                If pane Is Nothing Then Continue For

                If pane Is ActivePane Then
                    pane.Bounds = rectDisplaying
                Else
                    pane.Bounds = rectHidden
                End If
            Next

            MyBase.OnLayout(levent)
        End Sub
    End Class
End Namespace
