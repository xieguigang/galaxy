'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Collections.Generic
Imports System.Runtime.InteropServices
Imports Microsoft.Windows.Internal

Namespace Shell
	''' <summary>
	''' Represents the base class for all types of Shell "containers". Any class deriving from this class
	''' can contain other ShellObjects (e.g. ShellFolder, FileSystemKnownFolder, ShellLibrary, etc)
	''' </summary>
	<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification := "This will complicate the class hierarchy and naming convention used in the Shell area")> _
	Public MustInherit Class ShellContainer
		Inherits ShellObject
		Implements IEnumerable(Of ShellObject)
		Implements IDisposable

		#Region "Private Fields"

		Private desktopFolderEnumeration As IShellFolder
		Private m_nativeShellFolder As IShellFolder

		#End Region

		#Region "Internal Properties"

		Friend ReadOnly Property NativeShellFolder() As IShellFolder
			Get
				If m_nativeShellFolder Is Nothing Then
					Dim guid As New Guid(ShellIIDGuid.IShellFolder)
					Dim handler As New Guid(ShellBHIDGuid.ShellFolderObject)

					Dim hr As HResult = NativeShellItem.BindToHandler(IntPtr.Zero, handler, guid, m_nativeShellFolder)

					If CoreErrorHelper.Failed(hr) Then
						Dim str As String = ShellHelper.GetParsingName(NativeShellItem)
						If str IsNot Nothing AndAlso str <> Environment.GetFolderPath(Environment.SpecialFolder.Desktop) Then
							Throw New ShellException(hr)
						End If
					End If
				End If

				Return m_nativeShellFolder
			End Get
		End Property

		#End Region

		#Region "Internal Constructor"

		Friend Sub New()
		End Sub

		Friend Sub New(shellItem As IShellItem2)
			MyBase.New(shellItem)
		End Sub

		#End Region

		#Region "Disposable Pattern"

		''' <summary>
		''' Release resources
		''' </summary>
		''' <param name="disposing"><B>True</B> indicates that this is being called from Dispose(), rather than the finalizer.</param>
		Protected Overrides Sub Dispose(disposing As Boolean)
			If m_nativeShellFolder IsNot Nothing Then
				Marshal.ReleaseComObject(m_nativeShellFolder)
				m_nativeShellFolder = Nothing
			End If

			If desktopFolderEnumeration IsNot Nothing Then
				Marshal.ReleaseComObject(desktopFolderEnumeration)
				desktopFolderEnumeration = Nothing
			End If

			MyBase.Dispose(disposing)
		End Sub

		#End Region

		#Region "IEnumerable<ShellObject> Members"

		''' <summary>
		''' Enumerates through contents of the ShellObjectContainer
		''' </summary>
		''' <returns>Enumerated contents</returns>
		Public Function GetEnumerator() As IEnumerator(Of ShellObject) Implements IEnumerable(Of ShellObject).GetEnumerator
			If NativeShellFolder Is Nothing Then
				If desktopFolderEnumeration Is Nothing Then
					ShellNativeMethods.SHGetDesktopFolder(desktopFolderEnumeration)
				End If

				m_nativeShellFolder = desktopFolderEnumeration
			End If

			Return New ShellFolderItems(Me)
		End Function

		#End Region

		#Region "IEnumerable Members"

		Private Function System_Collections_IEnumerable_GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
			Return New ShellFolderItems(Me)
		End Function

		#End Region
	End Class
End Namespace
