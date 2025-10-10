Imports System.Drawing

Namespace Docking
    Public Class DockPanelColorPalette
        Public Sub New(factory As IPaletteFactory)
            factory.Initialize(Me)
        End Sub

        Public ReadOnly Property AutoHideStripDefault As AutoHideStripPalette = New AutoHideStripPalette()
        Public ReadOnly Property AutoHideStripHovered As AutoHideStripPalette = New AutoHideStripPalette()
        Public ReadOnly Property OverflowButtonDefault As ButtonPalette = New ButtonPalette()
        Public ReadOnly Property OverflowButtonHovered As HoveredButtonPalette = New HoveredButtonPalette()
        Public ReadOnly Property OverflowButtonPressed As HoveredButtonPalette = New HoveredButtonPalette()
        Public ReadOnly Property TabSelectedActive As TabPalette = New TabPalette()
        Public ReadOnly Property TabSelectedInactive As TabPalette = New TabPalette()
        Public ReadOnly Property TabUnselected As UnselectedTabPalette = New UnselectedTabPalette()
        Public ReadOnly Property TabUnselectedHovered As TabPalette = New TabPalette()
        Public ReadOnly Property TabButtonSelectedActiveHovered As HoveredButtonPalette = New HoveredButtonPalette()
        Public ReadOnly Property TabButtonSelectedActivePressed As HoveredButtonPalette = New HoveredButtonPalette()
        Public ReadOnly Property TabButtonSelectedInactiveHovered As HoveredButtonPalette = New HoveredButtonPalette()
        Public ReadOnly Property TabButtonSelectedInactivePressed As HoveredButtonPalette = New HoveredButtonPalette()
        Public ReadOnly Property TabButtonUnselectedTabHoveredButtonHovered As HoveredButtonPalette = New HoveredButtonPalette()
        Public ReadOnly Property TabButtonUnselectedTabHoveredButtonPressed As HoveredButtonPalette = New HoveredButtonPalette()
        Public ReadOnly Property MainWindowActive As MainWindowPalette = New MainWindowPalette()
        Public ReadOnly Property MainWindowStatusBarDefault As MainWindowStatusBarPalette = New MainWindowStatusBarPalette()
        Public ReadOnly Property ToolWindowCaptionActive As ToolWindowCaptionPalette = New ToolWindowCaptionPalette()
        Public ReadOnly Property ToolWindowCaptionInactive As ToolWindowCaptionPalette = New ToolWindowCaptionPalette()
        Public ReadOnly Property ToolWindowCaptionButtonActiveHovered As HoveredButtonPalette = New HoveredButtonPalette()
        Public ReadOnly Property ToolWindowCaptionButtonPressed As HoveredButtonPalette = New HoveredButtonPalette()
        Public ReadOnly Property ToolWindowCaptionButtonInactiveHovered As HoveredButtonPalette = New HoveredButtonPalette()
        Public ReadOnly Property ToolWindowTabSelectedActive As ToolWindowTabPalette = New ToolWindowTabPalette()
        Public ReadOnly Property ToolWindowTabSelectedInactive As ToolWindowTabPalette = New ToolWindowTabPalette()
        Public ReadOnly Property ToolWindowTabUnselected As ToolWindowUnselectedTabPalette = New ToolWindowUnselectedTabPalette()
        Public ReadOnly Property ToolWindowTabUnselectedHovered As ToolWindowTabPalette = New ToolWindowTabPalette()
        Public Property ToolWindowBorder As Color
        Public Property ToolWindowSeparator As Color
        Public ReadOnly Property DockTarget As DockTargetPalette = New DockTargetPalette()
        Public ReadOnly Property CommandBarMenuDefault As CommandBarMenuPalette = New CommandBarMenuPalette()
        Public ReadOnly Property CommandBarMenuPopupDefault As CommandBarMenuPopupPalette = New CommandBarMenuPopupPalette()
        Public ReadOnly Property CommandBarMenuPopupDisabled As CommandBarMenuPopupDisabledPalette = New CommandBarMenuPopupDisabledPalette()
        Public ReadOnly Property CommandBarMenuPopupHovered As CommandBarMenuPopupHoveredPalette = New CommandBarMenuPopupHoveredPalette()
        Public ReadOnly Property CommandBarMenuTopLevelHeaderHovered As CommandBarMenuTopLevelHeaderPalette = New CommandBarMenuTopLevelHeaderPalette()
        Public ReadOnly Property CommandBarToolbarDefault As CommandBarToolbarPalette = New CommandBarToolbarPalette()
        Public ReadOnly Property CommandBarToolbarButtonChecked As CommandBarToolbarButtonCheckedPalette = New CommandBarToolbarButtonCheckedPalette()
        Public ReadOnly Property CommandBarToolbarButtonCheckedHovered As CommandBarToolbarButtonCheckedHoveredPalette = New CommandBarToolbarButtonCheckedHoveredPalette()
        Public ReadOnly Property CommandBarToolbarButtonDefault As CommandBarToolbarButtonPalette = New CommandBarToolbarButtonPalette()
        Public ReadOnly Property CommandBarToolbarButtonHovered As CommandBarToolbarButtonHoveredPalette = New CommandBarToolbarButtonHoveredPalette()
        Public ReadOnly Property CommandBarToolbarButtonPressed As CommandBarToolbarButtonPressedPalette = New CommandBarToolbarButtonPressedPalette()
        Public ReadOnly Property CommandBarToolbarOverflowHovered As CommandBarToolbarOverflowButtonPalette = New CommandBarToolbarOverflowButtonPalette()
        Public ReadOnly Property CommandBarToolbarOverflowPressed As CommandBarToolbarOverflowButtonPalette = New CommandBarToolbarOverflowButtonPalette()

        Public ReadOnly Property ColorTable As VisualStudioColorTable
    End Class

    Public Class CommandBarToolbarOverflowButtonPalette
        Public Property Background As Color
        Public Property Glyph As Color
    End Class

    Public Class CommandBarToolbarButtonPressedPalette
        Public Property Arrow As Color
        Public Property Background As Color
        Public Property Text As Color
    End Class

    Public Class CommandBarToolbarButtonHoveredPalette
        Public Property Arrow As Color
        Public Property Separator As Color
    End Class

    Public Class CommandBarToolbarButtonPalette
        Public Property Arrow As Color
    End Class

    Public Class CommandBarToolbarButtonCheckedHoveredPalette
        Public Property Border As Color
        Public Property Text As Color
    End Class

    Public Class CommandBarToolbarButtonCheckedPalette
        Public Property Background As Color
        Public Property Border As Color
        Public Property Text As Color
    End Class

    Public Class CommandBarToolbarPalette
        Public Property Background As Color
        Public Property Border As Color
        Public Property Grip As Color
        Public Property OverflowButtonBackground As Color
        Public Property OverflowButtonGlyph As Color
        Public Property Separator As Color
        Public Property SeparatorAccent As Color
        Public Property Tray As Color
    End Class

    Public Class CommandBarMenuTopLevelHeaderPalette
        Public Property Background As Color
        Public Property Border As Color
        Public Property Text As Color
    End Class

    Public Class CommandBarMenuPopupHoveredPalette
        Public Property Arrow As Color
        Public Property Checkmark As Color
        Public Property CheckmarkBackground As Color
        Public Property ItemBackground As Color
        Public Property Text As Color
    End Class

    Public Class CommandBarMenuPopupDisabledPalette
        Public Property Checkmark As Color
        Public Property CheckmarkBackground As Color
        Public Property Text As Color
    End Class

    Public Class CommandBarMenuPopupPalette
        Public Property Arrow As Color
        Public Property BackgroundBottom As Color
        Public Property BackgroundTop As Color
        Public Property Border As Color
        Public Property Checkmark As Color
        Public Property CheckmarkBackground As Color
        Public Property IconBackground As Color
        Public Property Separator As Color
    End Class

    Public Class CommandBarMenuPalette
        Public Property Background As Color
        Public Property Text As Color
    End Class

    Public Interface IPaletteFactory
        Sub Initialize(palette As DockPanelColorPalette)
    End Interface

    Public Class DockTargetPalette
        Public Property Background As Color
        Public Property Border As Color
        Public Property ButtonBackground As Color
        Public Property ButtonBorder As Color
        Public Property GlyphBackground As Color
        Public Property GlyphArrow As Color
        Public Property GlyphBorder As Color
    End Class

    Public Class HoveredButtonPalette
        Public Property Background As Color
        Public Property Border As Color
        Public Property Glyph As Color
    End Class

    Public Class ButtonPalette
        Public Property Glyph As Color
    End Class

    Public Class MainWindowPalette
        Public Property Background As Color
    End Class

    Public Class MainWindowStatusBarPalette
        Public Property Background As Color
        Public Property Highlight As Color
        Public Property HighlightText As Color
        Public Property ResizeGrip As Color
        Public Property ResizeGripAccent As Color
        Public Property Text As Color
    End Class

    Public Class ToolWindowTabPalette
        Public Property Background As Color
        Public Property Text As Color
    End Class

    Public Class ToolWindowUnselectedTabPalette
        Public Property Background As Color ' VS2013
        Public Property Text As Color
    End Class

    Public Class ToolWindowCaptionPalette
        Public Property Background As Color
        Public Property Button As Color
        Public Property Grip As Color
        Public Property Text As Color
    End Class

    Public Class TabPalette
        Public Property Background As Color
        Public Property Button As Color
        Public Property Text As Color
    End Class

    Public Class UnselectedTabPalette
        Public Property Background As Color ' VS2013 only
        Public Property Text As Color
    End Class

    Public Class AutoHideStripPalette
        Public Property Background As Color
        Public Property Border As Color
        Public Property Text As Color
    End Class
End Namespace
