'Copyright (c) Microsoft Corporation.  All rights reserved.


Namespace Dialogs
	' ContentProperty allows us to specify the text 
	' of the button as the child text of
	' a button element in XAML, as well as explicitly 
	' set with 'Text="<text>"'
	' Note that this attribute is inherited, so it 
	' applies to command-links and radio buttons as well.
	''' <summary>
	''' Defines the abstract base class for task dialog buttons. 
	''' Classes that inherit from this class will inherit 
	''' the Text property defined in this class.
	''' </summary>
	Public MustInherit Class TaskDialogButtonBase
		Inherits TaskDialogControl

		''' <summary>
		''' Creates a new instance on a task dialog button.
		''' </summary>
		Protected Sub New()
		End Sub
		''' <summary>
		''' Creates a new instance on a task dialog button with
		''' the specified name and text.
		''' </summary>
		''' <param name="name">The name for this button.</param>
		''' <param name="text">The label for this button.</param>
		Protected Sub New(name As String, text As String)
			MyBase.New(name)
			Me.m_text = text
		End Sub

		' Note that we don't need to explicitly 
		' implement the add/remove delegate for the Click event;
		' the hosting dialog only needs the delegate 
		' information when the Click event is 
		' raised (indirectly) by NativeTaskDialog, 
		' so the latest delegate is always available.
		''' <summary>
		''' Raised when the task dialog button is clicked.
		''' </summary>
		Public Event Click As EventHandler

		Friend Sub RaiseClickEvent()
			' Only perform click if the button is enabled.
			If Not m_enabled Then
				Return
			End If

			RaiseEvent Click(Me, EventArgs.Empty)
		End Sub

		Private m_text As String
		''' <summary>
		''' Gets or sets the button text.
		''' </summary>
		Public Property Text() As String
			Get
				Return m_text
			End Get
			Set
				CheckPropertyChangeAllowed("Text")
				m_text = value
				ApplyPropertyChange("Text")
			End Set
		End Property

		Private m_enabled As Boolean = True
		''' <summary>
		''' Gets or sets a value that determines whether the
		''' button is enabled. The enabled state can cannot be changed
		''' before the dialog is shown.
		''' </summary>
		Public Property Enabled() As Boolean
			Get
				Return m_enabled
			End Get
			Set
				CheckPropertyChangeAllowed("Enabled")
				m_enabled = value
				ApplyPropertyChange("Enabled")
			End Set
		End Property

		Private defaultControl As Boolean
		''' <summary>
		''' Gets or sets a value that indicates whether
		''' this button is the default button.
		''' </summary>
		Public Property [Default]() As Boolean
			Get
				Return defaultControl
			End Get
			Set
				CheckPropertyChangeAllowed("Default")
				defaultControl = value
				ApplyPropertyChange("Default")
			End Set
		End Property

		''' <summary>
		''' Returns the Text property value for this button.
		''' </summary>
		''' <returns>A <see cref="System.String"/>.</returns>
		Public Overrides Function ToString() As String
			Return If(m_text, String.Empty)
		End Function
	End Class
End Namespace
