'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.ComponentModel

Namespace Dialogs
	''' <summary>
	''' Creates the event data associated with <see cref="CommonFileDialog.FolderChanging"/> event.
	''' </summary>
	''' 
	Public Class CommonFileDialogFolderChangeEventArgs
		Inherits CancelEventArgs
        ''' <summary>
        ''' Creates a new instance of this class.
        ''' </summary>
        ''' <param name="folder__1">The name of the folder.</param>
        Public Sub New(folder__1 As String)
			Folder = folder__1
		End Sub

		''' <summary>
		''' Gets or sets the name of the folder.
		''' </summary>
		Public Property Folder() As String
			Get
				Return m_Folder
			End Get
			Set
				m_Folder = Value
			End Set
		End Property
		Private m_Folder As String

	End Class
End Namespace
