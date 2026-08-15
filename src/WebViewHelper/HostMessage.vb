Imports System.Text
Imports System.Text.Json

Public Module HostMessage

    ''' <summary>
    ''' 序列化选项：忽略 Nothing 成员，使成功响应不出现多余的 "error": null。
    ''' 属性名保持原样（已是小写），以匹配 JS 侧的 { ok, data, error } 契约。
    ''' </summary>
    Public ReadOnly JsonOptions As New JsonSerializerOptions With {
        .DefaultIgnoreCondition = Serialization.JsonIgnoreCondition.WhenWritingNull,
        .Encoder = Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    }

    Public Function Success(data As Object) As String
        Return JsonSerializer.Serialize(New ApiResult With {.ok = True, .data = data}, JsonOptions)
    End Function

    Public Function Failure(message As String) As String
        Return JsonSerializer.Serialize(New ApiResult With {.ok = False, .error = message}, JsonOptions)
    End Function

    ''' <summary>统一的返回体：{ "ok": ..., "data": ..., "error": ... }。</summary>
    Public Class ApiResult

        Public Property ok As Boolean
        Public Property data As Object
        Public Property [error] As String

        Public Overrides Function ToString() As String
            Return JsonSerializer.Serialize(Me, JsonOptions)
        End Function
    End Class
End Module
