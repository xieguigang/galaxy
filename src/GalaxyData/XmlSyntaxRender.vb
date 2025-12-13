Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Xml

Public Class XmlSyntaxRender

    Dim WithEvents richXML As RichTextBox
    Dim doc As New XmlDocument()

    Sub New(richXML As RichTextBox, xmlfile As String)
        Me.doc.Load(xmlfile)
        Me.richXML = richXML
        Me.HighlightXML(doc.OuterXml)
    End Sub

    ''' <summary>
    ''' Pretty prints XML and applies syntax highlighting in RichTextBox
    ''' </summary>
    Private Sub HighlightXML(outerXml As String)
        richXML.Clear()
        Try
            Dim doc = New XmlDocument()
            doc.LoadXml(outerXml)

            ' Format XML nicely with indentation
            Dim sb = New StringBuilder()
            Using sw = New StringWriter(sb)
                Using xw = New XmlTextWriter(sw)
                    xw.Formatting = Formatting.Indented
                    xw.Indentation = 4
                    doc.Save(xw)
                End Using
            End Using
            richXML.Text = sb.ToString()
        Catch __unusedException1__ As Exception
            ' If formatting fails, show raw XML
            richXML.Text = outerXml
        End Try

        ' Highlight tags, attributes, and values
        Colorize("<.*?>", Color.Cyan)        ' Tags
        Colorize(""".*?""", Color.Orange)    ' Values in quotes
        Colorize(" [a-zA-Z0-9_:-]+=", Color.Violet) ' Attributes
    End Sub

    ''' <summary>
    ''' Applies regex-based syntax highlighting
    ''' </summary>
    Private Sub Colorize(pattern As String, color As Color)
        Dim matches = Regex.Matches(richXML.Text, pattern)
        For Each match As Match In matches
            richXML.Select(match.Index, match.Length)
            richXML.SelectionColor = color
        Next
        ' Reset cursor position
        richXML.Select(0, 0)
    End Sub
End Class
