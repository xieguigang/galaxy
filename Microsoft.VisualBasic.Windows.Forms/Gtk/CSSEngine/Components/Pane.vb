Public Class Pane
    Inherits Panel

    Private BCol As Color = Color.Black
    Private BSty As Drawing2D.DashStyle = Drawing2D.DashStyle.Solid
    Private BThk As Integer = 5
    Private Stck As Boolean = True

    Public Property CanStackItems() As Boolean
        Get

            Return Stck

        End Get
        Set(ByVal value As Boolean)

            Stck = value

        End Set
    End Property

    Public Property BorderColor() As Color
        Get

            Return BCol

        End Get
        Set(ByVal value As Color)

            BCol = value
            Me.Refresh()

        End Set
    End Property

    Public Shadows Property BorderStyle() As Drawing2D.DashStyle
        Get

            Return BSty

        End Get
        Set(ByVal value As Drawing2D.DashStyle)

            BSty = value
            Me.Refresh()

        End Set
    End Property

    Public Shadows Property BorderThickness() As Integer
        Get

            Return BThk

        End Get
        Set(ByVal value As Integer)

            If value > 20 Then value = 20
            BThk = value
            If Me.Padding.All < BThk Then Me.Padding = New Padding(BThk)
            Me.Refresh()

        End Set
    End Property

    Private Sub Pane_AutoSizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.AutoSizeChanged

        RefreshPos()

    End Sub

    Private Sub Pane_ControlAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.ControlEventArgs) Handles Me.ControlAdded

        RefreshPos()

    End Sub

    Private Sub Pane_ControlRemoved(ByVal sender As Object, ByVal e As System.Windows.Forms.ControlEventArgs) Handles Me.ControlRemoved

        RefreshPos()

    End Sub

    Private Sub Pane_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint

        Dim g As Graphics = e.Graphics
        Dim b As New Pen(BCol, BThk)
        b.DashStyle = BSty

        g.DrawLine(b, 1, CInt(BThk / 2), CInt(Me.Width - 1), CInt(BThk / 2)) 'Top
        g.DrawLine(b, CInt(Me.Width - 1 - Math.Round(BThk / 2, MidpointRounding.AwayFromZero)), CInt(Me.Height - 1), CInt(Me.Width - 1 - Math.Round(BThk / 2, MidpointRounding.AwayFromZero)), 1) 'Right
        g.DrawLine(b, CInt(BThk / 2), 1, CInt(BThk / 2), CInt(Me.Height - 1)) 'Left
        g.DrawLine(b, CInt(Me.Width - 1), CInt(Me.Height - 1 - Math.Round(BThk / 2, MidpointRounding.AwayFromZero)), 1, CInt(Me.Height - 1 - Math.Round(BThk / 2, MidpointRounding.AwayFromZero))) 'Bottom

        RefreshPos()

        b.Dispose()

    End Sub

    Private Sub Pane_RegionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.RegionChanged

        RefreshPos()

    End Sub

    Private Sub Pane_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.SizeChanged

        RefreshPos()

    End Sub

    Public Sub RefreshPos()

        Try

            Dim wat As Integer = 0 'x pos of current position
            Dim hat As Integer = 0 'Height of tallest item on line
            Dim bat As Integer = 0 'y pos of current line

            Dim l As Control = Nothing 'The last control

            l = New Control("", 0, 0, 0, 0)

            For Each c As Control In Controls

                If c.Width + c.Padding.Left > Me.Width Then 'If the control is bigger than us

                    If hat > 0 Then 'If there is stuff on the current line

                        bat += hat
                        c.Top = bat + c.Margin.Top
                        c.Left = c.Margin.Left
                        bat += c.Height
                        bat += c.Margin.Top
                        bat += c.Margin.Bottom
                        hat = 0
                        wat = 0

                    Else 'If the current line is empty

                        c.Top = bat + c.Margin.Top
                        c.Left = c.Margin.Left
                        bat += c.Height
                        bat += c.Margin.Top
                        bat += c.Margin.Bottom
                        hat = 0
                        wat = 0

                    End If

                    GoTo nxt

                End If

                If wat + c.Width + c.Margin.Left > Me.Width Then
                    'wrap

                    If l.Height + l.Margin.Top + l.Margin.Bottom + c.Height + c.Margin.Top + c.Margin.Bottom < hat AndAlso Stck = True Then
                        'This means that the control can fit in
                        c.Top = l.Top + l.Height + l.Margin.Bottom + c.Margin.Bottom
                        c.Left = (l.Left - l.Margin.Left) + c.Margin.Left

                    Else

                        bat += hat
                        wat = 0
                        hat = 0
                        c.Left = wat + c.Margin.Left
                        c.Top = bat + c.Margin.Top
                        hat = c.Height + c.Margin.Top + c.Margin.Bottom
                        wat += c.Width
                        wat += c.Margin.Left
                        wat += c.Margin.Right

                    End If

                Else
                    'add it on
                    c.Left = wat + c.Margin.Left
                    c.Top = bat + c.Margin.Top
                    If hat < c.Height + c.Margin.Top + c.Margin.Bottom Then hat = c.Height + c.Margin.Top + c.Margin.Bottom
                    wat += c.Width
                    wat += c.Margin.Left
                    wat += c.Margin.Right

                End If

nxt:

                l = c

            Next

        Catch

        End Try

    End Sub

    Public Shadows Property Padding() As Padding
        Get

            Return MyBase.Padding

        End Get
        Set(ByVal value As Padding)

            MyBase.Padding = value

        End Set
    End Property

End Class
