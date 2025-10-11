Imports System.Runtime.CompilerServices

Namespace DockDocument

    Public NotInheritable Class Pages

        Shared ReadOnly documentTypes As New Dictionary(Of String, Type)

        Private Sub New()
        End Sub

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Shared Sub SetDocument(name As String, type As Type)
            documentTypes(name) = type
        End Sub

        Public Shared Function OpenDocument(name As String) As DocumentWindow
            Return CommonRuntime.ShowDocument(documentTypes(name))
        End Function
    End Class
End Namespace