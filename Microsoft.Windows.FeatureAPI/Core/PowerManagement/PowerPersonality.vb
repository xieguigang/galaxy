'Copyright (c) Microsoft Corporation.  All rights reserved.

Namespace ApplicationServices

    ''' <summary>
    ''' Specifies the supported power personalities.  
    ''' </summary>
    Public Enum PowerPersonality
        ''' <summary>
        ''' The power personality Guid does not match a known value.
        ''' </summary>
        Unknown
        ''' <summary>
        ''' Power settings designed to deliver maximum performance
        ''' at the expense of power consumption savings.
        ''' </summary>
        HighPerformance

        ''' <summary>
        ''' Power settings designed consume minimum power
        ''' at the expense of system performance and responsiveness.
        ''' </summary>
        PowerSaver

        ''' <summary>
        ''' Power settings designed to balance performance 
        ''' and power consumption.
        ''' </summary>
        Automatic
    End Enum

End Namespace
