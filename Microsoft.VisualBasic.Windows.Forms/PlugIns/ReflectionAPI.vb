Imports System.ComponentModel
Imports System.Reflection
Imports Microsoft.VisualBasic.Language

Namespace PlugIns

    Public Module ReflectionAPI

        ''' <summary>
        ''' 从一个DLL文件之中加载命令
        ''' </summary>
        ''' <param name="menu"></param>
        ''' <param name="assem">Target DLL assembly file.(目标程序集模块的文件名)</param>
        ''' <returns>返回成功加载的命令的数目</returns>
        ''' <remarks></remarks>
        Public Function LoadPlugIn(menu As MenuStrip, assem As String) As PlugInEntry
            If Not FileIO.FileSystem.FileExists(assem) Then ' When the filesystem object can not find the assembly file, then this loading operation was abort.
                Return Nothing
            Else
                Return New PlugInLoader(menu, IO.Path.GetFullPath(assem)).Load ' Assembly.LoadFile required full path of a program assembly file.
            End If
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="parameters">Method calling parameters object array.</param>
        ''' <param name="Method">Target method reflection information.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Invoke(parameters As Object(), method As MethodInfo) As Object
            Dim NumberOfParameters = method.GetParameters().Length
            Dim CallParameters() As Object

            If parameters.Length < NumberOfParameters Then
                CallParameters = New Object(NumberOfParameters - 1) {}
                parameters.CopyTo(CallParameters, 0)
            ElseIf parameters.Length > NumberOfParameters Then
                CallParameters = New Object(NumberOfParameters - 1) {}
                Call Array.ConstrainedCopy(parameters, 0, CallParameters, 0, NumberOfParameters)
            Else
                CallParameters = parameters
            End If

            Return method.Invoke(Nothing, CallParameters)
        End Function

        ''' <summary>
        ''' 从一个指定的窗体对象之上获取一个特定类型的控件的集合
        ''' </summary>
        ''' <typeparam name="T"></typeparam>
        ''' <param name="host"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetControls(Of T As Component)(host As Form) As T()
            Return LinqAPI.Exec(Of T) <=
                From ctl As Control
                In host.Controls
                Where TypeOf ctl Is T
                Let component As Component = ctl
                Select DirectCast(component, T)
        End Function
    End Module
End Namespace