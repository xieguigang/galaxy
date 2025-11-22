Imports Galaxy.Workbench.CommonDialogs
Imports Microsoft.VisualBasic.Imaging

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
    Dim yes As Boolean = False
    Dim titles As Label()

    Public Shared Function ShowWizard(taskName As String, ParamArray steps As IWizardUI()) As TaskWizard
        If CommonRuntime.AppHost Is Nothing Then
            Throw New NullReferenceException("the required workbench windows main form is nothing!")
        End If

        Dim wizard As New TaskWizard
        Dim mask As MaskForm = MaskForm.CreateMask(CommonRuntime.AppHost)

        wizard.TaskName = taskName
        wizard.steps = steps

        If mask.ShowDialogForm(wizard) = DialogResult.OK Then
            wizard.yes = True
        Else

        End If

        Return wizard
    End Function

    Public Overloads Sub [Finally](act As Action)
        If yes AndAlso Not act Is Nothing Then
            Call act()
        End If
    End Sub

    Public Sub ShowStep()
        Dim current = steps(offset)
        Dim ctl As Control = DirectCast(CObj(current), Control)

        For i As Integer = 0 To titles.Length - 1
            If i = offset Then
                titles(i).ForeColor = Color.White
            Else
                titles(i).ForeColor = Color.Black
            End If
        Next

        If offset > 0 Then
            Dim old = DirectCast(CObj(steps(offset - 1)), Control)

            old.Visible = False
            old.Dock = DockStyle.None

            current.SetData(steps(offset - 1).GetData)
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
        Dim offset As Integer = 0

        titles = New Label(steps.Length - 1) {}

        For Each [step] As IWizardUI In steps
            Dim ctl As Control = DirectCast(CObj([step]), Control)
            Dim label As New Label

            Controls.Add(label)

            label.Text = [step].Title
            label.BackColor = "#4270a5".TranslateColor
            label.ForeColor = Color.Black
            label.Location = New Point(10, 10 + offset * 20)
            label.AutoSize = True

            titles(offset) = label
            offset += 1

            GroupBox1.Controls.Add(ctl)
            ctl.Visible = False
            ctl.Dock = DockStyle.Fill

            label.BringToFront()
        Next

        Call ShowStep()
    End Sub
End Class

Public Interface IWizardUI : Inherits IDataContainer

    ReadOnly Property Title As String

    Function OK() As Boolean

End Interface