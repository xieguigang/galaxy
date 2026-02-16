Imports System.ComponentModel

Namespace JSON.Models

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

End Namespace
