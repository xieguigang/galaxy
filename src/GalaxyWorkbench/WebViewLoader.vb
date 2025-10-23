Imports System.IO
Imports System.Text
Imports Microsoft.Web.WebView2.Core
Imports Microsoft.Web.WebView2.WinForms

Public Class WebViewLoader

    Public Shared Sub DeveloperOptions(WebView21 As WebView2, enable As Boolean, Optional TabText As String = "WebKit")
        WebView21.CoreWebView2.Settings.AreDevToolsEnabled = enable
        WebView21.CoreWebView2.Settings.AreBrowserAcceleratorKeysEnabled = enable
        WebView21.CoreWebView2.Settings.AreDefaultContextMenusEnabled = enable

        If enable AndAlso CommonRuntime.AppHost IsNot Nothing Then
            Call CommonRuntime.AppHost.StatusMessage($"[{TabText}] WebView2 developer tools has been enable!", Icons8.JobDone)
        End If
    End Sub

    ''' <summary>
    ''' just call this function in form load event
    ''' </summary>
    ''' <param name="WebView21"></param>
    ''' <param name="enableDevTool"></param>
    Public Shared Async Function Init(WebView21 As WebView2, Optional enableDevTool As Boolean = False) As Task
        Dim userDataFolder = (App.ProductProgramData & "/.webView2_cache/").GetDirectoryFullPath
        Dim env = Await CoreWebView2Environment.CreateAsync(Nothing, userDataFolder)

        If CommonRuntime.AppHost IsNot Nothing Then
            Call CommonRuntime.AppHost.StatusMessage($"set webview2 cache at '{userDataFolder}'.")
        End If

        Await WebView21.EnsureCoreWebView2Async(env)
    End Function

    ReadOnly hostname As String
    ReadOnly url As String
    ReadOnly filter As String
    ReadOnly loadDocument As String

    Dim WithEvents webView As WebView2
    Dim WithEvents core As CoreWebView2

    Sub New(webView2 As WebView2, document As String)
        webView = webView2
        core = webView.CoreWebView2
        loadDocument = document
        hostname = $"{Guid.NewGuid()}.net"
        url = $"https://{hostname}/"
        filter = $"*://{hostname}/*"
    End Sub

    Public Shared Sub NavigateToLargeString(webView As WebView2, value As String)
        Dim loader As New WebViewLoader(webView, value)

        Call webView.CoreWebView2.AddWebResourceRequestedFilter(loader.filter, CoreWebView2WebResourceContext.Document)
        Call webView.CoreWebView2.Navigate(loader.url)
    End Sub

    Private Sub navigationStartingEvent(sender As Object, e As CoreWebView2NavigationStartingEventArgs) Handles core.NavigationStarting
        On Error Resume Next

        If (Not e.Uri.Equals(url, StringComparison.OrdinalIgnoreCase)) Then
            Call webView.CoreWebView2.RemoveWebResourceRequestedFilter(filter, CoreWebView2WebResourceContext.Document)
        End If
    End Sub

    Private Sub webResourceRequestedHandler(sender As Object, e As CoreWebView2WebResourceRequestedEventArgs) Handles core.WebResourceRequested
        If (Not e.Request.Uri.Equals(url, StringComparison.OrdinalIgnoreCase)) Then
            Return
        End If

        Dim def = e.GetDeferral
        Dim s As New MemoryStream(Encoding.UTF8.GetBytes(loadDocument))

        Try
            e.Response = webView.CoreWebView2.Environment.CreateWebResourceResponse(s, 200, "OK", "Content-Type: text/html; charset=utf-8")
        Catch ex As Exception
            e.Response = webView.CoreWebView2.Environment.CreateWebResourceResponse(Nothing, 404, "Not found", "Content-Type: text/html; charset=utf-8")
        Finally
            def.Complete()
        End Try
    End Sub

    Public Shared Async Sub GotoPageLocation(webView21 As WebView2, scrollX As Integer, scrollY As Integer)
        ' 使用JavaScript设置滚动位置
        Await webView21.CoreWebView2.ExecuteScriptAsync($"window.scrollTo({scrollX}, {scrollY});")
        ' 添加平滑滚动效果
        Await webView21.CoreWebView2.ExecuteScriptAsync($"
            window.scrollTo({{
                top: {scrollY},
                left: {scrollX},
                behavior: 'auto'
            }});")
    End Sub

    Public Shared Async Function GetPageLocation(webView21 As WebView2) As Task(Of Point)
        Dim scrollXJson = Await webView21.CoreWebView2.ExecuteScriptAsync("window.scrollX;")
        Dim scrollYJson = Await webView21.CoreWebView2.ExecuteScriptAsync("window.scrollY;")
        ' 解析返回的JSON字符串，去除引号并转换为整数
        Dim scrollX = Integer.Parse(scrollXJson.Trim(""""c))
        Dim scrollY = Integer.Parse(scrollYJson.Trim(""""c))

        Return New Point(scrollX, scrollY)
    End Function
End Class
