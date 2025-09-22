Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Text
Imports System.Windows.Forms
Imports JsonViewer.Models
Imports JsonViewer.Plugin

''' <summary>
''' https://github.com/Sesshoumaru/JsonViewer
''' </summary>
Partial Public Class JsonViewer : Inherits UserControl

    Dim WithEvents viewer As JsonRender

    <Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", GetType(UITypeEditor))>
    Public Property Json As String
        Get
            If viewer Is Nothing Then
                Return Nothing
            End If
            Return viewer.ToString
        End Get
        Set(value As String)
            If viewer IsNot Nothing Then
                Call viewer.Render(jsonstr:=value)
            End If
        End Set
    End Property

    Public Property RootTag As String
        Get
            If viewer Is Nothing Then
                Return Nothing
            End If
            Return viewer.RootTag
        End Get
        Set(value As String)
            If viewer IsNot Nothing Then
                Call viewer.SetRootLabel(label:=value)
            End If
        End Set
    End Property

    Sub New()
        Call InitializeComponent()

        viewer = New JsonRender(tvJson)
        viewer.Render("{}")
    End Sub
End Class

Public Structure ErrorDetails
    Friend _err As String
    Friend _pos As Integer

    Public ReadOnly Property [Error] As String
        Get
            Return _err
        End Get
    End Property

    Public ReadOnly Property Position As Integer
        Get
            Return _pos
        End Get
    End Property

    Public Sub Clear()
        _err = Nothing
        _pos = 0
    End Sub
End Structure

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

Public Enum Tabs
    Viewer
    Text
End Enum