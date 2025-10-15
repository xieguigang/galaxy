
Imports System.Runtime.InteropServices
Imports Microsoft.VisualBasic.ComponentModel.Collection.Generic

Namespace Plugin.Repository

    <ClassInterface(ClassInterfaceType.AutoDual)>
    <ComVisible(True)>
    Public Class PluginMetadata : Implements INamedValue

        Public Property id As String Implements INamedValue.Key
        Public Property name As String
        Public Property desc As String
        Public Property ver As String
        Public Property author As String
        Public Property url As String

        ''' <summary>
        ''' "active" | "disable" | "incompatible"
        ''' </summary>
        ''' <returns></returns>
        Public Property status As String

        Public Overrides Function ToString() As String
            Return name
        End Function

    End Class
End Namespace