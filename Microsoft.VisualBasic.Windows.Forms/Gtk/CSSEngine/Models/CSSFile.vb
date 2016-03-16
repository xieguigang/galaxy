Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Serialization

Namespace Gtk.CSSEngine.Models

    Public Class CSSFile : Implements ICSSEngine

        Public Property Properties As New Dictionary(Of String, GtkObject)
        Public Property Location As String
        Public Property DefineColors As Dictionary(Of String, String)

        Public Event GetResource(ByRef item As Object) Implements ICSSEngine.GetResource

        Sub New(css As Gtk.CSSEngine.CSSFile)
            Dim objs = (From x As CSSProperty
                        In css.Properties
                        Let name As String = __getName(x)
                        Select x, name
                        Group By name Into Group)
            Properties = objs.ToArray(Function(x) New GtkObject(x.name, x.Group.ToArray(Function(o) o.x))).ToDictionary
            Location = css.Location
            DefineColors = css.DefineColors
        End Sub

        Private Shared Function __getName(x As CSSProperty) As String
            Dim name As String = x.ControlClass.Split.FirstOrDefault
            If name Is Nothing Then
                Return "*"
            Else
                Return name
            End If
        End Function

        Public Function FindObject(name As String) As GtkObject
            If Not Properties.ContainsKey(name) Then
                If name.IndexOf("."c) = 0 Then
                    name = Mid(name, 2).Trim
                End If
                If Not Properties.ContainsKey(name) Then
                    Return Nothing
                End If
            End If

            Return Properties(name)
        End Function

        Public Overrides Function ToString() As String
            Return Me.GetJson
        End Function
    End Class
End Namespace