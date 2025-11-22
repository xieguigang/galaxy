Imports Galaxy.Workbench.CommonDialogs

Public Class TaskWizard

    Public Property TaskName As String
        Get
            Return LinkLabel1.Text
        End Get
        Set(value As String)
            LinkLabel1.Text = value
        End Set
    End Property

    Dim steps As IWizardUI()
    Dim offset As Integer = 0

    Public Shared Function ShowWizard(taskName As String, ParamArray steps As IWizardUI()) As TaskWizard
        If CommonRuntime.AppHost Is Nothing Then
            Throw New NullReferenceException("the required workbench windows main form is nothing!")
        End If

        Dim wizard As New TaskWizard
        Dim mask As MaskForm = MaskForm.CreateMask(CommonRuntime.AppHost)

        wizard.TaskName = taskName
        wizard.steps = steps

        If mask.ShowDialogForm(wizard) = DialogResult.OK Then

        Else

        End If

        Return wizard
    End Function

    Public Sub ShowStep()
        Dim current = steps(offset)
        Dim ctl As Control = DirectCast(CObj(current), Control)

        If offset > 0 Then
            Dim old = DirectCast(CObj(steps(offset - 1)), Control)

            old.Visible = False
            old.Dock = DockStyle.None
        End If

        ctl.Visible = True
        ctl.Dock = DockStyle.Fill
    End Sub

    ''' <summary>
    ''' ok
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Not steps(offset).OK Then
            Return
        End If

        offset += 1

        If offset = steps.Length Then
            DialogResult = DialogResult.OK
        Else
            ShowStep()
        End If
    End Sub

    ''' <summary>
    ''' cancel
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        DialogResult = DialogResult.Cancel
    End Sub

    Private Sub TaskWizard_Load(sender As Object, e As EventArgs) Handles Me.Load
        For Each [step] As IWizardUI In steps
            Dim ctl As Control = DirectCast(CObj(steps), Control)

            Controls.Add(ctl)
            ctl.Visible = False
            ctl.Dock = DockStyle.Fill
        Next

        Call ShowStep()
    End Sub
End Class

Public Interface IWizardUI

    ReadOnly Property Title As String

    Function OK() As Boolean

End Interface