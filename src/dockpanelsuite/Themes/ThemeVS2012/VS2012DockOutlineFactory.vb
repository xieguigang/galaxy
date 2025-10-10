Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms
Imports WeifenLuo.WinFormsUI.Docking

Namespace WeifenLuo.WinFormsUI.ThemeVS2012
    Friend Class VS2012DockOutlineFactory
        Implements DockPanelExtender.IDockOutlineFactory
        Public Function CreateDockOutline() As DockOutlineBase Implements DockPanelExtender.IDockOutlineFactory.CreateDockOutline
            Return New VS2012LightDockOutline()
        End Function

        Private Class VS2012LightDockOutline
            Inherits DockOutlineBase
            Public Sub New()
                m_dragForm = New DragForm()
                SetDragForm(Rectangle.Empty)
                ' IMPORTANT: this color does not come from palette.
                DragForm.BackColor = ColorTranslator.FromHtml("#FFC2C2C2")
                DragForm.BackgroundColor = ColorTranslator.FromHtml("#FF5BADFF")
                DragForm.Opacity = 0.5
                DragForm.Show(False)
            End Sub

            Private m_dragForm As DragForm
            Private ReadOnly Property DragForm As DragForm
                Get
                    Return m_dragForm
                End Get
            End Property

            Protected Overrides Sub OnShow()
                CalculateRegion()
            End Sub

            Protected Overrides Sub OnClose()
                DragForm.Close()
            End Sub

            Private Sub CalculateRegion()
                If SameAsOldValue Then Return

                If Not FloatWindowBounds.IsEmpty Then
                    SetOutline(FloatWindowBounds)
                ElseIf TypeOf DockTo Is DockPanel Then
                    SetOutline(TryCast(DockTo, DockPanel), Dock, ContentIndex <> 0)
                ElseIf TypeOf DockTo Is DockPane Then
                    SetOutline(TryCast(DockTo, DockPane), Dock, ContentIndex)
                Else
                    SetOutline()
                End If
            End Sub

            Private Sub SetOutline()
                SetDragForm(Rectangle.Empty)
            End Sub

            Private Sub SetOutline(floatWindowBounds As Rectangle)
                SetDragForm(floatWindowBounds)
            End Sub

            Private Sub SetOutline(dockPanel As DockPanel, dock As DockStyle, fullPanelEdge As Boolean)
                Dim rect = If(fullPanelEdge, dockPanel.DockArea, dockPanel.DocumentWindowBounds)
                rect.Location = dockPanel.PointToScreen(rect.Location)
                If dock = DockStyle.Top Then
                    Dim height = dockPanel.GetDockWindowSize(DockState.DockTop)
                    rect = New Rectangle(rect.X, rect.Y, rect.Width, height)
                ElseIf dock = DockStyle.Bottom Then
                    Dim height = dockPanel.GetDockWindowSize(DockState.DockBottom)
                    rect = New Rectangle(rect.X, rect.Bottom - height, rect.Width, height)
                ElseIf dock = DockStyle.Left Then
                    Dim width = dockPanel.GetDockWindowSize(DockState.DockLeft)
                    rect = New Rectangle(rect.X, rect.Y, width, rect.Height)
                ElseIf dock = DockStyle.Right Then
                    Dim width = dockPanel.GetDockWindowSize(DockState.DockRight)
                    rect = New Rectangle(rect.Right - width, rect.Y, width, rect.Height)
                ElseIf dock = DockStyle.Fill Then
                    rect = dockPanel.DocumentWindowBounds
                    rect.Location = dockPanel.PointToScreen(rect.Location)
                End If

                SetDragForm(rect)
            End Sub

            Private Sub SetOutline(pane As DockPane, dock As DockStyle, contentIndex As Integer)
                If dock <> DockStyle.Fill Then
                    Dim rect = pane.DisplayingRectangle
                    If dock = DockStyle.Right Then rect.X += rect.Width / 2
                    If dock = DockStyle.Bottom Then rect.Y += rect.Height / 2
                    If dock = DockStyle.Left OrElse dock = DockStyle.Right Then rect.Width -= rect.Width / 2
                    If dock = DockStyle.Top OrElse dock = DockStyle.Bottom Then rect.Height -= rect.Height / 2
                    rect.Location = pane.PointToScreen(rect.Location)

                    SetDragForm(rect)
                ElseIf contentIndex = -1 Then
                    Dim rect = pane.DisplayingRectangle
                    rect.Location = pane.PointToScreen(rect.Location)
                    SetDragForm(rect)
                Else
                    Using path = pane.TabStripControl.GetOutline(contentIndex)
                        Dim rectF As RectangleF = path.GetBounds()
                        Dim rect As Rectangle = New Rectangle(rectF.X, rectF.Y, rectF.Width, rectF.Height)
                        Using matrix As Matrix = New Matrix(rect, New Point() {New Point(0, 0), New Point(rect.Width, 0), New Point(0, rect.Height)})
                            path.Transform(matrix)
                        End Using

                        Dim region As Region = New Region(path)
                        SetDragForm(rect, region)
                    End Using
                End If
            End Sub

            Private Sub SetDragForm(rect As Rectangle)
                DragForm.Bounds = rect
                If rect = Rectangle.Empty Then
                    If DragForm.Region IsNot Nothing Then
                        DragForm.Region.Dispose()
                    End If

                    DragForm.Region = New Region(Rectangle.Empty)
                ElseIf DragForm.Region IsNot Nothing Then
                    DragForm.Region.Dispose()
                    DragForm.Region = Nothing
                End If
            End Sub

            Private Sub SetDragForm(rect As Rectangle, region As Region)
                DragForm.Bounds = rect
                If DragForm.Region IsNot Nothing Then
                    DragForm.Region.Dispose()
                End If

                DragForm.Region = region
            End Sub
        End Class
    End Class
End Namespace
