'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Collections.ObjectModel
Imports System.Diagnostics
Imports System.Windows.Markup
Imports Microsoft.Windows.Resources

Namespace Dialogs.Controls
	''' <summary>
	''' Creates the ComboBox controls in the Common File Dialog.
	''' </summary>
	<ContentProperty("Items")> _
	Public Class CommonFileDialogComboBox
		Inherits CommonFileDialogProminentControl
		Implements ICommonFileDialogIndexedControls
		Private ReadOnly m_items As New Collection(Of CommonFileDialogComboBoxItem)()
		''' <summary>
		''' Gets the collection of CommonFileDialogComboBoxItem objects.
		''' </summary>
		Public ReadOnly Property Items() As Collection(Of CommonFileDialogComboBoxItem)
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
		''' <param name="name">Text to display for this control</param>
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
				' Don't update property if it hasn't changed
				If m_selectedIndex = value Then
					Return
				End If

				If HostingDialog Is Nothing Then
					m_selectedIndex = value
					Return
				End If

				' Only update this property if it has a valid value
				If value >= 0 AndAlso value < m_items.Count Then
					m_selectedIndex = value
					ApplyPropertyChange("SelectedIndex")
				Else
					Throw New IndexOutOfRangeException(LocalizedMessages.ComboBoxIndexOutsideBounds)
				End If
			End Set
		End Property

        ''' <summary>
        ''' Occurs when the SelectedIndex is changed.
        ''' </summary>
        ''' 
        ''' <remarks>
        ''' By initializing the SelectedIndexChanged event with an empty
        ''' delegate, it is not necessary to check  
        ''' if the SelectedIndexChanged is not null.
        ''' 
        ''' </remarks>
        Public Event SelectedIndexChanged As EventHandler Implements ICommonFileDialogIndexedControls.SelectedIndexChanged

        ''' <summary>
        ''' Raises the SelectedIndexChanged event if this control is 
        ''' enabled.
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
		''' Attach the ComboBox control to the dialog object
		''' </summary>
		''' <param name="dialog">The target dialog</param>
		<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")> _
		Friend Overrides Sub Attach(dialog As IFileDialogCustomize)
			Debug.Assert(dialog IsNot Nothing, "CommonFileDialogComboBox.Attach: dialog parameter can not be null")

			' Add the combo box control
			dialog.AddComboBox(Me.Id)

			' Add the combo box items
			For index As Integer = 0 To m_items.Count - 1
				dialog.AddControlItem(Me.Id, index, m_items(index).Text)
			Next

			' Set the currently selected item
			If m_selectedIndex >= 0 AndAlso m_selectedIndex < m_items.Count Then
				dialog.SetSelectedControlItem(Me.Id, Me.m_selectedIndex)
			ElseIf m_selectedIndex <> -1 Then
				Throw New IndexOutOfRangeException(LocalizedMessages.ComboBoxIndexOutsideBounds)
			End If

			' Make this control prominent if needed
			If IsProminent Then
				dialog.MakeProminent(Me.Id)
			End If

			' Sync additional properties
			SyncUnmanagedProperties()
		End Sub

	End Class

	''' <summary>
	''' Creates a ComboBoxItem for the Common File Dialog.
	''' </summary>
	Public Class CommonFileDialogComboBoxItem
		Private m_text As String = String.Empty
		''' <summary>
		''' Gets or sets the string that is displayed for this item.
		''' </summary>
		Public Property Text() As String
			Get
				Return m_text
			End Get
			Set
				m_text = value
			End Set
		End Property

		''' <summary>
		''' Creates a new instance of this class.
		''' </summary>
		Public Sub New()
		End Sub

		''' <summary>
		''' Creates a new instance of this class with the specified text.
		''' </summary>
		''' <param name="text">The text to use for the combo box item.</param>
		Public Sub New(text As String)
			Me.m_text = text
		End Sub
	End Class
End Namespace
