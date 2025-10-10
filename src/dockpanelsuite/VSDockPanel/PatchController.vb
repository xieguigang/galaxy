Imports Microsoft.Win32
Imports System

Namespace WeifenLuo.WinFormsUI.Docking
    Public Module PatchController
        Private _EnableAll As Boolean?

        Public Property EnableAll As Boolean?
            Private Get
                Return _EnableAll
            End Get
            Set(value As Boolean?)
                _EnableAll = value
            End Set
        End Property

        Public Sub Reset()
            EnableAll = Nothing
            _highDpi = Nothing
            _memoryLeakFix = Nothing
            _nestedDisposalFix = Nothing
            _focusLostFix = Nothing
            _contentOrderFix = Nothing
            _fontInheritanceFix = Nothing
            _activeXFix = Nothing
            _displayingPaneFix = Nothing
            _activeControlFix = Nothing
            _floatSplitterFix = Nothing
            _activateOnDockFix = Nothing
            _selectClosestOnClose = Nothing
            _perScreenDpi = Nothing
        End Sub

#Region "Copy this section to create new option, and then comment it to show what needs to be modified."
        '*
        Private _highDpi As Boolean?

        Public Property EnableHighDpi As Boolean?
            Get
                If _highDpi IsNot Nothing Then
                    Return _highDpi
                End If

                If EnableAll IsNot Nothing Then
                    _highDpi = EnableAll
                    Return EnableAll
                End If
#If NET35 Or NET40 Then
                var section = ConfigurationManager.GetSection("dockPanelSuite") as PatchSection;
                if (section != null)
                {
                    if (section.EnableAll != null)
                    {
                        return _highDpi = section.EnableAll;
                    }

                    return _highDpi = section.EnableHighDpi;
                }
#End If
                Dim environment = System.Environment.GetEnvironmentVariable("DPS_EnableHighDpi")
                If Not String.IsNullOrEmpty(environment) Then
                    Dim enable = False
                    If Boolean.TryParse(environment, enable) Then
                        _highDpi = enable
                        Return enable
                    End If
                End If

                If True Then
                    Dim key = Registry.CurrentUser.OpenSubKey("Software\DockPanelSuite")
                    If key IsNot Nothing Then
                        Dim pair = key.GetValue("EnableHighDpi")
                        If pair IsNot Nothing Then
                            Dim enable = False
                            If Boolean.TryParse(pair.ToString(), enable) Then
                                _highDpi = enable
                                Return enable
                            End If
                        End If
                    End If
                End If

                If True Then
                    Dim key = Registry.LocalMachine.OpenSubKey("Software\DockPanelSuite")
                    If key IsNot Nothing Then
                        Dim pair = key.GetValue("EnableHighDpi")
                        If pair IsNot Nothing Then
                            Dim enable = False
                            If Boolean.TryParse(pair.ToString(), enable) Then
                                _highDpi = enable
                                Return enable
                            End If
                        End If
                    End If
                End If

                _highDpi = True

                Return True
            End Get

            Set(value As Boolean?)
                _highDpi = value
            End Set
        End Property
        ' 
#End Region

        Private _memoryLeakFix As Boolean?

        Public Property EnableMemoryLeakFix As Boolean?
            Get
                If _memoryLeakFix IsNot Nothing Then
                    Return _memoryLeakFix
                End If

                If EnableAll IsNot Nothing Then
                    _memoryLeakFix = EnableAll
                    Return EnableAll
                End If
#If NET35 Or NET40 Then
                var section = ConfigurationManager.GetSection("dockPanelSuite") as PatchSection;
                if (section != null)
                {
                    if (section.EnableAll != null)
                    {
                        return _memoryLeakFix = section.EnableAll;
                    }

                    return _memoryLeakFix = section.EnableMemoryLeakFix;
                }
