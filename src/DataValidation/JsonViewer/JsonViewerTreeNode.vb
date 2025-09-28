Imports System.Text
Imports Galaxy.Data.JSON.Models
Imports Galaxy.Data.JSON.Plugin

Namespace JSON

    Public Class JsonViewerTreeNode
        Inherits TreeNode
        Private _jsonObject As JsonObject
        Private _textVisualizers As List(Of ICustomTextProvider) = New List(Of ICustomTextProvider)()
        Private _visualizers As List(Of IJsonVisualizer) = New List(Of IJsonVisualizer)()
        Private _init As Boolean
        Private _lastVisualizer As IJsonVisualizer

        Public Sub New(jsonObject As JsonObject)
            _jsonObject = jsonObject
        End Sub

        Public ReadOnly Property TextVisualizers As List(Of ICustomTextProvider)
            Get
                Return _textVisualizers
            End Get
        End Property

        Public ReadOnly Property Visualizers As List(Of IJsonVisualizer)
            Get
                Return _visualizers
            End Get
        End Property

        Public ReadOnly Property JsonObject As JsonObject
            Get
                Return _jsonObject
            End Get
        End Property

        Friend Property Initialized As Boolean
            Get
                Return _init
            End Get
            Set(value As Boolean)
                _init = value
            End Set
        End Property

        Friend Sub RefreshText()
            Dim sb As StringBuilder = New StringBuilder(_jsonObject.Text)
            For Each textVisualizer In _textVisualizers
                Try
                    Dim customText = textVisualizer.GetText(_jsonObject)
                    sb.Append(" (" & customText & ")")
                Catch
                    'silently ignore
                End Try
            Next
            Dim text As String = sb.ToString()
            If Not Equals(text, Me.Text) Then Me.Text = text
        End Sub

        Public Property LastVisualizer As IJsonVisualizer
            Get
                Return _lastVisualizer
            End Get
            Set(value As IJsonVisualizer)
                _lastVisualizer = value
            End Set
        End Property
    End Class


End Namespace