Imports WeifenLuo.WinFormsUI.Docking

Namespace WeifenLuo.WinFormsUI.ThemeVS2013
    Friend Class VS2013DockPaneSplitterControlFactory
        Implements DockPanelExtender.IDockPaneSplitterControlFactory
        Public Function CreateSplitterControl(pane As DockPane) As DockPane.SplitterControlBase Implements DockPanelExtender.IDockPaneSplitterControlFactory.CreateSplitterControl
            Return New VS2013SplitterControl(pane)
        End Function
    End Class
End Namespace