#End If
                Dim environment = System.Environment.GetEnvironmentVariable("DPS_EnableMemoryLeakFix")
                If Not String.IsNullOrEmpty(environment) Then
                    Dim enable = False
                    If Boolean.TryParse(environment, enable) Then
                        _memoryLeakFix = enable
                        Return enable
                    End If
                End If

                If True Then
                    Dim key = Registry.CurrentUser.OpenSubKey("Software\DockPanelSuite")
                    If key IsNot Nothing Then
                        Dim pair = key.GetValue("EnableMemoryLeakFix")
                        If pair IsNot Nothing Then
                            Dim enable = False
                            If Boolean.TryParse(pair.ToString(), enable) Then
                                _memoryLeakFix = enable
                                Return enable
                            End If
                        End If
                    End If
                End If

                If True Then
                    Dim key = Registry.LocalMachine.OpenSubKey("Software\DockPanelSuite")
                    If key IsNot Nothing Then
                        Dim pair = key.GetValue("EnableMemoryLeakFix")
                        If pair IsNot Nothing Then
                            Dim enable = False
                            If Boolean.TryParse(pair.ToString(), enable) Then
                                _memoryLeakFix = enable
                                Return enable
                            End If
                        End If
                    End If
                End If

                _memoryLeakFix = True

                Return True
            End Get

            Set(value As Boolean?)
                _memoryLeakFix = value
            End Set
        End Property

        Private _focusLostFix As Boolean?

        Public Property EnableMainWindowFocusLostFix As Boolean?
            Get
                If _focusLostFix IsNot Nothing Then
                    Return _focusLostFix
                End If

                If EnableAll IsNot Nothing Then
                    _focusLostFix = EnableAll
                    Return EnableAll
                End If
#If NET35 Or NET40 Then
                var section = ConfigurationManager.GetSection("dockPanelSuite") as PatchSection;
                if (section != null)
                {
                    if (section.EnableAll != null)
                    {
                        return _focusLostFix = section.EnableAll;
                    }

                    return _focusLostFix = section.EnableMainWindowFocusLostFix;
                }
#End If
                Dim environment = System.Environment.GetEnvironmentVariable("DPS_EnableMainWindowFocusLostFix")
                If Not String.IsNullOrEmpty(environment) Then
                    Dim enable = False
                    If Boolean.TryParse(environment, enable) Then
                        _focusLostFix = enable
                        Return enable
                    End If
                End If

                If True Then
                    Dim key = Registry.CurrentUser.OpenSubKey("Software\DockPanelSuite")
                    If key IsNot Nothing Then
                        Dim pair = key.GetValue("EnableMainWindowFocusLostFix")
                        If pair IsNot Nothing Then
                            Dim enable = False
                            If Boolean.TryParse(pair.ToString(), enable) Then
                                _focusLostFix = enable
                                Return enable
                            End If
                        End If
                    End If
                End If

                If True Then
                    Dim key = Registry.LocalMachine.OpenSubKey("Software\DockPanelSuite")
                    If key IsNot Nothing Then
                        Dim pair = key.GetValue("EnableMainWindowFocusLostFix")
                        If pair IsNot Nothing Then
                            Dim enable = False
                            If Boolean.TryParse(pair.ToString(), enable) Then
                                _focusLostFix = enable
                                Return enable
                            End If
                        End If
                    End If
                End If

                _focusLostFix = True

                Return True
            End Get

            Set(value As Boolean?)
                _focusLostFix = value
            End Set
        End Property

        Private _nestedDisposalFix As Boolean?

        Public Property EnableNestedDisposalFix As Boolean?
            Get
                If _nestedDisposalFix IsNot Nothing Then
                    Return _nestedDisposalFix
                End If

                If EnableAll IsNot Nothing Then
                    _nestedDisposalFix = EnableAll
                    Return EnableAll
                End If
#If NET35 Or NET40 Then
                var section = ConfigurationManager.GetSection("dockPanelSuite") as PatchSection;
                if (section != null)
                {
                    if (section.EnableAll != null)
                    {
                        return _nestedDisposalFix = section.EnableAll;
                    }

                    return _nestedDisposalFix = section.EnableNestedDisposalFix;
                }
