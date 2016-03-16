Imports Microsoft.VisualBasic.Parallel

Public Class TabIndicator : Implements IDisposable

    'ReadOnly __animatedTimer As New Timer With {
    '    .Interval = 2,
    '    .Enabled = True
    '}

    Public Property Speed As Integer = 30

    Public Property DefaultDock As Integer
    Public Property PointerDock As Integer

    Private Sub TabIndicator_Load(sender As Object, e As EventArgs) Handles Me.Load
        ' AddHandler __animatedTimer.Tick, AddressOf __runDock
        Call RunTask(AddressOf __animatedWorker)
    End Sub

    Private Sub __runDock(sender As Object, e As EventArgs)
        If Math.Abs(Panel1.Location.X - PointerDock) <= Speed Then
            Panel1.Location = New Point(PointerDock, Panel1.Location.Y)
        ElseIf Panel1.Location.X < PointerDock Then
            Panel1.Location = New Point(Panel1.Location.X + Speed, Panel1.Location.Y)
        ElseIf Panel1.Location.X = PointerDock Then
        Else
            Panel1.Location = New Point(Panel1.Location.X - Speed, Panel1.Location.Y)
        End If
    End Sub

    Private Sub __animatedWorker()
        Do While True
            Try
                Call Me.Invoke(Sub() __runDock(Nothing, Nothing))
            Catch ex As Exception
                If Me.IsDisposed Then
                    Exit Do
                End If
            End Try
            Call Threading.Thread.Sleep(2)
        Loop
    End Sub

    'Private Sub TabIndicator_MouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseMove
    '    PointerDock = e.X
    'End Sub
End Class
