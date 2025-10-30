'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.ComponentModel
Imports Microsoft.Windows.Internal

Namespace ApplicationServices

    ''' <summary>
    ''' Enables registration for 
    ''' power-related event notifications and provides access to power settings.
    ''' </summary>
    Public NotInheritable Class PowerManager
        Private Sub New()
        End Sub

        Private Shared m_isMonitorOn As System.Nullable(Of Boolean)
        Private Shared m_monitorRequired As Boolean
        Private Shared m_requestBlockSleep As Boolean

        Private Shared ReadOnly monitoronlock As New Object()


#Region "Notifications"

        ''' <summary>
        ''' Raised each time the active power scheme changes.
        ''' </summary>
        ''' <exception cref="InvalidOperationException">The event handler specified for removal was not registered.</exception>
        ''' <exception cref="System.PlatformNotSupportedException">Requires Vista/Windows Server 2008.</exception>
        Public Shared Custom Event PowerPersonalityChanged As EventHandler
            AddHandler(value As EventHandler)


                MessageManager.RegisterPowerEvent(EventManager.PowerPersonalityChange, value)
            End AddHandler
            RemoveHandler(value As EventHandler)

                CoreHelpers.ThrowIfNotVista()

                MessageManager.UnregisterPowerEvent(EventManager.PowerPersonalityChange, value)
            End RemoveHandler
            RaiseEvent()

            End RaiseEvent
        End Event

        ''' <summary>
        ''' Raised when the power source changes.
        ''' </summary>
        ''' <exception cref="InvalidOperationException">The event handler specified for removal was not registered.</exception>
        ''' <exception cref="System.PlatformNotSupportedException">Requires Vista/Windows Server 2008.</exception>
        Public Shared Custom Event PowerSourceChanged As EventHandler
            AddHandler(value As EventHandler)
                CoreHelpers.ThrowIfNotVista()

                MessageManager.RegisterPowerEvent(EventManager.PowerSourceChange, value)
            End AddHandler
            RemoveHandler(value As EventHandler)

                CoreHelpers.ThrowIfNotVista()

                MessageManager.UnregisterPowerEvent(EventManager.PowerSourceChange, value)
            End RemoveHandler
            RaiseEvent()

            End RaiseEvent
        End Event

        ''' <summary>
        ''' Raised when the remaining battery life changes.
        ''' </summary>
        ''' <exception cref="InvalidOperationException">The event handler specified for removal was not registered.</exception>
        ''' <exception cref="System.PlatformNotSupportedException">Requires Vista/Windows Server 2008.</exception>
        Public Shared Custom Event BatteryLifePercentChanged As EventHandler
            AddHandler(value As EventHandler)
                CoreHelpers.ThrowIfNotVista()

                MessageManager.RegisterPowerEvent(EventManager.BatteryCapacityChange, value)
            End AddHandler
            RemoveHandler(value As EventHandler)
                CoreHelpers.ThrowIfNotVista()

                MessageManager.UnregisterPowerEvent(EventManager.BatteryCapacityChange, value)
            End RemoveHandler
            RaiseEvent()

            End RaiseEvent
        End Event

        ''' <summary>
        ''' Raised when the monitor status changes.
        ''' </summary>
        ''' <exception cref="InvalidOperationException">The event handler specified for removal was not registered.</exception>
        ''' <exception cref="System.PlatformNotSupportedException">Requires Vista/Windows Server 2008.</exception>
        Public Shared Custom Event IsMonitorOnChanged As EventHandler
            AddHandler(value As EventHandler)
                CoreHelpers.ThrowIfNotVista()

                MessageManager.RegisterPowerEvent(EventManager.MonitorPowerStatus, value)
            End AddHandler
            RemoveHandler(value As EventHandler)
                CoreHelpers.ThrowIfNotVista()

                MessageManager.UnregisterPowerEvent(EventManager.MonitorPowerStatus, value)
            End RemoveHandler
            RaiseEvent()

            End RaiseEvent
        End Event

        ''' <summary>
        ''' Raised when the system will not be moving into an idle 
        ''' state in the near future so applications should
        ''' perform any tasks that 
        ''' would otherwise prevent the computer from entering an idle state. 
        ''' </summary>
        ''' <exception cref="InvalidOperationException">The event handler specified for removal was not registered.</exception>
        ''' <exception cref="System.PlatformNotSupportedException">Requires Vista/Windows Server 2008.</exception>
        Public Shared Custom Event SystemBusyChanged As EventHandler
            AddHandler(value As EventHandler)
                CoreHelpers.ThrowIfNotVista()

                MessageManager.RegisterPowerEvent(EventManager.BackgroundTaskNotification, value)
            End AddHandler
            RemoveHandler(value As EventHandler)
                CoreHelpers.ThrowIfNotVista()

                MessageManager.UnregisterPowerEvent(EventManager.BackgroundTaskNotification, value)
            End RemoveHandler
            RaiseEvent()

            End RaiseEvent
        End Event
