Imports System.Runtime.CompilerServices
Imports System.Text.RegularExpressions
Imports Microsoft.VisualBasic.Windows.Forms.Gtk.CSSEngine.Components

Namespace Gtk.CSSEngine

    Module CSSParser

        Public PPInch As Integer = 72
        Public PPCm As Integer = 28
        Public PPMm As Integer = 3
        Public PPPt As Integer = 1
        Public PPPc As Integer = 12

        Public TALK As Boolean = False

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="path"></param>
        ''' <param name="urlBase">所引用的资源的网络位置，即一个网站的url</param>
        ''' <returns></returns>
        <Extension>
        Public Function ParseDoc(path As String, Optional urlBase As String = "") As CSSFile
            Return FileIO.FileSystem.ReadAllText(path).ParseFile(urlBase)
        End Function

        <Extension>
        Public Function LoadModel(path As String, Optional urlBase As String = "") As Models.CSSFile
            Return New Models.CSSFile(path.ParseDoc(urlBase))
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Query">CSS file content.</param>
        ''' <param name="urlBase"></param>
        ''' <returns></returns>
        <Extension>
        Public Function ParseFile(Query As String, urlBase As String) As CSSFile
            Dim retfile As New CSSFile With {.Location = urlBase}
            Dim regx As New Regex("/\*[\d\D]*?\*/")

            Query = regx.Replace(Query, "") 'Removes comments
            Query = Query.TrimVBCrLf

            Dim source As String() = Query.Split(CChar("}"))

            For Each s As String In source
                Call PropertyParser(s, retfile)
            Next

            Return retfile
        End Function

        Public Function DefineColorParser(value As String, ByRef css As CSSFile) As Boolean
            Dim tokens As String() = value.Trim.Split(";"c)

            If css.DefineColors Is Nothing Then
                css.DefineColors = New Dictionary(Of String, String)
            End If

            For Each s As String In tokens
                If Not String.IsNullOrEmpty(s) Then
                    Call __defineParser(s, css.DefineColors)
                End If
            Next

            Return True
        End Function

        Private Sub __defineParser(s As String, ByRef hash As Dictionary(Of String, String))
            Dim tokens As String() = s.Split
            Dim key As String = tokens(1)
            Dim value As String = Mid(s, InStr(s, key) + key.Length + 1).Trim

            If hash.ContainsKey(key) Then
                Call hash.Remove(key)
            End If
            Call hash.Add(key, value)
        End Sub

        Public Function PropertyParser(s As String, ByRef css As CSSFile) As Boolean
            s = s.Trim(CStr("    	" & vbCrLf).ToCharArray)

            If s = "" Then Return False

            Do Until Left(s, 1) <> " " AndAlso Left(s, 1) <> vbCrLf AndAlso Right(s, 1) <> " " AndAlso Right(s, 1) <> vbCrLf
                s = s.Trim(CStr(" " & vbCrLf).ToCharArray)
            Loop

            Dim cp As CSSProperty = New CSSProperty
            Dim i As Integer = s.IndexOf("{")

            If i < 0 Then      ' @define
                Return DefineColorParser(s, css)
            End If

            cp.ControlID = "<" & Left(s, i).Trim(CStr(" " & vbCrLf).ToCharArray) & ">"

            Dim contents As String() = s.Substring(s.IndexOf("{") + 1).Split(CChar(";"))

            For Each x As String In contents
                x = x.Trim(CStr(" " & vbCrLf).ToCharArray).Replace(";", "")
            Next

            If contents(contents.Length - 1) = "" Then Array.Clear(contents, contents.Length - 1, 1)

            If cp.ControlID.Contains(",") Then
                Dim ww() As String = cp.ControlID.Replace(",", ">,<").Split(CChar(","))

                For Each a As String In ww
                    Call cp.Parse(GetIDType(a))
                    Call __valueParser(cp, contents)
                    Call css.Properties.Add(cp)

                    cp = New CSSProperty
                Next
            Else
                Call cp.Parse(GetIDType(cp.ControlID))
                Call __valueParser(cp, contents)
                Call css.Properties.Add(cp)
            End If

            Return True
        End Function

        Private Sub __valueParser(ByRef cp As CSSProperty, contents As String())
            For Each q As String In contents
                If String.IsNullOrEmpty(q) Then
                    Continue For
                End If

                Dim key As String = Left(q, CInt(IIf(q.LastIndexOf(":") < 0, 0, q.LastIndexOf(":")))).Trim
                Dim value As String = q.Substring(q.LastIndexOf(":") + 1).Trim(CChar(" "))

                If cp.Values.ContainsKey(key) Then
                    Call cp.Values.Remove(key)
                End If
                Call cp.Values.Add(key, value)
            Next
        End Sub

        Public Function GetIDType(input As String) As String() '{Class, Type, ID, Pseudo-class Selector, ParentPath}
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

        Public Function IsDescendent(pan As CSSProperty, pdes As Control) As Control
            If pdes.Parent Is Nothing Then Return Nothing
            If IsMatch(pan, pdes.Parent) Then Return pdes.Parent
            Dim x As Control = IsDescendent(pan, pdes.Parent)
            If x IsNot Nothing Then Return x

            Return Nothing
        End Function

        Public Function IsControlAfter(pbef As Control, paf As Control) As Boolean
            If pbef.Parent Is Nothing OrElse paf.Parent Is Nothing OrElse Not pbef.Parent.Controls.Contains(paf) Then Return False
            If pbef.Parent.Controls.Item(pbef.Parent.Controls.IndexOf(pbef) + 1) Is paf Then Return True

            Return False
        End Function

        Public Function IsControlAfter(pbef As Control, paf As Control, Loc As Integer) As Boolean
            If pbef.Parent Is Nothing OrElse paf.Parent Is Nothing OrElse Not pbef.Parent.Controls.Contains(paf) Then Return False
            If pbef.Parent.Controls.Item(pbef.Parent.Controls.IndexOf(pbef) + Loc) Is paf Then Return True

            Return False
        End Function

        Public Function IsControlAfter(pbef As Control, prop As CSSProperty, Loc As Integer) As Boolean
            If pbef Is Nothing OrElse pbef.Parent Is Nothing OrElse prop Is Nothing Then Return False
            If IsMatch(prop, pbef.Parent.Controls.Item(pbef.Parent.Controls.IndexOf(pbef) + Loc)) Then Return True

            Return False
        End Function

        Public Sub Populate(Inch As Integer, Cm As Integer, MM As Integer, Pc As Integer, Pt As Integer)
            PPInch = Inch
            PPCm = Cm
            PPMm = MM
            PPPc = Pc
            PPPt = Pt
        End Sub

        Public Function IsMatch(Prop As CSSProperty, query As Control) As Boolean
            If String.IsNullOrEmpty(query.Tag) Then Return False
            If Prop.ControlID <> "" AndAlso query.Name <> Prop.ControlID Then Return False
            If Prop.ControlClass <> "" AndAlso Not CStr(" " & query.Tag.ToString & " ").Contains(" " & Prop.ControlClass & " ") Then Return False

            If Prop.ControlType <> "" AndAlso Prop.ControlType <> "*" Then

                Select Case query.GetType.ToString

                    Case GetType(Components.Button).ToString

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

                            If current.GetType Is GetType(System.Windows.Forms.Form) OrElse CType(current, Control).Parent Is Nothing OrElse Not IsMatch(New CSSProperty(GetIDType(Left(wa, wail))), CType(current, Control).Parent) Then

                                Return False

                            End If

                            current = CType(current, Control).Parent

                        Case "<"

                            If current.GetType Is GetType(System.Windows.Forms.Form) Then Return False

                            tempdesc = IsDescendent(New CSSProperty(GetIDType(Left(wa, wail))), CType(current, Control))

                            If CType(current, Control).Parent Is Nothing OrElse tempdesc Is Nothing Then

                                Return False

                            End If

                            current = tempdesc

                        Case "+"

                            If current.GetType Is GetType(System.Windows.Forms.Form) OrElse CType(current, Control) Is Nothing OrElse Not IsControlAfter(query.Parent, New CSSProperty(GetIDType(Left(wa, wail))), 1) Then

                                Return False

                            End If

                    End Select

                    wa = wa.Substring(wail + 1)

                End While

            End If

            Return True
        End Function

        Public Function IsMatch(Prop As CSSProperty, query As Form) As Boolean
            If String.IsNullOrEmpty(query.Tag) Then Return False

            If Prop.ControlID <> "" AndAlso query.Name <> Prop.ControlID Then Return False
            If Prop.ControlClass <> "" AndAlso Not CStr(" " & query.Tag.ToString & " ").Contains(" " & Prop.ControlClass & " ") Then Return False

            If Prop.ControlType <> "" AndAlso Prop.ControlType <> "*" Then

                Select Case query.GetType.ToString

                    Case GetType(Components.Button).ToString

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

                        If current.GetType Is GetType(System.Windows.Forms.Form) OrElse CType(current, Control).Parent Is Nothing OrElse Not IsMatch(New CSSProperty(GetIDType(Left(wa, wail))), CType(current, Control).Parent) Then

                            Return False

                        End If

                        current = CType(current, Control).Parent

                    Case "<"

                        If current.GetType Is GetType(System.Windows.Forms.Form) Then Return False

                        tempdesc = IsDescendent(New CSSProperty(GetIDType(Left(wa, wail))), CType(current, Control))

                        If CType(current, Control).Parent Is Nothing OrElse tempdesc Is Nothing Then

                            Return False

                        End If

                        current = tempdesc

                    Case "+"

                        If current.GetType Is GetType(System.Windows.Forms.Form) OrElse CType(current, Control) Is Nothing OrElse Not IsControlAfter(query.Parent, New CSSProperty(GetIDType(Left(wa, wail))), 1) Then

                            Return False

                        End If

                End Select

                wa = wa.Substring(wail + 1)

            End While

            Return True

        End Function

        Private Function LeastIndex(pars() As Char, query As String) As Integer
            Dim least As Integer = query.Length

            For Each c As Char In pars

                If Not query.Contains(c) Then GoTo nxt
                If query.IndexOf(c) < least AndAlso query.IndexOf(c) <> -1 Then least = query.IndexOf(c)

nxt:        Next

            If least = query.Length Then least = -1

            Return least
        End Function
    End Module
End Namespace