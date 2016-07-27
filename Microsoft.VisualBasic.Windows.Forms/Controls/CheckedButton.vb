Imports Microsoft.VisualBasic.Windows.Forms.Controls.UI

Namespace Controls

    Public Class CheckedButton
        Implements UI.IImageButton(Of UI.CheckButton)

        Public Property Checked As Boolean
            Get
                Return _status.HasFlag(ButtonState.Checked)
            End Get
            Set(value As Boolean)
                If value Then
                    _status = ButtonState.Checked
                Else
                    _status = ButtonState.Normal
                End If
            End Set
        End Property

        Private Property IImageButton_UI As CheckButton Implements IImageButton(Of CheckButton).UI
            Get
                Return _uiRes
            End Get
            Set(value As CheckButton)
                _uiRes = value
                UI = value
            End Set
        End Property

        Dim _uiRes As UI.CheckButton

        Private Sub IImageButton_SetSize(size As Size) Implements IImageButton(Of CheckButton).SetSize
            Me.Size = size
        End Sub

        Protected Overrides Sub ImageButton_Paint(sender As Object, e As PaintEventArgs)
            If Checked Then
                Dim res As Image

                If _status.HasFlag(ButtonState.Pushed) Then
                    res = _uiRes.CheckedPress
                    Call Debug.WriteLine("Checked + Pressed")
                ElseIf _status.HasFlag(ButtonState.Flat) Then
                    res = _uiRes.CheckedHighlight
                    Call Debug.WriteLine("Checked + Highlight")
                ElseIf _status.HasFlag(ButtonState.Normal) Then
                    res = _uiRes.Checked
                    Call Debug.WriteLine("Checked + Normal")
                Else
                    Throw New NotSupportedException(_status.ToString)
                End If

                Call e.Graphics.DrawImage(res, New Rectangle(New Point, Size))
            Else
                Call MyBase.ImageButton_Paint(sender, e)
            End If
        End Sub

        Protected Overrides Sub ImageButton_MouseDown(sender As Object, e As MouseEventArgs)
            If Checked Then
                _status = ButtonState.Checked Or ButtonState.Pushed
            Else
                _status = ButtonState.Pushed
            End If
            Call Me.Invalidate()
        End Sub

        Protected Overrides Sub ImageButton_MouseEnter(sender As Object, e As EventArgs)
            If Checked Then
                _status = ButtonState.Checked Or ButtonState.Flat
            Else
                _status = ButtonState.Flat
            End If
            Call Me.Invalidate()
        End Sub

        Protected Overrides Sub ImageButton_MouseUp(sender As Object, e As MouseEventArgs)
            If Checked Then
                _status = ButtonState.Normal
            Else
                _status = ButtonState.Checked Or ButtonState.Normal
            End If

            Call Me.Invalidate()
            Call __fireClick()
        End Sub

        Protected Overrides Sub ImageButton_MouseLeave(sender As Object, e As EventArgs)
            If Checked Then
                _status = ButtonState.Checked Or ButtonState.Normal
            Else
                _status = ButtonState.Normal
            End If
            Call Me.Invalidate()
        End Sub
    End Class
End Namespace