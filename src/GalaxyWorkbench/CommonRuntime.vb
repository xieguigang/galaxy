Public Module CommonRuntime

    Public ReadOnly Property AppHost As AppHost

    Public Sub Hook(apphost As AppHost)
        _AppHost = apphost
    End Sub

End Module
