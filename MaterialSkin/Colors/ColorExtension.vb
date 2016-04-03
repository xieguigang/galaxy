Imports System.Runtime.CompilerServices

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
