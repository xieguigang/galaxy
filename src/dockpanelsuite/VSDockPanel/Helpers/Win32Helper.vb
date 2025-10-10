Imports System.Drawing
Imports System.Windows.Forms

Namespace Docking
    Public Module Win32Helper
        Private ReadOnly _isRunningOnMono As Boolean = Type.GetType("Mono.Runtime") IsNot Nothing

        Public ReadOnly Property IsRunningOnMono As Boolean
            Get
                Return _isRunningOnMono
            End Get
        End Property

        Friend Function ControlAtPoint(pt As Point) As Control
            Return Control.FromChildHandle(WindowFromPoint(pt))
        End Function

        Friend Function MakeLong(low As Integer, high As Integer) As UInteger
            Return (high << 16) + low
        End Function

        Friend Function HitTestCaption(control As Control) As UInteger
            Dim captionRectangle = New Rectangle(0, 0, control.Width, control.ClientRectangle.Top - control.PointToClient(control.Location).X)
            Return If(captionRectangle.Contains(Control.MousePosition), 2UI, 0UI)
        End Function
    End Module
End Namespace
