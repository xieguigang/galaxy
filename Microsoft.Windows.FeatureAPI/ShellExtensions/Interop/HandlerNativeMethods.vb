Imports System.Runtime.InteropServices
Imports System.Runtime.InteropServices.ComTypes
Imports Microsoft.Windows.Internal
Imports Microsoft.Windows.Shell
Imports Microsoft.Windows.Shell.Interop

Namespace ShellExtensions.Interop
    Friend NotInheritable Class HandlerNativeMethods
        Private Sub New()
        End Sub
        <DllImport("user32.dll")>
        Friend Shared Function SetParent(hWndChild As IntPtr, hWndNewParent As IntPtr) As IntPtr
        End Function

        <DllImport("user32.dll")>
        Friend Shared Function GetFocus() As IntPtr
        End Function

        <DllImport("user32.dll")>
        Friend Shared Sub SetWindowPos(hWnd As IntPtr, hWndInsertAfter As IntPtr, x As Integer, y As Integer, cx As Integer, cy As Integer,
            flags As SetWindowPositionOptions)
        End Sub

        Friend Shared ReadOnly IPreviewHandlerGuid As New Guid("8895b1c6-b41f-4c1c-a562-0d564250836f")
        Friend Shared ReadOnly IThumbnailProviderGuid As New Guid("e357fccd-a995-4576-b01f-234630154e96")

        Friend Shared ReadOnly IInitializeWithFileGuid As New Guid("b7d14566-0509-4cce-a71f-0a554233bd9b")
        Friend Shared ReadOnly IInitializeWithStreamGuid As New Guid("b824b49d-22ac-4161-ac8a-9916e8fa3f7f")
        Friend Shared ReadOnly IInitializeWithItemGuid As New Guid("7f73be3f-fb79-493c-a6c7-7ee14e245841")

        Friend Shared ReadOnly IMarshalGuid As New Guid("00000003-0000-0000-C000-000000000046")
    End Class

