Imports System
Imports System.Windows.Forms

Namespace EPocalipse.Json.Viewer
    Partial Class JsonViewer
        ''' <summary>
        ''' Required designer variable.
        ''' </summary>
        Private components As ComponentModel.IContainer = Nothing

        ''' <summary>
        ''' Clean up any resources being used.
        ''' </summary>
        ''' <paramname="disposing">true if managed resources should be disposed; otherwise, false.</param>
        Protected Overrides Sub Dispose(disposing As Boolean)
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

#Region "Component Designer generated code"

        ''' <summary>
        ''' Required method for Designer support - do not modify 
        ''' the contents of this method with the code editor.
        ''' </summary>
        Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(JsonViewer))
            Me.tvJson = New System.Windows.Forms.TreeView()
            Me.mnuTree = New System.Windows.Forms.ContextMenuStrip(Me.components)
            Me.mnuFind = New System.Windows.Forms.ToolStripMenuItem()
            Me.mnuExpandAll = New System.Windows.Forms.ToolStripMenuItem()
            Me.toolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
            Me.mnuCopy = New System.Windows.Forms.ToolStripMenuItem()
            Me.mnuCopyValue = New System.Windows.Forms.ToolStripMenuItem()
            Me.imgList = New System.Windows.Forms.ImageList(Me.components)
            Me.mnuVisualizerPnl = New System.Windows.Forms.ContextMenuStrip(Me.components)
            Me.mnuShowOnRight = New System.Windows.Forms.ToolStripMenuItem()
            Me.mnuShowOnBottom = New System.Windows.Forms.ToolStripMenuItem()
            Me.mnuTree.SuspendLayout()
            Me.mnuVisualizerPnl.SuspendLayout()
            Me.SuspendLayout()
            '
            'tvJson
            '
            Me.tvJson.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.tvJson.ContextMenuStrip = Me.mnuTree
            Me.tvJson.Dock = System.Windows.Forms.DockStyle.Fill
            Me.tvJson.HideSelection = False
            Me.tvJson.ImageIndex = 0
            Me.tvJson.ImageList = Me.imgList
            Me.tvJson.Location = New System.Drawing.Point(0, 0)
            Me.tvJson.Name = "tvJson"
            Me.tvJson.SelectedImageIndex = 0
            Me.tvJson.Size = New System.Drawing.Size(792, 558)
            Me.tvJson.TabIndex = 3
            '
            'mnuTree
            '
            Me.mnuTree.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFind, Me.mnuExpandAll, Me.toolStripMenuItem1, Me.mnuCopy, Me.mnuCopyValue})
            Me.mnuTree.Name = "mnuTree"
            Me.mnuTree.Size = New System.Drawing.Size(134, 98)
            '
            'mnuFind
            '
            Me.mnuFind.Name = "mnuFind"
            Me.mnuFind.Size = New System.Drawing.Size(133, 22)
            Me.mnuFind.Text = "&Find"
            '
            'mnuExpandAll
            '
            Me.mnuExpandAll.Name = "mnuExpandAll"
            Me.mnuExpandAll.Size = New System.Drawing.Size(133, 22)
            Me.mnuExpandAll.Text = "Expand &All"
            '
            'toolStripMenuItem1
            '
            Me.toolStripMenuItem1.Name = "toolStripMenuItem1"
            Me.toolStripMenuItem1.Size = New System.Drawing.Size(130, 6)
            '
            'mnuCopy
            '
            Me.mnuCopy.Name = "mnuCopy"
            Me.mnuCopy.Size = New System.Drawing.Size(133, 22)
            Me.mnuCopy.Text = "&Copy"
            '
            'mnuCopyValue
            '
            Me.mnuCopyValue.Name = "mnuCopyValue"
            Me.mnuCopyValue.Size = New System.Drawing.Size(133, 22)
            Me.mnuCopyValue.Text = "Copy &Value"
            '
            'imgList
            '
            Me.imgList.ImageStream = CType(resources.GetObject("imgList.ImageStream"), System.Windows.Forms.ImageListStreamer)
            Me.imgList.TransparentColor = System.Drawing.Color.White
            Me.imgList.Images.SetKeyName(0, "obj.bmp")
            Me.imgList.Images.SetKeyName(1, "array.bmp")
            Me.imgList.Images.SetKeyName(2, "prop.bmp")
            '
            'mnuVisualizerPnl
            '
            Me.mnuVisualizerPnl.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuShowOnRight, Me.mnuShowOnBottom})
            Me.mnuVisualizerPnl.Name = "mnuVisualizerPnl"
            Me.mnuVisualizerPnl.Size = New System.Drawing.Size(115, 48)
            '
            'mnuShowOnRight
            '
            Me.mnuShowOnRight.Checked = True
            Me.mnuShowOnRight.CheckOnClick = True
            Me.mnuShowOnRight.CheckState = System.Windows.Forms.CheckState.Checked
            Me.mnuShowOnRight.Name = "mnuShowOnRight"
            Me.mnuShowOnRight.Size = New System.Drawing.Size(114, 22)
            Me.mnuShowOnRight.Text = "&Right"
            '
            'mnuShowOnBottom
            '
            Me.mnuShowOnBottom.CheckOnClick = True
            Me.mnuShowOnBottom.Name = "mnuShowOnBottom"
            Me.mnuShowOnBottom.Size = New System.Drawing.Size(114, 22)
            Me.mnuShowOnBottom.Text = "&Bottom"
            '
            'JsonViewer
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.tvJson)
            Me.Name = "JsonViewer"
            Me.Size = New System.Drawing.Size(792, 558)
            Me.mnuTree.ResumeLayout(False)
            Me.mnuVisualizerPnl.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub

#End Region
        Private imgList As ImageList
        Private WithEvents tvJson As TreeView
        Private mnuTree As ContextMenuStrip
        Private mnuFind As ToolStripMenuItem
        Private mnuExpandAll As ToolStripMenuItem
        Private mnuVisualizerPnl As ContextMenuStrip
        Private mnuShowOnRight As ToolStripMenuItem
        Private mnuShowOnBottom As ToolStripMenuItem
        Private toolStripMenuItem1 As ToolStripSeparator
        Private mnuCopy As ToolStripMenuItem
        Private mnuCopyValue As ToolStripMenuItem

    End Class
End Namespace
