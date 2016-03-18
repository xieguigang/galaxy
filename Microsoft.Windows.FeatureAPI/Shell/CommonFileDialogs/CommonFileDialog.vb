'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Runtime.InteropServices
Imports System.Windows
Imports System.Windows.Interop
Imports System.Windows.Markup
Imports System.Collections.Generic
Imports Microsoft.Windows.Internal
Imports Microsoft.Windows.Resources
Imports Microsoft.Windows.Shell
Imports Microsoft.Windows.Dialogs.Controls
Imports Microsoft.Windows.Controls

Namespace Dialogs
    ''' <summary>
    ''' Defines the abstract base class for the common file dialogs.
    ''' </summary>
    <ContentProperty("Controls")>
    Public MustInherit Class CommonFileDialog
        Implements IDialogControlHost
        Implements IDisposable
        ''' <summary>
        ''' The collection of names selected by the user.
        ''' </summary>
        Protected ReadOnly Iterator Property FileNameCollection() As IEnumerable(Of String)
            Get
                For Each name As String In filenames
                    Yield name
                Next
            End Get
        End Property
        Private filenames As Collection(Of String)
        Friend ReadOnly items As Collection(Of IShellItem)
        Friend showState As DialogShowState = DialogShowState.PreShow

        Private nativeDialog As IFileDialog
        Private customize As IFileDialogCustomize
        Private nativeEventSink As NativeDialogEventSink
        Private canceled As System.Nullable(Of Boolean)
        Private resetSelections As Boolean
        Private parentWindow As IntPtr = IntPtr.Zero

        Private filterSet As Boolean
        ' filters can only be set once
