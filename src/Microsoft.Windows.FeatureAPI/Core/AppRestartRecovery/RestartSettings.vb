'Copyright (c) Microsoft Corporation.  All rights reserved.

Namespace ApplicationServices
	''' <summary>
	''' Specifies the options for an application to be automatically
	''' restarted by Windows Error Reporting. 
	''' </summary>
	''' <remarks>Regardless of these 
	''' settings, the application
	''' will not be restarted if it executed for less than 60 seconds before
	''' terminating.</remarks>
	Public Class RestartSettings
		Private m_command As String
		Private m_restrictions As RestartRestrictions

		''' <summary>
		''' Creates a new instance of the RestartSettings class.
		''' </summary>
		''' <param name="command">The command line arguments 
		''' used to restart the application.</param>
		''' <param name="restrictions">A bitwise combination of the RestartRestrictions 
		''' values that specify  
		''' when the application should not be restarted.
		''' </param>
		Public Sub New(command As String, restrictions As RestartRestrictions)
			Me.m_command = command
			Me.m_restrictions = restrictions
		End Sub

		''' <summary>
		''' Gets the command line arguments used to restart the application.
		''' </summary>
		''' <value>A <see cref="System.String"/> object.</value>
		Public ReadOnly Property Command() As String
			Get
				Return m_command
			End Get
		End Property

		''' <summary>
		''' Gets the set of conditions when the application 
		''' should not be restarted.
		''' </summary>
		''' <value>A set of <see cref="RestartRestrictions"/> values.</value>
		Public ReadOnly Property Restrictions() As RestartRestrictions
			Get
				Return m_restrictions
			End Get
		End Property

		''' <summary>
		''' Returns a string representation of the current state
		''' of this object.
		''' </summary>
		''' <returns>A <see cref="System.String"/> that displays 
		''' the command line arguments 
		''' and restrictions for restarting the application.</returns>
		Public Overrides Function ToString() As String
			Return String.Format(System.Globalization.CultureInfo.InvariantCulture, LocalizedMessages.RestartSettingsFormatString, m_command, m_restrictions.ToString())
		End Function
	End Class
End Namespace

