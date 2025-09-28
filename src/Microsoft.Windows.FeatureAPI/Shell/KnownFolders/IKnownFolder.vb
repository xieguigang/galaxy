' Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Collections.Generic
Imports System.IO

Namespace Shell
	''' <summary>
	''' Represents a registered or known folder in the system.
	''' </summary>
	Public Interface IKnownFolder
		Inherits IDisposable
		Inherits IEnumerable(Of ShellObject)
		''' <summary>
		''' Gets the path for this known folder.
		''' </summary>
		ReadOnly Property Path() As String

		''' <summary>
		''' Gets the category designation for this known folder.
		''' </summary>
		ReadOnly Property Category() As FolderCategory

		''' <summary>
		''' Gets this known folder's canonical name.
		''' </summary>
		ReadOnly Property CanonicalName() As String

		''' <summary>
		''' Gets this known folder's description.
		''' </summary>
		ReadOnly Property Description() As String

		''' <summary>
		''' Gets the unique identifier for this known folder's parent folder.
		''' </summary>
		ReadOnly Property ParentId() As Guid

		''' <summary>
		''' Gets this known folder's relative path.
		''' </summary>
		ReadOnly Property RelativePath() As String

		''' <summary>
		''' Gets this known folder's parsing name.
		''' </summary>
		ReadOnly Property ParsingName() As String

		''' <summary>
		''' Gets this known folder's tool tip text.
		''' </summary>
		ReadOnly Property Tooltip() As String

		''' <summary>
		''' Gets the resource identifier for this 
		''' known folder's tool tip text.
		''' </summary>
		ReadOnly Property TooltipResourceId() As String

		''' <summary>
		''' Gets this known folder's localized name.
		''' </summary>
		ReadOnly Property LocalizedName() As String

		''' <summary>
		''' Gets the resource identifier for this 
		''' known folder's localized name.
		''' </summary>
		ReadOnly Property LocalizedNameResourceId() As String

		''' <summary>
		''' Gets this known folder's security attributes.
		''' </summary>
		ReadOnly Property Security() As String

		''' <summary>
		''' Gets this known folder's file attributes, 
		''' such as "read-only".
		''' </summary>
		ReadOnly Property FileAttributes() As FileAttributes

		''' <summary>
		''' Gets an value that describes this known folder's behaviors.
		''' </summary>
		ReadOnly Property DefinitionOptions() As DefinitionOptions

		''' <summary>
		''' Gets the unique identifier for this known folder's type.
		''' </summary>
		ReadOnly Property FolderTypeId() As Guid

		''' <summary>
		''' Gets a string representation of this known folder's type.
		''' </summary>
		ReadOnly Property FolderType() As String

		''' <summary>
		''' Gets the unique identifier for this known folder.
		''' </summary>
		ReadOnly Property FolderId() As Guid

		''' <summary>
		''' Gets a value that indicates whether this known folder's path exists on the computer. 
		''' </summary>
		''' <remarks>If this property value is <b>false</b>, 
		''' the folder might be a virtual folder (<see cref="Category"/> property will
		''' be <see cref="FolderCategory.Virtual"/> for virtual folders)</remarks>
		ReadOnly Property PathExists() As Boolean

		''' <summary>
		''' Gets a value that states whether this known folder 
		''' can have its path set to a new value, 
		''' including any restrictions on the redirection.
		''' </summary>
		ReadOnly Property Redirection() As RedirectionCapability
	End Interface
End Namespace
