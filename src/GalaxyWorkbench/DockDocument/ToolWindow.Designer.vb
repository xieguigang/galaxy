#Region "Microsoft.VisualBasic::357c96670e6bd77560009c83f5eddb74, mzkit\src\mzkit\mzkit\pages\dockWindow\base\ToolWindow.Designer.vb"

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

'   Total Lines: 94
'    Code Lines: 62
' Comment Lines: 26
'   Blank Lines: 6
'     File Size: 4.47 KB


' Class ToolWindow
' 
'     Sub: Dispose, InitializeComponent
' 
' /********************************************************************************/

#End Region

Imports Microsoft.VisualStudio.WinForms.Docking

Namespace DockDocument

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class ToolWindow
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ToolWindow))
            DockContextMenuStrip1 = New ContextMenuStrip(components)
            FloatToolStripMenuItem = New ToolStripMenuItem()
            DockToolStripMenuItem = New ToolStripMenuItem()
            AutoHideToolStripMenuItem = New ToolStripMenuItem()
            seperatorLine = New ToolStripSeparator()
            CloseToolStripMenuItem = New ToolStripMenuItem()
            DockContextMenuStrip1.SuspendLayout()
            SuspendLayout()
            ' 
            ' DockContextMenuStrip1
            ' 
            DockContextMenuStrip1.Items.AddRange(New ToolStripItem() {FloatToolStripMenuItem, DockToolStripMenuItem, AutoHideToolStripMenuItem, seperatorLine, CloseToolStripMenuItem})
            DockContextMenuStrip1.Name = "ContextMenuStrip1"
            DockContextMenuStrip1.Size = New Size(129, 98)
            ' 
            ' FloatToolStripMenuItem
            ' 
            FloatToolStripMenuItem.Image = CType(resources.GetObject("FloatToolStripMenuItem.Image"), Image)
            FloatToolStripMenuItem.Name = "FloatToolStripMenuItem"
            FloatToolStripMenuItem.Size = New Size(128, 22)
            FloatToolStripMenuItem.Text = "Float"
            ' 
            ' DockToolStripMenuItem
            ' 
            DockToolStripMenuItem.Image = CType(resources.GetObject("DockToolStripMenuItem.Image"), Image)
            DockToolStripMenuItem.Name = "DockToolStripMenuItem"
            DockToolStripMenuItem.Size = New Size(128, 22)
            DockToolStripMenuItem.Text = "Dock"
            ' 
            ' AutoHideToolStripMenuItem
            ' 
            AutoHideToolStripMenuItem.Name = "AutoHideToolStripMenuItem"
            AutoHideToolStripMenuItem.Size = New Size(128, 22)
            AutoHideToolStripMenuItem.Text = "Auto Hide"
            ' 
            ' seperatorLine
            ' 
            seperatorLine.Name = "seperatorLine"
            seperatorLine.Size = New Size(125, 6)
            ' 
            ' CloseToolStripMenuItem
            ' 
            CloseToolStripMenuItem.Image = CType(resources.GetObject("CloseToolStripMenuItem.Image"), Image)
            CloseToolStripMenuItem.Name = "CloseToolStripMenuItem"
            CloseToolStripMenuItem.Size = New Size(128, 22)
            CloseToolStripMenuItem.Text = "Close"
            ' 
            ' ToolWindow
            ' 
            AutoScaleDimensions = New SizeF(7F, 15F)
            AutoScaleMode = AutoScaleMode.Font
            ClientSize = New Size(337, 779)
            DockAreas = DockAreas.Float Or DockAreas.DockLeft Or DockAreas.DockRight Or DockAreas.DockTop Or DockAreas.DockBottom Or DockAreas.Document
            Margin = New Padding(4, 3, 4, 3)
            Name = "ToolWindow"
            ShowHint = DockState.Unknown
            Text = "Tool Window"
            DockContextMenuStrip1.ResumeLayout(False)
            ResumeLayout(False)

        End Sub

        Friend WithEvents DockContextMenuStrip1 As ContextMenuStrip
        Friend WithEvents FloatToolStripMenuItem As ToolStripMenuItem
        Friend WithEvents DockToolStripMenuItem As ToolStripMenuItem
        Friend WithEvents AutoHideToolStripMenuItem As ToolStripMenuItem
        Friend WithEvents seperatorLine As ToolStripSeparator
        Friend WithEvents CloseToolStripMenuItem As ToolStripMenuItem
    End Class
End Namespace