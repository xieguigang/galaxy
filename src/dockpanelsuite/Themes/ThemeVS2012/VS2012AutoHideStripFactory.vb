Imports ThemeVS2015.WeifenLuo.WinFormsUI.Docking
Imports WeifenLuo.WinFormsUI.Docking

Namespace WeifenLuo.WinFormsUI.ThemeVS2012
    Friend Class VS2012AutoHideStripFactory
        Implements DockPanelExtender.IAutoHideStripFactory
        Public Function CreateAutoHideStrip(panel As DockPanel) As AutoHideStripBase Implements DockPanelExtender.IAutoHideStripFactory.CreateAutoHideStrip
            Return New VS2012AutoHideStrip(panel)
        End Function
    End Class
End Namespace
