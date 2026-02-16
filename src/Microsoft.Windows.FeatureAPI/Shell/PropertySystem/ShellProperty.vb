'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Reflection
Imports System.Runtime.InteropServices
Imports Microsoft.Windows.Internal

Namespace Shell.PropertySystem
	''' <summary>
	''' Defines a strongly-typed property object. 
	''' All writable property objects must be of this type 
	''' to be able to call the value setter.
	''' </summary>
	''' <typeparam name="T">The type of this property's value. 
	''' Because a property value can be empty, only nullable types 
	''' are allowed.</typeparam>
	Public Class ShellProperty(Of T)
		Implements IShellProperty
		#Region "Private Fields"

		Private m_propertyKey As PropertyKey
		Private imageReferencePath As String = Nothing
		Private imageReferenceIconIndex As System.Nullable(Of Integer)
		Private m_description As ShellPropertyDescription = Nothing

		#End Region

		#Region "Private Methods"

		Private Property ParentShellObject() As ShellObject
			Get
				Return m_ParentShellObject
			End Get
			Set
				m_ParentShellObject = Value
			End Set
		End Property
		Private m_ParentShellObject As ShellObject

		Private Property NativePropertyStore() As IPropertyStore
			Get
				Return m_NativePropertyStore
			End Get
			Set
				m_NativePropertyStore = Value
			End Set
		End Property
		Private m_NativePropertyStore As IPropertyStore

		Private Sub GetImageReference()
			Dim store As IPropertyStore = ShellPropertyCollection.CreateDefaultPropertyStore(ParentShellObject)

			Using propVar As New PropVariant()
				store.GetValue(m_propertyKey, propVar)

				Marshal.ReleaseComObject(store)
				store = Nothing

				Dim refPath As String
				DirectCast(Description.NativePropertyDescription, IPropertyDescription2).GetImageReferenceForValue(propVar, refPath)

				If refPath Is Nothing Then
					Return
				End If

				Dim index As Integer = ShellNativeMethods.PathParseIconLocation(refPath)
				If refPath IsNot Nothing Then
					imageReferencePath = refPath
					imageReferenceIconIndex = index
				End If
			End Using
		End Sub

		Private Sub StorePropVariantValue(propVar As PropVariant)
			Dim guid As New Guid(ShellIIDGuid.IPropertyStore)
			Dim writablePropStore As IPropertyStore = Nothing
			Try
				Dim hr As Integer = ParentShellObject.NativeShellItem2.GetPropertyStore(ShellNativeMethods.GetPropertyStoreOptions.ReadWrite, guid, writablePropStore)

				If Not CoreErrorHelper.Succeeded(hr) Then
					Throw New PropertySystemException(GlobalLocalizedMessages.ShellPropertyUnableToGetWritableProperty, Marshal.GetExceptionForHR(hr))
				End If

				Dim result As HResult = writablePropStore.SetValue(m_propertyKey, propVar)

				If Not AllowSetTruncatedValue AndAlso CInt(result) = ShellNativeMethods.InPlaceStringTruncated Then
					Throw New ArgumentOutOfRangeException("propVar", GlobalLocalizedMessages.ShellPropertyValueTruncated)
				End If

				If Not CoreErrorHelper.Succeeded(result) Then
					Throw New PropertySystemException(GlobalLocalizedMessages.ShellPropertySetValue, Marshal.GetExceptionForHR(CInt(result)))
				End If


				writablePropStore.Commit()
			Catch e As InvalidComObjectException
				Throw New PropertySystemException(GlobalLocalizedMessages.ShellPropertyUnableToGetWritableProperty, e)
			Catch generatedExceptionName As InvalidCastException
				Throw New PropertySystemException(GlobalLocalizedMessages.ShellPropertyUnableToGetWritableProperty)
			Finally
				If writablePropStore IsNot Nothing Then
					Marshal.ReleaseComObject(writablePropStore)
					writablePropStore = Nothing
				End If
			End Try
		End Sub

		#End Region

		#Region "Internal Constructor"

		''' <summary>
		''' Constructs a new Property object
		''' </summary>
		''' <param name="propertyKey"></param>
		''' <param name="description"></param>
		''' <param name="parent"></param>
		Friend Sub New(propertyKey As PropertyKey, description As ShellPropertyDescription, parent As ShellObject)
			Me.m_propertyKey = propertyKey
			Me.m_description = description
			ParentShellObject = parent
			AllowSetTruncatedValue = False
		End Sub

		''' <summary>
		''' Constructs a new Property object
		''' </summary>
		''' <param name="propertyKey"></param>
		''' <param name="description"></param>
		''' <param name="propertyStore"></param>
		Friend Sub New(propertyKey As PropertyKey, description As ShellPropertyDescription, propertyStore As IPropertyStore)
			Me.m_propertyKey = propertyKey
			Me.m_description = description
			NativePropertyStore = propertyStore
			AllowSetTruncatedValue = False
		End Sub

		#End Region

		#Region "Public Properties"

		''' <summary>
		''' Gets or sets the strongly-typed value of this property.
		''' The value of the property is cleared if the value is set to null.
		''' </summary>
		''' <exception cref="System.Runtime.InteropServices.COMException">
		''' If the property value cannot be retrieved or updated in the Property System</exception>
		''' <exception cref="NotSupportedException">If the type of this property is not supported; e.g. writing a binary object.</exception>
		''' <exception cref="ArgumentOutOfRangeException">Thrown if <see cref="AllowSetTruncatedValue"/> is false, and either 
		''' a string value was truncated or a numeric value was rounded.</exception>        
		Public Property Value() As T
			Get
				' Make sure we load the correct type
				System.Diagnostics.Debug.Assert(ValueType Is ShellPropertyFactory.VarEnumToSystemType(Description.VarEnumType))

				Using propVar As New PropVariant()
					If ParentShellObject.NativePropertyStore IsNot Nothing Then
						' If there is a valid property store for this shell object, then use it.
						ParentShellObject.NativePropertyStore.GetValue(m_propertyKey, propVar)
					ElseIf ParentShellObject IsNot Nothing Then
						' Use IShellItem2.GetProperty instead of creating a new property store
						' The file might be locked. This is probably quicker, and sufficient for what we need
						ParentShellObject.NativeShellItem2.GetProperty(m_propertyKey, propVar)
					ElseIf NativePropertyStore IsNot Nothing Then
						NativePropertyStore.GetValue(m_propertyKey, propVar)
					End If

					'Get the value
					Return If(propVar.Value IsNot Nothing, DirectCast(propVar.Value, T), Nothing)
				End Using
			End Get
			Set
				' Make sure we use the correct type
				System.Diagnostics.Debug.Assert(ValueType Is ShellPropertyFactory.VarEnumToSystemType(Description.VarEnumType))

				If GetType(T) IsNot ValueType Then
					Throw New NotSupportedException(String.Format(System.Globalization.CultureInfo.InvariantCulture, GlobalLocalizedMessages.ShellPropertyWrongType, ValueType.Name))
				End If

				If TypeOf value Is Nullable Then
					Dim t As Type = GetType(T)
					Dim pi As PropertyInfo = t.GetProperty("HasValue")
					If pi IsNot Nothing Then
						Dim hasValue As Boolean = CBool(pi.GetValue(value, Nothing))
						If Not hasValue Then
							ClearValue()
							Return
						End If
					End If
				ElseIf value Is Nothing Then
					ClearValue()
					Return
				End If

				If ParentShellObject IsNot Nothing Then
					Using propertyWriter As ShellPropertyWriter = ParentShellObject.Properties.GetPropertyWriter()
						propertyWriter.WriteProperty(Of T)(Me, value, AllowSetTruncatedValue)
					End Using
				ElseIf NativePropertyStore IsNot Nothing Then
					Throw New InvalidOperationException(GlobalLocalizedMessages.ShellPropertyCannotSetProperty)
				End If
			End Set
		End Property


		#End Region

		#Region "IProperty Members"

		''' <summary>
		''' Gets the property key identifying this property.
		''' </summary>
		Public ReadOnly Property PropertyKey() As PropertyKey Implements IShellProperty.PropertyKey
			Get
				Return m_propertyKey
			End Get
		End Property
		''' <summary>
		''' Returns a formatted, Unicode string representation of a property value.
		''' </summary>
		''' <param name="format">One or more of the PropertyDescriptionFormat flags 
		''' that indicate the desired format.</param>
		''' <param name="formattedString">The formatted value as a string, or null if this property 
		''' cannot be formatted for display.</param>
		''' <returns>True if the method successfully locates the formatted string; otherwise 
		''' False.</returns>
		Public Function TryFormatForDisplay(format As PropertyDescriptionFormatOptions, ByRef formattedString As String) As Boolean


			If Description Is Nothing OrElse Description.NativePropertyDescription Is Nothing Then
				' We cannot do anything without a property description
				formattedString = Nothing
				Return False
			End If

			Dim store As IPropertyStore = ShellPropertyCollection.CreateDefaultPropertyStore(ParentShellObject)

			Using propVar As New PropVariant()
				store.GetValue(m_propertyKey, propVar)

				' Release the Propertystore
				Marshal.ReleaseComObject(store)
				store = Nothing

				Dim hr As HResult = Description.NativePropertyDescription.FormatForDisplay(propVar, format, formattedString)

				' Sometimes, the value cannot be displayed properly, such as for blobs
				' or if we get argument exception
				If Not CoreErrorHelper.Succeeded(hr) Then
					formattedString = Nothing
					Return False
				End If
				Return True
			End Using
		End Function

		''' <summary>
		''' Returns a formatted, Unicode string representation of a property value.
		''' </summary>
		''' <param name="format">One or more of the PropertyDescriptionFormat flags 
		''' that indicate the desired format.</param>
		''' <returns>The formatted value as a string, or null if this property 
		''' cannot be formatted for display.</returns>
		Public Function FormatForDisplay(format As PropertyDescriptionFormatOptions) As String Implements IShellProperty.FormatForDisplay
			Dim formattedString As String

			If Description Is Nothing OrElse Description.NativePropertyDescription Is Nothing Then
				' We cannot do anything without a property description
				Return Nothing
			End If

			Dim store As IPropertyStore = ShellPropertyCollection.CreateDefaultPropertyStore(ParentShellObject)

			Using propVar As New PropVariant()
				store.GetValue(m_propertyKey, propVar)

				' Release the Propertystore
				Marshal.ReleaseComObject(store)
				store = Nothing

				Dim hr As HResult = Description.NativePropertyDescription.FormatForDisplay(propVar, format, formattedString)

				' Sometimes, the value cannot be displayed properly, such as for blobs
				' or if we get argument exception
				If Not CoreErrorHelper.Succeeded(hr) Then
					Throw New ShellException(hr)
				End If

				Return formattedString
			End Using
		End Function

		''' <summary>
		''' Get the property description object.
		''' </summary>
		Public ReadOnly Property Description() As ShellPropertyDescription Implements IShellProperty.Description
			Get
				Return m_description
			End Get
		End Property

		''' <summary>
		''' Gets the case-sensitive name of a property as it is known to the system,
		''' regardless of its localized name.
		''' </summary>
		Public ReadOnly Property CanonicalName() As String Implements IShellProperty.CanonicalName
			Get
				Return Description.CanonicalName
			End Get
		End Property

		''' <summary>
		''' Clears the value of the property.
		''' </summary>
		Public Sub ClearValue()
			Using propVar As New PropVariant()
				StorePropVariantValue(propVar)
			End Using
		End Sub

		''' <summary>
		''' Gets the value for this property using the generic Object type.
		''' To obtain a specific type for this value, use the more type strong
		''' Property&lt;T&gt; class.
		''' Also, you can only set a value for this type using Property&lt;T&gt;
		''' </summary>
		Public ReadOnly Property ValueAsObject() As Object Implements IShellProperty.ValueAsObject
			Get
				Using propVar As New PropVariant()
					If ParentShellObject IsNot Nothing Then

						Dim store As IPropertyStore = ShellPropertyCollection.CreateDefaultPropertyStore(ParentShellObject)

						store.GetValue(m_propertyKey, propVar)

						Marshal.ReleaseComObject(store)
						store = Nothing
					ElseIf NativePropertyStore IsNot Nothing Then
						NativePropertyStore.GetValue(m_propertyKey, propVar)
					End If

					Return If(propVar IsNot Nothing, propVar.Value, Nothing)
				End Using
			End Get
		End Property

		''' <summary>
		''' Gets the associated runtime type.
		''' </summary>
		Public ReadOnly Property ValueType() As Type Implements IShellProperty.ValueType
			Get
				' The type for this object need to match that of the description
				System.Diagnostics.Debug.Assert(Description.ValueType Is GetType(T))

				Return Description.ValueType
			End Get
		End Property

		''' <summary>
		''' Gets the image reference path and icon index associated with a property value (Windows 7 only).
		''' </summary>
		Public ReadOnly Property IconReference() As IconReference Implements IShellProperty.IconReference
			Get
				If Not CoreHelpers.RunningOnWin7 Then
					Throw New PlatformNotSupportedException(GlobalLocalizedMessages.ShellPropertyWindows7)
				End If

				GetImageReference()
				Dim index As Integer = (If(imageReferenceIconIndex.HasValue, imageReferenceIconIndex.Value, -1))

				Return New IconReference(imageReferencePath, index)
			End Get
		End Property

		''' <summary>
		''' Gets or sets a value that determines if a value can be truncated. The default for this property is false.
		''' </summary>
		''' <remarks>
		''' An <see cref="ArgumentOutOfRangeException"/> will be thrown if
		''' this property is not set to true, and a property value was set
		''' but later truncated. 
		''' 
		''' </remarks>
		Public Property AllowSetTruncatedValue() As Boolean
			Get
				Return m_AllowSetTruncatedValue
			End Get
			Set
				m_AllowSetTruncatedValue = Value
			End Set
		End Property
		Private m_AllowSetTruncatedValue As Boolean

		#End Region
	End Class
End Namespace
