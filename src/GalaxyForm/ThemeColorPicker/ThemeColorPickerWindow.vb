Imports System.Drawing

Namespace ThemeColorPicker
    Partial Public Class ThemeColorPickerWindow
        Inherits Form
        Private _c As Color

        Public Enum Action
            HideWindow
            CloseWindow
            DoNothing
        End Enum

        Private preventClose As Boolean = False

        Private _as As Action = Action.CloseWindow
        Private _al As Action = Action.CloseWindow

        Public Property ActionAfterColorSelected As Action
            Get
                Return _as
            End Get
            Set(value As Action)
                _as = value
            End Set
        End Property
        Public Property ActionAfterLostFocus As Action
            Get
                Return _al
            End Get
            Set(value As Action)
                _al = value
            End Set
        End Property

        Public Property CustomColors As Integer()
            Get
                Return themeColorPicker1.CustomColors
            End Get
            Set(value As Integer())
                themeColorPicker1.CustomColors = value
            End Set
        End Property

        Public Property Color As Color
            Get
                Return _c
            End Get
            Set(value As Color)
                _c = value
                If ColorSelectedEvent IsNot Nothing Then
                    Dim arg As ColorSelectedArg = New ColorSelectedArg(_c)
                    RaiseEvent ColorSelected(Me, arg)
                End If
                If ActionAfterColorSelected = Action.HideWindow Then
                    Visible = False
                ElseIf ActionAfterColorSelected = Action.CloseWindow Then
                    Close()
                ElseIf ActionAfterColorSelected = Action.DoNothing Then
                End If
            End Set
        End Property

        ''' <summary>
        ''' Occur when a color is selected.
        ''' </summary>
        Public Event ColorSelected(sender As Object, e As ColorSelectedArg)

        ''' <summary>
        ''' Create a new window for ThemeColorPicker.
        ''' </summary>
        ''' <param name="startLocation">The starting position on screen. Note: This is not location in Form.</param>
        ''' <param name="borderStyle">How the border should displayed.</param>
        ''' <param name="actionAfterColorSelected">The form action of 0o-.</param>
        ''' <param name="actionAfterLostFocus"></param>
        Public Sub New(startLocation As Point, borderStyle As FormBorderStyle, actionAfterColorSelected As Action, actionAfterLostFocus As Action)
            InitializeComponent()
            StartPosition = FormStartPosition.Manual
            Location = startLocation
            FormBorderStyle = borderStyle
            Me.ActionAfterColorSelected = actionAfterColorSelected
            Me.ActionAfterLostFocus = actionAfterLostFocus
            AddHandler LostFocus, New EventHandler(AddressOf ThemeColorPickerWindow_LostFocus)
            AddHandler Deactivate, New EventHandler(AddressOf ThemeColorPickerWindow_Deactivate)
            AddHandler themeColorPicker1.ShowCustomMoreColorWindow, New ThemeColorPicker.moreColorWindowShow(AddressOf themeColorPicker1_ShowCustomMoreColorWindow)
        End Sub

        Private Sub themeColorPicker1_ShowCustomMoreColorWindow(sender As Object)
            preventClose = True
            Dim cd As ColorDialog = New ColorDialog()
            cd.AllowFullOpen = True
            cd.FullOpen = True
            cd.Color = _c
            cd.CustomColors = themeColorPicker1.CustomColors
            If cd.ShowDialog() = DialogResult.OK Then
                Color = cd.Color
                themeColorPicker1.CustomColors = cd.CustomColors
            End If
            preventClose = False
        End Sub

        Private Sub ThemeColorPickerWindow_Deactivate(sender As Object, e As EventArgs)
            If preventClose Then Return

            If ActionAfterLostFocus = Action.HideWindow Then
                Visible = False
            ElseIf ActionAfterLostFocus = Action.CloseWindow Then
                Close()
            Else
            End If
        End Sub

        Private Sub ThemeColorPickerWindow_LostFocus(sender As Object, e As EventArgs)
            If ActionAfterLostFocus = Action.HideWindow Then
                Visible = False
            ElseIf ActionAfterLostFocus = Action.CloseWindow Then
                Close()
            Else
            End If
        End Sub

        Private Sub themeColorPicker1_ColorSelected(sender As Object, e As ColorSelectedArg)
            Color = e.Color
        End Sub
    End Class
End Namespace
