'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports Microsoft.Windows.Shell

Namespace Taskbar
	''' <summary>
	''' Represents a jump list item.
	''' </summary>
	Public Class JumpListItem
		Inherits ShellFile
		Implements IJumpListItem
		''' <summary>
		''' Creates a jump list item with the specified path.
		''' </summary>
		''' <param name="path">The path to the jump list item.</param>
		''' <remarks>The file type should associate the given file  
		''' with the calling application.</remarks>
		Public Sub New(path As String)
			MyBase.New(path)
		End Sub

		#Region "IJumpListItem Members"

		''' <summary>
		''' Gets or sets the target path for this jump list item.
		''' </summary>
		Public Shadows Property Path() As String Implements IJumpListItem.Path
			Get
				Return MyBase.Path
			End Get
			Set
				MyBase.ParsingName = value
			End Set
		End Property

		#End Region
	End Class
End Namespace
