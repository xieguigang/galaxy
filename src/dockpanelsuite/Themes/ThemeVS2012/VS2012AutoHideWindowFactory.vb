Imports WeifenLuo.WinFormsUI.Docking

Namespace WeifenLuo.WinFormsUI.ThemeVS2012
    Friend Class VS2012AutoHideWindowFactory
        Implements DockPanelExtender.IAutoHideWindowFactory
        Public Function CreateAutoHideWindow(panel As DockPanel) As DockPanel.AutoHideWindowControl Implements DockPanelExtender.IAutoHideWindowFactory.CreateAutoHideWindow
            Return New VS2012AutoHideWindowControl(panel)
        End Function
    End Class
End Namespace
