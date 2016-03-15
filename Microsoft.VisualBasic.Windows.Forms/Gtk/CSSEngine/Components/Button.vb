Namespace Gtk.CSSEngine.Components


    Public Class Button
        Inherits Control

        Private SUp As Image
        Private SOvr As Image
        Public Status As Integer = 0

        Public Property ImageOver() As Image
            Get

                Return SOvr

            End Get
            Set(value As Image)

                SOvr = value
                Me.Refresh()

            End Set
        End Property

        Public Property ImageUp() As Image
            Get

                Return SUp

            End Get
            Set(value As Image)

                SUp = value
                Me.Refresh()

            End Set
        End Property

        Private Sub Button_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown

            Status = 0
            Me.Refresh()

        End Sub

        Private Sub Button_MouseEnter(sender As Object, e As System.EventArgs) Handles Me.MouseEnter

            Status = 1
            Me.Refresh()

        End Sub

        Private Sub Button_MouseLeave(sender As Object, e As System.EventArgs) Handles Me.MouseLeave

            Status = 0
            Me.Refresh()

        End Sub

        Private Sub Button_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseUp

            Status = 1
            Me.Refresh()

        End Sub

        Private Sub Button_Paint(sender As Object, e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint

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
End Namespace