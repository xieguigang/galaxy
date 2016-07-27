Namespace Helpers

    Public Class DockHelper

        Dim WithEvents Form As Control

        ReadOnly _target As Control
        ReadOnly _dock As DockStyle

        Sub New(ctrl As Control, parent As Control, dock As DockStyle)
            _target = ctrl
            Form = parent
        End Sub

        Private Sub Form_SizeChanged(sender As Object, e As EventArgs) Handles Form.SizeChanged

        End Sub


    End Class
End Namespace
