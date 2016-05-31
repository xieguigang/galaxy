'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Runtime.InteropServices

Namespace ApplicationServices

    Public Module PowerManagementNativeMethods

#Region "Power Management"

        Public Const PowerBroadcastMessage As UInteger = 536
        Public Const PowerSettingChangeMessage As UInteger = 32787
        Public Const ScreenSaverSetActive As UInteger = &H11
        Public Const UpdateInFile As UInteger = &H1
        Public Const SendChange As UInteger = &H2

        ' This structure is sent when the PBT_POWERSETTINGSCHANGE message is sent.
        ' It describes the power setting that has changed and 
        ' contains data about the change.
        <StructLayout(LayoutKind.Sequential, Pack:=4)>
        Public Structure PowerBroadcastSetting
            Public PowerSetting As Guid
            Public DataLength As Int32
        End Structure

        ' This structure is used when calling CallNtPowerInformation 
        ' to retrieve SystemPowerCapabilities
        <StructLayout(LayoutKind.Sequential)>
        Public Structure SystemPowerCapabilities
            <MarshalAs(UnmanagedType.I1)>
            Public PowerButtonPresent As Boolean
            <MarshalAs(UnmanagedType.I1)>
            Public SleepButtonPresent As Boolean
            <MarshalAs(UnmanagedType.I1)>
            Public LidPresent As Boolean
            <MarshalAs(UnmanagedType.I1)>
            Public SystemS1 As Boolean
            <MarshalAs(UnmanagedType.I1)>
            Public SystemS2 As Boolean
            <MarshalAs(UnmanagedType.I1)>
            Public SystemS3 As Boolean
            <MarshalAs(UnmanagedType.I1)>
            Public SystemS4 As Boolean
            <MarshalAs(UnmanagedType.I1)>
            Public SystemS5 As Boolean
            <MarshalAs(UnmanagedType.I1)>
            Public HiberFilePresent As Boolean
            <MarshalAs(UnmanagedType.I1)>
            Public FullWake As Boolean
            <MarshalAs(UnmanagedType.I1)>
            Public VideoDimPresent As Boolean
            <MarshalAs(UnmanagedType.I1)>
            Public ApmPresent As Boolean
            <MarshalAs(UnmanagedType.I1)>
            Public UpsPresent As Boolean
            <MarshalAs(UnmanagedType.I1)>
            Public ThermalControl As Boolean
            <MarshalAs(UnmanagedType.I1)>
            Public ProcessorThrottle As Boolean
            Public ProcessorMinimumThrottle As Byte
            Public ProcessorMaximumThrottle As Byte
            <MarshalAs(UnmanagedType.I1)>
            Public FastSystemS4 As Boolean
            <MarshalAs(UnmanagedType.ByValArray, SizeConst:=3)>
            Public spare2 As Byte()
            <MarshalAs(UnmanagedType.I1)>
            Public DiskSpinDown As Boolean
            <MarshalAs(UnmanagedType.ByValArray, SizeConst:=8)>
            Public spare3 As Byte()
            <MarshalAs(UnmanagedType.I1)>
            Public SystemBatteriesPresent As Boolean
            <MarshalAs(UnmanagedType.I1)>
            Public BatteriesAreShortTerm As Boolean
            <MarshalAs(UnmanagedType.ByValArray, SizeConst:=3)>
            Public BatteryScale As BatteryReportingScale()
            Public AcOnlineWake As SystemPowerState
            Public SoftLidWake As SystemPowerState
            Public RtcWake As SystemPowerState
            Public MinimumDeviceWakeState As SystemPowerState
            Public DefaultLowLatencyWake As SystemPowerState
        End Structure

        Public Enum PowerInformationLevel
            SystemPowerPolicyAc
            SystemPowerPolicyDc
            VerifySystemPolicyAc
            VerifySystemPolicyDc
            SystemPowerCapabilities
            SystemBatteryState
            SystemPowerStateHandler
            ProcessorStateHandler
            SystemPowerPolicyCurrent
            AdministratorPowerPolicy
            SystemReserveHiberFile
            ProcessorInformation
            SystemPowerInformation
            ProcessorStateHandler2
            LastWakeTime
            LastSleepTime
            SystemExecutionState
            SystemPowerStateNotifyHandler
            ProcessorPowerPolicyAc
            ProcessorPowerPolicyDc
            VerifyProcessorPowerPolicyAc
            VerifyProcessorPowerPolicyDc
            ProcessorPowerPolicyCurrent
            SystemPowerStateLogging
            SystemPowerLoggingEntry
            SetPowerSettingValue
            NotifyUserPowerSetting
            PowerInformationLevelUnused0
            PowerInformationLevelUnused1
            SystemVideoState
            TraceApplicationPowerMessage
            TraceApplicationPowerMessageEnd
            ProcessorPerfStates
            ProcessorIdleStates
            ProcessorCap
            SystemWakeSource
            SystemHiberFileInformation
            TraceServicePowerMessage
            ProcessorLoad
            PowerShutdownNotification
            MonitorCapabilities
            SessionPowerInit
            SessionDisplayState
            PowerRequestCreate
            PowerRequestAction
            GetPowerRequestList
            ProcessorInformationEx
            NotifyUserModeLegacyPowerEvent
            GroupPark
            ProcessorIdleDomains
            WakeTimerList
            SystemHiberFileSize
            PowerInformationLevelMaximum
        End Enum

        <StructLayout(LayoutKind.Sequential)>
        Public Structure BatteryReportingScale
            Public Granularity As UInt32
            Public Capacity As UInt32
        End Structure

        Public Enum SystemPowerState
            Unspecified = 0
            Working = 1
            Sleeping1 = 2
            Sleeping2 = 3
            Sleeping3 = 4
            Hibernate = 5
            Shutdown = 6
            Maximum = 7
        End Enum

        <StructLayout(LayoutKind.Sequential)>
        Public Structure SystemBatteryState
            <MarshalAs(UnmanagedType.I1)>
            Public AcOnLine As Boolean
            <MarshalAs(UnmanagedType.I1)>
            Public BatteryPresent As Boolean
            <MarshalAs(UnmanagedType.I1)>
            Public Charging As Boolean
            <MarshalAs(UnmanagedType.I1)>
            Public Discharging As Boolean
            Public Spare1 As Byte
            Public Spare2 As Byte
            Public Spare3 As Byte
            Public Spare4 As Byte
            Public MaxCapacity As UInteger
            Public RemainingCapacity As UInteger
            Public Rate As UInteger
            Public EstimatedTime As UInteger
            Public DefaultAlert1 As UInteger
            Public DefaultAlert2 As UInteger
        End Structure

        <DllImport("powrprof.dll")>
        Public Function CallNtPowerInformation(informationLevel As PowerInformationLevel, inputBuffer As IntPtr, inputBufferSize As UInt32, ByRef outputBuffer As SystemPowerCapabilities, outputBufferSize As UInt32) As UInt32
        End Function

        <DllImport("powrprof.dll")>
        Public Function CallNtPowerInformation(informationLevel As PowerInformationLevel, inputBuffer As IntPtr, inputBufferSize As UInt32, ByRef outputBuffer As SystemBatteryState, outputBufferSize As UInt32) As UInt32
        End Function

        ''' <summary>
        ''' Gets the Guid relating to the currently active power scheme.
        ''' </summary>
        ''' <param name="rootPowerKey">Reserved for future use, this must be set to IntPtr.Zero</param>
        ''' <param name="activePolicy">Returns a Guid referring to the currently active power scheme.</param>
        <DllImport("powrprof.dll")>
        Public Sub PowerGetActiveScheme(rootPowerKey As IntPtr, <MarshalAs(UnmanagedType.LPStruct)> ByRef activePolicy As Guid)
        End Sub

        <DllImport("User32", SetLastError:=True, EntryPoint:="RegisterPowerSettingNotification", CallingConvention:=CallingConvention.StdCall)>
        Public Function RegisterPowerSettingNotification(hRecipient As IntPtr, ByRef PowerSettingGuid As Guid, Flags As Int32) As Integer
        End Function

        <DllImport("kernel32.dll", SetLastError:=True)>
        Public Function SetThreadExecutionState(esFlags As ExecutionStates) As ExecutionStates
        End Function

#End Region
    End Module
End Namespace
