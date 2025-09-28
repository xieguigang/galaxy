'Copyright (c) Microsoft Corporation.  All rights reserved.

Namespace Shell
	''' <summary>
	''' Specifies behaviors for known folders.
	''' </summary>
	<Flags> _
	Public Enum DefinitionOptions
		''' <summary>
		''' No behaviors are defined.
		''' </summary>
		None = &H0
		''' <summary>
		''' Prevents a per-user known folder from being 
		''' redirected to a network location.
		''' </summary>
		LocalRedirectOnly = &H2

		''' <summary>
		''' The known folder can be roamed through PC-to-PC synchronization.
		''' </summary>
		Roamable = &H4

		''' <summary>
		''' Creates the known folder when the user first logs on.
		''' </summary>
		Precreate = &H8
	End Enum
End Namespace
