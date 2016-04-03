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
