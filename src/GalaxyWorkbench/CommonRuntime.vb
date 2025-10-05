Imports Galaxy.Workbench.CommonDialogs

''' <summary>
''' Common workbench runtime
''' </summary>
Public Module CommonRuntime

    Public ReadOnly Property AppHost As AppHost

    ''' <summary>
    ''' set the opacity of the <see cref="MaskForm"/> when show the input dialog via <see cref="InputDialog.Input(Action(Of InputDialog), Action)"/>
    ''' </summary>
    ''' <returns></returns>
    Public Property MaskOpacity As Integer = 0.5

    ''' <summary>
    ''' set value to the <see cref="AppHost"/> in current common workbench runtime.
    ''' </summary>
    ''' <param name="apphost"></param>
    Public Sub Hook(apphost As AppHost)
        _AppHost = apphost
    End Sub

End Module
