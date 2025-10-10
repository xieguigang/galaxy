Imports System.Drawing
Imports System.Windows.Forms

Namespace Docking
    Public Class VisualStudioColorTable
        Inherits ProfessionalColorTable
        Private _palette As DockPanelColorPalette

        Public Sub New(palette As DockPanelColorPalette)
            _palette = palette
        End Sub

        Public ReadOnly Property ButtonCheckedHoveredBorder As Color
            Get
                Return _palette.CommandBarToolbarButtonCheckedHovered.Border
            End Get
        End Property

        Public ReadOnly Property ButtonCheckedHoveredBackground As Color
            Get
                Return _palette.CommandBarMenuPopupHovered.CheckmarkBackground
            End Get
        End Property

        Public ReadOnly Property ButtonCheckedBorder As Color
            Get
                Return _palette.CommandBarToolbarButtonChecked.Border
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonCheckedGradientBegin As Color
            Get
                Return _palette.CommandBarToolbarButtonChecked.Background
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonCheckedGradientMiddle As Color
            Get
                Return _palette.CommandBarToolbarButtonChecked.Background
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonCheckedGradientEnd As Color
            Get
                Return _palette.CommandBarToolbarButtonChecked.Background
            End Get
        End Property

        'public override Color ButtonCheckedHighlight
        '{
        '    get { return _palette.CommandBarMenuPopupDefault.CheckmarkBackground; }
        '}

        'public override Color ButtonCheckedHighlightBorder
        '{
        '    get { return _palette.CommandBarMenuPopupDefault.Checkmark; }
        '}

        Public Overrides ReadOnly Property CheckBackground As Color
            Get
                Return _palette.CommandBarMenuPopupDefault.CheckmarkBackground
            End Get
        End Property

        Public Overrides ReadOnly Property CheckSelectedBackground As Color
            Get
                Return _palette.CommandBarMenuPopupHovered.CheckmarkBackground
            End Get
        End Property

        Public Overrides ReadOnly Property CheckPressedBackground As Color
            Get
                Return _palette.CommandBarMenuPopupHovered.CheckmarkBackground
            End Get
        End Property

        'public override Color ButtonPressedHighlight
        '{
        '    get { return ButtonPressedGradientMiddle; }
        '}

        'public override Color ButtonPressedHighlightBorder
        '{
        '    get { return ButtonPressedBorder; }
        '}

        Public Overrides ReadOnly Property ButtonPressedBorder As Color
            Get
                Return _palette.CommandBarMenuTopLevelHeaderHovered.Border
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonPressedGradientBegin As Color
            Get
                Return _palette.CommandBarToolbarButtonPressed.Background
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonPressedGradientMiddle As Color
            Get
                Return _palette.CommandBarToolbarButtonPressed.Background
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonPressedGradientEnd As Color
            Get
                Return _palette.CommandBarToolbarButtonPressed.Background
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemPressedGradientBegin As Color
            Get
                Return _palette.CommandBarMenuPopupDefault.BackgroundTop
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemPressedGradientMiddle As Color
            Get
                Return _palette.CommandBarMenuPopupDefault.BackgroundTop
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemPressedGradientEnd As Color
            Get
                Return _palette.CommandBarMenuPopupDefault.BackgroundTop
            End Get
        End Property

        'public override Color ButtonSelectedHighlight
        '{
        '    get { return Color.Red; }
        '}
        'public override Color ButtonSelectedHighlightBorder
        '{
        '    get { return ButtonSelectedBorder; }
        '}

        Public Overrides ReadOnly Property ButtonSelectedBorder As Color
            Get
                Return _palette.CommandBarToolbarButtonChecked.Border
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonSelectedGradientBegin As Color
            Get
                Return _palette.CommandBarMenuTopLevelHeaderHovered.Background
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonSelectedGradientMiddle As Color
            Get
                Return _palette.CommandBarMenuTopLevelHeaderHovered.Background
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonSelectedGradientEnd As Color
            Get
                Return _palette.CommandBarMenuTopLevelHeaderHovered.Background
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemSelected As Color
            Get
                Return _palette.CommandBarMenuPopupHovered.ItemBackground
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemSelectedGradientBegin As Color
            Get
                Return _palette.CommandBarMenuTopLevelHeaderHovered.Background
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemSelectedGradientEnd As Color
            Get
                Return _palette.CommandBarMenuTopLevelHeaderHovered.Background
            End Get
        End Property

        Public Overrides ReadOnly Property GripDark As Color
            Get
                Return _palette.CommandBarToolbarDefault.Grip
            End Get
        End Property

        Public Overrides ReadOnly Property GripLight As Color
            Get
                Return _palette.CommandBarToolbarDefault.Grip
            End Get
        End Property

        Public Overrides ReadOnly Property ImageMarginGradientBegin As Color
            Get
                Return _palette.CommandBarMenuPopupDefault.IconBackground
            End Get
        End Property

        Public Overrides ReadOnly Property ImageMarginGradientMiddle As Color
            Get
                Return _palette.CommandBarMenuPopupDefault.IconBackground
            End Get
        End Property

        Public Overrides ReadOnly Property ImageMarginGradientEnd As Color
            Get
                Return _palette.CommandBarMenuPopupDefault.IconBackground
            End Get
        End Property

        'public override Color ImageMarginRevealedGradientBegin
        '{
        '    get { return Color.FromArgb(255, 231, 232, 236); }
        '}

        'public override Color ImageMarginRevealedGradientMiddle
        '{
        '    get { return Color.FromArgb(255, 231, 232, 236); }
        '}

        'public override Color ImageMarginRevealedGradientEnd
        '{
        '    get { return Color.FromArgb(255, 231, 232, 236); }
        '}

        Public Overrides ReadOnly Property MenuStripGradientBegin As Color
            Get
                Return _palette.CommandBarMenuDefault.Background
            End Get
        End Property

        Public Overrides ReadOnly Property MenuStripGradientEnd As Color
            Get
                Return _palette.CommandBarMenuDefault.Background
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemBorder As Color
            Get
                Return _palette.CommandBarMenuTopLevelHeaderHovered.Border
            End Get
        End Property

        Public Overrides ReadOnly Property MenuBorder As Color
            Get
                Return _palette.CommandBarMenuPopupDefault.Border
            End Get
        End Property

        'public override Color RaftingContainerGradientBegin
        '{
        '    get { return Color.FromArgb(255, 186, 192, 201); }
        '}
        'public override Color RaftingContainerGradientEnd
        '{
        '    get { return Color.FromArgb(255, 186, 192, 201); }
        '}

        Public Overrides ReadOnly Property SeparatorDark As Color
            Get
                Return _palette.CommandBarToolbarDefault.Separator
            End Get
        End Property

        Public Overrides ReadOnly Property SeparatorLight As Color
            Get
                Return _palette.CommandBarToolbarDefault.SeparatorAccent
            End Get
        End Property

        Public Overrides ReadOnly Property StatusStripGradientBegin As Color
            Get
                Return _palette.MainWindowStatusBarDefault.Background
            End Get
        End Property
        Public Overrides ReadOnly Property StatusStripGradientEnd As Color
            Get
                Return _palette.MainWindowStatusBarDefault.Background
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripBorder As Color
            Get
                Return _palette.CommandBarToolbarDefault.Border
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripDropDownBackground As Color
            Get
                Return _palette.CommandBarMenuPopupDefault.BackgroundBottom
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripGradientBegin As Color
            Get
                Return _palette.CommandBarToolbarDefault.Background
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripGradientMiddle As Color
            Get
                Return _palette.CommandBarToolbarDefault.Background
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripGradientEnd As Color
            Get
                Return _palette.CommandBarToolbarDefault.Background
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripContentPanelGradientBegin As Color
            Get
                Return _palette.CommandBarMenuDefault.Background
            End Get
        End Property
        Public Overrides ReadOnly Property ToolStripContentPanelGradientEnd As Color
            Get
                Return _palette.CommandBarMenuDefault.Background
            End Get
        End Property
        Public Overrides ReadOnly Property ToolStripPanelGradientBegin As Color
            Get
                Return _palette.CommandBarMenuDefault.Background
            End Get
        End Property
        Public Overrides ReadOnly Property ToolStripPanelGradientEnd As Color
            Get
                Return _palette.CommandBarMenuDefault.Background
            End Get
        End Property

        Public Overrides ReadOnly Property OverflowButtonGradientBegin As Color
            Get
                Return _palette.CommandBarToolbarDefault.OverflowButtonBackground
            End Get
        End Property
        Public Overrides ReadOnly Property OverflowButtonGradientMiddle As Color
            Get
                Return _palette.CommandBarToolbarDefault.OverflowButtonBackground
            End Get
        End Property
        Public Overrides ReadOnly Property OverflowButtonGradientEnd As Color
            Get
                Return _palette.CommandBarToolbarDefault.OverflowButtonBackground
            End Get
        End Property
    End Class

End Namespace
