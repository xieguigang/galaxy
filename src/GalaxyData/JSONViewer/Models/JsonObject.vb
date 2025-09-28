Imports System.Diagnostics

Namespace JSON.Models

    <DebuggerDisplay("Text = {Text}")>
    Public Class JsonObject
        Private _id As String
        Private _value As Object
        Private _jsonType As JsonType
        Private _fields As JsonFields
        Private _parent As JsonObject
        Private _text As String

        Public Sub New()
            _fields = New JsonFields(Me)
        End Sub

        Public Property Id As String
            Get
                Return _id
            End Get
            Set(value As String)
                _id = value
            End Set
        End Property

        Public Property Value As Object
            Get
                Return _value
            End Get
            Set(value As Object)
                _value = value
            End Set
        End Property

        Public Property JsonType As JsonType
            Get
                Return _jsonType
            End Get
            Set(value As JsonType)
                _jsonType = value
            End Set
        End Property

        Public Property Parent As JsonObject
            Get
                Return _parent
            End Get
            Set(value As JsonObject)
                _parent = value
            End Set
        End Property

        Public ReadOnly Property Text As String
            Get
                If Equals(_text, Nothing) Then
                    If JsonType = JsonType.Value Then
                        Dim val As String = (If(Value Is Nothing, "<null>", Value.ToString()))
                        If TypeOf Value Is String Then val = """" & val & """"
                        _text = String.Format("{0} : {1}", Id, val)
                    Else
                        _text = Id
                    End If
                End If
                Return _text
            End Get
        End Property

        Public ReadOnly Property Fields As JsonFields
            Get
                Return _fields
            End Get
        End Property

        Friend Sub Modified()
            _text = Nothing
        End Sub

        Public Function ContainsFields(ParamArray ids As String()) As Boolean
            For Each s In ids
                If Not _fields.ContainId(s) Then Return False
            Next
            Return True
        End Function

        Public Function ContainsField(id As String, type As JsonType) As Boolean
            Dim field = Fields(id)
            Return field IsNot Nothing AndAlso field.JsonType = type
        End Function
    End Class
End Namespace
