Imports System.Management
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ApplicationServices

Public Module Tools

    Public Sub OpenUrlWithDefaultBrowser(url As String)
        Try
            ' 启用系统关联程序（即默认浏览器）
            Dim startInfo As New ProcessStartInfo(url) With {
                .UseShellExecute = True
            }
            Call Process.Start(startInfo)
        Catch ex As Exception
            ' 异常处理见下文
            Call App.LogException(ex)
        End Try
    End Sub

    ''' <summary>
    ''' 递归终止指定父进程的所有子进程（包括孙子进程）
    ''' </summary>
    ''' <param name="parentProcessId"></param>
    Public Sub KillDescendantProcesses(parentProcessId As Integer)
        Dim searcher As New ManagementObjectSearcher(
            $"SELECT * FROM Win32_Process WHERE ParentProcessId = {parentProcessId}")

        For Each obj As ManagementObject In searcher.Get()
            Dim childId As Integer = Convert.ToInt32(obj("ProcessId"))
            Try
                Dim childProc As Process = Process.GetProcessById(childId)
                ' 先递归杀死子进程的子进程
                KillDescendantProcesses(childId)
                ' 再终止当前子进程
                childProc.Kill()
                childProc.WaitForExit(2000)
                childProc.Dispose()
            Catch ex As ArgumentException
                ' 进程已不存在，忽略
            Catch ex As System.ComponentModel.Win32Exception
                ' 权限不足或拒绝访问
            End Try
        Next
    End Sub

    ''' <summary>
    ''' 快速终止当前进程的所有子进程
    ''' </summary>
    Public Sub KillAllChildrenOfCurrentProcess()
        KillDescendantProcesses(Process.GetCurrentProcess().Id)
    End Sub
End Module
