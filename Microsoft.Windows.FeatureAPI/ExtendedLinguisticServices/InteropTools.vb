' Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Runtime.InteropServices
Imports System.Collections
Imports System.Collections.Generic

Namespace ExtendedLinguisticServices

	Friend NotInheritable Class InteropTools
		Private Sub New()
		End Sub
        Friend Shared ReadOnly SizeOfGuid As IntPtr = New IntPtr(Marshal.SizeOf(GetType(Guid)))
        Friend Shared ReadOnly SizeOfWin32EnumOptions As IntPtr = New IntPtr(Marshal.SizeOf(GetType(Win32EnumOptions)))
        Friend Shared ReadOnly SizeOfWin32Options As IntPtr = New IntPtr(Marshal.SizeOf(GetType(Win32Options)))
        Friend Shared ReadOnly SizeOfService As UInt64 = CType(Marshal.SizeOf(GetType(Win32Service)), UInt64)
        Friend Shared ReadOnly SizeOfWin32PropertyBag As IntPtr = New IntPtr(Marshal.SizeOf(GetType(Win32PropertyBag)))
        Friend Shared ReadOnly SizeOfWin32DataRange As UInt64 = CType(Marshal.SizeOf(GetType(Win32DataRange)), UInt64)
        Friend Shared ReadOnly OffsetOfGuidInService As UInt64 = CType(Marshal.OffsetOf(GetType(Win32Service), "_guid"), UInt64)
        Friend Shared ReadOnly TypeOfGuid As Type = GetType(Guid)

		Friend Shared Function Unpack(Of T As Structure)(value As IntPtr) As T
			If value = IntPtr.Zero Then
				Return Nothing
			End If

			Return DirectCast(Marshal.PtrToStructure(value, GetType(T)), T)
		End Function

		Friend Shared Function Pack(Of T As Structure)(ByRef data As T) As IntPtr
			Dim pointer As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(GetType(T)))
			Marshal.StructureToPtr(data, pointer, False)
			Return pointer
		End Function

		Friend Shared Sub Free(Of T As Structure)(ByRef pointer As IntPtr)
			If pointer <> IntPtr.Zero Then
				' Thus we clear the strings previously allocated to the struct:
				Marshal.StructureToPtr(Nothing, pointer, True)
				' Here we clean up the memory for the struct itself:
				Marshal.FreeHGlobal(pointer)
				' This is to avoid calling freeing this pointer multiple times:
				pointer = IntPtr.Zero
			End If
		End Sub

		Friend Shared Function UnpackStringArray(strPtr As IntPtr, count As UInteger) As String()
			If strPtr = IntPtr.Zero AndAlso count <> 0 Then
				Throw New LinguisticException(LinguisticException.InvalidArgs)
			End If

            Dim retVal As String() = New String(CInt(count) - 1) {}

            Dim offset As Integer = 0
            For i As Integer = 0 To retVal.Length - 1
                retVal(i) = Marshal.PtrToStringUni(Marshal.ReadIntPtr(strPtr, offset))
                offset += IntPtr.Size
            Next

            Return retVal
		End Function

	End Class

End Namespace
