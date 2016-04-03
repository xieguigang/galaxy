Imports System.Collections.Generic
Imports System.Drawing
Imports System.Drawing.Text
Imports System.Linq
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports MaterialSkin.Controls

Public Class MaterialSkinManager
    'Singleton instance
    Private Shared m_instance As MaterialSkinManager

    'Forms to control
    Private ReadOnly formsToManage As New List(Of MaterialForm)()

    'Theme
    Private m_theme As Themes
    Public Property Theme() As Themes
        Get
            Return m_theme
        End Get
        Set
            m_theme = value
            UpdateBackgrounds()
        End Set
    End Property

    Private m_colorScheme As ColorScheme
    Public Property ColorScheme() As ColorScheme
        Get
            Return m_colorScheme
        End Get
        Set
            m_colorScheme = value
            UpdateBackgrounds()
        End Set
    End Property

    Public Enum Themes As Byte
        LIGHT
        DARK
    End Enum

    'Constant color values
    Private Shared ReadOnly PRIMARY_TEXT_BLACK As Color = Color.FromArgb(222, 0, 0, 0)
    Private Shared ReadOnly PRIMARY_TEXT_BLACK_BRUSH As Brush = New SolidBrush(PRIMARY_TEXT_BLACK)
    Public Shared SECONDARY_TEXT_BLACK As Color = Color.FromArgb(138, 0, 0, 0)
    Public Shared SECONDARY_TEXT_BLACK_BRUSH As Brush = New SolidBrush(SECONDARY_TEXT_BLACK)
    Private Shared ReadOnly DISABLED_OR_HINT_TEXT_BLACK As Color = Color.FromArgb(66, 0, 0, 0)
    Private Shared ReadOnly DISABLED_OR_HINT_TEXT_BLACK_BRUSH As Brush = New SolidBrush(DISABLED_OR_HINT_TEXT_BLACK)
    Private Shared ReadOnly DIVIDERS_BLACK As Color = Color.FromArgb(31, 0, 0, 0)
    Private Shared ReadOnly DIVIDERS_BLACK_BRUSH As Brush = New SolidBrush(DIVIDERS_BLACK)

    Private Shared ReadOnly PRIMARY_TEXT_WHITE As Color = Color.FromArgb(255, 255, 255, 255)
    Private Shared ReadOnly PRIMARY_TEXT_WHITE_BRUSH As Brush = New SolidBrush(PRIMARY_TEXT_WHITE)
    Public Shared SECONDARY_TEXT_WHITE As Color = Color.FromArgb(179, 255, 255, 255)
    Public Shared SECONDARY_TEXT_WHITE_BRUSH As Brush = New SolidBrush(SECONDARY_TEXT_WHITE)
    Private Shared ReadOnly DISABLED_OR_HINT_TEXT_WHITE As Color = Color.FromArgb(77, 255, 255, 255)
    Private Shared ReadOnly DISABLED_OR_HINT_TEXT_WHITE_BRUSH As Brush = New SolidBrush(DISABLED_OR_HINT_TEXT_WHITE)
    Private Shared ReadOnly DIVIDERS_WHITE As Color = Color.FromArgb(31, 255, 255, 255)
    Private Shared ReadOnly DIVIDERS_WHITE_BRUSH As Brush = New SolidBrush(DIVIDERS_WHITE)

    ' Checkbox colors
    Private Shared ReadOnly CHECKBOX_OFF_LIGHT As Color = Color.FromArgb(138, 0, 0, 0)
    Private Shared ReadOnly CHECKBOX_OFF_LIGHT_BRUSH As Brush = New SolidBrush(CHECKBOX_OFF_LIGHT)
    Private Shared ReadOnly CHECKBOX_OFF_DISABLED_LIGHT As Color = Color.FromArgb(66, 0, 0, 0)
    Private Shared ReadOnly CHECKBOX_OFF_DISABLED_LIGHT_BRUSH As Brush = New SolidBrush(CHECKBOX_OFF_DISABLED_LIGHT)

    Private Shared ReadOnly CHECKBOX_OFF_DARK As Color = Color.FromArgb(179, 255, 255, 255)
    Private Shared ReadOnly CHECKBOX_OFF_DARK_BRUSH As Brush = New SolidBrush(CHECKBOX_OFF_DARK)
    Private Shared ReadOnly CHECKBOX_OFF_DISABLED_DARK As Color = Color.FromArgb(77, 255, 255, 255)
    Private Shared ReadOnly CHECKBOX_OFF_DISABLED_DARK_BRUSH As Brush = New SolidBrush(CHECKBOX_OFF_DISABLED_DARK)

    'Raised button
    Private Shared ReadOnly RAISED_BUTTON_BACKGROUND As Color = Color.FromArgb(255, 255, 255, 255)
    Private Shared ReadOnly RAISED_BUTTON_BACKGROUND_BRUSH As Brush = New SolidBrush(RAISED_BUTTON_BACKGROUND)
    Private Shared ReadOnly RAISED_BUTTON_TEXT_LIGHT As Color = PRIMARY_TEXT_WHITE
    Private Shared ReadOnly RAISED_BUTTON_TEXT_LIGHT_BRUSH As Brush = New SolidBrush(RAISED_BUTTON_TEXT_LIGHT)
    Private Shared ReadOnly RAISED_BUTTON_TEXT_DARK As Color = PRIMARY_TEXT_BLACK
    Private Shared ReadOnly RAISED_BUTTON_TEXT_DARK_BRUSH As Brush = New SolidBrush(RAISED_BUTTON_TEXT_DARK)

    'Flat button
    Private Shared ReadOnly FLAT_BUTTON_BACKGROUND_HOVER_LIGHT As Color = Color.FromArgb(20.PercentageToColorComponent(), &H999999.ToColor())
    Private Shared ReadOnly FLAT_BUTTON_BACKGROUND_HOVER_LIGHT_BRUSH As Brush = New SolidBrush(FLAT_BUTTON_BACKGROUND_HOVER_LIGHT)
    Private Shared ReadOnly FLAT_BUTTON_BACKGROUND_PRESSED_LIGHT As Color = Color.FromArgb(40.PercentageToColorComponent(), &H999999.ToColor())
    Private Shared ReadOnly FLAT_BUTTON_BACKGROUND_PRESSED_LIGHT_BRUSH As Brush = New SolidBrush(FLAT_BUTTON_BACKGROUND_PRESSED_LIGHT)
    Private Shared ReadOnly FLAT_BUTTON_DISABLEDTEXT_LIGHT As Color = Color.FromArgb(26.PercentageToColorComponent(), &H0.ToColor())
    Private Shared ReadOnly FLAT_BUTTON_DISABLEDTEXT_LIGHT_BRUSH As Brush = New SolidBrush(FLAT_BUTTON_DISABLEDTEXT_LIGHT)

    Private Shared ReadOnly FLAT_BUTTON_BACKGROUND_HOVER_DARK As Color = Color.FromArgb(15.PercentageToColorComponent(), &HCCCCCC.ToColor())
    Private Shared ReadOnly FLAT_BUTTON_BACKGROUND_HOVER_DARK_BRUSH As Brush = New SolidBrush(FLAT_BUTTON_BACKGROUND_HOVER_DARK)
    Private Shared ReadOnly FLAT_BUTTON_BACKGROUND_PRESSED_DARK As Color = Color.FromArgb(25.PercentageToColorComponent(), &HCCCCCC.ToColor())
    Private Shared ReadOnly FLAT_BUTTON_BACKGROUND_PRESSED_DARK_BRUSH As Brush = New SolidBrush(FLAT_BUTTON_BACKGROUND_PRESSED_DARK)
    Private Shared ReadOnly FLAT_BUTTON_DISABLEDTEXT_DARK As Color = Color.FromArgb(30.PercentageToColorComponent(), &HFFFFFF.ToColor())
    Private Shared ReadOnly FLAT_BUTTON_DISABLEDTEXT_DARK_BRUSH As Brush = New SolidBrush(FLAT_BUTTON_DISABLEDTEXT_DARK)

    'ContextMenuStrip
    Private Shared ReadOnly CMS_BACKGROUND_LIGHT_HOVER As Color = Color.FromArgb(255, 238, 238, 238)
    Private Shared ReadOnly CMS_BACKGROUND_HOVER_LIGHT_BRUSH As Brush = New SolidBrush(CMS_BACKGROUND_LIGHT_HOVER)

    Private Shared ReadOnly CMS_BACKGROUND_DARK_HOVER As Color = Color.FromArgb(38, 204, 204, 204)
    Private Shared ReadOnly CMS_BACKGROUND_HOVER_DARK_BRUSH As Brush = New SolidBrush(CMS_BACKGROUND_DARK_HOVER)

    'Application background
    Private Shared ReadOnly BACKGROUND_LIGHT As Color = Color.FromArgb(255, 255, 255, 255)
    Private Shared BACKGROUND_LIGHT_BRUSH As Brush = New SolidBrush(BACKGROUND_LIGHT)

    Private Shared ReadOnly BACKGROUND_DARK As Color = Color.FromArgb(255, 51, 51, 51)
    Private Shared BACKGROUND_DARK_BRUSH As Brush = New SolidBrush(BACKGROUND_DARK)

    'Application action bar
    Public ReadOnly ACTION_BAR_TEXT As Color = Color.FromArgb(255, 255, 255, 255)
    Public ReadOnly ACTION_BAR_TEXT_BRUSH As Brush = New SolidBrush(Color.FromArgb(255, 255, 255, 255))
    Public ReadOnly ACTION_BAR_TEXT_SECONDARY As Color = Color.FromArgb(153, 255, 255, 255)
    Public ReadOnly ACTION_BAR_TEXT_SECONDARY_BRUSH As Brush = New SolidBrush(Color.FromArgb(153, 255, 255, 255))

    Public Function GetPrimaryTextColor() As Color
        Return (If(Theme = Themes.LIGHT, PRIMARY_TEXT_BLACK, PRIMARY_TEXT_WHITE))
    End Function

    Public Function GetPrimaryTextBrush() As Brush
        Return (If(Theme = Themes.LIGHT, PRIMARY_TEXT_BLACK_BRUSH, PRIMARY_TEXT_WHITE_BRUSH))
    End Function

    Public Function GetSecondaryTextColor() As Color
        Return (If(Theme = Themes.LIGHT, SECONDARY_TEXT_BLACK, SECONDARY_TEXT_WHITE))
    End Function

    Public Function GetSecondaryTextBrush() As Brush
        Return (If(Theme = Themes.LIGHT, SECONDARY_TEXT_BLACK_BRUSH, SECONDARY_TEXT_WHITE_BRUSH))
    End Function

    Public Function GetDisabledOrHintColor() As Color
        Return (If(Theme = Themes.LIGHT, DISABLED_OR_HINT_TEXT_BLACK, DISABLED_OR_HINT_TEXT_WHITE))
    End Function

    Public Function GetDisabledOrHintBrush() As Brush
        Return (If(Theme = Themes.LIGHT, DISABLED_OR_HINT_TEXT_BLACK_BRUSH, DISABLED_OR_HINT_TEXT_WHITE_BRUSH))
    End Function

    Public Function GetDividersColor() As Color
        Return (If(Theme = Themes.LIGHT, DIVIDERS_BLACK, DIVIDERS_WHITE))
    End Function

    Public Function GetDividersBrush() As Brush
        Return (If(Theme = Themes.LIGHT, DIVIDERS_BLACK_BRUSH, DIVIDERS_WHITE_BRUSH))
    End Function

    Public Function GetCheckboxOffColor() As Color
        Return (If(Theme = Themes.LIGHT, CHECKBOX_OFF_LIGHT, CHECKBOX_OFF_DARK))
    End Function

    Public Function GetCheckboxOffBrush() As Brush
        Return (If(Theme = Themes.LIGHT, CHECKBOX_OFF_LIGHT_BRUSH, CHECKBOX_OFF_DARK_BRUSH))
    End Function

    Public Function GetCheckBoxOffDisabledColor() As Color
        Return (If(Theme = Themes.LIGHT, CHECKBOX_OFF_DISABLED_LIGHT, CHECKBOX_OFF_DISABLED_DARK))
    End Function

    Public Function GetCheckBoxOffDisabledBrush() As Brush
        Return (If(Theme = Themes.LIGHT, CHECKBOX_OFF_DISABLED_LIGHT_BRUSH, CHECKBOX_OFF_DISABLED_DARK_BRUSH))
    End Function

    Public Function GetRaisedButtonBackgroundBrush() As Brush
        Return RAISED_BUTTON_BACKGROUND_BRUSH
    End Function

    Public Function GetRaisedButtonTextBrush(primary As Boolean) As Brush
        Return (If(primary, RAISED_BUTTON_TEXT_LIGHT_BRUSH, RAISED_BUTTON_TEXT_DARK_BRUSH))
    End Function

    Public Function GetFlatButtonHoverBackgroundColor() As Color
        Return (If(Theme = Themes.LIGHT, FLAT_BUTTON_BACKGROUND_HOVER_LIGHT, FLAT_BUTTON_BACKGROUND_HOVER_DARK))
    End Function

    Public Function GetFlatButtonHoverBackgroundBrush() As Brush
        Return (If(Theme = Themes.LIGHT, FLAT_BUTTON_BACKGROUND_HOVER_LIGHT_BRUSH, FLAT_BUTTON_BACKGROUND_HOVER_DARK_BRUSH))
    End Function

    Public Function GetFlatButtonPressedBackgroundColor() As Color
        Return (If(Theme = Themes.LIGHT, FLAT_BUTTON_BACKGROUND_PRESSED_LIGHT, FLAT_BUTTON_BACKGROUND_PRESSED_DARK))
    End Function

    Public Function GetFlatButtonPressedBackgroundBrush() As Brush
        Return (If(Theme = Themes.LIGHT, FLAT_BUTTON_BACKGROUND_PRESSED_LIGHT_BRUSH, FLAT_BUTTON_BACKGROUND_PRESSED_DARK_BRUSH))
    End Function

    Public Function GetFlatButtonDisabledTextBrush() As Brush
        Return (If(Theme = Themes.LIGHT, FLAT_BUTTON_DISABLEDTEXT_LIGHT_BRUSH, FLAT_BUTTON_DISABLEDTEXT_DARK_BRUSH))
    End Function

    Public Function GetCmsSelectedItemBrush() As Brush
        Return (If(Theme = Themes.LIGHT, CMS_BACKGROUND_HOVER_LIGHT_BRUSH, CMS_BACKGROUND_HOVER_DARK_BRUSH))
    End Function

    Public Function GetApplicationBackgroundColor() As Color
        Return (If(Theme = Themes.LIGHT, BACKGROUND_LIGHT, BACKGROUND_DARK))
    End Function

    'Roboto font
    Public ROBOTO_MEDIUM_12 As Font
    Public ROBOTO_REGULAR_11 As Font
    Public ROBOTO_MEDIUM_11 As Font
    Public ROBOTO_MEDIUM_10 As Font

    'Other constants
    Public FORM_PADDING As Integer = 14

    <DllImport("gdi32.dll")>
    Private Shared Function AddFontMemResourceEx(pbFont As IntPtr, cbFont As UInteger, pvd As IntPtr, <[In]> ByRef pcFonts As UInteger) As IntPtr
    End Function

    Private Sub New()
        ROBOTO_MEDIUM_12 = New Font(LoadFont(My.Resources.Roboto_Medium), 12.0F)
        ROBOTO_MEDIUM_10 = New Font(LoadFont(My.Resources.Roboto_Medium), 10.0F)
        ROBOTO_REGULAR_11 = New Font(LoadFont(My.Resources.Roboto_Regular), 11.0F)
        ROBOTO_MEDIUM_11 = New Font(LoadFont(My.Resources.Roboto_Medium), 11.0F)
        Theme = Themes.LIGHT
        ColorScheme = New ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE)
    End Sub

    Public Shared ReadOnly Property Instance() As MaterialSkinManager
        Get
            If m_instance Is Nothing Then
                m_instance = New MaterialSkinManager
            End If
            Return m_instance
        End Get
    End Property

    Public Sub AddFormToManage(materialForm As MaterialForm)
        formsToManage.Add(materialForm)
        UpdateBackgrounds()
    End Sub

    Public Sub RemoveFormToManage(materialForm As MaterialForm)
        formsToManage.Remove(materialForm)
    End Sub

    Private ReadOnly privateFontCollection As New PrivateFontCollection()

    Private Function LoadFont(fontResource As Byte()) As FontFamily
        Dim dataLength As Integer = fontResource.Length
        Dim fontPtr As IntPtr = Marshal.AllocCoTaskMem(dataLength)
        Marshal.Copy(fontResource, 0, fontPtr, dataLength)

        Dim cFonts As UInteger = 0
        AddFontMemResourceEx(fontPtr, CUInt(fontResource.Length), IntPtr.Zero, cFonts)
        privateFontCollection.AddMemoryFont(fontPtr, dataLength)

        Return privateFontCollection.Families.Last()
    End Function

    Private Sub UpdateBackgrounds()
        Dim newBackColor As Color = GetApplicationBackgroundColor()
        For Each materialForm As Controls.MaterialForm In formsToManage
            materialForm.BackColor = newBackColor
            UpdateControl(materialForm, newBackColor)
        Next
    End Sub

    Private Sub UpdateToolStrip(toolStrip As ToolStrip, newBackColor As Color)
        If toolStrip Is Nothing Then
            Return
        End If

        toolStrip.BackColor = newBackColor
        For Each control As ToolStripItem In toolStrip.Items
            control.BackColor = newBackColor
            If TypeOf control Is MaterialToolStripMenuItem AndAlso TryCast(control, MaterialToolStripMenuItem).HasDropDown Then

                'recursive call
                UpdateToolStrip(TryCast(control, MaterialToolStripMenuItem).DropDown, newBackColor)
            End If
        Next
    End Sub

    Private Sub UpdateControl(controlToUpdate As Control, newBackColor As Color)
        If controlToUpdate Is Nothing Then
            Return
        End If

        If controlToUpdate.ContextMenuStrip IsNot Nothing Then
            UpdateToolStrip(controlToUpdate.ContextMenuStrip, newBackColor)
        End If
        Dim tabControl As MaterialTabControl = TryCast(controlToUpdate, MaterialTabControl)
        If tabControl IsNot Nothing Then
            For Each tabPage As TabPage In tabControl.TabPages
                tabPage.BackColor = newBackColor
            Next
        End If

        If TypeOf controlToUpdate Is MaterialDivider Then
            controlToUpdate.BackColor = GetDividersColor()
        End If

        If TypeOf controlToUpdate Is MaterialListView Then

            controlToUpdate.BackColor = newBackColor
        End If

        'recursive call
        For Each control As Control In controlToUpdate.Controls
            UpdateControl(control, newBackColor)
        Next

        controlToUpdate.Invalidate()
    End Sub
End Class
