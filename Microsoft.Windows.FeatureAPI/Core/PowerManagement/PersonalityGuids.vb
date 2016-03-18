'Copyright (c) Microsoft Corporation.  All rights reserved.


Namespace ApplicationServices
	Friend NotInheritable Class PowerPersonalityGuids
		Private Sub New()
		End Sub
		Friend Shared ReadOnly HighPerformance As New Guid(&H8c5e7fdaUI, &He8bf, &H4a96, &H9a, &H85, &Ha6, _
			&He2, &H3a, &H8c, &H63, &H5c)
		Friend Shared ReadOnly PowerSaver As New Guid(&Ha1841308UI, &H3541, &H4fab, &Hbc, &H81, &Hf7, _
			&H15, &H56, &Hf2, &Hb, &H4a)
		Friend Shared ReadOnly Automatic As New Guid(&H381b4222, &Hf694, &H41f0, &H96, &H85, &Hff, _
			&H5b, &Hb2, &H60, &Hdf, &H2e)

		Friend Shared ReadOnly All As New Guid(&H68a1e95e, &H13ea, &H41e1, &H80, &H11, &Hc, _
			&H49, &H6c, &Ha4, &H90, &Hb0)

		Friend Shared Function GuidToEnum(guid As Guid) As PowerPersonality
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
	End Class
End Namespace
