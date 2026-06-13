Imports Galaxy.Workbench
Imports KeySigned.RibbonLib.Controls
Imports Microsoft.VisualBasic.Drawing
Imports Microsoft.VisualStudio.WinForms.Docking
Imports ThemeVS2015

Public Class FormMain : Implements AppHost

    Public ReadOnly Property ActiveDocument As Form Implements AppHost.ActiveDocument
        Get
            Return m_dockPanel.ActiveDocument
        End Get
    End Property

    Private ReadOnly Property AppHost_ClientRectangle As Rectangle Implements AppHost.ClientRectangle
        Get
            Return New Rectangle(Location, ClientSize)
        End Get
    End Property

    Public Event ResizeForm As AppHost.ResizeFormEventHandler Implements AppHost.ResizeForm
    Public Event CloseWorkbench As AppHost.CloseWorkbenchEventHandler Implements AppHost.CloseWorkbench

    Dim ribbon As RibbonItems
    Dim vS2015LightTheme1 As New VS2015LightTheme
    Dim vsToolStripExtender1 As New VisualStudioToolStripExtender

    ReadOnly _toolStripProfessionalRenderer As New ToolStripProfessionalRenderer()

    Private Sub FormMain_Load(sender As Object, e As EventArgs) Handles Me.Load
        ribbon = New RibbonItems(Ribbon1)

        AddHandler ribbon.ButtonKeyGenerator.ExecuteEvent, Sub() Call KeyGeneratorToolStripMenuItem1_Click()
        AddHandler ribbon.ButtonLicenseGenerator.ExecuteEvent, Sub() Call LicenseGeneratorToolStripMenuItem_Click()

        Call initializeVSPanel()
        Call CommonRuntime.Hook(Me)
        Call CommonRuntime.RegisterOutputWindow()
    End Sub

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

    Private Sub KeyGeneratorToolStripMenuItem1_Click()
        Call CommonRuntime.ShowSingleDocument(Of KeyGeneratorForm)()
    End Sub

    Private Sub LicenseGeneratorToolStripMenuItem_Click()
        Call New LicenseGeneratorForm().ShowDialog()
    End Sub

    Public Sub SetWorkbenchVisible(visible As Boolean) Implements AppHost.SetWorkbenchVisible
        Me.Visible = visible
    End Sub

    Public Sub SetWindowState(stat As FormWindowState) Implements AppHost.SetWindowState
        Me.WindowState = stat
    End Sub

    Public Sub SetTitle(title As String) Implements AppHost.SetTitle
        Me.Text = title
    End Sub

    Public Sub StatusMessage(msg As String, Optional icon As Image = Nothing) Implements AppHost.StatusMessage
        ToolStripStatusLabel1.Text = msg
        ToolStripStatusLabel1.Image = icon
    End Sub

    Public Sub Warning(msg As String) Implements AppHost.Warning
        ToolStripStatusLabel1.Text = msg
    End Sub

    Public Sub LogText(text As String) Implements AppHost.LogText
        Call CommonRuntime.GetOutputWindow.AppendLine(text)
    End Sub

    Public Sub ShowProperties(obj As Object) Implements AppHost.ShowProperties
        Call CommonRuntime.GetPropertyWindow.SetObject(obj)
    End Sub

    Public Function GetDesktopLocation() As Point Implements AppHost.GetDesktopLocation
        Return Location
    End Function

    Public Function GetClientSize() As Size Implements AppHost.GetClientSize
        Return ClientSize
    End Function

    Public Function GetDocuments() As IEnumerable(Of Form) Implements AppHost.GetDocuments
        Return From doc As IDockContent In m_dockPanel.Documents Select DirectCast(doc, Form)
    End Function

    Public Function GetDockPanel() As Control Implements AppHost.GetDockPanel
        Return m_dockPanel
    End Function

    Public Function GetWindowState() As FormWindowState Implements AppHost.GetWindowState
        Return WindowState
    End Function

    Private Sub FormMain_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Call CommonRuntime.SaveUISettings()
        RaiseEvent CloseWorkbench(e)
    End Sub

    Private Sub FormMain_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        RaiseEvent ResizeForm(Location, Size)
    End Sub
End Class
