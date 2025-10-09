Imports System.Windows.Forms

Partial Public Class CommandLinkButton
    Inherits Button
    Private Const BS_COMMANDLINK As Integer = &HE
    Protected Overrides ReadOnly Property CreateParams As CreateParams
        Get
            Dim cParams = MyBase.CreateParams
            cParams.Style = cParams.Style Or BS_COMMANDLINK
            Return cParams
        End Get
    End Property
    Public Sub New()
        InitializeComponent()
        FlatStyle = FlatStyle.System
    End Sub
End Class
