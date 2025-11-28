Imports System.ComponentModel
Imports System.Reflection

Public Class DefaultExpandedArray : Inherits ExpandableObjectConverter

    Public Overrides Function GetProperties(
        context As ITypeDescriptorContext,
        value As Object,
        attributes() As Attribute) As PropertyDescriptorCollection

        ' 获取默认的属性描述符集合
        Dim pds As PropertyDescriptorCollection = MyBase.GetProperties(context, value, attributes)
        If value IsNot Nothing AndAlso TypeOf value Is String() Then
            Dim stringArray As String() = DirectCast(value, String())
            Dim propList As New List(Of PropertyDescriptor)

            ' 为数组中的每个元素创建一个属性描述符
            For i As Integer = 0 To stringArray.Length - 1
                Dim index As Integer = i ' 闭包问题，需要局部变量
                Dim pd As New ArrayPropertyDescriptor(stringArray, index)
                propList.Add(pd)
            Next

            Return New PropertyDescriptorCollection(propList.ToArray())
        Else
            Return pds
        End If
    End Function

    Public Overrides Function GetPropertiesSupported(
        context As ITypeDescriptorContext) As Boolean
        Return True
    End Function
End Class

Public Class ArrayPropertyDescriptor : Inherits PropertyDescriptor

    Private ReadOnly _array As String()
    Private ReadOnly _index As Integer

    Public Sub New(array As String(), index As Integer)
        MyBase.New($"#{index + 1}", Nothing)
        _array = array
        _index = index
    End Sub

    Public Overrides ReadOnly Property ComponentType As Type
        Get
            Return GetType(String())
        End Get
    End Property

    Public Overrides ReadOnly Property IsReadOnly As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides ReadOnly Property PropertyType As Type
        Get
            Return GetType(String)
        End Get
    End Property

    Public Overrides Function CanResetValue(component As Object) As Boolean
        Return False
    End Function

    Public Overrides Function GetValue(component As Object) As Object
        Return _array(_index)
    End Function

    Public Overrides Sub ResetValue(component As Object)
    End Sub

    Public Overrides Sub SetValue(component As Object, value As Object)
        _array(_index) = If(value?.ToString(), "")
        OnValueChanged(component, EventArgs.Empty)
    End Sub

    Public Overrides Function ShouldSerializeValue(component As Object) As Boolean
        Return False
    End Function
End Class