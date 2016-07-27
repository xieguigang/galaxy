Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Runtime.InteropServices
Imports System.Drawing

Namespace API.Dwm

    <StructLayout(LayoutKind.Explicit)>
    Public Structure RECT
        ' Fields
        <FieldOffset(12)>
        Public bottom As Integer
        <FieldOffset(0)>
        Public left As Integer
        <FieldOffset(8)>
        Public right As Integer
        <FieldOffset(4)>
        Public top As Integer

        ' Methods
        Public Sub New(rect As Rectangle)
            Me.left = rect.Left
            Me.top = rect.Top
            Me.right = rect.Right
            Me.bottom = rect.Bottom
        End Sub

        Public Sub New(left As Integer, top As Integer, right As Integer, bottom As Integer)
            Me.left = left
            Me.top = top
            Me.right = right
            Me.bottom = bottom
        End Sub

        Public Sub [Set]()
            Me.left = InlineAssignHelper(Me.top, InlineAssignHelper(Me.right, InlineAssignHelper(Me.bottom, 0)))
        End Sub

        Public Sub [Set](rect As Rectangle)
            Me.left = rect.Left
            Me.top = rect.Top
            Me.right = rect.Right
            Me.bottom = rect.Bottom
        End Sub

        Public Sub [Set](left As Integer, top As Integer, right As Integer, bottom As Integer)
            Me.left = left
            Me.top = top
            Me.right = right
            Me.bottom = bottom
        End Sub

        Public Function ToRectangle() As Rectangle
            Return New Rectangle(Me.left, Me.top, Me.right - Me.left, Me.bottom - Me.top)
        End Function

        ' Properties
        Public ReadOnly Property Height() As Integer
            Get
                Return (Me.bottom - Me.top)
            End Get
        End Property

        Public ReadOnly Property Size() As Size
            Get
                Return New Size(Me.Width, Me.Height)
            End Get
        End Property

        Public ReadOnly Property Width() As Integer
            Get
                Return (Me.right - Me.left)
            End Get
        End Property
        Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
            target = value
            Return value
        End Function
    End Structure

    ' Nested Types
    <StructLayout(LayoutKind.Sequential)>
    Public Structure DWM_BLURBEHIND
        Public dwFlags As Integer
        Public fEnable As Integer
        Public hRgnBlur As IntPtr
        Public fTransitionOnMaximized As Integer
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Public Structure DWM_PRESENT_PARAMETERS
        Public cbSize As Integer
        Public fQueue As Integer
        Public cRefreshStart As Long
        Public cBuffer As Integer
        Public fUseSourceRate As Integer
        Public rateSource As UNSIGNED_RATIO
        Public cRefreshesPerFrame As Integer
        Public eSampling As DWM_SOURCE_FRAME_SAMPLING
    End Structure

    Public Enum DWM_SOURCE_FRAME_SAMPLING
        DWM_SOURCE_FRAME_SAMPLING_POINT
        DWM_SOURCE_FRAME_SAMPLING_COVERAGE
        DWM_SOURCE_FRAME_SAMPLING_LAST
    End Enum

    <StructLayout(LayoutKind.Sequential)>
    Public Structure DWM_THUMBNAIL_PROPERTIES
        Public dwFlags As Integer
        Public rcDestination As RECT
        Public rcSource As RECT
        Public opacity As Byte
        Public fVisible As Integer
        Public fSourceClientAreaOnly As Integer
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Public Structure DWM_TIMING_INFO
        Public cbSize As Integer
        Public rateRefresh As UNSIGNED_RATIO
        Public rateCompose As UNSIGNED_RATIO
        Public qpcVBlank As Long
        Public cRefresh As Long
        Public qpcCompose As Long
        Public cFrame As Long
        Public cRefreshFrame As Long
        Public cRefreshConfirmed As Long
        Public cFlipsOutstanding As Integer
        Public cFrameCurrent As Long
        Public cFramesAvailable As Long
        Public cFrameCleared As Long
        Public cFramesReceived As Long
        Public cFramesDisplayed As Long
        Public cFramesDropped As Long
        Public cFramesMissed As Long
    End Structure

    Public Enum DWMNCRENDERINGPOLICY
        DWMNCRP_USEWINDOWSTYLE
        DWMNCRP_DISABLED
        DWMNCRP_ENABLED
    End Enum

    Public Enum DWMWINDOWATTRIBUTE
        DWMWA_ALLOW_NCPAINT = 4
        DWMWA_CAPTION_BUTTON_BOUNDS = 5
        DWMWA_FLIP3D_POLICY = 8
        DWMWA_FORCE_ICONIC_REPRESENTATION = 7
        DWMWA_LAST = 9
        DWMWA_NCRENDERING_ENABLED = 1
        DWMWA_NCRENDERING_POLICY = 2
        DWMWA_NONCLIENT_RTL_LAYOUT = 6
        DWMWA_TRANSITIONS_FORCEDISABLED = 3
    End Enum

    <StructLayout(LayoutKind.Sequential)>
    Public Structure UNSIGNED_RATIO
        Public uiNumerator As Integer
        Public uiDenominator As Integer
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Public Structure MARGINS
        Public cxLeftWidth As Integer
        Public cxRightWidth As Integer
        Public cyTopHeight As Integer
        Public cyBottomHeight As Integer
        Public Sub New(Left As Integer, Right As Integer, Top As Integer, Bottom As Integer)
            Me.cxLeftWidth = Left
            Me.cxRightWidth = Right
            Me.cyTopHeight = Top
            Me.cyBottomHeight = Bottom
        End Sub
    End Structure

    ''' <summary>
    ''' The Options of What Attributes to Add/Remove
    ''' </summary>
    <StructLayout(LayoutKind.Sequential)>
    Public Structure WTA_OPTIONS
        Public Flags As UInteger
        Public Mask As UInteger
    End Structure

    ''' <summary>
    ''' What Type of Attributes? (Only One is Currently Defined)
    ''' </summary>
    Public Enum WindowThemeAttributeType
        WTA_NONCLIENT = 1
    End Enum
End Namespace