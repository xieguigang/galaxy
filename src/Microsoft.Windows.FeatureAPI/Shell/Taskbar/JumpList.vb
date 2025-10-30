'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Collections
Imports System.Collections.Generic
Imports System.Runtime.InteropServices
Imports System.Text
Imports Microsoft.Windows.Internal
Imports Microsoft.Windows.Shell

Namespace Taskbar
	''' <summary>
	''' Represents an instance of a Taskbar button jump list.
	''' </summary>
	Public Class JumpList

		''' <summary>
		''' Create a JumpList for the application's taskbar button.
		''' </summary>
		''' <returns>A new JumpList that is associated with the app id of the main application window</returns>
		''' <remarks>If there are any other child (top-level) windows for this application and they don't have
		''' a specific JumpList created for them, they all will share the same JumpList as the main application window.
		''' In order to have a individual JumpList for a top-level window, use the overloaded method CreateJumpListForIndividualWindow.</remarks>
		Public Shared Function CreateJumpList() As JumpList
			Return New JumpList(TaskbarManager.Instance.ApplicationId)
		End Function

		''' <summary>
		''' Create a JumpList for the application's taskbar button.
		''' </summary>
		''' <param name="appId">Application Id for the individual window. This must be unique for each top-level window in order to have a individual JumpList.</param>
		''' <param name="windowHandle">Handle of the window associated with the new JumpList</param>
		''' <returns>A new JumpList that is associated with the specific window handle</returns>
		Public Shared Function CreateJumpListForIndividualWindow(appId As String, windowHandle As IntPtr) As JumpList
			Return New JumpList(appId, windowHandle)
		End Function

		''' <summary>
		''' Create a JumpList for the application's taskbar button.
		''' </summary>
		''' <param name="appId">Application Id for the individual window. This must be unique for each top-level window in order to have a individual JumpList.</param>
		''' <param name="window">WPF Window associated with the new JumpList</param>
		''' <returns>A new JumpList that is associated with the specific WPF window</returns>
		Public Shared Function CreateJumpListForIndividualWindow(appId As String, window As System.Windows.Window) As JumpList
			Return New JumpList(appId, window)
		End Function

		' Best practice recommends defining a private object to lock on
		Private ReadOnly [syncLock] As Object = New [Object]()

		' Native implementation of destination list
		Private customDestinationList As ICustomDestinationList

		#Region "Properties"

		Private customCategoriesCollection As JumpListCustomCategoryCollection
		''' <summary>
		''' Adds a collection of custom categories to the Taskbar jump list.
		''' </summary>
		''' <param name="customCategories">The catagories to add to the jump list.</param>
		Public Sub AddCustomCategories(ParamArray customCategories As JumpListCustomCategory())
			SyncLock [syncLock]
				If customCategoriesCollection Is Nothing Then
					customCategoriesCollection = New JumpListCustomCategoryCollection()
				End If
			End SyncLock

			If customCategories IsNot Nothing Then
				For Each category As JumpListCustomCategory In customCategories
					customCategoriesCollection.Add(category)
				Next
			End If
		End Sub

		Private userTasks As JumpListItemCollection(Of JumpListTask)
		''' <summary>
		''' Adds user tasks to the Taskbar JumpList. User tasks can only consist of JumpListTask or
		''' JumpListSeparator objects.
		''' </summary>
		''' <param name="tasks">The user tasks to add to the JumpList.</param>
		Public Sub AddUserTasks(ParamArray tasks As JumpListTask())
			If userTasks Is Nothing Then
				' Make sure that we don't create multiple instances
				' of this object
				SyncLock [syncLock]
					If userTasks Is Nothing Then
						userTasks = New JumpListItemCollection(Of JumpListTask)()
					End If
				End SyncLock
			End If

			If tasks IsNot Nothing Then
				For Each task As JumpListTask In tasks
					userTasks.Add(task)
				Next
			End If
		End Sub

		''' <summary>
		''' Removes all user tasks that have been added.
		''' </summary>
		Public Sub ClearAllUserTasks()
			If userTasks IsNot Nothing Then
				userTasks.Clear()
			End If
		End Sub

		''' <summary>
		''' Gets the recommended number of items to add to the jump list.  
		''' </summary>
		''' <remarks>
		''' This number doesn’t 
		''' imply or suggest how many items will appear on the jump list.  
		''' This number should only be used for reference purposes since
		''' the actual number of slots in the jump list can change after the last
		''' refresh due to items being pinned or removed and resolution changes. 
		''' The jump list can increase in size accordingly.
		''' </remarks>
		Public ReadOnly Property MaxSlotsInList() As UInteger
			Get
				' Because we need the correct number for max slots, start a commit, get the max slots
				' and then abort. If we wait until the user calls RefreshTaskbarlist(), it will be too late.
				' The user needs to use this number before they update the jumplist.

				Dim removedItems As Object
				Dim maxSlotsInList__1 As UInteger = 10
				' default
				' Native call to start adding items to the taskbar destination list
				Dim hr As HResult = customDestinationList.BeginList(maxSlotsInList__1, TaskbarNativeMethods.TaskbarGuids.IObjectArray, removedItems)

				If CoreErrorHelper.Succeeded(hr) Then
					customDestinationList.AbortList()
				End If

				Return maxSlotsInList__1
			End Get
		End Property

		''' <summary>
		''' Gets or sets the type of known categories to display.
		''' </summary>
		Public Property KnownCategoryToDisplay() As JumpListKnownCategoryType
			Get
				Return m_KnownCategoryToDisplay
			End Get
			Set
				m_KnownCategoryToDisplay = Value
			End Set
		End Property
		Private m_KnownCategoryToDisplay As JumpListKnownCategoryType

		Private m_knownCategoryOrdinalPosition As Integer
		''' <summary>
		''' Gets or sets the value for the known category location relative to the 
		''' custom category collection.
		''' </summary>
		Public Property KnownCategoryOrdinalPosition() As Integer
			Get
				Return m_knownCategoryOrdinalPosition
			End Get
			Set
				If value < 0 Then
					Throw New ArgumentOutOfRangeException("value", LocalizedMessages.JumpListNegativeOrdinalPosition)
				End If

				m_knownCategoryOrdinalPosition = value
			End Set
		End Property


		''' <summary>
		''' Gets or sets the application ID to use for this jump list.
		''' </summary>
		Public Property ApplicationId() As String
			Get
				Return m_ApplicationId
			End Get
			Private Set
				m_ApplicationId = Value
			End Set
		End Property
		Private m_ApplicationId As String

		#End Region

		''' <summary>
		''' Creates a new instance of the JumpList class with the specified
		''' appId. The JumpList is associated with the main window of the application.
		''' </summary>
		''' <param name="appID">Application Id to use for this instace.</param>
		Friend Sub New(appID As String)
			Me.New(appID, TaskbarManager.Instance.OwnerHandle)
		End Sub

		''' <summary>
		''' Creates a new instance of the JumpList class with the specified
		''' appId. The JumpList is associated with the given WPF Window.
		''' </summary>
		''' <param name="appID">Application Id to use for this instace.</param>
		''' <param name="window">WPF Window that is associated with this JumpList</param>
		Friend Sub New(appID As String, window As System.Windows.Window)
			Me.New(appID, (New System.Windows.Interop.WindowInteropHelper(window)).Handle)
		End Sub

		''' <summary>
		''' Creates a new instance of the JumpList class with the specified
		''' appId. The JumpList is associated with the given window.
		''' </summary>
		''' <param name="appID">Application Id to use for this instace.</param>
		''' <param name="windowHandle">Window handle for the window that is associated with this JumpList</param>
		Private Sub New(appID As String, windowHandle As IntPtr)
			' Throw exception if not running on Win7 or newer
			CoreHelpers.ThrowIfNotWin7()

			' Native implementation of destination list
			customDestinationList = DirectCast(New CDestinationList(), ICustomDestinationList)

			' Set application user model ID
			If Not String.IsNullOrEmpty(appID) Then
				ApplicationId = appID

				' If the user hasn't yet set the application id for the whole process,
				' use the first JumpList's AppId for the whole process. This will ensure
				' we have the same JumpList for all the windows (unless user overrides and creates a new
				' JumpList for a specific child window)
				If Not TaskbarManager.Instance.ApplicationIdSetProcessWide Then
					TaskbarManager.Instance.ApplicationId = appID
				End If

				TaskbarManager.Instance.SetApplicationIdForSpecificWindow(windowHandle, appID)
			End If
		End Sub

		''' <summary>
		''' Reports document usage to the shell.
		''' </summary>
		''' <param name="destination">The full path of the file to report usage.</param>
		Public Shared Sub AddToRecent(destination As String)
			TaskbarNativeMethods.SHAddToRecentDocs(destination)
		End Sub

		''' <summary>
		''' Commits the pending JumpList changes and refreshes the Taskbar.
		''' </summary>
		''' <exception cref="System.InvalidOperationException">Will throw if the type of the file being added to the JumpList is not registered with the application.</exception>
		''' <exception cref="System.UnauthorizedAccessException">Will throw if recent documents tracking is turned off by the user or via group policy.</exception>
		''' <exception cref="System.Runtime.InteropServices.COMException">Will throw if updating the JumpList fails for any other reason.</exception>
		Public Sub Refresh()
			' Let the taskbar know which specific jumplist we are updating
			If Not String.IsNullOrEmpty(ApplicationId) Then
				customDestinationList.SetAppID(ApplicationId)
			End If

			' Begins rendering on the taskbar destination list
			BeginList()

			Dim exception As Exception = Nothing

			Try
				' try to add the user tasks first
				AppendTaskList()
			Catch e As Exception
				' If this fails, save the exception but don't throw it yet.
				' We need to continue to try and add the custom categories
				exception = e
			End Try

			' Even it fails, continue appending the custom categories
			Try
				' Add custom categories
				AppendCustomCategories()
			Finally
				' End rendering of the taskbar destination list
				customDestinationList.CommitList()
			End Try

			' If an exception was thrown while adding the user tasks or
			' custom categories, throw it.
			If exception IsNot Nothing Then
				Throw exception
			End If
		End Sub

		Private Sub BeginList()
			' Get list of removed items from native code
			Dim removedItems As Object
			Dim maxSlotsInList As UInteger = 10
			' default
			' Native call to start adding items to the taskbar destination list
			Dim hr As HResult = customDestinationList.BeginList(maxSlotsInList, TaskbarNativeMethods.TaskbarGuids.IObjectArray, removedItems)

			If Not CoreErrorHelper.Succeeded(hr) Then
				Throw New ShellException(hr)
			End If

			' Process the deleted items
			Dim removedItemsArray As IEnumerable = ProcessDeletedItems(DirectCast(removedItems, IObjectArray))

            ' Raise the event if items were removed
            If _JumpListItemsRemoved IsNot Nothing AndAlso removedItemsArray IsNot Nothing AndAlso removedItemsArray.GetEnumerator().MoveNext() Then
                RaiseEvent JumpListItemsRemoved(Me, New UserRemovedJumpListItemsEventArgs(removedItemsArray))
            End If
        End Sub

        Dim _JumpListItemsRemoved As EventHandler(Of UserRemovedJumpListItemsEventArgs)

        ''' <summary>
        ''' Occurs when items are removed from the Taskbar's jump list since the last
        ''' refresh. 
        ''' </summary>
        ''' <remarks>
        ''' This event is not triggered
        ''' immediately when a user removes an item from the jump list but rather
        ''' when the application refreshes the task bar list directly.
        ''' </remarks>
        Public Custom Event JumpListItemsRemoved As EventHandler(Of UserRemovedJumpListItemsEventArgs)
            AddHandler(value As EventHandler(Of UserRemovedJumpListItemsEventArgs))
                Me._JumpListItemsRemoved = value
            End AddHandler
            RemoveHandler(value As EventHandler(Of UserRemovedJumpListItemsEventArgs))
                Me._JumpListItemsRemoved = Nothing
            End RemoveHandler
            RaiseEvent(sender As Object, e As UserRemovedJumpListItemsEventArgs)
                Call Me._JumpListItemsRemoved.Invoke(sender, e)
            End RaiseEvent
        End Event

        ''' <summary>
        ''' Retrieves the current list of destinations that have been removed from the existing jump list by the user. 
        ''' The removed destinations may become items on a custom jump list.
        ''' </summary>
        ''' <value>A collection of items (filenames) removed from the existing jump list by the user.</value>
        Public ReadOnly Property RemovedDestinations() As IEnumerable
			Get
				' Get list of removed items from native code
				Dim removedItems As Object

				customDestinationList.GetRemovedDestinations(TaskbarNativeMethods.TaskbarGuids.IObjectArray, removedItems)

				Return ProcessDeletedItems(DirectCast(removedItems, IObjectArray))
			End Get
		End Property

		Private Function ProcessDeletedItems(removedItems As IObjectArray) As IEnumerable(Of String)
			Dim removedItemsArray As New List(Of String)()

			Dim count As UInteger
			removedItems.GetCount(count)

            ' Process each removed item based on its type
            For i As UInteger = 0 To count - 1UI
                ' Native call to retrieve objects from IObjectArray
                Dim item As Object
                removedItems.GetAt(i, TaskbarNativeMethods.TaskbarGuids.IUnknown, item)

                Dim shellItem As IShellItem = TryCast(item, IShellItem)
                Dim shellLink As IShellLinkW
				' Process item
				If shellItem IsNot Nothing Then
					removedItemsArray.Add(RemoveCustomCategoryItem(shellItem))
				ElseIf shellLink.InlineCopy(TryCast(item, IShellLinkW)) IsNot Nothing Then
					removedItemsArray.Add(RemoveCustomCategoryLink(shellLink))
                End If
            Next
            Return removedItemsArray
		End Function

		Private Function RemoveCustomCategoryItem(item As IShellItem) As String
			Dim path As String = Nothing

			If customCategoriesCollection IsNot Nothing Then
				Dim pszString As IntPtr = IntPtr.Zero
				Dim hr As HResult = item.GetDisplayName(ShellNativeMethods.ShellItemDesignNameOptions.FileSystemPath, pszString)
				If hr = HResult.Ok AndAlso pszString <> IntPtr.Zero Then
					path = Marshal.PtrToStringAuto(pszString)
					' Free the string
					Marshal.FreeCoTaskMem(pszString)
				End If

				' Remove this item from each category
				For Each category As JumpListCustomCategory In customCategoriesCollection
					category.RemoveJumpListItem(path)

				Next
			End If

			Return path
		End Function


		Private Function RemoveCustomCategoryLink(link As IShellLinkW) As String
			Dim path As String = Nothing

			If customCategoriesCollection IsNot Nothing Then
				Dim sb As New StringBuilder(256)
				link.GetPath(sb, sb.Capacity, IntPtr.Zero, 2)

				path = sb.ToString()

				' Remove this item from each category
				For Each category As JumpListCustomCategory In customCategoriesCollection
					category.RemoveJumpListItem(path)
				Next
			End If

			Return path
		End Function

		Private Sub AppendCustomCategories()
			' Initialize our current index in the custom categories list
			Dim currentIndex As Integer = 0

			' Keep track whether we add the Known Categories to our list
			Dim knownCategoriesAdded As Boolean = False

			If customCategoriesCollection IsNot Nothing Then
				' Append each category to list
				For Each category As JumpListCustomCategory In customCategoriesCollection
					' If our current index is same as the KnownCategory OrdinalPosition,
					' append the Known Categories
					If Not knownCategoriesAdded AndAlso currentIndex = KnownCategoryOrdinalPosition Then
						AppendKnownCategories()
						knownCategoriesAdded = True
					End If

					' Don't process empty categories
					If category.JumpListItems.Count = 0 Then
						Continue For
					End If

					Dim categoryContent As IObjectCollection = DirectCast(New CEnumerableObjectCollection(), IObjectCollection)

					' Add each link's shell representation to the object array
					For Each link As IJumpListItem In category.JumpListItems
						Dim listItem As JumpListItem = TryCast(link, JumpListItem)
						Dim listLink As JumpListLink = TryCast(link, JumpListLink)
						If listItem IsNot Nothing Then
							categoryContent.AddObject(listItem.NativeShellItem)
						ElseIf listLink IsNot Nothing Then
							categoryContent.AddObject(listLink.NativeShellLink)
						End If
					Next

					' Add current category to destination list
					Dim hr As HResult = customDestinationList.AppendCategory(category.Name, DirectCast(categoryContent, IObjectArray))

					If Not CoreErrorHelper.Succeeded(hr) Then
						If CUInt(hr) = &H80040f03UI Then
							Throw New InvalidOperationException(LocalizedMessages.JumpListFileTypeNotRegistered)
						ElseIf CUInt(hr) = &H80070005UI Then
							'E_ACCESSDENIED
							' If the recent documents tracking is turned off by the user,
							' custom categories or items to an existing category cannot be added.
							' The recent documents tracking can be changed via:
							'      1. Group Policy “Do not keep history of recently opened documents”.
							'      2. Via the user setting “Store and display recently opened items in 
							'         the Start menu and the taskbar” in the Start menu property dialog.
							'
							Throw New UnauthorizedAccessException(LocalizedMessages.JumpListCustomCategoriesDisabled)
						End If

						Throw New ShellException(hr)
					End If

					' Increase our current index
					currentIndex += 1
				Next
			End If

			' If the ordinal position was out of range, append the Known Categories
			' at the end
			If Not knownCategoriesAdded Then
				AppendKnownCategories()
			End If
		End Sub

		Private Sub AppendTaskList()
			If userTasks Is Nothing OrElse userTasks.Count = 0 Then
				Return
			End If

			Dim taskContent As IObjectCollection = DirectCast(New CEnumerableObjectCollection(), IObjectCollection)

			' Add each task's shell representation to the object array
			For Each task As JumpListTask In userTasks
				Dim seperator As JumpListSeparator
				Dim link As JumpListLink = TryCast(task, JumpListLink)
				If link IsNot Nothing Then
					taskContent.AddObject(link.NativeShellLink)
				ElseIf seperator.InlineCopy(TryCast(task, JumpListSeparator)) IsNot Nothing Then
					taskContent.AddObject(seperator.NativeShellLink)
				End If
			Next

			' Add tasks to the taskbar
			Dim hr As HResult = customDestinationList.AddUserTasks(DirectCast(taskContent, IObjectArray))

			If Not CoreErrorHelper.Succeeded(hr) Then
				If CUInt(hr) = &H80040f03UI Then
					Throw New InvalidOperationException(LocalizedMessages.JumpListFileTypeNotRegistered)
				End If
				Throw New ShellException(hr)
			End If
		End Sub

		Private Sub AppendKnownCategories()
			If KnownCategoryToDisplay = JumpListKnownCategoryType.Recent Then
				customDestinationList.AppendKnownCategory(KnownDestinationCategory.Recent)
			ElseIf KnownCategoryToDisplay = JumpListKnownCategoryType.Frequent Then
				customDestinationList.AppendKnownCategory(KnownDestinationCategory.Frequent)
			End If
		End Sub

    End Class
End Namespace
