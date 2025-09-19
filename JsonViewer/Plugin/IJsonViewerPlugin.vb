Imports System.Windows.Forms

Namespace Plugin

    Public Interface IJsonViewerPlugin
        ReadOnly Property DisplayName As String
        Function CanVisualize(jsonObject As JsonObject) As Boolean
    End Interface

    Public Interface ICustomTextProvider
        Inherits IJsonViewerPlugin
        Function GetText(jsonObject As JsonObject) As String
    End Interface

    Public Interface IJsonVisualizer
        Inherits IJsonViewerPlugin
        Function GetControl(jsonObject As JsonObject) As Control
        Sub Visualize(jsonObject As JsonObject)
    End Interface

End Namespace