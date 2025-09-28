'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports Microsoft.Windows.Resources

Namespace ApplicationServices
	''' <summary>
	''' Defines methods and properties for recovery settings, and specifies options for an application that attempts
	''' to perform final actions after a fatal event, such as an
	''' unhandled exception.
	''' </summary>
	''' <remarks>This class is used to register for application recovery.
	''' See the <see cref="ApplicationRestartRecoveryManager"/> class.
	''' </remarks>
	Public Class RecoverySettings
		Private m_recoveryData As RecoveryData
		Private m_pingInterval As UInteger

		''' <summary>
		''' Initializes a new instance of the <b>RecoverySettings</b> class.
		''' </summary>
		''' <param name="data">A recovery data object that contains the callback method (invoked by the system
		''' before Windows Error Reporting terminates the application) and an optional state object.</param>
		''' <param name="interval">The time interval within which the 
		''' callback method must invoke <see cref="ApplicationRestartRecoveryManager.ApplicationRecoveryInProgress"/> to 
		''' prevent WER from terminating the application.</param>
		''' <seealso cref="ApplicationRestartRecoveryManager"/>
		Public Sub New(data As RecoveryData, interval As UInteger)
			Me.m_recoveryData = data
			Me.m_pingInterval = interval
		End Sub

		''' <summary>
		''' Gets the recovery data object that contains the callback method and an optional
		''' parameter (usually the state of the application) to be passed to the 
		''' callback method.
		''' </summary>
		''' <value>A <see cref="RecoveryData"/> object.</value>
		Public ReadOnly Property RecoveryData() As RecoveryData
			Get
				Return m_recoveryData
			End Get
		End Property

		''' <summary>
		''' Gets the time interval for notifying Windows Error Reporting.  
		''' The <see cref="RecoveryCallback"/> method must invoke <see cref="ApplicationRestartRecoveryManager.ApplicationRecoveryInProgress"/> 
		''' within this interval to prevent WER from terminating the application.
		''' </summary>
		''' <remarks>        
		''' The recovery ping interval is specified in milliseconds. 
		''' By default, the interval is 5 seconds. 
		''' If you specify zero, the default interval is used. 
		''' </remarks>
		Public ReadOnly Property PingInterval() As UInteger
			Get
				Return m_pingInterval
			End Get
		End Property

		''' <summary>
		''' Returns a string representation of the current state
		''' of this object.
		''' </summary>
		''' <returns>A <see cref="System.String"/> object.</returns>
		Public Overrides Function ToString() As String
			Return String.Format(System.Globalization.CultureInfo.InvariantCulture, LocalizedMessages.RecoverySettingsFormatString, Me.m_recoveryData.Callback.Method.ToString(), Me.m_recoveryData.State.ToString(), Me.PingInterval)
		End Function
	End Class
End Namespace

