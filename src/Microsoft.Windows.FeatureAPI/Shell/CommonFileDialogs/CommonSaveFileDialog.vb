'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Runtime.InteropServices
Imports System.Text
Imports Microsoft.Windows.Internal
Imports Microsoft.Windows.Shell
Imports Microsoft.Windows.Shell.PropertySystem

Namespace Dialogs
	''' <summary>
	''' Creates a Vista or Windows 7 Common File Dialog, allowing the user to select the filename and location for a saved file.
	''' </summary>
	''' <permission cref="System.Security.Permissions.FileDialogPermission">
	''' to save a file. Associated enumeration: <see cref="System.Security.Permissions.SecurityAction.LinkDemand"/>.
	''' </permission>
	Public NotInheritable Class CommonSaveFileDialog
		Inherits CommonFileDialog
		Private saveDialogCoClass As NativeFileSaveDialog

		''' <summary>
		''' Creates a new instance of this class.
		''' </summary>
		Public Sub New()
		End Sub
		''' <summary>
		''' Creates a new instance of this class with the specified name.
		''' </summary>
		''' <param name="name">The name of this dialog.</param>
		Public Sub New(name As String)
			MyBase.New(name)
		End Sub

		#Region "Public API specific to Save"

		Private m_overwritePrompt As Boolean = True
		''' <summary>
		''' Gets or sets a value that controls whether to prompt before 
		''' overwriting an existing file of the same name. Default value is true.
		''' </summary>
		''' <permission cref="System.InvalidOperationException">
		''' This property cannot be changed when the dialog is showing.
		''' </permission>
		Public Property OverwritePrompt() As Boolean
			Get
				Return m_overwritePrompt
			End Get
			Set
				ThrowIfDialogShowing(LocalizedMessages.OverwritePromptCannotBeChanged)
				m_overwritePrompt = value
			End Set
		End Property

		Private m_createPrompt As Boolean
		''' <summary>
		''' Gets or sets a value that controls whether to prompt for creation if the item returned in the save dialog does not exist. 
		''' </summary>
		''' <remarks>Note that this does not actually create the item.</remarks>
		''' <permission cref="System.InvalidOperationException">
		''' This property cannot be changed when the dialog is showing.
		''' </permission>
		Public Property CreatePrompt() As Boolean
			Get
				Return m_createPrompt
			End Get
			Set
				ThrowIfDialogShowing(LocalizedMessages.CreatePromptCannotBeChanged)
				m_createPrompt = value
			End Set
		End Property

		Private m_isExpandedMode As Boolean
		''' <summary>
		''' Gets or sets a value that controls whether to the save dialog 
		''' displays in expanded mode. 
		''' </summary>
		''' <remarks>Expanded mode controls whether the dialog
		''' shows folders for browsing or hides them.</remarks>
		''' <permission cref="System.InvalidOperationException">
		''' This property cannot be changed when the dialog is showing.
		''' </permission>
		Public Property IsExpandedMode() As Boolean
			Get
				Return m_isExpandedMode
			End Get
			Set
				ThrowIfDialogShowing(LocalizedMessages.IsExpandedModeCannotBeChanged)
				m_isExpandedMode = value
			End Set
		End Property

		Private m_alwaysAppendDefaultExtension As Boolean
		''' <summary>
		''' Gets or sets a value that controls whether the 
		''' returned file name has a file extension that matches the 
		''' currently selected file type.  If necessary, the dialog appends the correct 
		''' file extension.
		''' </summary>
		''' <permission cref="System.InvalidOperationException">
		''' This property cannot be changed when the dialog is showing.
		''' </permission>
		Public Property AlwaysAppendDefaultExtension() As Boolean
			Get
				Return m_alwaysAppendDefaultExtension
			End Get
			Set
				ThrowIfDialogShowing(LocalizedMessages.AlwaysAppendDefaultExtensionCannotBeChanged)
				m_alwaysAppendDefaultExtension = value
			End Set
		End Property

		''' <summary>
		''' Sets an item to appear as the initial entry in a <b>Save As</b> dialog.
		''' </summary>
		''' <param name="item">The initial entry to be set in the dialog.</param>
		''' <remarks>The name of the item is displayed in the file name edit box, 
		''' and the containing folder is opened in the view. This would generally be 
		''' used when the application is saving an item that already exists.</remarks>
		Public Sub SetSaveAsItem(item As ShellObject)
			If item Is Nothing Then
				Throw New ArgumentNullException("item")
			End If

			InitializeNativeFileDialog()
			Dim nativeDialog As IFileSaveDialog = TryCast(GetNativeFileDialog(), IFileSaveDialog)

			' Get the native IShellItem from ShellObject
			If nativeDialog IsNot Nothing Then
				nativeDialog.SetSaveAsItem(item.NativeShellItem)
			End If
		End Sub

		''' <summary>
		''' Specifies which properties will be collected in the save dialog.
		''' </summary>
		''' <param name="appendDefault">True to show default properties for the currently selected 
		''' filetype in addition to the properties specified by propertyList. False to show only properties 
		''' specified by pList.
		''' <param name="propertyList">List of properties to collect. This parameter can be null.</param>
		''' </param>
		''' <remarks>
		''' SetCollectedPropertyKeys can be called at any time before the dialog is displayed or while it 
		''' is visible. If different properties are to be collected depending on the chosen filetype, 
		''' then SetCollectedProperties can be called in response to CommonFileDialog::FileTypeChanged event.
		''' Note: By default, no properties are collected in the save dialog.
		''' </remarks>
		Public Sub SetCollectedPropertyKeys(appendDefault As Boolean, ParamArray propertyList As PropertyKey())
            ' Loop through all our property keys and create a semicolon-delimited property list string.
            ' The string we pass to PSGetPropertyDescriptionListFromString must
            ' start with "prop:", followed a list of canonical names for each 
            ' property that is to collected.
            If propertyList IsNot Nothing AndAlso propertyList.Length > 0 AndAlso propertyList(0) <> Nothing Then
                Dim sb As New StringBuilder("prop:")
                For Each key As PropertyKey In propertyList
                    Dim canonicalName As String = ShellPropertyDescriptionsCache.Cache.GetPropertyDescription(key).CanonicalName
                    If Not String.IsNullOrEmpty(canonicalName) Then
                        sb.AppendFormat("{0};", canonicalName)
                    End If
                Next

                Dim guid As New Guid(ShellIIDGuid.IPropertyDescriptionList)
                Dim propertyDescriptionList As IPropertyDescriptionList = Nothing

                Try
                    Dim hr As Integer = PropertySystemNativeMethods.PSGetPropertyDescriptionListFromString(sb.ToString(), guid, propertyDescriptionList)

                    ' If we get a IPropertyDescriptionList, setit on the native dialog.
                    If CoreErrorHelper.Succeeded(hr) Then
                        InitializeNativeFileDialog()
                        Dim nativeDialog As IFileSaveDialog = TryCast(GetNativeFileDialog(), IFileSaveDialog)

                        If nativeDialog IsNot Nothing Then
                            hr = nativeDialog.SetCollectedProperties(propertyDescriptionList, appendDefault)

                            If Not CoreErrorHelper.Succeeded(hr) Then
                                Throw New ShellException(hr)
                            End If
                        End If
                    End If
                Finally
                    If propertyDescriptionList IsNot Nothing Then
                        Marshal.ReleaseComObject(propertyDescriptionList)
                    End If
                End Try
            End If
        End Sub

		''' <summary>
		''' Retrieves the set of property values for a saved item or an item in the process of being saved.
		''' </summary>
		''' <returns>Collection of property values collected from the save dialog</returns>
		''' <remarks>This property can be called while the dialog is showing to retrieve the current 
		''' set of values in the metadata collection pane. It can also be called after the dialog 
		''' has closed, to retrieve the final set of values. The call to this method will fail 
		''' unless property collection has been turned on with a call to SetCollectedPropertyKeys method.
		''' </remarks>
		Public ReadOnly Property CollectedProperties() As ShellPropertyCollection
			Get
				InitializeNativeFileDialog()
				Dim nativeDialog As IFileSaveDialog = TryCast(GetNativeFileDialog(), IFileSaveDialog)

				If nativeDialog IsNot Nothing Then
					Dim propertyStore As IPropertyStore
					Dim hr As HResult = nativeDialog.GetProperties(propertyStore)

					If propertyStore IsNot Nothing AndAlso CoreErrorHelper.Succeeded(hr) Then
						Return New ShellPropertyCollection(propertyStore)
					End If
				End If

				Return Nothing
			End Get
		End Property

		#End Region

		Friend Overrides Sub InitializeNativeFileDialog()
			If saveDialogCoClass Is Nothing Then
                saveDialogCoClass = DirectCast(New NativeFileSaveDialog(), NativeFileSaveDialog)
            End If
		End Sub

		Friend Overrides Function GetNativeFileDialog() As IFileDialog
			System.Diagnostics.Debug.Assert(saveDialogCoClass IsNot Nothing, "Must call Initialize() before fetching dialog interface")
			Return DirectCast(saveDialogCoClass, IFileDialog)
		End Function

		Friend Overrides Sub PopulateWithFileNames(names As System.Collections.ObjectModel.Collection(Of String))
			Dim item As IShellItem
			saveDialogCoClass.GetResult(item)

			If item Is Nothing Then
				Throw New InvalidOperationException(LocalizedMessages.SaveFileNullItem)
			End If
			names.Clear()
			names.Add(GetFileNameFromShellItem(item))
		End Sub

		Friend Overrides Sub PopulateWithIShellItems(items As System.Collections.ObjectModel.Collection(Of IShellItem))
			Dim item As IShellItem
			saveDialogCoClass.GetResult(item)

			If item Is Nothing Then
				Throw New InvalidOperationException(LocalizedMessages.SaveFileNullItem)
			End If
			items.Clear()
			items.Add(item)
		End Sub

		Friend Overrides Sub CleanUpNativeFileDialog()
			If saveDialogCoClass IsNot Nothing Then
				Marshal.ReleaseComObject(saveDialogCoClass)
			End If
		End Sub

		Friend Overrides Function GetDerivedOptionFlags(flags As ShellNativeMethods.FileOpenOptions) As ShellNativeMethods.FileOpenOptions
			If m_overwritePrompt Then
				flags = flags Or ShellNativeMethods.FileOpenOptions.OverwritePrompt
			End If
			If m_createPrompt Then
				flags = flags Or ShellNativeMethods.FileOpenOptions.CreatePrompt
			End If
			If Not m_isExpandedMode Then
				flags = flags Or ShellNativeMethods.FileOpenOptions.DefaultNoMiniMode
			End If
			If m_alwaysAppendDefaultExtension Then
				flags = flags Or ShellNativeMethods.FileOpenOptions.StrictFileTypes
			End If
			Return flags
		End Function
	End Class
End Namespace
