Imports System.Collections.Generic
Imports System.Drawing
Imports System.Reflection
Imports System.Threading
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.Windows.Forms
Imports Microsoft.VisualBasic.Windows.Forms.Controls

Partial Public Class MainForm
    Inherits MaterialForm
    Private ReadOnly materialSkinManager As MaterialSkinManager
    Public Sub New()
        InitializeComponent()

        ' Initialize MaterialSkinManager
        materialSkinManager = MaterialSkinManager.Instance
        materialSkinManager.AddFormToManage(Me)
        materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT
        materialSkinManager.ColorScheme = New ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE)

        ' Add dummy data to the listview
        seedListView()
    End Sub

    Private Sub seedListView()
        'Define
        Dim data = {{"Lollipop", "392", "0.2", "0"}, {"KitKat", "518", "26.0", "7"}, {"Ice cream sandwich", "237", "9.0", "4.3"}, {"Jelly Bean", "375", "0.0", "0.0"}, {"Honeycomb", "408", "3.2", "6.5"}}

        'Add
        For Each version As String() In data.RowIterator
            Dim item = New ListViewItem(version)
            materialListView1.Items.Add(item)
        Next
        For Each version As String() In data.RowIterator
            Dim item = New ListViewItem(version)
            materialListView1.Items.Add(item)
        Next
    End Sub

    Private Sub materialButton1_Click(sender As Object, e As EventArgs)
        materialSkinManager.Theme = If(materialSkinManager.Theme = MaterialSkinManager.Themes.DARK, MaterialSkinManager.Themes.LIGHT, MaterialSkinManager.Themes.DARK)
    End Sub

    Private colorSchemeIndex As Integer
    Private Sub materialRaisedButton1_Click(sender As Object, e As EventArgs)
        colorSchemeIndex += 1
        If colorSchemeIndex > 2 Then
            colorSchemeIndex = 0
        End If

        'These are just example color schemes
        Select Case colorSchemeIndex
            Case 0
                materialSkinManager.ColorScheme = New ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE)
                Exit Select
            Case 1
                materialSkinManager.ColorScheme = New ColorScheme(Primary.Indigo500, Primary.Indigo700, Primary.Indigo100, Accent.Pink200, TextShade.WHITE)
                Exit Select
            Case 2
                materialSkinManager.ColorScheme = New ColorScheme(Primary.Green600, Primary.Green700, Primary.Green200, Accent.Red100, TextShade.WHITE)
                Exit Select
        End Select
    End Sub
End Class
