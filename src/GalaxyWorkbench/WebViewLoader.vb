Imports Microsoft.Web.WebView2.Core
Imports Microsoft.Web.WebView2.WinForms

Public Module WebViewLoader

    Public Sub DeveloperOptions(WebView21 As WebView2, enable As Boolean, Optional TabText As String = "WebKit")
        WebView21.CoreWebView2.Settings.AreDevToolsEnabled = enable
        WebView21.CoreWebView2.Settings.AreBrowserAcceleratorKeysEnabled = enable
        WebView21.CoreWebView2.Settings.AreDefaultContextMenusEnabled = enable

        If enable AndAlso CommonRuntime.AppHost IsNot Nothing Then
            Call CommonRuntime.AppHost.StatusMessage($"[{TabText}] WebView2 developer tools has been enable!")
        End If
    End Sub

    ''' <summary>
    ''' just call this function in form load event
    ''' </summary>
    ''' <param name="WebView21"></param>
    ''' <param name="enableDevTool"></param>
    ''' <param name="tabText"></param>
    Public Async Sub Init(WebView21 As WebView2, Optional enableDevTool As Boolean = False, Optional tabText As String = "WebKit")
        Dim userDataFolder = (App.ProductProgramData & "/.webView2_cache/").GetDirectoryFullPath
        Dim env = Await CoreWebView2Environment.CreateAsync(Nothing, userDataFolder)

        If CommonRuntime.AppHost IsNot Nothing Then
            Call CommonRuntime.AppHost.StatusMessage($"set webview2 cache at '{userDataFolder}'.")
        End If

        Await WebView21.EnsureCoreWebView2Async(env)

        Call DeveloperOptions(WebView21, enableDevTool, tabText)
    End Sub

    Private Sub Wait()

    End Sub
End Module
