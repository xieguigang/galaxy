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

        Public Property screenName As String
        Public Property x As Integer
        Public Property y As Integer

        Public Sub ApplySettings(window As ToolWindow)
            window.DockState = dock

            Select Case dock
                Case DockState.DockBottom, DockState.DockBottomAutoHide, DockState.DockTop, DockState.DockTopAutoHide
                    window.Height = height
                Case DockState.DockLeft, DockState.DockLeftAutoHide, DockState.DockRight, DockState.DockRightAutoHide
                    window.Width = width
                Case DockState.Float
                    Dim x = Me.x
                    Dim y = Me.y

                    If Not screenName.StringEmpty(, True) Then
                        ' 获取目标屏幕
                        Dim targetScreen As Screen = Screen.AllScreens.Where(Function(s) s.DeviceName = screenName).FirstOrDefault

                        If targetScreen IsNot Nothing Then
                            ' 计算居中坐标 (使用 WorkingArea 可以避免任务栏遮挡)
                            ' 如果你想让窗口铺满或者基于左上角，可以使用 targetScreen.Bounds.Left
                            x = targetScreen.Bounds.Left + x
                            y = targetScreen.Bounds.Top + y
                        End If
                    End If

                    window.StartPosition = FormStartPosition.Manual
                    window.Size = New Size(width, height)
                    window.Location = New Point(x, y)
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
                Dim currentScreen As Screen = Screen.FromControl(DirectCast(tool, Form))

                Yield New DockSettings With {
                    .dock = tool.DockState,
                    .key = tool.Name,
                    .width = tool.Width,
                    .height = tool.Height,
                    .screenName = currentScreen.DeviceName,
                    .x = tool.Left,
                    .y = tool.Top
                }
            Next
        End Function

    End Class
End Namespace