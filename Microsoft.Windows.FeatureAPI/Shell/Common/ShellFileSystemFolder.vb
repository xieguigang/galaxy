'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.IO
Imports Microsoft.Windows.Resources

Namespace Shell
	''' <summary>
	''' A folder in the Shell Namespace
	''' </summary>
	Public Class ShellFileSystemFolder
		Inherits ShellFolder
		#Region "Internal Constructor"

				' Empty
		Friend Sub New()
		End Sub

		Friend Sub New(shellItem As IShellItem2)
            m_nativeShellItem = shellItem
        End Sub

		#End Region

		#Region "Public Methods"
		''' <summary>
		''' Constructs a new ShellFileSystemFolder object given a folder path
		''' </summary>
		''' <param name="path">The folder path</param>
		''' <remarks>ShellFileSystemFolder created from the given folder path.</remarks>
		Public Shared Function FromFolderPath(path As String) As ShellFileSystemFolder
			' Get the absolute path
			Dim absPath As String = ShellHelper.GetAbsolutePath(path)

			' Make sure this is valid
			If Not Directory.Exists(absPath) Then
				Throw New DirectoryNotFoundException(String.Format(System.Globalization.CultureInfo.InvariantCulture, LocalizedMessages.FilePathNotExist, path))
			End If

			Dim folder As New ShellFileSystemFolder()
			Try
				folder.ParsingName = absPath
				Return folder
			Catch
				folder.Dispose()
				Throw
			End Try

		End Function

		#End Region

		#Region "Public Properties"

		''' <summary>
		''' The path for this Folder
		''' </summary>
		Public Overridable ReadOnly Property Path() As String
			Get
				Return Me.ParsingName
			End Get
		End Property

		#End Region

	End Class
End Namespace
