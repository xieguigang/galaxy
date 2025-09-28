'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports Microsoft.Windows.Internal
Imports Microsoft.Windows.Shell.Interop
Imports Microsoft.Windows.Taskbar

Namespace Shell

    Friend NotInheritable Class WindowUtilities
        Private Sub New()
        End Sub
        Friend Shared Function GetParentOffsetOfChild(hwnd As IntPtr, hwndParent As IntPtr) As System.Drawing.Point
            Dim childScreenCoord = New NativePoint()

            TabbedThumbnailNativeMethods.ClientToScreen(hwnd, childScreenCoord)

            Dim parentScreenCoord = New NativePoint()

            TabbedThumbnailNativeMethods.ClientToScreen(hwndParent, parentScreenCoord)

            Dim offset As New System.Drawing.Point(childScreenCoord.X - parentScreenCoord.X, childScreenCoord.Y - parentScreenCoord.Y)

            Return offset
        End Function

        Friend Shared Function GetNonClientArea(hwnd As IntPtr) As System.Drawing.Size
            Dim c = New NativePoint()

            TabbedThumbnailNativeMethods.ClientToScreen(hwnd, c)

            Dim r = New NativeRect()

            TabbedThumbnailNativeMethods.GetWindowRect(hwnd, r)

            Return New System.Drawing.Size(c.X - r.Left, c.Y - r.Top)
        End Function
    End Class

    <Flags>
    Friend Enum WindowStyles
        ''' <summary>
        ''' The window has a thin-line border.
        ''' </summary>
        Border = &H800000

        ''' <summary>
        ''' The window has a title bar (includes the WS_BORDER style).
        ''' </summary>
        Caption = &HC00000

        ''' <summary>
        ''' The window is a child window. 
        ''' A window with this style cannot have a menu bar. 
        ''' This style cannot be used with the WS_POPUP style.
        ''' </summary>
        Child = &H40000000

        ''' <summary>
        ''' Same as the WS_CHILD style.
        ''' </summary>
        ChildWindow = &H40000000

        ''' <summary>
        ''' Excludes the area occupied by child windows when drawing occurs within the parent window. 
        ''' This style is used when creating the parent window.
        ''' </summary>
        ClipChildren = &H2000000

        ''' <summary>
        ''' Clips child windows relative to each other; 
        ''' that is, when a particular child window receives a WM_PAINT message, 
        ''' the WS_CLIPSIBLINGS style clips all other overlapping child windows out of the region of the child window to be updated. 
        ''' If WS_CLIPSIBLINGS is not specified and child windows overlap, it is possible, 
        ''' when drawing within the client area of a child window, to draw within the client area of a neighboring child window.
        ''' </summary>
        ClipSiblings = &H4000000

        ''' <summary>
        ''' The window is initially disabled. A disabled window cannot receive input from the user. 
        ''' To change this after a window has been created, use the EnableWindow function.
        ''' </summary>
        Disabled = &H8000000

        ''' <summary>
        ''' The window has a border of a style typically used with dialog boxes. 
        ''' A window with this style cannot have a title bar.
        ''' </summary>
        DialogFrame = &H40000

        ''' <summary>
        ''' The window is the first control of a group of controls. 
        ''' The group consists of this first control and all controls defined after it, up to the next control with the WS_GROUP style. 
        ''' The first control in each group usually has the WS_TABSTOP style so that the user can move from group to group. 
        ''' The user can subsequently change the keyboard focus from one control in the group to the next control 
        ''' in the group by using the direction keys.
        ''' 
        ''' You can turn this style on and off to change dialog box navigation. 
        ''' To change this style after a window has been created, use the SetWindowLong function.
        ''' </summary>
        Group = &H20000

        ''' <summary>
        ''' The window has a horizontal scroll bar.
        ''' </summary>
        HorizontalScroll = &H100000

        ''' <summary>
        ''' The window is initially minimized. 
        ''' Same as the WS_MINIMIZE style.
        ''' </summary>
        Iconic = &H20000000

        ''' <summary>
        ''' The window is initially maximized.
        ''' </summary>
        Maximize = &H1000000

        ''' <summary>
        ''' The window has a maximize button. 
        ''' Cannot be combined with the WS_EX_CONTEXTHELP style. 
        ''' The WS_SYSMENU style must also be specifie
        ''' </summary>
        MaximizeBox = &H10000

        ''' <summary>
        ''' The window is initially minimized. 
        ''' Same as the WS_ICONIC style.
        ''' </summary>
        Minimize = &H20000000

        ''' <summary>
        ''' The window has a minimize button. 
        ''' Cannot be combined with the WS_EX_CONTEXTHELP style. 
        ''' The WS_SYSMENU style must also be specified.
        ''' </summary>
        MinimizeBox = &H20000

        ''' <summary>
        ''' The window is an overlapped window. 
        ''' An overlapped window has a title bar and a border. 
        ''' Same as the WS_TILED style.
        ''' </summary>
        Overlapped = &H0

        ''' <summary>
        ''' The windows is a pop-up window. 
        ''' This style cannot be used with the WS_CHILD style.
        ''' </summary>
        Popup = &H80000000UI

        ''' <summary>
        ''' The window has a sizing border. 
        ''' Same as the WS_THICKFRAME style.
        ''' </summary>
        SizeBox = &H40000

        ''' <summary>
        ''' The window has a window menu on its title bar. 
        ''' The WS_CAPTION style must also be specified.
        ''' </summary>
        SystemMenu = &H80000

        ''' <summary>
        ''' The window is a control that can receive the keyboard focus when the user presses the TAB key. 
        ''' Pressing the TAB key changes the keyboard focus to the next control with the WS_TABSTOP style.
        ''' 
        ''' You can turn this style on and off to change dialog box navigation. 
        ''' To change this style after a window has been created, use the SetWindowLong function. 
        ''' For user-created windows and modeless dialogs to work with tab stops, 
        ''' alter the message loop to call the IsDialogMessage function.
        ''' </summary>
        Tabstop = &H10000

        ''' <summary>
        ''' The window has a sizing border. 
        ''' Same as the WS_SIZEBOX style.
        ''' </summary>
        ThickFrame = &H40000

        ''' <summary>
        ''' The window is an overlapped window. 
        ''' An overlapped window has a title bar and a border. 
        ''' Same as the WS_OVERLAPPED style.
        ''' </summary>
        Tiled = &H0

        ''' <summary>
        ''' The window is initially visible.
        ''' 
        ''' This style can be turned on and off by using the ShowWindow or SetWindowPos function.
        ''' </summary>
        Visible = &H10000000

        ''' <summary>
        ''' The window has a vertical scroll bar.
        ''' </summary>
        VerticalScroll = &H200000

        ''' <summary>
        ''' The window is an overlapped window. 
        ''' Same as the WS_OVERLAPPEDWINDOW style.
        ''' </summary>
        TiledWindowMask = Overlapped Or Caption Or SystemMenu Or ThickFrame Or MinimizeBox Or MaximizeBox

        ''' <summary>
        ''' The window is a pop-up window. 
        ''' The WS_CAPTION and WS_POPUPWINDOW styles must be combined to make the window menu visible.
        ''' </summary>
        PopupWindowMask = Popup Or Border Or SystemMenu

        ''' <summary>
        ''' The window is an overlapped window. Same as the WS_TILEDWINDOW style.
        ''' </summary>
        OverlappedWindowMask = Overlapped Or Caption Or SystemMenu Or ThickFrame Or MinimizeBox Or MaximizeBox
    End Enum
End Namespace
