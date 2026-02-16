'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Collections
Imports System.Runtime.InteropServices
Imports System.Threading
Imports System.Windows.Forms

Namespace ApplicationServices

    ''' <summary>
    ''' This class generates .NET events based on Windows messages.  
    ''' The PowerRegWindow class processes the messages from Windows.
    ''' </summary>
    Friend NotInheritable Class MessageManager
        Private Sub New()
        End Sub
        Private Shared lockObject As New Object()
        Private Shared window As PowerRegWindow

#Region "Internal static methods"

        ''' <summary>
        ''' Registers a callback for a power event.
        ''' </summary>
        ''' <param name="eventId">Guid for the event.</param>
        ''' <param name="eventToRegister">Event handler for the specified event.</param>
        Friend Shared Sub RegisterPowerEvent(eventId As Guid, eventToRegister As EventHandler)
            EnsureInitialized()
            window.RegisterPowerEvent(eventId, eventToRegister)
        End Sub

        ''' <summary>
        ''' Unregisters an event handler for a power event.
        ''' </summary>
        ''' <param name="eventId">Guid for the event.</param>
        ''' <param name="eventToUnregister">Event handler to unregister.</param>
        Friend Shared Sub UnregisterPowerEvent(eventId As Guid, eventToUnregister As EventHandler)
            EnsureInitialized()
            window.UnregisterPowerEvent(eventId, eventToUnregister)
        End Sub

#End Region

        ''' <summary>
        ''' Ensures that the hidden window is initialized and 
        ''' listening for messages.
        ''' </summary>
        Private Shared Sub EnsureInitialized()
            SyncLock lockObject
                If window Is Nothing Then
                    ' Create a new hidden window to listen
                    ' for power management related window messages.
                    window = New PowerRegWindow()
                End If
            End SyncLock
        End Sub

        ''' <summary>
        ''' Catch Windows messages and generates events for power specific
        ''' messages.
        ''' </summary>
        Friend Class PowerRegWindow
            Inherits Form
            Private eventList As New Hashtable()
            Private readerWriterLock As New ReaderWriterLock()

            Friend Sub New()

                MyBase.New()
            End Sub

#Region "Internal Methods"

            ''' <summary>
            ''' Adds an event handler to call when Windows sends 
            ''' a message for an event.
            ''' </summary>
            ''' <param name="eventId">Guid for the event.</param>
            ''' <param name="eventToRegister">Event handler for the event.</param>
            Friend Sub RegisterPowerEvent(eventId As Guid, eventToRegister As EventHandler)
                readerWriterLock.AcquireWriterLock(Timeout.Infinite)
                If Not eventList.Contains(eventId) Then
                    Power.RegisterPowerSettingNotification(Me.Handle, eventId)
                    Dim newList As New ArrayList()
                    newList.Add(eventToRegister)
                    eventList.Add(eventId, newList)
                Else
                    Dim currList As ArrayList = DirectCast(eventList(eventId), ArrayList)
                    currList.Add(eventToRegister)
                End If
                readerWriterLock.ReleaseWriterLock()
            End Sub

            ''' <summary>
            ''' Removes an event handler.
            ''' </summary>
            ''' <param name="eventId">Guid for the event.</param>
            ''' <param name="eventToUnregister">Event handler to remove.</param>
            ''' <exception cref="InvalidOperationException">Cannot unregister 
            ''' a function that is not registered.</exception>
            Friend Sub UnregisterPowerEvent(eventId As Guid, eventToUnregister As EventHandler)
                readerWriterLock.AcquireWriterLock(Timeout.Infinite)
                If eventList.Contains(eventId) Then
                    Dim currList As ArrayList = DirectCast(eventList(eventId), ArrayList)
                    currList.Remove(eventToUnregister)
                Else
                    Throw New InvalidOperationException(GlobalLocalizedMessages.MessageManagerHandlerNotRegistered)
                End If
                readerWriterLock.ReleaseWriterLock()
            End Sub

#End Region

            ''' <summary>
            ''' Executes any registered event handlers.
            ''' </summary>
            ''' <param name="eventHandlerList">ArrayList of event handlers.</param>            
            Private Shared Sub ExecuteEvents(eventHandlerList As ArrayList)
                For Each handler As EventHandler In eventHandlerList
                    handler.Invoke(Nothing, New EventArgs())
                Next
            End Sub

            ''' <summary>
            ''' This method is called when a Windows message 
            ''' is sent to this window.
            ''' The method calls the registered event handlers.
            ''' </summary>
            Protected Overrides Sub WndProc(ByRef m As Message)
                ' Make sure it is a Power Management message.
                If m.Msg = PowerManagementNativeMethods.PowerBroadcastMessage AndAlso CInt(m.WParam) = PowerManagementNativeMethods.PowerSettingChangeMessage Then
                    Dim ps As PowerManagementNativeMethods.PowerBroadcastSetting = CType(Marshal.PtrToStructure(m.LParam, GetType(PowerManagementNativeMethods.PowerBroadcastSetting)), PowerManagementNativeMethods.PowerBroadcastSetting)

                    Dim pData As New IntPtr(m.LParam.ToInt64() + Marshal.SizeOf(ps))
                    Dim currentEvent As Guid = ps.PowerSetting

                    ' IsMonitorOn
                    If ps.PowerSetting = EventManager.MonitorPowerStatus AndAlso ps.DataLength = Marshal.SizeOf(GetType(Int32)) Then
                        Dim monitorStatus As Int32 = CType(Marshal.PtrToStructure(pData, GetType(Int32)), Int32)
                        PowerManager.IsMonitorOn = monitorStatus <> 0
                        EventManager.monitorOnReset.[Set]()
                    End If

                    If Not EventManager.IsMessageCaught(currentEvent) Then
                        ExecuteEvents(DirectCast(eventList(currentEvent), ArrayList))
                    End If
                Else
                    MyBase.WndProc(m)
                End If

            End Sub

        End Class
    End Class
End Namespace
