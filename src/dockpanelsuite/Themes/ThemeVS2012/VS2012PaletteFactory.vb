﻿Imports System.IO
Imports Microsoft.VisualStudio.WinForms.Docking

Namespace ThemeVS2012
    Public Class VS2012PaletteFactory
        Implements IPaletteFactory
        Private Const Env As String = "Environment"
        Private _xml As XDocument

        Public Sub New(file As Byte())
            _xml = XDocument.Load(New StreamReader(New MemoryStream(file)))
        End Sub

        Public Sub Initialize(palette As DockPanelColorPalette) Implements IPaletteFactory.Initialize
            palette.AutoHideStripDefault.Background = ColorTranslatorFromHtml("AutoHideTabBackgroundBegin")
            palette.AutoHideStripDefault.Border = ColorTranslatorFromHtml("AutoHideTabBorder")
            palette.AutoHideStripDefault.Text = ColorTranslatorFromHtml("AutoHideTabText")

            palette.AutoHideStripHovered.Background = ColorTranslatorFromHtml("AutoHideTabMouseOverBackgroundBegin")
            palette.AutoHideStripHovered.Border = ColorTranslatorFromHtml("AutoHideTabMouseOverBorder")
            palette.AutoHideStripHovered.Text = ColorTranslatorFromHtml("AutoHideTabMouseOverText")

            palette.CommandBarMenuDefault.Background = ColorTranslatorFromHtml("CommandShelfHighlightGradientBegin")
            palette.CommandBarMenuDefault.Text = ColorTranslatorFromHtml("CommandBarTextActive")

            palette.CommandBarMenuPopupDefault.Arrow = ColorTranslatorFromHtml("CommandBarMenuSubmenuGlyph")
            palette.CommandBarMenuPopupDefault.BackgroundBottom = ColorTranslatorFromHtml("CommandBarMenuBackgroundGradientEnd")
            palette.CommandBarMenuPopupDefault.BackgroundTop = ColorTranslatorFromHtml("CommandBarMenuBackgroundGradientBegin")
            palette.CommandBarMenuPopupDefault.Border = ColorTranslatorFromHtml("CommandBarMenuBorder")
            palette.CommandBarMenuPopupDefault.Checkmark = ColorTranslatorFromHtml("CommandBarCheckBox")
            palette.CommandBarMenuPopupDefault.CheckmarkBackground = ColorTranslatorFromHtml("CommandBarSelectedIcon")
            palette.CommandBarMenuPopupDefault.IconBackground = ColorTranslatorFromHtml("CommandBarMenuIconBackground")
            palette.CommandBarMenuPopupDefault.Separator = ColorTranslatorFromHtml("CommandBarMenuSeparator")

            palette.CommandBarMenuPopupDisabled.Checkmark = ColorTranslatorFromHtml("CommandBarCheckBoxDisabled")
            palette.CommandBarMenuPopupDisabled.CheckmarkBackground = ColorTranslatorFromHtml("CommandBarSelectedIconDisabled")
            palette.CommandBarMenuPopupDisabled.Text = ColorTranslatorFromHtml("CommandBarTextInactive")

            palette.CommandBarMenuPopupHovered.Arrow = ColorTranslatorFromHtml("CommandBarMenuMouseOverSubmenuGlyph")
            palette.CommandBarMenuPopupHovered.Checkmark = ColorTranslatorFromHtml("CommandBarCheckBoxMouseOver")
            palette.CommandBarMenuPopupHovered.CheckmarkBackground = ColorTranslatorFromHtml("CommandBarHoverOverSelectedIcon")
            palette.CommandBarMenuPopupHovered.ItemBackground = ColorTranslatorFromHtml("CommandBarMenuItemMouseOver")
            palette.CommandBarMenuPopupHovered.Text = ColorTranslatorFromHtml("CommandBarMenuItemMouseOver", True)

            palette.CommandBarMenuTopLevelHeaderHovered.Background = ColorTranslatorFromHtml("CommandBarMouseOverBackgroundBegin")
            palette.CommandBarMenuTopLevelHeaderHovered.Border = ColorTranslatorFromHtml("CommandBarBorder")
            palette.CommandBarMenuTopLevelHeaderHovered.Text = ColorTranslatorFromHtml("CommandBarTextHover")

            palette.CommandBarToolbarDefault.Background = ColorTranslatorFromHtml("CommandBarGradientBegin")
            palette.CommandBarToolbarDefault.Border = ColorTranslatorFromHtml("CommandBarToolBarBorder")
            palette.CommandBarToolbarDefault.Grip = ColorTranslatorFromHtml("CommandBarDragHandle")
            palette.CommandBarToolbarDefault.OverflowButtonBackground = ColorTranslatorFromHtml("CommandBarOptionsBackground")
            palette.CommandBarToolbarDefault.OverflowButtonGlyph = ColorTranslatorFromHtml("CommandBarOptionsGlyph")
            palette.CommandBarToolbarDefault.Separator = ColorTranslatorFromHtml("CommandBarToolBarSeparator")
            palette.CommandBarToolbarDefault.SeparatorAccent = ColorTranslatorFromHtml("CommandBarToolBarSeparatorHighlight")
            palette.CommandBarToolbarDefault.Tray = ColorTranslatorFromHtml("CommandShelfBackgroundGradientBegin")

            palette.CommandBarToolbarButtonChecked.Background = ColorTranslatorFromHtml("CommandBarSelected")
            palette.CommandBarToolbarButtonChecked.Border = ColorTranslatorFromHtml("CommandBarSelectedBorder")
            palette.CommandBarToolbarButtonChecked.Text = ColorTranslatorFromHtml("CommandBarTextSelected")

            palette.CommandBarToolbarButtonCheckedHovered.Border = ColorTranslatorFromHtml("CommandBarHoverOverSelectedIconBorder")
            palette.CommandBarToolbarButtonCheckedHovered.Text = ColorTranslatorFromHtml("CommandBarTextHoverOverSelected")

            palette.CommandBarToolbarButtonDefault.Arrow = ColorTranslatorFromHtml("DropDownGlyph")

            palette.CommandBarToolbarButtonHovered.Arrow = ColorTranslatorFromHtml("DropDownMouseOverGlyph")
            palette.CommandBarToolbarButtonHovered.Separator = ColorTranslatorFromHtml("CommandBarSplitButtonSeparator")

            palette.CommandBarToolbarButtonPressed.Arrow = ColorTranslatorFromHtml("DropDownMouseDownGlyph")
            palette.CommandBarToolbarButtonPressed.Background = ColorTranslatorFromHtml("CommandBarMouseDownBackgroundBegin")
            palette.CommandBarToolbarButtonPressed.Text = ColorTranslatorFromHtml("CommandBarTextMouseDown")

            palette.CommandBarToolbarOverflowHovered.Background = ColorTranslatorFromHtml("CommandBarOptionsMouseOverBackgroundBegin")
            palette.CommandBarToolbarOverflowHovered.Glyph = ColorTranslatorFromHtml("CommandBarOptionsMouseOverGlyph")

            palette.CommandBarToolbarOverflowPressed.Background = ColorTranslatorFromHtml("CommandBarOptionsMouseDownBackgroundBegin")
            palette.CommandBarToolbarOverflowPressed.Glyph = ColorTranslatorFromHtml("CommandBarOptionsMouseDownGlyph")

            palette.OverflowButtonDefault.Glyph = ColorTranslatorFromHtml("DocWellOverflowButtonGlyph")

            palette.OverflowButtonHovered.Background = ColorTranslatorFromHtml("DocWellOverflowButtonMouseOverBackground")
            palette.OverflowButtonHovered.Border = ColorTranslatorFromHtml("DocWellOverflowButtonMouseOverBorder")
            palette.OverflowButtonHovered.Glyph = ColorTranslatorFromHtml("DocWellOverflowButtonMouseOverGlyph")

            palette.OverflowButtonPressed.Background = ColorTranslatorFromHtml("DocWellOverflowButtonMouseDownBackground")
            palette.OverflowButtonPressed.Border = ColorTranslatorFromHtml("DocWellOverflowButtonMouseDownBorder")
            palette.OverflowButtonPressed.Glyph = ColorTranslatorFromHtml("DocWellOverflowButtonMouseDownGlyph")

            palette.TabSelectedActive.Background = ColorTranslatorFromHtml("FileTabSelectedBorder")
            palette.TabSelectedActive.Button = ColorTranslatorFromHtml("FileTabButtonSelectedActiveGlyph")
            palette.TabSelectedActive.Text = ColorTranslatorFromHtml("FileTabSelectedText")

            palette.TabSelectedInactive.Background = ColorTranslatorFromHtml("FileTabInactiveBorder")
            palette.TabSelectedInactive.Button = ColorTranslatorFromHtml("FileTabButtonSelectedInactiveGlyph")
            palette.TabSelectedInactive.Text = ColorTranslatorFromHtml("FileTabInactiveText")

            palette.TabUnselected.Text = ColorTranslatorFromHtml("FileTabText")
            palette.TabUnselected.Background = ColorTranslatorFromHtml("FileTabBackground")

            palette.TabUnselectedHovered.Background = ColorTranslatorFromHtml("FileTabHotBorder")
            palette.TabUnselectedHovered.Button = ColorTranslatorFromHtml("FileTabHotGlyph")
            palette.TabUnselectedHovered.Text = ColorTranslatorFromHtml("FileTabHotText")

            palette.TabButtonSelectedActiveHovered.Background = ColorTranslatorFromHtml("FileTabButtonHoverSelectedActive")
            palette.TabButtonSelectedActiveHovered.Border = ColorTranslatorFromHtml("FileTabButtonHoverSelectedActiveBorder")
            palette.TabButtonSelectedActiveHovered.Glyph = ColorTranslatorFromHtml("FileTabButtonHoverSelectedActiveGlyph")

            palette.TabButtonSelectedActivePressed.Background = ColorTranslatorFromHtml("FileTabButtonDownSelectedActive")
            palette.TabButtonSelectedActivePressed.Border = ColorTranslatorFromHtml("FileTabButtonDownSelectedActiveBorder")
            palette.TabButtonSelectedActivePressed.Glyph = ColorTranslatorFromHtml("FileTabButtonDownSelectedActiveGlyph")

            palette.TabButtonSelectedInactiveHovered.Background = ColorTranslatorFromHtml("FileTabButtonHoverSelectedInactive")
            palette.TabButtonSelectedInactiveHovered.Border = ColorTranslatorFromHtml("FileTabButtonHoverSelectedInactiveBorder")
            palette.TabButtonSelectedInactiveHovered.Glyph = ColorTranslatorFromHtml("FileTabButtonHoverSelectedInactiveGlyph")

            palette.TabButtonSelectedInactivePressed.Background = ColorTranslatorFromHtml("FileTabButtonDownSelectedInactive")
            palette.TabButtonSelectedInactivePressed.Border = ColorTranslatorFromHtml("FileTabButtonDownSelectedInactiveBorder")
            palette.TabButtonSelectedInactivePressed.Glyph = ColorTranslatorFromHtml("FileTabButtonDownSelectedInactiveGlyph")

            palette.TabButtonUnselectedTabHoveredButtonHovered.Background = ColorTranslatorFromHtml("FileTabButtonHoverInactive")
            palette.TabButtonUnselectedTabHoveredButtonHovered.Border = ColorTranslatorFromHtml("FileTabButtonHoverInactiveBorder")
            palette.TabButtonUnselectedTabHoveredButtonHovered.Glyph = ColorTranslatorFromHtml("FileTabButtonHoverInactiveGlyph")

            palette.TabButtonUnselectedTabHoveredButtonPressed.Background = ColorTranslatorFromHtml("FileTabButtonDownInactive")
            palette.TabButtonUnselectedTabHoveredButtonPressed.Border = ColorTranslatorFromHtml("FileTabButtonDownInactiveBorder")
            palette.TabButtonUnselectedTabHoveredButtonPressed.Glyph = ColorTranslatorFromHtml("FileTabButtonDownInactiveGlyph")

            palette.MainWindowActive.Background = ColorTranslatorFromHtml("EnvironmentBackground")
            palette.MainWindowStatusBarDefault.Background = ColorTranslatorFromHtml("StatusBarDefault")
            palette.MainWindowStatusBarDefault.Highlight = ColorTranslatorFromHtml("StatusBarHighlight")
            palette.MainWindowStatusBarDefault.HighlightText = ColorTranslatorFromHtml("StatusBarHighlight", True)
            palette.MainWindowStatusBarDefault.ResizeGrip = ColorTranslatorFromHtml("MainWindowResizeGripTexture1")
            palette.MainWindowStatusBarDefault.ResizeGripAccent = ColorTranslatorFromHtml("MainWindowResizeGripTexture2")
            palette.MainWindowStatusBarDefault.Text = ColorTranslatorFromHtml("StatusBarText")

            palette.ToolWindowCaptionActive.Background = ColorTranslatorFromHtml("TitleBarActiveBorder")
            palette.ToolWindowCaptionActive.Button = ColorTranslatorFromHtml("ToolWindowButtonActiveGlyph")
            palette.ToolWindowCaptionActive.Grip = ColorTranslatorFromHtml("TitleBarDragHandleActive")
            palette.ToolWindowCaptionActive.Text = ColorTranslatorFromHtml("TitleBarActiveText")

            palette.ToolWindowCaptionInactive.Background = ColorTranslatorFromHtml("TitleBarInactive")
            palette.ToolWindowCaptionInactive.Button = ColorTranslatorFromHtml("ToolWindowButtonInactiveGlyph")
            palette.ToolWindowCaptionInactive.Grip = ColorTranslatorFromHtml("TitleBarDragHandle")
            palette.ToolWindowCaptionInactive.Text = ColorTranslatorFromHtml("TitleBarInactiveText")

            palette.ToolWindowCaptionButtonActiveHovered.Background = ColorTranslatorFromHtml("ToolWindowButtonHoverActive")
            palette.ToolWindowCaptionButtonActiveHovered.Border = ColorTranslatorFromHtml("ToolWindowButtonHoverActiveBorder")
            palette.ToolWindowCaptionButtonActiveHovered.Glyph = ColorTranslatorFromHtml("ToolWindowButtonHoverActiveGlyph")

            palette.ToolWindowCaptionButtonPressed.Background = ColorTranslatorFromHtml("ToolWindowButtonDown")
            palette.ToolWindowCaptionButtonPressed.Border = ColorTranslatorFromHtml("ToolWindowButtonDownBorder")
            palette.ToolWindowCaptionButtonPressed.Glyph = ColorTranslatorFromHtml("ToolWindowButtonDownActiveGlyph")

            palette.ToolWindowCaptionButtonInactiveHovered.Background = ColorTranslatorFromHtml("ToolWindowButtonHoverInactive")
            palette.ToolWindowCaptionButtonInactiveHovered.Border = ColorTranslatorFromHtml("ToolWindowButtonHoverInactiveBorder")
            palette.ToolWindowCaptionButtonInactiveHovered.Glyph = ColorTranslatorFromHtml("ToolWindowButtonHoverInactiveGlyph")

            palette.ToolWindowTabSelectedActive.Background = ColorTranslatorFromHtml("ToolWindowTabSelectedTab")
            palette.ToolWindowTabSelectedActive.Text = ColorTranslatorFromHtml("ToolWindowTabSelectedActiveText")

            palette.ToolWindowTabSelectedInactive.Background = palette.ToolWindowTabSelectedActive.Background
            palette.ToolWindowTabSelectedInactive.Text = ColorTranslatorFromHtml("ToolWindowTabSelectedText")

            palette.ToolWindowTabUnselected.Text = ColorTranslatorFromHtml("ToolWindowTabText")
            palette.ToolWindowTabUnselected.Background = ColorTranslatorFromHtml("ToolWindowTabGradientBegin")

            palette.ToolWindowTabUnselectedHovered.Background = ColorTranslatorFromHtml("ToolWindowTabMouseOverBackgroundBegin")
            palette.ToolWindowTabUnselectedHovered.Text = ColorTranslatorFromHtml("ToolWindowTabMouseOverText")

            palette.ToolWindowSeparator = ColorTranslatorFromHtml("ToolWindowTabSeparator")
            palette.ToolWindowBorder = ColorTranslatorFromHtml("ToolWindowBorder")

            palette.DockTarget.Background = ColorTranslatorFromHtml("DockTargetBackground")
            palette.DockTarget.Border = ColorTranslatorFromHtml("DockTargetBorder")
            palette.DockTarget.ButtonBackground = ColorTranslatorFromHtml("DockTargetButtonBackgroundBegin")
            palette.DockTarget.ButtonBorder = ColorTranslatorFromHtml("DockTargetButtonBorder")
            palette.DockTarget.GlyphBackground = ColorTranslatorFromHtml("DockTargetGlyphBackgroundBegin")
            palette.DockTarget.GlyphArrow = ColorTranslatorFromHtml("DockTargetGlyphArrow")
            palette.DockTarget.GlyphBorder = ColorTranslatorFromHtml("DockTargetGlyphBorder")
        End Sub

        Private Function ColorTranslatorFromHtml(name As String, Optional foreground As Boolean = False) As Color
            Dim color = _xml.Root.Element("Theme").Elements("Category").FirstOrDefault(Function(item) Equals(item.Attribute("Name").Value, Env))?.Elements("Color").FirstOrDefault(Function(item) Equals(item.Attribute("Name").Value, name))?.Element(If(foreground, "Foreground", "Background")).Attribute("Source").Value
            If Equals(color, Nothing) Then
                Return Drawing.Color.Transparent
            End If

            Return ColorTranslator.FromHtml($"#{color}")
        End Function
    End Class
End Namespace
