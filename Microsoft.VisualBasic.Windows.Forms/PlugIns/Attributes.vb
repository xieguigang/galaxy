Imports System.Xml.Serialization
Imports Microsoft.VisualBasic.Serialization
Imports Microsoft.VisualBasic.Serialization.JSON

Namespace PlugIns.Attributes

    Public MustInherit Class CommandBase : Inherits Attribute

        ''' <summary>
        ''' The command item name that display on the menustrip control.
        ''' </summary>
        ''' <remarks></remarks>
        <XmlAttribute> Public Property Name As String
        ''' <summary>
        ''' Tooltip text or the description text.
        ''' </summary>
        ''' <remarks></remarks>
        <XmlAttribute> Public Property Description As String
        ''' <summary>
        ''' The icon resource name string.(图标资源名称，当本属性值为空的时候，对应的菜单项没有图标)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <XmlAttribute> Public Property Icon As String = ""

        Public Overrides Function ToString() As String
            Return Me.GetJson
        End Function
    End Class

    Public Enum EntryTypes
        ''' <summary>
        ''' This method is the entry point to destroy this plugin command and unload it from the target form.
        ''' </summary>
        ''' <remarks></remarks>
        Dispose
        ''' <summary>
        ''' Sometimes you need a initialize method to initialize your plugin.
        ''' </summary>
        ''' <remarks></remarks>
        Initialize
        ''' <summary>
        ''' This method is the icon loader entry point
        ''' </summary>
        ''' <remarks></remarks>
        IconLoader
    End Enum

    ''' <summary>
    ''' 
    ''' </summary>
    <AttributeUsage(AttributeTargets.Method, AllowMultiple:=False, Inherited:=True)>
    Public Class EntryFlag : Inherits Attribute

        Public ReadOnly Property Type As EntryTypes

        Sub New(flag As EntryTypes)
            Type = flag
        End Sub

        Public Overrides Function ToString() As String
            Return Type.ToString
        End Function

        Public Overloads Shared ReadOnly Property TypeId As Type = GetType(EntryFlag)
    End Class

    ''' <summary>
    ''' Function Main(Target As Form) As Object.(应用于目标模块中的一个函数的自定义属性，相对应于菜单中的一个项目)
    ''' </summary>
    ''' <remarks></remarks>
    <AttributeUsage(AttributeTargets.Method, AllowMultiple:=False, Inherited:=True)>
    Public Class PlugInCommand : Inherits CommandBase

        ''' <summary>
        ''' The menu path for this plugin command.(这个插件命令的菜单路径)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Path As String = "\"

        Sub New(menuPath As String)
            Path = menuPath
        End Sub

        Public Overrides Function ToString() As String
            If String.IsNullOrEmpty(Path) OrElse String.Equals("\", Path) Then
                Return String.Format("Name:={0}; Path:\\Root", Name)
            Else
                Return String.Format("Name:={0}; Path:\\{1}", Name, Path)
            End If
        End Function
    End Class

    ''' <summary>
    ''' Module PlugInsMain.(目标模块，在本模块之中包含有一系列插件命令信息，本对象定义了插件在菜单之上的根菜单项目)
    ''' </summary>
    ''' <remarks></remarks>
    <AttributeUsage(AttributeTargets.Class, AllowMultiple:=False, Inherited:=True)>
    Public Class PlugInEntry : Inherits CommandBase

        Public Property ShowOnMenu As Boolean = True

        Public Overrides Function ToString() As String
            Return Me.GetJson
        End Function
    End Class
End Namespace