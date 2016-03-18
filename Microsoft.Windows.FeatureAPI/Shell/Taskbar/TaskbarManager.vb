'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Diagnostics
Imports System.Windows.Interop
Imports Microsoft.Windows.Internal
Imports Microsoft.Windows.Resources

Namespace Taskbar
	''' <summary>
	''' Represents an instance of the Windows taskbar
	''' </summary>
	Public Class TaskbarManager
		' Hide the default constructor
		Private Sub New()
			CoreHelpers.ThrowIfNotWin7()
		End Sub

		' Best practice recommends defining a private object to lock on
		Private Shared _syncLock As New Object()

		Private Shared _instance As TaskbarManager
		''' <summary>
		''' Represents an instance of the Windows Taskbar
		''' </summary>
		Public Shared ReadOnly Property Instance() As TaskbarManager
			Get
				If _instance Is Nothing Then
					SyncLock _syncLock
						If _instance Is Nothing Then
							_instance = New TaskbarManager()
						End If
					End SyncLock
				End If

				Return _instance
			End Get
		End Property

		''' <summary>
		''' Applies an overlay to a taskbar button of the main application window to indicate application status or a notification to the user.
		''' </summary>
		''' <param name="icon">The overlay icon</param>
		''' <param name="accessibilityText">String that provides an alt text version of the information conveyed by the overlay, for accessibility purposes</param>
		Public Sub SetOverlayIcon(icon As System.Drawing.Icon, accessibilityText As String)
			TaskbarList.Instance.SetOverlayIcon(OwnerHandle, If(icon IsNot Nothing, icon.Handle, IntPtr.Zero), accessibilityText)
		End Sub

		''' <summary>
		''' Applies an overlay to a taskbar button of the given window handle to indicate application status or a notification to the user.
		''' </summary>
		''' <param name="windowHandle">The handle of the window whose associated taskbar button receives the overlay. This handle must belong to a calling process associated with the button's application and must be a valid HWND or the call is ignored.</param>
		''' <param name="icon">The overlay icon</param>
		''' <param name="accessibilityText">String that provides an alt text version of the information conveyed by the overlay, for accessibility purposes</param>
		Public Sub SetOverlayIcon(windowHandle As IntPtr, icon As System.Drawing.Icon, accessibilityText As String)
			TaskbarList.Instance.SetOverlayIcon(windowHandle, If(icon IsNot Nothing, icon.Handle, IntPtr.Zero), accessibilityText)
		End Sub

		''' <summary>
		''' Applies an overlay to a taskbar button of the given WPF window to indicate application status or a notification to the user.
		''' </summary>
		''' <param name="window">The window whose associated taskbar button receives the overlay. This window belong to a calling process associated with the button's application and must be already loaded.</param>
		''' <param name="icon">The overlay icon</param>
		''' <param name="accessibilityText">String that provides an alt text version of the information conveyed by the overlay, for accessibility purposes</param>
		Public Sub SetOverlayIcon(window As System.Windows.Window, icon As System.Drawing.Icon, accessibilityText As String)
			TaskbarList.Instance.SetOverlayIcon((New WindowInteropHelper(window)).Handle, If(icon IsNot Nothing, icon.Handle, IntPtr.Zero), accessibilityText)
		End Sub

		''' <summary>
		''' Displays or updates a progress bar hosted in a taskbar button of the main application window 
		''' to show the specific percentage completed of the full operation.
		''' </summary>
		''' <param name="currentValue">An application-defined value that indicates the proportion of the operation that has been completed at the time the method is called.</param>
		''' <param name="maximumValue">An application-defined value that specifies the value currentValue will have when the operation is complete.</param>
		Public Sub SetProgressValue(currentValue As Integer, maximumValue As Integer)
			TaskbarList.Instance.SetProgressValue(OwnerHandle, Convert.ToUInt32(currentValue), Convert.ToUInt32(maximumValue))
		End Sub

		''' <summary>
		''' Displays or updates a progress bar hosted in a taskbar button of the given window handle 
		''' to show the specific percentage completed of the full operation.
		''' </summary>
		''' <param name="windowHandle">The handle of the window whose associated taskbar button is being used as a progress indicator.
		''' This window belong to a calling process associated with the button's application and must be already loaded.</param>
		''' <param name="currentValue">An application-defined value that indicates the proportion of the operation that has been completed at the time the method is called.</param>
		''' <param name="maximumValue">An application-defined value that specifies the value currentValue will have when the operation is complete.</param>
		Public Sub SetProgressValue(currentValue As Integer, maximumValue As Integer, windowHandle As IntPtr)
			TaskbarList.Instance.SetProgressValue(windowHandle, Convert.ToUInt32(currentValue), Convert.ToUInt32(maximumValue))
		End Sub

		''' <summary>
		''' Displays or updates a progress bar hosted in a taskbar button of the given WPF window 
		''' to show the specific percentage completed of the full operation.
		''' </summary>
		''' <param name="window">The window whose associated taskbar button is being used as a progress indicator. 
		''' This window belong to a calling process associated with the button's application and must be already loaded.</param>
		''' <param name="currentValue">An application-defined value that indicates the proportion of the operation that has been completed at the time the method is called.</param>
		''' <param name="maximumValue">An application-defined value that specifies the value currentValue will have when the operation is complete.</param>
		Public Sub SetProgressValue(currentValue As Integer, maximumValue As Integer, window As System.Windows.Window)
			TaskbarList.Instance.SetProgressValue((New WindowInteropHelper(window)).Handle, Convert.ToUInt32(currentValue), Convert.ToUInt32(maximumValue))
		End Sub

		''' <summary>
		''' Sets the type and state of the progress indicator displayed on a taskbar button of the main application window.
		''' </summary>
		''' <param name="state">Progress state of the progress button</param>
		Public Sub SetProgressState(state As TaskbarProgressBarState)
			TaskbarList.Instance.SetProgressState(OwnerHandle, CType(state, TaskbarProgressBarStatus))
		End Sub

		''' <summary>
		''' Sets the type and state of the progress indicator displayed on a taskbar button 
		''' of the given window handle 
		''' </summary>
		''' <param name="windowHandle">The handle of the window whose associated taskbar button is being used as a progress indicator.
		''' This window belong to a calling process associated with the button's application and must be already loaded.</param>
		''' <param name="state">Progress state of the progress button</param>
		Public Sub SetProgressState(state As TaskbarProgressBarState, windowHandle As IntPtr)
			TaskbarList.Instance.SetProgressState(windowHandle, CType(state, TaskbarProgressBarStatus))
		End Sub

		''' <summary>
		''' Sets the type and state of the progress indicator displayed on a taskbar button 
		''' of the given WPF window
		''' </summary>
		''' <param name="window">The window whose associated taskbar button is being used as a progress indicator. 
		''' This window belong to a calling process associated with the button's application and must be already loaded.</param>
		''' <param name="state">Progress state of the progress button</param>
		Public Sub SetProgressState(state As TaskbarProgressBarState, window As System.Windows.Window)
			TaskbarList.Instance.SetProgressState((New WindowInteropHelper(window)).Handle, CType(state, TaskbarProgressBarStatus))
		End Sub

		Private _tabbedThumbnail As TabbedThumbnailManager
		''' <summary>
		''' Gets the Tabbed Thumbnail manager class for adding/updating
		''' tabbed thumbnail previews.
		''' </summary>
		Public ReadOnly Property TabbedThumbnail() As TabbedThumbnailManager
			Get
				If _tabbedThumbnail Is Nothing Then
					_tabbedThumbnail = New TabbedThumbnailManager()
				End If
				Return _tabbedThumbnail
			End Get
		End Property

		Private _thumbnailToolBarManager As ThumbnailToolBarManager
		''' <summary>
		''' Gets the Thumbnail toolbar manager class for adding/updating
		''' toolbar buttons.
		''' </summary>
		Public ReadOnly Property ThumbnailToolBars() As ThumbnailToolBarManager
			Get
				If _thumbnailToolBarManager Is Nothing Then
					_thumbnailToolBarManager = New ThumbnailToolBarManager()
				End If

				Return _thumbnailToolBarManager
			End Get
		End Property

		''' <summary>
		''' Gets or sets the application user model id. Use this to explicitly
		''' set the application id when generating custom jump lists
		''' </summary>
		Public Property ApplicationId() As String
			Get
				Return GetCurrentProcessAppId()
			End Get
			Set
				If String.IsNullOrEmpty(value) Then
					Throw New ArgumentNullException("value")
				End If

				SetCurrentProcessAppId(value)
				ApplicationIdSetProcessWide = True
			End Set
		End Property

		Private _ownerHandle As IntPtr
		''' <summary>
		''' Sets the handle of the window whose taskbar button will be used
		''' to display progress.
		''' </summary>
		Friend ReadOnly Property OwnerHandle() As IntPtr
			Get
				If _ownerHandle = IntPtr.Zero Then
					Dim currentProcess As Process = Process.GetCurrentProcess()

					If currentProcess Is Nothing OrElse currentProcess.MainWindowHandle = IntPtr.Zero Then
						Throw New InvalidOperationException(LocalizedMessages.TaskbarManagerValidWindowRequired)
					End If

					_ownerHandle = currentProcess.MainWindowHandle
				End If

				Return _ownerHandle
			End Get
		End Property

		''' <summary>
		''' Sets the application user model id for an individual window
		''' </summary>
		''' <param name="appId">The app id to set</param>
		''' <param name="windowHandle">Window handle for the window that needs a specific application id</param>
		''' <remarks>AppId specifies a unique Application User Model ID (AppID) for the application or individual 
		''' top-level window whose taskbar button will hold the custom JumpList built through the methods <see cref="Taskbar.JumpList"/> class.
		''' By setting an appId for a specific window, the window will not be grouped with it's parent window/application. Instead it will have it's own taskbar button.</remarks>
		Public Sub SetApplicationIdForSpecificWindow(windowHandle As IntPtr, appId As String)
			' Left as instance method, to follow singleton pattern.
			TaskbarNativeMethods.SetWindowAppId(windowHandle, appId)
		End Sub

		''' <summary>
		''' Sets the application user model id for a given window
		''' </summary>
		''' <param name="appId">The app id to set</param>
		''' <param name="window">Window that needs a specific application id</param>
		''' <remarks>AppId specifies a unique Application User Model ID (AppID) for the application or individual 
		''' top-level window whose taskbar button will hold the custom JumpList built through the methods <see cref="Taskbar.JumpList"/> class.
		''' By setting an appId for a specific window, the window will not be grouped with it's parent window/application. Instead it will have it's own taskbar button.</remarks>
		Public Sub SetApplicationIdForSpecificWindow(window As System.Windows.Window, appId As String)
			' Left as instance method, to follow singleton pattern.
			TaskbarNativeMethods.SetWindowAppId((New WindowInteropHelper(window)).Handle, appId)
		End Sub

		''' <summary>
		''' Sets the current process' explicit application user model id.
		''' </summary>
		''' <param name="appId">The application id.</param>
		Private Sub SetCurrentProcessAppId(appId As String)
			TaskbarNativeMethods.SetCurrentProcessExplicitAppUserModelID(appId)
		End Sub

		''' <summary>
		''' Gets the current process' explicit application user model id.
		''' </summary>
		''' <returns>The app id or null if no app id has been defined.</returns>
		Private Function GetCurrentProcessAppId() As String
			Dim appId As String = String.Empty
			TaskbarNativeMethods.GetCurrentProcessExplicitAppUserModelID(appId)
			Return appId
		End Function

		''' <summary>
		''' Indicates if the user has set the application id for the whole process (all windows)
		''' </summary>
		Friend Property ApplicationIdSetProcessWide() As Boolean
			Get
				Return m_ApplicationIdSetProcessWide
			End Get
			Private Set
				m_ApplicationIdSetProcessWide = Value
			End Set
		End Property
		Private m_ApplicationIdSetProcessWide As Boolean

		''' <summary>
		''' Indicates whether this feature is supported on the current platform.
		''' </summary>
		Public Shared ReadOnly Property IsPlatformSupported() As Boolean
			Get
				' We need Windows 7 onwards ...
				Return CoreHelpers.RunningOnWin7
			End Get
		End Property
	End Class
End Namespace