#Region "Interfaces"

    ''' <summary>
    ''' ComVisible interface for native IThumbnailProvider
    ''' </summary>
    <ComImport>
    <Guid("e357fccd-a995-4576-b01f-234630154e96")>
    <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Interface IThumbnailProvider
        ''' <summary>
        ''' Gets a pointer to a bitmap to display as a thumbnail
        ''' </summary>
        ''' <param name="squareLength"></param>
        ''' <param name="bitmapHandle"></param>
        ''' <param name="bitmapType"></param>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId:="2#"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId:="1#")>
        Sub GetThumbnail(squareLength As UInteger, <Out> ByRef bitmapHandle As IntPtr, <Out> ByRef bitmapType As UInt32)
    End Interface

    ''' <summary>
    ''' Provides means by which to initialize with a file.
    ''' </summary>
    <ComImport>
    <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    <Guid("b7d14566-0509-4cce-a71f-0a554233bd9b")>
    Interface IInitializeWithFile
        ''' <summary>
        ''' Initializes with a file.
        ''' </summary>
        ''' <param name="filePath"></param>
        ''' <param name="fileMode"></param>
        Sub Initialize(<MarshalAs(UnmanagedType.LPWStr)> filePath As String, fileMode As AccessModes)
    End Interface

    ''' <summary>
    ''' Provides means by which to initialize with a stream.
    ''' </summary>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")>
    <ComImport>
    <Guid("b824b49d-22ac-4161-ac8a-9916e8fa3f7f")>
    <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Interface IInitializeWithStream
        ''' <summary>
        ''' Initializes with a stream.
        ''' </summary>
        ''' <param name="stream"></param>
        ''' <param name="fileMode"></param>
        Sub Initialize(stream As IStream, fileMode As AccessModes)
    End Interface

    ''' <summary>
    ''' Provides means by which to initialize with a ShellObject
    ''' </summary>
    <ComImport>
    <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    <Guid("7f73be3f-fb79-493c-a6c7-7ee14e245841")>
    Interface IInitializeWithItem
        ''' <summary>
        ''' Initializes with ShellItem
        ''' </summary>
        ''' <param name="shellItem"></param>
        ''' <param name="accessMode"></param>
        Sub Initialize(shellItem As IShellItem, accessMode As AccessModes)
    End Interface

    <ComImport>
    <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    <Guid("fc4801a3-2ba9-11cf-a229-00aa003d7352")>
    Interface IObjectWithSite
        Sub SetSite(<[In], MarshalAs(UnmanagedType.IUnknown)> pUnkSite As Object)
        Sub GetSite(ByRef riid As Guid, <MarshalAs(UnmanagedType.IUnknown)> ByRef ppvSite As Object)
    End Interface

    <ComImport>
    <Guid("00000114-0000-0000-C000-000000000046")>
    <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Interface IOleWindow
        Sub GetWindow(ByRef phwnd As IntPtr)
        Sub ContextSensitiveHelp(<MarshalAs(UnmanagedType.Bool)> fEnterMode As Boolean)
    End Interface

    <ComImport>
    <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    <Guid("8895b1c6-b41f-4c1c-a562-0d564250836f")>
    Interface IPreviewHandler
        Sub SetWindow(hwnd As IntPtr, ByRef rect As NativeRect)
        Sub SetRect(ByRef rect As NativeRect)
        Sub DoPreview()
        Sub Unload()
        Sub SetFocus()
        Sub QueryFocus(ByRef phwnd As IntPtr)
        <PreserveSig>
        Function TranslateAccelerator(ByRef pmsg As Message) As HResult
    End Interface

    <ComImport>
    <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    <Guid("fec87aaf-35f9-447a-adb7-20234491401a")>
    Interface IPreviewHandlerFrame
        Sub GetWindowContext(pinfo As IntPtr)
        <PreserveSig>
        Function TranslateAccelerator(ByRef pmsg As Message) As HResult
    End Interface

    <ComImport>
    <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    <Guid("8327b13c-b63f-4b24-9b8a-d010dcc3f599")>
    Interface IPreviewHandlerVisuals
        Sub SetBackgroundColor(color As NativeColorRef)
        Sub SetFont(ByRef plf As LogFont)
        Sub SetTextColor(color As NativeColorRef)
    End Interface
#End Region

#Region "Structs"

    ''' <summary>
    ''' Class for marshaling to native LogFont struct
    ''' </summary>
    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)>
    Public Class LogFont
        ''' <summary>
        ''' Font height
        ''' </summary>
        Public Property Height() As Integer
            Get
                Return m_Height
            End Get
            Set
                m_Height = Value
            End Set
        End Property
        Private m_Height As Integer

        ''' <summary>
        ''' Font width
        ''' </summary>
        Public Property Width() As Integer
            Get
                Return m_Width
            End Get
            Set
                m_Width = Value
            End Set
        End Property
        Private m_Width As Integer

        ''' <summary>
        ''' Font escapement
        ''' </summary>
        Public Property Escapement() As Integer
            Get
                Return m_Escapement
            End Get
            Set
                m_Escapement = Value
            End Set
        End Property
        Private m_Escapement As Integer

        ''' <summary>
        ''' Font orientation
        ''' </summary>
        Public Property Orientation() As Integer
            Get
                Return m_Orientation
            End Get
            Set
                m_Orientation = Value
            End Set
        End Property
        Private m_Orientation As Integer

        ''' <summary>
        ''' Font weight
        ''' </summary>
        Public Property Weight() As Integer
            Get
                Return m_Weight
            End Get
            Set
                m_Weight = Value
            End Set
        End Property
        Private m_Weight As Integer

        ''' <summary>
        ''' Font italic
        ''' </summary>
        Public Property Italic() As Byte
            Get
                Return m_Italic
            End Get
            Set
                m_Italic = Value
            End Set
        End Property
        Private m_Italic As Byte

        ''' <summary>
        ''' Font underline
        ''' </summary>
        Public Property Underline() As Byte
            Get
                Return m_Underline
            End Get
            Set
                m_Underline = Value
            End Set
        End Property
        Private m_Underline As Byte

        ''' <summary>
        ''' Font strikeout
        ''' </summary>
        Public Property Strikeout() As Byte
            Get
                Return m_Strikeout
            End Get
            Set
                m_Strikeout = Value
            End Set
        End Property
        Private m_Strikeout As Byte

        ''' <summary>
        ''' Font character set
        ''' </summary>
        Public Property CharacterSet() As Byte
            Get
                Return m_CharacterSet
            End Get
            Set
                m_CharacterSet = Value
            End Set
        End Property
        Private m_CharacterSet As Byte

        ''' <summary>
        ''' Font out precision
        ''' </summary>
        Public Property OutPrecision() As Byte
            Get
                Return m_OutPrecision
            End Get
            Set
                m_OutPrecision = Value
            End Set
        End Property
        Private m_OutPrecision As Byte

        ''' <summary>
        ''' Font clip precision
        ''' </summary>
        Public Property ClipPrecision() As Byte
            Get
                Return m_ClipPrecision
            End Get
            Set
                m_ClipPrecision = Value
            End Set
        End Property
        Private m_ClipPrecision As Byte

        ''' <summary>
        ''' Font quality
        ''' </summary>
        Public Property Quality() As Byte
            Get
                Return m_Quality
            End Get
            Set
                m_Quality = Value
            End Set
        End Property
        Private m_Quality As Byte

        ''' <summary>
        ''' Font pitch and family
        ''' </summary>
        Public Property PitchAndFamily() As Byte
            Get
                Return m_PitchAndFamily
            End Get
            Set
                m_PitchAndFamily = Value
            End Set
        End Property
        Private m_PitchAndFamily As Byte

        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=32)>
        Private m_faceName As String = String.Empty

        ''' <summary>
        ''' Font face name
        ''' </summary>
        Public Property FaceName() As String
            Get
                Return m_faceName
            End Get
            Set
                m_faceName = Value
            End Set
        End Property
    End Class

    <StructLayout(LayoutKind.Sequential)>
    Friend Structure NativeColorRef
        Public Property Dword() As UInteger
            Get
                Return m_Dword
            End Get
            Set
                m_Dword = Value
            End Set
        End Property
        Private m_Dword As UInteger
    End Structure

#End Region

    <Flags>
    Friend Enum SetWindowPositionOptions
        AsyncWindowPos = &H4000
        DeferErase = &H2000
        DrawFrame = FrameChanged
        FrameChanged = &H20
        ' The frame changed: send WM_NCCALCSIZE
        HideWindow = &H80
        NoActivate = &H10
        CoCopyBits = &H100
        NoMove = &H2
        NoOwnerZOrder = &H200
        ' Don't do owner Z ordering
        NoRedraw = &H8
        NoResposition = NoOwnerZOrder
        NoSendChanging = &H400
        ' Don't send WM_WINDOWPOSCHANGING
        NoSize = &H1
        NoZOrder = &H4
        ShowWindow = &H40
    End Enum

    Friend Enum SetWindowPositionInsertAfter
        NoTopMost = -2
        TopMost = -1
        Top = 0
        Bottom = 1
    End Enum
End Namespace
