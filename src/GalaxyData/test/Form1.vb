Public Class Form1

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
End Class
