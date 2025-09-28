'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Security.Permissions

Namespace Internal

    ''' <summary>
    ''' Safe Region Handle
    ''' </summary>
    Public Class SafeRegionHandle : Inherits ZeroInvalidHandle

        ''' <summary>
        ''' Release the handle
        ''' </summary>
        ''' <returns>true if handled is release successfully, false otherwise</returns>
        Protected Overrides Function ReleaseHandle() As Boolean
            If CoreNativeMethods.DeleteObject(handle) Then
                Return True
            Else
                Return False
            End If
        End Function
    End Class
End Namespace
