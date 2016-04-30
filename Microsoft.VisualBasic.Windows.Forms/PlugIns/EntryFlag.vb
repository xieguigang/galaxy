Imports System.Reflection
Imports System.Xml.Serialization
Imports Microsoft.VisualBasic.Serialization
Imports Microsoft.VisualBasic.Windows.Forms.PlugIns.Attributes
Imports Microsoft.VisualBasic.SoftwareToolkits

Namespace PlugIns

    ''' <summary>
    ''' 
    ''' </summary>
    <AttributeUsage(AttributeTargets.Method, AllowMultiple:=False, Inherited:=True)>
    Public Class EntryFlag

        <XmlAttribute> Public ReadOnly Property EntryType As EntryTypes

        Public ReadOnly Property Target As MethodInfo
        Public ReadOnly Property resMgr As Resources

        Sub New(attr As Attributes.EntryFlag, method As MethodInfo)
            EntryType = attr.Type
            Target = method
        End Sub

        Sub New(attr As EntryFlag, resMgr As Resources)
            Me.EntryType = attr.EntryType
            Me.resMgr = resMgr
        End Sub

        Public ReadOnly Property GetIcon(Name As String) As Image
            Get
                If Not Target Is Nothing Then
                    Return ReflectionAPI.Invoke(New Object() {Name}, Target)
                Else
                    If resMgr Is Nothing Then
                        Return Nothing
                    Else
                        Return DirectCast(resMgr.GetObject(Name), Image)
                    End If
                End If
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return EntryType.ToString
        End Function
    End Class
End Namespace