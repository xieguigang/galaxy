'Copyright (c) Microsoft Corporation.  All rights reserved.


Namespace Dialogs
	''' <summary>
	''' Defines event data associated with a HyperlinkClick event.
	''' </summary>
	Public Class TaskDialogHyperlinkClickedEventArgs
		Inherits EventArgs
        ''' <summary>
        ''' Creates a new instance of this class with the specified link text.
        ''' </summary>
        ''' <param name="linkText__1">The text of the hyperlink that was clicked.</param>
        Public Sub New(linkText__1 As String)
			LinkText = linkText__1
		End Sub

		''' <summary>
		''' Gets or sets the text of the hyperlink that was clicked.
		''' </summary>
		Public Property LinkText() As String
			Get
				Return m_LinkText
			End Get
			Set
				m_LinkText = Value
			End Set
		End Property
		Private m_LinkText As String
	End Class
End Namespace
