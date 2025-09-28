Imports Galaxy.Workbench.CommonDialogs

Public Class DialogPromise

    Private _previousResult As DialogResult
    Private _previousForm As InputDialog
    Private _data As Object

    Private Sub New()
        _previousResult = DialogResult.None
    End Sub

    Friend Sub New(previousForm As InputDialog, previousResult As DialogResult, data As Object)
        _previousForm = previousForm
        _previousResult = previousResult
        _data = data
    End Sub

    ' 启动链式调用的入口方法
    Public Shared Function OpenDialog(Of T As {InputDialog, New})() As DialogPromise
        Return OpenDialog(Of T)(Nothing)
    End Function

    Public Shared Function OpenDialog(Of T As {InputDialog, New})(data As Object) As DialogPromise
        Dim form As New T()
        Dim mask As MaskForm = MaskForm.CreateMask(CommonRuntime.AppHost)
        Dim result = mask.ShowDialog(form)

        Return New DialogPromise(form, result, data)
    End Function

    ' 链式调用下一个对话框
    Public Function ThenDialog(Of T As {InputDialog, New})() As DialogPromise
        Return ThenDialog(Of T)(Function(f) True, Nothing)
    End Function

    Public Function ThenDialog(Of T As {InputDialog, New})(condition As Func(Of InputDialog, Boolean)) As DialogPromise
        Return ThenDialog(Of T)(condition, Nothing)
    End Function

    Public Function ThenDialog(Of T As {InputDialog, New})(data As Object) As DialogPromise
        Return ThenDialog(Of T)(Function(f) True, data)
    End Function

    Public Function ThenDialog(Of T As {InputDialog, New})(
        condition As Func(Of InputDialog, Boolean),
        data As Object) As DialogPromise

        If _previousResult <> DialogResult.OK Then
            Return New DialogPromise(Nothing, _previousResult, data)
        End If

        If condition IsNot Nothing AndAlso Not condition(_previousForm) Then
            Return New DialogPromise(Nothing, DialogResult.Cancel, data)
        End If

        Dim newForm As New T()
        Dim mask As MaskForm = MaskForm.CreateMask(CommonRuntime.AppHost)

        ' 可选：传递数据到下一个表单
        If data IsNot Nothing Then
            SetFormData(newForm, data)
        End If

        Dim result = mask.ShowDialog(newForm)
        Return New DialogPromise(newForm, result, data)
    End Function

    ' 处理最终结果
    Public Sub [Finally](action As Action(Of DialogResult, InputDialog))
        action?.Invoke(_previousResult, _previousForm)
    End Sub

    Public Sub [Finally](action As Action(Of DialogResult))
        action?.Invoke(_previousResult)
    End Sub

    Public Sub [Finally](action As Action)
        Call [Finally](
            Sub(dialogResult)
                If dialogResult = DialogResult.OK Then
                    Call action()
                End If
            End Sub)
    End Sub

    ' 只有当所有对话框都成功时才执行
    Public Sub OnSuccess(action As Action)
        If _previousResult = DialogResult.OK Then
            action?.Invoke()
        End If
    End Sub

    Public Sub OnSuccess(action As Action(Of InputDialog))
        If _previousResult = DialogResult.OK Then
            action?.Invoke(_previousForm)
        End If
    End Sub

    ' 当任何对话框取消时执行
    Public Sub OnCancel(action As Action)
        If _previousResult = DialogResult.Cancel Then
            action?.Invoke()
        End If
    End Sub

    ' 辅助方法：设置表单数据
    Private Sub SetFormData(form As InputDialog, data As Object)
        ' 这里可以根据需要实现数据传递逻辑
        ' 例如通过接口、反射或特定属性设置
        If TypeOf form Is IDataContainer Then
            DirectCast(form, IDataContainer).SetData(data)
        End If
    End Sub

End Class

' 可选的数据传递接口
Public Interface IDataContainer
    Sub SetData(data As Object)
    Function GetData() As Object
End Interface

