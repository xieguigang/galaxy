Imports Galaxy.Data.JSON
Imports Galaxy.Workbench
Imports Galaxy.Workbench.CommonDialogs

Public Class Form1 : Implements AppHost

    Public Event ResizeForm As AppHost.ResizeFormEventHandler Implements AppHost.ResizeForm

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
End Class
