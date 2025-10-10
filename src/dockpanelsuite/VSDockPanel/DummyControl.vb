Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms

Namespace WeifenLuo.WinFormsUI.Docking
    <ToolboxItem(False)>
    Friend NotInheritable Class DummyControl
        Inherits Control
        Public Sub New()
            SetStyle(ControlStyles.Selectable, False)
            ResetBackColor()
        End Sub

        Public Overrides Sub ResetBackColor()
            BackColor = SystemColors.ControlLight
        End Sub
    End Class
End Namespace
