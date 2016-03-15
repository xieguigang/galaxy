
Namespace Gtk.CSSEngine

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks>
    ''' Source code downloads from
    ''' https://sourceforge.net/projects/vbcssparser/?source=navbar
    ''' </remarks>
    Public Class CSSFile

        Public Properties As New ArrayList
        Public Location As String
        Public Event GetResource(ByRef item As Object)

        Public Sub ApplyWinForm(ByVal WinForm As Form)

            'Try

            For Each p As CSSFile.CSSProperty In Properties

                If IsMatch(p, WinForm) Then ApplyProperty(WinForm, p)

            Next

            For Each c As Control In WinForm.Controls

                ApplyWinForm(c)

            Next

            'Catch ex As Exception

            '    Select Case ex.Message

            '        Case "Some properties unavailable"

            '            Throw New Exception("This CSS file makes invalid references to unavailable properties.")

            '        Case Else

            '            Throw New Exception("The CSS file that is being applied to the WinForm is either corrupt or invalid.")

            '    End Select

            'End Try

        End Sub
        Public Sub ApplyWinForm(ByVal WinForm As Control)

            For Each p As CSSFile.CSSProperty In Properties

                If IsMatch(p, WinForm) Then ApplyProperty(WinForm, p)

            Next

            For Each c As Control In WinForm.Controls

                ApplyWinForm(c)

            Next

        End Sub

        Public Sub ApplyProperty(ByVal WinForm As Form, ByVal Prop As CSSFile.CSSProperty)

            Dim sh() As String = Nothing 'This will be used to break up shorthand values

            For Each s As String In Prop.Values.Keys

                Dim val As String = Prop.Values.Item(s).ToString

                val.Replace("  ", " ")

                Select Case s.ToLower

                    Case "padding"

                        val = ShortHand(val)
                        sh = val.Split(CChar(" "))

                        Select Case sh.Length

                            Case 1

                                WinForm.Padding = New Padding(CInt(sh(0)))

                            Case 2

                                WinForm.Padding = New Padding(CInt(sh(1)), CInt(sh(0)), CInt(sh(1)), CInt(sh(0)))

                            Case 3

                                WinForm.Padding = New Padding(CInt(sh(1)), CInt(sh(0)), CInt(sh(1)), CInt(sh(2)))

                            Case 4

                                WinForm.Padding = New Padding(CInt(sh(2)), CInt(sh(0)), CInt(sh(1)), CInt(sh(3)))

                        End Select

                    Case "font"

                        val = ShortHand(val)

                        WinForm.Font = GetFont(val)



                End Select

            Next

        End Sub
        Public Sub ApplyProperty(ByVal WinForm As Control, ByVal Prop As CSSFile.CSSProperty)

            Dim sh() As String = Nothing 'This will be used to break up shorthand values

            For Each s As String In Prop.Values.Keys

                Dim val As String = Prop.Values.Item(s).ToString

                val = val.Replace("  ", " ").Replace("	", "")
                s = s.Replace(" ", "").Replace(vbCrLf, "").Replace("  ", "").Replace("	", "")

                Select Case s.ToLower

                    Case "padding"

                        val = ShortHand(val)
                        sh = val.Split(CChar(" "))

                        Select Case sh.Length

                            Case 1

                                WinForm.Padding = New Padding(CInt(sh(0)))

                            Case 2

                                WinForm.Padding = New Padding(CInt(sh(1)), CInt(sh(0)), CInt(sh(1)), CInt(sh(0)))

                            Case 3

                                WinForm.Padding = New Padding(CInt(sh(1)), CInt(sh(0)), CInt(sh(1)), CInt(sh(2)))

                            Case 4

                                WinForm.Padding = New Padding(CInt(sh(2)), CInt(sh(0)), CInt(sh(1)), CInt(sh(3)))

                        End Select

                    Case "margin"

                        val = ShortHand(val)
                        sh = val.Split(CChar(" "))

                        Select Case sh.Length

                            Case 1

                                WinForm.Margin = New Padding(CInt(sh(0)))

                            Case 2

                                WinForm.Margin = New Padding(CInt(sh(1)), CInt(sh(0)), CInt(sh(1)), CInt(sh(0)))

                            Case 3

                                WinForm.Margin = New Padding(CInt(sh(1)), CInt(sh(0)), CInt(sh(1)), CInt(sh(2)))

                            Case 4

                                WinForm.Margin = New Padding(CInt(sh(2)), CInt(sh(0)), CInt(sh(1)), CInt(sh(3)))

                        End Select

                    Case "font"

                        If val.ToLower = "inherit" Then

                            WinForm.Font = WinForm.Parent.Font

                        Else

                            val = ShortHand(val)

                            WinForm.Font = GetFont(val)

                        End If

                    Case "margin-left"

                        WinForm.Margin = New Padding(CInt(val), WinForm.Margin.Top, WinForm.Margin.Right, WinForm.Margin.Bottom)

                    Case "margin-top"

                        WinForm.Margin = New Padding(WinForm.Margin.Left, CInt(val), WinForm.Margin.Right, WinForm.Margin.Bottom)

                    Case "margin-right"

                        WinForm.Margin = New Padding(WinForm.Margin.Left, WinForm.Margin.Top, CInt(val), WinForm.Margin.Bottom)

                    Case "margin-bottom"

                        WinForm.Margin = New Padding(WinForm.Margin.Left, WinForm.Margin.Top, WinForm.Margin.Right, CInt(val))

                    Case "padding-left"

                        WinForm.Padding = New Padding(CInt(val), WinForm.Padding.Top, WinForm.Padding.Right, WinForm.Padding.Bottom)

                    Case "padding-top"

                        WinForm.Padding = New Padding(WinForm.Padding.Left, CInt(val), WinForm.Padding.Right, WinForm.Padding.Bottom)

                    Case "padding-right"

                        WinForm.Padding = New Padding(WinForm.Padding.Left, WinForm.Padding.Top, CInt(val), WinForm.Padding.Bottom)

                    Case "padding-bottom"

                        WinForm.Padding = New Padding(WinForm.Padding.Left, WinForm.Padding.Top, WinForm.Padding.Right, CInt(val))

                    Case "color"

                        WinForm.ForeColor = GetARGB(val)

                    Case "font-family"

                        Select Case val.ToLower

                            Case "serif"

                                val = "Times New Roman"

                            Case "sans-serif"

                                val = "Veranda"

                            Case "monotype"

                                val = "Courier New"

                        End Select

                        WinForm.Font = New Font(val, WinForm.Font.Size, WinForm.Font.Style, WinForm.Font.Unit, WinForm.Font.GdiCharSet)

                    Case "font-size"

                        WinForm.Font = New Font(WinForm.Font.Name, CInt(val), WinForm.Font.Style, WinForm.Font.Unit, WinForm.Font.GdiCharSet)

                    Case "font-style"

                        Select Case val

                            Case "italic"

                                WinForm.Font = New Font(WinForm.Font, FontStyle.Italic Or WinForm.Font.Style)

                            Case "oblique"

                                WinForm.Font = New Font(WinForm.Font, FontStyle.Italic Or WinForm.Font.Style)

                            Case "none"

                                WinForm.Font = New Font(WinForm.Font, FontStyle.Regular)

                            Case "inherit"

                                If WinForm.Parent.Font.Style <> FontStyle.Regular Then
                                    WinForm.Font = New Font(WinForm.Font, WinForm.Parent.Font.Style)
                                Else

                                    If WinForm.Font.Style <> FontStyle.Regular Then

                                        WinForm.Font = New Font(WinForm.Font, Nothing)

                                    End If

                                End If
                        End Select

                    Case "font-weight"

                        If val = "lighter" OrElse val = "normal" Then Exit Select

                        WinForm.Font = New Font(WinForm.Font, FontStyle.Bold Or WinForm.Font.Style)

                    Case "background-color"

                        If val.ToLower = "inherit" Then

                            WinForm.BackColor = WinForm.Parent.BackColor

                        Else

                            WinForm.BackColor = GetARGB(val)

                        End If

                    Case "background-image"

                        Dim loc As String = Left(val.Substring(val.IndexOf("(") + 1), 1)

                        Select Case Left(val, val.IndexOf("(")).ToLower

                            Case "file"

                                loc = loc.Replace("%skinspath%", My.Application.Info.DirectoryPath & "\Skins\")
                                WinForm.BackgroundImage = Image.FromFile(loc)

                            Case "url"

                                If Left(loc, 4) <> "http" Then loc = Location & loc
                                Dim wc As New Net.WebClient
                                WinForm.BackgroundImage = Image.FromStream(wc.OpenRead(loc))
                                wc = Nothing

                            Case "this"

                                Dim res As Object = loc
                                RaiseEvent GetResource(res)
                                WinForm.BackgroundImage = CType(res, Image)
                                res = Nothing

                        End Select

                        loc = Nothing

                    Case "background-repeat"

                        Select Case val.ToLower

                            Case "no-repeat"

                                WinForm.BackgroundImageLayout = ImageLayout.None

                            Case "repeat"

                                WinForm.BackgroundImageLayout = ImageLayout.Tile

                            Case "center"

                                WinForm.BackgroundImageLayout = ImageLayout.Center

                            Case "inherit"

                                WinForm.BackgroundImageLayout = WinForm.Parent.BackgroundImageLayout

                            Case "zoom"

                                WinForm.BackgroundImageLayout = ImageLayout.Zoom

                            Case "stretch"

                                WinForm.BackgroundImageLayout = ImageLayout.Stretch

                        End Select

                    Case "text-align"

                        Try

                            If WinForm.GetType Is GetType(System.Windows.Forms.Button) OrElse
                            WinForm.GetType Is GetType(Label) OrElse
                            WinForm.GetType Is GetType(TextBox) OrElse
                            WinForm.GetType Is GetType(DividerLabel) Then

                                Select Case val.ToLower

                                    Case "left"

                                        CType(WinForm, Object).Textalign = HorizontalAlignment.Left

                                    Case "center"

                                        CType(WinForm, Object).Textalign = HorizontalAlignment.Center

                                    Case "right"

                                        CType(WinForm, Object).Textalign = HorizontalAlignment.Right

                                End Select

                            End If

                        Catch

                        End Try

                    Case "text-indent"

                        Try

                            Select Case CType(CType(WinForm, Object).Textalign, HorizontalAlignment)

                                Case HorizontalAlignment.Left

                                    WinForm.Padding = New Padding(CInt(val), WinForm.Padding.Top, WinForm.Padding.Right, WinForm.Padding.Bottom)

                                Case HorizontalAlignment.Right

                                    WinForm.Padding = New Padding(WinForm.Padding.Right, WinForm.Padding.Top, CInt(val), WinForm.Padding.Bottom)

                            End Select

                        Catch

                        End Try

                    Case "text-decoration"

                        Select Case val.ToLower

                            Case "none"

                                WinForm.Font = New Font(WinForm.Font, FontStyle.Regular)

                            Case "inherit"

                                WinForm.Font = New Font(WinForm.Font, WinForm.Parent.Font.Style)

                            Case "underline"

                                WinForm.Font = New Font(WinForm.Font, FontStyle.Underline Or WinForm.Font.Style)

                            Case "line-through"

                                WinForm.Font = New Font(WinForm.Font, FontStyle.Strikeout Or WinForm.Font.Style)

                        End Select

                    Case "text-transform"

                        Select Case val.ToLower

                            Case "uppercase"

                                WinForm.Text = WinForm.Text.ToUpper

                            Case "lowercase"

                                WinForm.Text = WinForm.Text.ToLower

                            Case "capitalize"

                                Dim ci As New Globalization.CultureInfo("en-US", False)
                                WinForm.Text = ci.TextInfo.ToTitleCase(WinForm.Text.ToLower)
                                ci = Nothing

                        End Select

                    Case "white-space"

                        Select Case val.ToLower

                            Case "normal"

                                WinForm.Text.Replace("  ", " ")

                            Case "pre"

                            Case "nowrap"

                        End Select

                    Case "image"

                        Dim loc As String = val.Substring(val.IndexOf("(") + 1, val.Length - 2 - val.IndexOf("("))

                        If WinForm.GetType.ToString <> GetType(System.Windows.Forms.Button).ToString AndAlso
                        WinForm.GetType.ToString <> GetType(TextBox).ToString AndAlso
                        WinForm.GetType.ToString <> GetType(Label).ToString AndAlso
                        WinForm.GetType.ToString <> GetType(DividerLabel).ToString AndAlso
                        WinForm.GetType.ToString <> GetType(Button).ToString _
                        Then Exit Select
                        'WinForm.GetType.ToString <> GetType(AddressBar).ToString AndAlso _

                        Dim ctm As Object = WinForm

                        Select Case WinForm.GetType.ToString

                            Case GetType(Button).ToString

                                Select Case Prop.PSSelector

                                    Case "up"

                                        CType(ctm, Button).ImageUp = GrabImage(val, loc)

                                    Case "over"

                                        CType(ctm, Button).ImageOver = GrabImage(val, loc)

                                End Select

                                'Case GetType(AddressBar).ToString

                                '    Select Case Prop.PSSelector

                                '        Case "left"

                                '            CType(ctm, AddressBar).ImageLeft = GrabImage(val, loc)

                                '        Case "middle"

                                '            CType(ctm, AddressBar).ImageMid = GrabImage(val, loc)

                                '        Case "right"

                                '            CType(ctm, AddressBar).ImageRight = GrabImage(val, loc)

                                '    End Select

                            Case Else

                                ctm.Image = GrabImage(val, loc)

                        End Select

                        loc = Nothing

                    Case "height"

                        WinForm.Size = New Size(WinForm.Width, Prop.ExpectedUnit(val, WinForm.Parent))

                    Case "width"

                        WinForm.Size = New Size(Prop.ExpectedUnit(val, WinForm.Parent), WinForm.Height)

                End Select

            Next

        End Sub

        Public Function GrabImage(ByVal val As String, ByVal loc As String) As Image

            Select Case Left(val, val.IndexOf("(")).ToLower

                Case "file"

                    Loc = Loc.Replace("%skinspath%", My.Application.Info.DirectoryPath & "\Skins\")
                    Return Image.FromFile(loc)

                Case "url"

                    If Left(loc, 4) <> "http" Then loc = Location & loc
                    Dim wc As New Net.WebClient
                    Return Image.FromStream(wc.OpenRead(loc))
                    wc = Nothing

                Case "this"

                    Dim res As Object = loc
                    RaiseEvent GetResource(res)
                    Return CType(res, Image)
                    res = Nothing

            End Select

            Return Nothing

        End Function

        Public Function GetARGB(ByVal val As String) As Color

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

        Private Function ShortHand(ByVal val As String) As String

rest:       val.Replace("  ", " ")

            If Not val.Contains(" ") Then

                val.Replace(vbCrLf, " ")
                GoTo rest

            End If

            Return val

        End Function

        Public Function GetFont(ByVal params As String) As Font

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

        Public Class CSSProperty

            Public ControlType As String
            Public ControlID As String
            Public ControlClass As String
            Public PSSelector As String
            Public ParentPath As String

            'Parent Path descriptor:
            'Parent<GrandParent<1GGrandParent<

            Public Values As New Hashtable

            Public Sub Parse(ByVal s() As String)

                ControlClass = s(0)
                ControlType = s(1)
                ControlID = s(2)
                PSSelector = s(3)
                ParentPath = s(4)

            End Sub

            Public Function ExpectedUnit(ByVal Format As String, ByVal parent As Control) As Integer

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
            Public Sub New(ByVal s() As String)

                Me.Parse(s)

            End Sub

        End Class

    End Class
    Module CSSParser

        Public PPInch As Integer = 72
        Public PPCm As Integer = 28
        Public PPMm As Integer = 3
        Public PPPt As Integer = 1
        Public PPPc As Integer = 12

        Public TALK As Boolean = False
        Public Function ParseFile(ByVal Query As String, ByVal Location As String) As CSSFile

            Dim retfile As New CSSFile
            Dim regx As New System.Text.RegularExpressions.Regex("/\*[\d\D]*?\*/")
            Dim cp As CSSFile.CSSProperty
            retfile.Location = Location

            Query = regx.Replace(Query, "") 'Removes comments

            Do Until Not Query.Contains(vbCrLf & vbCrLf)

                Query = Query.Replace(vbCrLf & vbCrLf, "")

            Loop

            For Each s As String In Query.Split(CChar("}"))

                s = s.Trim(CStr("    	" & vbCrLf).ToCharArray)

                If s = "" Then Exit For

                Do Until Left(s, 1) <> " " AndAlso Left(s, 1) <> vbCrLf AndAlso Right(s, 1) <> " " AndAlso Right(s, 1) <> vbCrLf

                    s = s.Trim(CStr(" " & vbCrLf).ToCharArray)

                Loop

                cp = New CSSFile.CSSProperty
                cp.ControlID = "<" & Left(s, s.IndexOf("{")).Trim(CStr(" " & vbCrLf).ToCharArray) & ">"

                Dim contents As String() = s.Substring(s.IndexOf("{") + 1).Split(CChar(";"))

                For Each x As String In contents

                    x = x.Trim(CStr(" " & vbCrLf).ToCharArray).Replace(";", "")

                Next

                If contents(contents.Length - 1) = "" Then Array.Clear(contents, contents.Length - 1, 1)

                If cp.ControlID.Contains(",") Then

                    Dim ww() As String = cp.ControlID.Replace(",", ">,<").Split(CChar(","))

                    For Each a As String In ww

                        cp.Parse(GetIDType(a))

                        For Each q As String In contents

                            cp.Values.Add(Left(q, CInt(IIf(q.LastIndexOf(":") < 0, 0, q.LastIndexOf(":")))), q.Substring(q.LastIndexOf(":") + 1).Trim(CChar(" ")))

                        Next

                        retfile.Properties.Add(cp)

                        cp = New CSSFile.CSSProperty

                    Next

                Else

                    cp.Parse(GetIDType(cp.ControlID))

                    For Each q As String In contents

                        If q = "" Then Exit For
                        cp.Values.Add(Left(q, CInt(IIf(q.LastIndexOf(":") < 0, 0, q.LastIndexOf(":")))), q.Substring(q.LastIndexOf(":") + 1).Trim(CChar(" ")))

                    Next

                    retfile.Properties.Add(cp)

                End If

            Next

            Return retfile

        End Function

        Public Function GetIDType(ByVal input As String) As String() '{Class, Type, ID, Pseudo-class Selector, ParentPath}

            Dim s As String() = {"", "", "", "", ""}
            Dim repchar As Char = CChar("<")

            input = input.Trim(CChar("<")) 'These remove the brackets that we put on both sides
            input = input.Trim(CChar(">")) 'of the individual selector items.

            input = input.Replace("*.", ".")
            input = input.Replace("*[", "[")
            input = input.Replace("*#", "#")

            input = input.Replace(" > ", " !")

            If input.Contains("+") Then

                Do Until Not input.Contains(" +") AndAlso Not input.Contains("+ ")

                    input = input.Replace(" +", "+")
                    input = input.Replace("+ ", "+")

                Loop

                input = input.Replace("+", " +")

            End If

            If input.Length > 1 AndAlso input.Contains("*") Then

                input = input.Replace("*", "")

            End If

            'If input.Contains(" ") Then

            'This could mean a few things.
            '
            ' - We are using CSS2 or CSS3
            ' - We have funny ID declarations

            'This line basically removes a phenomenon such as 'p#id' and replaces
            'it with 'p #id'

            If input.Contains("#") Then input = input.Replace("#", " #").TrimStart(CChar(" ")).Replace("  ", " ")

            For Each d As String In input.Split(CChar(" ")) 'This breaks it up into word chunks

                If Left(d, 1) = "!" Then

                    repchar = CChar("!")
                    d = d.Substring(1)

                ElseIf Left(d, 1) = "+" Then

                    repchar = CChar("+")
                    d = d.Substring(1)

                End If

                If s(3) <> "" AndAlso (s(1) <> "" OrElse s(0) <> "") Then 'This means that it is one of those crazy pseudo-class
                    'that will just inherit things like first-child

                    If s(0) <> "" OrElse s(1) <> "" Then 'This means that there is no error.
                        'It also helps to set up the inheritance/ParentPath to the object
                        'in the proper order.

                        If s(1) = "" Then 'This is simple. The Parent has no type, so
                            'we just write the class

                            s(4) = "." & s(0).Replace(" ", ".") & s(3) & repchar & s(4)

                        Else 'This is where the hard part comes in. We need to be able
                            'to tell whether to write it as a type or as a type.class

                            If s(0) = "" Then 'Here's it with just a type

                                s(4) = s(1) & s(3) & repchar & s(4)

                            Else 'This is type.class

                                s(4) = s(1) & "." & s(0) & s(3) & repchar & s(4)

                            End If

                        End If

                        s(0) = ""
                        s(1) = ""

                    End If

                    If d.Contains(".") Then

                        s(0) = d.Substring(d.IndexOf(".") + 1).Replace(".", " ")
                        s(1) = Left(d, d.IndexOf("."))

                    Else

                        s(1) = d

                    End If

                    GoTo no

                End If

                If d.Contains("#") Then 'This is an ID

                    s(2) = d.Substring(d.IndexOf("#") + 1)
                    GoTo no

                End If

                If d.Contains(":") Then 'This introduces a pseudo-class

                    s(3) = d.Substring(d.IndexOf(":") + 1)
                    d = Left(d, d.IndexOf(":"))

                End If 'This will not terminate the current word-block, because it still has to
                'interpret the inherited data.

                If s(0) <> "" OrElse s(1) <> "" Then 'This means it threw us for a loop and has
                    'some kind of inheritence.

                    If s(1) = "" Then 'This is simple. The Parent has no type, so
                        'we just write the class

                        s(4) = "." & s(0).Replace(" ", ".") & repchar & s(4)

                    Else 'This is where the hard part comes in. We need to be able
                        'to tell whether to write it as a type or as a type.class

                        If s(0) = "" Then 'Here's it with just a type

                            s(4) = s(1) & repchar & s(4)

                        Else 'This is type.class

                            s(4) = s(1) & "." & s(0).Replace(" ", ".") & repchar & s(4)

                        End If

                    End If

                    s(0) = ""
                    s(1) = ""

                End If

                If d.Contains(".") Then 'This is *most likely* a standard 'type.class' statement

                    s(1) = Left(d, d.IndexOf("."))
                    s(0) = d.Substring(d.IndexOf(".") + 1).Replace(".", " ") 'This line is
                    'funny. CSS2 calls for multiple classes, but in the stylesheets, they are
                    'defined by spaces. The detection code will have to be:
                    'if Object.class.contains(css.class) then...

                Else 'This could just be a standard type statement.

                    s(1) = d

                End If

no:             repchar = CChar("<")

            Next

            'End If

            Return s

        End Function

        Public Function IsDescendent(ByVal pan As CSSFile.CSSProperty, ByVal pdes As Control) As Control

            If pdes.Parent Is Nothing Then Return Nothing
            If IsMatch(pan, pdes.Parent) Then Return pdes.Parent
            Dim x As Control = IsDescendent(pan, pdes.Parent)
            If x IsNot Nothing Then Return x
            Return Nothing

        End Function

        Public Function IsControlAfter(ByVal pbef As Control, ByVal paf As Control) As Boolean

            If pbef.Parent Is Nothing OrElse paf.Parent Is Nothing OrElse Not pbef.Parent.Controls.Contains(paf) Then Return False
            If pbef.Parent.Controls.Item(pbef.Parent.Controls.IndexOf(pbef) + 1) Is paf Then Return True

            Return False

        End Function
        Public Function IsControlAfter(ByVal pbef As Control, ByVal paf As Control, ByVal Loc As Integer) As Boolean

            If pbef.Parent Is Nothing OrElse paf.Parent Is Nothing OrElse Not pbef.Parent.Controls.Contains(paf) Then Return False
            If pbef.Parent.Controls.Item(pbef.Parent.Controls.IndexOf(pbef) + Loc) Is paf Then Return True

            Return False

        End Function
        Public Function IsControlAfter(ByVal pbef As Control, ByVal prop As CSSFile.CSSProperty, ByVal Loc As Integer) As Boolean

            If pbef Is Nothing OrElse pbef.Parent Is Nothing OrElse prop Is Nothing Then Return False
            If IsMatch(prop, pbef.Parent.Controls.Item(pbef.Parent.Controls.IndexOf(pbef) + Loc)) Then Return True

            Return False

        End Function

        Public Sub Populate(ByVal Inch As Integer, ByVal Cm As Integer, ByVal MM As Integer, ByVal Pc As Integer, ByVal Pt As Integer)

            PPInch = Inch
            PPCm = Cm
            PPMm = MM
            PPPc = Pc
            PPPt = Pt

        End Sub

        Public Function IsMatch(ByVal Prop As CSSFile.CSSProperty, ByVal query As Control) As Boolean

            If Prop.ControlID <> "" AndAlso query.Name <> Prop.ControlID Then Return False
            If Prop.ControlClass <> "" AndAlso Not CStr(" " & query.Tag.ToString & " ").Contains(" " & Prop.ControlClass & " ") Then Return False

            If Prop.ControlType <> "" AndAlso Prop.ControlType <> "*" Then

                Select Case query.GetType.ToString

                    Case GetType(Button).ToString

                        If Prop.ControlType <> "button" Then Return False

                    Case GetType(System.Windows.Forms.Button).ToString

                        If Prop.ControlType <> "button" Then Return False

                    Case GetType(Label).ToString

                        If Prop.ControlType <> "text" OrElse Prop.ControlType <> "h1" OrElse Prop.ControlType <> "h2" OrElse Prop.ControlType <> "h3" OrElse Prop.ControlType <> "h4" OrElse Prop.ControlType <> "h5" OrElse Prop.ControlType <> "h6" Then Return False

                    Case GetType(PictureBox).ToString

                        If Prop.ControlType <> "image" Then Return False

                    Case GetType(TextBox).ToString

                        If Prop.ControlType <> "textbox" Then Return False

                    Case GetType(LinkLabel).ToString

                        If Prop.ControlType <> "link" Then Return False

                    Case GetType(Pane).ToString

                        If Prop.ControlType <> "pane" Then Return False

                    Case GetType(ComboBox).ToString

                        If Prop.ControlType <> "dropdown" Then Return False

                    Case GetType(CheckBox).ToString

                        If Prop.ControlType <> "checkbox" Then Return False

                    Case GetType(DividerLabel).ToString

                        If Prop.ControlType <> "divider" Then Return False

                    Case Else

                        If Prop.ControlType <> query.GetType.ToString.Replace("_", ".") Then

                            Return False

                        End If

                End Select

            End If

            If Prop.ParentPath <> "" Then

                Dim wa As String = Prop.ParentPath
                Dim wail As Integer

                While True

                    If wa.Length = 0 Then Exit While
                    If Not wa.Contains("!") AndAlso Not wa.Contains("<") AndAlso Not wa.Contains("+") Then

                        Exit While

                    End If

                    wail = LeastIndex("!+<".ToCharArray, wa)
                    Dim tempdesc As Object
                    Dim current As Object = query

                    Select Case wa.Substring(wail, 1)

                        Case "!"

                            If current.GetType Is GetType(Form) OrElse CType(current, Control).Parent Is Nothing OrElse Not IsMatch(New CSSFile.CSSProperty(GetIDType(Left(wa, wail))), CType(current, Control).Parent) Then

                                Return False

                            End If

                            current = CType(current, Control).Parent

                        Case "<"

                            If current.GetType Is GetType(Form) Then Return False

                            tempdesc = IsDescendent(New CSSFile.CSSProperty(GetIDType(Left(wa, wail))), CType(current, Control))

                            If CType(current, Control).Parent Is Nothing OrElse tempdesc Is Nothing Then

                                Return False

                            End If

                            current = tempdesc

                        Case "+"

                            If current.GetType Is GetType(Form) OrElse CType(current, Control) Is Nothing OrElse Not IsControlAfter(query.Parent, New CSSFile.CSSProperty(GetIDType(Left(wa, wail))), 1) Then

                                Return False

                            End If

                    End Select

                    wa = wa.Substring(wail + 1)

                End While

            End If

            Return True

        End Function
        Public Function IsMatch(ByVal Prop As CSSFile.CSSProperty, ByVal query As Form) As Boolean

            If Prop.ControlID <> "" AndAlso query.Name <> Prop.ControlID Then Return False
            If Prop.ControlClass <> "" AndAlso Not CStr(" " & query.Tag.ToString & " ").Contains(" " & Prop.ControlClass & " ") Then Return False

            If Prop.ControlType <> "" AndAlso Prop.ControlType <> "*" Then

                Select Case query.GetType.ToString

                    Case GetType(Button).ToString

                        If Prop.ControlType <> "button" Then Return False

                    Case GetType(Label).ToString

                        If Prop.ControlType <> "text" OrElse Prop.ControlType <> "h1" OrElse Prop.ControlType <> "h2" OrElse Prop.ControlType <> "h3" OrElse Prop.ControlType <> "h4" OrElse Prop.ControlType <> "h5" OrElse Prop.ControlType <> "h6" Then Return False

                    Case GetType(PictureBox).ToString

                        If Prop.ControlType <> "image" Then Return False

                    Case GetType(TextBox).ToString

                        If Prop.ControlType <> "textbox" Then Return False

                    Case GetType(LinkLabel).ToString

                        If Prop.ControlType <> "link" Then Return False

                    Case GetType(Pane).ToString

                        If Prop.ControlType <> "pane" Then Return False

                    Case GetType(ComboBox).ToString

                        If Prop.ControlType <> "dropdown" Then Return False

                    Case GetType(CheckBox).ToString

                        If Prop.ControlType <> "checkbox" Then Return False

                    Case GetType(DividerLabel).ToString

                        If Prop.ControlType <> "divider" Then Return False

                    Case Else

                        If Prop.ControlType <> query.GetType.ToString.Replace("_", ".") Then

                            Return False

                        End If

                End Select

            End If

            Dim wa As String = Prop.ParentPath
            Dim wail As Integer

            While True

                If wa.Length = 0 Then Exit While
                If Not wa.Contains("!") AndAlso Not wa.Contains("<") AndAlso Not wa.Contains("+") Then

                    Exit While

                End If

                wail = LeastIndex("!+<".ToCharArray, wa)
                Dim tempdesc As Object
                Dim current As Object = query

                Select Case wa.Substring(wail, 1)

                    Case "!"

                        If current.GetType Is GetType(Form) OrElse CType(current, Control).Parent Is Nothing OrElse Not IsMatch(New CSSFile.CSSProperty(GetIDType(Left(wa, wail))), CType(current, Control).Parent) Then

                            Return False

                        End If

                        current = CType(current, Control).Parent

                    Case "<"

                        If current.GetType Is GetType(Form) Then Return False

                        tempdesc = IsDescendent(New CSSFile.CSSProperty(GetIDType(Left(wa, wail))), CType(current, Control))

                        If CType(current, Control).Parent Is Nothing OrElse tempdesc Is Nothing Then

                            Return False

                        End If

                        current = tempdesc

                    Case "+"

                        If current.GetType Is GetType(Form) OrElse CType(current, Control) Is Nothing OrElse Not IsControlAfter(query.Parent, New CSSFile.CSSProperty(GetIDType(Left(wa, wail))), 1) Then

                            Return False

                        End If

                End Select

                wa = wa.Substring(wail + 1)

            End While

            Return True

        End Function

        Private Function LeastIndex(ByVal pars() As Char, ByVal query As String) As Integer

            Dim least As Integer = query.Length

            For Each c As Char In pars

                If Not query.Contains(c) Then GoTo nxt
                If query.IndexOf(c) < least AndAlso query.IndexOf(c) <> -1 Then least = query.IndexOf(c)

nxt:        Next

            If least = query.Length Then least = -1

        End Function

    End Module
End Namespace