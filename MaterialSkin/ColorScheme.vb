Imports System.Collections.Generic
Imports System.Drawing
Imports System.Runtime.CompilerServices

Public Class ColorScheme
    Public ReadOnly PrimaryColor As Color, DarkPrimaryColor As Color, LightPrimaryColor As Color, AccentColor As Color, TextColor As Color
    Public ReadOnly PrimaryPen As Pen, DarkPrimaryPen As Pen, LightPrimaryPen As Pen, AccentPen As Pen, TextPen As Pen
    Public ReadOnly PrimaryBrush As Brush, DarkPrimaryBrush As Brush, LightPrimaryBrush As Brush, AccentBrush As Brush, TextBrush As Brush

    ''' <summary>
    ''' Defines the Color Scheme to be used for all forms.
    ''' </summary>
    ''' <param name="primary">The primary color, a -500 color is suggested here.</param>
    ''' <param name="darkPrimary">A darker version of the primary color, a -700 color is suggested here.</param>
    ''' <param name="lightPrimary">A lighter version of the primary color, a -100 color is suggested here.</param>
    ''' <param name="accent">The accent color, a -200 color is suggested here.</param>
    ''' <param name="textShade">The text color, the one with the highest contrast is suggested.</param>
    Public Sub New(primary As Primary, darkPrimary As Primary, lightPrimary As Primary, accent As Accent, textShade As TextShade)
        'Color
        PrimaryColor = CInt(primary).ToColor()
        DarkPrimaryColor = CInt(darkPrimary).ToColor()
        LightPrimaryColor = CInt(lightPrimary).ToColor()
        AccentColor = CInt(accent).ToColor()
        TextColor = CInt(textShade).ToColor()

        'Pen
        PrimaryPen = New Pen(PrimaryColor)
        DarkPrimaryPen = New Pen(DarkPrimaryColor)
        LightPrimaryPen = New Pen(LightPrimaryColor)
        AccentPen = New Pen(AccentColor)
        TextPen = New Pen(TextColor)

        'Brush
        PrimaryBrush = New SolidBrush(PrimaryColor)
        DarkPrimaryBrush = New SolidBrush(DarkPrimaryColor)
        LightPrimaryBrush = New SolidBrush(LightPrimaryColor)
        AccentBrush = New SolidBrush(AccentColor)
        TextBrush = New SolidBrush(TextColor)
    End Sub
End Class

Public Module ColorExtension

    ''' <summary>
    ''' Convert an integer number to a Color.
    ''' </summary>
    ''' <returns></returns>
    <Extension>
    Public Function ToColor(argb As Integer) As Color
        Return Color.FromArgb((argb And &HFF0000) >> 16, (argb And &HFF00) >> 8, argb And &HFF)
    End Function

    ''' <summary>
    ''' Removes the alpha component of a color.
    ''' </summary>
    ''' <param name="color"></param>
    ''' <returns></returns>
    <Extension>
    Public Function RemoveAlpha(color As Color) As Color
        Return Color.FromArgb(color.R, color.G, color.B)
    End Function

    ''' <summary>
    ''' Converts a 0-100 integer to a 0-255 color component.
    ''' </summary>
    ''' <param name="percentage"></param>
    ''' <returns></returns>
    <Extension>
    Public Function PercentageToColorComponent(percentage As Integer) As Integer
        Return CInt(Math.Truncate((percentage / 100.0) * 255.0))
    End Function
End Module

