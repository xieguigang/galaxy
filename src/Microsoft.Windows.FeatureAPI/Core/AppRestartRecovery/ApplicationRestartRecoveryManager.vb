'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Runtime.InteropServices
Imports Microsoft.Windows.Internal

Namespace ApplicationServices

    ''' <summary>
    ''' Provides access to the Application Restart and Recovery
    ''' features available in Windows Vista or higher. Application Restart and Recovery lets an
    ''' application do some recovery work to save data before the process exits.
    ''' </summary>
    Public Module ApplicationRestartRecoveryManager

        ''' <summary>
        ''' Registers an application for recovery by Application Restart and Recovery.
        ''' </summary>
        ''' <param name="settings">An object that specifies
        ''' the callback method, an optional parameter to pass to the callback
        ''' method and a time interval.</param>
        ''' <exception cref="System.ArgumentException">
        ''' The registration failed due to an invalid parameter.
        ''' </exception>
        ''' <exception cref="System.ComponentModel.Win32Exception">
        ''' The registration failed.</exception>
        ''' <remarks>The time interval is the period of time within 
        ''' which the recovery callback method 
        ''' calls the <see cref="ApplicationRecoveryInProgress"/> method to indicate
        ''' that it is still performing recovery work.</remarks>        
        Public Sub RegisterForApplicationRecovery(settings As RecoverySettings)
            CoreHelpers.ThrowIfNotVista()

            If settings Is Nothing Then
                Throw New ArgumentNullException("settings")
            End If

            Dim handle As GCHandle = GCHandle.Alloc(settings.RecoveryData)

            Dim hr As HResult = AppRestartRecoveryNativeMethods.RegisterApplicationRecoveryCallback(AppRestartRecoveryNativeMethods.InternalCallback, CType(handle, IntPtr), settings.PingInterval, CUInt(0))

            If Not CoreErrorHelper.Succeeded(hr) Then
                If hr = HResult.InvalidArguments Then
                    Throw New ArgumentException(LocalizedMessages.ApplicationRecoveryBadParameters, "settings")
                End If

                Throw New ApplicationRecoveryException(LocalizedMessages.ApplicationRecoveryFailedToRegister)
            End If
        End Sub

        ''' <summary>
        ''' Removes an application's recovery registration.
        ''' </summary>
        ''' <exception cref="ApplicationServices.ApplicationRecoveryException">
        ''' The attempt to unregister for recovery failed.</exception>
        Public Sub UnregisterApplicationRecovery()
            CoreHelpers.ThrowIfNotVista()

            Dim hr As HResult = AppRestartRecoveryNativeMethods.UnregisterApplicationRecoveryCallback()

            If Not CoreErrorHelper.Succeeded(hr) Then
                Throw New ApplicationRecoveryException(LocalizedMessages.ApplicationRecoveryFailedToUnregister)
            End If
        End Sub

        ''' <summary>
        ''' Removes an application's restart registration.
        ''' </summary>
        ''' <exception cref="ApplicationServices.ApplicationRecoveryException">
        ''' The attempt to unregister for restart failed.</exception>
        Public Sub UnregisterApplicationRestart()
            CoreHelpers.ThrowIfNotVista()

            Dim hr As HResult = AppRestartRecoveryNativeMethods.UnregisterApplicationRestart()

            If Not CoreErrorHelper.Succeeded(hr) Then
                Throw New ApplicationRecoveryException(LocalizedMessages.ApplicationRecoveryFailedToUnregisterForRestart)
            End If
        End Sub

        ''' <summary>
        ''' Called by an application's <see cref="RecoveryCallback"/> method 
        ''' to indicate that it is still performing recovery work.
        ''' </summary>
        ''' <returns>A <see cref="System.Boolean"/> value indicating whether the user
        ''' canceled the recovery.</returns>
        ''' <exception cref="ApplicationServices.ApplicationRecoveryException">
        ''' This method must be called from a registered callback method.</exception>
        Public Function ApplicationRecoveryInProgress() As Boolean
            CoreHelpers.ThrowIfNotVista()

            Dim canceled As Boolean = False
            Dim hr As HResult = AppRestartRecoveryNativeMethods.ApplicationRecoveryInProgress(canceled)

            If Not CoreErrorHelper.Succeeded(hr) Then
                Throw New InvalidOperationException(LocalizedMessages.ApplicationRecoveryMustBeCalledFromCallback)
            End If

            Return canceled
        End Function

        ''' <summary>
        ''' Called by an application's <see cref="RecoveryCallback"/> method to 
        ''' indicate that the recovery work is complete.
        ''' </summary>
        ''' <remarks>
        ''' This should
        ''' be the last call made by the <see cref="RecoveryCallback"/> method because
        ''' Windows Error Reporting will terminate the application
        ''' after this method is invoked.
        ''' </remarks>
        ''' <param name="success"><b>true</b> to indicate the the program was able to complete its recovery
        ''' work before terminating; otherwise <b>false</b>.</param>
        Public Sub ApplicationRecoveryFinished(success As Boolean)
            CoreHelpers.ThrowIfNotVista()

            AppRestartRecoveryNativeMethods.ApplicationRecoveryFinished(success)
        End Sub

        ''' <summary>
        ''' Registers an application for automatic restart if 
        ''' the application 
        ''' is terminated by Windows Error Reporting.
        ''' </summary>
        ''' <param name="settings">An object that specifies
        ''' the command line arguments used to restart the 
        ''' application, and 
        ''' the conditions under which the application should not be 
        ''' restarted.</param>
        ''' <exception cref="System.ArgumentException">Registration failed due to an invalid parameter.</exception>
        ''' <exception cref="System.InvalidOperationException">The attempt to register failed.</exception>
        ''' <remarks>A registered application will not be restarted if it executed for less than 60 seconds before terminating.</remarks>
        Public Sub RegisterForApplicationRestart(settings As RestartSettings)
            ' Throw PlatformNotSupportedException if the user is not running Vista or beyond
            CoreHelpers.ThrowIfNotVista()
            If settings Is Nothing Then
                Throw New ArgumentNullException("settings")
            End If

            Dim hr As HResult = AppRestartRecoveryNativeMethods.RegisterApplicationRestart(settings.Command, settings.Restrictions)

            If hr = HResult.Fail Then
                Throw New InvalidOperationException(LocalizedMessages.ApplicationRecoveryFailedToRegisterForRestart)
            ElseIf hr = HResult.InvalidArguments Then
                Throw New ArgumentException(LocalizedMessages.ApplicationRecoverFailedToRegisterForRestartBadParameters)
            End If
        End Sub
    End Module
End Namespace