#Region "Constructors"

        ''' <summary>
        ''' Creates a new instance of this class.
        ''' </summary>
        Protected Sub New()
            If Not CoreHelpers.RunningOnVista Then
                Throw New PlatformNotSupportedException(LocalizedMessages.CommonFileDialogRequiresVista)
            End If

            filenames = New Collection(Of String)()
            m_filters = New CommonFileDialogFilterCollection()
            items = New Collection(Of IShellItem)()
            m_controls = New CommonFileDialogControlCollection(Of CommonFileDialogControl)(Me)
        End Sub

        ''' <summary>
        ''' Creates a new instance of this class with the specified title.
        ''' </summary>
        ''' <param name="title">The title to display in the dialog.</param>
        Protected Sub New(title As String)
            Me.New()
            Me.m_title = title
        End Sub

#End Region

        ' Template method to allow derived dialog to create actual
        ' specific COM coclass (e.g. FileOpenDialog or FileSaveDialog).
        Friend MustOverride Sub InitializeNativeFileDialog()
        Friend MustOverride Function GetNativeFileDialog() As IFileDialog
        Friend MustOverride Sub PopulateWithFileNames(names As Collection(Of String))
        Friend MustOverride Sub PopulateWithIShellItems(shellItems As Collection(Of IShellItem))
        Friend MustOverride Sub CleanUpNativeFileDialog()
        Friend MustOverride Function GetDerivedOptionFlags(flags As ShellNativeMethods.FileOpenOptions) As ShellNativeMethods.FileOpenOptions

#Region "Public API"

        Dim _FileOk As CancelEventHandler

        ' Events.
        ''' <summary>
        ''' Raised just before the dialog is about to return with a result. Occurs when the user clicks on the Open 
        ''' or Save button on a file dialog box. 
        ''' </summary>
        Public Custom Event FileOk As CancelEventHandler
            AddHandler(value As CancelEventHandler)
                _FileOk = value
            End AddHandler
            RemoveHandler(value As CancelEventHandler)
                _FileOk = Nothing
            End RemoveHandler
            RaiseEvent(sender As Object, e As CancelEventArgs)
                Call _FileOk.Invoke(sender, e)
            End RaiseEvent
        End Event

        Dim _FolderChanging As EventHandler(Of CommonFileDialogFolderChangeEventArgs)

        ''' <summary>
        ''' Raised just before the user navigates to a new folder.
        ''' </summary>
        Public Custom Event FolderChanging As EventHandler(Of CommonFileDialogFolderChangeEventArgs)
            AddHandler(value As EventHandler(Of CommonFileDialogFolderChangeEventArgs))
                _FolderChanging = value
            End AddHandler
            RemoveHandler(value As EventHandler(Of CommonFileDialogFolderChangeEventArgs))
                _FolderChanging = Nothing
            End RemoveHandler
            RaiseEvent(sender As Object, e As CommonFileDialogFolderChangeEventArgs)
                Call _FolderChanging.Invoke(sender, e)
            End RaiseEvent
        End Event

        Dim _FolderChanged As EventHandler

        ''' <summary>
        ''' Raised when the user navigates to a new folder.
        ''' </summary>
        Public Custom Event FolderChanged As EventHandler
            AddHandler(value As EventHandler)
                _FolderChanged = value
            End AddHandler
            RemoveHandler(value As EventHandler)
                _FolderChanged = Nothing
            End RemoveHandler
            RaiseEvent(sender As Object, e As EventArgs)
                Call _FolderChanged.Invoke(sender, e)
            End RaiseEvent
        End Event

        Dim _SelectionChanged As EventHandler

        ''' <summary>
        ''' Raised when the user changes the selection in the dialog's view.
        ''' </summary>
        Public Custom Event SelectionChanged As EventHandler
            AddHandler(value As EventHandler)
                _SelectionChanged = value
            End AddHandler
            RemoveHandler(value As EventHandler)
                _SelectionChanged = Nothing
            End RemoveHandler
            RaiseEvent(sender As Object, e As EventArgs)
                Call _SelectionChanged.Invoke(sender, e)
            End RaiseEvent
        End Event

        Dim _FileTypeChanged As EventHandler

        ''' <summary>
        ''' Raised when the dialog is opened to notify the application of the initial chosen filetype.
        ''' </summary>
        Public Custom Event FileTypeChanged As EventHandler
            AddHandler(value As EventHandler)
                _FileTypeChanged = value
            End AddHandler
            RemoveHandler(value As EventHandler)
                _FileTypeChanged = Nothing
            End RemoveHandler
            RaiseEvent(sender As Object, e As EventArgs)
                Call _FileTypeChanged.Invoke(sender, e)
            End RaiseEvent
        End Event

        Dim _DialogOpening As EventHandler

        ''' <summary>
        ''' Raised when the dialog is opening.
        ''' </summary>
        Public Custom Event DialogOpening As EventHandler
            AddHandler(value As EventHandler)
                _DialogOpening = value
            End AddHandler
            RemoveHandler(value As EventHandler)
                _DialogOpening = Nothing
            End RemoveHandler
            RaiseEvent(sender As Object, e As EventArgs)
                Call _DialogOpening.Invoke(sender, e)
            End RaiseEvent
        End Event

        Private m_controls As CommonFileDialogControlCollection(Of CommonFileDialogControl)
        ''' <summary>
        ''' Gets the collection of controls for the dialog.
        ''' </summary>
        Public ReadOnly Property Controls() As CommonFileDialogControlCollection(Of CommonFileDialogControl)
            Get
                Return m_controls
            End Get
        End Property

        Private m_filters As CommonFileDialogFilterCollection
        ''' <summary>
        ''' Gets the filters used by the dialog.
        ''' </summary>
        Public ReadOnly Property Filters() As CommonFileDialogFilterCollection
            Get
                Return m_filters
            End Get
        End Property

        Private m_title As String
        ''' <summary>
        ''' Gets or sets the dialog title.
        ''' </summary>
        ''' <value>A <see cref="System.String"/> object.</value>
        Public Property Title() As String
            Get
                Return m_title
            End Get
            Set
                m_title = Value
                If NativeDialogShowing Then
                    nativeDialog.SetTitle(Value)
                End If
            End Set
        End Property

        ' This is the first of many properties that are backed by the FOS_*
        ' bitflag options set with IFileDialog.SetOptions(). 
        ' SetOptions() fails
        ' if called while dialog is showing (e.g. from a callback).
        Private m_ensureFileExists As Boolean
        ''' <summary>
        ''' Gets or sets a value that determines whether the file must exist beforehand.
        ''' </summary>
        ''' <value>A <see cref="System.Boolean"/> value. <b>true</b> if the file must exist.</value>
        ''' <exception cref="System.InvalidOperationException">This property cannot be set when the dialog is visible.</exception>
        Public Property EnsureFileExists() As Boolean
            Get
                Return m_ensureFileExists
            End Get
            Set
                ThrowIfDialogShowing(LocalizedMessages.EnsureFileExistsCannotBeChanged)
                m_ensureFileExists = Value
            End Set
        End Property

        Private m_ensurePathExists As Boolean
        ''' <summary>
        ''' Gets or sets a value that specifies whether the returned file must be in an existing folder.
        ''' </summary>
        ''' <value>A <see cref="System.Boolean"/> value. <b>true</b> if the file must exist.</value>
        ''' <exception cref="System.InvalidOperationException">This property cannot be set when the dialog is visible.</exception>
        Public Property EnsurePathExists() As Boolean
            Get
                Return m_ensurePathExists
            End Get
            Set
                ThrowIfDialogShowing(LocalizedMessages.EnsurePathExistsCannotBeChanged)
                m_ensurePathExists = Value
            End Set
        End Property

        Private m_ensureValidNames As Boolean
        ''' <summary>Gets or sets a value that determines whether to validate file names.
        ''' </summary>
        '''<value>A <see cref="System.Boolean"/> value. <b>true </b>to check for situations that would prevent an application from opening the selected file, such as sharing violations or access denied errors.</value>
        ''' <exception cref="System.InvalidOperationException">This property cannot be set when the dialog is visible.</exception>
        ''' 
        Public Property EnsureValidNames() As Boolean
            Get
                Return m_ensureValidNames
            End Get
            Set
                ThrowIfDialogShowing(LocalizedMessages.EnsureValidNamesCannotBeChanged)
                m_ensureValidNames = Value
            End Set
        End Property

        Private m_ensureReadOnly As Boolean
        ''' <summary>
        ''' Gets or sets a value that determines whether read-only items are returned.
        ''' Default value for CommonOpenFileDialog is true (allow read-only files) and 
        ''' CommonSaveFileDialog is false (don't allow read-only files).
        ''' </summary>
        ''' <value>A <see cref="System.Boolean"/> value. <b>true</b> includes read-only items.</value>
        ''' <exception cref="System.InvalidOperationException">This property cannot be set when the dialog is visible.</exception>
        Public Property EnsureReadOnly() As Boolean
            Get
                Return m_ensureReadOnly
            End Get
            Set
                ThrowIfDialogShowing(LocalizedMessages.EnsureReadonlyCannotBeChanged)
                m_ensureReadOnly = Value
            End Set
        End Property

        Private m_restoreDirectory As Boolean
        ''' <summary>
        ''' Gets or sets a value that determines the restore directory.
        ''' </summary>
        ''' <remarks></remarks>
        ''' <exception cref="System.InvalidOperationException">This property cannot be set when the dialog is visible.</exception>
        Public Property RestoreDirectory() As Boolean
            Get
                Return m_restoreDirectory
            End Get
            Set
                ThrowIfDialogShowing(LocalizedMessages.RestoreDirectoryCannotBeChanged)
                m_restoreDirectory = Value
            End Set
        End Property

        Private m_showPlacesList As Boolean = True
        ''' <summary>
        ''' Gets or sets a value that controls whether 
        ''' to show or hide the list of pinned places that
        ''' the user can choose.
        ''' </summary>
        ''' <value>A <see cref="System.Boolean"/> value. <b>true</b> if the list is visible; otherwise <b>false</b>.</value>
        ''' <exception cref="System.InvalidOperationException">This property cannot be set when the dialog is visible.</exception>
        Public Property ShowPlacesList() As Boolean

            Get
                Return m_showPlacesList
            End Get
            Set
                ThrowIfDialogShowing(LocalizedMessages.ShowPlacesListCannotBeChanged)
                m_showPlacesList = Value
            End Set
        End Property

        Private addToMruList As Boolean = True
        ''' <summary>
        ''' Gets or sets a value that controls whether to show or hide the list of places where the user has recently opened or saved items.
        ''' </summary>
        ''' <value>A <see cref="System.Boolean"/> value.</value>
        ''' <exception cref="System.InvalidOperationException">This property cannot be set when the dialog is visible.</exception>
        Public Property AddToMostRecentlyUsedList() As Boolean
            Get
                Return addToMruList
            End Get
            Set
                ThrowIfDialogShowing(LocalizedMessages.AddToMostRecentlyUsedListCannotBeChanged)
                addToMruList = Value
            End Set
        End Property

        Private m_showHiddenItems As Boolean
        '''<summary>
        ''' Gets or sets a value that controls whether to show hidden items.
        ''' </summary>
        ''' <value>A <see cref="System.Boolean"/> value.<b>true</b> to show the items; otherwise <b>false</b>.</value>
        ''' <exception cref="System.InvalidOperationException">This property cannot be set when the dialog is visible.</exception>
        Public Property ShowHiddenItems() As Boolean
            Get
                Return m_showHiddenItems
            End Get
            Set
                ThrowIfDialogShowing(LocalizedMessages.ShowHiddenItemsCannotBeChanged)
                m_showHiddenItems = Value
            End Set
        End Property
        Private m_allowPropertyEditing As Boolean
        ''' <summary>
        ''' Gets or sets a value that controls whether 
        ''' properties can be edited.
        ''' </summary>
        ''' <value>A <see cref="System.Boolean"/> value. </value>
        Public Property AllowPropertyEditing() As Boolean
            Get
                Return m_allowPropertyEditing
            End Get
            Set
                m_allowPropertyEditing = Value
            End Set
        End Property

        Private m_navigateToShortcut As Boolean = True
        '''<summary>
        ''' Gets or sets a value that controls whether shortcuts should be treated as their target items, allowing an application to open a .lnk file.
        ''' </summary>
        ''' <value>A <see cref="System.Boolean"/> value. <b>true</b> indicates that shortcuts should be treated as their targets. </value>
        ''' <exception cref="System.InvalidOperationException">This property cannot be set when the dialog is visible.</exception>
        Public Property NavigateToShortcut() As Boolean
            Get
                Return m_navigateToShortcut
            End Get
            Set
                ThrowIfDialogShowing(LocalizedMessages.NavigateToShortcutCannotBeChanged)
                m_navigateToShortcut = Value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the default file extension to be added to file names. If the value is null
        ''' or string.Empty, the extension is not added to the file names.
        ''' </summary>
        Public Property DefaultExtension() As String
            Get
                Return m_DefaultExtension
            End Get
            Set
                m_DefaultExtension = Value
            End Set
        End Property
        Private m_DefaultExtension As String

        ''' <summary>
        ''' Gets the index for the currently selected file type.
        ''' </summary>
        Public ReadOnly Property SelectedFileTypeIndex() As Integer
            Get
                Dim fileType As UInteger

                If nativeDialog IsNot Nothing Then
                    nativeDialog.GetFileTypeIndex(fileType)
                    Return CInt(fileType)
                End If

                Return -1
            End Get
        End Property

        ''' <summary>
        ''' Tries to set the File(s) Type Combo to match the value in 
        ''' 'DefaultExtension'.  Only doing this if 'this' is a Save dialog 
        ''' as it makes no sense to do this if only Opening a file.
        ''' </summary>
        ''' 
        ''' <param name="dialog">The native/IFileDialog instance.</param>
        ''' 
        Private Sub SyncFileTypeComboToDefaultExtension(dialog As IFileDialog)
            ' make sure it's a Save dialog and that there is a default 
            ' extension to sync to.
            If Not (TypeOf Me Is CommonSaveFileDialog) OrElse DefaultExtension Is Nothing OrElse m_filters.Count <= 0 Then
                Return
            End If

            Dim filter As CommonFileDialogFilter = Nothing
            Dim Len As UInteger = CUInt(m_filters.Count) - 1UI

            For filtersCounter As UInteger = 0 To Len
                filter = DirectCast(m_filters(CInt(filtersCounter)), CommonFileDialogFilter)

                If filter.Extensions.Contains(DefaultExtension) Then
                    ' set the docType combo to match this 
                    ' extension. property is a 1-based index.
                    dialog.SetFileTypeIndex(filtersCounter + 1UI)

                    ' we're done, exit for
                    Exit For
                End If
            Next

        End Sub

        ''' <summary>
        ''' Gets the selected filename.
        ''' </summary>
        ''' <value>A <see cref="System.String"/> object.</value>
        ''' <exception cref="System.InvalidOperationException">This property cannot be used when multiple files are selected.</exception>
        Public ReadOnly Property FileName() As String
            Get
                CheckFileNamesAvailable()

                If filenames.Count > 1 Then
                    Throw New InvalidOperationException(LocalizedMessages.CommonFileDialogMultipleFiles)
                End If

                Dim returnFilename As String = filenames(0)

                ' "If extension is a null reference (Nothing in Visual 
                ' Basic), the returned string contains the specified 
                ' path with its extension removed."  Since we do not want 
                ' to remove any existing extension, make sure the 
                ' DefaultExtension property is NOT null.

                ' if we should, and there is one to set...
                If Not String.IsNullOrEmpty(DefaultExtension) Then
                    returnFilename = System.IO.Path.ChangeExtension(returnFilename, DefaultExtension)
                End If

                Return returnFilename
            End Get
        End Property

        ''' <summary>
        ''' Gets the selected item as a ShellObject.
        ''' </summary>
        ''' <value>A <see cref="Shell.ShellObject"></see> object.</value>
        ''' <exception cref="System.InvalidOperationException">This property cannot be used when multiple files
        ''' are selected.</exception>
        Public ReadOnly Property FileAsShellObject() As ShellObject
            Get
                CheckFileItemsAvailable()

                If items.Count > 1 Then
                    Throw New InvalidOperationException(LocalizedMessages.CommonFileDialogMultipleItems)
                End If

                If items.Count = 0 Then
                    Return Nothing
                End If

                Return ShellObjectFactory.Create(items(0))
            End Get
        End Property

        ''' <summary>
        ''' Adds a location, such as a folder, library, search connector, or known folder, to the list of
        ''' places available for a user to open or save items. This method actually adds an item
        ''' to the <b>Favorite Links</b> or <b>Places</b> section of the Open/Save dialog.
        ''' </summary>
        ''' <param name="place">The item to add to the places list.</param>
        ''' <param name="location">One of the enumeration values that indicates placement of the item in the list.</param>
        Public Sub AddPlace(place As ShellContainer, location As FileDialogAddPlaceLocation)
            If place Is Nothing Then
                Throw New ArgumentNullException("place")
            End If

            ' Get our native dialog
            If nativeDialog Is Nothing Then
                InitializeNativeFileDialog()
                nativeDialog = GetNativeFileDialog()
            End If

            ' Add the shellitem to the places list
            If nativeDialog IsNot Nothing Then
                nativeDialog.AddPlace(place.NativeShellItem, CType(location, ShellNativeMethods.FileDialogAddPlacement))
            End If
        End Sub

        ''' <summary>
        ''' Adds a location (folder, library, search connector, known folder) to the list of
        ''' places available for the user to open or save items. This method actually adds an item
        ''' to the <b>Favorite Links</b> or <b>Places</b> section of the Open/Save dialog. Overload method
        ''' takes in a string for the path.
        ''' </summary>
        ''' <param name="path">The item to add to the places list.</param>
        ''' <param name="location">One of the enumeration values that indicates placement of the item in the list.</param>
        Public Sub AddPlace(path As String, location As FileDialogAddPlaceLocation)
            If String.IsNullOrEmpty(path) Then
                Throw New ArgumentNullException("path")
            End If

            ' Get our native dialog
            If nativeDialog Is Nothing Then
                InitializeNativeFileDialog()
                nativeDialog = GetNativeFileDialog()
            End If

            ' Create a native shellitem from our path
            Dim nativeShellItem As IShellItem2
            Dim guid As New Guid(ShellIIDGuid.IShellItem2)
            Dim retCode As Integer = ShellNativeMethods.SHCreateItemFromParsingName(path, IntPtr.Zero, guid, nativeShellItem)

            If Not CoreErrorHelper.Succeeded(retCode) Then
                Throw New CommonControlException(LocalizedMessages.CommonFileDialogCannotCreateShellItem, Marshal.GetExceptionForHR(retCode))
            End If

            ' Add the shellitem to the places list
            If nativeDialog IsNot Nothing Then
                nativeDialog.AddPlace(nativeShellItem, CType(location, ShellNativeMethods.FileDialogAddPlacement))
            End If
        End Sub

        ' Null = use default directory.
        Private m_initialDirectory As String
        ''' <summary>
        ''' Gets or sets the initial directory displayed when the dialog is shown. 
        ''' A null or empty string indicates that the dialog is using the default directory.
        ''' </summary>
        ''' <value>A <see cref="System.String"/> object.</value>
        Public Property InitialDirectory() As String
            Get
                Return m_initialDirectory
            End Get
            Set
                m_initialDirectory = Value
            End Set
        End Property

        Private m_initialDirectoryShellContainer As ShellContainer
        ''' <summary>
        ''' Gets or sets a location that is always selected when the dialog is opened, 
        ''' regardless of previous user action. A null value implies that the dialog is using 
        ''' the default location.
        ''' </summary>
        Public Property InitialDirectoryShellContainer() As ShellContainer
            Get
                Return m_initialDirectoryShellContainer
            End Get
            Set
                m_initialDirectoryShellContainer = Value
            End Set
        End Property

        Private m_defaultDirectory As String
        ''' <summary>
        ''' Sets the folder and path used as a default if there is not a recently used folder value available.
        ''' </summary>
        Public Property DefaultDirectory() As String
            Get
                Return m_defaultDirectory
            End Get
            Set
                m_defaultDirectory = Value
            End Set
        End Property

        Private m_defaultDirectoryShellContainer As ShellContainer
        ''' <summary>
        ''' Sets the location (<see cref="Shell.ShellContainer">ShellContainer</see> 
        ''' used as a default if there is not a recently used folder value available.
        ''' </summary>
        Public Property DefaultDirectoryShellContainer() As ShellContainer
            Get
                Return m_defaultDirectoryShellContainer
            End Get
            Set
                m_defaultDirectoryShellContainer = Value
            End Set
        End Property

        ' Null = use default identifier.
        Private m_cookieIdentifier As Guid
        ''' <summary>
        ''' Gets or sets a value that enables a calling application 
        ''' to associate a GUID with a dialog's persisted state.
        ''' </summary>
        Public Property CookieIdentifier() As Guid
            Get
                Return m_cookieIdentifier
            End Get
            Set
                m_cookieIdentifier = Value
            End Set
        End Property

        ''' <summary>
        ''' Displays the dialog.
        ''' </summary>
        ''' <param name="ownerWindowHandle">Window handle of any top-level window that will own the modal dialog box.</param>
        ''' <returns>A <see cref="CommonFileDialogResult"/> object.</returns>
        Public Function ShowDialog(ownerWindowHandle As IntPtr) As CommonFileDialogResult
            If ownerWindowHandle = IntPtr.Zero Then
                Throw New ArgumentException(LocalizedMessages.CommonFileDialogInvalidHandle, "ownerWindowHandle")
            End If

            ' Set the parent / owner window
            parentWindow = ownerWindowHandle

            ' Show the modal dialog
            Return ShowDialog()
        End Function

        ''' <summary>
        ''' Displays the dialog.
        ''' </summary>
        ''' <param name="window">Top-level WPF window that will own the modal dialog box.</param>
        ''' <returns>A <see cref="CommonFileDialogResult"/> object.</returns>
        Public Function ShowDialog(window As Window) As CommonFileDialogResult
            If window Is Nothing Then
                Throw New ArgumentNullException("window")
            End If

            ' Set the parent / owner window
            parentWindow = (New WindowInteropHelper(window)).Handle

            ' Show the modal dialog
            Return ShowDialog()
        End Function

        ''' <summary>
        ''' Displays the dialog.
        ''' </summary>
        ''' <returns>A <see cref="CommonFileDialogResult"/> object.</returns>
        Public Function ShowDialog() As CommonFileDialogResult
            Dim result As CommonFileDialogResult

            ' Fetch derived native dialog (i.e. Save or Open).
            InitializeNativeFileDialog()
            nativeDialog = GetNativeFileDialog()

            ' Apply outer properties to native dialog instance.
            ApplyNativeSettings(nativeDialog)
            InitializeEventSink(nativeDialog)

            ' Clear user data if Reset has been called 
            ' since the last show.
            If resetSelections Then
                resetSelections = False
            End If

            ' Show dialog.
            showState = DialogShowState.Showing
            Dim hresult__1 As Integer = nativeDialog.Show(parentWindow)
            showState = DialogShowState.Closed

            ' Create return information.
            If CoreErrorHelper.Matches(hresult__1, CInt(HResult.Win32ErrorCanceled)) Then
                canceled = True
                result = CommonFileDialogResult.Cancel
                filenames.Clear()
            Else
                canceled = False
                result = CommonFileDialogResult.Ok

                ' Populate filenames if user didn't cancel.
                PopulateWithFileNames(filenames)

                ' Populate the actual IShellItems
                PopulateWithIShellItems(items)
            End If

            Return result
        End Function
        ''' <summary>
        ''' Removes the current selection.
        ''' </summary>
        Public Sub ResetUserSelections()
            resetSelections = True
        End Sub

        ''' <summary>
        ''' Default file name.
        ''' </summary>
        Public Property DefaultFileName() As String
            Get
                Return m_DefaultFileName
            End Get
            Set
                m_DefaultFileName = Value
            End Set
        End Property
        Private m_DefaultFileName As String

#End Region

#Region "Configuration"

        Private Sub InitializeEventSink(nativeDlg As IFileDialog)
            ' Check if we even need to have a sink.
            If _FileOk IsNot Nothing OrElse
                _FolderChanging IsNot Nothing OrElse
                _FolderChanged IsNot Nothing OrElse
                _SelectionChanged IsNot Nothing OrElse
                _FileTypeChanged IsNot Nothing OrElse
                _DialogOpening IsNot Nothing OrElse
                (m_controls IsNot Nothing AndAlso m_controls.Count > 0) Then

                Dim cookie As UInteger
                nativeEventSink = New NativeDialogEventSink(Me)
                nativeDlg.Advise(nativeEventSink, cookie)
                nativeEventSink.Cookie = cookie
            End If
        End Sub

        Private Sub ApplyNativeSettings(dialog As IFileDialog)
            Debug.Assert(dialog IsNot Nothing, "No dialog instance to configure")

            If parentWindow = IntPtr.Zero Then
                If System.Windows.Application.Current IsNot Nothing AndAlso System.Windows.Application.Current.MainWindow IsNot Nothing Then
                    parentWindow = (New WindowInteropHelper(System.Windows.Application.Current.MainWindow)).Handle
                ElseIf System.Windows.Forms.Application.OpenForms.Count > 0 Then
                    parentWindow = System.Windows.Forms.Application.OpenForms(0).Handle
                End If
            End If

            Dim guid__1 As New Guid(ShellIIDGuid.IShellItem2)

            ' Apply option bitflags.
            dialog.SetOptions(CalculateNativeDialogOptionFlags())

            ' Other property sets.
            If m_title IsNot Nothing Then
                dialog.SetTitle(m_title)
            End If

            If m_initialDirectoryShellContainer IsNot Nothing Then
                dialog.SetFolder(DirectCast(m_initialDirectoryShellContainer, ShellObject).NativeShellItem)
            End If

            If m_defaultDirectoryShellContainer IsNot Nothing Then
                dialog.SetDefaultFolder(DirectCast(m_defaultDirectoryShellContainer, ShellObject).NativeShellItem)
            End If

            If Not String.IsNullOrEmpty(m_initialDirectory) Then
                ' Create a native shellitem from our path
                Dim initialDirectoryShellItem As IShellItem2
                ShellNativeMethods.SHCreateItemFromParsingName(m_initialDirectory, IntPtr.Zero, guid__1, initialDirectoryShellItem)

                ' If we get a real shell item back, 
                ' then use that as the initial folder - otherwise,
                ' we'll allow the dialog to revert to the default folder. 
                ' (OR should we fail loudly?)
                If initialDirectoryShellItem IsNot Nothing Then
                    dialog.SetFolder(initialDirectoryShellItem)
                End If
            End If

            If Not String.IsNullOrEmpty(m_defaultDirectory) Then
                ' Create a native shellitem from our path
                Dim defaultDirectoryShellItem As IShellItem2
                ShellNativeMethods.SHCreateItemFromParsingName(m_defaultDirectory, IntPtr.Zero, guid__1, defaultDirectoryShellItem)

                ' If we get a real shell item back, 
                ' then use that as the initial folder - otherwise,
                ' we'll allow the dialog to revert to the default folder. 
                ' (OR should we fail loudly?)
                If defaultDirectoryShellItem IsNot Nothing Then
                    dialog.SetDefaultFolder(defaultDirectoryShellItem)
                End If
            End If

            ' Apply file type filters, if available.
            If m_filters.Count > 0 AndAlso Not filterSet Then
                dialog.SetFileTypes(CUInt(m_filters.Count), m_filters.GetAllFilterSpecs())

                filterSet = True

                SyncFileTypeComboToDefaultExtension(dialog)
            End If

            If m_cookieIdentifier <> Guid.Empty Then
                dialog.SetClientGuid(m_cookieIdentifier)
            End If

            ' Set the default extension
            If Not String.IsNullOrEmpty(DefaultExtension) Then
                dialog.SetDefaultExtension(DefaultExtension)
            End If

            ' Set the default filename
            dialog.SetFileName(DefaultFileName)
        End Sub

        Private Function CalculateNativeDialogOptionFlags() As ShellNativeMethods.FileOpenOptions
            ' We start with only a few flags set by default, 
            ' then go from there based on the current state
            ' of the managed dialog's property values.
            Dim flags As ShellNativeMethods.FileOpenOptions = ShellNativeMethods.FileOpenOptions.NoTestFileCreate

            ' Call to derived (concrete) dialog to 
            ' set dialog-specific flags.
            flags = GetDerivedOptionFlags(flags)

            ' Apply other optional flags.
            If m_ensureFileExists Then
                flags = flags Or ShellNativeMethods.FileOpenOptions.FileMustExist
            End If
            If m_ensurePathExists Then
                flags = flags Or ShellNativeMethods.FileOpenOptions.PathMustExist
            End If
            If Not m_ensureValidNames Then
                flags = flags Or ShellNativeMethods.FileOpenOptions.NoValidate
            End If
            If Not EnsureReadOnly Then
                flags = flags Or ShellNativeMethods.FileOpenOptions.NoReadOnlyReturn
            End If
            If m_restoreDirectory Then
                flags = flags Or ShellNativeMethods.FileOpenOptions.NoChangeDirectory
            End If
            If Not m_showPlacesList Then
                flags = flags Or ShellNativeMethods.FileOpenOptions.HidePinnedPlaces
            End If
            If Not addToMruList Then
                flags = flags Or ShellNativeMethods.FileOpenOptions.DontAddToRecent
            End If
            If m_showHiddenItems Then
                flags = flags Or ShellNativeMethods.FileOpenOptions.ForceShowHidden
            End If
            If Not m_navigateToShortcut Then
                flags = flags Or ShellNativeMethods.FileOpenOptions.NoDereferenceLinks
            End If
            Return flags
        End Function

#End Region

#Region "IDialogControlHost Members"

        Private Shared Sub GenerateNotImplementedException()
            Throw New NotImplementedException(LocalizedMessages.NotImplementedException)
        End Sub

        ''' <summary>
        ''' Returns if change to the colleciton is allowed.
        ''' </summary>
        ''' <returns>true if collection change is allowed.</returns>
        Public Overridable Function IsCollectionChangeAllowed() As Boolean Implements IDialogControlHost.IsCollectionChangeAllowed
            Return True
        End Function

        ''' <summary>
        ''' Applies changes to the collection.
        ''' </summary>
        Public Overridable Sub ApplyCollectionChanged() Implements IDialogControlHost.ApplyCollectionChanged
            ' Query IFileDialogCustomize interface before adding controls
            GetCustomizedFileDialog()
            ' Populate all the custom controls and add them to the dialog
            For Each control As CommonFileDialogControl In m_controls
                If Not control.IsAdded Then
                    control.HostingDialog = Me
                    control.Attach(customize)
                    control.IsAdded = True
                End If
            Next

        End Sub

        ''' <summary>
        ''' Determines if changes to a specific property are allowed.
        ''' </summary>
        ''' <param name="propertyName">The name of the property.</param>
        ''' <param name="control">The control propertyName applies to.</param>
        ''' <returns>true if the property change is allowed.</returns>
        Public Overridable Function IsControlPropertyChangeAllowed(propertyName As String, control As DialogControl) As Boolean Implements IDialogControlHost.IsControlPropertyChangeAllowed
            CommonFileDialog.GenerateNotImplementedException()
            Return False
        End Function

        ''' <summary>
        ''' Called when a control currently in the collection 
        ''' has a property changed.
        ''' </summary>
        ''' <param name="propertyName">The name of the property changed.</param>
        ''' <param name="control">The control whose property has changed.</param>
        Public Overridable Sub ApplyControlPropertyChange(propertyName As String, control As DialogControl) Implements IDialogControlHost.ApplyControlPropertyChange
            If control Is Nothing Then
                Throw New ArgumentNullException("control")
            End If

            Dim dialogControl As CommonFileDialogControl = Nothing
            If propertyName = "Text" Then
                Dim textBox As CommonFileDialogTextBox = TryCast(control, CommonFileDialogTextBox)

                If textBox IsNot Nothing Then
                    customize.SetEditBoxText(control.Id, textBox.Text)
                Else
                    customize.SetControlLabel(control.Id, textBox.Text)
                End If
            ElseIf propertyName = "Visible" AndAlso dialogControl.DirectCopy(TryCast(control, CommonFileDialogControl)) IsNot Nothing Then
                Dim state As ShellNativeMethods.ControlState
                customize.GetControlState(control.Id, state)

                If dialogControl.Visible = True Then
                    state = state Or ShellNativeMethods.ControlState.Visible
                ElseIf dialogControl.Visible = False Then
                    state = state And Not ShellNativeMethods.ControlState.Visible
                End If

                customize.SetControlState(control.Id, state)
            ElseIf propertyName = "Enabled" AndAlso dialogControl IsNot Nothing Then
                Dim state As ShellNativeMethods.ControlState
                customize.GetControlState(control.Id, state)

                If dialogControl.Enabled = True Then
                    state = state Or ShellNativeMethods.ControlState.Enable
                ElseIf dialogControl.Enabled = False Then
                    state = state And Not ShellNativeMethods.ControlState.Enable
                End If

                customize.SetControlState(control.Id, state)
            ElseIf propertyName = "SelectedIndex" Then
                Dim list As CommonFileDialogRadioButtonList
                Dim box As CommonFileDialogComboBox

                If list.DirectCopy(TryCast(control, CommonFileDialogRadioButtonList)) IsNot Nothing Then
                    customize.SetSelectedControlItem(list.Id, list.SelectedIndex)
                ElseIf box.DirectCopy(TryCast(control, CommonFileDialogComboBox)) IsNot Nothing Then
                    customize.SetSelectedControlItem(box.Id, box.SelectedIndex)
                End If
            ElseIf propertyName = "IsChecked" Then
                Dim checkBox As CommonFileDialogCheckBox = TryCast(control, CommonFileDialogCheckBox)
                If checkBox IsNot Nothing Then
                    customize.SetCheckButtonState(checkBox.Id, checkBox.IsChecked)
                End If
            End If
        End Sub

#End Region

#Region "Helpers"

        ''' <summary>
        ''' Ensures that the user has selected one or more files.
        ''' </summary>
        ''' <permission cref="System.InvalidOperationException">
        ''' The dialog has not been dismissed yet or the dialog was cancelled.
        ''' </permission>
        Protected Sub CheckFileNamesAvailable()
            If showState <> DialogShowState.Closed Then
                Throw New InvalidOperationException(LocalizedMessages.CommonFileDialogNotClosed)
            End If

            If canceled.GetValueOrDefault() Then
                Throw New InvalidOperationException(LocalizedMessages.CommonFileDialogCanceled)
            End If

            Debug.Assert(filenames.Count <> 0, "FileNames empty - shouldn't happen unless dialog canceled or not yet shown.")
        End Sub

        ''' <summary>
        ''' Ensures that the user has selected one or more files.
        ''' </summary>
        ''' <permission cref="System.InvalidOperationException">
        ''' The dialog has not been dismissed yet or the dialog was cancelled.
        ''' </permission>
        Protected Sub CheckFileItemsAvailable()
            If showState <> DialogShowState.Closed Then
                Throw New InvalidOperationException(LocalizedMessages.CommonFileDialogNotClosed)
            End If

            If canceled.GetValueOrDefault() Then
                Throw New InvalidOperationException(LocalizedMessages.CommonFileDialogCanceled)
            End If

            Debug.Assert(items.Count <> 0, "Items list empty - shouldn't happen unless dialog canceled or not yet shown.")
        End Sub

        Private ReadOnly Property NativeDialogShowing() As Boolean
            Get
                Return (nativeDialog IsNot Nothing) AndAlso (showState = DialogShowState.Showing OrElse showState = DialogShowState.Closing)
            End Get
        End Property

        Friend Shared Function GetFileNameFromShellItem(item As IShellItem) As String
            Dim filename As String = Nothing
            Dim pszString As IntPtr = IntPtr.Zero
            Dim hr As HResult = item.GetDisplayName(ShellNativeMethods.ShellItemDesignNameOptions.DesktopAbsoluteParsing, pszString)
            If hr = HResult.Ok AndAlso pszString <> IntPtr.Zero Then
                filename = Marshal.PtrToStringAuto(pszString)
                Marshal.FreeCoTaskMem(pszString)
            End If
            Return filename
        End Function

        Friend Shared Function GetShellItemAt(array As IShellItemArray, i As Integer) As IShellItem
            Dim result As IShellItem
            Dim index As UInteger = CUInt(i)
            array.GetItemAt(index, result)
            Return result
        End Function

        ''' <summary>
        ''' Throws an exception when the dialog is showing preventing
        ''' a requested change to a property or the visible set of controls.
        ''' </summary>
        ''' <param name="message">The message to include in the exception.</param>
        ''' <permission cref="System.InvalidOperationException"> The dialog is in an
        ''' invalid state to perform the requested operation.</permission>
        Protected Sub ThrowIfDialogShowing(message As String)
            If NativeDialogShowing Then
                Throw New InvalidOperationException(message)
            End If
        End Sub
        ''' <summary>
        ''' Get the IFileDialogCustomize interface, preparing to add controls.
        ''' </summary>
        Private Sub GetCustomizedFileDialog()
            If customize Is Nothing Then
                If nativeDialog Is Nothing Then
                    InitializeNativeFileDialog()
                    nativeDialog = GetNativeFileDialog()
                End If
                customize = DirectCast(nativeDialog, IFileDialogCustomize)
            End If
        End Sub
#End Region

#Region "CheckChanged handling members"
        ''' <summary>
        ''' Raises the <see cref="CommonFileDialog.FileOk"/> event just before the dialog is about to return with a result.
        ''' </summary>
        ''' <param name="e">The event data.</param>
        Protected Overridable Sub OnFileOk(e As CancelEventArgs)
            RaiseEvent FileOk(Me, e)
        End Sub
        ''' <summary>
        ''' Raises the <see cref="FolderChanging"/> to stop navigation to a particular location.
        ''' </summary>
        ''' <param name="e">Cancelable event arguments.</param>
        Protected Overridable Sub OnFolderChanging(e As CommonFileDialogFolderChangeEventArgs)
            RaiseEvent FolderChanging(Me, e)
        End Sub
        ''' <summary>
        ''' Raises the <see cref="CommonFileDialog.FolderChanged"/> event when the user navigates to a new folder.
        ''' </summary>
        ''' <param name="e">The event data.</param>
        Protected Overridable Sub OnFolderChanged(e As EventArgs)
            RaiseEvent FolderChanged(Me, e)
        End Sub
        ''' <summary>
        ''' Raises the <see cref="CommonFileDialog.SelectionChanged"/> event when the user changes the selection in the dialog's view.
        ''' </summary>
        ''' <param name="e">The event data.</param>
        Protected Overridable Sub OnSelectionChanged(e As EventArgs)
            RaiseEvent SelectionChanged(Me, e)
        End Sub
        ''' <summary>
        ''' Raises the <see cref="CommonFileDialog.FileTypeChanged"/> event when the dialog is opened to notify the 
        ''' application of the initial chosen filetype.
        ''' </summary>
        ''' <param name="e">The event data.</param>
        Protected Overridable Sub OnFileTypeChanged(e As EventArgs)
            RaiseEvent FileTypeChanged(Me, e)
        End Sub
        ''' <summary>
        ''' Raises the <see cref="CommonFileDialog.DialogOpening"/> event when the dialog is opened.
        ''' </summary>
        ''' <param name="e">The event data.</param>
        Protected Overridable Sub OnOpening(e As EventArgs)
            RaiseEvent DialogOpening(Me, e)
        End Sub

#End Region

#Region "NativeDialogEventSink Nested Class"

        Private Class NativeDialogEventSink
            Implements IFileDialogEvents
            Implements IFileDialogControlEvents
            Private parent As CommonFileDialog
            Private firstFolderChanged As Boolean = True

            Public Sub New(commonDialog As CommonFileDialog)
                Me.parent = commonDialog
            End Sub

            Public Property Cookie() As UInteger
                Get
                    Return m_Cookie
                End Get
                Set
                    m_Cookie = Value
                End Set
            End Property
            Private m_Cookie As UInteger

            Public Function OnFileOk(pfd As IFileDialog) As HResult Implements IFileDialogEvents.OnFileOk
                Dim args As New CancelEventArgs()
                parent.OnFileOk(args)

                If Not args.Cancel Then
                    ' Make sure all custom properties are sync'ed
                    If parent.Controls IsNot Nothing Then
                        For Each control As CommonFileDialogControl In parent.Controls
                            Dim textBox__1 As CommonFileDialogTextBox
                            Dim groupBox As CommonFileDialogGroupBox

                            If textBox__1.DirectCopy(TryCast(control, CommonFileDialogTextBox)) IsNot Nothing Then
                                textBox__1.SyncValue()
                                textBox__1.Closed = True
                                ' Also check subcontrols
                            ElseIf groupBox.DirectCopy(TryCast(control, CommonFileDialogGroupBox)) IsNot Nothing Then
                                For Each subcontrol As CommonFileDialogControl In groupBox.Items
                                    Dim textbox__2 As CommonFileDialogTextBox = TryCast(subcontrol, CommonFileDialogTextBox)
                                    If textbox__2 IsNot Nothing Then
                                        textbox__2.SyncValue()
                                        textbox__2.Closed = True
                                    End If
                                Next
                            End If
                        Next
                    End If
                End If

                Return (If(args.Cancel, HResult.[False], HResult.Ok))
            End Function

            Public Function OnFolderChanging(pfd As IFileDialog, psiFolder As IShellItem) As HResult Implements IFileDialogEvents.OnFolderChanging
                Dim args As New CommonFileDialogFolderChangeEventArgs(CommonFileDialog.GetFileNameFromShellItem(psiFolder))

                If Not firstFolderChanged Then
                    parent.OnFolderChanging(args)
                End If

                Return (If(args.Cancel, HResult.[False], HResult.Ok))
            End Function

            Public Sub OnFolderChange(pfd As IFileDialog) Implements IFileDialogEvents.OnFolderChange
                If firstFolderChanged Then
                    firstFolderChanged = False
                    parent.OnOpening(EventArgs.Empty)
                Else
                    parent.OnFolderChanged(EventArgs.Empty)
                End If
            End Sub

            Public Sub OnSelectionChange(pfd As IFileDialog) Implements IFileDialogEvents.OnSelectionChange
                parent.OnSelectionChanged(EventArgs.Empty)
            End Sub

            Public Sub OnShareViolation(pfd As IFileDialog, psi As IShellItem, ByRef pResponse As ShellNativeMethods.FileDialogEventShareViolationResponse) Implements IFileDialogEvents.OnShareViolation
                ' Do nothing: we will ignore share violations, 
                ' and don't register
                ' for them, so this method should never be called.
                pResponse = ShellNativeMethods.FileDialogEventShareViolationResponse.Accept
            End Sub

            Public Sub OnTypeChange(pfd As IFileDialog) Implements IFileDialogEvents.OnTypeChange
                parent.OnFileTypeChanged(EventArgs.Empty)
            End Sub

            Public Sub OnOverwrite(pfd As IFileDialog, psi As IShellItem, ByRef pResponse As ShellNativeMethods.FileDialogEventOverwriteResponse) Implements IFileDialogEvents.OnOverwrite
                ' Don't accept or reject the dialog, keep default settings
                pResponse = ShellNativeMethods.FileDialogEventOverwriteResponse.[Default]
            End Sub

            Public Sub OnItemSelected(pfdc As IFileDialogCustomize, dwIDCtl As Integer, dwIDItem As Integer) Implements IFileDialogControlEvents.OnItemSelected
                ' Find control
                Dim control As DialogControl = Me.parent.Controls.GetControlbyId(dwIDCtl)

                Dim controlInterface As ICommonFileDialogIndexedControls
                Dim menu As CommonFileDialogMenu

                ' Process ComboBox and/or RadioButtonList                
                If controlInterface.DirectCopy(TryCast(control, ICommonFileDialogIndexedControls)) IsNot Nothing Then
                    ' Update selected item and raise SelectedIndexChanged event                    
                    controlInterface.SelectedIndex = dwIDItem
                    controlInterface.RaiseSelectedIndexChangedEvent()
                    ' Process Menu
                ElseIf menu.DirectCopy(TryCast(control, CommonFileDialogMenu)) IsNot Nothing Then
                    ' Find the menu item that was clicked and invoke it's click event
                    For Each item As CommonFileDialogMenuItem In menu.Items
                        If item.Id = dwIDItem Then
                            item.RaiseClickEvent()
                            Exit For
                        End If
                    Next
                End If
            End Sub

            Public Sub OnButtonClicked(pfdc As IFileDialogCustomize, dwIDCtl As Integer) Implements IFileDialogControlEvents.OnButtonClicked
                ' Find control
                Dim control As DialogControl = Me.parent.Controls.GetControlbyId(dwIDCtl)
                Dim button As CommonFileDialogButton = TryCast(control, CommonFileDialogButton)
                ' Call corresponding event
                If button IsNot Nothing Then
                    button.RaiseClickEvent()
                End If
            End Sub

            Public Sub OnCheckButtonToggled(pfdc As IFileDialogCustomize, dwIDCtl As Integer, bChecked As Boolean) Implements IFileDialogControlEvents.OnCheckButtonToggled
                ' Find control
                Dim control As DialogControl = Me.parent.Controls.GetControlbyId(dwIDCtl)

                Dim box As CommonFileDialogCheckBox = TryCast(control, CommonFileDialogCheckBox)
                ' Update control and call corresponding event
                If box IsNot Nothing Then
                    box.IsChecked = bChecked
                    box.RaiseCheckedChangedEvent()
                End If
            End Sub

            Public Sub OnControlActivating(pfdc As IFileDialogCustomize, dwIDCtl As Integer) Implements IFileDialogControlEvents.OnControlActivating
            End Sub

        End Class

#End Region

#Region "IDisposable Members"

        ''' <summary>
        ''' Releases the unmanaged resources used by the CommonFileDialog class and optionally 
        ''' releases the managed resources.
        ''' </summary>
        ''' <param name="disposing"><b>true</b> to release both managed and unmanaged resources; 
        ''' <b>false</b> to release only unmanaged resources.</param>
        Protected Overridable Sub Dispose(disposing As Boolean)
            If disposing Then
                CleanUpNativeFileDialog()
            End If
        End Sub

        ''' <summary>
        ''' Releases the resources used by the current instance of the CommonFileDialog class.
        ''' </summary>        
        Public Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub

#End Region

        ''' <summary>
        ''' Indicates whether this feature is supported on the current platform.
        ''' </summary>
        Public Shared ReadOnly Property IsPlatformSupported() As Boolean
            Get
                ' We need Windows Vista onwards ...
                Return CoreHelpers.RunningOnVista
            End Get
        End Property

    End Class


End Namespace
