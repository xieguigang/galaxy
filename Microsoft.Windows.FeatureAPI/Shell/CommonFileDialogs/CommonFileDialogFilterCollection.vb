'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Collections.ObjectModel
Imports Microsoft.Windows.Shell

Namespace Dialogs
	''' <summary>
	''' Provides a strongly typed collection for file dialog filters.
	''' </summary>
	Public Class CommonFileDialogFilterCollection
		Inherits Collection(Of CommonFileDialogFilter)
		' Make the default constructor internal so users can't instantiate this 
		' collection by themselves.
		Friend Sub New()
		End Sub

		Friend Function GetAllFilterSpecs() As ShellNativeMethods.FilterSpec()
			Dim filterSpecs As ShellNativeMethods.FilterSpec() = New ShellNativeMethods.FilterSpec(Me.Count - 1) {}

			For i As Integer = 0 To Me.Count - 1
				filterSpecs(i) = Me(i).GetFilterSpec()
			Next

			Return filterSpecs
		End Function
	End Class
End Namespace
