'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Diagnostics
Imports System.Runtime.InteropServices
Imports Microsoft.Windows.Internal

Namespace Shell
	''' <summary>
	''' Internal class to represent the KnownFolder settings/properties
	''' </summary>
	Friend Class KnownFolderSettings
		Private knownFolderProperties As FolderProperties

		Friend Sub New(knownFolderNative As IKnownFolderNative)
			GetFolderProperties(knownFolderNative)
		End Sub

		#Region "Private Methods"

		''' <summary>
		''' Populates a structure that contains 
		''' this known folder's properties.
		''' </summary>
		Private Sub GetFolderProperties(knownFolderNative As IKnownFolderNative)
			Debug.Assert(knownFolderNative IsNot Nothing)

			Dim nativeFolderDefinition As KnownFoldersSafeNativeMethods.NativeFolderDefinition
			knownFolderNative.GetFolderDefinition(nativeFolderDefinition)

			Try
				knownFolderProperties.category = nativeFolderDefinition.category
				knownFolderProperties.canonicalName = Marshal.PtrToStringUni(nativeFolderDefinition.name)
				knownFolderProperties.description = Marshal.PtrToStringUni(nativeFolderDefinition.description)
				knownFolderProperties.parentId = nativeFolderDefinition.parentId
				knownFolderProperties.relativePath = Marshal.PtrToStringUni(nativeFolderDefinition.relativePath)
				knownFolderProperties.parsingName = Marshal.PtrToStringUni(nativeFolderDefinition.parsingName)
				knownFolderProperties.tooltipResourceId = Marshal.PtrToStringUni(nativeFolderDefinition.tooltip)
				knownFolderProperties.localizedNameResourceId = Marshal.PtrToStringUni(nativeFolderDefinition.localizedName)
				knownFolderProperties.iconResourceId = Marshal.PtrToStringUni(nativeFolderDefinition.icon)
				knownFolderProperties.security = Marshal.PtrToStringUni(nativeFolderDefinition.security)
				knownFolderProperties.fileAttributes = CType(nativeFolderDefinition.attributes, System.IO.FileAttributes)
				knownFolderProperties.definitionOptions = nativeFolderDefinition.definitionOptions
				knownFolderProperties.folderTypeId = nativeFolderDefinition.folderTypeId
				knownFolderProperties.folderType = FolderTypes.GetFolderType(knownFolderProperties.folderTypeId)

				Dim pathExists As Boolean
				knownFolderProperties.path = GetPath(pathExists, knownFolderNative)
				knownFolderProperties.pathExists = pathExists

				knownFolderProperties.redirection = knownFolderNative.GetRedirectionCapabilities()

				' Turn tooltip, localized name and icon resource IDs 
				' into the actual resources.
				knownFolderProperties.tooltip = CoreHelpers.GetStringResource(knownFolderProperties.tooltipResourceId)
				knownFolderProperties.localizedName = CoreHelpers.GetStringResource(knownFolderProperties.localizedNameResourceId)


				knownFolderProperties.folderId = knownFolderNative.GetId()
			Finally
				' Clean up memory. 
				Marshal.FreeCoTaskMem(nativeFolderDefinition.name)
				Marshal.FreeCoTaskMem(nativeFolderDefinition.description)
				Marshal.FreeCoTaskMem(nativeFolderDefinition.relativePath)
				Marshal.FreeCoTaskMem(nativeFolderDefinition.parsingName)
				Marshal.FreeCoTaskMem(nativeFolderDefinition.tooltip)
				Marshal.FreeCoTaskMem(nativeFolderDefinition.localizedName)
				Marshal.FreeCoTaskMem(nativeFolderDefinition.icon)
				Marshal.FreeCoTaskMem(nativeFolderDefinition.security)
			End Try
		End Sub

		''' <summary>
		''' Gets the path of this this known folder.
		''' </summary>
		''' <param name="fileExists">
		''' Returns false if the folder is virtual, or a boolean
		''' value that indicates whether this known folder exists.
		''' </param>
		''' <param name="knownFolderNative">Native IKnownFolder reference</param>
		''' <returns>
		''' A <see cref="System.String"/> containing the path, or <see cref="System.String.Empty"/> if this known folder does not exist.
		''' </returns>
		Private Function GetPath(ByRef fileExists As Boolean, knownFolderNative As IKnownFolderNative) As String
			Debug.Assert(knownFolderNative IsNot Nothing)

			Dim kfPath As String = String.Empty
			fileExists = True

			' Virtual folders do not have path.
			If knownFolderProperties.category = FolderCategory.Virtual Then
				fileExists = False
				Return kfPath
			End If

			Try
				kfPath = knownFolderNative.GetPath(0)
			Catch generatedExceptionName As System.IO.FileNotFoundException
				fileExists = False
			Catch generatedExceptionName As System.IO.DirectoryNotFoundException
				fileExists = False
			End Try

			Return kfPath
		End Function

		#End Region

		#Region "KnownFolder Properties"

		''' <summary>
		''' Gets the path for this known folder.
		''' </summary>
		''' <value>A <see cref="System.String"/> object.</value>
		Public ReadOnly Property Path() As String
			Get
				Return knownFolderProperties.path
			End Get
		End Property


		''' <summary>
		''' Gets the category designation for this known folder.
		''' </summary>
		''' <value>A <see cref="FolderCategory"/> value.</value>
		Public ReadOnly Property Category() As FolderCategory
			Get
				Return knownFolderProperties.category
			End Get
		End Property

		''' <summary>
		''' Gets this known folder's canonical name.
		''' </summary>
		''' <value>A <see cref="System.String"/> object.</value>
		Public ReadOnly Property CanonicalName() As String
			Get
				Return knownFolderProperties.canonicalName
			End Get
		End Property

		''' <summary>
		''' Gets this known folder's description.
		''' </summary>
		''' <value>A <see cref="System.String"/> object.</value>
		Public ReadOnly Property Description() As String
			Get
				Return knownFolderProperties.description
			End Get
		End Property

		''' <summary>
		''' Gets the unique identifier for this known folder's parent folder.
		''' </summary>
		''' <value>A <see cref="System.Guid"/> value.</value>
		Public ReadOnly Property ParentId() As Guid
			Get
				Return knownFolderProperties.parentId
			End Get
		End Property

		''' <summary>
		''' Gets this known folder's relative path.
		''' </summary>
		''' <value>A <see cref="System.String"/> object.</value>
		Public ReadOnly Property RelativePath() As String
			Get
				Return knownFolderProperties.relativePath
			End Get
		End Property

		''' <summary>
		''' Gets this known folder's tool tip text.
		''' </summary>
		''' <value>A <see cref="System.String"/> object.</value>
		Public ReadOnly Property Tooltip() As String
			Get
				Return knownFolderProperties.tooltip
			End Get
		End Property
		''' <summary>
		''' Gets the resource identifier for this 
		''' known folder's tool tip text.
		''' </summary>
		''' <value>A <see cref="System.String"/> object.</value>
		Public ReadOnly Property TooltipResourceId() As String
			Get
				Return knownFolderProperties.tooltipResourceId
			End Get
		End Property

		''' <summary>
		''' Gets this known folder's localized name.
		''' </summary>
		''' <value>A <see cref="System.String"/> object.</value>
		Public ReadOnly Property LocalizedName() As String
			Get
				Return knownFolderProperties.localizedName
			End Get
		End Property
		''' <summary>
		''' Gets the resource identifier for this 
		''' known folder's localized name.
		''' </summary>
		''' <value>A <see cref="System.String"/> object.</value>
		Public ReadOnly Property LocalizedNameResourceId() As String
			Get
				Return knownFolderProperties.localizedNameResourceId
			End Get
		End Property

		''' <summary>
		''' Gets this known folder's security attributes.
		''' </summary>
		''' <value>A <see cref="System.String"/> object.</value>
		Public ReadOnly Property Security() As String
			Get
				Return knownFolderProperties.security
			End Get
		End Property

		''' <summary>
		''' Gets this known folder's file attributes, 
		''' such as "read-only".
		''' </summary>
		''' <value>A <see cref="System.IO.FileAttributes"/> value.</value>
		Public ReadOnly Property FileAttributes() As System.IO.FileAttributes
			Get
				Return knownFolderProperties.fileAttributes
			End Get
		End Property

		''' <summary>
		''' Gets an value that describes this known folder's behaviors.
		''' </summary>
		''' <value>A <see cref="DefinitionOptions"/> value.</value>
		Public ReadOnly Property DefinitionOptions() As DefinitionOptions
			Get
				Return knownFolderProperties.definitionOptions
			End Get
		End Property

		''' <summary>
		''' Gets the unique identifier for this known folder's type.
		''' </summary>
		''' <value>A <see cref="System.Guid"/> value.</value>
		Public ReadOnly Property FolderTypeId() As Guid
			Get
				Return knownFolderProperties.folderTypeId
			End Get
		End Property

		''' <summary>
		''' Gets a string representation of this known folder's type.
		''' </summary>
		''' <value>A <see cref="System.String"/> object.</value>
		Public ReadOnly Property FolderType() As String
			Get
				Return knownFolderProperties.folderType
			End Get
		End Property
		''' <summary>
		''' Gets the unique identifier for this known folder.
		''' </summary>
		''' <value>A <see cref="System.Guid"/> value.</value>
		Public ReadOnly Property FolderId() As Guid
			Get
				Return knownFolderProperties.folderId
			End Get
		End Property

		''' <summary>
		''' Gets a value that indicates whether this known folder's path exists on the computer. 
		''' </summary>
		''' <value>A bool<see cref="System.Boolean"/> value.</value>
		''' <remarks>If this property value is <b>false</b>, 
		''' the folder might be a virtual folder (<see cref="Category"/> property will
		''' be <see cref="FolderCategory.Virtual"/> for virtual folders)</remarks>
		Public ReadOnly Property PathExists() As Boolean
			Get
				Return knownFolderProperties.pathExists
			End Get
		End Property

		''' <summary>
		''' Gets a value that states whether this known folder 
		''' can have its path set to a new value, 
		''' including any restrictions on the redirection.
		''' </summary>
		''' <value>A <see cref="RedirectionCapability"/> value.</value>
		Public ReadOnly Property Redirection() As RedirectionCapability
			Get
				Return knownFolderProperties.redirection
			End Get
		End Property

		#End Region
	End Class
End Namespace
