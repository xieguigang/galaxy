Imports System.ComponentModel
Imports System.Windows.Forms

Namespace Controls

    Public Class MaterialTabControl
        Inherits System.Windows.Forms.TabControl
        Implements IMaterialControl

        <Browsable(False)>
        Public Property Depth() As Integer Implements IMaterialControl.Depth

        <Browsable(False)>
        Public ReadOnly Property SkinManager() As MaterialSkinManager Implements IMaterialControl.SkinManager
            Get
                Return MaterialSkinManager.Instance
            End Get
        End Property
        <Browsable(False)>
        Public Property MouseState() As MouseState Implements IMaterialControl.MouseState

        Protected Overrides Sub WndProc(ByRef m As Message)
            If m.Msg = &H1328 AndAlso Not DesignMode Then
                m.Result = CType(1, IntPtr)
            Else
                MyBase.WndProc(m)
            End If
        End Sub
    End Class
End Namespace
