Imports Galaxy.Data.JSON
Imports Galaxy.Workbench
Imports Galaxy.Workbench.CommonDialogs

Public Class Form1 : Implements AppHost

    Public ReadOnly Property ActiveDocument As Form Implements AppHost.ActiveDocument
        Get
            Return Me
        End Get
    End Property

    Private ReadOnly Property AppHost_ClientRectangle As Rectangle Implements AppHost.ClientRectangle
        Get
            Return New Rectangle(Location, Size)
        End Get
    End Property

    Public Event ResizeForm As AppHost.ResizeFormEventHandler Implements AppHost.ResizeForm
    Public Event CloseWorkbench As AppHost.CloseWorkbenchEventHandler Implements AppHost.CloseWorkbench

    Public Sub SetWorkbenchVisible(visible As Boolean) Implements AppHost.SetWorkbenchVisible

    End Sub

    Public Sub SetWindowState(stat As FormWindowState) Implements AppHost.SetWindowState
        WindowState = stat
    End Sub

    Public Sub SetTitle(title As String) Implements AppHost.SetTitle
        Text = title
    End Sub

    Public Sub StatusMessage(msg As String, Optional icon As Image = Nothing) Implements AppHost.StatusMessage

    End Sub

    Public Sub Warning(msg As String) Implements AppHost.Warning

    End Sub

    Public Sub LogText(text As String) Implements AppHost.LogText

    End Sub

    Public Sub ShowProperties(obj As Object) Implements AppHost.ShowProperties

    End Sub

    Private Sub JsonViewer1_Load(sender As Object, e As EventArgs) Handles JsonViewer1.Load
        JsonViewer1.RootTag = "Hello World"
        JsonViewer1.Json = "
{
'a': [1,2,3,4,5], 
'b': {
    'c': true,
    'd': [
        {'flag':true},{'flag':false},{'flag': true}
    ]
},
'c': 'hello world',
'd': 123456
}"
    End Sub

    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        RaiseEvent ResizeForm(Location, Size)
    End Sub

    Private Sub JsonViewer1_FindAction(node As JsonViewerTreeNode) Handles JsonViewer1.FindAction
        Call InputDialog.Input(Of Form2)()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        Call CommonRuntime.Hook(Me)
    End Sub

    Public Function GetDesktopLocation() As Point Implements AppHost.GetDesktopLocation
        Return Location
    End Function

    Public Function GetClientSize() As Size Implements AppHost.GetClientSize
        Return Size
    End Function

    Public Iterator Function GetDocuments() As IEnumerable(Of Form) Implements AppHost.GetDocuments
        Yield Me
    End Function

    Public Function GetDockPanel() As Control Implements AppHost.GetDockPanel

    End Function

    Public Function GetWindowState() As FormWindowState Implements AppHost.GetWindowState
        Return WindowState
    End Function
End Class
