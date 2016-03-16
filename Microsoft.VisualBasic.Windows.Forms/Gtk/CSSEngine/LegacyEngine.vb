Imports System.Net
Imports Microsoft.VisualBasic.Windows.Forms.Gtk.CSSEngine.Components

Namespace Gtk.CSSEngine

    ''' <summary>
    ''' https://sourceforge.net/projects/vbcssparser/?source=navbar
    ''' </summary>
    Public Class LegacyEngine : Implements ICSSEngine

        Public ReadOnly Property CSS As CSSFile

        Public ReadOnly Property Properties As List(Of CSSProperty)
            Get
                Return CSS.Properties
            End Get
        End Property

        Public ReadOnly Property Location As String
            Get
                Return CSS.Location
            End Get
        End Property

        Sub New(css As CSSFile)
            Me.CSS = css
        End Sub

        Public Sub ApplyWinForm(WinForm As Form)
            Try
                For Each p As CSSProperty In Properties
                    If IsMatch(p, WinForm) Then ApplyProperty(WinForm, p)
                Next

                For Each c As Control In WinForm.Controls
                    ApplyWinForm(c)
                Next
            Catch ex As Exception
                Select Case ex.Message
                    Case "Some properties unavailable"
                        Throw New Exception("This CSS file makes invalid references to unavailable properties.", ex)

                    Case Else
                        Throw New Exception("The CSS file that is being applied to the WinForm is either corrupt or invalid.", ex)

                End Select
            End Try
        End Sub

        Public Sub ApplyWinForm(WinForm As Control)
            For Each p As CSSProperty In Properties
                If IsMatch(p, WinForm) Then ApplyProperty(WinForm, p)
            Next

            For Each c As Control In WinForm.Controls
                ApplyWinForm(c)
            Next
        End Sub

        Public Sub ApplyProperty(WinForm As Form, Prop As CSSProperty)
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

        Public Event GetResource(ByRef item As Object) Implements ICSSEngine.GetResource

        Public Sub ApplyProperty(WinForm As Control, Prop As CSSProperty)
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
                                Dim wc As New WebClient
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

                                        CType(ctm, Button).ImageUp = CSS.GrabImage(val, loc)

                                    Case "over"

                                        CType(ctm, Button).ImageOver = CSS.GrabImage(val, loc)

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

                                ctm.Image = CSS.GrabImage(val, loc)

                        End Select

                        loc = Nothing

                    Case "height"

                        WinForm.Size = New Size(WinForm.Width, Prop.ExpectedUnit(val, WinForm.Parent))

                    Case "width"

                        WinForm.Size = New Size(Prop.ExpectedUnit(val, WinForm.Parent), WinForm.Height)

                End Select

            Next
        End Sub
    End Class
End Namespace