Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.InteropServices

Namespace Shell
    Module IntPtrExtensions

        <System.Runtime.CompilerServices.Extension> Public Function MarshalAs(Of T)(ptr As IntPtr) As T
            Return DirectCast(Marshal.PtrToStructure(ptr, GetType(T)), T)
        End Function
    End Module
End Namespace
