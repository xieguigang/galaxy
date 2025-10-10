Imports WeifenLuo.WinFormsUI.Docking

Namespace WeifenLuo.WinFormsUI.ThemeVS2012
    Friend Class VS2012DockIndicatorFactory
        Implements DockPanelExtender.IDockIndicatorFactory
        Public Function CreateDockIndicator(dockDragHandler As DockPanel.DockDragHandler) As DockPanel.DockDragHandler.DockIndicator Implements DockPanelExtender.IDockIndicatorFactory.CreateDockIndicator
            Return New DockPanel.DockDragHandler.DockIndicator(dockDragHandler) With {
                .Opacity = 0.7
            }
        End Function
    End Class
End Namespace
