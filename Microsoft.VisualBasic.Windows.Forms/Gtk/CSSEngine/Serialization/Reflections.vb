Imports Microsoft.VisualBasic.Serialization

Namespace Gtk.CSSEngine.Serialization

    Public MustInherit Class Attribute : Inherits System.Attribute

        Public ReadOnly Property Name As String

        Sub New(name As String)
            Me.Name = name
        End Sub

        Public Overrides Function ToString() As String
            Return String.Format("{ ""name"" = ""{0}"" }", Name)
        End Function
    End Class

    <AttributeUsage(AttributeTargets.Class, AllowMultiple:=False, Inherited:=True)>
    Public Class GtkObject : Inherits Attribute

        Public Property Parent As String = ""
        Public Property ControlType As String = ""

        Sub New(name As String)
            Call MyBase.New(name)
        End Sub
    End Class

    <AttributeUsage(AttributeTargets.Property, AllowMultiple:=False, Inherited:=True)>
    Public Class [Property] : Inherits Attribute

        Sub New(name As String)
            Call MyBase.New(name)
        End Sub
    End Class
End Namespace