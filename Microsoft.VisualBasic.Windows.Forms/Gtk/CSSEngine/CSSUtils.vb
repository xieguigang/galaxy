Imports System.Runtime.CompilerServices

Namespace Gtk.CSSEngine

    Public Module CSSUtils

        Public Function ShortHand(val As String) As String
rest:       val.Replace("  ", " ")

            If Not val.Contains(" ") Then

                val.Replace(vbCrLf, " ")
                GoTo rest

            End If

            Return val
        End Function

        Public Function GetARGB(val As String) As Color
            Dim a As Integer
            Dim r As Integer
            Dim b As Integer
            Dim g As Integer

            If Left(val, 1) = "#" Then

                Return ColorTranslator.FromHtml(val.Substring(1))

            Else

                Dim s() As String = val.Substring(val.IndexOf("(")).Trim(" ""()".ToCharArray).Replace(" ", "").Split(CChar(","))

                For Each q As String In s

                    q = q.Trim(" "",.!@#$%^&*()_+=-".ToCharArray)

                Next

                Select Case s.Length

                    Case 1

                        a = 255
                        r = CInt(s(0))
                        g = CInt(s(0))
                        b = CInt(s(0))

                    Case 2

                        a = CInt(s(0))
                        r = CInt(s(1))
                        g = CInt(s(1))
                        b = CInt(s(1))

                    Case 3

                        a = 255
                        r = CInt(s(0))
                        g = CInt(s(1))
                        b = CInt(s(2))

                    Case 4

                        a = CInt(s(0))
                        r = CInt(s(1))
                        g = CInt(s(2))
                        b = CInt(s(3))

                End Select

            End If

            Return Color.FromArgb(a, r, g, b)
        End Function

        <Extension> Public Function GetFont(params As String) As Font
            Dim fsi As New FontStyle 'Italics holder
            Dim fsb As New FontStyle 'Bold holder
            Dim ic As Integer = 1
            Dim fm As String = "Arial" 'The default family
            Dim em As Integer = 1 'The default size
            Dim fl As Boolean = False

            For Each s As String In params.Split(CChar(" "))

                Select Case s.ToLower

                    Case "italic"

                        fsi = FontStyle.Italic

                    Case "oblique"

                        fsi = FontStyle.Italic

                    Case "normal"

                        If ic = 1 Then 'Means regular; no italics

                            fsi = FontStyle.Regular

                        ElseIf ic = 2 Then 'Means that it isn't small-caps

                            fl = False

                        ElseIf ic = 3 Then

                            fsb = FontStyle.Regular

                        End If

                    Case "small-caps"

                        fm = "Bank Gothic"
                        fl = True

                    Case "bold"

                        fsb = FontStyle.Bold

                    Case "bolder"

                        fsb = FontStyle.Bold

                    Case "lighter"

                        fsb = FontStyle.Regular

                    Case "100"

                        fsb = FontStyle.Regular

                    Case "200"

                        fsb = FontStyle.Regular

                    Case "300"

                        fsb = FontStyle.Regular

                    Case "400"

                        fsb = FontStyle.Regular

                    Case "500"

                        fsb = FontStyle.Bold

                    Case "600"

                        fsb = FontStyle.Bold

                    Case "700"

                        fsb = FontStyle.Bold

                    Case "800"

                        fsb = FontStyle.Bold

                    Case "900"

                        fsb = FontStyle.Bold

                End Select

                ic += 1
            Next

            Return New Font(fm, em, fsi And fsb, GraphicsUnit.Pixel)
        End Function
    End Module
End Namespace