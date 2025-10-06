Imports System.Threading
Imports Microsoft.VisualBasic.ComponentModel
Imports Mzkit_win32.BasicMDIForm.Container

Friend Class ActionRunner : Implements ITaskDriver

    ReadOnly progress As TaskProgress
    ReadOnly host As Control
    ReadOnly run As Action(Of ITaskProgress)
    ReadOnly safeCatch As Boolean

    Public Property title As String
    Public Property info As String

    Sub New(run As Action(Of ITaskProgress), progress As TaskProgress, host As Control, Optional safeCatch As Boolean = True)
        Me.run = run
        Me.progress = progress
        Me.host = host
        Me.safeCatch = safeCatch
    End Sub

    Private Sub RunImpl()
        If host Is Nothing Then
            Call run(progress)
        Else
            Call host.Invoke(Sub() Call run(progress))
        End If
    End Sub

    Private Function ITaskDriver_Run() As Integer Implements ITaskDriver.Run
        Do While Not progress.webkitLoaded
            Call Thread.Sleep(30)
        Loop

        Call progress.ShowProgressTitle(title)
        Call progress.ShowProgressDetails(info)

        If AppEnvironment.IsDevelopmentMode AndAlso Not safeCatch Then
            Call RunImpl()
        Else
            Try
                Call RunImpl()
            Catch ex As Exception
                Call App.LogException(ex)
                Call progress.ShowProgressTitle("Task Error!")
                Call progress.ShowProgressDetails(ex.Message)
                Call Thread.Sleep(3 * 1000)
            Finally

            End Try
        End If

        Call progress.CloseWindow()

        Return 0
    End Function
End Class
