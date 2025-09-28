Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace Internal
    Public Enum WindowMessage
        Null = &H0
        Create = &H1
        Destroy = &H2
        Move = &H3
        Size = &H5
        Activate = &H6
        SetFocus = &H7
        KillFocus = &H8
        Enable = &HA
        SetRedraw = &HB
        SetText = &HC
        GetText = &HD
        GetTextLength = &HE
        Paint = &HF
        Close = &H10
        QueryEndSession = &H11
        Quit = &H12
        QueryOpen = &H13
        EraseBackground = &H14
        SystemColorChange = &H15
        EndSession = &H16
        SystemError = &H17
        ShowWindow = &H18
        ControlColor = &H19
        WinIniChange = &H1A
        SettingChange = &H1A
        DevModeChange = &H1B
        ActivateApplication = &H1C
        FontChange = &H1D
        TimeChange = &H1E
        CancelMode = &H1F
        SetCursor = &H20
        MouseActivate = &H21
        ChildActivate = &H22
        QueueSync = &H23
        GetMinMaxInfo = &H24
        PaintIcon = &H26
        IconEraseBackground = &H27
        NextDialogControl = &H28
        SpoolerStatus = &H2A
        DrawItem = &H2B
        MeasureItem = &H2C
        DeleteItem = &H2D
        VKeyToItem = &H2E
        CharToItem = &H2F

        SetFont = &H30
        GetFont = &H31
        SetHotkey = &H32
        GetHotkey = &H33
        QueryDragIcon = &H37
        CompareItem = &H39
        Compacting = &H41
        WindowPositionChanging = &H46
        WindowPositionChanged = &H47
        Power = &H48
        CopyData = &H4A
        CancelJournal = &H4B
        Notify = &H4E
        InputLanguageChangeRequest = &H50
        InputLanguageChange = &H51
        TCard = &H52
        Help = &H53
        UserChanged = &H54
        NotifyFormat = &H55
        ContextMenu = &H7B
        StyleChanging = &H7C
        StyleChanged = &H7D
        DisplayChange = &H7E
        GetIcon = &H7F
        SetIcon = &H80

        NCCreate = &H81
        NCDestroy = &H82
        NCCalculateSize = &H83
        NCHitTest = &H84
        NCPaint = &H85
        NCActivate = &H86
        GetDialogCode = &H87
        NCMouseMove = &HA0
        NCLeftButtonDown = &HA1
        NCLeftButtonUp = &HA2
        NCLeftButtonDoubleClick = &HA3
        NCRightButtonDown = &HA4
        NCRightButtonUp = &HA5
        NCRightButtonDoubleClick = &HA6
        NCMiddleButtonDown = &HA7
        NCMiddleButtonUp = &HA8
        NCMiddleButtonDoubleClick = &HA9

        KeyFirst = &H100
        KeyDown = &H100
        KeyUp = &H101
        [Char] = &H102
        DeadChar = &H103
        SystemKeyDown = &H104
        SystemKeyUp = &H105
        SystemChar = &H106
        SystemDeadChar = &H107
        KeyLast = &H108

        IMEStartComposition = &H10D
        IMEEndComposition = &H10E
        IMEComposition = &H10F
        IMEKeyLast = &H10F

        InitializeDialog = &H110
        Command = &H111
        SystemCommand = &H112
        Timer = &H113
        HorizontalScroll = &H114
        VerticalScroll = &H115
        InitializeMenu = &H116
        InitializeMenuPopup = &H117
        MenuSelect = &H11F
        MenuChar = &H120
        EnterIdle = &H121

        CTLColorMessageBox = &H132
        CTLColorEdit = &H133
        CTLColorListbox = &H134
        CTLColorButton = &H135
        CTLColorDialog = &H136
        CTLColorScrollBar = &H137
        CTLColorStatic = &H138

        MouseFirst = &H200
        MouseMove = &H200
        LeftButtonDown = &H201
        LeftButtonUp = &H202
        LeftButtonDoubleClick = &H203
        RightButtonDown = &H204
        RightButtonUp = &H205
        RightButtonDoubleClick = &H206
        MiddleButtonDown = &H207
        MiddleButtonUp = &H208
        MiddleButtonDoubleClick = &H209
        MouseWheel = &H20A
        MouseHorizontalWheel = &H20E

        ParentNotify = &H210
        EnterMenuLoop = &H211
        ExitMenuLoop = &H212
        NextMenu = &H213
        Sizing = &H214
        CaptureChanged = &H215
        Moving = &H216
        PowerBroadcast = &H218
        DeviceChange = &H219

        MDICreate = &H220
        MDIDestroy = &H221
        MDIActivate = &H222
        MDIRestore = &H223
        MDINext = &H224
        MDIMaximize = &H225
        MDITile = &H226
        MDICascade = &H227
        MDIIconArrange = &H228
        MDIGetActive = &H229
        MDISetMenu = &H230
        EnterSizeMove = &H231
        ExitSizeMove = &H232
        DropFiles = &H233
        MDIRefreshMenu = &H234

        IMESetContext = &H281
        IMENotify = &H282
        IMEControl = &H283
        IMECompositionFull = &H284
        IMESelect = &H285
        IMEChar = &H286
        IMEKeyDown = &H290
        IMEKeyUp = &H291

        MouseHover = &H2A1
        NCMouseLeave = &H2A2
        MouseLeave = &H2A3

        Cut = &H300
        Copy = &H301
        Paste = &H302
        Clear = &H303
        Undo = &H304

        RenderFormat = &H305
        RenderAllFormats = &H306
        DestroyClipboard = &H307
        DrawClipbard = &H308
        PaintClipbard = &H309
        VerticalScrollClipBoard = &H30A
        SizeClipbard = &H30B
        AskClipboardFormatname = &H30C
        ChangeClipboardChain = &H30D
        HorizontalScrollClipboard = &H30E
        QueryNewPalette = &H30F
        PaletteIsChanging = &H310
        PaletteChanged = &H311

        Hotkey = &H312
        Print = &H317
        PrintClient = &H318

        HandHeldFirst = &H358
        HandHeldlast = &H35F
        PenWinFirst = &H380
        PenWinLast = &H38F
        CoalesceFirst = &H390
        CoalesceLast = &H39F
        DDE_First = &H3E0
        DDE_Initiate = &H3E0
        DDE_Terminate = &H3E1
        DDE_Advise = &H3E2
        DDE_Unadvise = &H3E3
        DDE_Ack = &H3E4
        DDE_Data = &H3E5
        DDE_Request = &H3E6
        DDE_Poke = &H3E7
        DDE_Execute = &H3E8
        DDE_Last = &H3E8

        User = &H400
        App = &H8000
    End Enum
End Namespace
