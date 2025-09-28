'Copyright (c) Microsoft Corporation.  All rights reserved.


Namespace Shell
	''' <summary>
	''' Contains special retrieval options for known folders.
	''' </summary>
	Friend Enum RetrievalOptions
		None = 0
		Create = &H8000
		DontVerify = &H4000
		DontUnexpand = &H2000
		NoAlias = &H1000
		Init = &H800
		DefaultPath = &H400
		NotParentRelative = &H200
	End Enum
End Namespace
