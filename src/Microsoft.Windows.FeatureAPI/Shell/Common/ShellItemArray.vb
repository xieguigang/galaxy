'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Collections.Generic
Imports Microsoft.Windows.Internal
Imports Microsoft.Windows.Shell.PropertySystem

Namespace Shell
    Friend Class ShellItemArray
        Implements IShellItemArray
        Private shellItemsList As New List(Of IShellItem)()

        Friend Sub New(shellItems As IShellItem())
            shellItemsList.AddRange(shellItems)
        End Sub

#Region "IShellItemArray Members"

        Public Function BindToHandler(pbc As IntPtr, ByRef rbhid As Guid, ByRef riid As Guid, ByRef ppvOut As IntPtr) As HResult Implements IShellItemArray.BindToHandler
            Throw New NotSupportedException()
        End Function

        Public Function GetPropertyStore(Flags As Integer, ByRef riid As Guid, ByRef ppv As IntPtr) As HResult Implements IShellItemArray.GetPropertyStore
            Throw New NotSupportedException()
        End Function

        Public Function GetPropertyDescriptionList(ByRef keyType As PropertyKey, ByRef riid As Guid, ByRef ppv As IntPtr) As HResult Implements IShellItemArray.GetPropertyDescriptionList
            Throw New NotSupportedException()
        End Function

        Public Function GetAttributes(dwAttribFlags As ShellNativeMethods.ShellItemAttributeOptions, sfgaoMask As ShellNativeMethods.ShellFileGetAttributesOptions, ByRef psfgaoAttribs As ShellNativeMethods.ShellFileGetAttributesOptions) As HResult Implements IShellItemArray.GetAttributes
            Throw New NotSupportedException()
        End Function

        Public Function GetCount(ByRef pdwNumItems As UInteger) As HResult Implements IShellItemArray.GetCount
            pdwNumItems = CUInt(shellItemsList.Count)
            Return HResult.Ok
        End Function

        Public Function GetItemAt(dwIndex As UInteger, ByRef ppsi As IShellItem) As HResult Implements IShellItemArray.GetItemAt
            Dim index As Integer = CInt(dwIndex)

            If index < shellItemsList.Count Then
                ppsi = shellItemsList(index)
                Return HResult.Ok
            Else
                ppsi = Nothing
                Return HResult.Fail
            End If
        End Function

        Public Function EnumItems(ByRef ppenumShellItems As IntPtr) As HResult Implements IShellItemArray.EnumItems
            Throw New NotSupportedException()
        End Function

#End Region
    End Class
End Namespace
