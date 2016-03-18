'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Drawing
Imports Microsoft.Windows.Shell
Imports Microsoft.Windows.Internal

Namespace Taskbar
	''' <summary>
	''' Represents a taskbar thumbnail button in the thumbnail toolbar.
	''' </summary>
	Public NotInheritable Class ThumbnailToolBarButton
		Implements IDisposable
		Private Shared nextId As UInteger = 101
		Private m_win32ThumbButton As ThumbButton

        Dim _Click As EventHandler(Of ThumbnailButtonClickedEventArgs)

        ''' <summary>
        ''' The event that occurs when the taskbar thumbnail button
        ''' is clicked.
        ''' </summary>
        Public Custom Event Click As EventHandler(Of ThumbnailButtonClickedEventArgs)
            AddHandler(value As EventHandler(Of ThumbnailButtonClickedEventArgs))
                Me._Click = value
            End AddHandler
            RemoveHandler(value As EventHandler(Of ThumbnailButtonClickedEventArgs))
                Me._Click = Nothing
            End RemoveHandler
            RaiseEvent(sender As Object, e As ThumbnailButtonClickedEventArgs)
                Call Me._Click(sender, e)
            End RaiseEvent
        End Event

        ' Internal bool to track whether we should be updating the taskbar 
        ' if any of our properties change or if it's just an internal update
        ' on the properties (via the constructor)
        Private internalUpdate As Boolean = False

        ''' <summary>
        ''' Initializes an instance of this class
        ''' </summary>
        ''' <param name="icon__1">The icon to use for this button</param>
        ''' <param name="tooltip__2">The tooltip string to use for this button.</param>
        Public Sub New(icon__1 As Icon, tooltip__2 As String)
			' Start internal update (so we don't accidently update the taskbar
			' via the native API)
			internalUpdate = True

			' Set our id
			Id = nextId

			' increment the ID
			If nextId = Int32.MaxValue Then
				nextId = 101
			Else
                ' our starting point
                nextId += 1UI
            End If

			' Set user settings
			Icon = icon__1
			Tooltip = tooltip__2

			' Defaults
			Enabled = True

			' Create a native 
			m_win32ThumbButton = New ThumbButton()

			' End our internal update
			internalUpdate = False
		End Sub

		#Region "Public properties"

		''' <summary>
		''' Gets thumbnail button's id.
		''' </summary>
		Friend Property Id() As UInteger
			Get
				Return m_Id
			End Get
			Set
				m_Id = Value
			End Set
		End Property
		Private m_Id As UInteger

		Private m_icon As Icon
		''' <summary>
		''' Gets or sets the thumbnail button's icon.
		''' </summary>
		Public Property Icon() As Icon
			Get
				Return m_icon
			End Get
			Set
				If m_icon IsNot value Then
					m_icon = value
					UpdateThumbnailButton()
				End If
			End Set
		End Property

		Private m_tooltip As String
		''' <summary>
		''' Gets or sets the thumbnail button's tooltip.
		''' </summary>
		Public Property Tooltip() As String
			Get
				Return m_tooltip
			End Get
			Set
				If m_tooltip <> value Then
					m_tooltip = value
					UpdateThumbnailButton()
				End If
			End Set
		End Property

		Private m_visible As Boolean = True
		''' <summary>
		''' Gets or sets the thumbnail button's visibility. Default is true.
		''' </summary>
		Public Property Visible() As Boolean
			Get
				Return (Me.Flags And ThumbButtonOptions.Hidden) = 0
			End Get
			Set
				If m_visible <> value Then
					m_visible = value

					If value Then
						Me.Flags = Me.Flags And Not (ThumbButtonOptions.Hidden)
					Else
						Me.Flags = Me.Flags Or ThumbButtonOptions.Hidden
					End If

					UpdateThumbnailButton()

				End If
			End Set
		End Property

		Private m_enabled As Boolean = True
		''' <summary>
		''' Gets or sets the thumbnail button's enabled state. If the button is disabled, it is present, 
		''' but has a visual state that indicates that it will not respond to user action. Default is true.
		''' </summary>
		Public Property Enabled() As Boolean
			Get
				Return (Me.Flags And ThumbButtonOptions.Disabled) = 0
			End Get
			Set
				If value <> m_enabled Then
					m_enabled = value

					If value Then
						Me.Flags = Me.Flags And Not (ThumbButtonOptions.Disabled)
					Else
						Me.Flags = Me.Flags Or ThumbButtonOptions.Disabled
					End If

					UpdateThumbnailButton()
				End If
			End Set
		End Property

		Private m_dismissOnClick As Boolean
		''' <summary>
		''' Gets or sets the property that describes the behavior when the button is clicked. 
		''' If set to true, the taskbar button's flyout will close immediately. Default is false.
		''' </summary>
		Public Property DismissOnClick() As Boolean
			Get
				Return (Me.Flags And ThumbButtonOptions.DismissOnClick) = 0
			End Get
			Set
				If value <> m_dismissOnClick Then
					m_dismissOnClick = value

					If value Then
						Me.Flags = Me.Flags Or ThumbButtonOptions.DismissOnClick
					Else
						Me.Flags = Me.Flags And Not (ThumbButtonOptions.DismissOnClick)
					End If

					UpdateThumbnailButton()
				End If
			End Set
		End Property

		Private m_isInteractive As Boolean = True
		''' <summary>
		''' Gets or sets the property that describes whether the button is interactive with the user. Default is true.
		''' </summary>
		''' <remarks>
		''' Non-interactive buttons don't display any hover behavior nor do they raise click events.
		''' They are intended to be used as status icons. This is mostly similar to being not Enabled, 
		''' but the image is not desaturated.
		''' </remarks>
		Public Property IsInteractive() As Boolean
			Get
				Return (Me.Flags And ThumbButtonOptions.NonInteractive) = 0
			End Get
			Set
				If value <> m_isInteractive Then
					m_isInteractive = value

					If value Then
						Me.Flags = Me.Flags And Not (ThumbButtonOptions.NonInteractive)
					Else
						Me.Flags = Me.Flags Or ThumbButtonOptions.NonInteractive
					End If

					UpdateThumbnailButton()
				End If
			End Set
		End Property

		#End Region

		#Region "Internal Methods"

		''' <summary>
		''' Native flags enum (used when creating the native button)
		''' </summary>
		Friend Property Flags() As ThumbButtonOptions
			Get
				Return m_Flags
			End Get
			Set
				m_Flags = Value
			End Set
		End Property
		Private m_Flags As ThumbButtonOptions

		''' <summary>
		''' Native representation of the thumbnail button
		''' </summary>
		Friend ReadOnly Property Win32ThumbButton() As ThumbButton
			Get
				m_win32ThumbButton.Id = Id
				m_win32ThumbButton.Tip = Tooltip
				m_win32ThumbButton.Icon = If(Icon IsNot Nothing, Icon.Handle, IntPtr.Zero)
				m_win32ThumbButton.Flags = Flags

				m_win32ThumbButton.Mask = ThumbButtonMask.THB_FLAGS
				If Tooltip IsNot Nothing Then
					m_win32ThumbButton.Mask = m_win32ThumbButton.Mask Or ThumbButtonMask.Tooltip
				End If
				If Icon IsNot Nothing Then
					m_win32ThumbButton.Mask = m_win32ThumbButton.Mask Or ThumbButtonMask.Icon
				End If

				Return m_win32ThumbButton
			End Get
		End Property

		''' <summary>
		''' The window manager should call this method to raise the public click event to all
		''' the subscribers.
		''' </summary>
		''' <param name="taskbarWindow">Taskbar Window associated with this button</param>
		Friend Sub FireClick(taskbarWindow As TaskbarWindow)
            If _Click IsNot Nothing AndAlso taskbarWindow IsNot Nothing Then
                If taskbarWindow.UserWindowHandle <> IntPtr.Zero Then
                    RaiseEvent Click(Me, New ThumbnailButtonClickedEventArgs(taskbarWindow.UserWindowHandle, Me))
                ElseIf taskbarWindow.WindowsControl IsNot Nothing Then
                    RaiseEvent Click(Me, New ThumbnailButtonClickedEventArgs(taskbarWindow.WindowsControl, Me))
                End If
            End If
        End Sub

		''' <summary>
		''' Handle to the window to which this button is for (on the taskbar).
		''' </summary>
		Friend Property WindowHandle() As IntPtr
			Get
				Return m_WindowHandle
			End Get
			Set
				m_WindowHandle = Value
			End Set
		End Property
		Private m_WindowHandle As IntPtr

		''' <summary>
		''' Indicates if this button was added to the taskbar. If it's not yet added,
		''' then we can't do any updates on it.
		''' </summary>
		Friend Property AddedToTaskbar() As Boolean
			Get
				Return m_AddedToTaskbar
			End Get
			Set
				m_AddedToTaskbar = Value
			End Set
		End Property
		Private m_AddedToTaskbar As Boolean

		Friend Sub UpdateThumbnailButton()
			If internalUpdate OrElse Not AddedToTaskbar Then
				Return
			End If

			' Get the array of thumbnail buttons in native format
			Dim nativeButtons As ThumbButton() = {Win32ThumbButton}

			Dim hr As HResult = TaskbarList.Instance.ThumbBarUpdateButtons(WindowHandle, 1, nativeButtons)

			If Not CoreErrorHelper.Succeeded(hr) Then
				Throw New ShellException(hr)
			End If
		End Sub

		#End Region

		#Region "IDisposable Members"

		''' <summary>
		''' 
		''' </summary>
		Protected Overrides Sub Finalize()
			Try
				Dispose(False)
			Finally
				MyBase.Finalize()
			End Try
		End Sub

		''' <summary>
		''' Release the native objects.
		''' </summary>
		Public Sub Dispose() Implements IDisposable.Dispose
			Dispose(True)
			GC.SuppressFinalize(Me)
		End Sub

		''' <summary>
		''' Release the native objects.
		''' </summary>
		''' <param name="disposing"></param>
		Public Sub Dispose(disposing As Boolean)
			If disposing Then
				' Dispose managed resources
				Icon.Dispose()
				m_tooltip = Nothing
			End If
		End Sub

		#End Region
	End Class

End Namespace
