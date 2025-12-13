Imports System.Drawing
Imports System.Windows.Forms
Imports ExcelPad.RibbonLib.Controls
Imports Galaxy.Workbench
Imports Microsoft.VisualBasic.Data.Framework.IO
Imports Microsoft.VisualBasic.Drawing
Imports Microsoft.VisualBasic.MIME.Office.Excel
Imports Microsoft.VisualStudio.WinForms.Docking
Imports RibbonLib
Imports ThemeVS2015

Public Class FormMain : Implements AppHost

    Dim vS2015LightTheme1 As New VS2015LightTheme
    Dim vsToolStripExtender1 As New VisualStudioToolStripExtender
    Dim ribbon As New Ribbon
    Dim ribbonItem As RibbonItems

    ReadOnly _toolStripProfessionalRenderer As New ToolStripProfessionalRenderer()

    Public Event ResizeForm As AppHost.ResizeFormEventHandler Implements AppHost.ResizeForm
    Public Event CloseWorkbench As AppHost.CloseWorkbenchEventHandler Implements AppHost.CloseWorkbench

    Private ReadOnly Property AppHost_ClientRectangle As Rectangle Implements AppHost.ClientRectangle
        Get
            Return Me.ClientRectangle
        End Get
    End Property

    Public ReadOnly Property ActiveDocument As Form Implements AppHost.ActiveDocument
        Get
            Return m_dockPanel.ActiveDocument
        End Get
    End Property

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
        AddHandler ribbonItem.ButtonImportsData.ExecuteEvent, AddressOf ImportsFile

        Call CommonRuntime.Hook(Me)
        Call CommonRuntime.RegisterOutputWindow()
    End Sub

    Private Sub ImportsFile()
        Call TaskWizard.ShowWizard("Imports Data File")
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

    Public Sub SetWorkbenchVisible(visible As Boolean) Implements AppHost.SetWorkbenchVisible
        Me.Visible = visible
    End Sub

    Public Sub SetWindowState(stat As FormWindowState) Implements AppHost.SetWindowState
        Me.WindowState = stat
    End Sub

    Public Function GetDesktopLocation() As Point Implements AppHost.GetDesktopLocation
        Return Location
    End Function

    Public Function GetClientSize() As Size Implements AppHost.GetClientSize
        Return Size
    End Function

    Public Function GetDocuments() As IEnumerable(Of Form) Implements AppHost.GetDocuments
        Return m_dockPanel.Documents.OfType(Of Form)()
    End Function

    Public Function GetDockPanel() As Control Implements AppHost.GetDockPanel
        Return m_dockPanel
    End Function

    Public Function GetWindowState() As FormWindowState Implements AppHost.GetWindowState
        Return WindowState
    End Function

    Public Sub SetTitle(title As String) Implements AppHost.SetTitle
        Text = title
    End Sub

    Public Sub StatusMessage(msg As String, Optional icon As Image = Nothing) Implements AppHost.StatusMessage
        ToolStripStatusLabel1.Text = msg
        ToolStripStatusLabel1.Image = icon
    End Sub

    Public Sub Warning(msg As String) Implements AppHost.Warning
        Call StatusMessage(msg, Icons8.Warning)
    End Sub

    Public Sub LogText(text As String) Implements AppHost.LogText
        Call CommonRuntime.GetOutputWindow.AppendLine(text)
    End Sub

    Public Sub ShowProperties(obj As Object) Implements AppHost.ShowProperties
        Call CommonRuntime.GetPropertyWindow.SetObject(obj)
    End Sub

    Private Sub FormMain_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Call CommonRuntime.SaveUISettings()

        RaiseEvent CloseWorkbench(e)
    End Sub

    Private Sub FormMain_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        RaiseEvent ResizeForm(Location, Size)
    End Sub
End Class
