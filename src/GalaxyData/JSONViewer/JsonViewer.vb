Imports System.ComponentModel
Imports System.Drawing.Design

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

        Public Event FindAction(node As JsonViewerTreeNode)

        Sub New()
            Call InitializeComponent()

            viewer = New JsonRender(TreeView1)
            viewer.Render("{}")
        End Sub

        Private Sub mnuExpandAll_Click(sender As Object, e As System.EventArgs) Handles ExpandAllToolStripMenuItem.Click
            If Not viewer Is Nothing Then
                Call viewer.ExpandAll()
            End If
        End Sub

        Private Sub mnuFind_Click(sender As Object, e As System.EventArgs) Handles FindToolStripMenuItem.Click
            If Not viewer Is Nothing Then
                RaiseEvent FindAction(viewer.GetSelectedTreeNode)
            End If
        End Sub

        Private Sub mnuCopyValue_Click(sender As Object, e As System.EventArgs) Handles CopyToolStripMenuItem.Click
            If Not viewer Is Nothing Then

            End If
        End Sub

        Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(JsonViewer))
            Me.TreeView1 = New System.Windows.Forms.TreeView()
            Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
            Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
            Me.ViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.FindToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.CopyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
            Me.ExpandAllToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ContextMenuStrip1.SuspendLayout()
            Me.SuspendLayout()
            '
            'TreeView1
            '
            Me.TreeView1.BorderStyle = System.Windows.Forms.BorderStyle.None
            Me.TreeView1.ContextMenuStrip = Me.ContextMenuStrip1
            Me.TreeView1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TreeView1.ImageIndex = 0
            Me.TreeView1.ImageList = Me.ImageList1
            Me.TreeView1.Location = New System.Drawing.Point(0, 0)
            Me.TreeView1.Name = "TreeView1"
            Me.TreeView1.SelectedImageIndex = 0
            Me.TreeView1.Size = New System.Drawing.Size(704, 707)
            Me.TreeView1.TabIndex = 0
            '
            'ImageList1
            '
            Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
            Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
            Me.ImageList1.Images.SetKeyName(0, "Class_256x.png")
            Me.ImageList1.Images.SetKeyName(1, "IndexerWizard_7974.png")
            Me.ImageList1.Images.SetKeyName(2, "FieldWizard_7973.png")
            '
            'ContextMenuStrip1
            '
            Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ViewToolStripMenuItem, Me.FindToolStripMenuItem, Me.CopyToolStripMenuItem, Me.ToolStripMenuItem1, Me.ExpandAllToolStripMenuItem})
            Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
            Me.ContextMenuStrip1.Size = New System.Drawing.Size(131, 98)
            '
            'ViewToolStripMenuItem
            '
            Me.ViewToolStripMenuItem.Name = "ViewToolStripMenuItem"
            Me.ViewToolStripMenuItem.Size = New System.Drawing.Size(130, 22)
            Me.ViewToolStripMenuItem.Text = "View"
            '
            'FindToolStripMenuItem
            '
            Me.FindToolStripMenuItem.Name = "FindToolStripMenuItem"
            Me.FindToolStripMenuItem.Size = New System.Drawing.Size(130, 22)
            Me.FindToolStripMenuItem.Text = "Find"
            '
            'CopyToolStripMenuItem
            '
            Me.CopyToolStripMenuItem.Name = "CopyToolStripMenuItem"
            Me.CopyToolStripMenuItem.Size = New System.Drawing.Size(130, 22)
            Me.CopyToolStripMenuItem.Text = "Copy"
            '
            'ToolStripMenuItem1
            '
            Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
            Me.ToolStripMenuItem1.Size = New System.Drawing.Size(127, 6)
            '
            'ExpandAllToolStripMenuItem
            '
            Me.ExpandAllToolStripMenuItem.Name = "ExpandAllToolStripMenuItem"
            Me.ExpandAllToolStripMenuItem.Size = New System.Drawing.Size(130, 22)
            Me.ExpandAllToolStripMenuItem.Text = "Expand All"
            '
            'JsonViewer
            '
            Me.Controls.Add(Me.TreeView1)
            Me.Name = "JsonViewer"
            Me.Size = New System.Drawing.Size(704, 707)
            Me.ContextMenuStrip1.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub
    End Class

End Namespace