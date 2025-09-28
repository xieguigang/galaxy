'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.ComponentModel
Imports System.Diagnostics.CodeAnalysis
Imports System.Runtime.InteropServices
Imports Microsoft.Windows.Internal
Imports Microsoft.Windows.Resources

Namespace ApplicationServices

    Public Module Power

        Public Function GetSystemPowerCapabilities() As PowerManagementNativeMethods.SystemPowerCapabilities
            Dim powerCap As PowerManagementNativeMethods.SystemPowerCapabilities

            Dim retval As UInteger = PowerManagementNativeMethods.CallNtPowerInformation(PowerManagementNativeMethods.PowerInformationLevel.SystemPowerCapabilities, IntPtr.Zero, 0, powerCap, CType(Marshal.SizeOf(GetType(PowerManagementNativeMethods.SystemPowerCapabilities)), UInt32))

            If retval = CoreNativeMethods.StatusAccessDenied Then
                Throw New UnauthorizedAccessException(LocalizedMessages.PowerInsufficientAccessCapabilities)
            End If

            Return powerCap
        End Function

        Public Function GetSystemBatteryState() As PowerManagementNativeMethods.SystemBatteryState
            Dim batteryState As PowerManagementNativeMethods.SystemBatteryState

            Dim retval As UInteger = PowerManagementNativeMethods.CallNtPowerInformation(PowerManagementNativeMethods.PowerInformationLevel.SystemBatteryState, IntPtr.Zero, 0, batteryState, CType(Marshal.SizeOf(GetType(PowerManagementNativeMethods.SystemBatteryState)), UInt32))

            If retval = CoreNativeMethods.StatusAccessDenied Then
                Throw New UnauthorizedAccessException(LocalizedMessages.PowerInsufficientAccessBatteryState)
            End If

            Return batteryState
        End Function

        ''' <summary>
        ''' Registers the application to receive power setting notifications 
        ''' for the specific power setting event.
        ''' </summary>
        ''' <param name="handle">Handle indicating where the power setting 
        ''' notifications are to be sent.</param>
        ''' <param name="powerSetting">The GUID of the power setting for 
        ''' which notifications are to be sent.</param>
        ''' <returns>Returns a notification handle for unregistering 
        ''' power notifications.</returns>
        Public Function RegisterPowerSettingNotification(handle As IntPtr, powerSetting As Guid) As Integer
            Dim outHandle As Integer = PowerManagementNativeMethods.RegisterPowerSettingNotification(handle, powerSetting, 0)

            Return outHandle
        End Function
    End Module
End Namespace
