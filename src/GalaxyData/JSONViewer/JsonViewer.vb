Imports System.ComponentModel
Imports System.Drawing.Design
Imports Galaxy.Data.JSON.Models

Namespace JSON

    ''' <summary>
    ''' https://github.com/Sesshoumaru/JsonViewer
    ''' </summary>
    Partial Public Class JsonViewer : Inherits UserControl

        Friend WithEvents TreeView1 As TreeView
        Friend WithEvents ImageList1 As ImageList
        Private components As IContainer
        Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
        Friend WithEvents ExpandAllToolStripMenuItem As ToolStripMenuItem
        Friend WithEvents ViewToolStripMenuItem As ToolStripMenuItem
        Friend WithEvents FindToolStripMenuItem As ToolStripMenuItem
        Friend WithEvents CopyToolStripMenuItem As ToolStripMenuItem
        Friend WithEvents ToolStripMenuItem1 As ToolStripSeparator
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

        Public Event FindAction(node As JsonViewerTreeNode, text As String)
        Public Event MenuAction(sender As ToolStripMenuItem, node As JsonObject)
        Public Event Visit(node As JsonViewerTreeNode)

        Dim addedCustomMenu As Boolean = False

        Sub New()
            Call InitializeComponent()

            viewer = New JsonRender(TreeView1)
            viewer.Render("{}")
        End Sub

        Public Sub Render(json As JsonObjectTree)
            Call viewer.Render(json)
        End Sub

        Public Function GetContextMenu() As ContextMenuStrip
            Return ContextMenuStrip1
        End Function

        Public Sub AddContextMenuItem(name As String, tag As String)
            Dim menu As New ToolStripMenuItem() With {.Text = name, .Name = tag}

            If Not addedCustomMenu Then
                addedCustomMenu = True
                ContextMenuStrip1.Items.Add(New ToolStripSeparator)
            End If

            Call Me.ContextMenuStrip1.Items.Add(menu)
            AddHandler menu.Click, AddressOf ClickHandler
        End Sub

        Private Sub ClickHandler(sender As Object, e As System.EventArgs)
            Dim node = viewer.GetSelectedTreeNode

            If node IsNot Nothing Then
                Dim tagObj As JsonObject = TryCast(node.Tag, JsonObject)

                If tagObj IsNot Nothing Then
                    RaiseEvent MenuAction(sender, tagObj)
                End If
            End If
        End Sub

        Private Sub mnuExpandAll_Click(sender As Object, e As System.EventArgs) Handles ExpandAllToolStripMenuItem.Click
            If Not viewer Is Nothing Then
                Call viewer.ExpandAll()
            End If
        End Sub

        Private Sub mnuFind_Click(sender As Object, e As System.EventArgs) Handles FindToolStripMenuItem.Click
            If Not viewer Is Nothing Then
                RaiseEvent FindAction(viewer.GetSelectedTreeNode, GetSelectedNodeText)
            End If
        End Sub

        Private Function GetSelectedNodeText() As String
            Dim text As String = viewer.GetSelectedTreeNode.Text
            text = text.GetTagValue(":", failureNoName:=True).Value
            text = Strings.Trim(text)

            Return text
        End Function

        Private Sub mnuCopyValue_Click(sender As Object, e As System.EventArgs) Handles CopyToolStripMenuItem.Click
            If Not viewer Is Nothing Then
                Call Clipboard.SetText(GetSelectedNodeText)
            End If
        End Sub

        Private Sub InitializeComponent()
            components = New Container()
            Dim resources As ComponentResourceManager = New ComponentResourceManager(GetType(JsonViewer))
            TreeView1 = New TreeView()
            ContextMenuStrip1 = New ContextMenuStrip(components)
            ViewToolStripMenuItem = New ToolStripMenuItem()
            FindToolStripMenuItem = New ToolStripMenuItem()
            CopyToolStripMenuItem = New ToolStripMenuItem()
            ToolStripMenuItem1 = New ToolStripSeparator()
            ExpandAllToolStripMenuItem = New ToolStripMenuItem()
            ImageList1 = New ImageList(components)
            ContextMenuStrip1.SuspendLayout()
            SuspendLayout()
            ' 
            ' TreeView1
            ' 
            TreeView1.BorderStyle = BorderStyle.None
            TreeView1.ContextMenuStrip = ContextMenuStrip1
            resources.ApplyResources(TreeView1, "TreeView1")
            TreeView1.ImageList = ImageList1
            TreeView1.Name = "TreeView1"
            ' 
            ' ContextMenuStrip1
            ' 
            ContextMenuStrip1.Items.AddRange(New ToolStripItem() {ViewToolStripMenuItem, FindToolStripMenuItem, CopyToolStripMenuItem, ToolStripMenuItem1, ExpandAllToolStripMenuItem})
            ContextMenuStrip1.Name = "ContextMenuStrip1"
            resources.ApplyResources(ContextMenuStrip1, "ContextMenuStrip1")
            ' 
            ' ViewToolStripMenuItem
            ' 
            resources.ApplyResources(ViewToolStripMenuItem, "ViewToolStripMenuItem")
            ViewToolStripMenuItem.Name = "ViewToolStripMenuItem"
            ' 
            ' FindToolStripMenuItem
            ' 
            resources.ApplyResources(FindToolStripMenuItem, "FindToolStripMenuItem")
            FindToolStripMenuItem.Name = "FindToolStripMenuItem"
            ' 
            ' CopyToolStripMenuItem
            ' 
            CopyToolStripMenuItem.Name = "CopyToolStripMenuItem"
            resources.ApplyResources(CopyToolStripMenuItem, "CopyToolStripMenuItem")
            ' 
            ' ToolStripMenuItem1
            ' 
            ToolStripMenuItem1.Name = "ToolStripMenuItem1"
            resources.ApplyResources(ToolStripMenuItem1, "ToolStripMenuItem1")
            ' 
            ' ExpandAllToolStripMenuItem
            ' 
            ExpandAllToolStripMenuItem.Name = "ExpandAllToolStripMenuItem"
            resources.ApplyResources(ExpandAllToolStripMenuItem, "ExpandAllToolStripMenuItem")
            ' 
            ' ImageList1
            ' 
            ImageList1.ColorDepth = ColorDepth.Depth32Bit
            ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), ImageListStreamer)
            ImageList1.TransparentColor = Color.Transparent
            ImageList1.Images.SetKeyName(0, "Class_256x.png")
            ImageList1.Images.SetKeyName(1, "IndexerWizard_7974.png")
            ImageList1.Images.SetKeyName(2, "FieldWizard_7973.png")
            ' 
            ' JsonViewer
            ' 
            Controls.Add(TreeView1)
            Name = "JsonViewer"
            resources.ApplyResources(Me, "$this")
            ContextMenuStrip1.ResumeLayout(False)
            ResumeLayout(False)

        End Sub

        Private Sub TreeView1_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles TreeView1.AfterSelect
            If TreeView1.SelectedNode IsNot Nothing Then
                RaiseEvent Visit(viewer.GetSelectedTreeNode)
            End If
        End Sub
    End Class

End Namespace