Imports ThemeVS2015.WeifenLuo.WinFormsUI.Docking
Imports WeifenLuo.WinFormsUI.Docking

Namespace WeifenLuo.WinFormsUI.ThemeVS2012
    Friend Class VS2012DockWindowFactory
        Implements DockPanelExtender.IDockWindowFactory
        Public Function CreateDockWindow(dockPanel As DockPanel, dockState As DockState) As DockWindow Implements DockPanelExtender.IDockWindowFactory.CreateDockWindow
            Return New VS2012DockWindow(dockPanel, dockState)
        End Function
    End Class
End Namespace
