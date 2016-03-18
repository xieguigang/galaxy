'Copyright (c) Microsoft Corporation.  All rights reserved.

Namespace ApplicationServices
	''' <summary>
	''' The <see cref="System.Delegate"/> that represents the callback method invoked
	''' by the system when an application has registered for 
	''' application recovery. 
	''' </summary>
	''' <param name="state">An application-defined state object that is passed to the callback method.</param>
	''' <remarks>The callback method will be invoked
	''' prior to the application being terminated by Windows Error Reporting (WER). To keep WER from terminating the application before 
	''' the callback method completes, the callback method must
	''' periodically call the <see cref="ApplicationRestartRecoveryManager.ApplicationRecoveryInProgress"/> method. </remarks>
	''' <seealso cref="ApplicationRestartRecoveryManager.RegisterForApplicationRecovery(RecoverySettings)"/>
	Public Delegate Function RecoveryCallback(state As Object) As Integer

	''' <summary>
	''' Defines a class that contains a callback delegate and properties of the application
	''' as defined by the user.
	''' </summary>
	Public Class RecoveryData
        ''' <summary>
        ''' Initializes a recovery data wrapper with a callback method and the current
        ''' state of the application.
        ''' </summary>
        ''' <param name="callback__1">The callback delegate.</param>
        ''' <param name="state__2">The current state of the application.</param>
        Public Sub New(callback__1 As RecoveryCallback, state__2 As Object)
			Callback = callback__1
			State = state__2
		End Sub

		''' <summary>
		''' Gets or sets a value that determines the recovery callback function.
		''' </summary>
		Public Property Callback() As RecoveryCallback
			Get
				Return m_Callback
			End Get
			Set
				m_Callback = Value
			End Set
		End Property
		Private m_Callback As RecoveryCallback

		''' <summary>
		''' Gets or sets a value that determines the application state.
		''' </summary>
		Public Property State() As Object
			Get
				Return m_State
			End Get
			Set
				m_State = Value
			End Set
		End Property
		Private m_State As Object

		''' <summary>
		''' Invokes the recovery callback function.
		''' </summary>
		Public Sub Invoke()
            Call m_Callback(State)
        End Sub
	End Class
End Namespace
