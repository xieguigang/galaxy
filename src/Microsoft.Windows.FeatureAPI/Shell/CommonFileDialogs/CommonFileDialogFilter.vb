'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Collections.ObjectModel
Imports System.Text
Imports Microsoft.Windows.Shell

Namespace Dialogs
	''' <summary>
	''' Stores the file extensions used when filtering files in File Open and File Save dialogs.
	''' </summary>
	Public Class CommonFileDialogFilter
		' We'll keep a parsed list of separate 
		' extensions and rebuild as needed.

		Private m_extensions As Collection(Of String)
		Private rawDisplayName As String

		''' <summary>
		''' Creates a new instance of this class.
		''' </summary>
		Public Sub New()
			m_extensions = New Collection(Of String)()
		End Sub

		''' <summary>
		''' Creates a new instance of this class with the specified display name and 
		''' file extension list.
		''' </summary>
		''' <param name="rawDisplayName">The name of this filter.</param>
		''' <param name="extensionList">The list of extensions in 
		''' this filter. See remarks.</param>
		''' <remarks>The <paramref name="extensionList"/> can use a semicolon(";") 
		''' or comma (",") to separate extensions. Extensions can be prefaced 
		''' with a period (".") or with the file wild card specifier "*.".</remarks>
		''' <permission cref="System.ArgumentNullException">
		''' The <paramref name="extensionList"/> cannot be null or a 
		''' zero-length string. 
		''' </permission>
		Public Sub New(rawDisplayName As String, extensionList As String)
			Me.New()
			If String.IsNullOrEmpty(extensionList) Then
				Throw New ArgumentNullException("extensionList")
			End If

			Me.rawDisplayName = rawDisplayName

			' Parse string and create extension strings.
			' Format: "bat,cmd", or "bat;cmd", or "*.bat;*.cmd"
			' Can support leading "." or "*." - these will be stripped.
			Dim rawExtensions As String() = extensionList.Split(","C, ";"C)
			For Each extension As String In rawExtensions
				m_extensions.Add(CommonFileDialogFilter.NormalizeExtension(extension))
			Next
		End Sub
		''' <summary>
		''' Gets or sets the display name for this filter.
		''' </summary>
		''' <permission cref="System.ArgumentNullException">
		''' The value for this property cannot be set to null or a 
		''' zero-length string. 
		''' </permission>        
		Public Property DisplayName() As String
			Get
				If m_showExtensions Then
					Return String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0} ({1})", rawDisplayName, CommonFileDialogFilter.GetDisplayExtensionList(m_extensions))
				End If

				Return rawDisplayName
			End Get

			Set
				If String.IsNullOrEmpty(value) Then
					Throw New ArgumentNullException("value")
				End If
				rawDisplayName = value
			End Set
		End Property

		''' <summary>
		''' Gets a collection of the individual extensions 
		''' described by this filter.
		''' </summary>
		Public ReadOnly Property Extensions() As Collection(Of String)
			Get
				Return m_extensions
			End Get
		End Property

		Private m_showExtensions As Boolean = True
		''' <summary>
		''' Gets or sets a value that controls whether the extensions are displayed.
		''' </summary>
		Public Property ShowExtensions() As Boolean
			Get
				Return m_showExtensions
			End Get
			Set
				m_showExtensions = value
			End Set
		End Property

		Private Shared Function NormalizeExtension(rawExtension As String) As String
			rawExtension = rawExtension.Trim()
			rawExtension = rawExtension.Replace("*.", Nothing)
			rawExtension = rawExtension.Replace(".", Nothing)
			Return rawExtension
		End Function

		Private Shared Function GetDisplayExtensionList(extensions As Collection(Of String)) As String
			Dim extensionList As New StringBuilder()
			For Each extension As String In extensions
				If extensionList.Length > 0 Then
					extensionList.Append(", ")
				End If
				extensionList.Append("*.")
				extensionList.Append(extension)
			Next

			Return extensionList.ToString()
		End Function

		''' <summary>
		''' Internal helper that generates a single filter 
		''' specification for this filter, used by the COM API.
		''' </summary>
		''' <returns>Filter specification for this filter</returns>
		''' 
		Friend Function GetFilterSpec() As ShellNativeMethods.FilterSpec
			Dim filterList As New StringBuilder()
			For Each extension As String In m_extensions
				If filterList.Length > 0 Then
					filterList.Append(";")
				End If

				filterList.Append("*.")

				filterList.Append(extension)
			Next
			Return New ShellNativeMethods.FilterSpec(DisplayName, filterList.ToString())
		End Function

		''' <summary>
		''' Returns a string representation for this filter that includes
		''' the display name and the list of extensions.
		''' </summary>
		''' <returns>A <see cref="System.String"/>.</returns>
		Public Overrides Function ToString() As String
			Return String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0} ({1})", rawDisplayName, CommonFileDialogFilter.GetDisplayExtensionList(m_extensions))
		End Function
	End Class
End Namespace