#End If
                Dim environment = System.Environment.GetEnvironmentVariable("DPS_EnableNestedDisposalFix")
                If Not String.IsNullOrEmpty(environment) Then
                    Dim enable = False
                    If Boolean.TryParse(environment, enable) Then
                        _nestedDisposalFix = enable
                        Return enable
                    End If
                End If

                If True Then
                    Dim key = Registry.CurrentUser.OpenSubKey("Software\DockPanelSuite")
                    If key IsNot Nothing Then
                        Dim pair = key.GetValue("EnableNestedDisposalFix")
                        If pair IsNot Nothing Then
                            Dim enable = False
                            If Boolean.TryParse(pair.ToString(), enable) Then
                                _nestedDisposalFix = enable
                                Return enable
                            End If
                        End If
                    End If
                End If

                If True Then
                    Dim key = Registry.LocalMachine.OpenSubKey("Software\DockPanelSuite")
                    If key IsNot Nothing Then
                        Dim pair = key.GetValue("EnableNestedDisposalFix")
                        If pair IsNot Nothing Then
                            Dim enable = False
                            If Boolean.TryParse(pair.ToString(), enable) Then
                                _nestedDisposalFix = enable
                                Return enable
                            End If
                        End If
                    End If
                End If

                _nestedDisposalFix = True

                Return True
            End Get

            Set(value As Boolean?)
                _focusLostFix = value
            End Set
        End Property

        Private _fontInheritanceFix As Boolean?

        Public Property EnableFontInheritanceFix As Boolean?
            Get
                If _fontInheritanceFix IsNot Nothing Then
                    Return _fontInheritanceFix
                End If

                If EnableAll IsNot Nothing Then
                    _fontInheritanceFix = EnableAll
                    Return EnableAll
                End If
#If NET35 Or NET40 Then
                var section = ConfigurationManager.GetSection("dockPanelSuite") as PatchSection;
                if (section != null)
                {
                    if (section.EnableAll != null)
                    {
                        return _fontInheritanceFix = section.EnableAll;
                    }

                    return _fontInheritanceFix = section.EnableFontInheritanceFix;
                }
#End If
                Dim environment = System.Environment.GetEnvironmentVariable("DPS_EnableFontInheritanceFix")
                If Not String.IsNullOrEmpty(environment) Then
                    Dim enable = False
                    If Boolean.TryParse(environment, enable) Then
                        _fontInheritanceFix = enable
                        Return enable
                    End If
                End If

                If True Then
                    Dim key = Registry.CurrentUser.OpenSubKey("Software\DockPanelSuite")
                    If key IsNot Nothing Then
                        Dim pair = key.GetValue("EnableFontInheritanceFix")
                        If pair IsNot Nothing Then
                            Dim enable = False
                            If Boolean.TryParse(pair.ToString(), enable) Then
                                _fontInheritanceFix = enable
                                Return enable
                            End If
                        End If
                    End If
                End If

                If True Then
                    Dim key = Registry.LocalMachine.OpenSubKey("Software\DockPanelSuite")
                    If key IsNot Nothing Then
                        Dim pair = key.GetValue("EnableFontInheritanceFix")
                        If pair IsNot Nothing Then
                            Dim enable = False
                            If Boolean.TryParse(pair.ToString(), enable) Then
                                _fontInheritanceFix = enable
                                Return enable
                            End If
                        End If
                    End If
                End If

                _fontInheritanceFix = True

                Return True
            End Get

            Set(value As Boolean?)
                _fontInheritanceFix = value
            End Set
        End Property

        Private _contentOrderFix As Boolean?

        Public Property EnableContentOrderFix As Boolean?
            Get
                If _contentOrderFix IsNot Nothing Then
                    Return _contentOrderFix
                End If

                If EnableAll IsNot Nothing Then
                    _contentOrderFix = EnableAll
                    Return EnableAll
                End If
#If NET35 Or NET40 Then
                var section = ConfigurationManager.GetSection("dockPanelSuite") as PatchSection;
                if (section != null)
                {
                    if (section.EnableAll != null)
                    {
                        return _contentOrderFix = section.EnableAll;
                    }

                    return _contentOrderFix = section.EnableContentOrderFix;
                }
