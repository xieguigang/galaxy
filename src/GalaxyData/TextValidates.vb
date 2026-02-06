Imports System.Runtime.CompilerServices

Public Module TextValidates

    Const title As String = "Text Validation"

    <Extension>
    Public Function ValidateDouble(txt As TextBox,
                                   Optional fieldName As String = Nothing,
                                   Optional pip As Object = "") As Double?

        If pip Is Nothing Then
            Return Nothing
        End If

        Dim prompt As String = If(fieldName.StringEmpty, "", $"({fieldName}: double)")

        If txt.Text.StringEmpty Then
            MessageBox.Show($"A required{prompt} text input should not be empty!", title, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return Nothing
        ElseIf Not txt.Text.IsNumeric(includesInteger:=True) Then
            MessageBox.Show($"A required{prompt} text input should be a number!", title, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return Nothing
        Else
            Return Val(txt.Text)
        End If
    End Function

    <Extension>
    Public Function ValidateInteger(txt As TextBox,
                                    Optional fieldName As String = Nothing,
                                    Optional pip As Object = "") As Integer?

        If pip Is Nothing Then
            Return Nothing
        End If

        Dim prompt As String = If(fieldName.StringEmpty, "", $"({fieldName}: integer)")

        If txt.Text.StringEmpty Then
            MessageBox.Show($"A required{prompt} text input should not be empty!", title, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return Nothing
        ElseIf Not txt.Text.IsInteger Then
            MessageBox.Show($"A required{prompt} text input should be an integer!", title, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return Nothing
        Else
            Return Integer.Parse(txt.Text)
        End If
    End Function

    <Extension>
    Public Function StringEmpty(txt As TextBox,
                                Optional whitespaceAsEmpty As Boolean = True,
                                Optional testEmptyFactor As Boolean = False) As Boolean

        If txt Is Nothing OrElse txt.Text.StringEmpty(whitespaceAsEmpty, testEmptyFactor) Then
            Return True
        Else
            Return False
        End If
    End Function
End Module
