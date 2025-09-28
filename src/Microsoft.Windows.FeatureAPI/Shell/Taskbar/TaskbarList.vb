
Namespace Taskbar
	''' <summary>
	''' Provides internal access to the functions provided by the ITaskbarList4 interface,
	''' without being forced to refer to it through another singleton.
	''' </summary>
	Friend NotInheritable Class TaskbarList
		Private Sub New()
		End Sub
		Private Shared _syncLock As New Object()

		Private Shared _taskbarList As ITaskbarList4
		Friend Shared ReadOnly Property Instance() As ITaskbarList4
			Get
				If _taskbarList Is Nothing Then
					SyncLock _syncLock
						If _taskbarList Is Nothing Then
							_taskbarList = DirectCast(New CTaskbarList(), ITaskbarList4)
							_taskbarList.HrInit()
						End If
					End SyncLock
				End If

				Return _taskbarList
			End Get
		End Property
	End Class
End Namespace
