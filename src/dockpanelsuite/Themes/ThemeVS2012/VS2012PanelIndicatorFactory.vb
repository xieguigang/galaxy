Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports WeifenLuo.WinFormsUI.Docking

Namespace WeifenLuo.WinFormsUI.ThemeVS2012
    Friend Class VS2012PanelIndicatorFactory
        Implements DockPanelExtender.IPanelIndicatorFactory
        Public Function CreatePanelIndicator(style As DockStyle, theme As ThemeBase) As DockPanel.IPanelIndicator Implements DockPanelExtender.IPanelIndicatorFactory.CreatePanelIndicator
            Return New VS2012PanelIndicator(style, theme)
        End Function

        <ToolboxItem(False)>
        Private Class VS2012PanelIndicator
            Inherits PictureBox
            Implements DockPanel.IPanelIndicator

            Private _imagePanelLeft As Image
            Private _imagePanelRight As Image
            Private _imagePanelTop As Image
            Private _imagePanelBottom As Image
            Private _imagePanelFill As Image
            Private _imagePanelLeftActive As Image
            Private _imagePanelRightActive As Image
            Private _imagePanelTopActive As Image
            Private _imagePanelBottomActive As Image
            Private _imagePanelFillActive As Image

            Public Sub New(dockStyle As DockStyle, theme As ThemeBase)
                _imagePanelLeft = theme.ImageService.DockIndicator_PanelLeft
                _imagePanelRight = theme.ImageService.DockIndicator_PanelRight
                _imagePanelTop = theme.ImageService.DockIndicator_PanelTop
                _imagePanelBottom = theme.ImageService.DockIndicator_PanelBottom
                _imagePanelFill = theme.ImageService.DockIndicator_PanelFill
                _imagePanelLeftActive = theme.ImageService.DockIndicator_PanelLeft
                _imagePanelRightActive = theme.ImageService.DockIndicator_PanelRight
                _imagePanelTopActive = theme.ImageService.DockIndicator_PanelTop
                _imagePanelBottomActive = theme.ImageService.DockIndicator_PanelBottom
                _imagePanelFillActive = theme.ImageService.DockIndicator_PanelFill

                m_dockStyle = dockStyle
                SizeMode = PictureBoxSizeMode.AutoSize
                Image = ImageInactive
            End Sub

            Private m_dockStyle As DockStyle

            Private ReadOnly Property DockStyle As DockStyle
                Get
                    Return m_dockStyle
                End Get
            End Property

            Private m_status As DockStyle

            Public Property Status As DockStyle Implements DockPanel.IHitTest.Status
                Get
                    Return m_status
                End Get
                Set(value As DockStyle)
                    If value <> DockStyle AndAlso value <> DockStyle.None Then Throw New InvalidEnumArgumentException()

                    If m_status = value Then Return

                    m_status = value
                    IsActivated = m_status <> DockStyle.None
                End Set
            End Property

            Private ReadOnly Property ImageInactive As Image
                Get
                    If DockStyle = DockStyle.Left Then
                        Return _imagePanelLeft
                    ElseIf DockStyle = DockStyle.Right Then
                        Return _imagePanelRight
                    ElseIf DockStyle = DockStyle.Top Then
                        Return _imagePanelTop
                    ElseIf DockStyle = DockStyle.Bottom Then
                        Return _imagePanelBottom
                    ElseIf DockStyle = DockStyle.Fill Then
                        Return _imagePanelFill
                    Else
                        Return Nothing
                    End If
                End Get
            End Property

            Private ReadOnly Property ImageActive As Image
                Get
                    If DockStyle = DockStyle.Left Then
                        Return _imagePanelLeftActive
                    ElseIf DockStyle = DockStyle.Right Then
                        Return _imagePanelRightActive
                    ElseIf DockStyle = DockStyle.Top Then
                        Return _imagePanelTopActive
                    ElseIf DockStyle = DockStyle.Bottom Then
                        Return _imagePanelBottomActive
                    ElseIf DockStyle = DockStyle.Fill Then
                        Return _imagePanelFillActive
                    Else
                        Return Nothing
                    End If
                End Get
            End Property

            Private m_isActivated As Boolean = False

            Private Property IsActivated As Boolean
                Get
                    Return m_isActivated
                End Get
                Set(value As Boolean)
                    m_isActivated = value
                    Image = If(IsActivated, ImageActive, ImageInactive)
                End Set
            End Property

            Private Property IPanelIndicator_Location As Point Implements DockPanel.IPanelIndicator.Location
                Get
                    Return Location
                End Get
                Set(value As Point)
                    Location = value
                End Set
            End Property

            Private Property IPanelIndicator_Visible As Boolean Implements DockPanel.IPanelIndicator.Visible
                Get
                    Return Visible
                End Get
                Set(value As Boolean)
                    Visible = value
                End Set
            End Property

            Private ReadOnly Property IPanelIndicator_Bounds As Rectangle Implements DockPanel.IPanelIndicator.Bounds
                Get
                    Return Bounds
                End Get
            End Property

            Private ReadOnly Property IPanelIndicator_Width As Integer Implements DockPanel.IPanelIndicator.Width
                Get
                    Return Width
                End Get
            End Property

            Private ReadOnly Property IPanelIndicator_Height As Integer Implements DockPanel.IPanelIndicator.Height
                Get
                    Return Height
                End Get
            End Property

            Public Function HitTest(pt As Point) As DockStyle Implements DockPanel.IHitTest.HitTest
                Return If(Visible AndAlso ClientRectangle.Contains(PointToClient(pt)), DockStyle, DockStyle.None)
            End Function
        End Class
    End Class
End Namespace
