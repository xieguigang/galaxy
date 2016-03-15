
Public Class Button
    Inherits Control

    Private SUp As Image = My.Resources.certificate_16
    Private SOvr As Image = My.Resources.certificate_16
    Public Status As Integer = 0

    Public Property ImageOver() As Image
        Get

            Return SOvr

        End Get
        Set(ByVal value As Image)

            SOvr = value
            Me.Refresh()

        End Set
    End Property

    Public Property ImageUp() As Image
        Get

            Return SUp

        End Get
        Set(ByVal value As Image)

            SUp = value
            Me.Refresh()

        End Set
    End Property

    Private Sub Button_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown

        Status = 0
        Me.Refresh()

    End Sub

    Private Sub Button_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.MouseEnter

        Status = 1
        Me.Refresh()

    End Sub

    Private Sub Button_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.MouseLeave

        Status = 0
        Me.Refresh()

    End Sub

    Private Sub Button_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseUp

        Status = 1
        Me.Refresh()

    End Sub

    Private Sub Button_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint

        Try

            With e.Graphics

                Select Case Status

                    Case 0

                        .DrawImage(SUp, CInt((Me.Width / 2) - (SUp.Width / 2)), CInt((Me.Height / 2) - (SUp.Height / 2)), SUp.Width, SUp.Height)

                    Case 1

                        .DrawImage(SOvr, CInt((Me.Width / 2) - (SOvr.Width / 2)), CInt((Me.Height / 2) - (SOvr.Height / 2)), SUp.Width, SUp.Height)

                End Select

            End With

        Catch

        End Try

    End Sub

    Public Sub New()
        Me.SetStyle(ControlStyles.SupportsTransparentBackColor, True)
        Me.SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint, True)
        Me.UpdateStyles()
    End Sub
End Class
