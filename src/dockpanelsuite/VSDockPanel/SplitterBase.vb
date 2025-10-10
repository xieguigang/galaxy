Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms

Namespace WeifenLuo.WinFormsUI.Docking
    <ToolboxItem(False)>
    Public Class SplitterBase
        Inherits Control
        Public Sub New()
            SetStyle(ControlStyles.Selectable, False)
        End Sub

        Public Overrides Property Dock As DockStyle
            Get
                Return MyBase.Dock
            End Get
            Set(value As DockStyle)
                SuspendLayout()
                MyBase.Dock = value

                If Dock = DockStyle.Left OrElse Dock = DockStyle.Right Then
                    Width = SplitterSize
                ElseIf Dock = DockStyle.Top OrElse Dock = DockStyle.Bottom Then
                    Height = SplitterSize
                Else
                    Bounds = Rectangle.Empty
                End If

                If Dock = DockStyle.Left OrElse Dock = DockStyle.Right Then
                    MyBase.Cursor = Cursors.VSplit
                ElseIf Dock = DockStyle.Top OrElse Dock = DockStyle.Bottom Then
                    MyBase.Cursor = Cursors.HSplit
                Else
                    MyBase.Cursor = Cursors.Default
                End If

                ResumeLayout()
            End Set
        End Property

        Protected Overridable ReadOnly Property SplitterSize As Integer
            Get
                Return 0
            End Get
        End Property

        Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
            MyBase.OnMouseDown(e)

            If e.Button <> MouseButtons.Left Then Return

            StartDrag()
        End Sub

        Protected Overridable Sub StartDrag()
        End Sub

        Protected Overrides Sub WndProc(ByRef m As Message)
            ' eat the WM_MOUSEACTIVATE message
            If m.Msg = Win32.Msgs.WM_MOUSEACTIVATE Then Return

            MyBase.WndProc(m)
        End Sub
    End Class
End Namespace
