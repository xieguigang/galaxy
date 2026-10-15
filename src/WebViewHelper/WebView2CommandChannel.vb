Imports System.Text.Json
Imports Microsoft.Web.WebView2.WinForms

Public NotInheritable Class WebView2CommandChannel

    ReadOnly webView As WebView2

    Public Sub New(webView As WebView2)
        Me.webView = webView
    End Sub

    Public Property IsReady As Boolean

    Public Sub Send(type As String, Optional payload As Object = Nothing)
        If IsReady Then
            Dim cmd As New CommandMessage With {
                .type = type,
                .payload = payload
            }
            Dim json As String = ToJson(any:=cmd)

            Call webView?.CoreWebView2?.PostWebMessageAsJson(json)
        End If
    End Sub

    Public Shared Function ToJson(any As Object) As String
        Dim options As New JsonSerializerOptions(JsonSerializerOptions.Web)
        Dim json As String = JsonSerializer.Serialize(any, options)

        Return json 
    End Function

    Public Class CommandMessage

        Public Property type As String
        Public Property payload As Object

    End Class
End Class