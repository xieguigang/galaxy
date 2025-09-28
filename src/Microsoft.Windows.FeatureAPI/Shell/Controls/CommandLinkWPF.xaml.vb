'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.ComponentModel
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Input
Imports System.Windows.Media
Imports Microsoft.Windows.Internal

Namespace Controls.WindowsPresentationFoundation

    Public Partial Class CommandLink
		Inherits UserControl
		Implements INotifyPropertyChanged
		''' <summary>
		''' Creates a new instance of this class.
		''' </summary>
		Public Sub New()
			' Throw PlatformNotSupportedException if the user is not running Vista or beyond
			CoreHelpers.ThrowIfNotVista()

			Me.DataContext = Me
			InitializeComponent()
			AddHandler Me.button.Click, New RoutedEventHandler(AddressOf button_Click)
		End Sub

		Private Sub button_Click(sender As Object, e As RoutedEventArgs)
			e.Source = Me
			RaiseEvent Click(sender, e)
		End Sub

		''' <summary>
		''' Routed UI command to use for this button
		''' </summary>
		Public Property Command() As RoutedUICommand
			Get
				Return m_Command
			End Get
			Set
				m_Command = Value
			End Set
		End Property
		Private m_Command As RoutedUICommand

		''' <summary>
		''' Occurs when the control is clicked.
		''' </summary>
		Public Event Click As RoutedEventHandler

		Private m_link As String

		''' <summary>
		''' Specifies the main instruction text
		''' </summary>
		Public Property Link() As String
			Get
				Return m_link
			End Get
			Set
				m_link = value

				RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Link"))
			End Set
		End Property
		Private m_note As String

		''' <summary>
		''' Specifies the supporting note text
		''' </summary>
		Public Property Note() As String
			Get
				Return m_note
			End Get
			Set
				m_note = value
				RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Note"))
			End Set
		End Property
		Private m_icon As ImageSource

		''' <summary>
		''' Icon to set for the command link button
		''' </summary>
		Public Property Icon() As ImageSource
			Get
				Return m_icon
			End Get
			Set
				m_icon = value
				RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Icon"))
			End Set
		End Property

		''' <summary>
		''' Indicates if the button is in a checked state
		''' </summary>
		Public Property IsCheck() As System.Nullable(Of Boolean)
			Get
				Return button.IsChecked
			End Get
			Set
				button.IsChecked = value
			End Set
		End Property


#Region "INotifyPropertyChanged Members"

        ''' <summary>
        ''' Occurs when a property value changes.
        ''' </summary>
        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

#End Region

        ''' <summary>
        ''' Indicates whether this feature is supported on the current platform.
        ''' </summary>
        Public Shared ReadOnly Property IsPlatformSupported() As Boolean
			Get
				Return CoreHelpers.RunningOnVista
			End Get
		End Property
	End Class
End Namespace
