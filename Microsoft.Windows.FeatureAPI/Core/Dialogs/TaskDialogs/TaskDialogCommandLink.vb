'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Globalization

Namespace Dialogs
	''' <summary>
	''' Represents a command-link. 
	''' </summary>
	Public Class TaskDialogCommandLink
		Inherits TaskDialogButton
		''' <summary>
		''' Creates a new instance of this class.
		''' </summary>
		Public Sub New()
		End Sub

		''' <summary>
		''' Creates a new instance of this class with the specified name and label.
		''' </summary>
		''' <param name="name">The name for this button.</param>
		''' <param name="text">The label for this button.</param>
		Public Sub New(name As String, text As String)
			MyBase.New(name, text)
		End Sub

		''' <summary>
		''' Creates a new instance of this class with the specified name,label, and instruction.
		''' </summary>
		''' <param name="name">The name for this button.</param>
		''' <param name="text">The label for this button.</param>
		''' <param name="instruction">The instruction for this command link.</param>
		Public Sub New(name As String, text As String, instruction As String)
			MyBase.New(name, text)
			Me.m_instruction = instruction
		End Sub

		Private m_instruction As String
		''' <summary>
		''' Gets or sets the instruction associated with this command link button.
		''' </summary>
		Public Property Instruction() As String
			Get
				Return m_instruction
			End Get
			Set
				m_instruction = value
			End Set
		End Property

		''' <summary>
		''' Returns a string representation of this object.
		''' </summary>
		''' <returns>A <see cref="System.String"/></returns>
		Public Overrides Function ToString() As String
			Return String.Format(CultureInfo.CurrentCulture, "{0}{1}{2}", If(Text, String.Empty), If((Not String.IsNullOrEmpty(Text) AndAlso Not String.IsNullOrEmpty(m_instruction)), Environment.NewLine, String.Empty), If(m_instruction, String.Empty))
		End Function
	End Class
End Namespace
