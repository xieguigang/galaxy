Namespace Controls

    Public Class ImageButton

        Public Property UI As UI.ImageButton
            Get
                Return _uiRes
            End Get
            Set(value As UI.ImageButton)
                _uiRes = value
            End Set
        End Property

        Dim _uiRes As UI.ImageButton

        Private Sub ImageButton_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        End Sub
    End Class
End Namespace