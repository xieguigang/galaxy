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
                Call Me.Invalidate()
            End Set
        End Property

        Dim _uiRes As UI.CheckButton

        Private Sub IImageButton_SetSize(size As Size) Implements IImageButton(Of CheckButton).SetSize
            Me.Size = size
        End Sub

        Protected Overrides Sub ImageButton_Paint(sender As Object, e As PaintEventArgs)
            Dim res As Image

            If Checked Then
                If _status.HasFlag(ButtonState.Normal) Then
                    res = _uiRes.Checked
                ElseIf _status.HasFlag(ButtonState.Pushed) Then
                    res = _uiRes.CheckedPress
                ElseIf _status.HasFlag(ButtonState.Flat) Then
                    res = _uiRes.CheckedHighlight
                Else
                    Throw New NotSupportedException(_status.ToString)
                End If
            Else
                If _status.HasFlag(ButtonState.Normal) Then
                    res = _uiRes.Normal
                ElseIf _status.HasFlag(ButtonState.Pushed) Then
                    res = _uiRes.Press
                ElseIf _status.HasFlag(ButtonState.Flat) Then
                    res = _uiRes.Highlight
                Else
                    Throw New NotSupportedException(_status.ToString)
                End If
            End If

            Call e.Graphics.DrawImage(res, New Rectangle(New Point, Size))
        End Sub

        Private Sub CheckedButton_MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
            If Checked Then
                _status = ButtonState.Checked Or ButtonState.Pushed
            Else
                _status = ButtonState.Pushed
            End If
            Call Me.Invalidate()
        End Sub

        Private Sub CheckedButton_MouseEnter(sender As Object, e As EventArgs) Handles Me.MouseEnter
            If Checked Then
                _status = ButtonState.Checked Or ButtonState.Flat
            Else
                _status = ButtonState.Flat
            End If
            Call Me.Invalidate()
        End Sub

        Private Sub CheckedButton_MouseUp(sender As Object, e As MouseEventArgs) Handles Me.MouseUp
            If Checked Then
                _status = ButtonState.Normal
            Else
                _status = ButtonState.Checked Or ButtonState.Normal
            End If

            Call Me.Invalidate()
            Call __fireClick()
        End Sub

        Private Sub CheckedButton_MouseLeave(sender As Object, e As EventArgs) Handles Me.MouseLeave
            If Checked Then
                _status = ButtonState.Checked Or ButtonState.Normal
            Else
                _status = ButtonState.Normal
            End If
            Call Me.Invalidate()
        End Sub
    End Class
End Namespace