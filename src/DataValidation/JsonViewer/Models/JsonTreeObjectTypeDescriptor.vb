Imports System
Imports System.Collections.Generic
Imports System.ComponentModel

Namespace Models
    Friend Class JsonTreeObjectTypeDescriptor
        Implements ICustomTypeDescriptor
        Private _jsonObject As JsonObject
        Private _propertyCollection As PropertyDescriptorCollection

        Public Sub New(jsonObject As JsonObject)
            _jsonObject = jsonObject
            InitPropertyCollection()
        End Sub

        Private Sub InitPropertyCollection()
            Dim propertyDescriptors As List(Of PropertyDescriptor) = New List(Of PropertyDescriptor)()

            For Each field In _jsonObject.Fields
                Dim pd As PropertyDescriptor = New JsonTreeObjectPropertyDescriptor(field)
                propertyDescriptors.Add(pd)
            Next
            _propertyCollection = New PropertyDescriptorCollection(propertyDescriptors.ToArray())
        End Sub

        Private Function GetAttributes() As AttributeCollection Implements ICustomTypeDescriptor.GetAttributes
            Return TypeDescriptor.GetAttributes(_jsonObject, True)
        End Function

        Private Function GetClassName() As String Implements ICustomTypeDescriptor.GetClassName
            Return TypeDescriptor.GetClassName(_jsonObject, True)
        End Function

        Private Function GetComponentName() As String Implements ICustomTypeDescriptor.GetComponentName
            Return TypeDescriptor.GetComponentName(_jsonObject, True)
        End Function

        Private Function GetConverter() As TypeConverter Implements ICustomTypeDescriptor.GetConverter
            Return TypeDescriptor.GetConverter(_jsonObject, True)
        End Function

        Private Function GetDefaultEvent() As EventDescriptor Implements ICustomTypeDescriptor.GetDefaultEvent
            Return TypeDescriptor.GetDefaultEvent(_jsonObject, True)
        End Function

        Private Function GetDefaultProperty() As PropertyDescriptor Implements ICustomTypeDescriptor.GetDefaultProperty
            Return Nothing
        End Function

        Private Function GetEditor(editorBaseType As Type) As Object Implements ICustomTypeDescriptor.GetEditor
            Return TypeDescriptor.GetEditor(_jsonObject, editorBaseType, True)
        End Function

        Private Function GetEvents1(attributes As Attribute()) As EventDescriptorCollection Implements ICustomTypeDescriptor.GetEvents
            Return TypeDescriptor.GetEvents(Me, attributes, True)
        End Function

        Private Function GetEvents2() As EventDescriptorCollection Implements ICustomTypeDescriptor.GetEvents
            Return TypeDescriptor.GetEvents(_jsonObject, True)
        End Function

        Private Function GetProperties1(attributes As Attribute()) As PropertyDescriptorCollection Implements ICustomTypeDescriptor.GetProperties
            Return _propertyCollection
        End Function

        Private Function GetProperties2() As PropertyDescriptorCollection Implements ICustomTypeDescriptor.GetProperties
            Return CType(Me, ICustomTypeDescriptor).GetProperties(Nothing)
        End Function

        Private Function GetPropertyOwner(pd As PropertyDescriptor) As Object Implements ICustomTypeDescriptor.GetPropertyOwner
            Return _jsonObject
        End Function
    End Class

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
