Namespace ThemeVS2015

    ''' <summary>
    ''' Visual Studio 2015 Dark theme.
    ''' </summary>
    Public Class VS2015DarkTheme
        Inherits VS2015ThemeBase
        Public Sub New()
            MyBase.New(Decompress(My.Resources.vs2015dark_vstheme))
        End Sub
    End Class
End Namespace
