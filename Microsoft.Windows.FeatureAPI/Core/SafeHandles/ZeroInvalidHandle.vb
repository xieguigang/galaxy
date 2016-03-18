'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Runtime.InteropServices
Imports System.Security.Permissions

Namespace Internal
	''' <summary>
	''' Base class for Safe handles with Null IntPtr as invalid
	''' </summary>
	Public MustInherit Class ZeroInvalidHandle
		Inherits SafeHandle
		''' <summary>
		''' Default constructor
		''' </summary>
		Protected Sub New()
			MyBase.New(IntPtr.Zero, True)
		End Sub

		''' <summary>
		''' Determines if this is a valid handle
		''' </summary>
		Public Overrides ReadOnly Property IsInvalid() As Boolean
			Get
				Return handle = IntPtr.Zero
			End Get
		End Property

	End Class
End Namespace

