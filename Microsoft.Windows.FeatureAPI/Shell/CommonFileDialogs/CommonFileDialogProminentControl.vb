'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Windows.Markup

Namespace Dialogs.Controls
	''' <summary>
	''' Defines the properties and constructors for all prominent controls in the Common File Dialog.
	''' </summary>
	<ContentProperty("Items")> _
	Public MustInherit Class CommonFileDialogProminentControl
		Inherits CommonFileDialogControl
		Private m_isProminent As Boolean

		''' <summary>
		''' Gets or sets the prominent value of this control. 
		''' </summary>
		''' <remarks>Only one control can be specified as prominent. If more than one control is specified prominent, 
		''' then an 'E_UNEXPECTED' exception will be thrown when these controls are added to the dialog. 
		''' A group box control can only be specified as prominent if it contains one control and that control is of type 'CommonFileDialogProminentControl'.
		''' </remarks>
		Public Property IsProminent() As Boolean
			Get
				Return m_isProminent
			End Get
			Set
				m_isProminent = value
			End Set
		End Property


		''' <summary>
		''' Creates a new instance of this class.
		''' </summary>
		Protected Sub New()
		End Sub

		''' <summary>
		''' Creates a new instance of this class with the specified text.
		''' </summary>
		''' <param name="text">The text to display for this control.</param>
		Protected Sub New(text As String)
			MyBase.New(text)
		End Sub

		''' <summary>
		''' Creates a new instance of this class with the specified name and text.
		''' </summary>
		''' <param name="name">The name of this control.</param>
		''' <param name="text">The text to display for this control.</param>
		Protected Sub New(name As String, text As String)
			MyBase.New(name, text)
		End Sub
	End Class
End Namespace
