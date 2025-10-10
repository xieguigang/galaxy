Namespace ThemeVS2015

    ''' <summary>
    ''' Visual Studio 2015 Light theme.
    ''' </summary>
    Public Class VS2015LightTheme
        Inherits VS2015ThemeBase
        Public Sub New()
            MyBase.New(Decompress(My.Resources.vs2015light_vstheme))
        End Sub
    End Class
End Namespace
