'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Runtime.InteropServices
Imports Microsoft.Windows.Internal
Imports Microsoft.Windows.Resources

Namespace Shell.PropertySystem
	''' <summary>
	''' Creates a property writer capable of setting multiple properties for a given ShellObject.
	''' </summary>
	Public Class ShellPropertyWriter
		Implements IDisposable

		Private m_parentShellObject As ShellObject

		' Reference to our writable PropertyStore
		Friend writablePropStore As IPropertyStore

		Friend Sub New(parent As ShellObject)
			ParentShellObject = parent

			' Open the property store for this shell object...
			Dim guid As New Guid(ShellIIDGuid.IPropertyStore)

			Try
				Dim hr As Integer = ParentShellObject.NativeShellItem2.GetPropertyStore(ShellNativeMethods.GetPropertyStoreOptions.ReadWrite, guid, writablePropStore)

				If Not CoreErrorHelper.Succeeded(hr) Then
					Throw New PropertySystemException(LocalizedMessages.ShellPropertyUnableToGetWritableProperty, Marshal.GetExceptionForHR(hr))
				Else
					' If we succeed in creating a valid property store for this ShellObject,
					' then set it on the parent shell object for others to use.
					' Once this writer is closed/commited, we will set the 
					If ParentShellObject.NativePropertyStore Is Nothing Then
						ParentShellObject.NativePropertyStore = writablePropStore
					End If

				End If
			Catch e As InvalidComObjectException
				Throw New PropertySystemException(LocalizedMessages.ShellPropertyUnableToGetWritableProperty, e)
			Catch generatedExceptionName As InvalidCastException
				Throw New PropertySystemException(LocalizedMessages.ShellPropertyUnableToGetWritableProperty)
			End Try
		End Sub

		''' <summary>
		''' Reference to parent ShellObject (associated with this writer)
		''' </summary>
		Protected Property ParentShellObject() As ShellObject
			Get
				Return m_parentShellObject
			End Get
			Private Set
				m_parentShellObject = value
			End Set
		End Property

		''' <summary>
		''' Writes the given property key and value.
		''' </summary>
		''' <param name="key">The property key.</param>
		''' <param name="value">The value associated with the key.</param>
		Public Sub WriteProperty(key As PropertyKey, value As Object)
			WriteProperty(key, value, True)
		End Sub

		''' <summary>
		''' Writes the given property key and value. To allow truncation of the given value, set allowTruncatedValue
		''' to true.
		''' </summary>
		''' <param name="key">The property key.</param>
		''' <param name="value">The value associated with the key.</param>
		''' <param name="allowTruncatedValue">True to allow truncation (default); otherwise False.</param>
		''' <exception cref="System.InvalidOperationException">If the writable property store is already 
		''' closed.</exception>
		''' <exception cref="System.ArgumentOutOfRangeException">If AllowTruncatedValue is set to false 
		''' and while setting the value on the property it had to be truncated in a string or rounded in 
		''' a numeric value.</exception>
		Public Sub WriteProperty(key As PropertyKey, value As Object, allowTruncatedValue As Boolean)
			If writablePropStore Is Nothing Then
				Throw New InvalidOperationException("Writeable store has been closed.")
			End If

			Using propVar As PropVariant = PropVariant.FromObject(value)
				Dim result As HResult = writablePropStore.SetValue(key, propVar)

				If Not allowTruncatedValue AndAlso (CInt(result) = ShellNativeMethods.InPlaceStringTruncated) Then
					' At this point we can't revert back the commit
					' so don't commit, close the property store and throw an exception
					' to let the user know.
					Marshal.ReleaseComObject(writablePropStore)
					writablePropStore = Nothing

					Throw New ArgumentOutOfRangeException("value", LocalizedMessages.ShellPropertyValueTruncated)
				End If

				If Not CoreErrorHelper.Succeeded(result) Then
					Throw New PropertySystemException(LocalizedMessages.ShellPropertySetValue, Marshal.GetExceptionForHR(CInt(result)))
				End If
			End Using
		End Sub

		''' <summary>
		''' Writes the specified property given the canonical name and a value.
		''' </summary>
		''' <param name="canonicalName">The canonical name.</param>
		''' <param name="value">The property value.</param>
		Public Sub WriteProperty(canonicalName As String, value As Object)
			WriteProperty(canonicalName, value, True)
		End Sub

		''' <summary>
		''' Writes the specified property given the canonical name and a value. To allow truncation of the given value, set allowTruncatedValue
		''' to true.
		''' </summary>
		''' <param name="canonicalName">The canonical name.</param>
		''' <param name="value">The property value.</param>
		''' <param name="allowTruncatedValue">True to allow truncation (default); otherwise False.</param>
		''' <exception cref="System.ArgumentException">If the given canonical name is not valid.</exception>
		Public Sub WriteProperty(canonicalName As String, value As Object, allowTruncatedValue As Boolean)
			' Get the PropertyKey using the canonicalName passed in
			Dim propKey As PropertyKey

			Dim result As Integer = PropertySystemNativeMethods.PSGetPropertyKeyFromName(canonicalName, propKey)

			If Not CoreErrorHelper.Succeeded(result) Then
				Throw New ArgumentException(LocalizedMessages.ShellInvalidCanonicalName, Marshal.GetExceptionForHR(result))
			End If

			WriteProperty(propKey, value, allowTruncatedValue)
		End Sub

		''' <summary>
		''' Writes the specified property using an IShellProperty and a value.
		''' </summary>
		''' <param name="shellProperty">The property name.</param>
		''' <param name="value">The property value.</param>
		Public Sub WriteProperty(shellProperty As IShellProperty, value As Object)
			WriteProperty(shellProperty, value, True)
		End Sub

		''' <summary>
		''' Writes the specified property given an IShellProperty and a value. To allow truncation of the given value, set allowTruncatedValue
		''' to true.
		''' </summary>
		''' <param name="shellProperty">The property name.</param>
		''' <param name="value">The property value.</param>
		''' <param name="allowTruncatedValue">True to allow truncation (default); otherwise False.</param>
		Public Sub WriteProperty(shellProperty As IShellProperty, value As Object, allowTruncatedValue As Boolean)
			If shellProperty Is Nothing Then
				Throw New ArgumentNullException("shellProperty")
			End If
			WriteProperty(shellProperty.PropertyKey, value, allowTruncatedValue)
		End Sub

		''' <summary>
		''' Writes the specified property using a strongly-typed ShellProperty and a value.
		''' </summary>
		''' <typeparam name="T">The type of the property name.</typeparam>
		''' <param name="shellProperty">The property name.</param>
		''' <param name="value">The property value.</param>
		Public Sub WriteProperty(Of T)(shellProperty As ShellProperty(Of T), value As T)
			WriteProperty(Of T)(shellProperty, value, True)
		End Sub
		''' <summary>
		''' Writes the specified property given a strongly-typed ShellProperty and a value. To allow truncation of the given value, set allowTruncatedValue
		''' to true.
		''' </summary>
		''' <typeparam name="T">The type of the property name.</typeparam>
		''' <param name="shellProperty">The property name.</param>
		''' <param name="value">The property value.</param>
		''' <param name="allowTruncatedValue">True to allow truncation (default); otherwise False.</param>
		Public Sub WriteProperty(Of T)(shellProperty As ShellProperty(Of T), value As T, allowTruncatedValue As Boolean)
			If shellProperty Is Nothing Then
				Throw New ArgumentNullException("shellProperty")
			End If
			WriteProperty(shellProperty.PropertyKey, value, allowTruncatedValue)
		End Sub

		#Region "IDisposable Members"

		''' <summary>
		''' Release the native objects.
		''' </summary>
		Public Sub Dispose() Implements IDisposable.Dispose
			Dispose(True)
			GC.SuppressFinalize(Me)
		End Sub

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
		''' Release the native and managed objects.
		''' </summary>
		''' <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.
		''' </param>
		Protected Overridable Sub Dispose(disposing As Boolean)
			Close()
		End Sub

		''' <summary>
		''' Call this method to commit the writes (calls to WriteProperty method)
		''' and dispose off the writer.
		''' </summary>
		Public Sub Close()
			' Close the property writer (commit, etc)
			If writablePropStore IsNot Nothing Then
				writablePropStore.Commit()

				Marshal.ReleaseComObject(writablePropStore)
				writablePropStore = Nothing
			End If

			ParentShellObject.NativePropertyStore = Nothing
		End Sub

		#End Region
	End Class
End Namespace
