Namespace Gtk.CSSEngine

    Public Class CSSProperty

        Public ControlType As String
        Public ControlID As String
        Public ControlClass As String
        Public PSSelector As String
        Public ParentPath As String

        'Parent Path descriptor:
        'Parent<GrandParent<1GGrandParent<

        Public Values As New Hashtable

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

        End Function

        Public Sub New()
        End Sub
        Public Sub New(s() As String)

            Me.Parse(s)

        End Sub

    End Class
End Namespace