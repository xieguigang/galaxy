Namespace Controls.UI

    Public Interface IImageButton
        Property UI As ImageButton
        Sub SetSize(size As Size)
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

        Public Sub SetValue(btn As IImageButton)
            Call btn.SetSize(Normal.Size)
            btn.UI = Me
        End Sub
    End Class

    Public Class CheckButton : Inherits ImageButton

        Public Property Checked As Image
    End Class
End Namespace
