'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Runtime.InteropServices
Imports Microsoft.Windows.Internal

Namespace Shell.PropertySystem
	Friend NotInheritable Class PropertySystemNativeMethods
		Private Sub New()
		End Sub
		#Region "Property Definitions"

		Friend Enum RelativeDescriptionType
			General
			[Date]
			Size
			Count
			Revision
			Length
			Duration
			Speed
			Rate
			Rating
			Priority
		End Enum

		#End Region

		#Region "Property System Helpers"

		<DllImport("propsys.dll", CharSet := CharSet.Unicode, SetLastError := True)> _
		Friend Shared Function PSGetNameFromPropertyKey(ByRef propkey As PropertyKey, <Out, MarshalAs(UnmanagedType.LPWStr)> ByRef ppszCanonicalName As String) As Integer
		End Function

		<DllImport("propsys.dll", CharSet := CharSet.Unicode, SetLastError := True)> _
		Friend Shared Function PSGetPropertyDescription(ByRef propkey As PropertyKey, ByRef riid As Guid, <Out, MarshalAs(UnmanagedType.[Interface])> ByRef ppv As IPropertyDescription) As HResult
		End Function

		<DllImport("propsys.dll", CharSet := CharSet.Unicode, SetLastError := True)> _
		Friend Shared Function PSGetPropertyKeyFromName(<[In], MarshalAs(UnmanagedType.LPWStr)> pszCanonicalName As String, ByRef propkey As PropertyKey) As Integer
		End Function

		<DllImport("propsys.dll", CharSet := CharSet.Unicode, SetLastError := True)> _
		Friend Shared Function PSGetPropertyDescriptionListFromString(<[In], MarshalAs(UnmanagedType.LPWStr)> pszPropList As String, <[In]> ByRef riid As Guid, ByRef ppv As IPropertyDescriptionList) As Integer
		End Function



		#End Region
	End Class
End Namespace
