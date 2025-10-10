Imports Microsoft.VisualStudio.WinForms.Docking

Namespace ThemeVS2013
    Friend Class VS2013DockPaneCaptionFactory
        Implements DockPanelExtender.IDockPaneCaptionFactory
        Public Function CreateDockPaneCaption(pane As DockPane) As DockPaneCaptionBase Implements DockPanelExtender.IDockPaneCaptionFactory.CreateDockPaneCaption
            Return New VS2013DockPaneCaption(pane)
        End Function
    End Class
End Namespace
