'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Collections.Generic
Imports System.Runtime.InteropServices
Imports Microsoft.Windows.Internal

Namespace Shell
    Friend Class EnumUnknownClass
        Implements IEnumUnknown
        Private conditionList As New List(Of ICondition)()
        Private current As Integer = -1

        Friend Sub New(conditions As ICondition())
            conditionList.AddRange(conditions)
        End Sub

#Region "IEnumUnknown Members"

        Public Function [Next](requestedNumber As UInteger, ByRef buffer As IntPtr, ByRef fetchedNumber As UInteger) As HResult Implements IEnumUnknown.[Next]
            current += 1

            If current < conditionList.Count Then
                buffer = Marshal.GetIUnknownForObject(conditionList(current))
                fetchedNumber = 1
                Return HResult.Ok
            End If

            Return HResult.[False]
        End Function

        Public Function Skip(number As UInteger) As HResult Implements IEnumUnknown.Skip
            Dim temp As Integer = current + CInt(number)

            If temp > (conditionList.Count - 1) Then
                Return HResult.[False]
            End If

            current = temp
            Return HResult.Ok
        End Function

        Public Function Reset() As HResult Implements IEnumUnknown.Reset
            current = -1
            Return HResult.Ok
        End Function

        Public Function Clone(ByRef result As IEnumUnknown) As HResult Implements IEnumUnknown.Clone
            result = New EnumUnknownClass(Me.conditionList.ToArray())
            Return HResult.Ok
        End Function

#End Region
    End Class
End Namespace