#End If
                Dim environment = System.Environment.GetEnvironmentVariable("DPS_EnableContentOrderFix")
                If Not String.IsNullOrEmpty(environment) Then
                    Dim enable = False
                    If Boolean.TryParse(environment, enable) Then
                        _contentOrderFix = enable
                        Return enable
                    End If
                End If

                If True Then
                    Dim key = Registry.CurrentUser.OpenSubKey("Software\DockPanelSuite")
                    If key IsNot Nothing Then
                        Dim pair = key.GetValue("EnableContentOrderFix")
                        If pair IsNot Nothing Then
                            Dim enable = False
                            If Boolean.TryParse(pair.ToString(), enable) Then
                                _contentOrderFix = enable
                                Return enable
                            End If
                        End If
                    End If
                End If

                If True Then
                    Dim key = Registry.LocalMachine.OpenSubKey("Software\DockPanelSuite")
                    If key IsNot Nothing Then
                        Dim pair = key.GetValue("EnableContentOrderFix")
                        If pair IsNot Nothing Then
                            Dim enable = False
                            If Boolean.TryParse(pair.ToString(), enable) Then
                                _contentOrderFix = enable
                                Return enable
                            End If
                        End If
                    End If
                End If

                _contentOrderFix = True

                Return True
            End Get

            Set(value As Boolean?)
                _contentOrderFix = value
            End Set
        End Property

        Private _activeXFix As Boolean?

        Public Property EnableActiveXFix As Boolean?
            Get
                If _activeXFix IsNot Nothing Then
                    Return _activeXFix
                End If

                If EnableAll IsNot Nothing Then
                    _activeXFix = EnableAll
                    Return EnableAll
                End If
#If NET35 Or NET40 Then
                var section = ConfigurationManager.GetSection("dockPanelSuite") as PatchSection;
                if (section != null)
                {
                    if (section.EnableAll != null)
                    {
                        return _activeXFix = section.EnableAll;
                    }

                    return _activeXFix = section.EnableActiveXFix;
                }
#End If
                Dim environment = System.Environment.GetEnvironmentVariable("DPS_EnableActiveXFix")
                If Not String.IsNullOrEmpty(environment) Then
                    Dim enable = False
                    If Boolean.TryParse(environment, enable) Then
                        _activeXFix = enable
                        Return enable
                    End If
                End If

                If True Then
                    Dim key = Registry.CurrentUser.OpenSubKey("Software\DockPanelSuite")
                    If key IsNot Nothing Then
                        Dim pair = key.GetValue("EnableActiveXFix")
                        If pair IsNot Nothing Then
                            Dim enable = False
                            If Boolean.TryParse(pair.ToString(), enable) Then
                                _activeXFix = enable
                                Return enable
                            End If
                        End If
                    End If
                End If

                If True Then
                    Dim key = Registry.LocalMachine.OpenSubKey("Software\DockPanelSuite")
                    If key IsNot Nothing Then
                        Dim pair = key.GetValue("EnableActiveXFix")
                        If pair IsNot Nothing Then
                            Dim enable = False
                            If Boolean.TryParse(pair.ToString(), enable) Then
                                _activeXFix = enable
                                Return enable
                            End If
                        End If
                    End If
                End If

                _activeXFix = False

                Return False ' not enabled by default as it has side effect.
            End Get

            Set(value As Boolean?)
                _activeXFix = value
            End Set
        End Property

        Private _displayingPaneFix As Boolean?

        Public Property EnableDisplayingPaneFix As Boolean?
            Get
                If _displayingPaneFix IsNot Nothing Then
                    Return _displayingPaneFix
                End If

                If EnableAll IsNot Nothing Then
                    _displayingPaneFix = EnableAll
                    Return EnableAll
                End If
