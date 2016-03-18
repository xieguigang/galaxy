'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Runtime.InteropServices
Imports Microsoft.Windows.Resources

Namespace Shell.PropertySystem
	''' <summary>
	''' Defines a unique key for a Shell Property
	''' </summary>
	<StructLayout(LayoutKind.Sequential, Pack := 4)> _
	Public Structure PropertyKey
		Implements IEquatable(Of PropertyKey)
		#Region "Private Fields"

		Private m_formatId As Guid
		Private m_propertyId As Int32

		#End Region

		#Region "Public Properties"
		''' <summary>
		''' A unique GUID for the property
		''' </summary>
		Public ReadOnly Property FormatId() As Guid
			Get
				Return m_formatId
			End Get
		End Property

		''' <summary>
		'''  Property identifier (PID)
		''' </summary>
		Public ReadOnly Property PropertyId() As Int32
			Get
				Return m_propertyId
			End Get
		End Property

		#End Region

		#Region "Public Construction"

		''' <summary>
		''' PropertyKey Constructor
		''' </summary>
		''' <param name="formatId">A unique GUID for the property</param>
		''' <param name="propertyId">Property identifier (PID)</param>
		Public Sub New(formatId As Guid, propertyId As Int32)
			Me.m_formatId = formatId
			Me.m_propertyId = propertyId
		End Sub

		''' <summary>
		''' PropertyKey Constructor
		''' </summary>
		''' <param name="formatId">A string represenstion of a GUID for the property</param>
		''' <param name="propertyId">Property identifier (PID)</param>
		Public Sub New(formatId As String, propertyId As Int32)
			Me.m_formatId = New Guid(formatId)
			Me.m_propertyId = propertyId
		End Sub

#End Region

#Region "IEquatable<PropertyKey> Members"

        ''' <summary>
        ''' Returns whether this object is equal to another. This is vital for performance of value types.
        ''' </summary>
        ''' <param name="other">The object to compare against.</param>
        ''' <returns>Equality result.</returns>
        Public Overloads Function Equals(other As PropertyKey) As Boolean Implements IEquatable(Of PropertyKey).Equals
            Return other.Equals(DirectCast(Me, Object))
        End Function

#End Region

#Region "equality and hashing"

        ''' <summary>
        ''' Returns the hash code of the object. This is vital for performance of value types.
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function GetHashCode() As Integer
			Return m_formatId.GetHashCode() Xor m_propertyId
		End Function

		''' <summary>
		''' Returns whether this object is equal to another. This is vital for performance of value types.
		''' </summary>
		''' <param name="obj">The object to compare against.</param>
		''' <returns>Equality result.</returns>
		Public Overrides Function Equals(obj As Object) As Boolean
			If obj Is Nothing Then
				Return False
			End If

			If Not (TypeOf obj Is PropertyKey) Then
				Return False
			End If

			Dim other As PropertyKey = CType(obj, PropertyKey)
			Return other.formatId.Equals(m_formatId) AndAlso (other.propertyId = m_propertyId)
		End Function

		''' <summary>
		''' Implements the == (equality) operator.
		''' </summary>
		''' <param name="propKey1">First property key to compare.</param>
		''' <param name="propKey2">Second property key to compare.</param>
		''' <returns>true if object a equals object b. false otherwise.</returns>        
		Public Shared Operator =(propKey1 As PropertyKey, propKey2 As PropertyKey) As Boolean
			Return propKey1.Equals(propKey2)
		End Operator

		''' <summary>
		''' Implements the != (inequality) operator.
		''' </summary>
		''' <param name="propKey1">First property key to compare</param>
		''' <param name="propKey2">Second property key to compare.</param>
		''' <returns>true if object a does not equal object b. false otherwise.</returns>
		Public Shared Operator <>(propKey1 As PropertyKey, propKey2 As PropertyKey) As Boolean
			Return Not propKey1.Equals(propKey2)
		End Operator

		''' <summary>
		''' Override ToString() to provide a user friendly string representation
		''' </summary>
		''' <returns>String representing the property key</returns>        
		Public Overrides Function ToString() As String
			Return String.Format(System.Globalization.CultureInfo.InvariantCulture, LocalizedMessages.PropertyKeyFormatString, m_formatId.ToString("B"), m_propertyId)
		End Function

		#End Region
	End Structure
End Namespace
