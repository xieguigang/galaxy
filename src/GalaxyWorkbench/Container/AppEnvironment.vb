Namespace Container

    ''' <summary>
    ''' A shared model for test application environment
    ''' </summary>
    Public Module AppEnvironment

        Public ReadOnly Property IsDevelopmentMode As Boolean

        Sub New()
            IsDevelopmentMode = CheckDevelopmentMode()
        End Sub

        Private Function CheckDevelopmentMode() As Boolean
            Return True
        End Function

    End Module
End Namespace