Imports Microsoft.VisualBasic.Serialization
Imports Microsoft.VisualBasic.Serialization.JSON

Namespace Gtk.CSSEngine

    Public Class CSSProperty

        Public Property ControlType As String
        Public Property ControlID As String
        Public Property ControlClass As String
        Public Property PSSelector As String

        ''' <summary>
        ''' Parent Path descriptor:
        ''' Parent&lt;GrandParent&lt;1GGrandParent&lt;
        ''' </summary>
        ''' <returns></returns>
        Public Property ParentPath As String
        Public Property Values As New Dictionary(Of String, String)

        Public Sub Parse(s() As String)
            ControlClass = s(0)
            ControlType = s(1)
            ControlID = s(2)
            PSSelector = s(3)
            ParentPath = s(4)
        End Sub

        Public Function ExpectedUnit(Format As String, parent As Control) As Integer
            Format = Format.TrimStart("+-".ToCharArray)
            Format = Format.Trim("""".ToCharArray)

            If Format = "auto" Then Return -2

            For i As Integer = 0 To Format.Length - 1

                If Not Char.IsDigit(CChar(Format.Substring(i, 1))) Then

                    Select Case Format.Substring(i).ToLower
                        Case "px"
                            Return CInt(Left(Format, i))
                        Case "in"
                            Return CInt(Left(Format, i)) * PPInch
                        Case "cm"
                            Return CInt(Left(Format, i)) * PPCm
                        Case "mm"
                            Return CInt(Left(Format, i)) * PPMm
                        Case "pt"
                            Return CInt(Left(Format, i)) 'Don't add a unit mod!
                        Case "pc"
                            Return CInt(Left(Format, i)) * PPPc
                    End Select
                End If
            Next

            Return 0
        End Function

        Public Sub New()
        End Sub

        Public Sub New(s() As String)
            Call Me.Parse(s)
        End Sub

        Public Overrides Function ToString() As String
            Return Me.GetJson
        End Function
    End Class
End Namespace