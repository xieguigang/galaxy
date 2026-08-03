
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ApplicationServices

Public Module TreeViewHelper

    ''' <summary>
    ''' 将 FileSystemTree 加载到 TreeView 控件中
    ''' </summary>
    ''' <param name="treeView">目标 TreeView 控件</param>
    ''' <param name="rootNode">要加载的 FileSystemTree 根节点</param>
    ''' 
    <Extension>
    Public Function LoadFileSystemTree(treeView As TreeView, rootNode As FileSystemTree, Optional folderIndex As Integer = 1, Optional fileIndex As Integer = 2) As TreeNode
        If treeView Is Nothing OrElse rootNode Is Nothing Then
            Return Nothing
        Else
            ' 关闭重绘以提高加载速度并防止闪烁
            Call treeView.BeginUpdate()
            Call treeView.Nodes.Clear()
        End If

        ' 递归创建节点
        Dim treeNode As TreeNode

        Try
            ' 递归创建节点
            treeNode = CreateTreeNode(rootNode, folderIndex, fileIndex)

            treeNode.ImageIndex = 0
            treeNode.SelectedImageIndex = 0
            treeNode.StateImageIndex = 0

            Call treeView.Nodes.Add(treeNode)
            ' 可选：默认展开根节点
            Call treeNode.Expand()
        Finally
            ' 恢复重绘
            Call treeView.EndUpdate()
        End Try

        Return treeNode
    End Function

    ''' <summary>
    ''' 递归构建 TreeNode 的核心方法
    ''' </summary>
    Private Function CreateTreeNode(fsNode As FileSystemTree, folderIndex As Integer, fileIndex As Integer) As TreeNode
        ' 创建当前层的 TreeNode
        ' 将原始的 FileSystemTree 对象存入 Tag，方便后续交互时直接获取数据
        Dim treeNode As New TreeNode(fsNode.Name) With {
            .Tag = fsNode
        }

        If fsNode.IsDirectory Then
            treeNode.ImageIndex = folderIndex
            treeNode.SelectedImageIndex = folderIndex
            treeNode.StateImageIndex = folderIndex
        Else
            treeNode.ImageIndex = fileIndex
            treeNode.SelectedImageIndex = fileIndex
            treeNode.StateImageIndex = fileIndex
        End If

        ' 检查是否有子节点（由于你的代码中 IsNullOrEmpty 可能是扩展方法，
        ' 这里为了代码健壮性，直接进行基础的 Nothing 和 Count 判断）
        If fsNode.Files IsNot Nothing AndAlso fsNode.Files.Count > 0 Then
            For Each kvp As KeyValuePair(Of String, FileSystemTree) In fsNode.Files _
                .OrderBy(Function(f) If(f.Value.IsDirectory, 0, 1)) _
                .ThenBy(Function(a)
                            Return a.Key
                        End Function)

                ' 递归创建子节点并添加到当前节点的 Nodes 集合中
                Call treeNode.Nodes.Add(CreateTreeNode(kvp.Value, folderIndex, fileIndex))
            Next
        End If

        Return treeNode
    End Function

End Module