'Copyright (c) Microsoft Corporation.  All rights reserved.

Namespace Shell
	''' <summary>
	''' Specifies the redirection capabilities for known folders. 
	''' </summary>
	Public Enum RedirectionCapability
		''' <summary>
		''' Redirection capability is unknown.
		''' </summary>
		None = &H0
		''' <summary>
		''' The known folder can be redirected.
		''' </summary>
		AllowAll = &Hff
		''' <summary>
		''' The known folder can be redirected. 
		''' Currently, redirection exists only for 
		'''  common and user folders; fixed and virtual folders 
		''' cannot be redirected.
		''' </summary>       
		Redirectable = &H1
		''' <summary>
		''' Redirection is not allowed.
		''' </summary>
		DenyAll = &Hfff00
		''' <summary>
		''' The folder cannot be redirected because it is 
		''' already redirected by group policy.
		''' </summary>
		DenyPolicyRedirected = &H100
		''' <summary>
		''' The folder cannot be redirected because the policy 
		''' prohibits redirecting this folder.
		''' </summary>
		DenyPolicy = &H200
		''' <summary>
		''' The folder cannot be redirected because the calling 
		''' application does not have sufficient permissions.
		''' </summary>
		DenyPermissions = &H400
	End Enum
End Namespace
