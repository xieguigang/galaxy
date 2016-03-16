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
                        Select x
                        Group x By x.ControlClass Into Group)
            Properties = objs.ToArray(Function(x) New GtkObject(x.ControlClass, x.Group.ToArray)).ToDictionary
            Location = css.Location
            DefineColors = css.DefineColors
        End Sub

        Public Function FindObject(x As Serialization.GtkObject) As GtkProperty
            Dim name As String = x.Name

            If Not Properties.ContainsKey(name) Then
                If name.IndexOf("."c) = 0 Then
                    name = Mid(name, 2).Trim
                End If
                If Not Properties.ContainsKey(name) Then
                    Return Nothing
                End If
            End If

            If x.Parent Is Nothing Then
                x.Parent = ""
            End If

            Dim source As GtkObject = Properties(name)

            If source.Properties.ContainsKey(x.Parent) Then
                Dim props As CSSProperty() = source.Properties(x.Parent)
                Return New GtkProperty(name, x.Parent, props)
            End If

            Return Nothing
        End Function

        Public Overrides Function ToString() As String
            Return Me.GetJson
        End Function
    End Class
End Namespace