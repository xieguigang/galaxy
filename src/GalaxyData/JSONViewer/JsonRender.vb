Imports Galaxy.Data.JSON.Models
Imports Galaxy.Data.JSON.Plugin

Namespace JSON

    Public Class JsonRender

        Dim WithEvents tvJson As TreeView

        Dim _json As String
        Dim _pluginsManager As New PluginsManager()

        Public ReadOnly Property RootTag As String = "JSON"

        Sub New(tree As TreeView, Optional warning As Action(Of String) = Nothing)
            Me.tvJson = tree

            Try
                _pluginsManager.Initialize()
            Catch e As Exception
                Dim msg As String = {
                    "There was an error during initialization.",
                    "If you get this message when trying to use the Fiddler plugin or the Visual Studio Visualizer, please follow the instructions in the ReadMe.txt file.",
                   $"The error was: {e.Message}"}.JoinBy(vbCrLf)

                warning = If(warning, New Action(Of String)(AddressOf JsonRender.Warning))
                warning(msg)
            End Try
        End Sub

        Private Shared Sub Warning(msg As String)
            MessageBox.Show(msg, "Json Viewer", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Sub

        Public Sub SetRootLabel(label As String)
            _RootTag = label
        End Sub

        Public Sub Render(jsonstr As String)
            _json = jsonstr
            Call Redraw()
        End Sub

        Public Overrides Function ToString() As String
            Return _json
        End Function

        Public Sub Clear()
            Call Render("{}")
        End Sub

        Private Sub Redraw()
            Try
                tvJson.BeginUpdate()
                Try
                    Reset()
                    If Not String.IsNullOrEmpty(_json) Then
                        Dim tree = JsonObjectTree.Parse(_json, _RootTag)
                        Call VisualizeJsonTree(tree)
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
        End Sub

        Private Sub VisualizeJsonTree(tree As JsonObjectTree)
            AddNode(tvJson.Nodes, tree.Root)
            Dim node As JsonViewerTreeNode = GetRootNode()
            InitVisualizers(node)
            node.Expand()
            tvJson.SelectedNode = node
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

        Private Shared Sub AddNode(nodes As TreeNodeCollection, jsonObject As JsonObject)
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

        Public Function GetSelectedTreeNode() As JsonViewerTreeNode
            Return CType(tvJson.SelectedNode, JsonViewerTreeNode)
        End Function

        Private Sub tvJson_BeforeExpand(sender As Object, e As TreeViewCancelEventArgs) Handles tvJson.BeforeExpand
            For Each node As JsonViewerTreeNode In e.Node.Nodes
                InitVisualizers(node)
            Next
        End Sub

        Public Sub ExpandAll()
            Call tvJson.BeginUpdate()

            Try
                If tvJson.SelectedNode IsNot Nothing Then
                    Dim topNode As TreeNode = tvJson.TopNode

                    tvJson.SelectedNode.ExpandAll()
                    tvJson.TopNode = topNode
                End If
            Finally
                Call tvJson.EndUpdate()
            End Try
        End Sub

        Private Sub tvJson_MouseDown(sender As Object, e As MouseEventArgs) Handles tvJson.MouseDown
            If e.Button = MouseButtons.Right Then
                Dim node = tvJson.GetNodeAt(e.Location)

                If node IsNot Nothing Then
                    tvJson.SelectedNode = node
                End If
            End If
        End Sub
    End Class
End Namespace