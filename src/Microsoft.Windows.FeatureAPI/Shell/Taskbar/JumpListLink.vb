'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Runtime.InteropServices
Imports Microsoft.Windows.Internal
Imports Microsoft.Windows.Shell
Imports Microsoft.Windows.Shell.PropertySystem

Namespace Taskbar
	''' <summary>
	''' Represents a jump list link object.
	''' </summary>
	Public Class JumpListLink
		Inherits JumpListTask
		Implements IJumpListItem
		Implements IDisposable
		Friend Shared PKEY_Title As PropertyKey = SystemProperties.System.Title

		''' <summary>
		''' Initializes a new instance of a JumpListLink with the specified path.
		''' </summary>
		''' <param name="pathValue">The path to the item. The path is required for the JumpList Link</param>
		''' <param name="titleValue">The title for the JumpListLink item. The title is required for the JumpList link.</param>
		Public Sub New(pathValue As String, titleValue As String)
			If String.IsNullOrEmpty(pathValue) Then
				Throw New ArgumentNullException("pathValue", LocalizedMessages.JumpListLinkPathRequired)
			End If

			If String.IsNullOrEmpty(titleValue) Then
				Throw New ArgumentNullException("titleValue", LocalizedMessages.JumpListLinkTitleRequired)
			End If

			Path = pathValue
			Title = titleValue
		End Sub

		Private m_title As String
		''' <summary>
		''' Gets or sets the link's title
		''' </summary>
		Public Property Title() As String
			Get
				Return m_title
			End Get
			Set
				If String.IsNullOrEmpty(value) Then
					Throw New ArgumentNullException("value", LocalizedMessages.JumpListLinkTitleRequired)
				End If

				m_title = value
			End Set
		End Property

		Private m_path As String
		''' <summary>
		''' Gets or sets the link's path
		''' </summary>
		Public Property Path() As String Implements IJumpListItem.Path
			Get
				Return m_path
			End Get
			Set
				If String.IsNullOrEmpty(value) Then
					Throw New ArgumentNullException("value", LocalizedMessages.JumpListLinkTitleRequired)
				End If

				m_path = value
			End Set
		End Property

		''' <summary>
		''' Gets or sets the icon reference (location and index) of the link's icon.
		''' </summary>
		Public Property IconReference() As IconReference
			Get
				Return m_IconReference
			End Get
			Set
				m_IconReference = Value
			End Set
		End Property
		Private m_IconReference As IconReference

		''' <summary>
		''' Gets or sets the object's arguments (passed to the command line).
		''' </summary>
		Public Property Arguments() As String
			Get
				Return m_Arguments
			End Get
			Set
				m_Arguments = Value
			End Set
		End Property
		Private m_Arguments As String

		''' <summary>
		''' Gets or sets the object's working directory.
		''' </summary>
		Public Property WorkingDirectory() As String
			Get
				Return m_WorkingDirectory
			End Get
			Set
				m_WorkingDirectory = Value
			End Set
		End Property
		Private m_WorkingDirectory As String

		''' <summary>
		''' Gets or sets the show command of the lauched application.
		''' </summary>
		Public Property ShowCommand() As WindowShowCommand
			Get
				Return m_ShowCommand
			End Get
			Set
				m_ShowCommand = Value
			End Set
		End Property
		Private m_ShowCommand As WindowShowCommand

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

				m_nativeShellLink.SetPath(Path)

				If Not String.IsNullOrEmpty(IconReference.ModuleName) Then
					m_nativeShellLink.SetIconLocation(IconReference.ModuleName, IconReference.ResourceId)
				End If

				If Not String.IsNullOrEmpty(Arguments) Then
					m_nativeShellLink.SetArguments(Arguments)
				End If

				If Not String.IsNullOrEmpty(WorkingDirectory) Then
					m_nativeShellLink.SetWorkingDirectory(WorkingDirectory)
				End If

				m_nativeShellLink.SetShowCmd(CUInt(ShowCommand))

				Using propVariant As New PropVariant(Title)
					Dim result As HResult = nativePropertyStore.SetValue(PKEY_Title, propVariant)
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
			If disposing Then
				m_title = Nothing
			End If

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
