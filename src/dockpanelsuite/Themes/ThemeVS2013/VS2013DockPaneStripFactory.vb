Imports WeifenLuo.WinFormsUI.Docking

Namespace WeifenLuo.WinFormsUI.ThemeVS2013
    Public Class VS2013DockPaneStripFactory
        Implements DockPanelExtender.IDockPaneStripFactory
        Public Function CreateDockPaneStrip(pane As DockPane) As DockPaneStripBase Implements DockPanelExtender.IDockPaneStripFactory.CreateDockPaneStrip
            Return New VS2013DockPaneStrip(pane)
        End Function
    End Class
End Namespace
