'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports Microsoft.Windows.Resources

Namespace Dialogs
	''' <summary>
	''' Defines the class of commonly used file filters.
	''' </summary>
	Public NotInheritable Class CommonFileDialogStandardFilters
		Private Sub New()
		End Sub
		Private Shared textFilesFilter As CommonFileDialogFilter
		''' <summary>
		''' Gets a value that specifies the filter for *.txt files.
		''' </summary>
		Public Shared ReadOnly Property TextFiles() As CommonFileDialogFilter
			Get
				If textFilesFilter Is Nothing Then
					textFilesFilter = New CommonFileDialogFilter(LocalizedMessages.CommonFiltersText, "*.txt")
				End If
				Return textFilesFilter
			End Get
		End Property

		Private Shared pictureFilesFilter As CommonFileDialogFilter
		''' <summary>
		''' Gets a value that specifies the filter for picture files.
		''' </summary>
		Public Shared ReadOnly Property PictureFiles() As CommonFileDialogFilter
			Get
				If pictureFilesFilter Is Nothing Then
					pictureFilesFilter = New CommonFileDialogFilter(LocalizedMessages.CommonFiltersPicture, "*.bmp, *.jpg, *.jpeg, *.png, *.ico")
				End If
				Return pictureFilesFilter
			End Get
		End Property

		Private Shared officeFilesFilter As CommonFileDialogFilter
		''' <summary>
		''' Gets a value that specifies the filter for Microsoft Office files.
		''' </summary>
		Public Shared ReadOnly Property OfficeFiles() As CommonFileDialogFilter
			Get
				If officeFilesFilter Is Nothing Then
					officeFilesFilter = New CommonFileDialogFilter(LocalizedMessages.CommonFiltersOffice, "*.doc, *.docx, *.xls, *.xlsx, *.ppt, *.pptx")
				End If
				Return officeFilesFilter
			End Get
		End Property
	End Class
End Namespace
