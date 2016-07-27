Imports Microsoft.VisualBasic.Windows.Forms.Helpers

Namespace Controls

    Public Class Caption

        Dim WithEvents Minimize As ImageButton
        Dim WithEvents Maximize As CheckedButton
        Dim WithEvents Closebtn As ImageButton

        Public Event OnClickMinimize()
        Public Event OnClickMaximize()
        Public Event OnClickClose()
        Public Event OnClickRestore()

        Public Sub ApplyUI(min As UI.ImageButton, max As UI.CheckButton, close As UI.ImageButton)
            Minimize = New ImageButton
            Call min.SetValue(Minimize)
            Maximize = New CheckedButton
            Call max.SetValue(chkbtn:=Maximize)
            Closebtn = New ImageButton
            Call close.SetValue(Closebtn)

            Controls.Add(Minimize)
            Controls.Add(Maximize)
            Controls.Add(Closebtn)

            Dim sz As Size = Size

            Closebtn.Location = New Point(sz.Width - Closebtn.Width, 0)
            Closebtn.Anchor = AnchorStyles.Top Or AnchorStyles.Right
            Maximize.Location = New Point(Closebtn.Location.X - Maximize.Width, 0)
            Maximize.Anchor = AnchorStyles.Top Or AnchorStyles.Right
            Minimize.Location = New Point(Maximize.Location.X - Minimize.Width, 0)
            Minimize.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        End Sub

        Private Sub Closebtn_DoClick(sender As ImageButton) Handles Closebtn.DoClick
            RaiseEvent OnClickClose()
        End Sub

        Private Sub Maximize_DoClick(sender As ImageButton) Handles Maximize.DoClick
            If Maximize.Checked Then
                RaiseEvent OnClickMaximize()
            Else
                RaiseEvent OnClickRestore()
            End If
        End Sub

        Private Sub Minimize_DoClick(sender As ImageButton) Handles Minimize.DoClick
            RaiseEvent OnClickMinimize()
        End Sub

        Dim moveScreen As MoveScreen

        Private Sub Caption_ParentChanged(sender As Object, e As EventArgs) Handles Me.ParentChanged
            moveScreen = New MoveScreen(Me, ParentForm)
        End Sub
    End Class
End Namespace