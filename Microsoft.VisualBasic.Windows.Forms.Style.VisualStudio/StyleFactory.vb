Imports Microsoft.VisualBasic.Windows.Forms.Controls

Public Module StyleFactory

    Public Sub ApplyStyle(caption As Caption)
        Dim min As New UI.ImageButton With {
            .Highlight = My.Resources.MinHighlight,
            .Normal = My.Resources.MinNormal,
            .Press = My.Resources.MinPress
        }
        Dim max As New UI.CheckButton With {
            .Highlight = My.Resources.MaxHighlight,
            .Normal = My.Resources.MaxNormal,
            .Press = My.Resources.MaxPress,
            .Checked = My.Resources.RestoreNormal
        }
        Dim close As New UI.ImageButton With {
            .Highlight = My.Resources.CloseHighlight,
            .Normal = My.Resources.CloseNormal,
            .Press = My.Resources.ClosePress
        }

        Call caption.ApplyUI(min, max, close)
    End Sub
End Module
