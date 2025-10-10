Imports System.Resources

Namespace Docking
    Friend Module ResourceHelper
        Private _resourceManager As ResourceManager = Nothing

        Private ReadOnly Property ResourceManager As ResourceManager
            Get
                If _resourceManager Is Nothing Then _resourceManager = New ResourceManager("WeifenLuo.WinFormsUI.Docking.Strings", GetType(ResourceHelper).Assembly)
                Return _resourceManager
            End Get

        End Property

        Public Function GetString(name As String) As String
            Return ResourceManager.GetString(name)
        End Function
    End Module
End Namespace
