' Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Runtime.InteropServices

Namespace ExtendedLinguisticServices

	Friend NotInheritable Class Win32NativeMethods
		Private Sub New()
		End Sub
		<DllImport("elscore.dll", EntryPoint := "MappingGetServices", SetLastError := True, CharSet := CharSet.Unicode)> _
		Friend Shared Function MappingGetServices(ByRef enumOptions As Win32EnumOptions, ByRef services As IntPtr, ByRef servicesCount As UInteger) As UInteger
		End Function

		' For performance reasons, if we need to pass NULL as the MappingEnumOptions
		<DllImport("elscore.dll", EntryPoint := "MappingGetServices", SetLastError := True, CharSet := CharSet.Unicode)> _
		Friend Shared Function MappingGetServices(enumOptions As IntPtr, ByRef services As IntPtr, ByRef servicesCount As UInteger) As UInteger
		End Function

		<DllImport("elscore.dll", EntryPoint := "MappingRecognizeText", SetLastError := True, CharSet := CharSet.Unicode)> _
		Friend Shared Function MappingRecognizeText(service As IntPtr, text As IntPtr, length As UInteger, index As UInteger, options As IntPtr, ByRef bag As Win32PropertyBag) As UInteger
		End Function

		<DllImport("elscore.dll", EntryPoint := "MappingDoAction", SetLastError := True, CharSet := CharSet.Unicode)> _
		Friend Shared Function MappingDoAction(ByRef bag As Win32PropertyBag, rangeIndex As UInteger, action As String) As UInteger
		End Function

		<DllImport("elscore.dll", EntryPoint := "MappingFreePropertyBag", SetLastError := True, CharSet := CharSet.Unicode)> _
		Friend Shared Function MappingFreePropertyBag(ByRef bag As Win32PropertyBag) As UInteger
		End Function

		<DllImport("elscore.dll", EntryPoint := "MappingFreeServices", SetLastError := True, CharSet := CharSet.Unicode)> _
		Friend Shared Function MappingFreeServices(pServiceInfo As IntPtr) As UInteger
		End Function

		<DllImport("elscore.dll", EntryPoint := "MappingFreeServices", SetLastError := True, CharSet := CharSet.Unicode)> _
		Friend Shared Sub MappingFreeServicesVoid(pServiceInfo As IntPtr)
		End Sub
	End Class

End Namespace
