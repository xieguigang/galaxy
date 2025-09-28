' Copyright (c) Microsoft Corporation.  All rights reserved.


Namespace Sensors
	''' <summary>
	''' Contains a list of well known event interest types. This class will be removed once wrappers are developed.
	''' </summary>
	Public NotInheritable Class EventInterestTypes
		Private Sub New()
		End Sub
		''' <summary>
		''' Register for asynchronous sensor data updates. This has power management implications.
		''' </summary>
		Public Shared ReadOnly DataUpdated As New Guid(&H2ed0f2a4, &H87, &H41d3, &H87, &Hdb, &H67, _
			&H73, &H37, &Hb, &H3c, &H88)

		''' <summary>
		''' Register for sensor state change notifications.
		''' </summary>
		Public Shared ReadOnly StateChanged As New Guid(&Hbfd96016UI, &H6bd7, &H4560, &Had, &H34, &Hf2, _
			&Hf6, &H60, &H7e, &H8f, &H81)
	End Class
End Namespace
