'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Runtime.InteropServices
Imports Microsoft.Windows.Shell
Imports Microsoft.Windows.Shell.PropertySystem
Imports Microsoft.Windows.Internal

Namespace Taskbar
	''' <summary>
	''' Represents a separator in the user task list. The JumpListSeparator control
	''' can only be used in a user task list.
	''' </summary>
	Public Class JumpListSeparator
		Inherits JumpListTask
		Implements IDisposable
		Friend Shared PKEY_AppUserModel_IsDestListSeparator As PropertyKey = SystemProperties.System.AppUserModel.IsDestinationListSeparator

		Private nativePropertyStore As IPropertyStore
		Private m_nativeShellLink As IShellLinkW
		''' <summary>
		''' Gets an IShellLinkW representation of this object
		''' </summary>
		Friend Overrides ReadOnly Property NativeShellLink() As IShellLinkW
			Get
				If m_nativeShellLink IsNot Nothing Then
					Marshal.ReleaseComObject(m_nativeShellLink)
					m_nativeShellLink = Nothing
				End If

				m_nativeShellLink = DirectCast(New CShellLink(), IShellLinkW)

				If nativePropertyStore IsNot Nothing Then
					Marshal.ReleaseComObject(nativePropertyStore)
					nativePropertyStore = Nothing
				End If

				nativePropertyStore = DirectCast(m_nativeShellLink, IPropertyStore)

				Using propVariant As New PropVariant(True)
					Dim result As HResult = nativePropertyStore.SetValue(PKEY_AppUserModel_IsDestListSeparator, propVariant)
					If Not CoreErrorHelper.Succeeded(result) Then
						Throw New ShellException(result)
					End If
					nativePropertyStore.Commit()
				End Using

				Return m_nativeShellLink
				

			End Get
		End Property

		#Region "IDisposable Members"

		''' <summary>
		''' Release the native and managed objects
		''' </summary>
		''' <param name="disposing">Indicates that this is being called from Dispose(), rather than the finalizer.</param>
		Protected Overridable Sub Dispose(disposing As Boolean)
			If nativePropertyStore IsNot Nothing Then
				Marshal.ReleaseComObject(nativePropertyStore)
				nativePropertyStore = Nothing
			End If

			If m_nativeShellLink IsNot Nothing Then
				Marshal.ReleaseComObject(m_nativeShellLink)
				m_nativeShellLink = Nothing
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
