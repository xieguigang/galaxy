''' <summary>
''' Common icons image from https://icons8.com/ website
''' </summary>
Public NotInheritable Class Icons8

    Private Sub New()
    End Sub

    ''' <summary>
    ''' icons for job done and success
    ''' </summary>
    ''' <returns></returns>
    Public Shared ReadOnly Property JobDone As Image
        Get
            Return My.Resources.icons8_done_144
        End Get
    End Property

    Public Shared ReadOnly Property Warning As Image
        Get
            Return My.Resources.icons8_warning_96
        End Get
    End Property

    Public Shared ReadOnly Property Information As Image
        Get
            Return My.Resources.icons8_information_96
        End Get
    End Property

End Class
