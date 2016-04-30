Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.Drawing
Imports System.Reflection
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Windows.Forms.PlugIns.Attributes

Namespace PlugIns

    ''' <summary>
    ''' Module PlugInsMain.(目标模块，在本模块之中包含有一系列插件命令信息，本对象定义了插件在菜单之上的根菜单项目)
    ''' </summary>
    ''' <remarks></remarks>
    <AttributeUsage(AttributeTargets.Class, AllowMultiple:=False, Inherited:=True)>
    Public Class PlugInEntry

        Public ReadOnly Property IconImage As Image
        Public ReadOnly Property MainModule As Type
        Public ReadOnly Property EntryList As EntryFlag()
        Public ReadOnly Property AssemblyPath As String
            Get
                Return Assembly.Location
            End Get
        End Property
        Public ReadOnly Property Assembly As Assembly

        Public ReadOnly Property base As Attributes.PlugInEntry

        Sub New(base As Attributes.PlugInEntry, type As Type, assm As Assembly)
            Me.base = base
            Me.MainModule = type
            Me.Assembly = assm
            Me.EntryList = LinqAPI.Exec(Of EntryFlag) <=
                From method As MethodInfo
                In type.GetMethods
                Let attrs As Object() = method.GetCustomAttributes(Attributes.EntryFlag.TypeId, False)
                Where 1 = attrs.Length
                Let attr As Attributes.EntryFlag = DirectCast(attrs(0), Attributes.EntryFlag)
                Select New EntryFlag(attr, method)
        End Sub

        Public Function GetEntry(EntryType As EntryTypes) As EntryFlag
            Return LinqAPI.DefaultFirst(Of EntryFlag) <=
                From entry As EntryFlag
                In EntryList
                Where entry.EntryType = EntryType
                Select entry
        End Function

        Public Overrides Function ToString() As String
            Return String.Format("PlugInEntry: {0}, //{1}", base.Name, AssemblyPath)
        End Function
    End Class
End Namespace