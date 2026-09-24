#Region "Microsoft.VisualBasic::b5c198c1768ce486bf4c15e4adfb693e, mzkit\src\mzkit\mzkit\pages\dockWindow\base\DocumentWindow.Designer.vb"

' Author:
' 
'       xieguigang (gg.xie@bionovogene.com, BioNovoGene Co., LTD.)
' 
' Copyright (c) 2018 gg.xie@bionovogene.com, BioNovoGene Co., LTD.
' 
' 
' MIT License
' 
' 
' Permission is hereby granted, free of charge, to any person obtaining a copy
' of this software and associated documentation files (the "Software"), to deal
' in the Software without restriction, including without limitation the rights
' to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
' copies of the Software, and to permit persons to whom the Software is
' furnished to do so, subject to the following conditions:
' 
' The above copyright notice and this permission notice shall be included in all
' copies or substantial portions of the Software.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
' IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
' FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
' LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
' SOFTWARE.



' /********************************************************************************/

' Summaries:


' Code Statistics:

'   Total Lines: 126
'    Code Lines: 82
' Comment Lines: 38
'   Blank Lines: 6
'     File Size: 6.71 KB


' Class DocumentWindow
' 
'     Sub: Dispose, InitializeComponent
' 
' /********************************************************************************/

#End Region

Imports Microsoft.VisualStudio.WinForms.Docking

