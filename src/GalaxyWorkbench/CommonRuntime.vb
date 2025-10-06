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

    Public Sub LogText(msg As String)
        If AppHost Is Nothing Then
            NoWorkbenchHostForm()
        Else
            Call AppHost.StatusMessage(msg, Icons8.Information)
        End If
    End Sub

    Public Sub Warning(msg As String)
        If AppHost Is Nothing Then
            NoWorkbenchHostForm()
        Else
            Call AppHost.StatusMessage(msg, Icons8.Warning)
        End If
    End Sub

    Private Sub NoWorkbenchHostForm()
        Call MessageBox.Show("Unable to display the message content because no associated workstation host module could be found in the current runtime environment. Please link a host instance via a Hook function during the program's initialization phase.",
                             "Missing Host Configuration",
                             MessageBoxButtons.OK,
                             MessageBoxIcon.Warning)
    End Sub
End Module
