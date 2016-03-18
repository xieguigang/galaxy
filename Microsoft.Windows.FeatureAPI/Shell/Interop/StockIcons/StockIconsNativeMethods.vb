'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Runtime.InteropServices
Imports Microsoft.Windows.Internal

Namespace Shell
	Friend NotInheritable Class StockIconsNativeMethods
		Private Sub New()
		End Sub
		#Region "StockIcon declarations"

		''' <summary>
		''' Specifies options for the appearance of the 
		''' stock icon.
		''' </summary>
		<Flags> _
		Friend Enum StockIconOptions
			''' <summary>
			''' Retrieve the small version of the icon, as specified by  
			''' SM_CXICON and SM_CYICON system metrics.
			''' </summary>
			Large = &H0

			''' <summary>
			''' Retrieve the small version of the icon, as specified by  
			''' SM_CXSMICON and SM_CYSMICON system metrics.
			''' </summary>
			Small = &H1

			''' <summary>
			''' Retrieve the shell-sized icons (instead of the 
			''' size specified by the system metrics). 
			''' </summary>
			ShellSize = &H4

			''' <summary>
			''' Specified that the hIcon member of the SHSTOCKICONINFO 
			''' structure receives a handle to the specified icon.
			''' </summary>
			Handle = &H100

			''' <summary>
			''' Specifies that the iSysImageImage member of the SHSTOCKICONINFO 
			''' structure receives the index of the specified 
			''' icon in the system imagelist.
			''' </summary>
			SystemIndex = &H4000

			''' <summary>
			''' Adds the link overlay to the icon.
			''' </summary>
			LinkOverlay = &H8000

			'''<summary>
			''' Adds the system highlight color to the icon.
			''' </summary>
			Selected = &H10000
		End Enum

		<StructLayoutAttribute(LayoutKind.Sequential, CharSet := CharSet.Unicode)> _
		Friend Structure StockIconInfo
			Friend StuctureSize As UInt32
			Friend Handle As IntPtr
			Friend ImageIndex As Int32
			Friend Identifier As Int32
			<MarshalAs(UnmanagedType.ByValTStr, SizeConst := 260)> _
			Friend Path As String
		End Structure

		<PreserveSig> _
		<DllImport("Shell32.dll", CharSet := CharSet.Unicode, ExactSpelling := True, SetLastError := False)> _
		Friend Shared Function SHGetStockIconInfo(identifier As StockIconIdentifier, flags As StockIconOptions, ByRef info As StockIconInfo) As HResult
		End Function

		#End Region
	End Class
End Namespace
