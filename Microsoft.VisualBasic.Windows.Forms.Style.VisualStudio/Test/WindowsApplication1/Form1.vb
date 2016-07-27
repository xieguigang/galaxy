Imports Microsoft.VisualBasic.Windows.Forms.Style

Public Class Form1

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Call VisualStudio.ApplyStyle(Caption1)
    End Sub

    Private Sub Caption1_OnClickClose() Handles Caption1.OnClickClose
        Close()
    End Sub

    Private Sub Caption1_OnClickMaximize() Handles Caption1.OnClickMaximize
        WindowState = FormWindowState.Maximized
    End Sub

    Private Sub Caption1_OnClickMinimize() Handles Caption1.OnClickMinimize
        WindowState = FormWindowState.Minimized
    End Sub

    Private Sub Caption1_OnClickRestore() Handles Caption1.OnClickRestore
        WindowState = FormWindowState.Normal
    End Sub

    Private Sub Form1_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        BorderColor = Color.FromArgb(0, 122, 204)
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        DrawBorderFrame = True
        BackColor = Color.FromArgb(45, 45, 48)
    End Sub

    Private Sub Form1_LostFocus(sender As Object, e As EventArgs) Handles Me.LostFocus
        BorderColor = Color.Gray
    End Sub
End Class
