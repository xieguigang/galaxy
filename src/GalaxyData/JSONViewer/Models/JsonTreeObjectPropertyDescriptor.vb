Imports System.ComponentModel

Namespace JSON.Models

    Friend Class JsonTreeObjectPropertyDescriptor
        Inherits PropertyDescriptor
        Private _jsonObject As JsonObject
        Private _jsonObjects As JsonTreeObjectTypeDescriptor()

        Public Sub New(jsonObject As JsonObject)
            MyBase.New(jsonObject.Id, Nothing)
            _jsonObject = jsonObject
            If _jsonObject.JsonType = JsonType.Array Then InitJsonObject()
        End Sub

        Private Sub InitJsonObject()
            Dim jsonObjectList As List(Of JsonTreeObjectTypeDescriptor) = New List(Of JsonTreeObjectTypeDescriptor)()
            For Each field In _jsonObject.Fields
                jsonObjectList.Add(New JsonTreeObjectTypeDescriptor(field))
            Next
            _jsonObjects = jsonObjectList.ToArray()
        End Sub

        Public Overrides Function CanResetValue(component As Object) As Boolean
            Return False
        End Function

        Public Overrides ReadOnly Property ComponentType As Type
            Get
                Return Nothing
            End Get
        End Property

        Public Overrides Function GetValue(component As Object) As Object
            Select Case _jsonObject.JsonType
                Case JsonType.Array
                    Return _jsonObjects
                Case JsonType.Object
                    Return _jsonObject
                Case Else
                    Return _jsonObject.Value
            End Select
        End Function

        Public Overrides ReadOnly Property IsReadOnly As Boolean
            Get
                Return False
            End Get
        End Property

        Public Overrides ReadOnly Property PropertyType As Type
            Get
                Select Case _jsonObject.JsonType
                    Case JsonType.Array
                        Return GetType(Object())
                    Case JsonType.Object
                        Return GetType(JsonObject)
                    Case Else
                        Return If(_jsonObject.Value Is Nothing, GetType(String), _jsonObject.Value.GetType())
                End Select
            End Get
        End Property

        Public Overrides Sub ResetValue(component As Object)
            Throw New Exception("The method or operation is not implemented.")
        End Sub

        Public Overrides Sub SetValue(component As Object, value As Object)
            'TODO: Implement?
        End Sub

        Public Overrides Function ShouldSerializeValue(component As Object) As Boolean
            Return False
        End Function

        Public Overrides Function GetEditor(editorBaseType As Type) As Object
            Return MyBase.GetEditor(editorBaseType)
        End Function
    End Class
End Namespace