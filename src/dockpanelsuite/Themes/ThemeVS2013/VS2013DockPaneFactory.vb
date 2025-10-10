Imports System.Diagnostics.CodeAnalysis
Imports Microsoft.VisualStudio.WinForms.Docking

Namespace ThemeVS2013
    Public Class VS2013DockPaneFactory
        Implements DockPanelExtender.IDockPaneFactory
        Public Function CreateDockPane(content As IDockContent, visibleState As DockState, show As Boolean) As DockPane Implements DockPanelExtender.IDockPaneFactory.CreateDockPane
            Return New VS2013DockPane(content, visibleState, show)
        End Function

        <SuppressMessage("Microsoft.Naming", "CA1720:AvoidTypeNamesInParameters", MessageId:="1#")>
        Public Function CreateDockPane(content As IDockContent, floatWindow As FloatWindow, show As Boolean) As DockPane Implements DockPanelExtender.IDockPaneFactory.CreateDockPane
            Return New VS2013DockPane(content, floatWindow, show)
        End Function

        Public Function CreateDockPane(content As IDockContent, previousPane As DockPane, alignment As DockAlignment, proportion As Double, show As Boolean) As DockPane Implements DockPanelExtender.IDockPaneFactory.CreateDockPane
            Return New VS2013DockPane(content, previousPane, alignment, proportion, show)
        End Function

        <SuppressMessage("Microsoft.Naming", "CA1720:AvoidTypeNamesInParameters", MessageId:="1#")>
        Public Function CreateDockPane(content As IDockContent, floatWindowBounds As Rectangle, show As Boolean) As DockPane Implements DockPanelExtender.IDockPaneFactory.CreateDockPane
            Return New VS2013DockPane(content, floatWindowBounds, show)
        End Function
    End Class
End Namespace
