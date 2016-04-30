Imports System.Reflection
Imports System.Windows.Forms

Namespace PlugIns

    ''' <summary>
    ''' Function Main(Target As Form) As Object.(应用于目标模块中的一个函数的自定义属性，相对应于菜单中的一个项目)
    ''' </summary>
    ''' <remarks></remarks>
    <AttributeUsage(AttributeTargets.Method, AllowMultiple:=False, Inherited:=True)>
    Public Class PlugInCommand

        Public ReadOnly Property base As Attributes.PlugInCommand
        Public ReadOnly Property Method As MethodInfo

        Sub New(base As Attributes.PlugInCommand, method As MethodInfo)
            Me.base = base
            Me.Method = method
        End Sub

        Public Overrides Function ToString() As String
            Return base.ToString
        End Function

        Public Function Invoke(Target As Form) As Object
            Return ReflectionAPI.Invoke({Target}, Method)
        End Function
    End Class
End Namespace