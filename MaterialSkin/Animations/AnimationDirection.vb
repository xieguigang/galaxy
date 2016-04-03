Namespace Animations

    Public Enum AnimationDirection

        ''' <summary>
        ''' In. Stops if finished.
        ''' </summary>
        [In]
        ''' <summary>
        ''' Out. Stops if finished.
        ''' </summary>
        Out
        ''' <summary>
        ''' Same as In, but changes to InOutOut if finished.
        ''' </summary>
        InOutIn
        ''' <summary>
        ''' Same as Out.
        ''' </summary>
        InOutOut
        ''' <summary>
        ''' Same as In, but changes to InOutRepeatingOut if finished.
        ''' </summary>
        InOutRepeatingIn
        ''' <summary>
        ''' Same as Out, but changes to InOutRepeatingIn if finished.
        ''' </summary>
        InOutRepeatingOut
    End Enum
End Namespace
