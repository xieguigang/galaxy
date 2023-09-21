'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.ComponentModel
Imports System.Threading
Imports Microsoft.Windows.Resources

Namespace Shell
	''' <summary>
	''' Listens for changes in/on a ShellObject and raises events when they occur.
	''' This class supports all items under the shell namespace including
	''' files, folders and virtual folders (libraries, search results and network items), etc.
	''' </summary>
	Public Class ShellObjectWatcher
		Implements IDisposable
		Private _shellObject As ShellObject
		Private _recursive As Boolean

		Private _manager As New ChangeNotifyEventManager()
		Private _listenerHandle As IntPtr
		Private _message As UInteger

		Private _registrationId As UInteger
		Private _running As Boolean

		Private _context As SynchronizationContext = SynchronizationContext.Current

		''' <summary>
		''' Creates the ShellObjectWatcher for the given ShellObject
		''' </summary>
		''' <param name="shellObject">The ShellObject to monitor</param>
		''' <param name="recursive">Whether to listen for changes recursively (for when monitoring a container)</param>
		Public Sub New(shellObject As ShellObject, recursive As Boolean)
			If shellObject Is Nothing Then
				Throw New ArgumentNullException("shellObject")
			End If

			If _context Is Nothing Then
				_context = New SynchronizationContext()
				SynchronizationContext.SetSynchronizationContext(_context)
			End If

			_shellObject = shellObject
			Me._recursive = recursive

			Dim result = MessageListenerFilter.Register(AddressOf OnWindowMessageReceived)
			_listenerHandle = result.WindowHandle
			_message = result.Message
		End Sub

		''' <summary>
		''' Gets whether the watcher is currently running.
		''' </summary>
		Public Property Running() As Boolean
			Get
				Return _running
			End Get
			Private Set
				_running = value
			End Set
		End Property

		''' <summary>
		''' Start the watcher and begin receiving change notifications.        
		''' <remarks>
		''' If the watcher is running, has no effect.
		''' Registration for notifications should be done before this is called.
		''' </remarks>
		''' </summary>
		Public Sub Start()
			If Running Then
				Return
			End If

			'#Region "Registration"
			Dim entry As New ShellNativeMethods.SHChangeNotifyEntry()
			entry.recursively = _recursive

			entry.pIdl = _shellObject.PIDL

			'ShellObjectChangeTypes.AllEventsMask,
			_registrationId = ShellNativeMethods.SHChangeNotifyRegister(_listenerHandle, ShellNativeMethods.ShellChangeNotifyEventSource.ShellLevel Or ShellNativeMethods.ShellChangeNotifyEventSource.InterruptLevel Or ShellNativeMethods.ShellChangeNotifyEventSource.NewDelivery, _manager.RegisteredTypes, _message, 1, entry)

			If _registrationId = 0 Then
				Throw New Win32Exception(LocalizedMessages.ShellObjectWatcherRegisterFailed)
			End If
			'#End Region

			Running = True
		End Sub

		''' <summary>
		''' Stop the watcher and prevent further notifications from being received.
		''' <remarks>If the watcher is not running, this has no effect.</remarks>
		''' </summary>
		Public Sub [Stop]()
			If Not Running Then
				Return
			End If
			If _registrationId > 0 Then
				ShellNativeMethods.SHChangeNotifyDeregister(_registrationId)
				_registrationId = 0
			End If
			Running = False
		End Sub

		Private Sub OnWindowMessageReceived(e As WindowMessageEventArgs)
			If e.Message.Msg = _message Then
                _context.Send(Sub(x) ProcessChangeNotificationEvent(e), Nothing)
            End If
		End Sub

		Private Sub ThrowIfRunning()
			If Running Then
				Throw New InvalidOperationException(LocalizedMessages.ShellObjectWatcherUnableToChangeEvents)
			End If
		End Sub

		''' <summary>
		''' Processes all change notifications sent by the Windows Shell.
		''' </summary>
		''' <param name="e">The windows message representing the notification event</param>
		Protected Overridable Sub ProcessChangeNotificationEvent(e As WindowMessageEventArgs)
			If Not Running Then
				Return
			End If
			If e Is Nothing Then
				Throw New ArgumentNullException("e")
			End If

			Dim notifyLock As New ChangeNotifyLock(e.Message)

			Dim args As ShellObjectNotificationEventArgs = Nothing
			Select Case notifyLock.ChangeType
				Case ShellObjectChangeTypes.DirectoryRename, ShellObjectChangeTypes.ItemRename
					args = New ShellObjectRenamedEventArgs(notifyLock)
					Exit Select
				Case ShellObjectChangeTypes.SystemImageUpdate
					args = New SystemImageUpdatedEventArgs(notifyLock)
					Exit Select
				Case Else
					args = New ShellObjectChangedEventArgs(notifyLock)
					Exit Select
			End Select

			_manager.Invoke(Me, notifyLock.ChangeType, args)
		End Sub

