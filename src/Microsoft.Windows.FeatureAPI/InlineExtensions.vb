Imports System.Windows.Forms

Public Module InlineExtensions

    Public ReadOnly Property MainWindow As Form

    Public Sub Hook(main As Form)
        _MainWindow = main
    End Sub

End Module
