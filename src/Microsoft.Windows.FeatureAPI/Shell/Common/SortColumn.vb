' Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Runtime.InteropServices
Imports Microsoft.Windows.Shell.PropertySystem

Namespace Shell
    ''' <summary>
    ''' Stores information about how to sort a column that is displayed in the folder view.
    ''' </summary>    
    <StructLayout(LayoutKind.Sequential)>
    Public Structure SortColumn

        ''' <summary>
        ''' Creates a sort column with the specified direction for the given property.
        ''' </summary>
        ''' <param name="propertyKey">Property key for the property that the user will sort.</param>
        ''' <param name="direction">The direction in which the items are sorted.</param>
        Public Sub New(propertyKey As PropertyKey, direction As SortDirection)
            Me.New()
            Me.m_propertyKey = propertyKey
            Me.m_direction = direction
        End Sub

        ''' <summary>
        ''' The ID of the column by which the user will sort. A PropertyKey structure. 
        ''' For example, for the "Name" column, the property key is PKEY_ItemNameDisplay or
        ''' <see cref="Shell.PropertySystem.SystemProperties.System.ItemName"/>.
        ''' </summary>                
        Public Property PropertyKey() As PropertyKey
            Get
                Return m_propertyKey
            End Get
            Set
                m_propertyKey = Value
            End Set
        End Property
        Private m_propertyKey As PropertyKey

        ''' <summary>
        ''' The direction in which the items are sorted.
        ''' </summary>                        
        Public Property Direction() As SortDirection
            Get
                Return m_direction
            End Get
            Set
                m_direction = Value
            End Set
        End Property
        Private m_direction As SortDirection


        ''' <summary>
        ''' Implements the == (equality) operator.
        ''' </summary>
        ''' <param name="col1">First object to compare.</param>
        ''' <param name="col2">Second object to compare.</param>
        ''' <returns>True if col1 equals col2; false otherwise.</returns>
        Public Shared Operator =(col1 As SortColumn, col2 As SortColumn) As Boolean
            Return (col1.Direction = col2.Direction) AndAlso (col1.PropertyKey = col2.PropertyKey)
        End Operator

        ''' <summary>
        ''' Implements the != (unequality) operator.
        ''' </summary>
        ''' <param name="col1">First object to compare.</param>
        ''' <param name="col2">Second object to compare.</param>
        ''' <returns>True if col1 does not equals col1; false otherwise.</returns>
        Public Shared Operator <>(col1 As SortColumn, col2 As SortColumn) As Boolean
            Return Not (col1 = col2)
        End Operator

        ''' <summary>
        ''' Determines if this object is equal to another.
        ''' </summary>
        ''' <param name="obj">The object to compare</param>
        ''' <returns>Returns true if the objects are equal; false otherwise.</returns>
        Public Overrides Function Equals(obj As Object) As Boolean
            If obj Is Nothing OrElse obj.[GetType]() IsNot GetType(SortColumn) Then
                Return False
            End If
            Return (Me = CType(obj, SortColumn))
        End Function

        ''' <summary>
        ''' Generates a nearly unique hashcode for this structure.
        ''' </summary>
        ''' <returns>A hash code.</returns>
        Public Overrides Function GetHashCode() As Integer
            Dim hash As Integer = Me.m_direction.GetHashCode()
            hash = hash * 31 + Me.m_propertyKey.GetHashCode()
            Return hash
        End Function

    End Structure

End Namespace
