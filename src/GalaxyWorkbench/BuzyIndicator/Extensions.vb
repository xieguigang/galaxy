Imports System.Runtime.CompilerServices

Public Module Extensions

    <Extension>
    Public Function Echo(task As ITaskProgress) As Action(Of String)
        Return New Action(Of String)(AddressOf task.SetInfo)
    End Function
End Module
