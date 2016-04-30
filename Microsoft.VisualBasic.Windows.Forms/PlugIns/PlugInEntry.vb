Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.Drawing
Imports System.Reflection
Imports Microsoft.VisualBasic.Language

Namespace PlugIns

    ''' <summary>
    ''' Module PlugInsMain.(目标模块，在本模块之中包含有一系列插件命令信息，本对象定义了插件在菜单之上的根菜单项目)
    ''' </summary>
    ''' <remarks></remarks>
    <AttributeUsage(AttributeTargets.Class, AllowMultiple:=False, Inherited:=True)>
    Public Class PlugInEntry : Inherits CommandBase

        Protected Friend IconImage As Image
        Protected Friend MainModule As Type
        Protected Friend EntryList As EntryFlag()
        Protected Friend AssemblyPath As String
        Protected Friend Assembly As Assembly

        Public Property ShowOnMenu As Boolean = True

        Public Function GetEntry(EntryType As EntryTypes) As EntryFlag
            Return LinqAPI.DefaultFirst(Of EntryFlag) <=
                From entry As EntryFlag
                In EntryList
                Where entry.EntryType = EntryType
                Select entry
        End Function

        Friend Function Initialize([Module] As Type) As PlugInEntry
            MainModule = [Module]
            Return Me
        End Function

        Public Overrides Function ToString() As String
            Return String.Format("PlugInEntry: {0}, //{1}", Name, AssemblyPath)
        End Function
    End Class
End Namespace