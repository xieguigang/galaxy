'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.Linq
Imports System.Runtime.InteropServices
Imports System.Runtime.InteropServices.ComTypes
Imports Microsoft.Windows.Resources
Imports Microsoft.Windows.Internal

Namespace Shell.PropertySystem
	''' <summary>
	''' Creates a readonly collection of IProperty objects.
	''' </summary>
	Public Class ShellPropertyCollection
		Inherits ReadOnlyCollection(Of IShellProperty)
		Implements IDisposable
#Region "Internal Constructor"

        ''' <summary>
        ''' Creates a new Property collection given an IPropertyStore object
        ''' </summary>
        ''' <param name="nativePropertyStore__1">IPropertyStore</param>
        Friend Sub New(nativePropertyStore__1 As IPropertyStore)
			MyBase.New(New List(Of IShellProperty)())
			NativePropertyStore = nativePropertyStore__1
			AddProperties(nativePropertyStore__1)
		End Sub

		#End Region

		#Region "Public Constructor"

		''' <summary>
		''' Creates a new Property collection given an IShellItem2 native interface
		''' </summary>
		''' <param name="parent">Parent ShellObject</param>
		Public Sub New(parent As ShellObject)
			MyBase.New(New List(Of IShellProperty)())
			ParentShellObject = parent
			Dim nativePropertyStore As IPropertyStore = Nothing
			Try
				nativePropertyStore = CreateDefaultPropertyStore(ParentShellObject)
				AddProperties(nativePropertyStore)
			Catch
				If parent IsNot Nothing Then
					parent.Dispose()
				End If
				Throw
			Finally
				If nativePropertyStore IsNot Nothing Then
					Marshal.ReleaseComObject(nativePropertyStore)
					nativePropertyStore = Nothing
				End If
			End Try
		End Sub

		''' <summary>
		''' Creates a new <c>ShellPropertyCollection</c> object with the specified file or folder path.
		''' </summary>
		''' <param name="path">The path to the file or folder.</param>        
		Public Sub New(path As String)
			Me.New(ShellObjectFactory.Create(path))
		End Sub

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

		Private Sub AddProperties(nativePropertyStore__1 As IPropertyStore)
			Dim propertyCount As UInteger
			Dim propKey As PropertyKey

			' Populate the property collection
			nativePropertyStore__1.GetCount(propertyCount)
            For i As UInteger = 0 To propertyCount - 1UI
                nativePropertyStore__1.GetAt(i, propKey)

                If ParentShellObject IsNot Nothing Then
                    Items.Add(ParentShellObject.Properties.CreateTypedProperty(propKey))
                Else
                    Items.Add(CreateTypedProperty(propKey, NativePropertyStore))
                End If
            Next
        End Sub

		Friend Shared Function CreateDefaultPropertyStore(shellObj As ShellObject) As IPropertyStore
			Dim nativePropertyStore As IPropertyStore = Nothing

			Dim guid As New Guid(ShellIIDGuid.IPropertyStore)
			Dim hr As Integer = shellObj.NativeShellItem2.GetPropertyStore(ShellNativeMethods.GetPropertyStoreOptions.BestEffort, guid, nativePropertyStore)

			' throw on failure 
			If nativePropertyStore Is Nothing OrElse Not CoreErrorHelper.Succeeded(hr) Then
				Throw New ShellException(hr)
			End If

			Return nativePropertyStore
		End Function




		#End Region

		#Region "Collection Public Methods"

		''' <summary>
		''' Checks if a property with the given canonical name is available.
		''' </summary>
		''' <param name="canonicalName">The canonical name of the property.</param>
		''' <returns><B>True</B> if available, <B>false</B> otherwise.</returns>
		Public Overloads Function Contains(canonicalName As String) As Boolean
			If String.IsNullOrEmpty(canonicalName) Then
				Throw New ArgumentException(LocalizedMessages.PropertyCollectionNullCanonicalName, "canonicalName")
			End If

			Return Items.Any(Function(p) p.CanonicalName = canonicalName)

		End Function

		''' <summary>
		''' Checks if a property with the given property key is available.
		''' </summary>
		''' <param name="key">The property key.</param>
		''' <returns><B>True</B> if available, <B>false</B> otherwise.</returns>
		Public Overloads Function Contains(key As PropertyKey) As Boolean
			Return Items.Any(Function(p) p.PropertyKey = key)
		End Function

        ''' <summary>
        ''' Gets the property associated with the supplied canonical name string.
        ''' The canonical name property is case-sensitive.
        ''' 
        ''' </summary>
        ''' <param name="canonicalName">The canonical name.</param>
        ''' <returns>The property associated with the canonical name, if found.</returns>
        ''' <exception cref="IndexOutOfRangeException">Throws IndexOutOfRangeException 
        ''' if no matching property is found.</exception>
        Default Public Overloads ReadOnly Property Item(canonicalName As String) As IShellProperty
            Get
                If String.IsNullOrEmpty(canonicalName) Then
                    Throw New ArgumentException(LocalizedMessages.PropertyCollectionNullCanonicalName, "canonicalName")
                End If

                Dim prop As IShellProperty = Items.FirstOrDefault(Function(p) p.CanonicalName = canonicalName)
                If prop Is Nothing Then
                    Throw New IndexOutOfRangeException(LocalizedMessages.PropertyCollectionCanonicalInvalidIndex)
                End If
                Return prop
            End Get
        End Property

        ''' <summary>
        ''' Gets a property associated with the supplied property key.
        ''' 
        ''' </summary>
        ''' <param name="key">The property key.</param>
        ''' <returns>The property associated with the property key, if found.</returns>
        ''' <exception cref="IndexOutOfRangeException">Throws IndexOutOfRangeException 
        ''' if no matching property is found.</exception>
        Default Public Overloads ReadOnly Property Item(key As PropertyKey) As IShellProperty
            Get
                Dim prop As IShellProperty = Items.FirstOrDefault(Function(p) p.PropertyKey = key)
                If prop IsNot Nothing Then
                    Return prop
                End If

                Throw New IndexOutOfRangeException(LocalizedMessages.PropertyCollectionInvalidIndex)
            End Get
        End Property

#End Region

        ' TODO - ShellProperties.cs also has a similar class that is used for creating
        ' a ShellObject specific IShellProperty. These 2 methods should be combined or moved to a 
        ' common location.
        Friend Shared Function CreateTypedProperty(propKey As PropertyKey, NativePropertyStore As IPropertyStore) As IShellProperty
			Return ShellPropertyFactory.CreateShellProperty(propKey, NativePropertyStore)
		End Function

		#Region "IDisposable Members"

		''' <summary>
		''' Release the native and managed objects
		''' </summary>
		''' <param name="disposing">Indicates that this is being called from Dispose(), rather than the finalizer.</param>
		Protected Overridable Sub Dispose(disposing As Boolean)
			If NativePropertyStore IsNot Nothing Then
				Marshal.ReleaseComObject(NativePropertyStore)
				NativePropertyStore = Nothing
			End If
		End Sub

		''' <summary>
		''' Release the native objects.
		''' </summary>
		Public Sub Dispose() Implements IDisposable.Dispose
			Dispose(True)
			GC.SuppressFinalize(Me)
		End Sub

		''' <summary>
		''' Implement the finalizer.
		''' </summary>
		Protected Overrides Sub Finalize()
			Try
				Dispose(False)
			Finally
				MyBase.Finalize()
			End Try
		End Sub

		#End Region
	End Class
End Namespace
