
Namespace Animations

    Public Enum AnimationType
        Linear
        EaseInOut
        EaseOut
        CustomQuadratic
    End Enum

    Public NotInheritable Class AnimationLinear
        Private Sub New()
        End Sub
        Public Shared Function CalculateProgress(progress As Double) As Double
            Return progress
        End Function
    End Class

    Public NotInheritable Class AnimationEaseInOut
        Private Sub New()
        End Sub
        Public Shared PI As Double = Math.PI
        Public Shared PI_HALF As Double = Math.PI / 2

        Public Shared Function CalculateProgress(progress As Double) As Double
            Return EaseInOut(progress)
        End Function

        Private Shared Function EaseInOut(s As Double) As Double
            Return s - Math.Sin(s * 2 * PI) / (2 * PI)
        End Function
    End Class

    Public NotInheritable Class AnimationEaseOut
        Private Sub New()
        End Sub
        Public Shared Function CalculateProgress(progress As Double) As Double
            Return -1 * progress * (progress - 2)
        End Function
    End Class

    Public NotInheritable Class AnimationCustomQuadratic
        Private Sub New()
        End Sub
        Public Shared Function CalculateProgress(progress As Double) As Double
            Dim kickoff As Double = 0.6
            Return 1 - Math.Cos((Math.Max(progress, kickoff) - kickoff) * Math.PI / (2 - (2 * kickoff)))
        End Function
    End Class
End Namespace