#Region "Change Events"

#Region "Mask Events"
        ''' <summary>
        ''' Raised when any event occurs.
        ''' </summary>
        Public Custom Event AllEvents As EventHandler(Of ShellObjectNotificationEventArgs)
			AddHandler(value As EventHandler(Of ShellObjectNotificationEventArgs))
				ThrowIfRunning()
				_manager.Register(ShellObjectChangeTypes.AllEventsMask, value)
			End AddHandler
            RemoveHandler(value As EventHandler(Of ShellObjectNotificationEventArgs))
                ThrowIfRunning()
                _manager.Unregister(ShellObjectChangeTypes.AllEventsMask, value)
            End RemoveHandler
            RaiseEvent()

            End RaiseEvent
        End Event

		''' <summary>
		''' Raised when global events occur.
		''' </summary>
		Public Custom Event GlobalEvents As EventHandler(Of ShellObjectNotificationEventArgs)
			AddHandler(value As EventHandler(Of ShellObjectNotificationEventArgs))
				ThrowIfRunning()
				_manager.Register(ShellObjectChangeTypes.GlobalEventsMask, value)
			End AddHandler
            RemoveHandler(value As EventHandler(Of ShellObjectNotificationEventArgs))
                ThrowIfRunning()
                _manager.Unregister(ShellObjectChangeTypes.GlobalEventsMask, value)
            End RemoveHandler
            RaiseEvent()

            End RaiseEvent
        End Event

		''' <summary>
		''' Raised when disk events occur.
		''' </summary>
		Public Custom Event DiskEvents As EventHandler(Of ShellObjectNotificationEventArgs)
			AddHandler(value As EventHandler(Of ShellObjectNotificationEventArgs))
				ThrowIfRunning()
				_manager.Register(ShellObjectChangeTypes.DiskEventsMask, value)
			End AddHandler
            RemoveHandler(value As EventHandler(Of ShellObjectNotificationEventArgs))
                ThrowIfRunning()
                _manager.Unregister(ShellObjectChangeTypes.DiskEventsMask, value)
            End RemoveHandler
            RaiseEvent()

            End RaiseEvent
        End Event
		#End Region

		#Region "Single Events"
		''' <summary>
		''' Raised when an item is renamed.
		''' </summary>
		Public Custom Event ItemRenamed As EventHandler(Of ShellObjectRenamedEventArgs)
			AddHandler(value As EventHandler(Of ShellObjectRenamedEventArgs))
				ThrowIfRunning()
				_manager.Register(ShellObjectChangeTypes.ItemRename, value)
			End AddHandler
            RemoveHandler(value As EventHandler(Of ShellObjectRenamedEventArgs))
                ThrowIfRunning()
                _manager.Unregister(ShellObjectChangeTypes.ItemRename, value)
            End RemoveHandler
            RaiseEvent()

            End RaiseEvent
        End Event

		''' <summary>
		''' Raised when an item is created.
		''' </summary>
		Public Custom Event ItemCreated As EventHandler(Of ShellObjectChangedEventArgs)
			AddHandler(value As EventHandler(Of ShellObjectChangedEventArgs))
				ThrowIfRunning()
				_manager.Register(ShellObjectChangeTypes.ItemCreate, value)
			End AddHandler
            RemoveHandler(value As EventHandler(Of ShellObjectChangedEventArgs))
                ThrowIfRunning()
                _manager.Unregister(ShellObjectChangeTypes.ItemCreate, value)
            End RemoveHandler
            RaiseEvent()

            End RaiseEvent
        End Event

		''' <summary>
		''' Raised when an item is deleted.
		''' </summary>
		Public Custom Event ItemDeleted As EventHandler(Of ShellObjectChangedEventArgs)
			AddHandler(value As EventHandler(Of ShellObjectChangedEventArgs))
				ThrowIfRunning()
				_manager.Register(ShellObjectChangeTypes.ItemDelete, value)
			End AddHandler
            RemoveHandler(value As EventHandler(Of ShellObjectChangedEventArgs))
                ThrowIfRunning()
                _manager.Unregister(ShellObjectChangeTypes.ItemDelete, value)
            End RemoveHandler
            RaiseEvent()

            End RaiseEvent
        End Event

		''' <summary>
		''' Raised when an item is updated.
		''' </summary>
		Public Custom Event Updated As EventHandler(Of ShellObjectChangedEventArgs)
			AddHandler(value As EventHandler(Of ShellObjectChangedEventArgs))
				ThrowIfRunning()
				_manager.Register(ShellObjectChangeTypes.Update, value)
			End AddHandler
            RemoveHandler(value As EventHandler(Of ShellObjectChangedEventArgs))
                ThrowIfRunning()
                _manager.Unregister(ShellObjectChangeTypes.Update, value)
            End RemoveHandler
            RaiseEvent()

            End RaiseEvent
        End Event

		''' <summary>
		''' Raised when a directory is updated.
		''' </summary>
		Public Custom Event DirectoryUpdated As EventHandler(Of ShellObjectChangedEventArgs)
			AddHandler(value As EventHandler(Of ShellObjectChangedEventArgs))
				ThrowIfRunning()
				_manager.Register(ShellObjectChangeTypes.DirectoryContentsUpdate, value)
			End AddHandler
            RemoveHandler(value As EventHandler(Of ShellObjectChangedEventArgs))
                ThrowIfRunning()
                _manager.Unregister(ShellObjectChangeTypes.DirectoryContentsUpdate, value)
            End RemoveHandler
            RaiseEvent()

            End RaiseEvent
        End Event

		''' <summary>
		''' Raised when a directory is renamed.
		''' </summary>
		Public Custom Event DirectoryRenamed As EventHandler(Of ShellObjectRenamedEventArgs)
			AddHandler(value As EventHandler(Of ShellObjectRenamedEventArgs))
				ThrowIfRunning()
				_manager.Register(ShellObjectChangeTypes.DirectoryRename, value)
			End AddHandler
            RemoveHandler(value As EventHandler(Of ShellObjectRenamedEventArgs))
                ThrowIfRunning()
                _manager.Unregister(ShellObjectChangeTypes.DirectoryRename, value)
            End RemoveHandler
            RaiseEvent()

            End RaiseEvent
        End Event

		''' <summary>
		''' Raised when a directory is created.
		''' </summary>
		Public Custom Event DirectoryCreated As EventHandler(Of ShellObjectChangedEventArgs)
			AddHandler(value As EventHandler(Of ShellObjectChangedEventArgs))
				ThrowIfRunning()
				_manager.Register(ShellObjectChangeTypes.DirectoryCreate, value)
			End AddHandler
            RemoveHandler(value As EventHandler(Of ShellObjectChangedEventArgs))
                ThrowIfRunning()
                _manager.Unregister(ShellObjectChangeTypes.DirectoryCreate, value)
            End RemoveHandler
            RaiseEvent()

            End RaiseEvent
        End Event

        ''' <summary>
        ''' Raised when a directory is deleted.
        ''' </summary>
        Public Custom Event DirectoryDeleted As EventHandler(Of ShellObjectChangedEventArgs)
            AddHandler(value As EventHandler(Of ShellObjectChangedEventArgs))
                ThrowIfRunning()
                _manager.Register(ShellObjectChangeTypes.DirectoryDelete, value)
            End AddHandler
            RemoveHandler(value As EventHandler(Of ShellObjectChangedEventArgs))
                ThrowIfRunning()
                _manager.Unregister(ShellObjectChangeTypes.DirectoryDelete, value)
            End RemoveHandler
            RaiseEvent()

            End RaiseEvent
        End Event

		''' <summary>
		''' Raised when media is inserted.
		''' </summary>
		Public Custom Event MediaInserted As EventHandler(Of ShellObjectChangedEventArgs)
			AddHandler(value As EventHandler(Of ShellObjectChangedEventArgs))
				ThrowIfRunning()
				_manager.Register(ShellObjectChangeTypes.MediaInsert, value)
			End AddHandler
            RemoveHandler(value As EventHandler(Of ShellObjectChangedEventArgs))
                ThrowIfRunning()
                _manager.Unregister(ShellObjectChangeTypes.MediaInsert, value)
            End RemoveHandler
            RaiseEvent()

            End RaiseEvent
        End Event

		''' <summary>
		''' Raised when media is removed.
		''' </summary>
		Public Custom Event MediaRemoved As EventHandler(Of ShellObjectChangedEventArgs)
			AddHandler(value As EventHandler(Of ShellObjectChangedEventArgs))
				ThrowIfRunning()
				_manager.Register(ShellObjectChangeTypes.MediaRemove, value)
			End AddHandler
            RemoveHandler(value As EventHandler(Of ShellObjectChangedEventArgs))
                ThrowIfRunning()
                _manager.Unregister(ShellObjectChangeTypes.MediaRemove, value)
            End RemoveHandler
            RaiseEvent()

            End RaiseEvent
        End Event

		''' <summary>
		''' Raised when a drive is added.
		''' </summary>
		Public Custom Event DriveAdded As EventHandler(Of ShellObjectChangedEventArgs)
			AddHandler(value As EventHandler(Of ShellObjectChangedEventArgs))
				ThrowIfRunning()
				_manager.Register(ShellObjectChangeTypes.DriveAdd, value)
			End AddHandler
            RemoveHandler(value As EventHandler(Of ShellObjectChangedEventArgs))
                ThrowIfRunning()
                _manager.Unregister(ShellObjectChangeTypes.DriveAdd, value)
            End RemoveHandler
            RaiseEvent()

            End RaiseEvent
        End Event

		''' <summary>
		''' Raised when a drive is removed.
		''' </summary>
		Public Custom Event DriveRemoved As EventHandler(Of ShellObjectChangedEventArgs)
			AddHandler(value As EventHandler(Of ShellObjectChangedEventArgs))
				ThrowIfRunning()
				_manager.Register(ShellObjectChangeTypes.DriveRemove, value)
			End AddHandler
            RemoveHandler(value As EventHandler(Of ShellObjectChangedEventArgs))
                ThrowIfRunning()
                _manager.Unregister(ShellObjectChangeTypes.DriveRemove, value)
            End RemoveHandler
            RaiseEvent()

            End RaiseEvent
        End Event

		''' <summary>
		''' Raised when a folder is shared on a network.
		''' </summary>
		Public Custom Event FolderNetworkShared As EventHandler(Of ShellObjectChangedEventArgs)
			AddHandler(value As EventHandler(Of ShellObjectChangedEventArgs))
				ThrowIfRunning()
				_manager.Register(ShellObjectChangeTypes.NetShare, value)
			End AddHandler
            RemoveHandler(value As EventHandler(Of ShellObjectChangedEventArgs))
                ThrowIfRunning()
                _manager.Unregister(ShellObjectChangeTypes.NetShare, value)
            End RemoveHandler
            RaiseEvent()

            End RaiseEvent
        End Event

		''' <summary>
		''' Raised when a folder is unshared from the network.
		''' </summary>
		Public Custom Event FolderNetworkUnshared As EventHandler(Of ShellObjectChangedEventArgs)
			AddHandler(value As EventHandler(Of ShellObjectChangedEventArgs))
				ThrowIfRunning()
				_manager.Register(ShellObjectChangeTypes.NetUnshare, value)
			End AddHandler
            RemoveHandler(value As EventHandler(Of ShellObjectChangedEventArgs))
                ThrowIfRunning()
                _manager.Unregister(ShellObjectChangeTypes.NetUnshare, value)
            End RemoveHandler
            RaiseEvent()

            End RaiseEvent
        End Event

		''' <summary>
		''' Raised when a server is disconnected.
		''' </summary>
		Public Custom Event ServerDisconnected As EventHandler(Of ShellObjectChangedEventArgs)
			AddHandler(value As EventHandler(Of ShellObjectChangedEventArgs))
				ThrowIfRunning()
				_manager.Register(ShellObjectChangeTypes.ServerDisconnect, value)
			End AddHandler
            RemoveHandler(value As EventHandler(Of ShellObjectChangedEventArgs))
                ThrowIfRunning()
                _manager.Unregister(ShellObjectChangeTypes.ServerDisconnect, value)
            End RemoveHandler
            RaiseEvent()

            End RaiseEvent
        End Event

		''' <summary>
		''' Raised when a system image is changed.
		''' </summary>
		Public Custom Event SystemImageChanged As EventHandler(Of ShellObjectChangedEventArgs)
			AddHandler(value As EventHandler(Of ShellObjectChangedEventArgs))
				ThrowIfRunning()
				_manager.Register(ShellObjectChangeTypes.SystemImageUpdate, value)
			End AddHandler
            RemoveHandler(value As EventHandler(Of ShellObjectChangedEventArgs))
                ThrowIfRunning()
                _manager.Unregister(ShellObjectChangeTypes.SystemImageUpdate, value)
            End RemoveHandler
            RaiseEvent()

            End RaiseEvent
        End Event

		''' <summary>
		''' Raised when free space changes.
		''' </summary>
		Public Custom Event FreeSpaceChanged As EventHandler(Of ShellObjectChangedEventArgs)
			AddHandler(value As EventHandler(Of ShellObjectChangedEventArgs))
				ThrowIfRunning()
				_manager.Register(ShellObjectChangeTypes.FreeSpace, value)
			End AddHandler
            RemoveHandler(value As EventHandler(Of ShellObjectChangedEventArgs))
                ThrowIfRunning()
                _manager.Unregister(ShellObjectChangeTypes.FreeSpace, value)
            End RemoveHandler
            RaiseEvent()

            End RaiseEvent
        End Event

		''' <summary>
		''' Raised when a file type association changes.
		''' </summary>
		Public Custom Event FileTypeAssociationChanged As EventHandler(Of ShellObjectChangedEventArgs)
			AddHandler(value As EventHandler(Of ShellObjectChangedEventArgs))
				ThrowIfRunning()
				_manager.Register(ShellObjectChangeTypes.AssociationChange, value)
			End AddHandler
            RemoveHandler(value As EventHandler(Of ShellObjectChangedEventArgs))
                ThrowIfRunning()
                _manager.Unregister(ShellObjectChangeTypes.AssociationChange, value)
            End RemoveHandler
            RaiseEvent()

            End RaiseEvent
        End Event
#End Region

#End Region

#Region "IDisposable Members"

        ''' <summary>
        ''' Disposes ShellObjectWatcher
        ''' </summary>
        ''' <param name="disposing"></param>
        Protected Overridable Sub Dispose(disposing As Boolean)
			[Stop]()
			_manager.UnregisterAll()

			If _listenerHandle <> IntPtr.Zero Then
				MessageListenerFilter.Unregister(_listenerHandle, _message)
			End If
		End Sub

		''' <summary>
		''' Disposes ShellObjectWatcher.
		''' </summary>
		Public Sub Dispose() Implements IDisposable.Dispose
			Dispose(True)
			GC.SuppressFinalize(Me)
		End Sub

		''' <summary>
		''' Finalizer for ShellObjectWatcher
		''' </summary>
		Protected Overrides Sub Finalize()
			Try
				Dispose(False)
			Finally
				MyBase.Finalize()
			End Try
		End Sub

		#End Region
	End Class


End Namespace
