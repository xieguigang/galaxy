Imports System.Runtime.CompilerServices
Imports System.Windows.Forms

Public Module TextValidates

    Const title As String = "Text Validation"

    <Extension>
    Public Function ValidateDouble(txt As TextBox, Optional fieldName As String = Nothing) As Double?
        Dim prompt As String = If(fieldName.StringEmpty, "", $"({fieldName}: double)")

        If txt.Text.StringEmpty Then
            MessageBox.Show($"A required{prompt} text input should not be empty!", title, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return Nothing
        ElseIf txt.Text.IsNumeric Then
            MessageBox.Show($"A required{prompt} text input should be a number!", title, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return Nothing
        Else
            Return Val(txt.Text)
        End If
    End Function

    <Extension>
    Public Function ValidateInteger(txt As TextBox, Optional fieldName As String = Nothing) As Integer?
        Dim prompt As String = If(fieldName.StringEmpty, "", $"({fieldName}: integer)")

        If txt.Text.StringEmpty Then
            MessageBox.Show($"A required{prompt} text input should not be empty!", title, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return Nothing
        ElseIf txt.Text.IsInteger Then
            MessageBox.Show($"A required{prompt} text input should be an integer!", title, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return Nothing
        Else
            Return Integer.Parse(txt.Text)
        End If
    End Function
End Module
