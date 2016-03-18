'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Threading

Namespace ApplicationServices
	''' <summary>
	''' This class keeps track of the current state of each type of event.  
	''' The MessageManager class tracks event handlers.  
	''' This class only deals with each event type (i.e.
	''' BatteryLifePercentChanged) as a whole.
	''' </summary>
	Friend NotInheritable Class EventManager
		Private Sub New()
		End Sub
		' Prevents reading from PowerManager members while they are still null.
		' MessageManager notifies the PowerManager that the member 
		' has been set and can be used.        
		Friend Shared monitorOnReset As New AutoResetEvent(False)

		#Region "Hardcoded GUIDS for each event"

		Friend Shared ReadOnly PowerPersonalityChange As New Guid(&H245d8541, &H3943, &H4422, &Hb0, &H25, &H13, _
			&Ha7, &H84, &Hf6, &H79, &Hb7)
		Friend Shared ReadOnly PowerSourceChange As New Guid(&H5d3e9a59, &He9d5, &H4b00, &Ha6, &Hbd, &Hff, _
			&H34, &Hff, &H51, &H65, &H48)
		Friend Shared ReadOnly BatteryCapacityChange As New Guid(&Ha7ad8041UI, &Hb45a, &H4cae, &H87, &Ha3, &Hee, _
			&Hcb, &Hb4, &H68, &Ha9, &He1)
		Friend Shared ReadOnly BackgroundTaskNotification As New Guid(&H515c31d8, &Hf734, &H163d, &Ha0, &Hfd, &H11, _
			&Ha0, &H8c, &H91, &He8, &Hf1)
		Friend Shared ReadOnly MonitorPowerStatus As New Guid(&H2731015, &H4510, &H4526, &H99, &He6, &He5, _
			&Ha1, &H7e, &Hbd, &H1a, &Hea)

		#End Region

		#Region "private static members"

		' Used to catch the initial message Windows sends when 
		' you first register for a power notification.
		' We do not want to fire any event handlers when this happens.
		Private Shared personalityCaught As Boolean
		Private Shared powerSrcCaught As Boolean
		Private Shared batteryLifeCaught As Boolean
		Private Shared monitorOnCaught As Boolean

		#End Region

		''' <summary>
		''' Determines if a message should be caught, preventing
		''' the event handler from executing. 
		''' This is needed when an event is initially registered.
		''' </summary>
		''' <param name="eventGuid">The event to check.</param>
		''' <returns>A boolean value. Returns true if the 
		''' message should be caught.</returns>
		Friend Shared Function IsMessageCaught(eventGuid As Guid) As Boolean
			Dim isMessageCaught__1 As Boolean = False

			If eventGuid = EventManager.BatteryCapacityChange Then
				If Not batteryLifeCaught Then
					batteryLifeCaught = True
					isMessageCaught__1 = True
				End If
			ElseIf eventGuid = EventManager.MonitorPowerStatus Then
				If Not monitorOnCaught Then
					monitorOnCaught = True
					isMessageCaught__1 = True
				End If
			ElseIf eventGuid = EventManager.PowerPersonalityChange Then
				If Not personalityCaught Then
					personalityCaught = True
					isMessageCaught__1 = True
				End If
			ElseIf eventGuid = EventManager.PowerSourceChange Then
				If Not powerSrcCaught Then
					powerSrcCaught = True
					isMessageCaught__1 = True
				End If
			End If

			Return isMessageCaught__1
		End Function
	End Class
End Namespace
