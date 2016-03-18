' Copyright (c) Microsoft Corporation.  All rights reserved.


Namespace Shell
	''' <summary>
	''' Defines the read-only properties for default shell icon sizes.
	''' </summary>
	Public NotInheritable Class DefaultIconSize
		Private Sub New()
		End Sub
		''' <summary>
		''' The small size property for a 16x16 pixel Shell Icon.
		''' </summary>
		Public Shared ReadOnly Small As New System.Windows.Size(16, 16)

		''' <summary>
		''' The medium size property for a 32x32 pixel Shell Icon.
		''' </summary>
		Public Shared ReadOnly Medium As New System.Windows.Size(32, 32)

		''' <summary>
		''' The large size property for a 48x48 pixel Shell Icon.
		''' </summary>
		Public Shared ReadOnly Large As New System.Windows.Size(48, 48)

		''' <summary>
		''' The extra-large size property for a 256x256 pixel Shell Icon.
		''' </summary>
		Public Shared ReadOnly ExtraLarge As New System.Windows.Size(256, 256)

		''' <summary>
		''' The maximum size for a Shell Icon, 256x256 pixels.
		''' </summary>
		Public Shared ReadOnly Maximum As New System.Windows.Size(256, 256)

	End Class

	''' <summary>
	''' Defines the read-only properties for default shell thumbnail sizes.
	''' </summary>
	Public NotInheritable Class DefaultThumbnailSize
		Private Sub New()
		End Sub
		''' <summary>
		''' Gets the small size property for a 32x32 pixel Shell Thumbnail.
		''' </summary>
		Public Shared ReadOnly Small As New System.Windows.Size(32, 32)

		''' <summary>
		''' Gets the medium size property for a 96x96 pixel Shell Thumbnail.
		''' </summary>
		Public Shared ReadOnly Medium As New System.Windows.Size(96, 96)

		''' <summary>
		''' Gets the large size property for a 256x256 pixel Shell Thumbnail.
		''' </summary>
		Public Shared ReadOnly Large As New System.Windows.Size(256, 256)

		''' <summary>
		''' Gets the extra-large size property for a 1024x1024 pixel Shell Thumbnail.
		''' </summary>
		Public Shared ReadOnly ExtraLarge As New System.Windows.Size(1024, 1024)

		''' <summary>
		''' Maximum size for the Shell Thumbnail, 1024x1024 pixels.
		''' </summary>
		Public Shared ReadOnly Maximum As New System.Windows.Size(1024, 1024)
	End Class
End Namespace
