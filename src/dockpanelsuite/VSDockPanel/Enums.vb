Imports System
Imports System.ComponentModel

Namespace Docking
    <Flags>
    <Serializable>
    <Editor(GetType(DockAreasEditor), GetType(Drawing.Design.UITypeEditor))>
    Public Enum DockAreas
        Float = 1
        DockLeft = 2
        DockRight = 4
        DockTop = 8
        DockBottom = 16
        Document = 32
    End Enum

    Public Enum DockState
        Unknown = 0
        Float = 1
        DockTopAutoHide = 2
        DockLeftAutoHide = 3
        DockBottomAutoHide = 4
        DockRightAutoHide = 5
        Document = 6
        DockTop = 7
        DockLeft = 8
        DockBottom = 9
        DockRight = 10
        Hidden = 11
    End Enum

    Public Enum DockAlignment
        Left
        Right
        Top
        Bottom
    End Enum

    Public Enum DocumentStyle
        DockingMdi
        DockingWindow
        DockingSdi
        SystemMdi
    End Enum

    ''' <summary>
    ''' The location to draw the DockPaneStrip for Document style windows.
    ''' </summary>
    Public Enum DocumentTabStripLocation
        Top
        Bottom
    End Enum
End Namespace
