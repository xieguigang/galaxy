Imports Galaxy.Data.TableSheet
Imports Microsoft.VisualBasic.Data.Framework.IO

Public Class FormDataSheet

    Dim WithEvents table As New AdvancedDataGridView
    Dim WithEvents toolbar As New AdvancedDataGridViewSearchToolBar

    Private Sub FormDataSheet_Load(sender As Object, e As EventArgs) Handles Me.Load
        Controls.Add(table)
        Controls.Add(toolbar)

        table.Dock = System.Windows.Forms.DockStyle.Fill
        toolbar.Dock = System.Windows.Forms.DockStyle.Top
    End Sub

    Public Overloads Sub LoadData(table As DataFrameResolver)

    End Sub
End Class