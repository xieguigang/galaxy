'Copyright (c) Microsoft Corporation.  All rights reserved.

Namespace Taskbar

	Friend Enum TaskbarProxyWindowType
		TabbedThumbnail
		ThumbnailToolbar
	End Enum

	''' <summary>
	''' Known category to display
	''' </summary>
	Public Enum JumpListKnownCategoryType
		''' <summary>
		''' Don't display either known category. You must have at least one
		''' user task or custom category link in order to not see the
		''' default 'Recent' known category
		''' </summary>
		Neither = 0

		''' <summary>
		''' Display the 'Recent' known category
		''' </summary>
		Recent

		''' <summary>
		''' Display the 'Frequent' known category
		''' </summary>
		Frequent
	End Enum

	''' <summary>
	''' Represents the thumbnail progress bar state.
	''' </summary>
	Public Enum TaskbarProgressBarState
		''' <summary>
		''' No progress is displayed.
		''' </summary>
		NoProgress = 0

		''' <summary>
		''' The progress is indeterminate (marquee).
		''' </summary>
		Indeterminate = &H1

		''' <summary>
		''' Normal progress is displayed.
		''' </summary>
		Normal = &H2

		''' <summary>
		''' An error occurred (red).
		''' </summary>
		[Error] = &H4

		''' <summary>
		''' The operation is paused (yellow).
		''' </summary>
		Paused = &H8
	End Enum
End Namespace
