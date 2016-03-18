'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Diagnostics
Imports System.Runtime.InteropServices

Namespace Shell
	''' <summary>
	''' Represents a registered file system Known Folder
	''' </summary>
	Public Class FileSystemKnownFolder
		Inherits ShellFileSystemFolder
		Implements IKnownFolder
		Implements IDisposable
		#Region "Private Fields"

		Private knownFolderNative As IKnownFolderNative
		Private m_knownFolderSettings As KnownFolderSettings

		#End Region

		#Region "Internal Constructors"

		Friend Sub New(shellItem As IShellItem2)
			MyBase.New(shellItem)
		End Sub

		Friend Sub New(kf As IKnownFolderNative)
			Debug.Assert(kf IsNot Nothing)
			knownFolderNative = kf

			' Set the native shell item
			' and set it on the base class (ShellObject)
			Dim guid As New Guid(ShellIIDGuid.IShellItem2)
            knownFolderNative.GetShellItem(0, guid, DirectCast(NativeShellItem, IShellItem2))
        End Sub

		#End Region

		#Region "Private Members"

		Private ReadOnly Property KnownFolderSettings() As KnownFolderSettings
			Get
				If knownFolderNative Is Nothing Then
					' We need to get the PIDL either from the NativeShellItem,
					' or from base class's property (if someone already set it on us).
					' Need to use the PIDL to get the native IKnownFolder interface.

					' Get the PIDL for the ShellItem
					If nativeShellItem IsNot Nothing AndAlso MyBase.PIDL = IntPtr.Zero Then
						MyBase.PIDL = ShellHelper.PidlFromShellItem(nativeShellItem)
					End If

					' If we have a valid PIDL, get the native IKnownFolder
					If MyBase.PIDL <> IntPtr.Zero Then
						knownFolderNative = KnownFolderHelper.FromPIDL(MyBase.PIDL)
					End If

					Debug.Assert(knownFolderNative IsNot Nothing)
				End If

				' If this is the first time this property is being called,
				' get the native Folder Defination (KnownFolder properties)
				If m_knownFolderSettings Is Nothing Then
					m_knownFolderSettings = New KnownFolderSettings(knownFolderNative)
				End If

				Return m_knownFolderSettings
			End Get
		End Property

		#End Region

		#Region "IKnownFolder Members"

		''' <summary>
		''' Gets the path for this known folder.
		''' </summary>
		''' <value>A <see cref="System.String"/> object.</value>
		Public Overrides ReadOnly Property Path() As String Implements IKnownFolder.Path
			Get
				Return KnownFolderSettings.Path
			End Get
		End Property

		''' <summary>
		''' Gets the category designation for this known folder.
		''' </summary>
		''' <value>A <see cref="FolderCategory"/> value.</value>
		Public ReadOnly Property Category() As FolderCategory Implements IKnownFolder.Category
			Get
				Return KnownFolderSettings.Category
			End Get
		End Property

		''' <summary>
		''' Gets this known folder's canonical name.
		''' </summary>
		''' <value>A <see cref="System.String"/> object.</value>
		Public ReadOnly Property CanonicalName() As String Implements IKnownFolder.CanonicalName
			Get
				Return KnownFolderSettings.CanonicalName
			End Get
		End Property

		''' <summary>
		''' Gets this known folder's description.
		''' </summary>
		''' <value>A <see cref="System.String"/> object.</value>
		Public ReadOnly Property Description() As String Implements IKnownFolder.Description
			Get
				Return KnownFolderSettings.Description
			End Get
		End Property

		''' <summary>
		''' Gets the unique identifier for this known folder's parent folder.
		''' </summary>
		''' <value>A <see cref="System.Guid"/> value.</value>
		Public ReadOnly Property ParentId() As Guid Implements IKnownFolder.ParentId
			Get
				Return KnownFolderSettings.ParentId
			End Get
		End Property

		''' <summary>
		''' Gets this known folder's relative path.
		''' </summary>
		''' <value>A <see cref="System.String"/> object.</value>
		Public ReadOnly Property RelativePath() As String Implements IKnownFolder.RelativePath
			Get
				Return KnownFolderSettings.RelativePath
			End Get
		End Property

        ''' <summary>
        ''' Gets this known folder's parsing name.
        ''' </summary>
        ''' <value>A <see cref="System.String"/> object.</value>
        Public Overrides Property ParsingName() As String Implements IKnownFolder.ParsingName
            Get
                Return MyBase.ParsingName
            End Get
            Protected Set(value As String)
                MyBase.ParsingName = value
            End Set
        End Property

        ''' <summary>
        ''' Gets this known folder's tool tip text.
        ''' </summary>
        ''' <value>A <see cref="System.String"/> object.</value>
        Public ReadOnly Property Tooltip() As String Implements IKnownFolder.Tooltip
			Get
				Return KnownFolderSettings.Tooltip
			End Get
		End Property
		''' <summary>
		''' Gets the resource identifier for this 
		''' known folder's tool tip text.
		''' </summary>
		''' <value>A <see cref="System.String"/> object.</value>
		Public ReadOnly Property TooltipResourceId() As String Implements IKnownFolder.TooltipResourceId
			Get
				Return KnownFolderSettings.TooltipResourceId
			End Get
		End Property

		''' <summary>
		''' Gets this known folder's localized name.
		''' </summary>
		''' <value>A <see cref="System.String"/> object.</value>
		Public ReadOnly Property LocalizedName() As String Implements IKnownFolder.LocalizedName
			Get
				Return KnownFolderSettings.LocalizedName
			End Get
		End Property
		''' <summary>
		''' Gets the resource identifier for this 
		''' known folder's localized name.
		''' </summary>
		''' <value>A <see cref="System.String"/> object.</value>
		Public ReadOnly Property LocalizedNameResourceId() As String Implements IKnownFolder.LocalizedNameResourceId
			Get
				Return KnownFolderSettings.LocalizedNameResourceId
			End Get
		End Property

		''' <summary>
		''' Gets this known folder's security attributes.
		''' </summary>
		''' <value>A <see cref="System.String"/> object.</value>
		Public ReadOnly Property Security() As String Implements IKnownFolder.Security
			Get
				Return KnownFolderSettings.Security
			End Get
		End Property

		''' <summary>
		''' Gets this known folder's file attributes, 
		''' such as "read-only".
		''' </summary>
		''' <value>A <see cref="System.IO.FileAttributes"/> value.</value>
		Public ReadOnly Property FileAttributes() As System.IO.FileAttributes Implements IKnownFolder.FileAttributes
			Get
				Return KnownFolderSettings.FileAttributes
			End Get
		End Property

		''' <summary>
		''' Gets an value that describes this known folder's behaviors.
		''' </summary>
		''' <value>A <see cref="DefinitionOptions"/> value.</value>
		Public ReadOnly Property DefinitionOptions() As DefinitionOptions Implements IKnownFolder.DefinitionOptions
			Get
				Return KnownFolderSettings.DefinitionOptions
			End Get
		End Property

		''' <summary>
		''' Gets the unique identifier for this known folder's type.
		''' </summary>
		''' <value>A <see cref="System.Guid"/> value.</value>
		Public ReadOnly Property FolderTypeId() As Guid Implements IKnownFolder.FolderTypeId
			Get
				Return KnownFolderSettings.FolderTypeId
			End Get
		End Property

		''' <summary>
		''' Gets a string representation of this known folder's type.
		''' </summary>
		''' <value>A <see cref="System.String"/> object.</value>
		Public ReadOnly Property FolderType() As String Implements IKnownFolder.FolderType
			Get
				Return KnownFolderSettings.FolderType
			End Get
		End Property
		''' <summary>
		''' Gets the unique identifier for this known folder.
		''' </summary>
		''' <value>A <see cref="System.Guid"/> value.</value>
		Public ReadOnly Property FolderId() As Guid Implements IKnownFolder.FolderId
			Get
				Return KnownFolderSettings.FolderId
			End Get
		End Property

		''' <summary>
		''' Gets a value that indicates whether this known folder's path exists on the computer. 
		''' </summary>
		''' <value>A bool<see cref="System.Boolean"/> value.</value>
		''' <remarks>If this property value is <b>false</b>, 
		''' the folder might be a virtual folder (<see cref="Category"/> property will
		''' be <see cref="FolderCategory.Virtual"/> for virtual folders)</remarks>
		Public ReadOnly Property PathExists() As Boolean Implements IKnownFolder.PathExists
			Get
				Return KnownFolderSettings.PathExists
			End Get
		End Property

		''' <summary>
		''' Gets a value that states whether this known folder 
		''' can have its path set to a new value, 
		''' including any restrictions on the redirection.
		''' </summary>
		''' <value>A <see cref="RedirectionCapability"/> value.</value>
		Public ReadOnly Property Redirection() As RedirectionCapability Implements IKnownFolder.Redirection
			Get
				Return KnownFolderSettings.Redirection
			End Get
		End Property

		#End Region

		#Region "IDisposable Members"

		''' <summary>
		''' Release resources
		''' </summary>
		''' <param name="disposing">Indicates that this mothod is being called from Dispose() rather than the finalizer.</param>
		Protected Overrides Sub Dispose(disposing As Boolean)
			If disposing Then
				m_knownFolderSettings = Nothing
			End If

			If knownFolderNative IsNot Nothing Then
				Marshal.ReleaseComObject(knownFolderNative)
				knownFolderNative = Nothing
			End If

			MyBase.Dispose(disposing)
		End Sub

		#End Region
	End Class
End Namespace
