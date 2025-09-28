'Copyright (c) Microsoft Corporation.  All rights reserved.

Namespace Shell
	''' <summary>
	''' Specifies the categories for known folders.
	''' </summary>
	Public Enum FolderCategory
		''' <summary>
		''' The folder category is not specified.
		''' </summary>
		None = &H0
		''' <summary>
		''' The folder is a virtual folder. Virtual folders are not part 
		''' of the file system. For example, Control Panel and 
		''' Printers are virtual folders. A number of properties 
		''' such as folder path and redirection do not apply to this category.
		''' </summary>
		Virtual = &H1
		''' <summary>
		''' The folder is fixed. Fixed file system folders are not 
		''' managed by the Shell and are usually given a permanent 
		''' path when the system is installed. For example, the 
		''' Windows and Program Files folders are fixed folders. 
		''' A number of properties such as redirection do not apply 
		''' to this category.
		''' </summary>
		Fixed = &H2
		''' <summary>
		''' The folder is a common folder. Common folders are 
		''' used for sharing data and settings 
		''' accessible by all users of a system. For example, 
		''' all users share a common Documents folder as well 
		''' as their per-user Documents folder.
		''' </summary>
		Common = &H3
		''' <summary>
		''' Each user has their own copy of the folder. Per-user folders 
		''' are those stored under each user's profile and 
		''' accessible only by that user.
		''' </summary>
		PerUser = &H4
	End Enum
End Namespace
