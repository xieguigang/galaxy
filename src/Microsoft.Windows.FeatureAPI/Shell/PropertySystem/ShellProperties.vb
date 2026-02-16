'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Runtime.InteropServices
Imports Microsoft.Windows.Internal

Namespace Shell.PropertySystem
	''' <summary>
	''' Defines a partial class that implements helper methods for retrieving Shell properties
	''' using a canonical name, property key, or a strongly-typed property. Also provides
	''' access to all the strongly-typed system properties and default properties collections.
	''' </summary>
	Public Partial Class ShellProperties
		Implements IDisposable
		Private Property ParentShellObject() As ShellObject
			Get
				Return m_ParentShellObject
			End Get
			Set
				m_ParentShellObject = Value
			End Set
		End Property
		Private m_ParentShellObject As ShellObject
		Private m_defaultPropertyCollection As ShellPropertyCollection

		Friend Sub New(parent As ShellObject)
			ParentShellObject = parent
		End Sub

		''' <summary>
		''' Returns a property available in the default property collection using 
		''' the given property key.
		''' </summary>
		''' <param name="key">The property key.</param>
		''' <returns>An IShellProperty.</returns>
		Public Function GetProperty(key As PropertyKey) As IShellProperty
			Return CreateTypedProperty(key)
		End Function

		''' <summary>
		''' Returns a property available in the default property collection using 
		''' the given canonical name.
		''' </summary>
		''' <param name="canonicalName">The canonical name.</param>
		''' <returns>An IShellProperty.</returns>
		Public Function GetProperty(canonicalName As String) As IShellProperty
			Return CreateTypedProperty(canonicalName)
		End Function

		''' <summary>
		''' Returns a strongly typed property available in the default property collection using 
		''' the given property key.
		''' </summary>
		''' <typeparam name="T">The type of property to retrieve.</typeparam>
		''' <param name="key">The property key.</param>
		''' <returns>A strongly-typed ShellProperty for the given property key.</returns>
		Public Function GetProperty(Of T)(key As PropertyKey) As ShellProperty(Of T)
			Return TryCast(CreateTypedProperty(key), ShellProperty(Of T))
		End Function

		''' <summary>
		''' Returns a strongly typed property available in the default property collection using 
		''' the given canonical name.
		''' </summary>
		''' <typeparam name="T">The type of property to retrieve.</typeparam>
		''' <param name="canonicalName">The canonical name.</param>
		''' <returns>A strongly-typed ShellProperty for the given canonical name.</returns>
		Public Function GetProperty(Of T)(canonicalName As String) As ShellProperty(Of T)
			Return TryCast(CreateTypedProperty(canonicalName), ShellProperty(Of T))
		End Function

        Private _propertySystem As PropertySystem
        ''' <summary>
        ''' Gets all the properties for the system through an accessor.
        ''' </summary>
        Public ReadOnly Property System() As PropertySystem
			Get
                If _propertySystem Is Nothing Then
                    _propertySystem = New PropertySystem(ParentShellObject)
                End If

                Return _propertySystem
            End Get
		End Property

		''' <summary>
		''' Gets the collection of all the default properties for this item.
		''' </summary>
		Public ReadOnly Property DefaultPropertyCollection() As ShellPropertyCollection
			Get
				If m_defaultPropertyCollection Is Nothing Then
					m_defaultPropertyCollection = New ShellPropertyCollection(ParentShellObject)
				End If

				Return m_defaultPropertyCollection
			End Get
		End Property

		''' <summary>
		''' Returns the shell property writer used when writing multiple properties.
		''' </summary>
		''' <returns>A ShellPropertyWriter.</returns>
		''' <remarks>Use the Using pattern with the returned ShellPropertyWriter or
		''' manually call the Close method on the writer to commit the changes 
		''' and dispose the writer</remarks>
		Public Function GetPropertyWriter() As ShellPropertyWriter
			Return New ShellPropertyWriter(ParentShellObject)
		End Function

		Friend Function CreateTypedProperty(Of T)(propKey As PropertyKey) As IShellProperty
			Dim desc As ShellPropertyDescription = ShellPropertyDescriptionsCache.Cache.GetPropertyDescription(propKey)
			Return New ShellProperty(Of T)(propKey, desc, ParentShellObject)
		End Function

		Friend Function CreateTypedProperty(propKey As PropertyKey) As IShellProperty
			Return ShellPropertyFactory.CreateShellProperty(propKey, ParentShellObject)
		End Function

		Friend Function CreateTypedProperty(canonicalName As String) As IShellProperty
			' Otherwise, call the native PropertyStore method
			Dim propKey As PropertyKey

			Dim result As Integer = PropertySystemNativeMethods.PSGetPropertyKeyFromName(canonicalName, propKey)

			If Not CoreErrorHelper.Succeeded(result) Then
				Throw New ArgumentException(GlobalLocalizedMessages.ShellInvalidCanonicalName, Marshal.GetExceptionForHR(result))
			End If
			Return CreateTypedProperty(propKey)
		End Function

		#Region "IDisposable Members"

		''' <summary>
		''' Cleans up memory
		''' </summary>
		Public Sub Dispose() Implements IDisposable.Dispose
			Dispose(True)
			GC.SuppressFinalize(Me)
		End Sub

		''' <summary>
		''' Cleans up memory
		''' </summary>
		Protected Overridable Sub Dispose(disposed As Boolean)
			If disposed AndAlso m_defaultPropertyCollection IsNot Nothing Then
				m_defaultPropertyCollection.Dispose()
			End If
		End Sub

		#End Region
	End Class
End Namespace
