Namespace JSON.Models

    Public Class JsonFields : Implements IEnumerable(Of JsonObject)

        Private _fields As List(Of JsonObject) = New List(Of JsonObject)()
        Private _fieldsById As Dictionary(Of String, JsonObject) = New Dictionary(Of String, JsonObject)()
        Private _parent As JsonObject

        Public Sub New(parent As JsonObject)
            _parent = parent
        End Sub

        Public Function GetEnumerator() As IEnumerator(Of JsonObject) Implements IEnumerable(Of JsonObject).GetEnumerator
            Return _fields.GetEnumerator()
        End Function

        Private Function GetEnumerator1() As IEnumerator Implements IEnumerable.GetEnumerator
            Return GetEnumerator()
        End Function

        Public Sub Add(field As JsonObject)
            field.Parent = _parent
            _fields.Add(field)
            _fieldsById(field.Id) = field
            _parent.Modified()
        End Sub

        Public ReadOnly Property Count As Integer
            Get
                Return _fields.Count
            End Get
        End Property

        Default Public ReadOnly Property Item(index As Integer) As JsonObject
            Get
                Return _fields(index)
            End Get
        End Property

        Default Public ReadOnly Property Item(id As String) As JsonObject
            Get
                Dim result As JsonObject = Nothing
                If _fieldsById.TryGetValue(id, result) Then Return result
                Return Nothing
            End Get
        End Property

        Public Function ContainId(id As String) As Boolean
            Return _fieldsById.ContainsKey(id)
        End Function
    End Class
End Namespace
