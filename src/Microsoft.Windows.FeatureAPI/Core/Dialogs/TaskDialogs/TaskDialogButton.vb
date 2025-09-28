'Copyright (c) Microsoft Corporation.  All rights reserved.

Namespace Dialogs
	''' <summary>
	''' Implements a button that can be hosted in a task dialog.
	''' </summary>
	Public Class TaskDialogButton
		Inherits TaskDialogButtonBase
		''' <summary>
		''' Creates a new instance of this class.
		''' </summary>
		Public Sub New()
		End Sub

		''' <summary>
		''' Creates a new instance of this class with the specified property settings.
		''' </summary>
		''' <param name="name">The name of the button.</param>
		''' <param name="text">The button label.</param>
		Public Sub New(name As String, text As String)
			MyBase.New(name, text)
		End Sub

		Private m_useElevationIcon As Boolean
		''' <summary>
		''' Gets or sets a value that controls whether the elevation icon is displayed.
		''' </summary>
		Public Property UseElevationIcon() As Boolean
			Get
				Return m_useElevationIcon
			End Get
			Set
				CheckPropertyChangeAllowed("ShowElevationIcon")
				m_useElevationIcon = value
				ApplyPropertyChange("ShowElevationIcon")
			End Set
		End Property
	End Class
End Namespace