#End Region

        ''' <summary>
        ''' Gets a snapshot of the current battery state.
        ''' </summary>
        ''' <returns>A <see cref="BatteryState"/> instance that represents 
        ''' the state of the battery at the time this method was called.</returns>
        ''' <exception cref="System.InvalidOperationException">The system does not have a battery.</exception>
        ''' <exception cref="System.PlatformNotSupportedException">Requires XP/Windows Server 2003 or higher.</exception>        
        Public Shared Function GetCurrentBatteryState() As BatteryState
            CoreHelpers.ThrowIfNotXP()
            Return New BatteryState()
        End Function

#Region "Power System Properties"

        ''' <summary>
        ''' Gets or sets a value that indicates whether the monitor is 
        ''' set to remain active.  
        ''' </summary>
        ''' <exception cref="T:System.PlatformNotSupportedException">Requires XP/Windows Server 2003 or higher.</exception>
        ''' <exception cref="T:System.Security.SecurityException">The caller does not have sufficient privileges to set this property.
        ''' </exception>
        ''' <remarks>This information is typically used by applications
        ''' that display information but do not require 
        ''' user interaction. For example, video playback applications.</remarks>
        ''' <permission cref="T:System.Security.Permissions.SecurityPermission"> to set this property. Demand value: <see cref="F:System.Security.Permissions.SecurityAction.Demand"/>; Named Permission Sets: <b>FullTrust</b>.</permission>
        ''' <value>A <see cref="System.Boolean"/> value. <b>True</b> if the monitor
        ''' is required to remain on.</value>
        Public Shared Property MonitorRequired() As Boolean
            Get
                CoreHelpers.ThrowIfNotXP()
                Return m_monitorRequired
            End Get
            <System.Security.Permissions.PermissionSetAttribute(System.Security.Permissions.SecurityAction.Demand, Name:="FullTrust")>
            Set
                CoreHelpers.ThrowIfNotXP()

                If Value Then
                    PowerManager.SetThreadExecutionState(ExecutionStates.Continuous Or ExecutionStates.DisplayRequired)
                Else
                    PowerManager.SetThreadExecutionState(ExecutionStates.Continuous)
                End If

                m_monitorRequired = Value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a value that indicates whether the system 
        ''' is required to be in the working state.
        ''' </summary>
        ''' <exception cref="System.PlatformNotSupportedException">Requires XP/Windows Server 2003 or higher.</exception>
        ''' <exception cref="System.Security.SecurityException">The caller does not have sufficient privileges to set this property.
        ''' </exception>
        ''' <permission cref="System.Security.Permissions.SecurityPermission"> to set this property. Demand value: <see cref="F:System.Security.Permissions.SecurityAction.Demand"/>; Named Permission Sets: <b>FullTrust</b>.</permission>
        ''' <value>A <see cref="System.Boolean"/> value.</value>
        Public Shared Property RequestBlockSleep() As Boolean
            Get
                CoreHelpers.ThrowIfNotXP()

                Return m_requestBlockSleep
            End Get
            <System.Security.Permissions.PermissionSetAttribute(System.Security.Permissions.SecurityAction.Demand, Name:="FullTrust")>
            Set
                CoreHelpers.ThrowIfNotXP()

                If Value Then
                    PowerManager.SetThreadExecutionState(ExecutionStates.Continuous Or ExecutionStates.SystemRequired)
                Else
                    PowerManager.SetThreadExecutionState(ExecutionStates.Continuous)
                End If

                m_requestBlockSleep = Value
            End Set
        End Property

        ''' <summary>
        ''' Gets a value that indicates whether a battery is present.  
        ''' The battery can be a short term battery.
        ''' </summary>
        ''' <exception cref="System.PlatformNotSupportedException">Requires XP/Windows Server 2003 or higher.</exception>
        ''' <value>A <see cref="System.Boolean"/> value.</value>
        Public Shared ReadOnly Property IsBatteryPresent() As Boolean
            Get
                CoreHelpers.ThrowIfNotXP()

                Return Power.GetSystemBatteryState().BatteryPresent
            End Get
        End Property

        ''' <summary>
        ''' Gets a value that indicates whether the battery is a short term battery. 
        ''' </summary>
        ''' <exception cref="System.PlatformNotSupportedException">Requires XP/Windows Server 2003 or higher.</exception>
        ''' <value>A <see cref="System.Boolean"/> value.</value>
        Public Shared ReadOnly Property IsBatteryShortTerm() As Boolean
            Get
                CoreHelpers.ThrowIfNotXP()
                Return Power.GetSystemPowerCapabilities().BatteriesAreShortTerm
            End Get
        End Property

        ''' <summary>
        ''' Gets a value that indicates a UPS is present to prevent 
        ''' sudden loss of power.
        ''' </summary>
        ''' <exception cref="System.PlatformNotSupportedException">Requires XP/Windows Server 2003 or higher.</exception>
        ''' <value>A <see cref="System.Boolean"/> value.</value>
        Public Shared ReadOnly Property IsUpsPresent() As Boolean
            Get
                CoreHelpers.ThrowIfNotXP()

                ' Because the native method doesn't return the correct value for .UpsPresent,
                ' use .BatteriesAreShortTerm and .SystemBatteriesPresent to check for UPS
                Dim batt As PowerManagementNativeMethods.SystemPowerCapabilities = Power.GetSystemPowerCapabilities()

                Return (batt.BatteriesAreShortTerm AndAlso batt.SystemBatteriesPresent)
            End Get
        End Property

        ''' <summary>
        ''' Gets a value that indicates the current power scheme.  
        ''' </summary>
        ''' <exception cref="System.PlatformNotSupportedException">Requires Vista/Windows Server 2008.</exception>
        ''' <value>A <see cref="PowerPersonality"/> value.</value>
        Public Shared ReadOnly Property PowerPersonality() As PowerPersonality
            Get
                Dim guid As Guid
                PowerManagementNativeMethods.PowerGetActiveScheme(IntPtr.Zero, guid)

                Try
                    Return PowerPersonalityGuids.GuidToEnum(guid)
                Finally
                    CoreNativeMethods.LocalFree(guid)
                End Try
            End Get
        End Property



        ''' <summary>
        ''' Gets a value that indicates the remaining battery life 
        ''' (as a percentage of the full battery charge). 
        ''' This value is in the range 0-100, 
        ''' where 0 is not charged and 100 is fully charged.  
        ''' </summary>
        ''' <exception cref="System.InvalidOperationException">The system does not have a battery.</exception>
        ''' <exception cref="System.PlatformNotSupportedException">Requires Vista/Windows Server 2008.</exception>
        ''' <value>An <see cref="System.Int32"/> value.</value>
        Public Shared ReadOnly Property BatteryLifePercent() As Integer
            Get
                ' Because of the way this value is being calculated, it should not be limited to granularity
                ' as the data from the event (old way) was.
                CoreHelpers.ThrowIfNotVista()
                If Not Power.GetSystemBatteryState().BatteryPresent Then
                    Throw New InvalidOperationException(LocalizedMessages.PowerManagerBatteryNotPresent)
                End If

                Dim state As PowerManagementNativeMethods.SystemBatteryState = Power.GetSystemBatteryState()

                Dim percent As Integer = CInt(Math.Truncate(Math.Round((CDbl(state.RemainingCapacity) / state.MaxCapacity * 100), 0)))
                Return percent
            End Get
        End Property

        ''' <summary>
        ''' Gets a value that indictates whether the monitor is on. 
        ''' </summary>
        ''' <exception cref="System.PlatformNotSupportedException">Requires Vista/Windows Server 2008.</exception>
        ''' <value>A <see cref="System.Boolean"/> value.</value>
        Public Shared Property IsMonitorOn() As Boolean
            Get
                CoreHelpers.ThrowIfNotVista()

                SyncLock monitoronlock
                    If m_isMonitorOn Is Nothing Then
                        Dim dummy As EventHandler = Sub(sender As Object, args As EventArgs)

                                                    End Sub
                        AddHandler IsMonitorOnChanged, dummy
                        ' Wait until Windows updates the power source 
                        ' (through RegisterPowerSettingNotification)
                        EventManager.monitorOnReset.WaitOne()
                    End If
                End SyncLock

                Return CBool(m_isMonitorOn)
            End Get
            Friend Set
                m_isMonitorOn = Value
            End Set
        End Property

        ''' <summary>
        ''' Gets the current power source.  
        ''' </summary>
        ''' <exception cref="System.PlatformNotSupportedException">Requires Vista/Windows Server 2008.</exception>
        ''' <value>A <see cref="PowerSource"/> value.</value>
        Public Shared ReadOnly Property PowerSource() As PowerSource
            Get
                CoreHelpers.ThrowIfNotVista()

                If IsUpsPresent Then
                    Return PowerSource.Ups
                End If

                If Not IsBatteryPresent OrElse GetCurrentBatteryState().ACOnline Then
                    Return PowerSource.AC
                End If

                Return PowerSource.Battery
            End Get
        End Property
#End Region

        ''' <summary>
        ''' Allows an application to inform the system that it 
        ''' is in use, thereby preventing the system from entering 
        ''' the sleeping power state or turning off the display 
        ''' while the application is running.
        ''' </summary>
        ''' <param name="executionStateOptions">The thread's execution requirements.</param>
        ''' <exception cref="Win32Exception">Thrown if the SetThreadExecutionState call fails.</exception>
        Public Shared Sub SetThreadExecutionState(executionStateOptions As ExecutionStates)
            Dim ret As ExecutionStates = PowerManagementNativeMethods.SetThreadExecutionState(executionStateOptions)
            If ret = ExecutionStates.None Then
                Throw New Win32Exception(LocalizedMessages.PowerExecutionStateFailed)
            End If
        End Sub

    End Class
End Namespace
