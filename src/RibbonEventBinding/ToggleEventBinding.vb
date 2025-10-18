Imports Galaxy.Workbench
Imports RibbonLib.Controls

Public Class ToggleEventBinding : Implements IDisposable

    Dim disposedValue As Boolean
    Dim WithEvents ribbon As RibbonToggleButton

    Public evt As Action(Of Boolean)

    Public ReadOnly Property HasHookEvent As Boolean
        Get
            Return Not evt Is Nothing
        End Get
    End Property

    Public ReadOnly Property Checked As Boolean
        Get
            Return ribbon.BooleanValue
        End Get
    End Property

    Sub New(btn As RibbonToggleButton)
        ribbon = btn
    End Sub

    Public Sub ClearHook()
        evt = Nothing
    End Sub

    Private Sub exec_call() Handles ribbon.ExecuteEvent
        If Not evt Is Nothing Then
            Try
                Call evt(ribbon.BooleanValue)
            Catch ex As Exception
                Call CommonRuntime.LogText($"[ribbon menu] error during exec for: {ribbon.Label}")
                Call CommonRuntime.Warning($"[ribbon menu] error during exec for: {ribbon.Label} ({ex.Message})")
                Call CommonRuntime.LogText(ex.ToString)
                Call App.LogException(ex)
            End Try
        Else
            Call CommonRuntime.LogText($"[ribbon menu] no event handler was attached: {ribbon.Label}")
        End If
    End Sub

    Public Overrides Function ToString() As String
        Return ribbon.ToString
    End Function

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: 释放托管状态(托管对象)
                Call ClearHook()
            End If

            ' TODO: 释放未托管的资源(未托管的对象)并重写终结器
            ' TODO: 将大型字段设置为 null
            disposedValue = True
        End If
    End Sub

    ' ' TODO: 仅当“Dispose(disposing As Boolean)”拥有用于释放未托管资源的代码时才替代终结器
    ' Protected Overrides Sub Finalize()
    '     ' 不要更改此代码。请将清理代码放入“Dispose(disposing As Boolean)”方法中
    '     Dispose(disposing:=False)
    '     MyBase.Finalize()
    ' End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        ' 不要更改此代码。请将清理代码放入“Dispose(disposing As Boolean)”方法中
        Dispose(disposing:=True)
        GC.SuppressFinalize(Me)
    End Sub
End Class
