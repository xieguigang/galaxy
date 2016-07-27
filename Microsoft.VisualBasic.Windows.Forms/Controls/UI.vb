Namespace Controls.UI

    Public Interface IImageButton
        Property UI As ImageButton
    End Interface

    Public Class ImageButton

        Public Property Normal As Image
        Public Property Highlight As Image
        Public Property Press As Image
        Public Property Disable As Image

        Sub New()
        End Sub

        Sub New(normal As Color, highlight As Color, press As Color, disable As Color)

        End Sub
    End Class
End Namespace
