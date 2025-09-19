Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
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
