Imports System
Imports System.Runtime.Serialization
Imports System.Text.RegularExpressions
Imports Microsoft.VisualBasic.MIME.application.json
Imports JArray = Microsoft.VisualBasic.MIME.application.json.Javascript.JsonArray
Imports JObject = Microsoft.VisualBasic.MIME.application.json.Javascript.JsonObject

Namespace Models
    Public Enum JsonType
        [Object]
        Array
        Value
    End Enum

    Friend Class JsonParseError
        Inherits ApplicationException
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub
        Protected Sub New(info As SerializationInfo, context As StreamingContext)
            MyBase.New(info, context)
        End Sub
        Public Sub New(message As String, innerException As Exception)
            MyBase.New(message, innerException)
        End Sub

    End Class

    Public Class JsonObjectTree
        Private _root As JsonObject
        Private Shared dateRegex As Regex = New Regex("^/Date\(([0-9]*)([+-][0-9]{4}){0,1}\)/$")

        Public Shared Function Parse(json As String) As JsonObjectTree
            Return New JsonObjectTree(New JsonParser(json).OpenJSON)
        End Function

        Public Sub New(rootObject As Object)
            _root = ConvertToObject("JSON", rootObject)
        End Sub

        Private Shared Function ConvertToObject(id As String, jsonObject As Object) As JsonObject
            Dim obj = CreateJsonObject(jsonObject)
            obj.Id = id
            AddChildren(jsonObject, obj)
            Return obj
        End Function

        Private Shared Sub AddChildren(jsonObject As Object, obj As JsonObject)
            Dim javaScriptObject As JObject = TryCast(jsonObject, JObject)
            If javaScriptObject IsNot Nothing Then
                For Each pair In javaScriptObject
                    obj.Fields.Add(ConvertToObject(pair.Name, pair.Value))
                Next
            Else
                Dim javaScriptArray As JArray = TryCast(jsonObject, JArray)
                If javaScriptArray IsNot Nothing Then
                    For i As Integer = 0 To javaScriptArray.Length - 1
                        obj.Fields.Add(ConvertToObject("[" & i.ToString() & "]", javaScriptArray(i)))
                    Next
                End If
            End If
        End Sub

        Private Shared Function CreateJsonObject(jsonObject As Object) As JsonObject
            Dim obj As JsonObject = New JsonObject()
            If TypeOf jsonObject Is JArray Then
                obj.JsonType = JsonType.Array
            ElseIf TypeOf jsonObject Is JObject Then
                obj.JsonType = JsonType.Object
            Else
                If GetType(String) Is jsonObject.GetType() Then
                    Dim match As Match = dateRegex.Match(TryCast(jsonObject, String))
                    If match.Success Then
                        ' I'm not sure why this is match.Groups[1] and not match.Groups[0]
                        ' we need to convert milliseconds to windows ticks (one tick is one hundred nanoseconds (e-9))
                        Dim ticksSinceEpoch = Long.Parse(match.Groups(1).Value) * CLng(10000.0)
                        jsonObject = Date.SpecifyKind(New DateTime(1970, 1, 1).Add(New TimeSpan(ticksSinceEpoch)), DateTimeKind.Utc)
                        ' Take care of the timezone offset
                        If Not String.IsNullOrEmpty(match.Groups(2).Value) Then
                            Dim timeZoneOffset = Long.Parse(match.Groups(2).Value)
                            jsonObject = CDate(jsonObject).AddHours(timeZoneOffset / 100)
                            ' Some timezones like India Tehran and Nepal have fractional offsets from GMT
                            jsonObject = CDate(jsonObject).AddMinutes(timeZoneOffset Mod 100)
                        End If
                    End If
                End If
                obj.JsonType = JsonType.Value
                obj.Value = jsonObject
            End If
            Return obj
        End Function

        Public ReadOnly Property Root As JsonObject
            Get
                Return _root
            End Get
        End Property

    End Class
End Namespace
