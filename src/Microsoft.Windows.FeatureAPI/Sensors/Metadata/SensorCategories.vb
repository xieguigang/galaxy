' Copyright (c) Microsoft Corporation.  All rights reserved.


Namespace Sensors
	''' <summary>
	''' Contains a list of well known sensor categories.
	''' </summary>
	Public NotInheritable Class SensorCategories
		Private Sub New()
		End Sub
		''' <summary>
		''' The sensor categoryId for all categories.
		''' </summary>
		Public Shared ReadOnly All As New Guid(&Hc317c286UI, &Hc468, &H4288, &H99, &H75, &Hd4, _
			&Hc4, &H58, &H7c, &H44, &H2c)

		''' <summary>
		''' The sensor location categoryId property key.
		''' </summary>
		Public Shared ReadOnly Location As New Guid(&Hbfa794e4UI, &Hf964, &H4fdb, &H90, &Hf6, &H51, _
			&H5, &H6b, &Hfe, &H4b, &H44)

		''' <summary>
		''' The environmental sensor cagetory property key.
		''' </summary>
		Public Shared ReadOnly Environmental As New Guid(&H323439aa, &H7f66, &H492b, &Hba, &Hc, &H73, _
			&He9, &Haa, &Ha, &H65, &Hd5)

		''' <summary>
		''' The motion sensor cagetory property key.
		''' </summary>
		Public Shared ReadOnly Motion As New Guid(&Hcd09daf1UI, &H3b2e, &H4c3d, &Hb5, &H98, &Hb5, _
			&He5, &Hff, &H93, &Hfd, &H46)

		''' <summary>
		''' The orientation sensor cagetory property key.
		''' </summary>
		Public Shared ReadOnly Orientation As New Guid(&H9e6c04b6UI, &H96fe, &H4954, &Hb7, &H26, &H68, _
			&H68, &H2a, &H47, &H3f, &H69)

		''' <summary>
		''' The mechanical sensor cagetory property key.
		''' </summary>
		Public Shared ReadOnly Mechanical As New Guid(&H8d131d68UI, &H8ef7, &H4656, &H80, &Hb5, &Hcc, _
			&Hcb, &Hd9, &H37, &H91, &Hc5)

		''' <summary>
		''' The electrical sensor cagetory property key.
		''' </summary>
		Public Shared ReadOnly Electrical As New Guid(&Hfb73fcd8UI, &Hfc4a, &H483c, &Hac, &H58, &H27, _
			&Hb6, &H91, &Hc6, &Hbe, &Hff)

		''' <summary>
		''' The bio-metric sensor cagetory property key.
		''' </summary>        
		Public Shared ReadOnly Biometric As New Guid(&Hca19690fUI, &Ha2c7, &H477d, &Ha9, &H9e, &H99, _
			&Hec, &H6e, &H2b, &H56, &H48)

		''' <summary>
		''' The light sensor cagetory property key.
		''' </summary>
		Public Shared ReadOnly Light As New Guid(&H17a665c0, &H9063, &H4216, &Hb2, &H2, &H5c, _
			&H7a, &H25, &H5e, &H18, &Hce)

		''' <summary>
		''' The scanner sensor cagetory property key.
		''' </summary>
		Public Shared ReadOnly Scanner As New Guid(&Hb000e77eUI, &Hf5b5, &H420f, &H81, &H5d, &H2, _
			&H70, &Ha7, &H26, &Hf2, &H70)
	End Class
End Namespace