#If NET35 Or NET40 Then
                var section = ConfigurationManager.GetSection("dockPanelSuite") as PatchSection;
                if (section != null)
                {
                    if (section.EnableAll != null)
                    {
                        return _displayingPaneFix = section.EnableAll;
                    }

                    return _displayingPaneFix = section.EnableDisplayingPaneFix;
                }
#End If
                Dim environment = System.Environment.GetEnvironmentVariable("DPS_EnableDisplayingPaneFix")
                If Not String.IsNullOrEmpty(environment) Then
                    Dim enable = False
                    If Boolean.TryParse(environment, enable) Then
                        _displayingPaneFix = enable
                        Return enable
                    End If
                End If

                If True Then
                    Dim key = Registry.CurrentUser.OpenSubKey("Software\DockPanelSuite")
                    If key IsNot Nothing Then
                        Dim pair = key.GetValue("EnableDisplayingPaneFix")
                        If pair IsNot Nothing Then
                            Dim enable = False
                            If Boolean.TryParse(pair.ToString(), enable) Then
                                _displayingPaneFix = enable
                                Return enable
                            End If
                        End If
                    End If
                End If

                If True Then
                    Dim key = Registry.LocalMachine.OpenSubKey("Software\DockPanelSuite")
                    If key IsNot Nothing Then
                        Dim pair = key.GetValue("EnableDisplayingPaneFix")
                        If pair IsNot Nothing Then
                            Dim enable = False
                            If Boolean.TryParse(pair.ToString(), enable) Then
                                _displayingPaneFix = enable
                                Return enable
                            End If
                        End If
                    End If
                End If

                _displayingPaneFix = True

                Return True
            End Get

            Set(value As Boolean?)
                _displayingPaneFix = value
            End Set
        End Property

        Private _activeControlFix As Boolean?

        Public Property EnableActiveControlFix As Boolean?
            Get
                If _activeControlFix IsNot Nothing Then
                    Return _activeControlFix
                End If

                If EnableAll IsNot Nothing Then
                    _activeControlFix = EnableAll
                    Return EnableAll
                End If
#If NET35 Or NET40 Then
                var section = ConfigurationManager.GetSection("dockPanelSuite") as PatchSection;
                if (section != null)
                {
                    if (section.EnableAll != null)
                    {
                        return _activeControlFix = section.EnableAll;
                    }

                    return _activeControlFix = section.EnableActiveControlFix;
                }
#End If
                Dim environment = System.Environment.GetEnvironmentVariable("DPS_EnableActiveControlFix")
                If Not String.IsNullOrEmpty(environment) Then
                    Dim enable = False
                    If Boolean.TryParse(environment, enable) Then
                        _activeControlFix = enable
                        Return enable
                    End If
                End If

                If True Then
                    Dim key = Registry.CurrentUser.OpenSubKey("Software\DockPanelSuite")
                    If key IsNot Nothing Then
                        Dim pair = key.GetValue("EnableActiveControlFix")
                        If pair IsNot Nothing Then
                            Dim enable = False
                            If Boolean.TryParse(pair.ToString(), enable) Then
                                _activeControlFix = enable
                                Return enable
                            End If
                        End If
                    End If
                End If

                If True Then
                    Dim key = Registry.LocalMachine.OpenSubKey("Software\DockPanelSuite")
                    If key IsNot Nothing Then
                        Dim pair = key.GetValue("EnableActiveControlFix")
                        If pair IsNot Nothing Then
                            Dim enable = False
                            If Boolean.TryParse(pair.ToString(), enable) Then
                                _activeControlFix = enable
                                Return enable
                            End If
                        End If
                    End If
                End If

                _activeControlFix = True

                Return True
            End Get

            Set(value As Boolean?)
                _activeControlFix = value
            End Set
        End Property

        Private _floatSplitterFix As Boolean?

        Public Property EnableFloatSplitterFix As Boolean?
            Get
                If _floatSplitterFix IsNot Nothing Then
                    Return _floatSplitterFix
                End If

                If EnableAll IsNot Nothing Then
                    _floatSplitterFix = EnableAll
                    Return EnableAll
                End If
