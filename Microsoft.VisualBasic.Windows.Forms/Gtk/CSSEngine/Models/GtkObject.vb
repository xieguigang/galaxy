Imports Microsoft.VisualBasic.ComponentModel.Collection.Generic
Imports Microsoft.VisualBasic.Serialization
Imports Microsoft.VisualBasic.Serialization.JSON

Namespace Gtk.CSSEngine.Models

    Public Class GtkObject : Implements sIdEnumerable

        Public Property Identifier As String Implements sIdEnumerable.Identifier
        Public Property Properties As Dictionary(Of String, CSSProperty())

        Sub New(name As String, data As CSSProperty())
            Identifier = name
            Properties = (From x As CSSProperty
                          In data
                          Select x
                          Group x By x.ParentPath Into Group) _
                               .ToDictionary(Function(x) x.ParentPath,
                                             Function(x) x.Group.ToArray)
        End Sub

        Public Overrides Function ToString() As String
            Return Identifier
        End Function
    End Class

    Public Class GtkProperty : Implements sIdEnumerable

        Public Property Identifier As String Implements sIdEnumerable.Identifier
        Public Property Parent As String
        Public Property Controls As Dictionary(Of String, CSSProperty())

        Sub New(name As String, parent As String, source As CSSProperty())
            Me.Identifier = name
            Me.Parent = parent
            Me.Controls = (From x As CSSProperty
                           In source
                           Select x
                           Group x By x.ControlType Into Group) _
                                .ToDictionary(Function(x) x.ControlType,
                                              Function(x) x.Group.ToArray)
        End Sub

        ''' <summary>
        ''' ControlType
        ''' </summary>
        ''' <param name="ctrl"></param>
        ''' <returns></returns>
        Public Function GetSkin(ctrl As String) As StatusEnumerator
            If Not Controls.ContainsKey(ctrl) Then
                Return Nothing
            Else
                Return New StatusEnumerator(Controls(ctrl))
            End If
        End Function

        Public Overrides Function ToString() As String
            Return Me.GetJson
        End Function
    End Class

    Public Class StatusEnumerator

        Public ReadOnly Property stats As Dictionary(Of String, CSSProperty())

        Sub New(data As CSSProperty())
            stats = (From x As CSSProperty
                     In data
                     Select x
                     Group x By x.PSSelector Into Group) _
                          .ToDictionary(Function(x) x.PSSelector,
                                        Function(x) x.Group.ToArray)
        End Sub
    End Class
End Namespace