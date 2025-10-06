Imports System.Threading
Imports Microsoft.VisualBasic.ComponentModel
Imports Mzkit_win32.BasicMDIForm.Container

Public Class LoadDataTask(Of T) : Implements ITaskDriver

    ReadOnly progress As TaskProgress
    ReadOnly host As Control
    ReadOnly streamLoad As Func(Of ITaskProgress, T)

    Dim tmp As T = Nothing

    Public Property title As String
    Public Property info As String
    Public ReadOnly Property result As T
        Get
            Return tmp
        End Get
    End Property

    Sub New(streamLoad As Func(Of ITaskProgress, T), progress As TaskProgress, host As Control)
        Me.streamLoad = streamLoad
        Me.host = host
        Me.progress = progress
    End Sub

    Private Sub loadData()
        tmp = Nothing

        If host Is Nothing Then
            tmp = streamLoad(progress)
        Else
            tmp = host.Invoke(Function() streamLoad(progress))
        End If
    End Sub

    Public Function Run() As Integer Implements ITaskDriver.Run
        Do While Not progress.webkitLoaded
            Call Thread.Sleep(30)
        Loop

        Call progress.ShowProgressTitle(title)
        Call progress.ShowProgressDetails(info)

        If AppEnvironment.IsDevelopmentMode Then
            Call loadData()
        Else
            Try
                Call loadData()
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