#If NET35 Or NET40 Then
                var section = ConfigurationManager.GetSection("dockPanelSuite") as PatchSection;
                if (section != null)
                {
                    if (section.EnableAll != null)
                    {
                        return _floatSplitterFix = section.EnableAll;
                    }

                    return _floatSplitterFix = section.EnableFloatSplitterFix;
                }
#End If
                Dim environment = System.Environment.GetEnvironmentVariable("DPS_EnableFloatSplitterFix")
                If Not String.IsNullOrEmpty(environment) Then
                    Dim enable = False
                    If Boolean.TryParse(environment, enable) Then
                        _floatSplitterFix = enable
                        Return enable
                    End If
                End If

                If True Then
                    Dim key = Registry.CurrentUser.OpenSubKey("Software\DockPanelSuite")
                    If key IsNot Nothing Then
                        Dim pair = key.GetValue("EnableFloatSplitterFix")
                        If pair IsNot Nothing Then
                            Dim enable = False
                            If Boolean.TryParse(pair.ToString(), enable) Then
                                _floatSplitterFix = enable
                                Return enable
                            End If
                        End If
                    End If
                End If

                If True Then
                    Dim key = Registry.LocalMachine.OpenSubKey("Software\DockPanelSuite")
                    If key IsNot Nothing Then
                        Dim pair = key.GetValue("EnableFloatSplitterFix")
                        If pair IsNot Nothing Then
                            Dim enable = False
                            If Boolean.TryParse(pair.ToString(), enable) Then
                                _floatSplitterFix = enable
                                Return enable
                            End If
                        End If
                    End If
                End If

                _floatSplitterFix = True

                Return True
            End Get

            Set(value As Boolean?)
                _floatSplitterFix = value
            End Set
        End Property

        Private _activateOnDockFix As Boolean?

        Public Property EnableActivateOnDockFix As Boolean?
            Get
                If _activateOnDockFix IsNot Nothing Then
                    Return _activateOnDockFix
                End If

                If EnableAll IsNot Nothing Then
                    _activateOnDockFix = EnableAll
                    Return EnableAll
                End If
#If NET35 Or NET40 Then
                var section = ConfigurationManager.GetSection("dockPanelSuite") as PatchSection;
                if (section != null)
                {
                    if (section.EnableAll != null)
                    {
                        return _activateOnDockFix = section.EnableAll;
                    }

                    return _activateOnDockFix = section.EnableActivateOnDockFix;
                }
#End If
                Dim environment = System.Environment.GetEnvironmentVariable("DPS_EnableActivateOnDockFix")
                If Not String.IsNullOrEmpty(environment) Then
                    Dim enable = False
                    If Boolean.TryParse(environment, enable) Then
                        _activateOnDockFix = enable
                        Return enable
                    End If
                End If

                If True Then
                    Dim key = Registry.CurrentUser.OpenSubKey("Software\DockPanelSuite")
                    If key IsNot Nothing Then
                        Dim pair = key.GetValue("EnableActivateOnDockFix")
                        If pair IsNot Nothing Then
                            Dim enable = False
                            If Boolean.TryParse(pair.ToString(), enable) Then
                                _activateOnDockFix = enable
                                Return enable
                            End If
                        End If
                    End If
                End If

                If True Then
                    Dim key = Registry.LocalMachine.OpenSubKey("Software\DockPanelSuite")
                    If key IsNot Nothing Then
                        Dim pair = key.GetValue("EnableActivateOnDockFix")
                        If pair IsNot Nothing Then
                            Dim enable = False
                            If Boolean.TryParse(pair.ToString(), enable) Then
                                _activateOnDockFix = enable
                                Return enable
                            End If
                        End If
                    End If
                End If

                _activateOnDockFix = True

                Return True
            End Get

            Set(value As Boolean?)
                _activateOnDockFix = value
            End Set
        End Property

        Private _selectClosestOnClose As Boolean?

        Public Property EnableSelectClosestOnClose As Boolean?
            Get
                If _selectClosestOnClose IsNot Nothing Then
                    Return _selectClosestOnClose
                End If

                If EnableAll IsNot Nothing Then
                    _selectClosestOnClose = EnableAll
                    Return EnableAll
                End If
