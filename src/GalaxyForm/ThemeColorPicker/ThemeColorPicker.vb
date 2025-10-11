Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing

Namespace ThemeColorPicker
    <DefaultEvent("ColorSelected")>
    Partial Public Class ThemeColorPicker
        Inherits UserControl
        Private _customColors As Integer() = New Integer(-1) {}

        Public Property CustomColors As Integer()
            Get
                Return _customColors
            End Get
            Set(value As Integer())
                _customColors = value
            End Set
        End Property

        Private _c As Color
        Private dic As Dictionary(Of String, Color)

        ''' <summary>
        ''' Occur when a color is selected.
        ''' </summary>
        Public Event ColorSelected(sender As Object, e As ColorSelectedArg)

        Public Delegate Sub moreColorWindowShow(sender As Object)

        ''' <summary>
        ''' Don't show pre-configure default Color Dialog
        ''' </summary>
        Public Event ShowCustomMoreColorWindow As moreColorWindowShow

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
            End Set
        End Property

        Public Sub New()
            InitializeComponent()

            dic = New Dictionary(Of String, Color)()

            dic("p01") = Color.FromArgb(255, 255, 255)
            dic("p02") = Color.FromArgb(242, 242, 242)
            dic("p03") = Color.FromArgb(216, 216, 216)
            dic("p04") = Color.FromArgb(191, 191, 191)
            dic("p05") = Color.FromArgb(165, 165, 165)
            dic("p06") = Color.FromArgb(127, 127, 127)

            dic("p11") = Color.FromArgb(0, 0, 0)
            dic("p12") = Color.FromArgb(127, 127, 127)
            dic("p13") = Color.FromArgb(89, 89, 89)
            dic("p14") = Color.FromArgb(63, 63, 63)
            dic("p15") = Color.FromArgb(38, 38, 38)
            dic("p16") = Color.FromArgb(12, 12, 12)

            dic("p21") = Color.FromArgb(238, 236, 225)
            dic("p22") = Color.FromArgb(221, 217, 195)
            dic("p23") = Color.FromArgb(196, 189, 151)
            dic("p24") = Color.FromArgb(147, 137, 83)
            dic("p25") = Color.FromArgb(73, 68, 41)
            dic("p26") = Color.FromArgb(29, 27, 16)

            dic("p31") = Color.FromArgb(31, 73, 125)
            dic("p32") = Color.FromArgb(198, 217, 240)
            dic("p33") = Color.FromArgb(141, 179, 226)
            dic("p34") = Color.FromArgb(84, 141, 212)
            dic("p35") = Color.FromArgb(23, 54, 93)
            dic("p36") = Color.FromArgb(15, 36, 62)

            dic("p41") = Color.FromArgb(79, 129, 189)
            dic("p42") = Color.FromArgb(198, 217, 240)
            dic("p43") = Color.FromArgb(184, 204, 228)
            dic("p44") = Color.FromArgb(149, 179, 215)
            dic("p45") = Color.FromArgb(54, 96, 146)
            dic("p46") = Color.FromArgb(36, 64, 97)

            dic("p51") = Color.FromArgb(192, 80, 77)
            dic("p52") = Color.FromArgb(242, 220, 219)
            dic("p53") = Color.FromArgb(229, 185, 183)
            dic("p54") = Color.FromArgb(217, 150, 148)
            dic("p55") = Color.FromArgb(140, 51, 49)
            dic("p56") = Color.FromArgb(99, 36, 35)

            dic("p61") = Color.FromArgb(155, 187, 89)
            dic("p62") = Color.FromArgb(235, 241, 221)
            dic("p63") = Color.FromArgb(215, 227, 188)
            dic("p64") = Color.FromArgb(195, 214, 155)
            dic("p65") = Color.FromArgb(118, 146, 60)
            dic("p66") = Color.FromArgb(79, 97, 40)

            dic("p71") = Color.FromArgb(128, 100, 162)
            dic("p72") = Color.FromArgb(229, 224, 236)
            dic("p73") = Color.FromArgb(204, 193, 217)
            dic("p74") = Color.FromArgb(178, 162, 199)
            dic("p75") = Color.FromArgb(95, 73, 122)
            dic("p76") = Color.FromArgb(63, 49, 81)

            dic("p81") = Color.FromArgb(75, 172, 198)
            dic("p82") = Color.FromArgb(219, 238, 243)
            dic("p83") = Color.FromArgb(183, 221, 232)
            dic("p84") = Color.FromArgb(146, 205, 220)
            dic("p85") = Color.FromArgb(49, 133, 155)
            dic("p86") = Color.FromArgb(32, 88, 103)

            dic("p91") = Color.FromArgb(247, 150, 70)
            dic("p92") = Color.FromArgb(253, 234, 218)
            dic("p93") = Color.FromArgb(251, 213, 181)
            dic("p94") = Color.FromArgb(250, 192, 143)
            dic("p95") = Color.FromArgb(171, 82, 7)
            dic("p96") = Color.FromArgb(151, 72, 6)

            dic("p07") = Color.FromArgb(192, 0, 0)
            dic("p17") = Color.FromArgb(255, 0, 0)
            dic("p27") = Color.FromArgb(255, 192, 0)
            dic("p37") = Color.FromArgb(255, 255, 0)
            dic("p47") = Color.FromArgb(146, 208, 80)
            dic("p57") = Color.FromArgb(0, 176, 80)
            dic("p67") = Color.FromArgb(0, 176, 240)
            dic("p77") = Color.FromArgb(0, 108, 186)
            dic("p87") = Color.FromArgb(0, 32, 96)
            dic("p97") = Color.FromArgb(112, 48, 160)

            SuspendLayout()
            For i = 1 To 9
                For j = 1 To 7
                    Dim p As Panel = New Panel()
                    p.Name = "p" & i.ToString() & "" & j.ToString()
                    p.Size = New Size(13, 13)
                    Dim pt As Point = CType(Controls.Find("p" & (i - 1).ToString() & "" & j.ToString(), False), Control())(0).Location
                    p.Location = New Point(pt.X + 4 + 13, pt.Y)
                    p.BackColor = Color.Transparent
                    AddHandler p.MouseClick, New MouseEventHandler(AddressOf p_MouseClick)
                    p.Cursor = Cursors.Hand
                    Controls.Add(p)
                Next
            Next
            ResumeLayout(False)
            PerformLayout()
        End Sub

        Private Sub p_MouseClick(sender As Object, e As MouseEventArgs)
            Color = dic(CType(sender, Control).Name)
        End Sub

        Private Sub pnMoreColor_MouseClick(sender As Object, e As MouseEventArgs)
            If ShowCustomMoreColorWindowEvent IsNot Nothing Then
                RaiseEvent ShowCustomMoreColorWindow(Me)
            Else
                ShowMoreColor()
            End If
        End Sub

        Public Overridable Sub ShowMoreColor()
            Dim cd As ColorDialog = New ColorDialog()
            cd.AllowFullOpen = True
            cd.FullOpen = True
            cd.Color = _c
            cd.CustomColors = _customColors
            If cd.ShowDialog() = DialogResult.OK Then
                Color = cd.Color
                _customColors = cd.CustomColors
            End If
        End Sub
    End Class
End Namespace
