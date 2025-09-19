Imports System
Imports System.Windows.Forms
Imports JsonViewer.Models

Namespace Plugin

    Friend Class AjaxNetDateTime
        Implements ICustomTextProvider
        Private Shared ReadOnly epoch As Long = New DateTime(1970, 1, 1).Ticks

        Public Function GetText(jsonObject As JsonObject) As String Implements ICustomTextProvider.GetText
            Dim text = CStr(jsonObject.Value)
            Return "Ajax.Net Date:" & ConvertJSTicksToDateTime(Convert.ToInt64(text.Substring(1, text.Length - 2))).ToString()
        End Function

        Private Function ConvertJSTicksToDateTime(ticks As Long) As Date
            Return New DateTime(ticks * 10000 + epoch)
        End Function

        Public ReadOnly Property DisplayName As String Implements IJsonViewerPlugin.DisplayName
            Get
                Return "Ajax.Net DateTime"
            End Get
        End Property

        Public Function CanVisualize(jsonObject As JsonObject) As Boolean Implements IJsonViewerPlugin.CanVisualize
            If jsonObject.JsonType = JsonType.Value AndAlso TypeOf jsonObject.Value Is String Then
                Dim text = CStr(jsonObject.Value)
                Return text.Length > 2 AndAlso text(0) = "@"c AndAlso text(text.Length - 1) = "@"c
            End If
            Return False
        End Function
    End Class

    Friend Class CustomDate
        Implements ICustomTextProvider
        Public Function GetText(jsonObject As JsonObject) As String Implements ICustomTextProvider.GetText
            Dim year, month, day, hour, min, second, ms As Integer
            year = CInt(CLng(jsonObject.Fields("y").Value))
            month = CInt(CLng(jsonObject.Fields("M").Value))
            day = CInt(CLng(jsonObject.Fields("d").Value))
            hour = CInt(CLng(jsonObject.Fields("h").Value))
            min = CInt(CLng(jsonObject.Fields("m").Value))
            second = CInt(CLng(jsonObject.Fields("s").Value))
            ms = CInt(CLng(jsonObject.Fields("ms").Value))
            Return New DateTime(year, month, day, hour, min, second, ms).ToString()
        End Function

        Public ReadOnly Property DisplayName As String Implements IJsonViewerPlugin.DisplayName
            Get
                Return "Date"
            End Get
        End Property

        Public Function CanVisualize(jsonObject As JsonObject) As Boolean Implements IJsonViewerPlugin.CanVisualize
            Return jsonObject.ContainsFields("y", "M", "d", "h", "m", "s", "ms")
        End Function
    End Class

    Friend Class Sample
        Implements IJsonVisualizer
        Private tb As TextBox

        Public Function GetControl(jsonObject As JsonObject) As Control Implements IJsonVisualizer.GetControl
            If tb Is Nothing Then
                tb = New TextBox()
                tb.Multiline = True
            End If
            Return tb
        End Function

        Public Sub Visualize(jsonObject As JsonObject) Implements IJsonVisualizer.Visualize
            tb.Text = String.Format("Array {0} has {1} items", jsonObject.Id, jsonObject.Fields.Count)
        End Sub

        Public ReadOnly Property DisplayName As String Implements IJsonViewerPlugin.DisplayName
            Get
                Return "Sample"
            End Get
        End Property

        Public Function CanVisualize(jsonObject As JsonObject) As Boolean Implements IJsonViewerPlugin.CanVisualize
            Return jsonObject.JsonType = JsonType.Array AndAlso jsonObject.ContainsFields("[0]")
        End Function
    End Class

End Namespace