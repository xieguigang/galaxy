'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Diagnostics.CodeAnalysis

Namespace ApplicationServices

    ''' <summary>
    ''' Enumeration of execution states.
    ''' </summary>
    <SuppressMessage("Microsoft.Usage", "CA2217:DoNotMarkEnumsWithFlags")>
    <Flags>
    Public Enum ExecutionStates
        ''' <summary>
        ''' No state configured.
        ''' </summary>
        None = 0

        ''' <summary>
        ''' Forces the system to be in the working state by resetting the system idle timer.
        ''' </summary>
        SystemRequired = &H1

        ''' <summary>
        ''' Forces the display to be on by resetting the display idle timer.
        ''' </summary>
        DisplayRequired = &H2

        ''' <summary>
        ''' Enables away mode. This value must be specified with ES_CONTINUOUS.
        ''' Away mode should be used only by media-recording and media-distribution applications that must perform critical background processing on desktop computers while the computer appears to be sleeping. See Remarks.
        ''' 
        ''' Windows Server 2003 and Windows XP/2000:  ES_AWAYMODE_REQUIRED is not supported.
        ''' </summary>
        AwayModeRequired = &H40

        ''' <summary>
        ''' Informs the system that the state being set should remain in effect until the next call that uses ES_CONTINUOUS and one of the other state flags is cleared.
        ''' </summary>
        Continuous = &H80000000UI
    End Enum
End Namespace
