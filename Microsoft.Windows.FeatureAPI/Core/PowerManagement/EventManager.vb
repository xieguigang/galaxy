'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Threading

Namespace ApplicationServices

    ''' <summary>
    ''' This class keeps track of the current state of each type of event.  
    ''' The MessageManager class tracks event handlers.  
    ''' This class only deals with each event type (i.e.
    ''' BatteryLifePercentChanged) as a whole.
    ''' </summary>
    Module EventManager

        ' Prevents reading from PowerManager members while they are still null.
        ' MessageManager notifies the PowerManager that the member 
        ' has been set and can be used.        
        Friend monitorOnReset As New AutoResetEvent(False)

#Region "Hardcoded GUIDS for each event"

        Friend ReadOnly PowerPersonalityChange As New Guid(&H245D8541, &H3943, &H4422, &HB0, &H25, &H13,
            &HA7, &H84, &HF6, &H79, &HB7)
        Friend ReadOnly PowerSourceChange As New Guid(&H5D3E9A59, &HE9D5, &H4B00, &HA6, &HBD, &HFF,
            &H34, &HFF, &H51, &H65, &H48)
        Friend ReadOnly BatteryCapacityChange As New Guid(&HA7AD8041UI, &HB45A, &H4CAE, &H87, &HA3, &HEE,
            &HCB, &HB4, &H68, &HA9, &HE1)
        Friend ReadOnly BackgroundTaskNotification As New Guid(&H515C31D8, &HF734, &H163D, &HA0, &HFD, &H11,
            &HA0, &H8C, &H91, &HE8, &HF1)
        Friend ReadOnly MonitorPowerStatus As New Guid(&H2731015, &H4510, &H4526, &H99, &HE6, &HE5,
            &HA1, &H7E, &HBD, &H1A, &HEA)

#End Region

#Region "private static members"

        ' Used to catch the initial message Windows sends when 
        ' you first register for a power notification.
        ' We do not want to fire any event handlers when this happens.
        Private personalityCaught As Boolean
        Private powerSrcCaught As Boolean
        Private batteryLifeCaught As Boolean
        Private monitorOnCaught As Boolean

#End Region

        ''' <summary>
        ''' Determines if a message should be caught, preventing
        ''' the event handler from executing. 
        ''' This is needed when an event is initially registered.
        ''' </summary>
        ''' <param name="eventGuid">The event to check.</param>
        ''' <returns>A boolean value. Returns true if the 
        ''' message should be caught.</returns>
        Friend Function IsMessageCaught(eventGuid As Guid) As Boolean
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
    End Module
End Namespace
