#Region "Microsoft.VisualBasic::00df72506357ca476f7d1afe80dd4cd1, mzkit\src\mzkit\mzkit\pages\dockWindow\tools\PropertyWindow.designer.vb"

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

    '   Total Lines: 60
    '    Code Lines: 36
    ' Comment Lines: 17
    '   Blank Lines: 7
    '     File Size: 2.36 KB


    '     Class PropertyWindow
    ' 
    '         Sub: Dispose, InitializeComponent
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System

Namespace DockDocument.Presets
    Partial Class PropertyWindow
        ''' <summary>
        ''' Required designer variable.
        ''' </summary>
        Private components As System.ComponentModel.IContainer = Nothing

        ''' <summary>
        ''' Clean up any resources being used.
        ''' </summary>
        ''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        Protected Overrides Sub Dispose(disposing As Boolean)
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If

            MyBase.Dispose(disposing)
        End Sub

#Region "Windows Form Designer generated code"
        ''' <summary>
        ''' Required method for Designer support - do not modify
        ''' the contents of this method with the code editor.
        ''' </summary>
        Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container()
            Me.propertyGrid = New System.Windows.Forms.PropertyGrid()
            Me.SuspendLayout()
            '
            'propertyGrid
            '
            Me.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill
            Me.propertyGrid.LineColor = System.Drawing.SystemColors.ScrollBar
            Me.propertyGrid.Location = New System.Drawing.Point(0, 3)
            Me.propertyGrid.Name = "propertyGrid"
            Me.propertyGrid.Size = New System.Drawing.Size(348, 573)
            Me.propertyGrid.TabIndex = 0
            '
            'PropertyWindow
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
            Me.ClientSize = New System.Drawing.Size(348, 579)
            Me.Controls.Add(Me.propertyGrid)
            Me.DockAreas = CType((((((Microsoft.VisualStudio.WinForms.Docking.DockAreas.Float Or Microsoft.VisualStudio.WinForms.Docking.DockAreas.DockLeft) _
            Or Microsoft.VisualStudio.WinForms.Docking.DockAreas.DockRight) _
            Or Microsoft.VisualStudio.WinForms.Docking.DockAreas.DockTop) _
            Or Microsoft.VisualStudio.WinForms.Docking.DockAreas.DockBottom) _
            Or Microsoft.VisualStudio.WinForms.Docking.DockAreas.Document), Microsoft.VisualStudio.WinForms.Docking.DockAreas)
            Me.DoubleBuffered = True
            Me.HideOnClose = True
            Me.Name = "PropertyWindow"
            Me.Padding = New System.Windows.Forms.Padding(0, 3, 0, 3)
            Me.ShowHint = Microsoft.VisualStudio.WinForms.Docking.DockState.DockRight
            Me.TabText = "Properties"
            Me.Text = "Properties"
            Me.ResumeLayout(False)

        End Sub
#End Region

        Friend propertyGrid As System.Windows.Forms.PropertyGrid
    End Class
End Namespace
