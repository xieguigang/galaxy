Public Module Tools

    Public Sub OpenUrlWithDefaultBrowser(url As String)
        Try
            Dim startInfo As New ProcessStartInfo(url)
            startInfo.UseShellExecute = True  ' 启用系统关联程序（即默认浏览器）
            Process.Start(startInfo)
        Catch ex As Exception
            ' 异常处理见下文
        End Try
    End Sub
End Module