#If NET35 Or NET40 Then
                var section = ConfigurationManager.GetSection("dockPanelSuite") as PatchSection;
                if (section != null)
                {
                    if (section.EnableAll != null)
                    {
                        return _selectClosestOnClose = section.EnableAll;
                    }

                    return _selectClosestOnClose = section.EnableSelectClosestOnClose;
                }
#End If
                Dim environment = System.Environment.GetEnvironmentVariable("DPS_EnableSelectClosestOnClose")
                If Not String.IsNullOrEmpty(environment) Then
                    Dim enable = False
                    If Boolean.TryParse(environment, enable) Then
                        _selectClosestOnClose = enable
                        Return enable
                    End If
                End If

                If True Then
                    Dim key = Registry.CurrentUser.OpenSubKey("Software\DockPanelSuite")
                    If key IsNot Nothing Then
                        Dim pair = key.GetValue("EnableSelectClosestOnClose")
                        If pair IsNot Nothing Then
                            Dim enable = False
                            If Boolean.TryParse(pair.ToString(), enable) Then
                                _selectClosestOnClose = enable
                                Return enable
                            End If
                        End If
                    End If
                End If

                If True Then
                    Dim key = Registry.LocalMachine.OpenSubKey("Software\DockPanelSuite")
                    If key IsNot Nothing Then
                        Dim pair = key.GetValue("EnableSelectClosestOnClose")
                        If pair IsNot Nothing Then
                            Dim enable = False
                            If Boolean.TryParse(pair.ToString(), enable) Then
                                _selectClosestOnClose = enable
                                Return enable
                            End If
                        End If
                    End If
                End If

                _selectClosestOnClose = True

                Return True
            End Get

            Set(value As Boolean?)
                _selectClosestOnClose = value
            End Set
        End Property

        Private _perScreenDpi As Boolean?

        Public Property EnablePerScreenDpi As Boolean?
            Get
                If _perScreenDpi IsNot Nothing Then
                    Return _perScreenDpi
                End If

                If EnableAll IsNot Nothing Then
                    _perScreenDpi = EnableAll
                    Return EnableAll
                End If
#If NET35 Or NET40 Then
                var section = ConfigurationManager.GetSection("dockPanelSuite") as PatchSection;
                if (section != null)
                {
                    if (section.EnableAll != null)
                    {
                        return _perScreenDpi = section.EnableAll;
                    }

                    return _perScreenDpi = section.EnablePerScreenDpi;
                }
#End If
                Dim environment = System.Environment.GetEnvironmentVariable("DPS_EnablePerScreenDpi")
                If Not String.IsNullOrEmpty(environment) Then
                    Dim enable = False
                    If Boolean.TryParse(environment, enable) Then
                        _perScreenDpi = enable
                        Return enable
                    End If
                End If

                If True Then
                    Dim key = Registry.CurrentUser.OpenSubKey("Software\DockPanelSuite")
                    If key IsNot Nothing Then
                        Dim pair = key.GetValue("EnablePerScreenDpi")
                        If pair IsNot Nothing Then
                            Dim enable = False
                            If Boolean.TryParse(pair.ToString(), enable) Then
                                _perScreenDpi = enable
                                Return enable
                            End If
                        End If
                    End If
                End If

                If True Then
                    Dim key = Registry.LocalMachine.OpenSubKey("Software\DockPanelSuite")
                    If key IsNot Nothing Then
                        Dim pair = key.GetValue("EnablePerScreenDpi")
                        If pair IsNot Nothing Then
                            Dim enable = False
                            If Boolean.TryParse(pair.ToString(), enable) Then
                                _perScreenDpi = enable
                                Return enable
                            End If
                        End If
                    End If
                End If

                _perScreenDpi = False

                Return False
            End Get

            Set(value As Boolean?)
                _perScreenDpi = value
            End Set
        End Property
    End Module
End Namespace
