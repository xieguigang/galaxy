Imports ThemeVS2015.WeifenLuo.WinFormsUI.ThemeVS2012
Imports ThemeVS2015.WeifenLuo.WinFormsUI.ThemeVS2013
Imports WeifenLuo.WinFormsUI.Docking

Namespace WeifenLuo.WinFormsUI.ThemeVS2015

    ''' <summary>
    ''' Visual Studio 2015 theme base.
    ''' </summary>
    Public MustInherit Class VS2015ThemeBase
        Inherits ThemeBase
        Public Sub New(resources As Byte())
            ColorPalette = New DockPanelColorPalette(New VS2012PaletteFactory(resources))
            Skin = New DockPanelSkin()
            PaintingService = New PaintingService()
            ImageService = New ImageService(Me)
            ToolStripRenderer = New VisualStudioToolStripRenderer(ColorPalette) With {
    .UseGlassOnMenuStrip = False
}
            Measures.SplitterSize = 6
            Measures.AutoHideSplitterSize = 3
            Measures.DockPadding = 6
            ShowAutoHideContentOnHover = False
            Extender.AutoHideStripFactory = New VS2012AutoHideStripFactory()
            Extender.AutoHideWindowFactory = New VS2012AutoHideWindowFactory()
            Extender.DockPaneFactory = New VS2013DockPaneFactory()
            Extender.DockPaneCaptionFactory = New VS2013DockPaneCaptionFactory()
            Extender.DockPaneStripFactory = New VS2013DockPaneStripFactory()
            Extender.DockPaneSplitterControlFactory = New VS2013DockPaneSplitterControlFactory()
            Extender.WindowSplitterControlFactory = New VS2013WindowSplitterControlFactory()
            Extender.DockWindowFactory = New VS2012DockWindowFactory()
            Extender.PaneIndicatorFactory = New VS2012PaneIndicatorFactory()
            Extender.PanelIndicatorFactory = New VS2012PanelIndicatorFactory()
            Extender.DockOutlineFactory = New VS2012DockOutlineFactory()
            Extender.DockIndicatorFactory = New VS2012DockIndicatorFactory()
        End Sub

        Public Overrides Sub CleanUp(dockPanel As DockPanel)
            PaintingService.CleanUp()
            MyBase.CleanUp(dockPanel)
        End Sub
    End Class
End Namespace
