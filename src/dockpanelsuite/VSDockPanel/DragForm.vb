Imports System
Imports System.Drawing
Imports System.Windows.Forms

Namespace WeifenLuo.WinFormsUI.Docking
    ' Inspired by Chris Sano's article:
    ' http://msdn.microsoft.com/smartclient/default.aspx?pull=/library/en-us/dnwinforms/html/colorpicker.asp
    ' In Sano's article, the DragForm needs to meet the following criteria:
    ' (1) it was not to show up in the task bar;
    '     ShowInTaskBar = false
    ' (2) it needed to be the top-most window;
    '     TopMost = true
    ' (3) its icon could not show up in the ALT+TAB window if the user pressed ALT+TAB during a drag-and-drop;
    '     FormBorderStyle = FormBorderStyle.None;
    '     Create with WS_EX_TOOLWINDOW window style.
    '     Compares with the solution in the artile by setting FormBorderStyle as FixedToolWindow,
    '     and then clip the window caption and border, this way is much simplier.
    ' (4) it was not to steal focus from the application when displayed.
    '     User Win32 ShowWindow API with SW_SHOWNOACTIVATE
    ' In addition, this form should only for display and therefore should act as transparent, otherwise
    ' WindowFromPoint will return this form, instead of the control beneath. Need BOTH of the following to
    ' achieve this (don't know why, spent hours to try it out :( ):
    '  1. Enabled = false;
    '  2. WM_NCHITTEST returns HTTRANSPARENT
    Public Class DragForm
        Inherits Form
        Public Sub New()
            FormBorderStyle = FormBorderStyle.None
            ShowInTaskbar = False
            SetStyle(ControlStyles.Selectable, False)
            Enabled = False
            TopMost = True
            AddHandler SizeChanged, Sub(sender, args)
                                        If BackgroundColor IsNot Nothing Then
                                            Invalidate()
                                        End If
                                    End Sub
        End Sub

        Public Property BackgroundColor As Color?

        Protected Overrides ReadOnly Property CreateParams As CreateParams
            Get
                Dim lCreateParams = MyBase.CreateParams
                lCreateParams.ExStyle = lCreateParams.ExStyle Or (Win32.WindowExStyles.WS_EX_NOACTIVATE Or Win32.WindowExStyles.WS_EX_TOOLWINDOW)
                Return lCreateParams
            End Get
        End Property

        Protected Overrides Sub WndProc(ByRef m As Message)
            If m.Msg = Win32.Msgs.WM_NCHITTEST Then
                m.Result = CType(Win32.HitTest.HTTRANSPARENT, IntPtr)
                Return
            End If

            MyBase.WndProc(m)
        End Sub

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            MyBase.OnPaint(e)
            If BackgroundColor Is Nothing Then
                Return
            End If

            Dim all = ClientRectangle
            If all.Width > 10 AndAlso all.Height > 10 Then
                Dim newLocation = New Point(all.Location.X + 5, all.Location.Y + 5)
                Dim newSize = New Size(all.Width - 10, all.Height - 10)
                Dim center = New Rectangle(newLocation, newSize)
                e.Graphics.FillRectangle(New SolidBrush(BackgroundColor.Value), center)
            End If
        End Sub

        'The form can be still activated by explicity calling Activate
        Protected Overrides ReadOnly Property ShowWithoutActivation As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overridable Overloads Sub Show(bActivate As Boolean)
            MyBase.Show()

            If bActivate Then Activate()
        End Sub
    End Class
End Namespace
