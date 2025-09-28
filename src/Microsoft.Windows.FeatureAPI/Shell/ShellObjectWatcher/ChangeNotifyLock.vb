Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.InteropServices
Imports System.Diagnostics
Imports Microsoft.Windows.Shell.Interop
Imports Microsoft.Windows.Internal

Namespace Shell
	Friend Class ChangeNotifyLock
		Private _event As UInteger = 0

		Friend Sub New(message As Message)
			Dim pidl As IntPtr
			Dim lockId As IntPtr = ShellNativeMethods.SHChangeNotification_Lock(message.WParam, CInt(message.LParam), pidl, _event)
			Try
				Trace.TraceInformation("Message: {0}", CType(_event, ShellObjectChangeTypes))

				Dim notifyStruct = pidl.MarshalAs(Of ShellNativeMethods.ShellNotifyStruct)()

				Dim guid As New Guid(ShellIIDGuid.IShellItem2)
				If notifyStruct.item1 <> IntPtr.Zero AndAlso (CType(_event, ShellObjectChangeTypes) And ShellObjectChangeTypes.SystemImageUpdate) = ShellObjectChangeTypes.None Then
					Dim nativeShellItem As IShellItem2
					If CoreErrorHelper.Succeeded(ShellNativeMethods.SHCreateItemFromIDList(notifyStruct.item1, guid, nativeShellItem)) Then
						Dim name As String
						nativeShellItem.GetDisplayName(ShellNativeMethods.ShellItemDesignNameOptions.FileSystemPath, name)
						ItemName = name

						Trace.TraceInformation("Item1: {0}", ItemName)
					End If
				Else
					ImageIndex = notifyStruct.item1.ToInt32()
				End If

				If notifyStruct.item2 <> IntPtr.Zero Then
					Dim nativeShellItem As IShellItem2
					If CoreErrorHelper.Succeeded(ShellNativeMethods.SHCreateItemFromIDList(notifyStruct.item2, guid, nativeShellItem)) Then
						Dim name As String
						nativeShellItem.GetDisplayName(ShellNativeMethods.ShellItemDesignNameOptions.FileSystemPath, name)
						ItemName2 = name

						Trace.TraceInformation("Item2: {0}", ItemName2)
					End If
				End If
			Finally
				If lockId <> IntPtr.Zero Then
					ShellNativeMethods.SHChangeNotification_Unlock(lockId)
				End If

			End Try
		End Sub

		Public ReadOnly Property FromSystemInterrupt() As Boolean
			Get
				Return (CType(_event, ShellObjectChangeTypes) And ShellObjectChangeTypes.FromInterrupt) <> ShellObjectChangeTypes.None
			End Get
		End Property

		Public Property ImageIndex() As Integer
			Get
				Return m_ImageIndex
			End Get
			Private Set
				m_ImageIndex = Value
			End Set
		End Property
		Private m_ImageIndex As Integer
		Public Property ItemName() As String
			Get
				Return m_ItemName
			End Get
			Private Set
				m_ItemName = Value
			End Set
		End Property
		Private m_ItemName As String
		Public Property ItemName2() As String
			Get
				Return m_ItemName2
			End Get
			Private Set
				m_ItemName2 = Value
			End Set
		End Property
		Private m_ItemName2 As String

		Public ReadOnly Property ChangeType() As ShellObjectChangeTypes
			Get
				Return CType(_event, ShellObjectChangeTypes)
			End Get
		End Property


	End Class
End Namespace
