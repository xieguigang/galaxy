'Copyright (c) Microsoft Corporation.  All rights reserved.


Namespace ApplicationServices

    Module PowerPersonalityGuids

        Friend ReadOnly HighPerformance As New Guid(&H8C5E7FDAUI, &HE8BF, &H4A96, &H9A, &H85, &HA6,
            &HE2, &H3A, &H8C, &H63, &H5C)
        Friend ReadOnly PowerSaver As New Guid(&HA1841308UI, &H3541, &H4FAB, &HBC, &H81, &HF7,
            &H15, &H56, &HF2, &HB, &H4A)
        Friend ReadOnly Automatic As New Guid(&H381B4222, &HF694, &H41F0, &H96, &H85, &HFF,
            &H5B, &HB2, &H60, &HDF, &H2E)

        Friend ReadOnly All As New Guid(&H68A1E95E, &H13EA, &H41E1, &H80, &H11, &HC,
            &H49, &H6C, &HA4, &H90, &HB0)

        Friend Function GuidToEnum(guid As Guid) As PowerPersonality
            If guid = HighPerformance Then
                Return PowerPersonality.HighPerformance
            ElseIf guid = PowerSaver Then
                Return PowerPersonality.PowerSaver
            ElseIf guid = Automatic Then
                Return PowerPersonality.Automatic
            Else
                Return PowerPersonality.Unknown
            End If
        End Function
    End Module
End Namespace
