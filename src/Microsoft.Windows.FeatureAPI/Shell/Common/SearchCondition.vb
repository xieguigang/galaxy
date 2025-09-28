'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Collections.Generic
Imports System.Runtime.InteropServices
Imports Microsoft.Windows.Internal
Imports Microsoft.Windows.Shell.PropertySystem

Namespace Shell
    ''' <summary>
    ''' Exposes properties and methods for retrieving information about a search condition.
    ''' </summary>
    Public Class SearchCondition
        Implements IDisposable
        Friend Sub New(nativeSearchCondition__1 As ICondition)
            If nativeSearchCondition__1 Is Nothing Then
                Throw New ArgumentNullException("nativeSearchCondition")
            End If

            NativeSearchCondition = nativeSearchCondition__1

            Dim hr As HResult = NativeSearchCondition.GetConditionType(m_conditionType)

            If Not CoreErrorHelper.Succeeded(hr) Then
                Throw New ShellException(hr)
            End If

            If ConditionType = SearchConditionType.Leaf Then
                Using propVar As New PropVariant()
                    hr = NativeSearchCondition.GetComparisonInfo(canonicalName, m_conditionOperation, propVar)

                    If Not CoreErrorHelper.Succeeded(hr) Then
                        Throw New ShellException(hr)
                    End If

                    PropertyValue = propVar.Value.ToString()
                End Using
            End If
        End Sub

        Friend Property NativeSearchCondition() As ICondition
            Get
                Return m_NativeSearchCondition
            End Get
            Set
                m_NativeSearchCondition = Value
            End Set
        End Property
        Private m_NativeSearchCondition As ICondition

        Private canonicalName As String
        ''' <summary>
        ''' The name of a property to be compared or NULL for an unspecified property.
        ''' </summary>
        Public ReadOnly Property PropertyCanonicalName() As String
            Get
                Return canonicalName
            End Get
        End Property

        Private m_propertyKey As PropertyKey
        Private emptyPropertyKey As New PropertyKey()
        ''' <summary>
        ''' The property key for the property that is to be compared.
        ''' </summary>        
        Public ReadOnly Property PropertyKey() As PropertyKey
            Get
                If m_propertyKey = emptyPropertyKey Then
                    Dim hr As Integer = PropertySystemNativeMethods.PSGetPropertyKeyFromName(PropertyCanonicalName, m_propertyKey)
                    If Not CoreErrorHelper.Succeeded(hr) Then
                        Throw New ShellException(hr)
                    End If
                End If

                Return m_propertyKey
            End Get
        End Property

        ''' <summary>
        ''' A value (in <see cref="System.String"/> format) to which the property is compared. 
        ''' </summary>
        Public Property PropertyValue() As String
            Get
                Return m_PropertyValue
            End Get
            Friend Set
                m_PropertyValue = Value
            End Set
        End Property
        Private m_PropertyValue As String

        Private m_conditionOperation As SearchConditionOperation = SearchConditionOperation.Implicit
        ''' <summary>
        ''' Search condition operation to be performed on the property/value combination.
        ''' See <see cref="Shell.SearchConditionOperation"/> for more details.
        ''' </summary>        
        Public ReadOnly Property ConditionOperation() As SearchConditionOperation
            Get
                Return m_conditionOperation
            End Get
        End Property

        Private m_conditionType As SearchConditionType = SearchConditionType.Leaf
        ''' <summary>
        ''' Represents the condition type for the given node. 
        ''' </summary>        
        Public ReadOnly Property ConditionType() As SearchConditionType
            Get
                Return m_conditionType
            End Get
        End Property

        ''' <summary>
        ''' Retrieves an array of the sub-conditions. 
        ''' </summary>
        Public Function GetSubConditions() As IEnumerable(Of SearchCondition)
            ' Our list that we'll return
            Dim subConditionsList As New List(Of SearchCondition)()

            ' Get the sub-conditions from the native API
            Dim subConditionObj As Object
            Dim guid As New Guid(ShellIIDGuid.IEnumUnknown)

            Dim hr As HResult = NativeSearchCondition.GetSubConditions(guid, subConditionObj)

            If Not CoreErrorHelper.Succeeded(hr) Then
                Throw New ShellException(hr)
            End If

            ' Convert each ICondition to SearchCondition
            If subConditionObj IsNot Nothing Then
                Dim enumUnknown As IEnumUnknown = TryCast(subConditionObj, IEnumUnknown)

                Dim buffer As IntPtr = IntPtr.Zero
                Dim fetched As UInteger = 0

                While hr = HResult.Ok
                    hr = enumUnknown.[Next](1, buffer, fetched)

                    If hr = HResult.Ok AndAlso fetched = 1 Then
                        subConditionsList.Add(New SearchCondition(DirectCast(Marshal.GetObjectForIUnknown(buffer), ICondition)))
                    End If
                End While
            End If

            Return subConditionsList
        End Function

#Region "IDisposable Members"

        ''' <summary>
        ''' 
        ''' </summary>
        Protected Overrides Sub Finalize()
            Try
                Dispose(False)
            Finally
                MyBase.Finalize()
            End Try
        End Sub

        ''' <summary>
        ''' Release the native objects.
        ''' </summary>
        Public Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub

        ''' <summary>
        ''' Release the native objects.
        ''' </summary>
        ''' <param name="disposing"></param>
        Protected Overridable Sub Dispose(disposing As Boolean)
            If NativeSearchCondition IsNot Nothing Then
                Marshal.ReleaseComObject(NativeSearchCondition)
                NativeSearchCondition = Nothing
            End If
        End Sub

#End Region

    End Class
End Namespace
