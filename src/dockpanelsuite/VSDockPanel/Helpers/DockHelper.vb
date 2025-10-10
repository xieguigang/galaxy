Imports System.Drawing
Imports System.Windows.Forms

Namespace Docking

    Public Module DockHelper
        Public Function IsDockStateAutoHide(dockState As DockState) As Boolean
            If dockState = dockState.DockLeftAutoHide OrElse dockState = dockState.DockRightAutoHide OrElse dockState = dockState.DockTopAutoHide OrElse dockState = dockState.DockBottomAutoHide Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Function IsDockStateValid(dockState As DockState, dockableAreas As DockAreas) As Boolean
            If (dockableAreas And DockAreas.Float) = 0 AndAlso dockState = dockState.Float Then
                Return False
            ElseIf (dockableAreas And DockAreas.Document) = 0 AndAlso dockState = dockState.Document Then
                Return False
            ElseIf (dockableAreas And DockAreas.DockLeft) = 0 AndAlso (dockState = dockState.DockLeft OrElse dockState = dockState.DockLeftAutoHide) Then
                Return False
            ElseIf (dockableAreas And DockAreas.DockRight) = 0 AndAlso (dockState = dockState.DockRight OrElse dockState = dockState.DockRightAutoHide) Then
                Return False
            ElseIf (dockableAreas And DockAreas.DockTop) = 0 AndAlso (dockState = dockState.DockTop OrElse dockState = dockState.DockTopAutoHide) Then
                Return False
            ElseIf (dockableAreas And DockAreas.DockBottom) = 0 AndAlso (dockState = dockState.DockBottom OrElse dockState = dockState.DockBottomAutoHide) Then
                Return False
            Else
                Return True
            End If
        End Function

        Public Function IsDockWindowState(state As DockState) As Boolean
            If state = DockState.DockTop OrElse state = DockState.DockBottom OrElse state = DockState.DockLeft OrElse state = DockState.DockRight OrElse state = DockState.Document Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Function ToggleAutoHideState(state As DockState) As DockState
            If state = DockState.DockLeft Then
                Return DockState.DockLeftAutoHide
            ElseIf state = DockState.DockRight Then
                Return DockState.DockRightAutoHide
            ElseIf state = DockState.DockTop Then
                Return DockState.DockTopAutoHide
            ElseIf state = DockState.DockBottom Then
                Return DockState.DockBottomAutoHide
            ElseIf state = DockState.DockLeftAutoHide Then
                Return DockState.DockLeft
            ElseIf state = DockState.DockRightAutoHide Then
                Return DockState.DockRight
            ElseIf state = DockState.DockTopAutoHide Then
                Return DockState.DockTop
            ElseIf state = DockState.DockBottomAutoHide Then
                Return DockState.DockBottom
            Else
                Return state
            End If
        End Function

        Public Function PaneAtPoint(pt As Point, dockPanel As DockPanel) As DockPane
            If Not IsRunningOnMono Then
                Dim control = ControlAtPoint(pt)

                While control IsNot Nothing
                    Dim content As IDockContent = TryCast(control, IDockContent)
                    If content IsNot Nothing AndAlso content.DockHandler.DockPanel Is dockPanel Then Return content.DockHandler.Pane

                    Dim pane As DockPane = TryCast(control, DockPane)
                    If pane IsNot Nothing AndAlso pane.DockPanel Is dockPanel Then Return pane
                    control = control.Parent
                End While
            End If

            Return Nothing
        End Function

        Public Function FloatWindowAtPoint(pt As Point, dockPanel As DockPanel) As FloatWindow
            If Not IsRunningOnMono Then
                Dim control = ControlAtPoint(pt)

                While control IsNot Nothing
                    Dim floatWindow As FloatWindow = TryCast(control, FloatWindow)
                    If floatWindow IsNot Nothing AndAlso floatWindow.DockPanel Is dockPanel Then Return floatWindow
                    control = control.Parent
                End While
            End If

            Return Nothing
        End Function
    End Module
End Namespace
