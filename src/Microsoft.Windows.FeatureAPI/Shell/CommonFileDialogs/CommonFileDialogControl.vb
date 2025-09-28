'Copyright (c) Microsoft Corporation.  All rights reserved.

Namespace Dialogs.Controls
	''' <summary>
	''' Defines an abstract class that supports shared functionality for the 
	''' common file dialog controls.
	''' </summary>
	Public MustInherit Class CommonFileDialogControl
		Inherits DialogControl
		''' <summary>
		''' Holds the text that is displayed for this control.
		''' </summary>
		Private textValue As String

		''' <summary>
		''' Gets or sets the text string that is displayed on the control.
		''' </summary>
		Public Overridable Property Text() As String
			Get
				Return textValue
			End Get
			Set
				' Don't update this property if it hasn't changed
				If value <> textValue Then
					textValue = value
					ApplyPropertyChange("Text")
				End If
			End Set
		End Property

		Private m_enabled As Boolean = True
		''' <summary>
		''' Gets or sets a value that determines if this control is enabled.  
		''' </summary>
		Public Property Enabled() As Boolean
			Get
				Return m_enabled
			End Get
			Set
				' Don't update this property if it hasn't changed
				If value = m_enabled Then
					Return
				End If

				m_enabled = value
				ApplyPropertyChange("Enabled")
			End Set
		End Property

		Private m_visible As Boolean = True
		''' <summary>
		''' Gets or sets a boolean value that indicates whether  
		''' this control is visible.
		''' </summary>
		Public Property Visible() As Boolean
			Get
				Return m_visible
			End Get
			Set
				' Don't update this property if it hasn't changed
				If value = m_visible Then
					Return
				End If

				m_visible = value
				ApplyPropertyChange("Visible")
			End Set
		End Property

		Private m_isAdded As Boolean
		''' <summary>
		''' Has this control been added to the dialog
		''' </summary>
		Friend Property IsAdded() As Boolean
			Get
				Return m_isAdded
			End Get
			Set
				m_isAdded = value
			End Set
		End Property

		''' <summary>
		''' Creates a new instance of this class.
		''' </summary>
		Protected Sub New()
		End Sub

		''' <summary>
		''' Creates a new instance of this class with the text.
		''' </summary>
		''' <param name="text">The text of the common file dialog control.</param>
		Protected Sub New(text As String)
			MyBase.New()
			Me.textValue = text
		End Sub

		''' <summary>
		''' Creates a new instance of this class with the specified name and text.
		''' </summary>
		''' <param name="name">The name of the common file dialog control.</param>
		''' <param name="text">The text of the common file dialog control.</param>
		Protected Sub New(name As String, text As String)
			MyBase.New(name)
			Me.textValue = text
		End Sub

		''' <summary>
		''' Attach the custom control itself to the specified dialog
		''' </summary>
		''' <param name="dialog">the target dialog</param>
		Friend MustOverride Sub Attach(dialog As IFileDialogCustomize)

		Friend Overridable Sub SyncUnmanagedProperties()
			ApplyPropertyChange("Enabled")
			ApplyPropertyChange("Visible")
		End Sub
	End Class
End Namespace
