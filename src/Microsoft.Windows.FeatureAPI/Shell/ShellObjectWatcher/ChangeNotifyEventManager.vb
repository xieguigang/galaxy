Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace Shell
	Friend Class ChangeNotifyEventManager
		#Region "Change order"







		Private Shared ReadOnly _changeOrder As ShellObjectChangeTypes() = {ShellObjectChangeTypes.ItemCreate, ShellObjectChangeTypes.ItemRename, ShellObjectChangeTypes.ItemDelete, ShellObjectChangeTypes.AttributesChange, ShellObjectChangeTypes.DirectoryCreate, ShellObjectChangeTypes.DirectoryDelete, _
			ShellObjectChangeTypes.DirectoryContentsUpdate, ShellObjectChangeTypes.DirectoryRename, ShellObjectChangeTypes.Update, ShellObjectChangeTypes.MediaInsert, ShellObjectChangeTypes.MediaRemove, ShellObjectChangeTypes.DriveAdd, _
			ShellObjectChangeTypes.DriveRemove, ShellObjectChangeTypes.NetShare, ShellObjectChangeTypes.NetUnshare, ShellObjectChangeTypes.ServerDisconnect, ShellObjectChangeTypes.SystemImageUpdate, ShellObjectChangeTypes.AssociationChange, _
			ShellObjectChangeTypes.FreeSpace, ShellObjectChangeTypes.DiskEventsMask, ShellObjectChangeTypes.GlobalEventsMask, ShellObjectChangeTypes.AllEventsMask}
		#End Region

		Private _events As New Dictionary(Of ShellObjectChangeTypes, [Delegate])()

		Public Sub Register(changeType As ShellObjectChangeTypes, handler As [Delegate])
			Dim del As [Delegate]
			If Not _events.TryGetValue(changeType, del) Then
				_events.Add(changeType, handler)
			Else
				del = MulticastDelegate.Combine(del, handler)
				_events(changeType) = del
			End If
		End Sub

		Public Sub Unregister(changeType As ShellObjectChangeTypes, handler As [Delegate])
			Dim del As [Delegate]
			If _events.TryGetValue(changeType, del) Then
				del = MulticastDelegate.Remove(del, handler)
				If del Is Nothing Then
					' It's a bug in .NET if del is non-null and has an empty invocation list.
					_events.Remove(changeType)
				Else
					_events(changeType) = del
				End If
			End If
		End Sub

		Public Sub UnregisterAll()
			_events.Clear()
		End Sub

		Public Sub Invoke(sender As Object, changeType As ShellObjectChangeTypes, args As EventArgs)
			' Removes FromInterrupt flag if pressent
			changeType = changeType And Not ShellObjectChangeTypes.FromInterrupt

			Dim del As [Delegate]
            For Each change As ShellObjectChangeTypes In _changeOrder.Where(Function(x) (x And changeType) <> 0)
                If _events.TryGetValue(change, del) Then
                    del.DynamicInvoke(sender, args)
                End If
            Next
        End Sub

		Public ReadOnly Property RegisteredTypes() As ShellObjectChangeTypes
			Get
                Return _events.Keys.Aggregate(ShellObjectChangeTypes.None, Function(accumulator, changeType) (changeType Or accumulator))
            End Get
		End Property
	End Class
End Namespace
