Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing
Imports System.Drawing.Design
Imports System.Text
Imports System.Threading
Imports System.Windows.Forms
Imports EPocalipse.Json.Viewer.Properties

Namespace EPocalipse.Json.Viewer
    Partial Public Class JsonViewer
        Inherits UserControl
        Private _json As String
        Private _maxErrorCount As Integer = 25
        Private _errorDetails As ErrorDetails
        Private _pluginsManager As PluginsManager = New PluginsManager()
        Private _updating As Boolean
        Private _lastVisualizerControl As Control

        Public Sub New()
            InitializeComponent()
            Try
                _pluginsManager.Initialize()
            Catch e As Exception
                MessageBox.Show(String.Format(Resources.ConfigMessage, e.Message), "Json Viewer", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End Try
        End Sub

        <Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", GetType(UITypeEditor))>
        Public Property Json As String
            Get
                Return _json
            End Get
            Set(value As String)
                If Not Equals(_json, value) Then
                    _json = value.Trim()
                    Redraw()
                End If
            End Set
        End Property

        <DefaultValue(25)>
        Public Property MaxErrorCount As Integer
            Get
                Return _maxErrorCount
            End Get
            Set(value As Integer)
                _maxErrorCount = value
            End Set
        End Property

        Private Sub Redraw()
            Try
                tvJson.BeginUpdate()
                Try
                    Reset()
                    If Not String.IsNullOrEmpty(_json) Then
                        Dim tree = JsonObjectTree.Parse(_json)
                        VisualizeJsonTree(tree)
                    End If

                Finally
                    tvJson.EndUpdate()
                End Try
            Catch e As JsonParseError

            Catch e As Exception

            End Try
        End Sub

        Private Sub Reset()
            tvJson.Nodes.Clear()
            _lastVisualizerControl = Nothing
        End Sub

        Private Sub VisualizeJsonTree(tree As JsonObjectTree)
            AddNode(tvJson.Nodes, tree.Root)
            Dim node As JsonViewerTreeNode = GetRootNode()
            InitVisualizers(node)
            node.Expand()
            tvJson.SelectedNode = node
        End Sub

        Private Sub AddNode(nodes As TreeNodeCollection, jsonObject As JsonObject)
            Dim newNode As JsonViewerTreeNode = New JsonViewerTreeNode(jsonObject)
            nodes.Add(newNode)
            newNode.Text = jsonObject.Text
            newNode.Tag = jsonObject
            newNode.ImageIndex = jsonObject.JsonType
            newNode.SelectedImageIndex = newNode.ImageIndex

            For Each field In jsonObject.Fields
                AddNode(newNode.Nodes, field)
            Next
        End Sub

        Public ReadOnly Property ErrorDetails As ErrorDetails
            Get
                Return _errorDetails
            End Get
        End Property

        Public Sub Clear()
            Json = String.Empty
        End Sub

        Public ReadOnly Property HasErrors As Boolean
            Get
                Return Not Equals(_errorDetails._err, Nothing)
            End Get
        End Property

        Public Function FindNext(text As String, includeSelected As Boolean) As Boolean
            Dim startNode = tvJson.SelectedNode
            If startNode Is Nothing AndAlso HasNodes() Then startNode = GetRootNode()
            If startNode IsNot Nothing Then
                startNode = FindNext(startNode, text, includeSelected)
                If startNode IsNot Nothing Then
                    tvJson.SelectedNode = startNode
                    Return True
                End If
            End If
            Return False
        End Function

        Public Function FindNext(startNode As TreeNode, text As String, includeSelected As Boolean) As TreeNode
            If Equals(text, String.Empty) Then Return startNode

            If includeSelected AndAlso IsMatchingNode(startNode, text) Then Return startNode

            Dim originalStartNode = startNode
            startNode = GetNextNode(startNode)
            text = text.ToLower()
            While startNode IsNot originalStartNode
                If IsMatchingNode(startNode, text) Then Return startNode
                startNode = GetNextNode(startNode)
            End While

            Return Nothing
        End Function

        Private Function GetNextNode(startNode As TreeNode) As TreeNode
            Dim [next] = If(startNode.FirstNode, startNode.NextNode)
            If [next] Is Nothing Then
                While startNode IsNot Nothing AndAlso [next] Is Nothing
                    startNode = startNode.Parent
                    If startNode IsNot Nothing Then [next] = startNode.NextNode
                End While
                If [next] Is Nothing Then
                    [next] = GetRootNode()
                End If
            End If
            Return [next]
        End Function

        Private Function IsMatchingNode(startNode As TreeNode, text As String) As Boolean
            Return (startNode.Text.ToLower().Contains(text))
        End Function

        Private Function GetRootNode() As JsonViewerTreeNode
            If tvJson.Nodes.Count > 0 Then Return CType(tvJson.Nodes(0), JsonViewerTreeNode)
            Return Nothing
        End Function

        Private Function HasNodes() As Boolean
            Return tvJson.Nodes.Count > 0
        End Function

        Private Sub FlashControl(control As Control, color As Color)
            Dim prevColor = control.BackColor
            Try
                control.BackColor = color
                control.Refresh()
                Thread.Sleep(25)
            Finally
                control.BackColor = prevColor
                control.Refresh()
            End Try
        End Sub

        Private Function GetSelectedTreeNode() As JsonViewerTreeNode
            Return CType(tvJson.SelectedNode, JsonViewerTreeNode)
        End Function

        Private Sub tvJson_BeforeExpand(sender As Object, e As TreeViewCancelEventArgs)
            For Each node As JsonViewerTreeNode In e.Node.Nodes
                InitVisualizers(node)
            Next
        End Sub

        Private Sub InitVisualizers(node As JsonViewerTreeNode)
            If Not node.Initialized Then
                node.Initialized = True
                Dim jsonObject = node.JsonObject
                For Each textVis In _pluginsManager.TextVisualizers
                    If textVis.CanVisualize(jsonObject) Then node.TextVisualizers.Add(textVis)
                Next

                node.RefreshText()

                For Each visualizer In _pluginsManager.Visualizers
                    If visualizer.CanVisualize(jsonObject) Then node.Visualizers.Add(visualizer)
                Next
            End If
        End Sub

        Private Sub expandallToolStripMenuItem_Click(sender As Object, e As EventArgs)
            tvJson.BeginUpdate()
            Try
                If tvJson.SelectedNode IsNot Nothing Then
                    Dim topNode = tvJson.TopNode
                    tvJson.SelectedNode.ExpandAll()
                    tvJson.TopNode = topNode
                End If

            Finally
                tvJson.EndUpdate()
            End Try
        End Sub

        Private Sub tvJson_MouseDown(sender As Object, e As MouseEventArgs)
            If e.Button = MouseButtons.Right Then
                Dim node = tvJson.GetNodeAt(e.Location)
                If node IsNot Nothing Then
                    tvJson.SelectedNode = node
                End If
            End If
        End Sub

        Private Sub mnuTree_Opening(sender As Object, e As CancelEventArgs)
            mnuFind.Enabled = (GetRootNode() IsNot Nothing)
            mnuExpandAll.Enabled = (GetSelectedTreeNode() IsNot Nothing)

            mnuCopy.Enabled = mnuExpandAll.Enabled
            mnuCopyValue.Enabled = mnuExpandAll.Enabled
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
End Namespace
