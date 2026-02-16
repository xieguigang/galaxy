Imports System.ComponentModel
Imports System.Runtime.CompilerServices

Namespace JSON.Models

    Friend Class JsonTreeObjectTypeDescriptor
        Implements ICustomTypeDescriptor

        ReadOnly _jsonObject As JsonObject
        ReadOnly _propertyCollection As PropertyDescriptorCollection

        Public Sub New(jsonObject As JsonObject)
            _jsonObject = jsonObject
            _propertyCollection = New PropertyDescriptorCollection(InitPropertyCollection(jsonObject).ToArray)
        End Sub

        Public ReadOnly Property RequireRegisteredTypes As Boolean? Implements ICustomTypeDescriptor.RequireRegisteredTypes

        Private Shared Iterator Function InitPropertyCollection(jsonObject As JsonObject) As IEnumerable(Of PropertyDescriptor)
            For Each field As JsonObject In jsonObject.Fields
                Yield New JsonTreeObjectPropertyDescriptor(field)
            Next
        End Function

        Public Function GetConverterFromRegisteredType() As TypeConverter Implements ICustomTypeDescriptor.GetConverterFromRegisteredType
            Return Nothing
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Private Function GetAttributes() As AttributeCollection Implements ICustomTypeDescriptor.GetAttributes
            Return TypeDescriptor.GetAttributes(_jsonObject, True)
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Private Function GetClassName() As String Implements ICustomTypeDescriptor.GetClassName
            Return TypeDescriptor.GetClassName(_jsonObject, True)
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Private Function GetComponentName() As String Implements ICustomTypeDescriptor.GetComponentName
            Return TypeDescriptor.GetComponentName(_jsonObject, True)
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Private Function GetConverter() As TypeConverter Implements ICustomTypeDescriptor.GetConverter
            Return TypeDescriptor.GetConverter(_jsonObject, True)
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Private Function GetDefaultEvent() As EventDescriptor Implements ICustomTypeDescriptor.GetDefaultEvent
            Return TypeDescriptor.GetDefaultEvent(_jsonObject, True)
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Private Function GetDefaultProperty() As PropertyDescriptor Implements ICustomTypeDescriptor.GetDefaultProperty
            Return Nothing
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Private Function GetEditor(editorBaseType As Type) As Object Implements ICustomTypeDescriptor.GetEditor
            Return TypeDescriptor.GetEditor(_jsonObject, editorBaseType, True)
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Private Function GetEvents1(attributes As Attribute()) As EventDescriptorCollection Implements ICustomTypeDescriptor.GetEvents
            Return TypeDescriptor.GetEvents(Me, attributes, True)
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Private Function GetEvents2() As EventDescriptorCollection Implements ICustomTypeDescriptor.GetEvents, ICustomTypeDescriptor.GetEventsFromRegisteredType
            Return TypeDescriptor.GetEvents(_jsonObject, True)
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Private Function GetProperties1(attributes As Attribute()) As PropertyDescriptorCollection Implements ICustomTypeDescriptor.GetProperties
            Return _propertyCollection
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Private Function GetProperties2() As PropertyDescriptorCollection Implements ICustomTypeDescriptor.GetProperties, ICustomTypeDescriptor.GetPropertiesFromRegisteredType
            Return CType(Me, ICustomTypeDescriptor).GetProperties(Nothing)
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Private Function GetPropertyOwner(pd As PropertyDescriptor) As Object Implements ICustomTypeDescriptor.GetPropertyOwner
            Return _jsonObject
        End Function
    End Class

End Namespace
