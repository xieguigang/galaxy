Imports System.Net
Imports Microsoft.VisualBasic.Windows.Forms.Gtk.CSSEngine.Components

Namespace Gtk.CSSEngine

    Public Interface ICSSEngine

        Event GetResource(ByRef item As Object)
    End Interface

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks>
    ''' Source code downloads from
    ''' https://sourceforge.net/projects/vbcssparser/?source=navbar
    ''' </remarks>
    Public Class CSSFile : Implements ICSSEngine

        Public Property Properties As New List(Of CSSProperty)
        Public Property Location As String
        Public Event GetResource(ByRef item As Object) Implements ICSSEngine.GetResource

        ''' <summary>
        ''' Load image resource from the location that this css file point to. The location it maybe a network location or local file.
        ''' </summary>
        ''' <param name="val"></param>
        ''' <param name="loc"></param>
        ''' <returns></returns>
        Public Function GrabImage(val As String, loc As String) As Image

            Select Case Left(val, val.IndexOf("(")).ToLower

                Case "file"

                    loc = loc.Replace("%skinspath%", My.Application.Info.DirectoryPath & "\Skins\")
                    Return Image.FromFile(loc)

                Case "url"

                    If Left(loc, 4) <> "http" Then loc = Location & loc
                    Dim wc As New WebClient
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
    End Class
End Namespace