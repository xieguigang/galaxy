Imports ThemeVS2015.WeifenLuo.WinFormsUI.ThemeVS2015

Namespace WeifenLuo.WinFormsUI.Docking

    ''' <summary>
    ''' Visual Studio 2015 Light theme.
    ''' </summary>
    Public Class VS2015BlueTheme
        Inherits VS2015ThemeBase
        Public Sub New()
            MyBase.New(Decompress(My.Resources.vs2015blue_vstheme))
        End Sub
    End Class
End Namespace
