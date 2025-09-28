'Copyright (c) Microsoft Corporation.  All rights reserved.

Namespace Internal

    ''' <summary>
    ''' HRESULT Wrapper    
    ''' </summary>    
    Public Enum HResult

        ''' <summary>     
        ''' S_OK          
        ''' </summary>    
        Ok = &H0

        ''' <summary>
        ''' S_FALSE
        ''' </summary>        
        [False] = &H1

        ''' <summary>
        ''' E_INVALIDARG
        ''' </summary>
        InvalidArguments = &H80070057UI

        ''' <summary>
        ''' E_OUTOFMEMORY
        ''' </summary>
        OutOfMemory = &H8007000EUI

        ''' <summary>
        ''' E_NOINTERFACE
        ''' </summary>
        NoInterface = &H80004002UI

        ''' <summary>
        ''' E_FAIL
        ''' </summary>
        Fail = &H80004005UI

        ''' <summary>
        ''' E_ELEMENTNOTFOUND
        ''' </summary>
        ElementNotFound = &H80070490UI

        ''' <summary>
        ''' TYPE_E_ELEMENTNOTFOUND
        ''' </summary>
        TypeElementNotFound = &H8002802BUI

        ''' <summary>
        ''' NO_OBJECT
        ''' </summary>
        NoObject = &H800401E5UI

        ''' <summary>
        ''' Win32 Error code: ERROR_CANCELLED
        ''' </summary>
        Win32ErrorCanceled = 1223

        ''' <summary>
        ''' ERROR_CANCELLED
        ''' </summary>
        Canceled = &H800704C7UI

        ''' <summary>
        ''' The requested resource is in use
        ''' </summary>
        ResourceInUse = &H800700AAUI

        ''' <summary>
        ''' The requested resources is read-only.
        ''' </summary>
        AccessDenied = &H80030005UI
    End Enum

    ''' <summary>
    ''' Provide Error Message Helper Methods.
    ''' This is intended for Library Internal use only.
    ''' </summary>
    Public Module CoreErrorHelper

        ''' <summary>
        ''' This is intended for Library Internal use only.
        ''' </summary>
        Private Const FacilityWin32 As Integer = 7

        ''' <summary>
        ''' This is intended for Library Internal use only.
        ''' </summary>
        Public Const Ignored As Integer = HResult.Ok

        ''' <summary>
        ''' This is intended for Library Internal use only.
        ''' </summary>
        ''' <param name="win32ErrorCode">The Windows API error code.</param>
        ''' <returns>The equivalent HRESULT.</returns>
        Public Function HResultFromWin32(win32ErrorCode As Integer) As Integer
            If win32ErrorCode > 0 Then
                win32ErrorCode = CInt((CUInt(win32ErrorCode) And &HFFFF) Or (FacilityWin32 << 16) Or &H80000000UI)
            End If
            Return win32ErrorCode

        End Function

        ''' <summary>
        ''' This is intended for Library Internal use only.
        ''' </summary>
        ''' <param name="result">The error code.</param>
        ''' <returns>True if the error code indicates success.</returns>
        Public Function Succeeded(result As Integer) As Boolean
            Return result >= 0
        End Function

        ''' <summary>
        ''' This is intended for Library Internal use only.
        ''' </summary>
        ''' <param name="result">The error code.</param>
        ''' <returns>True if the error code indicates success.</returns>
        Public Function Succeeded(result As HResult) As Boolean
            Return Succeeded(CInt(result))
        End Function

        ''' <summary>
        ''' This is intended for Library Internal use only.
        ''' </summary>
        ''' <param name="result">The error code.</param>
        ''' <returns>True if the error code indicates failure.</returns>
        Public Function Failed(result As HResult) As Boolean
            Return Not Succeeded(result)
        End Function

        ''' <summary>
        ''' This is intended for Library Internal use only.
        ''' </summary>
        ''' <param name="result">The error code.</param>
        ''' <returns>True if the error code indicates failure.</returns>
        Public Function Failed(result As Integer) As Boolean
            Return Not Succeeded(result)
        End Function

        ''' <summary>
        ''' This is intended for Library Internal use only.
        ''' </summary>
        ''' <param name="result">The COM error code.</param>
        ''' <param name="win32ErrorCode">The Win32 error code.</param>
        ''' <returns>Inticates that the Win32 error code corresponds to the COM error code.</returns>
        Public Function Matches(result As Integer, win32ErrorCode As Integer) As Boolean
            Return (result = HResultFromWin32(win32ErrorCode))
        End Function
    End Module
End Namespace
