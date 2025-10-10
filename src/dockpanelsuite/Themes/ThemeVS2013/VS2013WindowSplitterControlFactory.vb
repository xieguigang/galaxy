Imports WeifenLuo.WinFormsUI.Docking

Namespace WeifenLuo.WinFormsUI.ThemeVS2013
    Friend Class VS2013WindowSplitterControlFactory
        Implements DockPanelExtender.IWindowSplitterControlFactory
        Public Function CreateSplitterControl(host As ISplitterHost) As SplitterBase Implements DockPanelExtender.IWindowSplitterControlFactory.CreateSplitterControl
            Return New VS2013WindowSplitterControl(host)
        End Function
    End Class
End Namespace
