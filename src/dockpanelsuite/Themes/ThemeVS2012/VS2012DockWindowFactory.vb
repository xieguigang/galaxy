Imports Microsoft.VisualStudio.WinForms.Docking

Namespace ThemeVS2012
    Friend Class VS2012DockWindowFactory
        Implements DockPanelExtender.IDockWindowFactory
        Public Function CreateDockWindow(dockPanel As DockPanel, dockState As DockState) As DockWindow Implements DockPanelExtender.IDockWindowFactory.CreateDockWindow
            Return New VS2012DockWindow(dockPanel, dockState)
        End Function
    End Class
End Namespace
