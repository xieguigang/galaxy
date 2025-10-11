Imports System.Drawing
Imports System.Windows.Forms
Imports ExcelPad.RibbonLib.Controls
Imports Microsoft.VisualBasic.Data.Framework.IO
Imports Microsoft.VisualBasic.Drawing
Imports Microsoft.VisualBasic.MIME.Office.Excel
Imports Microsoft.VisualStudio.WinForms.Docking
Imports RibbonLib
Imports ThemeVS2015

Public Class FormMain

    Dim vS2015LightTheme1 As New VS2015LightTheme
    Dim vsToolStripExtender1 As New VisualStudioToolStripExtender
    Dim ribbon As New Ribbon
    Dim ribbonItem As RibbonItems

    ReadOnly _toolStripProfessionalRenderer As New ToolStripProfessionalRenderer()

    Shared Sub New()
        Call SkiaDriver.Register()
    End Sub

    Private Sub initializeVSPanel()
        m_dockPanel.ShowDocumentIcon = True

        Me.m_dockPanel.Dock = DockStyle.Fill
        Me.m_dockPanel.DockBackColor = Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.m_dockPanel.DockBottomPortion = 150.0R
        Me.m_dockPanel.DockLeftPortion = 200.0R
        Me.m_dockPanel.DockRightPortion = 200.0R
        Me.m_dockPanel.DockTopPortion = 150.0R
        Me.m_dockPanel.Font = New Font("Tahoma", 11.0!, FontStyle.Regular, GraphicsUnit.World, CType(0, Byte))

        Me.m_dockPanel.Name = "dockPanel"
        Me.m_dockPanel.RightToLeftLayout = True
        Me.m_dockPanel.ShowAutoHideContentOnHover = False

        Me.m_dockPanel.TabIndex = 0

        Call SetSchema(Nothing, Nothing)
    End Sub

    Private Sub SetSchema(sender As Object, e As EventArgs)
        m_dockPanel.Theme = vS2015LightTheme1
        EnableVSRenderer(VisualStudioToolStripExtender.VsVersion.Vs2015, vS2015LightTheme1)

        If m_dockPanel.Theme.ColorPalette IsNot Nothing Then
            StatusStrip1.BackColor = m_dockPanel.Theme.ColorPalette.MainWindowStatusBarDefault.Background
        End If
    End Sub

    Private Sub EnableVSRenderer(version As VisualStudioToolStripExtender.VsVersion, theme As ThemeBase)
        vsToolStripExtender1.SetStyle(StatusStrip1, version, theme)
    End Sub

    Private Sub FormMain_Load(sender As Object, e As EventArgs) Handles Me.Load
        ribbon.Dock = DockStyle.Top
        ribbon.ResourceName = "ExcelPad.RibbonMarkup.ribbon"
        ribbonItem = New RibbonItems(ribbon)

        Call Controls.Add(ribbon)
        Call initializeVSPanel()

        AddHandler ribbonItem.ButtonOpen.ExecuteEvent, AddressOf OpenFile
    End Sub

    Private Sub OpenFile()
        Using file As New OpenFileDialog With {.Filter = "Excel Data Table(*.csv;*.xlsx)|*.csv;*.xlsx"}
            If file.ShowDialog = DialogResult.OK Then
                If file.FileName.ExtensionSuffix("csv") Then
                    Dim data As DataFrameResolver = DataFrameResolver.Load(file.FileName)
                    Dim page As New FormDataSheet With {.TabText = file.FileName.FileName}

                    page.Show(m_dockPanel)
                    page.DockState = DockState.Document
                    page.LoadData(data)
                Else
                    Dim pkg = XLSX.File.Open(file.FileName)

                    For Each name As String In pkg.SheetNames
                        Dim data As DataFrameResolver = DataFrameResolver.CreateObject(pkg.GetTable(name))
                        Dim page As New FormDataSheet With {.TabText = file.FileName.FileName & $" [{name}]"}

                        page.Show(m_dockPanel)
                        page.DockState = DockState.Document
                        page.LoadData(data)
                    Next
                End If
            End If
        End Using
    End Sub
End Class
