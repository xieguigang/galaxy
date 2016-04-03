Imports System.Runtime.InteropServices

Namespace API.Dwm

    ''' <summary>
    ''' Metro UI (Zune like) Interface (form)
    ''' lipinho, 27 Dec 2010 CPOL
    '''
    ''' The future of Windows Interfaces is probably the Zune-like ones, with a borderless form and some controls inside of it.
    ''' The problem is: if you're using WindowsForm, creating that borderless form with shadows and resizing stuff isn't as easy as it seems.
    ''' This article will show you how to create those forms using a bit of DWM and some other Windows APIs.
    '''
    ''' (http://www.codeproject.com/Articles/138661/Metro-UI-Zune-like-Interface-form)
    ''' </summary>
    Public Module DwmAPI

        ' Fields
        Public Const DWM_BB_BLURREGION As Integer = 2
        Public Const DWM_BB_ENABLE As Integer = 1
        Public Const DWM_BB_TRANSITIONONMAXIMIZED As Integer = 4
        Public Const DWM_COMPOSED_EVENT_BASE_NAME As String = "DwmComposedEvent_"
        Public Const DWM_COMPOSED_EVENT_NAME_FORMAT As String = "%s%d"
        Public Const DWM_COMPOSED_EVENT_NAME_MAX_LENGTH As Integer = &H40
        Public Const DWM_FRAME_DURATION_DEFAULT As Integer = -1
        Public Const DWM_TNP_OPACITY As Integer = 4
        Public Const DWM_TNP_RECTDESTINATION As Integer = 1
        Public Const DWM_TNP_RECTSOURCE As Integer = 2
        Public Const DWM_TNP_SOURCECLIENTAREAONLY As Integer = &H10
        Public Const DWM_TNP_VISIBLE As Integer = 8
        Public Const WM_DWMCOMPOSITIONCHANGED As Integer = &H31E

        Public ReadOnly Property DwmApiAvailable As Boolean = (Environment.OSVersion.Version.Major >= 6)

        ' Methods
        <DllImport("dwmapi.dll")>
        Public Function DwmDefWindowProc(hwnd As IntPtr, msg As Integer, wParam As IntPtr, lParam As IntPtr, ByRef result As IntPtr) As Integer
        End Function

        <DllImport("dwmapi.dll")>
        Public Function DwmEnableComposition(fEnable As Integer) As Integer
        End Function

        <DllImport("dwmapi.dll")>
        Public Function DwmEnableMMCSS(fEnableMMCSS As Integer) As Integer
        End Function

        <DllImport("dwmapi.dll")>
        Public Function DwmExtendFrameIntoClientArea(hdc As IntPtr, ByRef marInset As MARGINS) As Integer
        End Function

        <DllImport("dwmapi.dll")>
        Public Function DwmGetColorizationColor(ByRef pcrColorization As Integer, ByRef pfOpaqueBlend As Integer) As Integer
        End Function

        <DllImport("dwmapi.dll")>
        Public Function DwmGetCompositionTimingInfo(hwnd As IntPtr, ByRef pTimingInfo As DWM_TIMING_INFO) As Integer
        End Function

        <DllImport("dwmapi.dll")>
        Public Function DwmGetWindowAttribute(hwnd As IntPtr, dwAttribute As Integer, pvAttribute As IntPtr, cbAttribute As Integer) As Integer
        End Function

        <DllImport("dwmapi.dll")>
        Public Function DwmIsCompositionEnabled(ByRef pfEnabled As Integer) As Integer
        End Function

        <DllImport("dwmapi.dll")>
        Public Function DwmModifyPreviousDxFrameDuration(hwnd As IntPtr, cRefreshes As Integer, fRelative As Integer) As Integer
        End Function

        <DllImport("dwmapi.dll")>
        Public Function DwmQueryThumbnailSourceSize(hThumbnail As IntPtr, ByRef pSize As Size) As Integer
        End Function

        <DllImport("dwmapi.dll")>
        Public Function DwmRegisterThumbnail(hwndDestination As IntPtr, hwndSource As IntPtr, ByRef pMinimizedSize As Size, ByRef phThumbnailId As IntPtr) As Integer
        End Function

        <DllImport("dwmapi.dll")>
        Public Function DwmSetDxFrameDuration(hwnd As IntPtr, cRefreshes As Integer) As Integer
        End Function

        <DllImport("dwmapi.dll")>
        Public Function DwmSetPresentParameters(hwnd As IntPtr, ByRef pPresentParams As DWM_PRESENT_PARAMETERS) As Integer
        End Function

        <DllImport("dwmapi.dll")>
        Public Function DwmSetWindowAttribute(hwnd As IntPtr, dwAttribute As Integer, pvAttribute As IntPtr, cbAttribute As Integer) As Integer
        End Function

        <DllImport("dwmapi.dll")>
        Public Function DwmUnregisterThumbnail(hThumbnailId As IntPtr) As Integer
        End Function

        <DllImport("dwmapi.dll")>
        Public Function DwmUpdateThumbnailProperties(hThumbnailId As IntPtr, ByRef ptnProperties As DWM_THUMBNAIL_PROPERTIES) As Integer
        End Function

        ''' <summary>
        ''' Do Not Draw The Caption (Text)
        ''' </summary>
        Public WTNCA_NODRAWCAPTION As UInteger = &H1
        ''' <summary>
        ''' Do Not Draw the Icon
        ''' </summary>
        Public WTNCA_NODRAWICON As UInteger = &H2
        ''' <summary>
        ''' Do Not Show the System Menu
        ''' </summary>
        Public WTNCA_NOSYSMENU As UInteger = &H4
        ''' <summary>
        ''' Do Not Mirror the Question mark Symbol
        ''' </summary>
        Public WTNCA_NOMIRRORHELP As UInteger = &H8

        ''' <summary>
        ''' Set The Window's Theme Attributes
        ''' </summary>
        ''' <param name="hWnd">The Handle to the Window</param>
        ''' <param name="wtype">What Type of Attributes</param>
        ''' <param name="attributes">The Attributes to Add/Remove</param>
        ''' <param name="size">The Size of the Attributes Struct</param>
        ''' <returns>If The Call Was Successful or Not</returns>
        <DllImport("UxTheme.dll")>
        Public Function SetWindowThemeAttribute(hWnd As IntPtr, wtype As WindowThemeAttributeType, ByRef attributes As WTA_OPTIONS, size As UInteger) As Integer
        End Function
    End Module
End Namespace