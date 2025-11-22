Imports Galaxy.Workbench.CommonDialogs

Public Class TaskWizard

    Public Property TaskName As String
        Get
            Return GroupBox1.Text
        End Get
        Set(value As String)
            GroupBox1.Text = value
        End Set
    End Property

    Private Sub TaskWizard_Load(sender As Object, e As EventArgs) Handles Me.Load

    End Sub

    Public Shared Function ShowWizard() As TaskWizard
        If CommonRuntime.AppHost Is Nothing Then
            Throw New NullReferenceException("the required workbench windows main form is nothing!")
        End If

        Dim wizard As New TaskWizard
        Dim mask As MaskForm = MaskForm.CreateMask(CommonRuntime.AppHost)

        If mask.ShowDialogForm(wizard) = DialogResult.OK Then

        Else

        End If

        Return wizard
    End Function

End Class