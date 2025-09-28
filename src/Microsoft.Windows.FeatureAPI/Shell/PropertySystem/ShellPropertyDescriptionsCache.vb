'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Collections.Generic
Imports Microsoft.Windows.Shell

Namespace Shell.PropertySystem
	Friend Class ShellPropertyDescriptionsCache
		Private Sub New()
			propsDictionary = New Dictionary(Of PropertyKey, ShellPropertyDescription)()
		End Sub

		Private propsDictionary As IDictionary(Of PropertyKey, ShellPropertyDescription)
		Private Shared cacheInstance As ShellPropertyDescriptionsCache

		Public Shared ReadOnly Property Cache() As ShellPropertyDescriptionsCache
			Get
				If cacheInstance Is Nothing Then
					cacheInstance = New ShellPropertyDescriptionsCache()
				End If
				Return cacheInstance
			End Get
		End Property

		Public Function GetPropertyDescription(key As PropertyKey) As ShellPropertyDescription
			If Not propsDictionary.ContainsKey(key) Then
				propsDictionary.Add(key, New ShellPropertyDescription(key))
			End If
			Return propsDictionary(key)
		End Function
	End Class
End Namespace
