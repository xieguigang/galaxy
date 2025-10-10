Imports WeifenLuo.WinFormsUI.Docking

Namespace WeifenLuo.WinFormsUI.ThemeVS2012
    Public Class ImageService
        Implements IImageService
        Private _Dockindicator_PaneDiamond As System.Drawing.Bitmap, _Dockindicator_PaneDiamond_Fill As System.Drawing.Bitmap, _Dockindicator_PaneDiamond_Hotspot As System.Drawing.Bitmap, _DockIndicator_PaneDiamond_HotspotIndex As System.Drawing.Bitmap, _DockIndicator_PanelBottom As System.Drawing.Image, _DockIndicator_PanelFill As System.Drawing.Image, _DockIndicator_PanelLeft As System.Drawing.Image, _DockIndicator_PanelRight As System.Drawing.Image, _DockIndicator_PanelTop As System.Drawing.Image, _DockPane_Close As System.Drawing.Bitmap, _DockPane_List As System.Drawing.Bitmap, _DockPane_Dock As System.Drawing.Bitmap, _DockPaneActive_AutoHide As System.Drawing.Bitmap, _DockPane_Option As System.Drawing.Bitmap, _DockPane_OptionOverflow As System.Drawing.Bitmap, _DockPaneHover_Close As System.Drawing.Bitmap, _DockPaneHover_List As System.Drawing.Bitmap, _DockPaneHover_Dock As System.Drawing.Bitmap, _DockPaneActiveHover_AutoHide As System.Drawing.Bitmap, _DockPaneHover_Option As System.Drawing.Bitmap, _DockPaneHover_OptionOverflow As System.Drawing.Bitmap, _DockPanePress_Close As System.Drawing.Bitmap, _DockPanePress_List As System.Drawing.Bitmap, _DockPanePress_Dock As System.Drawing.Bitmap, _DockPanePress_AutoHide As System.Drawing.Bitmap, _DockPanePress_Option As System.Drawing.Bitmap, _DockPanePress_OptionOverflow As System.Drawing.Bitmap, _TabHoverActive_Close As System.Drawing.Image, _TabActive_Close As System.Drawing.Image, _TabInactive_Close As System.Drawing.Image, _TabHoverInactive_Close As System.Drawing.Image, _TabHoverLostFocus_Close As System.Drawing.Image, _TabLostFocus_Close As System.Drawing.Image

        Public Property Dockindicator_PaneDiamond As Bitmap Implements IImageService.Dockindicator_PaneDiamond
            Get
                Return _Dockindicator_PaneDiamond
            End Get
            Friend Set(value As Bitmap)
                _Dockindicator_PaneDiamond = value
            End Set
        End Property

        Public Property Dockindicator_PaneDiamond_Fill As Bitmap Implements IImageService.Dockindicator_PaneDiamond_Fill
            Get
                Return _Dockindicator_PaneDiamond_Fill
            End Get
            Friend Set(value As Bitmap)
                _Dockindicator_PaneDiamond_Fill = value
            End Set
        End Property

        Public Property Dockindicator_PaneDiamond_Hotspot As Bitmap Implements IImageService.Dockindicator_PaneDiamond_Hotspot
            Get
                Return _Dockindicator_PaneDiamond_Hotspot
            End Get
            Friend Set(value As Bitmap)
                _Dockindicator_PaneDiamond_Hotspot = value
            End Set
        End Property

        Public Property DockIndicator_PaneDiamond_HotspotIndex As Bitmap Implements IImageService.DockIndicator_PaneDiamond_HotspotIndex
            Get
                Return _DockIndicator_PaneDiamond_HotspotIndex
            End Get
            Friend Set(value As Bitmap)
                _DockIndicator_PaneDiamond_HotspotIndex = value
            End Set
        End Property

        Public Property DockIndicator_PanelBottom As Image Implements IImageService.DockIndicator_PanelBottom
            Get
                Return _DockIndicator_PanelBottom
            End Get
            Friend Set(value As Image)
                _DockIndicator_PanelBottom = value
            End Set
        End Property

        Public Property DockIndicator_PanelFill As Image Implements IImageService.DockIndicator_PanelFill
            Get
                Return _DockIndicator_PanelFill
            End Get
            Friend Set(value As Image)
                _DockIndicator_PanelFill = value
            End Set
        End Property

        Public Property DockIndicator_PanelLeft As Image Implements IImageService.DockIndicator_PanelLeft
            Get
                Return _DockIndicator_PanelLeft
            End Get
            Friend Set(value As Image)
                _DockIndicator_PanelLeft = value
            End Set
        End Property

        Public Property DockIndicator_PanelRight As Image Implements IImageService.DockIndicator_PanelRight
            Get
                Return _DockIndicator_PanelRight
            End Get
            Friend Set(value As Image)
                _DockIndicator_PanelRight = value
            End Set
        End Property

        Public Property DockIndicator_PanelTop As Image Implements IImageService.DockIndicator_PanelTop
            Get
                Return _DockIndicator_PanelTop
            End Get
            Friend Set(value As Image)
                _DockIndicator_PanelTop = value
            End Set
        End Property

        Public Property DockPane_Close As Bitmap Implements IImageService.DockPane_Close
            Get
                Return _DockPane_Close
            End Get
            Friend Set(value As Bitmap)
                _DockPane_Close = value
            End Set
        End Property

        Public Property DockPane_List As Bitmap Implements IImageService.DockPane_List
            Get
                Return _DockPane_List
            End Get
            Friend Set(value As Bitmap)
                _DockPane_List = value
            End Set
        End Property

        Public Property DockPane_Dock As Bitmap Implements IImageService.DockPane_Dock
            Get
                Return _DockPane_Dock
            End Get
            Friend Set(value As Bitmap)
                _DockPane_Dock = value
            End Set
        End Property

        Public Property DockPaneActive_AutoHide As Bitmap Implements IImageService.DockPaneActive_AutoHide
            Get
                Return _DockPaneActive_AutoHide
            End Get
            Friend Set(value As Bitmap)
                _DockPaneActive_AutoHide = value
            End Set
        End Property

        Public Property DockPane_Option As Bitmap Implements IImageService.DockPane_Option
            Get
                Return _DockPane_Option
            End Get
            Friend Set(value As Bitmap)
                _DockPane_Option = value
            End Set
        End Property

        Public Property DockPane_OptionOverflow As Bitmap Implements IImageService.DockPane_OptionOverflow
            Get
                Return _DockPane_OptionOverflow
            End Get
            Friend Set(value As Bitmap)
                _DockPane_OptionOverflow = value
            End Set
        End Property
        Public ReadOnly Property DockPaneActive_Close As Bitmap Implements IImageService.DockPaneActive_Close
        Public ReadOnly Property DockPaneActive_Dock As Bitmap Implements IImageService.DockPaneActive_Dock
        Public ReadOnly Property DockPaneActive_Option As Bitmap Implements IImageService.DockPaneActive_Option

        Public Property DockPaneHover_Close As Bitmap Implements IImageService.DockPaneHover_Close
            Get
                Return _DockPaneHover_Close
            End Get
            Friend Set(value As Bitmap)
                _DockPaneHover_Close = value
            End Set
        End Property

        Public Property DockPaneHover_List As Bitmap Implements IImageService.DockPaneHover_List
            Get
                Return _DockPaneHover_List
            End Get
            Friend Set(value As Bitmap)
                _DockPaneHover_List = value
            End Set
        End Property

        Public Property DockPaneHover_Dock As Bitmap Implements IImageService.DockPaneHover_Dock
            Get
                Return _DockPaneHover_Dock
            End Get
            Friend Set(value As Bitmap)
                _DockPaneHover_Dock = value
            End Set
        End Property

        Public Property DockPaneActiveHover_AutoHide As Bitmap Implements IImageService.DockPaneActiveHover_AutoHide
            Get
                Return _DockPaneActiveHover_AutoHide
            End Get
            Friend Set(value As Bitmap)
                _DockPaneActiveHover_AutoHide = value
            End Set
        End Property

        Public Property DockPaneHover_Option As Bitmap Implements IImageService.DockPaneHover_Option
            Get
                Return _DockPaneHover_Option
            End Get
            Friend Set(value As Bitmap)
                _DockPaneHover_Option = value
            End Set
        End Property

        Public Property DockPaneHover_OptionOverflow As Bitmap Implements IImageService.DockPaneHover_OptionOverflow
            Get
                Return _DockPaneHover_OptionOverflow
            End Get
            Friend Set(value As Bitmap)
                _DockPaneHover_OptionOverflow = value
            End Set
        End Property

        Public Property DockPanePress_Close As Bitmap Implements IImageService.DockPanePress_Close
            Get
                Return _DockPanePress_Close
            End Get
            Friend Set(value As Bitmap)
                _DockPanePress_Close = value
            End Set
        End Property

        Public Property DockPanePress_List As Bitmap Implements IImageService.DockPanePress_List
            Get
                Return _DockPanePress_List
            End Get
            Friend Set(value As Bitmap)
                _DockPanePress_List = value
            End Set
        End Property

        Public Property DockPanePress_Dock As Bitmap Implements IImageService.DockPanePress_Dock
            Get
                Return _DockPanePress_Dock
            End Get
            Friend Set(value As Bitmap)
                _DockPanePress_Dock = value
            End Set
        End Property

        Public Property DockPanePress_AutoHide As Bitmap Implements IImageService.DockPanePress_AutoHide
            Get
                Return _DockPanePress_AutoHide
            End Get
            Friend Set(value As Bitmap)
                _DockPanePress_AutoHide = value
            End Set
        End Property

        Public Property DockPanePress_Option As Bitmap Implements IImageService.DockPanePress_Option
            Get
                Return _DockPanePress_Option
            End Get
            Friend Set(value As Bitmap)
                _DockPanePress_Option = value
            End Set
        End Property

        Public Property DockPanePress_OptionOverflow As Bitmap Implements IImageService.DockPanePress_OptionOverflow
            Get
                Return _DockPanePress_OptionOverflow
            End Get
            Friend Set(value As Bitmap)
                _DockPanePress_OptionOverflow = value
            End Set
        End Property
        Public ReadOnly Property DockPaneActiveHover_Close As Bitmap Implements IImageService.DockPaneActiveHover_Close
        Public ReadOnly Property DockPaneActiveHover_Dock As Bitmap Implements IImageService.DockPaneActiveHover_Dock
        Public ReadOnly Property DockPaneActiveHover_Option As Bitmap Implements IImageService.DockPaneActiveHover_Option

        Public Property TabHoverActive_Close As Image Implements IImageService.TabHoverActive_Close
            Get
                Return _TabHoverActive_Close
            End Get
            Friend Set(value As Image)
                _TabHoverActive_Close = value
            End Set
        End Property

        Public Property TabActive_Close As Image Implements IImageService.TabActive_Close
            Get
                Return _TabActive_Close
            End Get
            Friend Set(value As Image)
                _TabActive_Close = value
            End Set
        End Property

        Public Property TabInactive_Close As Image Implements IImageService.TabInactive_Close
            Get
                Return _TabInactive_Close
            End Get
            Friend Set(value As Image)
                _TabInactive_Close = value
            End Set
        End Property

        Public Property TabHoverInactive_Close As Image Implements IImageService.TabHoverInactive_Close
            Get
                Return _TabHoverInactive_Close
            End Get
            Friend Set(value As Image)
                _TabHoverInactive_Close = value
            End Set
        End Property

        Public Property TabHoverLostFocus_Close As Image Implements IImageService.TabHoverLostFocus_Close
            Get
                Return _TabHoverLostFocus_Close
            End Get
            Friend Set(value As Image)
                _TabHoverLostFocus_Close = value
            End Set
        End Property

        Public Property TabLostFocus_Close As Image Implements IImageService.TabLostFocus_Close
            Get
                Return _TabLostFocus_Close
            End Get
            Friend Set(value As Image)
                _TabLostFocus_Close = value
            End Set
        End Property
        Public ReadOnly Property TabPressActive_Close As Image Implements IImageService.TabPressActive_Close
        Public ReadOnly Property TabPressInactive_Close As Image Implements IImageService.TabPressInactive_Close
        Public ReadOnly Property TabPressLostFocus_Close As Image Implements IImageService.TabPressLostFocus_Close

        Private ReadOnly _palette As DockPanelColorPalette

        Public Sub New(theme As ThemeBase)
            _palette = theme.ColorPalette
            Dockindicator_PaneDiamond_Hotspot = My.Resources.Dockindicator_PaneDiamond_Hotspot
            DockIndicator_PaneDiamond_HotspotIndex = My.Resources.DockIndicator_PaneDiamond_HotspotIndex

            Dim arrow = _palette.DockTarget.GlyphArrow
            Dim outerBorder = _palette.DockTarget.Border
            Dim separator = _palette.DockTarget.ButtonBorder
            Dim innerBorder = _palette.DockTarget.Background
            Dim background = _palette.DockTarget.ButtonBackground
            Dim window = _palette.DockTarget.GlyphBorder
            Dim core = _palette.DockTarget.GlyphBackground
            Dim drawCore = core.ToArgb() <> background.ToArgb()

            Using layerArrow = GetLayerImage(arrow, 32, theme.PaintingService)
                Using layerWindow = GetLayerImage(window, 32, theme.PaintingService)
                    Using layerCore = If(drawCore, GetLayerImage(core, 32, theme.PaintingService), Nothing)
                        Using layerBorder = GetBackground(innerBorder, outerBorder, 40, theme.PaintingService)
                            Using bottom = GetDockIcon(My.Resources.MaskArrowBottom, layerArrow, My.Resources.MaskWindowBottom, layerWindow, My.Resources.MaskDock, background, theme.PaintingService, My.Resources.MaskCoreBottom, layerCore, separator)
                                Using center = GetDockIcon(Nothing, Nothing, My.Resources.MaskWindowCenter, layerWindow, My.Resources.MaskDock, background, theme.PaintingService, My.Resources.MaskCoreCenter, layerCore, separator)
                                    Using left = GetDockIcon(My.Resources.MaskArrowLeft, layerArrow, My.Resources.MaskWindowLeft, layerWindow, My.Resources.MaskDock, background, theme.PaintingService, My.Resources.MaskCoreLeft, layerCore, separator)
                                        Using right = GetDockIcon(My.Resources.MaskArrowRight, layerArrow, My.Resources.MaskWindowRight, layerWindow, My.Resources.MaskDock, background, theme.PaintingService, My.Resources.MaskCoreRight, layerCore, separator)
                                            Using top = GetDockIcon(My.Resources.MaskArrowTop, layerArrow, My.Resources.MaskWindowTop, layerWindow, My.Resources.MaskDock, background, theme.PaintingService, My.Resources.MaskCoreTop, layerCore, separator)
                                                DockIndicator_PanelBottom = GetDockImage(bottom, layerBorder)
                                                DockIndicator_PanelFill = GetDockImage(center, layerBorder)
                                                DockIndicator_PanelLeft = GetDockImage(left, layerBorder)
                                                DockIndicator_PanelRight = GetDockImage(right, layerBorder)
                                                DockIndicator_PanelTop = GetDockImage(top, layerBorder)

                                                Using five = GetFiveBackground(My.Resources.MaskDockFive, innerBorder, outerBorder, theme.PaintingService)
                                                    Dockindicator_PaneDiamond = CombineFive(five, bottom, center, left, right, top)
                                                    Dockindicator_PaneDiamond_Fill = CombineFive(five, bottom, center, left, right, top)
                                                End Using
                                            End Using
                                        End Using
                                    End Using
                                End Using
                            End Using
                        End Using
                    End Using
                End Using
            End Using

            TabActive_Close = GetImage(My.Resources.MaskTabClose, _palette.TabSelectedActive.Button, _palette.TabSelectedActive.Background)
            TabInactive_Close = GetImage(My.Resources.MaskTabClose, _palette.TabUnselectedHovered.Button, _palette.TabUnselectedHovered.Background)
            TabLostFocus_Close = GetImage(My.Resources.MaskTabClose, _palette.TabSelectedInactive.Button, _palette.TabSelectedInactive.Background)
            TabHoverActive_Close = GetImage(My.Resources.MaskTabClose, _palette.TabButtonSelectedActiveHovered.Glyph, _palette.TabButtonSelectedActiveHovered.Background, _palette.TabButtonSelectedActiveHovered.Border)
            TabHoverInactive_Close = GetImage(My.Resources.MaskTabClose, _palette.TabButtonUnselectedTabHoveredButtonHovered.Glyph, _palette.TabButtonUnselectedTabHoveredButtonHovered.Background, _palette.TabButtonUnselectedTabHoveredButtonHovered.Border)
            TabHoverLostFocus_Close = GetImage(My.Resources.MaskTabClose, _palette.TabButtonSelectedInactiveHovered.Glyph, _palette.TabButtonSelectedInactiveHovered.Background, _palette.TabButtonSelectedInactiveHovered.Border)

            TabPressActive_Close = GetImage(My.Resources.MaskTabClose, _palette.TabButtonSelectedActivePressed.Glyph, _palette.TabButtonSelectedActivePressed.Background, _palette.TabButtonSelectedActivePressed.Border)
            TabPressInactive_Close = GetImage(My.Resources.MaskTabClose, _palette.TabButtonUnselectedTabHoveredButtonPressed.Glyph, _palette.TabButtonUnselectedTabHoveredButtonPressed.Background, _palette.TabButtonUnselectedTabHoveredButtonPressed.Border)
            TabPressLostFocus_Close = GetImage(My.Resources.MaskTabClose, _palette.TabButtonSelectedInactivePressed.Glyph, _palette.TabButtonSelectedInactivePressed.Background, _palette.TabButtonSelectedInactivePressed.Border)

            DockPane_List = GetImage(My.Resources.MaskTabList, _palette.OverflowButtonDefault.Glyph, _palette.MainWindowActive.Background)
            DockPane_OptionOverflow = GetImage(My.Resources.MaskTabOverflow, _palette.OverflowButtonDefault.Glyph, _palette.MainWindowActive.Background)

            DockPaneHover_List = GetImage(My.Resources.MaskTabList, _palette.OverflowButtonHovered.Glyph, _palette.OverflowButtonHovered.Background, _palette.OverflowButtonHovered.Border)
            DockPaneHover_OptionOverflow = GetImage(My.Resources.MaskTabOverflow, _palette.OverflowButtonHovered.Glyph, _palette.OverflowButtonHovered.Background, _palette.OverflowButtonHovered.Border)

            DockPanePress_List = GetImage(My.Resources.MaskTabList, _palette.OverflowButtonPressed.Glyph, _palette.OverflowButtonPressed.Background, _palette.OverflowButtonPressed.Border)
            DockPanePress_OptionOverflow = GetImage(My.Resources.MaskTabOverflow, _palette.OverflowButtonPressed.Glyph, _palette.OverflowButtonPressed.Background, _palette.OverflowButtonPressed.Border)

            DockPane_Close = GetImage(My.Resources.MaskToolWindowClose, _palette.ToolWindowCaptionInactive.Button, _palette.ToolWindowCaptionInactive.Background)
            DockPane_Dock = GetImage(My.Resources.MaskToolWindowDock, _palette.ToolWindowCaptionInactive.Button, _palette.ToolWindowCaptionInactive.Background)
            DockPane_Option = GetImage(My.Resources.MaskToolWindowOption, _palette.ToolWindowCaptionInactive.Button, _palette.ToolWindowCaptionInactive.Background)

            DockPaneActive_Close = GetImage(My.Resources.MaskToolWindowClose, _palette.ToolWindowCaptionActive.Button, _palette.ToolWindowCaptionActive.Background)
            DockPaneActive_Dock = GetImage(My.Resources.MaskToolWindowDock, _palette.ToolWindowCaptionActive.Button, _palette.ToolWindowCaptionActive.Background)
            DockPaneActive_Option = GetImage(My.Resources.MaskToolWindowOption, _palette.ToolWindowCaptionActive.Button, _palette.ToolWindowCaptionActive.Background)
            DockPaneActive_AutoHide = GetImage(My.Resources.MaskToolWindowAutoHide, _palette.ToolWindowCaptionActive.Button, _palette.ToolWindowCaptionActive.Background)

            DockPaneHover_Close = GetImage(My.Resources.MaskToolWindowClose, _palette.ToolWindowCaptionButtonInactiveHovered.Glyph, _palette.ToolWindowCaptionButtonInactiveHovered.Background, _palette.ToolWindowCaptionButtonInactiveHovered.Border)
            DockPaneHover_Dock = GetImage(My.Resources.MaskToolWindowDock, _palette.ToolWindowCaptionButtonInactiveHovered.Glyph, _palette.ToolWindowCaptionButtonInactiveHovered.Background, _palette.ToolWindowCaptionButtonInactiveHovered.Border)
            DockPaneHover_Option = GetImage(My.Resources.MaskToolWindowOption, _palette.ToolWindowCaptionButtonInactiveHovered.Glyph, _palette.ToolWindowCaptionButtonInactiveHovered.Background, _palette.ToolWindowCaptionButtonInactiveHovered.Border)

            DockPaneActiveHover_Close = GetImage(My.Resources.MaskToolWindowClose, _palette.ToolWindowCaptionButtonActiveHovered.Glyph, _palette.ToolWindowCaptionButtonActiveHovered.Background, _palette.ToolWindowCaptionButtonActiveHovered.Border)
            DockPaneActiveHover_Dock = GetImage(My.Resources.MaskToolWindowDock, _palette.ToolWindowCaptionButtonActiveHovered.Glyph, _palette.ToolWindowCaptionButtonActiveHovered.Background, _palette.ToolWindowCaptionButtonActiveHovered.Border)
            DockPaneActiveHover_Option = GetImage(My.Resources.MaskToolWindowOption, _palette.ToolWindowCaptionButtonActiveHovered.Glyph, _palette.ToolWindowCaptionButtonActiveHovered.Background, _palette.ToolWindowCaptionButtonActiveHovered.Border)
            DockPaneActiveHover_AutoHide = GetImage(My.Resources.MaskToolWindowAutoHide, _palette.ToolWindowCaptionButtonActiveHovered.Glyph, _palette.ToolWindowCaptionButtonActiveHovered.Background, _palette.ToolWindowCaptionButtonActiveHovered.Border)

            DockPanePress_Close = GetImage(My.Resources.MaskToolWindowClose, _palette.ToolWindowCaptionButtonPressed.Glyph, _palette.ToolWindowCaptionButtonPressed.Background, _palette.ToolWindowCaptionButtonPressed.Border)
            DockPanePress_Dock = GetImage(My.Resources.MaskToolWindowDock, _palette.ToolWindowCaptionButtonPressed.Glyph, _palette.ToolWindowCaptionButtonPressed.Background, _palette.ToolWindowCaptionButtonPressed.Border)
            DockPanePress_Option = GetImage(My.Resources.MaskToolWindowOption, _palette.ToolWindowCaptionButtonPressed.Glyph, _palette.ToolWindowCaptionButtonPressed.Background, _palette.ToolWindowCaptionButtonPressed.Border)
            DockPanePress_AutoHide = GetImage(My.Resources.MaskToolWindowAutoHide, _palette.ToolWindowCaptionButtonPressed.Glyph, _palette.ToolWindowCaptionButtonPressed.Background, _palette.ToolWindowCaptionButtonPressed.Border)
        End Sub
    End Class
End Namespace
