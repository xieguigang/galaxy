'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports Microsoft.Windows.Internal

Namespace Shell.PropertySystem
	''' <summary>
	''' Defines the enumeration values for a property type.
	''' </summary>
	Public Class ShellPropertyEnumType
		#Region "Private Properties"

		Private m_displayText As String
		Private m_enumType As System.Nullable(Of PropEnumType)
		Private minValue As Object, setValue As Object, enumerationValue As Object

		Private Property NativePropertyEnumType() As IPropertyEnumType
			Get
				Return m_NativePropertyEnumType
			End Get
			Set
				m_NativePropertyEnumType = Value
			End Set
		End Property
		Private m_NativePropertyEnumType As IPropertyEnumType

		#End Region

		#Region "Internal Constructor"

		Friend Sub New(nativePropertyEnumType__1 As IPropertyEnumType)
			NativePropertyEnumType = nativePropertyEnumType__1
		End Sub

		#End Region

		#Region "Public Properties"

		''' <summary>
		''' Gets display text from an enumeration information structure. 
		''' </summary>
		Public ReadOnly Property DisplayText() As String
			Get
				If m_displayText Is Nothing Then
					NativePropertyEnumType.GetDisplayText(m_displayText)
				End If
				Return m_displayText
			End Get
		End Property

		''' <summary>
		''' Gets an enumeration type from an enumeration information structure. 
		''' </summary>
		Public ReadOnly Property EnumType() As PropEnumType
			Get
				If Not m_enumType.HasValue Then
					Dim tempEnumType As PropEnumType
					NativePropertyEnumType.GetEnumType(tempEnumType)
					m_enumType = tempEnumType
				End If
				Return m_enumType.Value
			End Get
		End Property

		''' <summary>
		''' Gets a minimum value from an enumeration information structure. 
		''' </summary>
		Public ReadOnly Property RangeMinValue() As Object
			Get
				If minValue Is Nothing Then
					Using propVar As New PropVariant()
						NativePropertyEnumType.GetRangeMinValue(propVar)
						minValue = propVar.Value
					End Using
				End If

				Return minValue
			End Get
		End Property

		''' <summary>
		''' Gets a set value from an enumeration information structure. 
		''' </summary>
		Public ReadOnly Property RangeSetValue() As Object
			Get
				If setValue Is Nothing Then
					Using propVar As New PropVariant()
						NativePropertyEnumType.GetRangeSetValue(propVar)
						setValue = propVar.Value
					End Using
				End If

				Return setValue
			End Get
		End Property

		''' <summary>
		''' Gets a value from an enumeration information structure. 
		''' </summary>
		Public ReadOnly Property RangeValue() As Object
			Get
				If enumerationValue Is Nothing Then
					Using propVar As New PropVariant()
						NativePropertyEnumType.GetValue(propVar)
						enumerationValue = propVar.Value
					End Using
				End If
				Return enumerationValue
			End Get
		End Property

		#End Region
	End Class
End Namespace
