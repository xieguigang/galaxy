Imports System.Reflection
Imports System.Xml.Serialization
Imports Microsoft.VisualBasic.Serialization
Imports Microsoft.VisualBasic.Windows.Forms.PlugIns.Attributes

Namespace PlugIns

    ''' <summary>
    ''' 
    ''' </summary>
    <AttributeUsage(AttributeTargets.Method, AllowMultiple:=False, Inherited:=True)>
    Public Class EntryFlag

        <XmlAttribute> Public ReadOnly Property EntryType As EntryTypes

        Friend Target As MethodInfo
        Friend GetIconInvoke As Func(Of String, Object)

        Sub New(attr As Attributes.EntryFlag, method As MethodInfo)
            EntryType = attr.Type
            Target = method
        End Sub

        Public ReadOnly Property GetIcon(Name As String) As Image
            Get
                If Not Target Is Nothing Then
                    Return ReflectionAPI.Invoke(New Object() {Name}, Target)
                Else
                    If GetIconInvoke Is Nothing Then
                        Return Nothing
                    Else
                        Return GetIconInvoke(Name)
                    End If
                End If
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return EntryType.ToString
        End Function
    End Class
End Namespace