'Color constantes
Public Enum TextShade
        WHITE = &HFFFFFF
        BLACK = &H212121
    End Enum

    Public Enum Primary
        Red50 = &HFFEBEE
        Red100 = &HFFCDD2
        Red200 = &HEF9A9A
        Red300 = &HE57373
        Red400 = &HEF5350
        Red500 = &HF44336
        Red600 = &HE53935
        Red700 = &HD32F2F
        Red800 = &HC62828
        Red900 = &HB71C1C
        Pink50 = &HFCE4EC
        Pink100 = &HF8BBD0
        Pink200 = &HF48FB1
        Pink300 = &HF06292
        Pink400 = &HEC407A
        Pink500 = &HE91E63
        Pink600 = &HD81B60
        Pink700 = &HC2185B
        Pink800 = &HAD1457
        Pink900 = &H880E4F
        Purple50 = &HF3E5F5
        Purple100 = &HE1BEE7
        Purple200 = &HCE93D8
        Purple300 = &HBA68C8
        Purple400 = &HAB47BC
        Purple500 = &H9C27B0
        Purple600 = &H8E24AA
        Purple700 = &H7B1FA2
        Purple800 = &H6A1B9A
        Purple900 = &H4A148C
        DeepPurple50 = &HEDE7F6
        DeepPurple100 = &HD1C4E9
        DeepPurple200 = &HB39DDB
        DeepPurple300 = &H9575CD
        DeepPurple400 = &H7E57C2
        DeepPurple500 = &H673AB7
        DeepPurple600 = &H5E35B1
        DeepPurple700 = &H512DA8
        DeepPurple800 = &H4527A0
        DeepPurple900 = &H311B92
        Indigo50 = &HE8EAF6
        Indigo100 = &HC5CAE9
        Indigo200 = &H9FA8DA
        Indigo300 = &H7986CB
        Indigo400 = &H5C6BC0
        Indigo500 = &H3F51B5
        Indigo600 = &H3949AB
        Indigo700 = &H303F9F
        Indigo800 = &H283593
        Indigo900 = &H1A237E
        Blue50 = &HE3F2FD
        Blue100 = &HBBDEFB
        Blue200 = &H90CAF9
        Blue300 = &H64B5F6
        Blue400 = &H42A5F5
        Blue500 = &H2196F3
        Blue600 = &H1E88E5
        Blue700 = &H1976D2
        Blue800 = &H1565C0
        Blue900 = &HD47A1
        LightBlue50 = &HE1F5FE
        LightBlue100 = &HB3E5FC
        LightBlue200 = &H81D4FA
        LightBlue300 = &H4FC3F7
        LightBlue400 = &H29B6F6
        LightBlue500 = &H3A9F4
        LightBlue600 = &H39BE5
        LightBlue700 = &H288D1
        LightBlue800 = &H277BD
        LightBlue900 = &H1579B
        Cyan50 = &HE0F7FA
        Cyan100 = &HB2EBF2
        Cyan200 = &H80DEEA
        Cyan300 = &H4DD0E1
        Cyan400 = &H26C6DA
        Cyan500 = &HBCD4
        Cyan600 = &HACC1
        Cyan700 = &H97A7
        Cyan800 = &H838F
        Cyan900 = &H6064
        Teal50 = &HE0F2F1
        Teal100 = &HB2DFDB
        Teal200 = &H80CBC4
        Teal300 = &H4DB6AC
        Teal400 = &H26A69A
        Teal500 = &H9688
        Teal600 = &H897B
        Teal700 = &H796B
        Teal800 = &H695C
        Teal900 = &H4D40
        Green50 = &HE8F5E9
        Green100 = &HC8E6C9
        Green200 = &HA5D6A7
        Green300 = &H81C784
        Green400 = &H66BB6A
        Green500 = &H4CAF50
        Green600 = &H43A047
        Green700 = &H388E3C
        Green800 = &H2E7D32
        Green900 = &H1B5E20
        LightGreen50 = &HF1F8E9
        LightGreen100 = &HDCEDC8
        LightGreen200 = &HC5E1A5
        LightGreen300 = &HAED581
        LightGreen400 = &H9CCC65
        LightGreen500 = &H8BC34A
        LightGreen600 = &H7CB342
        LightGreen700 = &H689F38
        LightGreen800 = &H558B2F
        LightGreen900 = &H33691E
        Lime50 = &HF9FBE7
        Lime100 = &HF0F4C3
        Lime200 = &HE6EE9C
        Lime300 = &HDCE775
        Lime400 = &HD4E157
        Lime500 = &HCDDC39
        Lime600 = &HC0CA33
        Lime700 = &HAFB42B
        Lime800 = &H9E9D24
        Lime900 = &H827717
        Yellow50 = &HFFFDE7
        Yellow100 = &HFFF9C4
        Yellow200 = &HFFF59D
        Yellow300 = &HFFF176
        Yellow400 = &HFFEE58
        Yellow500 = &HFFEB3B
        Yellow600 = &HFDD835
        Yellow700 = &HFBC02D
        Yellow800 = &HF9A825
        Yellow900 = &HF57F17
        Amber50 = &HFFF8E1
        Amber100 = &HFFECB3
        Amber200 = &HFFE082
        Amber300 = &HFFD54F
        Amber400 = &HFFCA28
        Amber500 = &HFFC107
        Amber600 = &HFFB300
        Amber700 = &HFFA000
        Amber800 = &HFF8F00
        Amber900 = &HFF6F00
        Orange50 = &HFFF3E0
        Orange100 = &HFFE0B2
        Orange200 = &HFFCC80
        Orange300 = &HFFB74D
        Orange400 = &HFFA726
        Orange500 = &HFF9800
        Orange600 = &HFB8C00
        Orange700 = &HF57C00
        Orange800 = &HEF6C00
        Orange900 = &HE65100
        DeepOrange50 = &HFBE9E7
        DeepOrange100 = &HFFCCBC
        DeepOrange200 = &HFFAB91
        DeepOrange300 = &HFF8A65
        DeepOrange400 = &HFF7043
        DeepOrange500 = &HFF5722
        DeepOrange600 = &HF4511E
        DeepOrange700 = &HE64A19
        DeepOrange800 = &HD84315
        DeepOrange900 = &HBF360C
        Brown50 = &HEFEBE9
        Brown100 = &HD7CCC8
        Brown200 = &HBCAAA4
        Brown300 = &HA1887F
        Brown400 = &H8D6E63
        Brown500 = &H795548
        Brown600 = &H6D4C41
        Brown700 = &H5D4037
        Brown800 = &H4E342E
        Brown900 = &H3E2723
        Grey50 = &HFAFAFA
        Grey100 = &HF5F5F5
        Grey200 = &HEEEEEE
        Grey300 = &HE0E0E0
        Grey400 = &HBDBDBD
        Grey500 = &H9E9E9E
        Grey600 = &H757575
        Grey700 = &H616161
        Grey800 = &H424242
        Grey900 = &H212121
        BlueGrey50 = &HECEFF1
        BlueGrey100 = &HCFD8DC
        BlueGrey200 = &HB0BEC5
        BlueGrey300 = &H90A4AE
        BlueGrey400 = &H78909C
        BlueGrey500 = &H607D8B
        BlueGrey600 = &H546E7A
        BlueGrey700 = &H455A64
        BlueGrey800 = &H37474F
        BlueGrey900 = &H263238

    End Enum

    Public Enum Accent
        Red100 = &HFF8A80
        Red200 = &HFF5252
        Red400 = &HFF1744
        Red700 = &HD50000
        Pink100 = &HFF80AB
        Pink200 = &HFF4081
        Pink400 = &HF50057
        Pink700 = &HC51162
        Purple100 = &HEA80FC
        Purple200 = &HE040FB
        Purple400 = &HD500F9
        Purple700 = &HAA00FF
        DeepPurple100 = &HB388FF
        DeepPurple200 = &H7C4DFF
        DeepPurple400 = &H651FFF
        DeepPurple700 = &H6200EA
        Indigo100 = &H8C9EFF
        Indigo200 = &H536DFE
        Indigo400 = &H3D5AFE
        Indigo700 = &H304FFE
        Blue100 = &H82B1FF
        Blue200 = &H448AFF
        Blue400 = &H2979FF
        Blue700 = &H2962FF
        LightBlue100 = &H80D8FF
        LightBlue200 = &H40C4FF
        LightBlue400 = &HB0FF
        LightBlue700 = &H91EA
        Cyan100 = &H84FFFF
        Cyan200 = &H18FFFF
        Cyan400 = &HE5FF
        Cyan700 = &HB8D4
        Teal100 = &HA7FFEB
        Teal200 = &H64FFDA
        Teal400 = &H1DE9B6
        Teal700 = &HBFA5
        Green100 = &HB9F6CA
        Green200 = &H69F0AE
        Green400 = &HE676
        Green700 = &HC853
        LightGreen100 = &HCCFF90
        LightGreen200 = &HB2FF59
        LightGreen400 = &H76FF03
        LightGreen700 = &H64DD17
        Lime100 = &HF4FF81
        Lime200 = &HEEFF41
        Lime400 = &HC6FF00
        Lime700 = &HAEEA00
        Yellow100 = &HFFFF8D
        Yellow200 = &HFFFF00
        Yellow400 = &HFFEA00
        Yellow700 = &HFFD600
        Amber100 = &HFFE57F
        Amber200 = &HFFD740
        Amber400 = &HFFC400
        Amber700 = &HFFAB00
        Orange100 = &HFFD180
        Orange200 = &HFFAB40
        Orange400 = &HFF9100
        Orange700 = &HFF6D00
        DeepOrange100 = &HFF9E80
        DeepOrange200 = &HFF6E40
        DeepOrange400 = &HFF3D00
        DeepOrange700 = &HDD2C00
    End Enum
