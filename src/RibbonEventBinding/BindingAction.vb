Public Interface IBindingAction
    Sub ClearHook()
End Interface

Public Class BindingAction

    Public Sub ClearRibbonHook(ParamArray bindings As IBindingAction())
        For Each button As IBindingAction In bindings
            If Not button Is Nothing Then
                Call button.ClearHook()
            End If
        Next
    End Sub
End Class