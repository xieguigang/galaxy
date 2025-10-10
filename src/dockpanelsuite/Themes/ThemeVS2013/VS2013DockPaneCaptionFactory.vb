Imports ThemeVS2015.WeifenLuo.WinFormsUI.Docking
Imports WeifenLuo.WinFormsUI.Docking

Namespace WeifenLuo.WinFormsUI.ThemeVS2013
    Friend Class VS2013DockPaneCaptionFactory
        Implements DockPanelExtender.IDockPaneCaptionFactory
        Public Function CreateDockPaneCaption(pane As DockPane) As DockPaneCaptionBase Implements DockPanelExtender.IDockPaneCaptionFactory.CreateDockPaneCaption
            Return New VS2013DockPaneCaption(pane)
        End Function
    End Class
End Namespace
