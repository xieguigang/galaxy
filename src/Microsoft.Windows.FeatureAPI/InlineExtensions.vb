Imports System.Windows.Forms

Public Module InlineExtensions

    Public ReadOnly Property MainWindow As Form

    Public ReadOnly Property MainWindowHandle As IntPtr
        Get
            If Not MainWindow Is Nothing Then
                Return MainWindow.Handle
            Else
                Return System.Diagnostics.Process.GetCurrentProcess.MainWindowHandle
            End If
        End Get
    End Property

    Public Sub Hook(main As Form)
        _MainWindow = main
    End Sub

End Module
