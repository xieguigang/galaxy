Namespace Controls.UI

    Public Interface IImageButton(Of TUI As ImageButton)
        Property UI As TUI
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

        Public Sub SetValue(btn As IImageButton(Of ImageButton))
            Call btn.SetSize(Normal.Size)
            btn.UI = Me
        End Sub
    End Class

    Public Class CheckButton : Inherits ImageButton

        Public Property Checked As Image
        Public Property CheckedPress As Image
        Public Property CheckedHighlight As Image

        Public Overloads Sub SetValue(chkbtn As IImageButton(Of CheckButton))
            Call chkbtn.SetSize(Normal.Size)
            chkbtn.UI = Me
        End Sub
    End Class
End Namespace
