'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.IO
Imports Microsoft.Windows.Resources

Namespace Shell
	''' <summary>
	''' A file in the Shell Namespace
	''' </summary>
	Public Class ShellFile
		Inherits ShellObject
		#Region "Internal Constructor"

		<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")> _
		Friend Sub New(path As String)
			' Get the absolute path
			Dim absPath As String = ShellHelper.GetAbsolutePath(path)

			' Make sure this is valid
			If Not File.Exists(absPath) Then
				Throw New FileNotFoundException(String.Format(System.Globalization.CultureInfo.InvariantCulture, LocalizedMessages.FilePathNotExist, path))
			End If

			ParsingName = absPath
		End Sub

		Friend Sub New(shellItem As IShellItem2)
            m_nativeShellItem = shellItem
        End Sub

		#End Region

		#Region "Public Methods"
		''' <summary>
		''' Constructs a new ShellFile object given a file path
		''' </summary>
		''' <param name="path">The file or folder path</param>
		''' <returns>ShellFile object created using given file path.</returns>
		Public Shared Function FromFilePath(path As String) As ShellFile
			Return New ShellFile(path)
		End Function

		#End Region

		#Region "Public Properties"

		''' <summary>
		''' The path for this file
		''' </summary>
		Public Overridable ReadOnly Property Path() As String
			Get
				Return Me.ParsingName
			End Get
		End Property

		#End Region
	End Class
End Namespace
