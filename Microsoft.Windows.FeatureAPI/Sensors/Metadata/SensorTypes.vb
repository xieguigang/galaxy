' Copyright (c) Microsoft Corporation.  All rights reserved.


Namespace Sensors
	''' <summary>
	''' Contains a list of well known sensor types. This class will be removed once wrappers are developed.
	''' </summary>
	Public NotInheritable Class SensorTypes
		Private Sub New()
		End Sub
		''' <summary>
		''' The GPS location sensor type property key.
		''' </summary>
		Public Shared ReadOnly LocationGps As New Guid(&Hed4ca589UI, &H327a, &H4ff9, &Ha5, &H60, &H91, _
			&Hda, &H4b, &H48, &H27, &H5e)

		''' <summary>
		''' The environmental temperature sensor type property key.
		''' </summary>
		Public Shared ReadOnly EnvironmentalTemperature As New Guid(&H4fd0ec4, &Hd5da, &H45fa, &H95, &Ha9, &H5d, _
			&Hb3, &H8e, &He1, &H93, &H6)

		''' <summary>
		''' The environmental atmostpheric pressure sensor type property key.
		''' </summary>
		Public Shared ReadOnly EnvironmentalAtmosphericPressure As New Guid(&He903829, &Hff8a, &H4a93, &H97, &Hdf, &H3d, _
			&Hcb, &Hde, &H40, &H22, &H88)

		''' <summary>
		''' The environmental humidity sensor type property key.
		''' </summary>
		Public Shared ReadOnly EnvironmentalHumidity As New Guid(&H5c72bf67, &Hbd7e, &H4257, &H99, &Hb, &H98, _
			&Ha3, &Hba, &H3b, &H40, &Ha)

		''' <summary>
		''' The environmental wind speed sensor type property key.
		''' </summary>
		Public Shared ReadOnly EnvironmentalWindSpeed As New Guid(&Hdd50607bUI, &Ha45f, &H42cd, &H8e, &Hfd, &Hec, _
			&H61, &H76, &H1c, &H42, &H26)

		''' <summary>
		''' The environmental wind direction sensor type property key.
		''' </summary>
		Public Shared ReadOnly EnvironmentalWindDirection As New Guid(&H9ef57a35UI, &H9306, &H434d, &Haf, &H9, &H37, _
			&Hfa, &H5a, &H9c, &H0, &Hbd)

		''' <summary>
		''' The accelerometer sensor type property key.
		''' </summary>
		Public Shared ReadOnly Accelerometer1D As New Guid(&Hc04d2387UI, &H7340, &H4cc2, &H99, &H1e, &H3b, _
			&H18, &Hcb, &H8e, &Hf2, &Hf4)

		''' <summary>
		''' The 2D accelerometer sensor type property key.
		''' </summary>
		Public Shared ReadOnly Accelerometer2D As New Guid(&Hb2c517a8UI, &Hf6b5, &H4ba6, &Ha4, &H23, &H5d, _
			&Hf5, &H60, &Hb4, &Hcc, &H7)

		''' <summary>
		''' The 3D accelerometer sensor type property key.
		''' </summary>
		Public Shared ReadOnly Accelerometer3D As New Guid(&Hc2fb0f5fUI, &He2d2, &H4c78, &Hbc, &Hd0, &H35, _
			&H2a, &H95, &H82, &H81, &H9d)

		''' <summary>
		''' The motion sensor type property key.
		''' </summary>
		Public Shared ReadOnly MotionDetector As New Guid(&H5c7c1a12, &H30a5, &H43b9, &Ha4, &Hb2, &Hcf, _
			&H9, &Hec, &H5b, &H7b, &He8)

		''' <summary>
		''' The gyrometer sensor type property key.
		''' </summary>
		Public Shared ReadOnly Gyrometer1D As New Guid(&Hfa088734UI, &Hf552, &H4584, &H83, &H24, &Hed, _
			&Hfa, &Hf6, &H49, &H65, &H2c)

		''' <summary>
		''' The 2D gyrometer sensor type property key.
		''' </summary>
		Public Shared ReadOnly Gyrometer2D As New Guid(&H31ef4f83, &H919b, &H48bf, &H8d, &He0, &H5d, _
			&H7a, &H9d, &H24, &H5, &H56)

		''' <summary>
		''' The 3D gyrometer sensor type property key.
		''' </summary>
		Public Shared ReadOnly Gyrometer3D As New Guid(&H9485f5a, &H759e, &H42c2, &Hbd, &H4b, &Ha3, _
			&H49, &Hb7, &H5c, &H86, &H43)

		''' <summary>
		''' The speedometer sensor type property key.
		''' </summary>
		Public Shared ReadOnly Speedometer As New Guid(&H6bd73c1f, &Hbb4, &H4310, &H81, &Hb2, &Hdf, _
			&Hc1, &H8a, &H52, &Hbf, &H94)

		''' <summary>
		''' The compass sensor type property key.
		''' </summary>
		Public Shared ReadOnly Compass1D As New Guid(&Ha415f6c5UI, &Hcb50, &H49d0, &H8e, &H62, &Ha8, _
			&H27, &Hb, &Hd7, &Ha2, &H6c)

		''' <summary>
		''' The 2D compass sensor type property key.
		''' </summary>
		Public Shared ReadOnly Compass2D As New Guid(&H15655cc0, &H997a, &H4d30, &H84, &Hdb, &H57, _
			&Hca, &Hba, &H36, &H48, &Hbb)

		''' <summary>
		''' The 3D compass sensor type property key.
		''' </summary>
		Public Shared ReadOnly Compass3D As New Guid(&H76b5ce0d, &H17dd, &H414d, &H93, &Ha1, &He1, _
			&H27, &Hf4, &Hb, &Hdf, &H6e)

		''' <summary>
		''' The inclinometer sensor type property key.
		''' </summary>
		Public Shared ReadOnly Inclinometer1D As New Guid(&Hb96f98c5UI, &H7a75, &H4ba7, &H94, &He9, &Hac, _
			&H86, &H8c, &H96, &H6d, &Hd8)

		''' <summary>
		''' The 2D inclinometer sensor type property key.
		''' </summary>
		Public Shared ReadOnly Inclinometer2D As New Guid(&Hab140f6dUI, &H83eb, &H4264, &Hb7, &Hb, &Hb1, _
			&H6a, &H5b, &H25, &H6a, &H1)

		''' <summary>
		''' The 3D inclinometer sensor type property key.
		''' </summary>
		Public Shared ReadOnly Inclinometer3D As New Guid(&Hb84919fbUI, &Hea85, &H4976, &H84, &H44, &H6f, _
			&H6f, &H5c, &H6d, &H31, &Hdb)

		''' <summary>
		''' The distance sensor type property key.
		''' </summary>
		Public Shared ReadOnly Distance1D As New Guid(&H5f14ab2f, &H1407, &H4306, &Ha9, &H3f, &Hb1, _
			&Hdb, &Hab, &He4, &Hf9, &Hc0)

		''' <summary>
		''' The 2D sensor type property key.
		''' </summary>
		Public Shared ReadOnly Distance2D As New Guid(&H5cf9a46c, &Ha9a2, &H4e55, &Hb6, &Ha1, &Ha0, _
			&H4a, &Haf, &Ha9, &H5a, &H92)

		''' <summary>
		''' The 3D distance sensor type property key.
		''' </summary>
		Public Shared ReadOnly Distance3D As New Guid(&Ha20cae31UI, &He25, &H4772, &H9f, &He5, &H96, _
			&H60, &H8a, &H13, &H54, &Hb2)

		''' <summary>
		''' The electrical voltage sensor type property key.
		''' </summary>
		Public Shared ReadOnly Voltage As New Guid(&Hc5484637UI, &H4fb7, &H4953, &H98, &Hb8, &Ha5, _
			&H6d, &H8a, &Ha1, &Hfb, &H1e)

		''' <summary>
		''' The electrical current sensor type property key.
		''' </summary>
		Public Shared ReadOnly Current As New Guid(&H5adc9fce, &H15a0, &H4bbe, &Ha1, &Had, &H2d, _
			&H38, &Ha9, &Hae, &H83, &H1c)

		''' <summary>
		''' The boolean switch sensor type property key.
		''' </summary>
		Public Shared ReadOnly BooleanSwitch As New Guid(&H9c7e371fUI, &H1041, &H460b, &H8d, &H5c, &H71, _
			&He4, &H75, &H2e, &H35, &Hc)

		''' <summary>
		''' The boolean switch array sensor property key.
		''' </summary>
		Public Shared ReadOnly BooleanSwitchArray As New Guid(&H545c8ba5, &Hb143, &H4545, &H86, &H8f, &Hca, _
			&H7f, &Hd9, &H86, &Hb4, &Hf6)

		''' <summary>
		''' The multiple value switch sensor type property key.
		''' </summary>       
		Public Shared ReadOnly MultivalueSwitch As New Guid(&Hb3ee4d76UI, &H37a4, &H4402, &Hb2, &H5e, &H99, _
			&Hc6, &Ha, &H77, &H5f, &Ha1)

		''' <summary>
		''' The force sensor type property key.
		''' </summary>
		Public Shared ReadOnly Force As New Guid(&Hc2ab2b02UI, &H1a1c, &H4778, &Ha8, &H1b, &H95, _
			&H4a, &H17, &H88, &Hcc, &H75)

		''' <summary>
		''' The scale sensor type property key.
		''' </summary>
		Public Shared ReadOnly Scale As New Guid(&Hc06dd92cUI, &H7feb, &H438e, &H9b, &Hf6, &H82, _
			&H20, &H7f, &Hff, &H5b, &Hb8)

		''' <summary>
		''' The pressure sensor type property key.
		''' </summary>
		Public Shared ReadOnly Pressure As New Guid(&H26d31f34, &H6352, &H41cf, &Hb7, &H93, &Hea, _
			&H7, &H13, &Hd5, &H3d, &H77)

		''' <summary>
		''' The strain sensor type property key.
		''' </summary>
		Public Shared ReadOnly Strain As New Guid(&Hc6d1ec0eUI, &H6803, &H4361, &Had, &H3d, &H85, _
			&Hbc, &Hc5, &H8c, &H6d, &H29)

		''' <summary>
		''' The Human presence sensor type property key.
		''' </summary>
		Public Shared ReadOnly HumanPresence As New Guid(&Hc138c12bUI, &Had52, &H451c, &H93, &H75, &H87, _
			&Hf5, &H18, &Hff, &H10, &Hc6)

		''' <summary>
		''' The human proximity sensor type property key.
		''' </summary>
		Public Shared ReadOnly HumanProximity As New Guid(&H5220dae9, &H3179, &H4430, &H9f, &H90, &H6, _
			&H26, &H6d, &H2a, &H34, &Hde)

		''' <summary>
		''' The touch sensor type property key.
		''' </summary>
		Public Shared ReadOnly Touch As New Guid(&H17db3018, &H6c4, &H4f7d, &H81, &Haf, &H92, _
			&H74, &Hb7, &H59, &H9c, &H27)

		''' <summary>
		''' The ambient light sensor type property key.
		''' </summary>
		Public Shared ReadOnly AmbientLight As New Guid(&H97f115c8UI, &H599a, &H4153, &H88, &H94, &Hd2, _
			&Hd1, &H28, &H99, &H91, &H8a)

		''' <summary>
		''' The RFID sensor type property key.
		''' </summary>
		Public Shared ReadOnly RfidScanner As New Guid(&H44328ef5, &H2dd, &H4e8d, &Had, &H5d, &H92, _
			&H49, &H83, &H2b, &H2e, &Hca)

		''' <summary>
		''' The bar code scanner sensor type property key.
		''' </summary>
		Public Shared ReadOnly BarcodeScanner As New Guid(&H990b3d8fUI, &H85bb, &H45ff, &H91, &H4d, &H99, _
			&H8c, &H4, &Hf3, &H72, &Hdf)
	End Class
End Namespace
