Imports System.ComponentModel
Imports System.Drawing.Drawing2D
Imports Microsoft.VisualStudio.WinForms.Docking

Namespace ThemeVS2012
    Friend Class VS2012PaneIndicatorFactory
        Implements DockPanelExtender.IPaneIndicatorFactory
        Public Function CreatePaneIndicator(theme As ThemeBase) As DockPanel.IPaneIndicator Implements DockPanelExtender.IPaneIndicatorFactory.CreatePaneIndicator
            Return New VS2012PaneIndicator(theme)
        End Function

        <ToolboxItem(False)>
        Private Class VS2012PaneIndicator
            Inherits PictureBox
            Implements DockPanel.IPaneIndicator

            Private _bitmapPaneDiamond As Bitmap
            Private _bitmapPaneDiamondLeft As Bitmap
            Private _bitmapPaneDiamondRight As Bitmap
            Private _bitmapPaneDiamondTop As Bitmap
            Private _bitmapPaneDiamondBottom As Bitmap
            Private _bitmapPaneDiamondFill As Bitmap
            Private _bitmapPaneDiamondHotSpot As Bitmap
            Private _bitmapPaneDiamondHotSpotIndex As Bitmap

            Private Shared _hotSpots As DockPanel.HotSpotIndex() = {New DockPanel.HotSpotIndex(1, 0, DockStyle.Top), New DockPanel.HotSpotIndex(0, 1, DockStyle.Left), New DockPanel.HotSpotIndex(1, 1, DockStyle.Fill), New DockPanel.HotSpotIndex(2, 1, DockStyle.Right), New DockPanel.HotSpotIndex(1, 2, DockStyle.Bottom)}

            Private _displayingGraphicsPath As GraphicsPath

            Public Sub New(theme As ThemeBase)
                _bitmapPaneDiamond = theme.ImageService.Dockindicator_PaneDiamond
                _bitmapPaneDiamondLeft = theme.ImageService.Dockindicator_PaneDiamond_Fill
                _bitmapPaneDiamondRight = theme.ImageService.Dockindicator_PaneDiamond_Fill
                _bitmapPaneDiamondTop = theme.ImageService.Dockindicator_PaneDiamond_Fill
                _bitmapPaneDiamondBottom = theme.ImageService.Dockindicator_PaneDiamond_Fill
                _bitmapPaneDiamondFill = theme.ImageService.Dockindicator_PaneDiamond_Fill
                _bitmapPaneDiamondHotSpot = theme.ImageService.Dockindicator_PaneDiamond_Hotspot
                _bitmapPaneDiamondHotSpotIndex = theme.ImageService.DockIndicator_PaneDiamond_HotspotIndex
                _displayingGraphicsPath = CalculateGraphicsPathFromBitmap(_bitmapPaneDiamond)

                SizeMode = PictureBoxSizeMode.AutoSize
                Image = _bitmapPaneDiamond
                Region = New Region(DisplayingGraphicsPath)
            End Sub

            Public ReadOnly Property DisplayingGraphicsPath As GraphicsPath Implements DockPanel.IPaneIndicator.DisplayingGraphicsPath
                Get
                    Return _displayingGraphicsPath
                End Get
            End Property

            Public Function HitTest(pt As Point) As DockStyle Implements DockPanel.IHitTest.HitTest
                If Not Visible Then Return DockStyle.None

                pt = PointToClient(pt)
                If Not ClientRectangle.Contains(pt) Then Return DockStyle.None

                For i = _hotSpots.GetLowerBound(0) To _hotSpots.GetUpperBound(0)
                    If _bitmapPaneDiamondHotSpot.GetPixel(pt.X, pt.Y) = _bitmapPaneDiamondHotSpotIndex.GetPixel(_hotSpots(i).X, _hotSpots(i).Y) Then Return _hotSpots(i).DockStyle
                Next

                Return DockStyle.None
            End Function

            Private m_status As DockStyle = DockStyle.None

            Public Property Status As DockStyle Implements DockPanel.IHitTest.Status
                Get
                    Return m_status
                End Get
                Set(value As DockStyle)
                    m_status = value
                    If m_status = DockStyle.None Then
                        Image = _bitmapPaneDiamond
                    ElseIf m_status = DockStyle.Left Then
                        Image = _bitmapPaneDiamondLeft
                    ElseIf m_status = DockStyle.Right Then
                        Image = _bitmapPaneDiamondRight
                    ElseIf m_status = DockStyle.Top Then
                        Image = _bitmapPaneDiamondTop
                    ElseIf m_status = DockStyle.Bottom Then
                        Image = _bitmapPaneDiamondBottom
                    ElseIf m_status = DockStyle.Fill Then
                        Image = _bitmapPaneDiamondFill
                    End If
                End Set
            End Property

            Private Property IPaneIndicator_Location As Point Implements DockPanel.IPaneIndicator.Location
                Get
                    Return Location
                End Get
                Set(value As Point)
                    Location = value
                End Set
            End Property

            Private Property IPaneIndicator_Visible As Boolean Implements DockPanel.IPaneIndicator.Visible
                Get
                    Return Visible
                End Get
                Set(value As Boolean)
                    Visible = value
                End Set
            End Property

            Private ReadOnly Property IPaneIndicator_Left As Integer Implements DockPanel.IPaneIndicator.Left
                Get
                    Return Left
                End Get
            End Property

            Private ReadOnly Property IPaneIndicator_Top As Integer Implements DockPanel.IPaneIndicator.Top
                Get
                    Return Top
                End Get
            End Property

            Private ReadOnly Property IPaneIndicator_Right As Integer Implements DockPanel.IPaneIndicator.Right
                Get
                    Return Right
                End Get
            End Property

            Private ReadOnly Property IPaneIndicator_Bottom As Integer Implements DockPanel.IPaneIndicator.Bottom
                Get
                    Return Bottom
                End Get
            End Property

            Private ReadOnly Property IPaneIndicator_ClientRectangle As Rectangle Implements DockPanel.IPaneIndicator.ClientRectangle
                Get
                    Return ClientRectangle
                End Get
            End Property

            Private ReadOnly Property IPaneIndicator_Width As Integer Implements DockPanel.IPaneIndicator.Width
                Get
                    Return Width
                End Get
            End Property

            Private ReadOnly Property IPaneIndicator_Height As Integer Implements DockPanel.IPaneIndicator.Height
                Get
                    Return Height
                End Get
            End Property
        End Class
    End Class
End Namespace