Namespace DockDocument

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class DocumentWindow
        Inherits DockContent

        'Form overrides dispose to clean up the component list.
        <System.Diagnostics.DebuggerNonUserCode()>
        Protected Overrides Sub Dispose(disposing As Boolean)
            Try
                If disposing AndAlso components IsNot Nothing Then
                    components.Dispose()
                End If
            Finally
                MyBase.Dispose(disposing)
            End Try
        End Sub

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()>
        Private Sub InitializeComponent()
            components = New ComponentModel.Container()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DocumentWindow))
            DockContextMenuStrip1 = New ContextMenuStrip(components)
            SaveDocumentToolStripMenuItem = New ToolStripMenuItem()
            CloseToolStripMenuItem = New ToolStripMenuItem()
            CloseAllDocumentsToolStripMenuItem = New ToolStripMenuItem()
            CloseAllButThisToolStripMenuItem = New ToolStripMenuItem()
            seperatorLine1 = New ToolStripSeparator()
            CopyFullPathToolStripMenuItem = New ToolStripMenuItem()
            OpenContainingFolderToolStripMenuItem = New ToolStripMenuItem()
            seperatorLine2 = New ToolStripSeparator()
            FloatToolStripMenuItem = New ToolStripMenuItem()
            DockContextMenuStrip1.SuspendLayout()
            SuspendLayout()
            ' 
            ' DockContextMenuStrip1
            ' 
            DockContextMenuStrip1.Items.AddRange(New ToolStripItem() {SaveDocumentToolStripMenuItem, CloseToolStripMenuItem, CloseAllDocumentsToolStripMenuItem, CloseAllButThisToolStripMenuItem, seperatorLine1, CopyFullPathToolStripMenuItem, OpenContainingFolderToolStripMenuItem, seperatorLine2, FloatToolStripMenuItem})
            DockContextMenuStrip1.Name = "ContextMenuStrip1"
            DockContextMenuStrip1.Size = New Size(202, 170)
            ' 
            ' SaveDocumentToolStripMenuItem
            ' 
            SaveDocumentToolStripMenuItem.Image = CType(resources.GetObject("SaveDocumentToolStripMenuItem.Image"), Image)
            SaveDocumentToolStripMenuItem.Name = "SaveDocumentToolStripMenuItem"
            SaveDocumentToolStripMenuItem.Size = New Size(201, 22)
            SaveDocumentToolStripMenuItem.Text = "Save Document"
            ' 
            ' CloseToolStripMenuItem
            ' 
            CloseToolStripMenuItem.Image = CType(resources.GetObject("CloseToolStripMenuItem.Image"), Image)
            CloseToolStripMenuItem.Name = "CloseToolStripMenuItem"
            CloseToolStripMenuItem.Size = New Size(201, 22)
            CloseToolStripMenuItem.Text = "Close"
            ' 
            ' CloseAllDocumentsToolStripMenuItem
            ' 
            CloseAllDocumentsToolStripMenuItem.Name = "CloseAllDocumentsToolStripMenuItem"
            CloseAllDocumentsToolStripMenuItem.Size = New Size(201, 22)
            CloseAllDocumentsToolStripMenuItem.Text = "Close All Documents"
            ' 
            ' CloseAllButThisToolStripMenuItem
            ' 
            CloseAllButThisToolStripMenuItem.Name = "CloseAllButThisToolStripMenuItem"
            CloseAllButThisToolStripMenuItem.Size = New Size(201, 22)
            CloseAllButThisToolStripMenuItem.Text = "Close All But This"
            ' 
            ' seperatorLine1
            ' 
            seperatorLine1.Name = "seperatorLine1"
            seperatorLine1.Size = New Size(198, 6)
            ' 
            ' CopyFullPathToolStripMenuItem
            ' 
            CopyFullPathToolStripMenuItem.Name = "CopyFullPathToolStripMenuItem"
            CopyFullPathToolStripMenuItem.Size = New Size(201, 22)
            CopyFullPathToolStripMenuItem.Text = "Copy Full Path"
            ' 
            ' OpenContainingFolderToolStripMenuItem
            ' 
            OpenContainingFolderToolStripMenuItem.Image = CType(resources.GetObject("OpenContainingFolderToolStripMenuItem.Image"), Image)
            OpenContainingFolderToolStripMenuItem.Name = "OpenContainingFolderToolStripMenuItem"
            OpenContainingFolderToolStripMenuItem.Size = New Size(201, 22)
            OpenContainingFolderToolStripMenuItem.Text = "Open Containing Folder"
            ' 
            ' seperatorLine2
            ' 
            seperatorLine2.Name = "seperatorLine2"
            seperatorLine2.Size = New Size(198, 6)
            ' 
            ' FloatToolStripMenuItem
            ' 
            FloatToolStripMenuItem.Image = CType(resources.GetObject("FloatToolStripMenuItem.Image"), Image)
            FloatToolStripMenuItem.Name = "FloatToolStripMenuItem"
            FloatToolStripMenuItem.Size = New Size(201, 22)
            FloatToolStripMenuItem.Text = "Float"
            ' 
            ' DocumentWindow
            ' 
            AutoScaleDimensions = New SizeF(7F, 15F)
            AutoScaleMode = AutoScaleMode.Font
            ClientSize = New Size(744, 405)
            DockAreas = DockAreas.Float Or DockAreas.DockLeft Or DockAreas.DockRight Or DockAreas.DockTop Or DockAreas.DockBottom Or DockAreas.Document
            Margin = New Padding(4, 3, 4, 3)
            Name = "DocumentWindow"
            ShowHint = DockState.Unknown
            Text = "Document"
            DockContextMenuStrip1.ResumeLayout(False)
            ResumeLayout(False)

        End Sub

        Protected Friend WithEvents DockContextMenuStrip1 As ContextMenuStrip
        Protected Friend WithEvents SaveDocumentToolStripMenuItem As ToolStripMenuItem
        Protected Friend WithEvents CloseToolStripMenuItem As ToolStripMenuItem
        Protected Friend WithEvents CloseAllDocumentsToolStripMenuItem As ToolStripMenuItem
        Protected Friend WithEvents CloseAllButThisToolStripMenuItem As ToolStripMenuItem
        Protected Friend WithEvents seperatorLine1 As ToolStripSeparator
        Protected Friend WithEvents CopyFullPathToolStripMenuItem As ToolStripMenuItem
        Protected Friend WithEvents OpenContainingFolderToolStripMenuItem As ToolStripMenuItem
        Protected Friend WithEvents seperatorLine2 As ToolStripSeparator
        Protected Friend WithEvents FloatToolStripMenuItem As ToolStripMenuItem

    End Class
End Namespace