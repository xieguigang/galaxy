Imports Galaxy.Workbench.DockDocument
Imports Microsoft.VisualBasic.ComponentModel.Collection.Generic
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualStudio.WinForms.Docking

Namespace Container

    Public Class DockSettings : Implements INamedValue

        Public Property dock As DockState
        Public Property width As Integer
        Public Property height As Integer
        Public Property key As String Implements INamedValue.Key

        Public Sub ApplySettings(window As ToolWindow)
            window.DockState = dock

            Select Case dock
                Case DockState.DockBottom, DockState.DockBottomAutoHide, DockState.DockTop, DockState.DockTopAutoHide
                    window.Height = height
                Case DockState.DockLeft, DockState.DockLeftAutoHide, DockState.DockRight, DockState.DockRightAutoHide
                    window.Width = width
            End Select
        End Sub

        Public Shared Sub ApplySettings(windows As IEnumerable(Of ToolWindow), settings As DockSettings())
            Dim dockIndex = settings.SafeQuery.ToDictionary(Function(a) a.key)

            For Each tool As ToolWindow In windows.SafeQuery
                If dockIndex.ContainsKey(tool.Name) Then
                    Call dockIndex(tool.Name).ApplySettings(window:=tool)
                End If
            Next
        End Sub

        Public Shared Iterator Function GetSettings(windows As IEnumerable(Of ToolWindow)) As IEnumerable(Of DockSettings)
            For Each tool As ToolWindow In windows.SafeQuery
                Yield New DockSettings With {
                    .dock = tool.DockState,
                    .key = tool.Name,
                    .width = tool.Width,
                    .height = tool.Height
                }
            Next
        End Function

    End Class
End Namespace