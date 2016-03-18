
Namespace Shell
	''' <summary>
	''' Base class for the Event Args for change notifications raised by <see cref="ShellObjectWatcher"/>.
	''' </summary>
	Public Class ShellObjectNotificationEventArgs
		Inherits EventArgs
		''' <summary>
		''' The type of the change that happened to the ShellObject
		''' </summary>
		Public Property ChangeType() As ShellObjectChangeTypes
			Get
				Return m_ChangeType
			End Get
			Private Set
				m_ChangeType = Value
			End Set
		End Property
		Private m_ChangeType As ShellObjectChangeTypes

		''' <summary>
		''' True if the event was raised as a result of a system interrupt.
		''' </summary>
		Public Property FromSystemInterrupt() As Boolean
			Get
				Return m_FromSystemInterrupt
			End Get
			Private Set
				m_FromSystemInterrupt = Value
			End Set
		End Property
		Private m_FromSystemInterrupt As Boolean

		Friend Sub New(notifyLock As ChangeNotifyLock)
			ChangeType = notifyLock.ChangeType
			FromSystemInterrupt = notifyLock.FromSystemInterrupt
		End Sub
	End Class

	''' <summary>
	''' The data that describes a ShellObject event with a single path parameter
	''' </summary>
	Public Class ShellObjectChangedEventArgs
		Inherits ShellObjectNotificationEventArgs
		''' <summary>
		''' The path of the shell object
		''' </summary>
		Public Property Path() As String
			Get
				Return m_Path
			End Get
			Private Set
				m_Path = Value
			End Set
		End Property
		Private m_Path As String

		Friend Sub New(notifyLock As ChangeNotifyLock)
			MyBase.New(notifyLock)
			Path = notifyLock.ItemName
		End Sub
	End Class

	''' <summary>
	''' The data that describes a ShellObject renamed event
	''' </summary>
	Public Class ShellObjectRenamedEventArgs
		Inherits ShellObjectChangedEventArgs
		''' <summary>
		''' The new path of the shell object
		''' </summary>
		Public Property NewPath() As String
			Get
				Return m_NewPath
			End Get
			Private Set
				m_NewPath = Value
			End Set
		End Property
		Private m_NewPath As String

		Friend Sub New(notifyLock As ChangeNotifyLock)
			MyBase.New(notifyLock)
			NewPath = notifyLock.ItemName2
		End Sub
	End Class

	''' <summary>
	''' The data that describes a SystemImageUpdated event.
	''' </summary>
	Public Class SystemImageUpdatedEventArgs
		Inherits ShellObjectNotificationEventArgs
		''' <summary>
		''' Gets the index of the system image that has been updated.
		''' </summary>
		Public Property ImageIndex() As Integer
			Get
				Return m_ImageIndex
			End Get
			Private Set
				m_ImageIndex = Value
			End Set
		End Property
		Private m_ImageIndex As Integer

		Friend Sub New(notifyLock As ChangeNotifyLock)
			MyBase.New(notifyLock)
			ImageIndex = notifyLock.ImageIndex
		End Sub
	End Class


End Namespace
