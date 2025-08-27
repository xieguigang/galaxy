'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Collections.ObjectModel
Imports System.Diagnostics
Imports System.Windows.Markup
Imports Microsoft.Windows.Resources

Namespace Dialogs.Controls
	''' <summary>
	''' Represents a radio button list for the Common File Dialog.
	''' </summary>
	<ContentProperty("Items")> _
	Public Class CommonFileDialogRadioButtonList
		Inherits CommonFileDialogControl
		Implements ICommonFileDialogIndexedControls
		Private m_items As New Collection(Of CommonFileDialogRadioButtonListItem)()
		''' <summary>
		''' Gets the collection of CommonFileDialogRadioButtonListItem objects
		''' </summary>
		Public ReadOnly Property Items() As Collection(Of CommonFileDialogRadioButtonListItem)
			Get
				Return m_items
			End Get
		End Property

		''' <summary>
		''' Creates a new instance of this class.
		''' </summary>
		Public Sub New()
		End Sub

		''' <summary>
		''' Creates a new instance of this class with the specified name.
		''' </summary>
		''' <param name="name">The name of this control.</param>
		Public Sub New(name As String)
			MyBase.New(name, String.Empty)
		End Sub

		#Region "ICommonFileDialogIndexedControls Members"

		Private m_selectedIndex As Integer = -1
		''' <summary>
		''' Gets or sets the current index of the selected item.
		''' </summary>
		<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")> _
		Public Property SelectedIndex() As Integer Implements ICommonFileDialogIndexedControls.SelectedIndex
			Get
				Return m_selectedIndex
			End Get
			Set
				' Don't update this property if it hasn't changed
				If m_selectedIndex = value Then
					Return
				End If

				' If the native dialog has not been created yet
				If HostingDialog Is Nothing Then
					m_selectedIndex = value
				ElseIf value >= 0 AndAlso value < m_items.Count Then
					m_selectedIndex = value
					ApplyPropertyChange("SelectedIndex")
				Else
					Throw New IndexOutOfRangeException(LocalizedMessages.RadioButtonListIndexOutOfBounds)
				End If
			End Set
		End Property

        ''' <summary>
        ''' Occurs when the user changes the SelectedIndex.
        ''' </summary>
        ''' 
        ''' <remarks>
        ''' By initializing the SelectedIndexChanged event with an empty
        ''' delegate, we can skip the test to determine
        ''' if the SelectedIndexChanged is null.
        ''' test.
        ''' </remarks>
        Public Event SelectedIndexChanged As EventHandler Implements ICommonFileDialogIndexedControls.SelectedIndexChanged

        ''' <summary>
        ''' Occurs when the user changes the SelectedIndex.
        ''' </summary>
        ''' <remarks>Because this method is defined in an interface, we can either
        ''' have it as public, or make it private and explicitly implement (like below).
        ''' Making it public doesn't really help as its only internal (but can't have this 
        ''' internal because of the interface)
        ''' </remarks>
        Private Sub ICommonFileDialogIndexedControls_RaiseSelectedIndexChangedEvent() Implements ICommonFileDialogIndexedControls.RaiseSelectedIndexChangedEvent
			' Make sure that this control is enabled and has a specified delegate
			If Enabled Then
				RaiseEvent SelectedIndexChanged(Me, EventArgs.Empty)
			End If
		End Sub

		#End Region

		''' <summary>
		''' Attach the RadioButtonList control to the dialog object
		''' </summary>
		''' <param name="dialog">The target dialog</param>
		Friend Overrides Sub Attach(dialog As IFileDialogCustomize)
			System.Diagnostics.Debug.Assert(dialog IsNot Nothing, "CommonFileDialogRadioButtonList.Attach: dialog parameter can not be null")

			' Add the radio button list control
			dialog.AddRadioButtonList(Me.Id)

			' Add the radio button list items
			For index As Integer = 0 To m_items.Count - 1
				dialog.AddControlItem(Me.Id, index, m_items(index).Text)
			Next

			' Set the currently selected item
			If m_selectedIndex >= 0 AndAlso m_selectedIndex < m_items.Count Then
				dialog.SetSelectedControlItem(Me.Id, Me.m_selectedIndex)
			ElseIf m_selectedIndex <> -1 Then
				Throw New IndexOutOfRangeException(LocalizedMessages.RadioButtonListIndexOutOfBounds)
			End If

			' Sync unmanaged properties with managed properties
			SyncUnmanagedProperties()
		End Sub
	End Class

	''' <summary>
	''' Represents a list item for the CommonFileDialogRadioButtonList object.
	''' </summary>
	Public Class CommonFileDialogRadioButtonListItem
		''' <summary>
		''' Gets or sets the string that will be displayed for this list item.
		''' </summary>
		Public Property Text() As String
			Get
				Return m_Text
			End Get
			Set
				m_Text = Value
			End Set
		End Property
		Private m_Text As String

		''' <summary>
		''' Creates a new instance of this class.
		''' </summary>
		Public Sub New()
			Me.New(String.Empty)
		End Sub

        ''' <summary>
        ''' Creates a new instance of this class with the specified text.
        ''' </summary>
        ''' <param name="text__1">The string that you want to display for this list item.</param>
        Public Sub New(text__1 As String)
			Text = text__1
		End Sub
	End Class
End Namespace
